# Issue #111 — 잔여 실사용 외부 NPC 참조 정리

## 적용 범위

레온 #109 완료(c00fbb63…) 및 파티 안내인 #110 완료(6c81a03d…) 후 진행했습니다. Catalog의 두 Resources 경로만 교체합니다. 새 원화/PNG/meta/GUID 변경·삭제는 없습니다.

| 실제 NPC | 승인 원화 | 생성/확인 위치 |
|---|---|---|
| arbel-pet-adoption | OGA17_EquipmentShop_Original_Approved | Arbel (5,-3) |
| arbel-resident-west | OGA11_Resident02_Original_Approved | Arbel (-5,2) |
| safezone_catacomb_entrance-pet-adoption | 같은 OGA17, PetAdoption fallback | Main21 완료 / Field_03 (-2.05,-3.55) |
| safezone_starter_village-pet-adoption | 같은 OGA17, PetAdoption fallback | Main21 완료 / 시작마을 (-5.8,-3.6) |

OGA17 GUID `5b473af875ba45b586eae7d500dbe10e`, PPU358.75, Pivot(.4990234375,.08203125)=(255.5,42). SHA256 `1282d90a7f89924e3efaad40679cbc272193bb9e05397765671549d9de646359`.
OGA11 GUID `152b9c2ebef44afe9f39710fd1377ff7`, PPU358.75, Pivot(.4990234375,.080078125)=(255.5,41). SHA256 `5fde2e4cfbcdca2839f7da1071dbbbc4b7f239ee85ab7f7b9985be48a5cb91f9`.
기존 승인 파일의 import 설정 그대로입니다. 50% alpha 가시 경계: OGA17 (141,60)-(370,469), OGA11 (150,62)-(361,471), 가시 높이 약1.14WU/발끝 원점 근방 유지. 원화·Transform·Collider 보정 추가0.

## 단색 주민 조사 — 변경하지 않음

Arbel.unity의 `VillageNpc`는 `VillageNpcPlaceholder.prefab`(GUID4c4988789a0e2a049b860bfa288d71bc) 인스턴스입니다. Scene에 (1.5,-1.5)로 직렬화되어 있으며 VillageNpcRole·StableID가 없습니다. NpcController의 이름은 ‘마을 주민’, 기본 대사는 ‘어서 오세요. 여기는 우리의 첫 번째 마을입니다.’입니다. 시설/퀘스트 전용 역할 연결은 확인되지 않았습니다.

`StarterVillageSceneGenerator.CreateStarterVillageScene`의 초기 마을 생성 코드가 같은 프리팹을 같은 이름·좌표로 생성합니다. 현재 Scene에 초기 임시 주민 인스턴스가 남아 있는 상태입니다. 누가 언제 Arbel에 복사했는지까지는 현재 파일만으로 확정하지 않습니다.
`PlaceholderVisual`이 원본 Sprite가 비어 있으면 1×1 흰 텍스처를 런타임 생성하고 노랑(1,.75,.2), 크기(.7,.9) 및 ‘마을 주민’ Label을 설정합니다. 외부 PNG 사용이 아닙니다. 아르벨 역할 NPC를 설치하는 Chapter2IntroFlow와 별개로 Scene 인스턴스가 유지됩니다. 삭제·숨김·신규 역할 부여는 하지 않았으며, 잔존 임시 주민의 향후 처리에는 별도 기획 결정이 필요합니다.

## 최소 Delta QA

기존 격리 프로젝트 `Temp/Art95_91Unity20261111/QA/Client` 재사용, Unity6000.6.5f1 숨김 Batch Mode. 새 대형 복제/foreground 조작0. 합성 Save·설정은 이번 Temp 하위에만 기록했습니다.

**DELTA160PASS / 0FAIL / runtime errors0 / ORIGINAL_WORKING_TREE_APPLIED**. 원본 컴파일 완료(compileFalse)·Console 오류0/경고42, Bootstrap/PlayFalse/dirtyFalse/선택null/포커스null 유지.

960/1280/1680×720 실제 렌더: 승인 외형·발 접지·화면 안 크기·얼굴 비가림, 기능 이름표Y1.37와 기존 E 안내 유지. Resident는 이름표 비활성 정책 유지(숨겨진 Label의 Y는 원래 조정하지 않는 구현). 실제 E/가상 패드A로 분양 확인→목록, 마우스 실제 raycast로 100탈렌트 분양·합성 Save, Escape/패드B 닫기. 서쪽 주민 기존 대화/패드 상호작용 유지. 레온·파티 등 기타 NPC 역할/대화/좌표/Collider/Label 데이터는 시각 교체 전후 동일성 비교.

첫 QA의 숨겨진 Label Y 단정은 기존 early return 규칙에 맞게 수정했습니다. 카타콤 진입 시 이전 합성 좌표 및 안전 반경 바깥 접근으로 비표적 전투가 발생해 QA 배치만 안전지대 안쪽으로 조정 후 재실행했습니다. 제품의 전투·안전지대 코드는 변경하지 않았습니다. 테스트 과정의 중단을 제품 PASS로 취급하지 않았습니다.

## 실사용과 보관 구분

동일한 #110 조사 범위인 3지역×BASE/Main21 완료 상태의 실제 NpcController 56건: BASE 시작마을12/카타콤4/아르벨10, 완료 후14/6/10. 잔여 legacy 실제 렌더0, 기존 legacy PNG12종은 보관 중이며 이 조사 범위에서는 렌더되지 않습니다. 레온 전용 원화·공식 세린은 legacy 외부 NPC로 분류하지 않았고, 단색 임시 주민은 런타임 Sprite입니다.

FrostwayVillage의 Main21 PetAdoption도 공통 fallback을 사용하므로 승인 OGA17 경로를 받습니다(정적 영향 확인). 이번 렌더·기능 검증은 요청된 시작마을/카타콤/아르벨 세 지역 범위이며 게임 전체 외부 애셋 제거를 뜻하지 않습니다. 외부 PNG는 모두 보관합니다.

## 원본 보호·기록

제품 변경: `Unity/Client/Assets/_Project/Scripts/NPC/VillageNpcAppearanceCatalog.cs` 두 문자열만. #106 이름표/안내, #109 레온 전용 안내 보정, Main04/Main07 및 승인 NPC12 원화/매핑 보호. 보호 기준8356파일 및 기존 Git 변경 상태 대조. Issue85 패키지/GUID HOLD·Save/UserData/Discord 유지. 신규 원화·PNG/meta/GUID·Scene 수정0. Push/Merge0.

로컬 브랜치 `codex/issue111-residual-npc-20261117`, 제품 commit `e4478f57f2e60a94d32a07b79d00b6fed1cf975e`. 원본 root main HEAD `52f84e643d5d860665eadfe1614a30c745209f00` 유지. 문서 커밋·최종 대조는 Final.json 및 Issue111 댓글 참조. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.

상세 기계 증거: `Temp/Issue111Residual20261117/Final.json`, `After/checks.txt`, `After/Errors.txt`, `After/NpcUsage.tsv`, `usage-audit.json`, 각 해상도 PNG 및 `Unity-Delta-R4.log`. Temp 증거·보호 목록은 비공개 로컬입니다. 기존 열린 원본 Scene/Play/포커스는 그대로 유지하며 대표 실제 새 Play 확인은 위 표 좌표입니다.
