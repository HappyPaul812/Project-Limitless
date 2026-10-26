# Issue #66 — 실제 Discord 시험 Host READY / 모바일 결과 대기

**READY_FOR_MOBILE_FAKE_TRIAL / MANUAL / 실제 AI0 / A 미활성**.

시험 `Limitless-test` app1564156403571105906과 운영 app1563102982386749440의 분리를 확인했습니다. 대표가 직접 입력한 전용 token은 새 자기 계정 DPAPI 저장소에만 있으며 앱/대표 소유 API200·같은 Windows 사용자 저장/읽기 PASS입니다. 운영 token/서비스 인증은 읽거나 복사하지 않았습니다.

## 실제 접속·등록

- Guild1563112744436244541 / `ai-시험실` Channel1564163234338967552.
- 시험 앱 owner 및 Gateway application 확인, 대표 채널 View/Use Commands 및 기존 관리자 권한 확인, bot View/Send·Administrator 없음 확인. 새 권한을 부여하거나 운영 역할을 변경하지 않았습니다.
- 시험 앱의 기존 guild 명령0/global 명령0 목록을 격리 경로에 백업했습니다.
- **단일 guild CREATE1** 성공. 반환 Command ID **1564175559733551186**, 그룹 `/limitless-trial`, 하위7개. sync/copy_global_to/bulk overwrite/global CREATE0. 기존 `/limitless approve` 수정0.
- Gateway PASS, 실제 Host PID26424 / 부모34112, 현재 Windows 사용자 **PAUL-PC\windows / Session9**. 이번 Fake Host는 서비스 Session0 실전이나 부팅 자동 실행을 새로 입증하는 시험이 아닙니다. 기존 Phase1 Session0 VERIFIED는 별도로 보존합니다.
- READY 당시 FakeExecutor0/모델0/공식 usage0/운영 SQLite 업무0. 모바일 응답은 아직 대표 확인 대기입니다.

Host 자동 종료 시각: **2026-10-26 16:34:38 KST**. worker는 준비 포함19분30초, 외부 Native Job은 최대20분30초 후 프로세스 트리를 강제 정리합니다. 마지막 원격 정리 helper가 필요한 경우 최대40초 추가로 정리만 수행합니다. Host를 자동 재실행하거나 새 요청을 발급하지 않습니다.

## 모바일 첫 명령과 순서

휴대전화의 `ai-시험실`에서 **Limitless-test**의 명령을 선택합니다.

```text
/limitless-trial status
/limitless-trial task title:휴대전화 연결 시험
/limitless-trial plan task_id:<기획 task ID>
/limitless-trial approve-execution task_id:<기획ID> sha256:<plan의 전체64자리SHA> ttl_seconds:300
/limitless-trial run-fake task_id:<기획ID>
/limitless-trial handoff task_id:<기획ID>
```

두 아트 task 각각 plan/승인/run-fake→기획 ID handoff→Codex task plan/승인/run-fake→기획 ID handoff로 원래 기획 업무 기록을 확인합니다. root1·Fake최대4·동시1, 실제 모델/공식 usage0입니다. 소비 전 취소는 cancel-execution이며 취소를 위해 root를 초기화하지 않습니다. 모든 응답은 가짜 실행/한국어/ephemeral/멘션 차단입니다.

고정 모의 quota fixture만 갱신하며 실제 구독 사용량을 조회하지 않습니다. task 승인 TTL은 늘리지 않습니다. SHA/만료/취소/소비·역할 인계/JSONL 검증은 기존 Phase2/3 어댑터 그대로 재사용합니다. 실제 followup message ID가 보존돼야 인계할 수 있습니다. 오류/결과불명은 checkpoint/HOLD·자동 재시도0입니다.

## 종료·정리

대표가 이 채팅에 **“시험 종료”**라고 알려주시면 접수를 차단하고 아래 Stop marker 경로로 안전 종료합니다. 자동 만료도 같은 정리 경로를 수행합니다. 작업 관리자에서 임의 강제 종료하지 않습니다.

```powershell
pwsh -NoProfile -File "F:\study\codex\Project-Limitless\Temp\MobileLive\2de25e01be5646f7b07a4a3bcf2413b4\run-mobile-trial-host.ps1" -Root "F:\study\codex\Project-Limitless\Temp\MobileLive\2de25e01be5646f7b07a4a3bcf2413b4" -Action Stop
```

접수 차단→Gateway close→이번 반환 ID만 DELETE1→guild/global 기존 목록 전후 대조→부모 Job의 자식0을 확인합니다. worker가 정리 전에 종료되면 별도 cleanup-only helper는 **반환 ID가 있고 DELETE 시도가 없는 경우만** 실행합니다. DELETE 결과가 불명/실패면 재시도하지 않고 HOLD합니다. CREATE 결과가 불명일 때 ID를 추정하여 삭제하지 않습니다.

소비 마커·승인·취소·업무/실제 보고 receipt·checkpoint·inventory·정제 진단은 새 현재 사용자 전용 ACL 폴더에 보존합니다. 명령을 삭제해도 시험 DB나 과거 마커를 지우지 않습니다. 네트워크/원격 삭제 실패까지 무조건 정리 성공으로 보장하지 않습니다.

## 새 구현·검증

- 별도 `mobile-trial-host.py`: 자기 DPAPI 해제, 고정 endpoint 단발 REST, 최소 Gateway cache, 7개 기존 callback 연결, 타이머/stop, 정확 ID 삭제/목록 대조. API scope를 운영 app/global write/bulk로 확장할 수 없습니다.
- `run-mobile-trial-host.ps1`: 현재 Windows 사용자·ACL/approval/package/host/Python SHA 확인, 이 일회성 부모의 자식 환경에서만 API 관련 변수를 제거, Native Job 실행 및 필요 시 정리 helper. 기존 서비스/시스템 환경변수 변경0.
- `MobileTrialJob.cs`: 기존 검증 구조를 별도 class로 복사해 시간 한도와 **ActiveProcessLimit1**을 적용했습니다. stdout/stderr 상한·drain·kill-on-close 유지. 기존 추론90초 Job은 수정하지 않았습니다.
- venv redirector는 추가 프로세스가 필요해 제한에 걸렸습니다. 제한을 풀지 않고 기존 불변 pyvenv.cfg의 **정확한 base interpreter+기존 SDK site-packages**를 고정 사용했습니다. 기존 venv/SDK/Python 설치를 바꾸지 않았습니다. Native 누적 membership 수는 동시 프로세스 수와 다르므로 total1이라고 주장하지 않습니다.
- 새 REST/정리/미확정 경계 **8 PASS/0 FAIL**, PowerShell 구문 PASS. 새 Native 일반 종료/timeout tree 정리/추가 자식 차단 PASS와 base Python SDK import PASS. 자기 trial DPAPI 읽기 PASS(네트워크0). 기존 Phase1/2/3 전체 PASS 검사는 반복하지 않았습니다.

이전 16파일 ZIP/Manifest는 그대로 두고 Host3파일을 별도 SHA로 새 실행 루트에 고정했습니다. `Evidence/DiscordMobileLiveHost20261026/PreparedRun.json`, `ReadySanitized.json`에 source/config/원래 목록 SHA·command ID·시각을 보존합니다. token·암호화 token 자료·원시 private DB/inventory는 Git에 넣지 않습니다.

실행 루트: `Temp/MobileLive/2de25e01be5646f7b07a4a3bcf2413b4/`.

현재는 **접속/등록 READY이며 모바일 실제 결과/최종 정리는 미완료**입니다. 결과 확인 후 별도 후속 보고합니다. 추가 AI/A 자동 활성화 없음. 기존 운영 bot/아트/부팅/운영 SQLite/승인/CLI/CODEX_HOME/인증/ACL·Unity/게임/Save·Push/main·PC 종료/재부팅 변경0. 새 시험 경로의 ACL만 현재 사용자로 제한했습니다.
