# 리뉴얼 아트 담당 전달문 — VenomBee Sting·Recover 최소 보완 준비

바울 대표 지시, Issue #63 후속입니다. [실제 Battle 좌표·권리 조사](OwlSpider_FinalApproval_BeeTarget_RightsContract_20261103.md)를 먼저 읽어 주세요. 이번 Codex 작업은 PNG 제작 지시를 실행한 것이 아니라 후속 제작 기준을 준비한 것입니다.

## 지금 제출할 계획 문서

1. 적은 화면 왼쪽, 아군은 오른쪽입니다. 후열 적도 공격 시 대상 앞까지 이동하며 최종 y는 대상 ActionRoot와 같습니다. 전열/후열·다른 Column을 포함한 +X 공격축으로 계획해 주세요. 기존 TEST_ONLY 우하단 보드를 실 Battle 좌표라고 재사용하지 마세요.
2. Sting 침뿌리·침끝S·몸 중심을 315×253 원본 pixel(좌상단 원점)과 normalized 좌표로 제시해 주세요. 축은 복부 뒤 독침을 유지한 채 몸통·복부를 돌려 오른쪽 표적으로 설명해야 합니다. 머리 앞에 침을 붙이지 마세요. 정확 회전 각도를 임의로 확정하지 마세요.
3. 대상 접점C는 아직 미확정입니다. 임의 숫자를 제품 좌표로 확정하지 말고 계획도에는 PROPOSED_CONTACT / NOT_RUNTIME_VERIFIED로 표시해 주세요. 실제 target Image·preserveAspect·flip 변환과 정지거리 결정은 기획/구현 담당 게이트입니다. 현행 중심 간격145 UI로는 기본 플레이어와 사각형 사이도 최소31.2 UI 간격이 남습니다.
4. 피해는 명목 .25초, 10FPS index02에 대응하지만 프레임 이벤트 연결은 없습니다. 전체1초 공격도 약.61초에 잘릴 수 있습니다. 이를 아트만으로 해결됐다고 보고하지 마세요. 시간표/접점·거리 승인 전 TARGET_CONTACT_HOLD입니다.
5. SourceDeclaration: 이번 후보 제작 과정, 사용 원본/레이어, PilotBee 참고·파생 여부, 외부 이미지/도구 사용과 권리 근거, 제출자가 보유·허가한 권리를 문서화해 주세요. 새 SHA만으로 독립 저작권이라고 주장하지 마세요. PilotBee CC BY3/4 불일치는 RIGHTS_HOLD 유지입니다.

## 게이트 해소 후 허용할 최소 아트 범위

Sting·Recover **새 독립 후보2개만** 보완합니다. 지금은 신규 PNG/전체20프레임을 제작하지 않습니다. 아래 기존 파일을 덮어쓰지 말고 후속 이름으로 분리해 주세요.

| 보호/기준 파일 | SHA256 | 후속 범위 |
|---|---|---|
| Attack_Prepare_REPAINT_v2_315x253.png | `e337921236b7b947f92bbb5d88824bc6729d6668a050a85fc2f519ff4df26212` | 그대로 보호 |
| Attack_Sting_REPAINT_v2_315x253.png | `391ed94e50ffc4ddcf3bc4442e9913e0b947cc9d5d9a57aaf865f46556e021ce` | 침끝 +X·몸통/복부 회전·날개 뿌리/다리 연결만 보완 |
| Attack_Recover_REPAINT_v2_315x253.png | `2afc69e803334b1ededdadb39e786f297d8fe3d9918e72997943c9f1593c740e` | 찌른 축에서 침 회수→복부·다리·날개→Idle 연결 |
| KEEP_Idle_Folded_238x215.png | `d17114176c13c2911cd533a0e7a363a1f4ae2ca110b540bcec5d63a2aa941605` | 그대로 보호 |
| KEEP_Idle_Spread_238x215.png | `d35f4583c0fa119396a40a6637f48b36b677fe00327640c655d86bde3dd47820` | 그대로 보호 |

315×253 RGBA·고정 캔버스·몸 중심 anchor를 유지해 주세요. 캔버스 확대·단순 전체 mirror·침 앞머리 부착·날개/관절 순간 점프 금지. 현행 Idle10+Attack10과 이전 오프라인20 전체 보존. Owl Attack4·Spider Walk8 USER_ART_APPROVED이므로 추가 수정하지 마세요. Spider Attack8/Shoot8, Chapter4 메카닉, 승인 Idle/정지원화 작업 금지.

## 후속 비교 납품 기준

- 정확 접점·거리/시간 계약 승인 번호를 README에 명시. 미승인 가정은 PROPOSED로 표시.
- 실제 새 Sting/Recover2 PNG의 이전/이후 전체 SHA·pixel delta·Manifest/SUMS. 동일 파일 재납품을 개선이라고 보고하지 않기.
- Prepare→새 Sting→새 Recover→보호 Idle의 128px/40px 순서 비교, 몸 중심/침끝 궤적/목표 방향 표시. 40px 검토 크기와 현재 UI115.6×95.2 사각형을 구분.
- **제안 시간표**: 10FPS의00~01 Prepare,02~03 Sting,04~06 Recover,07~09 Idle. 기존 포즈 반복이므로 새10장 양산 아님. GIF는 실제100ms 단위와 순서/반복을 명시하고 .25초 피해 창을 표기. 이는 현행 Runtime 동기화 PASS가 아닙니다.
- 루프 강제 반복보다 1회 Prepare→Sting→Recover→Idle 복귀를 우선 비교. 실제 공격조기종료 문제는 runtime 게이트로 남김.
- SourceDeclaration/권리 증거 및 RIGHTS_HOLD 상태 포함. 최종 미술·전투 교체 완료 선언 금지.

docs/art-renewal 보고서·WORLD_ART_RENEWAL_LATEST·#63에 계획/게이트/실제 제출 SHA를 기록해 주세요. 본편 Unity/Scene/Prefab/MonsterDefinition/Save/UserData·main Push/merge는 별도 승인 전 금지입니다. 이 지시서 작성이나 GitHub 댓글은 담당 세션의 실제 제작 착수·완료를 의미하지 않습니다.
