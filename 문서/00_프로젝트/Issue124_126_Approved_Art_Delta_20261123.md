# #124·#126 승인 아트 후속 Unity Delta — 2026-11-23

**Unity 6000.6.5f1 (3ff58d469c8a) 숨김 격리 Play. #124 화면 비교 10/10 PASS, #126 비루프·단회 시험 타격·Idle 복귀 PASS. 제품 적용 HOLD.**

## #124 길 능력 5종 리워크

[대표 최종 미술 승인](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7091428274)의 `LIMITLESS_Issue124_Trait5_Rework_20261123.zip`을 사용했습니다. 이전 후보가 들어 있던 임시 UI 프로젝트에서 후보 파일만 교체했습니다. 기존 외부 Sprite과 공식 길 문장, 기존 외부 캡처 및 파일 QA는 재사용했습니다. 실제 제품 `PathSelectionController`가 만드는 UI에서 1280×720, 흰 틴트, 비율 유지, 실제 어두운 패널(RGB 17,22,32)의 상세 30px와 동일 위치 시험 40px를 사용했습니다. 후보 10장과 기존 10장을 차분하여 변경이 Trait 사각형 안에만 있는지 **10/10 PASS**. 공식 문장 5와 카드·글자·배경 픽셀 유지. [실제 UI 비교판](Evidence/Issue124_126_Art_Approved_Delta_20261123/Trait_Rework_Unity_30_40.png) · [차분 로그](Evidence/Issue124_126_Art_Approved_Delta_20261123/ui-delta.txt)

| 길 | 30px | 40px | 기존 외부와 비교한 형태 |
|---|---|---|---|
| 시각 | PASS | PASS | 흰 눈·조준점 대신 남색 바탕의 초점과 방위 표식. 30px에서 집중 기능을 알아볼 수 있으나 눈의 외곽은 기존이 더 선명합니다. |
| 청각 | PASS | PASS | 흰 막대 파형과 달리 중심을 감싸는 대칭 공명파. 30px에서도 좌우 파형이 구분됩니다. |
| 지적 | PARTIAL | PASS | 흰 네트워크보다 입체감이 있으나 30px에서 교차하는 밝은 연결선 일부가 뭉칩니다. |
| 지체 | PARTIAL | PASS | 단순 톱니에서 회전 궤도 모티프로 바뀌었습니다. 30px에서 회전 윤곽은 보이나 중앙 기계 요소는 약합니다. |
| 마음의 상처 | PARTIAL | PASS | 기존 하트 방패는 작은 크기에서 외곽이 더 분명합니다. 리워크의 보호 형태는 남고 중심 회복선은 30px에서 약합니다. |

흰 틴트에서 냉청·은빛과 절제된 금색이 보이며 공식 금색 문장과 기능 슬롯이 구분됩니다. 위 판정은 시각 가독성입니다. 제품 교체, 다른 UI 크기 및 입력/씬 전환은 별도입니다.

## #126 빙각 수호체 Attack4 v2

[대표 최종 미술 승인](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7091428809)의 `LIMITLESS_Issue126_RimeSentinel_Attack4_v2_Targeted_20261123.zip` 사용. 승인 Idle4 시트는 읽기 전용으로 참조했습니다. v1·KeyPose·Idle4 기존 파일 QA 반복 0, 원본 변경 0.

- 실제 Unity Animator의 비루프 클립 **660ms**, 170/120/150/220ms, 0→1→2→3 순서. 실제 uGUI 136×112 슬롯과 40px에 렌더했습니다. 세 번째 셀에서 앞주먹과 어깨가 이어져 보이며 40px에서는 충격 구분에 피해 피드백이 도움이 됩니다.
- 격리 타격 이벤트 키 340ms. 120Hz 캡처의 상대 시간 로그에서 **325.0ms**, 세 번째 셀(기획 타격창 290–440ms)에 **정확히 1회** 발생했습니다. 시험 표적 HP 100→56, 피해 숫자 -44와 붉은 피격 표시 22프레임을 확인했습니다. 이 값은 격리 시험 콜백의 입력이며 실제 피해 계산 결과가 아닙니다.
- 명목 660ms 클립 종료에서 Idle 상태로 전이합니다. 캡처 상대 시간에서 최초 Idle0 **650.0ms**, Idle1 908.3ms, Idle2 1,175.0ms로, 이벤트와 전이의 로그는 시간 원점과 프레임 샘플 위치에 따라 저작 키와 약간 다릅니다. Idle 클립은 승인 규약 260ms/셀, 1.04초입니다. 공격 재시작 0, 피격 색·숫자 해제 확인.
- [타격·피해 피드백 캡처](Evidence/Issue124_126_Art_Approved_Delta_20261123/AttackV2_Impact.png) · [Idle 복귀 캡처](Evidence/Issue124_126_Art_Approved_Delta_20261123/AttackV2_IdleReturn.png) · [재생 추적](Evidence/Issue124_126_Art_Approved_Delta_20261123/attack-v2-check.txt).
- **제품 전투 연동 미검증.** 실제 근접 Presenter, 피해 계산·프로덕션 VFX, F5 정식 조우에서의 타격·정렬은 별도 승인·검증입니다. 현재 제품의 균등 FPS 재생기는 이번 격리 비균등 Animator 경로와 다릅니다.

격리 Unity Editor Search 초기화 예외 1건을 확인했으며 결과 캡처는 완료했습니다. 제품 Unity·공식 아이콘/아트·main·Save/UserData·Discord 변경 0, #85 HOLD, 병합·Push 0. 담당 브랜치 `codex/issue124-126-approved-art-delta-20261123`, 실제 SHA는 두 Issue에 기록합니다.
