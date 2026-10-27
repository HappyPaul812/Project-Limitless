# Issue66 — r1 native adapter 이름 충돌 확정 / r2 최소 수정 후보

[Issue #66 실제 원인·r2 후보 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6438090445). 최종 로컬 커밋은 Issue 후속 참조 / REMOTE_NOT_PUSHED.

**설치는 HOLD 유지, 직접 코드 결함을 재현해 r2 오프라인 후보를 준비했습니다.** r1 package/staging/승인/소비 기록은 보존하고 실제 Windows 변경을 하지 않았습니다. 앞선 대표의 재개 요청 범위에서 수정·검증·별도 후보 생성만 수행했습니다. r1 SHA 고정 설치 승인을 r2의 새 Manifest/ZIP에 적용하지 않습니다.

## 최신 관리자 근거와 추가 답변

대표 JSON은 native Stage.Verify·현재 승인 권한·sealed 정의 로드·metadata recheck·현재상태 reader 5개 모두PASS, 콘솔 stdin/stdout/stderr redirect=false/ConsoleHost/RawUI=true입니다. 상태 snapshot의 계정2/Task/CodeState/소비/lease는ABSENT·Result0, 보호된 r1 갱신 승인만PRESENT/expired/동일관리자/정확r1 결합, SHA `845f3f7818e0302cb5ac0cec43822860758e184b541426683f94bc8a7719b4e8`. 값 동일 재직렬화 NativeGuardAdminUserProvided.json, 원본 파일 경로/시각은 미제공입니다.

대표는 실패 당시 **Worker 암호까지 모두 입력했다**고 확인했습니다. 이 답변과 readonly PASS를 근거로 그 다음 `New-D3WindowsApi`의 실제 객체 생성 경로를 가짜 빈 SecureString으로 재현했습니다. 실제 암호를 받거나 backend 메서드를 호출하지 않았습니다.

## 직접 원인

고정 r1 `DedicatedWindows.ps1`은 note property `consume=$null`을 가진 PSCustomObject를 만든 후 ScriptMethod `Consume`을 Add-Member로 등록합니다. PowerShell 멤버 이름은 대소문자를 구분하지 않아 충돌합니다. 고정 sourceSHA를 확인한 코드에 실제처럼 ErrorActionPreference=Stop을 적용하면 정확히 다음이 발생합니다.

- System.InvalidOperationException / HRESULT **-2146233079**
- FullyQualifiedErrorId **MemberAlreadyExists,Microsoft.PowerShell.Commands.AddMemberCommand**
- 발생 위치 **Add-Member ScriptMethod Consume**, 실제 backend 호출0

따라서 현재 암호 입력 후의 실패를 재현하는 native adapter 결함입니다. 초기 자식 stdout/보안 입력 IOException, 외부 H alias 결함과 구분합니다. stage 라벨이 넓어 이번 직접 원인까지 숨겼던 문제도 기록합니다. **기존 fake 테스트는 실제 native adapter 생성까지 검증하지 못했습니다.** readonly의 함수 정의 로드PASS도 실제 객체 생성PASS를 뜻하지 않습니다.

## 최소 수정과 새 후보

저장 속성만 **consumeDirectory**로 바꾸고 그 참조를 수정했습니다. `Consume` 메서드 이름·소비 CREATE_NEW·권리·승인·ACL·보존·단발·Disabled/Trigger0·FAKE_ONLY 정책은 변경하지 않았습니다. Add-Member -Force로 덮어쓰거나 소비 검사를 제거하지 않습니다.

소스 수정: `Tools/DiscordAIServiceDiagnostics/DedicatedV2/DedicatedWindows.ps1`. 새 타깃 회귀 `test_native_adapter_constructor.ps1`. r1 대비 바뀐 payload는 정확히 `Code/V2/DedicatedWindows.ps1`1개이며 다른 payload765개는 byteSHA 동일합니다. frozen r1 폴더/ZIP·보호 staging은 덮어쓰지 않았습니다.

실물 `F:\study\codex\Project-Limitless\Temp\Session0DedicatedV2Candidate20261027-r2.zip`, 폴더는 확장자 없는 같은 경로, sidecar `.package.json`에 전체 파일SHA/원본재사용 SHA가 있습니다. payload766+Manifest1=767, source/package 바이트 pin 및 exact ZIP/SHA 검증, 기존 Installer의 **Inspect가 DEDICATED_PACKAGE_OFFLINE_PASS_NOT_INSTALLED**로 반환됐습니다. 실제 계정/Task 설치0입니다.

| 항목 | SHA256 |
|---|---|
| Installer (바이트 불변) | `83c97d781fb2d9aed797b41ea6bc799b83e939ad3ffa6f7af6a2d26f1080d68f` |
| r2 Manifest | `7c8410145ed84f06fc9901e93f78e18d0cc103ad89757172e08c84f91d8a3976` |
| r2 ZIP | `4efa0aa8a38b32fa40fa5a2dfb4042e028eeb07b1ab8d73c713a1d80efe028d0` |

새 package ID **3effaac30bf44760986fee9e6019b283**, `dedicated-install-3effaac30bf44760986fee9e6019b283`, 새 epoch/IPC를 사용합니다. 실제 SID는 아직 없으며 고정하지 않습니다. approved=false/auto_start=false/FAKE_ONLY/real_ai_enabled=false/EMPTY_NO_IMPORT_NO_LOGIN 유지. 계정명·Code/State 경로 등 기존 전용 계약은 유지하며 실제 자산 부재는 후속 설치 직전에 재검증해야 합니다.

## 신규 검증

**8 PASS / 0 FAIL + 패키지 Inspect PASS**. NativeAdapterConstructorR2Tests.json에 고정 r1 정확한 충돌 재현·수정된 실제 adapter 생성·13개 메서드 등록/속성 분리·-Force없음·정확한 최소rename·payload변경1개·새승인/epoch/pipe·fake정책·구문을 기록했습니다. 실제 backend/계정/Task/권리/IPC/Worker 메서드는 호출하지 않았으며 테스트 입력은 빈 가짜 SecureString입니다. 이전151/Preflight12/Stage10은 재실행하지 않았습니다. 새 후보의 실제 Windows 설치·유효권한/토큰·Session0/Job은 미검증/HOLD입니다.

## 다음 조건과 금지

**기존 r1 Install/Resume 명령은 더 실행하지 않습니다.** r1은 이 결함을 포함한 고정 후보로 보존합니다. r2는 Manifest/ZIP·승인ID가 달라 바울 대표의 **새 r2 보호 staging 및 설치 승인**이 필요합니다. 이번 승인/소비를 새 후보로 자동 전환하거나 만료 승인·기존 marker를 재사용/삭제하지 않습니다. 보호된 기존 staging/approval root를 재사용한다면 Owner/DACL을 검증하되 r2의 고유 package/approval/consume child만 CREATE_NEW로 분리하는 절차를 먼저 검토해야 합니다. 아직 실제 r2 staging/승인 파일은 만들지 않았습니다.

새 staging/설치 조건을 마련해도 TaskDisabled/Trigger0·BotDisabled/새Worker batch-only·빈Auth·단발·90초·재시도0·보호 readback을 유지하고 결과 후 중지합니다. 실제 토큰/GROUP/IPC/Job 독점·V1 REFERENCE_VERIFICATION_HOLD·MANUAL/A HOLD 유지. 기존Worker1007/11개·OnBoot2·봇아트·인증/CODEX_HOME·운영SQLite·Unity게임Save/승인증거 보존, 실제계정/권리/ACL/Task/새staging/승인/설치/로그인/IPC/AI/usage/Discord/Push0.

브랜치 `codex/discord-usage-diagnostics-20261026`, 시작HEAD `13bbddd6e618c621fa988170caa2613db45fd664`. 최종 로컬커밋·Issue 실제보고는 후속 참조, REMOTE_NOT_PUSHED.
