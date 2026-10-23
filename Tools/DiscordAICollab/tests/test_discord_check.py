import asyncio
import json
import tempfile
import unittest
from pathlib import Path
from types import SimpleNamespace
from unittest.mock import AsyncMock, patch

import discord

from collab.discord_check import EXPECTED, check_connection
from collab.core import Denied, Store
from collab.bot import create_bot

ENV={'DISCORD_BOT_TOKEN':'synthetic-not-real','LIMITLESS_OWNER_IDS':'1','LIMITLESS_GUILD_ID':'2',
     'LIMITLESS_CHANNEL_IDS':'3','LIMITLESS_ALLOW_DISCORD':'1'}


class FakeClient:
    def __init__(self,error=None,ready=True,wrong_guild=False,wrong_commands=False):
        self.error=error; self.ready=ready; self.closed=False; self.verified_ready=asyncio.Event()
        names=EXPECTED if not wrong_commands else {'status'}
        command=SimpleNamespace(name='limitless',to_dict=lambda:{'options':[{'name':n} for n in names]})
        self.tree=SimpleNamespace(fetch_commands=AsyncMock(return_value=[command]))
        self.channel=SimpleNamespace(guild=SimpleNamespace(id=99 if wrong_guild else 2),send=AsyncMock(return_value=SimpleNamespace(id=123)))
        self.fetch_channel=AsyncMock(return_value=self.channel)

    async def __aenter__(self): return self
    async def __aexit__(self,*_args): await self.close()
    async def start(self,*_args,**_kwargs):
        if self.error: raise self.error
        if self.ready: self.verified_ready.set()
        await asyncio.Event().wait()
    async def close(self): self.closed=True
    def is_closed(self): return self.closed


class DiscordCheckTests(unittest.TestCase):
    def setUp(self):
        self.tmp=tempfile.TemporaryDirectory(); self.root=Path(self.tmp.name)
    def tearDown(self): self.tmp.cleanup()

    def run_case(self,client,timeout=1):
        async def scenario():
            with patch('collab.discord_check.discord.TextChannel',SimpleNamespace):
                return await check_connection(self.root,timeout,environ=ENV,factory=lambda *_:client)
        return asyncio.run(scenario())

    def test_without_secret_no_network(self):
        def forbidden(*_args): raise AssertionError('must not construct client')
        result=asyncio.run(check_connection(self.root,environ={},factory=forbidden))
        self.assertEqual('WAITING_FOR_USER_SETUP',result['status'])

    def test_bounded_mock_gateway_commands_and_single_message(self):
        client=FakeClient(); result=self.run_case(client)
        self.assertEqual('PASS',result['status'])
        self.assertEqual(9,result['command_count'])
        self.assertEqual(1,client.channel.send.await_count)
        self.assertTrue(client.closed)
        self.assertEqual('NOT_VERIFIED',result['slash_interactions'])
        self.assertNotIn('synthetic-not-real',json.dumps(result))

    def test_wrong_guild_never_sends(self):
        client=FakeClient(wrong_guild=True); result=self.run_case(client)
        self.assertEqual('FAIL',result['status']); self.assertEqual(0,client.channel.send.await_count)

    def test_wrong_remote_command_contract_never_sends(self):
        client=FakeClient(wrong_commands=True); result=self.run_case(client)
        self.assertEqual('COMMAND_CONTRACT_MISMATCH',result['reason'])
        self.assertEqual(0,client.channel.send.await_count)

    def test_login_rejection_never_echoes_secret(self):
        result=self.run_case(FakeClient(error=discord.LoginFailure('synthetic-not-real')))
        self.assertEqual('TOKEN_REJECTED',result['reason'])
        self.assertNotIn('synthetic-not-real',json.dumps(result))

    def test_ready_timeout_closes_without_retry(self):
        client=FakeClient(ready=False); result=self.run_case(client)
        self.assertEqual('TIMEOUT_NO_RETRY',result['reason']); self.assertTrue(client.closed)

    def test_other_app_commands_safety_stop_does_not_echo_raw_exception(self):
        result=self.run_case(FakeClient(error=Denied('UNRELATED_COMMANDS_PRESENT synthetic-not-real')))
        self.assertEqual('COMMAND_SYNC_SAFETY_STOP',result['reason'])
        self.assertNotIn('synthetic-not-real',json.dumps(result))

    def test_timeout_upper_bound(self):
        with self.assertRaises(Denied): asyncio.run(check_connection(self.root,46,environ=ENV))

    def test_sync_never_overwrites_unrelated_commands(self):
        async def scenario():
            client=create_bot(Store(self.root/'queue.sqlite',{1},2,{3}),self.root/'data')
            client.tree.fetch_commands=AsyncMock(return_value=[SimpleNamespace(name='other-command')])
            client.tree.sync=AsyncMock()
            with self.assertRaises(Denied): await client.setup_hook()
            client.tree.sync.assert_not_awaited()
            await client.close()
        asyncio.run(scenario())

    def test_sync_owned_group_uses_existing_registration(self):
        async def scenario():
            client=create_bot(Store(self.root/'queue.sqlite',{1},2,{3}),self.root/'data')
            client.tree.fetch_commands=AsyncMock(return_value=[SimpleNamespace(name='limitless')])
            client.tree.sync=AsyncMock(return_value=[SimpleNamespace(name='limitless')])
            await client.setup_hook()
            client.tree.sync.assert_awaited_once()
            self.assertEqual(9,len(client.tree.get_command('limitless').commands))
            await client.close()
        asyncio.run(scenario())
