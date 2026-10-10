## 2026-10-10 StarterVillage v4 — 기술 PASS / LOCAL 적용 / 미술 승인 완료

[최종 검수](StarterVillage_Renewal_Art_v4_Inspection_20261010.md): 문 상·하단 및 지붕 접합/환경15종 PASS. 전용 V4 GUID·실제 Importer 검증, Scene284+남문 활성16+비활성2 Sprite만 교체. Generator 전용 로더 연결·ThirdParty 원본 불변. 격리 실제 RenderTexture·NPC12·남문 왕복/Spawn/저장 검사 PASS; 서브9종 Runtime/Continue 및 Main01~20 서비스 검사 PASS. 격리 Unity Search 내부 예외2건 미해결로 Console 검사 FAIL/전체 QA PARTIAL, 전체 메인 전투 회귀는 미검증. 사용자 기존 Scene 변경과 분리할 수 없어 실제 적용 Scene는 미스테이징 보존하고 LOCAL Sprite-only 패치를 Commit 증거로 남김. 보호1257개 중 허용3개 외1254개 불변. USER_ART_APPROVED. 마지막 관련 Commit은 이 항목을 포함한 `Feature: StarterVillage v4 전용 환경 통합 및 격리 QA`; 마차 보완 문서 Commit `1121cec`. Push0.


최종 승인 재검증: LOCAL 사본 대비 Sprite284개만 변경·그 외 불변. HEAD Renderer500/LOCAL284·공통 ID0으로 사용자 변경 없이 Scene Commit 분리 불가, LOCAL 미커밋 유지. Unity Search 예외2건 미해결·QA PARTIAL 유지. 통합 Commit `9af4e688`; 승인 문서 Commit `edb603e`.

---

## 2026-10-10 Main21 마차 Supplement v1 — 참고 검수 / PARTIAL·미구현

[보완 검수](Main21_Accessible_Wagon_Supplement_v1_Inspection_20261010.md): CRC/PNG4·Manifest PASS, RGB 참고 도해만 제공되어 신규 Sprite/Animation Import0. 휠체어2+좌석3·한 명씩 램프 탑승·가림 구역 개념 보완, 실측 치수/실제 회전 동선·기존 PNG Seat3/짐 대응·가림 레이어는 PARTIAL/USER_REVIEW_PENDING. 말 Rect/Pivot/접지점/FPS는 미제공, 기존 Grid 위험 미해결. 구현 준비 정본에 Anchor/Sorting/Scale TBD·추가 납품 요구 연결. 자동 귀환/Quest/Controller/Save 구현0. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main21 마차 보완 도해 검수와 미결 계약 기록`; 이전 아트 `ebf3872`. A 작업과 독립, Push0.

---

## 2026-10-10 Main21 접근 가능한 마차 v1 — 독립 아트 등록 / 이벤트 미구현

[검수 정본](Main21_Accessible_Wagon_Art_v1_Inspection_20261010.md): CRC/PNG5개·Manifest/정지 Import PASS. 전용 Art/Story/Main21Carriage/V1에 마차 Sprite2/미분할 참고 Texture2 신규 등록·원본해시 일치·실제 Importer/Console Error0 확인. 휠체어2대/벤치 구조는 보이나 승객5명·실제 치수 USER_ART_REVIEW_PENDING. 말3프레임 Rect/접지점·Scale·Anchor·가림 레이어는 후속 확정, Quest/Controller/Scene Transition/Save 구현0. 보호437파일 해시 불변·Bootstrap/Edit Mode/포커스 비활성 유지. 다음: 수용 배치 도해/Anchor·프레임 납품정보 및 최종 미술 검토 후 별도 구현·격리 QA. 하렌 이름 확정 commit `0ae6b3b`. 마지막 관련 commit은 이 항목을 포함한 `Chore: Main21 마차 v1 독립 아트 등록 및 검수`. Push0.

---

## 2026-10-10 하렌(Haren) — 정식 이름 확정 / Character 미구현

사용자 승인으로 태온 친형의 정식 이름을 하렌(Haren)으로 Story·팀 공통 Character 정본에 반영했다. 시각의 길/투사·제한적 시각 보조 판타지 바이저·형제애·Chapter3 마지막 “잘.했.어.” 유지. Character ID·연령·정확한 합류 시점/조건·현재 시력·눈 부상 정도는 계속 TBD. 기존3인 전투·폴/플레이어 구분·폴/미엘 Slow Burn·세린 역할 보호, Unity/Save/Character 아트 변경0. 마지막 관련 commit은 이 항목을 포함한 `Docs: 태온 친형 하렌 이름 정식 확정`. 아래 과거 기록의 이름 승인 대기는 이 확정으로 대체한다. Push0.

---

## 2026-10-10 StarterVillage v3 — NEEDS_ART_CORRECTION / 미통합

[v3 기술 검수](StarterVillage_Renewal_Art_v3_Inspection_20261010.md): ZIP CRC/PNG/환경15종 규격·매핑 PASS, v2 대비 문1종 변경/14종 동일. 상단 y10/x53~75 틈은 해결됐지만 하단 문 y114/x52~76의25×1px 투과 공백이 문 우선/벽 우선 모두 남아 FAIL. Unity 환경 교체0/300·비활성0/2, Scene/Prefab/Generator/Save/기능 수정0, 런타임 QA NOT_VERIFIED. 다음: 하단 접합 아트 수정 후 재검수; 최종 미술 USER_ART_REVIEW_PENDING. 마지막 관련 commit은 이 항목을 포함한 `Docs: StarterVillage v3 하단 접합 결함 검수 기록`; 이전 관련 `c3722da`. 기존 변경 보호·화면 조작/Push0.

---

## 2026-10-10 Main21 「돌아온 온기」 — DESIGN_DRAFT / IMPLEMENTATION_PENDING

[정식 기획 초안](../03_스토리/Chapter2_Main21_돌아온_온기.md)에 사용자 제공13목표·30~45분 목표(미검증)·게임플레이 중심·기존몬스터 전투A/B·세 지점 근거 판단 퍼즐·스토리5/자유편성 전투3·동료별 활약을 기록했다. 접근 가능한 보급 마차1종(접이식 램프/고정 공간/동료 좌석·휠체어2대)·자동 귀환/Skip·아트 미제작을 명시했다. 형 이름 하렌(Haren)은 가칭/USER_APPROVAL_PENDING, 형제 갈등 해결0·잘.했.어.는 Chapter3 결말 전용이다.

GitHub main/LOCAL HEAD 관련10문서 blob 일치 확인. 기존 형 등장→북쪽 징후 순서는 최신 초안의 징후11→형 첫만남12→북쪽준비13으로 참조 정리하고 Main20 구현 완료를 우선했다. ID/목표유형/좌표/조합/보상·형 이름 승인/정확 합류·대사/음악/최종 아트는 TBD. 팀별 후속 목록·문서 연결/검수 완료, 실제 Unity/Play/C#/Quest/Prefab/Sprite/BGM/TTS/Save 변경·제작0. 다음: 상세 사용자 승인 후 아트·구현·별도 QA, 승인 없이 구현 시작0. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main21 돌아온 온기 정식 기획 초안 기록`; 이전 구현준비 commit `6ba8f49`. 기존 변경 보호·Push0.

---

## 2026-10-10 Main21 자동 귀환 마차 — 기획 전제 확정 / 구현 준비 완료·미구현

아르벨→초보 마을 Main21 전용 일회성 자동 귀환·Yes/No·Skip·항상 접근 가능한 마차1종/최대 휠체어2대·스토리5명/전투3명 유지 전제를 [구현 준비 문서](../03_스토리/Main21_자동귀환_마차_구현준비_20261010.md)에 정리하고 Chapter2/메인 Story 정본에 연결했다. 기존 Scene/Dialogue/Quest/Spawn/Save/Input/BGM을 파일 대조했으며 Arbel 전용 연출 구간→안전한 마을 Spawn 전환을1순위 권장했다.

제공자는 레온1순위 후보, 도착은 기존 남문 복귀 좌표를 참고한 전용 Spawn 후보, 주행10~15초 목표. 정확 NPC/좌표·Objective/Dialogue·최종 아트/방식은 미정이다. 수락 체크포인트·도착 배치 뒤 진행·중복 Skip·저장 실패/Continue 복구·아트/placeholder·격리 QA 계획까지 기록. Unity/Script/Scene/Asset/Save/음원/placeholder 수정·생성0·화면 조작0. 링크/문서 차이 검증 완료, Runtime/Compile/RenderTexture NOT_VERIFIED. 다음은 미정 계약과 마차 아트 확정→실제 구현/QA. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main21 접근 가능한 자동 귀환 마차 구현 준비`이며 이전 관련 정본은 `0278d41`·`c3722da`. 기존 사용자 변경 보호·Push0.

---

## 2026-10-10 StarterVillage v2 — NEEDS_ART_CORRECTION / 미적용

ZIP CRC·PNG20 디코드·환경15종 치수/SHA/Manifest PASS, v1의 약14px 지붕 틈·길 Left/Right 매핑 해결. 그러나 실제 벽·문64px 겹침에서 문 상단23×1px/하단25×1px 배경 투과가 양 건물·양 그리기 순서에 남아 필수 접합 FAIL. [수정할 PNG·정확한 픽셀·확대 증거·전체 합성](StarterVillage_Renewal_Art_v2_Inspection_20261010.md). 기술 PASS/최종 미술 승인은 대기(`USER_ART_REVIEW_PENDING`).

Unity Import/Scene/Prefab/Generator 변경0·무료 Sprite 교체0. 기존 Scene284/남문 활성16+비활성2 유지, Unity/Tools/UserData3537파일·Save/설정·원본 ZIP SHA 동일. MCP Bootstrap/EditMode/포커스false·Compile/Import idle·ConsoleError0 읽기 확인, 화면 조작/Play/추가 Editor0. 새 Import/RenderTexture/NPC 가독성/남문·Save·Quest Runtime 회귀는 NOT_VERIFIED. 다음: 문PNG 조립공백 수정본 재검수→PASS 후 전용Art/Generator 통합과 별도 회귀. 구현 commit 없음; 마지막 관련 commit은 이 항목을 포함한 `Docs: StarterVillage v2 접합 재검수와 적용 중단 기록`. 기존 Git 변경 보존·Push0.

---

## 2026-10-10 팀 공통 인물·형제애·바이저 — 기획 보완 / 미구현

플레이어와 고유 동료 폴 구분·Main21 일행5/전투3·세린의 지속적인 탐험/판단 역할·태온의 경험 기반 자기 판단과 패턴 익히기·형의 제한적 마력 바이저·마지막 **잘.했.어.**를 [팀 공통 정본](../04_등장인물/동료_형제애_바이저_팀공통정본_20261010.md)에 기록하고 기존6정본을 연결했다. 형 합류/현재 시력/바이저 작동·외형, Main21 상세 목표/협동 전투, 세린 개인 서사 시기는 미정이다.

CompanionDefinitions4의 ID/Path/Job·동료 편성 최대2·패턴 익히기 구현 대조 및 링크49 검증 PASS. 이번 서사 기획은 미구현이며 Dialogue/TTS/Character/Save 변경0. 다음은 미정 상세 설계 확정. 마지막 관련 commit은 이 항목을 포함한 `Docs: 동료 형제애 바이저 팀 공통 정본 보완`.

---

## 2026-10-10 Chapter 3 형제애 — 기획 확정 / 미구현

Chapter3 Main22 시작·태온 친형 시각의 길+투사·과거 보호 중 눈 부상(길 자동 결정 아님)·가족식 반말·보호 책임감과 태온 자기결정권의 갈등을 확정했다. 형이 태온의 판단을 믿고 맡기게 되는 감정 변화와 마지막 직접 칭찬 **“잘.했.어.”**를 정본화했다. Main21 형제1:1 대련안 폐기·기존3인 전투 유지·협동 전투는 검토 방향. Main21 초보 마을 귀환/폴·미엘 Slow Burn/형 등장 유지. [담당별 공유·결말 연출·미정 항목 정본](../03_스토리/Chapter3_태온_형제애_서사_20261010.md).

문서 연결·확정/미정 경계 검토 완료. Unity 구현/Quest·Character Asset/음성·음악 제작0, 기존 변경 보존. 형 이름·나이·현재 시력·정확한 합류 조건·Chapter3 보스/퀘스트 수·Dialogue ID/TTS 제작은 미정. 다음은 별도 상세 설계 확정이며 이번 구현 승인은 없다. 마지막 관련 commit은 이 항목을 포함한 `Docs: Chapter 3 태온 형제애 서사 정본 확정`.

---

## 2026-10-09 최신 상태 — Main20 「심부의 거신」 Unity 구현 / 격리 Runtime PASS

**Quest9/9·Field11·Boss2Phase·승리 후 잔해/냉각·귀환 구현 완료.** Main19 완료 후 Field10 서쪽→Field11 진입, 현재 목표 조사/세린 텍스트 대화→거신 전투→9목표 완료까지 연결했다. Boss Lv15/HP1200/Attack30/AG10/BaseEXP140(기존 레벨 차 배율)/Talent40·Loot0, Quest EXP100/Talent80 별도1회. HP600 즉시 전환1회·기존 파동 예고 이행 후 Phase2 주입·용융 강타MeleePhysical 계약 반영. 도망/야수 습격 차단, 기존 Party/Formation 유지.

승인 v3 게임PNG17·기본시트+동기화Overlay16·정적VFX7·환경8(Trace 포함), Crowns 보스/Paths Field11·복귀를 연결했다. 원본17PNG/MP3 byte 동일·신규GUID. 원본Field10/StarterVillage·Side9/Loot7·Save/WAV/TTS 보존. 세린5 TTS_PENDING·무음3, API/WAV 생성0.

검증: 격리 Unity6000.5.7f1 Batch Main20 151·일반 Turn Queue 승리12·Main18 467·Main19 169·Side9 574 assertions PASS(합1373/FAIL0). 실제물리 출구 왕복·Quest9·패배/재도전·보상EXP/Talent 중복 차단·Continue3시점(Party/Inventory/HPMP/Beast/Path/좌표)·5슬롯Version1·카메라3비율/절벽동선 PASS. Loot7 실제 보상 확률경계·수량/판매/중첩, 파수꾼60%/장막 모델 PASS. 최종 격리 compile/Runtime Console Error0·로드Scene MissingScript0. 원본MCP Refresh/compile 후 ConsoleError0, Bootstrap/Edit Mode/포커스false 유지.

[최종 구현·QA·미검증 사항](Main20_Implementation_Final_QA_20261009.md) · [1373 assertion 증거](Main20_Runtime_Results_20261009.txt). 일반전투는Lv15 전사+세린+미엘 구성의 실제Attack19회 승리(HP160/100/56)이며 전직업 난이도 승인은 아니다. 구형Main18/19 검사기의 NPC메뉴/출입구 가정만 격리복사본에서 보정했다. 예상 밖 보호파일 변경0.

다음: 사용자 실제 화면 Ground/Arena·Boss/KO/Overlay/VFX/HUD·충돌 체감, 키보드/마우스/패드 입력, Crowns 최종 청취·전직업 난이도 검토. 기존 파수꾼 전체UI완주 재실행·7종 모든자연접촉 드롭통계는NOT_VERIFIED. v3 미술/Overlay 채택 승인은 완료·추가 아트 재제작 요구0, Main21 상세 구현0. 구현 commit `eaefb0c8b11596a50bc0e81ffa46c8582ad92ccc`; QA 문서 commit은 이 항목을 포함한 `Docs: Main20 최종 백그라운드 QA와 현재 상태 기록` 참조. 이번 GitHub Push0.

---

## 2026-10-09 최신 상태 — Main20 아트 v3 기술 PASS / 사용자 미술 검토 대기

**READY_FOR_USER_ART_REVIEW**. v2의 마지막 FAIL인 Cliff 회색 사각 가이드선 제거 PASS(문제3구간 alpha0·Field11 지면 합성 확인). 게임 PNG17 무결성 PASS, 실제 변경은 Cliff1개·나머지16 byte 동일. Boss1256/314/4×4·공용3행·Overlay16프레임 정렬/KO 소거·VFX7 기술 PASS 유지. 추가 Trace 보존·정본16역할 대응(이름15+Overlay대안1). v3 PNG22=게임17+QA5, text3·CRC/decode 손상0.

사용자 미술 승인/Overlay 최종 채택 대기. Ground/Arena·Idle/KO·VFX 실제 가독성 PARTIAL 유지, 필수 추가 아트 재제작 요구0. [v3 최종 검사·권장 기술 계약·승인 후 구현 순서](Main20_ArtPack_Inspection_v3_20261009.md). 기존 v1/v2 기록 보존. Phase 예고 이행/첫행동·용융강타 분류·EXP배율 권장안을 제시했으나 임의 확정0.

다음: 사용자 미술/Overlay·음악 검토→미결 기술 계약 결정→별도 Main20 구현 지시→신규 Import/Field11 왕복/Quest9·Boss·VFX·Save/BGM 연결→격리 QA. 이번 Unity Import·코드/Scene·Side9/잡템7·Save/PNG/meta/Audio 변경0, 포커스 조작/Push0. LOCAL 시작e75e7b7, 마지막 관련 commit은 이 항목을 포함한 `Docs: Main20 아트 v3 최종 기술 검수와 구현 준비 기록` 참조.

---

## 2026-10-09 최신 상태 — Main20 아트 v2 재검수 / NEEDS_ART_CORRECTION

LOCAL 시작 cad75de. v2 PNG21=게임 RGBA17+QA RGB4, text2·CRC/decode 손상0. 정본 이름15/16(Phase2는 Overlay 대안), 역할 대응16후보+추가 Trace 보존. Boss 공용3행10/11 방출 제거 PASS, Overlay16셀 정렬·KO overlay14/15 소거 PASS. 기술적 채택 가능하며 사용자 대안 승인 대기. VFX7 byte 보존·static 단일512 매핑 PASS. Ground 경계/Arena/Trace 완화 확인, 실제 화면은 PARTIAL.

**NEEDS_ART_CORRECTION**: Cliff_Boundary에 회색 사각 가이드선 잔존(Handoff 충돌), 수정 재납품 필요. Ground 반복 띠·Idle/KO 흐름·전체 Boss 열빛 소거·미술 승인은 별도 검토 대기. [v2 검수·정본16/목표9·후속 기술 미결](Main20_ArtPack_Inspection_v2_20261009.md). v1 검수 이력 보존. Phase 예고 경계/용융 강타 분류/EXP140 배율/Field10↔11/Crowns/Quest·Save 연결을 정리했으며 임의 확정·구현0.

원본 ZIP/PNG·Unity/Side9·Save·WAV/TTS/Audio 변경0, Import/Play/포커스 조작/Push0. 다음: Cliff 수정→재검수→사용자 미술 검토·Overlay 승인→미결 계약 결정→별도 Main20 구현 지시. 마지막 관련 commit은 이 항목을 포함한 `Docs: Main20 아트 v2 재검수와 통합 준비 기록` 참조.

---

## 2026-10-09 최신 상태 — Chapter2 잡템7 등록 / 사용자 미술검토 대기

## 2026-10-09 최신 상태 — Chapter1·2 서브 퀘스트 9종 구현

Chapter1 5/5 · Chapter2 4/4 등록, 현재 인벤토리 수집·확인 납품·정확 차감·EXP440/Talent173·동시 의뢰·NPC 명시 선택·HUD/QuestLog/추적 연결 완료. Arbel QuestLog 연결 보완. 기존 Main 완료 로직·몬스터·잡템7·Main20·사용자 Save/아트/Scene 보존.

검증: 격리 Unity6000.5.7f1 Batch Runtime9/9 실제 NPC 수락·거절·납품·Bootstrap Continue, 일부 수량/판매 감소/보상 실패·중복 차단/5슬롯 Version1 PASS. 최종 compile error0·Runtime console error0·로드3Scene Missing Script0. Main01~19 목표 서비스와 Main18 규칙 PASS; 전체 실제 전투·이동 완주 및 물리 입력/시각 승인은 미검증. 격리 검색 인덱스 초기화 오류는 격리 검색 시작 옵션 해제 후 재검증 통과.

[정식 기획·EXP 가정](Chapter1_Chapter2_SideQuest_Design_20261009.md) · [최종 QA·증거·미검증](Chapter1_Chapter2_SideQuest_Final_QA.md). 다음: 사용자 UI/키보드·패드/전투 수집→납품 확인, Main 완주 회귀와 실제 EXP 동선 측정. Chapter2 종료Lv16~17 보장 아님. 관련 commit 7a78aa8 / 14327fd, 마지막 QA commit은 이 항목을 포함한 `Chore: 서브 퀘스트 9종 격리 Runtime QA 기록` 참조. GitHub Push0.

---

아이콘7 기술PASS, ItemDefinition7/7·몬스터LootEntry7/7. 확률/성공1개/중복입력보상방어/stack99/판매/구매차단/저장JSON 서비스검증 PASS, Console error0. 기존3315파일 중 몬스터7 lootEntries만 변경, Save/기존PNG/meta·사용자변경 보존. [상세 정의·검수·구현파일·검증](Chapter2_Loot_Items_20261009.md).

USER_ART_REVIEW_PENDING. 실제UI·Continue·Main17~19 Runtime 회귀 미검증. 별도 잡템Handoff 원문 미발견으로 Manifest설명 사용·대조 후속. 시작commit0483167; 마지막 관련commit은 `Feature: Chapter2 잡템 7종 및 몬스터 드롭 등록` 참조. Push0·Main20 구현 미착수 유지.

---

## 2026-10-09 최신 상태 — Main20 아트 납품 검수 / NEEDS_ART_CORRECTION

ZIP PNG17+Manifest1, 정본16개 basename 정확일치7·이름/방식 대응후보9·추가Trace1. PNG decode/CRC PASS, 기본Boss1256/314/4×4/16셀·셀경계 PASS. Skill/Hit 공용3행 frame10/11 방출표현은 정본 충돌로 수정 필요. Phase2 완성시트 미납품·overlay 대안/동작 정합 확인 필요, VFX7은512px static1frame 매핑 대상. Trace는 흔적목표 대응 가능, cooled는Rift01 대응·Ground 반복경계는 사용자 검토 대기.

**NEEDS_ART_CORRECTION / USER_ART_REVIEW_REQUIRED / Unity 통합 미착수**. 검수보고서·CURRENT_STATUS만 갱신, 원본 ZIP/PNG·Unity Asset/Scene/C#/meta/GUID/Save 변경0·Import/Play/API/Push0. 다음은 공용3행 수정·Phase2/파일별칭 매핑 확인→재검수/사용자 시각 승인→별도 Unity 구현. [전체 검사·정본16매핑·목표9·통합 준비](Main20_ArtPack_Inspection_20261009.md). 시작/직전 관련commit9920f5c; 이번 마지막 관련commit은 이 항목을 포함한 `Docs: Main20 아트 납품 검수와 Unity 통합 준비 기록` 참조.

---

## 2026-10-09 최신 상태 — Main20 정식 설계·Art/Audio/TTS Handoff 완료 / 미구현

Main20 「심부의 거신」 / main_20_colossus_of_the_depths / Field_11_DeepCore 「열맥 심부」 / Main19 선행·목표9. 열맥 거신 Veinfire Colossus / veinfire_colossus: Lv15 HP1200 Attack30 Agility10·Boss EXP140/Talent40·Quest EXP100/Talent80·IsBosstrue·Flee/Pet불가·Loot없음. HP50% 첫 전환1회·고정4/5행동·공용 Burn/과열 유지. 세계 최종 원인 확정0·Main21 상세 확정0.

DESIGN_CONFIRMED / IMPLEMENTATION_PENDING. 정적 전체 NEEDS_IMPLEMENTATION_EXTENSION: Phase/예고/HUD·새Scene/Quest/Encounter·BGM 매핑 필요. 예고 중 Phase 전환 cursor·용융 강타 사거리·기본EXP/최종지급 경계를 임의 결정하지 않음. [정식 설계](../03_스토리/Chapter2_Main20_심부의_거신.md) · [QA](Main20_Design_QA.md).

[Art Handoff](Main20_Art_Handoff.md): Field11 환경·Boss4×4/16프레임·Phase2 대응안·VFX7·KO, ART_PENDING/USER_ART_REVIEW_REQUIRED. [Audio Handoff](Main20_Audio_Handoff.md): Paths 재사용·Crowns 원본 실재/177.815458초/MP3/44100Hz/stereo·decode PASS·Unity 동일byte0,IMPORT_PENDING/USER_LISTENING_REQUIRED. [TTS Manifest](Main20_TTS_Manifest.csv): 세린5 TTS_PENDING·Player2/지문1 무음·Schedar 유지.

이번 문서9개만 변경. 게임코드0·Unity Asset0·Audio Import/MP3복사0·TTS API/PNG/WAV생성0·SaveVersion변경0. 보호: Main18 성공15·잔여7·전체청취 미완료·UNITY_APPLIED0·성공15 재생성 금지, Main19 TTS_PENDING13·생성0 유지. Unity 실행 검증은 이번에 수행하지 않았으며 실제 Art/음악 청취·Boss 난이도/Runtime QA는 후속 구현 후 확인한다.

다음 권장: Main20 Art Handoff를 아트 담당 세션에 전달하여 정식 Boss/Field11/VFX 제작→미결 기술 계약 확인→별도 구현·백그라운드 QA. TTS 제작 우선순위 Main18→Main19 보호. 시작/직전 관련 commit6a4ccc3, 이번 마지막 관련 commit은 이 항목을 포함한 `Docs: Main20 심부의 거신 정식 설계 및 제작 Handoff` commit 참조. GitHub Push0.

---

## 2026-10-09 최신 상태 — Main18 TTS Day1 quota 중단 / 잔여7

전체22·성공15(Story01~14/16)·실패2(Story15/17)·미호출5(Story18~22)·API시도17. **UNITY_APPLIED0 / 사용자 전체청취 미완료 / 성공15 재생성 금지 / 다음 재개 정확히7 / Main19 TTS 생성0·미시작**. 원본 제작 단계이며 Unity Voice 복사·Catalog·Dialogue 연결은 전체22 완성과 사용자 직접 청취 승인 전 금지.

관측 quotaId `GenerateRequestsPerDayPerProjectPerModel-FreeTier`, quotaValue10·retryDelay 반환. Key 개수가 아닌 Project + Model의 실제 quota 상태로 계획한다. Story15 quota 실패 뒤 Story16/17 시도 이력은 보존하며 앞으로는 첫 quota 실패 즉시 중단. reset 시각·Key 소속 Project·현재 남은 quota는 미확정.

문서 검증: Manifest22 / checkpoint17=성공15+실패2 / WAV15 hash·ID 대응 / 잔여7·15+7=22 확인. 기존 Manifest·checkpoint·성공WAV 보존, 이번 API0·WAV변경0·Unity변경0·코드변경0. [재개7 Dialogue ID·증거·운영기록](TTS_FreeTier_Quota_운영_기록.md#다음-재개-대상--정확히-7개) · [장기 정책](TTS_음성_제작_정책.md#free-tier-quota-운영-규칙).

다음 권장: 실제 quota/checkpoint 확인→Main18 잔여7→전체22 존재 확인→사용자 전체 청취→필요한 재녹음→승인 후 별도 Unity 적용→quota에 맞춰 Main19 TTS13. 원본·checkpoint는 현재 LOCAL 미추적 자료이므로 후임에게 별도 전달 필요. Unity 사용자 확인은 승인 후 적용 작업에서 수행. 시작/직전 관련 commit `528f205`; 이번 기록의 마지막 관련 commit은 이 항목을 포함한 `Docs: TTS Free Tier quota 운영 규칙과 Main18 재개 상태 기록` commit 참조. GitHub Push0.

---

## 2026-10-08 최신 상태 — Main19 Art13 정식 반영·연출 기술 PASS

납품ZIP13PNG+Manifest 일치·원본13 byte 보존·새GUID/meta13·Null0. Field10 Ground/흔적/암벽/능선/심부 차단 정식 배치·조사GroundPulse1회·Witness 전 실루엣숨김/목격표시+먼flare1회·Continue 복원. **SOURCE_RECEIVED/AUDITED/IMPORTED/RUNTIME_VERIFIED**, 기술PASS_WITH_NOTE13·FAIL_BLOCKING0·미적 **USER_ART_REVIEW_REQUIRED13**. ART_PENDING 납품 대기는 해소, 최종 미술 승인 대기는 유지. 가이드 선/반복 경계/실루엣 식별·거리감/전체 색감 사용자 검토.

비포커스 격리 Play **314PASS/FAIL0**: Quest12·A/B 정상승리·각도망/전멸재도전·중복보상0·실제Trigger Field09↔10·Continue5시점·Silhouette Gate/VFX중복0·BGM유지·Bounds. CompileError0/RuntimeConsoleError·신규Warning0/MissingScript0. SaveVersion1·Party/Formation/Beast·Main18과열/기존Monster/Voice/BGM/Sprite·사용자Save/meta보호. baseline3301중3299byte동일·승인Scene/Flow2만변경. Main19 TTS_PENDING13·Main18 TTS_PENDING22, Main07/Main17 Voice CLOSED 유지·API0. Bootstrap EditMode/비포커스복구.

다음 권장: [원본/배치 Art 검토](Main19_Art_Import_QA.md)→승인 의견에 따른 후속 수정, NPC13 TTS 제작·직접 전체 청취 승인 후 별도 적용. 마지막 관련 구현commit `294b81e`, 원본등록 `3d477d6`; QA는 이 항목 포함 최신Docs commit. [통합QA/변경목록](Main19_Final_QA.md)·[Witness](QA_증거/Main19/Witness_1280.png). GitHub Push0.

---

## 2026-10-08 최신 상태 — Main19 정식 설계·기초 구현 PASS

Main19 「타오르는 맥동」 / `main_19_burning_pulse`, Main18 선행·12목표·EXP80/Currency70. Field10 「맥동의 열맥」 21×15 임시환경·Field09 왕복·서쪽 Main20 폐쇄. Story A 갑충+도마뱀, B 갑충+망령(후열), 기존 Monster 수치/과열/냉각/정화 보호. 탐색 Paths of Cracked Earth·전투 Blade and Gambit 기존곡 연결. Witness/Hook·Path5 정의. 신규 NPC13 **TTS_PENDING**, Player9+지문1 무음; API0. Art8종(균열 장식2변형)+VFX2 **ART_PENDING / USER_ART_REVIEW_REQUIRED**, 기존 그림 임시 재사용.

비포커스 격리 Play **169 PASS/FAIL0**: 실제12목표·A/B 정상승리/도망/전멸 재도전·보상 중복0·실제 Continue4시점·Party/Formation/Beast/소지품/HP·MP/성장 보존·3화면비 Bounds·MissingScript0. CompileError0/RuntimeConsoleError·신규RuntimeWarning0. SaveVersion1·사용자 저장/설정·기존 Main01~18/Voice/Sprite 보호, 기존3275파일 중3270 byte 동일·승인5만 변경. Main07 작별5/Main17 신규14 CLOSED, Main18 TTS_PENDING22 유지. clean Bootstrap EditMode 복구.

사용자 확인/다음 권장: 최종 Art 전달·실루엣/동선/난이도 체감 검토, NPC13 TTS 제작→직접 전체 청취 승인→별도 Unity 적용. Optional 실제 전투/다른 Path·Party·직업 전체 Play는 후속 QA. 마지막 관련 구현 commit `52259d5`; 정식 설계/최종 QA는 이 항목 포함 최신 Docs commit. [정식설계](../03_스토리/Chapter2_Main19_타오르는_맥동.md) · [최종QA/변경파일목록](Main19_Design_QA.md) · [Art](Main19_Art_Handoff.md) · [TTS23](Main19_TTS_Manifest.csv). GitHub Push0.

---

## 2026-10-08 최신 상태 — 폴 작별 Voice5 CLOSED

사용자 직접 청취 USER_LISTENED_PASS5/5·승인원본5 byte복사·신규GUID5·Catalog250→255/기존prefix보호·Source SHA/PCM/Unity전sample5/5오차0. 실제작별6페이지/Voice5·Player무음·모든5Clip자연종료후page유지·명시Next1page·같은프레임E/A/E재열림0·Main07완료·기존Main08Trace01 “잠깐만요.” 실제재생·BootstrapContinue2회·VoiceMixer100/40/0/Mute복원 **143PASS/FAIL0**. 전체40페이지/Character38resolve/Player2무음, 기존33Voice변경0/Main08변경0/API0. **TTS_REQUIRED5→0 / USER_LISTENED_PASS5 / UNITY_APPLIED5 / RUNTIME_VERIFIED5 / 이번 작별 누락 CLOSED**. 기존 별도Main07 청취 대기는 승격하지 않았다.

CompileError0/RuntimeConsoleError·신규Warning0/MissingScript0, 기존CS0618컴파일경고16 별도. 사용자Save·Sprite·Portrait·Battle·Growth·Retry·Main17/18 byte보호. cleanBootstrap EditMode·비포커스·격리Save/Settings/Input/Listener복구. 추가 작별Voice 청취요청0. 다음권장: 별도 요청의 기존 미확인목록, Main18 TTS_PENDING22 유지. 마지막구현commit `4c9a889`; QA는 이항목포함최신Docs commit. [최종QA](Main07_Paul_Farewell_Final_QA.md) · [적용표5](Main07_Paul_Farewell_Final_Matrix.csv). GitHub Push0.

---

## 2026-10-08 Main07 폴 작별 ID 누락 수정 / TTS_REQUIRED5

사용자 Play의 작별 Character Voice 무음 원인은 폴5줄 Stable ID 누락. main07_paul_supp_001~005 추가·Player “혼자 가시려고요?” 무음 유지. 전수40페이지/Character38/기존resolve33/추가ID누락0. **WAV 미제작·Unity Catalog 미적용·TTS_REQUIRED5, 실제 Voice 문제 OPEN**. [정본Handoff5](Main07_Paul_Farewell_TTS_Handoff.csv)·[QA](Main07_Paul_Farewell_QA.md).

실제6페이지 text fallback/명시Next1page/같은프레임E/A재열림0·Main07완료·기존Main08재진입/Trace01 “잠깐만요.” 실제재생·자연종료page유지·BootstrapContinue2회 73PASS/FAIL0. CompileError0/RuntimeConsoleError·신규Warning0/MissingScript0. 기존WAV·Catalog·Main08변경0/API0/사용자변경byte보호. 사용자 다음 확인은 TTS담당 제작 후 신규5 전체 청취 승인, 이후 별도 Unity 적용·검증 후 CLOSED. 마지막 구현commit `4a9e4c7`, QA는 이 항목 포함 최신Docs commit. GitHub Push0.

---

## 2026-10-08 Main17 승인 TTS14 적용·Runtime 검증 완료

사용자 직접 청취 **USER_LISTENED_PASS14/14**. 원본 byte 복사14·Catalog236→250(prefix보호)·SHA/PCM/Unity전sample14/14오차0. Default/Hearing 실제12목표·조건부Paul/Miel/Taeon 유무·자연종료14 후page유지·Next/Scene/Battle잔류0·정상승리/afterVoice·실제Continue5지점·Mixer100/40/0/Mute/복원 PASS. 기술고유646PASS/FAIL0, CompileError0/최종ConsoleError·Warning0/새Warning0/MissingScript0. **Main17 TTS_PENDING14→0, USER_LISTENED_PASS14·UNITY_APPLIED14·RUNTIME_VERIFIED14**. Main18 TTS_PENDING22·생성/적용/변경0, API0·가공0·기존Voice예상밖변경0.

기존3253파일 중3252byte보호·의도한Catalog1수정·삭제0, 사용자 미커밋 변경 보호. 추가 Main17 Voice 청취 요청0; 기존 음악/실물입력 및 Main18의 별도 미확인 항목 유지. 다음권장: 사용자 지정 후속 확인 또는 별도 승인 Main18 음성 작업. 마지막 관련 구현 commit `f0f44b3`, QA는 이 항목 포함 최신Docs commit. [최종TTSQA](Main17_TTS_Final_QA.md) · [적용14표](Main17_TTS_Applied_Matrix.csv). cleanBootstrap EditMode·비포커스·격리저장/설정/Listener복구. GitHub Push0.

---

## 2026-10-08 Main18 「검은 열기」 구현·자동 QA 완료

정식13목표/Field09 흑요석 상흔21×15/보고 냉각약3·아르벨 판매/독립 과열·HUD·VFX/갑충·감시자 Elite/BGM/무음 Player·NPC TTS_PENDING22 구현. Main19·Cleanse II·새 음성 생성0. 실제13목표 연속 진행·두 전투 도망/패배 재도전·정상승리·중복0, BootstrapContinue7지점 보존, 5Job×2전투·5Path관찰 PASS. Runtime 고유 500PASS/FAIL0, CompileError0/최종ConsoleError·Warning0/새Warning0/MissingScript0, Source17PASS_WITH_NOTE·핵심FAIL_BLOCKING0. 기존3156파일 중3144byte 동일·의도한12수정·삭제0, 기존 사용자 변경 보호.

사용자 직접 확인: **Art17 USER_ART_REVIEW_REQUIRED**(흰 원본 guide/반복 지면 경계 포함), **BGM2 USER_LISTENING_REQUIRED**, 실물 입력1. 자동 시각/청취 PASS로 승격0. 다음 권장: Art 검토→승인 원본 교체·BGM 청취→별도 승인 TTS; Main19 별도 요청. 마지막 관련 구현 commit `6361b00`(Art `543e82b`, 과열 `f72d907`, 설계 `2e6e86c`); QA는 이 항목을 포함한 최신 Docs commit. [최종QA](Main18_Final_QA.md) · [Art](Main18_Art_Import_QA.md) · [Balance](Main18_Balance_QA.md). 사용자 화면 보호, clean Bootstrap Edit Mode·비포커스 복구. GitHub Push0.

---

## 2026-10-07 Main05 태온001 WAV 혼입 복구 / 재감사 완료

사용자 실제Play의 무입력 추가발화는 기존001 WAV 안에 대표보고002 문장이 섞인 원인. 원본PCM 6.60초 무음경계에서001만 복구(10.55→6.60초), 정상5/원본/GUID/Catalog/게임Dialogue·Quest·Input 보호·TTS0. Voice6 새전사/nativePCM6/6, 실제Main04→Guard6→독립대표8→정식해금→Main06첫경계 304PASS/FAIL0, page2자연종료후5초무입력 유지/002자동재생0/1입력미엘page3/대표자동연결0/실제Continue3회 PASS. CompileError0/최종RuntimeConsoleError·Warning0/신규Warning0/MissingScript0·기존컴파일Warning16. QA첫Scene위치누락오류2건은 별도보존 후fixture수정, 최종오류0.

복구001 사람청취 **USER_CONFIRMED_REPAIRED**(사용자: “해당 두 문장만 나오고 끝도 자연스러움”), Main05 USER_LISTENING_REQUIRED0. 다음권장: 보호된별도후속요청. 보호baseline3497중2개변경/3495byte동일·Main07재도전/동료성장/미엘/Early7/Paul20와사용자Save보호. 이전Main05CLOSED는 당시이력이며001anchor판정을이번의미PASS로재사용0. [정본QA](Main05_Guard_Voice_Reaudit.md) · [Voice6](Main05_Guard_Voice_Reaudit_Matrix.csv) · [검증JSON](Main05_Guard_Reaudit_Verification.json). 마지막 관련WAVcommit `b96e8b3`, QA는 이항목을포함한최신 `Docs: Main05 태온001 혼입 재감사 및 Runtime QA` commit 참조. GitHub Push0.

---

## 2026-10-07 Main07 재도전·동료 성장·미엘 PCM 복구 완료

Main07 전투목표에서도 폴 위치의 E/F/A 재도전을 허용, 첫대화 반복0·승리후차단. 필수DefeatEncounter10 전수 재도전/단일승리/Continue PASS(Main11 보스도망금지). 동료 EffectiveLevel=PlayerLevel, 고유값+기존직업식/수호자·치유사 동료만 승인된 공통+1 적용, 일반·임시Story공유·동료레벨Save0·Player성장보존. 미엘기존3 rawPCM복구(6.94/3.63/4.72초), 태온3변경0·신규TTS0.

Main06→07 Early7/PaulFirst→도망·패배재도전→정상승리→MielMeeting→완료Continue, 별도도망재도전정상승리 PASS. 고유label431PASS/FAIL0(반복기록617), 실제33PCM오차0, CompileError0/RuntimeConsoleError·Warning0/기존컴파일Warning16/새Warning0/MissingScript0·관련9Scene0. Lv1/3/5/10/20/50×4, EXP레벨업/실제BootstrapContinue/수동Party·Formation·Beast·HP/MP보존 PASS. 기존3274중6변경/3268byte보호, Early7/Paul20/Main03~06/Main17/244/원본/사용자Save보호. cleanBootstrap EditMode·비포커스·격리Save/Settings/Input원복.

사용자 확인: 복구미엘3 청취PENDING(자동의미청취PASS로승격0), 기존실물입력/물리Collision 별도. 다음권장: 보호된미청취목록·별도후속요청. [통합QA](Main07_Integrated_Retry_Growth_Voice_QA.md) · [전수Retry10](Story_Battle_Retry_Matrix.csv) · [동료24행](Companion_Growth_Runtime_Matrix.csv) · [Voice3](Main07_Miel_Recovery_Matrix.csv). 마지막 관련구현commit `ec6d3e5`(재도전)·`74bea14`(성장)·`907b2e4`(Voice), QA문서는 이 항목을 포함한 최신 Docs commit. GitHub Push0.

---

## 2026-10-07 Main07 Early7 적용 / Paul20 PCM 복구 완료

- Early7 정식Asset/Catalog 연결·RuntimePCM7PASS. Paul20 전수에서묶음/단편/번호밀림확인후기존PCM분리·재배치·Paul008단편결합. 원본/meta/GUID보존. **TTS_REGEN_REQUIRED0 / TTS API0 / 잔여3 소비0**.
- 실제Main06완료→Main07초반연속+Paul20Voice fixture **227PASS/0FAIL**,27PCM오차0,Paul0무입력10초유지/002재생0/Next1회page1.Compile/ConsoleError0·MissingScript0.
- 사용자복구001/002청취PASS. Early7+Paul18 사람청취PENDING25(기술검증과구분), 과거생성API코드미확인. 다음권장: 나머지25청취,동일구batchPaul후속Main09별도감사. 기존Main03~06/Main17/244/UserSave보호. [정본QA](Main07_Early7_Paul_Recovery_QA.md).
- 마지막관련commit `c231565`(Early7),`5ed20c2`(Paul20). QA/문서 최신Docs commit 참조. GitHub Push0.

## 2026-10-07 Main07 초반 ID·입력 경계 수정 / TTS 대기

- NPC7 Stable ID 추가, **TTS_REQUIRED7** handoff/manifest 준비. 사용자 첫 자막 유지 오발화 확인으로 **Paul001 TTS_REGEN_REQUIRED1** 분리. 기존WAV 변경0; 전체 Voice CLOSED 아님.
- 실제 Main06완료→Field02재진입/Main07시작→흔적4→부상자4→Paul0/1 연속 84PASS/FAIL0. 무입력 18.42초page0 유지/002재생0, 명시Next1회page1. custom E/A 같은프레임 재열림 수정. Compile/ConsoleError0·MissingScript0.
- 사용자 확인/다음작업: TTS 담당이 신규7 및 Paul001 재생성 후 전체 청취·승인하고 별도 반영. Paul002 본문청취 미검증1 유지. 기존Voice/244/Main17/사용자Save 보호. [상세QA](Main07_Early_Dialogue_Voice_QA.md).
- 마지막 관련 commit `c8de0ec`(ID7), `89ff828`(Main07입력경계). QA/문서 최신 Docs commit 참조, GitHub Push0.

## 2026-10-07 Main05 승인 신규 Voice5 및 연속 QA 완료

- Main05 신규WAV5 적용, 최신 사용자 청취/자동QA5/5, 전체6매핑·PCM PASS. 문제 **CLOSED**.
- Main04후속9→Guard6→독립대표8→공식해금→Main06Field02첫001, 입력 중복/재열림/경계 및 실제 Save/Continue3회 PASS. Runtime 239PASS/0FAIL, CompileError0, ConsoleError0, MissingScript0. 기존 CS0618경고16 별도.
- 다른Voice·Main17·Battle·Beast·원본·Save·GUID 보존, NEEDS_LISTENING244 유지. [상세QA](Main05_FinalVoice_QA.md).
- 사용자 직접 추가 확인 필수 없음(승인 원본 및 자동 검증 범위). 다음 권장: 보호된 기존 미청취 목록을 별도 작업에서 진행.
- 마지막 관련 구현 commit `c67d632`(WAV5), `f2d7e0c`(기존 입력수정). 최신 QA/문서 commit은 이 항목이 포함된 `Docs: Main05 Voice Dialogue 최종 QA 상태`를 참조. Push0.

## 2026-10-07 Main05 귀환 보고 감사 / 입력 결함 수정 / Voice 의미 대기

같은프레임 Enter+Space page0→2 및 A 종료→NPC 재열림을 실제 가상장치 입력으로 재현·수정했다. DialoguePresenter 실제입력용 Advance/열림·닫힘 프레임 경계와 InteractionSystem 입력진입점만 최소수정, 대사배열/StableID/Objective/Catalog/WAV 변경0. Main04전투후9→GuardReport6→독립대표Report8→정식합류→Main06첫조사001 한 Play 연속 **221PASS/FAIL0**, 24페이지 기록·Main05 6PCM 원본오차0·단일Source·조기진행/자동다음NPC0. Programmatic Advance 유지.

**전체 Voice 의미 해결은 아직 아니다.** 사용자 Main05의 Main04문장 오발화 증거 유지. 현재 Mapping6PASS와 개별 WAV 의미는 구분: 의미NOT_VERIFIED6, 후속태온2/미엘3 **재생성후보5**, 개별Wrong/확정TTS_REGEN_REQUIRED0(어느 미엘 자막인지 사용자가 기억하지 못함). 후보를 확정 재생성목록으로 자동승격하지 않았다. TTS생성/Clip교환0·NEEDS_LISTENING244 marker 유지·Main04late5/Main06이후/Main17Voice 변경0. [최종 감사·한계](Main05_Return_Voice_QA.md) · [Voice Matrix6](Main05_Voice_Matrix.csv) · [정확본문 후보5](Main05_TTS_Regen_Candidates.csv). 최신 근거는 이 문서이며 이전 일반metadata PASS는 의미판정이 아니다.

현재검증: 최종Compile Error0·기존CS0618 Warning16·이번파일새Warning0, 최종 Runtime Console Error/Warning0·MissingScript0, Bootstrap EditMode/is_focused=false·격리Save/Settings 해제·InputSettings native 정상 및 두 설정값복원. 기존3135파일 중 게임C#2만 변경/3133 byte보호. QA 초기 환경실패는 최종PASS와 구분해 상세문서에 남겼다.

다음권장: Main05 후보5 개별원본청취로 Wrong ID 특정 후 정확본문TTS 재생성/승인원본반영. 실물Keyboard/Gamepad와 물리출구접촉은 이번가상입력/Scene Load 검증과 구분한다. 마지막 관련 수정commit `f2d7e0c`; QA문서commit은 최신local log. GitHub Push0.

---

## 2026-10-07 Main04 후반 음성 5개 최종 반영 완료

현재 Main04 Late Sequence의 재생성 대기 **5→0**. 미엘009/010/011·태온006/007 모두 **REGENERATED / USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED**. 사용자 전체 원본 청취PASS와 이번 Runtime 기술 검증을 구분한다. 실제 Actor 9페이지·7Voice 전체재생/Next/Portrait/화자/본문, 새5개 PCM 오차0, 완료→Main05/완료전후 Bootstrap Continue, 동일프레임 Next2/단일Source, Player·지문 무음 PASS. **148PASS/FAIL0**, Compile Error0·최종Console Error0/Warning0·Field Runtime Missing Script0. 원본·meta·GUID·Catalog·대사 변경0; WAV 정확히5만 교체. 기존 NEEDS_LISTENING244 marker/Queue219 및 Main17 TTS_PENDING14 유지·일괄PASS승격0. [최종 QA](Main04_Late5_Final_QA.md)와 [현재5개 Mapping](Main04_Late5_Source_Mapping.csv)이 우선하며 아래 이전 Wrong/Suspect/대기5 표기는 역사 기록이다. 음성commit `0e7809a`; Push0.

현재 검증: clean Bootstrap EditMode / is_focused=false / 격리Save·Settings 해제, 기존2356파일 byte보호. 새 검증 관련 Editor helper만 추가. 이번5개에 사용자 추가청취 요구 없음; 실물 입력·Standalone 등 기존 별도 미검증은 유지. 다음 권장: 기존 Queue219 사용자 청취 또는 별도 요청 작업(Main17 TTS14 별도). 마지막 관련 구현commit `0e7809a`, QA/문서commit은 최신local log 참조.

---

> 미래 설계 참고(2026-10-06): [Chapter2 Main12~21·후반 방향](../03_스토리/Chapter2_서부_방향.md), [Chapter3 Main22 시작 방향](../03_스토리/Chapter3_초기_방향.md), [폴–미엘 관계](../04_등장인물/폴_미엘_관계.md)를 문서화했다. **Main18~22/Chapter3 미구현**이며 아래 실제 구현·QA 상태는 유지한다. 코드/Asset/Audio 변경 없음.

## 2026-10-06 Main17 실제 Art / Battle BGM 최종 반영

FUNCTION_COMPLETE / ART_COMPLETE / BGM_COMPLETE / SAVE_COMPLETE / PATH_COMPLETE. 정확한 English.zip 원본9PNG 등록·7실제적용(지면변형2보관), Preview미사용. PNG/ZIP/MP3 원본 수정0. Blade and Gambit는 Chapter2 서부 일반/Story에 적용, 기존 단일Source/Mixer Fade0.20/0.75/0.9·LoopON, Field07/08 탐색곡 복귀·Boss예외 유지. Quest12/보상60·50/Serin·Fox/Party/5Path 및 기존44승리·KO0 보호.

이번 고유 PASS270 / FAIL0 / 자동 NOT_VERIFIED0 / 진행 Blocker0. 기존309/180PASS 별도 재사용. 실제 통합 Story 승리/완료Continue 및 Field07/08 일반 BGM·Fade·3해상도 HUD 캡처 PASS. Transform425/Collider15 예상 밖 변경0, 원본3075 누락0·PNG/WAV 보호. Compile완료/Error0, 기존 CS0618 Warning16·이번 클래스 Warning0, MissingScript0. Bootstrap 비포커스 EditMode·격리Save/설정 해제.

남은 사용자/외부: TTS_PENDING14, 실물 Keyboard/Gamepad/Dungeon USER_INPUT_REQUIRED3, Field08/Battle 음악 USER_LISTENING_REQUIRED2. 기존 장비DF1/Standalone DR1 분리. 다음 작업은 청취·실물/Release 검증이며 TTS/Main18은 별도 요청 전 작업하지 않는다. 마지막 구현 commit Art3b4820a / Audio53ef42c, 상세 [최종 QA](Main17_ArtAudio_QA.md). GitHub Push하지 않음.

---

## 2026-10-06 Main17 TTS 제외 최종 마감 완료

기능·Story·Battle·Save·Path 완료, 확인된 진행 Blocker0. Main17 보상은 사용자 승인 EXP60·탈렌트50으로 최종 확정, 실제 마지막 대화/중복지급 방지/완료Save·Continue/레벨업경계 PASS. 5Job×Lv4/9/12/16×2조우40회+치유4회 모두승리·KO0, 몬스터 수치변경0. 이번 고유180PASS/FAIL0/NV0, 기존309PASS 별도 재사용. Compile완료·ConsoleError0/Warning0·MissingScript0·비포커스Bootstrap/격리Save해제.

외부대기: TTS_PENDING14·환경 ART_WAITING_EXTERNAL·Chapter2 Battle BGM AUDIO_WAITING_EXTERNAL. Main17에 신규Elite 불필요, Main18/Field09/Overheat 생성0. 실제 Scene/import 기반 Art 및 Cue/Mixer/Loop/Fade 계약 Audio Handoff 완료. 사용자확인: Keyboard/Gamepad/Dungeon접촉 USER_INPUT_REQUIRED3, 탐색곡 적합성 USER_LISTENING_REQUIRED1. 일반장비DF1/StandaloneDR1 별도 유지. 다음: 외부납품 이후 해당 교체 영향 QA·청취/실물검증, Main18은 요청 전 시작하지 않는다. 마지막 구현·검증 commit `42640f4`, 상태기록 commit은 최신log 참조. 기존103상태·PNG/WAV/Save 보존·Push0. [최종QA](MAIN17_FINAL_QA.md) · [Balance](Main17_Balance_QA.md) · [Art 인계](Main17_Field08_Art_Handoff.md) · [Audio 인계](Main17_Audio_Handoff.md).
## 2026-10-06 Main17 대형 개발 — Phase1~9 완료

main_17_red_rift / 붉은 균열 / Field_08_RedRift 12목표, Main16 이후 연결·조사·Story Battle·승리·Save/Continue5지점·BGM·Main18 Hook 구현. LOCAL Main15 세린 정식 해금과 저장 Party/Formation/Beast 유지. 통합246PASS/Path120PASS, 고유 CSV PASS309/FAIL0/NV0/USER4. 최종 Console Error0/Warning0·Missing Script0·비포커스 Bootstrap EditMode 복귀. 지형 기본 도형 Placeholder, NPC14 TTS_PENDING. 사용자 확인: 실물 Keyboard/Gamepad/Dungeon/BGM. 일반장비DF1/StandaloneDR1 유지. 다음: TTS 제작·청취 또는 사용자 지정 후속 개발. 기존103작업/PNG/WAV/Save 보호·Push0. 선행 관련commit1783cee, Phase9 hash 최신local log/최종보고 참조. [설계](../03_스토리/Chapter2_Main17_붉은_균열.md) · [최종QA](MAIN17_FINAL_QA.md) · [체크포인트](MAIN17_RESUME_CHECKPOINT.md).
## 2026-10-06 전투 하위 메뉴 개선·통합 영향 회귀 완료

BattleSceneController에서 스킬/아이템 열기 때 기본5명령 GameObject를 비활성화하여 표시·interactable·Raycast·Navigation을 함께 차단, 메뉴/대상/행동동안 유지·취소/뒤로/다음조작명령에서 복원한다. 기존Esc와같은 취소에 패드B도 연결했다. 기존812PASS 재사용(817 CSV/JSON 변경0), 이번고유검증484: **PASS482/FAIL0/NV0/USER2**이며 서로중복합산하지 않는다.

5직업 Skill/Item/무효대상/사용/포커스·뒤로/5회표본/3해상도6캡처, 정상일반공격·Guard·Flee·Victory·Continue, Main03/04저작Dialogue/Portrait/정본Clip→Story2/3인정상승리·복귀·Roster보존, 전멸SafeZone/Continue PASS. 자연Main전체/음성의미청취는재실행하지 않았다. Compile0/ConsoleError0·기존CS0618 Warning16·이번 helper 신규Warning0·cleanBootstrapEditMode/비포커스/격리Save해제. 사용자확인:실물Keyboard/Gamepad/Dungeon접촉 USER3, 일반장비DF1/Standalone재시작DR1·Voice219/Main045유지. 다음권장:잔여실물확인 또는 다음개발, Blocker0. 게임코드1외QA·문서만변경·PNG/WAV/Save/Settings보존·기존103작업유지·Push0. 선행관련5a4d05a, 이번commit최종보고. [계획·전체QA](LIMITLESS_BATTLE_SUBMENU_QA.md) · [고유CSV](LIMITLESS_BattleSubmenu_Cases.csv).

## 2026-10-06 2차 QA 후속 분류 완료

총817 유지·기존812PASS/FAIL0 재사용(재실행0). 남은NV3의 실제상태를 코드/Scene/설정/로컬산출물로 확인해 **DEFERRED_FEATURE1(일반장비장착 미구현), DEFERRED_RELEASE_VALIDATION1(Standalone 재시작), USER_INPUT_REQUIRED3(Dungeon물리접촉·실물Keyboard·Gamepad), NOT_VERIFIED0**로 분리했다. 상태 재분류이며 새 PASS가 아니다. 장비의 Inventory 수량저장/Beast장착저장과 일반장비 장착Save를 구분한다.

Dungeon의 Collider/Default·Untagged/비Trigger/Collision→Battle 연결 및 승리·도망 grace2초·복귀이격1.2 정적 확인, 실제물리접촉 미실행. 저장소 안에 게임Standalone산출물없음·Scene19존재/중복0·현재ConsoleError0. 전체Build/Play/포커스전환0. 사용자 직접확인: 접촉→Battle/도망후재접촉, 실물키보드/게임패드. 확인된개발Blocker0·다음개발가능, Standalone Build/실행/OS재시작 및 실물검증은 Release전필수. Voice219/Main04재생성5 제외·Asset/게임C#/PNG/WAV/Save변경0. 마지막관련QA `feeaa08`, 이번문서commit 최종보고참조·Push0. [잔여·다음개발3단계](LIMITLESS_SECOND_QA_DEFERRED.md).

## 2026-10-06 2차 종합 회귀 RESUME 완료

중단 전750 PASS를 재사용하고 MP0/침묵/Guard·전멸 보존·안전지역 Party·Main16 Hearing/Default·남은 전투 화면을 완료했다. 최신세부체크817: **PASS812 / FAIL0 / NOT_VERIFIED3 / USER_INPUT_REQUIRED2**. Main16 각75내부체크는 총수 중복 합산하지 않는다. 기존 전체QA/15 Skill/8 Dungeon/50 Mapping/Quest103을 처음부터 반복하지 않았다.

실제결함1: Field03/Arbel Party 확정 제한 및 Field03 다음 프레임 닫힘. 기존 허용지역3 정책을 Open/Update/Confirm에 공유하도록 최소수정, 실제 NPC 적용→Save→Bootstrap Continue/유지·취소 및 Field/Battle 차단 PASS. 13대표UI×3해상도39PNG 주요 글자·버튼 잘림 없음. Compile0/ConsoleError0·기존CS0618 Warning16·이번 helper 신규Warning0·종료 cleanBootstrap EditMode/비포커스/사용자Save·Settings보존. 보호3075파일 중Party코드1만변경, 기존PNG/WAV/TTS0.

사용자확인: 실물Keyboard/Gamepad 입력2. 미검증: 실제 장착 상태(미구현), Dungeon물리접촉, standalone OS재시작3. 다음권장: 해당 미검증만 보완, 별도Voice청취219·기존Main04재생성5 유지. 마지막관련Fix `adbbfbb`, 이번QA commit 최종보고 참조. 기존103작업보존·Push0. [전체 QA](LIMITLESS_SECOND_REGRESSION_QA.md) · [Resume](LIMITLESS_SECOND_REGRESSION_RESUME.md) · [체크 CSV](LIMITLESS_SecondRegression_Cases.csv).

## 2026-10-06 전체 Story Voice Semantic Risk 사전 감사

현재 Flow 재추출331 Dialogue / Voice247(Intro18+Story229) / Player Silent42. 최신 사용자·Runtime 의미PASS23(Main03 10·Main04 13)을 SHA/GUID/path 보호하고 Matrix의 태온005 경계PASS 누락을 보완했다. 기존 NEEDS_LISTENING244는 PASS20/기존재생성5/새청취219가 겹치는 관리 범위로 유지한다. 기존 Main04 Wrong1·Suspect4 재생성필요5 변화0, 신규 Wrong/Partial/재생성확정0.

Risk HIGH35/MEDIUM129/LOW83, 새Queue219는 HIGH30/MEDIUM129/LOW60. 저음량18·극단짧은길이12·다문장짧은음성15, PCM/교차Main/교차화자/ID중복·Missing·본문/Mapping drift0. Risk는 청취 우선순위이며 의미판정이 아니다. [전체 감사/임계값/Main별표](StoryVoice_SemanticRisk_Audit.md) · [자연 진행 Queue219](StoryVoice_ListeningQueue.csv) · [위험 구간 우선 Queue219](StoryVoice_PriorityListeningQueue.csv) · [보호PASS23](StoryVoice_Protected_PASS.csv). 다음 권장: Main09(27Voice/HIGH7)→Main11(28/HIGH7) 실제 전체발화·음량 청취, 결과ID를 모아 별도 재생성 요청. 기존Main04 5개는 기존Handoff 별도 유지.

새 Editor 감사529검사/합성10case/독립247WAV·과거PASS23해시 검증PASS. Compile0/Console Error0. Bootstrap Edit Mode/is_focused=false 유지, 포커스/Play전환0. 보호 기존2260파일 변경0; TTS생성·WAV/meta/Catalog/Story/Registry/Mixer/Portrait/게임C# 변경0. 사용자 직접 확인: Queue 음성 의미 청취 미수행. 기존 작업103개 유지·직접 감사/CSV/문서만commit·Push0. 마지막 선행 관련commit `85592e54d3109b1ccbb031c9deba7d1d9509b2bc`, 이번commit 최종보고 참조.

## 2026-10-06 수정 완료 Voice 개발 기록 MP4

새 `F:\Downloads\Limitless_DevJourney_2026-10-06\Limitless_DevJourney_FixedVoice_2026-10-06.mp4` 완료. **5분28.13초 / 1920×1080 / 30fps / H264 / AAC48kStereo / 49.42MiB**. 공식 Recorder GameView+GameAudio 원본으로 Title·Intro·성별/길/직업 및 Sprite Preview·Main03후반·Main04첫조우·실제 전투 승리·결과 버튼 Field복귀를 담았다. 정상 새 격리 게임으로 진행했으며 미검증 구간은 녹화 밖 정상 진행했다. 첫 Main04 패배 후 정상 회복/재도전 승리를 사용하므로 구간 컷이 있으며 무편집 연속 플레이가 아니다.

**Intro001~006 6개 + Character16개(Main03태온008~012, Main04미엘001~007·태온001~004)** 최종MP4 AAC 원본 일치22/22. 실제 전투 BGM도 정합. 기존 사용자 Runtime 청취 PASS 원본만 사용했고 신규 사람청취를 주장하지 않는다. Subtitle/Portrait/Player무음/UI/Voice순서·cutoff 검사 PASS, 전체decode오류0/검은화면0/최종A/V길이차4.33ms/끝Field정상. 실제 스킬·적 행동·정상 명령11회 승리/버튼 복귀, 이후 NPC대화0. 미엘008은 전투후대화라 제외, 미해결5Voice 진입0 및 TTS_REGEN_REQUIRED5 유지.

게임 기능/수치/Quest/강제승리 조작0. 기존Unity관련3,048파일 해시 전부 동일, 임시 도구 제거 및 Package원래바이트복원, 사용자Save/Settings·Sprite·WAV보존. 종료 cleanBootstrap EditMode/컴파일오류0/포커스전환0. 정리 중 ImportWorker충돌2건은 동기Refresh 후 재컴파일완료·ConsoleError0으로 회복 확인했다. GitHubPush/기존영상덮어쓰기 없음.

[전체 녹화 QA](LIMITLESS_DevJourney_FixedVoice_2026_10_06.md) · [최종 AAC23행](LIMITLESS_DevJourney_FixedVoice_2026_10_06_AAC_QA.csv). 사용자 직접 확인: 최종MP4 시청. 다음 권장 작업: Main04전투후5Voice 재생성·별도Runtime청취. 마지막 선행 관련commit `162ae1eeb36f03589306bddadb73891291450954`; 이번문서commit은 최종보고 참조.

## 2026-10-06 Main04 대기 지문 수정 및 전투 후 Voice 5건 재생성 전달

대기 문장 “상황을 살피고 있습니다.”는 `MainQuest04FieldFlow.CreateStoryActor`의 공통 NpcController 설정으로, Dialogue ID·Sequence index·Manifest·Voice entry가 없다. 태온 기본 상호작용에서 이름/Portrait가 붙던 상태 설명을 **`<지문> 상황을 살피고 있습니다.`**로 수정했다. DialogueLine의 명시적 prefix 규칙으로 Character Speaker·Portrait·Voice를 제거하며, 잘못된 Voice ID가 전달돼도 Resolve하지 않는다. Player와 기존 Character 대사는 유지한다. 331 authored Dialogue에는 이 대기 설정이 원래 포함되지 않아 개수는 유지한다.

Main01~16 Matrix 및 Flow의 행위 표현 자동 검색 후보 **6행(고유본문5개)** 중 확정 지문은 **고유본문1개 / NPC설정2곳**이다. 나머지4행은 실제 발화 문맥으로 유지했다. [정식 작성 규칙](../03_스토리/Story_대사_지문_작성_규칙.md)과 [후보 감사](Main01_16_Direction_Candidates.csv)에 근거를 기록했다.

미엘 `main04_miel_supp_009` / AfterBattleConversation index2의 정본은 “저도 볼 겁니다.\n이번에는 순서대로요.”다. Runtime·Manifest·정본 부록 일치. 사용자 Runtime 및 직접 원본 WAV 청취 “원본도 자막과 다름”으로 **WRONG_AUDIO_CONTENT 1건**을 확정했다. 추가 청취 답변 “순서가 뒤죽박죽이다”에 따라 인접 태온006·미엘010·태온007·미엘011 **4건은 SEMANTIC_INTEGRITY_SUSPECT**로 등록했다. 정확한 오발화 문장/개별 Wrong·Partial은 미확정이며 metadata로 의미 PASS를 추정하지 않는다. 실제 Clip/PCM 정합으로 Mapping·Cache 문제 재현0. **TTS_REGEN_REQUIRED 0→5**, 단일5행(Miel3/Taeon2) 전달. TTS 생성·WAV 수정·기존 파일 재배열0, **NEEDS_LISTENING244 유지**.

백그라운드 실제 대기 표시 및 Character→지문→Player 전환, 첫 조우10→전투 전3→Story Battle/QA 승리/실제 결과버튼 복귀→전투 후9 전체 **335개 기술 검사 PASS / FAIL0**. 전투 후7Voice 전체 원본PCM오차0/Player Voice·Portrait0/Next·Portrait 전환·Voice cleanup·UI Wrap/동적높이/본문·Footer 비중첩 PASS. 실제 지문 스크린샷도 prefix 전체·이름/Portrait 없음 확인. 기술 PASS와 위5건 의미 문제를 구분한다. 초기 QA의 단일 Show 창을 Next로 닫는 잘못된 기대는 기존 Esc/Hide 정책에 맞게 수정해 전체 재실행했고 초기 실패는 최종 PASS 수에 포함하지 않았다.

보호 대상 기존 3,046파일 중 변경은 Presenter 및 Main04 Flow 코드2개뿐이다. 새 Editor QA와 meta 추가, Main03/정상Main04첫9/후속004·007·008/태온005/전체WAV·meta·Catalog·Registry·사용자Save·설정 변경0. Compile Error0/Console Error0, 기존 CS0618 경고2개는 이력으로 구분한다. 종료 clean Bootstrap Edit Mode/격리Save 해제/is_focused=false/포커스전환0. 전략전투 조작 검증은 생략했다.

[13페이지 인접 순서](Main04_Direction_Miel009_Adjacent_Sequence.csv) · [전투 후9페이지 WAV/PCM 감사](Main04_Direction_Miel009_Sequence_Audit.csv) · [5행 TTS Handoff](Main04_Direction_Miel009_TTS_Handoff.csv) · [실제 Runtime 로그](Main04_Direction_Miel009_Runtime_Results.txt). 다음은5개 정식본문 새원본 제작 및 청취 후 별도 Unity 교체/연속검증이다. 선행 관련 commit `a5460c8f9843c9a800d628915e1da9872ed67285`, 이번commit은 최종보고 참조. GitHub Push 없음. 아래는 준비 당시 이력이다.

## 2026-10-06 Main04 지문 및 미엘009 원본 불일치 수정 계획

“상황을 살피고 있습니다.”는 MainQuest04FieldFlow.CreateStoryActor의 NpcController 공통 대기 설정이다. Dialogue ID·Sequence index·Manifest·Voice entry가 없으며 태온/미엘 두 Actor가 같은 문장을 공유한다. InteractionSystem→VillageNpcRole의 기본 대화 표시에서 태온 이름과 Portrait가 붙었다. Story 설계 Main04의 현장 관찰 문맥과 Flow의 공통 대기 분기를 근거로 상태 설명 지문으로 분류한다. 정식 표기는 `<지문> 상황을 살피고 있습니다.`이며 공통 DialogueLine 규칙으로 Character Speaker/Portrait/Voice를 제거한다. 문장 형태만으로 다른 대사를 자동 전환하지 않는다.

미엘 `main04_miel_supp_009`는 AfterBattleConversation index2이며 Runtime·Manifest·정본 부록의 “저도 볼 겁니다.\n이번에는 순서대로요.”가 일치한다. 사용자 Runtime 불일치와 이번 직접 WAV 청취 “원본도 자막과 다름”으로 WRONG_AUDIO_CONTENT를 확정했다. 틀린 실제 문장의 정확한 전사는 미확정이다. WAV/Mapping 재배열 없이 TTS Handoff를 작성하며 TTS 생성은 하지 않는다. 전투 후9페이지 전체와 대기 지문을 백그라운드 QA로 검사한다. 기존 정상 Main03·Main04첫9개·후속004/007/008 및 태온005를 보호한다. NEEDS_LISTENING244 유지. 선행 관련commit `a5460c8f9843c9a800d628915e1da9872ed67285`.

## 2026-10-06 Main04 후속 Voice 3개 최종 반영 및 연속 Runtime PASS

`Limitless_TTS_Regen_Main04_PostFirstEncounter_3`의 태온004·미엘007·008을 기존 Unity WAV 경로에 반영했다. 사용자 최종 지시 “맡겠습니다 로 해.”에 따라 태온004 본문은 “또 옵니다.\n제가 앞을 맡겠습니다.”로 유지했다. Runtime·Manifest·Handoff·제작 보고서가 일치하며 본문/Registry/Catalog 변경은 없다.

실제 첫 조우10페이지 종료 → 태온004 → 미엘007 → Player “갑시다.” → Story Battle → QA 전용 승리 처리/실제 결과 버튼 → Field 복귀 → 미엘008 → 태온005 경계까지 연속 실행했다. **221개 기술 검사 PASS / FAIL 0**, 새3개 전체 PCM 원본 오차0. 사용자 이번 답변 **“다 잘 들린다”**로 새3개와 태온005의 의미·전체발화·화자·음량 PASS를 확인했다. **TTS_REGEN_REQUIRED 3 → 0**, **NEEDS_LISTENING 244 유지**. 경계005 WAV는 교체하지 않았으며 NEXT_AUDIT_BOUNDARY_FAIL은 없다.

Player Voice/Portrait0, 태온·미엘 Portrait 복원, UI Wrap/동적높이/본문·Footer·Portrait 비중첩, 자연 종료 후 Next, Battle 전후 잔류 제거와 최종 Cleanup PASS. 전략 전투 조작 및 Main04 전체 Quest 완료 검사는 수행하지 않았다. 경계 페이지의 로그 NEXT 표시는 공통 QA 기록이며 실제 Advance는 호출하지 않고 검증 후 Hide로 종료했다.

보호 대상 기존 3,044개 파일 중 변경은 대상 WAV3개뿐이다. 정상 첫 조우9개/Main03/태온005/meta/GUID/Importer/Catalog/게임 C#/설정/사용자 Save 변경0. 새 Editor QA와 meta만 추가했다. Compile Error0 · Console Error0, 기존 CS0618 경고2개는 별도 이력이다. 종료 clean Bootstrap Edit Mode/격리 Save 해제/is_focused=false, 포커스 전환0.

[Source 및 Unity PCM 감사](Main04_PostFirstEncounter_Final_Source_Audit.csv) · [연속 Runtime 결과](Main04_PostFirstEncounter_Final_Runtime_Results.txt) · [전체 Sequence](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv). 다음 권장 작업은 범위 밖 Voice의 별도 청취 감사다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`, 이번 완료 commit은 최종 보고에 기록한다. GitHub Push 없음. 아래 준비 단계는 당시 이력이며 현재 판정은 이 단락을 따른다.

## 2026-10-06 본문 충돌 해소 및 새 Voice 3개 적용

사용자가 최종 답변 “맡겠습니다 로 해.”로 태온 004의 기존 정본 “또 옵니다.\n제가 앞을 맡겠습니다.” 유지를 지시했다. 중간 “막겠습니다” 의도는 이 지시로 대체되었다. Runtime·Manifest·Handoff·새 제작 보고서 본문이 일치하며 본문 변경은 없다. 새 WAV 3개를 기존 경로에 반영했고 연속 Runtime 검증 대기다. 의미 해결 및 TTS_REGEN_REQUIRED 3→0은 이번 Runtime 청취 확인 이후 판정한다. 첫 조우 9개/Main03/태온 005/meta/GUID/Catalog/Registry 보호.

## 2026-10-06 Main04 후속 Voice 3개 적용 준비

새 폴더 `Limitless_TTS_Regen_Main04_PostFirstEncounter_3`에 대상 3개가 제작되어 있다. 미엘 007·008은 Unity 반영 대기다. 태온 004는 사용자가 이번 답변에서 정식 본문을 “또 옵니다.\n제가 앞을 막겠습니다.”로 변경하는 의도를 확인했다. 현재 로컬 Flow·Manifest·Handoff와 새 제작 보고서는 “맡겠습니다”이므로 MANIFEST_RUNTIME_DRIFT에 앞서 사용자 최신 의도와 기존 정본의 충돌이 확인되었다. “막겠습니다” 원본을 준비하기 전 태온 004를 적용하거나 의미 PASS 처리하지 않는다.

문서화 → 구현 → 검증 순서로 진행한다. 첫 조우 정상 Voice 9개, Main03 전체, 종료 경계 태온 005 WAV, GUID·meta·Importer·Catalog·Registry를 보호한다. 연속 Runtime 완료 전 TTS_REGEN_REQUIRED 3 및 NEEDS_LISTENING 244를 유지한다. 선행 관련 commit `83d0b4fd684434a219d1c1a6f23d9445f6e4a0fa`.

## 2026-10-06 Main04 첫 조우 이후 SelfCare 경계 직전 Voice 3건 감사

직전 FirstConversation 10페이지 / 9 Voice의 사용자 Runtime PASS를 유지한다. 감사 시작은 `main04_taeon_supp_004` (EncounterConversation index 0)이다. 종료 경계는 `main04_taeon_supp_005` (AfterBattleConversation index 1), 태온의 “본인부터 보셔야 하는 것 아닙니까?”이며 **HANDOFF_INCLUDED=NO**이다. 범위는 EncounterConversation 3페이지 → Story Battle → AfterBattleConversation index 0까지다. 총 Dialogue 4개, Voice 3개(미엘 2 · 태온 1), Player Silent 1개이며 전투는 Dialogue 수에 포함하지 않는다.

사용자가 실제 Runtime에서 이 범위의 전반적인 자막/발화 불일치를 확인했다. Voice 3개를 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED 후보**로 일괄 등록했다. 각 WAV의 정확한 오발화 내용과 Wrong/Partial 개수는 개별 청취 증거가 없어 미확정이다. Metadata 정상만으로 의미 PASS를 확정하지 않는다. 기존 WAV를 재배열하지 않고 현재 Runtime 본문으로 새 원본을 제작한다. 재생성 목록은 **0 → 3**, 기존 NEEDS_LISTENING 관리 범위는 **244**로 유지한다.

[단일 3행 TTS 전달](Main04_PostFirstEncounter_PreSelfCare_TTS_Handoff.csv) · [전체 4행 Sequence 및 원본 감사](Main04_PostFirstEncounter_PreSelfCare_Sequence_Audit.csv) · [해시/PCM 교차 비교](Main04_PostFirstEncounter_PreSelfCare_CrossComparison.csv). 이번 작업의 WAV, Unity 코드, Catalog, 본문, Main03 및 정상 Main04 Voice 변경은 모두 0이다. TTS API 실행, Editor/Play 재실행, 포커스 전환도 없다. 마지막 관련 commit은 `ca103570983e18711184f4d63eaa5e8e396a2056`이며 이번 문서 commit은 최종 보고에 기록한다. 다음 작업은 이 3개 원본 제작 → 청취 채택 → 별도 반영 및 연속 Runtime 검증이다.

## 2026-10-06 Main04 첫 조우9개 새Voice 반영·전체Runtime 의미PASS

원본 `Limitless_TTS_Regen_Main04_FirstEncounter_9` 미엘6 Sulafat·태온3 Gacrux,제작원본9건USER_LISTENING_PASS.기존WAV9개전체바이트교체/Path·meta·GUID·Importer·Catalog·Speaker·본문유지. **사용자 이번 “9개 모두 Runtime 청취 PASS”** 확인으로전체9건 RUNTIME_LISTENING_PASS 확정.001 두문장 같은Line/다른위치이동0/한칸밀림0/Main03재등장0/다른화자0/부분누락0. **TTS_REGEN_REQUIRED9→0**,NEEDS_LISTENING244관리범위유지.

실제Main04 MielActor.TryInteract→FirstConversation10페이지(Voice9/Player1)전체연속검증 149 assertions PASS/FAIL0,TRACE9/NEXT10별도.9Clip전PCM원본오차0/SourceUnitySHA동일·Registry/Speaker/Manifest일치·Catalog247 ID/Clip중복0/Missing·Null0.각Clip자연종료후수동Next/Source정리·정확한다음Clip·Miel/TaeonPortrait/Player Voice0·Portrait0/Wrap·동적높이·본문/Footer/Portrait비중첩·최종Panel/Source/Portrait종료PASS.다음Main04전투Objective·격리자동Save읽기/첫대화완료countPASS.첫대화완료이지Main04전체Quest완료검사는아님.

[최종QA](Main04_FirstEncounter_Final_적용_QA.md)·[9개Source/Unity/PCM감사](Main04_FirstEncounter_Final_Source_Audit.csv)·[이번연속Runtime로그](Main04_FirstEncounter_Final_Runtime_Results.txt).보호 3044파일중변경은WAV9+기존EditorQA1(결과경로/캡처프레임대기/격리Save진행검사)뿐.게임C#/Main03전체12WAV/meta/Catalog/Save/Settings/Scene/Packages불변.CompileError0·최종ConsoleError0/Warning0,재컴파일기존CS0618경고2건은이력보존.종료cleanBootstrapEditMode/audit해제/Play옵션·설정복원/is_focused=false,포커스전환0.

다음권장:이번9건추가TTS불필요,244관리범위나머지Voice별도청취.마지막선행관련commit `4b1d2e1a8b3722e7a226add5862756ac2485a978`,이번완료commit은최종보고참조.원본폴더Stage0/직접변경만commit·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main04 첫 조우 Voice9개 전체 재생성 전달 확정

사용자 실제Runtime:001 첫문장만정상/두번째문장이다른위치에서발화,이후미엘·태온Voice전반적자막순서불일치/Main03태온처럼들리는발화재등장.9건모두 **SEMANTIC_INTEGRITY_SUSPECT / TTS_REGEN_REQUIRED**,001은기존직접WAV청취로 **PARTIAL_AUDIO_CONTENT** 추가확정.나머지각파일의틀린정확한문장/Character를추측하지않는다.이전Runtime Playback/PCM/148검사PASS는기술증거로유지하고의미PASS로사용하지않는다.

LOCAL FirstConversation10페이지/Voice9(미엘6 Sulafat·태온3 Gacrux)/Player1 정상무음.원문을Flow에서재추출하고현재Manifest와줄바꿈포함9건일치,본문Drift0.기존WAV재배열/Mapping수정없이9개전체정식본문재제작방향확정. **현재LOCAL 재생성필요1→9**(사용자요청의0→9는이전001등록전기준;001중복등록없이전체9). NEEDS_LISTENING244관리범위유지/Main03해결상태보호.

정본전달 [9행 Handoff](Main04_FirstEncounter_TTS_Regen_Handoff.csv),[WAV9개감사](Main04_FirstEncounter_WAV_Audit.csv),[Main03비교원본목록](Main04_FirstEncounter_Main03_Comparison_Inventory.csv),[교차비교](Main04_FirstEncounter_CrossQuest_Comparison.csv),[Main04내부비교](Main04_FirstEncounter_Internal_PCM_Comparison.csv).현재Main03 WAV12개와9×12=108쌍/첫조우내36쌍비교.동일SHA/PCM cross duplicate 0,내부PCM duplicate 0.내용이유사한발화는해시불일치여도배제할수없어사람청취증거와구분한다.

WAV/TTS API/Unity코드/Catalog/정식본문/Player/Main03Voice변경0.이번Editor/Play재실행0/포커스변경0,문서화→감사·전달생성→정적검증.다음:이9행으로온전한원본제작→개별청취→별도승인된반영작업에서기존GUID보존교체·연속Runtime검증.마지막관련commit `3ab8e67a71fc684d55796b012aade22ea943ad07`,이번문서commit은최종보고참조.아래는이전감사이력이다.

## 2026-10-06 미엘 첫 조우 전체10페이지 감사 / 원본001 누락 확정

[전체 QA](Miel_First_Encounter_Voice_QA.md)·[순서10행](Miel_First_Encounter_Sequence_Audit.csv)·[확정 TTS전달](Miel_First_Encounter_TTS_Handoff.csv).Main04 FirstConversation,미엘6/태온3/Player1.문제 main04_miel_supp_001 정식본문 `조금만 참으세요. / 출혈은 멎었습니다.` 중사용자Runtime와직접WAV모두첫문장만청취:CaseA PARTIAL_AUDIO_CONTENT.2초원본전체PCM48000오차0/자연종료/자동Next·조기Cleanup재현0.이번TTS생성/Playback수정/WAV수정0,정확한전체본문으로재생성전달.

연속148assertions PASS/FAIL0,9Voice전PCM정합/Player Voice0·Portrait0/MielPortrait·UI/최종Cleanup·첫대화종료→다음Encounter Objective·격리Save읽기PASS.의미정상확정0/Partial확정1/나머지8 USER_LISTENING_REQUIRED;Metadata정상으로의미PASS추정금지. **TTS_REGEN_REQUIRED0→1**,NEEDS_LISTENING244기존관리범위유지,Main03태온008~012완료보호.Compile/최종ConsoleError0·Warning0.3042기존파일해시변경0,새EditorQA만추가/cleanBootstrapEditMode/포커스전환0.

다음:미청취8건전체발화확인→확정누락001원본재제작→사용자청취/기존GUID보존교체·회귀.마지막관련commit `5e660dba9fac846932eb3e075735a22e19ff6fa9`,이번감사commit은최종보고참조.직접변경만커밋·GitHubPush없음.아래는이전이력이다.

## 2026-10-06 Main03 008~012 최종 반영·연속 Runtime 사람 청취 PASS

008/010/011 새 원본·009 **NeFix 최종본**을 기존 Unity WAV 내용만 교체.012는 지정 원본과 이미 동일하여 재작성0/해결유지.5건 Source 사용자청취PASS + 이번 사용자 **“5개 모두 Runtime 청취 PASS”** 확인: 의미·전체문장·음량PASS,009 `네.` 포함.**TTS_REGEN_REQUIRED4→0**, NEEDS_LISTENING244 기존관리범위유지.331 Matrix에 완료flag 기록.005/006/007/003/004 기존PASS와001/002 보호,Player 정상무음.

LOCAL Runtime/Manifest 본문5건 줄바꿈 포함일치·Source/Unity WAV SHA동일·실제Unity PCM5개 전샘플오차0.실제 첫Battle→QA전용승리종료→정식복귀→008→Player→009→Player→010→011→012→단서001→002→Main03종료 연속PASS.전략전투/물리입력검증 아님.기존 QA 도구로 PlayUnfocused·격리Save/Settings 사용,포커스전환0.기술검사 227 PASS/FAIL0(7 TRACE·9 AUDIO는별도),Player Voice0/Portrait0·태온복원·UI Wrap/동적높이/본문잘림0/Footer·Portrait비중첩·끝까지Next/빠른Next/최종정리/Quest완료/격리자동Save읽기PASS.

[최종 적용QA](Main03_Supp008_012_Final_적용_QA.md)·[5건 Source/PCM 감사](Main03_Supp008_012_Final_Audit.csv)·[이번 연속 실행로그](Main03_Supp008_012_Final_Runtime_Results.txt).보호 3042파일 비교:변경은 Unity008~011 WAV4개뿐,meta/GUID/Importer/Catalog/게임·Editor C#/Save/Settings/Scene/Packages불변.종료cleanBootstrap EditMode/audit해제/격리설정·Play옵션복원.CompileError0/ConsoleError0·Warning0 조회;C#변경없어강제재컴파일0.초기 준비스크립트 실행실패 후 조기QA를중단했고 WAV교체후전체재실행PASS,조기실행은최종증거에포함하지않음.

다음 권장:244 관리범위의미확인Voice를별도청취.이번5건추가TTS불필요.마지막선행관련commit `e355b44c9b2a87b7072d504d95db33560ebf7b7c`,이번완료commit은최종보고참조.직접변경만Stage,원본TTS폴더/다른WAV Stage0·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main03 008~011 사용자 Runtime 의미 불일치 확정·TTS 전달

사용자 실제 Runtime 청취에서 008~011 모두 자막과 발화 불일치 확인. 네 건 모두 **WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**이며 Metadata 정상 여부로 의미 판정을 되돌리지 않는다. **TTS_REGEN_REQUIRED 1→4**, NEEDS_LISTENING **244는 기존 관리 범위**로 유지(이번 확정 문제/이미 해결된 항목을 포함하므로 미확인 244건이라는 뜻 아님). 012는 새 WAV·사용자 직접 청취·Unity Runtime 의미/전체 문장/정상 음량 PASS 해결 유지. 005/006/007/003/004 PASS와 001/002 기존 상태, Player 정상 무음 보호.

LOCAL MainQuest03FieldFlow.AfterBattleConversation 및 DialogueLine의 구형 화자 접두어 제거 규칙에서 전체 표시 본문을 추출한다. 현재 Manifest와 네 건 모두 줄바꿈까지 일치. 실제 AfterBattle 페이지 순서는 1/3/5/6(index 0/2/4/5); Player 페이지를 제외한 전달 4행이다. 009는 `네.` 뒤 줄바꿈을 보존한다. Actual Spoken Text는 **USER_CONFIRMED_MISMATCH**로 기록하며 잘못된 문장을 추측하지 않는다. 이전 008 직접청취의 별도 증거는 보존한다.

정본 전달: [4행 Handoff](Main03_Taeon_Supp008_011_TTS_Handoff.csv), [원본/참조 감사](Main03_Taeon_Supp008_011_Voice_Audit.csv), [공통 연기/설정/검증](Main03_Taeon_Supp008_011_TTS_Handoff_QA.md). 기존 7행 Remaining Handoff는 원본 청취 감사 목록이며 새 재생성 전달에는 이 4행만 사용한다. 문서화→감사/전달 생성→백그라운드 정적 검증 순서. WAV/meta/Unity 코드/Catalog/설정 변경 및 TTS API/Push 0. 이번 Editor/Play 재실행 없음; 새 음성 제공 후 직접/Runtime 의미 청취가 다음 작업이다.

마지막 선행 관련 commit: `bd4bffeca1efb1789079197f3b215e19ddb0fe48`. 이번 문서 commit은 최종 보고 참조. 아래 기존 기록은 당시 이력이며 현재 판정은 이 단락과 최신 Matrix/재생성 목록을 따른다.

## 2026-10-05 Main03012 최종 반영·후반부 연속 QA

012 새원본7.76초/RMS−16.16dBFS를기존WAV에교체,GUID/meta/import/Catalog보존. 사용자원본/이번Unity Runtime **의미·전체문장·음량PASS** 확인,PCM186240개오차0.012저음량해결.008은직접WAV/Runtime의006계열발화로 **WRONG_AUDIO_CONTENT** 확정,이번실제008 Resolve/Source도008이며MappingFix없음. **TTS_REGEN_REQUIRED2→1(008)**,244 NEEDS_LISTENING원래관리범위유지,후반미청취5종유지. 기존005006007003004보호.

008→Player→009→Player→010→011→012→단서001→002→Main03완료를격리연속검증:무음213/실제재생227 assertions PASS/FAIL0,각AUDIO9/TRACE7별도. Player Voice0/Portrait0,태온Portrait/UI/Next/최종Voice·Portrait정리/Quest완료/격리자동Save읽기PASS. 사용자Save/Settings/포커스변경0. CompileError0/최종ConsoleError0·Warning0;기존deprecated2/MCP연결경고1이력보존. 새기능/게임C#변경0.

상세 [적용·최종 QA](Main03_AfterBattle_FinalVoice_적용_QA.md). 다음작업:008정확한 “역시 이상합니다.” 원본제공→청취/교체/회귀,009010011001002는일괄의미청취. 마지막관련구현commit `06666354eeaf9e2c328b058895751872b405ef88`. 최종문서commit은보고참조. 아래는이전감사이력이며최신판정은이단락을따른다.

## 2026-10-05 Main03 008 의미 오류 원인 확정

008 WAV 직접청취와 실제 Runtime 모두 사용자 확인 “그냥 돌아다니는 것 같지만…” 계열. 기대 “역시 이상합니다.”와 불일치: **Case B WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**. 실제008 페이지 Resolve/AudioSource는008이며006과 Path/GUID/reference/hash가 모두 다르다. 기존 재생PASS는 기술 결과이며 의미PASS 해석을 철회한다. C#/Mapping/WAV 변경0. 올바른008 새 TTS 대기. 확정 재생성 **2건(008 의미 오류 +012 저음량 별도)**; 의미 청취 대기6종. 기존244 관리범위 유지. 정상005/006/007/003/004 보호. 기존 Player Voice0/Portrait0 결과 유지. 사용자 Play/Scene/Save/포커스 변경0. Console Error0/Warning0 조회 확인, C# 변경 없어 강제 컴파일 없음. 상세: [008 원인 QA](Main03_008_VoiceIdentity_원인_QA.md). 이전 관련 commit9a82ff8, 이번 commit은 최종 보고 참조.

아래 이전 감사는 당시 기술 검증/미청취 이력이며008 최신 판정은 위 결과를 따른다.

# Project-Limitless 현재 개발 상태

## Main03 후반부 전체 Voice 감사·원본012 저음량 전달 (2026-10-05)

- 문서화→집중 QA 구현→백그라운드 검증. [후반부 감사QA](Main03_RemainingVoice_감사_QA.md)·[순서9행CSV](Main03_Taeon_Remaining_Sequence_Audit.csv)·[일괄Handoff7행](Main03_Taeon_Remaining_TTS_Handoff.csv). “역시 이상합니다.”=008/AfterBattle index0. LOCAL 실제순서는005~007→003/004→첫Battle→후속008~012/Player→단서001/002→Main03완료이며 요청의 “008이후 첫전투” 순서와 차이를 보고/기존Story 유지했다.
- 후속9페이지(Voice7/Player2) 전수: Mapping/Missing/재현PlaybackBug/자동TransitionCut0.008 Clip/IsPlaying/IsAudible/PCM/정상RMS−21.63 확인, 사용자 보고무음은 격리조건에서 미재현·근본원인 미확정.012 원본RMS−51.90/Peak−32.62dBFS·1.45초 저레벨 결함 확정, **TTS_REGEN_REQUIRED0→1**. 모든WAV/meta/Catalog/게임code수정0·TTSAPI0.
- 사용자 이번 묶음 재생은 듣지못함.7개 USER_LISTENING_REQUIRED, 다른6개 Wrong/Truncated 추정확정금지. Handoff는012확정재생성1+다른6의원본 의미확인으로 구분.005/006/007/003/004 이전사람 RuntimePASS 유지, NEEDS_LISTENING244 관리범위 유지.
- 실제Main03 전체16페이지: 최종무음202assertions/실제재생214assertions PASS·각PCM측정9/FAIL0. 실제Battle진입→QA승리종료/정식결과버튼복귀→후속전체/단서/Quest완료·Player Voice0/Portrait0·태온Portrait·UI/Footer비중첩·빠른Next/끝까지Next정리PASS. 전투조작/전략 검증 아님. 후반부7WAV 모든PCM샘플오차0.
- Compile Error0/최종 Console Error0·Warning0, 기존CS0618 2건이력보존. 보호3040기존파일해시변경0·사용자419Git항목보존·cleanBootstrap EditMode/격리설정복원/포커스전환0. 다음권장: Handoff7개 원본을한번에청취하여006같은오발화 여부확정→012정상레벨 온전한원본제공→기존ID/meta보존교체·회귀. 직접변경만커밋·GitHubPush없음. 아래는이전이력이다.

위 후반부 감사 helper/증거 보고 관련 commit `d743227`. 후반부 최종 문서는 별도 Docs commit으로 기록한다.

## 태온 후속003/004 Unity 반영·최종 사람 Runtime PASS (2026-10-05)

- 문서화→기존 WAV2개 전체바이트 교체→격리 검증. 입력 `Limitless_TTS_Regen_Main03_Taeon_Supp003_004/`·[적용/집중 QA](Main03_Supp003004_Regen_적용_QA.md).003 0.96초/RMS−19.73dBFS·004 4.36초/RMS−18.95dBFS, Unity PCM 모든 샘플 원본과 오차0. 기존 GUID/meta/Import/Catalog/Registry/Story/게임 C# 유지.
- METADATA_PASS·원본 USER_LISTENING_PASS·실제 Unity **RUNTIME_LISTENING_PASS** 모두확정. 사용자 “003·004 모두 의미 일치·끝까지 재생 PASS” 답변. **확정 TTS_REGEN_REQUIRED2→0**,005~007 기존 해결3→0 유지. NEEDS_LISTENING244 관리범위 유지(이번 해결2건은 원래 그부분집합이며 완료 flag 추가, 미확인 전체244라는 뜻 아님). 다른 음성 청취/재생성0.
- 무음83assertions/실제재생88assertions PASS·각 PCM측정2·FAIL0. 실제005→Player→006→Player→007→003→004→Battle, 끝까지 대기후 수동Next·자동발화절단0·Player Voice0/Portrait0·태온복원·현재UI 원문/Portrait/Footer 비중첩·Battle 잔류0. 명시적 조기Next는 기존skip정책대로 정리하며 Timing/Quest/Save 수정0.
- Compile Error0/최종 Console Error0·Warning0, 기존 CS0618 2건 재컴파일 이력 보존.331 Matrix/Catalog247·Missing0/중복0/Speaker·Manifest본문 mismatch0. 보호3041중 기존변경은 WAV2뿐·다른WAV/meta/005~007/Catalog/Mixer/Save/Settings/Scene/Packages/사용자419Git항목(새Source4포함)보존. clean Bootstrap EditMode·격리 설정복원·포커스전환없음.
- 다음 권장: 기존244청취 관리범위에서 이미PASS한003/004를 구분하며 나머지 원본을 별도 순서로 청취. 이번2건 추가TTS 필요없음. 직접변경만 커밋·GitHub Push 없음. 반영/집중 QA commit `1f9fa8f`, 최종 문서는 별도 Docs commit. 아래는 이전 이력이다.

## 태온 후속 Voice 원본 결함 확정·장문 Dialogue 수정 완료 (2026-10-05)

- 문서화→공통 Presenter 최소 수정→백그라운드 검증. [적용 계획/Voice·UI QA](Main03_FollowupVoice_DialogueLayout_계획_QA.md). 사용자005/006/007 실제 Runtime 의미 청취 PASS로 기존 TTS_REGEN_REQUIRED3→0 해결. 세 WAV 재수정 없음. 새 전투 전003/004 두 줄 모두 문제라는 추가 확인을 반영했다.
- 003(“옵니다.”) 원본1.10초 RMS−61.60/Peak−39.04dBFS,004(“제가 앞을 막겠습니다. 뒤를 부탁드리겠습니다.”) 원본1.65초는 사용자 청취에서 다른/부분 문장. 모두 G. OTHER 원본 결함이며 Manifest/Catalog/Clip/GUID/Speaker 정상. 실제 재생 완료 전 자동 전투 전환 없음. 새 TTS 생성/가공/Mapping·Quest·Save 수정0, [다음 제작 전달2건](Story_Dialogue_TTS_REGEN_REQUIRED.csv)을 기록했다. 현재 재생성2/NEEDS_LISTENING244(새2건 포함), 아직 음성 해결 완료 아님.
- 기존 Legacy Text/Font/CanvasScaler 유지. Speaker/Body/Footer 분리, Wrap·내용 기반 높이(최소23%/최대40%)·본문28~24·NPC Portrait/Player 전체 폭 정상. 상한을 넘는 본문은 Mask와 스크롤로 끝까지 접근하며 원문 축약0/Player Portrait 생성0이다.
- 격리 PlayUnfocused550 assertions PASS/FAIL0 + PCM측정2건(로그 count552): 실제005→Player→006→Player→007→003→004→Battle/Voice 정리와 Player Voice0/Portrait0,3해상도(1920×1080/1600×900/1280×720)×22 Layout·선택창 비중첩, 현재 Story 최장10개·24개행 스크롤 마지막줄 접근 확인. 대표 배경 캡처 시각 검수. 실제 물리 입력장치는 미검증. Compile Error0/최종 Console Error0·Warning0, 기존 CS0618 2건·MCP 연결 경고 이력 보존.
- 보호3041기존파일 중 변경은 Presenter/기존 Editor QA2개뿐. WAV/meta/Catalog/Mixer/Save/Settings/Scene/Packages/사용자415Git항목 보존. clean Bootstrap EditMode·격리 설정/해상도 복원, 화면 포커스 전환 없음. 다음 작업은003/004 온전한 원본 제공→실제 청취→기존 ID/meta 보존 교체·회귀. GitHub Push 없음. 관련 커밋은 완료 기록 참조. 아래는 이전 이력이다.

마지막 재컴파일: Compile/Console Error0·기존 CS0618 Warning2(ExternalAssetImportEditor 53/59행). QA 종료 당시 Warning0과 구분한다. UI/집중 QA commit `284cbf2`.

## 태온 첫 조우 Voice3개 교체 진행 (2026-10-05)

- 입력 `Limitless_TTS_Regen_Main03_Taeon_005_007/005.wav·006.wav·007.wav` 원본/사용자 실제 청취 PASS, PCM24kHz/mono/16bit·4.04/7.16/6.52초. [적용 계획과 집중 QA](Main03_Taeon_Voice_Regen_적용_QA.md)을 먼저 기록하고 기존 Unity WAV3개만 전체 바이트 교체했다. GUID/meta/Importer/Catalog/게임코드/Mixer/다른Voice 유지, 실제 Unity PCM과 새 원본 모든 샘플 차이0. 백그라운드57checks/실제 재생45checks PASS, Compile/최종Console Error0/Warning0. **사용자 Runtime 의미 청취 확인 대기**, TTS_REGEN_REQUIRED3/NEEDS_LISTENING244 유지. 격리 QA는 Bootstrap으로 종료했고 이후 사용자 Field01 Play/Save 진행은 중단·되돌림 없이 보존한다. GitHub Push하지 않는다.
- 관련 WAV교체/집중 QA commit `191e817`. 남은 완료 조건은 사용자 Runtime 청취 결과 확인이며 확인 후에만 재생성 필요3→0으로 해결 기록한다. 기존 전체331개Runtime 증거와 이번 집중57/45검사를 구분하며 전체Story를 다시 재생하지 않았다.

## Story Dialogue 전수 감사·Player 중앙 정책 적용 (2026-10-05)

- 문서화→구현→격리 백그라운드 검증. [전수 QA](Story_Dialogue_Consistency_QA.md)·[Matrix](Story_Dialogue_Audit_Matrix.csv)·[LOCAL 원문 부록](../03_스토리/LOCAL_Story_Dialogue_원문_부록.md). 전체331페이지(Intro18/Main01~16 313), Player42, Voice247(Story229+Intro18). 기존 문서 exact23/본문미기재308을 구분해 구현 원문을 명문화했으며 기존 게임 기획/발화 본문 변경 없음.
- 구형 Main03의 Player4페이지가 태온 ID/Portrait를 사용하던 원인을 중앙 DialogueLine/Presenter/PortraitCatalog에서 수정. 모든 Player stable ID player·Voice NONE·Portrait NONE, 화자 mismatch4→0·잘못된 Player Portrait4→0. Named Portrait 태온/미엘/폴/세린 정상, 레온 등 미제작은 기존 text-only 유지. 개별 Flow/Save 구조 변경 없음.
- 실제 사용자 청취로 **태온 첫 만남 005/006/007 음성 불일치3** 확인. 006은005의 문장을 말하고 007은006 첫 문장만 말하고 종료. 005 실제 발화 원문은 미확정. Manifest/파일명/GUID/해시는 맞아도 원본 발화가 틀린 상태다. [TTS_REGEN_REQUIRED3개](Story_Dialogue_TTS_REGEN_REQUIRED.csv)와 사용자 증거를 기록. 나머지244개 NEEDS_LISTENING. **음성 정합성 해결 완료 아님**: 이번 TTS 생성 금지 조건에 따라 WAV/Catalog를 그대로 보존했으므로 현재 잘못된 원본 연결은 남아 있다.
- 기술 Mapping247/247·Manifest본문 mismatch0·Missing/Unexpected Audio0·Duplicate ID/Clip/Branch Collision0. 격리 PlayUnfocused **2574checks PASS/FAIL0**: Story313개 자막/ID/Clip/Portrait 전수, Player42개 정리, NPC↔Player/연속Next/Scene전환/VoiceVolume/Mute, Intro18개참조·Next8회/Skip정리. 전체 Story 수동 플레이/나머지244개 실제 발화 청취는 미실행. CompileError0·최종ConsoleError0/Warning0, 기존 CS0618 이력2건 유지.
- 사용자 승인으로 이전 Play 종료 후 격리 QA, 종료 clean Bootstrap EditMode·is_focused false. 보호3032파일 중 기존 변경은 중앙 UI코드2개만, WAV/meta/Catalog/Mixer/Save/Settings/Packages/Scene 및 기존410Git항목(94그룹)보존. 직접 변경만 Stage/commit, 작업 diff--check 검사, GitHub Push 없음. 계획 commit `af48c2e`, Runtime/QA commit **`08b07ad`**.
- 다음 권장: 정확한 현재 본문으로3개 원본 재제작/제공 → 실제 청취 채택 → 기존 ID/meta를 보존하여 교체 → 격리 회귀 QA. 아직 듣지 않은244개는 별도 청취. 이번 감사에서 잘못된 Voice에 자막을 맞추거나 추측 Shift Mapping하지 않았다. 아래는 이전 이력.

## LOCAL 전체 회귀검증 완료 (2026-10-05)

- [종합 QA](LIMITLESS_CURRENT_FULL_REGRESSION_QA.md)·[보존 실행 로그](LIMITLESS_FullRegression_Runtime_2026_10_05.txt): **21범주 PASS18/FAIL0/NOT_VERIFIED3**. 신규 격리244체크 PASS, Main09 내부115/Main10 내부63 PASS는 wrapper와 중복합산하지 않음. Title 수정 회귀11PASS. 기존 Main15/16·Audio/Voice·Dungeon/Boss QA는 현재 Runtime/데이터 SHA 보존을 확인하고 범위를 명시해 재사용.
- PlayerSprite **READY50/BLOCKED0**, 50 Mapping/Job Preview·800 Sprite·대표남녀 Confirm/World/실제 Bootstrap Continue/Battle Left Idle 정상. PNG/meta/ID/Audio/Catalog/사용자Save/Settings 불변. Main01–16 총16 Quest/103 Objective 참조·순수 규칙 감사와 Main09/10 실제 단계/편성/Continue/Battle/Dungeon 왕복 정상. Main13–15 Path 반응형 retrofit는 계속 설계후보이며 Main16 실제 Path 분기는 구현 상태 유지.
- 실제 최소수정: Renderer2D의 활성 Missing 디버그 리소스6개 정상복원(`1e0a957`), Title 다시보기 버튼 하단 Outline 여백(`80d2f0b`, 1920×1080/1600×900/1280×720 경계·Replay/Next/Skip 정리 PASS). Editor QA의 오래된 미래Quest Seed/석재 재사용/던전 미구현 기대값만 보정(`deb181c`). Quest/Save/Party/Battle 기능·수치 변경0.
- Serialized 검사12242객체/31Scene: Missing Script0, 현재 Runtime 활성 Missing Sprite/Audio/Material/기타 참조0. Build Scene19 Missing0. 과거 Milestone 백업Scene의 카메라 target 누락5와 제거된 Renderer 필드 YAML1행은 실행 범위 밖 이력으로 보존. 컴파일Error0/최종ConsoleError0·Warning0. 재컴파일에서 기존 ExternalAssetImportEditor의 CS0618 경고2건은 별도기록/원본보존.
- 남은 확인: J 스킬15종 실제UI/취소/타깃 전체, N Dungeon B1/B2 일반8조우 전승리, T Party/Pet/Settings 포함 주요 모든UI 다중해상도 시각 QA. 사람 음성청취 품질·모든 물리 입력장치·배포 Player 저장은 미검증. 정식 BGM/Voice 기술연결 정상, SFX 재생은 미구현이며 Chapter2Battle BGM TBD 정책 유지.
- clean Bootstrap Edit Mode·Audit Save/Settings/Play옵션/runInBackground 복원. 임시 Recorder/녹화스크립트 제거 및 manifest/lock 원본복원. 사용자410 Git항목(기존94그룹)보존·직접변경만 커밋·Push 없음. 다음권장작업은 위 J/N/T의 남은 검증이며 새 게임기획/Art 재가공 없음. 아래는 이전 이력.

## Player Sprite low-alpha QA 정책 정정/전체READY50 (2026-10-05)

- 문서화→QA helper구현→재판정/검증. [정책](../11_UI/Player_Sprite_QA_Policy.md)·[Mobility개별QA](../11_UI/Mobility_LowAlphaPolicy_QA.md). 이전non-zero Alpha 일괄차단은가시적오류와저Alpha노이즈를구분하지못한오탐. 가시적Crop/분리조각/침범/방향/장비손실은계속BLOCKED, 작은비가시Alpha1–16노이즈는비차단경고. 큰면적/긴선/반복/실루엣영향은별도검토.
- Mobility9종만재판정READY9·LOW_ALPHA_NOISE186성분(최대Alpha1–16/최대33px/bbox변14px). **전체READY41/BLOCKED9→READY50/BLOCKED0**. HealerFemale 포함기존READY41종 재판정없음/PNG/meta/Entry/QA/기존328Clip동일. PNG50수정0/Stage0·기존144SpriteID/AppearanceID/Save/fallback유지. 기존9Entry에만72Clip추가, 전체400Clip.
- Tools/SpriteQA helper: MaxAlpha/성분면적/bbox/본체거리/반복·경계후보 기록, 시각검수 없는자동PASS금지. 회귀11PASS/0FAIL(A-E,셀내절단/큰면적/긴선/반복/실루엣/빈Frame/검수누락). 게임/Editor C#변경0.
- 이전85PASS Runtime 재실행없음. 최소회귀81Mapping/Clip참조PASS·Preview/World Frame0참조9정상·PlayUnfocused BattleLeftIdle9PASS. Save→Continue는이전실행이력+StableID/정본Mapping/코드불변으로보호하며이번실제전체흐름반복없음. MissingSprite0·현재컴파일Error0/최종ConsoleError0/Warning0.
- 검증중RuntimeBattle해석기를EditMode호출해Destroy오류9건발생/기록. 임시객체9개정리후properPlay검사통과, 게임코드변경없음. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 사용자410Git항목보존. 직접변경diff--check확인·GitHubPush없음.
- 계획commit `d7b830f`, 반영commit `aabfe3d`. 다음권장: 별도요청시 실제걷기/입력 통합QA(이번미검증). 더필요한Mobility Art수정목록없음. 아래는정정전이력.


## Mobility v2 redownload9종 반영/엄격QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Mobility_Path_9_Restored_v2_redownload.zip`. 문서화→원본9PNG교체→검증. PNG9/Male5/Female4/정확9조합·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128. 이미지수정0. [계획](../11_UI/Mobility_RestoredV2Nine_적용_계약.md)·[개별QA/최소수정리스트](../11_UI/Mobility_RestoredV2Nine_QA.md).
- 144Frame 전체시각/Alpha검사: 이전가시적검/장비/바퀴아래조각 모두해소, 머리/휠체어/바퀴/방향/디자인 정상. 하지만사용자StrayPixel0을Alpha>0 고립성분까지적용하면9종에최대Alpha1-16의미세잔여픽셀이남음. **대상READY0/BLOCKED_ART9·전체READY41/BLOCKED9 유지**. 다른41종/HealerFemale은재판정하지않음. 상세Frame/좌표/픽셀수/Alpha를다음이미지세션최소수정리스트로기록.
- 기존9AppearanceID/144SpriteID/meta/import/Gender+Path+Job/Save/fallback보존. 다른41PNG/CatalogEntry/QA·기존328Clip 동일, 신규Clip0. 사용자410Git항목보존. 게임기능C#변경없음, QAHelper대상/대표만조정.
- 격리PlayUnfocused85PASS/0FAIL: 대상9Mapping/Preview, 대표남녀Fighter World/BattleLeftIdle/Save→BootstrapContinue의기존성별fallback유지. BLOCKED원본의실행직접표시는승격전보류. 전체800Sprite Missing0·대상import/rect/pivot 정상. 옛ID재계산/기본fallback 유지.
- 컴파일Error0·최종ConsoleError0/Warning0, 기존CS0618경고2건별도기록. cleanBootstrap/EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 작업diff--check확인, GitHubPush없음. 계획commit `e2d3620`, 반영commit `b6b5d35`. 아래는이전이력.

## Mobility 최종 복원10종 반영/QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Mobility_Path_10_Restored_Final.zip`. 문서화→원본10PNG교체→검증. PNG10/Male5/Female5/Job5×Gender2·512×512RGBA/4×4/16Frame/Cell128·중복0/누락0. 이미지수정0. [계획](../11_UI/Mobility_RestoredFinal10_적용_계약.md)·[개별QA/최소수정리스트](../11_UI/Mobility_RestoredFinal10_QA.md).
- 160Frame 실제검수: Mobility READY1(HealerFemale)/BLOCKED_ART9, 전체READY40/BLOCKED10→**READY41/BLOCKED9**. 잔여검/장비/바퀴아래 조각은0-basedFrame/셀좌표로기록. 다음이미지세션은해당9종의최소수정리스트만처리.
- 기존10AppearanceID/160SpriteID/meta/import/Gender+Path+Job/SaveMapping/fallback 유지. 통과Entry에8Clip추가, 다른40PNG/Entry/QA 및기존320Clip 동일. 게임기능C# 변경없음, QA helper 대상Path만Mobility로 변경. 사용자411Git항목 보존.
- 격리PlayUnfocused88PASS/0FAIL: 10Mapping/Preview·대표MaleFighter 기본fallback 및FemaleHealer 원본Preview/World/BattleLeftIdle/Save→Continue 통과. 전체800Sprite Missing0. 옛ID재계산/기본fallback 유지. 컴파일Error0/최종ConsoleError0/Warning0. 기존CS0618 2건/MCP재연결경고1건 별도기록.
- cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드 미검증. 직접변경diff--check 확인·GitHubPush없음. 계획commit `32f74ef`, 반영commit `e83df1b`. 아래는이전이력.

## Hearing v3 복원10종 READY 반영 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_v3.zip`, 문서화→구현→검증. 원본10PNG 바이트교체, Male5/Female5/Job5×Gender2·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128 확인. Sharpshooter파일은기존Marksman ID/경로에연결. 이미지/C#기능수정0. [계획](../11_UI/Hearing_RestoredV3Ten_적용_계약.md)·[개별QA](../11_UI/Hearing_RestoredV3Ten_QA.md).
- 새160Frame검수 **Hearing READY10/BLOCKED0·전체READY30/BLOCKED20→READY40/BLOCKED10**. v2 검/지팡이/장식/발/머리위 조각 및SharpshooterFemale Up12/14 머리절단해소, 귀장치/방향정상. 낮은Alpha미세가장자리성분은가시분리파츠와구분하며세부QA에공개. 다음이미지세션최소수정리스트 없음.
- 기존10AppearanceID/160SpriteID/meta/Import·Gender+Path+Job/SaveMapping/fallback 보존. 기존10Entry에만80Clip추가. Mobility 포함다른40PNG/Entry/QA·기존READY30/240Clip동일. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory12파일뿐·모든C#/Save/Settings불변, 사용자94항목(410Git 파일)보존.
- 격리PlayUnfocused88PASS/0FAIL: 10Mapping/JobPreview 및대표MaleFighter/FemaleHealer의최종Preview/World/BattleLeftIdle·실제Battle/Save→BootstrapContinue v3Sprite직접표시통과. CharacterCreation선택전기본Preview·구버전ID재계산/기본fallback통과. 전체800Sprite Missing0·대상160Sprite rect128/pivot64,0/Multiple/PPU128/Point/Uncompressed 정상.
- 컴파일요청완료·Error0/최종ConsoleError0/Warning0, 이전CS0618이력보존. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.
- 다음대상준비: 남은Mobility10종 BLOCKED 그대로보존. 원본ZIP 수령후적용계획/QA를시작할준비상태이며이번작업범위에포함하지않았다. 계획commit `dac6a9a`, 마지막 v3 원본반영·READY승격·검증commit bb0c8e8. 아래는이전이력.

## Hearing v2복원10종 원본 반영·QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_v2.zip`. 문서화→구현→검증 순으로Hearing10PNG 원본바이트교체. Male5/Female5/Job5×Gender2·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128 확인. 이미지수정0. [계획](../11_UI/Hearing_RestoredV2Ten_적용_계약.md)·[개별QA/Frame/좌표](../11_UI/Hearing_RestoredV2Ten_QA.md).
- 새160Frame 실제검수에서 **READY승격0/BLOCKED_ART10, 전체READY30/BLOCKED20 유지**. 이전큰발조각/일부머리위선개선. v2잔존/신규: Fighter발밑/셀옆조각, Guardian검파츠, Healer지팡이/머리위선, Mage장식/지팡이조각, Sharpshooter발/머리위선·FemaleUp12/14 머리수평절단. 귀보조장치존재/방향유지확인. 정상음파효과와구분했으며수정판명칭으로승격하지않음. 다음작업은개별QA 좌표의원본Art보정.
- 기존Hearing10AppearanceID/160SpriteID/meta/Import·Gender+Path+Job/SaveMapping·fallback 보존. Mobility 포함다른40PNG/Entry/QA·READY30/240Clip동일. 새Clip0, 사용자94항목(410Git 파일)과Save/Settings 보호. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory12파일뿐.
- 격리PlayUnfocused 88PASS/0FAIL: 10Mapping/JobPreview·대표MaleFighter/FemaleHealer의CharacterCreation/최종Preview/World/BattleLeftIdle·실제Battle/Save→Continue·구버전ID재계산/fallback 통과. BLOCKED이므로표시는기본성별fallback이며신규HearingArt직접표시검증완료로보고하지않음. 전체800Sprite Missing0·대상160Sprite rect128/pivot64,0/Multiple/PPU128/Point/Uncompressed 정상.
- 컴파일요청완료·현재Error0/Warning0·직전CS0618 2건이력보존, 최종Console Error0/Warning0. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.
- 계획commit `d9eb01e`, 마지막 v2 원본반영·QA·검증commit c568299. 아래는이전이력.

## Hearing 최종복원10종 원본 반영·QA 완료 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Hearing_Path_10_Restored_Final.zip`. 문서화→구현→검증 순으로Hearing10PNG 원본바이트교체. Male5/Female5/Job5×Gender2·중복0/누락0·512×512RGBA/4×4/16Frame/Cell128 확인. 이미지수정0. [계획](../11_UI/Hearing_RestoredFinal10_적용_계약.md)·[개별QA/Frame/좌표](../11_UI/Hearing_RestoredFinal10_QA.md).
- 새160Frame 실제검수에서 **READY승격0/BLOCKED_ART10, 전체READY30/BLOCKED20 유지**. Fighter발밑/머리위 조각, Guardian셀옆검은파츠, Healer발/이웃지팡이 조각, Mage이웃지팡이/장식조각, Sharpshooter발/머리위/이웃파츠잔존. 정상음파효과와구분했으며수정판명칭으로승격하지않음. 다음작업은개별QA 좌표의원본Art보정.
- 기존Hearing10AppearanceID/160SpriteID/meta/Import·Gender+Path+Job/SaveMapping·fallback 보존. Mobility 포함다른40PNG/Entry/QA·READY30/240Clip동일. 새Clip0, 사용자94항목(410Git 파일)과Save/Settings 보호. Assets/Settings/UserData 변경은10PNG+Catalog+Inventory+QAhelper13파일뿐.
- 격리PlayUnfocused 88PASS/0FAIL: 10Mapping/JobPreview·대표MaleFighter/FemaleHealer의CharacterCreation/최종Preview/World/BattleLeftIdle·실제Battle/Save→Continue·구버전ID재계산/fallback 통과. BLOCKED이므로표시는기본성별fallback이며신규HearingArt직접표시검증완료로보고하지않음. 전체800Sprite Missing0·대상160Sprite rect128/pivot64,0/Multiple/PPU128/Point/Uncompressed 정상.
- 컴파일Error0·기존CS0618경고2건보존, 최종Console Error0/Warning0. cleanBootstrap EditMode·Save/Settings/Play설정복원·포커스전환없음. 실제걷기/키보드/게임패드미검증. 직접변경diff--check통과, 기존사용자whitespace보존. GitHubPush없음.
- 계획commit `5130489`, 마지막 원본반영·QA·검증commit 6b4699b. 아래는이전이력.

## Vision512 최종 복원3종 반영 완료 (2026-10-05)

- `F:/Downloads/Limitless_Vision_3_Restored_512_Final.zip` 지정 Vision Fighter Male·Mage Female·Sharpshooter Male 원본3PNG 교체. 문서화→구현→검증, PNG3/중복0/누락0·512×512 RGBA·4×4·16Frame/Cell128 확인. 이미지 추가 수정0. 직전1254입력 보류 기록과 구분하여 새48Frame 실제QA. [계획](../11_UI/Vision_Restored512Final3_적용_계약.md)·[최신 QA](../11_UI/Vision_Restored512Final3_QA.md).
- **3종 모두 READY 승격**, Fighter 검 끝·Mage Up 머리/지팡이 접합·Sharpshooter Up12/15 머리·Up13 망토 복원 통과. 남은문제Frame 없음. 전체 **READY27/BLOCKED23 → READY30/BLOCKED20**. 이번3종 추가Art 수정 필요 없음, 다음 권장작업은 범위밖 나머지BLOCKED20의 기존QA 수정.
- 기존3 Appearance ID/48Sprite ID/meta/Import·Gender+Path+Job/Save Mapping·fallback 유지. 기존3Entry에만24Clip 연결. 다른47PNG/Entry/QA·기존READY27/216Clip 보존. 사용자 기존94항목(410Git 파일) 보호, Save/Settings/ProjectSettings 불변. gameRuntime 기능/SaveMigration 변경0, QA helper의 대상 범위만3종으로 조정.
- 격리PlayUnfocused90PASS/0FAIL: 3종Mapping/Preview/World/Battle Left Idle/실제Battle Scene/Save→Bootstrap Continue·CharacterCreation 성별기본Preview·구버전ID 재계산/기본fallback 통과. 전체800Sprite Missing0·48Sprite rect128/pivot64,0·Multiple/PPU128/Point/Uncompressed 확인.
- C# 컴파일 완료·Error0. 기존 CS0618 경고2건 보존, 최종Console Error0/Warning0. cleanBootstrap EditMode·Save/Settings/Play 설정 복원·포커스 전환 없음. 실제걷기/키보드/게임패드 미검증.
- 이번 변경 diff --check 통과, 기존 전체작업트리 whitespace는 수정하지 않음. GitHub Push 없음. 계획commit `30a3419`, 마지막 원본 반영·READY 승격·검증commit `39220c2`. 아래는 이전이력이다.

## Vision 복원 최종3종 입력 규격 불일치로 보류 (2026-10-05)

- 입력 `F:/Downloads/Limitless_Vision_3_Restored_Final.zip`, Vision Fighter Male·Mage Female·Sharpshooter Male 지정3PNG/중복0/누락0/RGBA 확인. 문서화 선행 후 검사했으나 **모두1254×1254**로 필수512×512·4×4·Cell128 규격 실패. [계획](../11_UI/Vision_RestoredFinal3_적용_계약.md)·[최종 QA](../11_UI/Vision_RestoredFinal3_QA.md).
- 게임 자산 교체0/승격0, **대상3종 BLOCKED_ART 유지·전체 READY27/BLOCKED23 유지**. 기대 READY30/BLOCKED20 미달성. 기존 문제 Frame: Fighter Male5/7/9/11 검 끝, Mage Female12-15 접합, Sharpshooter Male12/13/15 머리/망토. 새 입력 정식48Frame QA는 규격 불일치로 보류.
- 모든50PNG/meta/Entry/기존216Clip·다른47종·READY27·사용자 기존94항목(410개 Git 상태) 보존. Assets/ProjectSettings/UserData 해시 변경0. 기존 Appearance/Sprite IDs·Import·Mapping·Preview·Save/fallback 코드 변경0.
- Unity 읽기검증 전체800Sprite Missing0, 기존3종16Sprite씩/rect128/Multiple/PPU128/Point/Uncompressed. 새 복원본 Preview/World/Battle/Save Play QA·새 전체 컴파일 미수행. C# 변경0·현재 Console Error0/Warning0·clean Bootstrap Edit Mode, 포커스 전환 없음. 직전 CS0618 2건 보존.
- 다음 필요한 작업: 동일3종512×512 RGBA·4×4·Cell128 원본 ZIP 확보 후 교체/재검증. 직접 문서 diff --check 통과, 전체 diff --check는 기존 사용자 whitespace로 실패하며 보존. GitHub Push 없음.
- 관련 계획 commit `e58f432`, 마지막 게임 반영 commit `dbbfb54`. 이번 형식 불일치 기록은 후속 문서 commit에 포함한다. 아래는 이전 이력이다.

## Intellectual/Vision Restored8 원본 반영 완료 (2026-10-05)

- `Limitless_IntellectualVision_8_Restored.zip` 지정8PNG 원본 바이트 교체. PNG8/중복0/누락0·512×512 RGBA·128px×16 확인, 이미지 수정 없음. [계획](../11_UI/Intellectual_Vision_Restored8_적용_계약.md)·[8종 개별 QA](../11_UI/Intellectual_Vision_Restored8_QA.md).
- **READY승격5 / 대상 BLOCKED_ART3, 전체 Ready22/Blocked28 → Ready27/Blocked23**. READY: Intellectual Fighter Female·Mage Female/Male, Vision Guardian Male·Sharpshooter Female. 잔존: Vision Fighter Male5/7/9/11 검 끝, Vision Mage Female12–15 머리/지팡이 접합, Vision Sharpshooter Male12/13/15 Up 머리/망토. 다음 권장 작업은 이3종의 정확한 QA 문제Frame Art 보정.
- 다른42종 PNG/meta/Entry/QA/기존176 Clip 보존. Intellectual Fighter Male READY·Hearing/Mobility/EmotionalScar 불변. 기존8 Appearance ID/128 Sprite GUID/fileID·Import·Gender+Path+Job/Save Mapping 유지. READY5종 기존 Entry에만40 Clip 추가. 사용자 Save/Settings 보존, Runtime 기능/Save Migration 없음.
- 격리 PlayUnfocused86PASS/0FAIL: 대상8 Mapping/Preview·READY 대표 Intellectual Mage Male/Vision Guardian Male의 실제 Restored World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 통과. 전체800 Sprite Missing0·geometry 오류0. QA helper 대표 조합3줄만 변경, C# 컴파일 완료·Compile Error0. 기존 CS0618 경고2건, 최종 Console Error0/Warning0.
- clean Bootstrap Edit Mode·Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기·키보드/게임패드 미검증. 직접 변경 staged diff --check 통과·기존 사용자 변경/whitespace 보존. GitHub Push 없음.
- 관련 commit: 계획 `24e72d0`, 마지막 원본 반영·READY 승격·검증 `dbbfb54`. 이 상태 기록은 후속 문서 commit에 포함한다. 아래는 이전 이력이다.

## Intellectual/Vision 8종 셀 픽셀 정밀 수정 완료 (2026-10-05)

- Cleanup v2 원본 기준8종만128px 셀 단위로 정밀 수정. 생성형 재작업/보간/리사이즈/방향 변경 없음. 정상84셀 RGBA Hash·Pixel Diff0, 허용44셀 중41셀 수정·34셀 정수1–2px 이동, 전체87셀 그대로 보존. [계획](../11_UI/Intellectual_Vision_CellCleanup_적용_계약.md)·[개별 QA/남은 프레임](../11_UI/Intellectual_Vision_CellCleanup_QA.md).
- 분리 성분341px 제거 및 이동 밖 Alpha1–2 잔여124px 별도 기록. 원본의 머리·리본·검·망토 절단 단면은 이동만으로 복원되지 않아 **READY승격0/BLOCKED_ART8·전체 Ready22/Blocked28 유지**. 다음 권장 작업은 잘리기 전 완전한 원본 파츠 확보와 해당 문제 셀 복원. 남은 프레임·좌표는 QA 참조.
- 다른42종 PNG/meta/Entry/QA/Clip 및 Intellectual Fighter Male READY 보존. 기존8 ID/128 Sprite GUID/fileID·Import·Gender+Path+Job/Save Mapping 유지. Assets/ProjectSettings/UserData 시작 해시에서8PNG+Catalog+Inventory 총10파일만 변경, 사용자 Save/Settings 보존. C#/Runtime 기능 변경 없음.
- 격리 PlayUnfocused86PASS/0FAIL: 대상8 Mapping/Preview·대표 Intellectual Mage Male/Vision Mage Female World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 통과. BLOCKED 기본 성별 fallback 검증. Missing Sprite0·전체800 Sprite geometry 오류0, 최종 Console Error0/Warning0·현재 Compile Error0. 새 전체 C# 컴파일 미수행.
- clean Bootstrap Edit Mode·격리 Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기·키보드/게임패드 미검증. 직접 변경 staged diff --check 통과, 기존 사용자 whitespace 보존. GitHub Push 없음.
- 관련 commit: 계획 `c3a58e1`, 마지막 Art 수정·검증 `5ac5abb`. 이 상태 기록은 후속 문서 commit에 포함한다. 아래는 이전 이력이다.

## Intellectual/Vision Cleanup v2 8종 반영 완료 (2026-10-05)

- `Limitless_IntellectualVision_8_Cleanup_v2.zip` 지정8PNG만 원본 바이트로 교체. PNG8/중복0/누락0·512×512 RGBA·128px×16 확인. [적용 계획](../11_UI/Intellectual_Vision_CleanupV2_적용_계약.md)·[개별 QA/남은 프레임](../11_UI/Intellectual_Vision_CleanupV2_QA.md).
- 기존 큰 조각은 제거됐지만 머리·리본·망토 crop와 일부 인접 조각 잔존으로 **READY 승격0/BLOCKED_ART8, 전체 Ready22/Blocked28 유지**. Vision Fighter Male 방향 정상 유지. 다음 권장 작업은 QA 표의 원본 crop 윤곽 복구와 잔여 조각 제거.
- 다른42종 PNG/meta/Entry/Clip·판정과 사용자 변경 보존. Intellectual Fighter Male READY·Hearing/Mobility 불변. 기존8 ID/128 Sprite GUID/fileID·Import·Gender+Path+Job Mapping/Save/fallback 유지. Assets 전체에서8PNG+Catalog+Inventory 총10파일만 변경. Runtime 기능/C# 변경 없음.
- 격리 PlayUnfocused86PASS/0FAIL: 대상8 Mapping/Preview·대표 Intellectual Mage Male/Vision Mage Female World/Battle Left Idle/실제 Battle Scene/Save→Bootstrap Continue 통과. BLOCKED는 기존 기본 성별 임시 fallback. 첫 AssetDB 갱신시각 불일치2건은 동기 Import 후 전체 재검증으로 해소. 최종 Console Error0/Warning0·Missing Sprite0·전체800 Sprite geometry 오류0. 새 전체 C# 컴파일 미수행·현재 컴파일 오류0.
- clean Bootstrap Edit Mode·격리 Save/Settings/Play 설정 복원·포커스 전환 없음. 실제 걷기/키보드/게임패드 미검증. 직접 변경 파일의 staged diff --check 통과; 전체 diff의 기존 사용자 whitespace는 보존. GitHub Push 없음.
- 관련 commit: 계획 `4c9563a`, 마지막 Sprite 반영·검증 `501c0c8`. 이 상태 기록은 후속 문서 commit에 포함한다. 아래는 이전 작업 이력이다.

## Intellectual/Vision Cleanup8 반영 완료 (2026-10-03)

- 지정8종만 Cleanup ZIP 원본 반영. [계약](../11_UI/Intellectual_Vision_Cleanup8_적용_계약.md)·[QA/문제 프레임](../11_UI/Intellectual_Vision_Cleanup8_QA.md). 문서 `b73d949`, 반영 `745a913`; 검증 기록은 이 항목을 포함한 후속 commit.
- 대상 READY0/BLOCKED_ART8, 전체 **Ready22/Blocked28 유지**. crop/분리 조각이 남아 승격 없음. 다음 권장 작업은 QA의 문제 프레임 원본 미술 수정.
- 기존42종 PNG/meta/참조/판정 및 사용자94항목 보존. 기존8 ID/128 Sprite ID/import/Mapping/Save/fallback 유지. Runtime·다른 기능 변경 없음.
- 격리 PlayUnfocused86PASS/0FAIL, 대상8 Preview/Selection·대표 World/Battle Left Idle/Save→Continue·save-restore 통과. Console Error0/Warning0, Bootstrap Edit Mode·설정 복원. 새 C# 컴파일 미수행, 기존 deprecated 경고는 이전 이력 유지.
- Foreground/실제 걷기·키보드/게임패드 입력 미검증. 작업 diff --check 통과; 전체 diff에는 기존 사용자 whitespace가 남아 수정하지 않았다. GitHub Push 없음. 아래는 이전 이력이다.

## Intellectual3 / Vision5 V2 스프라이트 반영 (2026-10-03)

- `Limitless_IntellectualVision_8_Fixed_v2.zip`의 지정8종만512×512 RGBA 원본 그대로 교체했다. Target8/Duplicate0/Missing0·추가PNG0. 원본ZIP/추출PNG·이전8PNG/Git 이력 보존. 문서 선행 `6def064`, 반영 commit `35ea4ab`. [계약/기존8 ID](../11_UI/Intellectual_Vision_V2Eight_적용_계약.md)·[8종 개별 QA](../11_UI/Intellectual_Vision_V2Eight_QA.md).
- V2 READY0/Blocked_ART8·전체 **Ready22/Blocked28 유지**. Intellectual Mage Male 큰 조각은 제거됐으나 frame9/10에2–3px 잔여 조각. Vision Fighter Male Left 방향 일치, 잔여 조각으로 Art 보류. 다른6종도 머리 절단/인접 조각/Cell끝 잔여픽셀이 남는다. 다음 작업은 QA 표의8종 문제Frame 원본 미술 수정이다.
- 기존8 Appearance ID·128 Sprite GUID/fileID·meta/Import·Gender+Path+Job Mapping·Save/fallback 유지. 다른42종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory/QA 동일. 특히 Intellectual Fighter Male의 READY·PNG/meta/8 Clip은 교체/재판정하지 않았다. 기능/퀘스트/UI/음성/BGM/전투/Save 변경 없음, 기존 QA 도구의 대상/대표만8종으로 보정.
- 격리 PlayUnfocused86 PASS/0 FAIL: 대상8 UI Mapping/Preview, Intellectual Mage Male·Vision Mage Female 대표 생성/World/Battle Left Idle/Save→Bootstrap Continue 통과. Blocked의 기본 성별 임시 fallback 검증이며 Ready Art 재생은 아니다. Compile/Console Error0·최종 Warning0(컴파일 직후 기존 deprecated 경고2). clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원.
- Foreground/GameView 선택/포커스 전환/물리 입력 없음, 실제 걷기 품질 미검증. 관련 diff 검사 통과, 기존 사용자 변경 보존. GitHub Push 없음. 아래 기록은 이전 작업 이력이다.

## Intellectual4 / Vision5 Revised9 스프라이트 반영 (2026-10-03)

- 지정9종만 `Limitless_Intellectual4_Vision5_Revised.zip`의512×512 RGBA 원본 바이트 그대로 교체했다. 원본ZIP/추출PNG와 이전9PNG·Git 이력 보존. 문서 선행 `c66b43c`, 반영 commit `d4e0093`. [적용 계약](../11_UI/Intellectual_Vision_Revised9_적용_계약.md)·[9종 개별 QA](../11_UI/Intellectual_Vision_Revised9_QA.md).
- Intellectual Fighter Male READY 승격, 다른8종 BLOCKED_ART. Vision Fighter Male Left5/7 방향 오류는 해결됐으나 잔여 조각으로 Art 보류. 전체 **Ready22/Blocked28**. 남은8종의 문제 Frame·절단/조각을 QA 표에 기록했으며 다음 작업은 해당 원본 미술 수정이다.
- 기존9 Appearance ID·144 Sprite GUID/fileID·meta/Import·Gender+Path+Job 자동 Mapping·Save/fallback 유지. 승격1종의 Idle/Walk8 Clip만 Catalog에 추가. 다른41종 PNG/meta·Catalog Entry/Sprite/Clip·Inventory/QA 동일. 기능/UI/퀘스트/음성/BGM/전투/Save 코드 변경 없음; 기존 QA 감사만 승격 대상과 기대 집계 검사 보정.
- 이번 승인된 격리 PlayUnfocused89 PASS/0 FAIL: 대상9 UI Mapping/Preview, 수정판 Intellectual Male Fighter와 fallback Vision Female Mage의 생성/World/Battle Left Idle/Save→Bootstrap Continue 통과. Ready8 Clip key/16Frame·6fps/loop 검사. Compile/Console Error0·최종 Warning0(컴파일 직후 기존 deprecated 경고2). clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원.
- GameView 선택/창 활성화/포커스 전환/물리 입력 없음. 실제 걷기 품질은 미검증. 관련 diff 검사 통과, 기존 사용자 변경 보존, GitHub main 조회·Push 없음. 아래 기록은 이전 작업 이력이다.

## Intellectual4 / Vision5 Player Sprite 부분 수정 (2026-10-03)

- 정확히9종 교체: Intellectual Fighter/Mage 남녀4개는512×512 원본 그대로, Vision Fighter Male/Guardian Male/Mage Female/Marksman 남녀5개는 이번 사용자 승인으로1254×1254 원본을 보존한512×512 최근접 변환본 적용. 문서 선행 `5b1de7e`, 마지막 관련 기능 commit `7c65c78`. [입력·9조합 ID 계약](../11_UI/Intellectual_Vision_Fixed9_적용_계약.md)·[QA](../11_UI/Intellectual_Vision_Fixed9_QA.md).
- 기존9 Appearance ID·144 Sprite GUID/fileID·meta/Multiple/PPU128/Pivot/Point/Uncompressed·자동 Mapping/Save 스키마 유지. Duplicate0/Missing0. 다른41종 PNG/meta·Catalog/Sprite/Clip·Inventory/QA 동일. Hearing10/Mobility10 BLOCKED_ART 유지, Voice/BGM/Main16 변경 없음.
- 144Frame 관찰: Empty0·시트별 Duplicate0, Intellectual PASS0/Blocked_ART4, Vision PASS0/Blocked_ART4/Blocked_DIRECTION1(Fighter Male Left5/7이 Right 방향). 머리 절단·인접 잔여 조각이 남아 **전체 Ready21/Blocked29 유지**. 다음 작업은9종 QA의 문제 프레임 원본 미술 수정이다.
- 격리 PlayUnfocused89 PASS/0 FAIL: 대상9조합 실제 UI 자동 Mapping/Preview, Intellectual Male Fighter·Vision Female Mage 생성/World/Save→Bootstrap Continue/Battle Scene 전달 및 이전 ID 재계산 통과. 미술 Blocked의 기본 성별 임시 fallback 검증이며 Ready Art 재생은 아니다. 실제 걷기·물리 입력·Foreground는 미검증이다.
- Compile/Console Error0·최종 Warning0, 보호880파일 중 의도한11개만 변경. clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션/백그라운드 설정 복원, 포커스 조작 없음. 관련 diff 검사 통과, 기존 사용자 변경 보존, GitHub Push 없음.

## Mobility Player Sprite 수정판 교체 (2026-10-03)

- 제공 ZIP의512×512 RGBA 캐릭터 시트10개(Male5/Female5·5Job×2)를 기존 Mobility PNG 경로에 원본 바이트 그대로 반영했다. 참고 Preview.png는 제외, ZIP·원본 미술 변환/복원/재생성 없음. 문서 선행 `8cb64bb`, 마지막 관련 기능 commit `3179685`. [계약](../11_UI/Mobility_Player_Sprite_Fixed_적용_계약.md)·[10종 QA](../11_UI/Mobility_Player_Sprite_Fixed_QA.md).
- 기존 Appearance ID10개·Catalog/자동 Gender+Path+Job Mapping·Save 구조와160 Sprite GUID/fileID·meta/Import 설정 유지. Duplicate0/Missing0. 다른40종의 PNG/meta Hash·Catalog Entry/Sprite/Clip/QA·Inventory 동일, Hearing 재판정 및 Voice/BGM/Main16 변경 없음.
- 160Frame 관찰: Empty0·시트별 Duplicate0·본체4방향/휠체어 디자인 확인. 10종 모두 인접 머리/바퀴/무기 조각 또는 Up 머리 상단 절단이 남아 **Mobility Ready0/Blocked_ART10, 전체 Ready21/Blocked29 유지**. 원본 미술에서 각 Cell128px 안에 완전한 캐릭터를 배치하고 잔여 조각을 제거하는 것이 다음 권장 작업이다.
- 격리 PlayUnfocused 감사86 PASS/0 FAIL: 남녀10조합 실제 Path/Job Preview/Mapping/fallback 표시, Male Fighter·Female Sharpshooter 생성/World/Save→Bootstrap Continue/Battle Scene 전달 통과. Blocked의 기본 성별 임시 fallback 검증이며 수정판 Ready Art 재생은 아니다. 사람의 걷기 품질·키보드/게임패드·Foreground는 미검증이다.
- Compile Error0·최종 Console Error0/Warning0(컴파일 직후 기존 deprecated 경고2). 보호880파일 중 의도한12개만 변경. clean Bootstrap Edit Mode·격리 Save/Settings/Play 옵션/백그라운드 설정 복원, 포커스 전환 없음. 관련 diff 검사 통과, 기존 사용자 변경 보존, GitHub Push 없음.

## Hearing Player Sprite 수정판 변환·교체 (2026-10-03)

- 사용자 승인으로 원본 ZIP1254×1254 PNG10개를 보존하고 별도512×512 최근접 변환본을 기존 Hearing10종 경로에 반영했다. 원본 미술 재생성·복원·재배치·Alpha 보정 없음. 문서 선행 `f412331`, 교체·감사 `961e3a7`. [계약](../11_UI/Hearing_Player_Sprite_Fixed_적용_계약.md)·[QA/10종 문제 프레임](../11_UI/Hearing_Player_Sprite_Fixed_QA.md).
- Male5/Female5·각5Job×2, Duplicate0/Missing0. 기존10 Appearance ID·Gender+Path+Job Mapping·Save 구조와160 Sprite GUID/fileID·Import 설정 유지. 다른40종 PNG/meta Hash와 Catalog Entry/Sprite/Clip/QA 상태 동일, BGM/Voice/Main16 변경 없음.
- 변환본160프레임 관찰 결과 Hearing Ready0/Blocked_ART10. 인접 셀 조각·Right 발 아래 다음 행 머리·Up 머리 잘림·일부 무기/망토 절단이 남아 **전체 Ready21/Blocked29 유지**. 수정판이라고 PASS하지 않았으며 Runtime은 명시적 기본 성별 임시 fallback을 유지한다. 다음 작업은128px Cell마다 캐릭터/장치를 온전히 배치하는 원본 미술 수정이다.
- Hearing 전용 격리 PlayUnfocused 감사96 PASS/0 FAIL:10조합 실제 UI Mapping/Preview/Story, 대표 남녀 생성/World/Save→Bootstrap Continue/Battle Scene 전달 통과. 수정판 Art의 Runtime Ready 재생은 하지 않았다. 사람의 실제 걷기 품질·물리 입력·Foreground는 미검증이다.
- 최종 Compile/Console Error0·기존 deprecated 경고2, clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원. 보호880파일 중 의도한12개만 변경했다. 관련 diff 검사 통과, GitHub Push 없음.


## Player Sprite 자동 조합 매핑 설계 정정 (2026-10-03)

- 정식 규칙을 **Gender + Path + Job → 대응 Sprite 자동 결정**으로 정정했다. 성별/이름→Path→Job→Confirm 흐름을 유지하며 수동 Appearance 필터·이전/다음·목록·확정·번호 UI를 제거했다. Preview는 결과를 자동 갱신한다. Path Theme는 실제 Path 디자인이고 Job 추천은 선택 제한이 아니다. 아래 독립 Appearance 선택 기록은 폐기한 이전 구현 이력이다.
- Male25/Female25·Path별10·Job별10, Stable ID Mapping50 / Duplicate0 / Missing0. Marksman→sharpshooter 등 메타데이터 이름만 대응한다. [50조합 Matrix](../11_UI/Player_Sprite_Combination_Matrix.md)·[정식 계약](../11_UI/Player_Appearance_선택_Save.md).
- RuntimeReady21 / ArtBlocked29 유지. Blocked도 정확한 조합 ID를 기록하며 기본 성별 Sprite의 명시적 임시 fallback을 사용한다. 다른 Path/Job 또는 공통 Mobility Sprite로 대체하지 않는다. PNG50·Sprite800·Catalog·기존 Animator 재Import/재Slice/재생성 없음, 보호282파일 SHA256 동일. [기존 Blocked 프레임/사유](../11_UI/Player_Appearance_Foreground_QA.md).
- Save 정본 Gender/Path/Job/Name을 유지하고 AppearanceId는 계산 결과다. Continue는 누락·invalid·이전 수동 ID 불일치도 정본에서 재계산한다. Path/Job 없는 Version1은 기본 성별 fallback, 사용자 Save 삭제·초기화 없음. World Clip Override와 Battle Left Idle은 같은 결과를 사용한다.
- PlayUnfocused 격리 감사 **346 PASS/0 FAIL**, 실제 UI50조합·대표5Path 생성/World/Save→Bootstrap Continue/Battle Scene 전달·구버전 복원·Story 실제 Path 판정 통과. 백그라운드 UI Render **7 PASS/0 FAIL**, 1280×720·800×600 기본 정보/Job/Confirm 확인. Compile/Console Error0, 기존 deprecated 경고2. [검증·한계](../11_UI/Player_Sprite_Combination_QA.md).
- 사용자 직접 확인: 물리 키보드·게임패드/foreground 미검증. 기존800×600 최종 확인 중앙/능력치 패널 겹침과 직업명/상세 간격은 후속 UI 작업 대상. 다음 권장 작업은 Blocked29 원본 미술 수정·재검수와 최종 확인 반응형 레이아웃 정리다.
- 마지막 관련 commit: 문서 `98dd23a`, 자동 Mapping/UI `dc9feff`, Save/Continue `6102952`. clean Bootstrap Edit Mode·격리 Save/Settings·Play 옵션 복원. BGM/Voice/Main16 및 기존 사용자 변경 유지, GitHub Push 없음.


## Main16 Audio 마무리 — Field07 탐색 BGM (2026-10-03)

- Where the Earth Breathes를 원본 MP3(44.1kHz stereo/192kbps/180.17초) 그대로 `Audio/Music/`에 Import하고 기존 Catalog에 Field07 한 항목만 추가했다. 탐색·조사·불씨망령 전투 후 잔향·협곡 입구에 같은 곡을 쓴다. 정책 `f0a7129`, 마지막 관련 기능 commit `012be7d`.
- 전투 적용 최초 승인 응답은 사용자가 철회했고, 재확인은 **기존 유지: Field07 전투 null/TBD**다. Field07 일반/Story Battle은 음악을 정리하며 복귀는 새 탐색곡이다. Steel and Sunlight는 기존 Chapter1 일반전에 유지한다. 후보 Trail of the Ember Wraith·Beneath The Cracked Earth는 Downloads 보관·Runtime 미적용, 기존 Chapter2 소개/서부/Deep West 정책 유지.
- Main16 전용 격리 Audio 감사 최종217 PASS/0 FAIL: Title·Arbel·Field06↔07 실제Exit·단일 Source/Listener·Save→Continue·일반 Battle 진입/복귀·실제 Story 공격 Victory/복귀·패배 복귀 API·잔향 대사·Hearing/Default44 Dialogue/28 Voice 재생·Next/종료·Loop 순환. 전체 Quest428/507 검사나 Appearance QA는 반복하지 않았다. [방법·Fixture 수정 이력·한계](Main16_Audio_마무리_QA.md)·[로그](Main16_Audio_Runtime_Results.txt).
- Voice100/BGM80 유지, BGM0에서도 Voice 출력0.5678347·Mute 전체0·해제 복원 통과. 임시 메모리 SFX 제거, 원본 Gain/Mixer/전역 기본값/Ducking/Runtime 게임 코드 변경 없음. 보호750파일 해시 동일, Voice28 Mapping·Appearance21/29·사용자 변경 유지.
- 실제 사람 귀의 청취는 **0/28**다. 발음·감정·호흡·속도·Segment boundary·Speaker 음색·Voice/BGM balance/Masking·BGM Loop 경계·장면 적합성은 미검증이며 TTS 재생성 필요 목록은 미정이다. [개별28개 청취 체크리스트](Main16_Audio_청취_체크리스트.md). 다음 권장 작업은 사람이 기본 Voice100/BGM80으로 청취해 실제 문제만 기록하는 것이다.
- Unity 컴파일 오류0, 최종 Console Error0/Warning2(기존 ExternalAssetImportEditor의 deprecated spritesheet API 경고). clean Bootstrap Edit Mode, 격리 Save/Settings·Play 옵션·runInBackground 복원. 직접 변경만 로컬 커밋, GitHub Push 없음. 아래 기록은 당시 이력이다.

## Appearance Foreground·Main16 Voice 중단 작업 이어서 완료 (2026-10-03)

- 직전 완료된 Main16 WAV28개·Mapping28개와 외형50종 실제 화면 판정부터 재개했다. ZIP 재Import·Sprite 재Slice·기존 Voice Import·Quest 재구현·TTS 생성은 반복하지 않았다. 이번 continuation의 남은 범위 Foreground 승인을 별도로 받았다.
- Appearance 총50 / Ready21(남10·여11) / Blocked29 / 시트 육안 미판정0. Mobility10 전부 보류, Vision Alpha 경고10은 PASS5·보류5. Hearing10·Intellectual Fighter/Mage4를 추가 보류했다. [파일·Frame·사유](../11_UI/Player_Appearance_Foreground_QA.md). 원본 PNG50·Catalog800 Sprite GUID/fileID 동일, 준비 Clip168개. 기존 준비 Clip을 재사용하고 보류 정책만 반영했다.
- 최종 정책의 성별·5테마·이전/다음·Preview·선택·확정·Navigation·World·Save/Continue 실제 감사803 PASS/0 FAIL. 직전 Hearing 외형+Vision 실제 Path 독립·대표 World32상태를 재사용했다. 대표 Vision 여성의 실제 Battle Left Idle도 확인했다. 800×600 단계 표시 겹침을 최소 수정하고 같은 해상도 화면·경계로 재검증했다.
- Main16 Manifest28 / Source Audio33(segment28·합본5) / Import28 / Mapping28 / 기존201+28=Story229. Player 무음9·관찰7, Unmapped/Text/Speaker mismatch/Missing/Duplicate 모두0. Hearing·Default 실제 Scene/NPC/Story Battle/귀환·완료와 격리 Save/Continue428 PASS/0 FAIL. 연속 Next·화자 fallback·전투 정리·Voice100/40/0·Mute·retry 출력 통과. 기존201 Catalog prefix와 WAV 원본28개 동일. [적용·검증 상세](Main16_Voice_Foreground_QA.md).
- 컴파일 오류0, 최종 Console Error0/Warning0. 컴파일 시 기존 ExternalAssetImportEditor deprecated 경고2건은 별도 확인했다. clean Bootstrap Edit Mode, 임시 해상도·Play 옵션·격리 Save/Settings 복원。 기존 보호 대상211파일 해시 동일, 기존 작업 트리를 보존하고 직접 변경만 커밋했다.
- 사용자 직접 확인: 실제 청취0/28로 발음·감정·호흡·속도·Segment boundary·Text Audio 일치·음량 취향은 미검증이며 재생성 필요 수는 미정이다. OS Tab 전달 후 실제 이동을 확인하지 못해 키보드·게임패드 물리 입력은 미검증으로 남긴다. 다음 권장 작업은 보류29개 원본 그림 수정과 청취 품질 검수다.
- 마지막 관련 기능 commit: Main16 `b957aa7`, Appearance/UI `3953e49`. GitHub Push 없음. 아래 기록은 당시 상태를 보존한다.

## Player Appearance 선택·Save/Continue (2026-10-03)

- CharacterCreation에 성별·전체/5테마 필터, 이전/다음 외형 목록 탐색, 큰 Preview, 체크/이름/번호 선택 표시와 기존 Navigation을 연결했다. Catalog50종 중 **35 ready / 15 보류**(남17/여18 선택 가능)이며 기존 기본 외형도 유지한다. Vision 경계 경고10종157셀 중5종은 낮은 Alpha/장치 윤곽 접촉으로 합격,5종은 인접 행 조각으로 보류했다. Mobility10종은 분리된 본체·다른 Character·방향 오류 의심으로 보류하고 원본은 수정하지 않았다. [선택 계약](../11_UI/Player_Appearance_선택_Save.md)·[전수 QA](../11_UI/Player_Appearance_선택_QA.md).
- Stable Appearance ID를 Session/Version1 Save에 선택적 필드로 추가하고 Continue에서 복원한다. 유효한 ID는 공용 성별 Animator의 Sprite Clip만 Override하며 World와 기존 Battle Left Idle에 반영한다. 구버전·invalid·blocked ID는 기존 Male/Female+Path Variant를 유지한다. 외형 테마는 실제 Path/Job/Story와 독립이며 미용실/진행 중 변경 NPC는 미구현이다. 마지막 관련 기능 commit **`cc3aa90`**, 선택/Runtime `0ad3cc5`, 문서 선행 `eeb48b2`.
- Background PlayUnfocused **1197 PASS / 0 FAIL**: 실제 생성→World,35종4방향 Idle/Walk·Battle용 Sprite,35종 Save/Restore, 신규 남/여 Bootstrap Continue, 구버전/invalid 남/여 Continue, 기존 외형/Path fallback30조합과 Path×Job25조합. Catalog50/800·ID/meta/성별/테마 정상, Stable ID·Sprite 참조 유지·PNG50 원본 해시 동일·보호798파일 변화0. 컴파일 오류0·최종 Console Error0/Warning0(재컴파일 당시 구형 편입 도구 CS0618 경고2개 별도). Bootstrap clean Edit Mode·Editor unfocused·격리 경로/진입 옵션 복원, foreground/OS 입력 없음. 관련 diff 검사 통과, GitHub push 없음.
- 사용자 확인/다음 권장 작업:35종의 발 미끄러짐·방향 전환·프레임 흔들림·크기와 장치 표현, 실제 키보드/컨트롤러 입력을 육안/기기로 확인한다. 보류15종은 사용자 원본의 프레임/Alpha 문제를 먼저 해결한 뒤 재검수한다. Foreground Visual QA와 Battle 전체 playthrough는 미완료다. 아래50종 등록 항목의 미구현 설명은 등록 당시 이력이며 현재 선택/Save 상태는 이 항목을 따른다.

## 검증본 Player Appearance 50종 등록 (2026-10-03)

- 사용자 `Limitless_Player_Sprites_Validated_50.zip`의 PNG50개를 원본 바이트 그대로 `Art/Characters/Player/Validated50/`에 편입했다. 5테마 각10개·남25/여25·512×512 RGBA·4×4/128px16프레임이며 정식 Player의 PPU128·Point·무압축·하단 중앙 Pivot을 적용했다. [등록 계약과 검증 결과](../11_UI/Player_Appearance_Validated50.md)를 따른다. 문서 선행 commit `00aaaef`, 마지막 관련 기능 commit `d6937e4`.
- 기존 External50 Catalog GUID 및 stable ID50개를 유지하고 새 sheet/800 Sprite를 연결했다. 테마/직업은 메타데이터이며 선택 제한이 아니다. 이전 PNG·Animator·Prefab·Scene·Male/Female+Path fallback·Save 형식은 유지했다. Character Creation50종 UI·appearanceId Save·실제 게임 외형 Override는 미구현이며 `readyForSelection=false`다.
- 백그라운드 Edit Mode 검사:50/800 참조·Rect/Pivot/설정·유일 ID·meta 정상, 재편입 Sprite 참조800개 동일, 원본 ZIP/PNG 해시 동일. 보호 파일212개 중 의도한 Catalog만 변경, 기존 사용자 변경 보존. 컴파일 오류0·Console 오류0, 기존 외부 편입 도구의 구형 spritesheet API 경고2건. Bootstrap clean·Editor unfocused, foreground 조작 없음.
- 사용자 확인/다음 권장 작업:10시트157 Cell의 Alpha 경계 접촉·전체50종 방향 행/발 위치를 육안 검수하고 행 계약을 확정한다. 그 후 공용 Animator Clip Override와 선택/Save ID를 별도 연결한다. GitHub push 없음.

## Main16 재 속의 형상 (2026-10-03)

- **구현 및 범위 내 Runtime QA 완료**: `main_16_shape_in_the_ash`의 12개 순차 목표, Field07 재바람 황야·Field06 서쪽↔Field07 동쪽 연결·지정 불씨망령1체·협곡 입구·아르벨 보고를 기존 Quest/World/Battle/Save에 연결했다. 계약은 [Main16 Runtime](../03_스토리/Main16_Runtime_구현_계약.md)을 따른다. 마지막 관련 기능 commit `2ef6690`.
- 정식 `path.hearing` 분기에서는 Player가 최초 관찰하고 세린이 확인한다. Default는 세린이 먼저 공유한다. 분기별 stable Dialogue ID·현장 세린 NPC를 사용하며 저장 Party/Formation과 실제 전투 편성은 변경하지 않는다. 지정 승리 이후 일반 불씨망령 배치와 협곡 발견은 Quest count에서 복원한다.
- 격리 Save/Settings의 PlayUnfocused에서 Hearing/Default × 세린 편성/미편성 네 실행 모두 완료, PASS 체크507개. 실제 NPC·순차 조사·명령 공격 Victory·도망/재도전·Defeat 중앙 Arbel 복귀·Bootstrap Continue17회·Navigation·펫 미해금을 확인했다. 자세한 방법/Fixture와 한계는 [Main16 QA](../03_스토리/Main16_Runtime_QA_2026_10_03.md)를 따른다.
- 21:9 변경 직후 카메라 보간이 맵 밖을 보여 주는 오류를 수정하고 3비율×4모서리의 첫 프레임 viewport 제한을 검증했다. 목격 취소 후 자동 재열림도 방지했다. 최종 컴파일/Console 오류·경고0, Bootstrap clean Edit Mode·격리 경로/진입 옵션 복원·Editor unfocused. 관련 diff 검사 통과, 전체 트리는 기존 사용자 공백429건으로 exit2다.
- 사용자 확인/다음 권장 작업: 조사 표식·한국어 TextMesh·재/균열/협곡 배치 시각 QA와 실제 이동/입력 검수. Foreground/OS 입력 없음. Voice 미적용·Field07 BGM TBD이며 별도 확정 뒤 제작/배정한다. Main17/협곡 내부/Overheat/Boss는 구현하지 않는다. 직전 Asset/BGM/Background/Player 작업과 기존 사용자 변경을 보존했다. GitHub push 없음.

## 외부 Asset 정식 편입 (2026-10-03)

- 확정 MP3 7곡, Chapter1 배경 PNG 6종, Player 시트 50종을 원본 바이트 그대로 편입했다. Chapter1 일반전=Steel and Sunlight, B1=Stone Without Memory, B2=Beneath The Forgotten Hall, 파수꾼=The Warden’s Final Stand, Field04~06=Paths of Cracked Earth. 기존 Mixer/BGM 서비스·Scene 복귀 정책을 재사용했다. Intro 곡은 시작/종료 Hook만 준비했고 Deep West 곡은 최초 Scene 미확정으로 Catalog에 보관한다. 후보4곡은 Downloads에 보존하며 Chapter2 일반 전투 변주는 TBD다.
- 배경은 Field01 초원, Field02/03 숲, Dungeon B1/B2 지하묘지, 파수꾼 Boss 전용으로 연결했다. 독성 늪/고대 성소는 대응 Scene 미정으로 미배정이다. 원본1672×941 RGB를 기존 전체 화면 Background Image에 비율 유지 Fit하고 기존 HP/명령 영역을 유지했다.
- Vision/Hearing/Intellectual/Mobility/Emotional Scar 각10종(각 Male5/Female5)을 실제 집계했다. 고정 Appearance ID·원본 Texture 참조 Catalog를 준비했으며 Vision512×512만 160프레임 Slice했다. 나머지40종1254×1254는 128px Cell 규칙과 불일치해 Slice를 보류했다. Mobility10·Intellectual4종에는 Alpha가 없다. 전체50종 방향/발 Pivot 검수 전으로 선택 가능=false이며 Character Creation 신규 외형 선택과 appearanceId Save는 **미구현**이다. 기존 Male/Female·Path Variant·Save fallback은 유지했고 새 Animator Controller를 복제하지 않았다.
- 검증: Unity6000.5.7f1 Runtime/Editor 컴파일 완료·Console 오류/경고0, Edit Mode 최종216항목 실패0, 신규63파일 원본/SHA256 일치·GUID 유일, 배경6 Sprite·Audio7 Clip 참조 정상, 직접 변경 staged diff --check 통과. Bootstrap dirty=false 유지, Scene 저장·Play Mode·foreground 조작 없음.
- 사용자 직접 확인: 실제 음악/Loop 경계·일반전/Boss 후 탐색곡 복원·화면 비율별 배경/HP 가독성. 다음 권장 작업: 1254px 시트 Cell/Alpha 원본 확인과 50종 방향/발 위치 검수 후 공용 Animator Override·독립 외형 선택·Stable Appearance ID Save 연결. Intro 첫 진입 연출 종료 계약·Deep West 최초 Scene 확정도 별도 작업이다.
- [편입 정본·Inventory·검증](외부_Asset_정식_편입_2026_10_03.md). 마지막 관련 기능 commit: `e8603df`, 정책/검증 문서: `2b7218b`. GitHub Push 없음.

## Missing Story Voice Supplement Pack (2026-09-30)

- [보충팩 QA](Missing_Story_Voice_Supplement_QA.md)의 정식 CSV58행을 LOCAL 기존 무음 대사와 대조하고 사용자 허용에 따라 안정 ID를 지정했다. Main03 12/Main04 18/Main05 6/Main08 19/Main12 3개를 원본 WAV 그대로 적용했다. 입력61 WAV 중 Manifest 미참조 합본3개는 제외, Main01/02 신규 음성은 미제공이다. 기존 Registry의 태온 Gacrux34/미엘 Sulafat22/폴 Achird2개, gemini-3.8-flash-tts·Voice Design 사용 안 함을 기록했다.
- Existing Voice Wins 원칙으로 기존143+신규58=Story Catalog201개. 기존 ID 삭제/Clip 교체/GUID 변경/Character 변경0, Intro18개·Main06/07/09/10/11 기존102개·Main13~15 기존41개·BGM 보존. 기존 관련353파일 및 사용자 수정73파일 해시 동일. 모든 기존 대사 화자/본문/순서 보존, 불일치/누락/중복/범위 밖 Row0. Main01~12 정적228쪽 중 Voice160쪽(70.18%), Voice 없음68쪽이다.
- Background 실제 Play Mode에서 신규58개 참조/디코딩/비무음/58회 연속 Next/종료/fallback, 실제 Main03/04/05/08 대사 함수와 Main12 조사 Line/VoiceId 샘플의 Voice 출력 확인. Main01은 기존 무음5쪽 진행을 확인했다. Intro Storyteller·Main07/09/11/13 회귀, Voice100/40/0·Mute/Unmute 출력, 신규 음성 중 Scene 전환 후 Source0 통과. Main12 전체 Quest 상호작용/전체 playthrough·물리 입력·시각 검증은 미완료다.
- 컴파일 오류0·최종 Console Error0/Warning0·이번 변경 diff 검사 통과. 격리 Save/Settings와 백그라운드 실행/진입 옵션 복원, Bootstrap clean Edit Mode, Foreground 조작 없음. 정책 계획 commit `5814d34`, 마지막 관련 기능 commit `8756399`. 실제 WAV/Meta·Catalog·직접 연결 C#5개·문서만 커밋, 제작 Output 전체 Stage 및 GitHub push 없음.
- 사용자 확인/다음 권장 작업: 실제 청취로 발음·감정·호흡·캐릭터 취향·음량 균형을 검수한다. Main01/02 일반 NPC/Player를 포함한 남은 무음 대사는 별도 제작 범위를 확정한 뒤 보충한다. Main16은 기존 문서 설계 상태이며 이번에 구현하거나 Voice를 만들지 않았다. 세부 Mapping/GUID/해시/Runtime 출력은 [감사 JSON](Missing_Story_Voice_Supplement_Audit.json)을 따른다.

## Path 반응형 Story와 Main16 설계 (2026-09-30)

아래는 당시 문서 설계 작업 기록이다. 현재 Main16 Runtime 상태는 상단 2026-10-03 항목을 따른다.

- [Path 반응형 Story 정본](../03_스토리/Path_반응형_Story_연출_규칙.md)에 Player Agency First·공통 핵심 정보/Story State·Hearing Player 우선 관찰/세린 교차 확인·다른4개 Path 방향을 확정했다. [Story Scene 템플릿](../03_스토리/Story_Scene_설계_템플릿.md)에 Path-Reactive Check를 추가하고 Path/세린/Story/TTS 문서는 정본을 참조한다.
- Main13 첫 만남/장치 인식, Main14 반복·강한 진동, Main15 짧아진 진동·비교·서쪽 방향을 DESIGN RETROFIT CANDIDATE로 기록했다. 기존 대사/ID/Voice/Runtime은 유지하며 향후 별도 분기 확정·구현·검수를 해야 한다.
- [Main16 「재 속의 형상」](../03_스토리/Chapter2_Main16_재_속의_형상.md)의 정식 Story 설계 완료. Field07 재바람 황야/Scene 가칭·12 Objective·움직이는 재의 Hearing 분기·불씨망령 단독 Story Encounter·처치 후 남은 진동·지하 열기 가설·협곡 입구/Arbel 보고·세린 미편성 시 진행/편성 보존·Main17 후보를 기록했다. 실제 Stable ID/Scene 배치/보상 TBD, Main16 Runtime/Quest/Scene/Voice/BGM 미구현, Overheat 미사용·Boss 미노출이다.
- 이번 작업은 Markdown 문서15개만 변경했다. 링크/참조 경로 및 새 중복 제목 검사 통과, 이번 변경 diff 검사 통과. C#/Scene/Asset 수정·Quest/Dialogue 구현·TTS 생성·Unity 실행/Play Mode 없음. 기존 사용자 변경92항목 보존, 설계 commit `5a92b72`, GitHub push 없음.
- 다음 권장 작업: Main16 구현 전 체크리스트의 LOCAL API/ID·World 왕복/Bounds·순차 목표/Save·Story NPC/Override·분기별 Dialogue를 확인하고 확정한 뒤 구현한다. Voice는 Story→Branch→ID→Manifest 확정 후 제작하며 Field07 BGM은 별도 확정한다. 현재 Unity에서 확인할 신규 구현 사항은 없다.

## Main13~15 Voice 및 확정 BGM 3곡 (2026-09-30)

- 정식 `dialogue_manifest_main13_15.csv`41행과 LOCAL 화자/본문을 대조해 Main13 8/Main14 11/Main15 22개를 원본대로 Import/연결했다. 세린 Schedar15개·레온 Orus5개와 기존 태온 Gacrux6/미엘 Sulafat4/폴 Achird11개, 제작 모델 gemini-3.8-flash-tts/ko-KR를 Registry에 기록했다. Voice Design 사용 안 함, Tone/속도/Batch TBD. PCM24kHz/mono/16bit·253.40초·원본 바이트/신규 GUID41개 정상, 중복/누락/본문·화자 mismatch0. 기존102개를 유지한 Story Catalog143개, Intro18개 유지, Main16 이후 미적용.
- 정식 BGM3곡을 원본 MP3 그대로 Audio/Music에 Import했다. Before the First Light→Bootstrap Title, Morning at the Gate→StarterVillage/Arbel/Field03 던전 입구 안전 반경, Morning Over the Ridge→Field01/Chapter2 첫 서부 Field04. Field05/06·다른 Field·Dungeon/Battle/Boss·Intro는 TBD/이전 음악 정리. 단일 지속 Source·기존 BGM Mixer·Loop·같은 곡 재시작 방지·Voice 독립, Ducking/Crossfade 없음.
- 배경 실제 Play Mode에서41 ID/화자/Next·실제 NPC/조사 factory·인물5명 출력·Voice/BGM/SFX 독립 출력·전체 Mute0/설정 복원, 최종12 Scene 전환의 곡 선택/정리/서비스1·Listener1·세 곡 출력, 안전 반경 진입/이탈을 통과했다. Intro/기존Story120개 참조와 실제 Next/Skip 회귀도 확인했다. Intro→Title의 Listener 수명 누락 및 보충 중 Intro 중복을 수정해 재검증했다. 상세는 [Voice/BGM QA](Main13_15_Voice_BGM_QA.md).
- 최종 컴파일 오류0·Console Error0/Warning0·이번 변경 diff 검사 통과. 기존 사용자 변경/기존음성/입력원본 등370파일 SHA-256 동일. 격리 Save/Settings·Game View 진입 동작·백그라운드 설정 복원·Bootstrap clean Edit Mode 종료. Foreground 검증 없음. 계획 `5f3dca9`, Voice 기능 `6818b42`, 마지막 BGM 기능 `ea97a2d`. 관련 파일만 커밋, GitHub push 없음.
- 사용자 확인/다음 권장 작업: 실제 청취의 취향·감정·발음·호흡·Voice/BGM 균형·Loop 경계·Scene 감정 적합성을 검수한다. 전체 Main13~15 Quest playthrough·실제 이동/물리 입력/시각 QA는 미검증이다. 후속 Field/Dungeon/Battle/Boss 전용 BGM과 Main16 이후 Voice는 별도 확정 후 도입한다.

## Main01~12 Story Voice 적용 (2026-09-30)

- 외부 PreSerin 출력107 WAV 중 manifest104쪽을 LOCAL 화자/본문으로 대조해102개550.97초를 원본 그대로 Import/연결했다. 태온 Gacrux28개·미엘 Sulafat22개·폴 Achird52개. PCM24kHz/mono/16bit, 원본 바이트 동일·GUID/참조102개 정상·전체 입력 디코딩107개. 모델/Tone/Batch는 근거가 없어 TBD. Narrator/Intro18개 유지, 세린 미적용.
- Main06 3개/Main07 26개/Main09 27개/Main10 18개/Main11 28개 적용. Main01~05·Main08·실제 Main12는 음성 미제공으로 텍스트 진행 유지. CSV Main12 두 항목은 실제 Main14이라 제외했다. 정적 대화228쪽 중 Voice 없음126쪽, TEXT_AUDIO_MISMATCH0·중복0·범위 제외 Unmapped2. 기존 대사/Save/수동 Next 정책 보존.
- 기존 VoiceClipCatalog/VoicePlaybackSource/Voice Mixer를 재사용하고 선택적 Dialogue ID·화자 확인을 추가했다. 실제 백그라운드 Play Mode에서102 ID/Clip/화자·연속 Next·null/wrong-speaker fallback·실제 Main07/09/10/11 factory·Main01/02/05/12 무음 진행·Hide/단일 Show/확인창/비활성화·Scene 제거 정리를 확인했다. 인물별 실제 출력, Voice0/Mute/채널값 복원, Intro18 참조/8연속Next/Skip→CharacterCreation·잔류Source0 통과. 상세는 [Story Voice QA](Story_Voice_Main01_12_QA.md).
- 검증 상태: 컴파일 오류0·최종 Console Error0/Warning0·이번 변경 diff 검사 통과. Bootstrap clean Edit Mode·격리 Save/Settings·Game View 진입 동작·백그라운드 설정 복원. Foreground/Game View 활성화 없음. 계획 Docs `e4a495c`, 마지막 관련 기능 commit `ea776a9`. 이번 변경만 커밋, GitHub push 없음.
- 사용자 확인/다음 권장 작업: 실제 청취로 발음·감정·호흡·문장/자막 일치·컷 경계·음량 균형을 검수한다. 전체 Quest playthrough·물리 입력·시각 배치는 미검증이다. 미제공 Main01~05/08/12와 Main14로 분류된2개 제작 메타데이터를 후속 확인한다. Main13 이후·세린 Voice 도입은 별도 작업이다.

## 정식 Audio Volume Settings (2026-09-30)

- Master→BGM/SFX/Voice AudioMixer, 전체 음소거·채널별0–100 Slider/숫자·실시간 적용, Scene 공용 설정 화면을 구현했다. 버튼/F10/패드 Start 진입, 기존 Navigation과 모달·이동 잠금 사용. 기본값 Voice100/SFX100/BGM80/Mute OFF, 로그 dB 변환·0은-80dB, Mute에서도 채널값 보존. Intro Source는 Voice Group·volume1로 연결해 중복 감쇠 없음.
- 기존 사용자 JSON을 Version2로 확장해 슬롯과 독립적으로 저장한다. 구버전 Skip 보존·Audio 기본값 이행, 범위 제한, Scene 유지·재Load·Play 재시작에서63/47/72 및 Mute 복원 확인. 정식 BGM/SFX Source는 현재 없으며 신규 Source는 공용 Route API로 용도별 Group에 연결한다.
- 실제 백그라운드 Play Mode에서 Intro와 임시 BGM/SFX 신호의27개 출력 검사, UI 숫자·방향 이벤트·모달 정리, Intro18개 대응/Next/연속Next/Skip/전환 후 Source0 회귀 통과. TTS 원본·사용자 저장·기존 변경109파일 해시 동일. 상세는 [Audio QA](Audio_Volume_Runtime_QA_2026_09_30.md). 컴파일/게임 Error0, 재컴파일 MCP Warning1 별도 기록. Bootstrap clean Edit Mode·검증 경로/진입 동작 복원.
- 남은 확인/다음 권장 작업: Game View 시각 배치·물리 키보드/패드/마우스·청취 밸런스·배포 Player 저장 검수. BGM/SFX 실제 음원 도입 시 정식 Group 연결. 기존 Intro 발화·분할 경계 청취 검수도 계속 남아 있다. 계획 Docs `f628e11`, 마지막 관련 기능 commit `cd656e0`. 이번 변경만 커밋, GitHub push 없음.
- 최종 Console Error0/Warning0 재조회. 이번 변경 diff 검사 통과, 전체 작업 트리의 기존 사용자 변경에 남은 공백 오류는 보존했다. 검증용 설정 JSON·임시 신호 정리 완료.

## Intro Storyteller_4 음성 적용 (2026-09-30)

- 외부 생성 WAV 18개를 기존 Intro Voice 경로에 원본 그대로 적용했다. 24kHz/mono/PCM16, 합계83.20초, SHA-256 동일·GUID/Catalog 유지. Narrator 표시명 Storyteller_4와 제작 메타데이터의 Voice ID를 [TTS 정책](TTS_음성_제작_정책.md)에 기록했다. 다른 캐릭터 Voice는 TBD. API/SDK/Key/HTTP 추가 없음. 계획 Docs `602b1b8`, 마지막 관련 기능 commit `7e60218`.
- 안정 Dialogue ID→Clip 연결 유지, Next 즉시 중지·다음 블록 시작, Skip/정상 종료/외부 전환·비활성화에서 Coroutine/음성 정리. 기존 자동 최소 시간과 Clip+0.5초 규칙 유지, 독립 Voice Volume 기반 추가. Story·설정 UI 변경 없음.
- 백그라운드 실제 Play Mode 전체19블록/156.47초·18개 순서/자막/Clip·중간 Next·같은 프레임8회 Next·Skip→CharacterCreation/잔류 Source0·정상 Replay→Bootstrap·재진입001·복제 Catalog null 자동 진행·외부 전환 정리 통과. Import18개/디코딩18개/컴파일 오류0. 세부 결과·QA 도우미 예외와 의도한 null 경고는 [Intro QA](Intro_Storyteller4_적용_QA.md)에 구분 기록한다.
- 사용자 확인/다음 권장 작업: 실제 청취로18문장 발화 일치·자동 분할 경계·음질/음량을 검수한다. 자막/ID/Clip의 구조적 대응과 실제 발화 청취를 구분하며, 체감 BGM 밸런스 및 실제 물리 입력은 미검증이다. 원본 재가공이나 새 음성 생성은 하지 않았다. GitHub push 없음.
- 최종 Console Error0/Warning0, Bootstrap clean Edit Mode, 포커스 전환 없음·Save 격리 해제·사용자 저장 해시 동일. 18개 모두 실제 음성 출력 신호 확인. 이번 변경 diff 검사 통과, 전체 작업 트리는 기존 사용자 Animation/Scene/meta trailing whitespace로 exit2(기존 변경 보존).

## Main15 Runtime QA와 TTS 정책 (2026-09-30)

- TTS 정본 [음성 제작 정책](TTS_음성_제작_정책.md)을 추가했다. 2026-09-29 개발 방침으로 Gemini 3.8 Flash/Flash-Lite TTS, Free Tier 우선, Voice Design/Scene Style 분리·Voice Registry TBD, 사전 생성 Audio Asset·Client Key 비포함, 다음 Intro 제작을 기록했다. 기존 MeloTTS 문서는 제작 이력으로 연결했다. API/음성/Cloud/Key/Audio Asset 작업 없음. 문서 commit `d9bfc68`.
- 격리 Play Mode에서 Main15 시작→12개 순차 목표→정식 세린 합류를 완료했다. 실제 왕복 Collider·Arbel Safe Zone 유지·적 AI에 의한 전멸 중앙 복귀·HP/MP 회복·회복약 소비 유지, 4조우 실제 전투 명령 승리·4종 분양 목록·그을음들개100 구매, 세린 사수/청각/Fox·기존 미엘/폴 편성 보존·Party Manager 동료2명 제한을 확인했다. 목표 근처 배치는 QA가 했으며 전체 수동 이동 입력 검증은 아니다. 세부 증거·한계는 [Main15 QA](../03_스토리/Main15_Runtime_QA_2026_09_30.md)를 따른다.
- **실제 버그 2건 수정/재검증**: Arbel 런타임 Bounds 초기화 순서로 저장 위치가 Spawn에 덮이는 버그를 수정해 완료 전 `(2,-4)`/후 `(1,-4)` 및 Field06 `(5,-2)` Continue를 확인했다(`cfadcc8`). Field06 북쪽의 복제 원본 출구 틈으로 맵 밖 이동되는 버그를 Collider1개로 막아 실제 물리 이동이 y14.5 대신 y6.7에서 차단되는 것을 확인했다(`1e5cb54`). 동쪽 왕복·중앙 Spawn 유지. 마지막 관련 기능 commit `1e5cb54`.
- **검증/남은 항목**: Unity6000.5.7f1 컴파일 오류0, 기능 검증 중 게임 Error/Warning0. 3비율 viewport Bounds 계산 통과. 이번 사용자 승인으로 1016×569 Game View 배치·Navigation·공식 외형·최종 KO 유지 일부 시각 QA를 수행했다. 초기 직사각형 환경 Patch, 작은 조사 TextMesh 가독성, 세린 Battle Portrait fallback/미엘 후열 겹침은 후속 검토이며 미술·기획을 임의 변경하지 않았다. 전체 이동 경로·NPC 겹침·KO 전환·모든 비율 UI·Monster 정밀 QA는 미검증이다. MCP 재컴파일 WebSocket Warning1과 캡처 도우미 PlayerLoop Error5를 게임 코드와 구분해 기록했고 캡처를 중단했다. 기록 후 Console 정리·Edit Mode 재조회, 사용자 슬롯 SHA-256 동일·QA JSON 삭제·캡처만 보존·Bootstrap clean 종료. 관련 diff 검사 통과, GitHub push 없음.
- 다음 권장 작업: Field06 미술/표식·세린 전투 외형/배치의 별도 검토와 실제 이동 경로/KO 전환 시각 검증. TTS 제작은 정식 정책대로 Intro Free Tier Pipeline을 별도 작업으로 시작한다. 기존 BeastCompanion/안전지대 문서의 과거 미구현 설명은 별도 문서 감사 대상으로 남긴다.

## Main15 「타오르는 흔적」 구현 당시 기록 (2026-09-29)

- `main_15_burning_traces` 12개 순차 목표와 레온 의뢰·세린 조사/자발적 합류 대화를 구현했다. Main14 완료 뒤 레온 대화로 시작하며, 서쪽 첫 그을음들개 승리만 지정 목표를 진행한다. Quest Navigation은 레온, Arbel 북서쪽 출구, 조사 지점, 첫 조우, 귀환 지점과 세린에 연결했다. 완료 시 기존 명단에 세린을 영구 해금하되 선택된 동료 2명과 진형은 바꾸지 않는다. 진행 중 세린은 Story Temporary이며 기존 사수·청각의 길·Fox를 사용한다. 영구 해금과 Quest·Beast·편성·위치·안전지대는 기존 Save 구조를 사용한다.
- `Field_06_ScorchedTrail` Scene과 Arbel 북서 출구↔Field06 동쪽 출구의 별도 왕복 Spawn을 등록했다. Spawn과 Exit은 떨어져 있으며 Arbel Safe Zone을 유지하고 Field06에는 새 Respawn 거점을 두지 않는다. 기존 Field Scene을 바탕으로 서쪽의 풀 감소·고사목 색·황토·균열·그을음 표식을 단계적으로 더한다. 일반 Spawn M15-01 그을음들개, M15-02 그을음들개+열풍매, M15-03 균열도마뱀, M15-04 화열딱정벌레+열풍매를 기존 몬스터 정의·Battle·Victory→Arbel 펫 분양 경로에 연결했다. 불씨망령·Boss·Dungeon·과열·Fast Travel·Main16 Quest는 구현하지 않았다. Main16의 방향만 설계 문서에 남겼다.
- **검증 상태**: Unity 6000.5.7f1 Edit Mode 컴파일 오류 0. 정적 Asset 조회에서 Main15 목표 12개, Serin 사수/청각의 길 Definition, Field06 Scene Build 등록, 4개 Spawn의 Monster 참조, Arbel 왕복 Connection을 확인했다. Console에는 기능 오류 없이 MCP WebSocket 초기화 경고 1건이 남았다. 요청에 따라 Play Mode·Game View·실제 Main15 진행·전투/분양·Save/Continue·전멸 복귀·화면 비율·시각 QA는 **미수행**이다. 다음 작업에서 이 경로들과 Field06 배치/Bounds/카메라를 실제 Runtime으로 확인해야 한다. 관련 설계 commit `d656f15`, 기능 commit `aea23bc`. GitHub push 없음.

## Chapter 2 도입부 핵심 Runtime QA (2026-09-29)

- Unity 6000.5.7f1 격리 Play Mode에서 Main11 완료·석판 보유 상태로 Main12를 시작해 석판→잡화 상인→기록→서쪽 결정→Field03 서쪽 출구 순서를 진행했다. Field04 도착으로 Main12가 완료되고 Main13이 시작됐다. Main13의 마른 토양·물길·세린 첫 대화 4페이지를 확인했다. 세린의 청각 보조 장치 설명 문구, 임시 동행 상태, 전투 참가자의 `companion_serin`/사수/`path.hearing` 및 초기 Fox를 런타임에서 확인했다.
- 지정 Story Battle 두 건은 실제 Battle Scene 진입과 stable encounter ID를 확인하고, QA에서 승리 결과 함수를 호출해 목표 진행을 검증했다. 수동 전투 명령으로 승리한 검증은 아니다. Field04→Arbel 최초 진입에서 Main13 도착·`safezone_arbel` 자동 활성화·고정 NPC 8명 및 PortalAnchor를 확인했다. 레온에게 보고해 Main13을 완료했다.
- Main14는 레온 시작→우물→관개수로→세린 재합류 대화→감시초소→지정 조우 승리 처리→강한 진동 4페이지→레온 보고까지 진행해 완료를 확인했다. Arbel 상점 UI, 치유사 치료 대화, 편성 UI, 펫 분양·관리 UI, Portal 관리인 준비 안내를 확인했다. 파티 안내인의 Arbel Scene 제한과 분양 확인 대화의 모달 소유권 버그를 최소 수정하고 재검증했다.
- 격리 슬롯 5의 Bootstrap Continue에서 완료된 Main14·세린 임시 상태·Arbel Safe Zone을 복원했다. 진행 중 Main14 4단계 저장 자료를 구성해 Continue 후 목표 ID, 세린 임시 상태와 안전지대 복원도 확인했다. Field05 전투에서 QA가 전멸 결과 함수를 호출하자 Arbel `Spawn_Arbel_Center`로 복귀하고 저장됐다. 실제 적 공격에 의한 전멸은 미검증이다. 코드 재컴파일 후 Unity Console Error/Warning 0건. 화면 배치·NPC 위치 미관·실제 전투 명령 승리/전멸·도망·전체 전투/Quest/Pet 회귀는 이번 범위에서 검증하지 않았다. 다음 권장 작업은 별도 시각 QA와 실제 전투 입력 확인이다. GitHub push 없음. 마지막 관련 수정 commit `7387023`.

- 2026-09-29 **Chapter 2 도입부 구현**: Main12 「남겨진 기록」·Main13 「메마르는 땅」·Main14 「땅 아래의 울림」의 순차 Quest Definition과 현장 조사·NPC 대사·지정 Story Battle 승리 목표·Navigation을 연결했다. Field_03 서쪽 출구, `Field_04_WesternBorder`→`Arbel`→`Field_05_WesternOutskirts` 왕복 Scene/Spawn/경계 개구부를 추가했다. 새 Field와 Arbel Scene은 기존 Scene 복제 기반 초기 배치이며 환경·NPC 위치는 시각 QA 전이다. 기존 Chapter 2 몬스터 Definition을 새 Field 일반 Spawn과 지정 조우에 재사용했다. 아르벨 NPC의 기존 Sprite 매핑 누락은 정적 확인 중 보완했다. 설계 commit `c25d319`, 기능 commit `019454b`, 외형 연결 fix commit `f67a548`.
- 세린 `companion_serin`은 공식 방향 Animator와 초상화, `path.hearing`/사수 Definition을 연결했다. Main13 공동 전투~아르벨 도착, Main14 재합류 뒤 전투 참가 여부는 Quest 단계로 복원하며 영구 명단에 해금하지 않는다. 초기 Beast는 Fox다. 임시 세린은 현재 Pet 관리 화면의 정식 사수 목록에서는 제외된다. 아르벨에는 레온·상점·무료 치유·파티 안내·펫 분양/관리·주민·Portal 관리인과 준비 상태 상호작용을 연결했다. 정상 도착 시 기존 Safe Zone을 `safezone_arbel`/`Spawn_Arbel_Center`로 활성화하고 슬롯에 저장하도록 했다. 실제 Fast Travel과 Main15는 만들지 않았다.
- **검증 상태**: 구현 직후에는 정적 확인만 수행했으며, 이후 핵심 Runtime QA 결과와 남은 항목은 이 문서 맨 위에 기록했다.

- 2026-09-29 **Chapter 2 Monster 5종 Runtime 및 Pet 4종 연결**: 수정 공식 PNG 5개 모두 1256×1256 RGBA, SHA-256 원본/복사본 일치. Unity Importer에서 각 314×314·16프레임·314 PPU·Point·하단 중앙 Pivot을 확인하고, 명시적 Idle/Basic/Skill·Hit/KO 프레임과 5종 Definition을 연결했다. KO는 1회 재생 뒤 마지막 Down 프레임 유지, 별도 Walk 원본이 없어 Field 이동에는 Idle을 사용한다. 5종의 확정 Level/HP/Attack/Agility/EXP/Talent·전후열과 행동 배율, 개체별 예고/방어/재사용 순서, 공용 Burn을 연결했다. Loot·Chapter 2 Field/Spawn/Scene·Overheat·Serin Runtime은 추가하지 않았다. 기능 commit `d8df236`.
- **검증**: 포커스 없는 Unity 6000.5.7f1 Edit Mode에서 5종 Sprite 참조 16개씩·누락 0·수치/Formation/공격 범주, 5종 행동 순서와 균열 돌진·열핵 분출 예고 소비, 갑각 수축 직접 피해 100→60/DoT 100/다음 자기 행동 100을 확인했다. 사수 펫은 그을음들개 습격 36+Burn 2회, 열풍매 후열 습격 36→40, 균열도마뱀 첫 직접 피해 100→80·다음 100·DoT/0 미소비·새 전투 충전, 화열딱정벌레 첫 유효 Burn 무효·둘째 적용·새 전투 충전을 확인했다. 불씨망령은 펫 목록에서 제외된다. 포커스 없는 격리 Play Mode에서 `RecordVictory` 서비스로 승리 종을 기록한 뒤 4종 각 100 탈렌트 분양(400→0), 불씨망령 구매 거절, 장착, 저장 파일 불러오기·이어하기 복원을 확인하고 임시 슬롯 폴더를 삭제했다. 기존 독침벌 습격 36+독, Chapter 1 펫 5종 카탈로그, Wolf/Bear/Fox 기본 이용권·Bear HP 100→110·Fox 민첩 10→12를 자동 회귀 검사했다. 최종 Edit Mode, Bootstrap Scene, Editor 비활성 포커스, Console Error/Warning 0. 기능 스테이징 `git diff --check` 통과.
- **남은 확인**: 실제 Chapter 2 Field/Spawn이 없어 5종의 전체 Battle Scene 연출·사망/승리/Flee/Defeat 분기와 펫 외형 이동을 실전 조우로 확인하지 않았다. Game View의 KO 순서·Down Hold 시각 품질, 신규 분양소 스크롤/여러 화면 비율, 전체 기존 스킬 회귀도 미검증이다. 화면 포커스 전환이 필요한 시각 QA는 이번 작업에서 실행하지 않았다. 다음 권장 작업은 별도 승인된 격리 시각 QA 또는 Chapter 2 Field 설계 확정 후 실제 조우 연결이다.

- 2026-09-29 Chapter 2 일반 몬스터 5종의 사용자 확정 1차 Level/HP/Attack/Agility/EXP/Talent·스킬 배율·AI 방향·펫 효과를 설계 정본에 반영했다. 열풍매 펫은 기존 원거리 피격 -5% 안을 폐기하고 후열 대상 습격 최종 직접 피해 ×1.10으로 교체했다. 제공된 공식 PNG 5개는 모두 존재하고 1254×1254 RGBA이지만 양 축이 4로 나누어지지 않아 요청된 동일 크기 4×4 Slice 조건을 충족하지 않는다. 원본 변경·강제 Crop/Resize·대체 없이 5종 Asset Import와 이에 종속된 MonsterDefinition/AI/Pet Runtime·검증을 중단했다. 원본 SHA-256은 그을음들개 `459D3F08`, 열풍매 `ED35ECBA`, 균열도마뱀 `DF6AC182`, 화열딱정벌레 `F3859DDB`, 불씨망령 `975B13F0` 접두다. 문서 링크·staged `git diff --check` 통과, Unity Play Mode 미실행. 다음 작업은 4×4로 정확히 분할 가능한 공식 수정 시트 확보 후 구현 재개다. 관련 문서 commit `8ab63ed`.

- 2026-09-29 Chapter 2 몬스터 5종의 구현 전 파이프라인을 LOCAL 코드·Asset 기준으로 조사하고 종별 준비 체크리스트를 문서화했다. 기존 10종 Definition/Formation/Spawn/Animation, 공용 Burn, AI·보스 예고, BeastCompanion 승리→분양→장착 경로를 대조했다. 열풍매 원거리 직접 피해는 현 피격 API에서 안정 분류 불가로 구현 보류가 필요하다. 상대 링크·주요 코드 경로·staged `git diff --check` 통과. Unity Play Mode는 실행하지 않았으며 Chapter 2 최종 Sprite/수치/Definition/AI/Spawn/Pet Runtime은 미구현이다. 다음 작업은 최종 Asset·수치 확정 후 준비 문서의 순서대로 별도 구현·검증하는 것이다. 관련 문서 commit `74e5f37`.

- 2026-09-29 몬스터 설계 정본에 신규 종의 전투 특징·펫 가능 여부·계승 효과를 함께 정의하는 영구 규칙과 템플릿을 추가했다. Chapter 2 일반 몬스터 5종(그을음들개·열풍매·균열도마뱀·화열딱정벌레·불씨망령), 작열 감시자 Elite 후보, 기존 Burn 활용과 Overheat 미구현 후보를 문서화했다. 상대 링크와 staged `git diff --check` 통과. Unity는 실행하지 않았으며 Asset·MonsterDefinition·AI·Spawn·Pet Runtime은 미구현이다. 다음 작업은 별도 승인된 Chapter 2 구현에서 미확정 수치·피해 분류를 검토하는 것이다. 관련 설계 commit `3745968`.

- 2026-09-29 세린 공식 시각 자료 준비: 사용자 확정 Portrait(1374×1145 RGB)와 4×4 Sprite Sheet(512×512 RGBA)를 원본과 동일한 SHA-256으로 등록했다. Sheet는 128×128 16프레임, 128 PPU, Point, 하단 중앙 Pivot이며 방향별 Idle 4개·Walk 4개와 기존 플레이어 방향 상태 구조의 Animator를 준비했다. Unity Editor에서 Portrait/UI Sprite Import, 16프레임 크기·Pivot, 8개 상태의 Sprite 참조 누락 0건을 확인했고 Console Error/Warning 0, staged `git diff --check` 통과했다. Story/Companion/Battle Runtime·Main12/13·Chapter 2 Scene·BeastCompanion 세린 연결은 미구현이며, 실제 Scene 등장/화면 미관은 검증하지 않았다. 다음 작업은 별도 Chapter 2 기획·구현 시 이 자료 연결이다. 관련 기능 commit `9177f28`.

- 2026-09-29 BeastCompanion 실제 Play Mode QA: 사용자 승인 범위의 격리 슬롯과 검증용 임시 사수 Asset을 사용했다. 실제 전투 명령으로 도망 시 승리 기록·분양 후보 0과 필드 스폰 유지를 확인했고, 전멸은 검증 입력으로 아군 HP를 0으로 만든 뒤 Battle 종료 경로를 실행해 기록 0·안전지대 복귀·스폰 유지를 확인했다. 적 HP를 0으로 만든 실제 Battle 승리 종료 경로에서는 `venom_bee` stable ID 기록·분양 목록 등장·결과 화면 귀환 후 스폰 처치가 확인됐다. 분양 버튼으로 독침벌 100 탈렌트 구매(260→160), 관리 화면 장착, Bootstrap `이어하기` 후 승리/해금/잔액/장착 복원을 확인했다. 검증용 사수 A=Wolf/B=Bear는 같은 전투에서 별도 장착·A 기본 HP/B 120→132 HP·직접 피해 A 20→21/B 20 유지·각자 Wolf/Bear 습격·이어하기 후 별도 ID 유지를 확인했으며 정식 동료는 추가하지 않았다. 8종 모두 장착 후 실제 습격에서 자기 ID의 Image 생성, 최소 2개 Sprite 프레임 교체, 위치 이동, 종료 뒤 오브젝트 정리를 확인했다. 독침벌 습격은 저레벨 대상 HP 70→1, 생존 대상 일반 독, CD3을 확인했다.
- 전투/UI 회귀: 정조준 후열만 선택·적중·쿨타임, 화살비 후열 적중과 후열 부재 시 행동/CD 미소비, 기본 공격 대상 선택·종료, 회복약 대상 선택 취소 시 2개 유지와 사용 후 1개·HP 회복, Guard 피해 10→5 및 다음 자기 차례 종료, 일반전 Flee를 실제 Play에서 확인했다. 분양·관리 Game View는 1016×569(약 16:9), 1280×800(16:10), 800×600에서 목록·설명·조작 버튼 접근성을 확인했다. 고정 5종 목록은 ScrollRect 없이 패널에 들어간다. 관리 사수 선택 단계의 빈 Image 흰 사각형을 수정해 선택 전/복귀 시 숨김·상세 선택 시 재표시를 Play Mode에서 재검증했다(Fix commit `6661f59`). 최종 Editor 컴파일 대기 없음·Console Error/Warning 0. 검증용 Asset, 격리 저장, 화면 크기 및 임시 Screenshot은 정리했다. 도발 강제 대상의 실제 재현, 8종 외형의 세밀한 미관 판단, 모든 지역·모든 전투 스킬의 전체 회귀는 이번 QA 범위에서 미검증이다. `문서/10_전투/전투시스템.md`의 Beast 패시브/Wolf 전용 서술은 현재 설계·코드와 충돌해 별도 문서 정리가 필요하다.

- 2026-09-29 BeastCompanion 잔여 백그라운드 검증: Unity 6000.5.7f1의 포커스를 받지 않은 Edit Mode에서 `TurnOrderQueue`를 실제 생성하여 동일 우선도 민첩 10 사수/11 적의 순서가 Wolf일 때 적→사수, Fox일 때 사수→적으로 바뀜을 확인했다. 8종 Definition ID와 사용 가능한 원본/생성 프레임 수는 Wolf 6, Bear 5, Fox 6, 독침벌 10, 맹독뱀 4, 숲거미 8, 이끼딱정벌레 6, 그늘박쥐 6이었다. 격리 서비스 상태 검사에서 승리 종 중복 제거, 승리 후 분양 후보 표시, 구버전 Beast 필드 누락 및 잘못된 장착 ID의 Wolf fallback, Beast 저장 DTO 왕복과 Guard 직접 피해 20→10을 확인했다. Console 오류·경고 조회는 0건이다. 코드 수정은 없었다. 실제 Battle의 Victory/Flee/Defeat 기록 분기, 구매/Continue 최종 회귀, 두 사수 독립 장착, 8종 습격의 화면 재생·종료 정리, Skill/Item/Attack/Flee 전체 행동 회귀, UI 화면 비율별 시각 QA는 이번 백그라운드 검사에서 미검증이다. 이전 격리 Play 검증 결과는 아래 기록과 같다. 다음 권장 작업은 포커스 전환 없는 Play Mode 테스트 하네스 확보 후 남은 전투·저장 회귀와 별도 시각 QA다.

- 2026-09-29 사수 전용 BeastCompanion 공용 상태, 무료 Wolf/Bear/Fox 장착, 몬스터 펫 5종의 승리 기록→100 탈렌트 분양→영구 해금, 캐릭터별 장착 Save, 분양·관리 UI 호출 지점, 패시브와 `동료의 습격` 외형·독 효과를 연결했다. Chapter 2 시설·NPC·Scene과 세린은 만들지 않았다. 설계 commit `8ee189f`, 기능 commit `fe0ce94`, 검증 수정 commit `106bae1`.
- 검증: Unity 6000.5.7f1 컴파일 및 최종 Console 오류·경고 0. 격리 계산에서 Wolf 100→105/DoT 100, Bear 80↔88 및 1~100 HP 왕복 무증가, Fox 민첩 +2, 독침벌 180 피해·일반 독 3회, 뱀·박쥐 첫 유효 상태 1회 차단과 새 Battle 충전, 거미 광역 100→90·Guard 45/단일·DoT 100, 딱정벌레 직접 100→95/DoT 100, 기본 3종 사용권·구버전 Beast 누락→Wolf·중복 승리/잔액 부족 구매를 확인했다. 5종 원본 프레임 10/8/4/6/6개를 읽었다. 사용자 승인 후 격리 Play Mode에서 분양 숨김→승리 후 표시, 100 탈렌트 구매·중복 거절, Bear/Fox/벌 장착, 150 탈렌트·장착·승리 기록 Save/Continue, 분양·관리 UI 표시와 Bear 버튼 장착을 확인했다. 화면 전환 직후 옛 행 비활성화도 확인했다. 두 사수 독립 장착, 실제 전투에서 8종의 등장 애니메이션, 전투 명령·상태 전체 회귀와 여러 화면 비율의 미관은 미검증이다. 다음 권장 작업은 이 격리 전투 회귀와 Game View 비율별 QA다.

- 2026-09-28 Main11 `main_11_silent_catacomb` 「침묵의 지하묘지」를 Main10 완료 후 기존 Field_03 입구에 연결했다. B1/B2 조사·하강로·침묵의 파수꾼·보스 뒤 봉인실·부서진 석판 조각·귀환의 순차 목표 13개, 대사와 Navigation 지점, 입구 안전지대 완료 보상 EXP60/탈렌트50을 구현했다. Quest Item은 판매·사용 불가이며 완료 뒤에도 유지된다. Main12 Definition/Scene은 만들지 않았다. 정식 설계 commit `02beeed`, 기능 commit `fdb90e7`.
- 검증: Unity 6000.5.7f1 백그라운드 Play에서 Main10 완료 상태의 Field_03 실제 입구→Main11 시작, B1 목표 1~5 순차 진행·B2 진입, B2 목표 6~9 순차 진행, 보스 승리 시 10번 목표만 완료, 봉인실 조사·석판 1개 획득·`아직 조사한다` 선택 유지·`돌아간다` 선택 후 안전지대에서만 전체 완료를 확인했다. 보스 전멸은 목표 미진행·입구 안전지대 복귀·재도전 가능이었다. 13번 목표에서 저장/복원해 석판·보스 완료가 유지됐고, 완료 뒤 재알림·재저장/복원에서도 EXP 180·탈렌트 85(보스+Quest)가 늘지 않았다. Quest Item 판매 거절 및 B2 남쪽 출구→B1 북쪽 Spawn 복귀를 확인했다. 컴파일 오류 0; 슬롯 없이 출구만 확인한 테스트의 자동 저장 건너뜀 경고 3건, 최종 재컴파일 후 Unity MCP WebSocket 경고 1건은 기능 오류와 구분한다. 이번 기능 스테이징의 `git diff --check` 통과. 실제 Game View 화면 비율별 미관·글자 배치, B1/B2 일반 조우 전체와 기존 Skill 전체 회귀는 미확인이다. 다음 권장 작업은 이 시각 QA와 남은 회귀 검사다.

- 2026-09-28 `Dungeon_01_B2`의 봉인 구역 공간, 일반 조우 B2-01~04, 침묵의 잔영·봉인 수호체·첫 보스 침묵의 파수꾼을 구현했다. 최종 PNG 4개를 원본 그대로 등록하고 128×128 Sprite Slice 및 전투 아이템·해제약 아이콘을 연결했다. 보스는 표식·예고형 충격과 HP 60% 이하 1회 수호체 2기 소환·직접 피해 장막을 사용하며, Main11과 보스 뒤 스토리 단서는 아직 구현하지 않았다. 문서 commit `0440cf2`, 기능 commit `a393e5c`.
- 검증: Unity 6000.5.7f1 컴파일 성공. 격리 Play Mode에서 B2-01 접촉·침묵·전투 아이템 침묵 해제·도망·복귀, 보스 접촉·도망 차단·첫 공격·표식·충격 예고→다음 행동 전체 공격과 Guard 피해 31→16, 60% 전환·수호체 2기·장막 직접 피해 100→70/DoT 100 유지·수호체 전멸 시 장막 해제, 보스 승리·복귀·스폰 비재생성을 확인했다. 순수 로직 검사에서 잔영의 자기 행동 2회 재사용 제한, 수호체의 직접 피해 감소·DoT 제외·KO 해제를 확인했다. 감사 캐릭터에 지정한 유효하지 않은 `path_intellectual` 때문에 저장 경고가 21건 발생했고, 재컴파일 후 Unity MCP WebSocket 경고 1건이 남았다. 일반 `git diff --check`는 Unity 생성 YAML의 빈 값 뒤 공백을 지적하지만 이를 제외한 검사에는 오류가 없다. B2-02~04 실제 전투 승리, B1·기존 스킬 전체 회귀, Game View 시각 QA는 아직 확인하지 않았다. 다음 작업은 올바른 경로 ID의 격리 슬롯으로 나머지 조우와 시각 QA를 확인하는 것이다.

- 2026-09-28 전투 공용 아이템 명령과 생존 아군 대상 선택·취소·성공 후 공용 Inventory 소비, HP/MP 회복 및 Poison/Burn/Shock/Silence 전용 해제약을 구현했다. 4종 가격은 각 구매 40/판매 20 탈렌트다. Field03 지하묘지 입구에 전투 없는 안전지대, 보급 상인(침묵 해제 물약 최초 판매), 파티 안내인, 기존 무료 완전 회복 치유사, 상주 묘지 감시인을 런타임 설치한다. 전멸은 최근 활성 안전지대 Spawn으로 복귀하고 HP를 회복하며 소비품·EXP·탈렌트·장비·영구 진행은 되돌리지 않는다. Boss는 아직 없고 전투 임시 상태는 Battle Scene 종료로 초기화된다. 문서 commit `e150349`, 기능 commit `94dd402`.
- 검증: Unity 6000.5.7f1 컴파일 및 최종 Console 오류·경고 0. 격리 검사에서 전용 해제약 4종의 개별 상태 제거·무효 사용 미소비, HP 회복, 안전지대 저장 JSON 왕복과 Version1 누락 필드를 확인했다. 사용자 허락을 받아 격리 저장 슬롯의 실제 Play Mode에서 Player·태온·미엘·폴의 아이템 명령, 전열↔후열 대상, 취소·무효 사용 미소비, 성공 사용 후 행동 진행, 침묵 중 스킬 차단과 자가 해제, 정화의 4종 동시 제거를 확인했다. Field03의 NPC 4명 생성, 보급 상점·파티 UI·무료 치유사·감시인 대화, 몬스터 활동 중심의 안전지대 비침범, Main10 입구 진입을 확인했다. Dungeon 일반 조우 전멸 후 Field03 안전지대 복귀, HP 회복, 소비품·EXP·탈렌트 보존, 일반 몬스터 재전투 가능, 저장·복원된 안전지대와 공격·방어·도주 기본 회귀를 확인했다. 보급 상점과 파티 UI는 직접 열어 확인했으나 해당 NPC의 상호작용 버튼 경로, Bootstrap Continue UI, 전용 해제약 3종의 실제 Battle UI, 기존 스킬 15개 전체 회귀, Boss Play와 화면 비율별 시각 QA는 후속 확인 사항이다. 다음 권장 작업은 남은 UI 경로와 화면 비율 확인이다.

- `Dungeon_01` 「침묵의 지하묘지 B1」의 입구 홀·중앙 회랑·서쪽 안치실·동쪽 무너진 묘실·북쪽 봉인실 Scene과 기존 몬스터 일반 조우 4개를 구현했다. Main10 완료 격리 슬롯의 Field03 입구 상호작용→Dungeon Spawn, B1-01 실제 충돌·UI 공격/스킬/방어·승리(EXP16·탈렌트9·드롭 없음, Lv12), 결과 버튼 복귀·해당 스폰 제거, B1-02 UI 도망·스폰 유지·복귀 직후 1.99초 재조우 유예, B1 남쪽 출구 Trigger→Field03 복귀를 Play Mode에서 확인했다. Bootstrap Continue의 던전 좌표 복원과 Bounds 밖 좌표의 Spawn fallback도 확인했다. 기능 버그·코드 변경은 없었다. 감사 중 예상된 좌표 fallback/슬롯 해제 경고 2건을 기록했고 최종 Console 오류·경고 0건이다. 전용 묘지 Prop이 없어 기존 Kenney 회색 석재 Sprite로 단순하게 표현했으며 B2·Main11·새 몬스터·보스는 없다. 다음 작업은 별도 Main11 기획 전 실제 이동 난이도·석재 미관 확인이다. 기능 commit: `c72d771`.

- 공용 해로운 상태 `침묵 1`을 구현했다. 다음 성공한 실제 행동까지 Skill 명령만 차단하고 공격·방어는 허용하며, 완료 때 제거한다. 재적용은 중첩 없이 갱신하고 정화는 독·화상·감전·침묵을 한 번에 제거한다. 침묵 치유사는 자기 정화를 사용할 수 없다. HUD·상세에는 아이콘 없이 한글 fallback을 표시한다. 새 몬스터·부여 스킬·Dungeon·Quest는 추가하지 않았다. Runtime/Editor 백그라운드 컴파일 오류 0, Scene·저장 비변경 Editor 감사 통과, Unity Console 오류·경고 0. 실제 Battle Game View의 배지 가독성과 입력 포커스는 사용자 확인 사항이며 다음 작업은 별도 기획에 따라 침묵 부여 수단을 연결하는 것이다. 기능 commit: `3c8de16`.

- World EXP HUD를 좌상단 Anchor `(0,1)`, 좌·상단 24px 여백, 320×72 패널과 280×9 EXP Bar로 조정했다. 아이콘·레벨·이름·EXP 숫자를 첫 줄에 분리하고 EXP/레벨/저장 로직은 변경하지 않았다. 좁은 화면의 Quest Toast는 위쪽 24px을 유지하면서 오른쪽으로 옮겨 가로 간격 16px을 확보하고, Quest Navigation 마커는 EXP 패널과 겹칠 때만 아래로 피한다. 향후 EXP 위치 프리셋은 Anchor·Pivot·여백으로 확장할 수 있다. 백그라운드 Runtime/Editor Roslyn 컴파일 오류 0, 사용자 승인 후 1016×569 Game View 배치와 Play Mode HUD 감사·마커 회피 검사 통과, Console 오류·경고 0. `AGENTS.md`는 백그라운드 우선과 foreground 사전 승인 원칙으로 보완했다. HUD commit: `a4e5a63`, Toast 충돌 수정: `1fd64f3`, Navigation 충돌 수정: `96b90f9`, 규칙 commit: `046ca33`.

- 2026-09-24 유지보수: 실제 `BattleSkillCatalog`·상태 런타임·길 특성과 문서를 대조해 스킬/전투 정본, 미구현 후보, 과거 프리뷰를 분리했다. 프로젝트 전용 핵심 C#의 Quest·Save·Battle 길 특성·Companion·Navigation 설명 주석을 보강하고, `AGENTS.md`에 비전공자용 주석과 백그라운드 검증·화면 포커스 보호 규칙을 추가했다. 기능 동작은 변경하지 않았다. 백그라운드 Roslyn 전체 Runtime 컴파일 오류 0, 변경 Markdown 상대 링크와 관련 파일 diff 검사는 통과했다. Unity Editor가 실행 중이지 않아 Test Runner·Console·Main10 최종 Game View는 이번 세션에 재확인하지 못했다. 입구 시각 확인은 사용자 직접 확인 사항이다. 공식 `마음의 상처의 길`과 현재 Path Asset 표시명 `마음의 상처`의 불일치는 별도 Asset 수정 사항이다. 관련 commit: `d6e2fb2`(문서), `95ae1df`(규칙), `39ca7bd`(주석).

- Main10 `main_10_center_of_silence` 「침묵의 중심」의 7개 순차 조사 목표를 Field03에 구현했다. Main09 석재를 재사용하고 입구를 단계별로 공개한다. 완료 보상은 EXP40·탈렌트40·아이템 없음이며 Dungeon_01 Scene이 없을 때 입구는 안내 대사로 안전하게 끝난다. 원인·배후·빛의 정체는 공개하지 않았다.
- 어제 Unity Play Mode의 격리 저장 감사에서 61개 검사가 통과했고, 입구 위치를 위로 조정한 뒤 재실행한 감사도 통과했다. 이번 세션에는 Unity MCP와 Editor가 연결되지 않아 조정 후 최종 Game View 시각 확인 및 Console 재확인은 수행하지 못했다. 사용자 확인: Field03 실제 화면에서 입구와 HUD의 겹침, 조사 표식의 가독성을 확인한다. 다음 권장 작업은 이 시각 확인 후 Dungeon_01 별도 기획이다. 기능 commit: `91eee5f`.

- Main09 `main_09_reunion_in_silence` 「침묵 속의 재회」 구현: Field03의 푸른 빛 방향→평행 바퀴 자국→폴 재회→정보 교환→가공된 석재→폴의 합류 결정, stable Objective 6개. 원인·배후·빛의 정체는 미확정이다. 완료 보상 EXP35·탈렌트35·아이템 없음, `companion_paul` 1회 영구 해금. 이후 Main10도 위에 기록한 범위로 구현되었다.
- Main05 완료 후 시작 마을의 기존 파티 안내인에서 Player 고정+해금 동료 최대 2명 편성, 전열/후열, 초상화·직업·길·선택 상태, 확정/취소, 공격 역할 부족 경고를 제공한다. Guardian는 폴 최초 해금 시 미엘+폴, Healer는 태온+폴, Sharpshooter/Fighter/Mage는 태온+미엘을 기본 선택하며 수동 편성은 보존한다.
- 일반 Battle은 저장한 동료·행을 사용한다. Main03/04/07 Story 임시 편성은 독립적으로 유지된다. Save Version1에 선택적 Formation·수동 편성 플래그를 추가했고 구버전 누락 필드는 기본값으로 복원한다. `UserData/Main09Audit` 격리 폴더에서 A~E Save→Bootstrap→Continue, 세 일반 전투 조합 승리, 폴 마도사 스킬, Main07 Story 참가 구성, Main08→09 같은 Scene 연결을 Unity Play Mode 115개 검사로 확인했다. 상점·인벤토리 및 기존 퀘스트 서비스 감사도 통과했다.
- 사용자 확인: 마을 파티 화면의 실제 해상도별 글자·초상화 배치와 Field03의 조사 표식이 지형과 겹치지 않는지 Game View에서 시각 확인. MCP 화면 캡처가 오래된 프레임을 반환하고 외부 패키지의 PlayerLoop 진단을 남겨 자동 시각 QA는 제외했다. 이 문단의 다음 작업 제안은 Main10 구현 전 기록이며 현재 계획은 문서 상단을 따른다.
- 관련 기능 commit: `a93fef8`(파티), `6b9829d`(Main09). GitHub push는 하지 않았다.

- `Grave_Wight_Battle_Final.png` 사용자 최종 원본을 수정 없이 정식 등록하고 `monster_grave_wight` 묘지 망자를 구현했다. Lv7·HP125·공격20·민첩8·EXP32·탈렌트9의 느린 전열형 언데드이며 향후 `Dungeon_01` 「침묵의 지하묘지」에서 사용한다.
- 실제 1254×1254 RGBA의 Alpha 배치를 분석해 209×220px 6프레임씩 Idle/Walk/Attack/Hit/Defeat를 연결했다. Defeat는 왼쪽→오른쪽 F00→F05로 한 번 재생한 뒤 마지막 프레임을 유지한다. Point·Clamp·Mip Map Off·sRGB/Alpha On·Uncompressed·PPU 209로 Import했다.
- `material_grave_wight_fragment` 망자의 파편(판매 8 탈렌트, 드롭 35%, 아이콘 null/fallback)을 추가했다. 격리된 Battle 검증에서 묘지 망자 1마리와 전열 2마리 구성, 우향, 애니메이션 전환, HP/공격/민첩, 2마리 EXP64·탈렌트18·드롭 집계를 확인했다. 기존 6종 Definition과 이끼갑충·그늘박쥐 프레임 연결을 회귀 검사했으며 최종 Console Error/Warning 0이다. 기능 commit: `e80943f`.

- Main 02 지정 전투 진행을 `FieldMonsterSpawnDefinition` 객체 참조가 아니라 전투 진입 시 보존한 stable encounter ID로 판정하도록 수정했다. `field01_main02_investigation_encounter` 승리만 Objective 2→3을 진행하며 다른 일반 조우·도주·패배는 진행하지 않는다.
- 추적 중인 Quest Objective의 실제 Transform을 stable ID Registry로 연결하고, 화면 안 `◆ 현재 목표 + 대상명`, 화면 밖 방향 화살표·거리 Indicator를 표시하는 공용 Quest Navigation을 추가했다. Quest Log의 선택과 추적을 분리하고 `추적` 버튼·T·게임패드 Y를 지원하며 Modal 중에는 안내를 숨긴다.
- Main 01~08의 현재 Scene에서 안전하게 식별되는 NPC, 조사 지점, 흔적, 지정 Story Encounter를 연결했다. Scene 밖 목적지로 향하는 출구 안내는 후속 확장 대상으로 남겼다.
- Unity Play Mode에서 Main 02 지정/일반 encounter 분리, Objective 1/2/3/완료 JSON 저장·복원, 화면 안 Marker, 화면 밖 Clamp Indicator, Quest Log 추적 표시를 검증했다. 기능 commit: `ba9ef7c`.

- Field 03 `침묵의 숲길`과 Main 08 `main_08_what_they_avoid` 「피하고 있는 것」을 구현했다. 회피 흔적 3개, 폴의 안정적인 바퀴 자국, Field 03 조사 흔적, 깊은 구역의 푸른 빛까지 stable ID 7개로 진행하며 보상은 EXP 30·탈렌트 30·아이템 없음이다.
- 사용자 최종 원본 `Moss_Beetle_Battle_Final.png`, `Shade_Bat_Battle_Final.png`를 수정 없이 등록하고 이끼갑충·그늘박쥐의 명시적 Idle/Walk/Attack/Hit/Defeat, 민첩·보상·재료 Loot를 연결했다. Field 03 조우 수는 초입 3 > 중간 2 > 깊은 곳 1이다.
- Unity 정적 로드와 Field 03 Play Mode에서 Scene·Player·전환 1개·몬스터 6개·프레임 애니메이터 6개, 그늘박쥐 일반 전투 진입과 화면 방향을 확인했다. 두 몬스터의 Attack·Hit·Defeat 최종 프레임 전환과 Encounter A/B/C 구성을 런타임 검사했다. 직접 Scene 진입 검증에서는 저장 슬롯이 없어 자동 저장 생략 Warning 2건이 발생했으며 기능 오류는 없었다. Save A~G의 실제 Bootstrap 왕복과 모든 승패/도주 조작은 후속 수동 확인이 필요하다. 기능 commit: `1c10492`.

- Main 07 `main_07_deep_tracks` 「깊게 패인 흔적」을 구현했다. Main 06 완료 뒤 `Field_02`에서 두 줄 바퀴 자국 조사 → 부상자 확인 → 자국 추적 → 폴 첫 대화 → 지정 전투 → 미엘에게 복귀 → 미엘·폴 첫 만남 → 폴 작별 순서로 진행하며, stable Objective/Actor/Encounter ID 8개로 저장한다.
- 폴(`companion_paul`)은 28세 남성 마도사·지체의 길 휠체어 사용자다. 공식 `Portrait_Paul.png`와 `Paul_Battle_Final.png`를 연결했고, 전투 시 후열에서 기존 파이어볼·썬더볼트·가이아 웰과 지체의 길 보정을 사용한다. Main 07에서는 Player·Taeon·Paul 대 숲거미 2·맹독뱀 1의 Story Temporary Companion으로만 참가하며, 미엘은 전투에서 제외되고 폴은 영구 해금/기본 파티에 추가되지 않는다.
- Main 07 보상은 EXP 30·탈렌트 25·아이템 없음이며 완료 신호 반복에도 한 번만 지급된다. 임시 슬롯 5에서 A~G(Available, 각 핵심 진행 단계, Completed)를 실제 저장→로드→복원해 Scene·Objective·파티·보상과 중복 방지를 확인하고 삭제했다.
- Unity MCP에서 폴 초상화 stable ID 조회, 전투 시트 33프레임, Idle·Attack·Guard·Skill·Hit·Defeat Animator, 후열 마도사/지체의 길, 마도사 3스킬, Main 07 참가자 구성을 확인했다. Main 07 필드 진행·대화 Modal·보상도 Play Mode에서 검증했으며 최종 Console Error/Warning 0이다. 기능 commit: `48420ea`. 이 문단의 다음 작업 제안은 당시 기록이다.
- Main 06 `main_06_into_the_forest` 「숲으로」를 구현했다. Main 05 완료 직후에는 Available을 유지하고 정상 동선으로 `Field_01`에 나서면 시작된다. `Field_02` 진입 → `field02_main06_anomaly_trace` 조사 두 목표를 stable ID로 진행하며, 흔적은 몬스터들이 평소 길을 벗어나 한 방향으로 넓게 퍼진 사실만 전달한다.
- 조사 대화는 태온 2쪽·미엘 1쪽·플레이어 1쪽으로 구성했다. 태온·미엘 공식 초상화와 플레이어 null-safe 배치를 기존 `DialoguePortraitCatalog`로 사용하고, 대화 완료 뒤에만 퀘스트를 완료한다. 보상은 EXP 20·탈렌트 20·아이템 없음이며 반복 신호에도 한 번만 지급된다.
- Unity MCP Play Mode에서 Main 05 완료→Main 06 Available, Field_01 시작, Field_02 진입 목표 갱신, 조사 대화 Modal 열기/닫기, 완료와 EXP/탈렌트 20 지급, 중복 방지를 확인했다. 임시 슬롯 5를 재사용해 A~E 상태를 각각 저장→로드→복원하고 Scene·Objective·태온/미엘 정식 동료·보상을 확인한 뒤 삭제했다. Field_02 기존 몬스터 6개 로드와 최종 Console Error/Warning 0을 확인했다. 기능 commit: `a4b50a5`.
- Main 05 `main_05_return_of_three` 「돌아온 세 사람」을 실제 구현해 Main 01~05의 1장 도입부를 마무리했다. Main 04 완료 뒤 `Field_01`에서 시작해 시작 마을 귀환 → 남문 경비병 보고 → 주민 대표 최종 보고의 stable Objective 3개로 진행한다.
- 보고 대화는 Main 04의 다중 화자 구조를 재사용한다. 남문 경비병·주민 대표는 Sprite 미연결 시 null-safe 글자 배치, 태온은 `Portrait_Taeon`, 미엘은 `Portrait_Miel`, 플레이어는 초상화 숨김으로 전환된다. 결론은 초원 너머 숲 `Field_02`에서도 비슷한 일이 있다는 방향까지만 제시하고 이상 이동의 원인은 확정하지 않는다.
- Main 05 완료 보상은 기존 `RewardBundle`의 EXP 40·탈렌트 40·`item_healing_potion_small` 회복약 1개다. 보상 지급 뒤 `companion_taeon`·`companion_miel`을 stable ID 기반 정식 동료로 해금하고 기본 파티를 Player·Taeon·Miel로 저장한다. 기존 Version 1 Save에 동료 필드가 없으면 미해금 상태로 호환하며 Main 03/04 Story Encounter의 임시 참가 구조는 유지한다.
- Unity MCP Play Mode에서 Quest Log 목표·보상, Field 귀환, 경비병/주민 대표 Marker, 두 보고 대사와 Portrait 전환, Field_02 대사, Quest 완료, EXP 40·탈렌트 40·회복약 1 지급, 합류 알림 큐, 정식 동료·기본 파티 저장을 확인했다. 임시 슬롯 5로 A~E 진행 복원과 실제 Bootstrap Continue, 반복 주민 대표 대화·Scene 왕복의 보상 중복 방지, Field_01 Story Actor 비활성, 레벨업 시 새 MaxHP 완전 회복을 확인한 뒤 슬롯을 삭제했다. 최종 Console Error/Warning 0. 기능 commit: `b47c2da`. 다음 작업은 Field_02 후속 Main Quest 설계다.
- Main 04 `main_04_three_people` 「세 사람」을 실제 구현했다. Main 03 완료 뒤 `Field_01`에서 앞서간 흔적 도달 → 부상자를 치료 중인 미엘과 첫 다중 화자 대화 → 플레이어·태온·미엘의 지정 3대3 Story Encounter → 전투 후 단서 결합·마을 귀환 결정 순서로 진행한다. stable Location/NPC/Encounter ID 4개로 저장하며 Main 04 자체 완료 보상은 없다.
- 미엘은 구조 대기 대상이 아니라 도착 전부터 부상자를 치료하는 능동적 치유사로 등장하며, 전투 뒤 자기 상처도 확인하겠다고 말한다. 세 사람의 관찰을 합쳐 초원 안쪽의 이상만 추론하고 원인·배후·오염은 확정하지 않는다. 태온·미엘은 기존 공식 초상화와 전투 애셋을 사용하지만 아직 영구 파티가 아닌 Story Temporary Companion이다.
- 지정 전투 `field01_main04_three_people_encounter`는 초원 슬라임 1·독침벌 2와 싸우며 승리만 진행한다. 도주·패배는 같은 목표에서 재도전하고 기존 몬스터 EXP·탈렌트·Material Loot 정책을 유지한다. 완료 뒤 최소 데이터만 준비한 Main 05 `main_05_return_of_three` 「돌아온 세 사람」이 Available이 되며 실제 Main 05 내용과 정식 동료 합류는 구현하지 않았다.
- Unity MCP Play Mode에서 목표 4개·Quest Log, 화자별 태온/미엘 초상화와 플레이어 null-safe 배치, 실제 3대3 참가자·Formation, 미엘 회복/MP, 태온·미엘 공식 전투 애니메이션, 패배·도주 무진행, 승리 보상과 후속 대화, Main 04 Completed/Main 05 Available을 확인했다. 임시 슬롯 5에서 A~E 저장/이어하기를 모두 확인하고 삭제했으며, C/D 마을 왕복 재진입 시 대화·전투가 반복 실행되지 않고 승리 신호 중복도 진행을 건너뛰지 않음을 확인했다. 최종 Console Error/Warning 0. 기능 commit: `d0dd615`. 다음 작업은 Main 05 「돌아온 세 사람」이다.
- Main 03 `main_03_unfamiliar_companion` 「낯선 동행」을 실제 구현했다. Main 02 완료 뒤 `Field_01`에서 태온 첫 발견 → 첫 대화 → 지정 Story Encounter → 전투 후 단서 결합 → 태온 임시 동행 → 다음 조사 지점 순서로 진행한다. 목표는 stable Location/NPC/Encounter ID 5개로 저장하며 Main 03 자체 보상은 없다.
- 태온은 Main 02 흔적보다 안쪽 `(7.5, -0.8)`에서 공식 Field Sprite와 `Portrait_Taeon.png`를 사용한다. `관찰 → 이해 → 보호`를 중심으로 플레이어의 흔적과 태온의 반복 행동 관찰을 결합하고, 원인·배후·오염은 확정하지 않는다. 미엘은 등장하지 않으며 Main 04 「세 사람」에서 처음 등장한다.
- `field01_main03_taeon_encounter`는 일반 Field Encounter와 분리된 플레이어+태온 대 초원 슬라임+독침벌 2대2 전투다. 태온은 아직 정식 동료가 아닌 Story Temporary Companion이며 기존 수호자·지적의 길·공식 Left 전투 애니메이션을 그대로 쓴다. 승리만 Objective를 진행하고 도주·패배는 같은 목표에서 재도전한다.
- Unity MCP Play Mode에서 Quest Log/목표 5개, Marker, 태온 초상화·공식 Sprite, 실제 2대2 참가자, Idle·Attack·Guard·Skill·Hit, 기존 몬스터 EXP 20·탈렌트 7 지급, 두 번째 대화, 안전한 다음 지점 재배치, 치료 흔적, Main 03 Completed와 Main 04 Available을 확인했다. 임시 슬롯 5에서 A~E 저장/불러오기를 모두 확인한 뒤 삭제했고, 마을 왕복 재진입과 도주·패배 무진행도 확인했다. 기능 commit: `372d1fe`. 다음 작업은 Main 04 「세 사람」이다.
- 월드 상시 Quest Tracker를 정식 Quest Log로 교체했다. 키보드 `Q`와 게임패드 View/Back 계열 `<Gamepad>/select`로 열고 닫으며, `Esc`/게임패드 Cancel로 닫는다. 요청의 `<Gamepad>/selectButton`은 현재 Input System 장치에 없는 경로여서 실제 유효한 `select`를 사용했다.
- Quest Log는 왼쪽 ScrollRect에 `[메인 퀘스트]`/`[서브 퀘스트]`와 동적 제목 행, 오른쪽에 QuestDefinition 기반 이름·유형·설명·현재 Objective·단계·보상을 표시한다. Active Main을 기본 선택하고 Main이 없으면 첫 Side, 0개면 `진행 중인 퀘스트가 없습니다.`를 표시한다. 선택은 색상과 `▶`를 함께 쓰며 방향키·D-pad·Left Stick·Tab·Submit을 지원한다.
- 기존 Quest HUD는 상시 내용을 제거하고 Quest 시작·Objective 갱신·완료 시 4초 Toast만 표시한다. Quest Log는 `WorldModalState`를 재사용해 Dialogue·Shop·Inventory와 상호 배제되고, 열림 중 Player 이동과 World HUD를 잠근 뒤 닫을 때 Focus·잠금·Modal 소유권을 해제한다. Quest Save 구조와 NPC Quest Marker는 변경하지 않았다.
- Main 01 목표를 `주민 대표와 대화하세요.` → `남문 경비병과 대화하세요.`, Main 02 목표를 `초원 안쪽의 조사 지점으로 이동하세요.` → `조사 지점 주변의 몬스터 무리를 물리치세요.` → `전투 지점 너머의 흔적을 조사하세요.`로 명확히 했고 기존 스토리 문서 범위 안의 Quest 설명을 Definition에 추가했다.
- Unity MCP에서 Bootstrap 정상 시작, Q Toggle/Esc, 게임패드 Select Open/Cancel Close, Main 기본 선택, Main/Side 3개와 임시 Side 10개 동적 목록·Scroll, 0개 상태, 방향키·D-pad Focus, Tab Close Focus, Dialogue/Shop/Inventory 양방향 상호 배제, 이동 잠금/복구를 확인했다. Quest System Audit로 Main 01/02 진행·Save/Continue·구버전 Save·Toast 시작/갱신/완료/자동 숨김을 재검증했고 임시 Side 데이터는 저장하지 않았다. Console Error/Warning 0. 마지막 기능 commit: `b63d6b4`.
- 다음 직접 확인은 실제 1280×720 및 다른 화면 비율에서 긴 한글 설명 줄바꿈, 실제 연결 게임패드의 View/Back·D-pad·Left Stick 감각, Quest Toast의 4초 체감 시간이다.
- 대화 UI에 150×150 초상화 슬롯을 추가했다. `DialoguePortraitDefinition`/`DialoguePortraitCatalog`가 표시 이름과 분리된 stable ID로 Sprite를 조회하며, Sprite가 null이거나 정의가 없으면 프레임과 왼쪽 여백을 함께 숨겨 기존 글자 배치를 유지한다. 기존 ID 없는 `Show` 호출도 호환 오버로드로 유지한다.
- 주민 대표 `starter-village-main-guide`, 남문 경비병 `starter-village-gate-guard`, 태온 `companion_taeon`, 미엘 `companion_miel`의 Resources 정의를 준비했다. 태온 `Portrait_Taeon.png`와 미엘 공식 초상화 `Portrait_Miel.png` 적용을 완료했으며, 나머지 두 정의는 Sprite를 연결하기 전까지 기존 null-safe 글자 배치를 유지한다. 콘텐츠 생성 메뉴는 기존 연결 Sprite를 보존한다. 마지막 기능 commit: `ee9efcd`.
- 주민 대표·남문 경비병을 포함한 `VillageNpcRole`과 Main 01 연속 대화가 stable NPC ID를 전달하도록 연결했다. 대화/확인/연속 대화, 모달 소유권, 거리 이탈 종료, World HUD 숨김·복귀 경로는 유지했으며 Save 스키마는 변경하지 않았다.
- Unity 6000.5.7f1에서 컴파일 Error 0, 핵심 정의 4개 로드와 null Sprite를 확인했다. Play Mode에서 네 ID에 임시 Sprite를 각각 주입해 슬롯 표시·본문 여백 190을 확인하고, null 자동 숨김·본문 여백 28 복귀·모달 획득/해제·거리 이탈 자동 종료·HUD owner 1→0을 확인했다. 검증 Sprite는 저장하지 않았고 최종 Console Error/Warning은 0이다. 마지막 기능 commit: `f9d1437`.
- Unity에서 최종 초상화 Sprite를 연결한 뒤 실제 다양한 해상도에서 얼굴 크롭·150×150 프레임 가독성·긴 대사 줄바꿈·선택지와의 겹침을 직접 확인해야 한다. 다음 권장 작업은 태온·미엘 초상화 → 주민 대표·남문 경비병 초상화 → 태온·미엘 전투 애셋 순서다. 일반 주민·생활 NPC 초상화는 후순위다.
- 태온 공식 전투 Sprite `Taeon_Battle_Final.png`를 `companion_taeon` 전용 전투 비주얼에 적용했다. 기본 방향은 Left이며 Idle·Attack·Guard·Skill·Hit·Defeat 6개 애니메이션을 사용한다. 실제 Battle Scene Play Mode에서 재생·복귀와 Guardian 스킬 연결을 확인했고 최종 Console Error/Warning은 0이다. 마지막 기능 commit: `4854591`.
- 미엘 공식 전투 Sprite `Miel_Battle_Final.png`를 `companion_miel`에 적용했다. 태온과 같은 데이터 기반 Battle Animation 구조와 기본 Left 방향을 재사용하며 Idle·Attack·Guard·Skill·Hit·Defeat 6개 애니메이션을 연결했다. 실제 Battle Scene Play Mode에서 기본 공격·방어·치유사 3스킬·피격·전투불능, MP·회복·정화 수치와 태온 회귀를 확인했고 최종 Console Error/Warning은 0이다. 마지막 기능 commit: `34b7e06`.
- Main 02 `main_02_grassland_anomaly` 「초원의 이상」을 실제 구현했다. Main 01 완료 뒤 자동 시작하며 `Field_01`의 넓은 조사 구역 도달 → 지정 `grass_slime_01` 조우 승리 → 흔적 상호작용 순서로 진행한다. 일반 Kill Count가 아니며 다른 Encounter·도망·패배는 진행시키지 않는다.
- 조사 구역은 `(.4, 1)`, stable ID `field01_main02_investigation_area`이고, 지정 조우는 `field01_main02_investigation_encounter`, 흔적은 `(5.1, -1.2)`의 `field01_main02_tracks`다. 사용자 수정 Field Scene과 Monster Spawn/Respawn 데이터는 바꾸지 않고 런타임 연결로 분리했다.
- 흔적에는 `◇ 흩어진 흔적`과 `[E/F] 조사`를 함께 표시해 색상만으로 찾지 않게 했다. 다섯 Path별 1문장 반응은 표현만 다르고, 모두 "몬스터들이 마을을 공격하러 온 것이 아니라 무언가를 피해 밀려온 것 같다"는 같은 결론으로 합류한다. 원인·배후·오염 여부는 미확정이다.
- Main 02 자체 보상은 0이고 지정 전투의 기존 EXP·탈렌트·Material Loot는 유지한다. 완료 뒤 Main 03 `main_03_unfamiliar_companion` 「낯선 동행」이 Available이 되는 최소 데이터만 추가했으며 실제 내용은 아직 없다.
- Unity MCP Play Mode와 임시 슬롯 5에서 시작 전/조사 구역 후/지정 조우 후/완료 네 상태 저장·복원, 다른 Encounter 무시, 지정 Encounter 진행, 반복 조사 중복 완료 방지, Main 03 해금을 확인했다. 임시 슬롯과 캡처는 정리했다. 마지막 기능 commit: `d5e7417`.
- Main 01 `main_01_call_reaches` 「부름이 닿은 곳」을 실제 데이터로 추가했다. 시작 마을 주민 대표(`starter-village-main-guide`)에게 말을 걸면 별도 수락 창 없이 Active가 되고, 주민 대표 대사를 끝까지 진행하면 목표가 남문 경비병(`starter-village-gate-guard`)으로 바뀐다. 경비병 대사를 끝까지 진행하면 Completed가 되며 보상은 EXP 0·탈렌트 0·아이템 없음이다.
- Main 01 완료 뒤 Main 02 `main_02_grassland_anomaly` 「초원의 이상」이 Available이 되는 최소 해금 데이터만 추가했다. Main 02의 실제 목표·대사·Field 조사 내용은 아직 구현하지 않았다.
- 범용 NPC Quest Marker를 추가했다. `Available=✦ 새 이야기`, `ActiveObjective=◆ 현재 목표`, `ReadyToTurnIn=✓ 완료 보고`, `None=숨김`이며 색상 외에도 서로 다른 기호와 텍스트로 구분한다. stable NPC ID와 QuestDefinition의 Start/Objective/TurnIn ID를 연결하고 `QuestService.Changed` 때만 갱신한다.
- Marker는 기존 Nameplate Y 1.02, Interaction Prompt Y 1.48 위인 Y 1.92에 World Space uGUI로 배치했다. Main 01 시작 전 주민 대표만 표시되고, 첫 대화 뒤 경비병으로 이동하며, 완료 뒤 모두 사라진다.
- Unity MCP Play Mode에서 Main 01 전/주민 대표 후/완료 상태를 임시 슬롯 5에 실제 저장·불러오기해 `Available → ActiveObjective → Completed`, Main 02 Available, 반복 NPC 알림 중복 완료 방지를 확인했다. Available Marker Game View와 1.92 배치를 확인했으며 기능 코드 컴파일 Error 0이다. 프로젝트 기존 deprecated API Warning 6건은 이번 변경과 무관하게 남아 있다. 마지막 기능 commit: `30d0064`.
- 데이터 기반 퀘스트 기반을 구현했다. `QuestDefinition`/`QuestObjectiveDefinition`/`QuestCatalog`/`QuestRuntimeState`/`QuestService`를 분리했고, stable QuestId·ObjectiveId와 `TalkToNpc`·`ReachLocation`·`DefeatEncounter`·`Interact`·`GenericSignal` Notify로 Scene 검색 없이 순차 목표를 진행한다.
- 메인은 한 개만 Active인 직렬 진행과 선행 완료 해금을 사용하고, 서브는 여러 개를 동시에 진행할 수 있다. Active Main, Active Side, Completed 목록과 추적 Quest 변경 API를 제공하며 길은 진행·보상 우열에 관여하지 않는다.
- 마지막 목표 완료 시 기존 `RewardBundle`로 EXP·탈렌트·아이템을 지급하고 Completed를 기록해 반복 Signal의 중복 보상을 막는다. Save Version 1의 선택 `QuestProgress` 필드에 Active/Completed/Objective/Tracked 상태를 저장하며 구버전 null 필드는 빈 퀘스트 상태로 복원한다.
- World Overlay Canvas 왼쪽 위에 `[메인]`/`[서브]`, 퀘스트명, 현재 목표 하나를 표시하는 `QuestHudPresenter`를 추가했다. 메인 시작을 기본 추적하고 Dialogue·Shop·Inventory가 공유하는 `WorldModalState`가 열리면 HUD를 숨긴다. Main 01에서 주민 대표 목표가 경비병 목표로 즉시 바뀌고 완료 뒤 숨는다.
- Unity MCP에서 빈 Scene Play Mode 감사로 시작, 잘못된 ID 무시, 순차 목표 이동, 완료, Reward 1회, 반복 Signal 중복 방지, 서브 2개 동시 Active, 메인 선행 해금, JSON 저장/복원, 구버전 null 호환, HUD 텍스트와 Modal 숨김을 확인했다. `QUEST_SYSTEM_AUDIT Play Mode PASS`/`ALL PASS`, 최종 Console Error 0·Warning 0이다. 기능 commit은 `80227e6`이다.
- 몬스터 처치 보상을 기존 승리 EXP 흐름에 통합했다. 실제 전투불능 `Combatant`의 stable `MonsterDefinition`을 한 번 집계해 EXP·탈렌트·개체별 전리품 Roll을 `RewardBundle`로 만들고, 실제 적용 결과를 Victory UI와 Save가 함께 사용한다. 도망·패배는 세 보상 모두 0이며 EXP 레벨 차이 배율은 Currency/Loot에 적용하지 않는다.
- 초원 슬라임 3/점액 50%, 독침벌 4/침 45%, 숲거미 5/거미줄 40%, 맹독뱀 6/비늘 35%를 데이터로 추가했다. 네 Material은 MaxStack 999·SellPrice 2/3/4/5·사용 불가이며 ShopDefinition에는 넣지 않았다. 기존 Game-icons.net White 원본의 `dripping-goo`/`wasp-sting`/`spider-web`/`dorsal-scales`를 ItemDefinition.Icon에 연결하고 Material별 tint를 적용했다.
- Victory UI는 EXP, CurrencyIcon+탈렌트 텍스트, ItemId별 합산 이름·수량 또는 `없음`, 레벨 진행을 620×390 패널에 표시한다. Inventory 수용 실패 시 EXP/탈렌트는 유지하고 들어가는 전리품만 실제 지급 결과에 포함한다.
- Unity 6000.5.7f1 응답 파일 기준 Runtime/Editor 전체 정적 컴파일은 오류 0개, 기존 deprecated API 경고 Runtime 5개·Editor 1개다. Economy Inventory Audit에 조우 탈렌트 9/10/13/16, deterministic 0/1/동일 Item 3 Drop, Reward 데이터와 Material/Shop/부분 수용 검사를 추가했다. 실제 Audit/Play Mode 실행은 두 차례 시도했으나 이 PC의 Unity Licensing Client IPC가 반복 단절되어 Domain Reload 도중 진행되지 못했으며, Unity MCP 도구도 현재 세션에 노출되지 않았다. 따라서 실제 Victory UI·Save/Continue·Console 런타임 검증은 완료로 표시하지 않는다.
- 시작 마을 일반 주민 4명에 stable ID 기준 OGA-07/11/13/19 외형을 연결하고 주황 Placeholder를 제거했다. 일반 주민은 상시 이름표 없음, 기능 NPC·주민 대표는 역할명 한 줄 정책이며 Interaction Prompt는 별도로 유지한다.
- 표시되는 시작 마을 NPC 이름표는 TextMesh 폰트 40, 기본 character size 0.075(7자 이상 0.065), Y 1.02, 가운데 정렬, 검은 그림자를 적용한다. 상호작용 안내는 Y 1.48로 분리했고 플레이어 이름표·레벨 HUD는 변경하지 않았다.
- 시작 마을 역할 NPC 8명에 Eldiran CC0 외형을 stable ID 기반으로 연결했다: 잡화상 OGA-03, 장비상 OGA-17, 치유사 OGA-06, 은행 OGA-16, 동료 편성 OGA-10, 훈련장 관리 OGA-02, 남문 경비 OGA-20, 주민 대표 OGA-09.
- 원본 시트는 `Assets/ThirdParty/Eldiran/RPGCharacters32/Original`에 보존하고, 선택된 정면 32×32 셀의 정확한 마젠타만 투명화한 파생본은 `Assets/_Project/Resources/VillageNpcSprites/Eldiran`에서 관리한다. Point·PPU 28·발 기준 Pivot을 사용하며 태온·미엘 등 핵심 캐릭터 외형은 변경하지 않았다.
- 2026-09-15 Unity 6000.5.7f1에서 ScriptAssemblies 빌드·Domain Reload와 Unity MCP Play Mode 검증을 완료했다. C# 컴파일 오류와 기능 관련 런타임 Error는 없다.
- NPC 대화·치유 확인 등 상호작용 UI가 열리면 World EXP HUD를 CanvasGroup으로 숨기고 입력 간섭도 차단하며, 종료·비활성화·Scene 전환 시 복귀한다. 소유자별 공통 API라 향후 상점·은행·동료 편성·퀘스트 UI도 같은 규칙을 재사용할 수 있다.
- NPC 대화는 실제 Player/NPC Transform을 추적하며 상호작용 거리 2.0보다 넓은 3.0을 넘으면 이동을 막지 않고 자동 종료한다. 자동 종료도 공통 Hide 경로라 EXP HUD가 복귀하고 범위 밖 Prompt는 숨겨진다.
- 경제·인벤토리 최소 기반을 추가했다. Currency는 음수/overflow/실패 지출을 방어하고 Inventory는 stable ItemId+Count 배열로 저장한다. ItemDefinition/Catalog와 원자적 RewardBundle 연결점을 제공하며 상점·은행·장비·실제 아이템은 아직 구현하지 않았다.
- Save Version 1 선택 필드로 Currency/Inventory를 추가해 구버전 누락 필드는 0/빈 목록으로 호환한다. Inventory Capacity는 미확정·무제한이며 패배 화폐 손실과 휴식 EXP는 없다. 상세: `문서/12_시스템/경제와_인벤토리.md`.
- 사용자 표시 화폐명은 `탈렌트`로 확정했다. `CurrencyPresentation`이 표시명, 금색 tint와 Kenney Board Game Icons CC0 원형 토큰 Sprite를 공통 제공하며 World EXP HUD에는 추가하지 않았다.
- 시작 마을 `GeneralShop` 역할에 실제 잡화상 Shop을 연결했다. `ShopDefinition`/`ShopService`/공용 Modal UI를 분리하고 회복약 Buy20·Sell10, 마력 회복약 Buy30·Sell15를 ItemDefinition 데이터로 제공한다. 거래는 1개 단위이며 성공 시 현재 슬롯 저장, 상점 중 World EXP HUD·플레이어 이동 억제와 키보드/게임패드 Focus를 지원한다. 장비상·아이템 사용은 아직 미구현이다.
- Unity MCP Play Mode에서 100→80(회복약 0→1)→50(마력 회복약 0→1)→60(회복약 1→0), 잔액 0의 재구매 실패와 보유 0 판매 실패 무변경을 확인했다. Open/Close 5회, NPC 비활성화 자동 Close, HUD/이동 잠금 복귀, `Field_01` 왕복 재진입, 실제 빈 슬롯 5 Save/Restore 60 탈렌트·마력 회복약 3개 복원을 확인하고 검증 슬롯은 삭제했다. Audit ALL PASS, 최종 Console Error/Warning 0이며 기능 commit은 `c0a5b6a`다.
- 잡화상 거래 수량을 구매/판매 모드별 `TradeQuantity`로 일반화했다. 기본/아이템 전환 시 1, `-`·`+`·`최대` 조작, 선택 수량 총액과 거래 가능 문구를 표시하며 구매 최대는 자금·남은 MaxStack, 판매 최대는 보유량이다. `ShopService`는 Shop 포함 여부와 long 총액 overflow를 사전 검증하고 `InventoryService`도 MaxStack을 강제한다.
- Unity MCP Play Mode에서 회복약 3개 구매 200→140·0→3, 마력 회복약 4개 구매 140→20·0→4, 마력 회복약 구매 최대 0, 회복약 2개 판매 20→40·3→1과 수량 2→1 Clamp를 확인했다. MaxStack 98/99에서 추가 2개와 Shop 2개 구매가 모두 거부됐고, 빈 슬롯 5 Save/Restore 40 탈렌트·회복약 1·마력 회복약 4 복원 후 검증 슬롯을 삭제했다. Open/Close 5회와 아이템 전환 수량 초기화, HUD/이동/Focus 복귀, Audit ALL PASS를 확인했으며 기능 commit은 `e741bda`다.
- 잡화상 판매 탭을 `ShopDefinition`이 아니라 현재 Inventory 기반으로 전환했다. Count 양수·SellPrice 양수인 Consumable/Material만 이름과 수량으로 표시하고 Quest/KeyItem은 제외한다. 전량 판매 행 제거·다음 선택 이동·빈 목록 안내, 모드/아이템 전환 수량 1 초기화, `[구매]`/`[판매]` 텍스트 상태를 지원한다.
- Unity 6000.5.7f1의 열린 Editor와 Unity MCP에서 컴파일 오류 0, Economy Inventory Audit ALL PASS를 확인했다. Play Mode에서 점액 3개 판매 10→16·3→0과 품절 행 제거, 거미줄 2개 판매 16→24·4→2, 반복 모드 전환 수량 1 복귀, 빈 판매 목록 안내·조작 비활성, 테스트 슬롯 Save/Restore의 Currency·Inventory·Level/EXP·PartyResources 유지를 확인했다. 당시 Material 아이콘 미연결 상태도 기능 오류 없이 동작했다.
- Material 아이콘 연결 뒤 Unity MCP Play Mode에서 Inventory와 잡화상 판매 탭의 4개 Sprite 참조·동일 tint·`preserveAspect`를 짧게 확인했다. 기존 null Icon fallback과 판매/보상 로직은 변경하지 않았다.
- 월드/필드 소지품 UI와 소비 아이템 실제 사용을 완료했다. `ItemUseService`가 데이터 기반 HP +30/MP +12, 최대치 Clamp, 풀 자원·HP 0·MP 미사용 대상 실패 시 무소비를 공통 처리하고 `PartyResourceService`에 반영한다. `I`로 열며 아이콘·이름·수량·설명·효과·현재 파티 대상·탈렌트를 표시한다.
- Dialogue·Shop·Inventory는 `WorldModalState`의 단일 Modal 정책을 공유한다. Inventory 중 World EXP HUD와 이동을 억제하고 닫기·Scene 전환 시 복귀한다. Unity MCP에서 1280×720 Game View, 3회 반복, 상호 차단, `Field_01` 재설치, Audit ALL PASS 및 사용 뒤 Inventory/PartyResources 복원을 확인했다. 기능 commit은 `c5fe8fd`, UI commit은 `c089443`이다.

## 기준

- 갱신일: 2026-09-22
- 기준 브랜치: `main`
- 마지막 기능 관련 commit: `0454af8` (`Feature: 시작 마을 NPC 외형 적용`)
- 마지막 기능 수정 commit: `ac11356` (`Fix: 시작 마을 NPC 외형 런타임 설치 수정`)
- 마지막 NPC UI 수정 commit: `41df219` (`Fix: 시작 마을 NPC 이름표 가독성 개선`)
- 마지막 NPC 외형 마무리 commit: `23281a5` (`Feature: 시작 마을 일반 주민 외형 완성`)
- 마지막 상호작용 UI 우선순위 commit: `fcf5cf9` (`Fix: 상호작용 중 월드 EXP HUD 숨김`)
- 마지막 NPC 거리 대화 종료 commit: `6b42c26` (`Fix: NPC 거리 이탈 시 대화 종료`)
- 마지막 경제·인벤토리 기반 commit: `dcb76e6` (`Feature: 경제와 인벤토리 최소 기반 추가`)
- 마지막 인벤토리 기반 상점 판매 commit: `f6dbcca` (`Feature: 인벤토리 기반 상점 판매 목록`)
- 마지막 몬스터 Material 아이콘 commit: `82d60fe` (`Feature: 몬스터 Material 아이콘 연결`)
- 마지막 몬스터 보상 commit: `298f023` (`Feature: 몬스터 탈렌트와 전리품 보상 추가`)
- 마지막 퀘스트 시스템 기반 commit: `80227e6` (`Feature: 데이터 기반 퀘스트 시스템 기반 추가`)
- 마지막 탈렌트 표시 참조 commit: `0277317` (`Feature: 탈렌트 화폐 표시 참조 추가`)
- 마지막 오류 수정 commit: `486ff5a` (`Fix: 월드 경험치 바 채움 비율 수정`)
- 마지막 관련 문서 commit: `ace782e` (`Docs: 사수 전용 야수 동료 설계 확정`)
- 마지막 전투 UI 관련 commit: `7de1918` (`Refactor: 전투 스킬 설명 UI 정리`)
- 마지막 태온 전투 애니메이션 commit: `4854591` (`Feature: 태온 공식 전투 애니메이션 적용`)
- 마지막 미엘 전투 애니메이션 commit: `34b7e06` (`Feature: 미엘 공식 전투 애니메이션 적용`)
- 마지막 Main 03 구현 commit: `372d1fe` (`Feature: Main 03 낯선 동행 구현`)
- 마지막 Main 04 구현 commit: `d0dd615` (`Feature: Main 04 세 사람 구현`)
- 마지막 Main 05 구현 commit: `b47c2da` (`Feature: Main 05 돌아온 세 사람 구현`)
- 마지막 몬스터 후보 에셋 commit: `849bc12` (`Chore: 독 몬스터 후보 에셋 보존`)
- 마지막 Pilot Bee 검증 오류 수정 commit: `7a07f2a` (`Fix: Pilot Bee 검증 Scene 입력과 Camera 수정`)
- 마지막 음성 제작 도구 commit: `272b473` (`Chore: MeloTTS 오프라인 제작 도구 추가`)

이 문서는 완료된 기능과 미구현 범위를 빠르게 파악하기 위한 상태 요약이다. 세부 설계는 각 시스템 문서를 따른다.

## 최근 시작 마을 1차 허브

- 일반 주민 stable ID 01~04에 OGA-07/11/13/19 정면 Sprite를 연결했다. 기존과 같은 32×32, Point, PPU 28, Mipmap Off, 발 Pivot, 정확한 마젠타 Alpha 0 파생 규칙을 사용하며 원본 PNG는 변경하지 않았다.
- 이름표 정책은 `VillageNpcRoleType`으로 공통 처리한다. 일반 주민 Label은 비활성, 기능 NPC와 주민 대표는 역할명 한 줄이다. `PlaceholderVisual`은 복제된 기존 Label을 재사용하고 카탈로그도 중복 Label을 정리해 `주민`/`마을 주민` 동시 표시를 막는다. Interaction Prompt와 주민 대화는 독립 유지한다.
- 역할 외형 적용 경로에서 전용 Sprite 유무와 관계없이 주민·상인·안내인 전원의 기존 `Label` TextMesh를 공통 정규화한다. 기존 폰트 48/character size 0.12/Y 0.65를 폰트 40/기본 0.075/Y 1.02로 줄이고 올렸으며, 7자 이상 이름은 0.065로 축소한다. 중앙 한 줄 정렬과 검은 그림자를 사용하고 `[E] 대화하기`는 Y 1.48로 올려 이름표와 분리했다. `PlayerNameplate`와 World EXP HUD는 변경하지 않았다.
- `World_StarterVillage`의 기존 19×13 환경·Player·Camera·남문 `Field_01` 연결을 보존하고, `Chapter01_StarterVillageHub` 아래에 중앙 광장, 시장, 치유소/여관, 은행·파티 관리 안전 서비스, 훈련 구역, 주민 생활 구역, 남문 역할을 구분했다.
- `VillageNpcRole`이 표시 이름과 분리된 stable NPC ID·역할 데이터를 제공한다. 메인 안내, 잡화점, 장비점, 은행, 파티 관리, 훈련, 주민, 남문 경비는 후속 시스템의 진입점/중립 대사만 제공하며 퀘스트·거래·보관·편성 내용을 확정하지 않았다. Taeon/Miel은 배치하지 않았다.
- 치유사는 시간 제한 없는 `치료한다`/`괜찮습니다` 확인 UI를 제공한다. 확인하면 `PartyResourceService.HealPartyFully()`로 등록된 파티 HP/MP를 무료 완전 회복하고 `GameSaveService.SaveCurrentSession()`으로 즉시 저장한 뒤 중립 완료 메시지를 표시한다.
- Unity 컴파일 오류 0개를 확인했다. Unity MCP Play Mode에서 역할 12개가 모두 2m 대상 탐색으로 선택되고 NPC Collider 겹침 0건, 7개 Zone 런타임 설치, 19×13 Bounds·남문 Trigger·복귀 Spawn을 확인했다. 치유는 검증값 HP/MP 35/12→100/50, 슬롯 저장 100/50, Continue 복원 100/50이었고 마을→`Field_01`→마을 왕복 뒤에도 100/50을 유지했다. 최종 Console은 Error 0개·Warning 0개다. 실제 화면에서 NPC 간 시각 간격과 1280×720 선택지 가독성은 사용자가 확인하면 좋다. 상세: `문서/20_월드/시작_마을.md`.
- 이름표·주민 외형 변경은 Unity 6000.5.7f1 전체 응답 파일의 `Assembly-CSharp`·`Assembly-CSharp-Editor` 별도 출력과 Unity Editor ScriptAssemblies 컴파일에서 오류 0개를 확인했다. 신규 경고는 없고 기존 CS0618 경고는 Runtime 5개·Editor 1개이며 관련 파일 `git diff --check`를 통과했다.
- Unity MCP Play Mode에서 주민 1~4 Sprite OGA-07/11/13/19, 주황 Placeholder·마젠타 배경 0개, 주민 Label 비활성 4/4, 기능 NPC·주민 대표 단일 활성 Label 8/8, 주민 대사 호출 4/4, BoxCollider2D `(0.50, 0.34)`와 Circle Trigger, `Field_01` 왕복 재설치를 확인했다. 전체 화면에서 일반 주민 텍스트가 제거되어 혼잡도가 줄고 PlayerNameplate·EXP HUD는 기존 상태를 유지했다. 스크린샷은 `Unity/Client/ValidationCaptures/`에 보존한다.
- 기능 관련 Console Error는 0개다. 저장 슬롯 없이 World Scene을 직접 실행한 검증 경로에서 기존 `선택된 저장 슬롯이 없어 자동 저장을 건너뜁니다.` 경고가 3회 발생했으며 NPC 외형·이름표 신규 경고는 없다.
- World EXP HUD는 탐험 중 표시하고 대화·확인 UI가 열리면 소유자별 억제 상태로 숨긴다. HUD CanvasGroup은 alpha 0·interactable false·blocksRaycasts false이며 모든 상호작용 UI 종료 후 alpha 1로 복귀한다. DialoguePresenter는 Hide, OnDisable, OnDestroy에서 상태를 해제한다.
- 대화 Canvas sortingOrder 10을 World HUD Canvas 5보다 높게 두고 Panel을 열 때 마지막 sibling으로 올렸다. Unity MCP Play Mode에서 일반 주민·잡화 상인·주민 대표, 3회 반복, `StarterVillage→Field_01→StarterVillage` 왕복 후 재대화를 검증했고 모두 숨김/복귀가 정상이다.
- 전체 Assembly-CSharp·Assembly-CSharp-Editor 정적 컴파일 오류 0개, 기존 CS0618 경고 Runtime 5개·Editor 1개다. Play Mode 기능 관련 Error 0개이며 직접 World Scene 실행으로 인한 기존 자동 저장 건너뜀 Warning 1개만 확인했다. 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`.

## 최근 전투 간 파티 HP/MP 지속

- `PartyResourceService`가 플레이어와 동료의 CurrentHP/CurrentMP를 `BattleParticipantSetup.Id` 기준으로 관리한다. Battle 생성은 저장값을 HP 1~MaxHP, MP 0~MaxMP로 Clamp해 적용하고, 값이 없는 신규 캐릭터·구버전 Save는 현재 최대치로 시작한다.
- 일반 승리와 도망은 실제 종료 자원을 유지한다. 승리 시 전투불능 캐릭터는 HP 1로 복귀하며, 실제 레벨업한 플레이어만 연속 상승 후 최종 Level의 새 MaxHP/MaxMP까지 완전 회복한다. 패배는 파티 전체를 완전 회복한다. 휴식 경험치는 없다.
- Version 1 Save에 `PartyResources(CharacterId, CurrentHp, CurrentMp)` 선택 배열을 추가했다. 옛 JSON의 누락 배열은 호환 기본값으로 처리하며 게임 재실행은 무료 회복 수단이 아니다. 전투 상태이상·도발·방어·쿨타임·기세·Path Runtime은 저장하지 않는다. 향후 치유소는 `HealPartyFully()`를 재사용할 수 있지만 NPC와 아이템은 아직 구현하지 않았다.
- 현재 프로젝트에는 요청에 언급된 별도 최근 거점/체크포인트 패배 복귀 시스템이 없고 조우 Field 복귀가 구현되어 있어, 기존 복귀 흐름은 변경하지 않고 자원 완전 회복만 연결했다.
- 신규 소스를 포함하도록 현재 Bee 응답 파일을 보완한 전체 정적 컴파일에서 Assembly-CSharp 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개를 확인했고 관련 `git diff --check`를 통과했다. 실제 승리·도망·전투불능 승리·레벨업·패배·저장/Continue 시나리오는 Play Mode에서 직접 확인해야 한다. 마지막 기능 commit: `9e36ec0`.

## 최근 World EXP HUD PathSymbol 표시

- 화면 하단 `WorldExperienceHud`의 `Lv.n 이름` 왼쪽에 현재 세션 PathId의 공식 PathSymbol을 32×32px, 원본 비율·색상 유지로 표시한다. 전투 기능용 TraitIcon과 Path 이름은 추가하지 않았고 머리 위 PlayerNameplate도 변경하지 않았다.
- `PathPresentationResolver`가 저장에서 복원된 안정적인 PathId로 `PlayerPathDefinition.PathSymbol`을 찾는다. 이름·레벨·EXP와 함께 PathId 변경만 감시하며 Scene 전환, 이어하기와 레벨업 시 새 세션 상태로 갱신된다. 잘못된 PathId나 누락 Sprite는 Image만 숨겨 기존 HUD를 유지한다.
- 5개 PathDefinition의 PathSymbol fileID가 모두 유효하고 각 GUID가 실제 Sprite `.meta` 하나로 해석됨을 확인했다. 전체 Assembly-CSharp 컴파일 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개이며 관련 `git diff --check`를 통과했다.
- 실제 Play Mode에서 1280×720 크기·텍스트 겹침, EXP 획득·레벨업, StarterVillage/Field_01/Field_02 전환, 저장 후 Continue, Battle 진입 시 HUD 정리와 Console 오류를 직접 확인해야 한다. 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`. 마지막 기능 commit: `6b52c74`.

## 최근 FinalConfirmation UI 폴리싱

- 중앙 패널을 큰 PathSymbol(64px)·길 이름, 작은 TraitIcon(26px)·특성 이름·설명, 직업 역할·보너스·패시브·시작 스킬 순서로 분리했다. 길 표현은 `PathPresentationResolver`와 `PlayerPathDefinition.PassiveDescription`을 재사용하며 Path별 문자열 분기는 없다.
- 오른쪽 최종 능력치는 이름과 숫자를 별도 고정 열로 정렬하고 `기본 10 + 직업` 정책을 유지했다. 길 능력치 보너스와 기존 계산·저장·Scene 전환은 변경하지 않았다. `게임 시작`은 이전보다 큰 Primary Action으로 구분하되 기존 키보드 Focus·Enter·Space 입력을 유지했다.
- 5개 PathDefinition의 PathSymbol·TraitIcon GUID가 각각 실제 Sprite `.meta` 하나로 해석되고 JobDefinition 5개가 존재함을 확인했다. 전체 Assembly-CSharp 컴파일 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개이며 관련 `git diff --check`를 통과했다.
- 1280×720 기준 RectTransform 영역상 중앙 패널·능력치·버튼은 겹치지 않는다. 실제 Play Mode에서 5개 길과 5개 직업의 텍스트 렌더링, 긴 특성 설명, 캐릭터 Sprite, 이전·게임 시작과 Console 오류를 직접 확인해야 한다. 상세: `문서/11_UI/최종_확인.md`. 마지막 기능 commit: `88965c6`.

## 최근 MeloTTS 오프라인 음성 제작 시험

- 2026-09-09: OpeningIntro Scene과 런타임 생성 Camera에 AudioListener가 0개여서 AudioSource가 재생되어도 최종 출력이 무음이던 문제를 수정했다. Main Camera 생성 시 AudioListener를 함께 만들고 기존 활성 Listener가 있으면 재사용한다. AudioSource는 enabled, volume 1, mute false, 2D, Mixer 미연결을 명시하며 Listener volume 1·pause false로 시작한다.
- 기존 Editor 로그에서 `There are no audio listeners in the scene` 경고가 반복된 직접 증거를 확인했다. 첫 Clip은 외부 원본과 Unity 파일의 SHA256·크기가 같고 44.1kHz·모노·16비트 실제 신호다. 두 Assembly 컴파일 오류 0개이며 실제 소리, isPlaying, Enter/P/Esc는 수정 후 Play Mode에서 직접 확인해야 한다. 마지막 수정 commit: `a8dca02`.
- 사용자가 선택한 speed 1.00 기준으로 현재 OpeningIntro 19개 Slide 중 실제 내레이션 문장 18개를 생성했다. `opening_001.wav`~`opening_018.wav`는 44.1kHz·모노·16비트이며 외부 검증 후 `Assets/_Project/Audio/Voice/Opening`에 복사했다. 마지막 `LIMITLESS` 제목 장면은 무음이다.
- `VoiceClipCatalog`의 직접 AudioClip 참조로 문장 ID와 WAV를 1:1 연결하고 `VoicePlaybackSource`가 재생·일시정지·재개·정지를 맡는다. 자동 진행은 `max(기존 시간, Clip 길이 + 0.5초)`이고 수동 진행·Esc·건너뛰기는 즉시 정지한다. 자막은 항상 유지하며 다시 보기에서도 음성을 재생한다.
- Catalog 18/18 참조와 WAV 신호·형식을 확인했다. Unity Editor가 Assembly-CSharp와 Assembly-CSharp-Editor를 오류 0개로 컴파일했고 OpeningIntro 진입과 CharacterCreation 전환 로그에 누락 참조 경고나 런타임 예외가 없었다. 실제 청취 품질, 전 문장 순서, Enter/P/Esc, 다시 보기와 자동 Skip은 Play Mode에서 직접 확인해야 한다. 마지막 관련 commit: `47d8594`.
- 공식 MeloTTS `main` commit `209145371cff8fc3bd60d7be902ea69cbdb7965a`, 패키지 0.1.2를 Unity 밖 `F:/study/tts/MeloTTS`에 두고 전용 Conda `melotts` Python 3.9.25 CPU 환경에서 한국어 `KR` 모델을 로드했다. 기존 `py3_12` Python 3.12.0에는 설치하지 않았다.
- `태초에, 신은 세상을 창조했다.`를 speed 0.85/0.95/1.00으로 각각 44.1kHz·모노·16비트 WAV로 생성했다. 길이는 4.111/3.687/3.603초이며 RMS가 모두 0보다 커 실제 신호가 있다. 시험 WAV와 모델·캐시는 저장소 및 Unity Assets에 넣지 않았다.
- `Tools/TTS/melotts_batch.py`가 UTF-8 CSV/JSON의 `id,text,speaker,speed`를 읽어 모델 한 번 로드 후 `id.wav`를 만든다. Colab Notebook은 시스템 Python 대신 `uv`의 격리 Python 3.9를 사용하고 Drive mount는 선택 셀로 둔다.
- Windows에서는 `g2pkk`의 `eunjeon` 빌드가 MSVC 부재로 실패해 전용 KR 환경에 `python-mecab-ko` wheel과 최소 import shim을 사용했다. Notebook JSON·Python 구문·CSV/JSON 입력은 검증했으나 실제 Colab 런타임 실행은 별도 확인이 필요하다. 상세: `문서/00_프로젝트/MeloTTS_오프라인_음성_제작.md`. 마지막 관련 commit: `272b473`.

## 최근 정식 시작 이야기 구현

- 2026-09-09: 고정 1000×220 크기와 알파 0.86의 `CaptionShade`를 문장 실제 크기+좌우 42px·상하 20px로 자동 조절하도록 바꿨다. 화면 위 기준 약 69% 지점으로 내리고 알파 0.68의 방사형 배경을 사용해 중앙 Glow와 PathSymbol을 가리는 큰 사각 패널 인상을 줄였다. 렌더 순서는 배경→Glow→CaptionShade→자막→조작 UI다.
- 방사형 Glow Sprite·색·알파·확대와 기존 서사·입력·설정·Scene 흐름은 유지했다. Assembly-CSharp 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개이며 관련 diff 검사를 통과했다. 실제 Play Mode에서 문장별 배경 크기, Glow 원형, PathSymbol·하단 UI 간격과 자막 가독성을 확인해야 한다.
- 2026-09-09: 스프라이트가 없는 단색 uGUI Image가 RectTransform 전체를 칠해 `AbstractLight`가 노란 사각형으로 보이던 문제를 수정했다. 런타임 방사형 알파 Sprite를 바깥 빛과 중심 빛 두 겹에 적용해 가장자리 알파가 0이 되도록 했으며, 광원을 `SlideContent` 아래로 옮겨 자막과 같은 Fade를 적용한다. 은은한 알파 변화와 확대만 사용하며 자막 패널 뒤에 배치한다.
- 기존 19개 Slide 데이터, PathSymbol, `LIMITLESS`, 자동 진행, 클릭·Enter·Space, P, Esc, 체크박스 저장, 새 게임·이어하기·다시 보기 정책은 변경하지 않았다. Assembly-CSharp 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개로 별도 컴파일했다. 실제 Play Mode의 광원 외형과 입력은 직접 확인해야 한다.
- 플레이어 표시명을 `LIMITLESS`로 확정하고 Bootstrap·CharacterCreation·JobSelection·FinalConfirmation의 기존 제목을 변경했다. 저장소·namespace·Editor 메뉴의 개발명 `Project Limitless`는 유지한다.
- 빈 슬롯 새 캐릭터 흐름에 `OpeningIntro`를 추가했다. 확정 서사 19개 Slide는 `OpeningIntroSequence` 한 곳에서 문장·시간·연출을 관리하며 Fade, 느린 확대, 추상 빛과 공식 PathSymbol 5개를 사용한다. 클릭·Enter·Space 진행, P 일시정지, Esc·버튼 건너뛰기를 지원하고 중복 종료를 막는다.
- 슬롯과 분리된 `UserSettingsService`가 `SkipOpeningIntro`를 별도 JSON에 원자적으로 저장한다. 파일 누락·손상·버전 불일치는 false, I/O 실패는 경고 후 계속 진행한다. Esc는 설정을 바꾸지 않는다.
- Bootstrap에 `시작 이야기 다시 보기`를 추가했다. 설정을 무시하고 재생하며 완료·건너뛰기 뒤 Bootstrap으로 복귀한다. Editor 메뉴 `Project Limitless/Test/Play Opening Intro`도 다시 보기로 안전하게 진입한다.
- OpeningIntro를 Build Settings와 저장 금지 Scene에 등록했다. 기존 Continue, 선택 슬롯, 캐릭터 생성 이후 흐름과 Path·전투·성장 수치는 변경하지 않았다.
- 전체 Assembly-CSharp 정적 컴파일 오류 0개·기존 CS0618 경고 4개, Assembly-CSharp-Editor 오류 0개·기존 CS0618 경고 1개. 실제 Play Mode의 자동/수동 진행, 설정 재실행 유지, 새 슬롯·다시 보기 복귀와 화면 배치는 직접 확인해야 한다.
- 상세: `문서/11_UI/시작_이야기.md`. 마지막 관련 commit: `cec069f`.

## 최근 메인 스토리와 1장 도입 설계

- 하나의 공통 메인 스토리를 중심으로 선택한 길에 따라 장면·대사·개인 에피소드가 달라지는 구조를 확정했다. 길별 메인 캠페인 다섯 벌은 만들지 않으며, 길에 따른 정보·진행·경제 보상의 우열도 두지 않는다.
- 플레이어는 구체적인 신의 명령이 아니라 설명하기 어려운 `부름`을 따라 시작 마을에 도착한다. 부름의 이유와 신의 의도는 장기 미스터리로 남긴다.
- 1장 초반 Main 01~05를 시작 마을 적응 → 초원의 이상 조사와 첫 전투 → 태온과의 동행 및 미엘 조우 → 첫 3인 파티 전투 → 마을 보고와 정식 합류 흐름으로 설계했다. 몬스터가 무엇에게 쫓기는지는 확정하지 않았다.
- 태온은 수호자+지적의 길의 `관찰 → 이해 → 보호`, 미엘은 치유사+마음의 상처의 `회복탄력`을 이야기에서도 드러내되 장애·상처를 성격이나 초능력으로 단순화하지 않는다. 다음 작업은 실제 퀘스트 시스템 구현 전 세부 스토리·대사·퀘스트 데이터 설계다.
- 상세: `문서/03_스토리/메인_스토리와_퀘스트_설계.md`.

## 최근 길 공식 표시와 동료 길 적용

- 2026-09-09: `PlayerPathDefinition` 표현 자료를 자체 제작 공식 `PathSymbol`과 Game-icons.net 전투 `TraitIcon`으로 분리했다. 새 청각·시각·지체 심볼과 기존 마음의 상처·지적 심볼로 5개 공식 문장을 완성했으며, 캐릭터 Variant 외형을 공식 문장으로 사용하지 않는다.
- PathSelection 카드와 상세는 큰 PathSymbol·길 이름·작은 TraitIcon·특성 이름·추천 순서로 표시한다. 전투 상세는 두 아이콘과 두 이름을 분리하고, HUD의 `path.*` 상태는 TraitIcon과 한글 상태명을 함께 표시한다. 플레이어와 태온(지적/패턴 익히기), 미엘(마음의 상처/회복탄력)은 같은 Resolver를 사용한다.
- SaveData는 기존 PathId만 유지한다. `traitStatusMarkerIds`로 런타임 상태와 TraitIcon을 연결하며 Sprite 누락 시 Image만 숨기고 텍스트는 유지한다. Path 전투 수치와 NPC Runtime은 변경하지 않았다.
- 새 자체 제작 원본 3개는 `Assets/_Project/Art/Characters/PathVisuals/Symbols/`, 기존 2개는 원래 위치를 유지한다. Game-icons.net White/Black 10개와 라이선스도 원본 그대로 유지한다.
- 위치: `Assets/ThirdParty/GameIconsNet/Path/{White,Black}/`, 라이선스: `Assets/ThirdParty/GameIconsNet/license.txt`. 저작자·정확한 압축 내부 경로는 `외부에셋.md` 참조. 원본 PNG 10개 및 두 압축 라이선스의 SHA256 일치, 5개 Sprite GUID 연결을 확인했다. Sprite/Single·Bilinear·무압축·Alpha·비율 유지와 기존 Null 방어를 확인했다.
- 새 PNG 3개가 다운로드 원본과 SHA256 일치하며 5개 PathSymbol·5개 TraitIcon GUID가 각각 한 Sprite `.meta`로 해석됨을 확인했다. 전체 Assembly-CSharp 정적 컴파일 오류 0개, 기존 CS0618 경고 4개이며 관련 diff 검사를 통과했다. 실제 Unity Play Mode의 다섯 카드, 상세, HUD는 수동 확인한다. 마지막 기능 commit: `00f4810`.
- 다음 확인: Bootstrap 새 캐릭터 → PathSelection의 서로 다른 공식 심볼 5개와 작은 TraitIcon → 임의 직업 → Battle 플레이어 상세 → 태온(수호자/지적/Companion Emblem/Mesh Network) → 미엘(치유사/마음의 상처/Heart/Heart Shield) → 런타임 상태 TraitIcon과 Console 오류.

- `PathPresentationResolver`가 PathId로 공식 PathSymbol·길 이름·TraitIcon·특성·추천·설명을 같은 `PlayerPathDefinition`에서 제공한다. PathSelection과 전투 상세가 같은 Resolver를 사용한다.
- PathSelection을 상단 안내, 가로 5개 카드, 하단 상세 패널과 `이 길을 선택` 버튼 구조로 개편했다. 아이콘·이름·특성·한 줄 추천을 함께 표시하고 선택 시 배경, 굵은 테두리, 1.03배 확대, `✓ 선택됨`을 함께 사용한다.
- 공식 PathSymbol은 자체 제작 5종이며 Heart Shield, Sound Waves, Eye Target, Cog, Mesh Network는 White 기본의 TraitIcon이다. 누락 시 텍스트를 유지하는 방어도 유지한다.
- `BattleParticipantSetup.PathId`를 추가해 플레이어는 저장 PathId, 태온은 지적의 길, 미엘은 마음의 상처를 데이터로 전달한다. 이름 문자열 비교와 추천 강제는 없다.
- `PathCombatTraitRuntime`은 PathId가 있는 여러 실제 Combatant를 한 전투에서 독립 관리한다. 태온의 패턴 익히기와 미엘의 회복탄력·행동 2회 소비가 플레이어와 동일한 피해·치유 경로에 실제 연결된다.
- 전투 상세 팝업은 PathSymbol·길 이름과 TraitIcon·특성 이름을 분리하고, 집중·잔향·패턴 등 임시 상태는 TraitIcon을 쓰는 기존 상태 영역에 따로 표시한다. Field 이름표 `Lv.n 이름`은 변경하지 않았다.
- 전체 Assembly-CSharp 응답 파일 별도 컴파일 오류 0개, 기존 deprecated API 경고 4개. 관련 파일 `git diff --check` 통과. 실제 Game View의 5카드 배치와 태온·미엘 패시브 발동, 저장 이어하기는 수동 확인이 남았다.
- 상세: `문서/02_세계관/Path_시스템.md`, `문서/11_UI/길_선택.md`, `문서/10_전투/전투시스템.md`. 마지막 기능 commit: `a77c17d`.

## 최근 길 전투 특성 구현

- `SelectedPlayerPathId`를 전투 시작 때 한 번 읽는 `PathCombatTraitRuntime`을 추가했다. 플레이어에게만 적용하고 태온·미엘 및 추천 외 직업에는 별도 제한이 없어 5×5 조합이 모두 같은 경로를 사용한다. 전투 종료 뒤 상태는 저장하지 않는다.
- 마음의 상처 `회복탄력`은 아군 HP가 처음 50% 이하로 실제 내려갈 때 전투당 1회 발동해 다음 플레이어 성공 행동 2회의 직접 피해·치유를 10% 높인다. 청각 `잔향 포착`은 공격을 마친 실제 적별 잔향과 소비·다음 행동 시작 만료를 처리한다.
- 시각 `집중`은 같은 실제 적 단일 공격을 최대 3중첩으로 추적해 현재 공격에 0/3/6/9%를 적용한다. 광역은 기존 집중 대상만 보정하고 중첩을 만들지 않는다. 지체 `굳건한 자리`는 전열 직접 피해 -5%, 후열 직접 피해·치유 +5%다.
- 지적 `패턴 익히기`는 실제 적·아군 참조 쌍을 추적해 반복 직접 공격 뒤 다음 해당 피해를 10% 줄이고 소비한다. 패턴 보유 아군에게 플레이어가 주는 직접 치유는 대상별 10% 증가하며 패턴을 소비하지 않는다.
- 길 배율은 기존 방어·철벽·가이아·수호의 맹세 앞에서 한 번만 적용한다. 화상·독 DoT에는 길 배율을 적용하지 않고 회복탄력의 실제 HP 경계 통과만 관찰한다. HUD와 상세 팝업은 회복탄력·잔향·집중·굳건한 자리·패턴 및 대상 정보를 텍스트로 표시한다.
- 길 데이터의 직접 능력치 보너스를 비우고 캐릭터 생성 계산과 화면을 `기본 10 + 직업`으로 통일했다. 추천 직업 설명은 실제 전투 시너지를 안내하지만 선택을 잠그지 않는다.
- 전체 `Assembly-CSharp` 응답 파일 별도 컴파일 오류 0개, 기존 deprecated API 경고 4개. 관련 파일 `git diff --check` 통과. 실제 Play Mode에서 다섯 길의 수치·HUD·새 전투 초기화와 기존 스킬/DoT/Save 회귀를 직접 확인해야 한다.
- 상세 규칙: `문서/02_세계관/Path_시스템.md`, `문서/10_전투/전투시스템.md`. 마지막 기능 commit: `c86e931`.

## 최근 월드 레벨·EXP UI 구현

- EXP Fill에 Source Sprite가 없어 uGUI가 `Image.Type.Filled`와 `fillAmount` 대신 전체 사각형을 그리던 문제를 수정했다. 1×1 흰색 런타임 Sprite를 연결하고 왼쪽 시작 Horizontal Filled 방식 하나로 실제 길이를 갱신한다.
- 기존 Screen Space Overlay `PlayerNameplate`에 현재 슬롯의 Level을 연결해 머리 위에 `Lv.n 이름`을 흰색으로 표시한다. 위치·피벗·Camera 추적은 유지하고 몬스터 난도 색은 적용하지 않는다.
- 같은 Overlay Canvas 하단 중앙에 `Lv.n 이름`, `EXP 현재 / 필요`, 금색 진행 Bar를 표시하는 얇은 World HUD를 추가했다. 필요 EXP와 만렙은 기존 중앙 성장 API를 사용하며 Lv50은 `MAX LEVEL`과 Bar 100%로 표시한다.
- 이름·Level·CurrentExperience 값이 실제로 달라졌을 때만 갱신한다. Scene별 Player/Canvas 생명주기와 이름 기반 재사용으로 World 전환 중 중복을 막으며 Battle에는 생성하지 않는다.
- 격리 Unity 6000.5.7f1 빈 Scene Play Mode에서 0/24/50/75/99%, Lv3 85/170=50%, 레벨업 후 Lv2 20/130≈15.4%, Lv50=100%의 fillAmount와 실제 렌더 메시 폭 검사를 통과했다. 실제 Game View의 배치와 저장 슬롯 이어하기·SceneTransition·정상 전투 복귀는 수동 확인이 남았다.
- 상세: `문서/11_UI/월드_레벨과_EXP_HUD.md`. 마지막 기능 commit: `abd0175`, Fill 수정 commit: `486ff5a`.

## 최근 초반 성장과 공용 독 구현

- MaxLevel 50, 다음 레벨 요구 EXP `100 + 25*(L-1) + 5*(L-1)^2`(Lv1~49). CurrentExperience는 현재 레벨 진행량이며 초과분 이월·다중 레벨 업·Lv50 EXP 0을 처리한다.
- 슬라임/독침벌/숲거미/맹독뱀의 Level은 1/2/3/4, Base EXP는 8/12/16/20, HP는 60/70/80/95, 기본 공격은 10/12/14/16이다. Field_01 권장 Lv1~3, Field_02 Lv3~5.
- 몬스터와 플레이어의 레벨 차이로 이름색과 EXP 배율을 공용 판정한다. 회색/녹색/노랑/주황/빨강은 각각 0/50/100/125/150%이며 몬스터 이름에 Lv를 표시한다. 흰색은 플레이어/NPC용으로 유지한다.
- Field_02 기존 거미 4개를 유지하고 맹독뱀 2개를 추가했다. 원본 CC0 이동 4프레임을 프로젝트 전용 복사본에서 재사용하고 전투에서는 오른쪽을 향한다. 뱀 조우는 거미 2+뱀 1, 처치 후 30초 리스폰이다.
- 공용 PoisonDefinition으로 일반 독 최대 HP 5%×3회·맹독 7%×3회를 처리한다. 강한 독은 교체, 약한 독은 거절, 동급은 지속시간만 갱신하며 중첩하지 않는다. 정화·방어 무시 Tick·개체별 부여 대기시간을 유지한다. 향후 투사 독칼/사수 독화살도 같은 구조를 사용할 예정이다.
- 승리 시 실제 처치 개체별 EXP를 합산하고 결과 패널에 EXP/레벨 업/진행량을 표시한다. 현재 슬롯에 Level/EXP와 마지막 월드 위치를 즉시 저장하며 다른 슬롯과 전투 중간 상태는 저장하지 않는다.
- 정적 Assembly-CSharp 컴파일 오류 0개, 기존 CS0618 경고 4개. 격리 Unity 프로젝트에서 성장/배율/독/5개 슬롯/15개 버튼 감사와 실제 Field→Battle→승리→Field→31초 리스폰 자동 Play 검증 통과. Editor Search 패키지 시작 예외 1건은 별도로 관찰됐으며 게임 감사 실패는 없었다.
- 실제 사용자 Editor의 정상 전투 입력·시각적 애니메이션·전 직업 스킬/VFX/Timeline/Targeting 회귀는 수동 확인이 남았다. 자동 전투 검증은 적을 테스트 코드로 처치했다. 레벨 업 HP/MP 완전 회복은 미확정이며 기존 새 전투 시작 시 최대 HP/MP 생성 규칙을 변경하지 않았다.
- 상세 규칙·수정 파일·검증 범위: `문서/08_몬스터/초반_성장과_공용_독.md`. 마지막 관련 commit: `5c5a34c`.

## 최근 스킬 선택 버튼 아이콘 수정

- 파이어 볼 Warm Explosion index 4·썬더볼트 Electric Impact index 1·정화 Spectral Bloom index 5를 버튼 전용 고정 Sprite로 생성하고 null/파괴된 캐시는 다시 읽는다. 상세·상태·VFX 로더와 전투 계산은 변경하지 않았다.
- 기존 캐시는 파괴된 Sprite도 그대로 반환해 버튼 자식 Image가 생성되지 않았고, Rebuild로 복구되지 않았다. 수정 전 파괴 캐시 재현 실패를 확인했다. 실제 사용자 세션에서 최초 무효화를 일으킨 이벤트는 미확정이다.
- 추가로 Play 진입 때 동료의 습격 Wolf 버튼의 무효 캐시 재사용을 확인·복구했다. Wolf Run index 4와 나머지 정상 매핑은 유지한다. 현재 코드/문서 기준 도발은 pawn_left, 치유의 빛은 suit_hearts다.
- 15개 스킬의 실제 버튼 생성 감사: Edit Mode 및 빈 Scene Play Mode 2회, 각각 3회 재생성/부모 닫기·열기에서 Sprite 할당·enabled·activeSelf·alpha·캐시 재사용 통과. 세 Grid 버튼의 파괴/null 캐시 복구 후 Image 재할당도 통과했다.
- 전체 Assembly-CSharp 정적 컴파일 오류 0개, 기존 deprecated API 경고 4개. Unity 배치 Editor 컴파일 및 관련 staged diff 검사 통과. 상세 매핑/검증 범위는 `문서/10_전투/전투_UI_아이콘_에셋.md` 참조.
- 실제 Battle의 마도사 → 치유사 → 수호자/사수/투사 각 3개 버튼을 눈으로 확인하고, `열기 → Esc → 다시 열기` 반복·다음 턴·다음 전투 유지 여부를 수동 확인해야 한다. 자동 감사는 전투 진행을 실행하지 않았다.
- 마지막 관련 commit: `c870aaa`.

## 구현 확인된 항목

### 캐릭터 생성과 선택

- `Level + JobId` 기반 결정적 6능력치 성장과 Lv50 중앙 상한 구현. 딜러 주 스탯 평균 +1.5 정수 패턴, 체력 HP, 직업별 HP 성장, 수호자/치유사 의지 파생 수치를 계산하며 Path는 성장 계산에서 제외. 수호자·치유사의 미확정 6능력치 레벨 성장은 적용하지 않음
- 파생 능력치는 저장하지 않고 각 슬롯의 Level/JobId로 복원. 경험치 곡선·몬스터 EXP·현재 캐릭터 승리 보상을 구현했으며 향후 파티 EXP 분배 정책은 미확정

- Bootstrap의 5개 캐릭터 슬롯 목록과 슬롯별 `이어하기 / 새 캐릭터` 런타임 UI. 새 캐릭터는 기존 4단계 생성 흐름 유지
- `GameSaveData` Version 1 JSON을 Unity Editor 프로젝트 로컬 `UserData/Saves/save_slot_01.json`~`05.json`에 독립 저장
- 선택 슬롯만 자동 저장하며 손상 슬롯은 다른 슬롯에 영향을 주지 않고 사용 불가로 표시
- 기존 `Application.persistentDataPath/project_limitless_save.json`은 슬롯 1이 비었을 때만 검증·복사하고 원본 유지
- FinalConfirmation 확정, 정상 마을/Field 전환 완료, Battle 종료 뒤 Field 복구 시 자동 저장하며 Battle 중간 상태는 제외
- 손상 JSON·Version 불일치·잘못된 Visual/Path/Job/Scene을 삭제·덮어쓰기 없이 거부하고 새 게임으로 안전 복귀
- Editor 전용 `Project Limitless/Test/Manage Local Saves` 창의 슬롯별/전체 삭제 기능
- WorldBounds가 있는 마을/Field에서 5초 주기·Pause/Quit·Scene 도착·Battle 복귀 시 실제 float 좌표를 선택 슬롯에 저장
- 이어하기는 같은 Scene의 Bounds 안 유효 좌표를 우선하고 좌표 없음·NaN/Infinity·Bounds 밖이면 기존 SpawnPoint로 fallback
- SceneTransition 시작 시 이전 Scene 좌표를 무효화하고 목적지 Spawn 배치 뒤 새 좌표 저장
- Bootstrap 저장 슬롯의 `삭제` 버튼, 캐릭터 정보·복구 불가 경고 확인창, 해당 슬롯만 삭제 후 즉시 빈 슬롯 갱신

- `Bootstrap → CharacterCreation → PathSelection → JobSelection → FinalConfirmation → World_StarterVillage` Scene 흐름
- 이름 입력과 검증
- Male/Female 기본 외형 선택과 세션 유지
- 5개 길 선택 및 길별 전투 패시브 프리뷰, 길 직접 능력치 보너스 없음
- 길별 추천 직업 안내와 모든 5×5 조합 선택 허용
- 5개 직업 선택
- 직업 능력치, 패시브, 시작 스킬 3개 프리뷰
- FinalConfirmation의 선택 결과와 최종 능력치 표시

### 플레이어 외형과 월드 표시

- Player Nameplate
- Path Visual 데이터와 UI Preview
- 청각·시각·지체 길의 Male/Female Character Variant
- 지체 길의 전투형 전동 휠체어 Variant
- 지적 길의 동행의 문장 Symbol
- 마음의 상처 길의 이어진 심장석 Symbol
- 기본/Variant 공통 Player 이동과 방향 Animation 구조

### 마을과 필드

- `World_StarterVillage`의 Player 이동, Camera 추적, NPC E 대화
- `Field_01` Scene과 야외 환경
- 마을 남문 → Field_01 북쪽 입구 이동
- Field_01 북쪽 입구 → 마을 남문 복귀
- `SceneTransitionService`, `SceneTransitionTrigger`, `SceneSpawnPoint`
- 공통 `WorldBounds2D`와 `WorldBoundaryGeneratorUtility`
- Orthographic viewport 크기를 고려한 Camera Bounds
- Exit Opening을 제외한 월드 외곽 Collider
- 마을 외곽 울타리의 Tile 영역 밖 돌출 방지

### 첫 필드 몬스터

- 모든 Field 출입구에 데이터 기반 원형 몬스터 안전지대 적용. 생성점과 활동 반경 전체를 보정하며 2초 재조우 유예와 별도로 유지
- Field_01 북/남 안전지대와 Field_01↔Field_02 데이터 기반 양방향 연결
- Field_02 숲길 기본 Scene·그늘 테마·World/Camera Bounds 재사용, 숲거미 4개 스폰
- CC0 Spider Idle/Walk/Attack/Shoot 선별 프레임 연결, `독액 분사` 생존 아군 전체 직접 피해 20 구현
- Field_02 거미 조우를 전열 독침벌 2·후열 숲거미 1로 변경하고 숲거미 기본 공격 14, 독액 분사 20을 분리 유지
- 신규 Field/같은 Field 신규 몬스터가 직전 최강 일반 몬스터와 함께 등장하고 약 10~15%를 출발점으로 Play Mode에 맞춰 데이터 조정하는 공통 성장 규칙 문서화
- 같은 MonsterDefinition을 공유하는 복수 몬스터도 실제 Combatant 참조별로 감전·독·화상·독침 대기시간을 독립 저장하며, 썬더볼트 피해 처리 뒤 살아남은 EnemyAll 각 대상에 감전 1을 별도 단계로 적용

- 데이터 기반 `MonsterDefinition`과 Scene별 `FieldMonsterSpawnDefinition`
- Field_01 여러 빈터의 `초원 슬라임` 5마리 데이터 기반 런타임 배치
- 스폰별 시작 위치와 활동 반경 안의 독립적인 느린 무작위 배회
- Rigidbody2D 기반 장애물 Collider 충돌
- 플레이어 접촉 시 이동 정지와 `MonsterEncounterService.EncounterStarted` Event 발생
- 사용자 제공 4×4 Sprite 시트 기반 초원 슬라임 방향별 Idle/Walk Animation
- 배회 방향·이동/대기 상태와 `GrassSlime.controller` 동기화
- 초원 슬라임 데이터에 실제 Sprite/Animator 연결 및 녹색 Placeholder 미사용
- 승리한 스폰만 제거하고 데이터 기본값 30초 후 원래 위치에 독립 리스폰, 도망 시 스폰 유지
- `MonsterDefinition.DisplayName`을 표시하는 재사용 가능한 필드 몬스터 Overlay 이름표
- 레벨 차이별 이름색·Lv 표기·검은 외곽선의 이동 추적 이름표, 필드 HP Bar 미포함
- 독 몬스터 원본 보존: Pilot Bee(CC BY, 라이선스 버전 표기 충돌 기록)·2D Spider(CC0)·Simple Green Snake(CC0). 벌은 Field_01, 거미와 맹독뱀은 Field_02에 구현
- Pilot Bee 검증 Scene: Idle 238×215×10·Attack 315×253×10·기본 우향 구조를 런타임 분할하고 현재 초원 슬라임과 나란히 비교. Point Filter·무압축·투명·Read/Write 검증 복사본과 기본 Scale 0.85 제공, Field/Battle 미연결
- Pilot Bee 검증 Scene 입력을 새 Input System의 null 안전 `Keyboard.current` 방식으로 수정하고 메인 키보드·Numpad +/-를 지원. Scene 전용 직교 Main Camera를 연결해 `No cameras rendering` 표시 제거
- Field_01에 데이터 기반 독침벌 3개 스폰(`venom_bee_01`~`03`)을 추가해 기존 초원 슬라임 5개와 총 8개 배치. 공용 설치기·배회·접촉 조우·개별 30초 리스폰·도망 유지·2초 재조우 유예 재사용
- `02_VenomBee` MonsterDefinition이 Pilot Bee Idle/Attack 시트 구조와 Scale 0.85를 Field/Battle에 공통 제공. 런타임 분할 재생으로 원본 PNG를 수정하지 않으며 공용 일반 독을 부여

### 1차 턴제 전투

- 치유사 MP 숫자를 상단 HP HUD에서 제거하고 `UsesMp` 참가자의 Hover/Focus 상세 팝업 HP 다음 줄로 이동. 다른 직업은 빈 MP 줄을 표시하지 않음
- 모든 스킬 Tooltip·재사용 중 UI 용어를 `재사용 대기시간`으로 통일하고 공통 Tooltip 생성 경계에서 `구현: 사용 가능/미구현` 개발 상태 문구를 제거. MP 사용 스킬은 고정 비용을 함께 표시
- 화살비·회오리 베기·썬더볼트 광역 Target과 실행기에서 실제 Combatant 참조 중복을 제거해 대상당 피해 1회를 보장. 빈 슬롯·VFX 수는 피해/기세 횟수에서 제외하며 독침벌 A/B는 별개 인스턴스로 유지. 숲거미 독액 분사도 같은 참조 안전장치 적용
- 적대 광역 스킬은 스킬 메뉴를 닫기 전에 유효 Target을 검사한다. 화살비 후열 0명·회오리 베기 전열 0명·썬더볼트 전체 0명이면 범위별 경고 후 스킬 메뉴·취소·키보드 포커스를 즉시 복구하며 행동·턴·자원·쿨타임·VFX를 시작하지 않음
- 치유사 전용 MP 런타임 자원과 `MP 현재/최대` HUD 구현. MaxMP는 `임시 기본값 + (Level-1)×1 + 지능×2`로 재계산하며 레벨 성장 +1과 지능 계수 +2는 확정. 자기 행동 종료 회복 및 치유의 빛/회복의 파동/정화 선검사·성공 차감 적용
- 치유사 스킬은 고정 비용으로 `정화 < 치유의 빛 < 회복의 파동`을 유지한다. 치유의 빛은 쿨타임 없음, 회복의 파동 3턴·정화 2턴은 유지하며 기본 MP·회복량·실제 비용 숫자는 임시값, 레벨업 시 MP 완전 회복 여부는 미확정
- 행동 우선도→민첩→전투 시작 1회 Tie Break 순서 구현. 실제 Combatant 참조별 값을 전투 동안 유지하고, 민첩 동률 전투에서만 Overlay를 열어 전투 RNG와 분리된 지역 `System.Random`으로 1~6 눈을 0.09초 간격으로 1.2초간 변경. 최종 눈도 직전 값과 다르게 별도 추첨하며 입력 잠금과 실제 첫 행동/Timeline 판정 유지
- 회복의 파동·정화·철벽·수호의 맹세·가이아 웰 VFX는 캐릭터 본체와 독립된 Image만 사용한다. Image는 생성 즉시 숨기고 첫 유효 Sprite·Tint 설정 뒤 표시하며, null 프레임은 숨기고 종료 즉시 비활성화·파괴한다. CharacterSprite 뒤 강제 배치를 제거해 원본 후광의 안쪽이 가려져 바깥 색 레이어처럼 분리되는 현상을 방지했고, 수호의 맹세 이전 섬광의 의도적 null Sprite도 유효 Orb Sprite로 교체

- 재사용 가능한 `Battle` Scene과 Build Settings 연결
- `Combatant`, 전열/후열 2×3 `Formation`, `TargetResolver`, `TurnOrderQueue` 분리
- 근거리 전열·직선 후열 보호, 원거리 후열 우선, 마법 자유 대상 판정
- 플레이어와 몬스터 공통 대상 판정 및 단일 행동 도발 강제 대상 구조
- 민첩과 행동 우선도 기반 순서 및 다음 행동 타임라인
- 플레이어 이름·현재 외형, 초원 슬라임 Sprite와 양측 HP 표시
- 마우스·키보드 지원 남색/금색 전투 UI와 공격 가능·불가능 텍스트 구분
- 기본 공격, 다음 행동까지 피해 50% 감소 방어, 일반전 도망
- HP 0 전투불능, 적 전멸 승리, 승리·도망·패배 후 `Field_01` 복귀
- 복귀 시 전투 상태 초기화, 접촉 위치 이격과 2초 조우 유예
- 스킬 버튼과 NPC 직접 지시 확장용 `IsPlayerControlled` 구조
- 기본 공격 사거리: 수호자·투사 근거리, 사수 원거리, 마도사·치유사 마법. 치유사는 낮은 피해의 마법 공격
- 논리 2×3 Formation을 숨기고 적 왼쪽·아군 오른쪽의 사이드뷰 전장 좌표로 Sprite 배치
- 타임라인 아래·전장 위의 상단 HP 전용 HUD와 적군 3열×최대 2줄·아군 3열×1줄 이름/비율 HP 항목
- 캐릭터 근처 이름·HP·상태 텍스트 제거, Sprite 가로 중앙 위 대상 화살표와 행동 중 표시만 유지
- 상단 HP 항목의 행동 중·방어·도발·구현 스킬 재사용 턴 한 줄 요약과 상세 팝업 역할 분리
- 전투불능 시 HP HUD·상세 팝업의 행동 상태/쿨타임 제거와 회색 `전투불능` 단일 표시
- Hover/마우스 선택/키보드 포커스 공용 단일 상세 상태 팝업과 포커스 캐릭터·고정 HP 행 동시 강조
- 평상시 회색 발판 제거, 대상 선택 중 공격 가능 대상만 얇은 금색 선과 선택 화살표로 표현
- 상단 한글 `전투` 제목과 현재 행동자 강조·이후 순서 타임라인, 하단 전용 명령 패널
- 조작 안내를 13px 글씨로 명령 패널 내부 하단에 배치하고 버튼과 하단 padding을 확보
- 약 17% 축소한 공용 명령 버튼·축소 명령 패널과 대상/스킬 선택 중 Esc와 같은 흐름을 실행하는 마우스 `취소` 버튼
- Kenney Game Icons·Board Game Icons CC0 실제 Sprite와 한글을 함께 사용하는 공격·스킬·방어·도망·취소 버튼 및 HP HUD 상태 배지
- 역할 ID와 Resources 경로를 한곳에서 연결하는 `BattleUiIconCatalog`, Sprite 누락 시 문자 기호 없이 한글만 남기는 fallback
- HP HUD 도발은 `pawn_right`, 3턴 재사용은 표시값 3=`hourglass_top`·2=`hourglass`·1=`hourglass_bottom`으로 구분
- 직업 스킬 데이터의 `IconId`를 통해 수호자 도발=`pawn_left`, 치유의 빛=`suit_hearts`, 정조준=`target` Sprite를 스킬 이름 왼쪽에 표시
- 스킬 버튼은 아이콘·이름 중심으로 단순화하고, Hover와 키보드 포커스가 공유하는 팝업에서 설명·대상·효과·재사용·구현 여부를 표시
- 도발·치유의 빛·정조준의 현재 구현 규칙과 일치하는 설명 데이터를 `BattleSkillDefinition`에 연결하고 취소 아이콘을 `arrowLeft`로 교체
- 외부 신규 에셋 없이 남색·금색 하늘·원경·지면 층의 임시 전투 배경 구성
- 플레이어·NPC·몬스터가 공통 사용 가능한 `BattleActionPresenter`와 전투 계산 분리
- 근거리 기본 공격의 짧은 전진·타격 대기·원위치 복귀, 피격 좌우 흔들림·점멸, 떠오르는 피해 숫자
- 연출 중 명령·대상 선택·취소 입력 잠금과 연출 완료 후 다음 턴 진행
- 사수 기본 공격의 짧은 조준, 재사용 가능한 UI Projectile 이동, 도착 시 피해·피격 연출과 다음 턴 연결
- 마도사·치유사 기본 공격의 짧은 캐스팅과 Projectile, Fireball·밝은 금빛 구체 시각 구분
- 필드 Animator 현재 상태와 분리된 전투 Sprite 해석기, 아군 Left Idle·적 Right Idle 진입 및 공격 후 복구
- 전투 적 구성을 전열 초원 슬라임 A/B와 후열 독침벌 1로 변경. 독침벌은 원본 우향 Idle을 반복하고 기본 공격 중 Attack 10프레임으로 전환한 뒤 Idle 복귀, 피해 계산은 기존 적 기본 공격 유지
- 독침벌 기본 공격은 MonsterDefinition의 120% 데이터로 슬라임 10 대비 12 피해를 매 행동 적용. 독 부여 가능 공격만 독 3 부여·갱신 후 공격자별 독침 대기 2를 시작하고, 이후 해당 벌의 행동 종료에만 `2→1→0` 감소하여 네 번째 행동부터 다시 부여
- 대상의 독은 자신의 행동 종료마다 최대 HP 5% 올림·최소 1 피해로 `3→2→1→제거`되고 재적중은 독 3 갱신이다. 대상 독 지속시간과 벌의 독침 대기시간은 별도 저장되어 정화·자연 종료가 공격자 대기시간을 초기화하지 않음
- 독 HUD·상세 팝업에 PVFX Venom Ward index 6 아이콘과 `독 n`, `행동 종료 시 최대 HP 5% 피해` 표시. 틱은 작은 Acid Splash index 3~8과 피해 숫자만 사용하며 화상과 함께 있으면 두 틱 후 다음 턴 진행
- CC0 Polar_34 - Projectiles 원본 GIF 보존, 32×32 PNG Sprite 프레임 변환 및 사수 golden arrow·마도사 fireball 기본 공격 적용
- 시작·목표 X 좌표 비교 기반 공용 Projectile 좌우 반전과 GIF 프레임 지연 재생
- PVFX Foundry 0.3.0 CC0 원본·라이선스 보존, Magical Projectile travel 5프레임을 치유사 기본 공격에 적용
- 치유사 치유의 빛: 자신 포함 살아 있는 단일 아군, 최대 HP 35% 올림 회복, 최대 HP·전투불능 안전 처리
- 치유사 회복의 파동: Formation의 살아 있는 아군 전체(자신 포함)를 치유의 빛 기본 회복량의 60%로 동시 회복, 전투불능 제외·최대 HP 상한·회복 대상 없음 행동 미소비·치유사 행동 기준 3턴 쿨타임
- 회복의 파동 연출: 치유사 중심 Arcane Parry 1회와 대상별 작은 Radiant Heal 병렬 재생, Radiant Heal peak에서 전체 HP·회복 수치·HUD 동시 갱신, peak 고정 프레임 버튼/상세 아이콘 분리
- 치유사 정화: 살아 있는 아군 1명의 독·화상·감전을 `HarmfulStatusType` 공통 분류로 모두 제거, 상태 없음 행동 미소비, 치유사 행동 기준 2턴 쿨타임
- 정화 연출: PVFX spectral-bloom 96×96 전체 16프레임을 대상 위치에서 20 FPS로 재생하고 release index 7에서 상태·HUD 갱신, peak index 5 고정 버튼/상세 아이콘 분리
- PVFX Radiant Heal 96×96 14프레임을 대상 위치에서 재생하고 peak 7프레임에 회복·`+회복량`·HUD 갱신
- 치유 대상 선택 중 Esc/취소는 스킬 메뉴로, 스킬 메뉴 취소는 기본 명령으로 돌아가는 단계별 입력 흐름
- 사수 정조준: 기존 원거리 TargetResolver 후열 우선, 기본 공격력 160% 정수 올림, 사수 행동 기준 2턴 쿨타임
- `정조준!` 강조와 기존 golden_arrow 0.42초 이동, 도착 순간 피해·HP HUD·피격 연출 적용
- 사수 `화살비`: `EnemyRearRowAll`의 살아 있는 적 후열 전체에 일반 공격 120% 피해, 후열 0명에서는 전열 전환 없이 행동·쿨타임 미소비, 사수 행동 기준 2턴 쿨타임
- 화살비 `bow` 버튼 아이콘과 기존 golden_arrow 기반 조준→3발 상승→공중 대기→대상별 3발 낙하→공유 타격·동시 피격의 약 0.75초 연출
- 다중 Projectile은 순수 연출로 관리하고 실제 피해는 마지막 낙하 시점에 후열 대상마다 한 번만 적용한 뒤 전체 피격 완료 후 다음 턴 진행
- 화살비 다중 후열 실검증용 프로토타입 배치: 초원 슬라임 A는 전열 0열, B·C는 후열 0·1열에 배치하여 전열 1명+후열 2명 유지
- 정조준과 화살비 쿨타임은 같은 `BattleSkillCooldowns`에서 참가자·Skill ID별로 독립 관리하며, 화살비는 모든 피격 반응 완료 후 성공 확정 시 2턴 등록
- 사수 `동료의 습격`: 전후열 자유 단일 적, 도발 강제 대상 우선, 일반 공격 180%, 사수 행동 기준 3턴 쿨타임
- 별도 Combatant가 아닌 기본 Wolf가 매 사용 시 현재 사수 `ActionRoot` 위치·크기로 계산한 근처 지점에서 출발한다. 384×40 시트를 64×40 Sprite 6개로 나눈 12 FPS Run Coroutine과 570 UI 단위/초 Transform 이동 Coroutine이 독립적으로 동시에 실행되며, 대상 바로 앞 도착 시 Run을 멈추고 타격 후 0.12초 뒤 제거된다. `BeastCompanionDefinition` 경계로 Bear/Fox 교체 가능
- 모든 스킬 설명 팝업은 메뉴 Hover/키보드 포커스 중에만 표시하고, 스킬 확정 즉시 공통 경계에서 닫아 대상 선택·연출·행동 종료 뒤 기본 명령 화면까지 숨김 유지
- 모든 Battle Action의 `actionPlaying` 중 스킬 설명과 캐릭터/상태 상세 팝업을 함께 억제하고 기존 Hover·포커스 대상을 비워, 마우스가 HUD 위에 남아 있어도 공격·회복·Projectile·VFX를 가리지 않음
- 동료의 습격 버튼은 작은 버튼에서 실루엣이 선명한 Wolf Run 다섯 번째 프레임(index 4) 기반 실제 동물 아이콘을 사용하고 정조준은 기존 Kenney `target.png` 유지. Wolf 경로와 아이콘 프레임 번호는 `BeastCompanionDefinition`에서 제공
- 마도사 `파이어 볼`: 전후열 자유 단일 마법·도발 우선, 즉발 170%, 명중 당시 Attack의 30% 화상 2회, 마도사 행동 기준 3턴 쿨타임
- 화상은 대상 행동 종료 시 작은 fireball 불꽃과 함께 저장 피해를 한 번 적용하며, 재적중 시 중첩 없이 새 명중 피해·2회로 갱신. HUD와 상세 팝업에 Warm Explosion peak 아이콘+`화상 n` 표시
- 파이어 볼 연출은 Solar Shrapnel 초기 Charge 2프레임→기본 공격보다 큰 기존 fireball→Warm Explosion 15프레임이며 index 4에서 즉발 피해·화상 적용
- 마도사 `썬더볼트`: `EnemyAll`로 살아 있는 적 전열·후열 전체에 90% 피해, 대상별 감전 1, 마도사 행동 기준 3턴 쿨타임 구현. 광역이라 단일 도발 강제 대상은 적용하지 않음
- 감전은 다음 행동의 주는 피해를 15% 감소시키고 공격하지 않는 행동도 종료 시 제거되며, 재적중은 중첩 없이 감전 1 갱신. 전투불능 시 저장소와 HUD에서 제거
- PVFX electric-impact 96×96 14프레임을 청백색 예고 뒤 적 전체에서 거의 동시에 재생하고 peak index 1에서 피해·감전을 함께 적용. 스킬 아이콘은 peak 고정 Sprite, 상태 아이콘은 Kenney `power.png`
- 마도사 `가이아 웰`: 자기 자신에게 받는 피해 60% 감소를 부여하고 현재 행동을 제외한 자신의 다음 2회 행동 종료에서만 감소, 마도사 행동 기준 4턴 쿨타임 유지
- 가이아 웰 활성 중 공용 방어 입력은 지정 안내 후 행동·턴 미소비로 차단하고 버튼을 사용 불가 색상으로 표시. 계산에서도 방어 단계를 건너뛰어 두 효과가 절대 중첩되지 않음
- PVFX arcane-parry 96×96 16프레임을 자신 위치에서 20 FPS 재생하고 peak index 8에서 상태 적용. 실제 `dice_shield.png`로 `가이아 2/1` HUD·상세 상태 표시
- 수호자 `철벽`: 자신에게 받는 피해 70% 감소를 부여하고 사용 행동을 제외한 자신의 다음 2회 행동 종료에서만 `철벽 2→1→제거`, 수호자 행동 기준 4턴 쿨타임 유지
- 철벽은 도발과 동시에 유지하지만 공용 방어 50%와는 입력·피해 계산 양쪽에서 중첩을 차단한다. 활성 중 방어 시 지정 안내 후 행동·턴을 소비하지 않고, 원시 피해 10은 기존 올림 규칙으로 3이 됨
- PVFX `earth-rupture` 96×96 20프레임을 발밑에서 20 FPS로 한 번 재생하고 peak index 9에서 상태를 적용한다. Kenney `structure_wall.png`를 스킬 버튼과 `철벽 2/1` HUD·상세 상태에 함께 사용
- 수호자 `수호의 맹세`: 수호자의 다음 행동 시작 전까지 자신을 제외한 같은 진영 생존 아군 전체의 직접 공격·공격 스킬 피해를 최대 50% 감소시키고 감소량 절반을 수호자에게 이전. 고정 3인 목록 없이 진영·생존 상태를 피격 순간 판정
- 수호의 맹세 이전 예산은 사용 시 수호자 최대 HP 40%를 올림 계산하고 철벽 적용 전 이전 예정량으로 소비. 예산 부족 시 실제 이전량의 두 배까지만 감소하며 0 또는 수호자 전투불능에서 즉시 종료. 발동 시 대상별 `수호의 맹세 -감소량` 한 줄과 금색 이전선·섬광 표시
- 이전받은 실제 피해는 철벽 70%·공용 방어 등 수호자의 기존 개인 방어를 정상 적용하며, 화상·독 같은 DoT는 `BattleDamageOrigin` 분류로 보호에서 제외. 도발·철벽과 동시 유지하고 수호자 행동 기준 4턴 쿨타임
- PVFX `frost-nova` index 4~10을 금백색·낮은 알파의 짧은 파티 범위 VFX로 사용하고, 실제 이전 때 피격 아군→수호자 금색 선·섬광 표시. Kenney `pawns.png`를 버튼·수호자 HUD에 사용하고 상세 팝업에 남은/최대 이전 예산 표시
- 투사 `난도`: 기존 근거리 TargetResolver로 적 1명을 선택하고 전진 타격 순간 일반 공격 150% 피해와 자신 기세 +1 적용
- 기존 근거리 기본 공격 Presenter를 재사용하는 전진→타격·피해 숫자·피격 반응→원위치 복귀→다음 턴 흐름
- `난도`는 스킬명, `기세`는 Combatant별 0~3 개인 자원이며 난도 직접 세 번째 사용 기준 2턴 쿨타임과 독립 관리
- 투사 `회심의 일격`: 근거리 단일 공격, 기세 0/1/2/3에 일반 공격 100/130/160/190%, 적중 후 기세 전부 소비, 자체 쿨타임 없음
- `GetMomentum`·`AddMomentum`·`ConsumeAllMomentum` 구조로 회오리 베기 명중당 획득 확장 준비와 회심 소비 연결
- 난도 버튼 `cross`, 기세 HUD·회심의 일격 버튼 `skull`, 상단 `기세 n`·상세 `기세 0/3~3/3`, 전투불능 시 숨김
- 회심의 일격 설명 팝업에 데이터 기반 현재 기세와 현재 예상 피해 배율 표시
- 투사 `회오리 베기`: 살아 있는 적 전열 전체에만 일반 공격 80% 피해, 실제 적중한 전열 적 1명당 기세 +1(최대 3), 자체 쿨타임 없음. 전열이 비면 행동 미소비로 사용 불가
- 스킬 전용 광역 범위 `EnemyFrontRowAll`·`EnemyRearRowAll`·`EnemyAll`과 공용 해석기 구조. 회오리 베기는 전열, 화살비는 후열, 향후 썬더볼트는 적 전체 범위로 확정
- 회오리 베기는 `spinner` 버튼 아이콘과 투사 중심 약 0.3초 코드 기반 회전 참격을 사용하며, 동시 피해 숫자·피격 반응·HUD 갱신 후 다음 턴 진행
- 회오리 베기의 기세 획득은 공용 `AddMomentum`만 사용하고 난도 직접 사용 기록을 변경하지 않아 난도 쿨타임과 독립
- JobDefinition 프리뷰를 사용하는 재사용 가능한 전투 스킬 카탈로그·실행기·참가자별 쿨타임·상태효과 런타임
- 실제 스킬 메뉴와 Esc 복귀, 미구현 스킬 비활성 표시, 수호자 도발 제자리 강조 연출
- 수호자 도발의 적 전체 적용, 적별 다음 2회 행동 소모, 수호자 행동 기준 3턴 쿨타임과 적 HUD 상태 표시
- 참가자 목록 기반 `BattleEncounterSetup`과 3대3 프로토타입 Factory, 기존 2×3 Formation을 사용하는 실제 N대N 전투 생성
- 플레이어·태온(수호자)·미엘(치유사)의 플레이어 직접 조작과 독립 HP 미니 HUD
- 전열 슬라임 2명·후열 슬라임 1명의 독립 Combatant·턴·HP·도발 상태와 6명 전체 행동 타임라인
- 적·아군 후보를 공통 처리하는 대상 선택 UI 기반과 태온·미엘 코드 생성 임시 Visual
- 현재 HP·직업/몬스터 분류·방어·도발·구현 스킬 쿨타임을 계산 코드 변경 없이 조합하는 `BattleCombatantStatusViewModel`
- 방어·도발 표식을 안정적 ID와 수치로 분리해 향후 무료 아이콘 Asset으로 교체 가능한 표시 구조

## 데이터만 있고 실행 로직이 없는 항목

- 길 능력치와 고유 패시브의 전투 효과
- 직업 능력치, 패시브, 시작 스킬 프리뷰
- AP, 상태이상, 협동 기술, 보스 패턴의 초안 방향

이 항목들은 캐릭터 생성 UI에서 표시되지만 실제 전투 계산이나 효과로 실행되지 않는다.

## 문서화된 전투 설계

- 아군·적 공통 전열 3칸과 후열 3칸의 2×3 진형
- 근거리·원거리·마법 사거리와 바로 앞 전열의 후열 보호
- 단일 적대 행동에 일반 사거리보다 우선하는 도발 강제 대상
- 민첩·행동 우선도 기반 솔로 순서와 시간 제한 없는 직접 행동 지시
- 공용 공격·스킬·방어·도망 명령 및 승리·도망 필드 복귀
- 멀티플레이는 동시 입력, 기본 45초 제한, 남은 시간 10초 경고, 시간초과 시 방어로 설계되어 있으나 네트워크는 미구현
- AP, 상태이상, 행동·협동 기술, 보스 패턴은 미확정이며 구현하지 않음

## 미구현

- 투사 시작 스킬 3종을 제외한 나머지 직업별 스킬 효과와 길 패시브
- `BeastCompanion` 선택·장착·저장 UI와 Wolf/Bear/Fox 패시브 수치·능력치 계산
- NPC 동료 정식 CompanionDefinition·파티 편성·최종 Sprite
- 여러 몬스터 배치 전투와 보스전 실제 콘텐츠
- AP와 상태이상, 행동·협동 기술, 보스 패턴
- 멀티플레이 네트워크 전투
- 인벤토리·아이템·아이템 보상·파티 EXP 분배 정책
- 퀘스트와 영구 저장·불러오기
- 인스턴스 던전과 Field_02 이후 지역

## 현재 검증 상태

- MP 상세 팝업 이동·Tooltip 공통 문구 정리·광역 참조 중복 제거는 관련 파일 `git diff --check`와 Unity 6000.5.7f1 배치 실행 종료 코드 0을 확인했다. Presenter의 화살비·회오리 베기·썬더볼트·독액 분사 계산 콜백이 연출당 한 번인 구조와 Executor의 대상당 단일 피해를 정적으로 교차 확인했으며, 실제 1/2/3명 피해 숫자와 HUD 배치는 Play Mode 확인이 필요하다.
- 치유사 MaxMP 공식 변경은 관련 파일 `git diff --check`와 Unity 6000.5.7f1 배치 실행 종료 코드 0을 확인했다. 지능 12 기준 Lv1 44·Lv2 45·Lv3 46·Lv50 93, 마도사 0을 정적으로 교차 확인했으며 실제 MP 소비·행동 종료 회복·쿨타임·저장 슬롯별 재계산은 Play Mode 확인이 필요하다.
- 성장 능력치·치유사 MP·민첩 Tie Break 변경은 관련 파일 `git diff --check`를 통과했고 Unity 6000.5.7f1 배치 실행이 종료 코드 0으로 완료됐다. 실제 Play Mode의 Lv별 표시, MP HUD/회복/부족 거절, 동률 유무별 Overlay와 Timeline 일치는 사용자가 직접 확인해야 한다.
- Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조 응답과 Roslyn으로 새 전투 코드를 포함해 컴파일했으며 Compiler Error 0개를 확인했다.
- 치유사 기본 공격을 마법 사거리로 매핑하고 기존 검증용 공격력보다 4 낮게 적용한 뒤 동일한 Unity 참조로 Compiler Error 0개를 재확인했다.
- 임시 실행 테스트로 근거리 보호·후열 개방, 원거리 후열 우선, 마법 자유 대상, 도발 우선·2회 지속, 방어 50%, 행동 우선도·민첩 정렬의 12개 검증을 모두 통과했다.
- `Battle.unity` GUID와 Build Settings GUID 일치, Battle 항목 1개, 새 C# meta 3개와 `git diff --check` 통과를 확인했다.
- 사용자가 Unity Play Mode에서 `Field_01 → 초원 슬라임 → Battle` 진입, 기본 공격, 적 자동 공격, 턴 순환을 정상 검증했다.
- 사용자가 방어 시 받는 피해가 10에서 5로 감소해 50% 방어 규칙이 정상임을 확인했다.
- 사이드뷰 UI 변경 후 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다. 실제 사이드뷰 화면 배치와 입력 회귀는 사용자가 다시 확인해야 한다.
- 조작 안내를 명령 패널 내부로 옮긴 뒤 동일한 Unity 전체 참조로 Compiler Error 0개를 확인했다. 16:9에서의 하단 padding과 버튼 간격은 사용자가 직접 확인해야 한다.
- HP Fill의 실제 Rect 폭 갱신과 스폰 ID별 처치·30초 리스폰 구조를 적용한 뒤 Unity 전체 `Assembly-CSharp` 참조로 Compiler Error 0개를 확인했다.
- 사용자 Play Mode 확인에서 한 마리만 생성되는 문제를 재현했고, Unity Editor 로그에서 `grass_slime_02`~`05`의 meta YAML 마지막 줄바꿈 누락으로 GUID가 무효 처리되어 Asset import가 제외된 원인을 확인했다.
- 새 4개 meta를 정상 형식으로 수정하고, Installer의 실제 로드 개수·고유 ID 로그와 Field01SceneGenerator의 5개 ID·30초·최소 3유닛 간격 검증을 추가했다. Runtime/Editor C# 컴파일 오류 0개를 확인했으며 Play Mode 재검증이 필요하다.
- `BattleCore.cs`가 변경되지 않았으며 공격 10, 방어 50%, Formation·TargetResolver·TurnOrderQueue 계산 규칙을 유지했다. 실제 HP Bar 비율과 필드 리스폰 시간은 Play Mode에서 사용자가 확인해야 한다.
- 이전 사이드뷰 UI 작업에서는 `BattleCore.cs`와 조우/복귀 코드가 변경되지 않았음을 정적으로 확인했다. 이번 작업은 `BattleSceneFlow.cs`의 승리·도망 결과 전달만 확장했으며 전투 계산 규칙은 변경하지 않았다.

- 월드 전환 Scene/Spawn ID, Missing Script, Bounds 참조와 viewport 계산은 정적으로 확인했다.
- 초원 슬라임 Script GUID, Monster/Spawn Asset 연결, Field_01 대상 Scene과 조우 Event 구조를 정적으로 확인했다.
- 초원 슬라임 PNG를 1256×1256, 314×314 Cell의 4×4 구조로 확인하고 Sprite 16개, Animation Clip 8개, Animator와 데이터 참조를 정적으로 교차 확인했다.
- `MonsterNameplate`가 몬스터별 Text를 분리하고 `DisplayName`을 받으며 Camera 이동 뒤 화면 좌표를 갱신하는 구조를 정적으로 확인했다.
- Unity 6.5에서 오류가 된 `GetInstanceID()`를 권장 API인 `GetEntityId()`로 교체했다.
- Unity 6000.5.7f1이 사용하는 Roslyn과 전체 `Assembly-CSharp` 응답 파일로 재컴파일하여 Compiler Error 0개와 종료 코드 0을 확인했다.
- 최신 Unity Editor 로그에서 `MissingReferenceException`과 `NullReferenceException` 기록이 없음을 확인했다. 실제 Play Mode 기능 검증은 사용자가 직접 확인해야 한다.
- 관련 C# 변경은 `git diff --check`를 통과했다.
- 기본 공격 액션 연출 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 기존 API deprecation 경고만 남아 있다.
- `BattleCore.cs`, `TargetResolver`, `Formation`, 피해·방어 계산은 변경하지 않았다. 근거리 연출을 유지하고 사수 원거리 기본 공격만 Projectile 연출에 연결했으며, 마법 기본 공격은 기존 즉시 처리 흐름을 유지했다.
- 사수 Projectile 코드를 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 참조와 Roslyn으로 컴파일해 오류 0개를 확인했다. 조준 시간·Projectile 위치와 방향·입력 잠금은 Play Mode 확인이 필요하다.
- 기본·Path Variant·휠체어·초원 슬라임 Animator Controller에서 `Idle_Left`·`Idle_Right` 상태를 확인하고, `BattleVisualResolver`를 포함한 전체 `Assembly-CSharp` 컴파일 오류 0개를 확인했다. 실제 방향과 첫 프레임은 Play Mode 확인이 필요하다.
- `AnimationClip.SampleAnimation`이 Sprite PPtr 곡선을 적용하지 못하던 경로를 임시 Animator 상태 평가로 교체하고, Path Variant 기본 Sprite fallback과 null UI 투명 처리를 추가했다. 전체 `Assembly-CSharp` 컴파일 오류 0개이며 흰 사각형·경고 제거는 Play Mode 확인이 필요하다.
- 마도사·치유사 기본 마법 Projectile과 런타임 Orb Graphic을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 피해·치유사 감소값·마법 사거리 계산은 변경하지 않았으며 색상·도착 시점·Idle 복구는 Play Mode 확인이 필요하다.
- golden arrow 8프레임(80ms), fireball 7프레임(60ms)을 원본 32×32 크기로 추출하고 복사 전후 SHA-256 일치를 확인했다. 실제 에셋 연결 후 전체 Assembly-CSharp 컴파일 오류 0개이며 표시 크기·방향·프레임 재생은 Play Mode 확인이 필요하다.
- 사수 golden arrow와 마도사 fireball 이동시간을 각각 0.35초로 조정하고, 프레임 간격·도착 후 피해 적용·치유사 Projectile 0.24초·근거리 속도는 유지했다. 전체 Assembly-CSharp 컴파일 오류 0개이며 체감 속도는 Play Mode 확인이 필요하다.
- 치유사 임시 Orb를 PVFX Magical Projectile의 96×96 travel 프레임 5개로 교체했다. manifest 픽셀 해시 일치, Point Filter·투명 Sprite·50ms 프레임·0.24초 이동 유지와 전체 Assembly-CSharp 컴파일 오류 0개를 확인했다.
- 수호자 도발 스킬 구조를 Unity 전체 Assembly-CSharp 참조로 컴파일해 오류 0개를 확인했다. Combatant·TargetResolver 기존 도발 우선 판정을 재사용하고 BattleCore·Formation·TurnOrderQueue는 수정하지 않았다. 메뉴 조작·HUD 배치·2회 소모·3턴 쿨타임은 Play Mode 확인이 필요하다.

- 3대3 Encounter와 공용 대상 선택 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleCore`·기존 스킬 런타임·ActionPresenter·BattleVisualResolver는 변경하지 않았으며 실제 3대3 UI·입력·도발 분산은 Play Mode 확인이 필요하다.
- 전투 상태 UI와 ViewModel을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 전투 계산·N대N·대상 판정·연출 코드는 변경하지 않았으며 16:9 팝업 위치와 마우스/키보드 동작은 Play Mode 확인이 필요하다.
- 고정 HP 목록 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleCore`·Formation·TargetResolver·TurnOrderQueue·도발·피해 계산과 하단 명령 패널은 변경하지 않았으며, 좌우 목록 배치·실제 HP 비율·포커스 행 연동은 Play Mode 확인이 필요하다.
- 상단 HP HUD 재배치를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. HP HUD·전장·상태 Anchor·팝업 안전 영역만 변경했으며 N대N·Formation·Combatant·대상/턴/도발/피해 계산·명령·연출 코드는 변경하지 않았다.
- 대상 화살표 정렬·HP HUD 상태 요약·명령 버튼 축소·공용 취소 흐름을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. `BattleSceneController`의 UI와 입력 단계만 변경했으며 전투 계산·N대N·도발/방어 판정·행동 실행은 변경하지 않았다.
- 전투불능 상태 정리·회색 발판 숨김·단색 버튼/상태 아이콘·죽은 대상 선택 정리를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. ViewModel과 UI 갱신만 변경했으며 Combatant·Formation·TargetResolver·TurnOrderQueue와 피해/도발/방어 계산은 변경하지 않았다.
- Kenney 실제 Sprite 카탈로그·명령 버튼·HP HUD 상태 배지 변경을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 별도 출력 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 유지되며 `BattleCore`·Combatant 계산·Formation·TargetResolver·TurnOrderQueue·Projectile/VFX·취소 흐름은 변경하지 않았다. 실제 import와 화면 정렬은 Play Mode 확인이 필요하다.
- 도발 pawn_right와 재사용 3단계 모래시계 표시를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. `BattleSkillCooldowns`의 시작·감소 계산은 변경하지 않고 ViewModel이 남은 턴과 총 턴을 UI에 전달하며, 전투불능 목록 제거와 텍스트 fallback을 유지한다.
- 치유의 빛 회복 API·아군 대상 선택·Radiant Heal Presenter 확장을 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 원본 grid sheet SHA-256 일치와 96×96 프레임 14개를 확인했으며, `TakeDamage`·방어 50%·TargetResolver·Formation·TurnOrderQueue·도발·기존 Projectile/VFX는 변경하지 않았다. 실제 회복 시점과 화면 위치는 Play Mode 확인이 필요하다.
- 사수 정조준 데이터·원거리 대상 연결·golden_arrow 도착 피해를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 정수 퍼센트식은 Attack 12→20, 15→24, 20→32를 확인했으며 TargetResolver·Formation·BattleCore·Presenter·기본 공격과 기존 Projectile 에셋은 변경하지 않았다. 실제 후열/전열 선택과 2→1→사용 가능 흐름은 Play Mode 확인이 필요하다.
- 투사 난도의 근거리 대상 선택·150% 타격·적중 후 자원 증가·직접 사용 3회 쿨타임·HUD 표시를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 공격력 12→18, 15→23, 20→30의 정수 올림 계산을 확인했고 기존 deprecated API 경고 4개만 있었다. Formation·TurnOrderQueue·BattleCore·Projectile/VFX와 기존 세 직업 스킬은 변경하지 않았으며 이후 사용자 Play Mode 확인을 통과했다.
- 사용자가 Unity Play Mode에서 기존 난도 150% 근거리 공격·기세 획득 전 동작이 정상임을 확인했다.
- 기세 용어·API 리네임과 회심의 일격 데이터·근거리 타격·기세 소비·skull UI를 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 공격력 10 기준 기세 0/1/2/3 피해 10/13/16/19와 원본 ZIP·런타임 skull PNG SHA-256 일치를 확인했고 기존 deprecated API 경고 4개만 있었다. 실제 연출·HUD·쿨타임 독립은 Play Mode 확인이 필요하다.
- 회오리 베기 데이터·광역 실행·동시 피격 Presenter·spinner 아이콘 연결을 Unity 6000.5.7f1 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 있었고 Formation·TargetResolver·TurnOrderQueue·BattleCore·기존 Projectile/VFX는 변경하지 않았다. 실제 1~3명 대상 피해·기세와 연출은 Play Mode 확인이 필요하다.
- 회오리 베기 대상을 `EnemyFrontRowAll` 데이터와 공용 스킬 대상 해석기로 전열에 제한한 뒤 Unity 전체 `Assembly-CSharp` 참조 컴파일 오류 0개를 확인했다. Formation·TargetResolver·기존 VFX는 변경하지 않았으며, 후열 제외·전열 0명 행동 미소비는 Play Mode 확인이 필요하다.
- 화살비 `EnemyRearRowAll` 대상·120% 광역 실행·다중 golden_arrow Presenter·bow 아이콘을 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. Kenney 원본과 bow 복사본 해시 및 golden_arrow 8프레임 유지를 확인했고, 기존 deprecated API 경고 4개만 있었다. 실제 타이밍·후열 0명·대상당 단일 피해는 Play Mode 확인이 필요하다.
- 화살비 다중 후열 실검증용 프로토타입 적 배치를 전열 1명+후열 2명으로 바꾼 뒤 Unity 전체 `Assembly-CSharp` 참조 컴파일 오류 0개를 확인했다. 전투 계산·범위·연출 코드는 변경하지 않았으며 실제 동시 타격과 후열 전투불능 제외는 Play Mode 확인이 필요하다.
- 사용자가 Unity Play Mode에서 화살비가 후열 여러 명을 정상 공격하는 것을 확인했다.
- 화살비 성공 완료 후 2턴 쿨타임 등록과 설명 데이터를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 120%·EnemyRearRowAll·golden_arrow 연출은 변경하지 않았고 기존 deprecated API 경고 4개만 있었다. 2→1→사용 가능과 정조준 독립 표시는 Play Mode 확인이 필요하다.
- **동료의 습격 구현**: `BeastCompanionDefinition`·카탈로그, 자유 단일 대상+도발 우선, 180% `TakeDamage`, 3턴 쿨타임과 Wolf Run+이동+도착 타격 구조를 Unity 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 기존 deprecated API 경고 4개만 있으며 실제 화면 위치·속도·입력 잠금·쿨타임 흐름은 Play Mode 확인이 필요하다.
- **동료의 습격 Wolf 연출 조정**: 선명한 Wolf Run index 4 실물 프레임 아이콘, 사수 근처 출발, 최초 이동 속도의 3/4, 대상 바로 앞 정지·도착 타격·0.12초 여운 뒤 제거를 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 원본 SpriteSheet는 수정하지 않고 야수 정의가 경로·아이콘 프레임을 제공하며, 대상·180% 피해·도발·3턴 쿨타임 코드는 변경하지 않았다. 실제 아이콘 가독성·간격·체감 속도는 Play Mode 확인이 필요하다.
- **동료의 습격 Run 프레임 수정**: 원본 Wolf Run의 64×40 프레임 6개가 모두 서로 다른 이미지임을 확인하고, 전투 `Image.sprite`를 0→1→2→3→4→5→0 순서로 12 FPS 교체하는 Coroutine을 570 UI 단위/초 Transform 이동 Coroutine과 분리했다. 출발·도착점은 매 사용 시 현재 사수·대상 `ActionRoot` 위치와 표시 크기로 계산하며, 아이콘용 index 4 고정 Sprite와 전투용 6프레임 배열은 분리했다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 다리 움직임과 접촉 위치는 Play Mode 재확인이 필요하다.
- **마도사 파이어 볼 구현**: Magic 자유 단일 대상·도발 우선, 170% 즉발 피해, 명중 당시 Attack 30%를 저장한 행동 종료 화상 2회와 비중첩 2회 갱신, 마도사 행동 기준 3턴 쿨타임을 공용 스킬·상태·쿨타임 구조에 연결했다. PVFX Charge·Warm Explosion과 큰 기존 fireball, 작은 화상 틱을 새 시각 정의 경계로 연결했으며 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했다. 실제 크기·타격 프레임·행동 종료 틱은 Play Mode 확인이 필요하다.
- **마도사 썬더볼트 구현**: EnemyAll 생존 적 전체 90% 동시 피해, 대상별 감전 1의 15% 주는 피해 감소·행동 종료 제거·비중첩 갱신, 마도사 행동 기준 3턴 쿨타임을 기존 런타임에 연결했다. electric-impact 14프레임과 peak index 1 동시 타격, VFX/상태 고정 아이콘 분리를 적용했고 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했다. 실제 다중 대상 크기·동시성·상태 수명은 Play Mode 확인이 필요하다.
- **마도사 가이아 웰 방어 규칙 수정**: 자신 전용 받는 피해 감소를 60%로 상향하고 공용 방어 50%와 중첩되지 않게 입력·피해 계산 양쪽에서 차단했다. 활성 중 방어 버튼은 사용 불가 색상으로 보이지만 클릭·키보드 시 지정 안내를 제공하고 행동을 소비하지 않는다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 버튼 상태·피해 10→4는 Play Mode 확인이 필요하다.
- **수호자 철벽 구현**: 참가자별 철벽 상태·70% 받는 피해 감소·자신의 다음 2회 행동 지속·4턴 쿨타임과 공용 방어 중첩 차단을 기존 상태·쿨타임 구조에 연결했다. Earth Rupture 20프레임과 `structure_wall.png` 연결 후 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 실제 크기·피해 10→3·HUD·입력 흐름은 Play Mode 확인이 필요하다.
- **수호자 수호의 맹세 구현**: 직접 행동 피해 분류, 수호자별 최대 HP 40% 이전 예정 예산, 예산 비례 감소, 철벽 적용 후 실제 이전 피해, 다음 수호자 행동 시작·전투불능·예산 소진 종료를 공용 상태 경계에 연결했다. Frost Nova 중간 7프레임·금색 이전선과 실제 `pawns.png`, 대상별 감소량 피드백을 연결했으며 실제 VFX 범위·홀수 피해·예산 소진·복합 방어는 Play Mode 확인이 필요하다.
- **공통 스킬 설명 팝업 흐름 조정**: 스킬 확정 시 정보 UI를 닫는 공통 경계와 대상 선택·행동 완료 안전 숨김을 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. 스킬별 설명 내용·효과·대상·쿨타임은 변경하지 않았으며 실제 Hover/키보드 포커스와 취소 복귀 흐름은 Play Mode 확인이 필요하다.
- **전투 연출 중 정보 팝업 억제**: `actionPlaying` 화면 갱신에서 스킬 설명·캐릭터 상태 상세 팝업과 기존 Hover/포커스 대상을 함께 정리하고 두 표시 함수의 재오픈을 차단하는 코드를 전체 `Assembly-CSharp` 참조로 컴파일해 오류 0개를 확인했다. Wolf 570·Run 12 FPS·출발/정지 위치와 모든 전투 계산은 변경하지 않았으며 실제 마우스 잔류·새 Hover 복귀는 Play Mode 확인이 필요하다.
- **치유사 회복의 파동 구현**: 단일힐 35%에서 파생한 대상별 21% 올림 광역 회복, Formation 생존 아군 동적 목록, 치유사 행동 기준 3턴 쿨타임과 Arcane Parry+Radiant Heal 병렬 연출을 연결했다. 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개)를 확인했으며 대상 수·동시 HP 갱신·팝업 억제·쿨타임은 Play Mode 확인이 필요하다.
- **치유사 정화 구현**: 기존 독·화상 Dictionary와 감전 HashSet은 유지하고 `HarmfulStatusType`·공통 조회/단일 제거/전체 제거 API를 추가했다. 살아 있는 아군 1명의 세 해로운 상태를 모두 제거하며 상태가 없으면 행동·턴·쿨타임을 소비하지 않는다. spectral-bloom 16프레임·release index 7 제거·peak index 5 아이콘과 치유사 행동 기준 2턴 쿨타임을 연결했다. Unity 6000.5.7f1 자동 컴파일 오류 0개를 확인했으며 실제 대상 선택·복합 상태 제거·VFX·팝업 억제·쿨타임은 Play Mode 확인이 필요하다.
- **독침벌 독 부여 대기시간 구현**: `BattleMonsterAbilityRuntime`이 공격자 Combatant별 독침 대기 상태를 독 상태 저장소와 분리해 관리한다. 첫 공격은 120% 피해+독 3 뒤 대기 2, 다음 두 벌 행동은 120% 피해만 주고 행동 종료마다 1·0으로 감소하며 네 번째 행동부터 다시 독을 부여한다. 정화·독 자연 종료·다른 참가자 행동은 대기시간에 영향을 주지 않는다. Unity 6000.5.7f1 자동 컴파일 오류 0개를 확인했으며 4행동 순서와 복수 벌 독립성은 Play Mode 확인이 필요하다.
- **독 몬스터 후보 에셋 보존 및 Pilot Bee 검증 준비**: F:\Downloads 원본과 프로젝트 복사본 SHA-256 일치를 확인하고 벌·거미·뱀을 제작자별 ThirdParty 폴더에 분리했다. Pilot Bee 검증 스크립트를 포함한 전체 `Assembly-CSharp` 참조 컴파일 오류 0개(기존 deprecated API 경고 4개), Point·무압축·투명 Import 설정을 정적으로 확인했다. Unity 프로젝트가 이미 열려 있어 두 번째 배치 인스턴스는 안전하게 중단했으며 실제 화면의 Scale·픽셀 밀도·Idle/Attack 자연스러움은 전용 Scene Play Mode 확인이 필요하다.
- **동료의 습격 Run 검증 환경 유지**: ScratchIO `Animated Wild Animals` CC0 원본 ZIP과 Wolf/Bear/Fox 자산을 보존하고, 실제 Battle과 분리된 `CompanionAssaultRunValidation` Scene을 유지한다. 원본/복사본 SHA-256, 64px 프레임 구조, Point·무압축·투명 Import와 통일된 아래 중앙 Pivot은 정적으로 확인했으며 Unity Play Mode 직접 확인은 남아 있다.
- Working Tree에는 이번 문서 작업과 무관한 사용자 Asset·Scene·ProjectSettings 변경이 남아 있으며 이 상태 문서는 해당 미커밋 변경의 완성 여부를 판단하지 않는다.

## Unity에서 사용자가 직접 확인할 사항

1. Play Mode를 종료해 Asset 재import를 완료한 뒤 다시 시작하고, Console에 `필드 몬스터 배치 로드: Field_01, 5개 [grass_slime_01, ..., grass_slime_05]`가 출력되는지 확인한다.
2. HP가 100%에서 50%가 되었을 때 숫자와 함께 HP Bar 길이도 정확히 절반이 되는지 확인한다.
3. Player와 초원 슬라임 양쪽 HP Bar가 피해 직후 정상 감소하고, HP 0에서 완전히 비는지 확인한다.
4. 슬라임 처치 후 `Field_01` 복귀 시 조우했던 해당 슬라임만 사라져 있는지 확인한다.
5. 처치하지 않은 다른 슬라임들은 그대로 존재하며 계속 배회하는지 확인한다.
6. 약 30초 후 처치한 스폰의 시작 위치에서 슬라임이 다시 나타나는지 확인한다.
7. 도망한 경우 조우했던 슬라임이 사라지지 않고, 기존 2초 재조우 유예 뒤 정상 배회하는지 확인한다.
8. `Field_01`에 총 5마리의 초원 슬라임이 서로 다른 활동 반경 안에서 독립적으로 배회하는지 확인한다.
9. Console에 Compiler Error, MissingReferenceException, NullReferenceException이 없는지 확인한다.
10. 근거리 직업으로 플레이어 기본 공격 시 대상 앞까지 짧게 전진하고, 타격 후 원래 자리로 복귀하는지 확인한다.
11. 초원 슬라임 공격에도 같은 전진·복귀가 적용되고, 양측 피격 시 좌우 흔들림·짧은 점멸과 `-피해량` 숫자가 표시되는지 확인한다.
12. 연출 중 공격·스킬·방어·도망·대상 선택·Esc 취소가 중복 실행되지 않고, 연출 종료 뒤 다음 턴 입력이 정상 복구되는지 확인한다.
13. 공격 10과 방어 중 피해 5, HP 숫자·Bar, 행동 순서 타임라인, 승리·도망·패배 및 Field_01 복귀·리스폰이 기존과 동일한지 확인한다.
14. 사수 직업으로 기본 공격 시 제자리에서 짧게 조준하고 golden arrow 애니메이션이 약 0.35초 동안 왼쪽 적을 향해 이동하는지 확인한다.
15. Projectile 도착 순간에만 HP와 피해 숫자가 갱신되고 기존 흔들림·점멸 뒤 다음 턴으로 정상 진행되는지 확인한다.
16. Projectile 이동과 피격 연출 중 명령·대상 선택·Esc가 중복 실행되지 않는지 확인한다.
17. 필드에서 플레이어가 걷거나 상·하·우 방향을 보는 중 조우해도 Battle에서는 기본·Path Variant·휠체어 모두 Left Idle 첫 프레임인지 확인한다.
18. 초원 슬라임이 필드에서 이동하거나 다른 방향을 보는 중 조우해도 Battle에서는 Right Idle 첫 프레임인지 확인한다.
19. 근거리·Projectile 공격 종료 후 양측이 각자의 Left/Right 전투 Idle Sprite로 복귀하는지 확인한다.
20. 남자 지체의 길 수호자와 초원 슬라임이 흰 사각형 없이 표시되고, `Battle Visual` Warning·NullReference·Compile Error가 없는지 확인한다.
21. 마도사 기본 공격 시 제자리 캐스팅 후 fireball 애니메이션이 약 0.35초 동안 왼쪽의 전열 또는 후열 대상까지 이동하고, 도착 순간에만 HP·피해 숫자가 갱신되는지 확인한다.
22. 치유사 기본 공격 시 PVFX Magical Projectile이 0.24초 동안 왼쪽 대상을 향해 재생되고, 도착 순간 피해와 기존 낮은 피해량·마법 자유 대상 규칙이 유지되는지 확인한다.
23. 두 마법 연출 중 입력이 잠기고 완료 후 Left Idle과 다음 턴이 복구되며, 치유사 Magical Projectile과 근거리 연출도 정상인지 확인한다.
24. 테스트용으로 시작 X보다 목표 X가 큰 배치를 구성할 수 있을 때 golden arrow와 fireball이 원본 오른쪽 방향으로 표시되는지 확인한다.
25. 수호자로 Battle에 진입해 스킬 → 도발을 마우스와 키보드로 선택하고, Esc로 명령 메뉴에 복귀되는지 확인한다.
26. 도발 사용 시 수호자가 제자리에서 강조되고 도발! 텍스트 뒤 슬라임 HUD에 도발 2가 표시되는지 확인한다.
27. 슬라임 첫 행동 완료 후 도발 1, 두 번째 행동 완료 후 표시 제거를 확인한다.
28. 수호자 다음 행동 차례의 메뉴에 도발 [재사용 2턴], 이후 1턴, 종료 후 다시 사용 가능 상태가 표시되는지 확인한다.
29. 쿨타임 중 도발 버튼이 실행되지 않으며 철벽·수호의 맹세와 다른 직업 스킬 버튼의 구현 상태·재사용 표시가 실제 규칙과 일치하는지 확인한다.
30. 공격·방어·도망과 기존 근거리/Projectile 연출, HP Bar, 승리·패배·Field_01 복귀·30초 리스폰을 회귀 확인한다.
31. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

32. Battle 진입 시 플레이어·태온·미엘과 초원 슬라임 A·B·C가 3대3으로 겹치지 않고 표시되며 각 HUD에 이름·직업·HP가 보이는지 확인한다.
33. 타임라인에 여섯 참가자의 민첩 기반 순서가 표시되고 플레이어·태온·미엘 차례마다 시간제한 없이 직접 명령할 수 있는지 확인한다.
34. 전열 슬라임 A·B와 후열 슬라임 C에 근거리·원거리·마법 기본 공격 대상 규칙이 기존대로 적용되는지 확인한다.
35. 슬라임 하나의 HP가 0이 되어도 다른 두 슬라임의 HP·행동이 유지되고, 세 마리 전멸 때만 승리하는지 확인한다.
36. 태온 또는 플레이어 수호자의 도발 후 세 슬라임 HUD가 모두 도발 2가 되고, 각 슬라임 행동 때 자기 표시만 1로 감소한 뒤 두 번째 행동 후 사라지는지 확인한다.
37. 도발 중 세 슬라임의 단일 공격이 시전자 수호자에게 집중되고, 시전자 행동 기준 재사용 2턴→1턴→사용 가능인지 확인한다.
38. 아군 한 명 전투불능 시 그 참가자만 행동에서 제외되고 나머지 전투가 계속되며, 아군 세 명 전멸 때만 패배하는지 확인한다.
39. Console에 Compile Error, NullReferenceException, MissingReferenceException이 없는지 확인한다.

40. 16:9 Battle이 제목 → 타임라인 → 상단 HP HUD → 실제 전장 → 하단 명령 패널 순서로 표시되고 각 영역이 겹치거나 잘리지 않는지 확인한다.
41. 현재 3대3에서는 상단 HUD의 적군 첫 줄과 아군 한 줄에 각각 3개 HP 항목만 표시되고, 캐릭터 주변 이름·HP Bar가 없는지 확인한다.
42. 참가자 수를 줄인 Encounter에서는 빈 HP 항목이 생기지 않고, 적 4~6명 구성에서는 적군이 한 줄 최대 3명·최대 2줄로 배치되는지 확인한다.
43. 피해 직후 해당 HP 항목의 Bar 길이가 현재/최대 HP 실제 비율만큼 줄고, HP 0에서 완전히 비며 이름에 `[전투불능]`이 표시되는지 확인한다.
44. 대상 선택을 방향키로 전환할 때 역삼각형이 적/아군 전열·후열 각 Sprite의 가로 중앙 위로 정확히 이동하는지 확인한다.
45. 캐릭터 주변에는 대상 화살표와 `행동 중`만 보이고, 상단 HP 항목에 실제 Sprite와 함께 `행동 중`, `방어`, `도발 2/1`, 구현 스킬 `재사용 n턴`이 한 줄로 표시되는지 확인한다.
46. 참가자가 전투불능이 되는 즉시 화살표·행동 중·도발·방어·재사용 표시가 사라지고 HP HUD에는 회색 `전투불능`만 남는지 확인한다.
47. 전투불능 참가자 상세 팝업에는 이름·분류·HP 0/최대 HP만 표시되고 방어·도발·쿨타임이 남지 않는지 확인한다.
48. 평상시 모든 캐릭터 아래 회색 발판이 보이지 않고, 대상 선택 중 유효 대상에게만 얇은 금색 선이 표시되는지 확인한다.
49. 공격=sword, 스킬=star, 방어=shield, 도망=exitRight, 취소=cross Sprite와 한글이 함께 보이며 비율·정렬이 깨지거나 버튼 크기가 커지지 않았는지 확인한다.
50. 대상 선택 중 포커스된 참가자가 전투불능이 되면 강조가 제거되고 다음 유효 대상으로 이동하거나 후보가 없을 때 기본 명령으로 복귀하는지 확인한다.
51. 대상/스킬 선택의 취소 버튼·Esc와 공격·방어·도발 수치, Projectile·승패·필드 복귀가 기존과 같고 Console 오류가 없는지 확인한다.
52. 행동 중 arrowRight, 방어 shield와 재사용 단계별 hourglass가 상태와 함께 갱신되는지 확인한다.
53. 전투불능 즉시 모든 상태 Sprite와 글자가 사라지고 회색 `전투불능`만 남으며 Console에 MissingReference·NullReference가 없는지 확인한다.
54. 도발 상태에 pawn_right가 표시되고 기존 target은 나오지 않으며, 도발 2→1→제거가 유지되는지 확인한다.
55. 도발 사용 직후 재사용 3턴=hourglass_top, 다음 자기 차례 2턴=hourglass, 마지막 1턴=hourglass_bottom, 0턴=아이콘·텍스트 제거인지 확인한다.
56. 미엘 또는 플레이어 치유사 차례에 스킬→치유의 빛이 활성화되고 자신·플레이어·태온·미엘 중 살아 있는 아군만 선택되는지 확인한다.
57. 피해를 받은 아군에게 사용하면 최대 HP의 35% 올림 값만큼 회복하되 최대 HP를 넘지 않고, peak 순간 HP Bar·상세 HP와 `+회복량`이 함께 갱신되는지 확인한다.
58. 최대 HP 아군 선택 시 `이미 HP가 가득 찼습니다` 안내 후 스킬 메뉴로 돌아가며 행동이 소비되지 않는지 확인한다.
59. 전투불능 아군과 적은 대상이 아니며, 대상 선택 중 Esc/취소는 스킬 메뉴로, 스킬 메뉴 Esc/취소는 기본 명령으로 돌아가는지 확인한다.
60. Radiant Heal이 대상 위치에서 14프레임으로 재생되고 peak 뒤 연출 종료 시 다음 턴으로 진행하며, 연출 중 중복 입력과 Console 오류가 없는지 확인한다.
61. 플레이어 사수 차례에 스킬→정조준이 활성화되고 `강한 원거리 · 160% · 2턴` 안내가 보이는지 확인한다.
62. 후열 슬라임이 살아 있으면 후열만 선택되고, 후열 전멸 뒤 전열만 선택되며 전투불능 적은 후보에서 빠지는지 확인한다.
63. 정조준 선택 후 즉시 HP가 줄지 않고 `정조준!`→golden_arrow 0.42초 이동→도착 순간에만 피해·HP Bar·상세 HP·피해 숫자·피격 연출이 갱신되는지 확인한다.
64. 기본 공격력 12 기준 정조준 raw 피해가 20이며 방어 중 대상에는 기존 50% 감소가 적용되고 방어 무시·치명타·상태이상이 없는지 확인한다.
65. 사용 직후 HUD에 재사용 2턴/hourglass_top, 다음 사수 행동에 1턴/hourglass_bottom, 그다음 사용 가능 및 표시 제거인지 확인한다.
66. 정조준 대상 선택 중 Esc/취소는 스킬 메뉴, 스킬 메뉴 Esc/취소는 기본 명령으로 돌아가며 연출 중 입력이 잠기는지 확인한다.
67. 수호자·치유사·사수 스킬 메뉴에서 각각 pawn_left·suit_hearts·target 아이콘과 한글 스킬명이 함께 보이고, 적 도발 HUD에는 기존 pawn_right가 유지되는지 확인한다.
68. 스킬 버튼 Hover와 방향키 포커스에서 하나의 상세 팝업이 즉시 갱신되고, 설명·대상·효과 수치·재사용·미구현 여부가 현재 규칙과 일치하는지 확인한다.
69. 스킬 메뉴를 닫거나 대상 선택으로 이동하면 팝업이 사라지고, 16:9에서 HP HUD·하단 명령 패널과 겹치거나 화면 밖으로 잘리지 않는지 확인한다.
70. 취소 버튼에 arrowLeft와 `취소`가 함께 표시되고 cross가 나오지 않으며, 취소 버튼과 Esc의 단계별 복귀가 기존과 같은지 확인한다.
71. 투사 스킬 메뉴에 cross 아이콘+난도, skull 아이콘+회심의 일격이 함께 표시되는지 확인한다.
72. 난도 1회 적중 시 기존 150% 피해와 skull 아이콘+`기세 1`이 표시되는지 확인한다.
73. 기세 1에서 회심의 일격이 130% 피해를 주고 타격 후 기세 0이 되는지 확인한다.
74. 난도 2회 후 기세 2가 되고 회심의 일격이 160% 피해를 준 뒤 기세를 0으로 소비하는지 확인한다.
75. 난도를 직접 세 번째 사용할 때 기세 3과 난도 재사용 2턴이 표시되는지 확인한다.
76. 기세 3 회심의 일격이 190% 피해를 주고 기세만 0으로 만들며 난도의 남은 쿨타임은 유지하는지 확인한다.
77. 기세 0에서도 회심의 일격을 사용할 수 있고 일반 공격 100% 피해를 주는지 확인한다.
78. 상단 HUD는 기세 1/2/3만 표시하고 0에서 숨기며, 상세 팝업은 기세 0/3~3/3을 계속 표시하는지 확인한다.
79. 회심의 일격 Hover·키보드 포커스 설명에 100/130/160/190%, 전부 소비, 재사용 없음과 현재 기세·예상 배율이 표시되는지 확인한다.
80. 난도 설명이 적중 후 기세 +1·기세 최대 3·세 번째 직접 사용 후 2턴으로 통일됐는지 확인한다.
81. 회심의 일격 대상 선택 중 Esc/취소가 스킬 메뉴로 복귀하고 선택만으로 피해나 기세 소비가 발생하지 않는지 확인한다.
82. 방어 중 적에게 회심의 일격을 사용하면 기존 50% 피해 감소가 적용되는지 확인한다.
83. 투사 전투불능 시 기세·쿨타임 표시가 숨겨지고, 기존 도발·치유의 빛·정조준·기본 공격·승패·도망·Field 복귀와 Console이 정상인지 확인한다.
84. 투사 스킬 메뉴에 spinner 아이콘+회오리 베기가 표시되고 Hover·키보드 포커스 설명에 적 전열 전체·80%·적중당 기세 +1·최대 3·재사용 없음이 보이는지 확인한다.
85. 살아 있는 전열 적이 1/2/3명일 때 회오리 베기가 전열에만 각각 80% 정수 올림 피해를 동시에 적용하고 기세가 각각 +1/+2/+3 되는지 확인한다.
86. 기세 2에서 적 2명 이상을 맞혀도 기세가 3을 넘지 않고, 기세 3에서도 회오리 베기를 계속 사용해 피해는 주되 기세는 3으로 유지되는지 확인한다.
87. 회오리 베기로 기세 3을 만든 뒤 난도에 2턴 쿨타임이 생기지 않고, 회심의 일격이 190% 피해 후 기세를 0으로 소비하는지 확인한다.
88. `회오리 베기!` 뒤 투사 중심 회전 참격→모든 대상 피해 숫자·피격 반응→기세 HUD 갱신→다음 턴 순서와 연출 중 입력 잠금이 정상인지 확인한다.
89. 전열 2명·후열 1명에서 전열만 피해를 받고 기세 +2인지, 전열 1명·후열 2명에서는 기세 +1인지 확인한다.
90. 전열이 전멸하고 후열만 살아 있을 때 `회오리 베기로 공격할 전열 적이 없습니다.` 안내 후 행동과 턴이 소비되지 않는지 확인한다.
91. 사수 스킬 메뉴에 target+정조준과 bow+화살비가 함께 표시되고, 화살비 설명에 원거리 물리·적 후열 전체·120%·재사용 없음이 보이는지 확인한다.
92. 후열 생존자가 1/2/3명일 때 후열만 각각 120% 피해를 거의 동시에 한 번씩 받고 전열 HP는 변하지 않는지 확인한다.
93. 대상마다 golden_arrow 3발이 떨어져도 피해 숫자와 실제 HP 감소는 대상당 한 번이며, 방어 중 후열은 기존 50% 감소가 적용되는지 확인한다.
94. 후열이 전멸하고 전열만 남았을 때 `화살비로 공격할 후열 적이 없습니다.` 안내 후 Projectile·행동·턴이 시작되지 않는지 확인한다.
95. 도발 상태와 관계없이 후열 전체가 유지되고, `화살비!`→상승→공중 대기→다중 낙하→동시 피격의 약 0.75초 순서와 연출 중 입력 잠금·완료 후 단일 턴 진행이 정상인지 확인한다.
96. 화살비 설명에 재사용 2턴이 보이고, 성공 연출 완료 후 화살비 재사용 2턴/hourglass_top이 표시되는지 확인한다.
97. 다른 아군·적 행동에는 2턴이 유지되고 다음 사수 행동에 1턴/hourglass_bottom, 그다음 사수 행동에 0으로 사라져 다시 사용 가능한지 확인한다.
98. 화살비 쿨타임 중 정조준이 사용 가능하면 정상 사용되고, 정조준 사용·쿨타임이 화살비 남은 턴을 변경하지 않는지 확인한다.
99. 후열 0명에서 화살비 거절 후 쿨타임 표시가 생기지 않고 행동이 그대로 유지되는지 확인한다.
100. `Assets/_Project/Scenes/Validation/CompanionAssaultRunValidation.unity`를 열고 Play 시 Wolf/Fox/Bear가 같은 속도로 오른쪽→왼쪽을 반복하며 픽셀 흐림이나 발 기준점의 불필요한 흔들림 없이 보이는지 확인한다.
101. `Space` 일시정지, `+/-` Run FPS 조절, `F` 좌우 Flip이 동작하고 Wolf/Fox/Bear의 속도감·타격감·현재 Battle Sprite 표시 크기와의 조화를 직접 비교한다.
102. 검증 Scene 실행 뒤 기존 `Battle.unity`의 사수 기본 공격·정조준·화살비와 다른 직업 스킬, Formation·대상·턴·승패/도망/Field 복귀에 회귀가 없는지 확인한다.
103. 사수 스킬 메뉴에서 Wolf 실제 Sprite 아이콘+동료의 습격이 표시되고, 정조준은 기존 `target.png`를 유지하며 설명의 야수/단일 물리·적 1명·전후열 자유·180%·3턴이 보이는지 확인한다.
104. 전열과 후열 생존 적을 모두 선택할 수 있고, 사수에게 도발 강제 대상이 있으면 그 적만 선택되는지 확인한다.
105. 사수는 제자리에 있고 Wolf가 현재 행동 중인 사수의 실제 `ActionRoot` 위치 근처에서 나타나는지 확인한다. 전열/후열 등 사수 표시 위치를 바꿔도 고정 화면 좌표가 아니라 새 사수 위치를 따라 출발하며, 6개의 서로 다른 다리 자세가 0→1→2→3→4→5→0 순서로 12 FPS 반복되는 동안 Transform은 독립적으로 570 UI 단위/초로 대상 바로 앞까지 이동해야 한다.
106. Wolf가 대상을 관통하지 않고 바로 앞에 정지하며, 도착 전 HP가 줄지 않고 도착 순간 한 번만 180% 피해·피해 숫자·피격 반응이 발생하고 방어 중 대상에는 기존 50% 감소가 적용되는지 확인한다.
107. Wolf가 타격 위치에서 짧게 멈춘 뒤 제거되고 피격·제거가 모두 끝날 때까지 입력이 잠긴 뒤 다음 턴이 한 번만 진행되는지 확인한다.
108. 사용 직후 재사용 3턴/hourglass_top, 다음 사수 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능 순서이며 정조준·화살비 쿨타임과 독립인지 확인한다.
109. 대상 선택 중 Esc/취소가 스킬 메뉴로 복귀하고, 기존 도발·치유의 빛·투사 스킬·Formation·턴 순서·HP HUD·승리/도망/Field 복귀에 회귀와 Console 오류가 없는지 확인한다.
110. 모든 스킬 메뉴에서 Hover/키보드 포커스 중 설명이 보이고, 스킬 선택 즉시 대상 선택 또는 연출 전에 사라지며 연출 종료·기본 명령 복귀 뒤에도 숨겨졌다가 다음 스킬 메뉴의 새 Hover/포커스에서만 다시 표시되는지 확인한다.
111. 캐릭터/상태 상세 팝업을 연 채 기본 공격·도발·치유의 빛·정조준·화살비·동료의 습격·난도·회심의 일격·회오리 베기를 시작하면 즉시 닫히고, 마우스를 기존 HUD 위에 계속 둬도 연출 중 재오픈되지 않으며 연출 종료 뒤 새 Hover/포커스에서 다시 열리는지 확인한다.
112. 마도사 스킬 메뉴의 파이어 볼에 Warm Explosion peak 아이콘과 단일 마법·전후열 자유·170%·화상 30%×2·재사용 3턴 설명이 표시되는지 확인한다.
113. 전열과 후열 적을 자유롭게 선택할 수 있고 마도사에게 도발 강제 대상이 있으면 그 한 명만 선택되는지 확인한다. 대상 선택 Esc/취소는 스킬 메뉴로 돌아가며 행동·쿨타임을 소비하지 않아야 한다.
114. `파이어 볼!`→Solar Shrapnel 초기 Charge→80×80 fireball 이동→192×192 Warm Explosion 순서이며, 폭발 index 4 전에는 HP가 줄지 않고 index 4에서 한 번만 170% 피해·화상 2·피격 반응이 발생하는지 확인한다.
115. 화상 대상이 행동을 마칠 때마다 46×46 작은 불꽃과 30% 저장 피해가 한 번 발생해 `화상 2→1→제거`되는지 확인한다. 큰 Warm Explosion이나 새 Projectile은 틱 때 나오지 않아야 한다.
116. 화상 1회가 남았을 때 다시 맞으면 합산되지 않고 2회로 갱신되며, 명중 당시 마도사 Attack 기준의 새 틱 피해로 교체되는지 확인한다. 화상 종료 후 재적중하면 새 화상 2가 생겨야 한다.
117. 사용 직후 파이어 볼 재사용 3턴/hourglass_top, 다음 마도사 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능 순서이고 다른 참가자 행동에는 감소하지 않는지 확인한다.
118. 화상 틱과 VFX 중 입력·두 정보 팝업이 억제되고 피격 완료 뒤 다음 턴이 한 번만 진행되는지, 화상으로 전투불능/승리가 발생해도 턴·승리·Field 복귀가 정상인지 확인한다.
119. 마도사 스킬 메뉴에서 썬더볼트에 electric-impact peak index 1 아이콘과 `대상: 적 전체`, 90%, 감전 1, 다음 행동 주는 피해 15% 감소, 재사용 3턴 설명이 표시되는지 확인한다.
120. 전열·후열에 적을 나누어 배치하고 전투불능 적을 섞었을 때 살아 있는 적 전체에만 짧은 청백색 예고와 144×144 electric-impact가 거의 동시에 나타나는지 확인한다.
121. electric-impact index 1 전에는 HP가 줄지 않고 index 1에서 모든 대상이 각각 일반 공격 90% 피해와 감전 1을 동시에 받으며, 방어 중 대상에는 기존 50% 받는 피해 감소가 유지되는지 확인한다.
122. 적에게 도발 상태가 있어도 썬더볼트는 한 명으로 좁혀지지 않고 EnemyAll 전체를 유지하며, 전열 전용 회오리 베기와 후열 전용 화살비 범위는 그대로인지 확인한다.
123. 감전된 서로 다른 적의 HUD와 상세 팝업에 각각 Kenney power 아이콘+`감전 1`이 보이고, 한 대상이 다시 맞아도 감전 2가 아니라 1로 갱신되는지 확인한다.
124. 감전된 대상의 다음 기본 공격·단일/광역 스킬 피해가 기존 값의 85%로 적용되고, 여러 대상을 공격해도 그 행동의 모든 피해가 감소하는지 확인한다.
125. 감전된 대상이 방어·회복·도발처럼 공격하지 않는 행동을 해도 행동 종료 후 감전이 제거되며 다른 대상의 감전은 독립적으로 남는지 확인한다.
126. 감전 대상이 전투불능이 되면 power 아이콘과 상세 상태가 즉시 사라지고, VFX·피격 완료 전 입력과 두 정보 팝업이 억제된 뒤 다음 턴이 한 번만 진행되는지 확인한다.
127. 썬더볼트 성공 뒤 재사용 3턴/hourglass_top, 다음 마도사 행동마다 2/hourglass→1/hourglass_bottom→0/사용 가능이며 파이어 볼 쿨타임과 독립인지 확인한다.
128. 기존 파이어 볼·화상·도발·치유의 빛·사수/투사 스킬·Formation·대상 선택/Esc·턴 순서·HP HUD·승리/도망/Field 복귀와 Console에 회귀가 없는지 확인한다.
129. 마도사 스킬 메뉴의 가이아 웰에 `dice_shield.png` 아이콘과 자기 보호·자신·받는 피해 60% 감소·자신의 다음 2회 행동·재사용 4턴 설명이 정확히 표시되는지 확인한다.
130. 가이아 웰 선택 시 대상 선택이나 Projectile 없이 마도사 위치에서 150×150 arcane-parry 16프레임이 20 FPS로 재생되고 peak index 8에서 HUD에 `가이아 2`가 나타나는지 확인한다.
131. 가이아 웰 사용 행동이 끝난 직후에도 `가이아 2`가 유지되고, 다른 아군·적의 여러 행동에는 감소하지 않는지 확인한다.
132. 마도사가 다음 행동으로 공격·파이어 볼·썬더볼트 등을 정상 사용한 뒤 `가이아 1`, 그다음 자기 행동 종료 뒤 상태와 아이콘이 제거되는지 확인한다.
133. 가이아 웰 활성 중 원시 피해 10이 4로 줄어드는지 확인하고, 방어 버튼이 사용 불가 색상으로 보이는지 확인한다. 클릭·키보드 Submit 시 `더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.` 안내 후 행동·턴이 유지되어야 한다.
133-1. 가이아 웰과 방어가 어떤 경로에서도 중첩되지 않고, 가이아 종료 후 방어 버튼 색상과 기존 10→5 피해 감소가 정상 복구되는지 확인한다.
134. 가이아 웰이 다른 아군 피해를 줄이거나 도발 대상을 바꾸지 않고, 마도사 전투불능 시 `가이아 n` HUD·상세 상태가 즉시 제거되는지 확인한다.
135. 사용 직후 재사용 4턴 표시가 생기고 다음 마도사 행동 시작마다 3→2→1→0으로 감소하며, 쿨타임 중 다시 사용할 수 없고 다른 참가자 행동에는 감소하지 않는지 확인한다.
136. 연출 중 입력과 두 정보 팝업이 억제되고 완료 후 다음 턴이 한 번만 진행되며, 기존 파이어 볼/화상·썬더볼트/감전·도발·치유·사수/투사 스킬·승리/도망/Field 복귀에 회귀가 없는지 확인한다.
137. Field_01에서 기존 초원 슬라임 5마리와 독침벌 3마리, 총 8마리가 서로 과도하게 겹치지 않고 Bounds 안에서 배회하며 독침벌 Idle 날갯짓과 Scale 0.85가 자연스러운지 확인한다.
138. 각 독침벌과 접촉해 Battle로 진입하고 전열 초원 슬라임 A/B·후열 독침벌 1 배치, 이름·HP HUD·대상 판정과 독침벌 우향 Idle 반복을 확인한다.
139. 독침벌 기본 공격에서 실제 Attack 시트가 재생되고 공격 완료 뒤 Idle로 복귀하며, 피해량과 턴 진행은 기존 적 기본 공격 규칙이고 독 상태가 생기지 않는지 확인한다.
140. 독침벌 조우 승리 시 접촉한 필드 스폰만 사라졌다 30초 뒤 자기 시작 위치에 리스폰하고, 도망 시 제거되지 않으며 복귀 직후 2초 재조우 유예가 유지되는지 확인한다.
141. 독침벌 직접 공격이 슬라임 10 대비 12 피해이며 방어 중에는 기존 방어 규칙이 유지되고, 정상 적중 대상 HUD에 Venom Ward index 6 아이콘과 `독 3`이 표시되는지 확인한다.
142. 독 대상 자신의 행동 종료에만 최대 HP 5% 올림·최소 1 피해와 작은 Acid Splash가 발생해 `독 3→2→1→제거`되고 다른 참가자의 행동에는 감소하지 않는지 확인한다.
143. 독 1/2/3에서 다시 독침벌 공격을 받으면 더해지지 않고 독 3으로 갱신되며, 완전 종료 뒤 재적중하면 새 독 3이 생기는지 확인한다.
144. 상세 팝업에 `독 n`과 `독: 행동 종료 시 최대 HP 5% 피해`가 보이고, 독 피해로 전투불능이 되면 독 아이콘·문구가 즉시 사라지는지 확인한다.
145. 화상과 독이 같은 대상에 있으면 행동 종료에 두 작은 틱 연출이 순서대로 한 번씩 발생하고 모두 끝난 뒤 다음 턴이 한 번만 진행되는지 확인한다.
146. 치유사 스킬 메뉴에서 spectral-bloom peak index 5 아이콘과 `정화` 이름이 함께 보이고 상세 팝업에 상태이상 해제·살아 있는 아군 1명·독/화상/감전 모두 제거·재사용 2턴·구현 사용 가능이 표시되는지 확인한다.
147. 정화 대상 선택에 자신을 포함한 살아 있는 아군만 포함되고 전투불능 아군은 선택할 수 없으며, Esc/취소 시 행동 없이 스킬 메뉴로 돌아가는지 확인한다.
148. 상태가 없는 살아 있는 아군을 선택하면 `정화할 해로운 상태가 없습니다.` 안내 후 VFX·행동·턴·쿨타임 없이 스킬 메뉴가 유지되는지 확인한다.
149. 독 3 대상에게 정화를 사용하면 spectral-bloom 전체 16프레임이 20 FPS로 재생되고 release index 7 전후에 독 HUD·상세 상태가 즉시 사라지며 HP는 변하지 않는지 확인한다.
150. 독+화상 및 독+화상+감전 대상에게 각각 한 번 사용해 걸린 해로운 상태가 모두 동시에 제거되는지 확인한다.
151. 정화가 대상의 방어·가이아 웰·도발 관련 상태·기세·각 스킬 쿨타임을 제거하거나 변경하지 않는지 확인한다.
152. 정화 연출 중 입력과 스킬/상태 상세 팝업이 억제되고 연출 종료 뒤 다음 턴이 한 번만 진행되는지 확인한다.
153. 성공 직후 정화 재사용 2턴이 표시되고 다른 아군·적 행동에는 유지되며, 다음 치유사 행동에 1턴, 그다음 치유사 행동에 사용 가능으로 돌아오는지 확인한다.
154. 기존 치유의 빛·회복의 파동·독 틱·화상 틱·감전 피해 감소와 모든 직업 스킬, 방어·도망·승패·Field 복귀에 회귀가 없고 Console에 Compile Error·NullReference·MissingReference가 없는지 확인한다.
155. 독침벌의 첫 행동이 12 피해와 독 3을 함께 적용하고, 두 번째·세 번째 행동은 각각 12 피해만 적용하며 독을 새로 부여하거나 갱신하지 않는지 확인한다.
156. 두 번째 독침벌 행동 종료에 내부 대기 2→1, 세 번째 행동 종료에 1→0이 되고 네 번째 행동에서 다시 12 피해+독 3을 적용하는지 확인한다.
157. 첫 독 부여 직후 치유사 정화로 독을 제거하고, 이어지는 벌의 두 공격에서는 12 피해만 받고 독이 즉시 재적용되지 않으며 네 번째 벌 행동에서만 다시 독이 생기는지 확인한다.
158. 독이 정화 없이 자연 종료되어도 해당 벌의 독침 대기는 별도로 유지되는지 확인한다.
159. 독침이 사용 가능한 상태에서 이미 독인 대상을 공격하면 독이 4 이상 중첩되지 않고 3으로 갱신되며 해당 벌의 대기가 다시 2로 시작하는지 확인한다.
160. 향후 검증용으로 독침벌을 2마리 이상 배치했을 때 한 벌의 독 부여와 행동 종료가 다른 벌의 독침 대기시간을 시작하거나 감소시키지 않는지 확인한다.
161. 매 행동 120% 직접 피해, 독 5%×3, 독 HUD 3/2/1, 정화·화상·감전·모든 직업 스킬과 승리/도망/Field 복귀가 유지되고 Console 오류가 없는지 확인한다.
162. 수호자 스킬 메뉴에서 `structure_wall.png` 아이콘과 `철벽` 이름이 함께 보이고, 상세 팝업에 자기 보호·자신·70% 감소·자신의 다음 2회 행동·재사용 4턴·공용 방어 중첩 불가가 표시되는지 확인한다.
163. 철벽 사용 시 대상 선택·Projectile 없이 수호자 발밑에서 Earth Rupture 전체 20프레임이 짧게 한 번 재생되고 peak index 9 부근에 `철벽 2` HUD가 나타나며, 연출 종료 후 암석이 캐릭터를 가리지 않는지 확인한다.
164. 철벽 사용 행동 종료 직후 `철벽 2`가 유지되고 다른 아군·적 행동에는 감소하지 않으며, 수호자의 다음 공격·스킬·방어 외 행동 종료 후 `철벽 1`, 그다음 자기 행동 종료 후 제거되는지 확인한다.
165. 철벽 중 원시 피해 10이 3으로 적용되고, 방어 버튼이 사용 불가 색상이며 클릭·키보드 Submit 시 `더 강한 방어 효과가 이미 적용 중이라 방어를 사용할 수 없습니다.` 안내 후 행동·턴이 유지되는지 확인한다.
166. 철벽 종료 후 공용 방어가 다시 가능하고 피해 10→5가 유지되며, 철벽과 공용 방어가 어떤 입력·피해 경로에서도 중첩되지 않는지 확인한다.
167. 철벽과 도발을 동시에 유지한 상태에서 적 단일 공격이 수호자로 향하고 70% 감소가 적용되며 두 상태의 남은 횟수가 각자 규칙대로 독립 감소하는지 확인한다.
168. 철벽 성공 직후 재사용 4턴/hourglass_top이 표시되고 수호자의 다음 행동 시작마다 3→2→1→0으로 감소하며 다른 참가자 행동에는 변하지 않는지 확인한다.
169. 수호자 전투불능 시 철벽 상태·아이콘·상세 문구가 즉시 제거되고, 연출 중 스킬/상태 팝업 억제와 완료 후 입력 복구·단일 턴 진행이 정상인지 확인한다.
170. 기존 도발·공용 방어·가이아 웰·모든 직업 스킬·독/화상/감전/정화·Formation/TurnOrder·승리/도망/Field 복귀에 회귀가 없고 Console에 Compile Error·NullReference·MissingReference가 없는지 확인한다.
171. 수호자 스킬 메뉴에서 실제 `pawns.png`와 `수호의 맹세`가 보이고 상세 설명에 광역 보호·자신 제외 생존 아군 전체·직접 피해 최대 50%·감소량 절반 이전·최대 HP 40% 보호 예산·DoT 제외·다음 행동 전·4턴 재사용이 표시되는지 확인한다.
172. 사용 시 Frost Nova 중간 테두리가 금백색·낮은 강도로 파티 전체에 짧게 한 번 재생되고 공격용 얼음 폭발처럼 과도하지 않으며 지속 VFX가 남지 않는지 확인한다.
173. 원시 피해 20을 보호 아군이 받으면 아군 10, 이전 예정 5가 되고 수호자에게 철벽이 없을 때 실제 5 피해가 적용되며 금색 선·섬광과 HUD 예산 감소가 함께 보이는지 확인한다.
174. 최대 HP 100 수호자의 이전 예산이 40으로 시작하고 철벽 중에도 이전 예정량 기준으로만 40까지 소비되며, 각 실제 이전 피해에는 70% 감소가 적용되어 예정 5가 실제 2가 되는지 확인한다.
175. 남은 예산 3에서 큰 직접 피해를 받으면 수호자 이전 예정량 3, 아군 감소량 최대 6만 적용되고 예산 0과 동시에 수호의 맹세 HUD가 사라지는지 확인한다.
176. 화상·독 행동 종료 틱은 아군에게 그대로 적용되고 수호자 HP·이전 예산·금색 이전 연출이 변하지 않는지 확인한다.
177. 수호자의 다음 행동 시작 직전에 수호의 맹세가 종료되고 쿨타임이 3으로 감소하며, 다른 참가자의 행동 시작·종료에는 보호와 쿨타임이 유지되는지 확인한다.
178. 수호자 전투불능 즉시 보호가 종료되고 도발·철벽과 동시에 유지 가능하며, 수호자 자신의 직접 피격은 이전 없이 기존 철벽·방어 규칙으로만 처리되는지 확인한다.
179. 단일 기본 공격·파이어 볼 등 단일 공격 스킬·화살비/회오리 베기/썬더볼트 같은 광역 공격의 아군 대상마다 보호 예산이 순서대로 소비되고, 대상 수를 4명 이상으로 늘려도 고정 3인 제한이 없는지 확인한다.
180. 기존 공용 방어·가이아 웰 대상이 보호받을 때 수호의 맹세 감소 뒤 각 대상의 기존 개인 방어가 유지되고, 도발·정화·감전·회복·승패·도망·Field 복귀에 회귀 및 Console 오류가 없는지 확인한다.

기존 Male/Female, Path Visual과 Wheelchair Variant, 이름표, 월드 경계·전환·초원 슬라임 필드 Animation도 회귀가 없는지 함께 확인한다.

181. 한 번의 Play Mode에서 회복의 파동·정화·철벽·수호의 맹세·가이아 웰을 각각 3회 이상 사용해 캐릭터 본체 색 변화·흰 네모·단색 사각형·잔존 Image·반복 누적이 없고 원본 VFX 후광만 정상 표시되는지 확인한다. 이어서 Play→Stop을 최소 3회 반복해 같은 항목을 재확인한다.
182. 파이어 볼·썬더볼트·화살비·동료의 습격·독/화상/감전·숲거미 독액 분사의 기존 색과 피해 횟수가 유지되는지 확인한다.
183. 민첩 동률 전투를 여러 번 새로 시작해 주사위가 순차 반복이 아닌 1~6 랜덤 눈으로 바뀌고 최종 눈도 항상 1이 아닌지, 결과 문구·Timeline·첫 행동자가 일치하고 다음 라운드에는 재표시되지 않는지 확인한다.
184. 민첩 동률이 없는 전투에서는 Overlay가 없고 바로 정상 입력으로 시작하는지 확인한다.
185. 화살비 후열 0명과 회오리 베기 전열 0명에서 범위별 경고 뒤 같은 스킬 메뉴가 즉시 조작되고 Esc·취소로 상위 명령에 복귀하며, 행동·턴·쿨타임·기세·VFX가 변하지 않는지 확인한다.
186. 기본 공격과 단일 스킬 Target Selection에서 화면 취소·Esc가 각각 같은 이전 메뉴로 돌아가고, 대상이 있는 화살비·회오리 베기·썬더볼트의 대상당 1회 피해와 감전·기세가 유지되는지 확인한다.

VFX PNG 전부가 RGBA Alpha 0~255이고 Sprite Import의 Alpha Is Transparency가 켜져 있으며, Arcane Parry 16·Radiant Heal 14·Spectral Bloom 16·Earth Rupture 20·Frost Nova index 4~10의 선언 범위가 실제 시트 크기 안에 있음을 확인했다. 회복의 파동 중앙 VFX에 남아 있던 구형 `CreateEffectImage` 일곱 번째 인수를 제거했고 프로젝트 내 15개 호출이 현재 5/6개 인수 시그니처와 일치한다. 광역 스킬 0 Target 입력 복구까지 Unity 6000.5.7f1의 전체 `Assembly-CSharp` 응답 파일로 별도 출력 컴파일해 오류 0개, 기존 deprecated API 경고 4개만 확인했으며 관련 파일 `git diff --check`를 통과했다. Tie Break와 수호의 맹세 계산·피드백·VFX 코드는 변경하지 않았다. 실제 Play Mode의 화살비·회오리 베기 Invalid Action 메뉴 복구와 Esc·취소, 수호의 맹세 및 위 171~186 항목은 직접 확인해야 한다.

## 다음 권장 작업

길별 새 캐릭터로 Battle에 진입해 회복탄력 50% 통과와 2회 소비, 적별 잔향 소비·만료, 집중 0/3/6/9%와 광역 비증가, 굳건한 자리 전열/후열, 적·아군 쌍별 패턴 피해 감소·치유 증가를 확인한다. 같은 전투에서 화상·독이 길 배율을 받지 않는지, 전투 종료 후 새 전투와 저장 이어하기에서 길 런타임이 초기화되는지도 확인한다.

우선 실제 정상 입력으로 Field_01/02 전투 승리·레벨 업·즉시 저장/재실행 복원·맹독 교체/정화·뱀 이동/공격/리스폰을 확인한다. 이어 실제 Battle에서 위 15개 스킬 버튼의 가독성·Esc 재진입·다음 턴/새 전투 아이콘 유지를 확인한다. 썬더볼트 버튼 Electric Impact와 감전 상태 power.png의 구분을 유지한다.

Unity Play Mode에서 월드 안쪽으로 이동 후 5초 자동 저장과 Stop/재실행 뒤 실제 위치 복원, Bounds 밖 좌표의 SpawnPoint fallback, Field 전환 뒤 새 Scene 좌표 저장, Battle 중 종료 시 마지막 안전 좌표 유지를 확인한다. Bootstrap 삭제 확인의 취소·단일 슬롯 삭제·즉시 빈 슬롯 갱신과 Editor 관리 창도 함께 확인한다. 이후 Field_02 감전 독립 적용과 기존 전투·외형·리스폰 회귀도 확인한다.

## 갱신 규칙

기능 작업 완료 시 완료 기능, 검증 상태, Unity 직접 확인 사항, 다음 권장 작업과 마지막 관련 commit hash를 갱신한다. 긴 작업 로그는 기록하지 않는다.
