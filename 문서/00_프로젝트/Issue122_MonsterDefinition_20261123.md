# Issue #122 · Chapter 3 정식 MonsterDefinition 등록 Delta (2026-11-23)

**등록 1/6 · HOLD 5/6 · Unity Delta 10 PASS / 0 FAIL · DROP_LINK_HOLD 6/6**
브랜치 `codex/ch3-monster-definitions-20261123`, 제품 `8706671145c59fdbb0aa4c1ba268edfcbe65ec86`.

## 등록

- 서리들쥐 `frost_vole`를 기획 ID 규칙으로 `Resources/MonsterDefinitions/19_FrostVole.asset` 및 신규 meta에 등록했습니다. Lv17·HP200·ATK28·민첩12·EXP60·탈렌트16. 기존 공격 생성식에 맞춰 `battleAttackPercent=280`, 정의 민첩 사용을 설정했습니다.
- 기존 단일 근접 기본 공격 경로를 사용합니다. 추가 방어·상태·지원 몬스터·드롭 엔트리는 없습니다. 실제 조우가 승인되면 전열 편성, 기본 공격 100% 및 표시 연출을 실전에서 확인해야 합니다.
- 기존 승인 FrostFieldMouse A Idle4/Attack4/Hit4/Defeat4의 Sprite 16개를 직접 연결했습니다. 필드 Sprite는 승인 Idle 첫 셀입니다. 기존 시각 타이밍·스케일 기준을 사용했고, 워크 애니메이터나 스폰은 추가하지 않았습니다. Main22 프로토타입 자산과 ID는 그대로입니다.

## 미등록 5종

| 종 | 아트 확인 | HOLD 이유 및 다음 조건 |
|---|---|---|
| 설원족제비 | Recovery에 승인 KeyPose·Idle4·Attack4 v2·Hit4·Defeat4 확보 | 기본 물기 110% → 대상 예고 → 돌진 160% 및 대상 KO 취소를 종별로 연결할 경로가 필요합니다. 승인 시트의 정식 Sprite 연결과 패턴 Delta가 선행돼야 합니다. |
| 서리올빼미 | QA 리소스의 Idle8·Attack4 확인. 피격·퇴장은 기존 공용 연출 | 한기와 후열 전투는 현재 Main24 QA 경로에 묶여 있습니다. 정식 원거리·예고·90% 한기 규칙 및 승인 시각 경로로 연결할 수 있어야 합니다. QA ID나 Definition을 정식화하지 않았습니다. |
| 얼음갑충 | Recovery에 승인 KeyPose·Idle4·Attack4·Hit4·Defeat4 확보 | 갑각 닫기의 다음 유효 직접 피해 -50%, DoT/0 피해 비소비 및 자기 다음 행동 만료를 보장하는 종별 런타임과 방어 연출 계약이 필요합니다. |
| 눈보라 정령 | 승인 KeyPose·Idle8v2·Attack4·Hit4·Defeat4와 시각 Prefab/클립 존재 | 시각 참조는 가능하나 정식 후열 마법·예고·80% 동상 및 재부여 제한을 연결할 런타임이 필요합니다. 시각 Prefab과 공식 MonsterDefinition을 구분했습니다. |
| 빙각 수호체 | 승인 몬스터 본체·모션의 실물 연결 근거 미확인. 전리품 PNG는 존재 | 승인 아트와 110% 주먹·대상 예고·180% 강타 경로가 필요합니다. 불변 공식 조우 키·수령 대기·실제 Save는 별도 승인/검증 항목입니다. |

기획 근거 [북부 7종 전투 정본](../08_몬스터/Chapter3_북부몬스터7종_전투밸런스_20261118.md) 및 [조우 조사](Issue122_124_Encounter_Vision_Audit_20261123.md).

## Delta와 보호

- Unity 6000.6.5f1 숨김 배치, 제품 MonsterDefinition/PoisonDefinition 및 기존 정의 18개와 승인 Sprite만 복사한 소형 프로젝트. **10 PASS / 0 FAIL**. 신규 ID 충돌·카탈로그 19개, 승인 능력치, 프레임 4개씩과 직렬화 링크, 미설정 드롭·추가 공격을 확인했습니다. [로그](Evidence/Issue122_MonsterDefinition_20261123/delta.txt)
- 첫 배치는 소형 프로젝트에 내장 Animation 모듈 누락으로 컴파일 중단됐습니다. 임시 프로젝트에만 해당 모듈을 선언했고 두 번째 배치가 통과했습니다. 제품 Package 변경은 없습니다. 전체 런타임 Play/Save 미검증이며 기존 32/56/39 PASS 또는 아트 QA 재실행은 0입니다.
- 제품 파일 변경은 신규 .asset/.meta 2개입니다. 기존 전리품 6종·RNG 패치는 그대로입니다. 실제 스폰·조우·드롭·F3~F5 Scene·엘리트 Save·폭군 변경 0. #85 HOLD, Unity 버전, 승인 자산, 사용자 작업·Save/UserData·Discord 보호. 원본 main 병합과 원격 Push 0.
