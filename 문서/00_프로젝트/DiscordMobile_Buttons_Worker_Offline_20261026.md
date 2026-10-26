# Issue #66 모바일 버튼·Session0 Worker 연결 후보 — 2026-10-26

판정: **OFFLINE_CONNECTION_READY / LIVE_DEPLOYMENT_HOLD**. 실제 Discord→AI E2E 성공을 뜻하지 않습니다. 기준 commit `243bf0bab0841b545f5ff9cc4f187f2880826b4c`, 브랜치 `codex/discord-usage-diagnostics-20261026`. 실제 구현 commit은 CURRENT_STATUS 및 Issue #66 완료 댓글에 기록합니다.

## 구현과 보호 경계

`mobile_planning_buttons.py`는 기존 격리 `OfflineMobileRelay`의 Store/권한/트랜잭션·체크포인트를 재사용합니다. 시험 앱에만 연결 가능한 `/limitless-trial task` SDK 그룹 및 ephemeral 버튼 **업무 내용 보기 / 실행 승인 / 실행 / 취소 / 진행 상황 확인**을 만듭니다. 대표는 긴 SHA를 입력하지 않습니다. custom_id에는 임의 panel nonce와 동작만 있고, 서버에는 전체 SHA256·task/role/scope/base/root/upstream·대표/guild/channel/message·절대 만료 300초를 바인딩합니다. SDK timeout의 클릭별 연장과 무관하게 서버 절대 만료를 검사합니다.

SQLite에서 승인과 소비를 각각 처리합니다. 승인만으로 실행하지 않고 실행 시 승인 소비를 먼저 확정합니다. 같은 interaction의 다른 내용은 거부하고, 같은 업무의 새 interaction도 두 번째 실행을 만들지 않습니다. handoff도 CAS로 한 번만 claim합니다. 취소 후 소비를 복원하지 않습니다. 이미 완료한 업무 취소는 검증 결과를 지우지 않습니다. 재시작 시 미완료는 HOLD, 전송 중 결과는 UNKNOWN_NO_RETRY로 남기며 자동 재실행하지 않습니다. 다른 역할의 업무 생성/자동 승인 및 미술 최종 승인 기능은 없습니다. 기존 `/limitless approve` 및 Fake4역할 코드는 수정하지 않았습니다.

`mobile-worker-contract.py`는 불변 ticket SHA, 고정 Planning 역할/응답/정책, 기존 결과에서 안전한 투영만 허용하는 순수 계약입니다. 5시간(300분)·주간(10080분) 창이 모두 있고 신선도30초·ordinary_allowed·100% 미만·미래 회복 시각을 요구합니다. 실행 전 quota가 기록된 시각을 보존하여 정상 90초 응답의 quota가 나중에 30초를 넘었다는 이유만으로 오판하지 않습니다. 실제 quota 조회는 기존 서비스 실행기만 담당하도록 설계합니다.

`session0_planning_bridge.py`의 OfflineSession0Harness/OfflineButtonWorkerConnection은 합성 Session0/logon4 → 기존 strict JSONL 검증 함수 → 고정 한 문장·숫자 usage 투영 → 격리 SQLite → 단발 Discord 전송을 연결합니다. 서비스 SID/계정/세션/logon/비관리자 계약이 다르면 차단합니다. 검사 함수·실행기4파일 SHA도 고정했습니다. 고정 문장 이외의 결과, tool/error/비정상 종료는 기존 검증기로 거부합니다. fake 서비스 결과는 fixture 표식이 없는 workflow로 주입할 수 없으며 plain dict도 거부합니다.

**실제 권한 경계는 아직 구현·검증되지 않았습니다.** `LiveSession0Bridge.submit`은 무조건 HOLD하고 실제 프로세스 실행·Scheduler 등록·인증 접근을 하지 않습니다. `TrustedProjection(fixture=False)`도 발급하지 않습니다. 현재 후보를 실전 실행 가능한 Host/배포 패키지로 해석하면 안 됩니다. 이번에 준비한 것은 버튼 및 서비스 계약의 오프라인 연결 후보입니다.

## 다음 실전 연결에 남은 관리자 경계

1. 대표의 별도 실행 승인 및 새 요청에 대한 관리자 전용 arm/permit. PreparedPlan의 `approved=false`는 permit이 아닙니다. 준비 ID는 `1a2ffc6823db49fd8fe25a86ad72f51b`, 미발급·미소비 상태입니다. 실제 ticket은 실행 버튼을 누를 때 신규 ID로 만들고 정확 SHA와 승인 범위에 다시 바인딩해야 합니다.
2. 관리자 사전 준비 Supervisor가 고정 서비스 작업을 한 번만 시작하도록 구현·오프라인 검증해야 합니다. 서비스 계정 암호는 대표의 로컬 보안 입력만 사용하고 일반 사용자 봇에는 보관하지 않습니다. 기존 성공한 Password/Limited/Session0/logon4 절차와 변경 없는 Child/Native Job을 재사용할 계획입니다. 기존 OnBoot2개는 변경하지 않습니다.
3. 새 전용 ProgramData inbox/arm/outbox에서 bot은 요청 제출과 결과 읽기만, service는 자기 인증/진단만 담당합니다. arm/결과는 Administrators/SYSTEM/service 소유·DACL 검증이 필요합니다. 관리자 PowerShell과 일반 봇이 같은 Windows SID이므로 그 사용자 FullControl 또는 사용자 소유 파일은 보호 경계로 인정하지 않습니다. 기존 ACL은 변경하지 않았습니다. 보호된 결과 reader는 아직 구현하지 않았습니다.
4. ticket 생성부터 전달·준비·quota·추론·정리까지 **전체90초**를 monotonic Supervisor로 제한하고 큐 전달 지연을 차감해야 합니다. 기존 Native75초/Python70초 내부 예산만으로 전체90초를 보장했다고 보고하지 않습니다. 실행 중 취소는 고정 task/Job 트리 종료·active0 확인이 추가 필요합니다. 현재 후보의 취소는 큐·결과 수용을 차단하며 이미 실행된 모델을 되돌린다는 뜻이 아닙니다.
5. 기존 runtime44(불변40)·CLI·service config SHA 검증 후 기존 서비스 자기 CODEX_HOME/keyring/builtin OpenAI로만 실행합니다. 새 봇 모듈을 exact44 collab 폴더에 섞지 않습니다. CLI exec1/usage App Server1/응용 재시도0, 내부 builtin HTTP/stream 기본 재시도 허용, 원격 요청 정확1회 보장 불가 정책을 유지합니다.

따라서 **실전 배포 가능 판정은 HOLD**입니다. 남은 관리자 경계를 구현·검증한 뒤에만 단일 Planning 실전 시험 명령을 확정합니다. 지금 대표에게 실행할 관리자 명령이나 실제 모바일 명령을 요청하지 않습니다.

## 검증

신규 `test_mobile_planning_buttons.py`만 실행합니다. 이전 Phase1 VERIFIED/Phase2 24/Phase3 18+5/실제 웹 Fake4 성공 전체를 재실행하지 않았습니다. 새로운 합성 DB만 만들었고 subprocess.Popen/socket.socket을 테스트 구간에서 차단합니다. SDK import 시 OS 버전 초기화는 해당 차단 전에 수행하며 모델/네트워크 실행이 아닙니다.

대상: 버튼 SDK·권한·메시지 바인딩, 승인/실행 분리, 만료/취소, 중복 클릭/소비/인계, SHA·role·upstream 변조, 5시간·주간·오래된 quota, Session0 계약, 90초 deadline 거부, 금지 tool/최종 응답/exit 검증, fixture→live 거부, 보고 실패 후 재실행 금지, 재시작 복구, 단일 Planning 결과. 최종 수치 및 정제 시험 원문은 `팀운영/Evidence/DiscordMobileButtonsWorker20261026/Tests.txt`에 보존합니다. 합성 deadline 시험은 새 실제 Windows Supervisor의 시간 제한 검증이 아닙니다.

실제 모델0·공식 사용량 실조회0·Discord REST/Gateway/메시지0·서비스 task 등록0·운영 SQLite 업무0·활성 permit 생성0입니다. Issue 댓글 기록은 별도 GitHub 보고이며 Discord 호출 수에 포함하지 않습니다. 서비스 인증/토큰/개인 원시 로그를 읽거나 복제하지 않았습니다.

## SHA와 되돌리기

신규 코드4개는 전 SHA가 없음(null)이며 후 SHA는 `PreparedPlan.json/new_files`에 전체64자리로 기록합니다. 기존 실행기4개 SHA 일치, boot-approval SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f` 일치. 운영 SQLite 동적 상태·봇 건강·OnBoot 실행 상태는 이번에 재조회하지 않았으므로 새 정상 상태 검증으로 주장하지 않습니다. 이번 작업의 해당 변경·재시작·업무 처리0입니다.

운영 배포가 없으므로 현재 운영 복구 작업은 없습니다. 후보를 사용하지 않으면 기존 시스템 그대로입니다. 새 코드/정제 증거/로컬 commit을 보존하고 실전 arm을 생성하지 않으면 됩니다. 다음 별도 시험에서는 새 시험 폴더만 사용하고, 기존 inventory 백업→단일 CREATE 반환 ID 기록→Host 만료/Stop→해당 ID만 DELETE→목록 대조를 유지합니다. 실패해도 자동 재시험/소비 초기화/과거 snapshot 재사용은 하지 않습니다.

## 이후 대표의 휴대전화 시험 순서 — 현재 실행 불가

실전 경계 검증 및 별도 승인 후에만 Limitless-test/ai-시험실에서 `/limitless-trial task`를 선택합니다. 이어 **업무 내용 보기 → 실행 승인 → 실행 → 진행 상황 확인**을 누릅니다. 긴 SHA 입력은 없습니다. 실행 전 중지하려면 **취소**를 누릅니다. 이때 허용할 실제 업무는 PlanningAgent의 다음 한 문장뿐입니다.

> LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.

다른 역할 실제 호출0, A 미활성, MANUAL 유지. 시험 종료 시 신규 접수 차단·임시 작업/permit 비활성·Job/Host active0·반환 명령ID 단일 삭제·소비/진단/체크포인트 보존을 확인합니다. Discord 보고 실패/결과불명은 상태 확인만 하고 실제 추론을 다시 실행하지 않습니다.

공식 근거: [Codex 비대화형 실행](https://learn.chatgpt.com/docs/non-interactive-mode), [인증](https://learn.chatgpt.com/docs/auth), [discord.py interactions](https://discordpy.readthedocs.io/en/stable/interactions/api.html). 현재 공식 문서가 기존 고정 CLI의 실제 서비스 성공을 새로 증명하는 것은 아니며 기존 VERIFIED 증거와 구분합니다.
