# LIMITLESS Discord AI 협업 — 운영 정본·인계·장애 보류 기록 (2026-10-24)

> **2026-10-26 진단 기록 후보 구현 및 22 PASS (Codex 격리, 미배포):** [Issue #66 Codex 결과](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6404667041)에 따르면 `collab/usage_bridge.py`, 새 `collab/usage_diagnostics.py`, `collab/service_ai.py`, `collab/service_batch_once.py`, `Tools/DiscordAIServiceDiagnostics/service-final-once.py` 등에 마스킹된 RPC method/id/error.code/message·허용 data·stderr·종료 코드의 **서비스 계정 전용 보호 로그 기록**을 추가했다. 입력 길이·stderr·응답 이벤트 수·로그 파일 수 상한, 링크/타 프로필 차단, fail-closed 정책 유지. 가짜 App Server만으로 **22 PASS/0 FAIL**, 모델/실사용량 조회/운영 수정 0회. 브랜치 `codex/discord-usage-diagnostics-20261026`, 로컬 commit `970a724775b5dfdffd407412fdd3cfc8a130a52a`, `LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_DEPLOYED`. 신규 모듈로 후보 파일 수가 42→43개라 기존 승인 상한/고정 Manifest·영향 SHA 검토 및 **별도 운영 반영 승인**이 필요하다. 직접 서버 `Denied` 원인은 새 진단 코드 **배포 후 실제 사용량-only 조회하기 전까지 미확정**이다. 다음 권장 순서는 (1) 영향 경로·SHA·되돌리기/새 usage-only 실행기 오프라인 검토, (2) 명시적 운영 배포 승인, (3) 별도 명시적 **사용량 실조회 최대 1회·20초·재시도0·실제 모델0** 승인. 기존 모델 실행 runner로 사용량-only 테스트 금지. 기존 봇/아트 감시와 `MANUAL` 보존, Session0 실제 추론 `NOT_VERIFIED`.

> **2026-10-26 오프라인 진단 후속 (Issue #66):** `service-final-once.py:129~130` → `OfficialUsage.read()` → `collab/usage_bridge.py:64`에서 App Server JSON 응답의 `error`를 자체 `Denied('OFFICIAL_READ_REJECTED')`로 축약해 RPC method/id와 원시 `error.code/message/data`를 잃는 문제가 확인됐다. 진단 로깅 결함은 확정이나 **서버가 오류를 반환한 실질 원인과 과거 `turn.failed` 원인은 여전히 미확정**이다. 모의 데이터 3건으로 정보 유실 재현, 모델 호출/실사용량 조회 0회. stderr DEVNULL 및 사용량 사전 게이트의 로그 보존 누락이 확인돼, 다음 단계는 **보안 마스킹된 진단 보존 최소 수정과 오프라인 테스트**를 제안한다. 서비스 계정 보호 로그는 조사 계정 권한 부족으로 미열람, 임의 ACL 변경 금지. 최근 작업 스케줄러 실시간 상태 조회도 실패하여 **과거 부팅 VERIFIED와 현 시점 RUNNING 확인을 구분**한다. 운영 설정/모드/실제 추론은 변경 금지 상태. Codex 구현 로컬 commit `fcd0488ccc8a140cd08661822383f7ec3194e058`은 `LOCAL_COMMITTED / REMOTE_NOT_PUSHED`. [Issue #66 진단 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6404484227).

> **2026-10-26 재개 결정:** 바울 대표가 “디스코드 작업 계속 하자”라고 명시했습니다. **Issue #66 조사 재개는 승인**, 단 기존 `MANUAL`·정식 AI 호출/자동재개 `HOLD`는 유지합니다. 지금 허용된 범위는 마지막 `OFFICIAL_USAGE_AND_APPROVAL → OFFICIAL_READ_REJECTED (Denied)`의 로컬 코드·원시 로그를 이용한 **모델 호출 0회의 오프라인 근본원인 조사**입니다. 이 상태 변경은 운영 서비스 재시작·실제 추론 1회·A 모드 활성화·운영 파일 교체·Git Push·main 병합의 승인이 아닙니다. 원인 확인 후 대표에게 다음 단계 승인 요청 없이 자동 실행하지 않습니다. 이전 2026-10-24 ‘오늘 중단’ 기록은 역사로 보존합니다.

- **문서 상태:** `DOCUMENTED / CURRENT_OPERATION_MANUAL / SESSION0_AI_HOLD / PIN_FREE_BOT_VERIFIED / NOT_MAIN_MERGED`
- **근거 범위:** 2026-10-23~24 바울 대표가 전달한 Codex 로컬 보고 화면과 Discord 실제 조작 결과. 기획 담당은 F: 로컬 실행 파일·로그를 직접 열어 재검증하지 않았다. 화면에 없는 성공 결과를 추정하지 않는다.
- **담당:** 총괄 기획 / Discord AI 협업 구현은 Codex 담당
- **저장소:** `HappyPaul812/Project-Limitless`; 본 정본 기록 브랜치 `docs/planning`. Codex 구현 브랜치 `codex/discord-ai-collaboration`의 로컬 커밋은 이 기획 문서와 별개이고 **원격 Push 여부를 확인하지 않은 상태**로 취급한다.
- **핵심 결정:** 오늘부터 이 무인 AI 추론 장애 작업은 **HOLD**. Codex 5시간 사용량을 다른 실제 게임 개발 작업에 우선 배정한다. 추가 진단·추론 재시도·서비스 재시작을 자동으로 지시하지 않는다.

## 1. 운영 목적과 역할 경계

바울 대표가 **외부 휴대전화 Discord**로 업무 요청·상태 조회·승인 여부를 확인하고, 집 PC는 **Windows PIN 입력 없이 잠금화면에서도** 봇/아트 감시를 수행하도록 구성한다. 최종적으로 승인된 AI 업무까지 자동 처리하는 것이 목표지만, 현재는 **PIN 없는 실제 AI 추론은 검증 실패/보류** 상태다.

기존 ChatGPT 대화 세션 4개가 직접 상호 호출되는 것이 아니다. 외부 `PlanningAgent`, `CodexAgent`, `NewArtAgent`, `RenewalArtAgent`가 Codex CLI 기반으로 역할 문서를 읽고 Discord 중계·SQLite 작업 큐로 소통하는 **별도 에이전트**다. 각 에이전트의 읽기 전용 VERIFIED 기록은 기존 파일럿 근거로 유지한다. 실제 이미지 생성·Unity 적용 기능이 자동으로 생기는 것은 아니다.

## 2. 구현·검증된 사항 — 범위와 증거를 구분

| 영역 | 마지막으로 확인된 상태 | 증거의 성격 / 한계 |
| --- | --- | --- |
| Discord 봇 설치·연결·Slash Command | `VERIFIED` | 바울 대표의 실제 Discord 화면에서 봇 온라인 및 `/limitless status`, `/limitless pending` 응답 확인 |
| 4개 에이전트 개별 실제 추론 | `VERIFIED (기존 파일럿)` | Codex 보고와 Discord REAL 표시, **일반 실행환경** 기준 |
| 기획→Codex 2역할 자동 인계 | `VERIFIED (기존 파일럿)` | 중간 수동 복사 없이 실제 2회 추론·기획 업무 기록 연결 (보고 기준 7 PASS) |
| 기획→신규/리뉴얼 아트→Codex→기획 4역할 자동 인계 | `VERIFIED (기존 파일럿)` | 실제 추론 4회, 9 PASS, Discord 기록 5건 보고. **Session 0 무인 실행 성공 증거가 아님** |
| Discord 일반 PNG 첨부 수령 | `VERIFIED / TEST_ONLY` | 대표 계정 일반 첨부 SHA 일치, 격리 수령 및 Codex 검수 대기. 정식 미술 승인/Unity 적용 아님 |
| 부팅 시 봇·아트 감시 자동 시작 | `VERIFIED (대표 실사용 보고)` | OnBoot 2/2 등록·배치 실행 검증, 대표가 PIN 입력 없이 봇이 작동함을 확인 |
| 운영 SQLite·Worker 오프라인 연결 | `OFFLINE_PASS` | 31 PASS 보고; 운영 자동 추론 성공 증거는 아님 |
| A — AUTO_RESUME | `IMPLEMENTED_PARTIAL / NOT_VERIFIED` | 예약·보류·체크포인트·복구 및 명령 동작은 부분/모의 검증. 실제 한도 회복 후 자동 AI 실행 미검증 |
| B — SAFE_SHUTDOWN | `IMPLEMENTED_PARTIAL / NOT_EXECUTED` | 마감·동기화·안전 종료 조건 모의 검증. 실제 원격 Git 통합·PC 종료 실행 없음 |
| 서비스 계정 공식 Codex CLI·인증 | `PARTIAL_PASS` | 구독 로그인, 사용량 조회 성공 사례, 공식 전체 패키지 무결성, Session 0 `--version`·`--help` PASS |
| **PIN 없는 서비스 계정의 실제 AI 추론** | **`HOLD / NOT_VERIFIED`** | 이전 `turn.failed`의 원인 미확정. 마지막 시험은 사용량 사전 확인 `Denied`로 **AI 호출 전에 중단** |

**운영 모드의 마지막 명시 상태는 `MANUAL`이다.** 부팅 시 봇이 실행되어도 새 AI 업무를 무제한 자동 수행한다는 뜻이 아니다.

### 아트 수령 운영

- 격리 납품함: `Temp/ArtDeliveryInbox` (실제 Windows F: 경로는 운영 로컬 설정을 확인할 것).
- SHA256·Manifest·확장자/실제 파일 타입·중복·ZIP 경로 이탈 및 위험 파일을 검사한다.
- `TEST_ONLY` 결과와 실제 납품·승인 대기 업무를 혼동하지 않는다. 기술 PASS와 대표 미술 승인, Unity 본편 반영은 서로 다른 상태다.
- ChatGPT 아트 세션의 원화·ZIP이 외부 에이전트로 **자동 전달된다고 가정하지 않는다**. Discord 첨부 또는 명시적인 공유 납품 경로가 필요하다.
- 신규 아트 #59 및 리뉴얼 독침벌 #63의 `KEEP/HOLD`, 최종 미술 승인 대기, 권리 확인 미결은 별도 정본·Issue를 우선한다.

## 3. 무인 AI 실행 장애: 시간순 진단과 마지막 차단점

1. 첫 Session 0 추론에서 CLI가 종료 코드 1 및 `turn.failed`를 반환했고, stderr·JSONL 원문 보존 부족으로 직접 원인을 확정하지 못했다. 출력 유형 검사 `FORBIDDEN_ITEM` 등은 **협업 검증기의 분류**로 실제 위험 행위의 증거는 아니다.
2. 서비스 계정 비대화형 Windows 배치 작업은 초기 `0x8007052E / 1326`(사용자 이름/암호 거부)로 실패했으나 이후 정상 자격 증명·배치 유형 4·세션 0·종료 코드 0이 검증되었다.
3. 서비스 계정 공식 ChatGPT 구독 로그인과 사용량 조회가 성공한 사례가 있다. 이는 실제 모델 추론 허용을 보증하지 않는다.
4. 서비스 계정에 Codex 보조 실행 파일 3개(`codex-code-mode-host.exe`, `codex-command-runner.exe`, `codex-windows-sandbox-setup.exe`)가 빠져 있었고, 단일 `codex.exe`가 공식 전체 배포와 달랐다. 이 누락이 과거 실패의 **확정 원인은 아니다**.
5. 공식 Windows `0.164.0-alpha.11.1` 전체 패키지 45개 파일/SHA 및 서명, 서비스 계정에서 `--version`, `--help` 종료 코드 0 확인 후 `boot-approval.json`의 `service_codex.path`·`service_codex.sha256` 참조 전환을 로컬 PASS로 보고했다. 기존 봇·아트 감시·부팅 작업은 보호됐다.
6. **가장 최근 승인된 실제 시험:** 사전 게이트 `OFFICIAL_USAGE_AND_APPROVAL`에서 `OFFICIAL_READ_REJECTED (Denied)`. 서비스 계정·Session 0·배치 계정, 승인 CLI SHA 및 구독 인증은 확인됐지만 **실제 AI 모델 호출 0회, Discord 결과 게시 없음, MANUAL 유지**. 사용량 조회가 Denied된 직접 원인은 현존 보고만으로 미확정이다.
7. 최신 사용자 지시: **Discord 무인 추론 후속은 오늘 중단**하고 Codex 사용량을 다른 게임 작업에 배정. 추가 모델 호출과 복구 실험을 수행하지 않는다.

### 증거·커밋 기록의 주의

- 본 대화의 다수 로컬 Codex 보고는 `codex/discord-ai-collaboration` 브랜치의 단축 커밋만 제시되었으며 **GitHub 원격 반영은 확인되지 않거나 Push 없음으로 보고**되었다.
- 보고마다 원본 보호 검증 개수(7,356, 8,774 등)가 달라진다. **검증 범위·스냅샷이 다르므로 동일 집합의 수치라고 합산하거나 상호 대체하지 않는다.**
- 최신 제시된 운영 참조 전환 결과의 기록 커밋은 `75117899`(보고된 단축 SHA)이다. 실제 원격 해당 객체 존재는 확인하지 못했다.
- 상기의 마지막 `Denied` 보고는 바울 대표가 전달한 화면 기준이며 해당 건의 새 GitHub commit SHA는 현재 문서 작성 시 확인하지 않았다. 채팅 화면만으로 원격 코드까지 동기화됐다고 판단하지 않는다.

## 4. A/B 모드 및 승인 정책

**A / AUTO_RESUME**: 정식 확인 가능한 5시간·주간 사용량 한도 중 하나라도 부족하거나 조회가 거부되면 보류. SQLite에 미완료·권한·기준 SHA·실행 횟수를 저장하고, 회복 확인 후 **사전 승인된 범위만** 재개. 확인 불가면 추측하여 호출하지 않는다. 대표 지시에 따라 **일요일에도 AI 사전 승인 업무는 허용**하나 대표의 휴식은 유지한다. 자동 재개·기동은 아직 실제 검증 미완료.

**B / SAFE_SHUTDOWN**: 대표가 지정한 시각에 신규 접수 중단, 승인 작업 정리, GitHub/로컬 상태 검증, Discord 보고 후 **모든 안전 게이트**를 충족할 때만 PC 종료. 미저장 프로그램을 확실히 확인할 수 없거나 동기화·승인이 불명확하면 HOLD. 실제 종료 및 실제 원격 병합·Push는 미검증/미승인.

**변경 불가 안전 원칙**
- `main` 직접 커밋/Push 및 사용자 승인 없는 병합 금지. Codex 로컬 Push는 명시적 사용자 허가 필요.
- 아트: 제작 → Codex 기술/격리 Unity 검수 → **바울 대표 최종 미술 승인** → 해당 범위 정식 적용. 기술 PASS ≠ 미술 승인.
- 추가 유료 API·구독 외 크레딧 사용 자동 전환 금지.
- 사용자 원본·Save·GUID/meta·승인된 PNG/펫/음원과 미커밋 변경 보호.
- 종료/운영 모드 전환/미술 승인 등 고위험 명령은 Discord 대표 ID·명시적 범위·로그·중복 차단을 확인.
- Windows PIN은 제거하지 않으며 자동 로그인으로 문제를 우회하지 않는다.
- 비밀번호·Bot Token·기기 인증 코드·인증 캐시·개인 식별 계정정보·평문 오류 로그를 GitHub/Discord 문서에 기록하지 않는다.

## 5. 재개 Runbook — 대표의 새 지시가 있을 때만

**기본값:** `MANUAL / SESSION0_AI_HOLD`; 봇·아트 감시는 기존 정상 동작을 유지. 먼저 다시 실행하지 말고 작업 당시 실제 로그·가드 코드를 읽는다.

1. **오프라인 원인 조사:** 마지막 `OFFICIAL_READ_REJECTED (Denied)`를 발생시킨 실제 게이트의 반환 코드·호출 환경·API/CLI 오류를 비밀 제거한 로컬 로그로 확인. `OFFICIAL_READ_REJECTED`와 과거 `turn.failed`는 **서로 다른 실패 지점**이다. 코드/환경/권한/사용량 정보에 근거 없이 임의 성공 판정·보안 완화·무한 재시도 금지.
2. **참조/보호 감사:** 현재 `boot-approval.json` 실제 승인 SHA·CLI 전체 구성·운영 SQLite·OnBoot 두 작업·기존 `CODEX_HOME`과 수정 전 백업을 읽기 전용 확인. 기존 로컬 미커밋 사용자 변경을 건드리지 않는다.
3. **로그 보존:** 에러 이벤트·`turn.failed`·최종 응답 유무·stderr/JSONL 수집이 다음 실제 시험에서 가능하도록 보안 마스킹과 파일 ACL 확인. 사용자 로컬에만 민감자료 보존.
4. **사용자 별도 승인:** 정확한 원인이 좁혀지고 정식 사용량·구독 인증을 확인하면, 바울 대표가 읽기 전용 실제 모델 **최대 1회, 90초, 자동 재시도 0회**를 별도 승인하는 경우에만 실행.
5. **성공 기준:** 서비스 계정 Session 0 추론 완결, JSONL/최종 응답/검증 PASS, Discord `[REAL AI / VERIFIED]` 게시, 제품 변경 0, 호출량·요금 확인. 단발 시험 성공도 A 모드 전체 운영 성공과 별개다.
6. **A 활성화:** 단발 성공 후 실제 Discord→승인 SQLite→실행→Discord 보고→사용량 소진/회복 게이트를 별도 검증하고 **바울 대표의 A 모드 명시적 전환 승인** 필요.
7. **지속 블로커:** 서비스 계정 비대화형 추론이 계속 불가능하면 로그인 후 Worker는 예비안으로만 유지하고, 별도 공식 지원 실행환경을 비용·보안·실사용 기준으로 검토한다. 가족에게 PIN 입력을 요구하는 운영은 대표의 목표를 충족하지 못한다.

## 6. 운영 인계 및 다른 담당 전달

- **기획:** 이번 HOLD를 유지하고, Codex 잔여 사용량은 Chapter3/아트/TTS 등 게임 작업에 배정하도록 권고. 미술·코드 자동 승인을 제안하지 않는다.
- **Codex:** 다음부터 사용자 새 지시가 없으면 위 무인 추론 장애 재시험을 시작하지 않는다. 로컬 브랜치의 실제 상태/증거와 GitHub 원격 반영을 구분하여 보고. 해결되면 CODEX_LATEST 및 관련 Issue에 정확한 증거로 갱신.
- **아트 담당:** Discord 자동 첨부 수령은 격리 단계. `TEST_ONLY`를 납품 승인으로 취급하지 않고 Issue #59·#63의 최신 아트 계약/대표 승인 대기를 유지.
- **후임 기획:** `START_HERE.md` → `COMMON_RULES.md` → `PLANNING_LATEST.md` → 본 문서 순서로 확인. 상태 변경 시 근거/일시/실제 원격 커밋과 운영 적용 여부를 함께 기록.

### 다음 작업 재개 시 사용할 간단 지시

> [LIMITLESS / Discord 무인 AI 실행 / HOLD 해제 전 읽기전용 조사] 기존 봇·아트 감시와 MANUAL을 보존하고, 마지막 `OFFICIAL_USAGE_AND_APPROVAL`의 `OFFICIAL_READ_REJECTED (Denied)`가 발생한 실제 코드·로컬 로그만 조사하세요. 과거 `turn.failed`와 구분하고 추가 AI 호출·서비스 재시작·SHA 게이트 완화·원격 Push/병합·원본 Unity 변경 없이 원인·증거·다음 최소 시험 조건을 보고한 뒤 중지하세요. 대표 별도 승인 전 시험은 하지 마세요.

## 7. 외부 자료/구현 위치

- 저장소: `HappyPaul812/Project-Limitless`
- 로컬 Codex 격리 worktree(대표가 이전 PowerShell로 확인): `Temp/DiscordAICollab20261023/Worktree` (**사용자 PC의 현행 상태를 이 대화에서 직접 재확인하지 않음**).
- 핵심 구현 디렉터리: `Tools/DiscordAICollab/`; 이 작업브랜치 로컬 작업물은 미Push일 수 있어 `docs/planning`에서 바로 열리지 않을 수 있음.
- 프로젝트 규칙: `AGENTS.md`, `문서/00_프로젝트/팀운영/COMMON_RULES.md`, `ALL_TEAMS_GITHUB_REPORTING_POLICY_20261023.md`.
- 관련 아트 작업: GitHub Issue #59(신규 아트), #63(리뉴얼 독침벌); 이 문서가 그 이슈들의 상태를 변경하지 않음.

**변경 기록:** 2026-10-24 기획 문서 최초 작성. 실제 GitHub 커밋과 인계 연결 여부는 쓰기 성공 결과를 우선한다.
