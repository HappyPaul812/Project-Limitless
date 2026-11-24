# Issue #131 · #85·#124·#130 안전 통합

2026-11-24 · `codex/integration-20261124-main25-safe` · Unity 6000.6.5f1

[대표 승인과 원본 반영 조건](https://github.com/HappyPaul812/Project-Limitless/issues/131)

- 기준 local main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`. 통합 제품 `5399c89b2be41c2c7ea0a0aedb36b16eb70f3120`. #85 제품 `6fc46b429159026adf99da686b0b87e909238e19`를 별도 worktree에 적용한 뒤 #124를 선행 이력으로 가진 #130 최신 HEAD `2abf6dfe4002953c414f4ae3e782712df5b1583e`를 병합했습니다.
- **통합 후보 PASS** · 승인 변경 159경로. #85 패키지/GUID 7경로가 승인 커밋과 바이트 일치합니다. #124 기본 전투 버튼과 잡템 4종, Main24/25 제품 이력 전체 및 승인 음성 19개를 포함합니다. 카탈로그 ID 중복 0.
- **원본 FF HOLD** · 원본 tracked 미커밋 303경로, untracked 550파일, Save/UserData 보호 대상 23파일을 기준으로 대조했습니다. 기존 변경과 후보가 겹치는 8경로는 패키지 manifest/lock, 치유사 GUID 메타 5개, EditorBuildSettings입니다. 메타 5개는 줄바꿈 정규화 후 후보 내용과 일치하지만 세 설정 파일은 후보와 내용이 다릅니다. #131의 겹침 0건 조건을 충족하지 않아 원본 local main을 동기화하지 않았습니다.
- **클린 Import·C# 컴파일 및 게임 스모크 PASS / Editor Search HOLD** · 별도 worktree에 Library가 없는 상태로 시작해 후보 manifest/lock과 GUID를 그대로 사용했습니다. 최초 Asset Pipeline Refresh 400.297초, C# 컴파일 오류 0. 직후 `UnityEditor.Search.SearchInit.IndexationOnStartup`에서 기존 `ArgumentOutOfRangeException` 재현. 같은 실행에서 실제 Bootstrap 합성 슬롯 → Main24/25 완료와 F3 북진·Continue까지 진행했습니다. 대표 도망/취소·비늘 Sprite, Main24/25 음성 Clip 각 1개 및 F3 BGM 로드 확인. 표적 137 PASS, 실패 0, 종료 0. 실제 후보 게임 실행 가능. Unity가 격리 작업트리에 생성한 직렬화 변경은 제품 커밋에서 제외했습니다.
- 원본 HEAD·status 및 사용자 미커밋·untracked·Save/UserData 파일 해시를 전후 대조해 동일함을 확인했습니다. local main `65cceb2d6c3283d18abec6e27d8a6f1296411f17`, origin/main 추적 ref `bfd281545632877325f5d21e04cefb924042c465`, 원격 복구 추적 ref `65cceb2d6c3283d18abec6e27d8a6f1296411f17`을 보존했습니다. 원본 Discord와 Editor는 건드리지 않았습니다.
- **다음 작업** · 3개 설정 파일의 사용자 변경과 승인 후보를 별도 대조로 정리하고 원본 보호 조건을 다시 확인합니다. Search가 재현된 경우 그 예외의 해결도 원본 반영 전 필요합니다.

로그·로컬 증거 `Temp/i131-evidence/`. 기존 223/27 PASS 전수 재검사, Main26 이후 개발, 원격 Push를 수행하지 않았습니다.
