"""실제 CLI/네트워크/운영 DB 없이 단발 실행의 실패·게시 차단을 검사합니다."""
import contextlib
import importlib.util
import io
import json
import os
from pathlib import Path
import sqlite3
import subprocess
import tempfile
import unittest
from unittest.mock import patch

HERE = Path(__file__).resolve().parent
spec = importlib.util.spec_from_file_location('service_final_once', HERE / 'service-final-once.py')
m = importlib.util.module_from_spec(spec)
spec.loader.exec_module(m)

class FixtureStore:
    def __init__(self):
        self.db = sqlite3.connect(':memory:')
        self.db.executescript("CREATE TABLE operation_mode(singleton INTEGER,mode TEXT);INSERT INTO operation_mode VALUES(1,'MANUAL');CREATE TABLE settings(key TEXT PRIMARY KEY,value TEXT);CREATE TABLE tasks(state TEXT);")
    @contextlib.contextmanager
    def tx(self):
        with self.db:
            yield self.db
    def setting(self, db, key): return '0'
    def audit(self, db, name): pass

def run_immediate(awaitable):
    try:
        awaitable.send(None)
    except StopIteration as done:
        return done.value
    raise AssertionError('Unexpected asynchronous I/O in offline fixture')

class FinalOnceTests(unittest.TestCase):
    def exercise(self, mode):
        root = m.TOOL / 'data/service-final-offline-tests'
        root.mkdir(exist_ok=True)
        with tempfile.TemporaryDirectory(dir=root) as folder:
            out = Path(folder)
            private = out / 'private'
            private.mkdir()
            store = FixtureStore()
            calls, posts, logs = [], [], []
            answer = dict(role='PlanningAgent', summary='기획 정본을 관리합니다.\n담당자 인계를 조율합니다.', next_review='바울 대표')
            final = json.dumps(answer, ensure_ascii=False)
            events = [dict(type='turn.started'), dict(type='item.completed', item=dict(type='agent_message', phase='final_answer', text=final)), dict(type='turn.completed', usage=dict(input_tokens=10, cached_input_tokens=2, output_tokens=5))]
            if mode == 'error': events = [dict(type='error', message='synthetic network error'), dict(type='turn.failed', error=dict(message='synthetic failure'))]
            if mode == 'forbidden': events.insert(1, dict(type='item.completed', item=dict(type='command_execution')))
            stdout = '\n'.join(json.dumps(x, ensure_ascii=False) for x in events).encode()
            def invoke(command, **kwargs):
                calls.append(kwargs)
                self.assertEqual(kwargs['timeout'], 90)
                self.assertNotIn('DISCORD_BOT_TOKEN', kwargs['env'])
                (out / 'last-message.json').write_text(final, encoding='utf-8')
                if mode == 'timeout': raise subprocess.TimeoutExpired(command, 90, output=stdout, stderr=b'synthetic stderr')
                return subprocess.CompletedProcess(command, 1 if mode == 'error' else 0, stdout, b'')
            async def usage(): return dict(observed=1, source='SYNTHETIC_FIXTURE', ordinary_allowed=True)
            async def publish(result, channel):
                posts.append(result['answer'])
                if mode == 'discord_error': raise OSError('synthetic transport failure')
                return dict(message_id='fixture-only')
            snapshots = iter([{'fixture': 'before'}, {'fixture': 'changed' if mode == 'game_change' else 'before'}])
            def snapshot():
                try: return next(snapshots)
                except StopIteration: return {'fixture': 'changed' if mode == 'game_change' else 'before'}
            class Provider:
                executable = str(m.EXPECTED_CLI)
                environment = {'CODEX_HOME': 'fixture-own-profile'}
                read = staticmethod(usage)
            class Schedule:
                def __init__(self, ignored): pass
                def record_official_usage(self, *args): pass
            def archive(*args): logs.append(args); return dict(directory='synthetic-private-only')
            with contextlib.ExitStack() as stack:
                patches = dict(OUT=out, windows_context=lambda: dict(name='PAUL-PC\\LIMITLESSRelaySvc', profile='fixture'), prepare_provider=Provider,
                    prepare_archive=lambda *args: private, file_sha=lambda path: m.EXPECTED_SHA,
                    game_snapshot=snapshot, Store=lambda *args: store, Scheduler=Schedule, gate=lambda *args, **kwargs: None,
                    cli_info=lambda *args: dict(subscription_available=True, cli_version='fixture'), verify_cli_controls=lambda *args: dict(status='PASS'),
                    fetch_source=lambda: dict(commit='a'*40, text='공개 역할 정본'), publish=publish, save_execution_log=archive)
                for key, value in patches.items(): stack.enter_context(patch.object(m, key, value))
                stack.enter_context(patch.object(m.subprocess, 'run', invoke))
                stack.enter_context(patch.object(m.asyncio, 'run', run_immediate))
                stack.enter_context(patch.object(m.asyncio, 'wait_for', lambda awaitable, timeout: awaitable))
                stack.enter_context(patch.dict(os.environ, dict(LIMITLESS_BATCH_SESSION_ID='0', LIMITLESS_BATCH_LOGON_TYPE='4', LIMITLESS_BATCH_REQUEST_ID='a'*32, LIMITLESS_OWNER_IDS='1', LIMITLESS_CHANNEL_IDS='2', LIMITLESS_GUILD_ID='3', DISCORD_BOT_TOKEN='synthetic-not-a-secret')))
                with contextlib.redirect_stdout(io.StringIO()): code = m.run()
                result = json.loads((out / 'result.json').read_text(encoding='utf-8'))
                self.assertEqual(len(calls), 1)
                self.assertEqual(len(logs), 1)
                # 같은 시험을 다시 실행해도 subprocess는 추가로 호출되지 않아야 합니다.
                with contextlib.redirect_stdout(io.StringIO()): self.assertEqual(m.run(), 2)
                self.assertEqual(len(calls), 1)
            store.db.close()
            return code, result, posts
    def test_success(self):
        code, result, posts = self.exercise('success')
        self.assertEqual((code, result['status'], len(posts)), (0, 'VERIFIED', 1))
    def test_cli_error_blocks_publish(self):
        code, result, posts = self.exercise('error')
        self.assertEqual((code, result['status'], len(posts)), (2, 'NOT_VERIFIED', 0))
    def test_forbidden_item_blocks_publish(self):
        _, result, posts = self.exercise('forbidden')
        self.assertIn('FORBIDDEN_ITEM', result['validation']['reason_codes'])
        self.assertFalse(posts)
    def test_timeout_keeps_partial_evidence(self):
        _, result, posts = self.exercise('timeout')
        self.assertEqual(result['reason'], 'TIMEOUT_90S_NO_RETRY')
        self.assertIn('private_log', result)
        self.assertFalse(posts)
    def test_game_change_blocks_publish(self):
        _, result, posts = self.exercise('game_change')
        self.assertEqual(result['reason'], 'GAME_FILES_CHANGED_HOLD')
        self.assertFalse(posts)
    def test_discord_failure_is_not_verified(self):
        code, result, _ = self.exercise('discord_error')
        self.assertEqual((code, result['status']), (2, 'NOT_VERIFIED'))
    def test_five_sentence_limit(self):
        parsed = dict(status='PASS', answer=dict(summary='문장.\n'*6, next_review='바울 대표'), usage={})
        with self.assertRaises(m.Denied): m.validate_answer(parsed)
    def test_missing_tokens_rejected(self):
        parsed = dict(status='PASS', answer=dict(summary='문장입니다.', next_review='바울 대표'), usage=dict(input_tokens=1))
        with self.assertRaises(m.Denied): m.validate_answer(parsed)

if __name__ == '__main__': unittest.main()
