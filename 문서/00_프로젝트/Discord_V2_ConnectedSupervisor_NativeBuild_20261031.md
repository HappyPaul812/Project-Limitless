# Discord V2 FAKE_ONLY 연결 완료 Supervisor NativeAOT 단발 빌드

2026-10-31. **BUILD PASS / NATIVE_INCLUSION_STATIC_PASS / NO_GO / MANUAL / EXECUTION_HOLD**.

## 기준·입력

Issue #66 최신 연결 보고와 최종 인계 HEAD `ad58fbe2d051fd417ed616a00c9e69996162ff62` 확인. 실제 코드 `4a3221b48a1f5df7530fb156e2eb058532075f61`가 HEAD의 선행이며 Supervisor 소스 diff 0입니다. 이번에는 제품 소스·보안 계약을 수정하지 않았습니다. 브랜치 `codex/discord-v2-connected-supervisor-build-20261031`.

Main의 TrialEntryGate.Dispatch → 관리자 AdminWindows.Run / 감독 ReleaseAdmission.Acquire → Supervise 연결과 전역 `ReleaseAdmissionImplemented=false` 확인. Main의 무조건 RequireWindowsBoundary 호출 부재, strict 계약/정확 TrialId/FAKE_ONLY_EXECUTION_V1/동작별 승인/계정/이미지/신뢰/만료/cutoff 검사 구현 유지. 미승인·변조·만료 거부에 관한 기존 **36 PASS 및 34 PASS** 증거를 보존하고 시험은 재실행하지 않았습니다.

E격리 루트: `E:\LimitlessBuildDependencies\NativeAOT20261028\SupervisorConnected20261031-f3e1426a2564435682ab72aec14ad2b6`.

SDK 10.0.403 고정, 기존 MSVC/Windows SDK와 공식 NuGet 7종 10.0.10 재사용. 7종 SHA256/크기 및 기존 공식 서명 검증 결과 대조 PASS. 새 격리 packages/temp/home/cache/obj/bin/output 사용·offline feed 외 restore source 없음. MSBuild 정적 평가 PASS: net10.0-windows / PublishAot=true / runtime·compiler·linker10.0.10 / TargetLatestRuntimePatch=false. 기존 MSB4191 metadata 조건식 없음.

현재 소스·projitems·프로젝트·비공개 binding 총 **23개** 원본→복제 SHA 일치 및 발행 후 양쪽 SHA 불변 확인. 비공개 binding은 Git/보고에 실물 포함하지 않았습니다. 공개 입력/설정 SHA와 비공개 전체 manifest SHA는 증거에 기록했습니다. 실제 승인/소비/계약을 생성·재사용하지 않았습니다.

## 단발 발행 결과

- CreateNew 단발 marker/flush 후 **2026-10-31 15:22:08 KST** 시작. Supervisor publish **1회**, 종료코드 **0**. 자동 재시도 **0**, Probe publish **0**.
- 새 SHA256: **`6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7`**.
- 크기 **7,559,680바이트**, MZ/PE signature·**PE32+ AMD64 0x8664 / optional 0x20b / CLR directory 없음** 확인.
- 출력: 격리 루트의 `artifacts/Supervisor/Supervisor.exe`.
- 발행 로그 경고 **0**, **IL2091 0**, 오류 **0**. 이번에는 후처리 JSON까지 정상 완료했습니다.
- 생성 EXE 실행 **0**. 경고 0은 모든 경로의 런타임 안전성 증명이 아닙니다.

## 최종 EXE 네이티브 포함 — 정적 PASS

기존 dumpbin으로 새 NativeAOT object의 COFF 심볼·역어셈블을 읽고, COFF `.managedcode$I`의 함수 코드 prefix를 **최종 PE의 실행 가능한 `.text` 섹션** 바이트와 직접 대조했습니다. 일반 함수 96바이트(재배치 필드 마스킹), 관리자 짧은 thunk는 실제 본문 9바이트이며 각각 최종 PE 내 유일한 일치를 확인했습니다. 함수 전체의 의미적 동작을 증명한 것은 아닙니다.

**21개 함수 포함 PASS**:

- Program.Main, 두 Main 분기 lambda, TrialEntryGate.Dispatch/Validate.
- NativeTrialPolicy.Shape/Authorize, AdminWindows.Run, AdminController.ExecuteNative/ExecuteCore.
- ReleaseAdmission.Acquire/Prepare, Program.Supervise, TaskMutationBoundary.Validate.
- TaskSchedulerNative.Register/Enable/Run/Stop/Remove, JobMonitor.Run/Drain.

추가로 최종 PE의 실제 call/jmp 명령과 상대변위를 계산하여 **4개 분기 PASS**:

| 분기 | 명령 raw offset | 대상 raw offset |
|---|---:|---:|
| Main → Dispatch | 58140 | 30224 |
| 관리자 lambda → AdminWindows.Run | 151316 | 107312 |
| 감독 lambda → ReleaseAdmission.Acquire | 151439 | 73904 |
| 감독 lambda → Program.Supervise | 151627 | 58480 |

Main 역어셈블에 Dispatch 호출이 있고, 기존 무조건 throw 함수 호출은 없습니다. 따라서 3,172,352바이트 이미지의 **Main 무조건 throw → 경로 제거** 문제는 재발하지 않았습니다. 승인 거부를 위한 조건부 throw와 별도 보호 배포 backend 봉인은 정상적으로 유지합니다. 네이티브 코드 포함/분기 확인은 **실제 Windows 실행·등록·Batch4·Session0·Job90초·자식제한·실패정리·IPC 없음 등의 런타임 입증이 아닙니다**.

## 기존 이미지 보존

| 이미지 | SHA256 | 바이트 | 결과 |
|---|---|---:|---|
| Probe | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` | 1,892,352 | 보존 PASS |
| 기존 Supervisor | `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD` | 7,536,640 | 보존 PASS |
| 직전 HOLD Supervisor | `34D2B90A0EDE2BBA1BB8A848A80F30890B80ABB7463EE0118A35395B8BA5AB16` | 3,172,352 | 보존 PASS·실행 후보 아님 |

발행 전후/후속 SHA·크기 일치. 기존 EXE 수정·이동·실행·재발행 **0**. 새 이미지도 생성 직후·10초 후·후속 SHA 일치입니다.

## V3 관측·잔여 HOLD

V3 엔진 읽기 확인 **2026.10.31.01**. 새 이미지 삭제·변조 관측 없음. quarantine DB 메타데이터는 12,288바이트/10-28 18:00:50 유지이며 이것으로 무탐지를 주장하지 않습니다. 과거 문의 #745042는 제출 당시 샘플에 한정됩니다. **새 SHA의 공식 정상 판정·실제 V3 진단 결과 NOT_VERIFIED**. 백신 종료/예외/우회/복원/새 검사·GUI 조작 없음.

1. 새 SHA에 대한 바울 대표 승인과 신뢰/V3 확인 필요. 과거 샘플/이미지 승인 자동 승계 금지.
2. 보호 CodeRoot/EvidenceRoot 배포·소유권/비상속 DACL·부모 교체 방지·최소 Worker 권한·복제 SHA/readback은 별도 관리자 승인 대상. 이번 적용 0.
3. 현재 후보 TrialId가 실제 승인된 것은 아닙니다. 정확한 새 이미지 SHA/계정/경로/신뢰근거가 결합된 신규 계약 및 Register/Start/Stop/Remove 동작별 승인·영구 소비 부재 확인 필요. 기존 승인·소비 재사용/초기화 금지.
4. 계정/자격증명/다른 launcher/보호자산 관리의 대표 GUI 확인 필요. Windows 전체 동일 SID 부재 UNKNOWN. 새 바이너리 실제 동작·FAKE_ONLY 단발 시험은 **별도 승인 및 실행 검증 전 NOT_VERIFIED**.
5. cutoff **2026-11-01 23:59:59 KST**, 기존 계정만료 확인값 **2026-11-02 00:00:00 KST** 유지·자동 연장 금지.

실 EXE/보호 배포/ACL/Task/계정·암호·OnBoot/Session0/IPC·AI·Discord 업무/Unity·Save 변경 **0**. 전역 gatefalse·NO_GO/MANUAL/EXECUTION_HOLD 유지. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 보존, Push·main 병합·원본 동기화 없이 보고 후 중지합니다.

증거: [Summary](Evidence/DiscordV2ConnectedNativeBuild20261031/Summary.json), [Native 포함/PE 분기](Evidence/DiscordV2ConnectedNativeBuild20261031/NativeInclusion.json), [Main/Dispatch/lambda 역어셈블](Evidence/DiscordV2ConnectedNativeBuild20261031/MainDispatchDisassembly.txt), [발행 로그](Evidence/DiscordV2ConnectedNativeBuild20261031/Publish.log), [입력 SHA](Evidence/DiscordV2ConnectedNativeBuild20261031/BuildInputs.json), [설정 SHA](Evidence/DiscordV2ConnectedNativeBuild20261031/BuildSettingsSHA.json). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 바이너리·비공개 binding·비밀정보 업로드 없음.
