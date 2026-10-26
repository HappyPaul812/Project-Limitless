"""기존 서비스 로그 대신 합성 JSONL로만 정제 경계를 검사합니다."""
import ast
import importlib.util
import json
from pathlib import Path
import unittest

HERE=Path(__file__).resolve().parent
spec=importlib.util.spec_from_file_location('projection',HERE/'project-existing-inference-log.py')
m=importlib.util.module_from_spec(spec);spec.loader.exec_module(m)


def raw(events):return '\n'.join(json.dumps(v,ensure_ascii=False) for v in events).encode()


class ProjectionTests(unittest.TestCase):
    def project(self,events,stderr=b'',launch=None):
        return m.project(raw(events),stderr,{'process':{'exit_code':1,'timed_out':False}},launch or {},{'cli_sha256':'a'*64})
    def test_all_ten_errors_preserved(self):
        events=[{'type':'error','message':'HTTP status 403 permission denied Bearer sk-SYNTHETICSECRET'} for _ in range(10)]
        r=self.project(events);self.assertEqual(r['error_events_total'],10);self.assertEqual(len(r['errors']),10)
        self.assertEqual(r['errors'][0]['http_status_codes'],[403]);self.assertNotIn('SYNTHETICSECRET',json.dumps(r))
    def test_turn_failed_actual_code(self):
        r=self.project([{'type':'turn.failed','error':{'code':-32001,'message':'stream disconnected before completion'}}])
        self.assertEqual(r['turn_failed'][0]['error_code'],-32001);self.assertIn('TRANSPORT',r['turn_failed'][0]['signals'])
    def test_three_actual_item_types_and_order(self):
        r=self.project([{'type':'item.completed','item':{'type':t}} for t in ['mcp_startup','command_execution','system_message']]+[{'type':'turn.started'}])
        self.assertEqual([v['item_type'] for v in r['completed_items']],['mcp_startup','command_execution','system_message'])
        self.assertTrue(all(v['before_turn_started'] for v in r['completed_items']))
    def test_no_false_remote_success(self):
        r=self.project([{'type':'turn.started'},{'type':'turn.failed','error':{'message':'failed'}}])
        self.assertEqual(r['remote_response_evidence']['completed_turns'],0);self.assertFalse(r['remote_response_evidence']['success_confirmed'])
    def test_personal_paths_email_urls_masked(self):
        secret='OwnerPrivateName'
        r=self.project([{'type':'error','message':f'error C:\\Users\\{secret}\\auth.json https://private.invalid/?token=secret owner@example.invalid api_key=sk-EXAMPLE'}],
                       stderr=b'Authorization: Bearer sk-PRIVATE\nHTTP status 401 failed')
        text=json.dumps(r)
        for token in (secret,'owner@example.invalid','private.invalid','sk-EXAMPLE','sk-PRIVATE'):self.assertNotIn(token,text)
    def test_known_string_code_and_secret_code(self):
        r=self.project([{'type':'error','error':{'code':'rate_limit_exceeded','message':'usage limit exceeded'}},
                        {'type':'error','error':{'code':'sk-PRIVATE','message':'error'}}])
        self.assertEqual(r['errors'][0]['error_code'],'rate_limit_exceeded');self.assertIsNone(r['errors'][1]['error_code'])
    def test_unsafe_item_identifier_masked(self):
        r=self.project([{'type':'item.completed','item':{'type':'password=secret'}},{'type':'item.completed','item':{'type':'X'*100}}])
        self.assertTrue(all(v['item_type']=='UNRECOGNIZED_OR_REDACTED_TYPE' for v in r['completed_items']))
    def test_environment_values_and_commands_not_exported(self):
        r=self.project([],launch={'argv':['codex','--sandbox','read-only','-c','forced_login_method="chatgpt"','DO_NOT_EXPORT'],
                                 'environment':{'TOKEN':'PRIVATE'},'environment_keys':['TOKEN']})
        text=json.dumps(r);self.assertNotIn('DO_NOT_EXPORT',text);self.assertNotIn('PRIVATE',text)
        self.assertTrue(r['launch_environment']['sandbox_requested_read_only']);self.assertEqual(r['launch_environment']['exec_authentication_success'],'NOT_CONFIRMED')
    def test_large_error_list_bounded(self):
        r=self.project([{'type':'error','message':'error '*5000} for _ in range(40)])
        self.assertEqual(len(r['errors']),32);self.assertTrue(r['records_truncated']);self.assertLessEqual(len(r['errors'][0]['message']),2048)
    def test_duplicate_json_rejected(self):
        r=m.project(b'{"type":"error","type":"turn.failed"}',b'',{}, {},{})
        self.assertEqual(r['invalid_line_numbers'],[1])
    def test_no_cli_network_db_execution_capability(self):
        tree=ast.parse((HERE/'project-existing-inference-log.py').read_text(encoding='utf8'))
        imported={n.names[0].name.split('.')[0] for n in ast.walk(tree) if isinstance(n,ast.Import)}
        self.assertFalse(imported&{'subprocess','socket','http','urllib','sqlite3','requests','asyncio'})
        calls={n.func.id for n in ast.walk(tree) if isinstance(n,ast.Call) and isinstance(n.func,ast.Name)}
        self.assertFalse(calls&{'eval','exec','compile'})
    def test_identity_query_current_token_only(self):
        # 현재 실행 토큰만 조회하며 서비스 보호 로그나 인증 파일은 열지 않습니다.
        self.assertTrue(m.own_sid().startswith('S-1-'))
    def test_synthetic_dpapi_roundtrip_current_user(self):
        spec=importlib.util.spec_from_file_location('private_log_test',HERE.parent/'DiscordAICollab/collab/private_execution_log.py')
        helper=importlib.util.module_from_spec(spec);spec.loader.exec_module(helper)
        fixture=b'SYNTHETIC_LOG_ONLY_NO_AUTH'
        self.assertEqual(m.decrypt_own(helper.protect_bytes(fixture)),fixture)


if __name__=='__main__':unittest.main(verbosity=2)
