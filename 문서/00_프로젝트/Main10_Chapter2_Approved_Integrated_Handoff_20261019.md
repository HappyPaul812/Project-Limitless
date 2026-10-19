# Main10 + Chapter2 승인 통합 작업 최종 인계 (2026-10-19)

## 결과와 정확한 체크포인트

| 범위 | 브랜치 / 커밋 | 격리 검증 | 원본 적용 / 남은 게이트 |
|---|---|---|---|
| Main10 승인18 | codex/main10-voice-quest-interaction-fix-20261019 / 기능 `4cf37b0809cb2f278f03214114dde5ed91cfd716` / 직전보고2c1dd37c | 164 PASS/0 FAIL, 기존132재사용 | 새37+기존3 적용, 원본Editor 손플레이 미검증 |
| Field06 v1.1 | codex/field06-v11-unity-integration-20261019 / 기능 c16456b3 / 최종 `ccd06c615c732b33d1b4f8d62f5862220d64a964` | 349 PASS/0 FAIL, ZIP/14PNG/49구멍/3화면비/A30·B42 | 원본 적용0, B42권장·최종Road승인 대기, ArbelSpawn편차OPEN |
| Field07 v1 | codex/field07-v1-unity-integration-20261019 / 기능546ab604 / 최종 `6ebb4c90216bc286c08265e486b28e4f2711efbd` | 주실행324PASS/1FAIL(ArbelexactSpawn), 연결21PASS/0FAIL·최종도로Tint화면20PASS/0FAIL | 원본 적용0, Road선택·WindlessAsh7사각형 대체안 미결 |
| Field08 | Field07 브랜치 증거에 포함 | 왕복서비스/목적Spawn/Continue·공식PNG9+meta 보호 | NO_NEW_ART_REQUIRED, 새아트/원본수정0 |

모든 Push/PR/main병합0. 원본 main/origin/main/GitHub조회main = `675988c9f2c5c507d9a696b0378be27c0bd3309b`. GitHub기획 f53634fa9659b9a57ab82fc3250263428cbfe4fc, 리뉴얼3179ee8152ecf753ae5c39d18baad82bd73e718d 정본 확인. Main09 승인27은424b1912에 보존되고 Main10이 그 위에 쌓였으므로 future PR의 선행 의존 범위를 반드시 확인합니다. main을 직접 수정하거나 원본dirty를 branch로 옮겨 덮어쓰지 않습니다.

## 검증 한계와 OPEN

1. Field06/07 A30은 x=-4..10, B42는 x=-10..10/y±0.5. 서쪽(-7~-8)조사·출구와 Field05동서축 일관성 때문에 두 곳 모두B42권장. 각각 환경363→375 /372→384이며 Road12추가. 아직 최종구조승인 전이므로 원본Scene 확정/적용0. branchScene도 비도로 통합만 하고 남북Road30을 HOLD했습니다. 실물후보와 정확좌표JSON은 각Evidence에 있습니다.
2. Field07 WindlessAsh7은 불투명Ground 복제로 사각형이 실제 보입니다. 선택RuntimePNG 미적용. 기존 투명AshCrack의 작은부분을 재사용하는 최소안은 제안만 했으며 재와균열의 의미차이를 대표가 판단한 뒤 국소수정합니다. pulse/위치/조사·청각의길 분기는 유지했습니다.
3. Field06→Arbel 도착 actual(-5.545132,5) vs정의(-6,5), 0.4548683편차. exactSpawn검사1FAIL을 그대로 기록했습니다. Arbel Scene원본/격리SHA 동일, 현재 원인은 미확정. Collider/Spawn변경 없이 원인부터조사, 다른5전환은<0.001오차. Field07↔08 목적Spawn둘은 정상입니다.
4. Main15/16각12목표 정의·스토리코드·몹/보상/Save관련자산 변경0을 보호했고 접근·Save·Runtime로드 delta를 수행했습니다. **두 이야기 전체12단계와 Main16실전 전투승리는 이번에 재플레이하지 않아 신규전체PASS가 아닙니다.** 연결은완료상태를 구성한 QA세션의 기존전환서비스 검증이며 실제ExitTrigger도보관통검증과다릅니다.
5. Field06선택Runtime6/Decal2는 제외하여 기존 불투명Quad는 그대로남아있습니다. Main15Ground49숨김만 승인PNG조건에서 우회했고 레거시Scene동작은유지합니다. Propsfoot실측 차이는기록했으며 원본Collider/좌표변경0입니다.
6. 원본Editor 자연Import/사용자손플레이는 전범위미검증. Search IndexOutOfRange/URP Host type mismatch는기존기준선OPEN, 신규Runtime오류0. 원본에추가적용하지않아 기존Editor포커스/Play를조작할필요없습니다.

## 원본 보호

Task시작4235파일SHA중 승인Main10기존3변경 외4232전부불변, 새37파일은승인SHA/PCM일치. Main10백업104개/개별SHA, UserData23·Settings/ProjectSettings 백업. Main09원음/승인27·Main18/19·사용자PNG/meta·Field05/Arbel·Field08/Save변경0. 원본index수정0, mainHEAD유지. 정확검사목록은 각보고참고. 원본파일수정은 승인Main1040경로에만 제한했습니다.

## 바로 이어서 사용할 재개 지시

“Main10 승인18는 기능4cf37b0809cb2f278f03214114dde5ed91cfd716로 원본 디스크 반영·164deltaPASS 완료했으므로 다시 생성/연결하지 마세요. Field06 ccd06c615c732b33d1b4f8d62f5862220d64a964, Field07 6ebb4c90216bc286c08265e486b28e4f2711efbd worktree를 그대로 재사용하세요. 먼저 대표의 A30/B42 선택과 WindlessAsh 기존자산 최소대체안 결정을 확인하고, 승인된 Road만 LayoutJSON대로 확정하세요. Arbel0.455 Spawn편차는 Collider/Spawn을 옮기지 말고 기존충돌/배치원인을 읽기·격리 진단하세요. Main15/16 전체스토리실플레이 미검증을 PASS로 바꾸지 마세요. 필요한부분만회귀하고 원본SHA/백업충돌조건이통과한경우에만 승인범위적용하세요. Push/main병합은금지상태입니다.”

상세: [Main10](Main10_Approved18_Official_Integration_QA_20261019.md). Field06/07 상세보고·HTML은 각전용worktree의 문서/00_프로젝트/Evidence에 보존했습니다. 기존QA전체반복·신규TTS/API·Field08새그림은금지합니다.
