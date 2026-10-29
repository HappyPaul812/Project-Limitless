# LIMITLESS / Main24 / Issue #74 — Frost Owl v3 Idle 8 + Attack 4 animation pilot

**작업일:** 2026-10-29 KST  
**정식 상태:** `OWL_V3_SOURCE_APPROVED / CODEX_PREVIOUS_STATIC_TECH_PASS / ANIMATION_OFFLINE_FILE_QA_PASS / USER_MOTION_ART_REVIEW_PENDING / ANIMATION_UNITY_NOT_VERIFIED / NOT_UNITY_APPLIED`  
**브랜치:** `docs/art-new`

## 1. 최신 계약 및 근거

- [#74 바울 대표 v3 외형 승인](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6489269398)와 [Codex #74 격리 Unity 원화 기술 검증·계약](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6490161019). 기준 원본 `FrostOwl_KeyPose_512_v3_CANDIDATE.png` SHA256 **`81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`** 실물 재검증 PASS·바이트 보존. v1/v2 원본 재제작·편집0, v4 금지.
- **Codex의 기존 Unity TECH_PASS는 오직 512² 정적 v3 한 포즈 표시검사**(설치 에디터 Unity 6000.6.5f1, 기준 PPU512/Pivot center/Scale1, 1280×720/960×720/1680×720 격리 캡처). 현재 새 12프레임의 실제 Battle Scene·몬스터 로더·전투 애니메이션 TECH_PASS가 **아님**. 본 회차는 아트 제작·오프라인 파일 검증만.
- 프레임 계약 **512×512 RGBA / 512 PPU / Pivot(0.5,0.5) / VisualScale 1.0 / 공통 8FPS**. PNG에는 PPU·Pivot을 내장하지 못하므로 `Manifest.json`에 **Unity import 제안 계약만 기록**, 실제 신규 `.meta` 생성·Unity Runtime 적용0.

## 2. 실제 애니메이션 납품 — 별도 ZIP

- **실제 ZIP:** `Limitless_FrostOwl_v3_Idle8_Attack4_8FPS.zip`
- **ZIP SHA256:** `0caeeef60f325e1c7b76516b9596daad16f73466b4401968e81fa088101c4eb9`
- **크기:** 8,282,224 bytes / 24 entries / ZIP CRC PASS / `SHA256SUMS.txt` **23/23 PASS**.
- ChatGPT 현재 대화 다운로드 아티팩트: `/mnt/data/Limitless_FrostOwl_v3_Idle8_Attack4_8FPS.zip`. GitHub에는 **문서만 커밋**, PNG/ZIP 바이너리 업로드0. 사용자 PC `F:\Downloads` 수령/파일 해시 직접 검증은 현재까지 미실시.
- **Idle 8장 실 PNG**: `Frames/Idle/FrostOwl_v3_Idle_00_512_RGBA.png` … `_07_512_RGBA.png`; 크기 모두 512×512 RGBA. **Idle00은 승인된 v3 원본과 PNG 바이트/SHA exact 동일**, 다른 7장은 각각 다른 PNG SHA·픽셀 값.
- **Attack 4장 실 PNG**: `Frames/Attack/FrostOwl_v3_Attack_00_512_RGBA.png` … `_03_512_RGBA.png`; 모두 512×512 RGBA, 전부 서로 다른 SHA. 독립 파일 기반 4개 준비/발동/마무리 후보.
- **시트 2개**: `Sheets/FrostOwl_v3_Idle_8FPS_RGBA_Sheet.png` (2048×1024, 4×2), `Sheets/FrostOwl_v3_Attack_8FPS_RGBA_Sheet.png` (2048×512, 4×1). 시트는 동일 프레임 파일의 단순 격자 결합.
- **GIF 2개**: `Preview/FrostOwl_v3_Idle_8FPS.gif` (8프레임), `Preview/FrostOwl_v3_Attack_8FPS.gif` (4프레임). GIF 센티초 단위 한계 때문에 개별 표시시간 **120/130ms 번갈아 사용**, 한 주기 평균 프레임시간125ms = **8.000 FPS**. GIF는 남색 배경의 보기용이며 **Unity 게임 투명 렌더/모션 TECH_PASS 아님**.
- 128/40 실제 LANCZOS 축소 4비교판과 `Preview/OnePage_Owl_Animation_Review.png`, `Manifest.json`, `README_KR.md`, `SHA256SUMS.txt` 포함. 개별 프레임·시트·GIF의 **정확한 전체 SHA256**은 `Manifest.json.artwork_files` 및 `SHA256SUMS.txt`에 실제 파일별 수록.

## 3. 제작 방식 / 기술적·미술적 제한

- 원본의 청회색 날개·얼굴·부리·꼬리 형태·기본 자세를 바꾸어 새 캐릭터로 그리지 않았다. v3 RGBA 원본을 바탕으로 부위별 변위 필드에 의한 원형 보존 모션: 좌우 날개 바깥쪽은 지면 위 플랩, 몸통은 완만한 부유, 꼬리는 별도 늦은 왕복. **8 Idle / 4 Attack 전부 픽셀 차이 있는 실제 모션 프레임**.
- Idle은 한 원주기에 두 번의 완만한 상승·하강을 주고, Attack은 날개를 내려 냉기를 모으는 듯한 준비 → 날개를 크게 들어 올리는 방출 → 회복 후보. **별도 냉기 SkillVFX·SpawnVFX·파티클·발사체 생성0**.
- 독립 새 프레임은 원본을 픽셀 재사용·리샘플링하여 변형했으므로 실루엣이 프레임마다 움직이는 건 자연스러운 애니메이션 특성이다. **'새프레임 전부 원본 alpha-mask exact 동일'을 주장하지 않는다**. 반면 원본 v3 파일 바이트는 그대로 동결.
- 이 시안은 **구역별 이미지 변형 기반 첫 모션 파일럿**: 회전시 날개 관절의 실제 독립 작화·공격 anticipation/impact·꼬리의 물리적 추종/40px 게임 UX가 사용자가 만족할 만큼 분명한지는 **미술 검수 필요**. 기존 Codex 정적 원화 TECH PASS를 이 애니메이션 12프레임의 TECH PASS로 자동 승격 금지.
- Idle/Attack 각각 모든 PNG decode·모드·크기 PASS; 8/8 Idle·4/4 Attack SHA 고유, ZIP CRC·SHA23/23 PASS. PNG PPU512·Pivot 중앙은 문서화한 import 계약일 뿐 실제 Scene/Prefab/Sorting/UI 테스트 **NOT_RUN**.

## 4. 보호 및 남은 게이트

- **Hit/Defeat/Spawn/Skill VFX/Battle effect 0**, MonsterDefinition/MonsterID/Scene/Save/UserData/TTS/Quests/GUI/Unity 에디터 메타/원본`main` 수정0.
- F2 승인 후보·P1·P2 원본·Main23 승인43 및 지면9·F1·서리들쥐·Chapter1·2 리뉴얼 숲거미/독침벌 및 Wolf/Bear/Fox 보호. Owl v3 원본 SHA `81a31cf3...` 단일 프레임 기준.
- **바울 대표가 애니메이션의 실제 움직임 및 공격 4단계 미술을 새로 승인해야 함**(`USER_MOTION_ART_REVIEW_PENDING`); 승인 후 Codex 별도 격리 Unity에서 8FPS, 프레임 정렬/클리핑/전투 UI/128·40 보임, 타격 연출·기존 Hit routing 간섭 검증. 사용자 별도 본편 적용 승인 전 **`NOT_UNITY_APPLIED`, main 직접 Push/병합 없음**.

