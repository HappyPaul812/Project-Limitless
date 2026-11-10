# LIMITLESS / 리뉴얼 아트 / Issue #95 — 시작 마을 OGA06·OGA16 최종 미술 승인 검토 자료
**검토일:** 2026-11-11 (KST)  
**브랜치:** `docs/art-renewal`  
**판정:** `OGA06_OGA16_EXISTING_512_BYTE_KEEP / OFFLINE_128_40_READABILITY_PASS / LIVE_ORIGINAL_1TO1_COMPARE_PENDING / MIEL_ENV_REFERENCE_PENDING / USER_FINAL_ART_REVIEW_PENDING / UNITY_NOT_APPLIED`

## 1. 정본·최신 총괄 확인
- GitHub [Issue #95](https://github.com/HappyPaul812/Project-Limitless/issues/95), [Issue #57](https://github.com/HappyPaul812/Project-Limitless/issues/57), [2026-11-11 기획 최종 시각검토 지시](https://github.com/HappyPaul812/Project-Limitless/issues/95#issuecomment-6781033231), `WORLD_ART_RENEWAL_LATEST.md`, `COMMON_RULES.md` 참조.
- 기존 최종 납품 `LIMITLESS_ELDIRAN_NPC_OGA06_OGA16_ORIGINAL_PILOTS_20261110.zip` **569,127 bytes / SHA256 `89088854c3965299a206b9123e56c9ae072ced885731083e37a2347413e6c885` / ZIP CRC PASS**를 실제 오픈.
- **이번 회차 신규 NPC PNG 제작·수정·재인코딩 0.** 512² 원본 후보 2장과 과거 OGA03/OGA09 참조용 후보 2장 전부 *바이트 그대로* 복사해서 비교에만 사용. Portrait/Animation/추가 NPC 0.

## 2. 이미 제작된 후보 보존 확인

| NPC | 원본 납품 PNG | 512 RGBA SHA256 | 신규 제작 |
|---|---|---|---|
| OGA06 치유사 | `Candidates/OGA06_Healer_Original_512_RGBA.png` | **`67aef89677e9b9f174f30b2c2a9e854eaee4e61814774402bd94a18691bd401f`** | 0 / BYTE_KEEP |
| OGA16 은행 안내인 | `Candidates/OGA16_Bank_Original_512_RGBA.png` | **`1199c57d3c279b51af83452c8b580814d6456417ea8e3f3b83ea9fdc6c5f8dcd`** | 0 / BYTE_KEEP |

과거 제작 참고용 OGA03 상인 SHA `09b2d97643a7aaefd51930eb374c48bf358020f015cc4cce9e7ef4903853151d`, OGA09 주민대표 SHA `ca0336ab6bbc7aaeb16169037e73ee39ced1b674f48324baeb75b1debfa29fcf` 역시 원형 유지. **OGA03·OGA09는 공식 승인 완료 아트로 오인하지 말고 과거 미승인 시안**임을 명시.

## 3. 검수 산출물 — 실제 이미지·미검증을 엄격 구분

1. **`Review_OFFLINE/OGA06_OGA16_LiveOriginal_vs_New_CANDIDATE_ORIGINAL_UNAVAILABLE.png`**: 왼쪽은 **기존 게임 원본 이미지 대신** GitHub에 존재하는 파일의 경로/Blob SHA·바이트수 및 `UNAVAILABLE`를 표시하고, 오른쪽은 신규 512² 후보를 실제 파일에서 표시한 **상태 보드**다. **요청한 기존 Unity NPC 원본과 신규 후보의 이미지 동배율 1:1 검사는 아직 NOT_VERIFIED**. 빈 칸을 임의 픽셀 그림·서로 다른 캐릭터로 위장하지 않음.
   - 기존 게임 OGA06 경로 `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA06_Healer.png`, Git blob SHA `736bffc38e828467dc3c10f77a664f0778ed2a48` (916 bytes).
   - 기존 게임 OGA16 경로 `Unity/Client/Assets/_Project/Resources/VillageNpcSprites/Eldiran/OGA16_Bank.png`, Git blob SHA `4ce0874b038688f83b28715228e66eded60a5b7b` (786 bytes).
   - 실제 원격 디렉터리 메타데이터를 확인했으나 GitHub 텍스트 연결은 PNG 바이너리에 **UnicodeDecodeError/비-UTF8 응답 거부**하여 원본 화소를 확보하지 못함. 이것은 에셋 미존재나 아트 오류 증거가 아님. 후속 Codex 읽기전용 **실제 원본 확보·동월드 배율 대조**가 필요.
2. **`Review_OFFLINE/OGA06_OGA16_128_40_TRUE_SCALE.png`**: 기존 후보 512 전체 캔버스를 동일 방식으로 **128·40** 실축소. 인물 실루엣/기본 복장 구별. **40px에서 작은 약주머니·장부 글자는 단독 인지 어려울 수 있어** 역할은 크림 모자·녹색 외투 대 남색 정장/장부 형태/안경의 복합 단서로 전달. 글자·초상화/말풍선/대화 UI 제작 안 함.
3. **`Review_OFFLINE/OGA03_09_06_16_Style_FourCharacter_REAL_FILES.png`**: 지난 미승인 OGA03/09 **실물 PNG**와 두 신규 후보를 동일 512 캔버스에 나란히 비교. 얼굴/외곽선/몸통 비율, 단순 평면 음영과 따뜻한 악센트가 대체로 유사함. OGA16 남색 계열은 OGA09와 유사하지만 주민대표의 지팡이·두건 대 은행 안내인의 안경·정장·장부로 구별; OGA03의 앞치마·가방과 OGA16의 정장/장부 구별됨. **공식 승인 StarterVillage 환경 PNG 실물과의 화소 비교는 이번 작업에 없으므로 '환경 화풍 1:1 PASS' 주장하지 않음**.
4. **`Review_OFFLINE/OGA06_OGA16_40px_Nearest6x_Detail.png`**: 원래 40px 및 같은 40px를 6배 **nearest 확대**하여 작은 비주얼 판독.
5. **`Review_OFFLINE/OGA06_OGA16_BrightDarkVillage_OLDER_OFFLINE_KEEP.png`**: 기존 납품과 동일한 단색 밝음/어두움/마을색 참고 모형(승인된 실제 Eldiran Scene 스크린샷 아님).
6. **미엘 공식 캐릭터** 원본 초상화 또는 전신 공식 원화를 이번 작업공간에서 확보하지 못함. OGA06이 성인 남성 정지 안내인이고 미엘과 다른 역할/복장/머리로 설계됐다는 제작 의도는 있으나 **실제 미엘 원화와의 1:1 외형 유사성 PASS는 보류**.

## 4. Unity 계약과 미술 승인/교체 경계
- 기존 `OGA06_Healer.png.meta` Git blob `5e59a7eef3233900ca329a2b112a5354cb4543db`, `OGA16_Bank.png.meta` Git blob `0d49b6bd9cc94b19132fe047c89fdd9a625c2906`. 기존 문서/메타상 **PPU28, Sprite Pivot(0.5,0), Point Filter**; `starter-village-healer`/`starter-village-bank` stable ID 유지. OGA06의 다른 씬 재사용 `safezone-catacomb-healer`, `arbel-healer`; OGA16 `arbel-pet-management` 재사용도 고려.
- PPU28은 이미지 픽셀 단위 길이를 월드단위로 바꾸므로 **512 마스터를 그대로 Import하면 기존 NPC 표시 크기·접지가 달라질 수 있다.** 기존 원본의 실제 픽셀 치수, 이름표 위치/Collider/월드 높이와 512 마스터의 추후 게임용 다운스케일 크기/PPU/Pivot을 Codex가 **원본 읽기전용으로** 확인한 뒤에만 게임용 Sprite 별도 생성/격리 Unity 6000.6.5f1 표시 QA 가능. **현재 게임용 임포트 완료 상태가 아님**.
- **현재 최종 미술 권고:** 두 신규 후보 모두 **KEEP/NO_ART_CHANGE_RECOMMENDED**, 추가 캐릭터 제작·수정 0. 그러나 바울 대표 **최종 미술 승인은 별도 필요**하고 위 1:1 원본/미엘/환경 시각 비교 제한도 안내해야 함.
- 기존 Unity/Scene/Prefab/StableID·Quest/Collider/Nameplate/상호작용/Save/UserData/`main`·기존 공식 몬스터/펫·승인 환경 **변경0**. Github `docs/art-renewal` 문서 커밋 외 원본 소스 프로젝트 변경0. **main 직접 commit / Push / merge 0**.

## 5. 실제 ZIP·해시·전달
- **ZIP:** `LIMITLESS_ISSUE95_OGA06_OGA16_FINAL_ART_REVIEW_20261111.zip`, **745,547 bytes / SHA256 `c382592ac6857a47b6631e8ebdd4ff2ea87835eaf0ef4292ab5f3c77c8a90cb4` / 18 entries / ZIP CRC PASS / Manifest 16/16 byte length + SHA256 PASS**.
- 신규 원화 PNG **0장**, 기존 512 PNG byte KEEP 2장, OGA03/09 과거 후보 2장 byte KEEP, 실제 비교·상태 보드 5장/128·40 실제 축소 PNG4, QA JSON·원격 원본 Git blob 메타데이터·`README_KO.md`·SHA256SUMS 포함.
- ChatGPT Library `/LIMITLESS_Art_Delivery/Issue95_FinalReview_20261111/`에 ZIP·원본2·상태보드·실축소·화풍 4인 비교·40px 확대·README 합계 **8개 파일 업로드 성공**. 사용자 Windows F: 자동 복사, 원격 GitHub PNG/ZIP 바이너리 Push는 미실행.

## 6. 완료/추가 검증 요건
**현재 종료 상태:** `OGA06_OGA16_EXISTING_MASTER_BYTE_KEEP / OFFLINE_128_40_READABILITY_REVIEW_COMPLETE / EXISTING_LIVE_NPC_BINARY_NOT_ACQUIRED / USER_FINAL_ART_REVIEW_PENDING / NO_UNITY_APPLY`.

**최종 승인 전 미해결:** 현행 게임 원본 2종의 정확한 화소 동배율 비교, 공식 미엘/승인 환경 화풍 1:1 대조, 실제 화면 PPU28·하단 Pivot 물리/월드 크기. 사용자 미술 승인 후에도 원본 Unity 직접 적용 금지, 별도 Codex 격리 QA/적용 승인 필요.
