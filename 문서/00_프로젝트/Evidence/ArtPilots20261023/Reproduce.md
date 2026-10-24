# 격리 검수 재현

기반 HEAD3f30d135. 후보 PNG/meta와 Editor/ArtPilotsDeltaQA.cs만 신규다. 제품 Scene 저장은 하지 않는다.

1. worktree 경로는 `F:/study/codex/Project-Limitless/Temp/ArtPilots20261023/Worktree`로 가드되어 있다. 원본/기존 worktree에서 실행하면 거부한다.
2. 로컬 Unity6000.6.5f1 사용. 실행 전에 격리 Package/ProjectVersion만 해당 버전에 맞춘다. 당시 검증용 환경 사본은 `Temp/Main23FirstPlayable20261023/Runtime/Environment`에 있다. 이 임시 환경 변경은 커밋하지 않는다.
3. fixture는 기존 QA가 만든 `Temp/Main23FirstPlayable20261023/Runtime/Checkpoint_Arrival.json`. 사용자 Save는 쓰지 않는다.
4. Unity.exe `-batchmode -projectPath <격리 Unity/Client> -executeMethod ProjectLimitless.EditorTools.ArtPilotsDeltaQA.Start -logFile <전용 로그>`를 `Start-Process -WindowStyle Hidden`으로 시작한다. `-quit`은 넣지 않는다. 검수기가 Play Mode 후 종료한다.
5. 보정 화면만 필요하면 `-issue64-capture-fix`를 추가한다. 기존148/136 테스트 호출은 없다.
6. 출력 `Temp/ArtPilots20261023/Runtime`, 보정은 하위CaptureFix. 원본ZIP/Input과 소스 PNG는 byte-identical 유지한다.
7. Editor 종료 후 별도 worktree에서만 Unity 자동환경 변환을 되돌린다. 원본·기존worktree에는 restore/reset/clean을 실행하지 않는다.

Checks 일부는 보정 촬영 때 재확인된다. 이미지Before/After 비교판은 실제 Unity 캡처를 배율 변경 없이 세로로 배열하고 제목만 추가한 것이다. OFFLINE_ 파일은 원본PNG를 직접 배열/축소한 파일 검사자료다.
