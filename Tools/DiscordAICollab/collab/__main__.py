"""python -m collab demo|serve|status|intake|discord. 기본 실행은 네트워크를 쓰지 않습니다."""
import argparse
import json
import sys
from pathlib import Path

from .adapters import MockWorker
from .core import Denied, Identity, Store
from .intake import inspect_zip, safe_root


def sample(role='RenewalArtAgent', issue=63):
    return {'title':f'MOCK # {issue} 복제 검토 이벤트','role':role,'issue':issue,
            'base_sha':'47ad2eecb40135b2ee28e4947d642ebdbf0bf68a','scope':'MOCK_REVIEW_ONLY',
            'protected_assets':['Unity','Save','UserData','approved_art'],
            'inputs':['MOCK #63 renewal report','#64 QA reference','#65 board reference'],
            'verification':['모의 전달만 검사; 기존 QA 재실행 없음'],
            'approval_required':True,'artifact':'MOCK_NO_ART_FILE','artifact_sha':'0'*64}


def main():
    parser = argparse.ArgumentParser(description='LIMITLESS Discord 협업 로컬 파일럿')
    parser.add_argument('command',choices=('demo','serve','status','intake','discord'))
    parser.add_argument('--data',default=str(Path(__file__).resolve().parent.parent/'data'))
    parser.add_argument('--zip')
    parser.add_argument('--sha')
    args = parser.parse_args()
    root = safe_root(args.data)
    if args.command=='discord':
        from .bot import run_bot
        run_bot(root)
        return
    if args.command=='intake':
        if not args.zip:
            parser.error('--zip 필요')
        print(json.dumps(inspect_zip(args.zip,root/'quarantine',args.sha),ensure_ascii=False,indent=2))
        return
    # stdio는 로컬 모의 입력 전용입니다. 이 숫자는 Discord 신원으로 사용할 수 없습니다.
    store = Store(root/'mock.sqlite',{1})
    owner = Identity(1,1,1)
    if args.command=='demo':
        for issue,role in ((63,'RenewalArtAgent'),(64,'CodexAgent'),(65,'PlanningAgent')):
            store.command(owner,'demo:'+str(issue),'task',sample(role,issue))
        worker = MockWorker(store)
        for _ in range(20):
            worker.tick()
        print(json.dumps({'mode':'MOCK','external_calls':0,'additional_cost_usd':0,
                          'snapshot':store.snapshot()},ensure_ascii=False,indent=2))
    elif args.command=='status':
        print(json.dumps(store.snapshot(),ensure_ascii=False,indent=2))
    else:
        # 1행 JSON -> 1행 결과. 임의 셸 명령/AI 프롬프트를 실행하지 않습니다.
        store.recover()
        for line in sys.stdin:
            try:
                if len(line)>24000:
                    raise Denied('이벤트 크기 초과')
                event = json.loads(line)
                if event.get('mode')!='MOCK':
                    raise Denied('로컬 stdio는 MOCK 전용입니다')
                if event.get('action')=='tick':
                    result = MockWorker(store).tick()
                else:
                    actor = Identity(int(event.get('user_id',0)),1,1)
                    result = store.command(actor,event['event_id'],event['action'],event.get('args'))
                print(json.dumps({'ok':True,'result':result},ensure_ascii=False),flush=True)
            except (Denied,KeyError,ValueError,TypeError):
                print(json.dumps({'ok':False,'error':'접수 거부: 권한·계약·형식 확인'},ensure_ascii=False),flush=True)


if __name__=='__main__':
    try:
        main()
    except Denied as exc:
        print(str(exc),file=sys.stderr)
        sys.exit(2)
