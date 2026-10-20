# Chapter3 채택 A·B 1포즈 — 구조 PASS / 미술 FAIL 권고

2026-10-20. `codex/chapter3-ab-art-review-20261020`. **Codex 검수 의견은 이번 픽셀아트 납품본의 수정 권고(미술 합격 보류)**이며, 바울 대표의 최종 미술 승인·반려를 대신하지 않습니다. 디자인 방향 A/B 채택은 유지합니다. 신규 제작·픽셀 수정·Unity Import·애니메이션·Push/main 병합0.

## 1. 정본·ZIP 검증

GitHub [Issue #30](https://github.com/HappyPaul812/Project-Limitless/issues/30) 본문/댓글4개와 `docs/planning/Chapter3_SelectedAB_PixelArt_1Pose_VisualReview_Gate_20261020.md`(blob `6de1fa6f008f6667bb9cd03f8a05828afecd0570`)를 직접 확인했습니다. 이전 콘셉트 선택과 이번 픽셀아트 승인 대기를 구분했습니다.

- ZIP: `F:/Downloads/Limitless_Chapter3_ApprovedAB_PixelArt_1Pose_v1.zip`, 150,573 bytes.
- SHA256 **`4bd67210e4d813b1ceb5c22cd76a35af577837b63068af57392c41226da968fc`**, 기획/Manifest 일치.
- 표준 zipfile.testzip()==None, 실제 해제6파일: Sprite2·Preview1·128px QA1·Manifest.md1·Offline_QA.json1. CRC·파일별 SHA·PNG512×512 RGBA 디코딩·Manifest/QA의 SHA와 alpha bbox 일치 **PASS**.

| PNG | SHA256 | 가시 bbox 크기 | 실제 정보량 |
|---|---|---|---|
| FrostFieldMouse_A_PixelArt_1Pose.png | `868dbfa4f7be623509f844c5dd10fc0a51ebe690b4aa2ca0c2853cf6f671d5a3` | 225×147 | 75×49를3배 반복 |
| FrostTyrant_B_PixelArt_1Pose.png | `52c26b9359665255620addd21ed9c0f07d8bbb03923753c3ae580cd768adc251` | 426×363 | 142×121을3배 반복 |

두 후보의 그림 bbox를 3×3 블록으로 검사하면 **모든 블록의 RGBA가 정확히 동일**합니다. 제작 보고의 최근접3배와 일치합니다. 512 캔버스가512px만큼 세부 묘사를 가진 것은 아닙니다. 알파는0/255, 연결 성분1개. 내부 완전 투명 영역은 A27군집/1,521픽셀, B123군집/15,759픽셀로 측정됐습니다. 뿔 사이/팔 사이 빈 공간도 포함하므로 전부 결함이라고 단정하거나 삭제 대상으로 삼지 않습니다. 단일 연결 성분 PASS가 내부 디테일 품질을 보증하지 않습니다.

## 2. 공식 Chapter2 실제 규격과 비교 배율

MonsterDefinition의 fieldSprite GUID/fileID를 따라 공식 PNG/meta의 **첫 Idle 프레임**을 읽었습니다. 시트 전체를 한 마리로 취급하지 않았습니다. 그을음들개·열풍매·균열도마뱀·불씨갑충·불씨망령5종과 보스 열맥 거신을 사용했습니다.

| 공식 대상 | 실제 PNG / Sprite | PPU·Pivot | Field 프레임 크기 | Battle 기준1280×720 |
|---|---|---|---|---|
| Chapter2 일반5종 | 각1256×1256 시트, 첫프레임314×314 | 314 / (0.5,0), VisualScale1 | 1×1유닛 | box136×112, preserveAspect로 정사각프레임112×112 |
| 열맥 거신 | 1256×1256 시트, 첫프레임314×314 | 314 / (0.5,0), VisualScale3 | 3×3유닛 | box408×336, 정사각프레임336×336 |

근거: `BattleSceneController.cs:420` CanvasScaler1280×720, `:529` preserveAspect, `:531` 적 box136×112, `:534` VisualScale 곱셈. `FieldMonsterInstaller.cs:89`는 Field Sprite에 VisualScale을 적용합니다. `MonsterSpriteSheetAnimation`의 UI 프레임 전환은 이 box를 다시 바꾸지 않습니다. 실제 fileID·rect·PPU·가시 bbox·원본SHA는 [Validation.json](Evidence/Chapter3AB_ArtReview_20261020/Validation.json), 읽은 소스/meta 사본은 SourceSettings에 있습니다.

후보는 아직 PPU/프레임/스케일 계약이 없으므로 **최종 게임 크기를 확정하지 않았습니다**. 비교판1은 각각 일반/보스의 동일 UI box에 넣는 가정입니다. 이때 큰 투명 여백으로 A는 약49×32px, B는 약280×238px만 보입니다. 비교판2는 이 여백 편향을 분리하려고 후보 alpha bbox를 공식 들개/거신의 가시 체급에 맞췄습니다. 크기를 키워도 원본 픽셀 정보가 늘지는 않습니다.

비교판은 모두 **정적 오프라인 비교이며 Unity 캡처가 아닙니다**. 후보 원화 픽셀/팔레트/알파를 새로 제작하거나 고치지 않고 복사본을 Nearest 표시 배율로 배치했습니다. 크롭도 비교용 파생 이미지에만 적용했습니다.

## 3. 실제 미술 관찰

| 대상 | 살아 있는 부분 | 부족한 부분 / Codex 판정 |
|---|---|---|
| 서리들쥐 A | 설치류의 귀·주둥이·발·서리색 털은 구분 가능 | 둥근 몸통/큰 머리/웅크린 짧은 다리로 마스코트 인상 잔존. 긴 맨꼬리·드러난 앞니·공격적인 시선/근육 긴장이 작게 읽힘. 공식 들개의 주둥이·다리·털/명암에 비해 회청색 덩어리가 우세. **야생성/디테일 미술 FAIL 권고** |
| 설빙의 폭군 B | 가지뿔·해골 얼굴·벌린 앞팔로 웬디고 계열 윤곽은 읽힘. 형태 식별성은 개선 | 뿔/머리에는 정보가 몰리지만 몸통·손·발톱의 덩어리 분리가 약함. 낮고 압축된 몸/복잡한 밝은 외곽으로 보스의 무게감보다 장식적인 정면 아이콘 인상이 남음. 공식 거신처럼 큰 명암 면과 초점이 분명하지 않음. **윤곽 식별 PASS, 최종 보스 미술 FAIL 권고** |

**128px 검사:** alpha bbox 최대변128px로 맞춘 비교에서 A의 장난감 같은 둥근 체형은 남고, B의 뿔/해골은 보이지만 몸통·발톱 세부는 작은 회청색 점으로 합쳐집니다. 원본512 캔버스 전체를128로 축소하면 A는 약56×37px, B는 약107×91px만 차지해 손실이 더 큽니다. 공식5종은 같은128px에서 주둥이·날개·꼬리·갑각 등 큰 형태와 밝은 초점이 더 뚜렷합니다.

**과도한 확대:** 최근접 확대라는 이유만으로 모든 픽셀아트를 반려하는 것은 아닙니다. 하지만 이번 A는75×49, B는142×121 정보가3배 복제된 것이 실제 확인되며, 비교판에서 공식 아트보다 면/재질 정보가 부족합니다. 캔버스를 키우거나 PPU만 바꾸는 것으로 미술 품질 문제가 해소되지는 않습니다.

이전 반려 이미지와 채택된 고해상도 A/B 원본은 이번 ZIP에 없어 직접 픽셀 대응 비교를 하지 않았습니다. **과거 반려본과 동일 그림이라고 단정하지 않습니다.** 위 평가는 현재 납품 PNG2와 공식 Chapter2 실물에 대한 관찰입니다.

## 4. 결론·보호·승인

**ZIP/PNG/Manifest 구조 PASS / Codex 미술 검수 FAIL 권고 / USER_ART_APPROVAL_PENDING / NOT_GAME_READY.** A/B 콘셉트 선택은 유지하되, 이번1포즈를 최종 게임용으로 승인하거나 애니메이션 전체 양산하는 것은 권하지 않습니다. 바울 대표가 비교판을 보고 현재본 수용/해당1포즈 개선 여부를 먼저 결정해 주시는 단계입니다. 임의 개선본을 제작하지 않았습니다.

원본 Assets/_Project·UserData·Settings·Packages **3,546파일 SHA변경0**, 원본 Git HEAD/status 동일. 원본 Unity/공식 PNG/meta/Save 변경0, 후보 Import0, 새 애니메이션0, Push/main 병합0. 문서·비교 이미지·측정 기록만 격리 브랜치에 보존했습니다. [보호 증거](Evidence/Chapter3AB_ArtReview_20261020/Protection.json).

[비교 갤러리](Evidence/Chapter3AB_ArtReview_20261020/index.html) · [동일 Battle UI 배율](Evidence/Chapter3AB_ArtReview_20261020/01_BattleScale_ExactCanvas.png) · [가시 체급 비교](Evidence/Chapter3AB_ArtReview_20261020/02_AlphaBBox_RoleMatched.png) · [128px 가시 실루엣](Evidence/Chapter3AB_ArtReview_20261020/03_AlphaBBox_128px.png) · [전체 캔버스128px](Evidence/Chapter3AB_ArtReview_20261020/04_FullCanvas_128px.png)
