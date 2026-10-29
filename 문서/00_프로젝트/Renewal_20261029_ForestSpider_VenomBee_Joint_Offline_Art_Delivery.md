# LIMITLESS / 2026-10-29 / Chapter1 숲거미 + 독침벌 동시 리뉴얼 — 통합 제작·검수 보고

**상태:** `FILE_QA_PASS / FORESTSPIDER_DESIGN_PILOT_REVISE / VENOMBEE_MOTION_ART_REVISE / UNITY_NOT_TESTED / USER_ART_REVIEW_PENDING / NOT_PRODUCTION_APPLIED / MAIN_UNCHANGED`.

**정본 검토:** GitHub main `bfd281545632877325f5d21e04cefb924042c465`, `START_HERE.md`·`COMMON_RULES.md`·`WORLD_ART_RENEWAL.md`·`WORLD_ART_RENEWAL_LATEST.md`, [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60), [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63), [#65](https://github.com/HappyPaul812/Project-Limitless/issues/65). 바울 대표의 **최신 이번 발주**는 이전 숲거미 1포즈 게이트와 독침벌 20프레임 제작 보류를 **제작 범위에 관하여 갱신**했다. 단, 디자인 미술 승인이나 기존 Unity 본게임 교체 승인으로 확대해석하지 않는다.

## 1. 실제 납품 바이너리 (오프라인 원본, 이번 세션에서 생성·해시 계산)

- 파일: `Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip`
- 실제 압축 크기 **3,683,006 bytes** / **ZIP SHA256 `4b24310669e9db40b50e3d691acaf6077fab8b1102475e2dbaaddd5d57d1d82a`** / **35 entries / ZIP CRC PASS**.
- 전달 위치: 현재 ChatGPT 대화의 `/mnt/data/Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip`. Windows `F:\Downloads`에 자동 복사하거나 GitHub 저장소에 PNG/ZIP 바이너리를 Push한 것은 아니다.
- **실 PNG/시트 해시:**
  - `ForestSpider/ForestSpider_OriginalPilot_512_RGBA.png` — SHA256 `9a80f18c3793daa17aa2586c0effd19f9ecb72beaf654d70ba8f0af3392fb32e`
  - `ForestSpider/ForestSpider_OriginalPilot_Preview_128.png` — `106eccc224aaacc9e71498c3e36f797fdc7065791f693eb189b3602a1d8e0baf`
  - `ForestSpider/ForestSpider_OriginalPilot_Preview_40.png` — `9121790d784f0c0a8289b99ec97342ebc0998c6f2271124190e139de991e1dc8`
  - `VenomBee/VenomBee_Idle_10fps_238x215_CANDIDATE.png` — `075b7d9b5bb49f076c9e2a5c9be7316dc6f2fd583d0d104097bd2898a3b98531`
  - `VenomBee/VenomBee_Attack_10fps_315x253_CANDIDATE.png` — `700ac4c244f303ab93acfbaee0abe975cdc11a58edc245845e952a65bc55199a`
  - `Evidence/ForestSpider_VenomBee_DesignAndMotion_Comparison_OFFLINE.png` — `96064f5040d4b31e475b90a850f8a313700d2d0882b0b1ab630f8d7283f15a3d`
- 전체 35개 파일은 `Evidence/Manifest.json`에 파일별 bytes/SHA256, `Evidence/SHA256SUMS.txt`와 `Evidence/QA_Results.json`으로 전수 관리. 모든 PNG/GIF 디코딩 검증 및 ZIP CRC 검사 PASS. 위 ZIP SHA와 크기는 **재패키징 최종본** 기준이며 중간 시제품의 해시를 혼합하지 않음.
- 대표 판독 자료 `Evidence/Renewal_Joint_Report_20261029.md`, `Evidence/ART_README_AND_LIMITATIONS.md`도 ZIP 내부 포함.

## 2. ForestSpider — 원화 디자인 파일럿 제작, 애니메이션 HOLD

**현행 실사용 계약:** `MonsterDefinitions/03_ForestSpider.asset`, `forest_spider`, `MonsterAnimations/Spider/{Idle,Walk,Attack,Shoot}`; **각 8개=총 32개 PNG**, `animationFramesPerSecond=10`, `sourceFacesRight=1`, `visualScale=.7`, `PPU256` 중앙 Pivot. 기존 Idle336×226 / Walk359×229 / Attack325×228 / Shoot357×236 (각 폴더 첫 프레임의 PNG 헤더 기반; Codex 로컬 후속 전량 검증 참고). Field02 4+Field03 1 Spawn. [Codex #60](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6470843053)은 현행 32장과 기존 `ethanf30games / 2D Spider` CC0 원본 바이트 32/32 일치를 직접 보고했다. **오리지널 교체 확정**.

**이번 신규 원화:** `ForestSpider_OriginalConcept_Source.svg`(후보 벡터 작업 원본), `ForestSpider_OriginalPilot_512_RGBA.png` **512×512 RGBA 투명 1포즈**, `Preview_128.png`·`Preview_40.png`. 어두운 숲의 채도 낮은 녹회색 갑각/여덟 절지/독 송곳니/각진 주황색 눈·아랫면을 독자적으로 그린 **새 디자인**. 기존 외부 2D Spider 32PNG를 픽셀 트레이스·리컬러하거나 변형해 재사용하지 않음.

**판정 `DESIGN_PILOT_REVISE / MOTION_NOT_STARTED`**. 40px에서 거미 형상·다리 외곽은 시각 예비 판독 가능하지만, 벡터 질감이 기존 Chapter2 공식 몬스터의 자연스러운 페인팅/픽셀 밀도와 아직 어울리지 않는다. **사용자 시각 승인 필요**. 기존 우향 8×4=32 프레임 실교체는 준비되지 않음; 해당 애니메이션 파일 제작0. 디자인 확정 후 기존 각8프레임/10FPS/방향/PPU/Pivot·리소스 이름 규격에 맞춘 **독립 관절 동작 작화**, Codex 격리 QA 필요.

## 3. VenomBee — Idle10 + Attack10의 오프라인 모션 초안 생산, 작화 REVISE

**현행 실사용 계약:** `02_VenomBee.asset` 활성 원본 우향 `Idle10` **238×215 셀, 8열×2행**, `Attack10` **315×253 셀, 10열×1행**, **10FPS**, `visualScale=.85`; 기존 외부 PilotBee와 바이트 동일(Codex #64 확인). 제작자 `Jana Ochse / 2D!PIXX`. OpenGameArt `CC BY3.0` vs 패키지 `LICENSE.TXT CC BY4.0` **버전 불일치 미해결**, 기존 기록 보존.

**이번 산출:** 앞서 승인 전 후보로 보존했던 `VenomBee_OriginalKeyArt_Pilot_v1_512.png`를 참조 원화로 byte-preserving 포함. 이 원화 좌향을 **시각적으로 우향 반전**해 `VenomBee/Idle/VenomBee_Idle_00..09.png` **10장×238×215 RGBA**, `VenomBee/Attack/VenomBee_Attack_00..09.png` **10장×315×253 RGBA**, 정확한 8×2/10×1 **시트 PNG 2장**, 10fps GIF 2개를 오프라인 모션 초안으로 생성. 전 프레임 20개의 PNG 바이트는 각각 고유, 알파·시트 치수·가장자리 clipping 검사 PASS. **20개 생성은 사실이나 실전 품질 완성의 증거는 아니다.**

**작화 품질 판정 `MOTION_ART_REVISE / NOT_REPLACEMENT_READY`**: 대부분 원화 전체의 회전/부유/전진/약한 스케일 변화로 생성되었고, **날개·다리·독침을 별도 관절 작화한 정식 애니메이션이 아님**. 10FPS 루프/공격 동작이 생동감 있게 통과했다거나 기존 Battle 리소스와 교체 가능한 PASS로 주장하지 않는다. 40px 실전 배경에서 독침·날개맥 구별, PPU·Pivot/공격 연출/프레임 동작 QA도 남음. **추가 아트 REVISE 및 사용자 미술 승인, Codex 격리 Unity QA 후에만 교체 검토**.

## 4. 비교자료·검수 구분

- 신규 디자인 두 종의 512/128/40px 및 독침벌 20프레임 시간순 썸네일: `Evidence/ForestSpider_VenomBee_DesignAndMotion_Comparison_OFFLINE.png`. **OFFLINE** 정적 렌더로 실제 제품 GameView/전투 캡처가 아니며, 기존 활성 Spider/Bee의 정확한 원본 + Ch2 공식 몬스터 동배율 **동일 Canvas 실증 비교판을 첨부한 것도 아님**.
- 기존 #65의 실제 Unity 검토 Canvas `A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png`는 Codex 로컬 ZIP만 존재; 이 세션이 해당 원본을 직접 열어 품질 PASS 판정했다고 주장하지 않음. 기존 실게임 파일·Unity 씬/Prefab·MonsterDefinition/GUID/meta/Spawn/AI/Save는 이 회차 미접근·미변경.
- `PASS` = 새 파일 디코딩·치수·CRC/SHA와 독립 산출물 존재 같은 **정적 기술 점검**에 한정. `REVISE` = 실전 미술·동작 퀄리티 부족. `HOLD` = 실제 게임 교체 및 Spider 전체 모션. `NOT_TESTED` = Unity 실전 가독성·시각/시트 runtime 연결.

## 5. 다음 단계 / 보호

1. **바울 대표의 시각 검토:** Spider 1포즈 벡터 질감(Ch2 몬스터 대비) 및 Bee 동작 시트10+10에서의 실제 프레임 품질을 보시고 승인/표적 수정 판단. 현재 **디자인/실전 애니메이션 최종 승인 없음**.
2. **아트 보완:** Spider는 본격 질감·세부 도색부터, Bee는 회전·이동 복제에 그치지 않는 날개 박동·복부/다리/독침의 독립 20프레임 작화로 개선한 후 40/128 및 OFFLINE 기존 자산 비교판을 Codex와 완성.
3. **Codex 읽기 전용 → 격리 Unity QA:** 새 게임 적용 전 동일 PPU/스케일/방향/프레임 안정성·공격/독·드랍·AI/Spawn 회귀/접근성 검증. 본편 교체는 **바울 대표 승인 후 별도 발주**.
4. Wolf·Bear·Fox, Chapter2 공식 몬스터/보스/환경, 이전 NPC 시안, 원본 Unity/씬/Prefab/Resources/ThirdParty LICENSE, Quest·Save/UserData/기존 스프라이트 GUID/meta, `main` **전부 변경0**. 모든 신규 PNG/ZIP은 별도 오프라인 제작 경로에만 저장.
