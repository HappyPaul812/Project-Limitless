# Issue66 LSA 배치 권한·오류 기록 최소 수정 후보

2026-10-26. 기준 `d41ceaf156755ad18d8f8557d2b815ffb936ce3e`, branch `codex/discord-usage-diagnostics-20261026`. **OFFLINE36_PASS / NEW_CANDIDATE_NOT_APPROVED / NOT_INSTALLED**. 이번 승인은 수정·오프라인·새 SHA 보고까지이며 실제 복구·재설치 승인이 아닙니다.

## 변경 내용

`TrialAccountPolicy.cs`의 고정 Length32/Max34를 제거했습니다. 고정 권한 이름 `SeBatchLogonRight`의 실제 UTF-16 byte 수를 계산하여 **Length34/Max36**을 전달합니다. 빈값/내부NULL/잘못된 surrogate/USHORT 범위 초과는 할당 전에 거부합니다. 할당 후 이름 전체와 종료NULL을 검사하고 예외 경로에서도 Dispose/finally로 메모리와 handle을 해제합니다. 내부 시험 경계 ILsaApi만 주입 가능하며 공개 실행 경로의 관리자 및 신규SID 검증, 실제 정책 접근mask0x810, 단일 권한/단일 호출은 유지했습니다. fake 구현은 설치 payload에 포함하지 않습니다.

실제 존재하는 native 오류는 LsaPolicyException의 NtStatus/NativeErrorCode/Operation에 보존합니다. Open/Add/Close 실패를 구분하고, 원래 실패와 정리 실패가 함께 발생하면 원래 예외를 유지한 채 cleanup native 코드를 추가합니다. 자동 호출 재시도는 없습니다. 현재 과거 설치의 실제 native 원인 판정은 여전히 INCIDENT_NATIVE_CAUSE_NOT_VERIFIED이며 새 fake 결과로 과거 원시 코드를 만들어내지 않습니다.

`install-session0-trial.ps1`은 코드게시/계정생성/그룹/SID/컴파일/권한부여/Manifest/ACL/State/task/record 단계를 기록합니다. 실패 시 예외 type/HRESULT·실제 NtStatus/Win32·InnerException chain(최대8) 및 단계만 정제합니다. message/stack/InvocationInfo/암호/token/credential 내용은 기록하지 않습니다. 알려진 native Operation은 남기며 없는 코드는 만들어내지 않습니다.

오류 로그는 고정 관리자 보호 설치 root 아래 고유 `InstallFailure-<guid>.json`에 **원자 CREATE_NEW + protected DACL(SY/BA만 FullControl)**로 작성하고 실제 owner/group/DACL/ACE를 재조회합니다. parent의 Worker/Bot 읽기 ACE도 상속하지 않습니다. 초기 root 생성 실패 등으로 보호 로그를 만들거나 검증할 수 없으면 그 실패를 표시하며 일반 경로에 원문을 쓰는 fallback은 없습니다. 정제된 오류 객체는 HOLD 예외 Data로 wrapper까지 전달됩니다. 새 로그 생성·ACL readback은 이번에 실제 Windows에서 수행하지 않았습니다.

`run-atomic-trial-install.ps1`의 대화형 `exit 2`를 제거했습니다. JSON **logical_exit_code=2/0** 및 현재 세션 LASTEXITCODE로 논리 결과를 전달하고 현재 PowerShell을 종료하지 않습니다. 이 값은 별도 pwsh 프로세스의 실제 OS 종료코드를 관찰한 값과 구분합니다. 기존 receipt는 읽기만 하고 새 GUID receipt를 사용하며, 자동 설치 재실행/삭제는 없습니다. 보호 로그에는 오류 type/code만 들어가고 wrapper의 작업브랜치 JSON도 정제 metadata만 전달합니다.

## 신규 오프라인 검증

**36 PASS / 0 FAIL**, `test-lsa-repair-offline.ps1` 및 `LsaRepairOfflineTests.cs`. 기존29/원자35/중개기42 전체 테스트를 재실행하지 않았습니다.

- UTF-16 34/36·정확한 이름·종료NULL·Unicode surrogate·빈값/내부NULL·USHORT 한계·Dispose 검증.
- fake LSA 정상/잘못된 권한명 상태/접근거부/닫기실패/중복실패/NULL handle. 원시 NTSTATUS와 Win32 보존·원래 예외 유지·한 번 호출 확인.
- 설치 예외/InnerException 및 cleanup native 코드가 정제 JSON과 wrapper로 전달됨. 합성 비밀번호/token 문자열은 출력0, 없는 native 값은 추가0.
- private log의 protected DACL·정확2ACE·owner·추가 사용자 거부, 원자 생성 코드 및 새 publisher C# 컴파일(native호출0).
- 부분 code/state/bot/task가 있으면 가짜 효과 발생 전에 거부. 기존 task 등록 함수의 CREATE-only·동시SDDL·readback·불일치HOLD·충돌/중복재시도 금지를 가짜 Scheduler3경우로 집중 확인.
- 기존 원자 보안4개 함수의 AST 본문이 승인 atomic 후보와 동일함을 확인. 기존35 전체 재시험을 대신한 코드보존 확인입니다.
- runner ExitStatement0·논리코드 분리·후속 script 실행 지속 확인.

**실제 LSA/Windows Scheduler 호출0, installer 본문 실행0, Windows 권한 변경0.** 실제 Windows 설치·private log ACL 생성·Session0/IPC PASS로 보고하지 않습니다. 최초 신규35 PASS 후 cleanup metadata 검증1개를 추가한 최종36 PASS입니다.

## 유사 코드 읽기 전용 감사

Tools의 C#/PowerShell/Python에서 LSA 문자열·권한API 관련 코드를 검색했습니다. `install-boot-tasks.ps1`은 고정17글자 이름의 `name.Length*2` 및+2를 사용해34/36이며 이번 오프바이원 없음. 기존 부팅 설치기는 수정·실행하지 않았습니다. SHA `b38dc7d0018fcb534048cee343e7b90b678a7bd3f87fd17edca2e82eeb755e8d`.

readonly probe/runner의 LsaEnumerateAccountRights는 Windows가 반환한 Length를 사용하며 입력 권한 이름을 고정 길이로 만들지 않습니다. `Attr.Length=Marshal.SizeOf`는 구조체 크기여서 Unicode 문자열 길이와 별개입니다. 검색 증거 `LsaAudit.txt`. 이 감사는 기존 Windows 정책을 변경하거나 실제 API를 호출하지 않습니다.

## 새 패키지 및 SHA256

후보: `F:\study\codex\Project-Limitless\Temp\Session0LsaRepairCandidate20261026`

ZIP: 동일 경로 뒤 `.zip`, **15,503,163 bytes**. 이전 Ready/Atomic 후보 및 ZIP을 덮어쓰지 않았습니다. 생성 도구는 새 경로 전용이며 기존 경로가 있으면 거부합니다.

| 변경 파일 | 이전 SHA256 | 새 SHA256 |
|---|---|---|
| TrialAccountPolicy.cs | `1d85ea2b3b99027dbf0eeb4ee61b590b8be3539d7c9e7992af0dee03c8d8d96e` | `d9a0bc50137119bc28c17c5496b5fc20b4da81925b806bc472a538c4415f850b` |
| install-session0-trial.ps1 | `117065bbfca686474f9104bc0dff20fa6528dfb9c204e5a2f2720d91994958e9` | `79194ad63dfaf500158d7afa8aadf59e26e0a7487ee595ae9c685e35003c75f8` |
| run-atomic-trial-install.ps1 (payload밖) | `75ce33d60c34327b37d61cabd2fa362941933a280f919426c01eb7c9118bab70` | `04ccc520cf9271cb055674f5636e6360fdbaceefddaae73077c247644349de0e` |
| 새 LsaRepairOfflineTests.cs | 없음 | `3fcf5beb49e4816e7ac168aefda251772843f90482f0e466e22ff76b319e1133` |
| 새 test-lsa-repair-offline.ps1 | 없음 | `7feadf41ad51eb13943206d4e26523ddbe61ef1d490bfb826885d95fe4cc92b9` |
| 새 prepare-lsa-repair-candidate.py | 없음 | `e12c78ec6caecff8b1d6e448bc6c1855727bd9641e29f267c770859cd2e10a83` |

| 패키지 항목 | SHA256 |
|---|---|
| 새 Installer | `79194ad63dfaf500158d7afa8aadf59e26e0a7487ee595ae9c685e35003c75f8` |
| 새 Manifest | `33ed9a8318c039cd70cf8260338f56e81025ce891c0156ac18556c2aec36d165` |
| 새 ZIP | `d678e760c1ccd41d05a4683a776cf2e877026c1de9bc5fd0a61aeccb4231daf3` |

정확855 payload 중 변경2: `Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs`, `Deployment/install-session0-trial.ps1`. **동일853/추가0/삭제0**. HostManifest metadata 별도1이며 의미 차이는 이 두 hash 항목뿐입니다. CLI/Runtime741/Legacy47/FAKE_ONLY/auto_start=false/기존 permit미승인 설정 등은 그대로입니다. 테스트/생성도구/runner는 설치 payload에 추가하지 않았습니다. ZIP 내부 모든 파일 bytes SHA도 대조했습니다.

이전 Atomic 후보의855개+Manifest+ZIP 및 Ready 후보의855개+Manifest는 생성 전후 전체 inventory 동일. 원래 Install1 증거 SHA `80523d2b2320cabce31789d7322a3a7c480ff8f225d42c1f0120a3bdcbea5d6b`, 관리자 진단 SHA `f6f8c9e3589299a3dd918a4bb1395164d6797812bbfe85c832fda0cb234fb9be`, boot approval SHA `3e032ebcce3d91bd762a8dec81048d04607ddde761afd62489c0fa2b8e49140f` 보존 확인.

## 부분 설치 자산 처리 방안 — 실행 안 함

현재 설치된855개는 예전 사본이며 **새 source/후보와 별개**입니다. 이번에 다시 쓰거나 실시간 관리자 SHA 검사를 반복하지 않았고 마지막 관리자855일치 증거를 보존했습니다. Bot1008은 비활성, task현재부재, 보호 Code root 존재, Install1 소비 완료 상태를 변경하지 않았습니다.

새 설치기는 부분 환경을 자동 재사용하지 않습니다. 기존 root/계정이 남아 있으면 **신규 설치 거부**가 정상입니다. 이를 우회해 기존 계정을 활성화하거나 권한/코드를 덮어쓰는 경로는 구현하지 않았습니다.

권장 최소 계획은 다음과 같습니다.

1. 대표의 별도 정리 승인 뒤 관리자 읽기 전용으로 SID1008/계정명/Disabled·Code root 절대경로/BA소유/reparse없음/855해시 및 예상 밖 파일 유무를 재확인합니다. Worker1007/Admin1001/운영 task는 제외합니다.
2. 설치/진단/소비 증거와 필요한 code snapshot을 보호된 위치에 보관합니다. 새로운 파일이나 신원 불일치가 있으면 정리도 HOLD합니다.
3. 승인된 신규 시험 자원만 정리해 fresh 조건을 만족시킵니다. 현재 task부재를 이유로 다른 task를 지우지 않습니다. 소비증거/기존 후보/인증은 삭제하지 않습니다.
4. 새 Code/Installer/Manifest/ZIP SHA와 새 설치 범위에 대한 **별도 설치 승인** 후 새 계정/Disabled task를 설치합니다. 이전 Install1을 되돌리거나 재사용하지 않습니다.

기존 계정1008을 직접 재사용하려면 별도 SID바인딩/그룹/권한/만료/취소/소비 설계와 승인 추가가 필요합니다. 이번에는 그러한 재사용 모드를 넣지 않았습니다.

## 새 승인 후 관리자 실행 절차

**지금 실행하지 않습니다.** 부분 자원 확인/정리 승인과 새 SHA 설치 승인이 선행되어야 합니다.

관리자 PowerShell7의 새 세션에서 code/runner/패키지 SHA를 확인하고, 부팅task/boot승인·고정 Worker SID를 확인한 다음 새 runner의 Install을 단1회 실행합니다. 로컬 SecureString/Credential로만 새 Bot 및 기존 Worker 암호를 입력합니다. JSON logical_exit_code와 보호 로그를 확인하고 실제 SD/ACL/SID/24h/855SHA·OnBoot 전후 검증 뒤 **Disabled/trigger0/FAKE_ONLY 상태에서 중지**합니다. 실제 IPC/AI/usage/Discord는 별도 승인입니다.

향후 승인 후 사용될 정확한 명령(현재 실행 금지):

```powershell
& { $repairBytes=[IO.File]::ReadAllBytes('F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\run-atomic-trial-install.ps1'); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($repairBytes)).ToLower() -ne '04ccc520cf9271cb055674f5636e6360fdbaceefddaae73077c247644349de0e'){throw 'RUNNER_SHA_HOLD'}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($repairBytes))) -Action Install }
```

실패 시 자동 재실행/자동 삭제 없이 HOLD합니다. private log를 만들 수 없는 단계에서도 기록 불가를 명시하고, 보호 조건을 만족하지 않는 위치에 오류 원문을 저장하지 않습니다.

## 수행하지 않은 작업과 기록

이번 실제 Windows 계정/task/service/권한/ACL/부팅/부분root 변경0. 실제 LSA/Scheduler 등록/IPC/FakeWorker/모델/공식usage/Discord0. 기존 봇/아트/SQLite/인증/CODEX_HOME/Unity/게임/Save/Push/main/PC 변경0. MANUAL/A HOLD 유지. 원본 main tracked dirty308/untracked36 동일.

증거 `문서/00_프로젝트/팀운영/Evidence/Session0LsaRepair20261026/`의 NewRepairTests.txt, Candidate.json, BaselineInventory.json, CandidateInventory.json, SourceSHA256.json, LsaAudit.txt 및 Result.json. CURRENT_STATUS/CODEX_LATEST와 Issue66에 실제 로컬 commit을 기록합니다.

**오프라인 후보 완성 후 중지. 실제 복구·재설치 전 대표의 다음 승인이 필요합니다.**
