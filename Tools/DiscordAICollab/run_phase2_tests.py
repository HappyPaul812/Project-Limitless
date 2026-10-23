"""이번 변경분만 실행하며 실제 네트워크/추론은 사용하지 않습니다."""
import json
import sys
import time
import unittest
from pathlib import Path
from run_tests import Recorded

if __name__=='__main__':
    Recorded.records=[]
    start=time.monotonic()
    suite=unittest.defaultTestLoader.discover('tests',pattern='test_phase2.py')
    result=unittest.TextTestRunner(verbosity=2,resultclass=Recorded).run(suite)
    root=Path('data/phase2'); root.mkdir(parents=True,exist_ok=True)
    (root/'delta-tests.json').write_text(json.dumps({'mode':'LOCAL_MOCK_DELTA_ONLY',
        'run':result.testsRun,'failures':len(result.failures),'errors':len(result.errors),'skipped':len(result.skipped),
        'elapsed_seconds':round(time.monotonic()-start,3),'tests':result.records,
        'live_discord':'NOT_VERIFIED','actual_inference_in_tests':0},ensure_ascii=False,indent=2),encoding='utf8')
    sys.exit(0 if result.wasSuccessful() else 1)
