# Issue66 V2 r9 관리자 사전검사 준비

상태: **PACKAGE_PASS / WAIT_ADMIN_READONLY / HOLD / NOT_INSTALLED**. V1 REFERENCE_VERIFICATION_HOLD 유지. 기준 구현62578e5174d208dd4e83ebe23c2ac438303eb478, 이전 문서53fb709d171b532de2262545f6f8462621f2ce6f. 브랜치 codex/discord-usage-diagnostics-20261026. 최신 Issue66 기획 댓글6431175631과 로컬 r9 보고를 대조했습니다.

## 이번 실제 결과

- r9만 읽었습니다. Installer SHA `1a1d671d1cf86aab56c9c2392ed4ba35a8240e348134e6a467f8eb8ec81f17a0`, Manifest SHA `250bab2d46010841353a319fbcf34581784366ac3042e6bc0fdc97aadd17c09e`, ZIP SHA `a4a652bdf73c9a1007bd1e761ae539d3459896b46d8a021fa1459012a1d15c41` 모두 일치.
- payload766 + Manifest1, ZIP767 entry 정확 집합/각 bytes SHA/중복 검증 PASS. 후보·ZIP 변경0, r1~r8 사용0.
- 실제 Codex 토큰은 관리자 아님(admin=false). 관리자 Windows 경계는 실행하지 않고 HOLD. V2 계정/경로/Task/pipe 부재, Worker SID/직접 batch/group/유휴, 후보 소유권/쓰기권한, 기존 root855/OnBoot/운영 승인 SHA는 **NOT_VERIFIED**, PASS로 추정하지 않습니다.
- 실제 결과 JSON: `F:\study\codex\Project-Limitless\Temp\V2R9Preflight20261027\2ab98a66585f4560a0045a5c240cf015.json`.
- 설치/계정/권한/Task/IPC/Worker/인증 복사/소비·승인 파일 생성0. 기존 Install1/Cleanup 증거와 CODEX_HOME·ChatGPT 인증·운영 SQLite·Unity·Save 읽기/쓰기0. 이 무변경 사실은 기존 시스템 상태 전체의 새 SHA 검증과 다릅니다.

## 관리자 직접 읽기 전용 명령

도구: `Tools/DiscordAIServiceDiagnostics/V2/prepare-r9-readonly.ps1` (후보 패키지 밖의 전용 도구).

SHA256: `8193c5a121082866ec5473c6d99d4ca025bfbc8b84737a13af591a845a2d51a8`.

새 관리자 PowerShell 7에서 아래 블록을 실행합니다. **설치 승인/설치 명령이 아닙니다.** 암호를 요구하지 않습니다. 기존 Windows 보호 경계를 우회하지 않습니다. 창을 닫는 exit나 실행 정책 변경이 없습니다.

```powershell
$preflightPath = 'F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\V2\prepare-r9-readonly.ps1'
$preflightBytes = [IO.File]::ReadAllBytes($preflightPath)
$preflightSha = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($preflightBytes)).ToLowerInvariant()
if ($preflightSha -cne '8193c5a121082866ec5473c6d99d4ca025bfbc8b84737a13af591a845a2d51a8') {
    throw 'V2_R9_PREFLIGHT_SHA_HOLD'
}
& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($preflightBytes)))
```

결과는 `F:\study\codex\Project-Limitless\Temp\V2R9Preflight20261027\<새GUID>.json`에 CREATE_NEW로만 기록합니다. 오류 Message/Stack/명령줄/환경변수/토큰/암호를 출력하지 않고 type/HRESULT/가용 Win32 숫자만 보존합니다. 대상 pipe에는 연결하지 않고 이름 목록만 확인합니다. 불완전 조회/거부/이름충돌/Worker busy/기존 다른 실행 가능 작업/후보 비관리자 owner 또는 writer는 HOLD입니다. ACL을 자동 수정하거나 후보를 자동 이동하지 않습니다.

검사기는 SHA를 확인한 r9 memory bytes에서 계약 및 native 정의를 컴파일하며, adapter의 Context/Snapshot/Fresh/WorkerState 읽기 메서드만 호출합니다. Consume/AcquireLease/CreateAccount/GrantBatch/Publish/WriteBinding/RegisterTask/Verify/Record 및 설치기 진입점은 호출하지 않습니다. V1 root855 검사는 이번 관리자 승인 범위에서만 예정되며 기존19개 보호 프로세스 조사는 반복하지 않습니다. 기존 읽기 조건에서 오작동/조회불명이 발생해도 기준을 완화하지 않습니다.

## 신규 도구 검증과 한계

- PowerShell AST parse0오류, exit노드0, 금지 변경 cmdlet0.
- adapter 호출 AST allowlist는 Context/Snapshot/Fresh/WorkerState만이며 PASS.
- 실제 비관리자 실행에서 패키지 PASS, 관리자 gate HOLD, 정제 JSON 저장 확인.
- 기존84/18/9/2 suite는 반복하지 않았습니다. 이번 관리자 분기·LSA 읽기·CIM/COM·ACL 실측은 아직 미실행입니다. 위 정적 검사가 native Windows 성공을 증명하지 않습니다.

## 설치 승인 단계와 중지 조건

사용자 지시의 '사전검사가 모두 PASS라면' 조건이 아직 충족되지 않았으므로 **현재 설치 명령·INSTALL_V2 승인 파일은 발급하지 않습니다.** 관리자 JSON 판독 후 새 보호 승인 root/owner/DACL/정확3SHA/epoch/실제Admin·WorkerSID/1시간 이내 expiry와 단발 consume를 포함한 직접 설치 절차를 구체화합니다. 그 명령을 바울 대표가 직접 실행하는 행위를 설치1회 승인으로 연결해야 하며 Codex가 자동 실행하지 않습니다. 현 상태는 새 승인을 요구하는 것이 아니라 이미 요청된 관리자 읽기 검증 결과를 기다리는 단계입니다.

설치가 허용되더라도 대상은 신규 BotV2 Disabled/OS 발급 SID, 신규 V2 code/state, Disabled/trigger0/restart0/FAKE_ONLY 작업1개뿐입니다. 암호는 로컬 SecureString 입력, 기존 Worker 자격은 로컬 입력만 사용하고 인증/CODEX_HOME 복사는 하지 않습니다. 생성 시점 제한 DACL/SDDL, SID·groups·직접권한·owner·SHA·Task Principal/Owner/DACL과 기존 상태 readback을 확인해야 합니다. 시작/IPC/FakeWorker는 별도 승인 전0입니다.

오류/부분 상태는 HOLD, 기존 consume 유지, 자동 재시도/삭제/권한완화0. 생성된 V2 SID·객체·정제 오류 단계·보호된 결과를 확인한 뒤 별도 복구 승인으로만 처리합니다. V1 HOLD를 V2 설치로 해제하지 않으며 V1/기존Worker/OnBoot/운영을 복구 대상으로 섞지 않습니다. 설치 후 판독은 실제 설치가 실행된 경우에만 수행합니다.

LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 실제 Windows 설치0, 설치 명령 실행0, 관리자 검증 대기 후 중지.
