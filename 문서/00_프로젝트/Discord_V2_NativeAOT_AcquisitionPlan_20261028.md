# Discord V2 NativeAOT 공식 공급·설치 승인 계획

기준 Issue66 최신6458818054 / 로컬b7a5c9b1e1a4f5642a248584acc2c773d0a67057. **NO_GO / MANUAL / HOLD, 승인 대기**입니다. 계정 연장 완료 단계는 재조회·재적용하지 않았습니다.

## 이번에 확인한 사실

- 공식 NuGet registration/catalog JSON과 기존 Visual Studio Installer의 로컬 catalog만 읽었습니다. 패키지·설치 프로그램 바이너리 다운로드0, 설치·복원·발행0입니다.
- 기존 .NET SDK10.0.302를 고정하는 로컬 global.json, 사용자 전역 설정과 분리한 단일 offline feed·cache·7개 ID mapping의 NuGet.Config 준비본, 원본 소스를 보존한 격리 Build 입력5개를 준비했습니다.
- 기존 제품과 일치하는 VS18.8.2+12023.21 catalog에서 MSVC **14.51.36252**, 표시 Windows SDK **10.0.26100.8249**를 확인했습니다. SDK 내부 installer package version은10.0.26100.16이며 표시버전과 다릅니다.

## 정확한 NuGet 공급 경로·버전

아래7종은 모두 **10.0.10**으로 고정하며 현재 SDK의 기존 restore 요구를 충족합니다. 새로운 ASP.NET/WPF 기능을 추가하는 것이 아닙니다. 공식 catalog의 package dependency 추가0(ILLink net10.0 dependency group도 비어 있음), runtime-specific compiler도 아래에 포함됩니다. 다른 의존성이 발견되면 범위를 자동 확대하지 않습니다.

| 패키지/공식 nupkg URL | 다운로드 MB | 전개 MB |
|---|---:|---:|
| [Microsoft.DotNet.ILCompiler](https://api.nuget.org/v3-flatcontainer/microsoft.dotnet.ilcompiler/10.0.10/microsoft.dotnet.ilcompiler.10.0.10.nupkg) | 0.095 | 0.310 |
| [Microsoft.NET.ILLink.Tasks](https://api.nuget.org/v3-flatcontainer/microsoft.net.illink.tasks/10.0.10/microsoft.net.illink.tasks.10.0.10.nupkg) | 1.535 | 3.802 |
| [runtime.win-x64.Microsoft.DotNet.ILCompiler](https://api.nuget.org/v3-flatcontainer/runtime.win-x64.microsoft.dotnet.ilcompiler/10.0.10/runtime.win-x64.microsoft.dotnet.ilcompiler.10.0.10.nupkg) | 11.898 | 25.576 |
| [Microsoft.NETCore.App.Runtime.NativeAOT.win-x64](https://api.nuget.org/v3-flatcontainer/microsoft.netcore.app.runtime.nativeaot.win-x64/10.0.10/microsoft.netcore.app.runtime.nativeaot.win-x64.10.0.10.nupkg) | 29.433 | 79.700 |
| [Microsoft.NETCore.App.Runtime.win-x64](https://api.nuget.org/v3-flatcontainer/microsoft.netcore.app.runtime.win-x64/10.0.10/microsoft.netcore.app.runtime.win-x64.10.0.10.nupkg) | 39.890 | 98.198 |
| [Microsoft.AspNetCore.App.Runtime.win-x64](https://api.nuget.org/v3-flatcontainer/microsoft.aspnetcore.app.runtime.win-x64/10.0.10/microsoft.aspnetcore.app.runtime.win-x64.10.0.10.nupkg) | 12.758 | 29.970 |
| [Microsoft.WindowsDesktop.App.Runtime.win-x64](https://api.nuget.org/v3-flatcontainer/microsoft.windowsdesktop.app.runtime.win-x64/10.0.10/microsoft.windowsdesktop.app.runtime.win-x64.10.0.10.nupkg) | 39.156 | 99.035 |
| **합계** | **134.765** | **336.592** |

단위는decimal MB입니다. 공식 metadata의 packageSize/packageEntries 합계를 사용했고 실제 파일 수신은 하지 않았습니다. 공식 SHA512 기준값을 로컬 목록에 저장했습니다. 수신 후 SHA512·ID/버전·서명·실제 SHA256을 확인한 파일만 offline feed에 넣습니다. 서명 실패·알 수 없는 의존성은 HOLD이며 검사를 끄지 않습니다. [NuGet 공식 서비스](https://api.nuget.org/v3/index.json).

## 설치 대상과 공식 Microsoft 파일

기존 VS instance를 유지하며 component는 다음2개와 필수 의존성만 선택합니다.

- Microsoft.VisualStudio.Component.VC.Tools.x86.x64
- Microsoft.VisualStudio.Component.Windows11SDK.26100

필수 dependency closure를 x64 Windows/en-US·ko-KR 기준으로 계산한 계획은132 package node·중복제거359 payload·미해결dependency0입니다. 각 정확 Microsoft URL·SHA256·크기는 로컬 목록으로 보존합니다. 이는 실제 Installer 선택·기설치 차감을 반영한 최종 견적이 아니라 catalog 기반 추정입니다.

SDK 공식 vendor 묶음 내부에는 x86/ARM64/UWP 등 feature도 포함됩니다. 별도 Optional component를 추가하지 않지만 내부 묶음까지 없다고 표현하지 않습니다. 임의로 분해한 MSI/비공식 바이너리로 대체하지 않으며 빌드 target은win-x64입니다.

- 고정 bootstrapper: [VS18.8.2 메타데이터가 지정한 Microsoft 파일](https://download.visualstudio.microsoft.com/download/pr/58aec969-7d60-47ab-a001-285ca0c69097/f26687367d48386ce8400de02d99e264163ce9654c87cf43e24c4b8ec9c11723/vs_Setup.exe), 5,690,480bytes, SHA256 `f26687367d48386ce8400de02d99e264163ce9654c87cf43e24c4b8ec9c11723`.
- 고정 원 catalog: [Microsoft VisualStudio.vsman](https://download.visualstudio.microsoft.com/download/pr/58aec969-7d60-47ab-a001-285ca0c69097/879bcae9d39b2812075ae8ce0cfb27c6755c6bced1063454425b2add533bea67/VisualStudio.vsman), 31,191,413bytes, SHA256 `879bcae9d39b2812075ae8ce0cfb27c6755c6bced1063454425b2add533bea67`.

로컬 Installer의 가공된 catalog.json과 원 full vsman은 다른 바이트입니다. 같은 제품 version임을 확인해 계산했지만 원본 수신·hash검증 완료로 주장하지 않습니다. 승인 후 원 catalog hash/version과 선택 payload를 대조하고 불일치면 설치하지 않습니다.

## 용량과 관리자 권한

| 항목 | 다운로드 추정 | 설치·보존 공간 추정 | 관리자 |
|---|---:|---:|---|
| NuGet7종 | 134.8MB | feed+cache 압축본·전개 합606.1MB | 불필요 |
| MSVC 필수 묶음 | 602.4MB | 1,898.7MB | 설치 시 필요 |
| Windows SDK 묶음 | 536.4MB | 1,718.2MB | 설치 시 필요 |
| bootstrapper·원 catalog·channel metadata | 약37MB | 약37MB | 수신은 불필요 |
| **합계** | **약1.31GB + layout 공통 overhead** | **약5.40GB + 임시 빌드/layout overhead** | 아래 단계별 구분 |

VS 순설치 크기 추정은3.62GB이고5.40GB에는 내려받은 cache와NuGet 복제본도 포함합니다. 여유 공간은 **10GB 권장**입니다. 시스템·기존VS·격리 작업 드라이브에 나뉘므로 실제 배치는 Installer 계획에서 확인해야 합니다. 실제 download/disk delta는 기설치·캐시·언어·조건에 따라 달라지며, 예상보다 확대되면 설치 전 승인 범위를 갱신합니다.

## 승인 후 명령과 중지 경계

정확 설치 위치·관리자 명령과 단계별 실제 경로는 Git 제외 로컬 승인 문서에 보존했습니다. 공개 보고에는 보호 위치를 싣지 않습니다.

1. **공급 승인:** 정확7개 파일을 공식 URL에서 수신→metadata hash/서명/버전 확인→offline feed. 고정 VSbootstrapper/catalog를 확인한 뒤 두 component의 같은 제품version layout 준비. 이 단계도 이번에는 실행하지 않았습니다.
2. **설치 승인:** 검증된 layout의 실제 범위·최종 용량을 확인한 뒤 기존instance에 `modify --add <두component> --noWeb --noUpdateInstaller --quiet --norestart` 방식으로 추가합니다. `update/updateall/modifySettings/force`는 사용하지 않습니다. Installer 업그레이드 요구·제품version 변경·추가 구성·재부팅은 HOLD입니다. Bootstrapper의 `--wait`와 기존setup.exe의 차이를 명령에 반영했습니다.
3. **복원·발행 승인:** 고정SDK/격리source/cache를 사용한 restore 후 `publish -c Release -r win-x64 -p:NativePublish=true --no-restore`로각2개 source를 빌드하고 이미지 SHA256을 기록합니다. 일반 사용자 권한으로 수행하며 생성 프로그램을 실행하지 않습니다.

`--noWeb`만으로 모든 업데이트 메타 조회까지 차단된다고 주장하지 않습니다. 같은버전 catalog/layout과 `--noUpdateInstaller` 조건을 함께 적용하며 실패를 전체 업그레이드로 우회하지 않습니다. [공식 Installer 명령](https://learn.microsoft.com/en-us/visualstudio/install/use-command-line-parameters-to-install-visual-studio?view=visualstudio), [응답 파일 동작](https://learn.microsoft.com/en-us/visualstudio/install/automated-installation-with-response-file?view=visualstudio).

현재 native image0/SHA미발급입니다. 의존성 보충 뒤에도 source admission·관리자 등록/정리 구현 및 실제Session0/Batch/Job90초 cleanup 검증은 남습니다. 현재 Supervisor의 미완료 실행 gate도 유지합니다. 이미지 생성은 실시험 GO가 아닙니다.

이번 바이너리 다운로드·설치·복원·발행·계정 재조회/재연장·Task/OnBoot·로그온/IPC/AI/Discord·게임 변경·Push/main병합0. **승인 대기에서 중지하며 NO_GO/MANUAL/HOLD, 2026-11-01 마감을 유지합니다.**
