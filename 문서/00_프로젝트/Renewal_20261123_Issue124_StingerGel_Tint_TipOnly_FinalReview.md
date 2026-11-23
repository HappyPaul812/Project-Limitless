# LIMITLESS / 리뉴얼 아트 / Issue #124 / 독침벌의 침 v2 + 초원 슬라임 젤 Tint 검토
2026-11-23 / 브랜치 docs/art-renewal

## 근거
Codex의 2026-11-23 격리 검수: https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7096499843
Codex local codex/issue126-124-defeat-stinger-20261123 SHA 3c77498e09741649e2daa0ea2c9aef0c71ec5254.
128/50/40px PASS, 30px 침끝·독방울 PARTIAL, Gold Tint/Ch2 아이콘 화풍 PARTIAL. 이번에 Codex 실제 Unity QA를 재실행하지 않았다.

## 원본 바이트 보호
- 독침벌의 침 Organic v2 128×128 RGBA: VenomBeeStinger_Original_v2_128_RGBA.png SHA256 9b6d0b82f63b2e38813c9a0a979274b82729a970d84b8851456d22ea0179edd3.
- 초원 슬라임 젤 기존 128×128 RGBA: GrassSlimeGel_Original_128_RGBA.png SHA256 ca06a5384883e0c848c4a2e893f5f524ee0d81ef22a0420425f3c0c58a902a55.
- 기존 원본2개 ZIP에 *원래 파일 바이트 동일*로 포함. 디자인/PNG 교체0. 정확한 몬스터 이름 '초원 슬라임', 내부 식별자 GrassSlimeGel 그대로.
- 기존 Unity ItemDefinition 실제 값: VenomBeeStinger gold (r1,g0.72,b0.18,a1), GrassSlimeGel green (r0.55,g1,b0.38,a1). 기존 외부 스프라이트 GUID 원본 그대로.

## 결과
- 30/40/50px 크기로 원본 RGB에 기존 Tint vs White Tint를 곱해 LANCZOS 실축소, 두 아이콘 각각 밝음/어두움 배경 비교판과 투명 PNG 제공. Unity GameView 아님.
- Stinger: White Tint에서 검정·금색 키틴 + 자주색 독침 재질/색상 분별 증가; Gold Tint에서 색이 주황으로 눌리는 현상. 30px 독방울은 White라도 약하므로 좁은 국소 보완 후보만 제안.
- GrassSlimeGel: White Tint에서 원화 고유 녹색 하이라이트/명암 보존. Green Tint는 R/B 축소로 색감과 세부 대비를 약화. 원화 재제작 불필요; White tint 격리 화면 QA 먼저 권고.
- 새 독침 원화 전면 재제작0. 유일 선택 후보: VenomBeeStinger_v2_30px_TipOnly_OPTIONAL_128_RGBA.png SHA256 d614ed5250e790efb1ed57268aba4cfe99f5ad9982e2eb6a53c7f82a669de233. **기존 128 PNG 중 침끝·독방울 59픽셀만 차이**, 좌표 bbox x[23..31], y[106..116]. 나머지 128 전체 픽셀 동일. 사용자 미술 승인 전 채택 금지.
- 화풍: Chapter1 기존 두 전리품 후보 실제 파일로 비교. Chapter2 공식 잡템 실물 바이너리는 이번 작업환경 미보유이므로 직접 픽셀 동배율 PASS 주장 금지; Codex Ch2 STYLE_PARTIAL 유지.

## 제출물 및 무결성
- ZIP LIMITLESS_Issue124_StingerGel_Tint_TipOnly_20261123.zip; 418286 bytes; SHA256 a62442885359d75abd556cca01f648e1c7d71058dcf44050391860eff9cb2a98.
- ZIP CRC 26/26 PASS, Manifest payload24/24 파일크기+SHA256 PASS. Art/ 원본2·선택후보1, Review/ 30·40·50px 기존/White tint 비교·정적 보드·candidate, Docs/QA.json/README.md, Manifest.json, SHA256SUMS.txt.
- 이 ChatGPT 대화 컨테이너 다운로드 경로 /mnt/data/LIMITLESS_Issue124_StingerGel_Tint_TipOnly_20261123.zip. 대표님 Windows F: 자동복사 및 GitHub 이미지 바이너리 업로드0.
- Unity6000.6.5f1, Issue #85 HOLD; 승인 길 Trait5, 공식 PathSymbol5, 몬스터/펫/NPC/환경, Scene/Prefab/GUID/meta/ItemDefinition/Save/UserData/Discord/main 전부 변경0. 숲거미 실·맹독뱀 비늘 PNG0.
- 현재 결론: ORIGINAL_V2_SHA_PRESERVED / GRASSSLIMEGEL_SHA_PRESERVED / OFFLINE_TINT_QA_PASS / OPTIONAL_TIP_DELTA_59PX / CH2_STYLE_NOT_VERIFIED / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED.
