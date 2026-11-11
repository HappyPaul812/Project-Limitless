# LIMITLESS / 리뉴얼 아트 / Issue #57 — OGA03·OGA09 기존 NPC 원화 최종 승인 검토

**검토일:** 2026-11-11 KST  
**담당 브랜치:** `docs/art-renewal`  
**판정:** `OGA03_OGA09_EXISTING_512_BYTE_KEEP / 128_40_OFFLINE_REVIEW_PASS / KEEP_RECOMMENDED_2 / USER_FINAL_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`.

## 1. 최신 정본/범위

- [Issue #57 최신 기획 지시](https://github.com/HappyPaul812/Project-Limitless/issues/57#issuecomment-6786953795), [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60), [Issue #95 실제 원본 적용 및 사용자 확인](https://github.com/HappyPaul812/Project-Limitless/issues/95#issuecomment-6786869689), `WORLD_ART_RENEWAL_LATEST.md`, `COMMON_RULES.md` 확인.
- 대상은 **OGA03 잡화상인, OGA09 주민대표 기존 후보 2장**뿐. 2026-10-23 원본 `Limitless_Chapter12_NonOriginal_Audit_NPCPilot2_20261023.zip`을 실제 열어 기존 512×512 RGBA 원화 2장 확보. 2026-11-10 `LIMITLESS_ELDIRAN_NPC_OGA06_OGA16_ORIGINAL_PILOTS_20261110.zip` 안의 OGA03/OGA09 비교용 원본과 **두 장 모두 완전한 바이트 동일성** 확인.
- 기존 승인·적용 **OGA06 치유사/OGA16 은행안내**는 #95의 최신 Codex/기획 보고에 따라 `USER_ART_APPROVED / LOCAL_ORIGINAL_APPLIED / UNITY_QA_PASS / USER_REALPLAY_ACCEPTED / REMOTE_NOT_PUSHED`. **이전 리뉴얼 인계의 'OGA06/16 아직 미적용' 상태는 최신 #95 결과로 정정**. 이번 작업에서는 해당 승인 512 마스터 PNG 2장을 읽기 전용 화풍 기준으로 사용. 원본 로컬 Unity 직접 조작/실게임 재검수 0.

## 2. 기존 후보 원본 SHA256 고정

| 이름 | 기존 원본 ZIP 실제 경로 | 크기 | SHA256 | 미술 판단 |
|---|---|---|---|---|
| OGA03 잡화상인 | `NPC_Pilot2/Eldiran_GeneralShop_Concept_512.png` | **512×512 RGBA** | `09b2d97643a7aaefd51930eb374c48bf358020f015cc4cce9e7ef4903853151d` | **KEEP 권고** |
| OGA09 주민대표 | `NPC_Pilot2/Eldiran_VillageRepresentative_Concept_512.png` | **512×512 RGBA** | `ca0336ab6bbc7aaeb16169037e73ee39ced1b674f48324baeb75b1debfa29fcf` | **KEEP 권고** |

- 기존 승인 기준 화풍 PNG의 SHA: OGA06 `67aef89677e9b9f174f30b2c2a9e854eaee4e61814774402bd94a18691bd401f` / OGA16 `1199c57d3c279b51af83452c8b580814d6456417ea8e3f3b83ea9fdc6c5f8dcd`. 이번 비교 참조 2개도 소스 바이트 완전 보존.
- 원본 4장의 가시 알파(alpha>8)가 512 캔버스 네 변에 접촉한 픽셀 **각 0개**. OGA03 가시 bbox (138,70,367,472), OGA09 (137,75,404,472). **새 원화·재인코딩·기존 PNG 원본 덮어쓰기 0**.

## 3. 시각/역할 구별 판단

- **OGA03**: 갈색 앞치마와 양쪽 물품 가방, 머리띠/옷 소매가 잡화 거래 NPC로 읽힘. OGA06 치유사(크림 모자·녹색 겉옷·약주머니)와 충분히 구별. OGA16의 정장/장부와 차별화. 128px 얼굴/상의/가방 판독, 40px는 세밀한 표정이 약화되나 갈색 앞치마·가방 실루엣 보존. **전체 및 표적 수정 불필요, KEEP**.
- **OGA09**: 길고 진한 남색 외투, 밝은 머리/두건, 별이 있는 지팡이가 역할을 표현. OGA16 은행 안내인 역시 남색 계열이지만 안경·장부·짧고 정돈된 정장, 지팡이 유무가 달라 128·40px 구분 가능. 40px 세부 표정/지팡이 별 문양 자체는 희미해져도 역할 전체 실루엣 유지. **KEEP**.
- OGA03·09와 OGA06·16 네 명은 굵은 외곽선, 평면 음영, 따뜻한 피부 톤, 둥근 얼굴 비율이 일관. **오프라인 시안 대 승인된 원화의 화풍 비교 PASS**, Unity GameView/월드 실크기 검증 PASS는 아님.
- OGA20 남문경비는 태온 유사성 HOLD이므로 이번 리뷰/비교판에 포함하지 않음. 미엘/태온 원본 변경 0.

## 4. 실제 비교판/ZIP SHA·Manifest

- **원화 4명 동등 기준 시각 비교판 1장:** `Review_OFFLINE/OGA03_OGA09_FinalApproval_Compare_WITH_Approved06_16.png` — OGA03·09 승인 대기 / OGA06·16 승인·로컬 적용·실게임 확인 상태 명시, **각 실제 512 RGBA 원본을 화면 표시 크기로 동일 리사이징**, 128·40px 실축소를 독립적으로 표시. **OFFLINE / not Unity GameView**.
- 추가 40px 집중판 `Review_OFFLINE/OGA03_OGA09_128_40_FocusedReadability.png` — 128px·40px 원래 크기 및 **40px ×6 nearest 확대**를 명확히 구분.
- **기존 후보2 BYTE_KEEP** `Originals_BYTE_KEEP`, **기준 원화2 BYTE_KEEP** `Approved_References_BYTE_KEEP`, `Evidence/QA_Results.json`, `README_KO.md`, `Manifest.json`, `SHA256SUMS.txt` 포함.
- **ZIP:** `LIMITLESS_ISSUE57_OGA03_OGA09_FINAL_VISUAL_REVIEW_20261111.zip`, **606,247 bytes / SHA256 `7d5d4ca4def6e21164c2c318480b428c1cc14f708590ec44c4074579fcd058ab` / 10 entries / CRC PASS / Manifest 8/8 byte count+SHA256 PASS**.
- **전달:** ChatGPT Library `/LIMITLESS_Art_Delivery/Issue57_OGA03_OGA09_FinalReview_20261111/`에 **전체 ZIP·대상 PNG2·승인 비교판1·128/40 집중판1·QA1 총 6파일 업로드 성공**. 사용자 Windows `F:\Downloads` 자동 복사 또는 GitHub PNG/ZIP 바이너리 Push 없음.
- 과거 OGA03/09 구형 게임 PNG 실제 화소 비교/동월드 크기와 512 마스터 Unity 실제 적용 QA는 **NOT_VERIFIED**. Github remote main의 `VillageNpcAppearanceCatalog.cs`에서 `starter-village-general-shop`→`VillageNpcSprites/Eldiran/OGA03_GeneralShop`, `starter-village-main-guide`→`.../OGA09_VillageRepresentative`; `safezone-catacomb-supply`, `arbel-shop`, `arbel-leon` 재사용도 확인. 해당 Resources/Stable ID/기능은 수정 안 함.

## 5. 후속 승인 게이트/기능 보호

- **현재**: OGA03·09 기존 2후보 그림 **사용자 최종 미술 승인 대기**, 즉 `KEEP_RECOMMENDED_2`이지 `USER_ART_APPROVED`는 아님.
- 승인 뒤에도 구형 32px/PPU28·Pivot 및 실제 월드 높이/Collider/이름표·상점 거래/퀘스트 인도/키보드 E·패드·마우스 동작을 **Codex의 별도 읽기전용 실측→격리 Unity6000.6.5f1 DeltaQA**로 확인하고 이후 별도 제품 교체 승인 필요. **512 마스터를 PPU28로 곧바로 Import하지 않음**.
- 구형 NPC, stable ID, 상점 코드, Quest, Collider, Scene, Prefab, Dialogue, Save/UserData, Unity 프로젝트 및 `main` **변경0**. NPC 추가 제작, 대사 초상화, 애니메이션, OGA20 작업0. docs 브랜치 GitHub 문서 커밋 외 제품 변경0, `main` 직접 commit/Push/merge 금지.

**마감:** `OGA03_OGA09_EXISTING_CANDIDATES_REVIEW / SOURCE_BYTE_SHA_MATCH / 128_40_OFFLINE_READABILITY_PASS / USER_VISUAL_APPROVAL_PENDING / NOT_UNITY_APPLIED`.
