"""실제 실패 원본을 복구한 테스트가 아닙니다. 합성 이벤트로 진단 보존과 거부 유지를 확인합니다."""
import json
import unittest
from collab.failure_evidence import classify, failure_projection
from collab.output_parser import parse_result


def encode(events):
    return '\n'.join(json.dumps(e) for e in events)


class FailureEvidenceTests(unittest.TestCase):
    def test_ten_errors_preserved_without_secret_text(self):
        events = [{'type': 'error', 'message': 'unauthorized secret-token-example'} for _ in range(10)]
        details, replay = failure_projection(encode(events), 'secret-token-example')
        self.assertEqual(10, len(details['errors']))
        self.assertTrue(all(e['categories'] == ['AUTH'] for e in details['errors']))
        self.assertNotIn('secret-token-example', json.dumps(details) + replay)

    def test_item_types_are_evidence_not_proof_of_tool_execution(self):
        events = [{'type': 'item.completed', 'item': {'type': t}} for t in ('error', 'todo_list')]
        details, _ = failure_projection(encode(events), '')
        self.assertEqual({'error': 1, 'todo_list': 1}, details['item_types'])
        self.assertTrue(all(not i['policy_allowed'] for i in details['items']))
        self.assertIn('FORBIDDEN_ITEM', parse_result(encode(events), 1, 'PlanningAgent')['reason_codes'])

    def test_known_failure_projection_keeps_rejection_reasons(self):
        events = [{'type': 'thread.started'}, {'type': 'turn.started'},
                  {'type': 'item.completed', 'item': {'type': 'warning'}},
                  {'type': 'item.completed', 'item': {'type': 'command_execution', 'command': 'SECRET'}},
                  *[{'type': 'error', 'message': 'Reconnecting'} for _ in range(10)],
                  {'type': 'turn.failed', 'error': {'message': 'stream disconnected'}}]
        raw = encode(events)
        details, replay = failure_projection(raw, '')
        original = parse_result(raw, 1, 'PlanningAgent', require_last_message=True)
        projected = parse_result(replay, 1, 'PlanningAgent', require_last_message=True)
        self.assertEqual(original['reason_codes'], projected['reason_codes'])
        self.assertEqual('NOT_VERIFIED', projected['status'])
        self.assertEqual(['NETWORK'], details['turn_failed'][0]['categories'])
        self.assertNotIn('SECRET', replay)

    def test_numeric_and_known_codes_only(self):
        raw = encode([{'type': 'error', 'error': {'code': 'model_not_found', 'message': 'model_not_found'}},
                      {'type': 'error', 'code': 401}, {'type': 'error', 'code': 'SECRET'}])
        details, _ = failure_projection(raw, '')
        self.assertEqual(['model_not_found', 401, None], [e['code'] for e in details['errors']])

    def test_unknown_not_guessed(self):
        self.assertEqual(['UNKNOWN'], classify('opaque failure'))

    def test_permission_and_schema_classification(self):
        self.assertEqual(['PERMISSION'], classify('Access is denied'))
        self.assertEqual(['CONFIG_SCHEMA'], classify('invalid_json_schema'))

    def test_invalid_line_remains_rejected(self):
        details, replay = failure_projection('{INVALID', '')
        self.assertEqual([1], details['invalid_lines'])
        self.assertIn('JSONL_INVALID_OR_TRUNCATED', parse_result(replay, 1, 'PlanningAgent')['reason_codes'])

    def test_unknown_item_fields_do_not_leak(self):
        raw = encode([{'type': 'item.completed', 'item': {'type': ['SECRET'], 'arguments': 'SECRET'}}])
        details, replay = failure_projection(raw, '')
        self.assertEqual({'unknown_item': 1}, details['item_types'])
        self.assertNotIn('SECRET', json.dumps(details) + replay)


if __name__ == '__main__':
    unittest.main()
