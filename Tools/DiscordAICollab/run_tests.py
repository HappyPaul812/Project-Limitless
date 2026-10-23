"""실제 실행된 로컬 테스트 이름·결과를 JSON으로 보존합니다."""
import json
import sys
import unittest
from pathlib import Path


class Recorded(unittest.TextTestResult):
    def startTest(self,test):
        super().startTest(test)
        self.records.append({'test':test.id(),'status':'RUNNING'})

    def addSuccess(self,test):
        super().addSuccess(test)
        self.records[-1]['status']='PASS'

    def addFailure(self,test,err):
        super().addFailure(test,err)
        self.records[-1]['status']='FAIL'

    def addError(self,test,err):
        super().addError(test,err)
        self.records[-1]['status']='ERROR'

    def addSubTest(self,test,subtest,err):
        super().addSubTest(test,subtest,err)
        if err is not None:
            self.records[-1]['status']='FAIL'

    def addSkip(self,test,reason):
        super().addSkip(test,reason)
        self.records[-1]['status']='NOT_VERIFIED'


if __name__=='__main__':
    Recorded.records=[]
    suite=unittest.defaultTestLoader.discover('tests')
    result=unittest.TextTestRunner(verbosity=2,resultclass=Recorded).run(suite)
    out=Path('data'); out.mkdir(exist_ok=True)
    (out/'test-results.json').write_text(json.dumps({'mode':'LOCAL_UNIT_AND_MOCK',
        'run':result.testsRun,'failures':len(result.failures),'errors':len(result.errors),
        'skipped':len(result.skipped),'tests':result.records,
        'live_discord':'NOT_VERIFIED','live_ai':'NOT_VERIFIED','unity':'NOT_VERIFIED'},ensure_ascii=False,indent=2),encoding='utf8')
    sys.exit(0 if result.wasSuccessful() else 1)
