# LIMITLESS / 2026-11-04 / #89 냉기 VFX v2 · #90 거미 Attack3 v2 조건부 미술 승인

- 총괄 기획 담당 / `docs/planning`
- 사용자 원문: **“웹이랑 같다면 둘 다 좋아..”**
- 해석: 사용자가 본 웹/ChatGPT 비교판과 **실제 Windows F: ZIP이 SHA256으로 같은 경우**, 아래 특정 v2 아트 파일들의 **미술 승인**을 추가 질의 없이 확정할 수 있다. 동일성 미확인 단계에서는 `USER_CONDITIONAL_ART_APPROVAL / F_DRIVE_V2_SHA_PENDING`.
- 비교판/라이브러리 v2 실물 기술 검수: [20261104_Issue89_90_TargetedV2_ZipAndArtReview.md](20261104_Issue89_90_TargetedV2_ZipAndArtReview.md). 사용자 눈에 보인 것은 **오프라인 512·128·40 비교판**이므로 3화면비 실 Unity UX PASS나 제품 적용을 의미하지 않는다.

## 1. 승인하는 대상

| 이슈 | 사용자 검토한 v2 ZIP 전체 SHA256 | 승인할 아트 범위 | 유지할 미검증/미구현 |
|---|---|---|---|
| [#89](https://github.com/HappyPaul812/Project-Limitless/issues/89) | `9c07f5ff782c6e272796c4f003b816a39806ab35248c2353884325ac3c24a1eb` | 한기·동상·동결괴사 **v2 정지 RGBA 3 PNG 미술** | PPU/Pivot·Scale·실 GameView/HUD·상태/해제 실제 연결·VFX 애니메이션/Particle·원본 적용 |
| [#90](https://github.com/HappyPaul812/Project-Limitless/issues/90) | `804f8ebf85dbe7f7301cbfdecc6e85adc4e7d8888e23c3ee132bfca95854499b` | 기존 Prepare00 KEEP 및 **v2 Contact01·Recover02 정지 키포즈** 미술 | 실제 Attack8 나머지 **5 프레임 미제작**, Shoot8 미제작, 10FPS 전후 연결·실타격 접점·발 고정/미끄러짐 미검증, 원본 적용 |

**주의:** 이 범위의 승인만으로 신규 스킬/상태 수치/전투 구현·Prefab·UI를 승인하지 않는다. #88 냉기 런타임 Stage1 63PASS는 격리 실증으로 남고 원본 게임 적용 HOLD.

## 2. 실제 F: 검증 게이트 — Codex 단 1회

Codex는 **이미 확인한 사용자 Windows 경로**의 v2 ZIP 두 개만 SHA256을 계산해 위 값과 대조한다:
- `F:\Downloads\Limitless_Ch3_ColdVFX_Issue89_MinimalFollowup_20261104.zip`
- `F:\Downloads\LIMITLESS_SPIDER_ATTACK3_TARGETED_ISSUE90_v2.zip`

두 파일 중 해당 ZIP SHA가 정확히 일치하면 **그 이슈의 아트만** `USER_ART_APPROVED_SHA_MATCH / UNITY_NOT_APPLIED`로 승격하고 해당 이슈에 보고한다. 불일치·파일 부재·읽기 오류면 **그 이슈만 HOLD**하고 원본은 건드리지 않으며 실제 SHA/오류를 보고한다. 이미 완료한 Library CRC·Manifest·QA, 이전 #84/#85/#87/#88 회귀 테스트를 반복하지 않는다. 사용자의 컴퓨터에서 Codex 미실행인 현재 시점에는 F: 새 v2 SHA 일치를 **사실로 주장하지 않는다**.

## 3. 승인 확정 뒤 진행 가능한 다음 일

- **메인 신규 아트/VFX #89:** 승인된 새 v2 정지 PNG를 재제작하지 않고 보존. Codex에게 **Unity 6000.6.5f1 별도 격리** 환경에서 실제 40/128px, 밝은 F2/어두운 전투 배경, 960·1280·1680×720, 캐릭터/상태 아이콘·남은 턴/중첩·HUD 차폐 여부의 **시각 Delta QA**만 지시할 수 있다. 승인 없는 실제 전투 상태/클린즈/화로 자동 연결·원본 적용·새 Particle 대량 제작 금지.
- **리뉴얼 #90:** 승인된 Prepare00·Contact01v2·Recover02v2 **그대로 보존**하면서, 기존 실제 게임 기준 **우향/10FPS/PPU256/scale0.7**을 확인하고 8프레임 Attack 클립에 필요한 **미제작 중간5프레임**만 별도 신규 제작 후보로 발주 가능. 완료한3키포즈와 승인 Idle8/Walk8을 재작화하지 않는다. Attack8 완성 영상·관절·타깃 접점은 실제 제작·검증 전 PASS 아님. Shoot8은 별도 후속 승인 전 HOLD.
- **공통:** 담당 브랜치 `docs/art-new`/`docs/art-renewal`, Codex `codex/작업명` 별도 worktree, Unity 6000.6.5f1. 사용자 원본 Editor·Save/UserData·미커밋 변경·승인 프레임 보호. 직접 `main` 커밋·원격 Push/merge 금지.
- 두 이슈 상태는 **미술 승인 여부**와 **실제 원본 Unity 적용 여부**를 항상 분리 표기한다.
