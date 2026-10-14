# 아르벨 v3 정식 적용 QA — 2026-10-14

## 결과

바울 대표가 최종 승인한 기본 아트23종과 국소 지붕 투명·처마 접합 보완을 원본 Unity **디스크**에 적용했습니다. 전용 브랜치 `codex/arbel-v3-official-apply-20261014`. main 직접 Commit/Push·병합, 원본 Editor Refresh/Play/Scene/포커스 변경은 없습니다.

필수 기능·시각·보호 게이트 **PASS**. 전체 **PARTIAL**: 대조군에서 재현한 Unity/URP 패키지 오류 및 원본 Editor 실시간 Import·OS 손입력·정본6000.5.7f1 미검증을 분리합니다. 원본 실행 Editor/격리 실행은6000.6.5f1입니다.

## ZIP 정본

- 적용 소스: `Temp/ArbelV3Metadata20261014/Limitless_Arbel_Renewal_Art_v3.zip`, SHA256 `84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`.
- Downloads SHA256 `58fb2236599dfdf8b1b4eb18ccf4fba85aff8a924eb94573236aae1b9350c747`. CRC 두 ZIP PASS; PNG43 전체 byte/SHA 동일, Manifest·CSV·README 등 문서11만 다릅니다.
- GitHub `origin/docs/art-renewal`의 v3 인계와 보존 제작기록은 검증84 계열을 지목합니다. 전체84 패키지의 PNG와 문서만 사용했으며 Downloads 문서를 섞지 않았습니다. 선택 장식11 배치0, 신규 아트 PNG제작0.

## 구현 범위

- 284개 기존 환경 Renderer의 Sprite만23종으로 연결. 원본 Renderer·NPC 좌표·Collider·Quest·Portal·Camera 데이터 보호.
- 기존 두 건물의 지붕/벽/문 **8 Renderer 자체**를 SortingGroup2개에 묶었습니다. 복제 Renderer0, 기존 world position/scale/Collider bounds 유지, wall0/roof1/door2의 그룹 내부 순서와 외부 building2 유지.
- `ArbelBuildingDepth`는 발 위치와 지붕 X 범위로 **건물 주변 뒤1/앞5**를 결정합니다. 전역 연속 Y-Sorting 시스템으로 주장하지 않습니다. NPC와 Player의 현재 Sprite·atlas rect·pivot·flip·world-to-local을 매 프레임 읽어 국소 알파마스크를 갱신합니다.
- 지붕 4개 마스크 슬롯은 현재 겹치는 세린+플레이어를 수용합니다. 처마·표지·외곽·벽/문은 불투명, 세린 실루엣/애니메이션 영역의 가림만 알파0.18. 접합부 투명 픽셀은 기존 처마색 샘플로 보완합니다. 지붕 PNG/position/pivot/PPU는 승인 후보와 동일합니다.

## 검증

| 항목 | 결과 |
|---|---|
| 환경 Renderer |284/284, clone0|
| 런타임 Missing Sprite/Script |0/0|
| 23 BuildScene |참조23/23·Missing Script0|
| 세린 깊이·현재 애니메이션 마스크 |뒤/측면/문앞/접근, 도보/휠체어 PASS|
| 처마 접합·양쪽 문·벽 |불투명 유지·문 Sorting 우선·가림 재발0|
| 소품 접지·Collider |알파/불투명 본체 proxy 실측·원본 Collider 유지|
| 도보·휠체어 |실제 Rigidbody 우회, NPC/Portal 관리인 접근 PASS|
| 상점·치유·펫·NPC |실제 UI/확인/저장/상호작용 PASS|
| Save/Continue |격리 슬롯저장·Bootstrap 이어하기 실제 버튼·좌표/Quest 보존 PASS|
| 주요 회귀 |Main12/13 조사/세린/레온/도착·Story Battle·필드 몬스터2 실제 UI 승리/리스폰·Field04↔Arbel↔Field03 PASS|
| Field04 |승인 Renderer375·Road42 유지|
| 컴파일/Shader |최종 오류0·지원 Shader PASS, 기존 deprecated API 경고|
| 원본 호환 |main과 다른 원본 런타임5파일 overlay에서 core와 전투 회귀 모두 PASS|

main core1765 + main 회귀228 + 원본5파일 core1706 + 원본 회귀228 + BuildScene26 = **3953 PASS/0 FAIL**. 이는 물리 경로 waypoint별 assertion을 포함한 검사 수이며 독립 시나리오 3953개라는 의미는 아닙니다. 실제 core 캡처 main/root 각각50장, 별도 회귀 캡처도 보존했습니다.

## 기존 OPEN·시험 오류 구분

1. URP `SpeedTree8_PBRLit.shadergraph` Host type 오류: 동일 Packages/Editor/clean import 조건에서 **원본 main Scene+Depth0** 첫 Camera.Render에서도 동일 문구·스택 재현. `BaselineFirstResults`, `ArbelBaselineResults`, `unity_baseline*.log` 보존. 신규 런타임 오류0.
2. Search 인덱스 예외/기존 Editor 버전 차이·OS 직접 입력은 별도 환경 또는 미검증입니다.
3. Editor `VillageNpc` 빈 Sprite는 `VillageNpcPlaceholder.prefab` GUID `4c4988789a0e2a049b860bfa288d71bc`, `m_Sprite: {fileID: 0}`의 **의도적 초기값**이며 `PlaceholderVisual`이 Awake에서 생성합니다. 원본/후보 동일, 환경284 대상이 아니며 Runtime Missing Sprite0. 최초 모든 Renderer에 일괄 Sprite 요구한 QA fixture FAIL은 `BuildSceneFirstChecks.txt`로 보존하고 대조 후 수정했습니다. Missing GUID/fileID 오류로 보고하지 않습니다.
4. Portal↔나무 겹침 OPEN 유지. 최초 접근점(-6.25,-3)은 기존 관리인 Collider에 너무 가까워 반경0.53 fixture가 막혔습니다. 원본도 동일. 안전점(-6,-3)까지 실제 우회65waypoint와 관리인 OnInteract 통과, 후보 도보·휠체어도 PASS. Portal은 현행 정본상 준비 대화/표식이며 실제 텔레포트가 구현되어 있다고 주장하지 않습니다.
5. 원본 Editor 메모리는 변경하지 않았습니다. 바울 대표의 정상 작업 재개 후 자연스러운 Import/원본 청취·화면 확인은 남습니다.

## 보호 및 되돌리기

- 공통 baseline4001 기존파일 중4000 SHA 동일, 승인 `Arbel.unity`1만 변경. 추가 TTS/Story392 파일은 부모 담당 독립검사로 동일 확인.
- Scene 기존 YAML문서884/884 유지, 신규9문서. 기존 Script·Collider·NPC 데이터 의미변경0; 재저장 공백차이는 원본으로 복원했습니다.
- 신규55경로+기존Scene1, 총56경로. 기존 Sprite/WAV/GUID/meta/Save/Settings/Field04/Chapter3/TTS/BGM/사용자 dirty 파일과 rootHEAD `ff022e63dfe364f559fcc29819d27b123ad31127` 보호. 새로운 GUID 충돌0.
- 원본 Editor 전후 Bootstrap·PlayOFF·PausedOFF·Focusedfalse·Compilingfalse. 실제 Refresh/Play/Scene 전환 명령0.
- 롤백 원본: `Temp/ArbelOfficial20261014/OriginalBackup/Unity/Client/Assets/_Project/Scenes/Arbel.unity`. 필요 시 이 Scene을 복구하고 `apply_protection.json` 신규55개만 별도 정리하십시오. 사용자 다른 파일에 일괄 reset/clean을 하지 마십시오.

## 인계 자료

로컬 정본: `Temp/ArbelOfficial20261014/Delivery/` 및 `RootCoreResults`, `MainCoreResults`, `RootRegressionResults`, `MainRegressionResults`. 고정카메라4위치 비교 `Delivery/Final_Unity_Depth_4Positions.png`. 저장소 근거: `Evidence/Arbel_v3_Official_Apply_20261014/`.

Feature Commit: 648b1603c7d79803fdfa54fd0c2222e3ef8cc201
