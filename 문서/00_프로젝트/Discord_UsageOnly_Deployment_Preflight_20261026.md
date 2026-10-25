# Issue66 운영 배포 사전 점검·독립 usage-only 시험 준비 — 2026-10-26

**PREFLIGHT_READY / NOT_DEPLOYED / NOT_AUTHORIZED / MANUAL**. 기반후보970a724775b5dfdffd407412fdd3cfc8a130a52a. 기존22검사 재실행0, 새로운 독립 진입점 검사만12 PASS/0 FAIL. 실제 모델/공식실조회/Discord게시/진입점SQLite업무/재시도0.

## 정확한 승인 차이 — 42/43은 Python 모듈 수

기존 boot-approval.runtime_sha256은 **Python42 + DiscordSecretStore.psm1 1 = 총43항목**입니다. 기존 PS검증기의42 제한은 Python파일 수입니다. 후보는 Python43이고 기존PSM1을 보존하면 총44항목입니다. PSM1 누락·단순42→43 전체승인은 금지합니다.

검토용 패키지는 운영원본39 Python·PSM1의 **40개 승인 SHA를 그대로** 보존하며 기존3개 의미변경과 신규1개만 아래처럼 제안합니다. 새 모듈은 기존 usage_bridge와 service_ai가 import하는 보호경로/마스킹/기록 구현이므로 누락되면 import 실패하며 실행에 필요합니다.

| 항목 | 기존 승인 SHA256 | 검토 후보 SHA256 |
| --- | --- | --- |
| collab/service_ai.py | 6bc4351fb5288b0edc2e7ad6adeeb46864aa197144291f7529073dd81895ddeb | c72d88ee558824287c0284e349c1b51e65f906782eb3f0b4f8d39fa963980215 |
| collab/service_batch_once.py | de3ce4c90fbad6c2de6e99d278b3ab10ca0e65ab9d16106ef09f79360ab54bdb | 1dfa673007b71ebc2c70b75d6f107cee3a558f8e4fc4070ff032e7ac2be8be70 |
| collab/usage_bridge.py | d410857d4046721cf63bbf380b78dc1e710d81bba101c2780e41c6cfb6587751 | 2bc508b260f9bedaebbe58f27d663b7c989b39b66263c28bf949fb712483c0af |
| collab/usage_diagnostics.py | 신규 | 869deb9382404fae002055e562852a394e6fddbddb1b553196c21bb9667c29e2 |

격리 Git checkout을 운영 manifest에 무조건 대입하면 **26 Python + PSM1 1 = 27개의 줄바꿈만 다른 파일**도 SHA가 바뀝니다. 미변경13개는 이미 raw동일, 나머지 의미변경3개입니다. 이번 ReviewPackage에는 그27개도 운영원본bytes로 복사해 기존40승인항목의 SHA차이를0으로 만들었습니다. 운영 경로/승인파일을 수정한 것이 아닙니다. 정확한 목록은 PreflightProof.json에 있습니다.

사용량-only 진입점은 collab 밖에 두어 Python43에 다른 모듈을 무조건 추가하지 않았습니다. 별도 permit의 entry_sha256, roles_sha256, trial_tool_root 및 CLI SHA를 개별 검토해야 합니다. 기존 모델용 runner 변경SHA는 이번 배포 감사 대상일 뿐 **사용량 시험 패키지에는 포함하지 않았습니다**. 두 기존 테스트파일도 운영 승인목록에 넣지 않습니다.

## 독립 시험 진입점

`Tools/DiscordAIServiceDiagnostics/usage-only-once.py`. 기본 실행은 HOLD이며 실제 활성 permit은 만들지 않았습니다. 승인 초안은 approved=false/expires_at=0/placeholder request이므로 그대로 실행할 수 없습니다.

추후 대표가 별도 승인한 보호된 ProgramData/LimitlessRelay/usage-only-approval.json을 읽어 approved=true, 10분이내 만료, 새requestID, 기존boot승인 SHA, **별도 trial루트**, entry/roles/44개 정확한 파일집합/기존CLI SHA, MANUAL, 1회/20초/재시도0/모델0/Discord0/SQLite0 범위를 모두 확인해야 실행됩니다. 임의 추가파일·미변경40항목의 새SHA·변조·잘못된 한도는 HOLD입니다. 기존 승인 JSON의 tool_root를 파일에 덮어쓰지 않고, **별도 승인된 trial 루트가 정확히 일치한 경우에만** 메모리상의 독립 시험 정책으로 서비스 자기프로필/CLI 검증을 재사용합니다. 이는 기존 Worker의 배포 승인을 바꾸지 않으며 원본경로 검사를 생략하는 방법으로 사용하면 안 됩니다.

서비스 SID/프로필/비관리자문맥을 확인하고 자기 .codex에만 preflight 로그/소비마커를 씁니다. 동일 request는 실패후에도 재사용하지 않습니다. 1회는 provider.read 1회(account/rateLimits/read 1회)를 뜻하며 초기화·account/read 필수RPC는 기존대로 수행합니다. 기존 공식 ChatGPT계정 검증을 재사용하고 API/타사용자인증·새CLI로 전환하지 않습니다. 오류상세는970a7247의 보호로거, 사전검사 결과는 같은 자기프로필 로거에만 기록하고 공용결과는 고정 사유/횟수만 출력합니다. 서비스 신원이나 보호경로 자체가 확인되지 않으면 안전하게 로그를 쓸 수 없으므로 다른 위치로 유출하지 않고 HOLD합니다.

entry 전체 시작시20초 deadline을 잡고, 파일해시 중에도 초과를 검사합니다. 사전검사 후 남은시간에서2초를 빼고 최대18초 이내 read만 수행하며 기존 종료정리 상한을 예약합니다. OS/파일시스템의 비정상 정지를 hard realtime으로 보증하는 시험은 아니므로 실제 Session0에서는 전체 supervisor 시간제한도 별도 검토해야 합니다. 재실행 루프는 없습니다. 별도 모델단발runner·publish·Store/Scheduler/ResumeQueue·모드변경 호출이 없으며, MANUAL은 시험 직전 별도 운영확인으로 확인해야 합니다(진입점은SQLite를 읽거나 쓰지 않습니다).

## 새 오프라인 검증만 — 12 PASS / 0 FAIL

합성 승인/합성 서비스 프로필/가짜 provider 응답으로 정상1회·동일ID 재사용차단·거부/HOLD/보호로그·timeout무재시도·미승인/만료·기존PSM1 SHA보존·추가파일거부·코드변조·사전검사시간소진·잘못된횟수/CLI·기본실행HOLD·금지호출의존성부재를 확인했습니다. provider는 메모리 모형이며 실제CLI/계정/사용량은 조회하지 않았습니다. 기존22건과45패키지/부팅/게임QA는 재실행하지 않았습니다. 새2파일 구문/diff-check PASS.

## 전후 실제 SHA256

기존5개 운영관련파일과 테스트2개는 모두970a7247의 실제bytes SHA와 일치합니다. 전=보존된 운영원본bytes, 후=격리후보bytes이며 줄바꿈 포함입니다. 새entry/test는 이번 추가입니다.

| 파일 | 전 SHA256 | 후 SHA256 |
| --- | --- | --- |
| Tools/DiscordAICollab/collab/usage_bridge.py | d410857d4046721cf63bbf380b78dc1e710d81bba101c2780e41c6cfb6587751 | 2bc508b260f9bedaebbe58f27d663b7c989b39b66263c28bf949fb712483c0af |
| Tools/DiscordAICollab/collab/usage_diagnostics.py | 신규 | 869deb9382404fae002055e562852a394e6fddbddb1b553196c21bb9667c29e2 |
| Tools/DiscordAICollab/collab/service_ai.py | 6bc4351fb5288b0edc2e7ad6adeeb46864aa197144291f7529073dd81895ddeb | c72d88ee558824287c0284e349c1b51e65f906782eb3f0b4f8d39fa963980215 |
| Tools/DiscordAICollab/collab/service_batch_once.py | de3ce4c90fbad6c2de6e99d278b3ab10ca0e65ab9d16106ef09f79360ab54bdb | 1dfa673007b71ebc2c70b75d6f107cee3a558f8e4fc4070ff032e7ac2be8be70 |
| Tools/DiscordAIServiceDiagnostics/service-final-once.py | 1c21aa6b584e0b0d166551658f96085652d9ec27b49d02d44528df46531127be | 36a3213652d883c5c1e849babe8bd755477d9ba33e7e827e442ac99a12ff13ca |
| Tools/DiscordAICollab/tests/fake_usage_app_server.py | 신규 | 9f2ebce678044a2aeb57098efa1d1c423811db58cf7b85dffb5bde81baf5bd05 |
| Tools/DiscordAICollab/tests/test_usage_diagnostics_delta.py | 신규 | 340de06bd402688a8974629ee5add469177c8a81710906e03c52cc45c993ef69 |
| Tools/DiscordAIServiceDiagnostics/usage-only-once.py | 신규 | f176f64145009894659daecdeaed5085e633d624eebfcb0db3a651e0aea48fd7 |
| Tools/DiscordAIServiceDiagnostics/test_usage_only_once.py | 신규 | 1e59e499d94acf18adcf5254c02973f0a531cba3db0cdabcff5bc97533dc619d |

## 안전한 배포·복구 절차 — 제안, 미실행

1. 우선 **별도 trial 패키지**에 대한 명시승인을 받습니다. 현재 검토본은 `Temp/DiscordUsageDiagnostics20261026/DeploymentPreflight/ReviewPackage`이며 운영실행 경로·부팅트리거가 없습니다. 기존CLI/기존서비스자기CODEX_HOME/운영코드와 업무DB는 공유설정 변경 없이 보존합니다. 인증/암호/토큰/.venv 저장소를 복사하지 않았습니다.
2. 변경파일4개와entry·roles·runtime집합44의 SHA, 원본 승인SHA, 별도 trial루트, 새request/만료/권한 범위를 개별 승인합니다. 기존 boot-approval를 새값으로 자동갱신하지 않습니다. 기존 ProgramData 디렉터리 ACL 조회에서는 시스템/관리자/소유자쓰기·서비스ReadAndExecute를 확인했습니다. 이후 활성permit 생성 시에도 같은 보호를 확인하고 기존 ACL을 변경하지 않는 절차를 승인받아야 합니다.
3. 기존 봇/아트/OnBoot는 그대로 둡니다. trial 한정 서비스계정 Session0 실행방법/작업시간제한을 먼저 검토합니다. 암호를 코드나 파일로 복사하지 않고 기존 안전한 관리자·서비스 실행 절차만 별도 승인합니다. **실제사용량1회20초 승인이 없으면 실행하지 않습니다.**
4. 단발시험 전후 원본boot승인SHA/CLI/52보호파일/DB업무·승인/부팅상태를 대조합니다. 실패하면 HOLD·보호로그만 남기며 재시도하지 않습니다. 사용량성공도 AI/A활성화 승인이 아닙니다.
5. 되돌리기는 우선 trial 활성permit을 관리자 소유 비활성 보관본으로 원자적 이동하여 신규접수를 막고, 실행중 요청이 끝났는지 확인한 뒤 trial만 철수합니다. 이미 소비한 marker/진단로그는 삭제·초기화하지 않습니다. 원본코드·boot승인을 변경하지 않는 배치이므로 원본을 강제로 되돌리거나 봇을 재시작할 필요가 없습니다. 아직 활성permit/시험작업은 만들지 않아 이번에 복구할 운영변경은 없습니다.
6. 장기운영으로의 인플레이스 교체는 별도 작업입니다. 실행중 봇의 import 상태가 옛코드일 수 있으므로 live 파일덮어쓰기/42→43 자동확장으로 완료 판정하지 않습니다. 필요 시 별도 승인된 정지/재시작 창, 정확한 원본byte/ACL백업,44항목검토 및 PS검증기의 count/고정SHA 영향 검토 후 transactional 적용·실패복구를 계획해야 합니다. 이번에는 그 변경을 하지 않습니다.

## 보호와 종료

운영52파일 SHA불일치0, 업무DB핵심8테이블동일·MANUAL, 원본main/status와 운영worktree상태 동일. 보호검사용DB조회만 readonly이며 **시험 진입점의SQLite업무처리는0**입니다. 현재 OnBoot/CIM 재조회나 기존 정상기능 재시험은 하지 않았고, 부팅설정/서비스/CLI/CODEX_HOME/인증/ACL/게임/Save/Push/main병합/종료·재부팅 변경0입니다.

브랜치 codex/discord-usage-diagnostics-20261026. 최종로컬commit은Issue66/최종보고 참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 별도승인 전 운영배포·공식사용량조회·AI·A를 실행하지 말고 중지합니다.
