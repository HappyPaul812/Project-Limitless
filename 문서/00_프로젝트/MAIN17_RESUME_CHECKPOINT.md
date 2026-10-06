# Main17 RESUME CHECKPOINT

- 완료 Phase: 1 설계, 2 Quest Data.
- 현재 진행 Phase: 3 시작 전.
- 마지막 완료 파일: Main17ContentBuilder.cs/.meta, Main17_RedRift.asset/.meta.
- 미완성: Phase3~9 Field/Actor/Dialogue/Battle/Save/BGM/Runtime 전체.
- 다음 정확한 시작점: Main17ContentBuilder에 BuildField 추가. 기존 Field07 Scene을 새 Field08로 복사한 뒤 Additive로 편집/저장/닫기. Field07 원본은 건드리지 않고 새 Flow의 Awake에서 Bounds 서쪽 Opening 준비. 현재 EditorBuildSettings는 기존 사용자 변경 상태이므로 이번 Scene 추가분만 선택 stage해야 함.
- Compile: Unity6000.5.7f1 새 C# 컴파일 성공, Console Error0. 새 Quest를 실제 Resources 로드, ID/12목표/Main16 prerequisite/잘못된 순서 거절/12목표 순차 완료 확인. 격리 QuestRuntimeState만 검사해 사용자 Session/Save 수정0.
- Git: Phase1 cca5a1f, 시작 d1b7cb8. 기존103상태 보존, Phase2 관련6파일만 commit 예정. Push0.
- 충돌: 세린 Main15 정식 해금·Main16 현장 조사 유지. Chapter2 Battle BGM null/TBD 유지. LOCAL 우선 적용/보고 완료.
- 음악: F:/Downloads/bgm/Beneath_The_Cracked_Earth.mp3 실제 존재. 아직 가져오지 않음.
- QA: Phase2 데이터 단위 PASS17/FAIL0 (ID·목표수·선행조건·순서거절·12전환·완료), 실제 게임 진행 Runtime PASS0. TTS0.
