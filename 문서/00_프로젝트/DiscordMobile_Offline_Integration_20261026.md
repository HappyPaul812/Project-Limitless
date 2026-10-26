# Issue #66: Session0 추론 VERIFIED 및 2단계 오프라인 연결

## 1단계 실제 결과

요청 `d56fdba9618044b98f98fe98e4cff211`의 사용자 전달 결과와 실제 ProgramData `InferenceTrials/<request>/Output/summary.json`을 대조했습니다. 후보 commit `d9fa4f7c4e7eeb8f78c34ab6bc19ec92d4595635`, 준비 commit `90e8e7dde33d25a66d95651395d866a1cc1a48f4` 기준입니다.

- **VERIFIED**: CLI exit 0, 정상 완료 턴 1개, JSONL PASS, agent_message 1개, error/forbidden 항목 없음.
- 최종 응답: **LIMITLESS 기획 담당 AI입니다. 읽기 전용 연결 시험에 성공했습니다.**
- `PAUL-PC\LIMITLESSRelaySvc`, SID 일치, Session 0 / logon 4.
- 공식 사용량 App Server 1개와 추론 exec 1개. LIMITLESS 재실행 0회. 내부 HTTP/stream 재시도 수와 실제 원격 요청 수는 UNKNOWN입니다.
- 준비·추론·정리 전체 23.170초. worker 15.928초, stderr 0 bytes. Job exit 0 / 자식 0 / cleanup_verified.
- 기록된 추론 사용량 input 9,934 / output 22 / cached input 8,192. 사전 관측 5시간 53%, 주간 23%는 과거 실행 시점 값이며 재조회하지 않았습니다.
- permit 비활성화, 임시 작업 REMOVED, 신규 소비 marker 보존. 기존 marker 3개와 boot approval SHA, 배포된 launcher/entry/core SHA를 직접 확인했습니다.
- 실제 summary의 운영 장부·부팅 작업·승인·CODEX_HOME 불변 결과를 확인했습니다. 직접 대조 및 summary 판정 21/21은 `Evidence/DiscordMobileOffline20261026/Stage1Verified.json`에 보존합니다.

이 결과는 PIN 없는 서비스 계정의 읽기 전용 단발 추론을 입증합니다. 모바일 명령부터의 실제 전체 연결 또는 임의 업무·파일 수정 권한을 승인/입증하지 않습니다.

## 2단계 구현 범위

사용자의 기존 조건부 승인에 따라 1단계 확인 직후 **오프라인 연결 구현을 시작했습니다**. 새 `collab/mobile_offline.py`는 운영 bot에 import/등록하지 않은 별도 리허설입니다. 운영 파일을 수정하지 않고 새 합성 SQLite DB만 생성합니다. 운영 DB/기존 디렉터리 주입을 거부하며 재시작은 합성 식별자·소유자/서버/채널 설정이 일치해야 합니다.

재사용: 기존 Store 소유자/서버/채널 인증, interaction receipt, 작업 계약·lease·finish, 역할 표 `ROLE_FILES`/`FOUR_SLOTS`의 **역할 이름·순서만**, 엄격한 `output_parser.parse_result`, MANUAL/스케줄 게이트, `operation_checkpoints`. 기존 실제 실행 슬롯의 승인/소비 함수는 호출하지 않습니다.

연결은 모바일 interaction을 표현한 `mobile()` → 새 합성 업무 큐 → SHA에 묶인 실행 승인 → 고정 FakeExecutor → 기존 JSONL 검증 → FakeDiscord outbox → 역할 인계입니다. PlanningAgent → NewArtAgent/ RenewalArtAgent → CodexAgent → PlanningAgent 원래 업무에 결과를 기록합니다. 기획 회신 기록에는 5번째 추론이 없습니다. 후속 역할도 각각 소유자의 별도 실행 승인이 필요합니다.

- 실행 승인은 task/role/계약 전체/범위/base SHA/상위 응답/한도에 바인딩합니다. 단발 소비를 실행 전에 commit하며 만료 300초 이내, 역할당 1회·root당 최대 4회·동시 가짜 worker 최대 1개를 확인합니다.
- `OFFLINE_TEXT_REVIEW`만 허용, 합성 사용량 불가/정지/역할 정지/시간창/비 MANUAL이면 HOLD. 합성 사용량을 공식 관측으로 DB에 위장하지 않습니다. 기존 실제 사용량 게이트는 수정하지 않았습니다.
- 상위 응답·SHA와 전달 packet을 실행 직전에 대조합니다. 두 아트 결과 및 각각의 보고 성공 전에는 Codex를 만들지 않습니다.
- 오류/timeout/JSONL 불일치/금지 도구/잘못된 역할 인계는 NOT_VERIFIED + checkpoint. 소비한 요청은 재실행하지 않습니다.
- 보고는 task별 outbox로 중복을 막습니다. 전송 전후 오류는 UNKNOWN_NO_RETRY로 남겨 자동 재전송·후속 실행을 막습니다. 운영 연결 시에는 전송 영수증 대조 절차가 추가로 필요합니다.
- 실제 CLI/Discord 콜백을 받지 않으며 고정 FakeExecutor/FakeDiscord만 허용합니다. 모든 보고는 `[MOCK / OFFLINE_PASS]` 또는 `[MOCK / HOLD]`입니다. 미술 승인·제품 적용을 기록하지 않습니다.

## 모바일 승인 인터페이스 확인

기존 운영 `/limitless task`는 접수만 수행하며 `/limitless approve`는 **미술 승인 전용**입니다. 이 경로를 실행 승인으로 재해석하지 않았습니다. 오프라인 `approve-execution`은 별도 인터페이스 후보이며 **Discord에 등록되지 않았습니다**.

합성 인터페이스:
1. `mobile(owner, interaction_id, 'task', contract)` — `offline_only=true`, PlanningAgent, approval_required=true, 고정 읽기 범위.
2. `plan(task_id)`로 전체 계약 SHA 확인 후 `mobile(owner, new_interaction_id, 'approve-execution', {task_id, sha256, expires})`.
3. `step(owner, task_id)`, `report(owner, task_id)`, `advance(owner, root_id)`로 가짜 실행/보고/인계. 후속 task는 새 승인 대기.
4. `/status`, `/pending`, `/pause`, `/resume` 의미는 기존 Store 명령을 재사용합니다. resume는 실행 승인·재실행이 아닙니다.

동일 interaction의 동일 요청은 receipt를 재사용하고 다른 내용은 거부합니다. 기존 미술 승인·실제 승인 슬롯·이전 소비 marker와 새 합성 승인 테이블은 분리됩니다.

## 새 검증과 보호

`tests/test_mobile_offline.py`의 새 연결 경계만 실행했습니다. 기존 38/22+12/native/Unity 대규모 PASS 검사는 반복하지 않았습니다. 최종 **24 PASS / 0 FAIL / 0 ERROR / 0 SKIP**, 프로세스/네트워크 시도0·scratch 밖 Python 쓰기 시도0. 같은 Evidence 폴더의 `OfflineTests.json`을 기준으로 합니다. 테스트 중 subprocess.run/Popen과 socket.socket을 차단하고 Python open의 쓰기 경로를 짧은 합성 scratch로 감시했습니다. SQLite는 새 합성 경로만 생성하도록 별도로 제한했습니다.

모바일 인증 4가지 거부, interaction 중복/내용 변경, 정확한 승인 SHA·만료·계약 변경, 비승인/정지/합성 quota/모드 HOLD, 4역할 정상 인계 및 양쪽 아트 취합, 상위 결과 변조, 금지 항목/exit/error/invalid/timeout/경로 오류, 보고 전후 실패·중복, worker 중복·중단 복구·lease 만료를 검증합니다.

**1단계 이후 추가 실제 모델 0 / 공식 사용량 재조회 0 / Discord 게시 0 / 운영 SQLite 업무 0.** 운영 배포·봇/아트 감시 재시작·부팅 작업/승인 목록/인증·CLI·CODEX_HOME·ACL 변경 0. MANUAL 및 신규 실제 업무 실행 HOLD 유지. Unity/아트/Save/게임 원본·GitHub push/main 병합·PC 종료/재부팅 없음.

## 다음 승인 경계

오프라인 리허설을 운영 bot에 바로 배포하면 안 됩니다. 다음 단계는 별도 승인 하에 운영 adapter와 사용량/서비스 Supervisor 연결 설계·정확한 변경 SHA/manifest 차이·되돌리기를 검토해야 합니다. 기존 `real_roles.run_once`의 실제 실행 기본값을 새 모듈에 연결하지 않았습니다. 실전에는 이번에 검증된 keyring/feature 정책과 서비스 Session0의 제한 실행 구조를 유지한 별도 검토가 필요합니다.

실제 Discord 명령 등록/봇 배포·재시작, 운영 SQLite 변경, 추가 실제 모델 호출과 usage 조회에는 별도 승인 범위를 확정해야 합니다. A 모드는 여전히 미승인입니다. 기존 d56f 요청은 소비 완료이므로 재실행하지 않습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`. 시작 HEAD `90e8e7dde33d25a66d95651395d866a1cc1a48f4`. 구현 commit `0dffe9738eb8e56fe06af413e19411ab87f72b25`. 최종 문서 commit은 Issue #66 완료 댓글, 파일 SHA는 `ChangeHashes.json`에 기록합니다. 소스 push는 하지 않습니다.
