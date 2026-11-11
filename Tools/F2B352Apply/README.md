# F2 B352 승인 적용 Delta 검증

바울 대표의 2026-11-11 채팅 최종 승인 “어. 승인.”을 근거로 실행합니다. 제품 범위는 승인 commit23ea5e58의 Builder/FirstSnow/meta4파일과 F2 Scene1파일뿐입니다.

- 실행본: 저장소 `Temp/F2B352Apply20261111/QA/Client`. 원본 Editor에서는 실행하지 않습니다.
- 기존 성공한 B352 QA 복제본의 Assets/Packages/ProjectSettings/Library를 별도 복제합니다. UserData/UserSettings는 복사하지 않습니다. 과거 QA75Base와 meta는 제외하고 새 검사기만 Assets/QA75Apply에 복사합니다.
- Library 복제는 정확한 UnityLockfile/ArtifactDB-lock/SourceAssetDB-lock 파일만 제외합니다. PackageCache의 Search 등 정상 소스 폴더를 제외하지 않습니다. /R:0 /W:0, 필요 시 /MT:8로 완료를 확인한 뒤 Unity를 시작합니다.
- `F2ApplyBatch.Run`은 Unity Editor API로 복제 F2에 Root/Builder1개를 추가하고 기존11Sprite를 직렬화합니다. 기존 컴포넌트 JSON 불변을 확인한 후 복제 Scene을 작업 worktree에 저장합니다. 이후 비어 있는 검증 Scene에서 Play를 시작합니다.
- 런타임은 실제 SceneTransitionService로 마을→F2 입장합니다. Builder 설치·Build 수동 호출로 자동 Start 검증을 대신하지 않습니다. 중복 방지 검사에서만 이미 생성된 Build를 다시 호출합니다.
- 1280/960/1680 실제 GameView, 굽은 길 휠체어 왕복1회, 마을 복귀·F2 재입장·두 번째 퇴장 및 소유 Mesh/Material 해제를 검사합니다. 과거 전수 검증기는 호출하지 않습니다.
- 실행 명령: `Unity.exe -batchmode -projectPath <격리Client> -executeMethod F2ApplyBatch.Run -logFile <전용로그> -screen-width 1280 -screen-height 720`. PowerShell Start-Process -WindowStyle Hidden을 사용합니다. -nographics, 원본 Scene/Play/선택/포커스 조작은 금지합니다.
- Save/UserSettings는 격리 Audit 경로만 사용합니다. 개인 Save·원본 보호 manifest/백업은 Temp에 보존하며 commit/원격 보고에 포함하지 않습니다.
- 최종 제품5파일과 보고·검사자료를 분리해 commit하고, 모든 Delta PASS 및 사용자 변경 충돌0인 경우에만 로컬 main에 FF-only/no-autostash로 반영합니다. 원격 Push/Merge는 하지 않습니다.
