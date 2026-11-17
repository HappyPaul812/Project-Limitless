> 2026-11-17 EOD 선별 기록: 출처 docs/art-renewal 221ec3dc9f102037b08cf0bc6f2017a173ed6f69 / blob 50af3984033232073561b7ded6074cc9cec3fd91. 아래는 작성 당시 상태이며 최신 승인·실제 적용은 EOD 보고가 우선합니다. 얼음갑충·펫분양독립원화·눈보라정령은 게임 미적용, 설원족제비 Defeat4는 승인대기.

# LIMITLESS / Chapter1 / Issue #108 — Main07 숲길 부상 여행자 정지 원화 납품 (2026-11-17)

## 판정·근거
- 바울 대표 직접 결정: Main04 부상 여행자 최종 미술 승인. **기존 Main04 원본 PNG BYTE_KEEP**, SHA256 `2687c1c62e5bd20e6e0fb16c3428225aa28fc156c80a5c08b6af2350d9e9715d`. 이번 작업은 Main07 1명만.
- [Issue #108](https://github.com/HappyPaul812/Project-Limitless/issues/108), 공식 [스토리 정본](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/03_스토리/메인_스토리와_퀘스트_설계.md) Main07 「깊게 패인 흔적」, [MainQuest07FieldFlow.cs](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs) 실제 내용 및 최신 리뉴얼 인계 확인. Field_02 `field02_main07_wounded_traveler`, `WoundedTravelerVisual`은 임시 `VillageNpcSprites/Eldiran/OGA07_Resident01` 사용. 미엘은 치료를 위해 지정 전투에서 제외. Main04와 동일인임을 명시한 근거는 확인되지 않음. 동일인 여부/부상부위/신상 미확정 유지.
- #108 댓글 없음·담당 인계 및 접근 가능한 Library 납품 폴더 목록에서 기승인 Main07 전용 원화 확인되지 않음. Windows F: 전체 전수 확인은 아님. 이번 별도 외형 후보는 캐논 추가 아님.

## 실제 PNG
- `Main07_WoundedTraveler.png` **512×512 투명 RGBA 정지 1포즈**, **164593 bytes**, SHA256 **`90c727b95bee6b2586333d44ebb05b8077664a7e5877084cbbe4a1010676b432`**.
- 신규 AI 생성 원본 1254×1254 RGBA `a_detailed_pixel_chibi_rpg_illustration_on_a_trans.png`, SHA256 `52b7fd80a670b85687c5ac1fc4218e7cf0e48e33e27c80afb441614973da00ca`, 비율 유지 축소·중앙 정렬. 최종 alpha>8 bbox (96,122,414,466), 알파 캔버스 외곽 접촉0, 배치 기준 y470에 가까운 접지. 실제 미엘과 PPU/Pivot/씬 배율 및 접지는 미검증.
- **차별점:** 회색빛 앞머리, 길고 좁은 눈매, 성숙한 인상, 자주색 후드 여행복과 청회색 내의, 무릎을 세우고 한 손을 지지한 앉은 자세, 다리 천 붕대. Main04의 갈색 머리·녹색 외투·팔 붕대와 구별. OGA07의 두건/생활복 및 OGA11의 모자/조끼/채소바구니와 차이. 붕대 위치는 시각 제안에 한정; 정확한 부상 또는 신상 설정 확정0. 과한 피/고어 없음. 40px에서 세부 눈/붕대 묘사 제한.
- 실제 OGA07, OGA11, 사용자 승인 Main04 PNG를 읽기 전용으로 비교. 원본 바이트 변경0. `Main07_Actual_512_128_40_Comparison.png` (외부 별도 파일 `Main07_Comparison.png`) SHA256 `0bf98062bcac3b044a50ff558df890df902729ac63eb9e696baf681fa0114d25`. 원본512 및 LANCZOS 실제 128/40 크기; 검색/다른 생성 인물 대용0. 비교판은 오프라인.

## 다운로드/검사
- `/mnt/data/Limitless_Main07_Traveler.zip`: **815952 bytes / SHA256 `b943fb9198f02da7a6df90853457e1bac2b74d916c356b2faab6bac8089db6bd`**, 9 entries, CRC PASS, Manifest 7/7 size/SHA PASS, SHA256SUMS 8/8 PASS. PNG1, 128/40 PNG2, 비교판1, README/SourceDeclaration/QA/Manifest/SHA256SUMS.
- `/mnt/data/Main07_Traveler_PNG.zip`: **163802 bytes / SHA256 `64906abbb46200338bb8cf81ea7635c553dfdbf56e4887a64085a25c5da137da`**, 1 PNG, CRC PASS.
- ChatGPT Library `/LIMITLESS_Art_Delivery/Issue108_Main07_WoundedTraveler_20261117/`에 원화, ZIP2, 비교판 총4파일 저장 성공. Windows F: 자동복사/게임 적용 아님.

## 상태 및 보호
- `FILE_TECH_PASS / MAIN04_USER_ART_APPROVED_BYTE_KEEP / MAIN07_IDENTITY_NOT_CANON_CONFIRMED / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`.
- Main04 승인본, OGA07/기존 승인 NPC, 미엘·태온·폴, 원본 Scene/Resources/GUID/meta/Quest/대화 및 Save/UserData·Discord 전부 미변경. 초상화·애니메이션·추가 NPC 제작0, Unity6000.6.5f1 및 Issue85 HOLD 유지. main 직접 Commit/Push/Merge0.
- 다음 단계는 사용자 미술 승인 후 Codex의 Main07 기존 임시 Sprite 참조만 별도 전용 Sprite로 교체하는 최소 검증. 실제 배치·PPU/Pivot/세계 크기/미엘과 장면 및 UI 간섭 확인 전 적용 금지. Main04 적용 여부는 본 작업으로 확정하지 않음.
