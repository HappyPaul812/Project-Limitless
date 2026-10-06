# Main17 Reward 감사 및 최종 정책

2026-10-06 LOCAL QuestDefinition과 RewardBundle.TryApply/QuestService 완료 코드를 감사했다.

| Quest | 기존 EXP | 기존 탈렌트 | 근거 |
| --- | ---: | ---: | --- |
| Main12 | 60 | 50 | Main12_RemainingRecord.asset |
| Main13 | 60 | 50 | Main13_DryingLand.asset |
| Main14 | 60 | 50 | Main14_GroundPulse.asset |
| Main15 | 60 | 50 | Main15_BurningTraces.asset |
| Main16 | 0 | 0 | Main16 문서에 미확정으로 비워둠 명시 |
| Main17 | 0 | 0 | 이전 Main16 계약을 따른 임시값 |

별도 Quest Reward가 없는 것이 Chapter2 전체 정식 정책이라는 근거는 없었다. 차이를 사용자에게 보고했고 **사용자가 Main17 EXP60·탈렌트50 적용을 명시적으로 선택했다**. 기존 Main12~15의 실제 수치를 재사용하며 새 수치를 invent하지 않는다. Main16과 다른 Quest는 변경하지 않는다.

최종 Main17 RewardBundle: Experience=60, Currency=50, Items 없음. 전투 보상과 별개로 마지막 decide_to_withdraw 완료에서 지급한다. 기존 QuestService.TryComplete는 완료 기록으로 중복 지급을 막고 지급 후 같은 슬롯에 저장한다. 신규 Save Version/Flag/별도 보상 시스템은 추가하지 않는다. 이전 ‘빈 보상’ 기록은 당시 이력이며 이 문서와 최신 Main17 설계가 현재 정본이다.

변경: Main17_RedRift.asset 및 Main17ContentBuilder.BuildQuest의 재생성 보상 값. 마지막 실제 대화 완료·60/50 증가·중복 Notify 무효·완료 Save/실제 시작메뉴 Continue 이후 재지급 없음 검증을 Main17_Closure_Runtime.txt에 기록한다.

최종 Runtime: 일반 Lv12/EXP0→60, 레벨업 경계 Lv12/(RequiredExp12-30)→Lv13/EXP30 모두 실제 마지막 대화에서 지급했다. 두 경우 모두 중복60·50지급0·실제 Continue 후 정확한 Level/EXP/탈렌트 유지·Party/Beast 유지·Field08 BGM 복귀 PASS.
