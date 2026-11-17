> 2026-11-17 EOD 선별 기록: 출처 docs/art-renewal 221ec3dc9f102037b08cf0bc6f2017a173ed6f69 / blob 554e42f81bacd66d57bda14dbba0db7bc7804fd3. 아래는 작성 당시 상태이며 최신 승인·실제 적용은 EOD 보고가 우선합니다. 얼음갑충·펫분양독립원화·눈보라정령은 게임 미적용, 설원족제비 Defeat4는 승인대기.

# LIMITLESS / Chapter1 / Issue #107 — Main04 치료받는 부상 여행자 게임 임포트용 정지 원화 납품

## 실제 산출물
- **사용자 지시:** 그림 보기용이 아닌 **게임에 넣을 실제 스프라이트 파일** 제공.
- **파일:** `Main04_WoundedTraveler.png` (512×512, 투명 RGBA, 정지 1포즈, 중앙 정렬, 바닥 기준 y470).
- **PNG SHA256:** `2687c1c62e5bd20e6e0fb16c3428225aa28fc156c80a5c08b6af2350d9e9715d`.
- **실제 작업환경 경로:** `/mnt/data/Main04_WoundedTraveler.png`. 사용자 Windows F: 로컬 자동 복사 아님.
- **원본 이미지:** `치료받는_지친_여행자_npc.png` 1254×1254 RGBA, SHA256 `5491b56e714e224abb9ef54cb71cec9c88305b43dd0d4b7d299fa9637a3ab36b`; 고유 이미지 원본 alpha bbox x205..1060, y182..1174를 비율 유지 축소(가시 높이348px, 너비300px)하여 512 투명 정지 캔버스에 위치시켰음. 최종 alpha bbox `(106,122,406,470)`. 재생성 아닌 **이전 시안 원본 변환·패키징**.
- **오프라인 미리보기:** PNG 원본에서 직접 LANCZOS로 128×128 및 40×40 축소 각1장.
- **직접 비교판:** `Review_OFFLINE/Main04_Actual_512_128_40_Comparison.png`, SHA256 `75155f718905fd7dc16a9441bb0e90d49f08a835dbf5aa42295657d258e03bd7`: OGA07/OGA11 승인 원본과 신규 Main04 PNG의 512/128/40 **실제 픽셀만** 사용. 정식 미엘 원화가 포함된 비교는 아님. 이미지 검색/생성된 무관 인물 혼입0.
- **검수 자료 전체 ZIP** `Limitless_Main04_Traveler.zip`, **650099 bytes**, SHA256 `286fe97b6f68f44f45d22a6a4940b9e5b84fc09d8b53d7d9333211e3bc4311a3`, 9파일, ZIP CRC PASS. SourceDeclaration/QA/Manifest(비순환)/SHA256SUMS, 원화1·128/40 PNG2·비교판1 등.
- **바로 전달할 수 있는 PNG만 든 ZIP** `Main04_Traveler_PNG.zip`, **165562 bytes**, SHA256 `a72c51245e126313d1e75442adbf42ddfd6f7c21af86d909306179e4732d663e`, 1파일, ZIP CRC PASS.
- ChatGPT Library `/LIMITLESS_Art_Delivery/Issue107_Main04_WoundedTraveler_20261117/`에 **PNG·2 ZIP·실제 비교판 4개 파일 업로드 성공**. GitHub 바이너리 업로드0.

## 퀘스트 근거와 디자인 선택
- GitHub `문서/03_스토리/메인_스토리와_퀘스트_설계.md` Main04 「세 사람」 및 `MainQuest04FieldFlow.cs` 실물 확인. Field_01 미엘 첫 등장, 미엘이 습격당한 여행자를 치료. 현재 `CreateWoundedTraveler`은 `VillageNpcSprites/Eldiran/OGA07_Resident01`을 임시로 가져와 렌더링하고 `StoryLabel`을 사용함. 게임 대사·퀘스트·배치 변경0.
- 신규 후보는 **조심스러운 앉은 자세·피로한 얼굴·팔의 간결한 천 붕대·여행용 외투/가방**으로 응급처치 중인 정황을 표현. **붕대 위치는 시각적 제안이며 실제 공식 부상 부위/성별/신상/영구 장애/직업 확정 아님**. 과한 피·고어 없음. 기존 OGA07과 구별되는 헤어·복장·실루엣.
- **원본 주민12 및 미엘·태온 공식 원화, Scene/Prefab/StableID/Label/Quest/Save/UserData/Discord 전부 변경0**, Main07 정체성 임의 확정·제작0. Unity **6000.6.5f1**, Issue #85 HOLD, main 직접 commit/Push/Merge0.

## 적용 전 게이트
- **`MASTER_PNG_READY / FILE_TECH_PASS / USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED / GAMEVIEW_NOT_VERIFIED`**.
- 실무적으로는 **Unity 임포트 가능한 규격으로 PNG를 납품**한 것이며, PPU·Pivot·SpriteRenderer world bounds·기존 y1.15 라벨·Main04 미엘 배치/Sorting을 실제 Unity에서 검증한 것은 아님. 다음 단계는 사용자의 미술 승인 후 Codex가 `MainQuest04FieldFlow.CreateWoundedTraveler`에서 **임시 OGA07 참조만** 신규 전용 Sprite로 최소 교체하고 최소 DeltaQA를 수행하는 것. 512 마스터를 기존 PPU28 그대로 Import하지 않음. 재사용 중인 OGA07 승인 원본/메타와 기존 Scene은 수정하지 않음.
- 원본 신규 PNG와 승인 NPC는 별도 자산으로 유지. 최초 모델 PNG 파일 참조는 비교판 제작 과정에서도 읽기전용.
