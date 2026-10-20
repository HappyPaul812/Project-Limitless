# LIMITLESS — Chapter3 F1·서릿길 마을 신규 아트 파일럿 및 오프라인 QA (2026-10-20)

> **상태: PILOT_CREATED / USER_ART_REVIEW_PENDING / TECH_IMPORT_CONTRACT_PENDING / NOT_UNITY_INTEGRATED**.
> 담당: Chapter3 신규 아트 총괄 / 작업 브랜치: `docs/art-new` / GitHub Issue [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30).

## 발주 근거와 보호 범위

- 발주 정본은 `docs/planning`의 [Chapter3 병렬 제작 착수](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter3_Team_Parallel_Production_Kickoff_20261020.md). 우선순위 F1 흙·얇은 서리 지형 → Main23 서릿길 마을 목재/돌기초·남북 간선·동쪽 미래 포탈 **비작동·빈 공간** → 서리들쥐/설빙의 폭군 각 1포즈 개념 시안. 대량 몬스터·NPC·보스 애니메이션 제작은 별도 미술 승인 후.
- F1에는 두터운 적설·한기 상태이상을 새로 추가하지 않음. 마을 동쪽 광장에 포탈 본체/활성 FX/트리거 없음. 보스 전투 전용 온기의 화로를 마을 장식 화로로 자동 확정하지 않음.
- 기존 하렌·태온·세린·폴·미엘·플레이어 및 Chapter1·2 승인/리뉴얼 아트, Unity·Scene·기능·GUID/meta·Save·UserData 미수정. `main` 직접 커밋·Push·병합 없음.

## 실제 제작 산출물

- 생성 패키지: `Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip` (대화 첨부 다운로드). **ZIP SHA-256: `6a22ac4e9c4d4f4f9e37c2ac8270b145d5483676d00f83c320cdf0e6925382b4`**. 로컬 제작 경로 `/mnt/data/Limitless_Chapter3_ArtNew_F1_Frostway_Pilot_v1.zip`는 사용자 Windows PC 경로가 아님.
- 내용: **독립 아트 PNG 12개**(F1 6, Village 4, KeyArt 2), **미리보기 PNG 3개**, **QA 2×2 반복 미리보기 PNG 3개**, `Manifest.md`, `Manifest.json`, `QA/QA_Summary.json`. ZIP 항목 총 21개, CRC/디코딩 검사 PASS.
- 실제 생성 방법: 서드파티 이미지 재색칠/복제 없이 독립적인 결정론적 픽셀 드로잉(Pillow). 추가 이미지 생성 참고 보드는 게임 자산에 사용하지 않음.
- **GitHub에는 본 QA·SHA 목록만 기록했으며, 바이너리 PNG/ZIP은 아직 브랜치에 업로드하지 않았음**. 실제 이미지가 GitHub에 있다고 주장하거나 이 문서만으로 게임 Import 진행 금지. 원본 패키지는 본 대화의 클릭 가능한 파일 링크에서 전달.

## 독립 아트 PNG / SHA-256

| 구분·파일 | 크기 | 모드 | SHA-256 |
| --- | --- | --- | --- |
| `F1/NorthF1_Ground_Earth_Base.png` | 256×256 | RGBA | `dd2f4e125f97702fd4352beb4f600c5ba0bd7a0c9cb00f2922af86988b9ccbe8` |
| `F1/NorthF1_Ground_ThinFrost_Base.png` | 256×256 | RGBA | `fee1e35d61d8bb90ac27b902b81a96310a2f0918d6dd291f138bc927ffa27346` |
| `F1/NorthF1_EarthToFrost_Transition_Overlay.png` | 256×256 | RGBA | `5da0155fb31c0f2684738a642e47af074919a0732967f1f3248b3aafdab427fa` |
| `F1/NorthF1_Path_WornTrail_Overlay.png` | 256×256 | RGBA | `3595a805bc81d365985f60e1e642806f476bec601e3d3866bd0015ba282f56d7` |
| `F1/NorthF1_Prop_FrostShrub_01.png` | 256×256 | RGBA | `508ab9c7eabd5b7fa97b3baa4ce48a252a9a95f429b2d6d4e7c73bad76be860b` |
| `F1/NorthF1_Prop_Waymarker_01.png` | 256×256 | RGBA | `446a3b3121fd0531d25937abad6c8f91fc97e59fce50f67baa7f08c438bb6a47` |
| `Village/Frostway_Ground_Cobble_Base.png` | 256×256 | RGBA | `8ea392ead2d4005935236ea542adb7a1ecb231276d5069ad9f3f68dd965f4c03` |
| `Village/Frostway_Building_WoodStone_01.png` | 512×512 | RGBA | `d5c082e3417b4f7a7e9e219a65cc66461765ec16c8934760143ea0a7a9692a50` |
| `Village/Frostway_Prop_SupplyCrate_01.png` | 256×256 | RGBA | `e81b77946bf178ff83e5eec654b7e9306145cd770a2f930f2cd271d1f937072b` |
| `Village/Frostway_Prop_WarmLantern_01.png` | 256×256 | RGBA | `a0fee8d75997ddf2d98147c71dcdc1b901b275189f1e14c1bd0f762d90443016` |
| `KeyArt/NorthF1_FrostFieldMouse_KeyArt_Concept.png` | 512×512 | RGBA | `2222b8051ca724ddcf5b3be6ef401c0b51275af6cf9c58fa119c91f07e2b36d8` |
| `KeyArt/FrostTyrant_Silhouette_Concept.png` | 512×512 | RGBA | `8f9975e2cf9640932ed09910f5b042f1175bc5bdfd19e85b186016c8f8a1066e` |

## 검증 결과 및 한계

1. **PNG 디코딩/규격:** 모든 개별 12개 256×256 또는 512×512·RGBA PASS. 바닥 타일 3개는 의도적으로 불투명 RGBA; 소품·오버레이·KeyArt는 투명 영역 포함.
2. **타일 경계 검사:** `NorthF1_Ground_Earth_Base`, `NorthF1_Ground_ThinFrost_Base`, `Frostway_Ground_Cobble_Base`의 2×2 반복 배치 시 좌우 및 상하 맨자리 픽셀 배열 정확히 일치 PASS. 이는 눈에 띄는 반복 패턴·실제 Unity 스케일/월드 매핑 검증을 대체하지 않음. 전환 오버레이는 **한 방향 전용**이므로 좌우 반복 Seamless라고 표기하지 않음. 길 오버레이는 Y축 반복용.
3. **ZIP:** `ZipFile.testzip()` CRC 정상, 총21항목, SHA기록. 프리뷰 3개 중 F1 / 마을 예시는 **Pillow로 조합한 콘셉트 미리보기**, 실제 게임/Unity 실화면 아님.
4. **시각 QA 제한:** 건물의 하향/전면 표현 및 실제 게임 타일 스타일 조화, 반복적인 무늬의 체감, 경로 폭과 색상 가독성, F1↔마을 지면 이음, 붉지 않은 냉기 조명, 휠체어 동선은 **USER_ART_REVIEW_PENDING**. 콘셉트 승인 후 수정판 필요 가능.
5. **유니티 기술 계약 미정:** Issue #35 Codex 읽기 전용 감사에서 실사용 PPU, Pivot, 셀/월드 단위, Collider, Sorting Layer, 모자이크·캔버스 크기, Scene 위치/스폰/휠체어 통로 계약 확인 대기. 제안한 픽셀 크기는 **제작 임시 규격**이므로 Game-ready 승인 확정 아님.
6. **기획 미잠금:** Main22~24 Scene/Quest/Dialogue/Monster/Skill/Item ID, 마을 NPC 고유명/배치 좌표, 보스 TurnOrder, 스킬 가격·경제값은 여기서 생성·결정하지 않음.

## 다음 단계 / 책임

1. 바울 대표에게 독립 PNG와 예시 조합 미술 검토 요청. **최종 승인 필요**.
2. 기획 `docs/planning`의 Production Lock 및 Codex `codex/chapter3-tech-preflight-20261020`의 PPU·Pivot·Collider·맵/안전지대 동선 계약을 대조. 정식 계약 도착 전 원본 게임 Scene 생성/수정 없음.
3. 승인·기술 계약 후 F1와 마을 필요한 자산만 실제 규격으로 다듬고 다시 오프라인 QA. **Codex는 이후 별도 승인을 받아** Unity 통합·애니메이터/Runtime/Save 회귀를 수행.
4. 서리들쥐와 설빙의 폭군은 **1포즈 콘셉트만** 제작, 총7종 몬스터 전투 시트·보스 애니메이션 일괄 양산 금지.
