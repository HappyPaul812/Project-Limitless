# Discord V2 실행 차단2건 조사 — 공개 상태 보고

기준62be493ce9e709c780c0c2d7d4a81ad2338ee8f4. **NO_GO / MANUAL / HOLD**. 기존 경로만 조사했으며 새 아키텍처·검증기·모의시험·발행 재시도는 없습니다. 보안상세·정확수집명령은 Git제외 로컬자료에만 보관합니다.

## A. Windows 상태 — 관리자 조회 필요

계정 현재 활성/만료, 암호 만료 정책·현재 만료 상태, 직접·그룹 Batch 허용/거부와 기존 Task 등록/비활성/Trigger 상태는 관리자 결과 대기입니다. 이전 비관리자 조회 실패를 계정·권리·Task 부재로 단정하지 않았습니다. 과거 만료예정시각은 대조 기준이지 현재 상태가 아닙니다.

요청된 PowerShell7 읽기전용 수집기·SHA 검증 한줄명령을 로컬에 준비했습니다. 기존 NetAPI/LSA 읽기 구현을 재사용하고 전체과거검사/모의분기를 실행하지 않습니다. 암호 최대수명/계정무기한flag/현재암호만료/계정만료를 별도필드로 기록합니다. Batch직접·local group 권리와 실제유효로그온을 구분합니다. 실제로그온은 하지않으므로 최종유효token/도메인정책은 미검증입니다.

수집기구문오류0·수집실행0. 바울대표가 기존 관리자콘솔에서직접실행하고 새고유JSON을 제공해야 현재값을판독할수있습니다. 오류는UNKNOWN/원시코드보존, 자동재시도·설정변경0.
**해결책 A 하나: 최소 관리자읽기1회 직접실행→결과판독.** 계정연장/암호변경/권리부여/Task변경승인이아닙니다.

## B. NU1100 직접 원인 — 활성 restore source0 + 패키지 미공급

기존 config와project.assets.json만 판독했습니다. 해당 publish는 준비용config를명시했고 그config의packageSources가clear여서 **활성source0**이었습니다. 격리packages폴더도부재입니다. 따라서 실패한원격feed URL은없으며 네트워크/인증/TLS장애로분류하지않습니다. Microsoft도NU1100원인으로source미설정을설명합니다. [공식NU1100문서](https://learn.microsoft.com/en-us/nuget/reference/errors-and-warnings/nu1100).

기존로그9항목을중복제거한누락7개는모두 **10.0.10** 입니다.

| 공식 패키지 | 기존 복원 요구 |
|---|---|
| [Microsoft.DotNet.ILCompiler](https://www.nuget.org/packages/Microsoft.DotNet.ILCompiler/10.0.10) | >=10.0.10 |
| [Microsoft.NET.ILLink.Tasks](https://www.nuget.org/packages/Microsoft.NET.ILLink.Tasks/10.0.10) | >=10.0.10 |
| [runtime.win-x64.Microsoft.DotNet.ILCompiler](https://www.nuget.org/packages/runtime.win-x64.Microsoft.DotNet.ILCompiler/10.0.10) | =10.0.10 |
| [Microsoft.NETCore.App.Runtime.NativeAOT.win-x64](https://www.nuget.org/packages/Microsoft.NETCore.App.Runtime.NativeAOT.win-x64/10.0.10) | =10.0.10 |
| [Microsoft.NETCore.App.Runtime.win-x64](https://www.nuget.org/packages/Microsoft.NETCore.App.Runtime.win-x64/10.0.10) | =10.0.10 |
| [Microsoft.AspNetCore.App.Runtime.win-x64](https://www.nuget.org/packages/Microsoft.AspNetCore.App.Runtime.win-x64/10.0.10) | =10.0.10 |
| [Microsoft.WindowsDesktop.App.Runtime.win-x64](https://www.nuget.org/packages/Microsoft.WindowsDesktop.App.Runtime.win-x64/10.0.10) | =10.0.10 |

## 실제 설치·캐시 조사와 추가 설치 판단

- SDK10.0.302와shared NETCore/AspNetCore/WindowsDesktop runtime10.0.10 존재. Ref/Host packs도있지만 AOTcompiler/runtime NuGetpack과는다릅니다.
- 지정격리packages·기본globalcache·SDKfallback에공급없음. HTTPcache파일0. 확인한기존로컬구성만으로는발행불가입니다. 전PC모든임의사본부재를주장하지않습니다.
- 공식vswhere로 기존VisualStudio설치를추가확인했습니다. 기본설치경로에없다는이유로미설치로단정하지않았습니다. 다만C++x64도구component조건을만족하는instance0입니다.
- WindowsSDK버전등록정보는존재하나실제headers/libs/tools가용성을확정하지못했습니다. SDK부재/사용가능어느쪽도단정하지않습니다. NU1100은링크전restore실패이므로이를C++링커실패라고보고하지않습니다.

**해결책 B 하나:** 기존SDK/VS를유지하고정확한공식빌드의존성만보충합니다. 승인후 위7개10.0.10및필요의존성을오프라인feed로공급하고 별도project전용config로사용합니다. 기존config·user/global설정은유지합니다. 기존VS에C++데스크톱개발구성의MSVC x64/x86도구와지원WindowsSDK를추가하는범위이며, 이미충족하는SDK를중복설치할필요는없습니다. [공식NativeAOT전제조건](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/).
내부runtimepack을csproj직접참조로추가하지않고SDK가요구하는패키지로공급합니다. 이번다운로드/restore재실행/설치/config·보안설정변경0.

## 승인과 남은 일

필요승인은 A관리자읽기직접실행과 B공식패키지공급·기존VS필요구성추가입니다. Windows시험자산변경·계정수정은포함하지않습니다.
이후에도남은실행admission/관리자준비정리구현→native이미지2개와SHA→최소OS준비승인→단발Session0/Batch/Job종료정리승인이필요합니다. native이미지0/SHA미발급이며실행가능판정은아닙니다.

새계정·Task/로그온·Job·IPC·AI0, 기존소비/OnBoot/게임원본변경0, Push/main병합0. 이번두차단조사와자료준비후중지합니다. **마감2026-11-01, 이후별도연장없이게임개발복귀.**

Issue66 공개 상태 게시 완료: https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6458283473 . 상세 명령·Task/ACL·보호 경로 게시0. 조사 보고 commit641c6d0672d73a7358d882aaa2504895f74b1c21. 관리자수집/다운로드/설치 미실행·NO_GO 유지.
