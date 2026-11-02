# LIMITLESS / 2026-11-03 — 유기형 ForestSpider 최종 알파 후보 + VenomBee 모션 준비 합동 납품

## 정본 / 사용자 승인 범위
- 브랜치 `docs/art-renewal`, 원격 main **`bfd281545632877325f5d21e04cefb924042c465`** 확인. `START_HERE.md`, `COMMON_RULES.md`, 담당 `WORLD_ART_RENEWAL.md`, `WORLD_ART_RENEWAL_LATEST.md`, [Issue #60](https://github.com/HappyPaul812/Project-Limitless/issues/60), [#63](https://github.com/HappyPaul812/Project-Limitless/issues/63), [#65](https://github.com/HappyPaul812/Project-Limitless/issues/65) 최신 댓글 확인.
- **중요 최신 승인:** 바울 대표가 유기형 숲거미 외형 방향을 승인([#60 댓글 6495054916](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6495054916)). **단, 최종 알파/실제 게임 및 32프레임 양산 승인 아님**. 이후 [Codex 왼쪽 앞다리 하단 잔상 보고](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6494824157) 근거로 최소 수정.
- 마을 NPC/Quest NPC polishing 후순위, 실사용 외부 전투 몬스터 오리지널화를 우선. 기존 공식 Chapter2, Wolf/Bear/Fox, NPC, Unity 원본/Save/UserData/Scene·Prefab·MonsterDefinition/GUID/meta/독/Drop/AI/Spawn, 메카닉 Spider v3(Chapter4 `ARCHIVE_ONLY`) 모두 변경하지 않음.

## 실물 납품
- ZIP: **`LIMITLESS_Spider_AlphaFinal_Bee_MotionPrep_20261103.zip`**, **5,501,345 bytes / SHA256 `56f194561895ad875de3d89d75385471ce6709780d4fb52ad4fa82e222739f48` / 30 entries / ZIP CRC PASS**.
- ZIP의 `Manifest.json` **28개 페이로드 파일 SHA256/bytes 전수 일치**; `SHA256SUMS.txt`와 `Evidence/QA_Results.json` 포함. 새 ZIP과 PNG는 현재 대화의 `/mnt/data` 산출물이며 **바울 대표의 Windows F:에 자동 저장하거나 GitHub PNG/ZIP을 Push한 것은 아님**.
- 기존 정상 파일 자체는 포함된 경우에도 **바이트 보존**, 새 원본 게임/Unity 파일 0.

## 1. ForestSpider — 유기형 v3_EDGE 표적 알파
- 입력(기존 채택 디자인/마지막 α후보): `ForestSpider_Organic_v2_CLEAN_ALPHA_512_RGBA.png`, **512×512 RGBA / SHA256 `c949f37ad5511753b064b83d521e79bb76409b0427cd0abf0c9742d6d9d46661`** (기존 SHA 동일, 새 ZIP `Spider/ForestSpider_Organic_v2_REFERENCE_KEEP_512_RGBA.png` 바이트 복사).
- 신규 **`Spider/ForestSpider_Organic_v3_EDGE_FINAL_CANDIDATE_512_RGBA.png`**, **512×512 RGBA / SHA256 `52d2e5fbf0090bf9925cd2fc295df1afd1e20ad6f1368dac10368d3e1fa8a137`**.
- 실제 픽셀 검사: **RGB 262,144 픽셀 중 변경 0**, 기존 알파보다 감소한 픽셀 **282**, 증가0. 영역은 왼쪽 앞다리 밑 밝고 납작한 가장자리 띠 및 인접 색빠진 마트에 집중. 다리·눈·복부·이끼·거미줄 RGB·원래 질감 재해석/새 오리지널 디자인/메카닉화0. 세세한 털/거미줄의 알파 경계는 최종 검토 필요.
- 비교판: `Evidence/Spider_v2_v3_512_128_40_OFFLINE.png` 실제 512/128/40비율, `Evidence/Spider_ThreeBackgrounds_512_OFFLINE.png` 실제 동일 PNG를 밝은/어두운/절차합성 숲바닥 3배경에 합성. 가상 배경이므로 실제 Field03 지면이 아님. 밝은/어두운 512 원본 보관. **최종 미술 알파 PASS / Unity 동일 PPU / GameView 미검증**. 상태 `USER_DESIGN_DIRECTION_APPROVED / ALPHA_LOCAL_TECH_PASS / EDGE_VISUAL_REVIEW_PENDING / UNITY_NOT_TESTED`.
- 현 외부 CC0 Spider Runtime Idle8/Walk8/Attack8/Shoot8(총32), 10fps/PPU256/우향, Field02·03 Spawn 변경0, **신규32프레임0**. 디자인 외형 채택이 애니메이션 발주/게임 교체 허가를 뜻하지 않음.

## 2. VenomBee — 전투 교체 전 단계 비교·미결 기술 계약
- 원본 자료 **`Limitless_Chapter12_ForestSpider_VenomBee_OfflineArtPilot_20261029.zip`**에서 실제 Idle10/Attack10 **20 PNG의 SHA256을 재계산**해 `Evidence/Existing20_SHA256_PRESERVE_REFERENCE.json`으로 제공. 원본 사용/수정 0. 이 20장은 **오프라인 애니메이션 초안**이지 현재 게임 런타임의 교체 승인 시트가 아님.
- 최신 별도 5키포즈 **Idle 접힘/펼침 2 (238×215 RGBA) + Attack 준비/찌르기/회수 3 (315×253 RGBA)**를 기존 `Limitless_ForestSpider_v3_VenomBee_5RedrawnKeyposes_20261029.zip`에서 **SHA 그대로** 읽어 `Bee` 폴더에 분리 보존. 10fps 2/3포즈 GIF 역시 원본 바이트 보존.
- 새 안내판 `Evidence/Bee_Existing20_ContactSheet_OFFLINE.png`은 20개 실제 프레임 전시, `Evidence/Bee_5Keyposes_128_40_RatioPreserved_OFFLINE.png`는 원본 종횡비를 유지해 128/40 뷰포트에 중앙배치. **프레임별 종횡비를 128×128 또는40×40으로 강제 변형하지 않음**.
- 현재 키포즈의 Idle 날개 접힘/펼침 대비, Attack 준비/전진/회수 대비는 이전 Warp 초안보다 개선됐으나, **우향 머리 vs 좌측으로 튀어나간 독침 축/공격 대상 방향**, 날개 뿌리, 6개 다리 관절, 복부 기준점, alpha fringe, 회수시 동일모델 연결성은 아직 `MOTION_REVISE_CONTINUE`. 이 단계 **5장 추가 재작화0**, 기존20 전량 재작업0. '준비 작업'은 5개 포즈의 문제를 특정하고 실제 수정 게이트를 정하는 단계임.
- 확장 계약: 먼저 **우향 공격 축과 몸체/침 기준점(현행 sourceFacesRight=1)**을 Codex 기술 검수와 사용자 미술 승인으로 확정 → 꼭 필요한 키포즈 날개 뿌리/앞다리/복부-독침의 독립 작화 → 필요한 기존20 프레임만 단계별 교체 후보 → 10fps 40/128 및 Loop/Recovery/Unity 격리 QA → 사용자 전투 교체 최종 승인.
- **라이선스:** 외부 `Jana Ochse/2D!PIXX PilotBee` OpenGameArt 웹 CC BY3.0 / 동봉 LICENSE.TXT CC BY4.0 충돌 **미해결**. 현행 외부 아트가 제품에 연결된 상태이므로 출처 보존/전면 교체 확인 전 해결로 간주하지 않음.

## 3. Chapter2 공식 아트 비교 — 현재 증거와 한계
- GitHub `Unity/Client/Assets/_Project/Art/Monsters/11_SootHound`, `12_HeatwindHawk`, `13_FissureLizard`, `14_EmberBeetle`, `15_EmberWraith` Sprite Sheet 5종 경로 확인.
- `Ch2_References/Official_Chapter2_ActualSprite_Online_Comparison_ONLINE.html`: GitHub 실제 공식 PNG URL을 브라우저에서 직접 불러오는 기존 Library 제공 뷰어를 추가. **접속/접근권한이 있어야 표시**되며, 네이티브 Unity 동일 PPU·셀 크기·CombatSlot 검증 증거가 아님.
- `Ch2_References/Ch2_LibraryMoodboard_vs_Bee_Style_REFERENCE_ONLY.png`: 라이브러리 아트 참고 보드와 5키포즈를 대조한 **비공식 화풍 참고판**. 실제 런타임 11~15 Sprite Sheet 동일배율/동밀도 **PASS를 주장하지 않음**. Codex가 공식 PNG를 로컬에서 확인할 수 있으므로 [#60 로컬 5종 보완판](https://github.com/HappyPaul812/Project-Limitless/issues/60#issuecomment-6490544447) 재활용/경로 확인을 우선.
- 정확한 최종 동배율 시각 판정은 Codex 격리 Unity 동일 PPU·Pixel Density 비교가 필요해 `CH2_SAME_PPU_COMPARISON_HOLD`로 기록.

## 최종 보호·상태
- 파일/규격/해시·ZIP 무결성 `FILE_TECH_PASS`, 숲거미 `DESIGN_APPROVED_BY_USER / EDGE_ALPHA_TARGETED_CANDIDATE / FINAL_ALPHA_REVIEW_PENDING`, 독침벌 `EXISTING_20_PRESERVED / 5_KEYPOSES_PRESERVED / MOTION_PLAN_READY / ATTACK_DIRECTION_REVISE / RUNTIME_HOLD`.
- **신규 애니메이션 생산0 / Unity·Save·Scene·Prefab/MonsterDefinition/GUID/meta·Spawn·AI·독/Drop/원본 게임·공식 Chapter2/펫/환경/main 수정0**. 브랜치 `docs/art-renewal`에서 문서만 기록. 대표 미술 최종 승인 필요; 본게임 적용은 별도 승인.
