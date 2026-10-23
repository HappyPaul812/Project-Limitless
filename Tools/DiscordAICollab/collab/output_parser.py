"""JSONL의 실행/정책 검사와 최종 응답 검사를 분리합니다. 원문/추론/비밀은 진단에 남기지 않습니다."""
import json
from collections import Counter

EVENTS={'thread.started','turn.started','turn.completed','turn.failed','error',
        'item.started','item.updated','item.completed'}
ITEMS={'reasoning','agent_message'}
MESSAGES={
    'TIMEOUT_NO_RETRY':'제한 시간을 초과해 중지했습니다. 자동 재시도하지 않습니다.',
    'EXEC_FAILED_NO_RETRY':'CLI 실행을 완료하지 못했습니다. 자동 재시도하지 않습니다.',
    'CLI_OUTPUT_REJECTED_NO_RETRY':'과거 검증기에서 거부됐으나 세부 진단이 남아 있지 않습니다.',
    'RAW_OUTPUT_CLEANUP_FAILED':'임시 응답 원문 정리를 확인하지 못해 결과를 보류했습니다.',
    'EXIT_NONZERO':'CLI가 오류 코드로 종료됐습니다.',
    'JSONL_EMPTY':'실행 이벤트가 비어 있습니다.',
    'JSONL_INVALID_OR_TRUNCATED':'JSONL 이벤트가 잘못되었거나 잘렸습니다.',
    'UNKNOWN_EVENT_TYPE':'지원하지 않는 이벤트가 있어 안전하게 거부했습니다.',
    'CLI_ERROR_EVENT':'CLI가 실패 이벤트를 반환했습니다.',
    'FORBIDDEN_ITEM':'도구 실행 등 허용하지 않은 항목을 발견했습니다.',
    'INVALID_MESSAGE_PHASE':'알 수 없는 응답 단계입니다.',
    'TURN_COUNT_INVALID_OR_INCOMPLETE':'완료된 추론이 정확히 1회인지 확인할 수 없습니다.',
    'FINAL_OUTPUT_MISSING':'최종 응답 파일이 없거나 비어 있습니다.',
    'FINAL_EVENT_MISSING':'최종 응답에 대응하는 이벤트가 없습니다.',
    'FINAL_AMBIGUOUS':'최종 응답 후보가 여러 개여서 확정할 수 없습니다.',
    'FINAL_EVENT_FILE_MISMATCH':'최종 응답 파일과 이벤트 내용이 일치하지 않습니다.',
    'FINAL_JSON_INVALID_OR_TRUNCATED':'최종 응답 JSON이 잘못되었거나 잘렸습니다.',
    'FINAL_FIELDS_INVALID':'최종 응답의 필수 필드가 없거나 추가 필드가 있습니다.',
    'FINAL_ROLE_MISMATCH':'승인된 담당 역할과 응답 역할이 다릅니다.',
    'FINAL_VALUE_INVALID':'필수 응답 값이 비어 있거나 길이·형식이 잘못됐습니다.',
}


def strict_json(text):
    def pairs(items):
        result={}
        for key,value in items:
            if key in result: raise ValueError('duplicate key')
            result[key]=value
        return result
    def bad_constant(_):
        raise ValueError('nonfinite constant')
    return json.loads(text,object_pairs_hook=pairs,parse_constant=bad_constant)


def parse_result(stdout, code, role, *, last_message=None, require_last_message=False):
    reasons=[]; counts=Counter(); item_counts=Counter(); turns=0; usage={}; finals=[]; legacy=[]
    commentary=0; invalid_lines=[]
    def reject(reason):
        if reason not in reasons: reasons.append(reason)
    if code!=0: reject('EXIT_NONZERO')
    if not stdout.strip(): reject('JSONL_EMPTY')
    for number,line in enumerate(stdout.splitlines(),1):
        if not line.strip(): continue
        try:
            if '\ufffd' in line: raise ValueError('decode replacement')
            event=strict_json(line)
            if not isinstance(event,dict) or not isinstance(event.get('type'),str): raise ValueError('event type')
        except (ValueError,TypeError):
            invalid_lines.append(number); reject('JSONL_INVALID_OR_TRUNCATED'); continue
        kind=event['type']; counts[kind if kind in EVENTS else 'unknown']+=1
        if kind not in EVENTS: reject('UNKNOWN_EVENT_TYPE')
        if kind in ('turn.failed','error'): reject('CLI_ERROR_EVENT')
        if kind=='turn.completed':
            turns+=1
            raw=event.get('usage') or {}
            if not isinstance(raw,dict):
                reject('JSONL_INVALID_OR_TRUNCATED')
            else:
                for key in ('input_tokens','cached_input_tokens','output_tokens'):
                    value=raw.get(key)
                    if value is not None:
                        if type(value) is not int or value<0: reject('JSONL_INVALID_OR_TRUNCATED')
                        else: usage[key]=usage.get(key,0)+value
        if kind in ('item.started','item.updated','item.completed'):
            item=event.get('item')
            if not isinstance(item,dict): reject('JSONL_INVALID_OR_TRUNCATED'); continue
            item_type=item.get('type'); item_counts[item_type if item_type in ITEMS else 'forbidden']+=1
            if item_type not in ITEMS: reject('FORBIDDEN_ITEM')
            if item_type=='agent_message' and kind=='item.completed':
                text=item.get('text'); phase=item.get('phase')
                if not isinstance(text,str): reject('JSONL_INVALID_OR_TRUNCATED'); continue
                if phase=='commentary': commentary+=1
                elif phase=='final_answer': finals.append(text)
                elif phase is None: legacy.append(text)
                else: reject('INVALID_MESSAGE_PHASE')
    if turns!=1: reject('TURN_COUNT_INVALID_OR_INCOMPLETE')
    selected=None; source='none'
    if len(finals)>1: reject('FINAL_AMBIGUOUS')
    if require_last_message or last_message is not None:
        source='output-last-message'
        if not isinstance(last_message,str) or not last_message.strip():
            reject('FINAL_OUTPUT_MISSING')
        else:
            selected=last_message
            # phase가 없는 CLI도 공식 최종 파일과 마지막 후보를 대조해 중간 문구와 구분합니다.
            candidate=finals[-1] if finals else (legacy[-1] if legacy else None)
            if candidate is None: reject('FINAL_EVENT_MISSING')
            elif candidate.strip()!=last_message.strip(): reject('FINAL_EVENT_FILE_MISMATCH')
    elif len(finals)==1:
        source='phase-final_answer'; selected=finals[0]
        if legacy: reject('FINAL_AMBIGUOUS')
    elif len(legacy)==1:
        source='legacy-single-message'; selected=legacy[0]
    elif len(legacy)>1: reject('FINAL_AMBIGUOUS')
    else: reject('FINAL_OUTPUT_MISSING')
    answer=None; parsed=False
    if selected is not None:
        try:
            if '\ufffd' in selected: raise ValueError('decode replacement')
            answer=strict_json(selected)
            parsed=True
        except (ValueError,TypeError): reject('FINAL_JSON_INVALID_OR_TRUNCATED')
        if parsed:
            if not isinstance(answer,dict) or set(answer)!={'role','summary','next_review'}:
                reject('FINAL_FIELDS_INVALID')
            else:
                if answer['role']!=role: reject('FINAL_ROLE_MISMATCH')
                if any(not isinstance(value,str) or not 0<len(value)<=1200 for value in answer.values()):
                    reject('FINAL_VALUE_INVALID')
    diagnostics={'event_counts':dict(counts),'item_counts':dict(item_counts),'commentary_completed':commentary,
                 'explicit_final_completed':len(finals),'legacy_messages_completed':len(legacy),
                 'invalid_line_numbers':invalid_lines[:20],'final_source':source,
                 'final_chars':len(selected) if isinstance(selected,str) else 0}
    result={'status':'NOT_VERIFIED' if reasons else 'PASS','mode':'REAL_CLI_READONLY',
            'execution_status':'EXIT_ZERO' if code==0 else 'EXIT_NONZERO','validation_status':'REJECTED' if reasons else 'PASS',
            'usage':usage,'completed_turns':turns,'reason':reasons[0] if reasons else 'OK',
            'reason_codes':reasons,'diagnostics':diagnostics}
    if not reasons: result['answer']=answer
    return result


def korean_reason(result):
    return MESSAGES.get(result.get('reason'),'실행 결과를 검증하지 못했습니다.')
