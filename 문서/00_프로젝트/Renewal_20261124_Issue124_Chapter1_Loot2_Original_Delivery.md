# 2026-11-24 / LIMITLESS 리뉴얼 / Issue #124 Chapter1 남은 잡템 2종

## 최신 발주 및 보호
- 기획 발주 https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121604210 확인. 기존 승인 침·젤 및 기본 명령5종 보호. 신규 제작 대상은 ForestSpiderSilk, VenomSnakeScale 2종만.
- 승인 숲거미 실물: #90 최종 Attack8 ZIP SHA256 58c51cb49d63cde4669dc183da8583701e7e0bc9dceae9fae5542a5573f0fc9d 일치, 그 안 Frames/ForestSpider_Attack_00_512_RGBA.png SHA256 439ef26e4528e9f57877b471f602f9cfaa2f7bcd2a86b2fce991ec2e17ae5bf2 실제 열람. #90 승인/로컬 반영 기록 6624688949/6629264307 확인.
- 승인 맹독뱀 실물: #43 v3 KeyArt SHA256 7001d199b279cb8e541fc603bd62351ab460d3b44da91bc91a2d65dc099f8fd0 일치, #97 실제 적용 Idle00 SHA256 ab03751c9ab91569c56dbb775730829a63e5c7d61f99ce29b349d63e2694fcac 일치 및 직접 열람. #97 실제 적용 기록 6763843177.
- 기존 승인 침/젤 PNG SHA256 9b6d0b82f63b2e38813c9a0a979274b82729a970d84b8851456d22ea0179edd3 / ca06a5384883e0c848c4a2e893f5f524ee0d81ef22a0420425f3c0c58a902a55 일치. 비교용 원본 바이트 복사만 수행.

## 신규 실물 납품
- Art/ForestSpiderSilk_Original_128_RGBA.png SHA256 af0270104e6f9a18230fdfc12367c12b4b4068bd0e385b9f1e2151985802369a, 29223 bytes. 흰색/아이보리 섬유 묶음과 이끼빛 소량. 최초 실타래 시안을 승인 숲거미의 이끼 낀 갈색 키틴 및 옅은 섬유질과 실제 대조하고 잎/이끼 계열 국소 색조 보완. 거미 자체나 거미줄 덫으로 제작하지 않음.
- Art/VenomSnakeScale_Original_128_RGBA.png SHA256 ab5233986383622101e0b3b168cabf6050c6d1cb1b13a34606f7beb81e74eea2, 26383 bytes. 최초 밝은 녹색 비늘 시안은 실제 승인 뱀의 색과 맞지 않아 폐기. 승인 Idle00의 흑갈색/황금색 지그재그와 능선형 외피를 바탕으로 독립 원화 재생성. 외부 원본 직접 트레이스 없음.
- 두 PNG 128×128 RGBA, alpha 0..255, 네 모서리 투명, PNG 열기 PASS. 원색과 현재 Tint 각각 실제 30/40/50px 미리보기 및 승인 몬스터·기존 침/젤 비교판. 실 30px 세부 가는 섬유는 축소됨, 비늘 조각은 오프라인에서 식별 가능. 실제 UI는 NOT_VERIFIED.
- 외부 기존 파일 2종 GitHub main 실제 512px PNG 조회·시각 확인: Material/White/spider-web.png blob bb2687b1a67e88e493a5d395f7a8097bdba981f9, dorsal-scales.png blob 226de62cdb4de9536a8ada9f1fdb7f3dc18aa706. Lorc CC BY 3.0 출처/원본 보호. 정확한 소스 PNG의 Adam7 인덱스/알파를 디코딩해 16×16 평균으로 32px, 4-bit alpha 양자화한 비교용 파생 PNG 제공. 원본 512px 바이너리 자체가 동봉된 것은 아님. 기존 아이콘은 현행 Tint, 신규는 원색으로 비교.
- 실제 main ItemDefinition 조회: ForestSpiderSilk Tint (0.85,0.9,0.95,1), VenomSnakeScale Tint (0.3,0.82,0.45,1). 원색 vs 현행 Tint 오프라인 RGB 곱 비교. 특히 기존 녹색 Tint는 뱀 비늘의 흑갈/황금 무늬를 잃게 하므로 White Tint 우선 검토 권고. 본편 설정 변경0. 실제 Unity 색공간 렌더 아님. Chapter2 공식 잡템 원본 실물은 확보 못하여 동배율 화풍 확인 NOT_VERIFIED, 기존 Codex 비교와 별도 후속 QA 필요.
- ZIP LIMITLESS_Issue124_Chapter1_Loot2_Original_20261124.zip, 3143659 bytes, SHA256 907f6a188cdc1eca792f2026ef2156b0696ae38d53b76b273a9555630d2d2f39; CRC 29/29 PASS, Manifest 27/27 SHA256+bytes PASS, SHA256SUMS 28/28 PASS. Art/ 신규 게임 후보2종만, Reference/ Source/ Review/ Docs/는 비교용. 실제 대화 다운로드 첨부, F: 자동 복사 또는 GitHub 아트 바이너리 업로드 아님.

## 상태
승인 몬스터/잡템/버튼 재제작 및 Unity 적용0. ItemDefinition/아이템 ID/수치/드롭률/판매가/GUID/meta/Scene/Prefab/Save/UserData/Discord/main 변경0. Unity6000.6.5f1/#85 보호. Codex 표적 검수 및 대표 최종 미술 승인 전 제품 교체 HOLD.
LOOT2_CANDIDATES_DELIVERED / APPROVED_MONSTER_REFS_VERIFIED / FILE_QA_PASS / CH2_STYLE_NOT_VERIFIED / GAMEVIEW_NOT_VERIFIED / USER_ART_APPROVAL_PENDING / NOT_UNITY_APPLIED.
