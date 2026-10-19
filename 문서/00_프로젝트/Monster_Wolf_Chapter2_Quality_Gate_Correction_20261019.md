# LIMITLESS / 몬스터·Wolf Chapter2 공식 아트 품질 게이트 및 잘못된 비교 이미지 정정 (2026-10-19)

- 담당: Chapter1·2 리뉴얼 아트 / GitHub `docs/art-renewal` 전용.
- 기획 최신 근거: `docs/planning/문서/00_프로젝트/Field09_Zip_Header_Repair_And_MonsterWolf_Chapter2_Quality_Gate_20261019.md` 실물 본문 확인.
- 사용자 평가: VenomSnake v2와 Wolf v2의 과도한 픽셀 블록감·미술 완성도 부족. **v2 USER_QUALITY_HOLD**, 기존 v1 명시적 반려 유지.
- **수정 공지**: 이 담당 세션이 별도 생성한 'LIMITLESS Monster Concept Art' 장식용 이미지 2개의 하단 비교란은 실제 Chapter2 공식 5종 PNG를 조합한 것이 아니며, 게임 내 World Size/PPU/화면 RenderTexture를 측정한 자료가 아님. **공식 이미지 품질·배율 비교 증거로 사용 금지**, GAME_READY·USER_ART_APPROVED 표시 금지. 대형 원화가 내부 애니메이션 시트와 정합된 실제 적용 가능 Sprite라고도 주장 금지.

## 기준 5종
`11_SootHound`, `12_HeatwindHawk`, `13_FissureLizard`, `14_EmberBeetle`, `15_EmberWraith` 5종 공식 원화의 품질 기준: 실루엣·비례·층진 명암·질감·광원·자연스러운 근육/비늘/털/동작, 소형 화면 판독성. `11,12` 등 실제 Sprite Sheet meta에는 314×314 셀/PPU314 기준이 등장하지만 전 자산에 314 강제 아님. `32×32` 단순 확대/인위적 선명도 보정/공식 아트 픽셀 복제 금지.

## 현재 정확 상태
`V1_REJECTED / V2_USER_QUALITY_HOLD / CHAPTER2_QUALITY_TARGET_CONFIRMED / ACTUAL_OFFICIAL_5_ASSET_COMPARISON_NOT_VERIFIED / V3_KEYART_STYLE_APPROVAL_PENDING / FULL_ANIMATION_NOT_AUTHORIZED / WOLF_BEAR_FOX_HOLD / UNITY_UNCHANGED`.

다음 단계는 (1) 공식 원본 Sprite5종의 실제 단일 프레임과 .meta/PPU/Transform 검증 (해당 원본 파일이 직접 열리지 않으면 Codex 로컬 읽기 전용 조사), (2) **맹독뱀 Key Art 1장 + Wolf Key Art 1장**을 고해상도 독립 제작하고 별도로 단일 PNG 납품, (3) 5종 원본과 실제 World Size·동일 RenderScale/구조 기반으로만 비교판 제작, (4) 바울 대표 미술 스타일 승인 후에만 20+6 애니메이션 제작. Wolf 승인 전 Bear/Fox 제작 보류.

원본 v1·v2 ZIP / 5 공식 아트 / Scene·GUID/meta·Source·Quest·Save·AI·펫 규칙 변경0, `main` 직접 commit·Push 금지. Field09 ZIP 헤더 정상화는 별도 Codex 기술 트랙이며 본 아트 품질 게이트에 포함하지 않음.
