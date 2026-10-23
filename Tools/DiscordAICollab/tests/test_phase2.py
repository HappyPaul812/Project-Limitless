"""Phase2 변경분만 검사합니다. 실제 Discord/모델 호출은 포함하지 않습니다."""
import asyncio
import json
import os
import subprocess
import tempfile
import unittest
from datetime import datetime
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import AsyncMock, patch

from collab.__main__ import sample
from collab.bot import create_bot, run_bot
from collab.core import Denied, Identity, KST, Store, sha256
from collab.phase2 import APPROVAL, MARKER, child_environment, cli_info, discord_preflight, parse_events, probe_command, run_probe_once


def events(marker=MARKER,usage=None):
    return '\n'.join(json.dumps(x) for x in [
        {'type':'item.completed','item':{'type':'agent_message','text':marker}},
        {'type':'turn.completed','usage':usage or {'input_tokens':10,'cached_input_tokens':2,'output_tokens':4}}])


class FakeCLI:
    def __init__(self,body=None,exit_code=0,auth=True,timeout=False):
        self.calls=[]; self.body=events() if body is None else body
        self.exit_code=exit_code; self.auth=auth; self.timeout=timeout

    def __call__(self,args,**kwargs):
        self.calls.append((args,kwargs))
        if args[1]=='--version':
            return SimpleNamespace(returncode=0,stdout='codex-cli 0.163.0-alpha.11.2',stderr='')
        if args[1:]==['login','status']:
            return SimpleNamespace(returncode=0,stdout='Logged in using '+('ChatGPT' if self.auth else 'an API key'),stderr='private-auth-sentinel')
        if args[1:3]==['features','list']:
            return SimpleNamespace(returncode=0,stdout='\n'.join(x+' stable false' for x in ['shell_tool','apps','browser_use','computer_use','multi_agent']),stderr='')
        if self.timeout: raise subprocess.TimeoutExpired(args,90)
        return SimpleNamespace(returncode=self.exit_code,stdout=self.body,stderr='private-error-sentinel')

    @property
    def inference_count(self):
        return sum(args[1]=='exec' for args,_ in self.calls)


class Phase2Tests(unittest.TestCase):
    def setUp(self):
        self.tmp=tempfile.TemporaryDirectory()
        self.root=Path(self.tmp.name)
        self.store=Store(self.root/'queue.sqlite',{1},clock=lambda:datetime(2026,10,23,12,tzinfo=KST).timestamp())
        self.owner=Identity(1,1,1)

    def tearDown(self): self.tmp.cleanup()

    def probe(self,fake=None,**kwargs):
        return run_probe_once(self.store,self.root/'empty','codex.exe',approved=True,invoke=fake or FakeCLI(),**kwargs)

    def test_missing_discord_config_is_waiting_without_token_output(self):
        status=discord_preflight({'DISCORD_BOT_TOKEN':'SECRET_VALUE'})
        self.assertEqual('WAITING_FOR_USER_SETUP',status['status'])
        self.assertNotIn('SECRET_VALUE',json.dumps(status))
        self.assertEqual('NOT_VERIFIED',status['live_test'])

    def test_discord_requires_numeric_ids_and_explicit_switch(self):
        env={'DISCORD_BOT_TOKEN':'SECRET_VALUE','LIMITLESS_OWNER_IDS':'1','LIMITLESS_GUILD_ID':'2','LIMITLESS_CHANNEL_IDS':'3'}
        self.assertEqual('WAITING_FOR_USER_SETUP',discord_preflight(env)['status'])
        env['LIMITLESS_ALLOW_DISCORD']='1'
        self.assertEqual('LOCAL_CONFIG_READY',discord_preflight(env)['status'])
        env['LIMITLESS_OWNER_IDS']='display-name'
        self.assertEqual('WAITING_FOR_USER_SETUP',discord_preflight(env)['status'])

    def test_child_process_has_no_api_discord_github_or_provider_override(self):
        child=child_environment({'OPENAI_API_KEY':'x','CODEX_API_KEY':'x','DISCORD_BOT_TOKEN':'x',
            'GITHUB_TOKEN':'x','OPENAI_BASE_URL':'https://invalid','PATH':'ok','USERPROFILE':'ok'})
        self.assertEqual({'PATH':'ok','USERPROFILE':'ok'},child)

    def test_command_is_readonly_subscription_no_tools_and_no_reserved_override(self):
        command=probe_command('codex.exe',self.root)
        for control in ('forced_login_method="chatgpt"','features.shell_tool=false','features.apps=false',
                        'features.multi_agent=false','web_search="disabled"','project_doc_max_bytes=0'):
            self.assertIn(control,command)
        self.assertIn('read-only',command)
        self.assertIn('--ignore-user-config',command)
        self.assertNotIn('--dangerously-bypass-approvals-and-sandbox',command)
        self.assertFalse(any(x.startswith('model_providers.openai.') for x in command))

    def test_no_approval_no_inference(self):
        fake=FakeCLI()
        with self.assertRaises(Denied): run_probe_once(self.store,self.root/'empty','codex',invoke=fake)
        self.assertEqual(0,fake.inference_count)

    def test_api_auth_fails_closed_without_fallback(self):
        fake=FakeCLI(auth=False)
        result=self.probe(fake)
        self.assertEqual('WAITING_FOR_USER_SETUP',result['status'])
        self.assertEqual(0,fake.inference_count)
        self.assertNotIn('private-auth-sentinel',json.dumps(result))

    def test_config_bootstrap_failure_never_starts_model(self):
        fake=FakeCLI()
        def invoke(args,**kwargs):
            if args[1:3]==['features','list']:
                return SimpleNamespace(returncode=1,stdout='',stderr='reserved provider')
            return fake(args,**kwargs)
        result=run_probe_once(self.store,self.root/'empty','codex',approved=True,invoke=invoke)
        self.assertEqual('CLI_CONTROLS_NOT_VALIDATED',result['reason'])
        self.assertEqual(0,fake.inference_count)

    def test_nonempty_probe_workspace_is_rejected(self):
        root=self.root/'empty'; root.mkdir(); (root/'private.txt').write_text('secret')
        fake=FakeCLI()
        with self.assertRaises(Denied): self.probe(fake)
        self.assertEqual(0,fake.inference_count)

    def test_single_call_restart_gate_and_usage_are_durable(self):
        fake=FakeCLI()
        result=self.probe(fake)
        self.assertEqual('PASS',result['status'])
        self.assertEqual(1,fake.inference_count)
        self.assertEqual(10,result['usage']['input_tokens'])
        self.store=Store(self.root/'queue.sqlite',{1},clock=self.store.clock)
        with self.assertRaises(Denied): self.probe(fake)
        self.assertEqual(1,fake.inference_count)
        with self.store.tx() as db:
            self.assertEqual(1,db.execute('SELECT COUNT(*) FROM usage').fetchone()[0])
            self.assertEqual(0,db.execute('SELECT units FROM usage').fetchone()[0])

    def test_no_arbitrary_model_text_or_auth_output_is_stored(self):
        fake=FakeCLI(body=events('secret-model-text'))
        result=self.probe(fake)
        self.assertEqual('NOT_VERIFIED',result['status'])
        with self.store.tx() as db:
            stored=''.join(str(tuple(row)) for row in db.execute('SELECT * FROM tasks'))
        for secret in ('secret-model-text','private-auth-sentinel','private-error-sentinel'):
            self.assertNotIn(secret,stored+json.dumps(result))

    def test_timeout_never_retries_and_preserves_unknown_usage(self):
        fake=FakeCLI(timeout=True)
        result=self.probe(fake)
        self.assertEqual('TIMEOUT_NO_RETRY',result['reason'])
        with self.assertRaises(Denied): self.probe(fake)
        self.assertEqual(1,fake.inference_count)

    def test_tool_output_and_multiple_turns_are_not_pass(self):
        tool=json.dumps({'type':'item.completed','item':{'type':'command_execution','command':'secret'}})
        self.assertNotEqual('PASS',parse_events(tool+'\n'+events(),0)['status'])
        self.assertNotEqual('PASS',parse_events(events()+'\n'+events(),0)['status'])

    def test_malformed_events_and_invalid_usage_not_pass_or_counted(self):
        self.assertNotEqual('PASS',parse_events('[]\nnot-json',0)['status'])
        result=parse_events(events(usage={'input_tokens':True,'output_tokens':-1}),0)
        self.assertEqual({},result['usage'])

    def test_quota_error_pauses_without_retry(self):
        fake=FakeCLI(body=json.dumps({'type':'turn.failed','error':{'message':'429 quota'}}),exit_code=1)
        result=self.probe(fake)
        self.assertEqual('QUOTA_EXCEEDED',result['reason'])
        self.assertTrue(self.store.snapshot()['paused'])
        self.assertEqual(1,fake.inference_count)

    def test_probe_result_cannot_be_art_approved(self):
        result=self.probe()
        artifact=self.root/'synthetic-art.txt'; artifact.write_bytes(b'not an art asset')
        with self.store.tx() as db:
            task=self.store.task(db,result['task_id']); payload=json.loads(task['payload'])
            payload.update(artifact=str(artifact),artifact_sha=sha256(artifact))
            db.execute('UPDATE tasks SET payload=? WHERE id=?',(json.dumps(payload),task['id']))
        self.store.auto_handoff(result['task_id'])
        # Codex->Planning으로 전달돼도 CODEX_SUBSCRIPTION_PROBE 보고를 자동 승인하지 않음.
        task=self.store.get(result['task_id'])
        self.assertEqual('USER_REVIEW_PENDING',task['art'])
        self.assertNotEqual('ART_APPROVED',task['art'])
        # 직접 승인대기로 바꿔도 mode 게이트가 차단합니다.
        with self.store.tx() as db: db.execute("UPDATE tasks SET state='APPROVAL_PENDING' WHERE id=?",(task['id'],))
        with self.assertRaises(Denied):
            self.store.command(self.owner,'approve-probe','approve',{'task_id':task['id'],'sha':sha256(artifact),'scope':APPROVAL})

    def test_pause_and_sunday_never_start_probe(self):
        fake=FakeCLI()
        self.store.command(self.owner,'pause','pause')
        with self.assertRaises(Denied): self.probe(fake)
        self.store.command(self.owner,'resume','resume')
        self.store.clock=lambda:datetime(2026,10,25,12,tzinfo=KST).timestamp()
        with self.assertRaises(Denied): self.probe(fake)
        self.assertEqual(0,fake.inference_count)

    def test_two_running_jobs_prevent_probe(self):
        for i in range(2):
            task_id=self.store.command(self.owner,'job'+str(i),'task',sample())['task_id']
            self.store.triage(task_id)
            self.assertIsNotNone(self.store.claim())
        fake=FakeCLI()
        with self.assertRaises(Denied): self.probe(fake)
        self.assertEqual(0,fake.inference_count)

    def test_pause_during_result_does_not_reactivate_job(self):
        fake=FakeCLI()
        def invoke(args,**kwargs):
            if args[1]=='exec': self.store.command(self.owner,'emergency','pause')
            return fake(args,**kwargs)
        result=run_probe_once(self.store,self.root/'empty','codex',approved=True,invoke=invoke)
        self.assertEqual('NOT_VERIFIED',self.store.get(result['task_id'])['state'])
        self.assertEqual(1,fake.inference_count)

    def test_live_bot_error_message_never_echoes_token(self):
        env={'DISCORD_BOT_TOKEN':'SECRET_VALUE','LIMITLESS_OWNER_IDS':'1','LIMITLESS_GUILD_ID':'1',
             'LIMITLESS_CHANNEL_IDS':'1','LIMITLESS_ALLOW_DISCORD':'1'}
        with patch.dict(os.environ,env),patch('collab.bot.create_bot') as factory:
            factory.return_value.run.side_effect=RuntimeError('SECRET_VALUE')
            with self.assertRaises(Denied) as context: run_bot(self.root/'data')
            self.assertNotIn('SECRET_VALUE',str(context.exception))

    def interaction(self,key,user=1):
        return SimpleNamespace(id=key,user=SimpleNamespace(id=user,bot=False),guild_id=1,channel_id=1,
                               response=SimpleNamespace(defer=AsyncMock()),followup=SimpleNamespace(send=AsyncMock()))

    def pending_art(self,key):
        path=self.root/(key+'.txt'); path.write_bytes(b'synthetic fixture')
        payload=sample('PlanningAgent'); payload.update(artifact=str(path),artifact_sha=sha256(path))
        task_id=self.store.command(self.owner,key,'task',payload)['task_id']
        self.store.triage(task_id); task=self.store.claim()
        self.store.finish(task_id,task['lease'],'TECH_PASS',{'mode':'LOCAL_FILE_QA'})
        self.store.auto_handoff(task_id)
        return task_id,sha256(path),payload['scope']

    def test_all_nine_callbacks_owner_and_reply_receipts(self):
        async def scenario():
            a=self.pending_art('approve'); b=self.pending_art('reject')
            tid=self.store.command(self.owner,'handoff-task','task',sample())['task_id']
            self.store.triage(tid); running=self.store.claim()
            self.store.finish(tid,running['lease'],'TECH_PASS',{'mode':'MOCK'})
            bot=create_bot(self.store,self.root/'data')
            group=bot.tree.get_command('limitless')
            params={'status':(), 'task':(json.dumps(sample()),), 'handoff':(tid,), 'pending':(),
                    'approve':a, 'reject':b, 'pause':('RenewalArtAgent',), 'resume':('RenewalArtAgent',),
                    'budget':(0,0,0)}
            for i,(name,args) in enumerate(params.items(),100):
                interaction=self.interaction(i)
                await group.get_command(name).callback(interaction,*args)
                self.assertEqual(1,interaction.followup.send.await_count,name)
            with self.store.tx() as db:
                kinds={row[0] for row in db.execute("SELECT kind FROM audit WHERE kind LIKE 'DISCORD_REPLY_SENT_%'")}
            self.assertEqual(9,len(kinds))
            await bot.close()
        asyncio.run(scenario())

    def test_all_nine_commands_reject_non_owner(self):
        async def scenario():
            bot=create_bot(self.store,self.root/'data')
            group=bot.tree.get_command('limitless')
            params={'status':(), 'task':('sensitive-ignored-input',), 'handoff':('x',), 'pending':(),
                    'approve':('x','0'*64,'x'), 'reject':('x','0'*64,'x'), 'pause':(), 'resume':(),
                    'budget':(1,1,1)}
            for i,(name,args) in enumerate(params.items(),200):
                interaction=self.interaction(i,user=999)
                await group.get_command(name).callback(interaction,*args)
                self.assertIn('바울 대표 ID',interaction.followup.send.call_args.args[0])
                self.assertNotIn('sensitive-ignored-input',interaction.followup.send.call_args.args[0])
            self.assertEqual([],self.store.snapshot()['tasks'])
            self.assertTrue(self.store.snapshot()['roles']['SFXAgent'])
            await bot.close()
        asyncio.run(scenario())

    def test_ready_receipt_has_no_token_or_account_data(self):
        async def scenario():
            bot=create_bot(self.store,self.root/'data')
            bot.tree.fetch_commands=AsyncMock(return_value=[])
            bot.tree.sync=AsyncMock(return_value=[SimpleNamespace(name='limitless')])
            await bot.setup_hook(); await bot.on_ready()
            receipt=json.loads((self.root/'data/discord-connection.json').read_text(encoding='utf8'))
            self.assertEqual(9,len(receipt['subcommands']))
            self.assertEqual('CONNECTED_COMMANDS_REGISTERED',receipt['status'])
            self.assertFalse(receipt['tokens_logged'])
            self.assertNotIn('user_id',receipt)
            await bot.close()
        asyncio.run(scenario())
