# Discord V2 독립 관리자 Bootstrap 구현·관리형 모의 검증

2026-10-31. **IMPLEMENTATION_PASS_MANAGED_MOCK_SCOPE / 59 UNIQUE PASS / 0 FAIL / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 보고·로컬 HEAD `871ee2c97e471fd18e3ad4e7fe99cc6fc38c5e02`. 브랜치 `codex/discord-v2-protected-bootstrap-20261031`. 최초 구현/57건 코드 `b6c9786ffc7d7f9e2a1e62d237443ee65b433de2`, backend 결합 강화/추가2건을 포함한 최종 코드 **`5a105bcadedd8f75dfe0c864def4f42dfc921ecf`**.

## 구현 범위

기존 배포 도구와 코드를 공유하지 않는 `Tools/DiscordV2/ProtectedBootstrap/`를 추가했습니다. NativeBatch·독립 배포 도구·Probe/Supervisor·Native binding은 변경하지 않았습니다.

| 파일 | 역할 |
|---|---|
| BootstrapPolicy.cs | 고정 6경로, Bootstrap 전용 승인/부모 snapshot/관리자·이미지·시각 검증, 단계별 생성 게이트 및 owner/DACL/최종 경로/HANDLE/volume readback 정책 |
| WindowsBootstrapHost.cs | 읽기 전용 HANDLE pin과 CreateDirectoryW(SECURITY_ATTRIBUTES) 어댑터. 기존 ACL 변경·파일 쓰기/복사/삭제·Task/계정 API 없음 |
| Program.cs | --review / --create 전용 진입. 보호 승인 파일 경로/원문 SHA만 입력. 일반 deploy/Task/force/cleanup 모드 없음 |
| ProtectedBootstrap.csproj | 독립 `Limitless.ProtectedBootstrap` 제품 프로젝트 정의. 향후 NativeAOT 대상이며 이번에 제품 build/publish하지 않음 |
| Tests/Program.cs, Tests/Bootstrap.Tests.csproj | 별도 managed test DLL·메모리 host. Windows backend/제품 Main을 생성·호출하지 않음 |

고정 생성 대상은 아래 **6개만**입니다. 실행 인자/환경변수로 바꿀 수 없으며 승인 Targets의 내용·순서도 정확히 같아야 합니다.

```text
C:\ProgramData\Limitless
C:\ProgramData\Limitless\DiscordV2
C:\ProgramData\Limitless\DiscordV2\Control
C:\ProgramData\Limitless\DiscordV2\Control\DeploymentTool
C:\ProgramData\Limitless\DiscordV2\Control\Approvals
C:\ProgramData\Limitless\DiscordV2\Control\DeploymentLedger
```

## 생성 순간 보호·실제 변경 경계

1. strict Bootstrap 승인 JSON/원문 SHA, 새 ApprovalId, `DISCORD_V2_PROTECTED_BOOTSTRAP_V1`, 정확한 도구 SHA·관리자 SID·OwnerSid(BA 또는 SYSTEM)·6경로·C루트/ProgramData의 보안 descriptor SHA 및 file key·만료/cutoff를 검사합니다. 이번에 실제 승인 파일은 만들지 않았습니다.
2. 실제 host는 thread token 부재 확인(조회 불명도 HOLD), Administrators 활성 역할 및 TOKEN_ELEVATION을 검사합니다. 자기 EXE와 승인 파일은 대상 신규 경로 밖의 기존 관리자 보호 위치여야 하며, 원문을 읽는 HANDLE·부모 HANDLE을 계속 보유해 write/delete 공유를 허용하지 않습니다. 자기 이미지 SHA도 재검증합니다.
3. C 고정 NTFS, 부모 reparse/최종 경로/owner/적용 위험 ACE·승인된 descriptor hash/file key·volume을 검증합니다. 모든 대상이 부재여야 합니다. 파일/폴더/reparse/조회 불명·권한 거부·선점 중 하나라도 있으면 쓰기 전에 HOLD합니다.
4. Windows host와 gate가 **동일 객체**에 결합돼야 합니다. 각 `CreateDirectoryW` 바로 전에 토큰·자기 이미지·부모·기존 생성 결과·대상 부재·현재 시각/취소를 다시 검사하고 단계를 시도 상태로 고정합니다. 단순 bool/임의 callback/환경변수로 게이트를 열지 않습니다.
5. `ConvertStringSecurityDescriptorToSecurityDescriptorW`로 아래 descriptor를 만들고 **CreateDirectoryW의 SECURITY_ATTRIBUTES에 처음부터 전달**합니다. 생성 후 기존 객체에 ACL을 덧씌우는 경로는 없습니다.

```text
O:BAG:SYD:P(A;;FA;;;SY)(A;;FA;;;BA)
```

승인 OwnerSid가 SYSTEM이면 owner 부분만 SYSTEM으로 사용합니다. 실제 OS가 해당 owner 생성을 허용하는 관리자/SYSTEM 컨텍스트인지 확인해야 하며, 도구는 소유권 탈취·특권 활성화·기존 권한 수정을 하지 않습니다. owner 설정 실패는 그대로 HOLD입니다.

6. 생성 직후 non-inheritable·공유 read-only HANDLE로 열고 최종 경로·디렉터리 종류/reparse 없음·file ID/volume·owner/group·protected DACL·정확한 SY/BA Full 두 ACE(상속 flags 없음)·HANDLE 상속 없음 readback을 수행합니다. Worker/Users/Authenticated Users ACE를 추가하지 않습니다. 생성된 HANDLE도 종료까지 보유하고 다음 생성 전/최종 단계에서 재검증합니다.
7. 기존 항목/경합은 오류이며 채택·overwrite·retry하지 않습니다. Native 오류·취소·readback 실패·중간 결과 변조 시 이후 생성은 멈춥니다. 부분 생성된 보호 폴더를 자동 삭제하거나 ACL을 복구하지 않습니다. 같은 gate의 Native 시도는 실패/응답 유실 후에도 재시도할 수 없습니다. 성공/부분 생성 후 재실행도 기존 대상 검사에서 거부합니다.

이 도구에는 배포 파일 복사, Task/계정 변경, 기존 파일/ACL 수정, 원장 운영 binding 발급, 영구 승인 소비·이전 기능이 없습니다. ApprovalId는 Bootstrap 요청 결합용이며 기존 배포 원장/승인을 소비하거나 재사용하지 않습니다. 실제 운용의 일회성 실행 기록/감사 절차는 실행 승인에서 별도로 확정해야 합니다.

## 검증 결과와 증거 범위

- 최초 관리형 컴파일 **exit0 / 경고0 / 오류0**, 신규 **57 PASS / 0 FAIL**.
- 정적 검토 후 gate와 backend 동일 객체 결합을 강화하고 그 변경만 **2 PASS / 0 FAIL**, 최종 코드 관리형 컴파일 **exit0 / 경고0 / 오류0**.
- **고유 신규 59 PASS / 0 FAIL**. 앞선 57건을 재실행하거나 합산 중복하지 않았고 기존 24·36·34·기타 완료 시험도 반복하지 않았습니다. 57건과 후속2건의 코드 SHA/입력 SHA/로그는 단계별로 보존했습니다.

표적 내용: 정상 고정6경로·초기 명시 SD/owner readback, review 무변경, 권한/승인/도구 SHA/기한/상위 SHA/file ID·scope 거부, 모든 기존 대상 보존, 부모 DELETE/DELETE_CHILD/reparse/HANDLE 상속/owner 거부, 선점·부모 교체·조회 중 만료/관리자 권한 상실/취소 차단, 생성 후 owner/DACL/최종 경로/reparse/HANDLE/volume 오염 시 HOLD, 부분 실패 보존·재시도 거부, 기존 객체 불변, 모드/force 거부, 다른 host의 승인 증명 결합 거부입니다.

**모두 메모리 모의 검증입니다. WindowsBootstrapHost 생성/실행·CreateDirectoryW 호출·제품 Main 실행·실제 C 보호 폴더/ACL 변경은 0회**입니다. SDK로 test DLL만 빌드/실행했고 제품/NativeAOT 발행과 EXE 실행은 하지 않았습니다. 테스트 코드를 NativeAOT 이미지/실제 Windows 동작 검증으로 승격하지 않습니다. SDK 최초 실행의 워크로드 안내가 있었지만 설치/업데이트하지 않았습니다.

## 관리자 실행 방식 및 추가 승인 조건

현재 도구 EXE/새 SHA는 없습니다. 관리형 모의 PASS는 실제 생성 승인 실행 준비 완료가 아닙니다.

1. 별도 승인으로 이 도구의 NativeAOT 빌드·새 이미지 SHA/PE·네이티브 진입/생성·검증 코드 포함·경고·신뢰/V3 판정을 확인해야 합니다. 실제 Windows API/owner·DACL·HANDLE readback은 **NOT_VERIFIED**입니다.
2. 실행본 및 승인 파일을 놓을 **기존 관리자 전용 보호 staging 위치와 그곳의 안전한 봉인 배치**를 별도 확정해야 합니다. 생성 대상 아래나 개인 사용자 소유 작업 폴더에서 자기 이미지 검증을 우회하지 않습니다. 현재 F 작업브랜치/E 빌드 원본을 그대로 관리자 실행해도 된다는 뜻이 아닙니다. 이번 파일 복사/배치 0회입니다.
3. 실제 적용 직전 정확한 새 Bootstrap 승인 ID·관리자 SID·도구 SHA·owner·6경로·부모 SD SHA/file key·유효기간을 확인해야 합니다. 보호된 승인 자료의 원문 SHA를 별도 검토해야 하며, 실제 승인 파일을 이번에 발급하지 않았습니다.
4. 실행은 검토된 관리자/SYSTEM 컨텍스트의 명시적 호출만 허용합니다. 도구가 UAC 자동 상승/암호 취득/특권 활성화를 시도하지 않습니다. 향후 호출 형식은 `--review --authorization <protected-file> --authorization-sha <sha>` 및 별도 승인된 `--create`이며 **이번에는 어느 쪽 제품 Main도 실행하지 않았습니다**.
5. 신규 폴더의 실제 안전 생성/readback, 부모 SDDL 무변경, 기존 프로그램/자산 영향 확인은 다음 실제 생성 승인 단계의 필수 검증입니다. 모의 결과만으로 이를 완료 처리하지 않습니다. 이후 실제 배포·파일 복사·원장 binding/소비·Task/FAKE_ONLY 실행은 다시 별도 승인입니다.

## 기존 자산·상태 보존

| 항목 | 결과 |
|---|---|
| Supervisor | 7,559,680바이트 / `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7` 보존 PASS |
| Probe | 1,892,352바이트 / `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` 보존 PASS |
| Native binding | 두 이미지의 SourceManifest 입력 SHA와 일치·변경 0 |
| 기존 NativeBatch/독립 배포 소스 | 기준 HEAD 대비 diff0 |
| 실제 C 보호 폴더/ACL·C Limitless_Temp/E ACL·Task/계정/OnBoot/V3·Unity/Save | 변경 0 |
| 실제 승인/원장·기존 파일 수정·이미지 복사/실행·NativeAOT·Push/main 병합 | 0 |

작업 브랜치의 신규 코드/관리형 QA 산출물·문서만 작성했습니다. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 유지. cutoff **2026-11-01 23:59:59 KST** 자동 연장·이전 승인 재사용 금지. **NO_GO / MANUAL / EXECUTION_HOLD**로 검증 후 중지합니다.

증거: [Summary](Evidence/DiscordV2ProtectedBootstrap20261031/Summary.json), [57PASS](Evidence/DiscordV2ProtectedBootstrap20261031/Initial57_Delta.log), [결합강화2PASS](Evidence/DiscordV2ProtectedBootstrap20261031/HostBinding2_Delta.log), [최종 관리형 컴파일](Evidence/DiscordV2ProtectedBootstrap20261031/HostBinding2_ManagedCompile.log), [최종 입력 SHA](Evidence/DiscordV2ProtectedBootstrap20261031/HostBinding2_InputsSHA.json). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 실제 바이너리/private binding/비밀정보 업로드 없음.

### GitHub 보고 범위 및 실제 영수증

최종코드 `5a105bcadedd8f75dfe0c864def4f42dfc921ecf`, 로컬 보고/증거 `6a36c89524141da46e18072ddbfcb7d7608a413c`. 상세 Issue66 댓글은 자동 승인 검토가 내부 보안 구조·로컬 경로·승인 경계·운영 상태의 외부 공개 위험으로 거부하여 게시하지 않았습니다. 우회하지 않고 민감 설계/경로를 제거한 최소 결과 요약으로 축소했습니다.

[Issue #66 비민감 결과 요약](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6542279957) 실제 등록·전체본문재조회일치 확인. 원격에는 59PASS/0FAIL·컴파일경고오류0·실적용/제품실행없음·실환경미검증·로컬상세보존/코드미Push라는 요약만 있습니다. 상세 기술 보고가 GitHub에 게시됐다고 주장하지 않습니다. 후속 영수증은 로컬 문서뿐입니다. NO_GO/MANUAL/EXECUTION_HOLD·LOCAL_COMMITTED/REMOTE_NOT_PUSHED로 중지합니다.
