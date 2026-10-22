# LIMITLESS / 2026-10-22 서릿길 마을 v6 + Bear·Fox Run6 v1 실제 격리 Unity QA 결과 및 표적 보완 결정

## 근거 및 상태
- 바울 대표가 Codex 결과 화면 전달. 이 기획 세션은 사용자 로컬 Windows F:와 Unity를 직접 접근/실행하지 않았으므로 아래 SHA readback 및 결과는 **Codex 스크린샷 보고**. 전날 EOD 승인 완료 Wolf/Chapter3/F1/main 원본은 보호.
- 전3개 ZIP 실제 SHA256 납품 예상값 일치, CRC/Manifest/SHA256SUMS PASS.
  - Village VariantTiles v6: `519ed96fa6157b7de451830412d6e881b633241b0f8cc459d186465765a4ea7e`
  - Fox Run6 v1: `6d352219505497a261ffe2c1d687cece28bdf4859003d16fce88334d9a15f43d`
  - Bear Run6 v1: `1a29475207c0f99db87726b637d301e5010bf5ef6790e2415ad679f67ffd7f19`
- Codex branch `codex/qa-village-v6-bear-fox-run6-20261022`, 화면 보고 커밋 `6df8e1d5`, `9f9b560b` (실제 commit 전체 SHA·로컬 worktree 여부는 Codex 보고 필요).
- 격리 실제 Unity: Village 실제 Camera/RenderTexture 3화면비 및 통행 proxy 6/6 PASS, 기존43 PNG byte-identical 및 결합 PASS; Bear/Fox 12FPS·128px·40px 재생 검증. **세 모듈 모두 기술 PASS, 미술 HOLD**, 제품원본 변경0, Push/병합0. SearchDatabase 기존 환경 예외 OPEN 1. 사용자 원본4,400파일·Wolf·Save/UserData·미커밋·HEAD/status/index 보호.
- **OPEN16 기존 CSV/좌표JSON** 기반으로 **유지16 / 해결0**, 출처 원문·좌표·수정 분류 전달자료 준비(실제 사용자 외부 공유 완료 여부는 미확인). 원본에 없는 severity 추측 금지.

## A. 서릿길 Village v6: ART_HOLD 지속
- Earth/ThinFrost/Gravel 각각 01+02+03 혼합 5×5에서도 **공통 모티프·주기성 반복**. 기술 edge PASS!=미술 PASS. 과거 v5 기존43원본·SkillShop/Gable/StonePath KEEP.
- **신규아트 표적보완 v7 권고(아직 납품·승인 아님):** 모티프 반복 원인을 실제 Unity 좌우 v5↔v6 화면에서 파일/타일 좌표로 근거화. 01을 다시 전면 제작하지 않고 02/03의 풀·큰돌·서리/자갈 군집을 **실제 서로 다른 배치와 실루엣**으로 국소 재제작. 기존 같은 패치 3장 교환만 반복 금지. 필요시 소규모 별도 무작위가 아닌 **규칙·밀도 제한된 overlay/decal 후보**를 별도 아트 후보로 제작하되 Codex의 TileGrid/renderer 계약 없이는 런타임 자동배치·새 GameObject 양산 금지.
- v6의 바깥12px 모두 같게 유지하는 제약이 눈에 띄는 네모 테두리 반복을 만들 수 있으므로, 동일지형 변형 간 **실제 접합 edge 1픽셀/인접접합 검사**를 기준으로 확장 가능한 최소 고정경계 폭을 판단. 보존필수 영역은 Codex가 실측한 PPU/Tilemap 및 기존 접합에 따라 결정. 색상 톤·통행·건물 접합을 유지하면서 바닥 무늬만 진짜 다양하게 한다.
- 5x5 뿐 아니라 8x8 이상의 3변형 불규칙 배치 및 동일 배율/카메라 실제 게임뷰에서 전후 비교(기술 자료와 미술감상 별도). 기존 OPEN16은 분류확보 전 임의 결함수정/해결 주장 금지. 사용자 승인 전 Main23 본편·원본 변경0.

## B. Bear Run6 v1: ART_HOLD
- **05→00 프레임 사이 머리 높이 튐** + 관절 접지·중량 지지 불합리. 00·05를 연속적으로 다시 설계하고, 필요한 인접 프레임 04·01에 한정해 머리/가슴·앞발/뒷발 높이와 무게중심·발 접지 순서를 정합. 정적 포즈6이 기술PASS여도 달리는 동작 부자연스러우면 보류.
- 소형 약120px 원본 보드 추출·캔버스 확대에서 생긴 해상도·선명도 한계 주의. 다른 승인 정적 Bear KeyArt 존재를 확인하지 못했으므로 '원본 승인 Bear 기반' 주장 금지. 신규 v2는 실제 필요한 포즈를 시각적 자연스러움 우선으로 손질, 단순 flip/warp/alpha fill로 대체 금지. 프레임 몸길이·머리·관절 좌표와 12FPS 05→00 GIF/APNG/Unity 비교 필요.

## C. Fox Run6 v1: ART_HOLD
- **앞다리 좌우 구분/중첩, 꼬리 굴곡 실루엣의 시간적 일관성 부족**. 우선 실제 12FPS에서 겹치거나 분간되지 않는 프레임별 앞다리 위치·발 착지/이탈을 재드로잉하고, 00~05 꼬리 몸통 연결부/끝 윤곽을 자연스러운 한 호(arc)로 유지. 교차 장면에서는 앞다리 overlap에 대비·명암차 주되 인공 외곽선 남용 금지.
- 소형 소스 확대 화질·꼬리/귀/털 흐릿함과 프레임별 체형 비율 검수. 기존 6프레임/12FPS·512×384 등 기술 규격은 실측 계약으로 유지하며 필요시 관련 포즈만 수정. Bear와 독립 ZIP/QA·미술 사용자 승인.

## 공통 작업 지시·게이트
- 부서별 독립 변경: 신규아트 `docs/art-new`, 리뉴얼 `docs/art-renewal`. 게임용 공식 Wolf PNG/Controller 및 10/21 PR#48·PR#49 승인 main 재검증·재반영 금지.
- 신규 v7 Village, Bear v2, Fox v2는 각각 **비공식 후보**. 기존 납품 ZIP 원본/기존43 PNG 보존, 새 ZIP 개별 SHA/CRC·Manifest, Source 개별 SHA/프레임diff+동일조건 실시간 동작 비교, 미술 HOLD 특정 원인 개선 증거, 실제 Unity 검증 별도. 상업권리/AI 출처 및 미술 품질 한계 명시.
- 별도 Codex 검사 후 user art approval, 그 후 런타임 PR/merge. 사용자 PC 원본·Save·UserData·Editor·dirty 보호, main 직접 commit/force push 금지.
- 빠른 업무 방식: **마을 타일 수정과 Bear/Fox 모션 수정은 동시 발주**. Codex가 보유한 OPEN16 원문/좌표자료는 `docs/planning`에서 읽을 수 있도록 개인정보/절대경로 제외한 항목별 정확한 인계 후 art-new가 관련 항목만 검토. OPEN16 전체 무작위 재작업 금지.
