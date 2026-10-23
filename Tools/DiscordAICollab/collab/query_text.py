"""조회 화면만 한국어로 바꿉니다. SQLite 상태/승인 값은 그대로 보존합니다."""
import json

STATES={'NEW':'접수','TRIAGED':'분류 완료','RUNNING':'진행 중','REVIEW_PENDING':'검수 인계 대기',
        'APPROVAL_PENDING':'바울 대표 검토 대기','APPROVED':'승인 기록됨','DONE':'완료',
        'BLOCKED':'보류','FAILED':'실패','CANCELLED':'취소','NOT_VERIFIED':'미검증'}
ROLES={'PlanningAgent':'기획','CodexAgent':'Codex','NewArtAgent':'메인 아트','RenewalArtAgent':'리뉴얼 아트'}


def kind(task):
    evidence=json.loads(task.get('evidence') or '{}')
    payload=json.loads(task.get('payload') or '{}')
    if evidence.get('test_only') is True or payload.get('test_only') is True:
        return 'TEST_ONLY 시험 납품·실제 미술 승인 불가'
    mode=str(evidence.get('mode',''))
    if mode.startswith('MOCK') or str(payload.get('scope','')).startswith('MOCK') or task.get('title','').startswith('MOCK'):
        return 'MOCK 모의 업무'
    if mode=='REAL_CLI_READONLY':
        return '실제 AI 응답' if evidence.get('status')=='PASS' else '실제 AI 요청·응답 미검증'
    if mode=='CODEX_SUBSCRIPTION_PROBE':
        return '과거 연결 시험·응답 미검증'
    return '일반 접수·실제 AI 여부 미확인'


def format_query(action,result,store):
    if action=='status':
        lines=['LIMITLESS 업무 현황','운영: '+('일시 정지' if str(result['paused'])=='1' else '접수 가능')]
        lines += [f"• {STATES.get(row['state'],row['state'])}: {row['count']}건" for row in result['counts']]
        if not result['counts']: lines.append('등록된 업무가 없습니다.')
        lines.append('MOCK는 실제 AI 응답·미술 승인 대상이 아닙니다. AI 실행은 별도 승인 대기입니다.')
        return '\n'.join(lines)
    lines=['LIMITLESS 대기 업무']
    if not result['tasks']: lines.append('현재 대기 업무가 없습니다.')
    for row in result['tasks'][:8]:
        label=kind(store.get(row['id']))
        lines.append(f"• {row['id']} | {ROLES.get(row['role'],row['role'])} | {STATES.get(row['state'],row['state'])}\n  {label}")
        if label.startswith(('MOCK','TEST_ONLY')):
            lines.append('  실제 미술 승인에 사용하지 않습니다.')
        elif label.startswith('실제 AI') or label.startswith('과거 연결'):
            lines.append('  텍스트 연결 검토이며 미술 승인 대상이 아닙니다.')
        else:
            lines.append('  미술 승인은 실제 자산·검수 근거 확인 후 바울 대표가 결정합니다.')
    if len(result['tasks'])>8: lines.append(f"그 밖의 대기 {len(result['tasks'])-8}건은 로컬 장부에서 확인해 주세요.")
    return '\n'.join(lines)
