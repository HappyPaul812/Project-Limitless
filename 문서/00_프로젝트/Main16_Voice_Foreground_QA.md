# Main16 Voice와 Appearance Foreground 검증 — 2026-10-03

## 작업 계약

사용자가 이번 작업에 한해 Unity Foreground/Game View/Play Mode와 포커스 전환을 승인했다. 범위는 Appearance 50종의 시각 검수와 Main16 음성·분기 검수다. 문서화 → Voice 구현 → Foreground 검증 → 필요한 최소 수정 → 재검증 순서로 진행한다.

LOCAL Main16DialogueCatalog와 `Limitless_TTS_Output_Main16/dialogue_manifest_main16.csv`를 대조한다. 일치한 segment만 기존 StoryVoiceCatalog에 추가하며 이전 201개와 Intro18개를 보존한다. Player와 현장 관찰은 무음이다. 원본 WAV를 바이트 그대로 복사하고 기존 Voice Mixer/Next/정리 흐름을 사용한다. Field07 BGM은 TBD를 유지한다.

Appearance는 현재 ready35/보류15에서 시작한다. 50개 Preview/네 방향과 보류15의 전체 Walk 프레임을 실제 Game View에서 관찰한다. 특히 Mobility10을 개별 판정한다. 정적 경계 접촉만으로 차단하지 않으며 실제 그림·방향·Alpha·캐릭터 변경 문제만 구체적인 ID/0-based 프레임 번호와 함께 남긴다. 사용자 Sprite, 그림, TTS를 수정하거나 재생성하지 않는다.

Character Creation의 성별/테마/목록/이전·다음/확인/키보드, Hearing 외형+Vision 실제 길의 독립성, 대표 World 이동, 실제 Battle1회, 격리 Save→Continue를 확인한다. Main16은 Hearing/Default 실제 분기와 화자5명, Next/연속 Next/무음 fallback/종료/Scene·전투 전후 정리/Voice 음량/Mute를 확인한다.

청취 결과는 실제 오디오를 인지한 경우에만 기록한다. 오디오 출력·Clip 대응 확인과 발음/감정/호흡을 듣는 검수는 구분한다. 지원되지 않는 청취 항목을 PASS 또는 문제0으로 표시하지 않는다.

## 결과

직전 중단 지점은 음성28개 복사·Catalog 연결 완료, 외형50종 실제 화면 판정 완료, 판정 데이터 반영·Main16 runtime 음성 검증·최종 UI 재검증·문서·commit 미완료였다. Continuation에서는 사용자에게 남은 범위 Foreground 승인을 다시 받아 진행했다. ZIP/Slice/기존 Voice Import/Quest 구현/음악 배치를 반복하지 않았다.

### Main16 적용·Runtime 결과

Manifest28행 / Source WAV33개(segment28·합본5) / Unity Audio28개 / Mapping28개 / Story Catalog229개(기존201+28). LOCAL 고유 대사44개 중 Player 무음9·관찰 무음7. Unmapped·Text mismatch·Speaker mismatch·Missing Audio·Duplicate ID 모두0. 원본 WAV 바이트 일치, 기존201 Catalog prefix 동일, Intro를 변경하거나 재검증하지 않았다. Registry는 TTS 정책의 후속 Main16 항목에 제공 모델·화자·Voice ID를 기록했다.

실제 Scene/NPC/조사/전투 버튼을 통한 Hearing·Default 두 경로 검증 **428 PASS / 0 FAIL**. Hearing은 Player 우선, Default는 Serin 우선이며 해당 분기의 Stable ID 배열만 실행됐다. 조사 Objective와 Arbel 귀환·완료 대화 및 단계별 격리 Save/Continue를 통과했다. 대표 Vision Fighter Female의 Battle Left Idle frame4 참조와 진입 Voice cleanup을 확인했다. 전투 화면 증거는 로컬 `Unity/Client/Temp/ForegroundReviewQA/Main16_Battle.png`다.

44개 실제 DialogueLine과28 Clip의 연속 Next / 마지막 종료 / 잘못된 Speaker fallback 통과. 음량100/40 출력 peak0.5678347/0.2283038, Voice0은 설정 직후 이전 버퍼 잔류가 있어 안정화 후 별도 측정해0, Mute0, retry Clip 출력0.01413437. Clip 정리 true. 자연 진행에서 retry 이외27개를 재생했고 retry는 추가 기술 재생으로 확인했다. 출력 측정은 실제 청취가 아니다. [보존된 Runtime 결과](Main16_Voice_Runtime_Results.txt)를 따른다.

### 외형 기존 관찰 결과 보존

총50 / PASS21 / Blocked29 / 시트 육안 미판정0. [개별 파일·Frame·판정](../11_UI/Player_Appearance_Foreground_QA.md)을 따른다. Mobility10은 전부 보류(캐릭터 교체1·Alpha2·방향2·Art5). Vision Alpha 경고10은 PASS5·Art 보류5. 기존 Ready에서 추가 보류는 Hearing10·Intellectual Fighter/Mage4다. 원본 PNG50개 SHA256 동일,800 Sprite와 Stable ID는 유지한다. 선택 정책만 반영하며 기존 준비 Clip 참조를 재사용했다.

직전 Character Creation35 Preview와 성별·선택 표시·이름→길→직업 화면 검수, Hearing 외형+Vision 실제 Path 조합과 Story Hearing=false, 대표4종 World 방향/Idle/Walk32상태, Vision Fighter Female Save→Continue 복원은 재사용했다. World 이동은 실제 이동 컴포넌트를 호출한 자동 검증이며 물리 키 입력 검증으로 표현하지 않는다. 최종21종 정책은 **803 PASS / 0 FAIL**로 성별·전체/5테마·Previous/Next·기본 fallback·큰 Preview·선택 표시·확정·EventSystem/Tab 순서·실제 생성 흐름·Save/Continue를 검증했다. [결과](../11_UI/Player_Appearance_Selection_Runtime_Results.txt)를 따른다.

800×600 실제 Game View에서 상단 단계 패널 간격이 좁아 겹치는 문제를 발견했다. 비율 Anchor 대신 중앙 기준 동일 Canvas 간격으로만 수정해 재검증했다. 단계4개 경계 겹침0, Preview·선택·이름·다음·Navigation 안내가 화면 안에 있다. [최종800×600 화면](../11_UI/Player_Appearance_800x600.png). 실제 OS Tab을 전달했지만 Unity 선택 이동을 확인하지 못했으므로 OS 입력 PASS로 처리하지 않는다.

### 청취·남은 한계

실제 발음·감정 청취 완료 **0/28개**. 지원되는 도구가 음성을 인지하지 못하므로 PRONUNCIATION/EMOTION/BREATH/속도/음량 취향/Segment boundary/Text Audio 일치의 청취 판정은 모두 미검증이다. 문제0 또는 재생성 필요0으로 확정하지 않는다. 재생성 필요 수는 미정이며 TTS를 생성·변경하지 않았다. 실제 OS 키보드/Gamepad 물리 입력과800×600보다 작은 화면은 별도 확인 대상이다.
