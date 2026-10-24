"""합성 데이터만 사용합니다. 모델·로그인·네트워크·다른 계정의 비밀을 읽지 않습니다."""
import ctypes
import json
from pathlib import Path
import tempfile
import unittest
from collab.private_execution_log import Blob, protect_bytes, redact_text, redact_value, save_execution_log
from collab.failure_evidence import failure_projection


class PrivateExecutionLogTests(unittest.TestCase):
    def setUp(self):
        self.temp_root = Path(__file__).resolve().parent.parent / 'data/private-log-tests'
        self.temp_root.mkdir(parents=True, exist_ok=True)

    def test_dpapi_roundtrip_synthetic_bytes(self):
        original = b'{"type":"error","message":"synthetic-only"}\n'
        encrypted = protect_bytes(original)
        self.assertNotIn(original, encrypted)
        source_buffer = ctypes.create_string_buffer(encrypted)
        source = Blob(len(encrypted), ctypes.cast(source_buffer, ctypes.POINTER(ctypes.c_ubyte)))
        target = Blob()
        crypt = ctypes.WinDLL('crypt32', use_last_error=True)
        kernel = ctypes.WinDLL('kernel32', use_last_error=True)
        kernel.LocalFree.argtypes = [ctypes.c_void_p]
        kernel.LocalFree.restype = ctypes.c_void_p
        try:
            ok = crypt.CryptUnprotectData(ctypes.byref(source), None, None, None, None, 1, ctypes.byref(target))
            self.assertTrue(ok)
            self.assertEqual(original, ctypes.string_at(target.data, target.size))
        finally:
            if target.data:
                ctypes.memset(target.data, 0, target.size)
                kernel.LocalFree(target.data)

    def test_redaction_preserves_diagnostic_message(self):
        text = 'stream disconnected Bearer abc.secret https://name:pass@example.com/?access_token=abc&x=1 sk-test123'
        clean = redact_text(text)
        for bad in ('abc.secret', 'name:pass', 'access_token=abc', 'sk-test123'):
            self.assertNotIn(bad, clean)
        self.assertIn('stream disconnected', clean)

    def test_nested_tokens_removed_counts_preserved(self):
        clean = redact_value({'error': {'access_token': 'SECRET'}, 'usage': {'input_tokens': 7}, 'message': 'KNOWN'}, ['KNOWN'])
        self.assertEqual('[REDACTED]', clean['error']['access_token'])
        self.assertEqual(7, clean['usage']['input_tokens'])
        self.assertNotIn('KNOWN', json.dumps(clean))

    def test_exact_bytes_go_only_to_encryptor_and_local_first_error(self):
        raw = b'{"type":"error","message":"first failure","access_token":"SECRET"}\n{"type":"turn.failed","error":{"message":"stream disconnected"}}\n'
        received = []
        def encrypt(data):
            received.append(data)
            return b'SYNTHETIC_ENCRYPTED'
        with tempfile.TemporaryDirectory(dir=self.temp_root) as directory:
            result = save_execution_log(directory, raw, b'Bearer abc.secret', {'exit_code': 1, 'request_id': 'a'*32}, protector=encrypt)
            self.assertEqual([raw], received)
            self.assertEqual(b'SYNTHETIC_ENCRYPTED', (Path(directory)/'stdout.original.jsonl.dpapi').read_bytes())
            event = json.loads((Path(directory)/'execution.json').read_text(encoding='utf8'))
            self.assertEqual('first failure', event['first_error']['event']['message'])
            self.assertEqual(1, len(event['turn_failed']))
            self.assertEqual(1, event['exit_code'])
            self.assertNotIn('SECRET', (Path(directory)/'stdout.redacted.jsonl').read_text(encoding='utf8'))
            self.assertNotIn('first failure', json.dumps(result))

    def test_unsupported_item_and_tool_stay_distinct_and_disallowed(self):
        raw = '\n'.join(json.dumps({'type':'item.completed','item':{'type':kind}}) for kind in ('error','command_execution'))
        details, _ = failure_projection(raw, '')
        self.assertEqual(['UNSUPPORTED_ITEM_TYPE','TOOL_OR_SIDE_EFFECT_REPORTED'], [x['classification'] for x in details['items']])
        self.assertTrue(all(not x['policy_allowed'] for x in details['items']))

    def test_invalid_original_bytes_still_preserved(self):
        with tempfile.TemporaryDirectory(dir=self.temp_root) as directory:
            raw = b'\xff\xfe\n'
            got = []
            save_execution_log(directory, raw, b'', {'exit_code':1}, protector=lambda b: got.append(b) or b'encrypted')
            self.assertEqual([raw], got)


if __name__ == '__main__':
    unittest.main()
