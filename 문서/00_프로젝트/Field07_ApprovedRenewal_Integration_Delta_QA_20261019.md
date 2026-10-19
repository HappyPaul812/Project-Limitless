# Field07 승인 리뉴얼 통합 준비·실제 Unity delta (2026-10-19)

## 승인·무결성·적용 경계

사용자 미술 승인 완료. ZIP SHA `5d398295b6f98491a294628a18763b6d72e0dedfdf5f691ccd2e8d0539150403`, CRC PASS, 27파일/PNG18/Manifest15 전부 검증. 정식 실사용14만 branch의 `Art/Field07Renewal`에 추가하고 기존 Scene의 비도로 Renderer만 매핑했습니다. 새 PNG는 납품 SHA 그대로, 기존PNG/meta/GUID 덮어쓰기0. Geometry/Collider/Spawn/Quest Script 참조는 원본과 동일하고 Sprite/Color/GroundFlip만 변경했습니다. Importer PPU/Pivot/Point/NoMip/무압축은 실제 Unity 확인. Road6개 RGBA 접합 조건도 일치합니다.

`codex/field07-v1-unity-integration-20261019`, 기능 `546ab604acd48baf36d4df7a1aaac0251d2b7e09`. **원본 디스크에는 이 Field의 Scene·코드·PNG를 반영하지 않았습니다.** branch의 원본 Scene에는 비도로 통합만 있고 기존 남북 Road30은 HOLD, A30/B42는 격리 Runtime 배치·좌표JSON으로 준비했습니다. A30은 x=-4..10, B42는 x=-10..10/y±0.5. B42는 추가 Road12이며 서쪽 조사·출구(-8부근)까지 연결되어 Field05 동서축과 일관됩니다. **B42 권장, 대표 최종 구조 승인 전 원본 확정 금지**. 기존717QA/다른 Main 대규모QA 반복0. Push/PR/main병합0.

Unity6000.6.5f1 Batch/기존 격리 Client 사용, 사용자 원본Editor/Play/Refresh/포커스0. RenderTexture는 실제 Unity 렌더링이며 PNG합성/테스트빌드가 아닙니다. 물리 이동은 QA용 별도 Session/Save로 수행했고 사용자 Save진행을 사용하지 않았습니다. Main15·16의 12목표 정의/원문/ID·스토리 관련 C#/전투/몬스터/세린/Save Asset은 보존했습니다. **12단계 전체 실제 이야기 진행·Main16 지정전투 승리는 이번 delta에서 재플레이하지 않았으므로 새 PASS로 계수하지 않습니다.** 전환 검수는 기존 정의를 읽어 SceneTransitionService를 호출한 것이며 실제 출구 Trigger 도보 관통 QA와 구분합니다. 원본Editor 손플레이·자연Import 미검증입니다.

원본 보호: 기존4235파일 중 승인Main10 기존3외4232 SHA 그대로, Main10신규37 정확SHA/PCM, UserData23·설정 보호. Main09승인27/Main18·19/Arbel·Field05/Field08원본 변경0. HEAD/origin/main `675988c9f2c5c507d9a696b0378be27c0bd3309b`. Main09·10 미병합 브랜치는 그대로 보존합니다.

## Field07 delta 결과 — 정적/비교 준비 / 최종 통합 PARTIAL

주 실행 324 PASS / 1 FAIL. 실패1은 앞서 설명한 **Field06→Arbel exact Spawn 편차**이며 Field07의 새 Collider 변경이 아닙니다. A30/B42 환경372/384, 두 안의 실제3화면비·휠체어 접근·Save/Continue·도보/청각의길 분기는 완료했습니다. 초기 이동 중 막힌 자동 경로는 도달 가능한 경로 재탐색만 수행했고 Scene/Collider를 바꾸지 않았습니다.

기존 Road Color(0.58,0.5,0.43)가 새 그림을 중복 어둡게 하므로 최종 배치에는 **Color.white/Flipfalse**를 사용합니다. 이 수정 후 화면만 재검증 **Visual 20 PASS / 0 FAIL**. 앞선 After의 어두운 Road 화면은 참고이며 최종 제출 화면은 Visual 폴더입니다. 두 안 좌표는 그대로, 이동/Story 전체 재반복0.

Runtime WindlessAsh7은 새 불투명 Ground를 복제해 **작은 직사각형7개로 보이는 문제가 실제 확인**됐습니다. 위치·pulse·조사 기능 유지, 선택 신규Runtime PNG 미적용. 해결 후보는 기존 투명 AshCrack Decal의 제한된 부분을 사용해 모양을 바꾸는 최소 Sprite 선택/크기 수정입니다. 균열과 재의 표현이 달라질 수 있으므로 **이 문서에서는 제안만 하며 코드·기능에 자동 적용하지 않았습니다**. 대표가 기존 자산 대체안 사용을 결정하거나 승인 범위를 별도 좁힌 뒤 최소 화면 검증이 필요합니다.

## Field07↔08 / Field08 공식9종 보호

Bridge 21 PASS / 0 FAIL: 기존 서비스 왕복6전환 요청/Scene 도착, Field08 Save/Continue·Quest 상태 유지. exact Spawn은 로그로 따로 기록, 6개중5개 오차<0.001이며 Arbel1개만0.4548683. Field07→08 동쪽(7,0), Field08→07 서쪽(-7,0)은 일치합니다. Main16완료 상태를 QA세션에 구성한 연결 전용 측정이며 Main16실전 승리 재검증으로 주장하지 않습니다.

실제 동일camera 조건의 Field07서쪽/Field08동쪽을 비교했습니다. 회갈색 재지대에서 붉은 균열로 팔레트·패턴 차이는 크지만 서로 다른 Scene의 의도된 지역 전환이며 통로를 끊는 새 Alpha구멍·새 아트 요구 근거는 확인하지 못했습니다. **NO_NEW_ART_REQUIRED**. 기존 Main17 공식 PNG9+meta/Material/Scene 보호SHA는 Field08OfficialSHA.json, 7사용/2미사용 기존계약 유지, 새제작/덮어쓰기0. Field08 신규자산·Tint·Decal 보완안도 적용하지 않았습니다.

다음: Field07 Road최종안(B42권장) 선택, WindlessAsh 기존자산 최소 대체안 결정 → 국소 수정·화면/조사 접근만 재검증 → 원본충돌검사 후 안전적용. Arbel편차는 별도경로 조사로 분리합니다.

[실제 비교 화면](Evidence/Field07_ApprovedRenewal_20261019/index.html) · [QA수치](Evidence/Field07_ApprovedRenewal_20261019/QA_Summary.json) · [보호](Evidence/Field07_ApprovedRenewal_20261019/OriginalProtection.json).
