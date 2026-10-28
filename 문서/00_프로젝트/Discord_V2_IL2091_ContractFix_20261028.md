# Discord V2 IL2091 생성자 보존 계약 수정 — 관리형3 PASS / NativeAOT NOT_VERIFIED

2026-10-28 / 기준c7085cf4bdd4804784cd0f25d166ac132c900f22·Issue #66. 바울 대표 제공 상태: 안랩 고객기술지원 #745042에 Probe.exe 샘플 제출 완료, 공식 악성/오진 판정 대기. 이는 대표가 수행한 일이며 Codex 추가 전송은0입니다. NO_GO / MANUAL / HOLD, 2026-11-01 마감 유지.

## 근본 원인과 최소 수정

이전 NativeAOT10.0.10 로그는 Native.Info<T>의 T가 Marshal.PtrToStructure<T>의 PublicConstructors/NonPublicConstructors 보존 요구를 전달하지 않는다고 IL2091을 보고했습니다. where T:struct는 값 형식 제약일 뿐 트리머에 필요한 멤버 보존 선언이 아닙니다. [Microsoft IL2091 설명](https://learn.microsoft.com/en-us/dotnet/core/deploying/trimming/trim-warnings/il2091)은 호출 대상의 요구를 제네릭 원본 매개변수에도 선언하도록 설명합니다.

NativeInterop.cs의 Info<T> 형식 매개변수에 **DynamicallyAccessedMembers(PublicConstructors | NonPublicConstructors)**를 추가했습니다. 보존 마스크는7이며 All로 넓히지 않았습니다. 현재 실제 호출 형식은 int와 TokenStats로 정적 고정돼 있습니다. 구체 형식별 helper로 분리하거나 버퍼를 unsafe 재해석하는 방법보다 기존 helper에 정확한 보존 계약을 전달하는 수정이 작고 네이티브 동작을 유지합니다.

필드·StructLayout·Pack·P/Invoke 선언·Marshal.SizeOf/PtrToStructure·버퍼할당/해제·토큰 query·반환길이 조건·실행 흐름은 그대로입니다. 경고 무시/NoWarn/분석기 비활성화/RequiresUnreferencedCode/회피용 API 교체·난독화0. 안랩 진단이나 탐지 기준을 바꾸려는 수정이 아닙니다. 기존 소스/빌드/제출 샘플 증거는 덮어쓰지 않았으며 이번 변경 이후 source는 제출 샘플과 별개의 revision입니다.

## 변경 부분만의 검증

| 검증 | 결과 | 범위 |
|---|---|---|
| 보존 계약·값형식 제약·RUC 미사용 | 관리형 PASS | 실제 Info<T> 메타데이터에서 정확 constructor mask7 확인 |
| int 배치·버퍼 복원 | 관리형 PASS | 크기4·비대칭 비트값 복원 |
| TokenStats x64 배치·필드 복원 | 관리형 PASS | 크기56/LUID8·전체 필드 오프셋·고유 byte pattern 값 확인 |
| 기존 시험 프로젝트 관리형 compile | PASS/경고0·오류0 | 신규 선택 진입점3개만 실행 |
| Supervisor·Probe DLL 전용 compile | 각 PASS/경고0·오류0 | PublishAot=false/NativePublish=false/UseAppHost=false, exe 생성·실행 안함 |
| IL2091 정적 진단 소멸 | NOT_VERIFIED | 수정 전 baseline/의도된 진단 대조군도 일반 관리형 분석에서 IL2091을 내지 않아 분석 효과를 인증하지 못함 |
| NativeAOT IL2091 소멸·링크·런타임 | NOT_VERIFIED | 재발행 금지, native 분석/코드생성/실행하지 않음 |

총 **변경 관련 관리형3 PASS/0 FAIL**. 테스트는 기존 runner의 --interop-trim-contract 경로이며 Windows 보안 API를 호출하지 않고 메모리 마샬링/메타데이터만 검사합니다. 기존52·17·Windows7 PASS는 재실행0. 다른 ABI 구조체 전체 시험도 반복하지 않았습니다.

int=4, TokenStats=56, LUID=8. TokenStats 필드 TokenId/AuthenticationId/Expiration/Type/Impersonation/Charged/Available/Groups/Privileges/ModifiedId 오프셋은0/8/16/24/28/32/36/40/44/48입니다. LUID의 Low/High=0/4. 구조체 선언은 diff상 무변경입니다. 관리형 합성 버퍼 결과는 실제 Windows 토큰/Session0 ABI 재검증을 대신하지 않습니다.

초기 Probe --no-restore 일반 compile은 오래된 obj의 과거 NU1100 오류9개를 다시 표시했습니다. 새 네트워크 restore 실패나 새 source 컴파일 오류로 분류하지 않았습니다. 별도 새 obj와 packageSources clear 설정으로 설치된 reference만 해석한 DLL 전용 build는 성공했습니다. 원래 실패 assets·안랩/빌드 증거를 지우지 않았습니다. 추가 다운로드/설치0.

기존 공식 ILLink Roslyn analyzer10.0.10을 읽기 연결하고 분석 config에서 IL2091을 error로 지정하는 확인도 했으나 대조군이 반응하지 않았습니다. 그러므로 경고0 로그를 IL2091 소멸 증거로 사용하지 않습니다. 소스의 누락된 계약 수정과 관리형 테스트 완료만 보고하고, NativeAOT 전용 경고 해소는 NOT_VERIFIED로 남깁니다. 임의의 분석기 회피나 native 재발행으로 확인을 강행하지 않았습니다.

## 안랩 회신 이후 남은 절차

1. #745042의 공식 판정을 먼저 보존·검토합니다. 현재 cloud미확정/공식 판정 대기이며 이번 수정이 기존 Trojan 진단을 해소했거나 샘플이 안전하다고 주장하지 않습니다.
2. vendor가 추가 자료를 요구하면 공개 범위를 먼저 확인하고 대표가 별도로 승인해야 합니다. 복원된 Probe.exe/제출 Probe.zip 접근0, 샘플 재생성·실행0, ARC/추가로그 수집·자동전송0.
3. 공식 검토와 별도 빌드 승인 후에만 변경 revision·입력SHA를 새로 봉인하고 NativeAOT IL2091/다른 분석 경고·링크·PE/SHA를 확인합니다. 이전 Probe 샘플 판정이 새 binary의 자동 안전/실행 승인은 아닙니다. 보안검사/예외·V3 설정으로 우회하지 않습니다.
4. 실제 단발 실행은 계정/보호자산 신뢰 근거·정확 이미지/계약·동작별 등록/시작/정리 승인, 제한 Batch token/Session0/HANDLE 지원·Job90초·실패 정리 증거가 별도로 필요합니다. 전역 동일SID 부재 UNKNOWN 및 운영 계약은 유지합니다.

이번 새 NativeAOT Publish0/실행0·기존누적Probe1/Supervisor0 유지, gate=false. 계정·Task·ACL·OnBoot·업무IPC/AI/Discord·Unity/Save 변경0, Push/main 병합0. 생성관리형DLL도 실행한 것은 신규 테스트 runner뿐이며 Probe/Supervisor Main은 실행하지 않았습니다. 빌드 임시·결과는 새 Git제외 F검토 폴더이며 E의 분석기 패키지 외 restored sample/zip은 접근하지 않았습니다.

소스 변경: NativeInterop.cs. 변경 시험: Tests/InteropTrimContractTests.cs와 기존 runner/프로젝트 선택 경로. 브랜치 codex/discord-usage-diagnostics-20261026, 마지막관련c7085cf4bdd4804784cd0f25d166ac132c900f22;이번 최종SHA 완료보고/Issue66. LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 11-01까지 실동작 미검증이면 자동연장/재시도 없이 증거·인계 보존 후 게임 개발 복귀. 작업 후 중지합니다.