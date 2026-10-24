# Main24 스킬 6슬롯 영속성 — 읽기 전용 기술 설계·테스트 계획

2026-10-23 / Issue #46 / READ_ONLY_PREFLIGHT / IMPLEMENTATION_NOT_STARTED
기준 제품 dcf9d6af + 문서2979e327, Issue46 본문/2026-10-23 댓글 및 기획 계약 f7cbe0a434d43cefb51109f53c98f765d1ff3660.
이번에는 C#·DTO·판매·결제·Save 구조를 변경하지 않았습니다. 아래 필드명과 손상 복구 정책은 **후속 구현을 위한 제안**이며 정식 가격·상품·강화 콘텐츠 확정이 아닙니다. 합성 Save 테스트도 아직 실행하지 않은 계획입니다.

## 1. 실제 코드에서 확인한 결손

|현행 코드|확인한 동작|영속성 영향|
|---|---|---|
|Scripts/Battle/SkillLoadoutService.cs: SlotCount/State/Resolve|6칸 string 배열, characterId + ':' + JobId 키의 static Dictionary. 라이브러리는 BattleSkillCatalog.GetSkills(job)의 기본 스킬. 첫3칸에 기본 스킬 장착|메모리만 보유. 빈칸의 위치를 유지해야 함|
|TrySetSlot|보유 스킬만 허용, 동일 ID를 다른 칸에 장착하면 이전 칸을 비움. 빈 ID는 해제|Import에서도 중복을 그대로 살려서는 안 됨|
|RegisterApprovedDefinition|IsImplemented 검사 후 라이브러리 추가. replacesId면 기존 항목과 모든 장착 칸을 새 ID로 바꿈|구매/강화 소유권·직업 적합성·가격·디스크 거래는 이 메서드가 보장하지 않음|
|Scripts/Core/GameSaveService.cs: GameSaveData/SaveCurrentSession|Version1. Currency/Inventory/QuestProgress/CompanionRoster/BeastCompanions는 기록하나 SkillLoadout 필드 없음|소유 스킬/강화/장착 저장 없음|
|RestoreSession → GameSessionData.Reset|Reset이 SkillLoadoutService.Reset을 호출한 뒤 Character/Job/Party 등을 복원. 스킬 Import 없음|Continue·다른 슬롯 로드 시 기본 라이브러리/배치로 돌아감|
|Scripts/Battle/BattleSceneController.cs:409~411|전투 시작 시 Snapshot/GetSlots를 복사|런타임 장착을 복원해야 다음 전투에도 적용됨. 전투 중 스냅샷은 유지|
|Scripts/UI/SkillManagementPresenter.cs:134~174|GetLibrary/GetSlots/TrySetSlot 사용|현재 무료 장착 UI를 재사용할 수 있음. 새 상점 UI와 혼합하지 않음|
|Scripts/Core/EconomyService.cs|TrySpendCurrency는 메모리 잔액만 감소|스킬 등록+화폐+저장을 원자적 거래라고 간주할 수 없음|

실제 스킬 정의는 BattleSkillSystem.cs의 BattleSkillCatalog에 C#으로 구성되어 있습니다. GameSaveData가 ScriptableObject나 BattleSkillDefinition 객체를 직렬화하도록 만들지 않습니다. Character/Job ID의 현재 문자열 조합을 미래 DTO에서 다시 파싱하는 대신 두 필드를 분리합니다.

## 2. 제안 데이터 계약 — 별도 승인 후 구현

기존 GameSaveData.Version=1을 유지하고 **선택 필드 `SkillState`**를 추가하는 최소 확장안을 우선 제안합니다. 최상위 버전은 현재 Validate가 정확히 일치해야 하므로 임의로2로 올리지 않습니다. 구버전 실행본이 새 필드를 버릴 수 있는 역방향 호환은 별도 제한으로 고지합니다.

```text
GameSaveData.SkillState?                 // null = 구버전, 무료 기본3 + 기본장착으로 복원
  SchemaVersion: 1                      // 이 하위 데이터의 버전
  Characters: [
    CharacterId: stable string
    JobId: stable string
    AcquiredSkillIds: string[]          // 유료/해금된 추가 기본형. 무료 기본3을 중복 구매 기록하지 않음
    Replacements: [{FromSkillId, ToSkillId}] // 승인된 강화 관계. To 소유권을 부여하고 From의 유효 소유권을 대체
    EquippedSlots: string[6]            // null/빈 ID = 빈칸. 배열 순서를 그대로 보존
  ]
```

이름과 구조는 제안입니다. 강화가 단일 ID 교체인지 별도 레벨인지, 구매 소유권이 캐릭터별/직업별로 귀속되는지는 기획 확정이 선행돼야 합니다. 현재 실행 중 char+job 단위 구조를 우선 존중하며, 계정 전체 공용 소유권으로 몰래 변경하지 않습니다. 기본3은 현행 Job 정의에서 파생하고, 구매·강화 ID는 **승인된 판매/스킬 카탈로그의 안정 ID**만 허용합니다. 중복된 소유권 파생 기록을 동시에 정본으로 두지 않습니다.

## 3. 복원 순서와 손상 복구 제안

1. 기존 최상위 캐릭터/직업/동료 명단 복원·Story 호환 해금이 끝난 뒤, UI 표시와 첫 전투 이전에 스킬 데이터를 복원합니다. Reset 전 데이터가 다음 슬롯으로 새지 않게 현재 Dictionary를 비웁니다.
2. SkillState 누락이면 무료 기본3·기본장착. **명시적인 6개 빈 슬롯은 사용자가 모두 해제한 상태**이므로 기본3을 강제로 다시 넣지 않습니다. 누락과 의도적 빈칸을 구별합니다.
3. 알 수 없는 Character/Job/Skill ID, 다른 직업 ID, 미구현 ID는 실행 정의/승인 카탈로그로 검증합니다. 깨진 스킬 한 개 때문에 기존 Save 슬롯 전체를 무효화하지 않는 국소 복구안을 제안합니다.
4. 유효 소유권 = 무료 기본 + 유효 추가획득 + 승인된 강화 교체 결과. 미보유 equipped ID는 빈칸으로 정리하고 자동 무료 소유권을 주지 않습니다.
5. 배열 <6은 빈칸으로 채움, >6은 첫6만 유지. 중복 equipped ID는 **앞쪽 유효 슬롯 우선** 제안. 같은 Character/Job 레코드 중복은 최초 유효 레코드 우선 제안. 이 정책은 구현 전 계약 확인 대상입니다.
6. 강화 관계는 승인된 From→To 매핑만 적용하고 순환/다중 분기/단계 누락/무료 기반 미보유 조건을 검사합니다. 잘못된 교체는 폐기하고 이전 유효 기본형을 유지하는 안을 제안합니다. 같은 스킬의 기본형과 강화형 동시 장착은 허용하지 않는 현행 교체 의미를 유지합니다.
7. 활성 파티 밖 동료·현재 비활성 Job의 기록도 저장합니다. 직업 변경/파티교체로 기록을 삭제하지 않으며, 다른 캐릭터의 스킬 소유권을 복사하지 않습니다. Save 슬롯 간에는 완전 분리합니다.
8. 구버전 호환/손상 복구는 금액을 추정 환급하지 않습니다. 가격·환불 정책이 없으므로 손상 레코드로 탈렌트를 임의 생성하지 않습니다. 진단은 ID와 복구 원인만 남기고 개인 Save 원문을 공개 로그에 남기지 않습니다.

## 4. 구매·강화·저장 실패의 후속 거래 경계

현재 SaveCurrentSession의 `.tmp` 작성 → File.Copy 덮어쓰기 경로는 기존 Quest 완료를 포함해 메모리/디스크 전체 롤백을 보장하지 않습니다. 이 경로 앞에서 TrySpendCurrency와 RegisterApprovedDefinition을 순서대로 호출하고 저장만 뒤에 붙이면, 실패 시 결제와 소유권이 어긋날 수 있습니다.

후속 구현에서는 한 요청의 대상 슬롯/Character/Job/상품/강화 기반/가격을 다시 검증하고 중복 처리 잠금을 잡은 뒤, **메모리를 먼저 확정하지 않는 거래 후보 스냅샷**을 만드는 안을 권장합니다. 후보 안에 화폐 잔액·획득/강화·6슬롯을 함께 담아 검증·저장 성공 후에만 실행 상태에 반영합니다. 현재 SaveCurrentSession은 전역 메모리에서 DTO를 만들므로, 이 설계를 쓰려면 검증된 DTO를 저장하는 최소 내부 진입점과 동시 자동저장 제어가 추가로 필요합니다. 해당 수정은 이번 범위에 포함하지 않습니다.

저장 성공이 확인되기 전에는 구매 성공 UI/아이템 지급/탈렌트 차감을 확정하지 않고, 실패 시 원상태·재시도 가능성을 보장해야 합니다. 이미 소유한 동일 상품/완료한 동일 강화 재호출은 차감 전에 거절합니다. `.tmp` 잔여·덮어쓰기 실패·프로세스 종료 때 기존 파일 보존/교체 보장 여부는 Windows와 실제 배포 플랫폼에서 별도 검증해야 합니다. 파일 API 이름만으로 전원 차단 내구성을 주장하지 않습니다.

장착은 기존 무료·전투 밖 규칙을 유지합니다. 전투 Snapshot은 전투가 끝날 때까지 고정하며 대상 취소/메뉴 재개 시 저장 슬롯을 다시 읽어 바꾸지 않습니다. 스킬 장착 자동저장 시점과 디스크 실패 안내 정책은 후속 구현 계약에서 정합니다.

## 5. 최소 delta 테스트 계획 — 이번에 실행하지 않음

|ID|격리 합성 데이터·실행|예상 결과|
|---|---|---|
|S01|SkillState 없는 현행 Version1 → Continue|기존 캐릭터/Quest/화폐 보존, 무료 기본3 복원|
|S02|빈칸 포함6슬롯을 저장→서비스 Reset/프로세스 종료→Continue|위치·빈칸·순서 동일. 단순 Scene 왕복만으로 영속 PASS 선언 금지|
|S03|6칸 모두 비움 → 저장/종료/Continue|6칸 빈 상태 유지|
|S04|추가 스킬 구매 후 저장/종료/Continue|소유·금액·장착 상태 일치. 새 스킬 자동 장착 없음|
|S05|장착한 기본형 강화 → 종료/Continue|동일 슬롯에 승인 강화형 승계, 기존 ID 제거|
|S06|미장착 기본형 강화|라이브러리만 교체. 임의 빈칸 자동 장착0|
|S07|플레이어/동료A/동료B·JobA/JobB·활성/비활성 파티 교대|각 레코드 독립, 미해금 동료/다른 직업 장착 거절|
|S08|슬롯1→2→빈슬롯/삭제/새게임|소유권·6칸·금액 교차 유출0|
|S09|미보유/미구현/삭제된/다른Job/빈/중복 ID, 배열0/5/7/100|계약에 따른 국소 복원, Quest 등 다른 기록 보존|
|S10|중복 Character/Job·불명 하위 schema·순환/분기 강화관계|결정된 fallback/진단, 임의 소유권/환불 생성0|
|S11|잔액 부족·음수가격·overflow·이중 클릭·동일상품 재호출|차감·소유·강화 변경0 또는 정확히1회|
|S12|폴더쓰기 실패/.tmp 실패/교체 실패/동시 자동저장/처리 중 종료|이전 유효 파일 보존, 중복 차감0, 재시도 이후 정합성|
|S13|전투 시작 후 변경 시도, 대상 취소·도망·패배·귀환|Snapshot 고정, 전투 중 장착 변경 거절, 귀환 후 기존 정책 유지|
|S14|기본3스킬15개·Main23 체크포인트·Main22 완료·승인펫 보존|변경 영향 경로만 delta 비교, 기존 전수QA 반복 금지|

구매 ID/가격·강화 카탈로그·캐릭터별 소유권 정책 확정 → 별도 구현 지시 → 독립 작업 단위에서 DTO/서비스 최소 수정 → 합성 Save와 실제 종료/Continue delta 검증 순서를 제안합니다. 이번 Main23 화면 보완과 별개이며 Issue #46을 해결/구현 완료로 닫지 않습니다.
