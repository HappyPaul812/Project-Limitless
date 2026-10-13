# Chapter2 완료 후 안전지역 펫 서비스 확대 / Chapter3 상점 준비

상태: IMPLEMENTED_ISOLATED / QA_PARTIAL / NPC_VISUAL_REVIEW_PENDING. 브랜치 `codex/safezone-services-20261013`. 구현 Commit `0d151e3f0222dac992145e69717f7607158b8106`. 원본 main·사용자 작업 트리·Scene·Save·승인 PNG·TTS·BGM 변경 없음. Push/main 병합 없음.

## 정본과 실제 구조

- 기준 main `0e04766`, 기획 `docs/planning` `c0f1d7c`의 `Chapter_Clear_SafeZone_Service_Rollout_20261013.md`. 문서의 과거 “아직 실행 승인 아님”은 이번 바울 대표의 Chapter2 서비스 실제 구현 지시로 갱신된다. 미정 Chapter3 조건/상품/NPC 계약은 그대로 대기한다.
- 필수 시작 문서, 공통 규칙, CODEX 인계, Main21 상세 정본, Chapter3 펫·경제 준비 문서를 확인했다. 실제 프로젝트 버전은 `ProjectVersion.txt`의 Unity `6000.5.7f1`이다.
- 실제 안전지역은 아래 세 곳이다. `GameSessionData.ActivateSafeZone`를 호출하는 거점 설치 코드를 기준으로 확인했다. 전투가 없는 모든 Scene을 안전지역으로 간주하지 않는다.

| SafeZone | Scene/설치 코드 | 기존 펫 서비스 | 이번 동작 |
| --- | --- | --- | --- |
| `safezone_starter_village` | `World_StarterVillage` / StarterVillageHubInstaller | 없음 | Main21 완료 후 분양/관리 각 1명 |
| `safezone_arbel` | `Arbel` / Chapter2IntroFlow | 분양 `(5,-3)`, 관리 `(7,-3)` | 클리어 전후 기존 두 NPC 그대로 사용 |
| `safezone_catacomb_entrance` | `Field_03` 일부 / CatacombEntranceSafeZone | 없음 | Main21 완료 후 분양/관리 각 1명 |

Field03 전체는 안전지역이 아니다. 지하묘지 입구의 기존 중심 `(.55,-3.65)`, 반경 `3.3`, 복귀 Spawn/보급/치유/파티/감시인과 입구를 그대로 둔다.

## 구현 범위

`SafeZonePetServiceRollout`은 등록된 Scene마다 하나의 런타임 관리자를 설치한다. 기존 `Chapter2Main21Flow.Completed`(QuestService의 `main_21_returning_warmth` 완료 이력)만 사용하며 SaveVersion/저장 필드/새 완료 플래그는 만들지 않는다. 첫 LateUpdate에서 기존 Arbel Start 설치가 끝난 뒤 같은 역할의 NPC를 확인한다. 기존 역할이 있으면 추가하지 않는다.

Quest Changed와 완료 여부를 재평가하므로 클리어 직후·재입장·Continue·같은 Scene에서 다른 슬롯 복원도 처리한다. 미완료로 돌아가면 **이 관리자가 만든 NPC만** 즉시 비활성화하고 제거한다. 기존 Arbel 두 NPC는 삭제하거나 잠그지 않는다.

| 추가 NPC | 후보 좌표 | 보존 기준 |
| --- | --- | --- |
| 마을 분양 담당 | `(-5.8,-3.6)` | 기존 상점/남문/주민/퀘스트 지점 불변 |
| 마을 관리 담당 | `(5.8,-3.6)` | 기존 훈련/주민/남문 동선 불변 |
| 묘지 입구 분양 담당 | `(-2.05,-3.55)` | 보급 `(-2.05,-2.55)`·치유 `(-2.05,-4.55)` 사이 |
| 묘지 입구 관리 담당 | `(2.85,-3.55)` | 파티 `(2.85,-2.55)`·감시 `(2.85,-4.55)` 사이 |

좌표와 역할명은 이번 격리 기능 후보이며 공식 인물 이름·대사·전용 외형은 아트/스토리 검토 대상으로 남긴다. 신규 그림을 만들지 않고 기존 Arbel 분양/관리 담당의 `VillageNpcSprites/Eldiran/OGA17_EquipmentShop`/`OGA16_Bank` 원본 Sprite를 재사용한다. 기존 AppearanceCatalog의 동일 body `.5×.34`, offset `(0,.17)`, Scale1, Sorting5/이름표7을 따른다.

## 공용 데이터와 사수 계약

- 모든 거점은 기존 `PetFacilityPresenter`와 `BeastCompanionService`를 호출한다. 거점별 구매/소유/발견/장착 재고를 만들지 않는다. 기본 wolf/bear/fox, 승리 발견 조건, 100탈렌트, 기존 보유 목록·캐릭터별 장착·MaxHP 비율/실패 rollback을 유지한다.
- 조사 중 기존 분양 `TryAdopt`가 **사수 0명이어도 구매 가능**함을 확인했다. 관리 화면만 “현재 관리할 사수가 없습니다”를 표시했다. 이는 확정 문서의 사수 전용 계약과 차이가 있었다.
- 이번 구현은 UI 진입 및 구매 경계에 기존 `GetSharpshooterCharacterIds` 판정을 추가한다. 사수가 0명일 때 “야수 동료는 사수 전용” 안내와 무청구 반환을 제공한다. 타 직업 주인공이라도 보유한 사수 동료(세린 등)가 있으면 기존처럼 분양/관리가 가능하다. 플레이어 직업만 검사해 세린을 막지 않는다.
- 전투 Scene에서 분양 구매를 거절하고 기존 장착 금지 동작을 유지한다. 현재 NPC 설치 목록에는 Battle이 없다. 정식으로 없는 훈련/부활/펫 업그레이드는 추가하지 않는다.

## Chapter3 스킬 상점 확대 준비 — 기획 결정 대기

이번에는 스킬 상점 NPC/상품/가격/Chapter3 완료 Quest 또는 플래그를 생성하지 않았다. 완료 조건 확정 전 유사한 숫자나 가짜 ID로 분기를 구현하지 않는다.

후속 공용 설계는 다음과 같다.

1. **최초 학습 접근**과 **모든 거점 판매망 확대**를 별도 판정한다. Chapter3 진행 중 최초 상점에서 판매가 가능해야 하며 전체 확대 조건 때문에 첫 상점을 클리어 뒤로 잠그지 않는다.
2. 실제 정식 SafeZone 목록을 펫과 공통으로 확인하되 서비스별 기존 NPC 검색/새 역할/위치를 별도로 등록한다. 기존 상점이 있는 곳은 같은 서비스를 다시 설치하지 않는다. 역할 enum을 임의의 기존 GeneralShop/PetManagement로 위장하지 않는다.
3. 구매·학습 상태는 안전지역별 재고가 아니라 캐릭터별 공용 소유 기록으로 공유한다. 미승인 Save 확장 규격이 확정되기 전 새 저장 필드를 생성하지 않는다. 학습과 6칸 장착은 분리하고 패시브/펫 기본 특성은 슬롯에 넣지 않는다.
4. 강화판은 기존 스킬을 대체하고 장착 위치를 승계한다. 원본+강화판 중복 구매/전투 노출, 반복 결제, 저장 실패의 화폐/소유 원자성을 검증한다.
5. 거점별 설치는 펫 서비스와 다른 관리자/역할로 하되 같은 NPC가 둘 이상 생기지 않게 기존 역할을 먼저 조회한다. 위치 선정 시 기존 펫·상점·NPC·Quest·Spawn·Collider·휠체어 통행·이름표를 함께 확인한다.

필요 결정: Chapter3 마지막 완료 Quest, 최초 상점 접근 시점·위치·NPC 계약, 실제 상품/Skill ID/가격, 캐릭터별 학습 소유권 및 Save 호환 규격, 승인된 인물/배치. 결정 전 실제 활성화는 대기한다.

## 검증과 보호

루트 담당이 공유 격리 Client에서 `ProjectLimitless.EditorTools.SafeZonePetServiceAudit.RunBatch`를 순차 Batch 실행했다. 원본 Editor/MCP Scene/선택/OS 포커스를 변경하지 않았다. Save/설정은 별도 `SafeZonePetResults/Saves`, `Settings`로 격리하고 음소거했다. 진입 전·진입 직후 격리 경로를 설정하고 Save 경로가 지정 폴더 안인지 검사했다.

최초 격리 컴파일에서 QA 도구의 `GetInstanceID` 두 호출이 Unity `6000.6.5f1`의 폐기 API 오류로 차단됐다. 신규 Runtime 코드 오류는 없었으며 QA 도구를 stable NPC ID 정렬 + 실제 객체 참조 비교로 수정했다. 두 번째 시도는 QA 도구의 Collider 엔진 객체 JSON 직렬화에서 중단되어 Collider primitive 값 비교로 수정했다. SceneManager 직접 로드 fixture의 월드 위치 기록과 실제 Collider offset도 보완했다. 재검증 결과는 아래 표를 갱신한다. 원본 `ProjectVersion.txt`(`6000.5.7f1`)는 바꾸지 않았으며 실제 격리 검증 Editor 버전과 구분한다.

세 번째 시도에서 마을 미완료/완료/중복0/기존NPC 보존/신규NPC 몸 겹침0은 통과했으나 QA 접근 거리 `.7`이 Player radius `.5` + NPC 반폭 `.25`보다 좁아 통행 검사에서 중단됐다. NPC 좌표는 바꾸지 않고 실제 양쪽 Collider 반폭 합 + `.1`(최소 `.9`)의 **중앙 통행로 쪽** 접근점으로 fixture를 보완했다. 묘지 입구는 접근점과 플레이어 몸 여유도 안전 반경 안에 있는지 추가 검사한다.

| 구분 | 상태 | 검사 범위 |
| --- | --- | --- |
| 선택 diff/공백 검사 | PASS | `git diff --check` |
| Scene·기존 GUID/PNG/SaveVersion 변경 | PASS(정적) | Scene 수정0, 기존 meta 수정0, 신규 C# meta2, Save 코드/버전 수정0 |
| Unity Compile / 신규 Runtime Console | PASS | 최종 격리 Unity6000.6.5f1 컴파일 성공, Runtime 오류0 |
| 기존 Editor Console | PARTIAL | 기존 SearchDatabase 인덱스 ArgumentOutOfRangeException1. 정확한 stack 판정으로 별도 원문 보존, 다른 오류는 제외하지 않음 |
| 클리어 전후·중복·기존 NPC·Collider/접근 | PASS | 3거점 기존 역할/좌표/Scale/Sprite/Collider primitive값 보존, 추가2/미완료0(Arbel기존2), 반경내/몸겹침0, 실제 InteractionTarget |
| 실제 NPC→UI / 경제·펫 공유 / Continue·타슬롯 | PASS | 6NPC 확인 버튼/UI 진입/닫기 입력해제, 실제UI100구매/중복결제0/3거점 공유/완료·미완료 Bootstrap Continue/타슬롯/쓰기실패 rollback |
| 지체의 길 공간 통행 | PASS(공간 검사) | 실제 Player Collider1×1 +.04 여유와 offset, Bounds 내 중앙 통행로→접근점 BFS/중간점 검사. 묘지 접근점+몸 여유도 반경 내부 |
| Main21 전체 재플레이 / 원본 프로젝트 버전6000.5.7f1 | NOT_VERIFIED | 기존 Main21 QA를 보호하고 완료Quest fixture를 사용. 전체13목표를 이번에 다시 실행하지 않음, 버전 파일 변경0 |
| 사람 손플레이·이름표 가독성·공식 NPC 미술/대사 승인 | NOT_VERIFIED | OS 입력·사용자 화면 조작하지 않음 |

소스 변경은 World 확대 관리자, 기존 AppearanceCatalog의 펫 역할 fallback, PetFacilityPresenter 사수 안내, BeastCompanionService 구매 사수/전투 경계, 격리 전용 Editor QA이다. 기존 NPC/Quest/Battle/Save/Scene/아트/TTS/BGM 자산을 바꾸지 않는다. 보호 파일 총수와 전후 해시는 루트 담당의 작업 전체 snapshot을 따른다.

최종 attempt6 결과는 **PASS106 / FAIL0 / PARTIAL1(기존 Editor Search)**이다. [실행 검사](Evidence/SafeZonePetServices_20261013/runtime_results.txt), [Runtime 오류 파일(0)](Evidence/SafeZonePetServices_20261013/runtime_console_errors.txt), [기존 Editor Search 원문](Evidence/SafeZonePetServices_20261013/known_editor_search_errors.txt), [전체 상태](Evidence/SafeZonePetServices_20261013/status.txt)를 선별 기록했다. 초기 attempt1~5 로그는 `Temp/ABC20261013/c_unity_attempt1..6.log`에 루트 담당이 보존했으며 실패 원문을 삭제하지 않았다. 의도적인 Save 실패주입은 환불/해금 rollback 검사이고 정상 오류로 은폐하지 않는다. 사용자 Save나 QA Save JSON은 Commit하지 않았다.

다음 단계: 바울 대표가 신규 NPC 후보 위치/외형/안내와 이름표 가독성을 확인 → 작업 브랜치 PR/병합 승인 결정. Chapter2 기능은 격리 구현·자동 검증 완료이며 Chapter3 상점은 미정 계약 확정 후 별도 구현한다. 자동 Push/main 반영은 하지 않는다.
