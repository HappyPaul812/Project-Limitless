# LIMITLESS — 맹독뱀·Wolf 기존 v3 실물 재확인·128px 리뷰 납품 (2026-10-21)

## 0. 최신 결정 및 상태
- GitHub [Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43), [Issue #38](https://github.com/HappyPaul812/Project-Limitless/issues/38), 기획 정본 `docs/planning/문서/00_프로젝트/Renewal_20261021_Map_Closeout_And_MonsterWolf_V3_Resume.md` 및 `Chapter1_2_Monsters_And_Beasts_Original_Art_Renewal_Assignment_20261019.md` 직접 확인.
- **Field09 사용자 승인5+B 파생 Glow (PR #41) 원본 보호**; Field09 HeatVent/Spire, Field10 아트13 최종 미술검토는 환경 backlog. 이 세션에서 환경 아트 제작·수정0.
- 몬스터·펫: `V1_REJECTED / V2_QUALITY_HOLD / V3_SOURCE_PNG2_FOUND_AND_SHA_VERIFIED / V3_ART_REVIEW_PENDING / REAL_UNITY_SAME_SCALE_COMPARISON_NOT_VERIFIED / GAME_SPRITE_NOT_AUTHORIZED`.
- Codex는 Chapter3 서리들쥐 전투 v2 검수 중이므로 추가 Codex 호출0, 기존 게임·MonsterDef·BeastCompanionSystem·Save·Scene 수정0.

## 1. 기존 v3 실제 원본 PNG를 찾음 — 새 그림 생성 0
이번 ChatGPT 작업 컨테이너에 과거 납품 원본이 자동 마운트되어 실제 존재함:
- **VenomSnake**: `/mnt/data/LIMITLESS_KeyArt_v3/VenomSnake_KeyArt_v3.png`; RGBA **1254×1254**, **2,285,408 bytes**, **SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91a2d65dc099f8fd0`**. 이전 v3 보고와 해시 일치.
- **Wolf**: `/mnt/data/LIMITLESS_KeyArt_v3/Wolf_KeyArt_v3.png`; RGBA **952×1306**, **1,022,752 bytes**, **SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`**. 이전 v3 보고와 해시 일치.
- 원본2는 그대로 유지. 이번 검토 ZIP의 `Assets/`에도 **동일 바이트**를 복사해 SHA 대조. **새 v3 재생성/임의 스타일 변화/새 PNG로 대체 없음**.
- v3는 **1포즈 고해상도 Key Art**. 현재 사용되는 게임 스프라이트 해상도·PPU·Pivot·WorldSize/전투 애니메이션 규격과 호환된다는 의미가 아니며 대표 미술 승인도 없음.

## 2. Chapter2 공식 미술 5종 — 실제 원본 링크와 정확한 Unity meta 계약
공식 `main` 실제 Sprite Sheet 5종 파일 경로와 각 `.png.meta` 본문 직접 읽음:
1. `Unity/Client/Assets/_Project/Art/Monsters/11_SootHound/Soot_Hound_Sprite_Sheet.png`
2. `Unity/Client/Assets/_Project/Art/Monsters/12_HeatwindHawk/Heatwind_Hawk_Sprite_Sheet.png`
3. `Unity/Client/Assets/_Project/Art/Monsters/13_FissureLizard/Fissure_Lizard_Sprite_Sheet.png`
4. `Unity/Client/Assets/_Project/Art/Monsters/14_EmberBeetle/Ember_Beetle_Sprite_Sheet.png`
5. `Unity/Client/Assets/_Project/Art/Monsters/15_EmberWraith/Ember_Wraith_Sprite_Sheet.png`
- 5종 `.meta`: **실제 sprite rect 314×314, `spritePixelsToUnits: 314`, Pivot center (0.5,0.5)**, Grid cells 기준 각 **1×1 Unity world unit**. PNG 파일 자체는 GitHub에서 존재 확인. **GitHub 커넥터가 이 대형 PNG의 실제 바이트를 빈 content로 반환하여 컨테이너로 다운로드/실물 픽셀 비교하는 데 실패**. 따라서 공식 5종 이미지를 로컬에 가지고 있거나 PNG를 화면에서 실제 픽셀로 렌더·검증했다고 주장하지 않음.
- `Official_Chapter2_ActualSprite_Online_Comparison.html`에서는 **임의 생성 대체물이 아니라 위 실제 공식 GitHub raw PNG URL**을 외부 연결로 로드하여 각 원본 Sprite Sheet 상단 왼쪽 314×314 프레임을 CSS로 128px 칸에 보여주는 **실제 소스 링크 기반 비교**를 제공. 이 페이지는 사용자가 네트워크로 공식 PNG에 접속할 수 있어야 제대로 표시됨. **HTML로 배치한 128px 비교는 검증된 Unity 실제 동일 카메라/RenderScale 결과가 아니며, v3의 최종 PPU/WorldSize는 아직 결정되지 않았음**.
- Chapter2 자체 원화가 크고 게임 내 PPU314로 1 world unit이 된다는 점은 확인했으나, 뱀·늑대 v3를 동일 1 WU에 맞추어 직접 게임에서 확인한 증거 없음. 도구 접근성과 Codex 업무 중복 방지를 고려해 실제 Unity 사이즈 QA는 현재 HOLD.

## 3. 이번 실물 납품 — 이미 있던 키아트 2+미리보기+QA
- **실제 채팅 파일 ZIP**: `Limitless_MonsterWolf_KeyArt_v3_Review_20261021.zip`; **3,468,949 bytes**, **SHA256 `a5189bdb542988da69581135f137b39c98b3c5434272cb5020d13310a98fdae8`**. 13 entries.
- ZIP `Assets/`: 키아트 원본 PNG 정확히 2개(원래 v3 SHA256 동일), `Preview/VenomSnake_128px_RGBA.png`, `Preview/Wolf_128px_RGBA.png`, 각 charcoal/woodland 128px 배경 테스트 4종, `Preview/Both_128px_SameSlot_NotUnity.png`, `Official_Chapter2_ActualSprite_Online_Comparison.html`, `Manifest.json`, `QA/Offline_Validation.json`, `README.md`.
- **미리보기 규칙**: 원본 픽셀을 수정하지 않고 투명한 아트 바운딩박스를 잘라 116px 안에 비율 유지 축소·128×128 투명 캔버스 가운데 배치. 실제 Unity import 이미지가 아니라 QA용 소형 가독성 샘플. 인위적으로 배경을 생성한 이미지도 게임 실제 Field 소스라 주장하지 않음.
- 실제 검사: 원본2 SHA256 이전기록 2/2 일치, PIL PNG Decode 2/2, RGBA/알파, 압축 ZIP 재열기 `testzip()==None`, ZIP내 `Assets/` 2개 SHA256 2/2 PASS.
- **아트 QA WARNING 정확 증거**:
  - VenomSnake 원본 알파 >0 픽셀의 경계 접촉: canvas x=0에서 **25 pixels**; 연결된 nontransparent 픽셀 군집 **552개**, 그중 **16픽셀 미만 548개**. 일부 외딴 알파/클리핑 가능성. 직접 Sprite Import 전 가장자리·조각 수정 여부 검수 요구. 작은 128px에서 눈·독니·뱀 전신 판독을 바울 대표가 판단.
  - Wolf 원본 Alpha bbox [48,48,904,1258], 중심 실루엣. 털과 발끝 배경제거 매트 미세 흔적 및 자세/팔다리 판독성 검토 필요. 128px에서 늑대 실루엣이 살아도 스타일과 게임 Runtime에서 통일된다는 보증 아님.
- **USER_VISUAL_APPROVAL_PENDING**, 실제 공식 5종 PNG 픽셀 직접 다운로드/정적 비교 이미지 `NOT_AVAILABLE`, 동일 Unity 화면 크기 `NOT_VERIFIED`, 스킬/MonsterDef/전투/펫 Run/정식 애니메이션 `NOT_STARTED`.

## 4. 승인 이후 게이트 / 범위 보호
- 바울 대표가 VenomSnake와 Wolf **각 1포즈**의 형태/화풍/소형 판독성을 먼저 승인 또는 보완 지시.
- 승인 전 독사 Idle/Walk/Attack/Hit/Defeat 20프레임 및 Wolf Run 6프레임, Bear/Fox 신작 제작0.
- 승인 후에만 Codex가 실제 Ch2 공식 sprite 동일 Camera/WorldSize 비교·Importer/애니메이션 기술 계약 및 검수(현재 FrostFieldMouse Fix v2 먼저 마무리)를 수행하도록 별도 분리.
- Field09 승인 5+B Glow, HeatVent/Spire backlog, Field10 미술검토, 원본 PNG/meta/GUID·Scene·Collider/Save, MonsterDefinitions18, Beast/Pet 직업 시스템, Chapter3 신규 아트 전부 보호. `docs/art-renewal` 외 브랜치와 `main` 직접 commit/Push/병합0.
- **납품 실제 위치**는 이 대화 Sandbox ZIP 및 PNG. 이 브랜치에는 기록·인계 문서만 커밋하므로 사용자 F: 디스크·GitHub에 ZIP 바이너리 자동저장으로 주장 금지.
