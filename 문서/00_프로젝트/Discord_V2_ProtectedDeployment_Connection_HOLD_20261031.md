# Discord V2 보호 배포 연결 검토 — Supervisor 변경 필요로 중지

2026-10-31. **IMPLEMENTATION_NOT_COMPLETED / HOLD / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 보고 및 HEAD `c8b2b4c7f47ce172ae3a6266e740baefbddfd8af`. 작업 브랜치 `codex/discord-v2-protected-deployment-review-hold-20261031`. 이번 변경은 문서/정적 증거뿐이며 제품 소스 변경·컴파일·NativeAOT 발행·실행·배포는 없습니다.

## 중지 근거

바울 대표의 이번 지시 6: **“새 Supervisor 수정이나 재빌드가 불가피하면 원인과 변경 범위를 보고하고 중지하세요.”**

현재 구조에서 Supervisor에 실제 보호 배포 진입을 연결하려면 다음 변경이 필요하므로 구현 전에 중지했습니다.

1. `ProtectedDeployment.cs`와 `ProtectedDeploymentWindows.cs`는 `Supervisor.Core.projitems` 소속이지만 **제품 코드의 Execute 호출 및 Windows backend 생성 경로가 없습니다**. Execute 호출은 기존 모의 시험에만 있습니다.
2. `Program.Main`은 TrialEntryGate를 거쳐 기존 관리자 Task 동작/감독 경로만 호출합니다. TrialEntryGate는 배포 모드를 허용하지 않습니다. 소스 파일이 프로젝트에 포함된 것만으로 실제 배포 기능이 연결된 것은 아닙니다.
3. 실제 Windows backend의 승인 읽기·영구 소비·폴더 생성·파일 복사 4개 경계는 `NativeTrialPolicy.RequireWindowsBoundary()`에서 무조건 HOLD입니다. 단순 guard 삭제는 허용할 수 없으며 배포 전용 승인/검증 경계로 대체해야 합니다.
4. 현재 7,559,680바이트 Supervisor의 보존 NativeAOT object에는 ProtectedDeployment/Windows의 함수 본문 심볼이 없습니다. 실제 호출 경로도 없으므로 현재 이미지로 배포 기능을 실행할 수 있다고 인정하지 않습니다. 새 진입/검증 구현을 같은 Supervisor에 넣으면 소스/네이티브 입력·새 이미지 SHA가 바뀌고 새 발행/검증이 필요합니다.

별도 독립 관리자 배포 도구를 만들면 **현재 두 이미지를 변경하지 않는 설계는 가능**합니다. 따라서 모든 방식에서 Supervisor 재빌드가 필연적이라고 주장하지 않습니다. 다만 현재 Supervisor에 연결하는 방식과 별도 신뢰 주체/도구를 도입하는 방식은 서로 다른 승인 경계입니다. 현 중지 지시를 별도 실행 도구의 자동 신설/게이트 우회 허가로 해석하지 않았습니다. 독립 도구는 이번에 구현/빌드/실행하지 않았습니다.

## 기존 후보에 있는 보호 로직과 미완료 상태

정적 확인한 후보 로직: strict manifest/승인 JSON, manifest SHA, 정확 TrialId/binding, 배포 ApprovalId/유효기간/cutoff, 기존 대상 거부, 모든 원본/상위 경로 pin 후 영구 단발 승인 소비, 명시 초기 owner/D:P DACL의 CreateDirectoryW/CreateFileW CREATE_NEW, Worker 최소 mask, 원본 read handle 유지, flush·복제 SHA/크기·owner/DACL·최종 경로 readback, 실패 시 원본/부분 증거 보존·자동 rollback/삭제 없음.

이 목록은 **소스에 있는 후보 기능**이며 실제 연결 완료/Windows 안전성 검증 PASS가 아닙니다. 향후 배포 진입을 구현할 때 관리자 상승/비위장 상태·고정 승인 파일/원문 SHA·정확한 배포 manifest·현재 계약/이미지·단발 소비·유효기간을 실제 변경 직전에 재검증해야 합니다. 상위 경로 교체 방지·유효권한·새 소유권/비상속 DACL·복제 SHA의 전체 적용 전 dry-run 및 적용 후 readback도 검증해야 합니다. 승인되지 않은 부모 ACL 수정이나 기존 증거 덮어쓰기를 허용해서는 안 됩니다.

## 이번 확인 결과

| 구분 | 결과 |
|---|---|
| Supervisor SHA/크기 | PASS — `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7`, 7,559,680바이트 |
| 승인 대기 Probe SHA/크기 | PASS — `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322`, 1,892,352바이트 |
| 정적 구조 관찰 | 6항목 PASS — 프로젝트 포함, Main 분기, 배포 모드 부재, guard4곳, 무조건 HOLD 구현, 제품 호출/생성 부재 |
| 변경 배포 경계의 신규 모의 QA | **NOT_RUN / HOLD — 코드 변경 전 중지**, 신규 PASS 0 / FAIL 0. 기존 시험 결과를 신규 성과로 합산하지 않음 |
| 기존 36·34 PASS | 기록 보존, 재실행 0 |
| 실제 보호 폴더/ACL/EXE 복사·실행/Task/계정/OnBoot | 모두 0 |

**이미지 보존 PASS와 배포 기능 HOLD는 별개**입니다. 두 파일은 승인 후 배포할 원본 후보로 보존/재사용할 수 있지만, 배포 실행기나 사용 승인으로 간주할 수 없습니다. Supervisor 내장 방식으로 바꾸면 Probe는 그대로 두고 Supervisor 새 SHA가 필요합니다. 독립 도구 방식이면 현 두 이미지 보존이 가능하나 별도 도구의 구현·신뢰·승인은 미완료입니다.

## 필요한 최소 후속 결정/승인

1. **구현 방향 결정**: Supervisor 내장 진입/보호 경계 수정→표적 모의 QA→별도 단발 NativeAOT 발행·새 SHA 승인, 또는 독립 관리자 배포 도구의 별도 코드/검증 범위 승인. 이번 단계에서는 어느 방식을 확정하거나 실행하지 않았습니다.
2. 도구 검증 완료 후 실제 배포 승인은 정확한 도구/두 이미지 SHA·크기·대상 CodeRoot/EvidenceRoot·Worker SID·TrialId·manifest/승인 원문 SHA·새 DeploymentId·유효기간·영구 소비 경로·생성/복제 허용 파일 목록에 결합해야 합니다. 기존 경로/파일/소비를 채택·삭제·초기화하거나 부모 ACL을 수정하는 승인이 아닙니다.
3. 바울 대표의 관리자 GUI 확인: 자격증명 비공유, 해당 실계정의 다른 실행 경로 미사용/보호자산 관리, 새 이미지에 대한 V3 판정. 현재는 **각각 UNKNOWN**, 새 SHA 공식 정상 판정 **NOT_VERIFIED**입니다. 설치 엔진 2026.10.31.01 확인이 새 파일의 정상 판정은 아닙니다. Windows 전체 동일 SID 부재 입증도 하지 않았습니다.
4. 실제 폴더/ACL/복제 적용과 실제 FAKE_ONLY/Task 동작은 각각 별도 승인입니다. 이번 작업은 그 승인이나 실행 준비 완료를 의미하지 않습니다.

cutoff **2026-11-01 23:59:59 KST** 유지·만료 후 자동 연장/옛 승인 재사용 금지. 실계정/보호 경로/자격증명 조회 없음, V3 설정/IPC·AI·Discord 업무/Unity·Save/원본 main 변경 없음. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 보존. Push·main 병합·원본 동기화 없이 중지합니다.

[정적 관찰 및 이미지 보존 증거](Evidence/DiscordV2ProtectedDeploymentReview20261031/Summary.json). LOCAL_COMMITTED / REMOTE_NOT_PUSHED. 바이너리·비공개 binding·비밀정보는 업로드하지 않습니다.

### 실제 GitHub 보고 영수증

보고/정적증거 로컬커밋 `df71e4b5ceaf139f2b4bef52862d63a0ed6edc9e`. [Issue #66 HOLD 보고](https://github.com/HappyPaul812/Project-Limitless/issues/66#issuecomment-6540138211) 실제 등록·전체본문재조회일치 확인. 후속 영수증은 문서만 변경합니다. 두이미지보존PASS·정적6관찰PASS·신규모의QA미실행/HOLD·제품코드수정/빌드/복사실행/실Windows변경0·NO_GO/MANUAL/EXECUTION_HOLD·LOCAL_COMMITTED/REMOTE_NOT_PUSHED로 중지합니다.
