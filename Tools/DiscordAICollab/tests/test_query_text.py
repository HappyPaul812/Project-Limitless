import json
import tempfile
import unittest
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import AsyncMock
import asyncio
from collab.core import Store, Identity, Denied, sha256
from collab.__main__ import sample
from collab.bot import create_bot
from collab.query_text import format_query, kind


class QueryTextTests(unittest.TestCase):
    def test_korean_status_preserves_counts(self):
        text=format_query('status',{'paused':'0','counts':[{'state':'DONE','count':2},{'state':'APPROVAL_PENDING','count':1},{'state':'NOT_VERIFIED','count':1}]},None)
        for word in ('완료: 2건','바울 대표 검토 대기: 1건','미검증: 1건','별도 승인'):
            self.assertIn(word,text)
        self.assertNotIn('{',text)

    def test_empty_pending(self):
        self.assertIn('없습니다',format_query('pending',{'tasks':[]},None))

    def test_unknown_not_labelled_real(self):
        self.assertIn('미확인',kind({}))

    def test_mock_pending_explicitly_excludes_real_art_approval(self):
        task={'id':'L-fixture','role':'PlanningAgent','state':'APPROVAL_PENDING'}
        store=SimpleNamespace(get=lambda _: {'evidence':json.dumps({'mode':'MOCK'}),'payload':'{}'})
        text=format_query('pending',{'tasks':[task]},store)
        self.assertIn('MOCK 모의 업무',text)
        self.assertIn('실제 미술 승인에 사용하지 않습니다',text)

    def test_real_readonly_never_art_approved(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        with tempfile.TemporaryDirectory(dir='data/tmp') as tmp:
            store=Store(Path(tmp)/'db.sqlite',{1})
            path=Path(tmp)/'fixture.txt'; path.write_text('synthetic')
            payload=sample('PlanningAgent'); payload.update(artifact=str(path),artifact_sha=sha256(path))
            owner=Identity(1,1,1)
            tid=store.command(owner,'fixture','task',payload)['task_id']
            store.triage(tid); task=store.claim()
            store.finish(tid,task['lease'],'TECH_PASS',{'mode':'REAL_CLI_READONLY','status':'PASS'})
            store.auto_handoff(tid)
            with self.assertRaises(Denied):
                store.command(owner,'approve','approve',{'task_id':tid,'sha':sha256(path),'scope':payload['scope']})

    def test_actual_callbacks_send_korean_and_receipts_without_mutating_queue(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        with tempfile.TemporaryDirectory(dir='data/tmp') as tmp:
            store=Store(Path(tmp)/'db.sqlite',{1})
            store.command(Identity(1,1,1),'mock','task',sample())
            before=store.snapshot()
            async def scenario():
                bot=create_bot(store,Path(tmp)/'data')
                for n,name in enumerate(('status','pending'),100):
                    interaction=SimpleNamespace(id=n,user=SimpleNamespace(id=1,bot=False),guild_id=1,channel_id=1,response=SimpleNamespace(defer=AsyncMock()),followup=SimpleNamespace(send=AsyncMock()))
                    await bot.tree.get_command('limitless').get_command(name).callback(interaction)
                    text=interaction.followup.send.call_args.args[0]
                    self.assertIn('LIMITLESS',text)
                    self.assertNotIn('{',text)
                await bot.close()
            asyncio.run(scenario())
            self.assertEqual(before,store.snapshot())
