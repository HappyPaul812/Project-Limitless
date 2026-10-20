# LIMITLESS — Chapter3 서리들쥐 A·설빙의 폭군 B 1포즈 v2 대표 미술 승인 (2026-10-20)

- 기획 담당 브랜치: `docs/planning`. 신규 메인 아트 담당: `docs/art-new`, [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30).
- 최신 승인 근거: Codex의 동일 게임 UI 비교 검수 스크린샷을 보고 바울 대표가 **“좋네”**라고 답변함. 두 캐릭터 1포즈 미술은 **사용자 승인**으로 판정. 이전 A/B 고해상도 콘셉트 선택도 유지.
- Codex QA 보고: **총괄 PASS 및 두 포즈 미술 승인 권고**. 기술 ZIP·CRC·Manifest·PNG2 PASS. 서리들쥐 A는 128px에서 긴 꼬리·앞니·발톱·긴장된 체형 식별, v1 마스코트 인상 개선. 설빙의 폭군 B는 거대한 앞손·긴 팔·흉곽이 분리되며 최종 보스의 위협감 개선. 기존 v1의 단순 3배 확대·재포장이 아니라 형상·세부 묘사가 변경됐다는 검수. 다만 개별 수작업 픽셀 드로잉의 **작업 원본이 없어 제작 과정 자체는 독립 검증하지 못함**. Codex branch `codex/chapter3-ab-v2-art-review-20261020`, QA commit `3f23d1b4`, 인계 commit `7f9ca843`. 정적 동배율 비교이지 Unity 런타임 적용 검증은 아님.
- 아트 담당 `docs/art-new/문서/00_프로젝트/Chapter3_ApprovedAB_1Pose_ArtRework_v2_20261020.md`, 문서 commit `a3822c157c754b`, 인계 `b0ab6692909eba`.
- 납품 파일: `F:\Downloads\Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`. 담당 보고 ZIP SHA256 `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919` (8 entries, 실제 ZIP 바이너리는 GitHub 미등록).
  - `Sprites/FrostFieldMouse_A_1Pose_Rework_v2.png` 512×512 RGBA, SHA256 `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2`.
  - `Sprites/FrostTyrant_B_1Pose_Rework_v2.png` 512×512 RGBA, SHA256 `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830`.
- **상태:** `DESIGN_A_B_CONFIRMED / ONE_POSE_V2_USER_ART_APPROVED / FILE_TECH_PASS / STATIC_VISUAL_QA_PASS / PPU_RUNTIME_CONTRACT_PENDING / ANIMATION_NOT_STARTED / UNITY_NOT_APPLIED / MAIN_NOT_MERGED`.
- 이전 ZIP `ApprovedAB_PixelArt_1Pose_v1.zip`은 예전 기술 PASS·미술 HOLD로 보존, 이번 승인에 포함하지 않는다.

## 다음 제작 단계와 경계

1. 사용자 승인 1포즈 PNG2 **바이트 SHA 기준선 보호**. 원본 캐릭터 디자인을 임의 변경/기존 1포즈 재양산 금지.
2. Codex [Issue #35](https://github.com/HappyPaul812/Project-Limitless/issues/35)의 실제 몬스터/보스 **Animator·Frame 치수/방향/PPU/Pivot/Canvas·Alpha bbox/전투 표시 체급/Sorting** 기술 계약을 정리하고 모호한 항목은 확정 전 반드시 보고. 구현 Scene을 즉시 수정하는 작업과는 분리.
3. 다음 메인 아트 제작은 **2캐릭터만 1개씩 짧은 Idle/호흡 파일럿 → 정적 1포즈 스타일/실사용 스케일 검증 → 미술 승인 이후** Attack/Hit/Defeat·방향별 전체 전투 스프라이트로 단계별 확대. 정확 프레임/행 숫자는 게임 현재 Animator 계약에 맞춰 확정, 임의 “6행/20프레임” 만들지 않는다. 몬스터7종 전체 및 Boss 보스 기믹/신규 Scene 구현 자동 허가 아님.
4. 기존 Chapter3 F1·서릿길 마을 환경 PNG10은 별도 미술·Codex 기술승인 대기, 신규1포즈 승인을 환경10종 승인으로 혼동하지 않는다. Chapter1·2 리뉴얼 Field09·공식 스프라이트/Save/UserData 원본 보호.
5. Animation 파일럿 발주 전 사용자와 프레임 계약/업무 순서에 맞춘 단계별 제작 범위 확인. 신규 아트는 `docs/art-new`, 기술/Unity는 `codex/*`, 기획은 `docs/planning`, main 직접 commit/Push/merge 금지. 실제 게임 적용·정식 등록·애니메이션 완료는 별도 QA/승인 상태.

**기획 판정:** A·B **1포즈 v2 미술 승인 완료**. 다음 단계는 애니메이션 전체 자동 양산이 아니라 실제 게임 프레임 규격을 맞춘 1차 움직임 샘플 제작 준비.
