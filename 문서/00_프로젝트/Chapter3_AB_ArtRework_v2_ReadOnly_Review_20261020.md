# Chapter3 A·B 1포즈 ArtRework v2 — 미술 PASS / 대표 최종 승인 대기

2026-10-20, 브랜치 `codex/chapter3-ab-v2-art-review-20261020`. **종합 PASS(이번 정적 1포즈 미술 검수 범위)**. 서리들쥐 A PASS, 설빙의 폭군 B PASS. 바울 대표에게 이번1포즈 미술 승인을 권고합니다. 게임 적용 규격·애니메이션·Runtime은 별도 미검증이며 Unity 적용 허가가 아닙니다.

## 1. 최신 정본과 무결성 — PASS

GitHub main의 AGENTS/CURRENT_STATUS/CODEX_WORKFLOW/COMMON_RULES/CODEX_LATEST를 직접 조회했습니다. `docs/planning`의 기존 검수 게이트(blob `6de1fa6f008f6667bb9cd03f8a05828afecd0570`), 후속 재작업 지시(blob `1d83ff983d75847e34f409384bc0614b0e35383c`), [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30)의 최신 v2 납품 댓글과 `docs/art-new/Chapter3_ApprovedAB_1Pose_ArtRework_v2_20261020.md`(blob `842a4c67a1003f79e0d64a4816f3527eb34ea836`)를 확인했습니다. 이전 v1 HOLD/A·B 디자인 선택 유지/이번 미술 승인 대기 구분을 따릅니다.

ZIP `F:/Downloads/Limitless_Chapter3_ApprovedAB_1Pose_ArtRework_v2.zip`: **1,474,324 bytes / 8파일**. Sprite2, Preview1, QA 비교판2, QA JSON1, Manifest.md1, SHA256SUMS.txt1. zipfile.testzip()==None·전체 실제 해제·PNG decode·Manifest/QA와 PNG SHA·SHA256SUMS7항목 일치 **PASS**. 파일별 CRC/SHA/크기는 Validation.json에 보존했습니다.

| 실파일 | SHA256 |
|---|---|
| ZIP | `0b1a3711bed9616b828c83e51070f35ed161e2d8a4d93de9cb949d318dc46919` |
| `Sprites/FrostFieldMouse_A_1Pose_Rework_v2.png` | `0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2` |
| `Sprites/FrostTyrant_B_1Pose_Rework_v2.png` | `f1a3c5f91a5fef69cd881e27b578c32253d0124d5d8628a8ca748f7479333830` |

## 2. 실효 크기·알파·재작업 여부

| 항목 | 서리들쥐 A v2 | 설빙의 폭군 B v2 |
|---|---|---|
| 캔버스 | 512×512 RGBA | 512×512 RGBA |
| alpha bbox (끝 제외) | (22,124)-(490,490) | (22,25)-(490,490) |
| 가시 크기 | **468×366** | **468×465** |
| 이전 v1 | 가시225×147, 실제75×49의3배 | 가시426×363, 실제142×121의3배 |
| 정확3×3 동일 RGBA 블록 반복 | **아님** | **아님** |
| v1 대비512 RGBA 차이 픽셀 | 96,895 | 159,092 |
| 7px 미만 미세 알파 군집 | 81개/158픽셀, 최대alpha3 | 70개/135픽셀, 최대alpha3 |
| 몸체 내부5px 침식영역 alpha 중앙값 | 252/255 | 251/255 |

좌표는512 캔버스 위쪽 원점0입니다. bbox는 가시 영역 크기이며 모든 픽셀이 독립적으로 손수 그려졌다는 뜻은 아닙니다. RGB/alpha 색상수 증가는 보조 수치일 뿐 미술 품질 점수로 쓰지 않았습니다.

**단순3배 확대 재포장 아님:** 형태·포즈·긴 꼬리/털/발톱·흉곽·팔 구조가 v1과 실제로 달라졌으며, v1의 정확한3×3 RGBA 반복도 사라졌습니다. 새로 표현된 해부학/재질 정보가 비교판에서 확인됩니다. 다만 담당 보고는 **1254px 신규 고해상도 픽셀풍 디지털 원화 생성 후512 출력**이며 레이어 원본·중간 작업 단계는 ZIP에 없습니다. 따라서 **수작업 픽셀 재드로잉 공정 자체를 독립 입증한 것은 아닙니다**. 정통 제한팔레트 수작업 픽셀아트 완성이라고 주장하지 않습니다.

캔버스 가장자리는 투명하고 비정상 사각 매트/흰 가이드선/잘린 윤곽은 관찰되지 않았습니다. 미세 군집은 존재하지만 모두 alpha≤3(약1.2%)로, 어두운 배경·밝은 배경 및128px 비교에서 뚜렷한 부유 잡픽셀로 보이지 않았습니다. 원본의 군집을 임의 삭제하거나 알파를 보정하지 않았습니다. 최종 Import 계약 단계에서 미세 alpha 잔여 여부를 정리할 수 있으나 이번 미술 PASS를 막는 결함으로 판단하지 않습니다.

## 3. 공식 Chapter2 비교 기준

[이전 실측](Chapter3_AB_1Pose_ReadOnly_ArtReview_20261020.md)의 공식 일반5종(그을음들개/열풍매/균열도마뱀/불씨갑충/불씨망령)과 보스 열맥 거신을 재사용했습니다. 공식 PNG6 SHA 동일, PPU314·첫 Idle fileID·VisualScale1/3 유지 확인. 시트1256²의 **실제314² 첫프레임**을 사용했으며 시트 전체를 한 마리로 표시하지 않았습니다.

현행 `BattleSceneController.cs`의 UI box136×112(일반)/408×336(보스), preserveAspect 및1280×720 Canvas 기준은 그대로입니다. 정사각 Sprite의 표시 크기는112²/336²입니다. Field는314PPU에 scale1/3이므로 프레임1×1/3×3유닛입니다.

후보의 최종 PPU/Pivot/VisualScale은 **미확정**입니다. 비교판1은 같은 일반/보스 UI box에 후보512 캔버스를 넣는 가정이며, A가시약102×80px/B가시약307×305px입니다. 비교판2는 각 alpha bbox 최대변을128px로 맞춰 형태/디테일만 비교합니다. 별도로 원본캔버스128px(A가시약117×92, B약117×116)과 v1/v2 동일 가시128px를 준비했습니다. 납품자의145px 정규화 비교판을 실제 게임 배율이라고 쓰지 않았습니다.

**모든 신규 비교판은 코드·Sprite 규격 기반 정적 자료이며 실제 Unity 캡처가 아닙니다.** 크롭·리사이즈·배경 합성은 검토용 파생 이미지에만 사용했고 후보 원본 파일에는 재저장하지 않았습니다.

## 4. 시각 판정

| 대상 | v1→v2 개선 | 현재 판정 |
|---|---|---|
| 서리들쥐 A | 둥근 장난감형 덩어리에서 굽힌 등·앞으로 낮춘 머리·뾰족한 주둥이·길고 맨살인 꼬리로 변경. 앞니/눈빛/벌린 앞발·발톱·거친 털이 분리 | **PASS** — 128px에서 야생 설치류의 경계감이 읽히며 마스코트 인상이 이번 기준을 막는 수준으로 남지 않음. 동일 일반 UI box에서도 꼬리·발·주둥이 긴장감 유지 |
| 설빙의 폭군 B | 정면 장식형 뿔 실루엣에 머물던 v1과 달리 앞으로 내민 거대 손/긴 손가락, 긴 팔·두드러진 어깨, 분리된 흉곽/두개골·입 구조 표현 | **PASS** — 보스 box336px에서 상체 압박·발톱·늑골이 명확. 128px에서도 뿔·거대 앞손·얼음 갑각이 분리되어 단순 실루엣만 좋은 수준을 넘음 |

공식 Chapter2의 큰 명암 면/선명한 초점과 비교하면 v2는 털/얼음의 가는 고대비 선이 더 많습니다. 128px에서 모든 털끝/늑골 선이 유지되는 것은 아니지만, 주요 해부학과 위협감이 무너지지 않아 **이번1포즈 미술 적합성 PASS**로 판단했습니다. 거신처럼 육중한 암석 덩어리여야 한다고 요구하지 않고, 폭군의 웬디고·얼음/골격 모티브 안에서 압박감을 평가했습니다. 양산 프레임에서 같은 명암/윤곽을 유지하는지는 아직 검증하지 않았습니다.

## 5. 승인 권고·보호

**종합 PASS / 서리들쥐 A PASS / 설빙의 폭군 B PASS / 사용자 최종 미술 승인 권고.** v1 HOLD는 이번v2 합격으로 대체하는 새 버전 판정이며 기존 기록은 보존합니다. 승인 범위는1포즈2종 미술입니다. **Game-ready는 HOLD**: PPU/Pivot·프레임·압축/Animator·방향/애니메이션·Runtime은 미검증입니다. 미술 승인만으로 전체 애니메이션이나 Unity 적용을 시작하지 않습니다.

원본 프로젝트 Assets/_Project·UserData·Settings·Packages **3,546파일 SHA변경0**, 원본 Git HEAD/status 동일. F1/서릿길 마을/공식 자산·Save/UserData 변경0, 아트 수정0·새 애니메이션0·Unity Import0·Push/main 병합0. 문서·비교판·검수 기록만 전용 격리 브랜치에 기록했습니다. [보호 증거](Evidence/Chapter3AB_ArtReworkV2_20261020/Protection.json).

[비교 갤러리](Evidence/Chapter3AB_ArtReworkV2_20261020/index.html) · [동일 Battle UI 배율](Evidence/Chapter3AB_ArtReworkV2_20261020/01_BattleScale_v2.png) · [공식128px 비교](Evidence/Chapter3AB_ArtReworkV2_20261020/02_128px_Official_vs_v2.png) · [v1→v2](Evidence/Chapter3AB_ArtReworkV2_20261020/03_v1_v2_128px.png) · [단독 미리보기](Evidence/Chapter3AB_ArtReworkV2_20261020/04_v2_Standalone.png) · [전체캔버스128px](Evidence/Chapter3AB_ArtReworkV2_20261020/05_FullCanvas_128px.png) · [SHA·알파 실측](Evidence/Chapter3AB_ArtReworkV2_20261020/Validation.json)
