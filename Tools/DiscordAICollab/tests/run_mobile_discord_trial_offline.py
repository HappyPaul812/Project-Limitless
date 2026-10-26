"""Phase3 신규 테스트 전용. 프로세스/네트워크 차단과 합성 쓰기 경계를 함께 기록합니다."""
import json
import os
from pathlib import Path
import socket
import subprocess
import sys
import time
import unittest
from unittest.mock import patch

sys.dont_write_bytecode = True
TOOL = Path(__file__).resolve().parents[1]
sys.path.insert(0, str(TOOL))
import discord
from discord import app_commands


def main():
    suite = unittest.defaultTestLoader.discover(str(TOOL / 'tests'), pattern='test_mobile_discord_trial.py')
    scratch = Path('F:/study/codex/Project-Limitless/Temp/MobileDiscordTrial').resolve()
    active, outside = [True], []
    calls = dict(process=0, socket=0, discord=0)

    def audit(event, args):
        if active[0] and event == 'open':
            path, _, flags = args
            if isinstance(path, (str, bytes)) and flags & (os.O_WRONLY | os.O_RDWR | os.O_CREAT | os.O_TRUNC | os.O_APPEND):
                target = Path(os.fsdecode(path)).resolve()
                if not target.is_relative_to(scratch):
                    outside.append(str(target))
                    raise RuntimeError('OFFLINE_WRITE_BOUNDARY')

    def blocked(kind):
        def deny(*args, **kwargs):
            calls[kind] += 1
            raise RuntimeError('OFFLINE_EXTERNAL_CALL_BLOCKED')
        return deny

    async def no_discord(*args, **kwargs):
        calls['discord'] += 1
        raise RuntimeError('DISCORD_LIVE_CALL_BLOCKED')

    sys.addaudithook(audit)
    started = time.monotonic()
    with (patch.object(subprocess, 'Popen', blocked('process')),
          patch.object(subprocess, 'run', blocked('process')),
          patch.object(socket, 'socket', blocked('socket')),
          patch.object(discord.Client, 'login', no_discord),
          patch.object(discord.Client, 'start', no_discord),
          patch.object(app_commands.CommandTree, 'sync', no_discord)):
        result = unittest.TextTestRunner(verbosity=2).run(suite)
    active[0] = False
    report = {'status': 'PASS' if result.wasSuccessful() and not any(calls.values()) and not outside else 'FAIL',
              'tests': result.testsRun, 'failures': len(result.failures), 'errors': len(result.errors),
              'skipped': len(result.skipped), 'elapsed_seconds': round(time.monotonic() - started, 3),
              'sdk_version': discord.__version__, 'blocked_boundary_attempts': calls,
              'outside_scratch_python_write_attempts': outside, 'actual_model_calls': 0,
              'official_usage_reads': 0, 'discord_posts': 0, 'actual_command_registrations': 0,
              'production_sqlite_operations': 0, 'previous_phase1_phase2_tests_repeated': False}
    output = TOOL.parents[1] / '문서/00_프로젝트/Evidence/DiscordMobilePhase3Prepared20261026/OfflineTests.json'
    output.write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding='utf8')
    print(json.dumps(report, ensure_ascii=False))
    return 0 if report['status'] == 'PASS' else 2


if __name__ == '__main__':
    raise SystemExit(main())
