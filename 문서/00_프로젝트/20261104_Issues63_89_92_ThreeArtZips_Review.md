# LIMITLESS / 2026-11-04 / #63·#89·#92 세 ZIP 기술·오프라인 미술 검수

- **담당:** 총괄 기획 / `docs/planning`
- **사용자 제출 PC 경로:** `F:\Downloads\VenomBee_ORIGINAL_3POSE_ISSUE63_20261104.zip`, `F:\Downloads\LIMITLESS_Cold_Status_Icons_3_GameReady_20261104.zip`, `F:\Downloads\LIMITLESS_ISSUE92_CH2_BATTLEBG_2PILOT_20261104.zip`.
- **직접 검사 바이트:** ChatGPT Library에 저장된 동일 이름의 ZIP 3개를 실제 열어 SHA256/ZIP CRC, SHA256SUMS, Manifest, 이미지 크기·미술 비교판을 확인. **PC F: 자체 ZIP 전체SHA는 기획 세션에서 직접 재계산하지 않음**; GitHub 담당 납품 SHA와 보관 ZIP 전체SHA 정확히 일치.

| 이슈·ZIP | 라이브러리 실측 전체 SHA256 | ZIP/SUMS/Manifest/이미지 | 시각적 판정 |
|---|---|---|---|
| #63 VenomBee_ORIGINAL_3POSE | `172df8f2d658b02f6859554284ab4b719175b2ce86b922fe5270f4ae9803b325` | CRC PASS, SUMS **11/11**, Manifest **11/11**, 신규 315×253 Prepare + 238×215 IdleFolded/IdleSpread **RGBA 3장** | 신규독침벌 Sting/Recover v3 승인 모양과 대체로 부합. `NEW3_ART_APPROVAL_RECOMMENDED / USER_VISUAL_REVIEW_PENDING` |
| #89 Cold_Status_Icons_3 | `f758b59b73847f18c13e0d48aafee2f58eac33802ec597a1c739078335b2baa3` | CRC PASS, SUMS **24/24**; Manifest는 항목별 파일 목록이 아니라 설계/아이콘 계약. 실 PNG 128RGBA 3, 12/14/20/40 실제 축소 크기 자료 | 14px 무채색에서도 눈송이/세 기둥/금 간 얼음의 형태 차이 확인. `ICON_ART_APPROVAL_RECOMMENDED / ACTUAL_UNITY_14UI_HUD_NOT_VERIFIED` |
| #92 Ch2_BATTLEBG_2PILOT | `e439cd32bda173063ee05fd3cb3fd8a099dc4f3a6a67b805150b2777ba6b7ff0` | CRC PASS, SUMS **10/10**, Manifest **9/9**, 새 1672×941 불투명 RGB 2장 | 잿빛 화염황야(05~08)·흑요석 균열지대(09~10) 지역구별/중앙 전투지면·적왼쪽 아군오른쪽 오프라인 자리 확보. **Ch1 공식 배경 원본 실제1:1 화풍 비교 누락**, 어두운 적/상태 아이콘 실 Unity 가독성 미검증. `CH2_TWO_BG_ART_REVIEW_PENDING` |

## 중요한 신규 사실 및 기존 오류 정정
- [#63 Codex 새 출처·실제 게임 잔존 조사](https://github.com/HappyPaul812/Project-Limitless/issues/63#issuecomment-6626600381): 11/03 기존 Prepare·Idle2 또한 **신규 생성 작업 이력**이 있었음. 이전 기획의 '기존 PilotBee 유래로 확정' 진술은 **사실과 다른 과단정**. 새 11/04 Prepare·Idle3 후보도 별도 작품이므로, 무조건 앞선 세 장을 폐기하거나 이전3과 신3을 모두 채택하지 말고 **최종 디자인/재생 일관성 비교 후 한 벌을 선택**. Sting/Recover v3는 사용자 승인 SHA 바이트 보호.
- 기존 PilotBee 원본 Sprite 2장이 **`Assets/_Project/Resources/MonsterValidation/PilotBee`**에 있고 `02_VenomBee.asset`가 해당 GUID를 직접 참조한다는 Codex 정적 확인 기록이 있음. **이미 출시빌드에서 배제 완료라고 주장 금지**. 바울 대표가 기존 PilotBee를 최종 미사용하기로 정했으므로 CC BY 3/4 버전 충돌 추가 해결은 불필요; **Resources/정의/아틀라스 잔존 제거 및 BuildReport 확인은 제품 적용 승인 이후**.
- [#89 B안](https://github.com/HappyPaul812/Project-Limitless/issues/89#issuecomment-6624594309) 50PASS/0FAIL, 기존 99PASS/0FAIL. 이번 신 아이콘은 **독립 아트**로, 실제14UI≈12px Unity HUD/시인성 QA와 사용자 미술 승인이 없으므로 이전 PASS를 신 아이콘 PASS로 옮기지 않는다. 한기·동상·괴사 상태명·남은 턴/중첩 필수.
- Chapter2 후보 2PNG는 [#92 담당 납품](https://github.com/HappyPaul812/Project-Limitless/issues/92#issuecomment-6626875471)과 전부 일치. 오프라인 1280×720의 도형 캐릭터/HUD는 실 Unity 캡처가 아니고, Ch1 배경 참조판은 **기존 기준의 문자 정리이지 실제 Ch1 원본 PNG 화소 패널이 아님**. 사용자 그림 최종 승인 전에 화면 품질까지 PASS라고 주장하지 않는다.

## 다음 결정·QA 경계
1. 바울 대표의 실제 비교판 **미술 승인**이 나면 #63 신규3·#89 상태 아이콘3·#92 배경2를 각각 독립 승인 기록. **미술 승인 자체가 실제 원본 Unity 적용 승인은 아님**.
2. Codex에 시킬 때는 필요 시 F: ZIP 전체 SHA만 한 번 비교하고, 기존99/50/15PASS 등 QA 재실행 금지. #63은 실제 신규5포즈 조합 공격→Idle/외부원본 최종 제외, #89는 B안 HUD에 정식 상태아이콘 최소 렌더/14UI, #92는 실제 Ch1-vs-Ch2 화풍 및 승인 뒤 실 전투 3인 HUD/적왼쪽 아군오른쪽 GameView 최소 델타만 **격리6000.6.5f1**에서 검증.
3. Unity 원본 Editor/Scene/Prefab/meta/MonsterDefinition·BattleCatalog/Save/UserData/사용자 미커밋/승인아트 보호. `main` 직접 커밋, 원격 Push/병합 없음. 실제 적용은 별도 승인 필요.

**최종 상태:** `LIBRARY_THREE_ZIPS_TECH_PASS / USER_NEW_ART_VISUAL_APPROVAL_PENDING / F_DRIVE_SHA_NOT_RECHECKED_THIS_TURN / UNITY_NOT_APPLIED`.
