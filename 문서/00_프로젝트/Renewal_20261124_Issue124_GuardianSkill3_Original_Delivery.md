# 2026-11-24 / LIMITLESS 리뉴얼 / Issue #124 수호자 기본 스킬 3종 신규 원화

## 발주 및 보호 범위
- 최신 기획 발주 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7123880804 확인. 도발 skill.guardian.taunt (pawn_left), 철벽 skill.guardian.iron_wall (structure_wall), 수호의 맹세 skill.guardian.cover_allies (pawns) 3종만 제작.
- Codex 휴식 준수. 검수 요청, 호출, Unity 실행/적용0. 기존 status.taunt 별도 아이콘 제작0. status.guardian.iron_wall 및 status.guardian.cover_allies 공유 계약 변경0. 나머지 직업과 VFX 신규 제작0.

## 실제 납품
- Art/Guardian_Taunt_Original_128_RGBA.png, SHA256 960bd0a3b87eadae637a1e2243eb3eab848b28963651ba328b01e699dba5687b, 24845 bytes. 금속 방패와 중심을 향하는 붉은 위협 화살표.
- Art/Guardian_IronWall_Original_128_RGBA.png, SHA256 76954eabf4557c6c4c44aacb8b6a0a9432d005373db159b5956d63c3d14a960e, 32558 bytes. 성벽과 금속 방패, 청색 방어 강조.
- Art/Guardian_CoverAllies_Original_128_RGBA.png, SHA256 e6dd2c2516c07e8d3a415b04cb60599fbc50a62cba64a603ced686a8d4593906, 27516 bytes. 금속 방패와 두 동료 표식, 보호막.
- 각 128×128 RGBA, alpha 0..255, 네 모서리 alpha0. 생성 원본 3종 바이트 보존, 낮은 alpha<=8 제거 후 콘텐츠 비율 유지하며 116px 안에 맞추고 128px 중앙 배치. 기존 승인 공격·방어·스킬 원본은 비교용 바이트 복사만 수행, 각각 기존 공인 SHA256 cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32 / 026574c506cf03db37eca4b57a24ba74251db4d6e48f5c868cf421ca69c43413 / dd9b8a971751d597d97c216ae83b44db68b0caabaf0ea32503e8b13100baab00 일치.
- Review/GuardianSkill3_ApprovedCommands_128_40_30_HUD_Comparison.png: 승인 기본 명령3종과 신규3종 실제 128/40/30/24/20px 동배율 비교, 밝은 배경 30px 포함. Review/GuardianSkill3_30px_Actual_4xZoom.png 및 신규3종별 40/30/24/20px 투명 미리보기.
- 오프라인 시각 관찰: 30/40px에서 도발 붉은 화살표·철벽 상단 성벽·맹세 삼각형 보호막/동료로 구분 가능하나, 공통 방패 모티프 때문에 20/24px 상태 HUD에서 철벽/맹세/일반 방어 구분 약화 가능. 실제 Unity HUD 및 기존 Kenney 원본 픽셀 비교 NOT_VERIFIED, 미술 최종 승인 전.
- ZIP LIMITLESS_Issue124_GuardianSkill3_Original_20261124.zip, 4991933 bytes, SHA256 8526cfc6e041022e0b1019535bd0350f9de9b69f5fec1b98f17232fdde93c5eb; CRC27/27, Manifest25/25, SHA256SUMS26/26 검증 PASS. Art/만 신규 게임 후보, Reference/는 비교 전용. Source/ 생성 원본, Review/, Docs/QA.json 및 README.md 포함. 실제 대화 다운로드 실물. Windows F: 자동 복사 및 GitHub PNG 바이너리 업로드 아님.

## 상태
GUARDIAN_SKILL3_CANDIDATES_DELIVERED / FILE_QA_PASS / SMALL_HUD_DISTINCTION_REVIEW / USER_ART_APPROVAL_PENDING / CODEX_REST / NOT_UNITY_APPLIED.
기존 승인 잡템4/기본 명령5/Trait5/길 문장/몬스터/펫 등 원화 수정0. 기존 기능·수치·스킬 ID·코드·GUID/meta·Scene/Prefab·Save/UserData·Discord·Unity6000.6.5f1·#131·#85·main 보호. Unity 적용/병합/Push0. Codex 재개 전 검수 요청 금지.
