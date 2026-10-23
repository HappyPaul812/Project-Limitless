# Planning 출력 거부 조사·최소 수정 — 추가 AI 호출0

## 결론과 증거 한계

정확히 확인한 발생 지점은 `a59b5783`의 `collab/real_roles.py:107`, `parse_result`의 통합 거부 반환입니다. 종료코드0이므로 당시 `turns!=1`, `invalid`, `not valid` 중 하나 이상이 거부 원인입니다. 그러나 **실제 호출에서 어느 세부 조건이 참이었는지는 확정할 수 없습니다.** 기존 원문 JSONL/최종 응답/진단카운터가 저장되지 않았습니다. 이를 모델 정책위반이나 출력잘림으로 단정하지 않습니다.

합성 재생으로는 확인된 파서 결함이 있습니다. 정상 최종 JSON 앞에 `agent_message` 중간 설명이 있으면, 기존 코드는 그 설명에도 json.loads를 적용하고 실패 플래그를 켭니다. 공식 최종 JSON이 뒤에 있어도 통합 거부됩니다. 이 결함은 수정했지만 **합성 재현이 과거 실제 호출 원인을 입증하는 것은 아닙니다.** 과거 작업은 NOT_VERIFIED로 유지합니다.

## 읽기 전용 조사 범위

- 기존 planning-actual-result.json, queue evidence/usage, 승인 계획, response-schema.json, 사용한 실행 코드/옵션을 확인했습니다. 원문은 기존 subprocess 메모리에서 폐기됐습니다.
- 당시 `exec --json --ephemeral --ignore-user-config --sandbox read-only --output-schema ... -`, timeout90초, `capture_output=True`, UTF8/errors=replace. `--output-last-message`는 사용하지 않았습니다. 애플리케이션에서 stdout 길이를 잘라 전달하는 코드는 없었습니다. 이것만으로 모델 최종 JSON의 완전성을 증명할 수는 없습니다.
- 원래 실행 폴더에는 schema만 있고 final 파일이 없습니다. ‘최종 파일이 없었다’는 사실과 ‘모델이 최종 답변을 만들지 않았다’는 주장을 구분합니다. CLI종료0·입력18075/출력385는 보존했으나 답변 자체를 복원할 수 없습니다.
- 사용자 요청에 한해 Codex 로그 DB를 mode=ro로 열어 테이블 구조와 **정확한 격리cwd**만 조회했습니다. 일치하는 저장 스레드0·롤아웃0. 다른 대화/인증파일/계정 비밀은 조회하지 않았으며 로그 원문도 출력하지 않았습니다. 모든 OS 로그의 부재를 주장하지 않습니다.
- 누락/잘림/역할 불일치/추가필드/도구 이벤트/완료turn수 오류 모두 과거 세부 판정 불명입니다. 정책위반이 확인됐다는 근거는 없습니다.

## 수정 범위

`collab/output_parser.py`로 순수 파서를 분리했습니다. JSONL 이벤트의 종료·완료횟수·도구/오류 검사는 끝까지 수행하고 최종 AI JSON은 별도로 검사합니다. 중간 commentary와 명시 final_answer를 구분합니다. phase 없는 CLI는 공식 최종파일과 마지막 후보가 일치해야 하며, 파일 없이 후보가 여러 개면 계속 거부합니다.

`real_roles.py`는 기존 output-schema를 유지하고 공식 `--output-last-message`를 추가합니다. 최종파일과 JSONL이 일치하지 않거나 누락되면 거부합니다. 임시 원문은 성공/timeout 모두 정리하고, 결과에는 이벤트 수·완료turn수·문자수·최종응답 출처·고정 거부코드만 남깁니다. 원시 stdout/stderr/중간 설명/인증정보는 진단에 남기지 않습니다. 정리 실패도 NOT_VERIFIED로 처리합니다.

필수3필드·역할일치·값형식/길이1200상한·단일완료turn·도구차단·오류차단을 유지했습니다. 중복 JSON키/비정상 상수/알 수 없는 이벤트나 phase도 거부합니다. 최종파일이 있어도 도구·오류·추가turn이 발견되면 PASS하지 않습니다. CLI exit0과 validation REJECTED를 각각 기록하고, timeout은 결과검증 미평가로 구분합니다. 종료0인 모델 텍스트의 단어를 외부 HTTP 오류로 오분류하지 않도록 stderr 분류는 종료오류에만 적용합니다.

Discord 오류문은 고정 한국어 설명으로 표시합니다. 에러 이벤트에 섞인 원문 비밀은 전송하지 않습니다. 과거 실패 장부만 읽은 오류 재게시 message1563146632512671807 전송PASS. 이는 새 추론이나 실패 재실행이 아닙니다. 기존 Discord 프로세스는 그대로 연결 유지했습니다.

공식 근거: [JSONL/최종파일/구조화 출력](https://learn.chatgpt.com/docs/non-interactive-mode), [응답 phase](https://learn.chatgpt.com/docs/app-server). 설치 CLI의 `exec --help`에서 json/ephemeral/output-schema/output-last-message 지원을 확인했으며 `exec` 추론은 실행하지 않았습니다.

## AI 없는 재생 및 테스트

고정 과거 커밋에서 파서 함수만 추출해 합성 이벤트를 재생했습니다. 기존 실패 원문을 복구한 샘플이라고 표시하지 않습니다.

|합성 사례|기존|수정|
|---|---|---|
|최종 JSON1개|PASS|PASS|
|중간 설명+최종 JSON|통합거부|PASS(최종파일/phase 대조)|
|도구 실행+정상 최종 JSON|통합거부|FORBIDDEN_ITEM 거부|

**39 PASS / 0 FAIL**: 새 파서22·입력선별4·변경 실행기 가짜subprocess13. 마지막 timeout/정상파일정리2개 재확인은 중복 합산하지 않습니다. 정상/구형출력, 최종파일누락/불일치, 필수필드누락·추가, null/배열, 역할/길이오류, 중복키, JSONL/최종JSON잘림, 완료누락·다중turn, 도구/오류 이벤트, 미지원 이벤트/phase, 비밀 제외 한국어 Discord 오류, exit와validation 분리, 승인없음/만료/소진/다른역할차단, 중복실행방지를 확인했습니다. 모든 모델 subprocess는 fake이며 유료/실제 AI 호출0입니다.

## 입력 절약 후보

기존 승인계획은 수정하지 않고 `compact_input.py`로 새 **미승인 후보**만 생성했습니다. 입력25497→16687bytes, **34.6% 감소**. AGENTS 및 PLANNING 역할 지침은 전체 원문을 보존하고 Git blob SHA로 완전성을 확인합니다. 같은 소스 중복과 반복 URL/JSON 메타데이터를 제거하고, 이번 ‘현재 업무 요약’과 직접 관련 없는 세계관 일반설명/오래된 CURRENT_STATUS 이력 대신 고정SHA의 최신 기획 인계 발췌를 사용합니다. 원본 문서나 승인 규칙을 고치지 않았고 최신 인계가 발췌라는 사실도 유지합니다.

main47ad2eec와 docs/planning d73bdbf9 등의 기존 고정 참조는 그대로입니다. 후보계획SHA `beeff74c5482f9fb374a58530c11d19d30811e9dc87f221a48e6a3c6ac1f8022`. 기존 승인SHA/실행 진입점에 이 후보를 덮어쓰지 않았습니다. 바이트 감소는 토큰 감소와 동일하지 않으며 CLI 고정 시스템 문맥 비용도 있어 실제 절감량은 미검증입니다. 새 실제 계획 검토/별도 승인 없이는 사용할 수 없습니다.

## 보호와 종료 상태

실제 실행 예약/사용량 행은 기존1개 그대로, 슬롯CONSUMED, 과거 업무NOT_VERIFIED. DONE2/APPROVAL_PENDING1/NOT_VERIFIED2 유지. 승인 장부/SQLite/MOCK/미술 상태를 되돌리거나 초기화하지 않았습니다. 원본7356파일 SHA변경0·누락0·원본Git상태동일. 게임/Unity/Save/UserData/승인아트/Push/main병합0·SFX PAUSED.

이번 코드 검증을 위해 실제 AI 호출은 필요하지 않았고 실행하지 않았습니다. **수정된 파서와 실제 설치 CLI의 end-to-end 성공 확인에는 다음 별도 승인1회가 필요**합니다. 이번에는 후보만 남기고 중지합니다. 브랜치 codex/discord-ai-collaboration, 로컬커밋만 기록합니다.

[비밀 없는 근거·합성 재생·후보 입력](../Evidence/PlanningOutputFix20261023/).
