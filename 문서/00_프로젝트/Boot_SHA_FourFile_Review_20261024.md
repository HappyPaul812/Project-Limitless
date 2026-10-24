# 신규 진단 코드 4개 SHA 등록 검토 — 제안서만 작성

## 결론과 선별 기준

**SHA 목록 변경·배포·실행 승인은 이번 범위에 포함되지 않습니다.** 네 파일은 진단 자료가 아니라 Python 실행 코드입니다. 라이브러리2개와 명령 진입점2개로 구분합니다.

- **기존 부팅 기능의 의존성으로 필요한 신규 파일: 0개.** 기존 승인 코드38개와 새 사용자 Worker는 이4개를 import하지 않습니다. 기능 의존성만 기준으로 하면 모두 부팅 실행 대상에서 제외합니다.
- **현재 위치와 기존 전체 SHA 검사를 유지하는 경우의 조건부 등록 후보: 4개.** boot-service.ps1은 collab/*.py 전체에 대해 등록 SHA가 있어야 통과시킵니다. 네 파일이 현재 위치에 남아 있는 한 일부만 등록해도 RUNTIME_SOURCE_SHA_MISMATCH 조건은 남습니다. 부팅을 실행한 결과가 아니라 검사 코드와 현재 목록을 대조한 판단입니다.
- 따라서 현재 위치 유지·기존 검사 유지로 정합성을 복구하려는 목적에 한해서 네 SHA 모두 검토 대상으로 제안합니다. 이 전제를 승인하지 않으면 전체 보류합니다. 기능상 불필요한 파일을 배포에서 제외하려면 별도 배포 범위 검토가 필요합니다. 이번에 파일 삭제·이동·검사 예외 추가는 하지 않습니다.
- offline_batch_diagnosis.py와 service_batch_once.py를 부팅 실행 대상 또는 일반 사용자 Worker의 의존성에 추가하는 것은 제외합니다. 네 파일 모두 자동 실행·재시험·Discord 게시 권한은 부여하지 않습니다.

기존 승인 Python38개는 모두 승인 SHA와 일치합니다. 미등록은 아래4개뿐이며, 실제 SHA는 이전 제안과4/4 일치하고 현재 commit blob과 바이트도 같습니다. 이번 검토에서는 실행 코드를 수정하지 않았습니다.

## 경로·생성 이력·정확한 SHA

기준 디렉터리: F:\study\codex\Project-Limitless\Temp\DiscordAICollab20261023\Worktree\Tools\DiscordAICollab\collab\

각 절대 경로는 동봉 JSON의 absolute_path에도 개별 기록합니다. 생성 브랜치는 모두 codex/discord-ai-collaboration이며 해당 브랜치 reflog와 파일 이력으로 확인했습니다. Git commit 객체 자체에 생성 브랜치 필드가 있다는 뜻은 아닙니다.

| 파일 | 최초 생성 commit | 현재 내용 commit | SHA256 |
| --- | --- | --- | --- |
| failure_evidence.py | 8dbe40765df3c8842e01cf5d69430232d13ead7e | f32d4f04666f40f45b3872b9efdc5037aad89054 | 83d129e36da79aeff500b279109fa4e528c722cc0f78242c3f2e9fdd1c458302 |
| offline_batch_diagnosis.py | 8dbe40765df3c8842e01cf5d69430232d13ead7e | f32d4f04666f40f45b3872b9efdc5037aad89054 | ad544477ab7149103799c44071ea6a4d15a8b8bca28d58607e2e77e9216180df |
| private_execution_log.py | f32d4f04666f40f45b3872b9efdc5037aad89054 | f32d4f04666f40f45b3872b9efdc5037aad89054 | 691c8e433b8e46e3f3e3f777084184b006973bc1ec62dd898abeba770fe2e492 |
| service_batch_once.py | 293db5d64cae9a7744ec1c8c6d4b1bf5744a7565 | f32d4f04666f40f45b3872b9efdc5037aad89054 | de3ce4c90fbad6c2de6e99d278b3ab10ca0e65ab9d16106ef09f79360ab54bdb |

## 파일별 내용과 위험 검토

### failure_evidence.py — 낮음 / 진단 라이브러리

실패 JSONL을 메모리에서 분석하여 고정 오류 분류·알려진 item type·행 번호·내용 제거 projection을 기록합니다. 네트워크·모델·인증·subprocess·임의 코드 평가·주 실행 진입점이 없습니다. 쓰기 위치는 호출자가 전달하므로 이 함수가 경로 접근 권한을 확장하지는 않습니다.

기존 output_parser의 EVENTS/strict_json을 재사용하지만 허용 항목·성공 판정을 변경하지 않습니다. 도구/부작용 보고와 미지원 항목 분리는 진단 정보일 뿐이며 둘 다 기존 검증기에서는 계속 거부합니다. 기존 승인 기능을 대체하지 않고 추가 진단 함수를 제공합니다.

현재 호출자는 offline_batch_diagnosis와 service_batch_once입니다. 정상 부팅·현재 사용자 Worker에는 필요하지 않습니다. 현재 위치를 유지할 때만 조건부 SHA 등록 후보이며 호출 승인은 별도입니다.

### offline_batch_diagnosis.py — 중간 / 수동 진단 진입점

기존 batch-summary와 서비스 프로필의 .codex/log/*.log 및 .sandbox/sandbox.log를 읽고 해당 시각의 고정 분류만 data 아래 JSON에 기록합니다. 자유 형식 로그 본문은 반환하지 않습니다. CLI·모델·네트워크·예약 작업 호출이 없고 인증/keyring/config/상태DB를 직접 읽지 않습니다.

다만 일반 자료 파일이 아니라 python -m 명령으로 읽기·쓰기를 수행하는 실행 코드입니다. 원본 서비스 로그에는 민감정보가 포함될 수 있어 실행 계정·읽을 범위·결과 취급은 별도 검토가 필요합니다. 현재 원본 로그 존재/접근권한은 미확인입니다. 정상 부팅·추론·현재 사용자 Worker 의존성에서 제외하고 현재 위치 유지 시에만 조건부 SHA 등록 대상으로 봅니다.

### private_execution_log.py — 중간~높음 / 암호화·저장 라이브러리

원본 stdout 바이트를 CurrentUser DPAPI/UI_FORBIDDEN으로 암호화하고 같은 자기 프로필에 비밀 제거 stderr/JSONL, 최초 error·turn.failed·실행 메타데이터를 저장합니다. ctypes로 CryptProtectData/LocalFree를 호출하고 사용한 네이티브 메모리를 지웁니다. 복호화·전송·추론·인증 저장소 읽기는 없습니다.

원본 stdout에는 추론 내용이나 잠재적 비밀이 포함될 수 있어 일반 분류 함수보다 위험도가 높습니다. 비밀 제거는 best-effort여서 완전성을 보장하지 않습니다. 동일 보호 프로필에만 저장하고 외부 자동 게시를 금지하는 전제가 필요합니다. 실제 서비스 계정의 저장 경로 ACL·재분석 지점을 포함한 검증은 아직 완료되지 않았습니다. SHA 검토가 이 검증을 대신하지 않습니다.

현재 호출자는 service_batch_once뿐이며 정상 부팅·현재 사용자 Worker에는 필요하지 않습니다. 같은 설계의 향후 진단을 선택할 때 필요하며 현재 위치 유지 시에만 조건부 SHA 등록 후보입니다. 배포·호출 승인은 별도입니다.

### service_batch_once.py — 높음 / 실제 추론·외부 게시 진입점

서비스 계정·프로필·고정 CLI를 확인하고 GitHub 단일 문서를 받으며 공식 인증/사용량을 조회합니다. SQLite에 사용량·소비 슬롯·감사를 기록하고 subprocess로 Codex exec1회/90초를 실행할 수 있습니다. 기존 JSONL·최종 응답 검증을 통과하면 Discord에 게시하는 코드도 있습니다. 순수 오프라인 진단 자료로 분류할 수 없습니다.

기존 승인 파일에 같은 기능의 버전은 없습니다. 기존 provider/CLI controls/parser/gate를 재사용하지만 전용 슬롯과 소비 마커를 가진 추가 실행 경로이며, 사용자 Worker가 재사용하는 기존4역할 run_once와는 별개입니다. 과거 마커/CONSUMED 상태를 새 실행 승인과 혼동하지 않아야 합니다. 이번에 마커 삭제·승인 재발행·호출은 하지 않았습니다.

정상 부팅·현재 Worker 의존성·자동 재시험 대상에서 제외합니다. 현재 위치 유지 시 내용 고정을 위한 SHA 등록만 조건부 검토 대상으로 보며, 추론·인증 조회·로그·DB 쓰기·Discord 게시 권한은 포함하지 않습니다. 향후 실행은 별도 계획과 대표의 명시적 승인이 필요합니다.

## 참조 관계와 기존 기능 영향

정적 import 역참조와 실행 스크립트의 호출은 다음뿐입니다.

```text
offline_batch_diagnosis -> failure_evidence
service_batch_once -> failure_evidence, private_execution_log
service-batch-once.ps1 -> python -m collab.service_batch_once
tests -> failure_evidence, private_execution_log
```

기존 승인 코드38개에서 위4개로 향하는 import는 발견하지 못했습니다. collab/__init__.py도 자동 실행 등록을 하지 않습니다. OnBoot 검사 단계는 파일 내용을 hash로 확인할 뿐 이4개를 실행하지 않습니다. 그러므로 SHA를 등록한다고 자동으로 AI·진단이 시작되는 것은 아닙니다. 다만 등록 대상 실행 코드를 늘리는 관리 결정은 파일별 내용 검토가 선행되어야 합니다.

## 비밀정보 검사

네 파일 전체 내용을 읽고 AST/문자열 검사를 수행했습니다. Windows 경로·로컬 계정명·공개 GitHub 주소·환경변수 이름은 있지만 Windows 암호·Codex/Discord/GitHub 실토큰·API키·개인키의 하드코딩은 발견하지 못했습니다. OpenAI키/JWT/GitHub키/개인키PEM의 알려진 패턴 일치0입니다. 이 결과는 향후 런타임 로그에도 비밀이 없다는 보장이 아닙니다.

service_batch_once는 런타임 환경의 비밀을 메모리에서 비밀 제거에 쓰고 Discord token을 접속에 사용합니다. private_execution_log는 런타임 출력을 보존합니다. 이 성격을 위험 등급에 반영했습니다. 실제 비밀 저장소를 열어 검사하거나 다른 계정으로 복사하지 않았습니다.

## 보호와 적용 경계

현재 ProgramData 승인 파일 SHA256 기준값은 ec85c406fe222efc1d7460ba7757ba9d207b419ef5f3648b82880b91eea4c121이며 조사 전후 같은 값인지 확인합니다. 실행 코드4개·기존 승인38개·OnBoot2·검사 코드·ACL을 변경하지 않습니다. Bot/Art RUNNING·MANUAL 유지, AI/로그인/공식사용량조회/Discord게시0, 게임/Save/Push/merge0입니다.

동봉 JSON은 대표가 이후 검토할 조건부 후보 목록이지 ProgramData에 자동 적용하는 변경 명령이 아닙니다. functional_boot_registration_required는 전부false, required_by_current_scanner_while_present는 전부true입니다. 부분 등록을 선택하면 남는 HOLD 조건을 수용해야 하며 이번에 이를 해결했다고 보고하지 않습니다.

이번 승인은 제안서 작성만 허용합니다. 앞선 파일 이동 거부를 취소한 것으로 보지 않고 이동 재시도·검사 우회·자동 승인0을 유지합니다. SHA 내용 등록, 배포/의존성 변경, 진단 실행, AI/Discord 실행, Worker 배포는 각각 별도 승인 대상입니다.
