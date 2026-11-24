# 2026-11-24 / LIMITLESS 리뉴얼 / Issue #124 기본 도망 버튼 1종

- 최신 기획 발주 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7119305405 확인. command.flee / Kenney exitRight의 이탈 기능 참고. 공격·방어·스킬 승인 원화 보존. 기존 스킬 격리 적용 보고 7119567556 확인, 제품 작업 범위 변경 없음.
- 신규 원화 1종: 금색 출구 구조와 우향 은색 화살표, 청색 이동 궤적. 직접 트레이스/기존 Kenney 수정 없음. 우향 이탈 방향은 식별되나 아치형 출구와 청색 광휘가 포털로 읽힐 가능성 있어 **PORTAL_DISTINCTION_PARTIAL**. 기존 포털/취소/뒤로가기/Kenney 원본 PNG 실물 대조와 실제 Unity UI 검증은 NOT_VERIFIED. 미술 최종 승인 필요, 임의 채택 금지.
- Art/FleeButton_Original_128_RGBA.png: 128×128 RGBA, alpha 0..255, 네 모서리 alpha0, bbox (5,5,123,122), SHA256 b8bb8399c52ea6992810ce93eb1b56198817f789cae431a5313c855cd4bce054.
- Reference/ 승인 공격·방어·스킬 원본 각각 SHA256 cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32 / 026574c506cf03db37eca4b57a24ba74251db4d6e48f5c868cf421ca69c43413 / dd9b8a971751d597d97c216ae83b44db68b0caabaf0ea32503e8b13100baab00. 실제 파일 해시 일치, 비교 전용.
- Review/Attack_Defend_Skill_Flee_128_40_30_Actual_Comparison.png: 실제 4종 동일 크기 128/40/30px 다크·라이트 가상 슬롯, 30px NEAREST 3배 확대, 40/30px 도망 투명 PNG 별도. 실제 Unity GameView 아님.
- ZIP LIMITLESS_Issue124_FleeButton_Original_20261124.zip, 1383049 bytes, SHA256 2d406c8556e2eef67b0b6d3cceab5868ee0804036490981a273eada238221f91, CRC 12/12, Manifest payload 10/10, SHA256SUMS 11/11 PASS. 대화 첨부 실물, Windows F: 자동 복사/GitHub 바이너리 업로드 아님.
- 승인 공격·방어·스킬·전리품·Trait5/공식 길 문장 및 기존 게임 자산 보호. Unity6000.6.5f1 #85 보호, 기존 도망 로직/코드/GUID/meta/Scene/Prefab/Save/UserData/Discord/main 변경0. Unity 적용/병합/Push0.
- 상태 FLEE_PILOT_1_DELIVERED / FILE_QA_PASS / PORTAL_DISTINCTION_PARTIAL / USER_ART_APPROVAL_PENDING / GAMEVIEW_NOT_VERIFIED / NOT_UNITY_APPLIED.
