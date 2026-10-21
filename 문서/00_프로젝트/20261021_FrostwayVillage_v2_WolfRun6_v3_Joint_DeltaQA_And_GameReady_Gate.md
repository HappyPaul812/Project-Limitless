# LIMITLESS — 2026-10-21 서릿길 마을 v2 / Wolf Run6 v3 합동 검수 및 Game-Ready 전환 게이트

- 기획 담당: `docs/planning`. 사용자 전달 로컬 ZIP:
  - `F:\Downloads\Limitless_Chapter3_FrostwayVillage_Environment_ArtPilot_v2.zip`
  - `F:\Downloads\Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v3.zip`
- **본 기획세션은 Windows F: 원본의 실제 바이트에 접근하지 못함**. 아래 SHA·파일 구성은 담당 GitHub 납품 보고값이고 실제 Codex ZIP readback과 독립 Unity 동작QA 전 상태. 이번 2건은 한 번에 Codex를 호출하되 `A: Chapter3 신규마을`, `B: Chapter1·2 펫 Wolf Run` 테스트·승인·변경 이력은 분리한다.
- 사용자 요청은 **두 신규 ZIP의 검수**이지 미승인 콘셉트/모션을 사용자 원본 Unity 게임에 바로 Import하거나 main 병합하는 승인이 아니다.

## A. Main23 서릿길 마을 Environment ArtPilot v2

- 담당 `docs/art-new`, 정본 `문서/00_프로젝트/Chapter3_FrostwayVillage_Environment_ArtPilot_v2_English_Package_QA_20261021.md`, 제작 GitHub Issue #45.
- ZIP SHA256 **`2142fe641c64a2ead8ad37309258d49727f35e5aaea74e429cbd6bae546154ad`**, 총 **19항목**. 파일: `Tiles/` 원화 PNG5, `Buildings/` 합성 시트 PNG1, `Props/` 합성 시트 PNG1, `Layouts/` 큰 마을 콘셉트·배치 시안 PNG4 (**source11**), 화면비별 오프라인 Preview4, Manifest JSON·MD/QA JSON/SHA256SUMS 4. 이번 v2는 **기존 생성 이미지의 영문 ZIP 재포장**이며 각각을 실제 게임 Sprite로 완성한 리뉴얼/분해 아트 납품이 아님.
- 기존 납품 자체 ZIP testzip/PNG decode 및 각 SHA는 아트 담당 보고에선 PASS. **1254×1254 RGB/RGBA 다수, 1448×1086 RGBA의 건물·소품 합성시트** 등 직접 게임 타일로 사용하기 어려운 큰 콘셉트 원화이며, **256×256 seamless Tile, 독립 알파 건물/문/지붕·소품, PPU·Pivot·동선 Scene 계약 미확정**. 오프라인 Preview16:9/4:3/21:9는 **Unity GameView 아님**.
- **Codex QA 이번 회차**: 실제 ZIP SHA·CRC·Manifest/개별 PNG SHA→형식 확인. 기존 F1 6 승인Sprite 및 Chapter2 공식 미술과 **동일 표시 스케일 비교**(source 실물 접근 범위), 게임 실사용 가능/불가를 다음 역할별로 엄격히 점검.
  1. 땅/마을 돌길·얇은 서리의 **실사용 Tile 분해성·2×2 접합 가능성**: 원본 RGB 대형 콘셉트를 seamless tile이라 허위 합격 처리 금지.
  2. `Buildings/` sheet에서 건물 개별 지붕/벽/문/여관·상점 역할을 알파 침범 없이 **실제 스프라이트 사각형**으로 분해 가능한지.
  3. `Props/` sheet에서 장작/랜턴/상자/길 표식/서비스 아이콘 및 **미래 포탈 돌기단(비작동)**이 개별/독립 Alpha·접지정보를 가진 게임용 Asset인지.
  4. Main23 공식 설계 마을 남문=F1 / 북문=F2 미개통 / 동쪽 7×7WU 포탈 공터 비작동/중앙길 도보·휠체어 후보3WU / NPC 서비스 배치 등 필수 역할 대응 **실물 coverage table**. 실제 Collider/Scene 재생·SafeZone·SaveContinue 136/64/40 기존 QA 재수행 금지.
  5. F1→마을 경계 색·흙·얇은 서리 스타일/전경 NPC·길표지 대비와 최소 화면비 시안 사용성. 실제 GameView를 만들지 않았으면 `OFFLINE_ART_REVIEW_ONLY` 명시.
- **A 결과 구분**: ZIP 파일 기술 PASS는 독립, `CONCEPT_ART_VISUAL_PASS/HOLD/FAIL`, `GAME_READY_ASSET_SLICING_PASS/FAIL`, 미술 품질 보완이 필요한 PNG·세부 좌표 및 독립 Tile/Building/Prop/Signs/InactivePortal의 **누락 Sprite 정확 목록**. GameReady가 FAIL이면 아트 담당에게 무작정 전체 새 마을 그림을 재발주하지 않고 **기존 source11을 보존해 게임용 각 Sprite 분해·부분 보완 패키지 v3 제작계약**을 제안하고 대표 사용자에게 기술과 미술을 구분 보고. 산출물/원본 PNG를 Codex가 멋대로 색칠하거나 게임용 Asset이라고 이름 바꾸지 않음.

## B. Chapter1·2 펫 Wolf Run6 v3

- 담당 `docs/art-renewal`, 정본 `문서/00_프로젝트/Wolf_Run6_ApprovedV5_ArtRework_v3_Delivery_20261021.md`, GitHub Issue #43.
- ZIP SHA256 **`171b954ddeef2b37d1bfaeb197b8a9f028293494457b66664c1d83509b87b5e0`**, **36항목**, 2,788,020bytes. `Assets/Poses/Wolf_Run_{00..05}_v3.png` 각각 **512×384 RGBA**, strip **3072×384** (가로512 6셀), atlas **1536×768** (3×2), 12FPS GIF/APNG 및 v2↔v3 preview, Alpha/Frame QA/Manifest.
- 아트 담당 자체 CRC/독립6 PNG decode, strip/atlas 픽셀동일6/6, 프레임 가장자리 3px alpha0, 서로 다른 포즈6, 기존 v5 KeyArt SHA `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1` 불변. 기술 자체 QA PASS 보고.
- 제작 변경은 v2 **03·04만** 보정 (03: 8,198px / 04: 7,615px), 00/01/02/05 기존 바이트 불변. 03·04의 잘린 꼬리·뒷발을 다른 Frame05의 동일 늑대 털/발 페인팅 원본으로 국소 재구성. **6장 전체 신규 독립 그림이라고 허위 보고하지 않음**. v2 미술 FAIL 원인인 03·04 tail clipping·몸통 비율/발 접지·05→00 루프·실제 성견 늑대 느낌 전체를 비교 검증해야 함.
- GIF 프레임별 duration **80,80,90,80,80,90ms** 총0.5초, APNG **83,83,84,83,83,84ms** 총0.5초. 실제 Unity 12FPS 분할에 필요한 PPU/WorldScale/foot Pivot는 현재 임시 Legacy 64×40×6 좌향·12FPS·DisplayScale2와 실제 화면 표시 크기 기준 Codex가 별도 격리에서 맞춰야 함. 고해상도 512×384을 무조건 기존 64×40 픽셀로 축소하지 않음.
- **Codex 이번 회차**: v3 ZIP SHA/CRC/Manifest/개별 PNG Byte/Strip/Atlas Rect 대조→v2 4 KEEP프레임 SHA·03/04 changed pixel 검증→같은 128px·40px 표시 크기의 **12FPS Unity 격리 Run 애니메이션 실제 재생**(가능한 경우) 및 05→00 loop, 03·04 꼬리/발·몸통 흔들림·접지/실루엣/프레임 간 어깨·허리 길이, 오리지널 Wolf v5 색·눈·털 비교. 비정상 경계 흔들림·다리 튐/잘린 꼬리가 남으면 정확 Frame/화면정지·좌표로 FAIL/HOLD, 미술 완성을 추정하지 않는다.
- 현재 Run v2는 검수자 **기술 PASS / 미술 FAIL**, **v3는 `CODEX_MOTION_QA_PENDING / USER_RUN_APPROVAL_PENDING`**. 실제 Unity Runtime BeastCompanionSystem 기존 ThirdParty Wolf_Run/세린 CompanionAssault를 교체하지 않고 **새 임시 Sprite/UI 격리 harness**에만 Import, 기존 GUID/Meta/Sprite reference 보호. 만약 실제 Unity 환경에서 실시간 재생 불가하면 `OFFLINE_ONLY_UNITY_NOT_VERIFIED`라고 표시.

## 공통 원본 보호·보고 계약

- 2026-10-20 GitHub main 이력 SHA `e4e9891f6f01ea95cada9e63b1f09946d94d6041` (실행 전 최신 refs 재확인). 원본 user Windows `F:\Downloads`는 사용자 직접 정리했으나 **이번 두 경로만 명시**, 파일 이동/수정/삭제/재압축 금지.
- Main22 격리136PASS·F1 Unity64PASS·서리들쥐 Battle40PASS와 기존 Main23 Village 기술 Preflight 상태는 그대로 재사용, 대규모 전수QA 반복·Main23 실제 Scene/Quest 구현·Main22 원본 반영·Main24 상점/Skill 저장 구현 금지.
- 별도 `codex/qa-frostway-village-wolf-run6-20261021` 격리 worktree에서 두 패키지 검수하되 **모듈 A/B 판정 분리**, 실제 GameView 여부 명시, SHA/ZIP Test·미술 분리, 사전부터 완료 보고 이후 사용자 original Unity/Save/UserData/Editor/dirty/untracked/원본 Approved F1/Chapter1·2 Assets 불변 확인.
- 결과: ①Village `FILE_PASS/FAIL`·미술`PASS/HOLD/FAIL`·`GAME_READY_SLICING_PASS/HOLD/FAIL`·missing sprite list ②Wolf `FILE_PASS/FAIL`·`RUN_MOTION_PASS/HOLD/FAIL`·`UNITY_ISOLATED_PLAYBACK_PASS/NOT_VERIFIED` 및 03/04/05→00 시각 evidence, 적용금지 영역 ③두 ZIP 실제 SHA, 대표 미술 최종 승인 필요 여부 ④브랜치/커밋·사용자 파일 보호 доказ evidence·Push/PR/main반영 모두 0.
- 이 회차에서 미술 PASS도 **대표 최종 승인 및 원본 Runtime 적용 자동 승인 아님**. 산출물은 최소 변경점/후속 작업으로 이어가고 챕터3 보스 **설빙의 폭군**·다른 몬스터/펫·대규모 아이콘/스킬 아트 전체 동시 재작업 금지.
