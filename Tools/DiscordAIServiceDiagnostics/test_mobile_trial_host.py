"""새 Host의 REST 범위·단일 삭제·불명 결과 경계만 오프라인 검증합니다."""
import importlib.util
import json
from pathlib import Path
import tempfile
import unittest
from unittest.mock import patch

spec = importlib.util.spec_from_file_location('mobile_host', Path(__file__).with_name('mobile-trial-host.py'))
host = importlib.util.module_from_spec(spec)
spec.loader.exec_module(host)


class TrialHostTests(unittest.TestCase):
    def setUp(self):
        scratch = Path('F:/study/codex/Project-Limitless/Temp/MobileHostTests')
        scratch.mkdir(exist_ok=True)
        self.tmp = tempfile.TemporaryDirectory(dir=scratch)
        self.root = Path(self.tmp.name)

    def tearDown(self):
        self.tmp.cleanup()

    def test_owner_individual_team_and_wrong_app(self):
        self.assertTrue(host.owner_matches({'id': str(host.TRIAL), 'owner': {'id': str(host.OWNER)}}))
        self.assertTrue(host.owner_matches({'id': str(host.TRIAL), 'team': {'owner_user_id': str(host.OWNER)}}))
        self.assertFalse(host.owner_matches({'id': str(host.PRODUCTION), 'owner': {'id': str(host.OWNER)}}))
        self.assertFalse(host.owner_matches({'id': str(host.TRIAL), 'owner': {'id': 'wrong'}}))

    def test_rest_rejects_production_post_bulk_put_and_arbitrary_delete(self):
        rest = host.Rest('SYNTHETIC', self.root)
        for method, path in [('POST', f'/applications/{host.PRODUCTION}/guilds/{host.GUILD}/commands'),
                             ('PUT', f'/applications/{host.TRIAL}/guilds/{host.GUILD}/commands'),
                             ('DELETE', f'/applications/{host.TRIAL}/guilds/{host.GUILD}/commands/999')]:
            with self.subTest(method=method), self.assertRaises(ValueError), patch.object(host.http.client, 'HTTPSConnection', side_effect=AssertionError('NETWORK_FORBIDDEN')):
                rest.request(method, path)

    def test_inventory_order_does_not_change_comparison(self):
        a = [{'id': '1', 'name': 'a'}, {'id': '2', 'name': 'b'}]
        self.assertEqual(host.canonical_inventory(a), host.canonical_inventory(a[::-1]))

    def prepare(self):
        host.save(self.root / 'registered.json', {'id': '123', 'name': host.NAME, 'application_id': str(host.TRIAL)})
        host.save(self.root / 'commands-before.json', [{'id': '55', 'name': 'untouched'}])
        host.save(self.root / 'global-commands-before.json', [])

    def test_cleanup_deletes_exact_id_once_and_restores_inventory(self):
        self.prepare()
        class Fake:
            calls = []
            def request(inner, method, path):
                inner.calls.append((method, path))
                if method == 'DELETE': return None
                return [] if path.endswith(f'{host.TRIAL}/commands') else [{'id': '55', 'name': 'untouched'}]
        fake = Fake()
        result = host.cleanup(self.root, fake)
        self.assertEqual('CLEANUP_PASS', result['status'])
        self.assertEqual(1, sum(m == 'DELETE' for m, _ in fake.calls))
        host.cleanup(self.root, fake)
        self.assertEqual(3, len(fake.calls))

    def test_delete_unknown_result_never_retries(self):
        self.prepare()
        class Fake:
            calls = 0
            def request(inner, *_):
                inner.calls += 1
                raise OSError('synthetic')
        fake = Fake()
        with self.assertRaises(OSError): host.cleanup(self.root, fake)
        self.assertEqual('DELETE_RESULT_UNKNOWN_NO_RETRY', host.cleanup(self.root, fake)['status'])
        self.assertEqual(1, fake.calls)

    def test_unknown_create_never_guesses_id(self):
        host.save(self.root / 'create-attempted.json', {})
        self.assertEqual('CREATE_RESULT_UNKNOWN_HOLD', host.cleanup(self.root, None)['status'])

    def test_corrupted_binding_does_not_delete(self):
        host.save(self.root / 'registered.json', {'id': '123', 'name': host.NAME, 'application_id': str(host.PRODUCTION)})
        with self.assertRaises(ValueError): host.cleanup(self.root, None)

    def test_existing_launch_marker_cannot_be_reissued(self):
        host.save(self.root / 'launch-consumed.json', {}, exclusive=True)
        with self.assertRaises(FileExistsError): host.save(self.root / 'launch-consumed.json', {}, exclusive=True)


if __name__ == '__main__':
    unittest.main()
