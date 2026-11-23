# Issue #124 · 길 능력 아이콘 5종 최종 화면 검수 — 2026-11-23

**실제 길 선택 UI 비교 5/5 완료 · 40px 판독 PASS · 30px 시각 PASS / 나머지 4종 PARTIAL · 본편 교체 HOLD**

## 검수 환경과 증거

Unity 6000.6.5f1 (3ff58d469c8a) 숨김 격리 Play에서 제품 `PathSelectionController`, `PathPresentationResolver`, 5개 PathDefinition 및 원래 참조 Sprite를 사용했습니다. 기존 외부 5개·공식 문장 5개의 GUID 연결을 유지했고, 5개 후보는 납품 ZIP에서 읽었습니다. [시각 v2 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7089737097) · [Trait4 납품](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7090172309)

- 1280×720, 1:1 픽셀로 상세 UI의 30px를 캡처하고 같은 위치의 아이콘만 40px로 확대해 비교했습니다. 40px은 제품 기본 슬롯이 아닙니다. 카드의 24px 표시와 공식 문장·특성명은 그대로 뒀습니다.
- 실제 제품 UI 렌더를 Camera RenderTexture로 캡처했습니다. 입력은 실제 카드 Button 콜백을 사용했고 격리 세션 저장과 설정 진입은 대체했습니다. 전체 게임 Scene 전환·사용자 입력은 미검증입니다.
- 캡처 20장(5종×2크기×기존/후보), 기존/후보 차이가 상세 Trait 사각형에만 있는지 픽셀 차이 검사 **10/10 PASS**. Image는 흰 틴트·비율 유지, 어두운 배경 캡처 샘플은 RGB (17,22,32)였습니다. 후보의 흰색·냉청색 외곽은 모두 배경에서 분리됐습니다. 격리 Editor Search 초기화 예외 1건은 엔진 초기화에서 발생했으며 캡처와 차이 검사를 완료했습니다.
- [5종 30/40px 실물 캡처 비교판](Evidence/Issue124_Path_UI_20261123/Path_Trait_30_40_Unity_Comparison.png) · [30px 전체 화면 기존](Evidence/Issue124_Path_UI_20261123/vision_30_current.png) · [30px 전체 화면 후보](Evidence/Issue124_Path_UI_20261123/vision_30_candidate.png) · [검사 로그](Evidence/Issue124_Path_UI_20261123/screen-check.txt)

## 화면 기준 판정

| 길 | 30px | 40px | 기존 외부와의 차이 |
|---|---|---|---|
| 시각 | **PASS** · 눈과 동공 식별 | **PASS** | 기존 흰 눈의 실루엣이 더 즉각적. 후보는 청색 호·4방 표식이 추가되고 금색 공식 문장과 구분됩니다. |
| 청각 | **PARTIAL** · 귀보다 고리 윤곽이 먼저 보임 | **PASS** · 귀와 파장 구분 | 기존 파형은 작은 크기에서 한눈에 읽히고 후보는 인체 모티프가 추가됩니다. |
| 지적 | **PARTIAL** · 결절 사이 연결선 일부가 뭉침 | **PASS** · 결절과 연결 구조 인식 | 여백이 넓은 기존 네트워크가 선을 구분하기 쉽습니다. |
| 지체 | **PARTIAL** · 원형 바퀴는 보이나 궤적이 약함 | **PASS** · 바퀴와 방향 궤적 구분 | 기존 톱니의 외곽이 명확하고 후보는 이동의 방향을 보여 줍니다. |
| 마음의 상처 | **PARTIAL** · 하트는 보이나 균열·봉합이 약함 | **PASS** · 하트와 균열 구분 | 기존 하트 방패가 단순하고 후보는 회복·상처의 표현이 추가됩니다. |

후보 다섯 종은 30px에서 공통 냉청/백색 프레임이 먼저 읽힐 수 있으므로 특성명과 함께 판독했습니다. 차례대로 공식 문장과 별도 위치에 보이며 두 역할을 구분할 수 있습니다. 최종 미술 판단과 교체 승인 전 본편 적용 HOLD. 기존 파일 QA 반복 0, 새 그림·제품 에셋 수정·main 병합·Push 0, #85 HOLD.

로컬 브랜치 `codex/issue124-trait-ui-final-review-20261123`. 실제 SHA는 Issue #124 댓글 참조.
