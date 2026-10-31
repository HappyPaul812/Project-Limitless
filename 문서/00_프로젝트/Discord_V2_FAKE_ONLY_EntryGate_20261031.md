# Discord V2 FAKE_ONLY 최종 진입 게이트 연결·모의 검증

2026-10-31. **CODE_IMPLEMENTED / DELTA_36_PASS_0_FAIL / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 댓글 및 `5cb5512cd2ec9b50800cab3ead500c546663e4e8`. 작업 브랜치 `codex/discord-v2-fake-only-entry-gate-20261031`. 실제 코드 **`4a3221b48a1f5df7530fb156e2eb058532075f61`**. 원본 main/게임/Save 변경 없이 격리 작업했습니다.

## 크기 차이의 정적 원인 — 작은 이미지 실행 후보 부적합

| 이미지 | 바이트 | SHA256 |
|---|---:|---|
| 기존 Supervisor | 7,536,640 | `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD` |
| 직전 HOLD Supervisor | 3,172,352 | `34D2B90A0EDE2BBA1BB8A848A80F30890B80ABB7463EE0118A35395B8BA5AB16` |
| 기존 Probe | 1,892,352 | `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` |

모두 원위치 SHA/크기 보존 PASS. EXE 실행·수정·이동 및 재빌드 0.

기존 MSVC dumpbin으로 보존된 두 NativeAOT `Supervisor.obj`의 COFF 심볼과 작은 object의 역어셈블을 읽었습니다. 작은 object에는 Main/RequireWindowsBoundary 본문이 있고, **AdminWindows.Run / Program.Supervise / ReleaseAdmission.Acquire / TaskSchedulerNative.Register 본문 심볼은 없습니다**. 큰 object에는 모두 있습니다.

작은 Main은 무조건 예외를 던지는 RequireWindowsBoundary 호출 후 trap, catch에서 70 반환으로 끝납니다. 실제 작은 EXE에서도 Main의 고유 prologue가 raw offset **8032**에 1회 존재하고, 해당 함수+0x91의 call 상대변위가 raw offset **7152**의 throw 함수로 이어지며 이후 trap/catch-return70 바이트가 object 역어셈블과 일치합니다. 따라서 단순 심볼 이름 부재만으로 추측한 결론이 아닙니다. 무조건 throw 이후 관리자·감독 경로가 도달 불가하여 NativeAOT가 해당 본문을 생성하지 않은 것을 확인했습니다. 모든 4,364,288바이트 차이를 개별 함수별로 산정한 것은 아닙니다.

직전 BUILD PASS는 생성/PE/SHA 확인이지 실행 게이트 연결 PASS가 아닙니다. **3,172,352바이트 이미지는 실제 FAKE_ONLY 실행 후보로 사용하면 안 됩니다.** 큰 이미지 역시 현재 수정 코드와 동일하지 않아 대체 실행하면 안 됩니다. 새 구현의 NativeAOT 포함·실행 가능성은 **NOT_VERIFIED**입니다.

## 진입 연결 및 승인 전 부작용 차단

- `Program.cs`: 운영용 `ReleaseAdmissionImplemented=false` 유지. 모든 모드 앞의 무조건 throw를 제거하고 `TrialEntryGate.Dispatch`로 정확한 모드/계약 경로/SHA/인자 수만 분기합니다. Dispatch 자체는 실행 승인이 아니며 후속 보호 검증 없이는 변경에 도달하지 않습니다. 강제 옵션·환경변수 경로·일반 운영 모드 없음.
- `TrialEntryGate.cs`: 관리자/감독이 공유하는 읽기 승인 검증. strict 계약 원문 SHA/범위·정확 후보 TrialId `deef7539de5b4efcb30e062f9b734342`·FAKE_ONLY_EXECUTION_V1·GLOBAL UNKNOWN·noIPC/noAI·90초/1+1·계정 binding·두 이미지 SHA·신뢰 증표/근거 SHA·freshness·현재 시각/만료/cutoff·동작별 ApprovalId/operation/ContractSha를 검사합니다. 이미지/보호 경로/실계정은 실제 어댑터 읽기 검증을 거치며, 고비용 조회 후 신뢰/현재시각/취소를 다시 검사합니다. 후보 ID나 CLI만으로 승인하지 않습니다.
- `AdminController.cs`: 기존 ExecuteNative/TaskMutationBoundary 검증 유지. Register/Start/Stop/Remove의 각 intent 영구 생성 직전에 현재 계약/최초와 동일한 동작 승인 원문 SHA/신뢰/실계정/이미지/만료를 추가 재검증합니다. 승인 전 거부 시 intent/소비/Task 변경 모두 0이며 HOLD 기록도 새로 쓰지 않습니다. intent 이후 실패는 HOLD 증거를 보존하고 자동 재시도하지 않습니다. 실제 COM 직전 기존 34PASS의 재검증 코드는 무변경입니다.
- `ReleaseAdmission.cs`: 보호 경로/파일 pin·이미지·시작 등록 증표·신뢰·계정·위험 HANDLE 읽기 검사 → 전체 ValidateEntry → `.supervisor-once` CREATE_NEW/flush → 재검증 → 자기 process/thread/token DACL hardening → 재검증 순서입니다. 중복 소비는 DACL 변경 전에 거부합니다. 실패 후 소비를 지우지 않습니다. Supervisor는 Register 승인이 아니라 정확한 Start 승인과 start-intent/instance 증표를 요구합니다.
- `AdminWindows.cs`/projitems: 실제 관리자 어댑터 연결/설명과 새 공통 검증 소스 포함. 별도 보호 배포 backend의 무조건 HOLD는 유지합니다. Job/자식 제한/자기 안전 종료·Task 변경 직전 만료 경계·Probe는 무변경입니다.

동작 경계는 `--register-only` / `--start-once` / `--stop-owned-instance` / `--remove-owned-task`로 분리되며 Supervisor만 Start 승인을 공유합니다. Stop/Remove는 정확한 자기 Task/instance/소유 증표와 새 cleanup 동작 승인이 필요합니다. 유효 시작 lease의 자기 Task 안전 재비활성화와 기존 자기 Job 실패 종료는 유지됩니다.

## 이번 검증

- **36 PASS / 0 FAIL**: Main과 같은 Dispatch·공유 Validate 및 ExecuteNative+fake backend만 실행. 허용/잘못된 모드·인자·경로·TrialId·profile·SHA·동작승인·만료/cutoff·신뢰변조·계정거부·조회 중 만료/취소·영구 intent 전 계약 변경·등록/시작/정리 분리·중복 Start 및 실제 AdmissionConsumption의 메모리 CREATE_NEW 충돌·실패 후 소비 유지/자기 disable 확인.
- 정적 연결 **8항목 PASS**: gatefalse, Main 공통분기, Main 무조건 throw 부재, 실제 관리자 restricted 진입, Supervisor consume 전 전체 읽기 검증/consume 후 DACL 순서, 4개 intent 재검증, 승인 전 HOLD쓰기 없음, 배포 backend 봉인.
- 관리형 시험 DLL 컴파일 경고/오류 **0/0**. 시험에 사용한 복제 소스와 현재 소스 전체 SHA 일치. SDK 최초 실행의 도구 PATH/워크로드 안내는 발생했으나 설치·업데이트는 하지 않았습니다.
- 기존 표적 **34 PASS/0 FAIL 기록 보존**, 34·58·과거 전수 시험 재실행 0. 새 36건의 수정 후 최종 결과만 집계하며 중간 반복 결과를 합산하지 않습니다.
- NativeAOT 발행 **0**, Supervisor/Probe EXE·Native Main 실행 **0**, 실제 승인 소비/보호폴더/ACL/Task/계정/OnBoot/Session0/IPC/AI/V3 예외/UnitySave 변경 **0**. 관리형 테스트만 실행했습니다.

## 후속 조건과 중지

**새 Supervisor 재빌드 필요**: 코드가 기존 두 이미지에 없으므로 별도 단발 빌드 승인 후 새 SHA/PE/경고/NativeAOT 실제 진입 본문 포함 여부를 다시 확인해야 합니다. 이번 단계에서는 빌드하지 않았습니다. NativeAOT 컴파일/트리밍/실제 Windows 실행/배포·ACL·Task 생명주기·Batch4·Session0·Job90초·자식제한·실패정리는 NOT_VERIFIED입니다.

새 이미지의 V3 신뢰 확인·정확 SHA에 대한 바울 대표 승인, 보호 CodeRoot/EvidenceRoot 배포 및 관리자 ACL readback 승인, 계정/자격증명/다른 launcher/보호자산 GUI 확인, 새 계약·신뢰근거·등록/시작/정리별 일회성 승인과 소비 부재 확인, 실제 FAKE_ONLY 단발 시험의 별도 승인이 필요합니다. 기존 샘플 정상 판정이나 과거 승인/소비를 재사용할 수 없으며 Windows 전체 동일 SID 부재는 UNKNOWN입니다.

cutoff **2026-11-01 23:59:59 KST**, 기존 계정만료 확인값 **2026-11-02 00:00:00 KST** 유지. 자동 연장/소비 초기화 금지. **NO_GO / MANUAL / EXECUTION_HOLD**로 중지하며 Push/main 병합/원본 동기화는 하지 않습니다.

증거: [Summary](Evidence/DiscordV2EntryGate20261031/Summary.json), [36PASS](Evidence/DiscordV2EntryGate20261031/Delta36PASS.log), [이미지/COFF 비교](Evidence/DiscordV2EntryGate20261031/StaticImages.json), [작은 Main 역어셈블](Evidence/DiscordV2EntryGate20261031/HeldMainDisassembly.txt), [관리형 입력 SHA](Evidence/DiscordV2EntryGate20261031/ManagedInputsSHA.json). 로컬 커밋/REMOTE_NOT_PUSHED, 바이너리·비공개 binding·비밀정보 업로드 없음.
