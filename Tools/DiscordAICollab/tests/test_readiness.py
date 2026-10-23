"""최종 운영 준비 선별 검사. 격리 DB·합성 Discord만 사용하며 AI/OS/제품 변경 없음."""
import json
import unittest
from datetime import datetime
from types import SimpleNamespace
from unittest.mock import AsyncMock
import discord
from tests import test_operations as fixtures
from collab.bot import create_bot
from collab.core import KST
from collab.scheduling import gate,shutdown_preview

class ReadinessTests(unittest.IsolatedAsyncioTestCase,fixtures.Fixture):
    async def asyncSetUp(self):
        self.setup();self.bot=create_bot(self.store,self.root/'adapter');self.sequence=100
    async def asyncTearDown(self):
        await self.bot.close();self.cleanup()
    async def invoke(self,name,owner=1,**kwargs):
        self.sequence+=1
        response=SimpleNamespace(id=self.sequence+1000,flags=SimpleNamespace(ephemeral=True))
        interaction=SimpleNamespace(id=self.sequence,user=SimpleNamespace(id=owner,bot=False),guild_id=1,channel_id=1,
            type=discord.InteractionType.application_command,response=SimpleNamespace(defer=AsyncMock()),
            followup=SimpleNamespace(send=AsyncMock(return_value=response)))
        await self.bot.tree.get_command('limitless').get_command(name).callback(interaction,**kwargs)
        return interaction
    async def test_manual_query_no_mode_change(self):
        item=await self.invoke('mode');self.assertIn('MANUAL',item.followup.send.call_args.args[0])
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT count(*) FROM operation_history').fetchone()[0],0)
    async def test_a_private_reply_receipt_then_manual(self):
        item=await self.invoke('resume');self.assertIn('AUTO_RESUME',item.followup.send.call_args.args[0])
        self.assertTrue(item.followup.send.call_args.kwargs['ephemeral']);self.assertTrue(item.followup.send.call_args.kwargs['wait'])
        with self.store.tx() as db:
            self.assertEqual(tuple(db.execute('SELECT owner_verified,ephemeral,gateway FROM control_interactions').fetchone()),(1,1,1))
            self.assertEqual(db.execute('SELECT count(*) FROM usage').fetchone()[0],0)
        await self.invoke('mode',value='MANUAL')
    async def test_a_owner_denied(self):
        item=await self.invoke('resume',owner=2);self.assertIn('바울 대표 ID',item.followup.send.call_args.args[0])
        with self.store.tx() as db:self.assertEqual(db.execute('SELECT mode FROM operation_mode').fetchone()[0],'MANUAL')
    async def test_b_reserve_finish_cancel_no_integration(self):
        await self.invoke('mode',value='SAFE_SHUTDOWN')
        item=await self.invoke('finish',until='2026-10-24 02:00',scope='TEST_ONLY 읽기 전용 정리')
        self.assertIn('서울 시각',item.followup.send.call_args.args[0])
        with self.store.tx() as db:
            sid=db.execute('SELECT id FROM control_schedules').fetchone()[0]
            self.assertEqual(db.execute('SELECT approval FROM operation_scopes').fetchone()[0],'NO_INTEGRATION_APPROVAL')
        item=await self.invoke('finish',schedule_id=sid);self.assertIn('통합 HOLD',item.followup.send.call_args.args[0])
        self.assertNotIn('SYNC_COMPLETE',item.followup.send.call_args.args[0])
        item=await self.invoke('cancel',schedule_id=sid,release_intake=True);self.assertIn('CANCELLED',item.followup.send.call_args.args[0])
    async def test_usage_korean_two_windows_no_account(self):
        self.observe();item=await self.invoke('usage');text=item.followup.send.call_args.args[0]
        for word in ('5시간','주간','KST'):self.assertIn(word,text)
        self.assertNotIn('accountId',text)
    async def test_schedule_status_preserves_manual(self):
        item=await self.invoke('schedule-status');self.assertIn('MANUAL',item.followup.send.call_args.args[0])
    async def test_sunday_kst_boundary(self):
        saturday=datetime(2026,10,24,23,59,59,tzinfo=KST).timestamp();sunday=saturday+1
        with self.store.tx() as db:
            self.assertIsNone(gate(db,saturday,intake=True));self.assertEqual(gate(db,sunday,intake=True),'SUNDAY_HOLD')
    async def test_sync_or_unsaved_unknown_never_complete(self):
        value=shutdown_preview({'sync_verified':False,'unsaved_work_clear':None},self.now)
        self.assertEqual(value['state'],'HOLD');self.assertEqual(value['os_calls'],0)
        self.assertIn('sync_verified',value['blockers']);self.assertIn('unsaved_work_clear',value['blockers'])
