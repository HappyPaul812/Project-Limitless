# LIMITLESS / Main24 F2 / Issue #75·#81 — 자연형 그루터기·부러진 나무 독립 후보 1종 (2026-11-03)

**담당 브랜치:** `docs/art-new`  
**판정:** `NATURAL_STUMP_ART_CANDIDATE / OFFLINE_PNG_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_SCALE_AND_ACCESSIBILITY_NOT_VERIFIED / NOT_UNITY_APPLIED`.

## 1. 최신 검수·제작 기준

- [Codex Issue #81 실제 통합 ZIP QA](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6590636705) 및 [기획 후속 결정](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6590736542), [Issue #75](https://github.com/HappyPaul812/Project-Limitless/issues/75#issuecomment-6590647264) 기준.
- 기존 `Sign FULLREDRAW` SHA **`864e964ee4cc3868bd315af2249fe92d04e888937ae66f963232636cda8e4488`** 및 `LowFence FULLREDRAW` SHA **`49c2969b8607d91b6bf15cb0e4485bcc3f25379669064b025d63b099c9c62e8c`**는 `KEEP_CANDIDATE`로 바이트 보존, 정식 교체/Unity 적용0.
- 기존 `F2_SnowStumpPerch_v3_CANDIDATE.png`(국소 수정) 및 `F2_SnowStumpPerch_FULLREDRAW_REFERENCE.png`(과거 전면 대안)는 둘 다 **가구 같은 둥근 원판과 곧은 지주 잔존으로 `ART_HOLD`**. 새 횃대 한 개만 제작하며 기존 후보 원본을 재편집·덮어쓰기하지 않음.
- Codex의 `Main24 81 PASS / 정적 14 PASS`는 기존 Main24 구현 검증 결과로 **그대로 보존**. 이번 제작품의 실제 Unity 기술 PASS가 아니다.

## 2. 실제 제작 파일 및 무결성

- **실물 ZIP 파일명:** `Limitless_F2_Stump_Natural_Issue81_20261103.zip`
- **ZIP SHA256:** `890013d4885319e1dc8a5a7f4ac2771c485b7ed2664617882cf535e6b48b70b5`
- **크기:** **820,391 bytes / 6 entries**, ZIP `testzip()` CRC PASS, 내부 `SHA256SUMS.txt` **5/5 PASS**.
- **전달:** ChatGPT Library 경로 `/LIMITLESS/Main24_Issue81_Stump_Natural_20261103/Limitless_F2_Stump_Natural_Issue81_20261103.zip` **실제 업로드 성공**. 대화 파일 `/mnt/data/Limitless_F2_Stump_Natural_Issue81_20261103.zip`도 생성. 기존 sandbox 다운로드 실패 반복에 따라 Library를 기본 인계 수단으로 사용. **사용자 Windows F:\Downloads 자동 저장/PC 복사본 SHA 확인0**.
- **실제 신규 게임용 후보** `Candidate/F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png` **320×384 투명 RGBA** SHA256 **`22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`** (실 파일 122,413 bytes). 기존 후보와 똑같은 캔버스 픽셀 크기를 유지하되 **새 생성 원화를 축소·투명 가장자리 정리한 한 포즈**이다. v3의 RGB/알파를 국소 수정한 파일은 아니므로 **기존 v3의 연속 메쉬/애니메이션 호환성을 주장하지 않는다**.
- **비교 1** `Preview/Issue81_Stump_Natural_OnePage_Compare_OFFLINE.png` (1410×1080 RGB): `old v3 HOLD` / `prior full-redraw HOLD` / `new natural candidate` 원본 + **실제 PNG에서 크기 너비128/40px LANCZOS 파생**을 한 장으로 배치. 원본 후보 및 신규가 가진 큰 빈 캔버스 상단은 별도 비교 상단에서 보기 편하도록 **미리보기만 투명 bbox 크롭**했으며 **실제 납품 PNG 320×384 원본은 크롭하지 않았다**.
- **비교 2** `Preview/F2_Environment_Style_Wayfinding_Mockup_OFFLINE.png` (1440×650 RGB): 기존 F2 P1 소나무/서리관목과 보존한 Sign/Fence FULLREDRAW 대안을 *읽기 전용 참조*로 사용한, 새 그루터기의 색감·길 가장자리 배치 구상 **OFFLINE 합성**. 실제 F2 Unity/GameView·Collider·F1→F2 길 이음매의 QA 증거가 **아니다**.
- `Manifest.json`은 현재 신규 PNG·Preview 두 장의 파일 SHA256, 크기·모드 및 기존 v3/과거 대안/KEEP Sign·Fence 보호 SHA, `SPEC_PENDING` 표기를 갖는다. `README_KR.md`, `SHA256SUMS.txt` 수록. **신규 후보 PNG 1개만**.

## 3. 미술 변화·정확한 한계

- 새 원화는 눈에 덮인 짧은 넓은 그루터기 옆에 **위로 갈수록 불규칙하게 찢긴 가지·나무 단면과 굽은 잔가지·수피결**을 가진 부러진 나무를 배치. 이전의 **작은 둥근 원판·수직 막대형 가구**를 이 설계에서 제거했다. 같은 첫눈 숲 계열의 갈색 수피·청회색 바위·흰/푸른 적설색을 유지하려고 했다.
- 새로운 포즈는 순수 미술 시안이므로 기존 F2와의 질감·픽셀 밀도, 40px에서 부러진 목질의 인지, 시야차폐/휠체어 길의 안전은 **바울 대표 최종 미술 검토 및 Codex 격리 QA 대기**. 단일 320×384 파일 규격만 확정; `PPU/Pivot/Footprint/Sorting/Collider=SPEC_PENDING`, Unity Scene/Prefab/메타 수정 없음.
- 아트의 기존 11/03 v3 source SHA `eb32bf200da3f08023f0f83b32df9a2f75b9357fa8ffc1f2647b116a5f5b60b8` 및 이전 full redraw SHA `251fad7f5c6271f4f22dab6eeb4389e75864e4ec11380888b267b8892a435af3` **불변**.
- F2 P2 나뭇가지/약한 눈발, P1 GroundOverlay/SnowPine/FrostShrub, 승인 Owl v3 정지 원화 SHA `81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`(Attack `ART_HOLD` 유지), 기존 Main23 환경43/지면9·F1·공식 NPC/펫·Ch1·2 Spider/Bee·Save/UserData/Unity/BuildSettings **수정0**.

## 4. 판정/다음 단계

- **완료한 기술 검증:** 실제 1 PNG 320×384 RGBA/투명 알파 확인, ZIP CRC 및 내부 파일 SHA256 5/5 PASS, 보존된 이전 ZIP에서 v3·전체대안 해시 참조.
- **안 된 검증:** Unity에서 F2 실제 크기/PPU/Pivot/휠체어 접근·동선, GameView 3비율, 기존 #80 81PASS 재검사. `NOT_RUN / NOT_VERIFIED`.
- **기존 공식으로 승격 금지:** Sign/Fence FULLREDRAW는 `KEEP_CANDIDATE` 그대로, 새 Stump는 `ART_CANDIDATE`; 바울 대표의 최종 미술 승인 전 `USER_ART_REVIEW_PENDING / NOT_UNITY_APPLIED`. 보존 자산과 F2 몬스터/본편 수정, `main` 직접 commit/Push/merge 금지.
- **후속 Codex 지시**: 대표님이 실제 원화·128/40 비교를 보고 후보 승인 시, 새 Stump 1개만 격리 Unity에서 화풍/PPU·Pivot·실크기/휠체어 길 차폐·접지/3비율 최소 DeltaQA. 기존 Main24 81/14PASS 전체 재실행 금지. 본게임 적용은 별도 승인.
