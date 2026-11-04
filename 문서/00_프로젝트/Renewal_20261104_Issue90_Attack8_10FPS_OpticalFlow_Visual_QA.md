# LIMITLESS / 리뉴얼 아트 / Issue #90 — Organic ForestSpider Attack8 오프라인 10FPS 관절·접지 표적 검수

**검수일:** 2026-11-04 (KST) · **브랜치:** `docs/art-renewal` · **작업 범위:** 신규 중간 5프레임 미술 검수만. **새 PNG 생성 0 / 기존 파일 수정 0 / Unity 실행·적용 0**.

## 1. 최신 기획·실물 근거

- [Issue #90 최신 기획 검수](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622459940) 및 [Attack8 실물 납품](https://github.com/HappyPaul812/Project-Limitless/issues/90#issuecomment-6622194831), `WORLD_ART_RENEWAL_ISSUE90_ATTACK3_DELTA_20261104.md` 확인.
- ChatGPT 라이브러리 `/LIMITLESS_Art_Delivery/LIMITLESS_SPIDER_ATTACK8_ISSUE90_20261104.zip` **실제 파일** 재취득하여 독립 확인: **5,922,807 bytes / SHA256 `e612a88a6d4c6b4422cc025c0d32fb34e4ffc2f40337ffa909a1885cfae34041` / CRC PASS**. 기존 납품 ZIP 원본 변경 없음.
- 전체 독립 `512×512 RGBA` PNG 8개 순차 판독, `128px` 및 `40px` GIF 각각 **8프레임 × 정확히 100ms = 10FPS** 디코드 검사. 원본 시트와 GIF는 이번 보고에서 **재작화·재출력하지 않음**.
- 기존 승인 키포즈 실제 SHA 유지: 00 Prepare `439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2`, 03 Contactv2 `0d0e24c2fae579f2f944255fe3bd3841216299b14f439cc3612906269020f697`, 06 Recoverv2 `0501a452467a07f34688abdfff5ea5ef49b0dfd93e1c99bdc368229986d44096`. 정지 v5 SHA `c600cd2264f6a0323771288666d16b49aa213ad938036d0746ed6f7f1d50adb9`, Idle8·Walk8 승인 상태 유지.

## 2. 실제 10FPS 프레임별 판정

| 슬롯/역할 | 판정 | 육안·픽셀 근거 | 최소 후속 |
|---|---|---|---|
| **00 Prepare** | **APPROVED_KEEP** | 사용자 승인된 원본 | 변경 금지 |
| **01 Windup** | **KEEP_CANDIDATE** | 준비 자세의 실루엣 연결과 다리 축 비교적 자연스러움. 00→01 겹침률(IoU) **0.861**, 유의미한 분리 소조각 없음 | 이번 교정 대상 제외. 01→02 접속만 후속 확인 |
| **02 Lunge** | **ART_REVISE** | 앞쪽(화면 오른쪽) 다리가 가늘고 길게 변형되며 어깨→무릎→발끝 연속성 약화. 01→02 IoU **0.772**, 알파>16 면적 **74,803→66,721(−10.8%)**, 오른쪽 bbox 끝 **x502**(512 캔버스 경계는 아직 미접촉) | **오른쪽 전방 다리/관절 부위만** 재작화·형상 보완; 승인 03 고정 |
| **03 Contact** | **APPROVED_KEEP** | 사용자 승인 v2 | 변경 금지 |
| **04 Recoil** | **ART_REVISE** | 오른쪽 다리 끝 옆 **분리 알파 31px**, bbox **x491..496, y278..286**. 광학흐름 중 회수 다리 조각 분리·관절 연결 불명확 | 오른쪽 다리 끝의 끊어진 조각/관절만 수정, 03 고정 |
| **05 Return** | **ART_REVISE 우선** | 왼쪽 다리 근처 분리 알파 **17·14·3px**(x78..86, y308..330). **04→05** 실루엣 IoU **0.720**, 알파 무게중심 약 **(+11px, −7px)** 이동. 빠른 프레임 점프/지지발 미끄러짐 위험 | 왼쪽 지지 다리·발끝과 몸통 복귀 동작만 표적 수정, 06 고정 |
| **06 Recover** | **APPROVED_KEEP** | 사용자 승인 v2 | 변경 금지 |
| **07 IdleBridge** | **ART_REVISE** | 오른쪽 지지다리 근처 **분리 알파 7px**(x459..461, y309..314). 06→07 다리/몸통 위치가 이동하며, 07→Idle00 접속도 모션 시각 점검 필요 | 오른쪽 발/무릎·Idle 복귀 실루엣만 수정, 06·Idle 원본 고정 |

**용어 주의:** 이 표의 IoU·알파 무게중심은 이미지 실루엣의 측정값입니다. 실제 몸통 Pivot·여덟 다리 관절 좌표·물리 지면고정값이 아닙니다. `WALK_FOOT_LOCK_PASS`·`ATTACK_TARGET_CONTACT_PASS`를 주장하지 않습니다.

## 3. 보행/연결 검토의 경계

- **02 → 03:** 02의 전방 다리 얇아짐은 Contact03의 승인 관절 형태와 10FPS 연결 시 튀는 위험. **02만** 수정.
- **03 → 04:** Contact의 전방 공격 다리가 뒤로 접히면서 04 오른쪽 끝에 떠 있는 작은 조각 발생. **04만** 수정.
- **04 → 05:** 이번 공격 모션 내에서 실루엣 IoU **0.720**으로 특히 큰 변형 구간, 왼쪽 다리 조각과 지지발 이동 후보. **05 우선 수정**.
- **06 → 07 → Idle:** `07→승인 Idle00` 실루엣 IoU **0.752**. 원본 GIF는 8슬롯 뒤 자동으로 **07→Attack00**으로 루프되어 이 경로의 IoU **0.651**로 더 크게 튀지만 **그 GIF 루프가 실제 게임 Runtime 전환 계약은 아님**. 본게임 종료 후 Idle 복귀는 별도 QA. 07만 국소 교정.
- 실제 전열/후열 표적 접촉, 공격 피해 타이밍, 8다리 지면 고정, PPU256/Scale0.7 게임 내 10FPS 재생 **전부 NOT_VERIFIED**. 본 회차에 Unity 실행 금지 조건 준수.

## 4. 보호·후속 발주 명세

- **수정 대상을 `02·04·05·07` 네 프레임으로 한정**. 신규 01은 `KEEP_CANDIDATE`; 승인 00·03·06 **BYTE KEEP**, 원본 v5·Idle8·Walk8 **보호**. 전체 Attack8 재작업 금지.
- 기존 VenomBee v3의 `RIGHTS_HOLD`, 승인 Owl/Chapter2 공식 아트·Wolf/Bear/Fox·Chapter4 메카닉 Spider 후보 보호. **Shoot8 제작0, Unity/Scene/Prefab/MonsterDefinition/GUID/meta/Spawn/AI/독/Drop/Save/UserData/main 변경0**.
- 이번 회차 **신규 애니메이션 PNG 제작·기존 프레임 수정 0**. 문서 보고용 오프라인 검수 증거만 생성한 것으로, 현행 Attack8은 사용자 최종 모션 승인 대기.
- **검수 증거 ZIP** `LIMITLESS_ATTACK8_ISSUE90_MOTION_QA_20261104.zip` — **295,072 bytes / SHA256 `045fed92f86b50b2b4ea1eda3b95b397fa1219171c7702846934daa7fa63b2e8` / CRC PASS**. 내용: 실물 02/04/05/07 확대판, SHA·프레임별 실루엣 수치, 검수 요약, **원본 40px 10FPS GIF 바이트 그대로**. 실제 게임 아트 PNG 수정본은 포함하지 않음.
- **최종 판정:** `ATTACK8_8PNG_FILE_TECH_PASS / GIF_10FPS_VERIFIED / APPROVED_00_03_06_BYTE_KEEP / NEW01_KEEP_CANDIDATE / TARGET_REVISE_02_04_05_07 / FOOT_LOCK_NOT_VERIFIED / TARGET_CONTACT_NOT_VERIFIED / USER_MOTION_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED`.

**후속 순서:** 바울 대표의 표적 수정 범위 확인 → **02·04·05·07만** 필요 부위 재작화 → 승인 키포즈 바이트/10FPS GIF/40px 재검증 → 대표 최종 모션 승인. 이 과정에서도 기존 프로젝트 원본·main 직접 Push·병합 금지.
