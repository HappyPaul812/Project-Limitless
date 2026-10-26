"""재개 후 추가한 callback만 검증합니다. 기존 성공 시험은 실행하지 않습니다."""
import asyncio
import unittest
from types import SimpleNamespace
from unittest.mock import patch,Mock
import discord
from collab.session0_planning_bridge import LiveButtonWorkerConnection,LiveSession0Bridge

def drive(coroutine):
    try:coroutine.send(None)
    except StopIteration as result:return result.value
    raise AssertionError('EXTERNAL_AWAIT_FORBIDDEN')

async def direct(function,*args):return function(*args)

class Workflow:
    offline_fixture=False
    def __init__(self):self.pending=True;self.reports=[];self.holds=[]
    def ticket(self,request):return {'request_id':request}
    def accept_result(self,request,result):return True
    def claim_report(self,request):
        if not self.pending:return None
        self.pending=False;return '합성 callback 응답'
    def report_result(self,request,message_id=None):self.reports.append(message_id)
    def hold(self,request,*reason):self.holds.append((request,reason))

class CallbackTests(unittest.TestCase):
    def setUp(self):
        self.workflow=Workflow();self.bridge=LiveSession0Bridge()
        self.connection=LiveButtonWorkerConnection(self.workflow,self.bridge);self.messages=[]
        test=self
        class Followup:
            async def send(inner,text,**kwargs):test.messages.append(text);return SimpleNamespace(id=123)
        self.interaction=SimpleNamespace(followup=Followup())
        self.thread=patch.object(asyncio,'to_thread',direct);self.thread.start()
    def tearDown(self):self.thread.stop()
    def test_success_and_duplicate_callback_only_one_submit(self):
        with patch.object(self.bridge,'submit',return_value=object()) as submit:
            drive(self.connection.on_run('r',self.interaction));drive(self.connection.on_run('r',self.interaction))
            self.assertEqual(1,submit.call_count);self.assertEqual([123],self.workflow.reports)
    def test_delivery_failure_never_submits_again(self):
        async def fail(*args,**kwargs):raise OSError('synthetic')
        self.interaction.followup.send=fail
        with patch.object(self.bridge,'submit',return_value=object()) as submit:
            drive(self.connection.on_run('r',self.interaction));drive(self.connection.on_run('r',self.interaction))
            self.assertEqual(1,submit.call_count);self.assertEqual([None],self.workflow.reports)
    def test_bridge_failure_holds_without_retry(self):
        with patch.object(self.bridge,'submit',side_effect=ValueError('synthetic')) as submit:
            drive(self.connection.on_run('r',self.interaction));drive(self.connection.on_run('r',self.interaction))
            self.assertEqual(1,submit.call_count);self.assertEqual(1,len(self.workflow.holds));self.assertEqual([],self.messages)
    def test_cancel_unknown_holds(self):
        with patch.object(self.bridge,'cancel',side_effect=ValueError('synthetic')) as cancel:
            drive(self.connection.on_cancel('r'));self.assertEqual(1,cancel.call_count)
            self.assertEqual('SERVICE_CANCEL_UNKNOWN_NO_RETRY',self.workflow.holds[0][1][0])

if __name__=='__main__':unittest.main()
