# Field06 승인 리뉴얼 통합 준비·실제 Unity delta (2026-10-19)

## 승인·무결성·적용 경계

사용자 미술 승인 완료. ZIP SHA `d8551df7315495dcc6e4556ec33f30b711af8aeb85dc7b0a1c3612a5a6197f42`, CRC PASS, 41파일/PNG29/Manifest22 전부 검증. 정식 실사용14만 branch의 `Art/Field06Renewal`에 추가하고 기존 Scene의 비도로 Renderer만 매핑했습니다. 새 PNG는 납품 SHA 그대로, 기존PNG/meta/GUID 덮어쓰기0. Geometry/Collider/Spawn/Quest Script 참조는 원본과 동일하고 Sprite/Color/GroundFlip만 변경했습니다. Importer PPU/Pivot/Point/NoMip/무압축은 실제 Unity 확인. Road6개 RGBA 접합 조건도 일치합니다.

`codex/field06-v11-unity-integration-20261019`, 기능 `c16456b3c1cea337bb758fc3d7e66f9830abdfd7`. **원본 디스크에는 이 Field의 Scene·코드·PNG를 반영하지 않았습니다.** branch의 원본 Scene에는 비도로 통합만 있고 기존 남북 Road30은 HOLD, A30/B42는 격리 Runtime 배치·좌표JSON으로 준비했습니다. A30은 x=-4..10, B42는 x=-10..10/y±0.5. B42는 추가 Road12이며 서쪽 조사·출구(-8부근)까지 연결되어 Field05 동서축과 일관됩니다. **B42 권장, 대표 최종 구조 승인 전 원본 확정 금지**. 기존717QA/다른 Main 대규모QA 반복0. Push/PR/main병합0.

Unity6000.6.5f1 Batch/기존 격리 Client 사용, 사용자 원본Editor/Play/Refresh/포커스0. RenderTexture는 실제 Unity 렌더링이며 PNG합성/테스트빌드가 아닙니다. 물리 이동은 QA용 별도 Session/Save로 수행했고 사용자 Save진행을 사용하지 않았습니다. Main15·16의 12목표 정의/원문/ID·스토리 관련 C#/전투/몬스터/세린/Save Asset은 보존했습니다. **12단계 전체 실제 이야기 진행·Main16 지정전투 승리는 이번 delta에서 재플레이하지 않았으므로 새 PASS로 계수하지 않습니다.** 전환 검수는 기존 정의를 읽어 SceneTransitionService를 호출한 것이며 실제 출구 Trigger 도보 관통 QA와 구분합니다. 원본Editor 손플레이·자연Import 미검증입니다.

원본 보호: 기존4235파일 중 승인Main10 기존3외4232 SHA 그대로, Main10신규37 정확SHA/PCM, UserData23·설정 보호. Main09승인27/Main18·19/Arbel·Field05/Field08원본 변경0. HEAD/origin/main `675988c9f2c5c507d9a696b0378be27c0bd3309b`. Main09·10 미병합 브랜치는 그대로 보존합니다.

## Field06 delta 결과 — 기술 PASS / 최종 통합 PARTIAL

Runtime **349 PASS / 0 FAIL**. Ground315 모두 활성, 기존 숨김49 구멍 해소. 새 승인PNG에만 Main15의 legacy 숨김/Tint를 건너뛰는3줄을 추가했습니다. 레거시 Scene은 기존 동작, 기존 Runtime6 Quad/Decal2 선택 PNG는 추가·교체하지 않았습니다. 따라서 이전 갈색/검정 사각 Quad는 실제 화면에 그대로 남는 기존 시각 항목이며 이번 범위의 해결로 주장하지 않습니다.

실제 A30=환경363, B42=375. 16:9/4:3/21:9 동일 camera 기준, 중앙 도로에 바닥이 관통하는 Alpha 틈이 보이지 않음을 확인했습니다. Wheelchair4 접근점×2안·도보키보드 동서 경로, Collider 동일·4 Encounter actual actor·세린·Save/Continue 확인. RockNorth85×54/PPU100 포함14Importer 검증. 소품 Alpha-foot/Rect/PPU 실측은 feet.tsv. 이전v1과 Props7 PNG가 동일하므로 기존 foot차이(Fence+0.078125, RockWest-0.1484375 world 등)는 임의좌표보정 없이 그대로 기록합니다. 그림과 기존Collider 접지의 최종 사용자 판단은 별도이며 물리좌표변경0입니다.

연결 보조 진단은 Field07 브랜치 Bridge 증거를 따릅니다. Field06↔07 및 Arbel→Field06 Spawn은 맞습니다. **Field06→Arbel exact Spawn 검사 FAIL**: 정의(-6,5), 실제(-5.545132,5), 차이0.4548683. Arbel 원본/격리 Scene SHA 동일, 원인 확정 전 Collider/Spawn 변경금지·별도 OPEN. 전환 Scene 로드 자체는 성공했습니다. 이 이슈를 PASS에서 제외하고 원본 적용을 보류합니다.

다음: B42 또는 A30 최종 선택 → 좌표JSON대로 Road만 확정·12 Renderer 필요시 추가 → Arbel Spawn 편차 원인 확인 및 Props접지 미결 확인 → 변경분QA → 충돌/백업/SHA 후 원본 안전 적용. Runtime6/Decal2는 계속 범위 밖입니다.

[실제 비교 화면](Evidence/Field06_ApprovedRenewal_20261019/index.html) · [QA수치](Evidence/Field06_ApprovedRenewal_20261019/QA_Summary.json) · [보호](Evidence/Field06_ApprovedRenewal_20261019/OriginalProtection.json).
