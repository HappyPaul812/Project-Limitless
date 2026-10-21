# LIMITLESS — Chapter1·2 리뉴얼 Wolf v5 승인본 기반 Run 6프레임 아트 제작 발주 (2026-10-21)

## 사용자 지시·담당
- 바울 대표: 늑대 v5 단일 KeyArt 1포즈를 알려진 작은 뒷다리 옆 검은 돌기까지 수용하여 최종 미술 승인. 이어서 “리뉴얼도 일 시켜” 지시. 본 작업은 **Chapter1·2 기존 야수 펫 Wolf 리뉴얼의 후속 실물 제작**이다.
- 제작 담당 `docs/art-renewal`, 구현/Unity 기술 검증 담당 Codex, 기획 `docs/planning`. Issue #43. 다른 역할 `docs/art-new`의 Chapter3 신규 아트와 중복금지.
- Wolf 승인 v5 `Wolf_KeyArt_v3_AlphaFix_v5.png` **952×1306 RGBA** SHA256 `b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`, ZIP SHA256 `920459eb2ef4e160461b2b6425a9b183da633cd14411655a4d5affdbaf3d8bc7`. v3/v4/v5 각각 원본 보존. 기술 PASS, Codex 시각 HOLD 잔여와 사용자 '현 v5 그대로' 승인 모두 역사 기록으로 유지. **다시 v6 매트 수정 요구 금지.**

## 이미 조사된 실제 펫 시스템과 과거 실패 방지
- 기존 Codex/Art 감사 `Beast_Wolf_Original_Run_Pilot_v1_Delivery_20261019.md`: `BeastCompanionSystem.cs`/`Resources/CompanionAssaultValidation/Wolf_Run.png`는 실제 런타임 연결된 ThirdParty ScratchIO 임시 참조. 현재 Legacy Wolf Run 원본 **384×40, 64×40×6, FPS12, 기본 LEFT, IconFrameIndex4, DisplayScale2, pivot 하단중앙 (.5,0), TravelSpeed570**. Bear/Fox는 독립 프레임수/크기이므로 변형 금지. 프레임수·FPS는 Wolf 신규 애니메이션 타이밍 참조, 이 구형 64×40 픽셀 규격을 신작 품질 기준으로 무조건 강제하면 **이전 v1/v2 단순·작은 만화 강아지 문제 재발**.
- 기존 `Limitless_Beast_Companions_Wolf_v1.zip` / 384×40 새 픽셀 Wolf pilot는 바울 대표 미술 반려·보류된 과거 시도. **그 Run v1을 색만 바꾸거나 늘려서 재납품 금지**. ThirdParty 원본 시트 trace/픽셀복제/재색칠도 금지.
- Sprite 크기·PPU/WorldUnit·실제 `BeastCompanionCatalog` 리소스 경로/새 메타 GUID는 **Codex 실측 후 별도 구현 계약**. 아트 담당은 Preview를 128px(현재 legacy 64px×2 화면 표시 등 비교 가능하도록) 및 실제 64×40 슬롯에 넣어 차이 보고하되, 품질 확보를 위해 **6포즈 원화와 고해상도 strip 후보를 별도 제공**한다. 고해상도 canvas/셀 규격은 원본 Wolf v5 비율·footAnchor를 기준으로 작업자가 정한 값과 명확한 `SpriteRects/PPU 후보`로 보고하고 사용 전 기술 검증 필요라고 표기.

## 제작 대상 — Wolf Run만
1. **Run 6개의 서로 다른 네발 질주/달리기 자세**를 승인 Wolf v5 모티브로 제작. 최소 한 주기 6프레임, 12FPS(0.5초) 미리보기. 단순 6장 획일 XY 흔들기/색보정/스케일 변경으로 모션 위조 금지. 앞뒤 다리 교대·몸통 무게중심·어깨와 엉덩이·꼬리 반작용·접지발 1~2 프레임·공중 비접촉 1프레임 등 동작이 물리적으로 납득되어야 함.
2. **늑대 해부학과 미술**: 승인 v5의 주둥이, 귀, 눈, 회색·아이보리 털, 가슴 털 질감, 긴 다리/발·꼬리, 날렵하고 야성적인 성견 인상 유지; '보노보노/만화 강아지', 저해상도 덩어리, 닫힌 두 다리 같은 형태 회귀 금지. 좌향/우향은 실제 게임 계약 확인까지 **한 방향 1종**으로 충분하나 기본 기존 Left를 우선 참조, 실제 소스와 화면에서는 direction/flip 명시.
3. 프레임마다 늑대의 몸 길이·머리 비율·털·눈·발톱이 통일되고, 05→00 재생 경계 순간적인 팝/색 변화 없어야 함. 동료의 습격에 필요한 질주 방향/가속감과 40px UI 아이콘 프레임4로 적절한 구분 유지.
4. **납품 정본**: 고품질 실원화 6포즈/투명 배경 원본 파일(각 frame), 가로 Run Strip 6프레임 실PNG(셀·캔버스 명시), 12FPS 루프(GIF/APNG)와 128px·40px 아이콘 비교/어두운 전투 배경 비교, 접지 line/pivot 프레임별 bbox·cell intrusion0·stray alpha QA, PNG/ZIP SHA/CRC/Manifest/source provenance, 미술 변경없는 approved v5 SHA. 포즈는 원본 v5 단일 그림을 무리하게 늘린 스캔/왜곡 대신 실질적 새 6자세여야 한다.
5. 권장 ZIP명 `Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtPilot_v2.zip`. 이전 Run v1/v2와 혼동 없게 naming, `ART_RUN6_CANDIDATE_DELIVERED / USER_MOTION_APPROVAL_PENDING / CODEX_UNITY_INTEGRATION_NOT_DONE`. PNG와 ZIP **실물**을 사용자에게 납품하되 사용자 정리된 F:\Downloads\ 폴더 위치 임의 추측/삭제/기존자료 이동 금지.

## 이번 작업 범위 외
- User가 **현 v5 정적 원화**를 승인했을 뿐 Run6 모션의 최종 아트나 런타임 화면·기술적 PPU/Scale 수정까지 승인한 것은 아님. 아트 후보 제작·오프라인 QA만 진행하고 실제 Unity Player/BeastCompanionSystem 원본 코드·PNG/GUID/MonsterDefinitions·Save/UserData/Editor/원격 main 변경/Push/merge 금지.
- Bear/Fox Run 신규 제작, Snake 애니메이션, Chapter3 신규 몬스터·보스, Field09 HeatVent/Spire 잔여, NPC/UI 대규모 작업과 섞지 말 것. 기존 승인 Wolf v5를 몰래 별도 v6로 재편집 금지.
- 다음 Codex 기술격리 회차는 **Wolf Run 실PNG 납품 뒤** 현 런타임 source 6프레임/12fps/Direction/Scale 표시구조와 새 시트 사이 Mapper/Importer 계약 및 미술 동일 크기 Unity GameView 검사. 대표 Run 모션 승인을 받은 뒤 기존 ThirdParty Wolf 참조를 새 GUID로 최소 교체할지 결정.
- 모든 문서 `docs/art-renewal` 브랜치에 기록, commit/sha/실PNG용량/프레임수·FPS/Preview/QA·다음 승인 구분 보고. 담당 최신 인계 업데이트.
