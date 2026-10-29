# LIMITLESS / Chapter1·2 Organic Forest Spider — 배경 알파 표적수정 v2 (2026-10-29)

**Issue:** [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60). **작업 브랜치:** `docs/art-renewal`. **참조 정본:** `main` `bfd281545632877325f5d21e04cefb924042c465`, [COMMON_RULES](팀운영/COMMON_RULES.md), [WORLD_ART_RENEWAL_LATEST](팀운영/인계/WORLD_ART_RENEWAL_LATEST.md), [#60 Codex 통합검수](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6492718484), [#60 최신 기획 표적수정 계약](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6493038497).

## 결론

**`OFFLINE_PNG_TECH_PASS / ALPHA_MATTE_TARGETED_FIX / USER_FINAL_DESIGN_REVIEW_PENDING / UNITY_NOT_TESTED / SPIDER_32FRAMES_HOLD / MAIN_UNCHANGED`.**

바울 대표의 Chapter1·2 숲거미 유기형 디자인은 변경하지 않았다. 기존 `ForestSpider_Organic_v1_512_RGBA_CANDIDATE.png`와 픽셀 단위 비교 시 **RGB 512×512 전체 262,144픽셀 100% 동일**, 배경 분리에 필요한 **알파만 감소 32,433픽셀 / 증가0**. 이는 구조·색상·텍스처 전면 재제작이 아닌 **회백색 배경 쐐기·갈색 지면 매트·잔상 제거**라는 한정 수정. 머리·갑각·8다리·이끼/털/거미줄 원화 RGB 픽셀은 보존. 단, 극세선의 알파는 별도 미술/Unity 시인성 검토가 필요하다.

## 1. 실제 파일 납품 및 SHA

- 기존 원본(바이트 불변): `Original_KEEP/ForestSpider_Organic_v1_512_RGBA_UNCHANGED.png` **512×512 RGBA / SHA256 `692ee2a520c661f0ece5d9ba955d7de1ed32f3a44ed1fdc38d9d934b9e61e3c1`** (기존 #60 기록과 일치).
- 신규 알파 정리본: `Candidate/ForestSpider_Organic_v2_CLEAN_ALPHA_512_RGBA.png` **512×512 RGBA / SHA256 `c949f37ad5511753b064b83d521e79bb76409b0427cd0abf0c9742d6d9d46661`**.
- 정비 비교: `Review/Original_vs_Clean_512_128_40_OFFLINE.png`: v1/v2 실제 PNG의 512·128·40px 동일 캔버스 LANCZOS 축소, 기존/수정 투명 배경 비교.
- 3배경 합성: `Review/Three_Backgrounds_AlphaQA_OFFLINE.png`: 밝은 중립 배경, 어두운 중립 배경, **절차적으로 합성한 가상 숲바닥**에 대한 v1/v2 비교. 실제 Unity/Field02·03 GameView/공식 환경 캡처 **아님**. 배경별 512px 합성 PNG 6장 포함.
- 별도 128×128/40×40 원본·수정 PNG는 각각 `Candidate`/`Review` 폴더에 분리. 일괄 픽셀 확대/왜곡 없음.
- 납품 ZIP: **`LIMITLESS_OrganicSpider_AlphaFix.zip` / 3,118,909 bytes / SHA256 `716a7f800a2cd43fa316eb280607b614112a818637aaed086a73f6e91b0372bf` / 18 entries / ZIP CRC PASS**. `Manifest.json` 페이로드 16개 파일별 bytes/SHA256, `SHA256SUMS.txt` 파일목록 17개, `QA/QA_Results.json` 비교 수치.
- 이 ZIP은 **현재 ChatGPT 컨테이너 `/mnt/data`의 실제 파일**. Windows `F:\Downloads` 저장을 자동 수행한 것이 아니며 원본 Unity/원격 GitHub에 PNG·ZIP 바이너리 업로드하지 않았다.

## 2. 수정 방법·실측 검증·남은 한계

- 새로운 투명 아트 레퍼런스의 알파 형태를 구형 512 후보의 원화와 SIFT/RANSAC 정합하여 거친 지면만 제거하고, 좌측 다리 사이 밝은 회백색 쐐기·오른쪽 배경 잔상 영역을 부가 분리했다.
- 최종 RGB는 신규 렌더링 이미지를 사용하지 않고 **원래 v1 PNG의 RGB 배열을 그대로 유지**했다. 오직 기존 투명도보다 더 낮은 alpha만 적용, 알파 범위 0..255 / 모서리 alpha=0. 절차 설명은 작업 재현 기록이며 어떤 신규 그림도 게임 본편에 적용하지 않았다.
- 원본 RGBA bbox / 수정 bbox, alpha변경 픽셀 수 및 RGB 차이 0을 `QA_Results.json`에 기록. ZIP 전체를 생성 후 재개봉하여 CRC 이상 없음을 확인.
- **정적 QA:** 투명 PNG 디코딩/치수·원본 RGB/원본 해시 보존·알파 감소·512/128/40 축소·3배경 비교 자료 존재 `PASS`.
- **미술/Runtime 검수:** 아주 가는 거미줄/털 끝과 오른쪽 다리 끝의 알파 경계는 512px 확대 확인을 계속 권장. 실제 Unity `PPU256`, `visualScale=.7`, FrameLoader, Field02/03 카메라/배경·실전 40px 시인성은 **NOT_TESTED**. `USER_FINAL_DESIGN_APPROVAL_PENDING`으로 기록. **`PRODUCTION_READY` 또는 `FINAL_ART_PASS`로 표기 금지**.

## 3. 범위 보호·후속 게이트

- **기존 메카닉 Spider v3는 Chapter4 `CH4_MECHANIC_ARCHIVE_ONLY`**. 기존 유기형 v1과 메카닉 v1/v2/v3 각각의 원화·SHA·ZIP은 변경0; 기존 모든 공식 승인 몬스터/환경과 펫 Wolf·Bear·Fox 변경0.
- 게임의 외부 `ethanf30games 2D Spider` CC0 Idle8/Walk8/Attack8/Shoot8 **총32 PNG/10FPS**, MonsterDefinition·Sprite GUID/meta·Field02/03 Spawn·AI·독·드롭·Quest·Save/UserData·Unity Scene/Prefab/Editor/원본 모두 **불변**. 새 32프레임 제작0/32, 본게임 적용0.
- VenomBee의 오리지널 v1 및 기존 Idle10/Attack10 **20프레임**, 독립 5키포즈/GIF·SHA 보존. PilotBee OpenGameArt CC BY3.0 vs 내부 LICENSE CC BY4.0 불일치 **미해결 유지**. 이번 회차 신규 Bee 아트0.
- **다음은 바울 대표의 유기형 디자인 최종 미술 검토** → 필요시 알파 프린지 국소 보완 → Codex 격리 동일 PPU·40px 실제 GameView QA → 별도 승인이 있은 뒤에만 Spider 32프레임 제작/본편 적용. `main` 직접 Commit/Push/Merge 금지.

**이번 결과는 독립된 '알파 수정 후보'이지 원본 게임 자산 교체 승인이 아니다.**
