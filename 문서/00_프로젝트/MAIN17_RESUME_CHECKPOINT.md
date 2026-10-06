# Main17 RESUME CHECKPOINT

- 완료 Phase: 1 설계, 2 Quest Data, 3 Field08/Transition.
- 현재 진행 Phase: 4 시작 전.
- 마지막 완료 파일: Main17ContentBuilder.cs/.meta, Main17_RedRift.asset/.meta.
- 미완성: Phase4~9 Actor/Dialogue/Battle/Path/Save/BGM/통합 QA.
- 다음 정확한 시작점: Chapter2Main17Flow에 Start/Site 설치 및 Main17Site 수동 조사·완료 콜백 추가. Main17DialogueCatalog 새 공통 본문 구현. Available일 때 Field07 서쪽 접근/Field08 진입에서만 TryStart. 실제 Story Battle은 Phase5, Path 분기는 Phase6.
- Compile: Unity6000.5.7f1 새 C# 컴파일 성공, Console Error0. 새 Quest를 실제 Resources 로드, ID/12목표/Main16 prerequisite/잘못된 순서 거절/12목표 순차 완료 확인. 격리 QuestRuntimeState만 검사해 사용자 Session/Save 수정0.
- Git: Phase1 cca5a1f, Phase2 8cfecc6, 시작 d1b7cb8. 기존103상태 보존, Phase3 관련파일만 commit 예정. Push0.
- 충돌: 세린 Main15 정식 해금·Main16 현장 조사 유지. Chapter2 Battle BGM null/TBD 유지. LOCAL 우선 적용/보고 완료.
- 음악: F:/Downloads/bgm/Beneath_The_Cracked_Earth.mp3 실제 존재. 아직 가져오지 않음.
- QA: Phase2 데이터 단위 PASS17/FAIL0, Phase3 백그라운드 PlayMode16체크 PASS16/FAIL0 (ID·목표수·선행조건·순서거절·12전환·완료), 실제 게임 진행 Runtime PASS0. TTS0.

- Phase3 결과: 실제 Main16 이전 잠금/완료 개방·07→08→07·출구2/1·21×15 Bounds·Camera·서쪽심부닫힘·Spawn±7·즉시역전환없음 확인. Console Error0/현재Warning0, 이전CS0618 16 이력은 유지. Bootstrap EditMode 복귀·포커스전환0·사용자Save변경0. BuildSettings 새08항목만 stage, 기존 app-ui항목은 보존.
