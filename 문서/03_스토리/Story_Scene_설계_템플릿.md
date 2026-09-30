# Story Scene 설계 템플릿

> 공통 규칙은 [Path 반응형 Story 정본](Path_반응형_Story_연출_규칙.md)을 참조한다. 아래를 개별 장면에 채워 넣는다. 미확정 ID·수치·Asset은 TBD로 남긴다.

## 기본 정보

- Main / 장면명 / 설계 상태:
- 발생 위치·선행 Objective·진입 조건:
- 관찰 사실 / 가설 / 아직 모르는 정보:
- 참여 Player·Companion·Story NPC와 실제 파티에서 제외된 인물의 등장 방식:
- 주요 대사 / 선택지 / 읽기·입력 시간:
- Scene 종료 조건·다음 Objective·실패/재시도:
- 기존 Runtime·Save·Navigation과 연결할 범위:
- Dialogue ID / Voice Manifest / BGM / 구현·청취 검증 상태:

## Path-Reactive Check

| 항목 | 작성 값 |
| --- | --- |
| 관련 Path | 없음 / Vision / Hearing / Intellectual / Mobility / Emotional Scar |
| 해당 Path Player 반응 | 첫 감지·관찰·판단 및 대사 |
| 일반 Player 반응 | 정보 공유자와 플레이어 참여 |
| 관련 Companion | 인물·역할 / 없음 |
| Player가 해당 Path일 때 Companion 역할 | 확인·보완·해석·추가 정보 |
| Story State 차이 | 없음 / 있음: 차이와 확정 근거 |
| Quest 진행 차이 | 없음 / 있음: 차이와 확정 근거 |
| 선택 Dialogue | 분기 조건·본문·서로 다른 Stable ID, 미정은 TBD |
| Accessibility/Representation 주의 | 자막/읽기 시간/단서 전달/장애 표현·장치·경험의 구분 |

관련 Path가 없으면 이를 명시하고 나머지 분기 칸은 해당 없음으로 기록한다. 대사의 발견 순서가 달라도 공통 사실과 최종 상태로 수렴하는 지점을 적는다.
