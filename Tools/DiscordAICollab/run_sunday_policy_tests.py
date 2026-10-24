"""주일 정책 변경과 부팅 중계 보호에 관련된 격리 테스트만 재현합니다. 실제 AI 호출 없음."""
import json
import sys
import tempfile
import unittest
from pathlib import Path

sys.path.insert(0,str(Path.cwd()))
folder=Path('data/sunday-policy');folder.mkdir(parents=True,exist_ok=True)
temp=folder/'tmp';temp.mkdir(exist_ok=True);tempfile.tempdir=str(temp.resolve())
names=[
 'tests.test_sunday_policy',
 'tests.test_modes.ModesTests.test_sunday',
 'tests.test_modes.ModesTests.test_no_manual_resume',
 'tests.test_modes.ModeResumeTests.test_changed_sha_hold',
 'tests.test_modes.ModeResumeTests.test_unknown_sha_hold',
 'tests.test_modes.ModeResumeTests.test_expired_grant_not_extended_or_retried',
 'tests.test_modes.ModeResumeTests.test_consumed_slot_not_reused',
 'tests.test_modes.ModeResumeTests.test_failed_attempt_has_no_auto_retry',
 'tests.test_modes.ModeResumeTests.test_restart_running_is_unknown_never_retried',
 'tests.test_modes.ModeResumeTests.test_sunday_manual_never_runs',
 'tests.test_pilot.CoreTests.test_korean_sunday_allows_work_and_preserves_stop',
 'tests.test_scheduling.SafetyTests.test_sunday_start_allowed_with_usage_gate',
 'tests.test_boot_relay.SupervisorTests.test_wifi_delay_then_connected',
 'tests.test_boot_relay.SupervisorTests.test_offline_bounded_no_launch',
 'tests.test_boot_relay.SupervisorTests.test_disconnect_rechecks_before_restart',
 'tests.test_boot_relay.SupervisorTests.test_reconnect_limit',
 'tests.test_boot_relay.SupervisorTests.test_boot_run_has_no_usage_or_ai_worker',
 'tests.test_boot_relay.BootAdapterTests.test_mode_a_denied_and_usage_hold',
]
class Recorded(unittest.TextTestResult):
    records=[]
    def addSuccess(self,test):super().addSuccess(test);self.records.append({'test':test.id(),'status':'PASS'})
    def addFailure(self,test,err):super().addFailure(test,err);self.records.append({'test':test.id(),'status':'FAIL'})
    def addError(self,test,err):super().addError(test,err);self.records.append({'test':test.id(),'status':'ERROR'})
r=unittest.TextTestRunner(verbosity=2,resultclass=Recorded).run(unittest.defaultTestLoader.loadTestsFromNames(names))
(folder/'regression-tests.json').write_text(json.dumps({'run':r.testsRun,'failures':len(r.failures),'errors':len(r.errors),'actual_ai_calls':0,'tests':Recorded.records},ensure_ascii=False,indent=2),encoding='utf8')
sys.exit(not r.wasSuccessful())
