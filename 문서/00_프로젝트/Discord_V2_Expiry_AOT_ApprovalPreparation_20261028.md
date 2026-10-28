> 최신 결과: 바울 대표 직접 실행 결과의 SHA와 intent/final을 판독했습니다. 지정된 계정 유한 만료 목표와 사후 값 일치, 변경 시도1회·오류0·비교 계정 metadata9개 전후 일치를 확인해 계정 연장 단계만 완료로 기록합니다. 아래 준비·미적용 표시는 이력입니다. 추가 계정 조회·변경 및 NativeAOT 작업은 하지 않았으며 전체 NO_GO/MANUAL/HOLD는 유지합니다.

> 최신 후속: 사용자 제공 최소 읽기로 기준·실제값의 시간 정밀도 불일치가 원인임을 확인했습니다. 정확한 실관측 전체 정밀도를 고정하는 v2 준비본·새 SHA/명령은 로컬에만 보존합니다. 구명령은 사용 중지·이력 보존, 비교 검사 완화와 실제 적용은 하지 않았습니다.

> 후속: 사용자 적용 시도는 사전 일치 검사에서 중지됐으며 이번 호출의 쓰기 명령은 도달하지 않았습니다. 어느 사전 조건이 달랐는지 최소 읽기 결과를 대기합니다. 원본 기준 완화·재적용·설정 변경 없이 NO_GO를 유지합니다.

# Discord V2 계정 만료·NativeAOT 보충 승인 준비

기준 Issue66 최신6458350517 및 로컬88edfe834049b7ce66716299ae2745d6f7b92915. **NO_GO / MANUAL / HOLD** 유지입니다. 이번에는 준비 자료만 작성했으며 기존 PASS·관리자 조회를 반복하지 않았습니다.

## 계정 만료 변경 준비

바울 대표가 지정한 유한 종료 경계를 목표로 고정하고 UTC로 변환했습니다. 정확 SID·계정명·기존 만료값 일치, 실행 환경, 변경 전후 계정 metadata 대조, 새 intent/final 기록을 포함한 명령을 Git 제외 로컬 자료로 보존했습니다. SID·명령·보호 위치·실제 계정 값은 공개 보고에 포함하지 않습니다.

쓰기 cmdlet은 정확 SID에 **AccountExpires 한 속성만** 전달하며 소스 호출1개입니다. 다른 속성·암호·권리·그룹·ACL·Task 변경 API는 사용하지 않습니다. 별도 적용 승인 스위치가 없으면 조회 전에 중지합니다. 정적 구문 오류0이며 실제 계정 조회·변경은0입니다.

실제 적용 승인을 받은 뒤에만 직전값을 읽습니다. 기존값 불일치·이미 목표값·다른 상태 변화면 쓰지 않고 중지합니다. 사후 목표값 불일치·보존 metadata 변화·조회/로그 실패는 HOLD이며 자동 재시도·자동 복구를 하지 않습니다. 이 검증은 같은 계정 metadata 범위이며 전체 ACL·Task 보존 감사를 뜻하지 않습니다.

복구는 별도 승인, 정확 대상·현재 목표값·변경 전 지문과 기록 일치, 해당 계정 작업이 없다는 확인이 선행돼야 합니다. 과거 만료값 복구가 즉시 계정 만료를 일으킬 수 있으므로 실행 중/불명 상태에서 자동 복구하지 않습니다. 이번에는 변경·복구를 하지 않았습니다.

## 공식 빌드 의존성 공급 계획

기존 .NET SDK10.0.302와 기존 Visual Studio instance를 유지합니다. [앞선 보고의 공식7개 패키지](Discord_V2_TwoBlockers_20261028.md)는 **모두10.0.10으로 고정**했습니다. 로컬 공급 목록에는 공식 NuGet gallery와 V3 flatcontainer URI, 아직 미다운로드·SHA미발급·서명미검증 상태를 명시했습니다.

승인 후 공식 패키지의 ID/버전·서명·실제 SHA를 확인해 오프라인 feed로 공급하는 계획입니다. 내부 runtime pack을 임의 csproj 직접 참조로 바꾸지 않습니다. 추가 의존성이 발견되면 공식성·정확 ID/버전과 필요성을 확인해 범위를 확정하고, 임의 패키지·비공식 mirror·미검증 바이너리로 대체하지 않습니다. 기존 source0 config와 사용자/global 설정은 보존하며 별도 프로젝트 전용 config만 사용할 계획입니다.

기존 VS에 다음 **2개 component와 필수 의존성만** 보충하는 범위입니다.

| Component ID | 목적 |
|---|---|
| Microsoft.VisualStudio.Component.VC.Tools.x86.x64 | stable MSVC x64/x86 compiler·linker·표준 libraries |
| Microsoft.VisualStudio.Component.Windows11SDK.26100 | Windows SDK10.0.26100 계열 headers·x64 UM/UCRT libraries·필요 tools |

기설치 SDK 파일이 충족하면 중복 설치하지 않습니다. 등록 metadata만으로 충족 처리하지 않습니다. 전체 workload의 모든 Recommended/Optional, Preview·ARM·ATL/MFC·C++CLI·Clang/CMake·vcpkg·테스트·AI/그래픽 도구를 추가하는 범위는 아닙니다.
실제 기존 설치 채널의 component revision·필수 의존성·용량은 승인용 installer payload 계획에서 고정해야 합니다. 온라인 component 표의 revision을 기존 채널과 같다고 가정하지 않습니다. 기존 VS 업데이트·새 VS/SDK 교체가 필요하면 자동 확대하지 않고 추가 승인 대상으로 둡니다. 근거는 [공식 NativeAOT 전제조건](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)과 [공식 VS component 목록](https://learn.microsoft.com/en-us/visualstudio/install/workload-component-id-vs-community?view=visualstudio)입니다.

## 별도로 승인받아야 할 항목

1. **계정 적용:** 지정된 정확 계정의 만료 속성만 유한 연장1회, 그 변경 직전·직후 읽기와 증거 기록. 암호·권리·그룹·ACL·Task 변경은 제외합니다.
2. **공급·설치:** 공식7개 정확 버전 패키지 공급과 서명/SHA 확인, 기존 VS의 위 component 및 필수 의존성 보충. 실제 payload 계획 범위까지만 승인받습니다.
3. **복원·빌드:** 공급된 공식 의존성으로 별도 offline config에서 복원·NativeAOT 발행. 생성 프로그램 실행은 포함하지 않습니다.
4. **OS 준비·실제 시험:** 남은 admission/관리자 등록·정리 구현과 native 이미지2개·SHA가 완성된 후 별도로 승인받습니다.

현재 native 이미지0개·SHA미발급, 실제 Session0/Batch token·Job90초/강제 종료·정리 미검증입니다. 계정 연장이나 의존성 공급 승인을 실제 시험 승인으로 해석하지 않습니다.

이번 계정·Task 조회/변경0, 다운로드·설치·복원0, 실제 로그온·Job·IPC·AI0, 기존 소비·OnBoot·Discord 운영·Unity 원본 변경0, Push/main병합0. 준비 후 중지합니다. **11-01 업무 종료 후 연장 없이 게임 개발로 복귀하며, 요청된 계정 만료 경계는 개발 기한 연장이 아닙니다.**
