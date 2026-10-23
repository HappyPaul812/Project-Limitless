"""합성 JSONL로만 검증합니다. 실제 실패 원문을 복구한 샘플이 아닙니다."""
import json
import unittest
from collab.output_parser import parse_result
from collab.real_roles import discord_text

ANSWER={'role':'PlanningAgent','summary':'현재 업무와 승인 대기를 검토합니다.','next_review':'바울 대표'}


def message(text,phase=None):
    item={'type':'agent_message','text':text}
    if phase: item['phase']=phase
    return {'type':'item.completed','item':item}


def stream(*items,complete=True):
    events=[{'type':'thread.started','thread_id':'synthetic'},{'type':'turn.started'},*items]
    if complete: events.append({'type':'turn.completed','usage':{'input_tokens':18,'output_tokens':3}})
    return '\n'.join(json.dumps(x,ensure_ascii=False) for x in events)


class OutputParserTests(unittest.TestCase):
    def result(self,events=None,answer=ANSWER,**kwargs):
        text=json.dumps(answer,ensure_ascii=False)
        return parse_result(events if events is not None else stream(message(text)),0,'PlanningAgent',**kwargs)

    def rejected(self,result,reason):
        self.assertEqual('NOT_VERIFIED',result['status'])
        self.assertIn(reason,result['reason_codes'])

    def test_legacy_single_valid(self):
        self.assertEqual('PASS',self.result()['status'])

    def test_commentary_and_explicit_final(self):
        text=json.dumps(ANSWER)
        r=self.result(stream(message('정본을 검토합니다.','commentary'),message(text,'final_answer')))
        self.assertEqual('PASS',r['status']);self.assertEqual(1,r['diagnostics']['commentary_completed'])

    def test_legacy_commentary_uses_official_final_file(self):
        text=json.dumps(ANSWER)
        r=self.result(stream(message('중간 설명은 JSON이 아닙니다.'),message(text)),last_message=text,require_last_message=True)
        self.assertEqual('PASS',r['status'])

    def test_unphased_multiple_without_file_still_rejected(self):
        self.rejected(self.result(stream(message('중간 설명'),message(json.dumps(ANSWER)))),'FINAL_AMBIGUOUS')

    def test_final_file_required(self):
        self.rejected(self.result(require_last_message=True),'FINAL_OUTPUT_MISSING')

    def test_final_file_mismatch(self):
        self.rejected(self.result(last_message=json.dumps({**ANSWER,'summary':'다름'})),'FINAL_EVENT_FILE_MISMATCH')

    def test_file_alone_without_event_rejected(self):
        self.rejected(self.result(stream(),last_message=json.dumps(ANSWER)),'FINAL_EVENT_MISSING')

    def test_missing_field(self):
        self.rejected(self.result(answer={'role':'PlanningAgent','summary':'내용'}),'FINAL_FIELDS_INVALID')

    def test_extra_field(self):
        self.rejected(self.result(answer={**ANSWER,'approve':True}),'FINAL_FIELDS_INVALID')

    def test_null_and_array_are_not_objects(self):
        for value in (None,[],True):
            with self.subTest(value=value): self.rejected(self.result(answer=value),'FINAL_FIELDS_INVALID')

    def test_role_mismatch(self):
        self.rejected(self.result(answer={**ANSWER,'role':'CodexAgent'}),'FINAL_ROLE_MISMATCH')

    def test_empty_or_long_value(self):
        for value in ('','가'*1201,3):
            with self.subTest(value=value if isinstance(value,int) else len(value)):
                self.rejected(self.result(answer={**ANSWER,'summary':value}),'FINAL_VALUE_INVALID')

    def test_duplicate_key_and_truncated_final(self):
        for text in ('{"role":"PlanningAgent","role":"CodexAgent"}', '{"role":"PlanningAgent"'):
            self.rejected(self.result(stream(message(text))),'FINAL_JSON_INVALID_OR_TRUNCATED')

    def test_truncated_jsonl(self):
        self.rejected(self.result(stream(message(json.dumps(ANSWER)))+'\n{"type":'),'JSONL_INVALID_OR_TRUNCATED')

    def test_missing_completed_turn(self):
        self.rejected(self.result(stream(message(json.dumps(ANSWER)),complete=False)),'TURN_COUNT_INVALID_OR_INCOMPLETE')

    def test_multiple_completed_turns(self):
        raw=stream(message(json.dumps(ANSWER)))+'\n'+json.dumps({'type':'turn.completed'})
        self.rejected(self.result(raw),'TURN_COUNT_INVALID_OR_INCOMPLETE')

    def test_tools_blocked_even_with_valid_final_file(self):
        for kind in ('command_execution','file_change','mcp_tool_call','web_search','plan'):
            raw=stream({'type':'item.started','item':{'type':kind}},message(json.dumps(ANSWER)))
            with self.subTest(kind=kind):
                self.rejected(self.result(raw,last_message=json.dumps(ANSWER)),'FORBIDDEN_ITEM')

    def test_error_event_blocks_and_secrets_not_reported(self):
        r=self.result(stream({'type':'error','message':'SECRET_FIXTURE_NOT_A_CREDENTIAL'},message(json.dumps(ANSWER))))
        self.rejected(r,'CLI_ERROR_EVENT')
        self.assertNotIn('SECRET_FIXTURE',json.dumps(r)+discord_text(r))
        self.assertIn('실패 이벤트',discord_text(r))

    def test_exit_and_validation_separated(self):
        r=self.result(answer={'invalid':'value'})
        self.assertEqual('EXIT_ZERO',r['execution_status']);self.assertEqual('REJECTED',r['validation_status'])
        bad=parse_result(stream(message(json.dumps(ANSWER))),1,'PlanningAgent')
        self.rejected(bad,'EXIT_NONZERO')

    def test_unknown_event_and_phase_rejected(self):
        self.rejected(self.result(stream({'type':'unknown'},message(json.dumps(ANSWER)))),'UNKNOWN_EVENT_TYPE')
        self.rejected(self.result(stream(message(json.dumps(ANSWER),'unknown'))),'INVALID_MESSAGE_PHASE')

    def test_multiple_explicit_finals_rejected(self):
        text=json.dumps(ANSWER)
        self.rejected(self.result(stream(message(text,'final_answer'),message(text,'final_answer')),last_message=text),'FINAL_AMBIGUOUS')

    def test_diagnostics_only_counts_not_message_body(self):
        r=self.result(stream(message('PRIVATE_PROGRESS_FIXTURE','commentary'),message(json.dumps(ANSWER),'final_answer')))
        self.assertNotIn('PRIVATE_PROGRESS_FIXTURE',json.dumps(r))
