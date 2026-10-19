> **최종 승인·정식 적용 완료:** 바울 대표 승인 후 PR21 main병합 `f3196b6b4fc4dfaf8a901a6a29ea07254ae70325`, 원본 기능9파일 적용·SHA9/9일치. 아래 PR 검토대기/원본미적용 문구는 승인 전 검수 시점 기록입니다. 원본Editor Runtime은 포커스 보호로 미검증입니다.

# Main09 Field03 퀘스트 안내 UX 수정 및 실제 Unity QA

상태: **기능 PASS / 환경 PARTIAL / 전용 PR 검토 대기**. main 병합·원본 Unity 디스크 적용은 하지 않았습니다.

- 브랜치 `codex/main09-quest-clarity-20261019`, 기능 `932c3573a0ad4a4d94c0b92c4d0bb932662ae419`, 기준 main `643a16dddc74201638d9505a999040933091258d`.
- 기획 `docs/planning` Main09_Field03_Quest_Navigation_Onboarding_UX_Fix_20261019를 반영했습니다. Main18 음성 작업을 반복하지 않았습니다.
- [실제 Unity 개선 전·후 및 6단계 화면](Evidence/Main09Clarity20261019/index.html). 16:9=1280×720,4:3=960×720,21:9=1680×720. 원본 Slot01 플레이어 좌표를 읽어 1/6 별도 테스트 저장으로 재현했으며 원본 Save/Editor는 변경하지 않았습니다. 원본 첨부 이미지 자체는 이 대화에 없으므로 픽셀 단위 동일 재현을 주장하지 않습니다.

## 변경과 원인

1. 여섯 목표 문구에 대상·이동·조사/대화·E/F·게임패드A를 명시했습니다. Main09QuestGuidance가 로그/월드/토스트/생성기의 같은 문구를 제공합니다. 기존 Quest asset에는 여섯 Description만 수정했습니다.
2. 실제 조사 중심에 흑백 이중 링과 조사/대화 텍스트를 표시합니다. 점멸·PNG 재제작·Scene 수정0. 기존 1.45 위 앵커를 실제 중심으로 맞추고 안내 패널은 플레이어·폴·NPC·몬스터·EXP HUD를 피합니다.
3. 화면 밖 화살표 유지, 거리 정본은 플레이어 XY↔목표 XY입니다. 화면·로그·토스트는 1/6~6/6을 표시하고 4/6은 **폴에게 다시 말을 거세요**를 명시합니다. 가까이서는 조작 버튼과 조사/대화를 표시합니다.
4. 실제 입력 검증에서 첫 조사점과 기존 여관 NPC가 가까워 일반 NPC 입력이 먼저 소비되는 문제를 확인했습니다. 원래 1.6 반경 안의 현재 Main09 목표만 일반 NPC보다 먼저 받도록 고쳤습니다. 이 범위에서는 중복 NPC 프롬프트도 숨깁니다. 다른 퀘스트/반경 밖의 기존 우선순위는 유지합니다.
5. 4:3 로그에서는 긴 설명보다 현재 행동·단계·조작·보상을 먼저 표시해 패널 밖으로 보상이 잘리는 문제를 고쳤습니다. Quest 설명 데이터/대사는 유지합니다.

## QA 결과

**348 PASS / 0 FAIL**: 키보드 E/F/Enter·WASD 도보와 가상 게임패드 A·좌스틱 휠체어로 기존 Collider를 피하는 실제 물리 이동과 6단계 전체 완료. 실제 대화 Modal·취소·거리 이탈·목표 순서·Main09 중간2회 및 완료 Save/Bootstrap Continue·폴 합류·보상EXP35/탈렌트35 1회·중복 보상0. 6단계 실제 월드/로그 세 화면비 캡처.

**추가79 PASS / 0 FAIL**: 최종 패널 배치/줄수/화면 내 표시, 여섯 단계×세 화면비, 4:3 로그 보상, 추적 미지정 Main09 fallback, 서브퀘스트 추적 중 Main09 바닥 링 유지, Q→T로 Main09 복귀, 실제 Battle Scene 진입·복귀 시 퀘스트/마커 보존, 다른 퀘스트에서는 Main09 입력 우선권 해제, Missing Script0·신규 Runtime Console0.

Save/Scene 진입과 UI를 직접 실행했으며 OS 키보드·물리 게임패드/원본 Editor 조작은 하지 않았습니다. 입력은 Unity Input System에 가상 장치 이벤트로 전달했습니다. 환경은 설치 Editor6000.6.5f1, main ProjectVersion6000.5.7f1. 기존 Search/URP SpeedTree 오류는 기준선과 동일하여 환경 PARTIAL이며 신규 오류와 구분합니다. 검수 초기 잘못된 고정 도착점/해상도 전환 대기/백그라운드 입력 초기화 실패 기록은 Temp에 보존하고 최종 PASS 수치에는 포함하지 않았습니다.

## 보호 및 파일

- 기존 대사 소스 전체 동일, Objective ID/Type/Target/RequiredCount/보상/진행조건 동일, 네 조사·폴 위치 및 1.6 반경 동일. Field03 Scene/몬스터/NPC/Collider/이동 경로 Asset 변경0. Main08·Main18 WAV/meta/Catalog 변경0.
- 원본 보호 **4844파일 SHA 변경0**, UserData23 백업. 원본 main ff022e63 및 미커밋 파일 보존. 기존 문서 충돌5개로 local sync PARTIAL은 별도 유지합니다.
- 기능9파일: Main09_ReunionInSilence.asset, MainQuest09ContentEditor.cs, MainQuest09FieldFlow.cs, Main09QuestGuidance.cs/meta, InteractionSystem.cs, QuestNavigationPresenter.cs, QuestHudPresenter.cs, QuestLogPresenter.cs. 모든 변경은 `_Project` 범위입니다.
- 다음: PR의 실제 개선 화면을 검토한 뒤 main 반영. Field05는 바울 대표가 별도로 최종 미술 승인했으므로 다음 순서에서 기존 B42 검증/커밋을 재사용해 통합합니다.
