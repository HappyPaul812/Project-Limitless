# Issue #66 — r2 보호 Staging 생성·검증 PASS / 미설치

## 대표 관리자 실행 결과 판독

[Issue #66 실제 Stage 성공 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6438760726). 제공 JSON 재직렬화 저장본 SHA256 `9233a98a61d6515f369459e28b1ab2dddb56ed75172ba3fa5adc87775941c5ad`, 관리자 원본 해시 아님. 최종 로컬 문서 커밋은 Issue 후속 참조 / REMOTE_NOT_PUSHED.

**R2_STAGING_CREATED_VERIFIED_NOT_INSTALLED — 보호 Stage 생성·검증 PASS / installed=false**입니다. 대표 제공 JSON을 [AdminStageUserProvided.json](팀운영/Evidence/DedicatedV2R2Staging20261027/AdminStageUserProvided.json)에 값 동일 재직렬화했습니다. 원본 디스크 JSON 경로·실행시각·도구SHA 필드는 제공되지 않았으므로 독립 native 재조회/원본 바이트 해시 검증으로 표현하지 않습니다. 다음 표는 대표가 제공한 실제 관측 필드이며 예정 계약과 구분합니다.

| 보고 항목 | 관리자 제공 결과 |
|---|---|
| Bundle | `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\3effaac30bf44760986fee9e6019b283\Bundle` |
| ZIP | 같은 package의 `Bundle.zip` |
| Owner / Group | S-1-5-32-544(Administrators) / S-1-5-18(SYSTEM) |
| Bundle directory SDDL | `O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)` |
| Manifest file SDDL | `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)` |
| Directory FileID / volume | `00730000000311c1` / `ca54aa62` |
| Bundle 수 / ZIP 포함 생성시도 수 | payload766+Manifest1=767 / file_creation_attempts768 |
| 전체 metadata·SHA / 정확byte | all_file_metadata_sha_verified=true / exact_bytes_verified=true |
| r1 보존 / 생성범위 | r1_directory_metadata_preserved=true / writes_restricted_to_r2=true |
| 계정 / Task / 승인 / IPC·AI / retry | 모두0 |

3SHA와 Package ID도 고정 r2 값과 일치합니다. 디렉터리/FileID·SDDL은 위 관측값이며 전체파일 metadata/SHA·link/ADS/reparse/path·원본과복사본 exact byte 검증은 고정 도구의 성공 반환·명시된 true 필드로 판독합니다. 개별767개 raw inventory를 추가 제공받았다고 주장하지 않습니다. r1 보존은 boundary 쓰기 제한·directory metadata 동일성의 보고 범위이며 r1 전체 파일을 새로 재검사한 결과는 아닙니다.

오류/close-error/부분실패 없이 성공 반환됐고 복구 필요 증거는 없습니다. **Stage1회 완료, 같은 명령 재실행·삭제·자동복구0**. Codex는 이번 판독에서 Stage/시험/Inspect를 반복하지 않고 문서만 갱신했습니다. 아래 실행 명령은 당시 절차 이력으로 보존합니다.

**다음 실제 설치는 별도 승인 필요**: 새 승인ID/소비기록·계정2/Batch권리·보호CodeStateAuth·Task1 범위와 설치직전 Stage 무결성/권한 재검증 절차. 신규SID/유효토큰/GROUP/Session0IPC/Job독점·V1/MANUAL/A/runtime HOLD 유지, 실제계정/Task/승인JSON/설치/로그인/IPC/FakeWorker/AI/usage/Discord/Push0. 설치·시작 없이 중지합니다.

## 이하: Stage 준비·실행 안내 당시 이력

[Issue #66 실제 준비 상태 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6438597021), 최종 로컬 커밋은 Issue 후속 참조 / REMOTE_NOT_PUSHED.

2026-10-27 KST. **준비 PASS: 신규 메모리25 PASS/0 FAIL + 실제 도구 Inspect PASS. 실제 Windows Stage 실행0**입니다. 대표 직접 실행 결과를 받기 전 생성·Owner/DACL 검증 완료로 표시하지 않습니다. r1은 RETIRED_EVIDENCE_ONLY이며 Preflight/Stage/Resume/Install을 실행하거나 자산·승인·소비 기록을 변경하지 않습니다.

브랜치 `codex/discord-usage-diagnostics-20261026`, 기준/시작HEAD `1b9b054b511fbbd899759b6eaaafad2464facf8f`. r2 후보/기존 r1 소스·패키지는 변경하지 않았고, 별도 R2Staging 도구만 추가했습니다. 기존151/Preflight12/Stage10/native생성8 검증은 반복하지 않았습니다.

## 고정 r2 계약과 경로 분리

| 항목 | 고정값 |
|---|---|
| 원본 ZIP | `F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2.zip` |
| Installer SHA | `83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f` |
| Manifest SHA | `7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976` |
| ZIP SHA | `4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0` |
| Package | `3effaac30bf44760986fee9e6019b283` |
| 새 생성 대상 | `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027\3effaac30bf44760986fee9e6019b283` |
| Bundle / ZIP | 새 대상 아래 `Bundle` / `Bundle.zip` |
| 향후 설치 승인 ID | `dedicated-install-3effaac30bf44760986fee9e6019b283` (이번 생성0) |
| epoch / IPC | `20261027-dedicated-3effaac30bf44760986fee9e6019b283` / `LimitlessDedicatedV2-3effaac30bf44760986fee9e6019b283` |

고정 r2 Manifest의 staging_root 계약은 공통 보호 부모 `C:\Program Files\LimitlessRelayDedicatedV2Staging20261027`입니다. 이를 바꾸거나 새 SHA를 임의 승인하지 않습니다. **신규 생성 경로는 r2의 새 GUID 하위뿐**이며, r1의 `a5af3087b78b45d1baae46067d5b7ac9` 하위를 사용하지 않습니다. 공통 부모의 Owner/ACL은 읽고 검증만 하며 새 r2 자식 entry가 추가됩니다. 부모 ACL을 수정하거나 r1 파일/폴더를 새 후보로 덮어쓰지 않습니다. 향후 승인/소비 ID도 새 package에 결합하되 이번 도구는 승인 경로에 쓰기 자체가 불가능합니다.

## Stage 전용 최소 도구

`Tools/DiscordAIServiceDiagnostics/R2Staging/stage-r2-only.ps1`은 기본 Inspect / 명시 Stage 두 분기만 있습니다. Install/계정/Task/LSA/암호/승인 JSON 분기가 없습니다. 외부 인수로 ZIP·SHA·Package ID를 바꾸지 못하게 고정했고, ZIP 전체 → Manifest/assembly → 정확 payload766+Manifest1/767 entries·각SHA·Installer SHA → package/approval ID/epoch/pipe/staging_root 일치까지 검증합니다. package와 path 이름만 바꿔 기존 r1 wrapper를 실행하지 않습니다.

`R2StageBoundary.cs`는 IV2StageFs 생성 호출을 **새 r2 경로**에만 허용합니다. 공유 부모 자체·r1 하위·승인 경로·경로별칭/상대탈출·ADS·UNC·유사접두사로 Directory/File 생성하면 delegate native 호출 전에 R2_STAGE_WRITE_SCOPE로 HOLD합니다. 고정 r2 archive가 검증한 C# 구현을 로드하며 외부 boundary도 별도 SHA pin으로 메모리에서 검증·컴파일합니다.

| 실행 코드 | SHA256 |
|---|---|
| stage-r2-only.ps1 | `87509891aa464805d3b86e3a4e1bb3969d5a14a99f77c2500205d3eee550431c` |
| R2StageBoundary.cs | `7203eec5d26fe02b18fb9379e1ad12cb36fb8fea7169a58c6987e012f4cf3664` |

Stage는 관리자 PS7 x64·기존 보호 부모·r1 package directory의 실제 handle/NTFS/path/Owner/Group/보호DACL·새r2대상 부재를 검사하고, source 및 부모 전체를 다시 검증한 뒤 r2만 CREATE_NEW합니다. Source/file/dir의 reparse/hardlink/ADS·최종path·FileID/volume·pending 상태를 검사합니다. **생성 시 SECURITY_ATTRIBUTES 제한DACL**, 각 entry의 metadata/SHA·정확 tree·ZIP readback 후 원본과 모든 복사본을 **SequenceEqual byte 비교**까지 수행합니다. 전체 생성은767 Bundle파일+별도ZIP=768파일이며 디렉터리는 별도입니다.

고정 descriptor는 dir `O:BAG:SYD:P(A;OICI;FA;;;SY)(A;OICI;FA;;;BA)`, file `O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)`입니다. Owner BA S-1-5-32-544, Group SYSTEM S-1-5-18·BA/SY만 full write/delete/DACL 권한을 갖는 계약입니다. 실제 성공 JSON에는 관측 Owner/Group·Bundle/file SDDL·Bundle FileID/volume·전체검증/byte일치 여부를 반환합니다. **현재는 예정 계약이며 실제 관측값은 아직 없습니다.**

r1 package directory handle을 유지하고 path/FileID/volume/SDDL/stream/flags metadata stamp가 끝까지 같은지 확인합니다. 생성 callback 자체가 r1 쓰기를 거부하므로 r1에 대한 내부 생성 호출은0입니다. 실제 r1 전파일SHA를 새로 조사했다고 주장하지 않으며, r1 원본 보존의 추가 근거는 write boundary·동일 directory metadata입니다.

## 검증과 제한

[MemoryTests.json](팀운영/Evidence/DedicatedV2R2Staging20261027/MemoryTests.json) **25PASS/0FAIL**: 기존 보호 부모+r1 sentinel을 가진 fake FS에서 r2 전체 생성/768파일/BA-SY 생성descriptor/정확byte·r1 sentinel보존,9개범위탈출거부·기존target거부·8개오류주입(복사오염/hardlink/ADS/reparse/writer/group/extraentry/생성실패)·ZIP/Manifest오류·r1archive혼용·복사후byte변경거부·Stage전용분기·PS구문을 확인했습니다. fake FS만 변경했고 실제 private 경로/ACL을 만들지 않았습니다.

[ArchiveInspect.json](팀운영/Evidence/DedicatedV2R2Staging20261027/ArchiveInspect.json)은 **실제 전체 entry point Inspect 실행 성공**입니다. 고정SHA sourceZIP·766payload/Manifest1·767entry·새identity·C#전체컴파일과boundary pin을 확인했으며 creation_attempted=false입니다. actual parent 권한/Owner/Native Create 성공 근거는 아닙니다. SourceSHA256.json에 실제 도구/테스트SHA를 보존했습니다. 실패한 기존 테스트 전체나 실제 r1 진단을 반복하지 않았습니다.

## 대표 직접 실행 — Stage 1회만

준비 검증 PASS이므로 아래 한 줄을 관리자 PowerShell7에서 최대1회 직접 실행합니다. 새 NoProfile 프로세스에서 메모리 SHA 확인한 코드만 실행해 기존 C#타입 잔류를 배제합니다. 대화형 부모 창을 닫는 exit는 없습니다. 암호를 묻지 않고 계정/Task/승인/설치·IPC/AI를 실행하지 않습니다.

```powershell
& 'C:/Program Files/PowerShell/7/pwsh.exe' -NoLogo -NoProfile -Command '$r2StageBytes=[IO.File]::ReadAllBytes("F:\study\codex\Project-Limitless\Temp\DiscordUsageDiagnostics20261026\Worktree\Tools\DiscordAIServiceDiagnostics\R2Staging\stage-r2-only.ps1");if([Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($r2StageBytes)).ToLowerInvariant() -cne "87509891aa464805d3b86e3a4e1bb3969d5a14a99f77c2500205d3eee550431c"){throw "R2_STAGE_TOOL_SHA_HOLD"};& ([scriptblock]::Create([Text.Encoding]::UTF8.GetString($r2StageBytes))) -Action Stage -ConfirmCreateR2StageOnce'
```

기대 결과 **R2_STAGING_CREATED_VERIFIED_NOT_INSTALLED**와 고정3eff… Bundle 경로입니다. 생성 전 불명/권한거부/기존target, 생성중오류/byte불일치/metadata·close오류는 **HOLD, 자동 재시도/삭제0**. 부분 자산과 정제된 guard/오류코드를 보존하고 동일명령 반복 대신 결과를 판독합니다. 기존 r1 자료를 삭제하거나 새 도구가 기존 부분 r2를 덮어쓰지 않습니다.

## 완료 보고 상태와 다음 조건

1. **준비PASS / 실제Stage미실행·관리자결과대기**.
2. 새경로·도구SHA·고정3SHA·별도identity 계약 확정.
3. 실제 Owner/DACL/767파일+ZIP/정확byte readback 결과는 대표 실행 후에만 판독.
4. r1 실제쓰기/도구재실행/기존승인변경0. 메모리r1sentinel보존PASS, 실제 r1 metadata 비교는 future Stage에 포함.
5. 이번 Codex 작업에서 partial stage·복구 실행 없음. 대표 실행 후 실패면 보존HOLD·복구도 별도 검토.
6. 정상생성 후에도 **r2 계정2·Batch권리·보호CodeStateAuth·Task1·새설치승인/소비의 실제 설치는 별도 승인**. 기존r1 protected root/기록은 유지하고 새ID child만 CREATE_NEW하는 절차·설치직전stage재검증 필요.
7. 신규SID/유효토큰/GROUP/Session0IPC/Job독점·V1 REFERENCE_VERIFICATION_HOLD·MANUAL/A HOLD 유지. 기존Worker1007/11·OnBoot2·봇아트·인증/CODEX_HOME·운영SQLite/Unity게임Save 보존. 실제 계정/권리/Task/설치승인JSON/로그인/IPC/FakeWorker/AI/usage/Discord/A/Push/main병합/종료·재부팅0.

최종 로컬커밋/Issue 실제보고는 후속 참조. 준비 완료 후 대표의 단발 Stage JSON을 기다리고 설치·시작으로 이어가지 않습니다.
