# LIMITLESS / Chapter3 M2 설원족제비 Attack4 입·턱 국소 보정 v2 — 2026-11-16

**담당:** 메인 신규 아트 (`docs/art-new`) · **이슈:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)

**상태:** `ATTACK4_MOUTH_TARGETED_FIX_V2_DELIVERED / OFFLINE_FILE_QA_PASS / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_ORIGINAL_APPLIED`.

## 1. 사용자 피드백과 범위

바울 대표: **“Attack4 전체 공격 흐름은 괜찮지만 2·3프레임 입 모양이 조금 어색하다.”**

- 수정 대상 **프레임02·03의 입·아래턱·주둥이 주변만**. 과장된 붉은 입 공간, 아래턱 길이/돌출, 부리같은 실루엣을 최소화. **Attack4 전체 재제작 금지** 준수.
- Frame01/Frame04 **RGBA 픽셀 변경0**. Frame02에서 **1,015 픽셀**, Frame03에서 **575 픽셀**만 변경.
- 변경 경계는 512셀 기준 F02 **[449,307)-(490,345)**, F03 **[456,312)-(494,340)**. 허용 구역 이외 픽셀 변경 **0**. 기존 귀·눈·몸통·흰 털 명암·검은 꼬리끝·발·접지·애니메이션 순서 그대로.
- 원래 Attack4 파일과 승인 KeyPose·Idle4를 재저장하거나 덮어쓰지 않음. 신규 **v2 수정본을 별도 파일**로 납품.

## 2. 실제 납품 / 무결성

- 신규 게임용 후보 **딱 1개**: `Sprite/SnowStoat_M2_Attack4_MouthFix_v2_2048x512_RGBA.png`, 2048×512 RGBA, 가로4셀×512×512, 우향. SHA256 **`5175483e486889736f4f89402ec8a04595491c5c587e51a3f6bbd1365391364a`**.
- 압축 `Stoat_Attack4_MouthFix.zip`: **852,864 bytes / 11 entries**, SHA256 **`76e27b0c8d4fcfab09a11f857235755ce759a97572ae96bbb8641ea990388cb4`**. 압축 CRC **PASS**, `SHA256SUMS.txt` 10/10 **PASS**, Manifest 포함.
- 추가 **변경 프레임 QA 컷 2장**, `Preview/Mouth_Before_After_Frames02_03_OFFLINE.png` 실제 이전/이후 입 부분 확대 비교, `Preview/Attack4_MouthFix_512_128_40_OFFLINE.png` 및 실제 128/40px 투명축소, 참고 8FPS GIF. Preview는 **OFFLINE**, 실제 Unity GameView 아님.
- 기존 Attack4 원본 SHA256 `a5f5856115473329d1b1a00a5db1022f1831ec48955e5e34f29b711dfb12141a`.
- 승인 KeyPose SHA256 `fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778`.
- 승인 Idle4 SHA256 `ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633`.

## 3. 전달 및 승인 게이트

- 실제 ZIP, 시트 PNG, 전후 비교판을 **ChatGPT Library `/LIMITLESS/Issue100_SnowStoat_Attack4/`에 업로드하여 재조회 확인**, 채팅에도 Sandbox 링크 제공. GitHub에 PNG/ZIP 바이너리를 올렸다고 주장하지 않음. 사용자 PC `F:\` 로컬 자동 복사X.
- 제작 방식: 기존 승인 소스 기반 국소 픽셀아트 입/턱 수정. ImageGen 실험 재렌더는 **전체 재작화되므로 납품 시트로 사용하지 않고**, 외부 자산을 가져와 섞지도 않았음.
- `Attack4 MOUTH_V2_USER_ART_REVIEW_PENDING`; 기존 KeyPose·Idle4 승인 유지. 대표 승인 전 Hit/Defeat 제작 금지, Unity6000.6.5f1 Import/Animator/Scene/Prefab/Save/UserData/Discord/Issue85 HOLD/원격 main 직접 커밋·Push·Merge 변경0.
- 게임용 PPU·Pivot·타격 타이밍/실 플레이, 기존 전체 아트의 렌더 동등성은 이번에 추가 검증하지 않음.
