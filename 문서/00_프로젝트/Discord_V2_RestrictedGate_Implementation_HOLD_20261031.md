# Discord V2 제한 게이트·배포 도구 구현 후보 / 만료 경계 FAIL로 빌드 HOLD

**A_PARTIAL / SECURITY_REGRESSION_FAIL / B_NOT_ATTEMPTED / NO_GO / MANUAL / EXECUTION_HOLD.** 새 Supervisor NativeAOT 발행은 **0회**, 새 EXE/SHA는 없습니다. 확인된 보안 위험이면 빌드하지 말라는 바울 대표 지시에 따라 중지했습니다.

브랜치 `codex/discord-v2-restricted-gate-impl-20261031`, 시작 `27b3b4bfc116da5af9438ec0806195920dda929f`, 기준 준비안 e1ab9f85/Issue66 보고6539095364. 구현 후보 commit `4323359ee75b23ec8d284d781cc812dbfa55690c`, 실패 회귀·증거·보고는 후속 commit입니다. 실제 배포/실행 가능한 완료본으로 승인하지 않습니다.

## 구현한 범위

- `NativeTrialPolicy.cs`: 새 후보 TrialId `deef7539de5b4efcb30e062f9b734342`·FAKE_ONLY_EXECUTION_V1·GLOBAL UNKNOWN·noIPC/noAI·90초/1+1·정확 mode·계약/이미지 SHA·scope binding·계정·새 동작승인·현재 cutoff/만료를 검증하는 순수 정책입니다. **후보 ID는 실행 승인서가 아닙니다.** 실제 계약/승인/신뢰진술 발급0입니다.
- `AdminController.cs`/`AdminWindows.cs`: 실제 어댑터를 ExecuteNative로 분리하고 기존 strict 계약·신뢰자료/이미지/계정/직접Batch·Task/receipt 검증 후 첫 쓰기 전 새 정책을 적용합니다. 모의 Execute 경로를 실제 CLI에서 선택하는 옵션은 없습니다.
- `ReleaseAdmission.cs`: 계약 전의 process/thread/token Hardening을 읽기 검증 이후로 재배치했습니다. 재개 전에도 동작/계정/시각을 재확인합니다. 기존 영구 소비·Job/자식 제한·실패 정리 코드는 유지합니다.
- `ProtectedDeployment.cs`: 보호된 배포승인 원문/manifestSHA·후보ID/고정SID·경로·만료·허용파일명·원본SHA·상위검사·대상 부재를 검증하고, 쓰기 전에 별도 배포승인을 영구소비하는 조정기입니다. CREATE_NEW·기존경로채택/덮어쓰기/자동rollback 금지입니다.
- `ProtectedDeploymentWindows.cs`: 관리자 보호 승인 읽기·원본pin/해시·명시초기owner/DACL의CreateDirectoryW·CreateFileW CREATE_NEW·복제후SHA/DACL/최종경로/비상속HANDLE readback 구현 후보입니다. 실제 backend는 **한 번도 호출하지 않았고**, CLI 배포 모드도 연결하지 않았습니다. 실제 Windows 적용/유효권한은 미검증입니다.
- `RestrictedGateTests.cs`: 새 정책·관리자 모의 흐름·배포 모의 backend만 검증합니다. 기존52/17/Windows7/관리형3 시험 재실행0입니다.

## 발견한 차단 위험 — 등록용 암호 입력 후 만료 재검증 없음

실제 기존 `TaskSchedulerNative.Register`는 승인/현재시각 검증을 마친 뒤 **암호 입력을 무한 대기할 수 있고**, 입력 완료 후에는 취소 여부만 확인한 채 COM 등록 호출로 진행합니다. 새 제한 경계를 이 상태로 열면 대기 중 승인 만료/cutoff를 넘은 다음에도 실제 등록이 진행할 수 있습니다. 이전 전역false 때문에 닫혀 있던 경로를 새 경계로 열 때 확인한 보안 위험입니다.

실제 암호/콘솔/Task를 사용하지 않고, 모의 Register 직전에 시각을 계약만료+1초로 전진시켰습니다. 기대는 거부·등록호출0이나 관측은 **rejected=False / registration_calls=1**입니다. 검증기는 이를 **0 PASS / 1 FAIL**로 반환했습니다. 실제 Windows 쓰기0, Native Main0입니다. 실패 증거를 삭제하거나 PASS로 바꾸지 않았습니다.

후속에는 암호 입력 종료 뒤 **실제 COM/Windows 변경 직전** 현재시각·동작승인·신뢰자료/계정·계약 결합을 다시 검증하는 경계와 지연/유실/만료 모의 시험이 필요합니다. Enable/Run/Stop/Remove 등 실제 변경 지점도 같은 관점의 검토가 필요하며, 이미 승인된 자기Job의 fail-safe 종료/자기Task 재비활성화를 막지 않아야 합니다. 이번에는 그 후속 구현·실Windows 동작으로 이어가지 않습니다.

## 안전한 현재 상태

기존 **ReleaseAdmissionImplemented=false**를 그대로 두었습니다. 더해 `RequireWindowsBoundary()`가 명시적으로 `HOLD_NATIVE_MUTATION_DEADLINE_REVIEW`를 발생시키도록 하여 **실제 Main 진입과 실제 배포 backend의 읽기/소비/생성/복제를 닫았습니다**. 단순true/환경변수/강제옵션으로 여는 경로가 없습니다. 순수/모의 정책 테스트만 진행할 수 있습니다. 미승인 ID뿐 아니라 후보 ID도 현재 실제 실행 불가입니다.

이 HOLD 가드는 보안 위험을 해결한 것이 아니라 노출하지 않도록 닫은 상태입니다. 이를 제거하거나 NativeAOT 발행·배포로 진행하는 것은 현재 결과로 승인되지 않습니다. 기존 승인/소비기록 재사용·초기화0, cutoff2026-11-01 23:59:59KST 및 계정만료11-02 00:00KST 변경0·자동연장0입니다.

## 변경 범위 검증 결과

| 검증 | 결과 | 범위 |
|---|---|---|
| 최종 관리형 테스트 DLL 컴파일 | **경고0/오류0** | UseAppHost=false / NativePublish=false / PublishAot=false |
| 새 제한 정책·관리자모의·HOLD 가드 | **36 PASS** | 다른ID/profile/mode·변조/누락·만료/cutoff·계정/권한불명·중복승인·Run응답유실·인가전부작용차단 |
| 새 배포 조정기 모의 경계 | **22 PASS** | 미승인/만료/변조·경로탈출/중첩·기존파일/루트·부모/원본거부·부분실패/소비보존 |
| 등록 대기 중 만료 거부 회귀 | **FAIL1 / PASS0** | 미해결·빌드차단 근거, Windows쓰기0 |
| 새 Supervisor NativeAOT | **NOT_ATTEMPTED / HOLD** | 조건B 미충족·시도0·새SHA없음 |
| 실제 도구 배포·Batch4/Session0/Job90초 | **NOT_EXECUTED / NOT_VERIFIED** | 승인범위 밖·실제어댑터차단 |

새 경계의 합계는 **58 PASS + 차단 FAIL1**이며 “필수 테스트 모두 통과”가 아닙니다. 58PASS는 모의/순수 경계에 한정되고 실제 Window 보안이나 배포 PASS를 뜻하지 않습니다. 테스트 DLL만 dotnet으로 실행했으며 Probe/Supervisor EXE를 실행하지 않았습니다.

초기 검증 설정은 오래된global.json의SDK10.0.302 때문에 컴파일 전 실패했습니다. 설치된 기존SDK10.0.403으로 **새 E 테스트 루트만** 정정했습니다. 초기 관리형 컴파일의 nullable 경고2건은 명시적null 거부로 수정했고 최종0입니다. 초기로그·57PASS/0FAIL 단계·최종58PASS·별도차단FAIL을 모두 보존했습니다. 초기결과가 최종차단을 대체하지 않습니다.

격리 검증 루트는 `E:/LimitlessBuildDependencies/NativeAOT20261028/RestrictedGate20261031-1356d187a3d149439c85d22949262745`이며 실제binding 대신 X드라이브·합성SID의기존TestBinding을사용했습니다. 외부패키지/설치·새다운로드0, 기존offline feed·설치reference만사용했습니다. 원시SDK/빌드 로그는 이 루트에 보존하고 정제된 PASS/FAIL 로그는 관련Evidence에 기록합니다.

## 기존 이미지·V3·남은 승인

두 기존 이미지는 바이트·크기 보존PASS입니다.

- Probe SHA `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`, **1,892,352bytes**. Probe 소스/NativeInterop/실물 재빌드·수정0입니다.
- 기존 Supervisor SHA `CCA8202417DD4D3A1E4F0D301143E9960EEFC3B984EAC79FC92DC6FA08974ECD`, **7,536,640bytes**. 재발행0, 기존 이미지 이동/패치0입니다.

새 Supervisor가 없으므로 **새NativeAOT 경고/PE/V3 결과는 해당 없음/미검증**입니다. 기존 새이미지공식V3정상판정NOT_VERIFIED·전역SIDUNKNOWN을유지합니다. 백신종료/예외/새검사/cloud전송/복원샘플접근0입니다.

다음은 확인된 실제 변경 직전 만료/권한 경계의 보완과 회귀검증에 대한 **새 지시**가 필요합니다. 이 FAIL 상태에서는 조건부B를 자동재개하지 않습니다. 이후 통과하더라도 새Supervisor단발빌드/봉인·사용자이미지승인, 정확보호배포/실제도구검증, 신뢰사실/새trial·동작승인, Task등록·단발시작·정리 각각은 별도 단계입니다. 실제Windows·계정/ACL/Task/OnBoot·Session0·IPC/AI/Discord업무·UnitySave·Push/main병합 변경/실행0으로중지합니다.

코드·실패회귀·정제보고는 LOCAL_COMMITTED / REMOTE_NOT_PUSHED입니다. 완료/배포가능 표기나 가짜신뢰진술을 발급하지 않습니다. 관련CODEX_LATEST/CURRENT_STATUS를갱신했습니다. 코드후보4323359e·실패회귀/보고commit `398aa71163246b4fbf01c38c735f38b3f7d5a42a`, [Issue66 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6539490315) 실제등록·전체본문재조회일치 확인. 후속영수증은문서만변경하며 차단FAIL·B미시도·실제진입HOLD를유지합니다.
