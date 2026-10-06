# Story 대사 및 지문 작성 규칙

Character 대사는 화자와 발화 본문을 기존 방식으로 표시한다. Player 대사는 Voice와 Portrait가 없다.

행동·상황을 설명하는 지문은 `<지문> 내용` 형식으로 명시한다. 예: `<지문> 상황을 살피고 있습니다.` 지문에는 Character Speaker Name, Character Portrait, Character Voice를 붙이지 않는다. DialogueLine이 prefix를 인식하면 화자 ID와 표시 이름을 비우고 IsDirection으로 분류한다. Presenter는 지문에 Clip을 Resolve하지 않고 이전 Voice를 정리한다. 지문 페이지는 Next 순서에는 포함하지만 Character 발화/Voice 수에는 포함하지 않는다. Story 식별용 ID가 있어도 Character 음성을 재생하지 않는다.

빈 화자만으로 지문을 추정하지 않는다. Player·System·선택 페이지와 기존 무음 대사는 유지한다. 행위 표현 자동 검색은 후보 수집에만 사용하고 Story 문서와 Runtime 분기 근거가 있는 문장만 전환한다. Narration 음성인 Intro18은 이 Character 지문 규칙으로 일괄 변경하지 않는다.

## Main04 확정 근거

`MainQuest04FieldFlow.CreateStoryActor`는 태온과 미엘에 같은 대기 문장을 설정한다. 이 문장은 정식 First/Encounter/AfterBattleConversation 밖에 있고 Manifest와 331 Matrix의 authored Dialogue에도 없다. [Main04 정식 설계](메인_스토리와_퀘스트_설계.md)의 현장 관찰 및 치료 상황에 대한 공통 상태 설명으로 분류한다. 독립 Sequence ID는 없으므로 새 Dialogue ID를 만들지 않는다. 정식 전투 후 미엘009는 실제 Character 대사이며 지문으로 바꾸지 않는다.
