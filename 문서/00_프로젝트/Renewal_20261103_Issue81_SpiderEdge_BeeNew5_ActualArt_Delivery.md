# LIMITLESS / 2026-11-03 — Issue #81 후속: 숲거미 왼앞다리 알파 표적수정 + VenomBee 실제 신규 5포즈

**담당:** Chapter1·2 Renewal, 브랜치 `docs/art-renewal`. [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60) / [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63) / [#81 Codex 실제 검수](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6590636705) 및 [기획 11/03 판정](https://github.com/HappyPaul812/Project-Limitless/issues/81#issuecomment-6590736542) 확인. 기존 `WORLD_ART_RENEWAL_LATEST.md` 정본과 공통규칙 준수.

## 결론

| 대상 | 이번 실제 작업 | 결과 |
|---|---|---|
| **ForestSpider Organic** | 기승인 v3 EDGE 후보의 지정 ROI `x35..95, y304..353`에서 **RGB 전부 동일/알파491픽셀 감소**만 수행한 신규 512 RGBA 1포즈 | `TARGETED_ALPHA_TECH_PASS / USER_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED` |
| **VenomBee** | 예전 10/29 키포즈 5장 복사/이름 변경 **아님**. 11/03 새로 그린(생성된) 5포즈 시트에서 각 독립 PNG Idle2·Attack3을 분리/안전한 RGBA 프레임 규격으로 변환 | **5/5 새 SHA 이전과 다름**, 자세 실물도 비교 가능. 하지만 `ATTACK_AXIS_ART_REVISE / MOTION_CONTINUITY_HOLD / NOT_COMBAT_READY` |
| **게임 적용** | 신규32/20 애니메이션 양산 및 Unity 본편 변경 **0** | `NOT_UNITY_APPLIED / MAIN_UNCHANGED` |

**주의:** 새로운 Bee 프레임 5개의 SHA가 달라진 것이 곧 **새 동작의 자연스러움·대체 가능 PASS는 아님**. 현재 새 Attack Sting도 복부 끝 독침이 화면 왼쪽으로 뻗는 점이 남아 있어 기존 우향 머리와 **공격축 충돌 미해결**. 같은 모델로 프레임 간 관절 위치, 6다리 개수, 날개 뿌리, 타격→회수, 붉은/노란 알파 외곽을 직접 검토하고 필요한 부분만 재작화해야 함. 기존 5장 중 정상이었던 파일·원본20장·원화 완전 보존.

## 납품 실물 / ZIP 무결성

- `LIMITLESS_ISSUE81_ART_DELTA.zip` — **2,444,044 bytes**, **35 entries**, **ZIP CRC PASS**, 실제 SHA256 **`d3ac1c2137a150e0b883e8fe3b236e71575915758620949ee8c84d8ceb1c1955`**.
- 실제 위치: 현재 대화 컨테이너 `/mnt/data/LIMITLESS_ISSUE81_ART_DELTA.zip`. 바울 대표 Windows F:에 자동 복사하거나 GitHub PNG/ZIP 바이너리 업로드한 것은 아님.
- 모든 파일별 바이트와 SHA256은 `Manifest.json` / `SHA256SUMS.txt`; 세부 검증은 `Evidence/QA.json`(PNG/GIF 실제 디코딩, 기존 대비 SHA 비교). ZIP 생성 후 재열기 `testzip() = None` (CRC PASS).
- **미리보기**: Spider v3/v4 512/128/40, 3색 배경 밝음/어두움/**합성 숲바닥**(실 Field03 Unity GameView 아님), 다리 영역 6배 확대. Bee 기존/신규 5포즈 비교, 40/128 종횡비 유지, Idle2/Attack3 **100ms/프레임 = 10FPS 검토용 GIF**, 공식 10+10 전투 시트 완성 아님.
- 공식 Chapter2 몬스터와 **동일 PPU 실제 PNG GameView 실측 비교는 이번 회차 수행하지 못함**; Codex #81 이미 확보된 OFFLINE 비교를 원본 별도 실물 검증 없이 재합격 처리하지 않음.

## ForestSpider 상세

- 기존 원본 `Spider/ForestSpider_v3_REFERENCE_KEEP_512.png`: 512×512 RGBA, SHA256 **`52d2e5fbf0090bf9925cd2fc295df1afd1e20ad6f1368dac10368d3e1fa8a137`**.
- 신규 단일 포즈 `Spider/ForestSpider_Organic_v4_ALPHA_EDGE_CANDIDATE_512.png`: 512×512 RGBA, SHA256 **`c9eedca2634baaae41b53af26f5c26e18b1abe3be7fe15fe35a717d0a7e8378d`**.
- 원본 RGB 전체 **262,144픽셀 전부 동일**, 알파 감소 491픽셀/증가0. 지정 ROI 내 회백색·연갈색 저채도 매트 색상과 경계 인접 픽셀만 선택적으로 알파 0/축소, 컬러아트·갑각·눈·다리/털·이끼·거미줄 RGB 변경0.
- 원본/결과 그림의 실제 픽셀에 기반한 `Evidence/Spider_Foreleg_ROI_6x.png` 및 합성 배경 3종 `Evidence/Spider_BeforeAfter_ThreeSyntheticBackgrounds.png`. **합성 숲바닥**이며 실제 Field03 지면 아님. 섬세한 털/웹 끝은 오프라인 시각 QA가 남아 있으므로 `FINAL_ALPHA_USER_REVIEW_PENDING`.
- 사용자가 **유기형 디자인 방향은 이미 승인**(Issue #60 댓글 6495054916), 이 회차는 방향 변경 아님. 이전 메카닉 Spider v3는 Chapter4 `ARCHIVE_ONLY` 그대로, 새 Idle/Walk/Attack/Shoot 32개 **제작0**.

## VenomBee 신규 포즈와 기존 SHA 대조

아래 5개는 기존 10/29 PNG를 다시 포장하지 않은 **새 생성 원화 시트 출처의 독립 포즈**. 캔버스는 기존 호환 목표(Idle 238×215, Attack 315×253), 우향 머리·10FPS 기존 계약의 오프라인 후보이며 Unity Runtime 호환 검증은 **NOT_TESTED**.

| 대상 | 새 PNG 경로(`Bee/New5/`) | 새 SHA256 | 기존 SHA256 (10/29) |
|---|---|---|---|
| Idle 날개 접힘 | `Idle_Folded_238x215.png` | `d17114176c13c2911cd533a0e7a363a1f4ae2ca110b540bcec5d63a2aa941605` | `aa4e85942704b9c14192ed787073f85c4bcca788d509a5c708be412e54dddd1f` |
| Idle 날개 펼침 | `Idle_Spread_238x215.png` | `d35f4583c0fa119396a40a6637f48b36b677fe00327640c655d86bde3dd47820` | `fdba594ccaf3cc50f38e25d0e18487ced1b70b634c05f62239f15d009cbbc759` |
| Attack 준비 | `Attack_Prepare_315x253.png` | `50c8047125e86bced17073fead14adaaa35545dea19beb9306f1c267481ea7e8` | `2b78256f9d8c2d6813447b4f65fb9fd4c5b466e0e6df9f1b121ef23e4141965e` |
| Attack 찌르기 | `Attack_Sting_315x253.png` | `17bd20da372f1453ec903228a0394148bf1a0000c3d6d714d0b2404c31863624` | `03baaaf637dc196b6ca89b69efcf536799b813837b078ee6c93182d2b19b62c3` |
| Attack 회수 | `Attack_Recover_315x253.png` | `9002a76ffa2b35b8aa34c35178e3cae4967992878e2df2b72db3e96e068d18be` | `6ac34146367c5fcf7b35dba41788d61269149e7df2eb63836246928326b96fed` |

- `Bee/Old5_KEEP` 폴더에 **기존 원본5장 그대로** 함께 납품, `Bee/New5`는 새 포즈 PNG5장과 128/40 프리뷰10장. `Evidence/Bee_Old_vs_New_5Poses_OFFLINE.png`에서 실제 이미지 비교.
- 이번 다섯 포즈는 **새로운 그림을 생성한 시트에서 추출**했으나 같은 캐릭터 모델/부품 연속성을 사람이 프레임별로 실제 재작화했다는 뜻이 아님. **부족한 부위/독침 축이 그대로**이므로 `ART_REVISE` 및 실전 대체 HOLD.
- 전체 Idle10+Attack10 기존 **20프레임은 `Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip`에 그대로 보존**. 이번 새 ZIP에 기존20장을 다시 중복 수록하지 않음.
- 외부 PilotBee 제작자 Jana Ochse/2D!PIXX, OpenGameArt 웹 CC BY 3.0 vs 동봉 `LICENSE.TXT` CC BY 4.0 버전 충돌 **미해결**.

## 권고/보호
1. 바울 대표는 Spider 왼앞다리 확대·합성 배경 및 Bee 5포즈 전후를 시각 검토. `FILE_TECH_PASS`이지만 `USER_FINAL_ART_REVIEW_PENDING`.
2. Bee는 **찌르기 포즈의 독침이 실제 우측 표적을 공격하도록** 디자인을 교정하고, 날개 뿌리·다리 관절·복부 연결을 표적 재작화. 그 뒤 동일 캐릭터 10FPS 프레임 사이 연결 검수. 사전 승인 없는 기존20 프레임 재생산·Unity 적용 금지.
3. GitHub `docs/art-renewal`에 본 문서만 커밋. Chapter2 공식몬스터·Wolf/Bear/Fox·승인 환경/NPC, Unity/Scene/Prefab, MonsterDefinition·MonsterSpawn/AI/독/Drop, GUID/meta, Save/UserData, main **변경0**. Git Push·main 병합 없음.
