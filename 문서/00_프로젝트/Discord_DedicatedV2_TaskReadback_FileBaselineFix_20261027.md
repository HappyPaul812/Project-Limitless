# r2 Task 읽기 재검증 PREPARE 오류 수정 — 2026-10-27

대표 원본 Admin-354b78ca922a41ebb1ff3f4765c96652.json 판독: HOLD / PREPARE / read_attempts0, MethodInvocationException 두 단계 뒤 PropertyNotFoundException(-2146233087). 관리자 Task COM/보안 후보 비교에 도달하지 않았으므로 실제 Task 권한 실패라고 판정하지 않습니다. descriptorUNKNOWN/effective_worker_accessUNKNOWN/runtimefalse 유지. changes/install/IPC/model0.
원본 경로의 전달 문자열 공백은 실제 00_프로젝트 경로와 대조했습니다. 원본은 변경하지 않았습니다.

직접 원인: backend.File은 먼저 FILESYSTEM_<Path> query의 .value를 읽고, 나중에 Manifest/Host 전용 query로 대체했습니다. 기존 snapshot의 Manifest는 INSTALLED_MANIFEST, Host는 TASK_ACTION_PIN_COMPARE_NO_ARGUMENT_OUTPUT.value.host_metadata에만 있으므로 첫 빈 query 결과의 .value 접근이 StrictMode PropertyNotFoundException을 냈습니다. 기존 가짜 Prepare 시험은 File을 가짜로 대체해 이 실제 조회 경로를 놓쳤습니다. 제공 도구의 결함이며 설치/계정/Task 문제로 돌리지 않습니다.

구도구 probe-task-security-readonly.ps1(SHA de9bf4639b1d1ae079802ade7d1e5f8bdc251af51a587bfdfa61f799eb95af41)은 증거로 보존하고 명령은 사용중지합니다.
별도 probe-task-security-readonly-v2.ps1은 Path별 stage를 먼저 결정한 뒤 정확1개/READ_OK를 확인하고 .value에 접근합니다. Host에만 host_metadata를 선택합니다. 기준누락/중복/UNKNOWN은 계속 HOLD입니다. SourceSHA/ACL/Task정책·후보48SHA·Core·금지정책은 바꾸지 않았습니다.
v2 SHA8968636afea53fdba1c371ff2b54cc53bd07ac0937059b07a090730f41b280f4.
새 회귀 test-file-baseline-v2-offline.ps1 SHA d5e26b5501993f4505c1d7e13becc54f609529fccd770c30f6a83dc61c5fcb28.

신규9 PASS/0 FAIL: 실제 구File 메서드의 Manifest/Host 예외 재현2건, 실제 수정File 메서드3경로 성공, 기준누락/중복/UNKNOWN거부, 구문. 파일/ACL/읽기함수만 가짜로 대체하고 실제 메서드와 고정 snapshot을 사용했습니다. 대표와 같은 EncodedCommand→ScriptBlock 경로로 검증했습니다.
기존48/22 전체 테스트와 관리자45항목·현장조회는 재실행하지 않았습니다. 실제 관리자 재조회0, OS변경0. 증거 FileBaselineV2Offline.json 및 FileBaselineV2Preparation.json.

## 별도 승인 후 바울 대표 직접 실행용 — 수정 읽기 진단 한 줄

이번 턴에서는 실행하지 않았습니다. 기존 Install/Stage/Resume나 구진단 명령을 다시 실행하지 않습니다.
`powershell
$r2TaskCmd='$r2TaskBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\TaskSecurityReadback\probe-task-security-readonly-v2.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2TaskBytes)).ToLowerInvariant() -cne "8968636afea53fdba1c371ff2b54cc53bd07ac0937059b07a090730f41b280f4"){throw "R2_TASK_READONLY_V2_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2TaskBytes))) -ConfirmApprovedReadOnly';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2TaskCmd))) -NoNewWindow -Wait
`

결과는 같은 Evidence/TaskSecurityReadback20261027/Admin-<새GUID>.json으로 CREATE_NEW됩니다. 기존 원본/승인/소비 파일 덮어쓰기0, 암호입력0/계정TaskACL변경0.
이전 준비 문서의 최소대상 범위·사전SHA/관리자PS7x64/전후지문·성공후 effective access HOLD 정책은 그대로 유지됩니다. 실제 Task descriptor 판정은 다음 승인된 읽기 결과가 오기 전 UNKNOWN입니다.
실패 시 자동 재시도/삭제/복구 금지. V1/MANUAL/A/runtime HOLD, 설치·IPC·AI·운영변경·Push0. 브랜치 codex/discord-usage-diagnostics-20261026, 기준8d48b7041eb0c37bcb3de66389474822eb08d4b0, 최종 로컬SHA Issue후속 / REMOTE_NOT_PUSHED.
