# Issue #123 · F3 환경 파일럿 기술 검수 — 2026-11-23

**FILE_TECH_PASS 3/3 / UNITY_IMPORT_PASS 3/3 / F3_SCENE_NOT_VERIFIED / ART_APPROVAL_REQUIRED**

[F3 전달 및 통합 지시](https://github.com/HappyPaul812/Project-Limitless/issues/123#issuecomment-7087453538), [11/19 복구 기록](EOD_20261119_SafeIntegration_Recovery.md), [F2 승인 B352 적용·QA](Issue75_F2_B352_OriginalApply_20261111.md).

## 납품 무결성 및 PNG별 결과

지정 ZIP `F:/Downloads/LIMITLESS_Issue123_F3_SnowPass_3Asset_ArtPilot_20261123.zip`, 1,047,761바이트, SHA-256 `5704d754355d7a4d353be8324c0c442e0b8672e623fe7492849e40c49c4e8be4`. CRC PASS, Manifest 커버리지·크기·해시 8/8, SHA256SUMS 9/9. 격리 임포트에는 ZIP의 PNG 원본 바이트를 사용했습니다.

| 후보 | 파일 / Unity 임포트 | 알파·접지 | 사용 판정 |
|---|---|---|---|
| 눈·암석 지면 접합 | PASS · 256×256, 2×2 WU, Pivot (128,128), 시험 order -27 | 외곽 알파 0, 알파 범위 x11–244/y20–235, 1픽셀 섬 15개. 바위 전면 단차가 뚜렷합니다. | 단독 오버레이 PARTIAL. 반복 타일 HOLD. 정격 간격으로 반복하면 좌우 최소 22px·상하 최소 40px 투명 이격이 생깁니다. F3 지면과의 접합 실측은 미검증. |
| 길가 저상 빙설 암반 | PASS · 320×192, 2.5×1.5 WU, Pivot (160,96), 시험 order -8 | 외곽 알파 0, 알파 범위 x13–306/y57–186, 1픽셀 섬 18개. 바위 하단과 눈이 낮은 받침으로 읽힙니다. | 길 바깥 배치 PARTIAL. 맵 적용 시 투명 하단 5px 위 접지점을 기준으로 Pivot과 캐릭터 가림 순서를 검증합니다. |
| 바닥 돌 경계 표식 | PASS · 256×128, 2×1 WU, Pivot (128,64), 시험 order -23 | 외곽 알파 0, 알파 범위 x9–246/y64–122, 1픽셀 섬 8개. 낮은 돌은 판독되며 가운데 파란 면이 물·빙판처럼 보일 수 있습니다. | 시각 경계 PARTIAL. 실제 배경에서 위험 표식으로 읽히는지 대표 판단 필요. |

8방향 연결, 알파 >0 기준의 고립 픽셀입니다. 세 PNG 모두 RGBA, 중간 알파를 포함하고 완전 투명부의 잔존 RGB와 캔버스 변의 비투명 픽셀은 각각 0입니다. 비교판과 배치 카메라에서는 잘림이나 뚜렷한 테두리 번짐을 관찰하지 못했습니다. 원본 보완을 요구할 확정 파일 결함은 없습니다. 실제 배경에서 표식 오독이 재현되면 파란 얼음 띠의 대비를 낮추거나 눈·돌로 구분하도록 수정 요청합니다. 반복 지면이 필요해지면 이음매 보완이 포함된 별도 평탄 베이스가 필요합니다.

## Unity 및 F1·F2 기준

- 별도 소형 프로젝트에서 **6000.6.5f1 (3ff58d469c8a), 숨김 BatchMode**. Sprite Single·128 PPU·중앙 Pivot·Point·Clamp·Full Rect·비압축·mip off에서 3종 임포트 및 Sprite 조회 완료. 시험 Sorting은 표의 값이며 제품 Sorting 승인 전입니다. 첫 배치는 임시 스크립트의 잘못된 임포터 속성으로 실패했고, `TextureImporterSettings`로 수정한 배치에서 임포트와 렌더가 완료됐습니다.
- 실물 meta 확인 결과 F1 ThinFrost/WornTrail과 F2 FirstSnow는 PPU128·Point·중앙 Pivot입니다. F2 승인 그루터기는 PPU128·Point·Pivot y≈0.029, Scene order 0입니다. F2 B352는 Ground -30, FirstSnow -27, Trail -24이므로 F3 후보는 지면층과 소품층을 따로 판단합니다. F1/F2 계열의 청회색 눈·암석 대비와 이어지지만 F3 바위 단면과 얼음은 더 강하고 표식 파란 면은 색의 의미 확인이 필요합니다.
- [960×720](Evidence/Issue122_123_20261123/Unity_PilotCamera_960x720.png) · [1280×720](Evidence/Issue122_123_20261123/Unity_PilotCamera_1280x720.png) · [1680×720](Evidence/Issue122_123_20261123/Unity_PilotCamera_1680x720.png)은 Unity 카메라의 RenderTexture 캡처입니다. 회색 배경의 시험 오브젝트이며 실제 F3 Scene 또는 GameView 화면은 **NOT_VERIFIED**입니다. [F2–F3 화풍 비교판](Evidence/Issue122_123_20261123/F2_ApprovedAndCandidate_vs_F3_OfflineStyleComparison.png)은 납품 ZIP의 오프라인 합성입니다. F2 승인 그루터기와 미승인 후보를 표기대로 구분합니다.
- 제품 Scene과 BuildSettings에 북부 F1·F2만 확인됐습니다. F3 Scene·실제 GameView·4.5WU 유효 폭·도보/휠체어·출입구·본편 접지/Sorting/Collider는 **NOT_VERIFIED**. F2 B352와 기존 Collider의 11/11 증거를 재사용했으며 제품 자산·Scene·Meta·GUID 변경은 0입니다.

## 승인

대표 미술 승인 및 실제 맵에서 동선·배치·접지·가림이 검증될 때까지 본편 적용 HOLD. 지면 접합은 단일 오버레이 후보로 유지합니다. #85 HOLD 유지. 담당 브랜치 `codex/ch3-loot6-contract-20261123`, 실제 보고 SHA는 Issue 댓글에 기록합니다.
