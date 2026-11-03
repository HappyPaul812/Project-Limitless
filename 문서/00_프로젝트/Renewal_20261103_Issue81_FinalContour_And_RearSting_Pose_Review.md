# LIMITLESS / 2026-11-03 / Issue #60·#63·#81 — 숲거미 최종 윤곽 알파와 독침벌 후방찌르기 3키포즈 후보

**브랜치 `docs/art-renewal` / 제품·Unity 원본 미변경 / 승인 보류.**
[공통규칙](팀운영/COMMON_RULES.md), [기존 최신 인계](팀운영/인계/WORLD_ART_RENEWAL_LATEST.md), [Issue #81 Codex 실검수](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6591984789), [기획 판정](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6592162086) 확인. 이전 v4는 RGB 전체 동일·알파491 감소였어도 앞다리 아래 직선 잔상으로 `ALPHA_FINAL_HOLD`였음. 기존 Bee 11/03 New5는 실제 새 PNG로 인정됐지만 우향 머리·좌향 뒤쪽 침의 표적 지향 및 시간 연속성 미비로 `MOTION_REVISE_CONTINUE`.

## 1. 이번 새 실물 납품 (바이트 직접 생성·재검증)

- ZIP: `LIMITLESS_ISSUE81_FINAL_TARGETED_20261103.zip`
- **2,443,598 bytes, SHA256 `53e280ffe499049dc7544e436bbc471c3de00ea9df81975988fe81220e28fb46`, 29 entries, CRC PASS, Manifest 27개 페이로드 전체 SHA256 검증 PASS**.
- 대화의 `/mnt/data/`에서 파일을 생성한 결과이며, 사용자 Windows `F:\Downloads`로 자동 복사 또는 GitHub PNG/ZIP 업로드는 하지 않음. `Manifest.json`, `SHA256SUMS.txt`, `Evidence/QA_Results.json`에 각 파일의 실제 SHA·bytes 및 픽셀 델타 기록.
- **명시적 구분:** PNG 파일 디코딩·치수·CRC·기존 대비 SHA·변경 픽셀은 `OFFLINE TECH PASS`. 실제 Field03/GameView/PPU·전투 스킬/타격 정합은 `NOT_TESTED`, 제품 교체 `HOLD`.

## 2. Organic forest_spider — 기존 디자인 보존, 앞다리 윤곽따라 알파 v5 표적수정

- 이전 기준 `ForestSpider_Organic_v4_ALPHA_EDGE_CANDIDATE_512.png` **512 RGBA / SHA256 `c9eedca2634baaae41b53af26f5c26e18b1abe3be7fe15fe35a717d0a7e8378d`**를 `Spider/ForestSpider_Organic_v4_ORIGINAL_KEEP.png`로 바이트 복사해 보존.
- 신규 `Spider/ForestSpider_Organic_v5_FORELEG_CONTOUR_ALPHA_512.png`, **512×512 RGBA SHA256 `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`**.
- **RGB 원본 262,144픽셀 중 변경0**. 알파 **380픽셀 감소**, 증가0. 변경 bbox **x23..72, y341..388**. 지정 x41..95/y306..353 주변에서 시작해 실제 거미 왼앞다리 끝을 따라 내려가는 회색 매트 부분만 처리. 큰 직사각형 영역 전체를 투명화한 것이 아니며 **각 스캔라인마다 다리 외곽의 완만하게 달라지는 윤곽 경계로 마스크를 적용**. 좌측 실제 붉은 갈색 각질·털·발끝 보존을 위해 경계 근접 1~2px 투명도를 완충, 바탕 RGB 변경0.
- 실제 원본/수정 픽셀 판독판 `Review/Spider_Foreleg_BEFORE_AFTER_4p8x.png`, v4/v5 512·128·40 비율 축소, 밝은 배경/어두운 배경/절차합성 **가상 숲바닥** OFFLINE 비교 3종 수록. 실제 Field03 지면 텍스처/Unity 캡처는 이 세션에 없어서 **동일배경 GameView 검증·최종 알파 PASS 아님**. 작은 흩어진 머리털/실끝·잔여 색물림은 사용자 및 Codex 로컬 미술 검수 게이트 유지.
- 바울 대표가 **유기형 디자인 방향은 승인**했지만 알파·애니메이션·본게임 교체를 승인한 것은 아님. 기존 Ch4 메카닉 Spider v3 `ARCHIVE_ONLY`, 현재 외부 원본 Idle8+Walk8+Attack8+Shoot8 **32개 프레임 그대로 / 신규32프레임0**.

## 3. VenomBee — Idle2 보존, Attack3 신규 독립 원화 후보

- 2026-11-03 Issue81 ZIP의 정상 Idle `Idle_Folded_238x215.png`, `Idle_Spread_238x215.png`는 `Bee/KEEP_` 이름으로 **바이트 그대로 복사/해시 보존**.
- 기존 Attack3 `Attack_Prepare_315x253.png`, `Attack_Sting_315x253.png`, `Attack_Recover_315x253.png`는 `Bee/Attack_*_OLD_KEEP.png`로 원본 보존.
- 새 독립 그림 3장 소스(이번 회차 새 렌더)에서 **등/복부/6개 다리/날개를 각각 다른 자세로 표현한 PNG**를 추출해 기존 Attack 셀 **315×253 RGBA·우향 머리 기준·종횡비 보존**으로 배치. 단순 이전 PNG 해시만 바꾼 재포장이나 이전 전체 이미지 이동/회전이 아님. 새 포즈는 보다 큰 화폭의 별도 페인팅에서 축소된 시안이며, **프레임 간 같은 캐릭터의 정확한 관절 일관성이 완성된 수작업 애니메이션이라고 주장하지 않음**.
- 실제 이전 vs 신규 SHA 및 변경 RGBA 픽셀:
  - **Prepare** 이전 `50c8047125e86bced17073fead14adaaa35545dea19beb9306f1c267481ea7e8` → 새 `Bee/Attack_Prepare_REPAINT_v2_315x253.png` **`e337921236b7b947f92bbb5d88824bc6729d6668a050a85fc2f519ff4df26212`**, 변경 **37,807px**.
  - **Sting** 이전 `17bd20da372f1453ec903228a0394148bf1a0000c3d6d714d0b2404c31863624` → 새 `Bee/Attack_Sting_REPAINT_v2_315x253.png` **`391ed94e50ffc4ddcf3bc4442e9913e0b947cc9d5d9a57aaf865f46556e021ce`**, 변경 **31,970px**.
  - **Recover** 이전 `9002a76ffa2b35b8aa34c35178e3cae4967992878e2df2b72db3e96e068d18be` → 새 `Bee/Attack_Recover_REPAINT_v2_315x253.png` **`2afc69e803334b1ededdadb39e786f297d8fe3d9918e72997943c9f1593c740e`**, 변경 **39,037px**.
- 신규 포즈마다 **128px/40px 원비율 유지** 축소 이미지, 전후 직접 비교 `Review/Bee_Attack3_OLD_vs_NEW_128_40_OFFLINE.png`, **3포즈·각100ms 10FPS 검토용** `Bee/Attack_3KEYPOSES_REVIEW_10FPS.gif` 포함. GIF는 전체 Attack10 완성본이 아니다.
- **후방 찌르기 기획:** 신체 구조에 맞게 독침은 복부 뒤쪽(왼편)에 위치, Prepare에 움츠림→Sting에서 복부 뻗음/독침 후방 방향→Recover 원래 자세. 기존 게임의 우향 `sourceFacesRight=1`과 표적이 우측인 전투 연출에서는 **방향 불일치가 아직 해소되지 않았다**. 목표가 뒤에 있을 때만 맞는 후보이므로 **실제 공격 표적 방향·좌우 반전/체축 회전·타격 시점과 복귀 연결을 Codex·바울 대표가 명시 확인할 때까지 `ATTACK_AXIS_HOLD / MOTION_CONTINUITY_REVISE / NOT_COMBAT_READY`**.
- 기존 오프라인 Idle10+Attack10 **20프레임 마스터·시트 원본과 정상 Idle2 및 기존 작업 ZIP/SHA 모두 보존**, 이번 회차 전체 20프레임 새로 양산하지 않음. PilotBee 원본 OpenGameArt **CC BY3.0 / 동봉 LICENSE CC BY4.0 충돌 미해결**, 외부 본편 기존 Sprite/License 제거하지 않음.

## 4. 기능·권리·승인 보호

- 현재 MonsterDefinition, Spawn, AI, 독, 드롭, Sprite GUID/meta, Field02·03 Scene/Prefab, Quest, Save/UserData, 원본 Unity, Chapter2 공식 몬스터·보스, Wolf/Bear/Fox, 승인 환경/NPC 자산 일절 변경0. `main` 직접 commit·Push·병합0.
- **다음:** (1) 바울 대표는 Spider 한정 알파의 확대/128/40 시각 검토, Bee 후방 공격 방식/시간적 연결 여부 검토 (2) Codex 읽기 전용 실제 Field03 배경·실 Unity 동일 PPU/10fps/공격 표적 검증 (3) 추가 문제가 발견되면 해당 α 경계/Attack 키포즈만 추가 개선 (4) 최종 바울 대표의 제품 교체 별도 승인 전 `NOT_UNITY_APPLIED`.
- 최종 `OFFLINE_FILE_TECH_PASS / SPIDER_ALPHA_USER_REVIEW / BEE_ATTACK_KEYPOSES_NEW_PIXELS_PASS / BEE_TARGET_DIRECTION_HOLD / MOTION_CONTINUITY_REVISE / FINAL_ART_APPROVAL_REQUIRED`.
