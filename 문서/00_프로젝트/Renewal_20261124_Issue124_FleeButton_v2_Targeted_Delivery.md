# 2026-11-24 / LIMITLESS 리뉴얼 / Issue #124 도망 버튼 v2 국소 보완

## 근거
- Codex v1 검수 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7120230169, 보고 89dade05a51a73d68ee4488b04c952fb5e683ed8: 파일/Unity 30·40px 표시, 비활성 차단, 취소 복귀, 일반전 Field_01 복귀 PASS. 포털 구분 PARTIAL. 보스전 제한 별도 실행 미검증. 기존 기능 검수 반복하지 않음.
- 최신 기획 발주 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7120275605 확인. v1 원본 보호, 출구/광휘 부분만 수정.

## 실제 산출물
- Art/FleeButton_v2_128_RGBA.png: 128×128 RGBA, alpha 0..255, 네 모서리 alpha0, bbox [19,20,123,113], SHA256 58d7f66283031737f65069df6fb281a818d8c32e6ae7a6230b2f5e53783f5d73.
- 기존 v1 PNG SHA256 b8bb8399c52ea6992810ce93eb1b56198817f789cae431a5313c855cd4bce054 일치, Reference/에 변경 없이 복사. 승인 공격/방어/스킬 PNG 세 개도 이전 공식 SHA256 일치, 비교 전용.
- 신규 생성 편집 이미지에서 상단 직선 문틀 부분만 잘라 합성. v1 금색 기둥·문턱과 은색 우향 화살촉 재사용, 외부 청색 원호 제거, 내부 청색 마법광을 중성 어두운 갈색 문 안쪽으로 교체, 화살표 꼬리 및 문턱의 청색을 중성화. 전체 재생성 이미지로 교체하지 않음. 발광 제거로 실제 콘텐츠 bbox가 좁아졌으나 128 캔버스/기존 화살촉 위치 유지.
- 원본 또는 후보의 alpha>0인 영역 중 RGBA 동일 3134px, 변경 5362px(광휘 삭제와 문틀/내부 변경 포함). 우측 화살촉 영역(x>=88,y42..89)의 기존·후보 alpha>0 픽셀 중 동일 878px. 전체 이미지 픽셀 동일이라고 주장하지 않음.
- Review/Flee_v1_v2_128_40_30_Comparison.png: v1/v2 실제 128/40/30px, 다크·라이트, 30px 3배 확대. Review/Attack_Defend_Skill_Flee_v2_Comparison.png: 승인 세 버튼과 동배율 비교. 별도 v1/v2 30·40px PNG 포함. 오프라인 검토에서 포털 광휘 대신 일반 문틀과 우향 이탈 화살표가 식별됨. 실제 Unity v2 포털/취소/뒤로가기 구분은 NOT_VERIFIED.
- ZIP LIMITLESS_Issue124_FleeButton_v2_Targeted_20261124.zip, 354563 bytes, SHA256 931274626db9230cdaeb63e191100c68a43af14e8b50969c0ed26608ecb74fed, CRC15/15 PASS, Manifest13/13 SHA/bytes PASS, SHA256SUMS14/14 PASS. Docs/QA.json 및 README 포함. 현재 채팅 다운로드 파일, Windows F:나 GitHub 바이너리 자동복사 아님.

## 보호
기존 v1·승인 공격/방어/스킬·전리품·Trait5·공식 길 문장 보호. Unity6000.6.5f1/#85 HOLD, command.flee 기능/전투 로직, 기존 코드/GUID/meta/Scene/Prefab/Save/UserData/Discord/main 변경0. Unity 적용/병합/Push0. v2만 후속 변경부 가독성 표적 QA 대상. 대표 최종 미술 승인 및 제품 교체 대기.
상태: V1_KEEP / FLEE_V2_TARGETED_DELIVERED / FILE_QA_PASS / V2_GAMEVIEW_NOT_VERIFIED / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED.
