# LIMITLESS / 2026-11-03 — Organic ForestSpider Idle8·Walk8 1차 파일럿 + VenomBee 표적공격 계약 검토

**작업 브랜치:** `docs/art-renewal`. **정본/근거:** 최신 공통 규칙, `WORLD_ART_RENEWAL.md`, `WORLD_ART_RENEWAL_LATEST.md`, Issue [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60), [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63), [#81](https://github.com/HappyPaul812/Project-Limitless/issues/81), [#82](https://github.com/HappyPaul812/Project-Limitless/issues/82). 사용자 결정: **ForestSpider 유기형 v5 승인 원화는 확정 / 애니메이션 1차 16프레임 제작 허가 / Attack·Shoot는 미허가**, VenomBee는 실제 타깃·접점 근거가 없으면 새 공격 포즈 생성하지 말고 HOLD.

## 실제 납품·검증 결과

| 산출물 | 실제 크기 | ZIP SHA256 | 파일 수 | 검증 |
|---|---:|---|---:|---|
| `spider_motion.zip` | 7,953,765 bytes | `d4c7c73a7c830f2e7884a664ec45759b673434ed5d70fcc49fc52458ea4524db` | 33 | CRC PASS / 내부 manifest 31/31 SHA PASS |
| `bee_target.zip` | 709,946 bytes | `450b78e4c68b812afff4f9b30d6ff0d54a5d74cb898feb5770cc308a0e9d7bd0` | 9 | CRC PASS / 내부 manifest 7/7 SHA PASS |
| `limitless_motion_full.zip` | 8,666,252 bytes | `f7264dae1a585a9670624149e1c72dd622e6e48674ce0c664064241dc50c53d1` | 41 | CRC PASS / 내부 manifest 37/37 SHA PASS |

**실물 위치:** 이 대화의 `/mnt/data`. 바울 대표 Windows `F:\Downloads`에 자동 복사한 것이 아니며 PNG/ZIP을 GitHub에 Push하지 않음. 각 ZIP에 독립 Manifest/Checksum을 제공하여 이전처럼 '합본 참조 항목 때문에 개별 ZIP 검증 불완전' 오류가 발생하지 않게 함. 각 파일 bytes/SHA256은 ZIP 내부 `Manifest.json` 또는 `Manifest_standalone.json`, `SHA256SUMS.txt`에서 실물로 확인 가능.

## 1. 숲거미: 승인 v5 보존 + Idle8·Walk8 1차 후보

- **유일한 승인 원화:** `ForestSpider_Organic_v5_FORELEG_CONTOUR_ALPHA_512.png`, **512×512 RGBA / SHA256 `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`**. ZIP의 `Spider/Source_KEEP/ForestSpider_v5_APPROVED_BYTE_KEEP.png`와 byte-for-byte SHA 동일. **Idle00 역시 승인 PNG 바이트를 그대로 사용**. 원화 RGB/알파/기존 마스크 자체 수정0.
- **신규 16개 RGBA PNG 실제 생성:** `Spider/Idle/ForestSpider_Idle_00.png`~`_07.png` 8개, `Spider/Walk/ForestSpider_Walk_00.png`~`_07.png` 8개. **각 512×512 RGBA**. 기존 승인 v5와 다른 실제 PNG 픽셀은 `Review/QA_Results.json`에 프레임별로 기록. Idle00은 원본과 바이트 동일, 나머지는 고유 프레임 이미지.
- **시트 2개:** `Spider/Sheets/ForestSpider_Idle_8_4x2_512_RGBA.png`, `ForestSpider_Walk_8_4x2_512_RGBA.png` — **2048×1024 RGBA**, 4열×2행, 셀512. 본게임 Sprite Library/Loader 변경 금지; 기존 8개 프레임 **분리 리소스 Runtime 계약**과 다르게 실제 Unity 연결 전에는 폴더·파일명/PPU256/중앙 pivot 적합성 별도 검수.
- **미리보기:** Idle/Walk 각각 128px·40px 원비율 8프레임 ContactSheet, **8FPS(125ms) 임시 검토 GIF와 기존 게임 10FPS(100ms) 계약 참고 GIF**를 각각 128px·40px로 제공. 기존 현행 `MonsterDefinitions/03_ForestSpider.asset`의 `animationFramesPerSecond=10`, `sourceFacesRight=1`, `visualScale=.7`, `MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}` 각8/총32(현재 외부 CC0 PNG)가 확인된 정본. 8FPS를 게임의 새로운 확정값으로 기록하지 않음.
- **제작 방법의 사실관계:** 승인 v5 정지 원화의 **발끝·무릎 주변 위치별 국소 비강체 픽셀 변형**으로 8개 다리 끝의 교대 스윙·리프트 및 작은 Idle 호흡을 시뮬레이션. 몸통 전체 일괄 이동/회전을 사용한 초안과는 다르지만, **각 다리를 새로 수작업 재그린 IK/분절 관절 리깅 애니메이션은 아님**. 피부/갑각 이음매, 지면 그림자, 실제 다리 접지, 40px 움직임의 자연스러움은 `MOTION_ART_REVIEW/REVISE_POSSIBLE`. **정적 파일 QA PASS는 미술·Unity PASS가 아님**. 사용자 모션 시각 승인 및 Codex 독립 격리 테스트 전 본편 미적용.
- **남은 모션:** Attack8/Shoot8 **신규 제작0**. 1차 Idle/Walk 16장 사용자 검수 및 런타임 접지·PPU 검증 후에만 진행한다.

## 2. 독침벌: 공격 표적의 실사용 계약 미확정 → 스토리보드만, 새 공격 포즈 HOLD

- Codex [Issue #82](https://github.com/HappyPaul812/Project-Limitless/issues/82#issuecomment-6593291711)는 신규 Attack3의 **Sting 독침 끝이 우하향**한다고 관찰했으나, **실 Battle의 적 전열/후열 표적 Slot 상대좌표·충돌 접촉점·몸 이동/회수 타이밍은 확인하지 못함**. 외부 화면에서 임의로 실제 표적 좌표·타격 위치를 단정할 수 없음.
- **실제 신규 제작:** `Bee/VenomBee_Attack_Target_Storyboard_TEST_ONLY.png` 1장. 기존 `Prepare`→`Sting`→`Recover`의 **원본 315×253 RGBA PNG3장 + 정상 Idle2 원본**을 별도 `Bee/Source_KEEP`에 바이트 보존하고, 오른쪽 **가상 타깃 `TEST_ONLY`**을 그림에 표시했다. 후방 복부 독침이 실제 우측 Target에 접촉했다고 주장하지 않는다. 앞머리에 새 독침을 생성하지 않음.
- **신규 Sting/Recover 키포즈:** **0장**. 실제 Target 계약이 불명확한 상태에서 불필요한 이미지 재생성/가짜 수정 SHA를 만들지 않고 `TARGET_CONTRACT_HOLD`. 기존 Idle10+Attack10 **20개 오프라인 프레임은 이전 ZIP 그대로**, 이번 회차 수정·전량양산0.
- 파일의 그림 비교는 **오프라인 참고판**, 실제 10FPS 전투 모션 / 표적·접점/공격축/동일 PPU 검증은 **HOLD**. 우향 sourceFacesRight=1이 곧 공격시 뒤쪽 침으로 상대를 타격하는 방향 보증을 의미하지 않는다.
- **권리:** PilotBee 원본 웹 CC BY 3.0 / 동봉 LICENSE CC BY 4.0 표기 충돌은 **`RIGHTS_HOLD` 미해결**. 외부 원본 보존/권리 크레딧 승인 전 제품 교체 금지.

## 보호 및 후속 미술 승인

- 원본 Unity 6000.6.5f1 프로젝트, Save/UserData, 기존 MonsterDefinition/Runtime·Field02/03 Spawn·AI/독·Drop·Prefab·Sprite GUID/meta·Scene·Quest, Chapter2 공식 몬스터/보스·Wolf/Bear/Fox·환경/NPC 승인 자산 및 Chapter4 메카닉 Spider v3 `ARCHIVE_ONLY` 모두 변경0. GitHub에는 `docs/art-renewal` **문서만 커밋**, `main` 직접 커밋/Push/병합 0.
- **판정:** `SPIDER_16_FILES_TECH_PASS / SPIDER_MOTION_ART_REVIEW / 8FPS_REVIEW_ONLY / GAME_CONTRACT_10FPS / ATTACK_SHOOT_NOT_STARTED / BEE_TARGET_CONTRACT_HOLD / BEE_NEW_ATTACK_POSES_ZERO / RIGHTS_HOLD / NOT_UNITY_APPLIED`.
- **차기 최소 Codex 지시:** (a) `forest_spider` 8 FPS 임시 GIF와 기존 10 FPS Runtime 비교, 8다리 각 무릎/발끝 접지·그림자 프레임별 모양·128/40px 시인성만 격리 QA. 자연스럽지 않으면 프레임별 REVISE 사유를 확정 (b) `venom_bee` 실제 Battle에서 공격자/표적 slot·sprite 방향·기존 타격 좌표/animation event를 읽기전용으로 확인해 `TARGET_CONTRACT` 작성. 검증 전 Sting/Recover 수정 발주 금지.
