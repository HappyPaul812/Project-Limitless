"""Discord 첨부 어댑터만 fake message/attachment로 검사합니다."""
import asyncio
import os
import unittest
from types import SimpleNamespace
from unittest.mock import patch
from tests.test_art_delivery import DeliveryTests
from collab.delivery_discord import ingest_message


class Attachment:
    def __init__(self,name,data,number=1):self.filename=name;self.data=data;self.size=len(data);self.id=number;self.reads=0
    async def read(self):self.reads+=1;return self.data


class DiscordDeliveryTests(unittest.TestCase):
    setUp=DeliveryTests.setUp
    tearDown=DeliveryTests.tearDown
    def message(self,author=1,bot=False,guild=1,channel=1):
        return SimpleNamespace(id=10,guild=SimpleNamespace(id=guild),channel=SimpleNamespace(id=channel),
            author=SimpleNamespace(id=author,bot=bot),attachments=[Attachment('sample.png',self.data),Attachment('sample.png.manifest.json',self.meta,2)])
    def ingest(self,msg,probe=None):
        with patch.dict(os.environ,{'LIMITLESS_OWNER_IDS':'1','LIMITLESS_GUILD_ID':'1'}):
            return asyncio.run(ingest_message(msg,SimpleNamespace(user=SimpleNamespace(id=9)),self.engine,{'channel_id':'1'},probe))
    def test_owner_attachment_to_queue(self):
        m=self.message();r=self.ingest(m);self.assertEqual('HOLD',r[0]['status']);self.assertEqual(1,m.attachments[0].reads)
        self.assertEqual('DISCORD_ATTACHMENT',r[0]['inspection']['source'])
    def test_unauthorized_sender_no_download(self):
        m=self.message(author=2);self.assertEqual([],self.ingest(m));self.assertEqual(0,m.attachments[0].reads)
    def test_wrong_channel_and_guild_no_download(self):
        for m in (self.message(channel=2),self.message(guild=2)):
            self.assertEqual([],self.ingest(m));self.assertEqual(0,m.attachments[0].reads)
    def test_bot_upload_excluded_normally(self):
        m=self.message(author=9,bot=True);self.assertEqual([],self.ingest(m));self.assertEqual(0,m.attachments[0].reads)
    def test_exact_self_transport_probe_labelled(self):
        r=self.ingest(self.message(author=9,bot=True),probe=10);self.assertEqual('DISCORD_SELF_TRANSPORT_TEST',r[0]['inspection']['source'])
    def test_other_probe_id_cannot_bypass(self):
        self.assertEqual([],self.ingest(self.message(author=9,bot=True),probe=11))
    def test_repeat_message_not_redownloaded(self):
        m=self.message();self.ingest(m);self.assertEqual([],self.ingest(m));self.assertEqual(1,m.attachments[0].reads)
    def test_oversize_not_downloaded_and_reported(self):
        m=self.message();m.attachments[0].size=33*1024*1024;r=self.ingest(m)
        self.assertEqual('REJECTED',r[0]['status']);self.assertEqual(0,m.attachments[0].reads)
    def test_executable_not_downloaded(self):
        m=self.message();m.attachments=[Attachment('attack.exe',b'MZbad')];r=self.ingest(m)
        self.assertEqual('REJECTED',r[0]['status']);self.assertEqual(0,m.attachments[0].reads)
    def test_stop_request_is_local_only(self):
        from collab import delivery_discord
        with patch.object(delivery_discord,'ROOT',self.root/'control'),patch('sys.argv',['delivery_discord','stop']):
            asyncio.run(delivery_discord.main())
            self.assertTrue((self.root/'control/stop-request').exists())


if __name__=='__main__':unittest.main()
