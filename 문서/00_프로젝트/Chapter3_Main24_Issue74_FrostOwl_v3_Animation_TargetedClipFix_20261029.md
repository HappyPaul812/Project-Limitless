# LIMITLESS / Main24 / Issue #74 — Frost Owl v3 모션 상단 잘림 최소 수정 (2026-10-29)

**브랜치**: `docs/art-new`  
**결론**: `APPROVED_SOURCE_V3_KEEP / IDLE02_ATTACK02_ALPHA_EDGE_FIXED_OFFLINE / ATTACK_MOTION_ART_HOLD / USER_ANIMATION_REVIEW_PENDING / NOT_UNITY_APPLIED`.

## 근거
- [Codex 최신 통합 읽기 전용 검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6492706191): Idle02 상단 alpha 접촉7px(≥128:2), Attack02 상단 접촉28px(≥128:24) 및 큰깃 잘림. GIF 평균8FPS·나머지 파일·원본 v3 TECH 파일 QA PASS, 신규12프레임 **Unity 런타임 검증은 NOT_RUN**.
- [#74 기획 표적 수정](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6493035987): 원화 디자인고정/바이트보존, Idle02·Attack02만 표적, 정상 10개 수정금지, Attack 준비→발동→복귀 대비 미술검수 필요.
- 이전 원본 `Limitless_FrostOwl_v3_Idle8_Attack4_8FPS.zip` 8,282,224 bytes / SHA256 `0caeeef60f325e1c7b76516b9596daad16f73466b4401968e81fa088101c4eb9`, 실제 입력 무결성 검증. 승인 v3 정적 원화 `FrostOwl_KeyPose_512_v3_CANDIDATE.png`/Idle00 SHA256 **`81a31cf37cb834799ab5c9428da612ab13a5a496025805685daefed7b5d3c467`** 동일.

## 실제 파일 납품
- **ZIP**: `Limitless_FrostOwl_v3_TargetedFrames_20261029.zip`
- **ZIP SHA256**: `d42d4e7e24607e759dfc8de04b807ec8e6a04fc117abbb2a4aa87b27be0b5c1d`
- **크기**: 11,486,616 bytes / **21 entries** / ZIP CRC PASS / 내부 SHA256SUMS **20/20 PASS**.
- 실물 현재 대화 컨테이너 `/mnt/data/Limitless_FrostOwl_v3_TargetedFrames_20261029.zip`에서 제공; ZIP·PNG **GitHub 업로드0**, 사용자의 F:\ 복사/PC SHA 확인0.
- `Frames/Idle/` 8개의 512² RGBA, `Frames/Attack/` 4개의 512² RGBA를 동일 경로로 수록하여 재구성 ZIP의 파일단위 비교 가능. **기존 정상 10개 동일 상대경로 바이트/SHA exact 보존**; 수정 프레임 두 개만 별도 SHA:
  - `Frames/Idle/FrostOwl_v3_Idle_02_512_RGBA.png` **SHA256 `d9e64454812a95dcc05c16b8bb17daf6e28bb61f9a8eacd321a96ec67fc3f7e5`**, 변경 pixels18,646, RGB/alpha 변경 bbox `(22,0)-(216,166)`, **상단 접촉7→0**, 현재 top-alpha0, 최상단 가시 y10.
  - `Frames/Attack/FrostOwl_v3_Attack_02_512_RGBA.png` **SHA256 `6057b42ca0268b237e4930d8d2e1efbd93edddacf748700fc87e032e321d6064`**, 변경 pixels20,433 bbox `(22,0)-(217,168)`, **상단 접촉28→0**, 현재 top-alpha0, 최상단 가시 y10.
- 방법: 동일 승인 v3의 **정상 Idle01 상부 날개깃** 픽셀을 참조해 *결함이 있던 두 프레임의 상단 왼쪽 날개부*에만 페더 보간/형상 복원. 전신 일괄축소·캔버스 증가·별도 얼굴/날개 원화 재설계/투명여백 전체 편집 없음. 깨끗한 feather donor 기반이므로 **개별 프레임 간 그라데이션 겹침·관절 자연스러움은 사용자의 모션 미술 검토 필요**.
- 동일 8FPS 제작 규격: **512×512 RGBA, Import Target PPU512 / Pivot(0.5,0.5) / Scale1.0 / 8FPS**. PNG에는 Unity importer 값 내장하지 않으며 실제 `.meta` 작성0.
- 재생시트 Idle `2048×1024`, Attack `2048×512`; 수정된 정확한 12 PNG를 사용해 시트 재생성. GIF Idle8·Attack4와 각 GIF의 **BEFORE_AFTER 2열 비교** 제공. GIF frame duration **120ms/130ms 교차 → 주기 평균125ms = 8FPS**, gif frame counts 8·4 PASS.
- `Preview/OnePage_Owl_BeforeAfter_OFFLINE.png`: 실제 Idle02/Attack02 수정 전·후 512 기반 비교와 각각 **128/40 실제 LANCZOS 파생**. GIF 4개/시트2개/프레임12/Manifest/SHA 포함.

## 미결·승인
- **상단 잘림 파일 결함 해소는 OFFLINE_ALPHA_EDGE_PASS**. 단순 새 원화 정적 v3의 Unity TECH PASS가 새 프레임12의 전투 런타임 PASS는 아님.
- **Attack 준비→발동→복귀 대비의 사용자 체감 개선**은 프레임02 상부 깃 복원만으로 완전 입증되지 않아 **`ATTACK_MOTION_ART_REVIEW/HOLD`** 유지. GIF 실재 재생·128/40 판독성 보드 제공, 원화와 정상 프레임을 건드리지 않는 범위 때문에 새 공격포즈 대규모 작화0. 바울 대표 결정 전 모션 최종 승인 또는 `UNITY_APPLY_READY` 주장 금지.
- Codex는 승인 후 **Idle02·Attack02 alpha/clip 및 두 시트/GIF·8FPS 전투 UI·Attack pose 구분만 Delta QA**, 정적 v3의 기존81+22/기타 전체 QA 반복하지 않음. Hit/Defeat/Spawn/SkillVFX/BattleEffect·MonsterDefinition/Scene/Save/UserData/Unity 본편·기존F2/F1·Main23 아트·다른 담당 자산 변경0. main 직접 commit/Push/merge0.

**상태**: `ART_CANDIDATE / OFFLINE_FILE_QA_PASS / USER_ANIMATION_REVIEW_PENDING / UNITY_ANIMATION_NOT_VERIFIED / NOT_UNITY_APPLIED`.
