# LIMITLESS / 2026-10-29 — Chapter1·2 유기형 숲거미 후보 및 Spider v3 Chapter4 기계형 재분류

**바울 대표 최신 확정 결정 — Chapter 1·2 forest_spider와 Chapter 4 메카닉 Spider를 분리.**

브랜치 `docs/art-renewal`. [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60) · [Issue #63](https://github.com/HappyPaul812/Project-Limitless/issues/63) 및 2026-10-29 [Codex Spider v3/Bee QA](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6490544447)와 기존 [10월 29일 제작 보고](Renewal_20261029_ForestSpider_v3_VenomBee_FivePose_Redraw_Delivery.md) 대조. 최신 common rules/담당/인계 문서 확인.

## A. 상태 변경 — 승인 판단과 자산 분류를 구분

- **현재 Spider v3** `Spider_v3_512_RGBA_UNCHANGED_CH4_FUTURE.png`은 현재 Chapter1·2 `forest_spider` **후보에서 철회**. **Chapter4용 메카닉 스파이더 미래 후보 `CH4_MECHANIC_ARCHIVE_ONLY`**로 재분류. Chapter4 아트 제작·세부 색상 조정·애니메이션·Unity 적용을 이 담당이 지금 시작하지 않음. Chapter3+ 신규 아트 부서로 향후 제안만 가능. 승인된 공식 Chapter4 자산은 아님.
- 앞선 `v1/v2/v3` 버전 원본을 각각 byte-identical reference로 신규 ZIP에 보존: v1 SHA256 `9a80f18c3793daa17aa2586c0effd19f9ecb72beaf654d70ba8f0af3392fb32e`, v2 SHA256 `3dedf30e2d5e95e299016980bec1c31456f4c3c796cd2222da948eeab92784ca`, v3 SHA256 `d7ad4062eea84d6520e672b46630a26cfa92f54e55d587d1da936ac33bc50fc4`. v1/v2는 역사적 검토 원본, v3는 Chapter4 분류 보존용. 기존 ZIP과 SHA도 그대로.
- 기존 Chapter1·2 게임의 외부 `ethanf30games / 2D Spider` CC0 전투 애니메이션 32장 Idle8/Walk8/Attack8/Shoot8, 10FPS, 우향, PPU256, visualScale.7 및 Field02·03 Spawn은 **임시 유지**. 새 Chapter1·2 오리지널 디자인의 32프레임은 **0/32**, 런타임 교체는 `HOLD`.

## B. 새 Chapter1·2 유기형 `forest_spider` 1포즈

- **실물** `Candidate_Chapter12/ForestSpider_Organic_v1_512_RGBA_CANDIDATE.png`, **512×512 RGBA**, 새 그늘 숲/독니/거미줄·이끼갑각/유기 다리·여러 주황 눈을 가진 별도 디자인. PNG SHA256 **`692ee2a520c661f0ece5d9ba955d7de1ed32f3a44ed1fdc38d9d934b9e61e3c1`**.
- `Candidate_Chapter12/ForestSpider_Organic_v1_Preview_128.png`, `...Preview_40.png`은 동일 마스터를 실제 128×128/40×40으로 축소. 확인용 `Evidence/Chapter12_Organic_vs_Chapter4_Mechanic_512_128_40_OFFLINE.png`는 신규 유기형 vs 기존 메카닉 512·128·40 비교로, 실제 PNG 픽셀로 제작한 **오프라인** 자료. Unity GameView/동PPU/전투 크기 PASS를 주장하지 않음.
- **제작 방식과 제한:** 신규 유기형 원본 이미지가 포함된 `Evidence/OrganicConcept_SourceBoard_NOT_SPRITE.png`는 **RGB 디자인 보드**로 Sprite 아님. 새 유기형 단일 PNG는 이 보드에서 OpenCV GrabCut의 전경 분리 및 리샘플링으로 추출한 **투명 RGBA 콘셉트 후보**임. 이미지 자체는 외부 Spider를 트레이스·색변경한 것이 아니지만, 마스크에 **다리 및 땅 사이 숲 배경·바닥 잔상이 남아 있다. `ALPHA_MATTE_REVISE` / 정식 Unity-ready PNG가 아님**. alpha 영역 bbox (11,102)-(501,410), 완전 투명 pixel 153,340, 부분 alpha pixel 7,801 (512×512 기준). 실제 이미지 시각 승인 전 보정해야 한다.
- Chapter1·2 몬스터의 40px 적 실루엣은 *초안에서* 대체로 식별되나, 화풍 일치·다리 명확성/알파 프린지/현행 몬스터 GameView 검수는 **`USER_DESIGN_REVIEW_PENDING / OFFLINE_ONLY`**. 새 디자인은 Chapter4 메카닉 v3를 덧칠하거나 비슷하게 만든 것이 아니다.

## C. 전달 ZIP·실제 무결성

- `Limitless_ForestSpider_OrganicCh12_MechanicCh4_Handoff_20261029.zip` — **4,184,421 bytes**, **14 files**, 실제 ZIP CRC PASS, 압축 내부 페이로드 Manifest **12/12 SHA256 PASS**, SHA256 전체 **`4cb287994e069bd2f99b0fcff7517500fd504ce6b9ba13d16b0258f3bca68234`**.
- ZIP 내부 `Evidence/Manifest.json` 파일별 byte/SHA256 기록, `Evidence/SHA256SUMS.txt`, `Evidence/QA_Results.json`, `Evidence/Review_QA_and_Classification.md`, 후보 PNG·원본보드·비교판·v1/v2/v3 **원본 바이너리** 보존. ZIP·PNG는 이 ChatGPT 작업 컨테이너에만 납품하여 링크 제공. Windows 사용자 `F:\Downloads` 자동 저장 또는 GitHub 바이너리 Push를 주장하지 않음.
- **정확한 검수:** `ZIP_CRC_PASS / FILE_HASH_PASS / 512_RGBA_PASS / ORGANIC_ART_REVIEW / ALPHA_MATTE_REVISE / CH4_ARCHIVE_ONLY / UNITY_NOT_TESTED / NO_MAIN_MERGE`.

## D. VenomBee 보존·부서 간 인계

- VenomBee 기존 오리지널 1포즈 마스터, `Idle10+Attack10` **20개 오프라인 프레임**, 최신 별도 `Idle2+Attack3` 5개 키포즈, 128/40/GIF/기존 ZIP 및 모든 실제 SHA **그대로 보호/재제작 0**.
- Codex #60·#63 QA: 새 키포즈 방향은 개선됐으나 **날개 뿌리·6다리·머리/복부 기준점·독침 축(머리 우향/찔러 나가는 침 좌향)·알파/타격 흐름**이 미완으로 `MOTION_REVISE_CONTINUE / RUNTIME_HOLD / NOT_REPLACEMENT_READY`. 이번 숲거미 방향 전환으로 이 판단을 되돌리지 않음.
- 원본 `Jana Ochse / 2D!PIXX PilotBee`의 **OGA CC BY3.0 vs 원본 패키지 CC BY4.0 충돌 미해결**, Credit/License 보존.
- **기존 Ch2 공식 몬스터·보스, Wolf/Bear/Fox, 환경·NPC 승인 자산, 사용자 Unity/Scene/Prefab, MonsterDefinition·Sprite GUID/meta·Spawn/AI·독·드롭·전투, Quest·Save/UserData·main 전부 미변경**.

## E. 다음 단계와 승인

1. 바울 대표는 이번 Chapter1·2 **유기형 1포즈 외형**에 대해 `승인 / 국소 수정 / 반려` 시각 결정만 하며, 아직 승인으로 처리하지 않는다.
2. 승인 방향이어도 바닥색/알파 마스크 보정이 필요하고, Codex 격리 동일 PPU·40/128px/Chapter2 공식 비교 및 전투 가독성 QA 후 별도 승인까지 **새 Spider 32프레임 제작 및 Unity 본편 적용 금지**.
3. Chapter4 메카닉 v3는 보관만. 추후 Chapter3+ 담당에서 필요 시 정식 발주로 검토 가능.
4. VenomBee는 이전 20장과 5키포즈 보존, 별도 후속 아트 계약에서 **자연 관절 동작·독침 방향**만 정정. 이번 회차 신규 Bee 아트0.
