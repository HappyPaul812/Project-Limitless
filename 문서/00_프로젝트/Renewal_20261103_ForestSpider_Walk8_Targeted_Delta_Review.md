# LIMITLESS / Chapter1·2 / Issue #60 — Organic ForestSpider Walk8 표적 수정 결과 (2026-11-03)

**작업 브랜치:** `docs/art-renewal` / **원본 게임 미적용** / **미술 재검토 필요**.

## 1. 이번 요청과 최신 정본

- [WORLD_ART_RENEWAL_LATEST.md](팀운영/인계/WORLD_ART_RENEWAL_LATEST.md), [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60) 및 [Codex 실제 3ZIP 통합 검수](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6594778265), [Codex Issue #60 세부 결과](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6594793586), [기획 후속 보완](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6594918071)을 확인.
- **확정:** 유기형 Spider v5 정지 원화 `ForestSpider_Organic_v5_FORELEG_CONTOUR_ALPHA_512.png` SHA256 **`c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`** `USER_ART_APPROVED`. 원본 바이트 수정 0.
- **기존 모션:** Idle8은 `KEEP_CANDIDATE`(최종 모션 승인 아님). Walk8은 40px에서 발 접지보다는 흔들림이 강조돼 `WALK_CONTACT_HOLD`. 기존 게임 `forest_spider`의 Idle/Walk/Attack/Shoot는 각 8개, **10 FPS**·PPU256·중앙 Pivot·우향·visualScale0.7. 8 FPS는 미리보기 후보였으나 이전 GIF의 120ms×8=8.333FPS 오류를 발견했으므로 이번 공식 비교는 **100ms×8=정확히 10FPS**만 제작. 기존 Runtime 설정 변경0.

## 2. 실물 납품 및 QA

- **ZIP 이름:** `LIMITLESS_WALK8_FIX.zip` (현재 이 대화 작업 파일)
- **ZIP 바이트:** **4,687,734** / **SHA256 `d840e5ddd8ac2666e4b0c7a9f4b2d0bdfc688b5ff03e01a0019ba7808d6b86b8`**
- ZIP 내부 **18개 파일**, CRC **PASS**, `Manifest.json`에 기재된 **16/16 파일 SHA256 일치**, `SHA256SUMS.txt` 수록.
- **새 산출물:** `Walk/ForestSpider_Walk_00.png`~`07.png` **8장 모두 512×512 RGBA**, `Walk8_4x2_512_RGBA.png` **2048×1024, 4×2셀/512**, `Preview/Walk8_128px_10FPS.gif`, `Preview/Walk8_40px_10FPS.gif` 모두 **8프레임 × 100ms 정확히 검증**. `Review/Walk8_Old_vs_New_128_40.png`, `Review/Walk_8Leg_PhasePlan_OFFLINE.png`, `QA/Walk8_ChangedPixels_And_SHA.csv`, `QA/QA.json`.
- **승인 원화와 Idle8 바이트는 손대지 않음.** 승인 v5 SHA와 기존 Idle8 각 SHA 목록을 QA JSON에 보존. `Walk00`과 `Walk04`는 원래 파일과 SHA **완전 동일**, 문제 후보 **Walk01·02·03·05·06·07만 수정**. 사용자 F:에는 자동 복사되지 않았고 PNG/ZIP 바이너리를 GitHub에 올리지 않음.

### 변경 실제 SHA·픽셀

| 프레임 | 이전 SHA256 | 이후 SHA256 | RGBA 변경 픽셀 |
|---|---|---|---:|
| Walk00 | `cc1b5670c27bda75ebe5e8dc720cfa893b97016f927e926caa3aa5c7aaa291e6` | 동일 | 0 |
| Walk01 | `6fb3b65c716cfe55eaa5bbc29452e7e1704724965a37afa902ea77074a9b0d2a` | `4dbd78009d3d6f6ee6a1ae1c0811f7d8684568cc1191c82a538602081d8c0bf8` | 67,113 |
| Walk02 | `85eb99c3c6ec445015d2070c792ac62cb49b5ad67dfe511b54fbe370ca6882ef` | `168369181f890acaebef2c9e898df105ae29133953b03dae1c0cb6b9b997748b` | 72,909 |
| Walk03 | `6a4b6636d1e21a7f7a100bcd8ae3fc6954525c380f10179ccebcd399cb721586` | `64a7297d2e43cdde8376e2d99efef97b2468660e2d9d4c9fca2941e0469d83e9` | 63,603 |
| Walk04 | `7de4a98be27b2f7f2021b83742e788f0c6f5e7e21474d9ec6fccdac0083df7e9` | 동일 | 0 |
| Walk05 | `4955cc56b1667769aeeaf14fe5230e0f985dd2374283584edd7292360c24f768` | `435ed6e6451a60f42670010b64cf78099ce1170fb925a11cb3e22bcc967b3785` | 71,876 |
| Walk06 | `0ad5e1447e0fca32436bf4efff32940805b78b3a38a0430246953d55cd0a1f58` | `9ed5850db957e476888d04482f729af163bf06438622ae8be2d1cecfac942223` | 80,129 |
| Walk07 | `96cd74dfde3ffe2c1bce8973ab4d961bec6b8e57b2048fcb82d540b5b7edc782` | `9155976e57b82bddbae21d218f8a42a5d931bd7502b89a812b265b6dccd09c20` | 68,958 |

## 3. 미술·접지 판정 — 기술 PASS와 미술 HOLD 구분

**수행한 수정:** 기존 화풍·투명 PNG의 본체를 그대로 활용하고 발끝과 무릎의 여러 위치에 서로 다른 이동량을 적용해 **1–3프레임 왼쪽 앞다리/오른쪽 뒷다리, 5–7프레임 오른쪽 앞다리/왼쪽 뒷다리의 들어올리기·스윙·착지를 단계별로 구분**. 발끝 기준 위치는 초록색(지지)/주황색(스윙) **OFFLINE 계획도**로 표시했으며, 40·128px 미리보기와 10FPS GIF를 제출.

**엄밀한 한계:** 실제 제작 방법은 관절·발끝 주변 **국소 가우시안 픽셀 리매핑**이며, 8개 다리를 독립 마스크로 수작업 재작화하거나 지면에 지지 발끝을 고정하는 물리적 IK 애니메이션 **완료가 아님**. 중간 관절 그림이 늘어나거나, 실제 게임 속 보행 방향·속도와 발 디딤이 맞지 않을 가능성 있음. 지원 다리의 표시 위치는 `CONTACT_PHASE_PLAN`이고 **실측 Unity Foot Lock PASS가 아니다**. Walk8은 사용자 미술 검토 후 **`WALK_CONTACT_ART_REVIEW / REVISE_POSSIBLE`**로 판정; 기술적 PNG/CRC/프레임/타이밍만 PASS. 동일한 문제를 지적받으면 추가 국소 워프만 무한 반복하지 말고 관절/접지점을 분리한 정식 키포즈 수작업 작화 또는 레이어별 IK 방식으로 전환해야 한다.

- **상태:** `V5_STATIC_ART_APPROVED / IDLE8_KEEP / WALK6_PIXELS_CHANGED / WALK8_FILE_TECH_PASS / WALK_GAIT_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_PRODUCTION_APPLIED`.
- **금지 유지:** 신규 Attack8·Shoot8 제작0; VenomBee 신규 작업0(표적 계약 미확정, CC BY3.0/4.0 충돌 HOLD), 기존 Chapter2 공식 몬스터·보스·Wolf/Bear/Fox·메카닉 Ch4 Spider v3, 원본 Unity/Scene/Prefab/Sprite GUID/meta/MonsterDefinition/AI·독·Drop·Spawn·Save/UserData/`main` 변경0.

## 4. 다운로드가 안 되는 바울 대표 PC용 별도 재현 경로

사용자가 여러 차례 ChatGPT ZIP 다운로드에 실패했으므로 **이번에는 별도의 GitHub 텍스트 재현 도구를 같이 기록**한다. [Walk8_Targeted_Rebuild_20261103.py](Walk8_Targeted_Rebuild_20261103.py)는 `docs/art-renewal`의 문서 부속 텍스트만 수정하며 **Unity 게임에 설치하거나 실행하지 않는다**. Codex는 이미 로컬에서 검증한 3ZIP 중 `LIMITLESS_Spider_Idle8_Walk8.zip` 혹은 동일 `spider_motion.zip`을 **격리 Temp에서만 읽고** 재현 스크립트를 실행해 6개 수정 PNG·시트·10FPS GIF를 만든 다음 SHA·모션 미술 검수를 진행할 수 있다. 의존성 `numpy`, `opencv-python`, `Pillow`. Python/Pillow 버전에 따라 PNG 압축 바이트 SHA가 다를 수 있으므로 **픽셀 동일성+원본보존 확인**이 최종 비교 기준이다.

**Codex 복붙 실행 계약:** `docs/art-renewal`에서 위 재현 텍스트를 읽고 소스 ZIP 경로를 탐색. 없으면 임의로 예측하지 말고 FILE_NOT_FOUND. 사용자 승인된 v5/Idle8/Unity original/main **변경 금지**. `Temp/Walk8Review`로 출력. 실제 게임 접지에 근거 없이 WALK_ART_PASS 주장 금지.

**최종 승인 필요:** 바울 대표의 **Walk8 모션 미술 승인** 및 별도 Codex 격리 10FPS/PPU256 실제 Unity 접지·무미끄러짐 검증. 둘 다 끝나기 전 게임 교체 금지.
