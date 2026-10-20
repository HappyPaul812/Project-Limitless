# LIMITLESS — Chapter3 서리들쥐 A·설빙의 폭군 B Idle v1 격리 Unity 14 PASS / 최종 모션 승인 대기 (2026-10-20)

- 기획 브랜치 `docs/planning`; 신규 아트 담당 `docs/art-new`; GitHub [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30).
- 근거: 바울 대표가 제공한 **Codex 검수 스크린샷**. 본 기획 세션은 사용자 로컬 `F:\Downloads\Limitless_Chapter3_FrostFieldMouse_FrostTyrant_Idle_v1.zip`과 Codex 격리 GameView/동영상을 직접 재생하거나 독립 검증하지 않음.
- Codex 검수: **구조/기술 PASS, 서리들쥐A 미술·모션 PASS 권고, 설빙의 폭군B 미술·모션 PASS 권고.** 격리 Unity14PASS/0FAIL, 실제 루프 약0.500초, 승인 1포즈가 Idle 첫 프레임과 픽셀 동일, 발 접지 고정·Sprite 경계 잘림 없음, 프레임03→00 자연스러운 연결. 서리들쥐: 야생성·꼬리·앞니·발톱·눈 유지, 128px 작은 움직임 적절. 폭군: 흉곽/어깨/긴 팔 호흡 동작과 보스 체급 유지.
- Codex 브랜치 `codex/chapter3-idle-v1-review-20261020`, QA commit `1b9ad951`, 인계 commit `3ef27f34`. 원본 사용자 Unity, 승인 1포즈, Save/UserData/Editor 보호, 본게임 적용0, Push·main병합0.
- 아트 납품 정본 `docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_Idle_4Frame_v1_Production_QA_20261020.md`, ZIP `Limitless_Chapter3_FrostFieldMouse_FrostTyrant_Idle_v1.zip` 총12항목 (ZIP 바이너리는 ChatGPT 아트 세션과 사용자 F:에 있으며 GitHub에는 문서/해시 기록만 등록).
- 검수 SHA256:
  - ZIP `6c01e55c147ab57147c767f43503553164197cc037a3834ecd2d15d10d96a23c`
  - Mouse Idle `Sprites/FrostFieldMouse_A_Idle_4Frames.png`: `5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2`
  - Tyrant Idle `Sprites/FrostTyrant_B_Idle_4Frames.png`: `9d9b8d05be1813c013a5d784d749ce2d8ef751541ac36ce90712f3255cb53f43`.
- 기술 계약은 동일: 2048×512 RGBA, 4×1 프레임 각512, 8 FPS/0.5s, PPU512/Pivot 하단중앙(0.5,0), 우향 1개, VisualScale Mouse1/Boss3. 원본 1포즈 SHA A=`0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`, B=`f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830` 그대로 유지.

## 승인 경계/다음
- 현재 상태: `IDLE_V1_CODEX_TECH_PASS_14_0 / IDLE_V1_CODEX_VISUAL_PASS_RECOMMENDED / USER_MOTION_APPROVAL_PENDING / ORIGINAL_UNITY_NOT_APPLIED / ANIMATION_RUNTIME_NOT_REGISTERED / MAIN_NOT_MERGED`.
- Codex 시각 PASS 추천은 사용자 승인으로 대체할 수 없음. **바울 대표가 실제 Idle 움직임을 보고 서리들쥐 A·설빙의 폭군 B 두 모션을 최종 승인했는지 명시적 확인 후** 다음 제작 게이트를 진행.
- 승인 시: Idle 2시트 바이트 정본 고정 → 별도 Codex worktree/프레임 Sprite import·기존 MonsterDefinition 명시적 IdleFrames 연결·실제 UI/필드 표시 및 최소 QA 계약 수립. 단 아직 전투 원본 구현, 공격·피격·사망 애니메이션, 다른 몬스터 신규 제작/Push/main병합 자동 승인 아님. 추가 Animator, Collider, Main22 구현 등 범위 밖 금지.
- 사용자 결정 전 신규 아트 담당에게 Idle 다시 제작하거나 Attack/Hit/Defeat 전체를 발주하지 않음. 현재 Field09 B 파생 Glow 격리 통합39PASS/0FAIL과 원본 Unity 적용·PR main병합 승인 대기는 **별도 작업 흐름으로 분리**.
