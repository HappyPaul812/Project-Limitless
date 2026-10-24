"""서비스 CLI 고정 경로 검증만 검사합니다. CLI/인증/AI를 실행하지 않습니다."""
import hashlib
from pathlib import Path
import tempfile
import unittest
from collab.core import Denied
from collab.service_ai import select_executable

class ServiceAuthPinTests(unittest.TestCase):
    def setUp(self):
        Path('data/tmp').mkdir(parents=True,exist_ok=True)
        self.temp=tempfile.TemporaryDirectory(dir='data/tmp')
        self.file=Path(self.temp.name).resolve()/'fake-cli.exe';self.file.write_bytes(b'FAKE_NOT_EXECUTED')
        self.approval={'service_codex':{'path':str(self.file),'sha256':hashlib.sha256(self.file.read_bytes()).hexdigest()}}
    def tearDown(self):self.temp.cleanup()
    def test_pinned_binary_needs_no_service_path_override(self):
        self.assertEqual(select_executable(self.approval,{}),str(self.file))
    def test_tamper_holds_instead_of_falling_back(self):
        self.file.write_bytes(b'CHANGED')
        with self.assertRaises(Denied):select_executable(self.approval,{'PATH':'C:/Windows'})
    def test_missing_binary_holds(self):
        self.file.unlink()
        with self.assertRaises(Denied):select_executable(self.approval,{})
