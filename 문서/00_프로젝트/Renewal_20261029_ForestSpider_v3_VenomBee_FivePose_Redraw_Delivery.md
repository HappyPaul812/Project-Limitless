# LIMITLESS / 2026-10-29 — 숲거미 v3·독침벌 독립 5포즈 후속 정식 납품

**담당:** Chapter 1·2 리뉴얼 / 브랜치 `docs/art-renewal` / [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60) · [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63). [Codex 10/29 통합 QA](https://github.com/HappyPaul812/Project-Limitless/issues/74#issuecomment-6489115424)와 기존 [Spider v2/Bee 5키포즈 보고](Renewal_20261029_Spider_v2_Bee_5_Keyposes_TargetedFix_Report.md) 및 [최신 인계](팀운영/인계/WORLD_ART_RENEWAL_LATEST.md) 확인 후 다음 보완. 본 문서에는 **추측성 PASS를 기록하지 않는다**.

## 1. 납품 실물 — 오프라인 아트 바이너리

- ZIP `Limitless_ForestSpider_v3_VenomBee_5RedrawnKeyposes_20261029.zip`
- 생성 작업 컨테이너: `/mnt/data/Limitless_ForestSpider_v3_VenomBee_5RedrawnKeyposes_20261029.zip` — **7,539,712 bytes / SHA256 `779da207c79be9193e5d23f88901b6b9f863b4ebbc53b30e82eb1c7392a12439` / ZIP 35 entries / CRC PASS**.
- `Evidence/Manifest.json`의 33개 페이로드 파일 **SHA256 33/33 일치**, `Evidence/SHA256SUMS.txt`의 34개 체크 항목(Manifest 포함) **34/34 일치**, PNG·GIF 디코딩 및 각 프레임 치수 검사 PASS. 실제 SHA는 생성 후 파일을 열어 재계산했다.
- 이번 산출물은 채팅의 ZIP 다운로드만 제공. **Windows `F:\Downloads`에 자동 복사하지 않았고, GitHub에 PNG/GIF/ZIP을 업로드하지 않았다**. GitHub에는 Markdown 문서만 등록.
- 직접 비교 자료(게임 아님):
  - `Evidence/ForestSpider_v2_v3_512_128_40_Comparison_OFFLINE.png` SHA256 `4a504e377a086dd2466f2a1e012cad56a27b9ec426db2bc05eef1b5fd94bf64b`
  - `Evidence/VenomBee_5Poses_PreviousVsNew_128_40_OFFLINE.png` SHA256 `9c3890cf3a9f8c200549acd03f90a57d4f29b6326fc733993c574132851b2e09`
  - `Evidence/Chapter2_Official_Style_Paths_Comparison_PENDING.png` SHA256 `6bf1174b38c56ca15fc781c68ee1f912af72f4f85010d9fdde48ee08760bdadc`. **이 마지막 자료는 공식 Chapter2 파일의 경로 목록만 기록한 확인 대기판**이며 공식 아트 PNG 픽셀을 인용한 실제 스타일 비교판이 **아니다**.

## 2. 숲거미 forest_spider — 새로운 v3 재구성 1포즈

- 새 `Spider/ForestSpider_v3_REPAINT_CANDIDATE_512_RGBA.png` **512×512 RGBA** / SHA256 **`d7ad4062eea84d6520e672b46630a26cfa92f54e55d587d1da936ac33bc50fc4`**.
- 이전 v2 `ForestSpider_OriginalPilot_v2_TargetedTexture_512_RGBA.png`와 파일 바이트 변경 없이 `Spider/ForestSpider_v2_REFERENCE_KEEP_512_RGBA.png`로 참조 복사. 원본 v2 SHA는 이전 검증 `3dedf30e2d5e95e299016980bec1c31456f4c3c796cd2222da948eeab92784ca`; 신규 v3가 이를 덮어쓰지 않았다.
- **단순 노이즈가 아닌 별도 생성 재묘사**: 기존의 녹회색 갑각·주황 눈·복부 갈라진 주황 별형 문양·8다리 기계관절 인상을 유지하면서 판금 균열/광택/마모, 얼굴 명암과 관절의 앞뒤 구분을 새로 그림. 단 **실루엣과 부품 비례가 v2와 완전히 같지는 않음**: 사용자가 요청한 '최대한 보존' 적합성은 미술 검토 필요.
- `Spider/ForestSpider_v3_Preview_128.png`, `...Preview_40.png`: 128×128 / 40×40 **투명 PNG**, 1:1 캔버스에 원본 종횡비를 보존해 축소(별도 Unity PPU/카메라 검증 아님).
- 공식 Ch2 참조 위치는 main의 `Unity/Client/Assets/_Project/Art/Monsters/11_SootHound`부터 `15_EmberWraith`까지 5개 공식 Sprite Sheet. 이번 파일 환경에서는 공식 PNG 바이트를 직접 인출하지 못하였으므로 **실제 동배율 Ch2 원화 비교 `HOLD_OFFICIAL_PIXEL_REFERENCE`**.
- 이전 외부 `ethanf30games / 2D Spider` 32프레임 원본과 본편 리소스는 불변. 기존 `forest_spider` Idle/Walk/Attack/Shoot 각8(총32), 10FPS, 우향, PPU256/중앙 Pivot, scale0.7, Field02 4 + Field03 1 Spawn 계약 유지. **신규 애니메이션 0/32**.
- 판정 `TECH_FILE_PASS / SPIDER_V3_DESIGN_REVIEW / SPIDER_ANIMATION_HOLD`. 바울 대표 디자인 승인이 선행되어야 하며 이전 v2를 자동 폐기하지 않는다.

## 3. VenomBee — 5개 별도 작화 포즈 + 2/3프레임 검토 GIF

- 기존 오리지널 v1 마스터와 `Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip`의 **Idle10+Attack10=기존20프레임 원본 바이트 그대로**. 이번 작업은 기존 5키포즈 국소 픽셀 워프를 다시 반복하지 않고 **한 번에 새로 그린 독립 5포즈 원화**를 각 연결 영역별로 분리, 프레임 캔버스에 비율 보존 배치한 신규 후보이다. 원본 PilotBee 외부 PNG 트레이스/색상변경이 아니다.
- Idle 후보 (우향, **238×215 RGBA**, 현행 Idle 10FPS 계약 참고):
  1. `Bee/Idle/VenomBee_Idle_Key01_WingsFolded_238x215_RGBA.png`, SHA256 **`aa4e85942704b9c14192ed787073f85c4bcca788d509a5c708be412e54dddd1f`**.
  2. `Bee/Idle/VenomBee_Idle_Key02_WingsSpread_238x215_RGBA.png`, SHA256 **`fdba594ccaf3cc50f38e25d0e18487ced1b70b634c05f62239f15d009cbbc759`**.
- Attack 후보 (우향, **315×253 RGBA**, 현행 Attack 10FPS 계약 참고):
  1. `Bee/Attack/VenomBee_Attack_Key01_Prepare_315x253_RGBA.png`, SHA256 **`2b78256f9d8c2d6813447b4f65fb9fd4c5b466e0e6df9f1b121ef23e4141965e`**.
  2. `Bee/Attack/VenomBee_Attack_Key02_Sting_315x253_RGBA.png`, SHA256 **`03baaaf637dc196b6ca89b69efcf536799b813837b078ee6c93182d2b19b62c3`**.
  3. `Bee/Attack/VenomBee_Attack_Key03_Recovery_315x253_RGBA.png`, SHA256 **`6ac34146367c5fcf7b35dba41788d61269149e7df2eb63836246928326b96fed`**.
- 실제 각 포즈 PNG를 종횡비 유지하여 별도로 **128×128·40×40 투명 PNG** 생성. 이번 40px 미리보기는 과거 Codex가 지적한 '원래 238×215 / 315×253의 종횡비 늘림' 결함을 반복하지 않음. 하지만 실제 게임 PPU·WorldSize/Renderer 실측은 아직 하지 않았으므로 GameView PASS 아님.
- `Bee/VenomBee_Idle_Keyposes_REVIEW_ONLY_10FPS.gif`는 **2장×100ms**, `Bee/VenomBee_Attack_Keyposes_REVIEW_ONLY_10FPS.gif`는 **3장×100ms**의 반복 검토용 GIF. **Idle10/Attack10 완성 모션이나 10개의 독립 키프레임으로 오해 금지**.
- 재묘사 포즈에서는 날개 각도, 다리 뻗기, 복부 전진, 독침 찌르기/회수가 앞선 전체회전·픽셀워프보다 뚜렷해졌다. **그러나 5개 별도 이미지 사이 동일 관절·실루엣·스케일의 프레임 연속성과 공격 후 자연 복귀, 생성 이미지의 적황색 알파 잔상/발 개수는 수작업 교정·실전 검증이 필요하다**. PNG 추출에 섞인 소형 중복 Thumbnail은 제거하여 재생성한 최종 파일을 해시 재검증함.
- 현행 활성 우향 `Idle10 (238×215×8열×2행), Attack10 (315×253×10열×1행), 10FPS, visualScale0.85`. `sourceFacesRight`, flip·PPU·Pivot·실제 타격 타이밍 검증 전 **`ART_REVIEW / MOTION_CONTINUITY_HOLD / UNITY_RUNTIME_NOT_TESTED`**.
- 원래 PilotBee 외부 `Jana Ochse / 2D!PIXX` 웹 **CC BY3.0** vs 동봉 `LICENSE.TXT` **CC BY4.0** 충돌은 미해결 그대로. 신규 오리지널 후보가 생겼다는 사실만으로 기존 활성 외부 원본 권리 문제가 해결되었다고 주장하지 않는다.

## 4. 미술 승인·기존 기능 보호·후속 계획

| 검증 | 판정 | 설명 |
|---|---|---|
| ZIP/PNG/GIF 무결성/규격 | **TECH_FILE_PASS** | 35 entries CRC, Manifest 33/33, SHA256SUMS 34/34, PNG RGBA 및 프레임 치수 확인 |
| Spider v3 디자인 | **USER_ART_REVIEW_PENDING** | 1포즈 후보, v2와 실루엣 비례 일부 차이, 40px 판독 및 Ch2 공식 실제 비교 미승인 |
| Bee 새 5포즈 | **POSE_REVIEW_PENDING** | 5개 서로 다른 새 원화, 후반 키포즈 동일 모델 연속성/관절/알파 확인 필요 |
| Ch2 공식 동배율 스타일 | **HOLD** | 공식 PNG 실제 파일 인출 실패, 확인대기 파일명 보드만 수록 |
| Unity GameView / 런타임 | **NOT_TESTED** | 본편 적용·Sprite/GUID/Prefab/Animator/FrameLoader/Save 모두 미변경 |
| Spider 32프레임 / Bee 전체20 재제작 | **NOT_STARTED** | 요청한 추가 양산 금지 준수 |

**후속**: 바울 대표는 디자인 승인 여부만 판단; Codex가 이 납품 ZIP을 읽기 전용으로 확인해 **원본 공식 Chapter2 Sprite 실물 및 기존 Spider/Bee와 동일 PPU/렌더러 비교**, 새 Bee 후보의 2→3포즈 연속성·조인트/날개/스팅 및 alpha fringe 표적 QA를 수행. 필요한 프레임/관절만 후속 수정하고 **본편 적용·main Push/병합은 바울 대표 별도 승인 전 금지**. 기존 Wolf/Bear/Fox, Chapter2 공식 몬스터·보스·승인 환경, NPC3, 사용자 Unity 원본/Scene/Prefab, MonsterDefinition·기존 Sprite GUID/meta·독/드롭/AI·Spawn/Quest·Save/UserData 및 main **수정 0**.
