# Discord V2 E드라이브 고정 원장 후보 — 상위 ACL 보호 미충족 HOLD

2026-10-31. **CANDIDATE_ADOPTION_HOLD / PRIVATE_BINDING_NOT_CREATED / NO_GO / MANUAL / EXECUTION_HOLD**.

기준 Issue #66 최신 보고 및 HEAD `95c5683a07724f92128636756091c18760e370d5`. 브랜치 `codex/discord-v2-ledger-candidate-review-20261031`, 최소 시험 코드 `a025888116c7cd13dede9d0c799e7ceb35f334e3`. 제품 구현/원장 binding/기존 이미지 변경 없이 읽기 수집과 관리형 정책 시험만 수행했습니다.

## 채택 여부와 근거

후보: **`E:\Program Files\Limitless\DiscordV2\Control\DeploymentLedger`**.

- 고정 로컬 NTFS 볼륨, 경로 정규 형식 확인. 실제 비공개 Native CodeRoot·EvidenceRoot 양쪽과 중첩 없음. 실제 Native 경로는 공개 보고에서 제외했습니다.
- `E:\`와 `E:\Program Files` 두 기존 상위 경로는 SYSTEM 소유이며 수집 시 ReparsePoint 표시/LinkType은 없습니다.
- 그러나 두 경로의 **Authenticated Users(S-1-5-11) 허용 ACE `0x1301bf`에 DELETE(`0x10000`)가 포함**됩니다. E루트는 적용 허용 ACE, Program Files는 현재 개체에도 적용되는 상속 ACE입니다. 둘 다 InheritOnly가 아니므로 기존 `AdmissionPolicy.Descriptor(..., ancestor:true)`가 **ANCESTOR_REPLACE_ACCESS**로 거부합니다.
- 두 DACL은 상속 차단 상태가 아니지만, 상위 경로에서는 이 사실만으로 거부한 것이 아닙니다. 판정의 직접 근거는 적용 가능한 비신뢰 주체의 DELETE 권한입니다. 현재 mask에 WRITE_DAC/WRITE_OWNER가 있다고 주장하지 않습니다.
- `Limitless`, `DiscordV2`, `Control`, `DeploymentLedger` **4개 하위 디렉터리는 현재 없습니다**. 이들의 owner/DACL/원장 보호를 이미 확인했다고 주장할 수 없습니다.

**현재 후보를 안전한 경로로 채택할 수 없어 비공개 `DeploymentLedgerBinding.Root`를 고정하지 않았습니다.** 기존 미발급/빈 값 → HOLD를 유지합니다. 다른 옛 ApprovalRoot를 가져오거나 부모 ACL을 바꾸거나 정책을 완화하지 않았습니다. 테스트 PASS를 경로 보호 PASS로 해석하면 안 됩니다.

ACL snapshot은 읽기 수집 전후 SDDL 일치까지 확인했습니다. Worker로 로그인/위장하거나 실제 삭제·rename을 시도하지 않았으며, SID별 Windows AccessCheck/전체 권한 부재를 입증한 것은 아닙니다. 현재 ACL의 정책 위반만으로 HOLD에 충분합니다. 배포 시점의 HANDLE pin/최종 경로·유효권한 검증은 별도입니다.

## 소유권·권한 계약 대조

최종 원장 계약은 owner BA 또는 SYSTEM, group SYSTEM, DACL protected, **상속 없는 SYSTEM·Administrators Full 허용 ACE 두 개만**입니다. Worker의 변경/삭제/WRITE_DAC/WRITE_OWNER 권한은 부여하지 않습니다. 부모 교체 방지도 별도로 충족해야 하므로, 최종 leaf만 관리자 전용으로 만드는 것으로 현재 상위 문제를 해결했다고 볼 수 없습니다.

현재 두 상위 경로의 SYSTEM 소유는 PASS 관찰이지만, Authenticated Users DELETE로 상위 보호는 HOLD입니다. 부재한 4개 경로의 보호는 미검증입니다. 현재 후보의 보호 완료를 근거로 개발용 binding을 발급하지 않았습니다.

## 관리형 컴파일·최소 시험

관리형 테스트 DLL **exit 0 / 경고 0 / 오류 0**. 후보 전용 `--candidate-review`만 실행하여 **6 PASS / 0 FAIL**:

1. 실제 Native scope에 대한 경로 형식/고정 NTFS/중첩 없음.
2. 두 기존 부모의 SYSTEM owner·ReparsePoint 미표시 관찰.
3. E루트의 적용 Authenticated Users DELETE를 기존 정책이 정확히 거부.
4. E Program Files의 적용 Authenticated Users DELETE를 기존 정책이 정확히 거부.
5. 하위 4개 부재 → 원장 owner/DACL 보호를 주장하지 않음.
6. 미충족 후보가 production binding에 설치되지 않음.

기존 **24 PASS 및 36·34/기타 완료 시험 재실행 0**. 독립 도구 product/NativeAOT 발행·Main/Windows backend·QA child peer 실행 0. 테스트 DLL 컴파일/정적 snapshot 평가만 수행했습니다.

## 관리자 보호 bootstrap에 필요한 정확한 별도 승인

**현재 상태에서는 하위 폴더 생성만 승인받아 바로 적용할 수 없습니다.** 다음 승인은 서로 구분해야 합니다.

1. **상위 경로 문제 해결안의 별도 검토/승인**: 현재 `E:\`, `E:\Program Files`의 적용 AU DELETE가 기존 정책을 위반합니다. 이 두 경로는 다른 E드라이브 사용에도 영향을 주므로, 변경이 필요하면 정확한 대상/해당 ACE/전후 SDDL·영향·보존 범위를 먼저 검토해 명시 승인해야 합니다. 이번 작업은 그 변경안을 확정하거나 실행하지 않았습니다. 드라이브 전체 `/reset`, 재귀 ACL 변경, 자동 소유권 취득 또는 정책 예외는 허용한 것으로 해석하지 않습니다. 다른 후보 선택도 별도 결정입니다.
2. **부모 보호 재검증 후 신규 bootstrap 승인**: 현재 없는 아래 4개 경로만 parent→child 순서로 CREATE_NEW/명시 초기 보안 descriptor로 생성하는 범위입니다. owner BA(또는 명시 승인된 SYSTEM), group SY, `D:P(A;;FA;;;SY)(A;;FA;;;BA)`, 상속 HANDLE=false. 생성 전후 최종 경로/reparse·owner/DACL·비상속/부모 교체 방지를 readback해야 합니다.
   - `E:\Program Files\Limitless`
   - `E:\Program Files\Limitless\DiscordV2`
   - `E:\Program Files\Limitless\DiscordV2\Control`
   - `E:\Program Files\Limitless\DiscordV2\Control\DeploymentLedger`
3. 적용 시 기존 항목/경합/권한 불명확/검증 실패면 HOLD하고 부분 증거를 보존합니다. 기존 경로 채택·덮어쓰기·자동 삭제/rollback·기존 소비 record 초기화/다른 원장 복사는 승인 범위가 아닙니다. bootstrap 수행 코드/도구의 정확 SHA·대상·새 승인 ID·유효기간·증거 저장 범위도 별도 승인해야 합니다.
4. 안전 조건 확인 후에만 비공개 개발 binding을 확정하고 별도 도구 NativeAOT 빌드·새 SHA/코드 포함·V3 신뢰 검증을 승인할 수 있습니다. 도구 실제 실행/보호 배포·Task/FAKE_ONLY 시험은 또 별도입니다.

## 원본 보호 및 중지

Supervisor `6DF94E47C301C20EAB537EB6AD49C680129B80787536358E6BDC78ED50A842E7` / 7,559,680바이트, Probe `0BABC05A2E0B24B1E6563D049B8FAABD7375F737B2ACD5AEDEAF840FC5E59322` / 1,892,352바이트 보존 PASS. 기존 NativeBatch/제품 원장 코드 diff 0, 실제 binding 생성 0. 실제 보호 폴더/ACL/Task/계정/암호/OnBoot/V3 변경·소비 record 삭제/초기화/수입·이미지 복사/실행 0. 원본 main `ab47b931fec92976170f9f50aabde1822d78bb90` 유지.

자격증명 비공유/다른 실행 경로 미사용·V3 공식 정상 판정 UNKNOWN/NOT_VERIFIED는 유지합니다. cutoff **2026-11-01 23:59:59 KST** 자동 연장/옛 승인 재사용 금지. **NO_GO / MANUAL / EXECUTION_HOLD**로 보고 후 중지합니다. Push/main 병합/원본 적용 없음.

증거: [Summary](Evidence/DiscordV2LedgerCandidateReview20261031/Summary.json), [상위 ACL 공개 snapshot](Evidence/DiscordV2LedgerCandidateReview20261031/PathSnapshot.json), [6건 정책 시험](Evidence/DiscordV2LedgerCandidateReview20261031/Delta.log), [관리형 컴파일](Evidence/DiscordV2LedgerCandidateReview20261031/ManagedCompile.log), [입력 SHA](Evidence/DiscordV2LedgerCandidateReview20261031/InputsSHA.json). 실제 Native 경로를 포함한 원본 snapshot은 ignored 로컬 QA에만 보존했습니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
