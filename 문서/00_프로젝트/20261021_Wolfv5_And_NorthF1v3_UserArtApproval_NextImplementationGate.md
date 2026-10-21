# LIMITLESS — 2026-10-21 Wolf v5 / Chapter3 북부 F1 v3 바울 대표 최종 미술 승인 및 다음 실제 구현 게이트

- 기획 담당 `docs/planning`. 사용자 명시: Codex Delta QA를 확인한 뒤 **“보니까 괜찮아.. 승인..”**. 승인 대상은 직전 한 번에 검수·제시한 **Wolf v5 단일 포즈**와 **F1 v3 전체 6개 환경 PNG**.
- 이 승인은 **현 미술안을 게임 제작 정본으로 고정하는 승인**이며, 실제 Unity GameView 미실행·원본 게임 적용/메인 병합 완료를 뜻하지 않는다. 특이사항: Wolf v5의 Codex 미술 HOLD 판정과 보이는 결함을 사용자 스스로 알고 받아들인 **알려진 잔여 결함을 수용한 승인**. 검수자가 Wolf PASS를 판정했다고 바꿔 쓰지 않는다.
- 바울 대표 제공 Codex 결론: branch `codex/art-review-wolfv5-northf1v3-delta-20261021`, 최종 QA commit `a6472c51054fdc6933cb065c21a9438d041b1595`. 두 패키지 ZIP/PNG 기술 PASS, 기존 전체 QA 재반복 없음, 사용자 원본4,351파일/Save/UserData/Editor 유지, 원본 게임 적용0, main 직접 commit/Push/merge0. 이미지 비교는 모두 **오프라인 합성**, Unity GameView 아님.

## A. Wolf v5 — **USER_ART_APPROVED_WITH_KNOWN_REMAINDER**

- 사용자 제공 `F:\Downloads\Limitless_Wolf_KeyArt_v3_TargetedAlphaFix_v5.zip`; ZIP SHA256 `920459eb2ef4e160461b2b6425a9b183da633cd14411655a4d5affdbaf3d8bc7`.
- 승인 정적 1포즈 `Wolf_KeyArt_v3_AlphaFix_v5.png`, 952×1306 RGBA, SHA256 **`b770c73c36b9035a2bb0e70b8c126090b5fa248d27db542af077094d12ee2ea1`**. v4→v5 RGB 변경0/Alpha 감소2,174픽셀, 원본 v3/v4 바이트 보존.
- 기술 PASS, **Codex 미술 HOLD** 이유: 뒷다리 옆 검은 돌기 Alpha 흔적이 일부 남음. Codex는 원본 왼쪽 위 좌표계 **x=150~194 / y=1125~1174**의 검은 돌기 국소 Alpha 정리를 최소 후속 보완으로 권고함. 그러나 **바울 대표가 현 결과를 직접 보고 '괜찮다, 승인'**했으므로 **추가 Wolf v6 의무 제작 없음**, 해당 결함은 **KNOWN_ACCEPTED_VISUAL_REMAINDER**로 기록. 기존 파일 수정 없이 현 v5를 기준으로 고정하고 다음 **실제 게임용 Sprite/Run 변환 때만** 실 GameView 128px에서 검은 돌기가 플레이에 영향을 주는지 추가 QA, 화면에서 문제 없으면 유지. 새 검수 근거/대표 재결정 없이는 추가 alpha 정리 강요 금지.
- Wolf v5는 **고해상도 단일 포즈 KeyArt일 뿐 공식 Companion Wolf 애니메이션용 4행 Run SpriteSheet/PPU/Importer 계약 아님**. Art 승인으로 즉시 기존 펫 자산 덮어쓰기/MonsterDef/BeastCompanionSystem 교체 금지; Codex가 기존 Wolf 펫 모션/엔진 배열 계약 읽기전용 확정한 뒤 기존 v5 정본으로 후속 애니메이션 발주 가능.
- 맹독뱀 v3는 별도 Codex 미술 정적 PASS 권고이나 **이번 사용자 명시 승인 대상이 아님**. 그대로 사용자 승인 대기, 새 뱀 제작/전체 애니메이션 발주 자동 X.

## B. Chapter3 F1 — **USER_ART_APPROVED_6_OF_6**

- 사용자 제공 `F:\Downloads\Limitless_Chapter3_NorthF1_Environment_6_TargetedFix_v3.zip`, ZIP SHA256 **`488a65c051d1d017787771e0169124e47437b472911caa9949fdd59e55079054`**.
- Codex 격리 Delta QA **기술 PASS / F1 전체 미술 PASS 권고**. 기존 v2 PASS의 지면·흙길·서리 관목 3개는 바이트/SHA 그대로, 신규 v3 얇은 서리·전환부·길 표지판 3개 추가 PASS. **바울 대표 최종 미술 승인**으로 **F1 6 PNG 정확한 SHA를 아트 정본**으로 고정.
- 기존 v2 SHA 그대로 KEEP: `NorthF1_Ground_Earth_Base_v2.png` SHA `ccf2c22e90bee292ecf2d0d030dad53e0032457272ebb94fccdf6491fabcf8b9`; `NorthF1_Path_WornTrail_Overlay_v2.png` SHA `e78b41d48707dfce6a79dd4e826f864c5c2608b45a0f457af414ee9768a20175`; `NorthF1_Prop_FrostShrub_01_v2.png` SHA `6c83c88881e7f89245ed580d50424b2671629ad9491da93f247aab99bfe4746e`.
- 신규 v3 SHA 승인: `NorthF1_Ground_ThinFrost_Base_v3.png` SHA `12fe03d156944e1b72aec4e807770ee8016c61fe1de5804366819e716b97c30c`; `NorthF1_EarthToFrost_Transition_Overlay_v3.png` SHA `93126dd1cda0f3b97823ae4756344600ed7f6af619eb42013d1ec45a4933c4d1`; `NorthF1_Prop_Waymarker_01_v3.png` SHA `6e55b64dbc2e906a50cf8b7310849bb156a90bc40436f28a86832e59e232b589`.
- F1 세부 아트 승인 ≠ Unity Scene Import/Collider·Tile Grid/PPU·Pivot·Sorting·자동 전환 타일 4방향 검증. 현재 **원본 Unity에 정식 적용0, 실제 GameView/접근성 통행/카메라 및 Main22 퀘스트 검증0**, `main` 병합0. 길은 시각 PASS일 뿐 휠체어의 물리 통행 가능성 미검증.

## C. 다음 실행 — 무중복 우선순위

1. **Codex만** 별도 `codex/chapter3-f1-unity-preflight-20261021` 격리 worktree에서 기 승인 ZIP 6 PNG 읽기전용 SHA/CRC 재확인, 기존 Chapter2 공식 지형과 StarterVillage→NorthF1 Scene/PPU/TileGrid/Pivot/Collision/Sorting/Camera·Main22 퀘스트5목표/휠체어 동선 **기존 Issue #35 조사 내용을 우선 재사용**해 게임적용 제작계약을 잠금. 가능하면 기존 격리 Unity에서 **F1 6개 승인 시트의 최소 TileGrid/Scene 시각 프리뷰 및 3개 화면비 GameView**를 **새 씬 샌드박스에만** 구성하고, 실제 Unity/GameView와 오프라인 자료를 확실히 구별. 원본 local main/Editor·Scene·Quest/Save/UserData 불변; PR/main Push·병합 금지.
2. Codex가 기술 격리 확인 후 F1 Main22 첫 플레이의 실제 Scene/Quest/Spawner/검수 결선·원본 적용 계획을 제시; 아직 전투 1종 `서리들쥐` 외 신규 몬스터 전체양산이나 보스 `설빙의 폭군` 전투시트 추가 발주 X. 앞서 Codex 서리들쥐 Combat-v2 승인 PNG 연결 격리 Battle 40PASS/0FAIL 완료한 기능을 다시 작성하거나 전체 QA 반복 금지.
3. 별도 `docs/art-renewal` Wolf v5의 미술 승인·잔여 결함 수용을 문서화하고, **다음 펫 동작 애니메이션 생산은 실제 PPU/Run/MonsterSprite 계약 후** 발주. 늑대 키아트와 Chapter3 F1 환경 작업 범위 혼합 금지.
4. 기획은 이전 사용자 요청대로 신규 스킬·강화 스킬 아이콘, 따끈 어묵탕/냉기 대응 소모품, 첫 몬스터 잡템·스킬 상점·인벤토리 UI 아이콘 누락을 별도 내부 자산대장으로 관리. 상품 ID/가격/갯수/스프라이트 규격 임의 확정 금지; Main22 첫 플레이 병목 해결 우선.

## 정확한 상태 코드

`WOLF_V5_CODEX_TECH_PASS / WOLF_V5_CODEX_ART_HOLD_REMAINDER_KNOWN / WOLF_V5_USER_ART_APPROVED_AS_IS / F1_V3_CODEX_TECH_AND_ART_PASS / F1_V3_USER_ART_APPROVED_6_OF_6 / UNITY_RUNTIME_QA_PENDING / ORIGINAL_UNITY_NOT_APPLIED / MAIN_NOT_MERGED`.
