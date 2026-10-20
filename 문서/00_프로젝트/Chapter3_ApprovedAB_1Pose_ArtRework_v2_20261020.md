# LIMITLESS Chapter3 — 서리들쥐 A / 설빙의 폭군 B 1포즈 v2 미술 재작업 납품 (2026-10-20)

- **브랜치:** `docs/art-new`; **Issue:** [#30](https://github.com/HappyPaul812/Project-Limitless/issues/30).
- **입력:** `docs/planning/문서/00_프로젝트/Chapter3_ApprovedAB_1Pose_CodexVisualHold_ArtRework_20261020.md`와 Codex의 기술 PASS·미술 HOLD 댓글. 1차 픽셀 아트 v1은 `ART_HOLD`. A/B 디자인 선택은 유지.
- **상태:** `REWORK_V2_DELIVERED / USER_ART_APPROVAL_PENDING / ACTUAL_GAME_SCALE_QA_PENDING / NOT_UNITY_INTEGRATED`.

## 변경 범위와 결과물

- **새 그리기:** 서리들쥐 A와 설빙의 폭군 B 각각 고해상도 픽셀풍 원화를 새로 생성. 이전 75×49/142×121의 작은 v1 픽셀 소스를 3배 확대하거나 색만 변경한 것이 아님. 두 신규 원화는 1254×1254 RGBA이며, 가시적 알파 영역을 검토하여 **512×512 RGBA 투명 1포즈 출력 PNG**로 정리.
- **주의:** 새 원화는 픽셀풍 디지털 드로잉을 출력한 것으로 **정통 수동 픽셀 단위 보정 완료/Chapter2 스타일 일치/사용자 미술 합격을 주장하지 않음**. 128px에서 뾰족한 발톱, 거친 털, 주둥이, 뿔, 흉곽의 가독성은 비교 시안을 통해 확인 요청.
- **납품 ZIP:** `Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`; **SHA256:** `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919`.
- **실물 위치:** 해당 ChatGPT 대화에 클릭 가능한 다운로드 ZIP 제공; **GitHub에는 문서/해시만 등록**하며 실제 PNG/ZIP 바이너리나 사용자 PC F: 경로에 자동 배치했다고 주장하지 않음.

| 파일 | 규격 | 투명 알파 bbox (L,T,R,B) | SHA-256 |
|---|---|---|---|
| `Sprites/FrostFieldMouse_A_1Pose_Rework_v2.png` | 512×512 RGBA | (22,124,490,490) | `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2` |
| `Sprites/FrostTyrant_B_1Pose_Rework_v2.png` | 512×512 RGBA | (22,25,490,490) | `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830` |

## ZIP 내부 8개 파일

- 위 PNG 2개
- `Preview/Rework_1Pose_Overview.png` — 두 1포즈 모습
- `QA/Display_128px_Comparison.png` — 동일 128px 표시 상자 비교
- `QA/Chapter2_Reference_SameImageScale_Comparison.png` — 원본 Chapter2 공식 시트 **첫 프레임**(Obsidian Beetle, Scorching Watcher, Veinfire Colossus)과 v2를 **동일 윤곽 폭/높이 145px로 정규화**하여 비교; *Unity 실제 PPU/실게임 스케일 같다는 뜻 아님*. Chapter2 시트 원본은 각각 `Limitless_Main18_ObsidianScar_Art_Pack.zip`, `Limitless_Main20_DeepCore_VeinfireColossus_Art_Pack_v3.zip`에서 로컬 참조 추출. 공식 PNG는 재포장/변경하지 않음.
- `Manifest.md`, `QA/Offline_QA.json`, `SHA256SUMS.txt`

## QA: 통과/미검증

- **PASS**: 2 PNG decode, 512×512 RGBA, 투명 캔버스 가장자리(alpha=0), 캐릭터 잘림 방지용 22px 이상 좌우 여백, 128px 검토판 존재, ZIP 총8항목 CRC `testzip()` PASS, 파일별 SHA 목록 확인.
- **HOLD**: 공식 Chapter2 아트 품질과 수동 픽셀 마감 일치, 실제 표시 PPU/전투 타겟 스케일, 보스의 양산용 몸체 비율, 게임 내 미술 승인은 판단 전.
- **NOT DONE**: Unity/Animator/Scene/Monster/Save 원본 변경, 프레임 애니메이션, 다른 몬스터 추가, 리뉴얼 작업.
- **원본 보호**: F1/서릿길 마을 환경 PNG10, 기존 캐릭터, Chapter1·2 공식 몬스터/보스 파일 및 로컬 메타/GUID 변경0.

## 다음 단계

1. **대표의 1포즈 미술 판단 필요**, 이전 v1의 기술 PASS가 미술 PASS를 대체하지 않음.
2. Codex에게 신규 PNG 2장만 기존 UI 실표시 스케일과 인접 비교하도록 요청 — 이번 임시 비교판은 동일 실게임 PPU라는 증거가 아님.
3. 필요 시 수작업 픽셀 디테일 정리 후 재검토. **미술 승인 전 애니메이션 전체 제작/Unity 통합 금지**.
4. Main 직접 커밋·Push·강제 병합 없음; 모든 문서 작업은 `docs/art-new`에만.
