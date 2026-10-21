# LIMITLESS — Main23 서릿길 마을 개별 환경 아트 v3 납품 및 오프라인 QA (2026-10-21)

> **43_INDIVIDUAL_ART_CANDIDATES_DELIVERED / OFFLINE_FILE_QA_PASS / HUMAN_ART_QA_PENDING / CODEX_MAIN23_UNITY_CONTRACT_PENDING / NOT_UNITY_INTEGRATED**

담당: Chapter3 이후 신규 아트 / 브랜치 **`docs/art-new`** / [Issue #45](https://github.com/HappyPaul812/Project-Limitless/issues/45).

## 1. 발주·기존 문제

- Main23 기획 정본: `docs/planning/문서/00_프로젝트/20261021_Chapter3_Main23_Frostway_Village_Art_And_Tech_Parallel_Kickoff.md`. Issue #45 최신 Codex 판정: ArtPilot v2는 PNG/CRC 기술 PASS, 콘셉트 HOLD, Unity 실사용 **Game-Ready 분해 FAIL**.
- 기존 v2의 `1254×1254` 콘셉트·`1448×1086` 콜라주를 이름만 바꾸어 재압축하는 방식 금지. 그래서 이번에는 독립 빌딩·부품·소품·지면 타일을 **각각의 개별 RGBA PNG**로 분해 및 실사용 후보로 제작.
- 기존 v2 콘셉트 원본11, F1 승인6, Chapter1·2 공식 자산, 승인 서리들쥐·설빙의 폭군, 하렌·태온·폴·세린·미엘·플레이어, Unity Scene/Save/GUID/meta 전혀 수정 안 함.

## 2. 실제 바이너리 납품

- **영문 ZIP:** `Limitless_Chapter3_FrostwayVillage_Environment_GameArt_v3.zip`
- **최종 SHA256:** `2a74677d5f62d7e13e64ea85ba62e12175be0be0e0d19864c23233f1bd84e79a`
- **바이트 크기:** 17,311,281 bytes, **ZIP 멤버 59개**, `testzip() == None` PASS. 실제 ZIP 다운로드 파일은 본 ChatGPT 대화에 첨부됨. **GitHub에는 이 보고서만 등록, PNG/ZIP 바이너리는 아직 원격 업로드하지 않음.**
- **개별 아트 후보 43개**: `Tiles/` 기초 4 (흙·자갈·돌길·약한 서리, 256×256 RGBA), `Transitions/` 전환/길 경계 2, `Buildings/` 독립 목석 건물 5, `Buildings/Modular/` 지붕·벽·문 9, `Props/` 장작·상자·랜턴·울타리·벤치 등 14, `Signs/` 빈 안내·북문/남문 방향표시·여관/일반상점/스킬상점 표식 6, `Portal_Inactive/` **비작동** 석제 돌기단/석재 소품 3.
- **QA/Preview PNG 12개**: 타일4종의 2×2·3×3 반복8 + 독립 Sprite Layout 16:9/4:3/21:9 정확 화면비 합성3 + Asset Overview1.
- 메타 텍스트4개: `Manifest.json`(개별 파일별 모드/규격/실제 SHA/역할/출처/승인상태 포함), `Manifest.md`, `QA/Offline_QA.json`, `SHA256SUMS.txt`.
- 16:9 / 4:3 / 21:9 레이아웃은 **오프라인 합성**. 절대 Unity GameView라 표기하지 않는다.

## 3. 실제 이미지 검사

- 43개 후보 PNG **RGBA** 및 바이트별 SHA 기록. 개별 투명 오브젝트 전체에서 PNG 끝자리에 비투명 픽셀이 닿지 않음(경계 0): 소스 그림에서 분리된 장작/표지/문/건물/랜턴 등이 최소 투명 여백을 가짐.
- 기초 바닥 타일4종 모두 **256×256**, 알파255, 첫/마지막 X·Y 픽셀 배열 정확히 일치 (**2×2/3×3 반복 경계 PASS**). 외부 가장자리의 검은 선 결함을 발견하여 원본 그림의 투명 외곽을 제외한 **내부 소재부**에서 다시 타일 제작 후 검증함.
- 전환 오버레이는 **방향 지정형 1종**이며 완전한 4방향/코너 자동 타일 연결을 보증하지 않음. 반복은 픽셀 경계 일치만 확인했으며 **반복 무늬 자체의 가시적 패턴**은 실제 미술 검토 필요.
- 합성 Preview는 남문→중앙 광장→북문 넓은 길 및 동쪽 비작동 미래 포탈 공간을 시각화한 개념 배치. 실제 3WU 도보/휠체어 clearance 또는 Collider 인증 아님. 문/건물/서비스 NPC 동선과 PPU·Sprite Pivot·Sorting·Scene 크기는 Codex Main23 기술 계약 전까지 **미확정**.
- 건물의 개별 `Buildings/Modular/` 지붕/벽/문 조각은 존재하지만 **문-벽-지붕 접합 정밀 Unity 검증 전**, 부속품 일부 경계/오버랩의 손수정 필요 가능. 생성 원화의 픽셀풍/안티앨리어싱으로 기존 공식 픽셀아트와의 일관성은 사용자·Codex 시각 QA 대기.
- 모든 59개 ZIP 엔트리 CRC 및 PNG 디코딩, 내부 `SHA256SUMS.txt` 대응 여부 PASS. 이번 단계는 파일/알파/타일 경계 기술 PASS이지 **Game-ready art 최종 PASS** 의미는 아님.

## 4. v2 SHA 불일치 조사 범위와 증거

- 이번 ChatGPT 작업공간에 남은 **v2 로컬 사본**의 SHA: `2142fe641c64a2ead8ad37309258d49727f35e5aaea74e429cbd6bae546154ad`, 기존 보고서 해시와 **같음**. 해당 사본 19엔트리 CRC PASS, Manifest에 기재된 검사 가능 15엔트리의 실제 SHA 15/15 일치.
- 반면 **Codex가 바울 대표 Windows F:의 v2 ZIP 실물에서 보고한 SHA**는 `19dfad3cc6f7e27b03e1dcfc194cdb1bf56d9e133b48f303288eff98886833d`. 따라서 **서로 다른 ZIP 파일 사본이 존재함은 확인**, 그러나 이 작업공간에는 Codex의 19df 사본이 없어 **두 ZIP의 엔트리 목록/PNG 바이너리 차이와 차이가 발생한 근본 원인을 확정할 수 없음**. 같은 원본 ZIP인지, 압축 멤버 변경/재압축인지 추측하지 말 것.
- Codex에 F:사본 `unzip -l`·각 엔트리 SHA 및 본 기록의 2142 ZIP 비교 요청 필요. 이번 v3는 ZIP 작성 뒤 최종 파일 바이트 SHA를 별도 sidecar에 기록(자기 자신을 포함하는 ZIP 내부 해시 작성 금지). **사용자 로컬 다운로드본 해시와 비교해 파일 전송 동일성 확인할 것**.

## 5. 보호, 승인 게이트, 후속

1. 현재 미술 **ART_REVIEW_PENDING**. 대표님께 새 독립 43종·배치 미리보기와 공식 F1의 질감/명도 차이를 먼저 승인받음.
2. Codex가 `codex/...`에서 실제 PPU/Pivot/Sorting/TileGrid/3WU 휠체어 통로/문 접합/실제 16:9·4:3·21:9 GameView·Collider/서비스 진입을 검증. 통과하지 못하면 **해당 PNG만** 표적 보완.
3. 승인/기술계약 전 새 Main23 Scene/Quest/NPC·상점 가격/포탈 액션/Save 및 기존 게임 원본 변경 금지. **main 직접 Commit·Push·병합 금지**. 다른 담당 리뉴얼·몬스터 전투 재작업과 중복 금지.
