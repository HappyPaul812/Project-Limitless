# r2 읽기 진단 ScriptBlock 범위 오류 수정 — 2026-10-27

상태: READONLY_V2_READY / ADMIN_RESULT_PENDING / PARTIAL_INSTALL_HOLD.
직접 원인: 대표 명령은 외부 PS1을 -File로 실행하는 대신 ScriptBlock::Create 후 &로 호출합니다. 그 로컬 실행 범위에 $r/@queries를 만들었으나 Add-R2ProbeQuery 함수는 $script:r을 읽어 변수 미정의 오류가 발생했습니다. 기존 -File SelfTest는 이 실제 부트스트랩 경로를 검증하지 못했습니다. Codex 제공 도구의 오류이며 Windows 계정/Task 설치 실패 원인이 아닙니다.

실패 위치는 첫 R2_CONSUMPTION_CHILDREN 읽기 query의 결과 누적 지점입니다. 해당 읽기는 일부 실행됐을 수 있지만 이후 Task COM·계정 조회·최종 JSON 저장에는 도달하지 못한 것으로 코드 흐름상 판독합니다. 관리자 결과 수집 성공을 주장하지 않습니다. 설치·계정·Task·ACL 변경 코드는 호출하지 않았습니다. 설치 승인·소비 마커 상태는 그대로 보존하며 Task 상태/설치 직접원인은 계속 UNKNOWN입니다.

원래 probe-r2-partial.ps1 및 SHA d3e9b499491dae3b6087878c1b40c2b8f4d6b3d96c1b0d8c4847377d621fe407은 수정하지 않았습니다. 이전 진단 명령은 사용 중지합니다.
새 probe-r2-partial-v2.ps1은 결과/발견기록/합성시험 누적 변수의 초기화 범위를 기존 함수가 사용하는 script 범위와 일치시킵니다. 실제 Add-R2ProbeQuery를 합성검사에서도 사용하도록 정의 위치를 정리했습니다. 조회 대상·권한·정제·금지 정책을 바꾸지 않았습니다.

대표와 같은 새 PowerShell -NoProfile -EncodedCommand → 해시검증 대상 바이트의 ScriptBlock 실행 경로로 SelfTest를 수행했습니다. 21 PASS / 0 FAIL, 실제 OS조회/변경/설치/암호입력0. 신규 회귀: 실제 query 함수 성공·오류 결과 누적, 발견 Result 경로 유지, COM/Manifest 모의상태 유지. 기존 도구의 신규17검사를 수정 도구의 회귀로 함께 검증했으며 과거 Windows 성공검사/설치/Stage는 반복하지 않았습니다.
증거: Evidence/DedicatedV2R2Readonly20261027/ScopeFixOffline.json 및 ScopeFixPreparation.json.

## 관리자 PowerShell 7 수정 진단 명령

아래는 읽기 전용 진단입니다. 비밀번호 입력이 없습니다. 설치 명령을 다시 실행하지 않습니다.
새 도구 SHA256 d3d9fe8f10ad2e18028f1e4e8010eb1e7421a36ef0f062e9743dc597fe59ea84

`powershell
$r2ProbeCmd='$r2ProbeBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Readonly\probe-r2-partial-v2.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2ProbeBytes)).ToLowerInvariant() -cne "d3d9fe8f10ad2e18028f1e4e8010eb1e7421a36ef0f062e9743dc597fe59ea84"){throw "R2_READONLY_V2_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2ProbeBytes))) -Mode Collect -ConfirmReadOnly';Start-Process -FilePath 'C:\Program Files\PowerShell\7\pwsh.exe' -ArgumentList @('-NoLogo','-NoProfile','-EncodedCommand',[Convert]::ToBase64String([Text.Encoding]::Unicode.GetBytes($r2ProbeCmd))) -NoNewWindow -Wait
`

결과 경로는 종전 Evidence/DedicatedV2R2Readonly20261027/Admin-<GUID>.json, 새 결과파일1개만 생성합니다. 출력된 요약과 정제 JSON을 판독해야 Task/계정/권리/부분자산 상태를 판단할 수 있습니다. 실패해도 자동재시도/삭제/복구하지 않습니다. 실제 설치·새승인·계정Task권한 변경·IPC·AI0, V1/MANUAL/A/토큰GROUPJob HOLD. 브랜치 codex/discord-usage-diagnostics-20261026. 로컬 커밋은 Issue 후속 보고, REMOTE_NOT_PUSHED.
