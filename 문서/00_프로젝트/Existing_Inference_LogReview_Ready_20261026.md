# Issue66 기존 추론 로그 조사 준비 / 조건부2단계 승인 기록 (2026-10-26)

**STAGE1_NOT_VERIFIED / LOG_REVIEW_READY_WAIT_OWNER / STAGE2_NOT_STARTED / MANUAL / AI_EXECUTION_HOLD.**

바울 대표가 기존717a 실패로그 조사와 원인확인 후 최소수정·오프라인검증을 지시했습니다. **추후 별도승인된 실제추론1단계가6조건 모두VERIFIED이면 별도지시없이 즉시2단계 오프라인연결 구현·검증을 시작**하는 조건부승인도 받았습니다. 현재1단계는실패이므로 조건false, 2단계미착수입니다.

## 현재까지 확정·미확정

기존summary에서CLI exit1, error10, turn.failed1, forbidden item3, 완료turn0·최종응답없음을확인했습니다. 이수치만으로실제error.code/message나item.type을복원할수없습니다. 보호프로필은현재agent 비관리자Session9에서접근거부이며서비스계정암호를보유하지않습니다. **이전실패의직접원인은아직미확정**입니다. 기존소비marker를보존하며새모델·usage·시험permit을생성하지않았습니다.

실제자료가필요한부분을추정으로수정하지않았습니다. 검증기/Sandbox/승인게이트변경0. 보호로그를다른계정으로읽거나ACL변경·관리자권한만으로복호화·인증복사를시도하지않았습니다.

## 새 로그 읽기 도구

`review-existing-inference-log.ps1`은기존성공한관리자→서비스Password/Limited·Session0/logon4 임시작업방식만재사용합니다. 새전용 LogReviews 폴더와출력권한만만들며기존계정/로그ACL은유지합니다. 서비스SID/세션·기존boot승인SHA·실행스크립트/프로젝터/metadata/20초Job SHA를검사하고,Python 프로젝터에최대14초예산을줍니다. 모델시험용90초runner는호출하지않습니다.

`project-existing-inference-log.py`에는 subprocess/socket/http/SQLite/CLI 실행경로가없습니다. ctypes의허용Windows DLL로자기Token SID와자기DPAPI만확인합니다. **보호로그를열기전에 실제Token SID가기존service_sid와같은지확인**합니다. 대상은consumed request717a의고정로그폴더하나뿐입니다.

- 자기계정DPAPI JSONL을메모리에서복호화하고기존execution.json의원본SHA와대조합니다. 원문/암호문/환경값을공개출력에복사하지않습니다. 서비스인증파일은열지않습니다.
- error이벤트10개의순번/정수또는허용된오류code/진단어휘만남긴message/HTTP·OS code/고정분류신호, turn.failed, item.completed3개의실제enum형식type과turn.started이전여부를추출합니다. 로그내용이예상과다르면기존summary의event count대조에서HOLD합니다.
- stderr는제한된진단어휘로다시정제합니다. URL·경로·이메일·Bearer/키·임의문자열/긴식별자는삭제합니다. 실제code가확인되지않으면unknown/null로유지하고추정하지않습니다. 출력최대64KiB, source읽기도유한상한을둡니다.
- 실제실행metadata의read-only요청/도구차단/공식provider·구독인증요청/CLI SHA·exit/원격응답증거만선택합니다. argv의개인경로·환경값은내보내지않습니다. argv만으로Sandbox실제강제나exec인증성공을증명한다고쓰지않습니다.
- 원본5개파일의전후SHA를비교해읽기외변경0을확인합니다. 보호원본은서비스프로필에남고새출력은정제된 projection.json 하나뿐입니다.
- 부모는해당새진단Task만정리하고기존부팅작업/장부/MANUAL·승인·717a추론소비marker 보존을확인합니다. 신규AI permit/모델slot은만들지않습니다. 진단review GUID는모델승인이아닙니다.

## 오프라인 검증

**13 PASS / 0 FAIL**. 합성error10개전수보존/정수code·허용문자code/turn.failed/실제item enum·순서/원격성공오판금지/경로·이메일·URL·토큰삭제/환경값·command미출력/과다출력상한/중복JSON거부/네트워크·CLI·DB호출경로없음/자기토큰조회/합성DPAPI메모리roundtrip을확인했습니다. 실제서비스파일·인증파일은테스트입력으로사용하지않았습니다.

PowerShell구문·자동변수/switch충돌0·비관리자가드PASS. 가드는 `HOLD_ADMIN_REQUIRED_EXISTING_LOGS_ONLY_NO_MODEL_NO_USAGE`에서정지했습니다. 기존44개/22+12/Job의기존PASS전체는재실행하지않았습니다. 실제서비스계정의로그읽기성공은아직검증하지않았습니다.

## 바울 대표의 관리자 직접 실행 명령

현재agent는비관리자이며서비스계정의로컬암호입력이필요하므로대표직접실행을기다립니다. **추가AI승인을다시요청하는것이아니라 이미승인한기존로그읽기를서비스계정으로수행하는실행절차**입니다. 암호를채팅/Discord에보내지말고로컬보안입력에만입력합니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\review-existing-inference-log.ps1"
```

이명령은 **새모델0 / 공식usage0 / Discord0 / SQLite업무0**입니다. 실행후출력의 `projection_path`와summary를근거로agent가정제결과만읽을수있습니다. 출력위치는 `C:/ProgramData/LimitlessRelay/InferenceLogReviews/<review-id>/Output/`입니다. private원문을복사하거나보호로그ACL을풀필요가없습니다. 도구실행이막히면정책을완화하지않고HOLD합니다.

실제projection을받은뒤오류10개와turn.failed·item3종을확인해직접원인/미확정부분을분리하고최소수정안을작성합니다. 원인이확정되기전에추론실행기를바꾸지않습니다. 실제추론재시험은기존소비marker와분리한**별도신규범위·승인**이있어야합니다.

## 지속되는 조건부2단계 승인 — 후임도 적용

추후별도승인된실제1단계시험이아래6개를모두만족하는증거를확인하면, **같은작업에서추가지시를묻지않고2단계의오프라인연결구현·검증을즉시시작**합니다.

1. 실제종료코드0.
2. 정상추론turn완료.
3. 실제최종응답존재.
4. JSONL검증PASS.
5. 승인된읽기전용범위준수.
6. 기존파일·권한·보안정책보호.

2단계범위는 Discord모바일명령 → 승인업무큐 → Codex실행기 → Discord결과보고의 **오프라인연결구현·가짜입출력검증**입니다. 실제추가모델호출·실제봇변경/재시작·A활성화·실전업무슬롯소비·추가유료API·Unity/Save/게임변경·Push/main병합·PC종료/재부팅은하지않습니다. MANUAL을유지합니다. 이승인은새모델시험승인이아닙니다. 실제성공결과를확인하는후속작업의자동진행규칙이며새상시감시/스케줄러를운영에설치하는것도아닙니다.

**현재조건false. 새projection은실패원인자료일뿐1단계VERIFIED가아니므로그것만으로2단계를시작하지않습니다.**

## 파일·기록

| 새파일 (Tools/DiscordAIServiceDiagnostics) | 변경전 | SHA256 |
| --- | --- | --- |
| `project-existing-inference-log.py` | 신규 | `0a11e451d023b31197535bbaddcb1dc33db171133af22e910b781bf586454be9` |
| `review-existing-inference-log.ps1` | 신규 | `bfed719bc6c0b7ea8acd46249ea77bf6a3793ef1607e0712734e94d89ca2dcf2` |
| `test_existing_log_projection.py` | 신규 | `42fd354fcc09392ec4ba4deb2a358ec0efebea27c240d840b37215914ee25f8c` |

브랜치 `codex/discord-usage-diagnostics-20261026`, 기반 `9b46a03ffd01b8b0d01fdc4a0e271f5b56c9a25f`. 최종로컬commit은Issue66/최종보고참조. `Evidence/ExistingInferenceLogReview20261026/Preparation.json`·CURRENT_STATUS/CODEX_LATEST 최신화. **LOCAL_COMMITTED / REMOTE_NOT_PUSHED / LOG_REVIEW_WAIT_OWNER**.

이번실제모델·usage·운영배포·기존계정ACL/인증/봇·아트·OnBoot/SQLite/게임변경0. 원인조사는서비스직접실행결과대기이며,최소수정/실제재시험/2단계미착수입니다.
