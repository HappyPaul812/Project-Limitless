# LIMITLESS / Chapter 1·2 Renewal Art / Issue #97 — VenomSnake v3 Idle4 + Attack4 실제 애니메이션 파일 납품

**작성일:** 2026-11-10 (KST)  
**담당 브랜치:** `docs/art-renewal`  
**상태:** `VENOMSNAKE_ANIM8_ART_CANDIDATE / 8_FRAMES_FILE_TECH_PASS / USER_ANIM_VISUAL_APPROVAL_PENDING / NOT_UNITY_APPLIED`

## 1. 기준과 승인 원화 보존

[Issue #97 최신 제작 계약](https://github.com/HappyPaul812/Project-Limitless/issues/97), `START_HERE.md`, `COMMON_RULES.md`, `WORLD_ART_RENEWAL_LATEST.md`를 확인. 바울 대표가 최종 미술 승인한 `VenomSnake_KeyArt_v3.png` **1254×1254 RGBA**, SHA256 **`7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0`**를 **바이트 원형 보호**, 실제 복사본을 ZIP `Evidence/APPROVED_V3_ORIGINAL_BYTES_KEEP.png`에 넣어 SHA 일치 재검증. 기존 v2·구형 32px Sprite를 트레이싱/리컬러하지 않음.

**정본 코드 계약:** 원격 main의 `04_VenomSnake.asset` 기준 monsterId `venom_snake`, visualSize `(0.9,0.75)`, 구형 Idle·Attack 시트는 같은 Texture GUID `4eebc05f001d494daecb632a39bcfb95`, **32×32 cell / 4columns / 각4frames / 8FPS / sourceFacesRight=false**. 신규 원화는 **우향(+X)**이므로 기존 코드의 좌향 플래그와 차이가 있다. 이 버전에서 Unity importer, Sprite GUID/meta, MonsterDefinition, PPU/scale/pivot/Flip을 일절 건드리지 않았으며, **게임 적용 가능한 동일 배율**로 확정하지 않는다.

## 2. 신규 생성 아트와 실제 8개 독립 파일

기존 승인 v3의 굽은 S자 목/복부 비늘/몸통 코일, 흑갈색·올리브색 독성 패턴, 누런 눈, 벌어진 입과 독니 디자인을 참조하여 **8개의 별도 생성형 원화를 각각 1254×1254 RGBA로 제작**. 이 원본들을 `Sources_Generated1254/`에 각 1장씩 복사하고 원본 SHA를 `Evidence/QA_Spec.json`에 기록. 단순 이동/상하 색변화 복제본이 아니라 별도 원화 8장. 이들을 **원본 각 전체 1254 캔버스를 480×480으로 축소, 512 투명 캔버스 16px 여백 안에 배치**하여 새 애니메이션 마스터 8장으로 납품. 승인된 v3 PNG를 변형해 대체한 것은 아니다.

| 슬롯 | 동작 | 512 RGBA PNG SHA256 |
|---|---|---|
| Idle00 | 중립·호흡 기점 | `ab03751c9ab91569c56dbb775730829a63e5c7d61f99ce29b349d63e2694fcac` |
| Idle01 | 흡기·목 긴장 | `d60e3f25c6acb2c359e0fa1452489ac731afe32f7e1669f9952c9582272e51da` |
| Idle02 | 호기·머리 미세 전진 | `710463c54fe6484fc12393fe7f571597ccd38ccfb68e1d17e79508146a1da420` |
| Idle03 | 초기 대기 포즈 복귀 | `ffb2cec3a2d7c6327c7f584dd675befcf4052b898cdad4c045331e9bbf976258` |
| Attack00 | 코일·목 수축 준비 | `cb485208e5ab663ac7872ae4d496af466bd155684b5ae92651907b665b777d2c` |
| Attack01 | 우향 목/머리 돌진 | `7b2dde6e54799eb8545fb48ab51b7cd72d3f3185603f1909c0e7c4ce69d25b10` |
| Attack02 | 독니 최대 타격 | `db3369eb1ba733f25a84fd373e598f5ab6f926c23b09a3e1e7a5dba9ba582efa` |
| Attack03 | 목 회수·복귀 | `f3db2ee1e5e8da9803c0a0735b676f1442ec51084828d3eaad82fe4198cc257d` |

8개 512 PNG **서로 다른 SHA**, 전부 RGBA, 네 변 최외곽 가시 알파=0. 그러나 **원본 1254 생성 이미지에서 코일 아래쪽이 소스 경계까지 닿는 경우가 있어** 512 마스터 여백 확보는 더 이상 캔버스 클리핑이 없다는 뜻일 뿐 이전에 잘린 아랫비늘을 복구했다는 뜻이 아니다.

## 3. 스프라이트 시트·8FPS·실크기 축소

- **512px 셀 마스터 통합** `Sheets_512/VenomSnake_Anim8_4col2row_512cell_RGBA.png`: **2048×1024, 4열×2행**, 윗줄 Idle00~03 / 아랫줄 Attack00~03.
- **별도 시트:** `Sheets_512/VenomSnake_Idle4_4col1row_512cell_RGBA.png`, `...Attack4...` 각각 **2048×512**.
- **신규 게임크기 참고 시트:** `Sheets_Candidate128/VenomSnake_Anim8_4x2_{128,40,32}cell_OFFLINE_RGBA.png` 각각 512×256 /160×80 /128×64. **128/40/32 이미지가 32px MonsterDefinition에 그대로 사용할 수 있는 정식 PPU/Pivot 시트라고 주장하지 않음**. 128px 개별 참조 PNG도 8장.
- **GIF:** `Preview/VenomSnake_{Idle,Attack}4_{128,40,32}px_8FPS_OFFLINE.gif` 총 6개, 각 4프레임. GIF 단위시간이 **10ms**이므로 정확한 125ms를 기록할 수 없어 프레임 지연을 **120/130/120/130ms**로 교차 적용, **4프레임 500ms = 평균 정확 8FPS**. 각 GIF 프레임수·지연을 실제 디코드 확인. `VenomSnake_Anim8_128px_8FPS_STORYBOARD_ONLY.gif`도 8개 슬롯×교차 지연 비교용.
- `Preview/VenomSnake_Anim8_8Frames_Overview_OFFLINE.png`, `VenomSnake_128_40_32_PixelReadability_OFFLINE.png`, `VenomSnake_ApprovedV3_vs_Anim8_DesignAnchor_OFFLINE.png`: 승인 원화 대비·실축소·전체 포즈 비교. 게임 GameView 아님.

## 4. 모션 검수: 기술과 미술을 구분

- **정적 시트/규격/PNG/8FPS GIF/ZIP 검사:** PASS. 동일 512 프레임을 단순 재패키지한 8장은 아님.
- **Idle 인접 실루엣 IoU:** 00→01 **0.854**, 01→02 **0.875**, 02→03 **0.849**, 03→00 **0.825**. 숫자는 전체 alpha 중첩이며 실제 호흡 근육 정합·장면상의 자연스러움을 증명하지 않음.
- **Attack 구간:** 준비00→돌진01 IoU **0.690**, 돌진01→타격02 **0.783**, 타격02→복귀03 **0.664**. 특히 **Attack02→03의 목/턱 회수·코일 무게 연결이 큰 변화**이며 8FPS 재생 시 **ART_MOTION_REVIEW_PENDING**, 사용자 판단 필요.
- Attack GIF의 무한 반복은 **OFFLINE 검토 편의일 뿐 실제 런타임은 Attack 종료 시 Idle로 복귀**해야 한다. 32px 실제 게임 세계크기, 새 우향 원화와 기존 sourceFacesRight=false 충돌, 공격 실제 Hit 타이밍/범위 및 독/AI는 **NOT_VERIFIED**. 캐릭터 크기·PPU·Pivot/Flip/32px·40px 게임 내 가독성은 승인 후 Codex가 Unity 6000.6.5f1 **격리 QA**로 확인.
- **최종 미술 상태:** `USER_ANIM_VISUAL_APPROVAL_PENDING`. 제품 애니메이션 완료·본게임 적용을 선언하지 않음.

## 5. ZIP·SHA/Manifest·전달

**원본 포함 FULL ZIP:** `LIMITLESS_VENOMSNAKE_ANIM8_ISSUE97_20261110.zip`, **29,495,533 bytes / SHA256 `3baee658437ba56fb0becd59211b1ac1440456848b78e06e7c2fd0072d7fabdb` / 47 entries / CRC PASS / Manifest 45/45 SHA+bytes PASS**.

**간편 DELIVERY_ONLY ZIP:** `LIMITLESS_VENOMSNAKE_ANIM8_ISSUE97_DELIVERY_ONLY_20261110.zip`, **12,586,609 bytes / SHA256 `07f5922d89078b869b95abcb165a3f66355cc49a9ea63667f631c438bad74256` / 39 entries / CRC PASS / Manifest 37/37 SHA+bytes PASS**. FULL에는 1254 RGBA 신규 원화8 원본 추가 포함.

두 ZIP 모두 **ChatGPT 라이브러리 `/LIMITLESS_Art_Delivery/VenomSnake_Issue97_Anim8_20261110/`**에 저장하여 다시 목록 검증, 개별 512 PNG8·마스터 시트3·8FPS GIF4·비교판3 등 총 20개 항목 제공. 사용자 Windows `F:\Downloads` 자동 복사나 GitHub 바이너리 ZIP/PNG Push 없음.

## 6. 원형 보호 / 다음 게이트

- 바울 대표가 승인한 VenomSnake v3 고정 SHA `7001d199...` 불변. 구형 VenomSnake 현행 32×32 시트, `04_VenomSnake.asset`, AI·독·드롭·전투 수치, Unity 6000.6.5f1 원본/Scene/Prefab/GUID/meta·Save/UserData/`main` **변경0**.
- 기존 승인 Spider/Bee/Wolf/Bear/Fox/Ch2 공식 아트/배경 및 OGA06·OGA16 NPC 미술 파일 보호. **이번 회차 NPC 추가 제작0 / HP·MP 새 아이콘0 / 외부 원화 트레이스0**.
- **다음 단계:** 바울 대표의 8FPS Idle/Attack 시각 검토 및 애니메이션 승인/수정대상 지적 → 승인 시 Codex 최소 격리 Unity 동작/PPU·Flip·32×32 구계약·AI/독/Hit·Idle 복귀 점검 → 별도 승인 전 본편 적용/Push·merge 금지.

**결론:** `VENOMSNAKE_ANIM8_ART_CANDIDATE / EIGHT_INDEPENDENT_FRAMES_FILE_TECH_PASS / USER_ANIM_VISUAL_APPROVAL_PENDING / NOT_UNITY_APPLIED`.
