# Issue #124 · 수호의 맹세 배치 보완 · 2026-11-27

**표적 PASS.** [이전 검수의 PARTIAL 근거](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7215691718)를 기준으로 Unity 6000.6.5f1 실제 Battle에서 수호의 맹세 한 종을 비교했습니다.

| 항목 | 결과 |
|---|---|
| 배치 | 기존 Rect 460×250에서 1:1 원화가 약 250×250에 맞춰져 시전자 왼쪽에 집중됐습니다. 보완안은 보호 대상 태온과 오른쪽 미엘 각자의 발밑에 동일 원화 한 장씩 동시 재생합니다. 각 172×172px, 종횡비 1:1, 대상 ActionRoot 기준 (0, −58)이며 각 캐릭터 Sprite 뒤에 배치했습니다. |
| 밝은 설원 대비 | 금백색 Tint RGBA (1, .94, .72, .94), 그림자 RGBA (.24, .15, .06, .82), 오프셋 (0, −2)로 조정했습니다. 밝은 설원과 같은 배경의 어두운 틴트에서 두 동료의 금백색 파동을 확인했습니다. |
| 가림 | 상태 적용 프레임과 가장 밝은 구간에서 캐릭터·휠체어 및 상태·명령 HUD가 식별됩니다. 파동은 동료 발밑에 국한되며 입력 레이캐스트를 받지 않습니다. |
| 재생 계약 | 원화의 연속 7프레임(시트 슬롯 4~10), 프레임 간격 45ms, 기존 index 1(시트 슬롯 5) 상태 적용 1회 확인. 기존 프레젠터의 시계와 상태 콜백을 두 배치에 공통 사용했습니다. |

원화 재가공 없이 보완 목표를 충족했습니다. 사용한 수호의 맹세 시트 SHA256은 `a1593cd43d9d41f092192f9ddb52425b1ba09a0a36a4d398e444596f9dafda04`입니다. 휠체어 수호자와 동료 둘을 둔 격리 QA 배치 결과입니다. 제품 Unity 변경은 0건이며 철벽 v1·도발·공용 FrostNova, 기능·수치·상태 계약 및 Save/UserData를 보호했습니다. 기존 19 PASS 전수 반복, 본편 적용, main 병합, Push는 없었습니다. 표적 로그 8개, 배치 종료 0. Editor Search 인덱스 예외가 한 건 기록됐으며 대상 검수는 정상 종료했습니다.

- 밝은 설원 1280×720, index 3 · [보완 전](../../Evidence/Issue124_GuardianOathLayout_20261127/Light_Before_3.png) · [보완 후](../../Evidence/Issue124_GuardianOathLayout_20261127/Light_After_3.png)
- 어두운 틴트 1280×720, index 3 · [보완 전](../../Evidence/Issue124_GuardianOathLayout_20261127/Dark_Before_3.png) · [보완 후](../../Evidence/Issue124_GuardianOathLayout_20261127/Dark_After_3.png)
- [밝은 설원 상태 적용 index 1](../../Evidence/Issue124_GuardianOathLayout_20261127/Light_After_1.png) · [표적 로그](../../Evidence/Issue124_GuardianOathLayout_20261127/unity-summary.txt)
