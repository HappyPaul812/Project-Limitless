# Discord V2 안랩 회신 후 NativeAOT 빌드 전용 결과

**PARTIAL_BUILD_ONLY / PROBE_FAIL / SUPERVISOR_BUILD_PASS / NO_GO / MANUAL / EXECUTION_HOLD.** 실행 게이트 `ReleaseAdmissionImplemented=false` 유지. 이번 승인은 빌드·정적 확인뿐이며 생성 EXE 실행0입니다.

## 공식 회신과 엔진 반영 확인

바울 대표 제공 사실: 안랩 문의 **#745042**는 제출 Probe.exe를 테스트용 프로그램으로 확인하고 진단 제외 조치를 결정했으며 **V3 엔진2026.10.30.00** 반영 예정이라고 회신했습니다. 이는 대표가 받은 공식 회신의 기록이며 Codex가 메일/제출 샘플을 열거나 새로 전송한 것은 아닙니다. 새 소스/바이너리 전체에 대한 정상 판정으로 확대하지 않습니다.

빌드 전에 V3의 읽기 전용 HKLM `AhnLab/V3Clinic` 값 **AscVersion=2026.10.31.01**, AscVersionEx=`3.30.2.3 (Build 38421)`을 확인했습니다. 제품의 `asc/nosinfo.txt`도 `ver=2026.10.31.01`로 일치하고 LastUpdate는 **2026-10-31 10:13:13 KST**입니다. 반영 예정 엔진 이후의 설치 업데이트를 확인한 다음에만 발행했습니다. 제품 버전4.17.0.9·파일 시각만으로 엔진 반영을 추정하지 않았습니다. 업데이트 실행·V3 중지·검사 예외·복원·정책 변경0입니다.

## 소스·공급망·격리

- 브랜치 `codex/discord-v2-aot-resume-20261031`, 시작 `564e74e9109d4f75588e2c491219e4a8d3d63696`. 구현/IL2091 기준 `fe5b22e60a3c505961cead1b7b80089194f0a44a`를 그대로 사용했습니다.
- Native.Info<T>의 PublicConstructors|NonPublicConstructors 보존 선언과 실행gate=false를 확인했습니다. tracked 구현 소스 변경0, source20파일 복사 SHA 봉인, 기존 ignored Probe/Binding/project 입력은 앞선 공급망 검토 SHA와 일치합니다. 복원된 옛 Probe.exe/제출 ZIP 접근·실행0입니다.
- 기존 공식 NuGet7종10.0.10의 nupkg SHA256/크기와 기존 공식 서명검증exit0 기록을 대조했습니다. 새 다운로드0, 공급원은 기존 검증 offline feed1개입니다. SDK10.0.403·기존 MSVC를 사용했고 전역 설치/설정 변경0입니다. 과거 VS 카탈로그 실패를 PASS로 바꾸지 않았습니다.
- 새 격리 출력은 `E:/LimitlessBuildDependencies/NativeAOT20261028/NativeResume20261031-7ed1a2ca6f724242a49884cba87f5360`입니다. NuGet cache·obj/bin·TEMP·dotnet-home·발행 출력은 이 루트에 제한했고 두 publish 시작 표식을 CreateNew로 남겼습니다. 이 위치는 배포 위치나 실행 승인 경로가 아닙니다.

## 각 1회 시도 결과

| 대상 | 횟수·결과 | SHA256·크기·아키텍처 |
|---|---|---|
| Probe | **1회 / FAIL**, exit1. 컴파일 전 MSB4191 | 새 EXE 미생성, SHA/크기/PE **해당 없음** |
| Supervisor | **1회 / PASS**, exit0. NativeAOT compile/link/publish 완료 | SHA256 `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD`, **7,536,640 bytes**, **PE32+ AMD64 (0x8664)**, CLR directory 없음 |

Supervisor 실물: 위 격리 루트의 `artifacts/Supervisor/Supervisor.exe`. 바이너리/PDB/source/원시 로그를 GitHub에 올리지 않았습니다. SHA와 PE 검사는 파일을 데이터로 읽은 것이며 프로그램 실행이 아닙니다.

### Probe 초기 실패와 처리

Codex가 재사용한 준비 스크립트의 Directory.Build.targets에 오래된 `KnownILCompilerPack.TargetFramework` 사용자 메타데이터 조건이 남아 **MSB4191**이 발생했습니다. 이전 실제 발행 루트의 수정 완료 targets 대신 이전 준비 스크립트를 사용한 오류입니다. V3 탐지/차단이나 IL2091 실패가 아닙니다. 이 시점에는 컴파일/복원/native 이미지 생성에 도달하지 않았습니다.

초기 targets를 `.INITIAL_FAILED`로 보존하고, 이전 실제 NativeAOT 발행에서 사용한 targets(동일 공식10.0.10 고정, 해당 잘못된 조건 없음)로 격리 설정만 바로잡았습니다. **Probe는 횟수 제한에 따라 재시도하지 않았고**, 아직 시도하지 않은 Supervisor만 최초1회 진행했습니다. 최초 실패 로그·Probe 시작 표식·결과를 지우거나 Supervisor 성공으로 대체하지 않았습니다.

Supervisor publish 로그의 warning/IL2091은0입니다. 수정된 generic 보존 선언을 포함한 실제 Supervisor NativeAOT 경계에서 IL2091이 나타나지 않았다는 근거이며, 실패한 Probe의 NativeAOT 경고 소멸/링크 성공이나 실행 검증을 뜻하지 않습니다. 기존52/17/Windows7/관리형3 시험 전수반복0입니다.

## V3 진단 결과의 정확한 범위

Supervisor 생성 직후 정적검사, **10초 후 SHA 재검사**, 추가 정적 확인에서도 EXE 존재/크기/SHA가 유지됐습니다. 관측 범위에서 새 출력의 실시간 삭제·변경은 없었습니다. Probe는 새 이미지가 없어 새 바이너리 진단 대상이 없습니다. 재탐지를 확인했다면 즉시 중단할 정책을 유지했으며 예외/우회/복원은 하지 않았습니다.

**새 Supervisor의 V3 공식 정상/미탐지 판정은 NOT_VERIFIED**입니다. 파일이 남은 사실을 정상 인증으로 바꾸지 않습니다. 읽기 가능한 Windows AhnLab 이벤트 공급자를 찾지 못했고, 기존 독자형 V3 로그/DB의 기록 부재를 진단 부재로 해석하지 않았습니다. 새 수동 검사·클라우드 분석 요청·샘플 전송·UI 포커스 조작도 하지 않았습니다. 이번 결과는 “출력 삭제 미관측”과 “정식 진단 확인 미완”을 분리합니다.

## 남은 HOLD / 실제 FAKE_ONLY 시험 조건

1. **Probe 빌드 미확보**: 수정 완료한 준비 설정으로 새 Probe 시도1회를 추가하려면 별도 승인이 필요합니다. 이번 승인 횟수를 넘겨 재시도하지 않습니다.
2. 두 최종 이미지 SHA·PE·현재 V3 결과·대상 의존성·보호 배포 신뢰 근거를 확보해야 합니다. 이번에는 Supervisor 하나만 확보했으며 계정/보호 자산의 신뢰 사실을 합성하거나 서명하지 않았습니다.
3. 기존 FAKE_ONLY 한정 위험 수용 승인은 보존하지만 실제 동일SID 비협조 코드 부재·자격증명 비공유·다른 실행경로 미사용·보호 자산 관리 근거를 대신하지 않습니다. 운영 전역 독점성은 **UNKNOWN**입니다.
4. 기존 cutoff **2026-11-01 23:59:59 KST**와 유한 계정·새 승인 유효기간의 정합성 확인, 정확 이미지/동작에 묶인 신규 단발 승인·Task 등록/시작/정리 각각의 허가가 필요합니다. 옛 소비/승인 재사용·초기화·만료 자동연장 금지입니다.
5. 실제 Batch4/Session0·제한token HANDLE 지원·Job 내 단일 대상/child 제한/90초/취소·실패 정리 검증은 별도 승인입니다. 실행gate 개방이나 운영 Task·계정·ACL 변경은 이번 범위 밖입니다. 업무 IPC·AI 호출·Discord 실제 업무 재개 승인이 아닙니다.

현재 **NO_GO / MANUAL / HOLD, gate=false**로 중지합니다. main·Unity·개인Save/UserData·운영 Task/계정/ACL/OnBoot·Session0·IPC/AI 변경/실행0, GitHub Push·main 병합0. 안랩 회신과 빌드 결과가 게임 개발 복귀 일정을 자동 변경하지 않습니다.

## 증거와 인계

원시 source 봉인/정확한 private binding·전체 로그·초기 targets/발행 표식/결과는 기존 worktree의 ignored `Temp/NativeBatchPreparation20261028/NativeResume20261031` 및 새 E 격리 루트에 보존했습니다. 샘플/격리물은 이 증거에 복사하지 않았습니다. 공개 가능한 요약은 `Evidence/DiscordV2VendorBuild20261031/Summary.json`에 있습니다. 관련 CODEX_LATEST/CURRENT_STATUS/NativeBatch README를 갱신했습니다. 보고 commit `9227601ccc92b32db15e8a935a48bd64e954e8ee`, [Issue66 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6538682825) 실제 등록·본문 전체 재조회 일치 확인. 후속 영수증은 문서만 변경하며 **LOCAL_COMMITTED / REMOTE_NOT_PUSHED**를 유지합니다. 원본 main ab47b931/Git상태344항목 유지 확인, gate=false·실행0·Push0입니다.
