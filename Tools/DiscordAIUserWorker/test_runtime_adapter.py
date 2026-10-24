"""원본 로그 연결만 모의 subprocess로 검사합니다. 실제 CLI/인증/모델 호출은 없습니다."""
from pathlib import Path
import subprocess
import sys
import unittest
from unittest.mock import Mock,patch

HERE=Path(__file__).resolve().parent
sys.path.insert(0,str(HERE))
from test_queue import QueueTests
from DiscordAIUserWorker.runtime_adapter import recorded_run_once


class RuntimeAdapterTests(QueueTests):
    def test_exec_original_bytes_logged_before_decoding(self):
        original=b'{"type":"error","message":"FIXTURE"}\n\xff'
        process=Mock(return_value=subprocess.CompletedProcess(['mock','exec'],1,original,b'FIXTURE_STDERR'))
        def verified(*args,invoke):
            value=invoke(['mock','exec'],input='FIXTURE_PROMPT',text=True,encoding='utf8',errors='replace',timeout=90)
            self.assertEqual(1,value.returncode);self.assertIsInstance(value.stdout,str)
            return {'status':'NOT_VERIFIED'}
        with patch('DiscordAIUserWorker.runtime_adapter.prepare_archive',return_value=self.root),patch('DiscordAIUserWorker.runtime_adapter.save_execution_log',return_value={'private_only':True}) as save:
            result=recorded_run_once(self.store,self.owner,{'sha256':'a'*64},self.root,self.binary,self.context,verified,process)
            self.assertEqual(original,save.call_args.args[1]);self.assertTrue(result['private_log']['private_only'])
            self.assertEqual(b'FIXTURE_PROMPT',process.call_args.kwargs['input']);process.assert_called_once()

    def test_timeout_partial_preserved_without_retry(self):
        process=Mock(side_effect=subprocess.TimeoutExpired(['mock','exec'],90,output=b'PARTIAL',stderr=b'TIMEOUT'))
        def verified(*args,invoke):return invoke(['mock','exec'],input='FIXTURE',text=True,timeout=90)
        with patch('DiscordAIUserWorker.runtime_adapter.prepare_archive',return_value=self.root),patch('DiscordAIUserWorker.runtime_adapter.save_execution_log',return_value={}) as save:
            with self.assertRaises(subprocess.TimeoutExpired):
                recorded_run_once(self.store,self.owner,{'sha256':'a'*64},self.root,self.binary,self.context,verified,process)
            self.assertEqual(b'PARTIAL',save.call_args.args[1]);process.assert_called_once()

    def test_auth_status_output_not_archived(self):
        process=Mock(return_value=subprocess.CompletedProcess(['mock','login','status'],0,'FIXTURE_AUTH_STATUS',''))
        def verified(*args,invoke):
            invoke(['mock','login','status'],capture_output=True,text=True)
            return {'status':'FIXTURE_ONLY'}
        with patch('DiscordAIUserWorker.runtime_adapter.prepare_archive',return_value=self.root),patch('DiscordAIUserWorker.runtime_adapter.save_execution_log') as save:
            recorded_run_once(self.store,self.owner,{'sha256':'a'*64},self.root,self.binary,self.context,verified,process)
            save.assert_not_called();process.assert_called_once()


def load_tests(loader,tests,pattern):
    return unittest.TestSuite(RuntimeAdapterTests(name) for name in RuntimeAdapterTests.__dict__ if name.startswith('test_'))


if __name__=='__main__':unittest.main()
