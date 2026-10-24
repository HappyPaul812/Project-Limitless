# SHA 등록 및 사용자 Worker 오프라인 연결 완료

상태: **OFFLINE_CONNECTED_EXECUTION_DISABLED / MANUAL / 실제 AI0**. 자동 시작 등록·A 활성화·실제 인증/사용량/Discord 호출0. 운영 outbox에는 모의 보고를 넣지 않았다.

## 승인된 내용 등록

검토 제안서의 절대 경로에서 소스를 다시 읽고 실제 바이트 SHA/길이를 대조했다. 아래4개 모두 일치하여 등록했고 파일 내용은 수정하지 않았다.

| 파일 | SHA256 |
| --- | --- |
| failure_evidence.py | 83d129e36da79aeff500b279109fa4e528c722cc0f78242c3f2e9fdd1c458302 |
| offline_batch_diagnosis.py | ad544477ab7149103799c44071ea6a4d15a8b8bca28d58607e2e77e9216180df |
| private_execution_log.py | 691c8e433b8e46e3f3e3f777084184b006973bc1ec62dd898abeba770fe2e492 |
| service_batch_once.py | de3ce4c90fbad6c2de6e99d278b3ab10ca0e65ab9d16106ef09f79360ab54bdb |

별도 `C:\ProgramData\LimitlessRelay\user-worker-integrity.json`을 만들고 실행 허가·자동 시작=false, 사용자 SID/공식CLI 묶음4개/연결코드5개/기존Python38개/진단4개의 내용을 고정했다. 현재 총51개 파일 내용 검사 PASS. 새 목록은 보호 DACL로 현재 소유자·SYSTEM·Administrators만 접근하며, 기존 승인 파일 ACL·소유자는 바꾸지 않았다.

기존 부팅 검사는 별도 목록을 읽지 않으므로 `boot-approval.json`의 runtime_sha256에 호환용 SHA4개만 추가했다. 기존38 Python 및 원래 map39개 항목(다른 모듈1개 포함)의 값 보존, runtime_sha256 이외 모든 JSON 필드 동일을 별도로 확인했다. 기존 검사 코드·부팅 설정·OnBoot2 변경0. 현재 collab Python42개 SHA 불일치0이며 검사를 완화하거나 파일을 이동하지 않았다.

- 승인 파일 전 SHA: ec85c406fe222efc1d7460ba7757ba9d207b419ef5f3648b82880b91eea4c121
- 승인 파일 후 SHA: 0cd2ce55f7e23b44190e7f4786c2e9f68e2e291b1888e5768ea606f391c8d2d6
- 최종 별도 목록 SHA: 5a4eeb4ae4469e578624de2048e6cdfdd05dd0c9b6f89faeba2f5a4e6df49827
- 보호된 기존 승인 백업: C:\ProgramData\LimitlessRelay\boot-approval.before-diagnostics-8e8e1fa23ae44e37aee458f39bf09138.json
- 로컬 결과: Tools/DiscordAICollab/data/user-worker-registration.json, user-worker-runtime-binding.json

SHA 등록은 내용 신뢰 고정이며 실제 실행 허가가 아니다. 실제 adapter는 released 기본false이며 별도 목록 실행 허가=false도 독립 검사하므로 실행을 막는다. 새 목록에 일반 사용자 완전 CLI와 동반exe4개를 등록했지만 CLI를 호출하거나 인증을 열지 않았다.

## 운영 SQLite 연결

기존 `Tools/DiscordAICollab/data/phase2/relay.sqlite`에 아래 전용 표4개를 추가했다. 다른 DB로 업무를 분산하지 않았다.

- user_worker_jobs: 정확히 승인된 계획/SHA/SID/상태
- user_worker_lock: Worker 단일 claim, 중단 후 자동 재탈취 금지
- user_worker_checkpoints: 업무 상태·남은 재검증 항목
- user_worker_handoffs: 기존 역할 규칙에 따른 인계·후속 승인 대기

온라인 SQLite backup API로 먼저 백업했고 파일 연결도 명시적으로 닫는다. 백업: `data/user-worker-backups/before-worker-connect-6c7b37edb6b84bb0bcfca193e0d481b2.sqlite`.

BEGIN IMMEDIATE 트랜잭션에서 기존 표12개의 모든 행을 전후 정규화 SHA로 비교해 동일함을 확인했다. tasks21/settings29/approvals0/operation_mode1/operation_checkpoints14/control_resume0/control_schedules1/operation_history5/usage13/audit152/control_usage82/control_outbox10 보존. 이후 봇이 자기 heartbeat/업무를 갱신하는 것과 이번 연결이 기존 행을 변경하지 않은 것은 구분한다.

Worker 운영 업무0건·실행0건이다. 실제 업무나 모의 계획/가짜 승인을 운영 DB에 넣지 않았고 기존 중단 작업을 재발행하지 않았다. `data/user-worker-connection.json`에 백업·보존 결과를 기록했다.

## 연결된 동작

1. 로그인 전 enqueue는 유효한 정확한 SHA/역할/미소비 승인만 받아 PENDING_LOGIN으로 보관한다. 승인 만료/소비/변경/기존 시도 결과가 있으면 실행하지 않는다.
2. 사용자 Worker는 실행 허가·Windows SID/Session>0/자기 HOME/CODEX_HOME·등록 CLI 전체 묶음·연결 코드·기존 runtime SHA를 확인한다. 서비스·관리자·다른 프로필·다른 SID는 거부한다.
3. 공식 ChatGPT login status/App Server 두 사용량 창/정본 SHA/예약·정지 상태/승인 범위를 재확인한다. 이 callback은 이번 검증에서 전부 모의였다. 범위가 지정된 기존 작업 체크포인트는 현재 task 역할·상태·scope·base SHA와 비교한다.
4. 기존 PlanningAgent/CodexAgent/NewArtAgent/RenewalArtAgent의 real_roles.run_once와 출력/JSONL 검증을 그대로 사용한다. 기존38개 수정0. runtime_adapter는 exec의 원본 바이트를 자기 프로필 DPAPI 로그에만 보존하며 로그인 상태 원문은 기록하지 않는다. timeout 부분 출력도 보존한다.
5. 성공한 task의 역할/event_key/TECH_PASS/모드/plan SHA를 확인한 뒤 기존 auto_handoff를 재사용한다. NewArt/Renewal→Codex→Planning→대표 검토로 업무만 인계하고 후속 AI 승인이나 연쇄 실행은 만들지 않는다. 인계 증거가 불명확하면 NOT_VERIFIED로 보류한다.
6. 실패/한도 소진/인증 불명은 체크포인트와 control_outbox에 고정 HOLD 보고를 기록한다. 기존 봇이 기존 전송 경로로 보고하므로 Worker는 Discord 토큰을 읽거나 직접 연결하지 않는다. outbox 전송 불명은 기존 UNCERTAIN 처리로 중복 전송을 막는다. 이번에는 격리 DB와 모의 게시기만 사용했고 실제 전송0이다.

## 오프라인 검증

신규 연결8개·기존 회귀20개·원본로그 adapter3개, **고유31개 PASS**. 변경 영향이 있는 4역할 전달은 마지막 adapter 연결 후 선별 재검증했다. 실패했던 migration fixture의 Row 정규화·선택 표 부재·백업 연결 종료를 수정하고 해당 항목 PASS 후 운영 DB에 적용했다. outbox 검증은 네트워크/Windows IOCP 없이 실제 기존 코루틴을 모의 publish로 재생했다.

- 역할별 인계, 후속 승인 자동 발급 없음, 중복 업무·중단 claim 차단
- 미승인/만료/소비 승인·다른 SID/Session0·역할 정지·정본/CLI 변경 차단
- 한도 소진/UNKNOWN/HOLD·실패 체크포인트·outbox 전송1회/중복 차단
- 운영 migration 전후 원래 표 보존 및 backup 생성
- SHA 등록만으로 실행 허가되지 않음
- exec 원본 바이트/timeout 부분 보존, 로그인 상태 원문 미기록

Python compile/PowerShell 구문/diff 검사 PASS. 부팅 작업의 현재 XML 읽기는 권한상 불가하여 새 XML 대조 PASS로 보고하지 않는다. 변경 명령은 실행하지 않았고 Bot/Art RUNNING·heartbeat 증가, MANUAL 유지, 기존 source/부팅 설정 보존을 확인했다. 실제 재부팅/사용자 실제 추론/실제 Discord 전송 실증은 이번 범위 밖이다.

## 실제 읽기 전용1회에 남은 조건

**현재 즉시 실행 가능한 상태는 아니다.** 기존 실행기가 허용하는 단발 슬롯12개가 전부 CONSUMED이며 validate는 그 슬롯 집합만 허용한다. 과거 슬롯을 삭제/재사용하지 않는다. 새 단일 계획을 승인할 슬롯 지원 확장과 그 코드 SHA 재검토가 먼저 필요하다. 이는 기존38개 보존 조건 때문에 이번에 변경하지 않았다.

다음 조건을 모두 충족한 별도 승인 후에만 시험한다.

1. 소비된 슬롯과 분리된 새 승인 슬롯 지원을 검토하고 변경 코드의 SHA를 별도 승인한다.
2. 대표가 읽을 문서·역할·정확한 계획SHA·90초·호출1회·게시 여부를 승인한다.
3. 로그인한 일반 사용자 identity/배정 SID·완전 CLI pin·자기 계정 원본 로그 저장을 확인한다. 다른 계정 인증을 복사하지 않는다.
4. 특정 시험의 실행 허가를 별도로 부여하고 실행 직전 공식 인증/5시간·주간usage/정본·예약·미소비 승인 gate를 통과한다. MANUAL·자동 시작off는 그대로 둘 수 있다.
5. 실패 재시도0, 금지 도구/출력 검증 유지, 성공·실패 결과를 보존하고 중지한다.

이번에는 이 조건을 제시하고 멈춘다. 실행 허가·새 슬롯·새 승인·A모드·Worker 자동 시작·유료 API·미승인 아트 적용·Unity/Save·Push/merge·종료/재부팅0.

브랜치 codex/discord-ai-collaboration, 선행 commit fe0f9865. 이번 commit SHA는 최종 보고를 따른다.
