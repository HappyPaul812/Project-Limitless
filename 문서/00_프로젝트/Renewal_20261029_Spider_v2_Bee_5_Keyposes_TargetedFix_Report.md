# LIMITLESS / 2026-10-29 — Spider v2 1포즈 & VenomBee Idle2+Attack3 표적 보완 결과

**브랜치:** `docs/art-renewal` · **정본:** main `bfd281545632877325f5d21e04cefb924042c465` 확인, 공통 정책·[역할](팀운영/담당/WORLD_ART_RENEWAL.md)·[최신 인계](팀운영/인계/WORLD_ART_RENEWAL_LATEST.md)·Issue [#60](https://github.com/HappyPaul812/Project-Limitless/issues/60), [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63), [#65](https://github.com/HappyPaul812/Project-Limitless/issues/65), [#74 Codex 통합 검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6488218748)와 기획의 [10/29 표적 수정 판정](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6488513653) 근거.

## 0. 직접 납품·파일 검증

- **이번 새 ZIP**: `Limitless_Spider_v2_Bee_5Keyposes_20261029.zip`, **1,986,288 bytes**, **26 entries**, ZIP **CRC PASS**, **SHA256 `b509445f62c0fafc91513f846913ca05c9bc427cb0c30749e0c54b0c85801cc4`**. 이 채팅 작업 컨테이너 `/mnt/data/Limitless_Spider_v2_Bee_5Keyposes_20261029.zip`에 실제 바이트 존재·해시 직접 계산. 바울 대표 PC `F:\Downloads`에 자동 복사하지 않음. GitHub 저장소 PNG/ZIP 바이너리 커밋 0.
- ZIP 내부 `Evidence/Manifest.json` 파일별 bytes/SHA256, `Evidence/SHA256SUMS.txt`, `Evidence/QA_Results.json`, `Evidence/README_KO.md`, **Spider 기존 vs v2**, **Bee 기존 프레임 vs 신규 키포즈**의 오프라인 실제 픽셀 비교판 포함. 이번 파일 검증 PASS는 **정적 파일·CRC·해상도** 한정이며 Unity/GameView·미술 최종 승인 PASS가 아님.
- 이전 제작 묶음 `Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip` 원본 **SHA256 `4b24310669e9db40b50e3d691acaf6077fab8b1102475e2dbaaddd5d57d1d82a`** 보존(원본 ZIP 변경 0). 기존 Spider v1/512 원본과 Bee 마스터 및 Bee 기존 Idle10+Attack10 20개 파일 **덮어쓰기 0**.

## 1. 숲거미 forest_spider — v2 표적 원화 1포즈

- **신규 원화:** `Spider/ForestSpider_OriginalPilot_v2_TargetedTexture_512_RGBA.png`, 512×512 RGBA · SHA256 **`3dedf30e2d5e95e299016980bec1c31456f4c3c796cd2222da948eeab92784ca`**. 기존 v1을 출발점으로 표면의 명암·거친 질감, 기존 다리 관절 윤곽과 복부 갑각 마모/눈매를 국소 보완. 원본 v1과 **알파 채널·실루엣 픽셀 완전 동일** 정적 검사 PASS. 전체 종/형태 전면 재제작 또는 외부 ethanf30games 2D Spider 원본 트레이스·리컬러가 아님.
- 새 축소 이미지 `Spider/ForestSpider_v2_Preview_128.png` 128×128, `...Preview_40.png` 40×40. 기존-v2 직접 원본 픽셀 비교판 `Evidence/Spider_v1_vs_v2_OFFLINE_Comparison.png`, SHA256 **`7f569eff201a2551edc5c4d019c1d177c2a7139220b4147c2aff45e3ed55aaff`**. **OFFLINE** 비교로 게임과 공식 Chapter 2 실제 동배율 검증 아님. 작은 크기/명암/화풍 판독은 사용자 미술 심사 필요. 현재 `SPIDER_V2_ART_CANDIDATE / USER_REVIEW_PENDING / NO_ANIMATION_32`.
- 기존 런타임 `forest_spider`, Field02 4/Field03 1 Spawn, `MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}` 각 8PNG =32, 10FPS, 우향, PPU256/중앙 Pivot, visualScale0.7. 현재 외부 `ethanf30games / CC0 1.0` 32PNG와 ThirdParty 원본 32/32 일치(Codex 직접검수) — 본편에서 안전하게 임시 유지하되 **오리지널 교체 큐 P1 확정**. **v2 최종 디자인 승인 전 새로운 32프레임 애니메이션 제작 0**.

## 2. 독침벌 VenomBee — 기존 20장 보존, 핵심 관절 영역 키포즈 5장 추가

- 기존 오리지널 v1 마스터 **512×512 RGBA** 및 신규 `Idle10 + Attack10 =20개` 원본 개별 PNG·시트·10FPS GIF **수정 0**. 이번 산출은 후보 별도 폴더 `Bee/`의 추가 PNG **5장**.
- 실제 기술 계약: 현행 외부 PilotBee 우향 Idle10 @238×215 셀(8×2), Attack10 @315×253 셀(10×1), 10FPS, visualScale.85; 신규 마스터는 좌향이었으므로 이번 후보는 **우향으로 기준을 맞추고** 캔버스/표시 PPU 등의 본게임 적용은 별도 QA로 남김.
- **Idle 2장**: `Bee/Idle_Key01_WingsOpen_238x215_RGBA.png` SHA256 `a6372a671446740ef7bff33cbe03dbe321dca07c626b96f122db3249035d3520`; `Bee/Idle_Key02_WingsFold_238x215_RGBA.png` SHA256 `1fc6771190f5643cd3491c460cee068a73c9f11ec6141e38daa45cca4ee54b4f`.
- **Attack 3장**: `Bee/Attack_Key01_Reach_315x253_RGBA.png` SHA256 `2617ff4b22e545470c2619dd4e8ff8b6416dca7fd8a7f3ffa9dc2b4df27bbb14`; `Bee/Attack_Key02_Sting_315x253_RGBA.png` SHA256 `0067b04225abfef7eeaf8cb0d0ed9af9ca60f430914f48b41e4b338e5bf0c1ea`; `Bee/Attack_Key03_Recovery_315x253_RGBA.png` SHA256 `edf57bc026b14be368eb80efbe918fb1e92aff010cd56a12e37b9b51cfc6a7ee`.
- **방법과 엄밀한 한계:** 이전 전체 이미지 균일 이동/회전과 달리 날개 양측·앞다리·중앙다리·복부/독침 부위에 서로 다른 **가우시안 국소 픽셀 비강체 워프**를 적용하여 위치 변화가 분리되는 후보를 만들었음. 그러나 **손으로 새 관절을 재작화·리깅한 정식 독립 애니메이션이 아니며 5장 모두 `ART_REVISE` 후보**. 관절/날개 연결부의 실제 작화, 40px 독침 가독성/피벗·타격 타이밍과 전투 루프는 별도 미술/Unity QA 필요.
- `Evidence/Bee_Existing20_vs_5New_Keyposes_OFFLINE.png` SHA256 **`4b9bbb88248b1a8a87fe0b05b627550b40121da93ebe7da7dc84ea7a5385fcd7`**. 기존 Idle[00,05]·Attack[01,04,08] 각각 vs 5장 후보 원본 크기·128/40px 실제 축소 OFFLINE 비교. `Bee/Idle_Keyposes_ONLY_10fps.gif`는 2키포즈, `Bee/Attack_Keyposes_ONLY_10fps.gif`는 3키포즈를 **100ms/프레임** 반복하는 **검토용 플립북**일 뿐, 기존 각 10프레임 전체 전투 애니메이션 대체 가능을 의미하지 않음.
- **판정 `BEE_5_KEYPOSES_FILE_PASS / MOTION_ART_REVISE / EXISTING20_KEPT / NOT_REPLACEMENT_READY`**. 10FPS/방향/셀 크기 문서·오프라인 크기 확인. 실제 제품 Unity 표현·실전 기술/시각 PASS 금지.

## 3. 원본·권리 보호 및 다음 작업

- VenomBee 활성 외부 `Jana Ochse / 2D!PIXX PilotBee`, OpenGameArt CC BY 3.0 vs 동봉 `LICENSE.TXT` CC BY 4.0 불일치 **`RIGHTS_VERSION_CONFLICT_OPEN`**, 출처·저작자 표시·기존 라이선스 문서 그대로 보존. 새 자체 후보가 있다고 충돌 해결되거나 활성 외부 Sprite 자동 삭제되었다고 하지 않음.
- 게임 원본 Unity/Scene/Prefab/Resources/MonsterDefinition/기존 Sprite·GUID/meta, 공격 판정·독/드롭·AI/Spawn/퀘스트·Save/UserData 및 사용자의 PC, 기존 Wolf·Bear·Fox / Chapter2 공식 몬스터·보스/환경·OGA03/09/20 **변경 0**. GitHub docs/art-renewal **문서만 신규 작성**.
- 다음 게이트: 바울 대표가 Spider v1/v2, Bee 기존/5후보의 **실제 이미지·GIF**를 보고 수정/승인 방향 결정. Spider 합격 시에만 Idle8/Walk8/Attack8/Shoot8 단계별 별도 아트 계약. Bee는 우선 5키포즈의 **수작업 독립 날개/다리/독침 도색/실루엣 재검수**가 필요하며 나머지 기존20 일괄 재제작 금지. 승인 후 Codex **격리 Unity 동일 배율 40/128·GameView·10FPS·원본 스킬/전투 불변 검수**. **본편 적용·main 직접 Push/병합 금지.**
