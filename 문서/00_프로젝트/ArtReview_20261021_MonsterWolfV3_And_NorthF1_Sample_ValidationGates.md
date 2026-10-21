# LIMITLESS — 2026-10-21 맹독뱀·Wolf v3와 Chapter3 F1 지면 샘플2종 병렬 자산 검증 및 제작 백로그

- 기획 담당 `docs/planning`. 바울 대표가 로컬 ZIP 두 파일을 전달하면서 자산 세부 우선순위표는 본인에게 굳이 표시할 필요 없다고 안내함. 기획이 내부적으로 누락/상태/검증 게이트 관리.
- 사용자 F: Downloads를 최근 직접 정리했으므로 이번 메시지에서 제공한 경로만 현재 검수 입력으로 사용. 본 ChatGPT GitHub 세션은 Windows F: 파일을 직접 열거나 SHA를 직접 검사할 수 없으므로 **아래 파일 해시는 각 담당의 기존 GitHub 문서상 값**이며 **Codex 로컬 검증 전**임.
- 새 시트/장비·아이템·스킬·몬스터의 실제 게임 적용은 모두 별도 사용자 승인 게이트. 이번 2건은 아트 검수 대상이며 `main` 직접 Push/병합 금지.

## 1. Chapter1·2 리뉴얼 — MonsterWolf KeyArt v3

- 사용자 ZIP: `F:\Downloads\Limitless_MonsterWolf_KeyArt_v3_Review_20261021.zip`.
- 정본: `docs/art-renewal/문서/00_프로젝트/MonsterWolf_v3_ActualFile_Review_128px_And_Chapter2_Reference_20261021.md`, [Issue #43](https://github.com/HappyPaul812/Project-Limitless/issues/43). 작성 커밋 `8ee584f26e01fc3`, 인계 `b216964d8fb7129`.
- 담당 신고 ZIP SHA256 `a5189bdb542988da69581135f137b39c98b3c5434272cb5020d13310a98fdae8`, 총13항목. 원본 기존 v3 두 PNG 동일 바이트:
  - `Assets/VenomSnake_KeyArt_v3.png` 1254×1254 RGBA, SHA256 `7001d199b279cb8e541fc603bd62351ab460d3b44da91a2d65dc099f8fd0`.
  - `Assets/Wolf_KeyArt_v3.png` 952×1306 RGBA, SHA256 `30b2d464fe2c456c0fa7d82f1fad51f64065b6e8379ea174548deec7d1b91b4e`.
- 담당 오프라인 ZIP CRC·PNG SHA PASS. 원본 2 KeyArt `USER_ART_REVIEW_PENDING`. Snake: 알파 canvas x=0 접촉 **25px** + 작은 연결군집 **548개/16픽셀 미만**, 외딴 픽셀/좌측 잘림 우려. Wolf: 전신 알파 bbox [48,48,904,1258]이나 발·털 배경 매트 가능성, 펫으로서 실제 늑대의 자세 판독과 게임 톤 별도 시각 평가 필요.
- Chapter2 공식 5 Monster Sprite .meta 실제 **314×314 / PPU314**, raw 파일 자체를 기존 art 세션에서 가져오지 못해 실제 동일 게임 표시 배율 비교 검증 없음. KeyArt preview를 `128px`에 넣은 것은 Unity 실 GameView와 같지 않음. Codex는 사용자 원본 Unity 공식5 원본의 **실제 PNG와 render/alpha ratio**를 읽기 전용으로 확인하고 같은 배율 비교판만 제작; 1 WU / PPU 원본 정확성을 따를 것. KeyArt를 곧바로 Sprite Sheet/몬스터런타임/펫모션으로 정식 등록 금지.
- 기술 규격 통과해도 미술평가 PASS 단정 금지. 사용자 1포즈 승인 전 독사 전체 전투모션 및 Wolf Run/Bear/Fox 제작 금지.

## 2. Chapter3 신규 환경 — NorthF1 기존6 보호, 지면·서리·전환부 신규 후보3

- 사용자 ZIP: `F:\Downloads\Limitless_Chapter3_NorthF1_ArtReview_Samples_v1.zip`.
- 정본: `docs/art-new/문서/00_프로젝트/Chapter3_F1_Original6_ArtReview_TargetedGroundSamples_20261021.md`, [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30). 작성 커밋 `929eff1f668cbcb`, 인계 `5362f980bfa58f1`.
- 담당 신고 ZIP SHA256 `dd9963d2a2ce442ad12ecccc8c3ae74edbfb7421eb838003d21ae2df767bd086`, 총18항목. 기존 F1 6 PNG는 SHA 동일·미술 전체 **HOLD**. 신규 후보는 정확히 3개(256×256 RGBA):
  - `NorthF1_Ground_Earth_Base_ArtSample_v2.png` SHA256 `573a11195bbc71135441688fca2792fb0fa2de0a7322e400f08275dfcf8c9a18`.
  - `NorthF1_Ground_ThinFrost_Base_ArtSample_v2.png` SHA256 `6158bdb1962f9f8934a673c767258f1bd2912b22e2ac8250c631fc32e027a803`.
  - `NorthF1_EarthToFrost_Transition_Overlay_ArtSample_v2.png` SHA256 `6785a24c0332d3f9b5fee13b317576eada7a3fc4e1f0f2839d4c625c6a6844a9`.
- 담당 원형 ground 두 PNG 2×2 seamless 경계/ZIP CRC PASS; 전환부는 **우측 1방향 전용**이므로 4방향 자동 타일 아님. 비교판은 Pillow 오프라인 Preview, Unity 실제 GameView 아님. 근본 미결: **기존 Path_WornTrail_Overlay의 길/보행 가독성 미흡**, FrostShrub/Waymarker 도식적, F1 6종 모두 미승인. F1 기술 PPU/TileGrid/월드 크기/Collider/카메라/휠체어 통행계약 확정 전.
- Codex는 Original6 byte-SHA와 새3 PNG/Manifest·CRC를 확인, Ch2 공식 DeepCore 실물과 게임 표시 배율/실제 F1 expected Scene 구성에 맞춰 시각 델타 비교; 지면·서리·전환만 따로 PASS/HOLD/FAIL 및 **길/소품 나머지 HOLD**를 유지. `Main22` 전체 Scene/Quest 자동 구현 금지.
- 후보3만으로 F1 전체 미술 PASS 및 게임-ready 선언 금지. 바울 대표 지시대로 설빙의 폭군 Attack/Hit/Defeat 우선 제작 금지.

## 3. 2026-10-21 첫 번째 Codex 작업: 두 ZIP 비파괴 공동 QA

- 기존 서리들쥐 Combat Fixv2 사용자 미술 승인, 원본 연결 Codex `codex/chapter3-mouse-combat-v2-apply-20261021` 완료(전투 QA 40PASS/0FAIL), **feature 브랜치에만 로컬 커밋, main 직접 Push/merge 없음**; F1 Main22 전체 구현0. 반복 작업/재검증하지 말 것.
- Codex 격리 새 `codex/art-review-monsterwolf-f1-20261021` worktree에서 두 사용자 ZIP **READ-ONLY 검증**, 압축 SHA/CRC/Manifest/기존 원본 SHA 보존 및 구형 F: 경로를 임의 탐색/이동/정리하지 않음.
- 몬스터·펫: 실제 공식 Ch2 몬스터 PPU314 sprite와 VenomSnake/Wolf 키아트 동표시 크기 비교·스네이크 alpha stray/clipping 수치·늑대 matte·디자인 품질 평가. 모션·원본 Import 실행 X.
- F1: 새 ground/frost/transition 3개와 기존6 비교, official Ch2 스타일·2×2 tiled seam/서리 비중, 길·표지·관목 HOLD, 다양한 화면비에서 안전한 명암 판독과 1차 걷기/휠체어 경로 시각만 QA. **진짜 Unity Scene 미생성 상황이면 오프라인 합성 결과만 제시하고 Unity 실제 화면이라고 주장 금지**. 코드/게임원본 변경 금지.
- 결과: 각 ZIP SHA/CRC, 모션과 별개 **각 개별 후보 Art PASS/HOLD/FAIL**, 실제 게임 표시 크기 대비 물리적 비교 근거 및 이미지, Alpha QA 숫자, 결함 정확 프레임/픽셀/시트, 후보 다음 보완 목록, Github docs & latest/handoff, branch/commit/원본 사용자 수정/Save/UserData/Editor 불변, 결론 `USER_ART_APPROVAL_PENDING`.
- Codex 완료 후 바울 대표가 필요한 **압축 전후 비교 시각 자료만** 확인하도록 하고, 자산 우선순위표 전체를 요구하지 않는다.

## 4. 누락 아이콘·잡템 내부 발주 대장 (정식 신규 개별 Item ID 임의 발행 금지)

- Main22 first playable: 서리들쥐 새 잡템(이름/확률/판매가 아직 밸런스 계약 대기) 및 기초 드롭 아이콘/인벤토리·아이템 설명/전리품 UI `ASSET_CONTRACT_PENDING`. 첫 전투 Loot가 빈 아이콘으로 표시되지 않게 Source metadata 준비.
- Main23/24 최초 스킬상점: 기존 스킬3 무료/직업5, 신규/강화 스킬 관련 개별 아이콘 수·규칙/카탈로그, 탈렌트 표시, 구매/강화 상태 아이콘 `ICON_MAPPING_AUDIT_PENDING`. 예전 기획 '신규2/직업'은 수치 목표일 뿐, 확정 판매 상품/ID/아이콘 개수를 실 Asset Catalog 분석 없이 10개라고 단정하지 말 것.
- Main24 첫 한기 대응 아이템: '따끈 어묵탕' 한기 해제, 기타 초기 대응/회복·부활 아이템은 설계 참조/가격·ID 미확정. 실제 신규 UI PNG·품목 정본과 동일한 명칭·규격을 확정한 뒤 아트 발주. 다른 지역 HP 포션·기존 7개 Chapter2 잡템 공식 아이콘 교체 금지.
- Main22~Main29 북부 일반5/엘리트1/보스1의 **몬스터 전투 sprites·loot icons·companion/icon/Cold debuff UI** 누락을 `BACKLOG_OPEN`으로 관리, 등장 순서 Main22 서리들쥐 → Main23 선택적 설원족제비 → Main24 서리올빼미·첫 한기; 얼음갑충/눈보라 정령/빙각 수호체/설빙의 폭군은 후속. 폭군 B 전투 작업은 사용자 명시 후순위.
- Chapter1·2 리뉴얼 맹독뱀·늑대는 `docs/art-renewal`, Chapter3 신규 환경/몬스터·아이콘은 `docs/art-new`, 상품·명칭/수치 승인 `docs/planning`, Codex Unity Catalog·Asset/PPU·기술검증. **팀별 오리지널 아트와 Unity/파일 접근 책임 구분**.
- 담당별 신규 전체 제작 지시 대신 **필요한 게임용 Sprite/Item/Skill 카탈로그 및 확정 ID 감사**를 다음 기획 후속에서 정리해 아트 발주한다. 사용자에게 별도 대장 출력을 요구하지 않음.

## 5. 안전/정확 보고
- 원본 사용자 EOD 기준 remote main `e4e9891f6f01ea95cada9e63b1f09946d94d6041`, local main=origin/main이었다는 직전 Codex 완료 증거. **새 실행시 최신 ref 새로 확인**.
- 사용자 Save/UserData/Editor/dirty changes와 승인 몬스터·B Glow를 읽기 전용 보호. main 직접 Commit/Push/merge0. 비교 자료 및 미술 PASS는 **격리 Render/scaling evidence**와 대표 최종 승인 구분.
