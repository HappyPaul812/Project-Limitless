# LIMITLESS Discord AI 협업 — 운영 정본·인계·장애 보류 기록 (2026-10-24)

> **2026-10-26 설치 Preflight HOLD — 작업 등록 SDDL 설정의 시점/검증 누락 (미설치):** Codex [Issue #66 사전 점검](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6416602437) 기준 후보 Manifest `9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec`, 설치기 `337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829`, 대상 **855개 SHA 모두 PASS**. 그러나 기존 `install-session0-trial.ps1` 112행 `Register-ScheduledTask`로 먼저 작업을 등록하고 114행 `SetSecurityDescriptor`로 사후 ACL 적용하는 구조이며, 실제 task descriptor readback 검증도 없다. 잠시라도 의도보다 넓은 권한이 부여될 수 있으므로 **미설치·HOLD**(실제 PC에서 악용/권한 노출이 발생했다는 증거는 아님). **BOT 시험계정0 / Scheduled Task0 / FakeWorker0 / 실제 모델·usage·Discord0, PIN·기존 2개 OnBoot·운영 DB·아트·코드/CLI·게임/Save·main 모두 불변.** 초기 29 PASS의 범위에는 등록 시 DACL 적용/OS readback이 포함되지 않았음을 정확히 정정. 기존 installer/Manifest/855바이트 변경 금지, 기존 설치 명령은 실행하지 말 것. **다음은 Codex 격리 브랜치에서만** TaskScheduler COM `RegisterTaskDefinition`의 한 호출에 검증된 SDDL을 전달(기존 작업 overwrite/update/trigger 금지), owner·group·DACL·허용 ACE를 `GetSecurityDescriptor`로 직후 대조하고 예상 외 권한·확인불가 시 fail-closed. 관리자·SYSTEM/Worker의 최소 권한 설계, 관리자 계정과 Bot 계정 구별을 보존하며 `DONT_ADD_PRINCIPAL_ACE` 및 CREATE·IGNORE_TRIGGERS flags는 실제 COM 제약/등록 방식에 맞는지 오프라인 검증. 가짜 scheduler에서 SDDL 동시 전달/중복 이름/권한 불일치/호출 실패·불명 결과 재시도0 테스트 및 **변경 설치기·Manifest/Bundle의 새 SHA**와 855파일 영향 차이를 보고한 뒤 중지. 이후 **새 후보에 대한 대표의 별도 설치 승인**이 있어야 실제 계정·작업 생성 및 계정 간 Fake-only IPC 시험 가능. 현재 브랜치 `codex/discord-usage-diagnostics-20261026`, 로컬 문서 commit `5d259e2738768dd0816540b9a31228049db70a56`, `REMOTE_NOT_PUSHED`. 운영 MANUAL/A HOLD 유지.

> **2026-10-26 Host↔고정 Worker 접합 완료, 29 PASS·설치 전 HOLD:** [Codex Issue #66 최신](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6415686545) 기준, 버튼5→LiveSession0Bridge→Windows 인증 pipe→C# Session0Host→BrokerJob90→고정 read-only Worker→엄격 JSONL→정제 결과 경로 코드 조립이 완료됐다. **신규 오프라인29 PASS/0 FAIL**; 과거 성능시험·Phase1/2/3 결과 반복0. 실제 BOT/ADMIN/WORKER 서로 다른 SID의 Windows IPC 성공·작업/계정 설치·실전 Discord→AI 호출은 **아직 0·미검증**. 기존 CLI/기존 계정·인증/승인 파일·OnBoot2·운영 봇/아트·SQLite·게임/Save·main 보존, `MANUAL / AI_EXECUTION_HOLD`. 배포 후보 `Temp/Session0HostBundle20261026Ready`, Host Manifest SHA `9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec`, installer SHA `337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829`; 로컬 branch `codex/discord-usage-diagnostics-20261026`, 구현 commit `5f1dad2284d7c2cf78c4d80d451555c3619eb60a`, docs `a8b0be3971aa461d09432348b309179cade27981` (미Push). **다음 분리 승인 대상:** 별도 최소권한 시험 Bot 계정 `PAUL-PC\\LIMITLESSBotTrial` 생성(Users만, 기존 admin/worker와 별도 SID, 24h 만료) 및 관리자 소유 고정 코드 원자 배치, 신규 시험용 스케줄 작업 `LIMITLESS-Trial-S0Host` 기본 Disabled/trigger0/FAKE_ONLY만 준비. 그 후에만 별도 승인 또는 명시된 승인 범위에 따라 **계정 간 fake-only IPC/Job 종료** 검증. 이미 저장된 기존 일반 사용자 DPAPI Bot Token/서비스 인증은 복사·이전 금지(신규 사용자에서 시험앱 토큰 안전 재입력 별도 단계). 설치 시 CLI가 사용자 쓰기 가능 등 신뢰 경계 하나라도 실패하면 HOLD·원상복구. **실제 AI/official usage/Discord 명령 등록/A 모드/부팅 자동 시작은 별도 승인 전 금지.** 보고한 '29PASS'를 실제 무인 운영 성공으로 처리하지 않는다.

> **2026-10-26 Session0 보안 중개기 42 PASS / 실제 계정·Host 결합 SECURITY HOLD:** [Issue #66 Codex 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6412596448) 기준 `secure_session0_broker.py`, `Session0PipeBoundary.cs`, `session0_pipe_transport.py`, `BrokerJob90.cs` 등으로 인증된 파이프 후보·일회성 소비·고정요청·취소/결과검증·프로세스트리 cleanup 구성요소를 격리 구현, 정책·mock 전달34 + native8 = **42 PASS/0 FAIL**. 그러나 **실제 Host ↔ Worker backend 최종 조립과 서로 다른 Windows SID 간 IPC 실증 미완료**, 실제 운영/모델/usage/Discord/서비스 설치 호출은0. 현재 Discord 봇 일반 사용자와 관리자 PowerShell이 동일 SID이므로 UAC elevation·개별 writable 파일 ACL만으로 안전한 사용자 경계가 아니다. 설계상 BOT/ADMIN/WORKER 각각 독립 SID가 전제이며 **별도 최소 권한 시험 봇 계정, 안전한 토큰 저장소·서비스 Host 설치·전후 권한·부팅 계획은 별도 사용자 승인 대상**. 이전 LIMITLESS-test 일반 사용자 CurrentUser DPAPI credential을 새 계정에 복사/재암호화하여 우회하지 말 것; 계정별 새 보안 입력 필요. 우선 **고정 Worker 연동 코드 완성과 합성 fake-only 전달·위협모델 검토**를 격리에서 진행할 수 있으며, 실제 로컬 계정/ACL/서비스/OnBoot 변경 및 실제 Discord→모델 E2E/A 활성화는 분리 승인 전 HOLD. 기존 `MANUAL / AI_EXECUTION_HOLD`, 봇·아트/OnBoot2/운영 SQLite/기존 인증/승인 SHA/게임·Save·main 보호. Codex 로컬 구현 `82af13e15469ae23d8ca13e96ac6b651f3160a9f`, 문서 `ef1dec16bf285024c9caa733845ff93985e07d7f` (`codex/discord-usage-diagnostics-20261026`, `REMOTE_NOT_PUSHED / NOT_DEPLOYED`).

> **2026-10-26 모바일 버튼+Worker 연결 후보 오프라인 35 PASS / 실전 HOLD:** [Codex Issue #66 결과](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6411977426) 기준, `mobile_planning_buttons.py`의 대표 전용 ephemeral 버튼5개(업무 내용 보기/실행 승인/실행/취소/진행 상황 확인)와 `session0_planning_bridge.py`의 합성 검증 연결이 **35 PASS/0 FAIL**. 기존 전체SHA256·owner/guild/channel/message·role/scope/base/upstream·TTL≤300초·단발 승인·소비/중복·HOLD 검증 유지, 승인≠실행, 미술 `/limitless approve` 불변. **LiveSession0Bridge는 현재 무조건 HOLD**해 실제 프로세스를 시작하지 않는다. 관리자 선행 arm/고정 서비스 Task 전달, 관리자·서비스 계정이 통제하는 mailbox 및 결과 판독기, 준비~전달~Job~정리 총90초 supervisor, 실행 중 취소/HOLD와 전후 SHA/rollback 등이 미완성. 일반 봇 사용자와 관리자 Windows SID가 같아 일반 사용자 FullControl 파일 ACL만으로 보안 경계를 성립시키지 않는다. 실제 모델0·usage0·Discord 접속/명령등록0·운영 SQLite0·서비스 임시Task0, 운영모드 MANUAL/A HOLD, 기존 운영 봇·아트/OnBoot/인증/CLI/게임/Save/Main 무변경. 구현 로컬 commit `a61d8c17a8414335830939d7c1ec159ab1d8a023`, 완료 문서 로컬 commit `f93d13e79b336453711a69ea5de34be2035b1d99` (`codex/discord-usage-diagnostics-20261026`, **REMOTE_NOT_PUSHED/NOT_DEPLOYED**). 다음 단계는 **실제 모델·Discord·usage를 호출하지 않는 격리 보안 경계/실행 전달의 오프라인 구현·검증**으로 한정. 이미 성공한 1단계 Session0 단발 추론/2단계 Fake 4역할/3단계 Discord Fake 웹·모바일 상태시험 결과는 별도로 유효하나 이들을 연결한 전체 실제 E2E는 아직 미검증. 대표의 별도 신규 명시 승인 전에 trial 봇 등록/실AI 연결/A 자동재개 금지.

> **2026-10-26 실제 Discord Web 4역할 Fake PASS 및 정상 정리:** [Issue #66 실전 완료](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6411156106). 기존 휴대전화 `/limitless-trial status`·`task`는 실제 Discord 응답2건/기획 NEW 1건 PASS; 이후 **동일 대표 계정의 PC 웹 Discord**에서 미진행 승인→가짜 실행→인계만 새 격리 trial로 수행. Planning→NewArt/RenewalArt→Codex→Planning 반환 기록 **4개 FakeExecutor 실행/승인 소비4 / 신규 실제 전송 SENT11·결과 불명0 / 4역할 TECH_PASS**. 이전 모바일 응답2건과 신규11건을 중복 합산하지 않는다. 실제 AI·공식 사용량 조회·운영 SQLite 업무는 모두0회. 2026-10-26 16:53:56 KST 대표 완료로 Host stop(종료코드0·자식0), 새로 CREATE한 trial guild 명령의 **해당 반환 ID 1개만 DELETE**, 시험 앱의 기존 guild/global 명령 목록 복원 PASS, 시험/소비/receipt/체크포인트 증거 보존. 원본 부팅/운영 Discord bot·아트감시·CLI/CODEX_HOME·계정 인증·SQLite·게임/Save·main 모두 변경0, `MANUAL`/A HOLD 지속. **시험은 휴대전화 status/task + 웹에서 approve/실행/인계 조합**이므로 '휴대전화 전체 흐름 VERIFIED'로 과장 금지. 실제 Discord→실제 Codex Worker 연결·Session0 모바일 무인 수행·주간/5시간 소진 후 A 자동재개는 미검증/미승인. 승인 UX의 64자리 SHA 입력은 모바일에서 불편해 **버튼 또는 짧은 코드** 설계를 다음 개선 대상으로 채택하되, 서버가 불변 전체SHA·owner/guild/channel/역할/scope/TTL/소비1회 검증을 계속 수행해야 하며 후속 역할 무단 자동승인 금지. Codex 격리 브랜치 `codex/discord-usage-diagnostics-20261026` 로컬 구현 `341f6a5168a4e14b89f417de4089b5a59167d88e`, 최종 문서 `243bf0bab0841b545f5ff9cc4f187f2880826b4c` (`LOCAL_COMMITTED/REMOTE_NOT_PUSHED`). 권고: UX 최소 변경·실제 Worker 접합과 fail-closed/명령 ACL/정확 package SHA/롤백을 오프라인 검토하고, 이후 **실제 Discord→읽기전용 AI 1회**는 별도 대표 승인으로 제한 시험. 운영 A·실제 게임 적용/봇 영구배포는 또 별도 승인.

> **2026-10-26 Phase3 Discord 어댑터 오프라인 준비 완료:** [Issue #66 Codex 완료 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6409096093)에 따르면 실제 `discord.py` 2.7.1 SDK callback을 가짜 interaction/FakeExecutor에 연결하는 `collab/mobile_discord_trial.py`가 신규 **18 PASS/0 FAIL**, 패키지 smoke **5/5 PASS**. 별도 `/limitless-trial`의 `status/task/plan/approve-execution/run-fake/handoff/cancel-execution` 7종을 설계했으나 **실제 Discord 명령 등록0·접속0·모바일 왕복0·모델0·공식 사용량조회0·운영 DB0**. 시험 ZIP16파일 SHA `103ecc96c6796d288986eb6ba3ddf5e8612153010b4976f4b9c39a4dcc88099b`, manifest SHA `f69049c61737476e6707bcb9559957e88115fbc19da0a56a92c8ae339348e5a7` 모두 미승인·미배포. 신규 어댑터 SHA `dbe27663a4d47bcbc1c30e8d85cc1c343ec46476b151967d582b2df9824d06de`; `mobile_offline.py` 기존 SHA 불변. Codex 로컬 구현 commit `8cbf90ba84c93f9c45f4979a65a605fda727d464`, 로컬 문서 commit `6fcaafa06ca5b89bde7eeffe55431afa125ddb40` (`REMOTE_NOT_PUSHED`). 운영 `/limitless approve`는 **기존 미술 승인 전용**으로 보호하고 시험 `approve-execution`과 혼동 금지. 권장 차기 단계는 **별도 시험 Discord application/bot/guild 및 좁은 권한**을 로컬/사용자에서 확인하고, 운영 봇을 재시작하거나 기존 명령 집합을 bulk-sync하지 않는 **단일 guild 명령 CREATE·반환 ID로만 제거**하는 가짜 Worker 모바일 E2E. 다음 단계는 별도 범위 승인·로컬 credential 필요. 실모델추론/사용량조회/A모드/운영 SQLite/운영 bot 변경 금지; MANUAL 유지. 

> **2026-10-26 최신 단계 판정: PHASE1_VERIFIED / PHASE2_OFFLINE_PASS / PRODUCTION_NOT_DEPLOYED.** [Issue #66 상세 결과](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6408778370). 1단계: `LIMITLESSRelaySvc` Session0/logon4에서 공식 ChatGPT 구독 Codex CLI 실제 추론 성공(CLI exit0, turn1, 최종응답 정확, JSONL PASS, 오류0), 23.170초, input9934/output22/cached8192; 내부 원격 요청·내부 재시도 횟수 미확정. 서비스 임시 작업/permit 정리, MANUAL/원본 보호. 2단계: `mobile_offline.py`로 Discord→승인 SQLite→4역할 인계→검증→가짜 Discord 결과의 **합성 오프라인 연결 24 PASS/0 FAIL**. 운영 Discord 앱/봇 명령 등록·운영 DB·A 모드/자동 재개·실제 모바일 E2E는 **미실행/미승인**. 기존 `/limitless approve`는 미술 승인 전용이고 신규 `approve-execution`은 가짜 인터페이스로 실제 Discord 미등록. 새 격리 브랜치 `codex/discord-usage-diagnostics-20261026` 구현 local commit `0dffe9738eb8e56fe06af413e19411ab87f72b25`, 문서 local commit `d2db1e47b541e3e578985dc746eb31ec066b318c` (**REMOTE_NOT_PUSHED**). 다음은 기존 봇·아트 안전 보호하에 운영 어댑터/명령 권한·manifest SHA/되돌리기와 **가짜 실행기 기반 실제 Discord 왕복 검증**의 승인 범위를 준비해야 한다. 실모델 추가 호출·운영 A 활성화는 별도 승인 필요.

> **2026-10-26 usage-only 단발 실행 중 PowerShell 변수 충돌 2건 확인/격리수정:** [Codex Issue #66 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6406235162). 이전에 실제 서비스 계정 `PAUL-PC\\LIMITLESSRelaySvc` Session0/logon4 및 44 SHA/기존40 보존 PASS를 확인한 상태에서, 사용량 조회 스크립트가 실행 전 실패했다. 자식 source 45행의 `$home`은 대소문자 비구분 PowerShell 읽기전용 `$HOME`과 충돌하여 `SessionStateUnauthorizedAccessException`; 부모 source 169행의 결과경로 변수 `$child`는 `[switch]$Child` 매개변수와 충돌하여 변환 예외. 격리 후보는 `serviceCodexHome`·`childSummaryPath`로 이름만 바꿨고 신규 오프라인 binding 회귀 **3 PASS/0 FAIL**, 실서비스 재시도0. 이전 실제 배포본/운영 본체는 **미수정**, 로컬 수정 후보 launcher SHA `3a57e70ac09ac2d357d4224ef4cc9c5ad23d7610f499380436eeb2aac80fec74`, 로컬 fix commit `e10d02cbb4fccc9679a2ebd035c8426f3c603557`(`codex/discord-usage-diagnostics-20261026`, `REMOTE_NOT_PUSHED / LOCAL_FIX_NOT_DEPLOYED`). 해당 시험에서 임시작업 launch 1회는 소모되었지만 **사용량 provider/Python/App Server/RPC/모델 0회**; permit 비활성·임시작업 제거, 기존 **launch fuse/소비마커는 보존**. '조회 0회'와 '시험 시작권 소비'는 별개이므로 새 승인 없이 동일 요청 재사용, 소비마커 삭제·초기화, 새 request/permit 발급·실행을 하지 않는다. 현재 `MANUAL / AI_EXECUTION_HOLD`, `OFFICIAL_READ_REJECTED`/과거 `turn.failed`의 실질 원인 여전히 미확정. 다음은 수정 launcher diff/SHA·마커 보호·최종 관리자 실행 경로만 정적 검토하고 **대표 별도 명시 승인 후에만** 새 고유 trial과 실제 usage-only 최대1회/20초 시험을 수행한다.

> **2026-10-26 Session 9 실행 선행조건 HOLD (시험권 미소진):** [Codex Issue #66 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6405623813)와 바울 대표 전달 화면에 따르면 사용량-only 1회가 허가된 뒤에도, Codex 실행 환경이 **비관리자 사용자 Session 9**이고 작업 스케줄러 검사 역시 `CimJobException`으로 실패하여 대상 **서비스 계정 Session 0 / 배치 로그온 유형4** 조건을 검증할 수 없었다. 이에 따라 **격리 배포 NOT_DEPLOYED, 활성 permit/request/시험 프로세스 미생성, 실제 공식 조회 NOT_ATTEMPTED(0), 모델0, Discord0, SQLite 시험업무0**, 기존 `MANUAL`·운영52파일·부팅 설정 보존. SHA44 및 기존40 보호 PASS. 이번 결과는 서버 RPC 거부의 재발이 아니며 `CimJobException`은 App Server 코드가 아니다. **승인된 조회 1회는 소진되지 않았지만 자동 재개 금지.** 다음은 기존 검증된 관리자 PowerShell/서비스 배치 진입 경로에서 실행계정·세션·작업스케줄러 접근과 최소 보호 정책을 확인한 뒤, 조건 충족 시에만 이전 허가 범위에 한정해 별도 격리 시험 배포 및 조회를 재개하는 방향. 관리자 PowerShell을 열었다는 사실만으로 Session0을 확정하지 말 것. 사용자 암호·토큰을 채팅에 올리거나 기존 부팅 작업/ACL/인증을 재설정하지 말 것. Codex 로컬 기록 commit `84486500a79cdf59c41d4204d6a7c3427356422d` (`REMOTE_NOT_PUSHED`), 시험 후보 기준 `0f98620bbd565e0ee8df6b743a6a251b21ae75b6`. 마지막 `OFFICIAL_READ_REJECTED` 실질 원인·과거 `turn.failed` 원인은 여전히 미확정.

> **2026-10-26 usage-only 격리 시험 배포 사전점검 완료 (12 PASS, 미배포):** Codex [Issue #66 최신 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6404994131)와 대표 전달 화면 기준, 기존 `boot-approval.json.runtime_sha256`은 Python42 + `DiscordSecretStore.psm1` 1 = **총43**, 후보는 Python43 + 해당 PS모듈1 = **총44**. 운영/시험 파일 전체를 무조건 재해시하지 않고 의미 변경 기존3(`collab/usage_bridge.py`, `collab/service_ai.py`, `collab/service_batch_once.py`) + 신규1(`collab/usage_diagnostics.py`)만 별도 후보로 준비했고 **나머지40개 승인 항목 SHA는 기존 운영 bytes 그대로 보존**. 26 Python+PSM1 raw SHA가 Git checkout 줄바꿈 때문에 달라지는 위험을 회피했고 운영 파일 미수정. 별도 `usage-only-once.py` 진입점은 기본 비활성, 승인된 보호 ProgramData permit·trial 루트·정확한 file/role/44 SHA·기존 CLI·새 request ID·만료·Session0/logon4가 모두 맞아야 **사용량-only 최대 1회/20초/재시도0/AI 모델0/Discord 게시0/SQLite 업무0**으로 실행되도록 설계. 신규 12 PASS/0 FAIL은 **합성 오프라인 검사**, 기존22건 반복0, 실제 사용량 조회/모델 추론0. 기존 `MANUAL / AI_EXECUTION_HOLD`, OnBoot 2개/봇·아트/게임/Save/CLI 인증/운영 승인 목록 모두 불변. 브랜치 `codex/discord-usage-diagnostics-20261026`, 로컬 commit `0f98620bbd565e0ee8df6b743a6a251b21ae75b6`, `LOCAL_COMMITTED / REMOTE_NOT_PUSHED / NOT_DEPLOYED`. 다음에는 **격리 시험 배포와 사용량 실조회 각각 명시적 승인** 필요하며, 같은 실행에서 모델 추론이나 A 모드 전환 금지. 테스트 목적은 `Denied` 실제 RPC 원인 확보이며 당시 원인은 여전히 미확정.

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
