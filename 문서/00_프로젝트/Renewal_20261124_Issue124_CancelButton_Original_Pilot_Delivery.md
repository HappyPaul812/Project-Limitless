# 2026-11-24 / LIMITLESS 리뉴얼 / Issue #124 기본 취소 버튼 원화 1종

## 최신 근거
- 기획 발주 및 도망 v2 최종 승인 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121163563 확인. command.cancel / Kenney arrowLeft의 뒤로가기 기능 계승. 승인 공격·방어·스킬·도망 v2 원본 보호.
- 도망 v2 Codex 표적 검수 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121035060 보고 8fe6e0eab4b16e2f43c37d40969cdb051e285d8c 확인. 기존 PASS 유지, 보스전 제한 별도 미검증 유지. 도망 격리 적용은 별도 Codex 범위.

## 실제 납품
- Art/CancelButton_Original_128_RGBA.png: 128×128 RGBA, alpha 0..255, 네 모서리 alpha0, bbox [6,8,122,119], SHA256 3eeb5e4090ec330c60cf45ee65ebf02b7bc008b1557e0a77a1a20c5b9ba47533. 같은 원화 512px 마스터 포함.
- 은색 금속 좌향 화살표 및 금색 테두리, 문틀·마법광 없음. 기존 우향 출구 도망 버튼과 방향/실루엣으로 구분. Kenney 원본 트레이스/수정 없이 독립 생성.
- Reference/ 승인 공격·방어·스킬·도망 v2 PNG 실제 바이트 복사 및 SHA256 검증. 각각 cf775727e27be825809a6b3d9bd04b227abe3408055873f70616cc8a9ca19a32 / 026574c506cf03db37eca4b57a24ba74251db4d6e48f5c868cf421ca69c43413 / dd9b8a971751d597d97c216ae83b44db68b0caabaf0ea32503e8b13100baab00 / 58d7f66283031737f65069df6fb281a818d8c32e6ae7a6230b2f5e53783f5d73. 비교 전용, 재등록/재작화 금지.
- Review/Attack_Defend_Skill_FleeV2_Cancel_128_40_30_Actual_Comparison.png: 실제 원본 5종 동배율 128/40/30px, 밝은/어두운 가상 UI 및 30px NEAREST 3배 확대. 30px 좌향 실루엣 식별 및 도망과 구분 양호한 오프라인 관찰. 실제 Kenney 원본 픽셀 대조 및 Unity GameView 미검증.
- ZIP LIMITLESS_Issue124_CancelButton_Original_20261124.zip, 1149497 bytes, SHA256 b879e2ce1768511d0f06ab07063dbd5ea9eead3dd4fcc7165c06baa6cf6e8ef0, CRC14/14, Manifest12/12 SHA/bytes, SHA256SUMS13/13 PASS. Source 생성 원천/Docs QA.json/README 포함. 대화 다운로드 실물, F: 자동 복사나 GitHub 바이너리 업로드 아님.

## 보호/상태
기존 승인 원화 전체, Unity6000.6.5f1/#85, command.cancel 및 모든 전투 로직, 기존 Kenney 원본/라이선스, 코드/GUID/meta/Scene/Prefab/Save/UserData/Discord/main 변경0. Unity 적용/병합/Push0. 신규 취소 PNG만 후속 Codex 표적 QA 및 대표 미술 승인 대상. CANCEL_PILOT_1_DELIVERED / FILE_QA_PASS / APPROVED_REFERENCES_KEEP / GAMEVIEW_NOT_VERIFIED / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED.
