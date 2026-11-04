# LIMITLESS / 2026-11-04 / Chapter2 전투 배경 실물 감사·Spider Attack8 모션 QA·냉기 VFX 다음 단계

- 작성: 총괄 기획 / 브랜치 `docs/planning`
- 접수: 바울 대표의 Codex 로컬 조사 스크린샷 및 `F:\Downloads\LIMITLESS_ATTACK8_ISSUE90_MOTION_QA_20261104.zip`
- 증거: [#91 Ch2 로컬 원본 감사](https://github.com/HappyPaul812/Project-Limitless/issues/91#issuecomment-6622600289), [#90 모션 QA 실보고](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622626595), [#89 VFX 표시 배치안](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6622602474).

## 1. Chapter2 전투 배경 — 읽기 전용 실제 원본 감사 결론
- 로컬 Scene39/Prefab13, PNG949파일 목록, Catalog/.asset/Resources/GUID 및 실 전투 선택 로직을 Codex가 조사했으며, 이는 `QA_PASS` 수가 아님.
- Chapter1 PNG 6개 보유, 카탈로그에서 실제 고유 4종 선택. ToxicSwamp/AncientSanctuary 실 PNG는 참조 안 됨.
- Chapter2 **Field_04~10**: 전용 Sprite가 아니라 NightSky/DistantField/BattleGround/GoldenHorizon 색상 패널 fallback.
- **Field_11 DeepCore/Main20**: `Main20/Environment/DeepCore_Ground_Base` 512² 지면 PNG를 배경 Sprite로 *재사용*. 별도 전투 배경 전용 그림이 아님.
- Field10 `BurningPulse_DistantFlameSilhouette`은 월드 원경 연출용으로, Battle Catalog 대상 아님.
- **결론:** `CH2_DEDICATED_BATTLE_BG_NOT_FOUND / CH2_FALLBACK_IN_USE`. 기존 Ch1 전투/Ch2 Main20 참조·GUID를 무단 교체하지 않음. Chapter2 전용 배경을 새로 만들지 결정할 미술 제안은 별도; 우선 **Ch3 신규2종의 기존 Ch1 화풍 비교/사용자 시각승인** 우선.
- Chapter3 후보 ZIP `Limitless_Chapter3_BattleBackground_2Pilot_20261104.zip` 실제 F: 전체 SHA `a8c02e9ab6189a6feb074d3f83c8cd6a8bce61ff1b8de110e7c8216934468eb9`가 라이브러리 사본/보고값과 일치. **파일 무결성 PASS != 사용자 Ch3 미술승인/Unity 적용**.

## 2. Attack8 신규 모션 QA ZIP 실제 검수 및 후속 교정
- 새 검수 자료 ZIP `LIMITLESS_ATTACK8_ISSUE90_MOTION_QA_20261104.zip` ChatGPT Library 사본 **295,072 bytes, SHA256 `045fed92f86b50b2b4ea1eda3b95b397fa1219171c7702846934daa7fa63b2e8`, CRC PASS**. `QA_Findings.md`, `Motion_QA_Metrics.json`, `Attack8_FrameMetrics.csv`, 실제 프레임 확대판, 원본 40px 10FPS GIF를 열고 대조. **사용자 F: 이 최신 QA ZIP 자체의 SHA는 여기서 직접 확인하지 못함**. 개별 아트 PNG 수정 파일이 아닌 QA 증거 패키지.
- 기존 승인 `00 Prepare/03 Contactv2/06 Recoverv2`는 **APPROVED_KEEP / 바이트 동결**, 신규 `01 Windup`은 **KEEP_CANDIDATE**.
- **표적 수정만**: `02 Lunge` 전방 오른쪽 다리 관절/발끝 얇아짐·축소(01→02 가시 픽셀 74803→66721 -10.8%); `04 Recoil` x491..496/y278..286 분리31px; `05 Return` x78..86/y308..330 분리17·14·3px, 04→05 IoU0.720 및 중심(+11,-7) 이동이 최대 연결 위험; `07 IdleBridge` x459..461/y309..314 분리7px·06→07→Idle 복귀 연결.
- `07→Attack00` GIF 반복 IoU0.651은 **실전 전투 루프 근거 아님**. `07→승인Idle00` IoU0.752; Runtime 상태전환·foot-lock·attack-contact 검증0.
- **작업 게이트:** 원본 승인3+01을 재작화하지 않고, 실제 네 PNG **02/04/05/07만** 국소 보정한 `v2_TARGETED_CANDIDATE` 새 파일로 납품. 512² RGBA·실제 40/128 비교·10FPS 8슬롯 연결 보기·관절/지지발 가이드·SHA256SUMS/Manifest/CRC·ZIP; 새4장의 미술 승인 및 격리 UnityClip QA는 별도. Shoot8·Bee RIGHTS_HOLD 유지.
- 이 검수 결과는 **`ATTACK8_QA_EVIDENCE_PASS / REVISE_ONLY_02_04_05_07 / NEW4_ART_NOT_YET_PRODUCED / NOT_UNITY_APPLIED`**.

## 3. 냉기 VFX #89 99PASS 시각 PARTIAL 후속
- Codex의 [99PASS를 재실행하지 않은 3개의 표시/레이아웃 제안](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6622602474)은 **`PROPOSAL_ONLY / NOT_IMPLEMENTED`**.
- 40px: 휠체어 바퀴/발 아래 VFX와 약 8px 분리하는 시험 오프셋.
- 128px: 캐릭터 뒤 레이어 + 아래 약 24px 이동하는 시험 오프셋; 휠체어·하체 가림 완화 목표.
- HUD: 960px 상태배지를 2행/별도 상태 그룹으로 분리, 1680 전장 시작은 HUD 하단에서 약8px 아래에 두는 비교안.
- **숫자는 QA 비교용 가정, 확정 규격 아님.** 승인 3종 PNG 바이트 보호·화면 구성2~3안 소량 격리 Delta QA가 다음 게이트. 40px은 VFX 단독으로 상태 식별을 강제하지 않고 **모양 다른 아이콘·한글 상태명·남은 턴/중첩·위험문구** 병행. 기존 12조건·99PASS 전수재실행 금지.

## 4. 우선순위 및 보호
1. 리뉴얼 `docs/art-renewal` → 거미 프레임 02/04/05/07만 수정 후보 작성·#90 보고 (실행 전까지 미완료).
2. 메인 신규 아트 `docs/art-new` → Chapter3 배경2와 기존 Ch1 공식 BG **동일 배율 화풍 비교** 우선; 사용자 최종 시각 승인 전 기존 배경 재작화·Unity Import 금지.
3. Codex → #89의 제안 레이아웃은 필요 시 **격리6000.6.5f1 표적2~3안 테스트**; 실제 사용자 승인/원본적용 별도. 이미 조사한 Ch2 원본 재감사 반복 금지.
4. Chapter2 전용 배경 제작은 감사상 필요한 잠재 신규 업무지만 **사용자 신규 아트/범위 결정 전까지 발주 HOLD**. 필드별 개별7장 자동 양산 금지.
5. 원본 Unity6000.6.5f1 Editor/Scene/Prefab/.meta/Catalog/Save/UserData, 기존 승인 Owl·Spider v5/Idle/Walk/키포즈/냉기PNG, 개인 미커밋 파일 보호. `main` 직접 commit·원격 Push·merge 금지.

이번 문서는 기획 기록/업무 범위 규정만; **다른 Codex 및 아트 채팅을 자동으로 실행하지 않음**.
