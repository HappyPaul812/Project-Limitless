# Issue65 재현

기반91510554, 검수기7076fade. C#의 Start 진입은 이 worktree에서 Review65만 선택합니다. 기존 Run/CaptureFix/Comparison은 호출하지 않습니다.

- 경로: F:/study/codex/Project-Limitless/Temp/ArtReview65/Worktree/Unity/Client
- Unity6000.6.5f1 -batchmode -projectPath <위 경로> -executeMethod ProjectLimitless.EditorTools.ArtPilotsDeltaQA.Start -logFile <별도 로그>
- Start-Process -WindowStyle Hidden으로 실행합니다. -quit 없이 검수기가 Play후 종료합니다.
- 기존 합성 Checkpoint_Arrival.json만 사용합니다. UserSettings BeginAudit와 AuditSaveDirectory가 Temp 하위로 격리됩니다.
- 비교 프레임만: -issue65-final-frames. 기존33 이동 검사는 실행하지 않습니다.
- 기존환경과의 버전차이를 위해 검수용 Packages/ProjectVersion만6000.6.5에 맞춘 후 실행하고 종료 뒤 격리worktree내 자동변환만 되돌립니다. 원본/이전worktree는 수정하지 않습니다.
- 산출물 B_New65_checks.txt33행 / B_FinalFrames_checks.txt8행. 21 PNG 중17개 고정카메라비교프레임을 우선하고 나머지4개는 실제 이동/통과 화면입니다.
- BoardSourceMap은 크롭좌표/원본SHA를, ImageIndex는 최종PNG28개의 SHA를 기록합니다. Board1~3은 원본크롭이며 Overview만 축소했습니다.
