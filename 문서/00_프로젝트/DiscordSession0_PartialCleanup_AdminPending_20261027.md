# Issue66 과거 부분 설치 자산 정리 — 관리자 실행 대기

2026-10-27, 구현 commit `82e41f64e85e57bc3675c7e4413c6f4857023caa`, 기준 `ae163034dd299254fbc0b2e30a482f821faf08ca`, branch `codex/discord-usage-diagnostics-20261026`.

**CLEANUP_APPROVED / WAIT_ADMIN_CLEANUP / 실제 삭제0**. 대표는 실패한 Install1의 신규 시험 계정과 옛 Code root 정리만 승인했습니다. 새 패키지 설치 승인 아님. 현재 Codex는 PAUL-PC\windows/Session9/비관리자이므로 관리자 재검사·정리는 대표가 아래 명령으로 직접 수행해야 합니다. 정리 승인을 다시 묻지 않습니다.

## 정확한 변경 대상

- 삭제 후보 Code root: **C:\Program Files\LimitlessRelayTrial**. 다른 Program Files/Temp/프로젝트/새 후보는 삭제 대상으로 받을 수 없습니다.
- 삭제 후보 계정: **PAUL-PC\LIMITLESSBotTrial**, SID **S-1-5-21-2858504392-1958168942-1662906075-1008**. 비활성/Users 단일그룹/직접권한 없음/기존만료/설명 marker가 일치해야 합니다.
- 시험 task **LIMITLESS-Trial-S0Host**는 관리자 재조회로 부재가 확인돼야 합니다. task가 있거나 조회가 불명확하면 HOLD. 등록/삭제 기능은 도구에 없습니다.
- 새로 만드는 보호 백업/단발 정리 소비 위치: **C:\ProgramData\LimitlessRelayTrialCleanup20261027**. 기존 경로가 있으면 덮어쓰지 않고 HOLD합니다. 정리 후에도 백업·소비·결과를 남깁니다.
- 기존 Worker SID1007/Admin1001/OnBoot2개·인증·CodeX_HOME·운영 SQLite·새 LSA 후보는 변경 대상이 아닙니다.

## 도구와 검증

| 파일 | SHA256 |
|---|---|
| cleanup-partial-trial.ps1 | `4d2725591b67eb8d744299a6fe8e15fbad8069a0dc72725323b1b208c606ce29` |
| TrialCleanupNative.cs | `1c8abecf66cacf45e4dcc1958f1ea44f4243472012f273c46e2255722d9b50f9` |
| test-trial-cleanup-offline.ps1 | `62ec9cc0bf4610c89c424b6a76e260fc102b419192d5c2f5b30d545abefdf6a7` |

신규 합성 **43 PASS/0 FAIL**. 실제 Windows 파일/계정 삭제·백업 생성·LSA/Scheduler 호출0인 테스트입니다. 기존36/29/35/42 전체 검사는 반복하지 않았습니다. 처음39/1에서 임의 대문자 메시지의 요약 노출 문제를 발견하여 고정 식별자 allowlist로 수정했고, 참조트리3개 추가 후 최종43/0입니다.

검증: no-exit·설치/Task/프로세스 변경명령 없음·계정 SID 삭제1곳만, 정확한 경로/유사이름/../프로젝트경로 거부, handle로 검증한 파일만 삭제·빈 디렉터리 비재귀 삭제, 원자 private 백업, owner/DACL/추가ACE 거부, 정규화 fingerprint, long/short/SID 참조 탐지, hidden Task 트리·빈XML/조회실패 거부, 서비스+드라이버 포함, 비밀 오류문구 제거, SID/활성/그룹/권한/만료/Task/파일/참조/조회불명/프로필/logon/보존 불일치 차단, 백업→재검사→Code→계정재검사→SID삭제→최종 순서, 백업/재검사/파일삭제 실패 시 후속 계정삭제·재시도0.

현재 일반 토큰 Inspect 결과 `ce0507152c8c407d85e8a02999b91bbf`: FIXED_ADMIN_REQUIRED, cleanup_consumed=false, code/account/Task 삭제0. **실제 관리자 정리 PASS가 아닙니다.**

## 실제 관리자 실행 시 순서

1. 고정 Admin SID1001의 실제 관리자 토큰과 Native helper SHA를 확인합니다. 과거 Install1/관리자 진단/기존 Atomic 및 새 LSA 후보 Manifest·Installer·ZIP·각855개 hash, Worker SID1007, OnBoot2개 XML hash와 boot 승인 SHA를 대조합니다. 기존 파일을 읽거나 hash하는 것이며 설치기는 호출하지 않습니다.
2. Bot1008의 정확한 name/SID/Disabled/marker·Users·직접권한[]·기존 만료시각을 확인합니다. 현재 logon/profile이 있거나 LastLogon이 발생했으면 예상 밖 사용 이력이므로 HOLD합니다.
3. 기존 Task의 exact-not-found 및 hidden 포함 관리자 열거를 확인합니다. Code root의 절대경로/모든 ancestor·자식 reparse/owner·group·DACL/855 SHA/예상 directory 목록/숨김·시스템·readonly·ADS·hardlink 여부를 검사합니다. 파일과 directory는 읽기 전용 독점 handle로 사용 중 여부도 확인합니다. 예상 밖 항목은 지우지 않습니다.
4. Scheduler 전체 폴더의 hidden 포함 XML, Win32_Service 및 Win32_SystemDriver 경로·계정, 모든 일반 프로세스의 executable/command line을 원문 저장 없이 검사합니다. root 이름·8.3 short path·계정명·SID·시험 task 참조를 탐지합니다. PID0/4의 정확한 OS kernel pseudo-process 외에 필요한 메타데이터를 읽지 못하면 **UNKNOWN/HOLD**합니다. 일부 보호 프로세스 때문에 이 조건이 실패할 수 있으며 그 경우 안전하다고 추정해 통과시키지 않습니다. 프로세스 종료/서비스 중단/권한 우회는 하지 않습니다.
5. 모든 조건 통과 후에만 새 백업root를 **생성과 동시에 BA owner/SY group/보호 DACL(SY/BA Full만)**로 만듭니다. 정리 단발 소비 marker 역할도 하므로 재실행 시 기존 경로를 지우거나 재사용하지 않습니다.
6. CodeSnapshot에855개를 복사하고 각각 SHA·private ACL을 검증합니다. Evidence에는 pinned 원본 승인 Manifest·Install1 실패 JSON·관리자 진단 JSON·원래 ACL/시각/속성/파일 SHA/계정 상태의 inventory를 별도 보존합니다. 인증 저장소/프로필/토큰은 복사하지 않습니다.
7. 모든 상태·참조·보존을 다시 읽고 fingerprint가 같아야 정리를 시작합니다. 검증한855개 파일을 **그 파일 handle의 SHA/정확한 최종 경로를 확인한 뒤** 하나씩 삭제합니다. directory는 길이 역순으로 **비재귀·빈 경우만** 삭제합니다. 문자열 조합 경로를 다른 shell로 넘기거나 recursive delete/force/ACL 수정으로 장애를 우회하지 않습니다.
8. 계정/SID/비활성·참조·보존을 다시 확인한 뒤 **Remove-LocalUser -SID1008**만 수행합니다. 마지막으로 계정목록/읽기 가능한 부모목록/Task 부재·OnBoot/승인/Worker 보존을 읽어 최종 판정합니다. 새 계정/설치/Task/Worker는 만들거나 실행하지 않습니다.

참조 검사에는 Restart Manager를 사용하지 않습니다. [해당 리소스 등록](https://learn.microsoft.com/en-us/windows/win32/api/restartmanager/nf-restartmanager-rmregisterresources)은 registry write를 동반하므로 이번 읽기 전용 사전검사 대신 COM/CIM/읽기 handle 검사로 구성했습니다. 모든 과정에서 원문 XML/명령 인수·서비스 암호/token을 출력하거나 복사하지 않습니다.

## 대표 직접 실행 — 관리자 PowerShell7에서 한 번

아래 명령은 **별도 -NoProfile 자식 PowerShell**에서 SHA 검증한 정리 도구만 실행합니다. 현재 대화형 창을 종료하는 exit는 없습니다. 암호/토큰 입력도 없습니다. 기본 Action은 Inspect이며, 승인된 실제 정리를 명시한 아래 Cleanup+Confirm 조합에서만 변경할 수 있습니다.

```powershell
& 'C:\Program Files\PowerShell\7\pwsh.exe' -NoProfile -NonInteractive -Command '& { $cleanupBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\cleanup-partial-trial.ps1"); if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($cleanupBytes)).ToLower() -ne "4d2725591b67eb8d744299a6fe8e15fbad8069a0dc72725323b1b208c606ce29"){throw "CLEANUP_SHA_HOLD"}; & ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($cleanupBytes))) -Action Cleanup -ConfirmApprovedCleanup }'
```

**CLEANUP_COMPLETE_NO_INSTALL**만 정리 성공입니다. HOLD/오류면 반복 실행하거나 backup/소비 marker를 삭제하지 말고 정제 JSON을 인계합니다. 작업브랜치 결과는 `Evidence/Session0TrialCleanup20261027/<request_id>.json`, 실제 backup이 생성된 경우 관리자 보호 root에도 Result JSON을 남깁니다. logical_exit_code=2/0은 JSON과 세션 LASTEXITCODE이며 새 child의 실제 OS ExitCode와 구분합니다.

## 실패·복구 및 새 패키지 설치 가능 여부

조회/불일치 실패는 삭제0입니다. 백업 이후 중간 실패라면 남은 자산·삭제 수를 기록하고 HOLD합니다. 파일 정리 실패 시 계정 삭제 단계로 가지 않으며, 백업은 보존하고 자동 복원·재시도하지 않습니다. account 삭제가 실패하면 Code root는 정리됐더라도 Disabled 계정과 백업을 남겨 별도 복구 판독합니다.

현재 정리 **실행 전**이므로 새 패키지 설치 가능 여부는 미확정이며 기존 부분 설치 gate를 우회하지 않습니다. 정리 PASS 후에도 새 LSA package Installer79194…/Manifest33ed…/ZIPd678…는 **미설치·설치 미승인**입니다. 다음 실제 설치는 대표의 별도 승인과 새 fresh/SHA 검증이 필요합니다. 이번 도구에는 새 패키지 실행/계정 생성/배치권한 부여/Task 등록/IPC/Fake/AI/usage/Discord 경로가 없습니다.

## 운영/인계

지금 실제 정리·백업·새 계정·Task·권한 변경0, 기존 봇/아트/부팅/서비스계정/인증/SQLite/Unity/게임/Save/Push/main/PC 변경0, MANUAL/A HOLD 유지. 실제 정리 후 상태는 관리자 결과를 받은 뒤 갱신하며 현재 성공으로 주장하지 않습니다.

브랜치 LOCAL_COMMITTED/REMOTE_NOT_PUSHED, 실제 commit은 Issue66 및 CURRENT_STATUS/CODEX_LATEST 마감 참조. 증거 `Evidence/Session0TrialCleanup20261027/{OfflineTests.txt,SourceSHA256.json,ce050715…json}`. **현재는 관리자 실행 결과 대기이며 새 설치는 자동으로 진행하지 않습니다.**
