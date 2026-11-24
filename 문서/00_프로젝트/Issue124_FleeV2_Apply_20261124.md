# Issue #124 도망 버튼 v2 격리 적용 · 2026-11-24

- **제품 적용 PASS** · `codex/issue124-loot2-attack-icon-20261124`의 기존 HEAD `8fe6e0eab4b16e2f43c37d40969cdb051e285d8c` 위에 제품 `6a6fc331a5b9672834d823c30965eb9a9057f9d9`를 커밋했습니다. `command.flee`가 `BattleItemIcons/FleeButton_v2_128_RGBA`를 로드합니다. 승인 PNG·고유 메타·카탈로그 한 줄, 총 3파일입니다. [최종 승인과 적용 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121163563)
- 로컬 다운로드 실물 확인. ZIP SHA256 `931274626db9230cdaeb63e191100c68a43af14e8b50969c0ed26608ecb74fed` 및 PNG SHA256 `58d7f66283031737f65069df6fb281a818d8c32e6ae7a6230b2f5e53783f5d73` 승인값 일치, ZIP CRC 통과. 새 GUID `f2186047e08e4f51b3347ed7f53f82cb`가 프로젝트 메타에서 유일합니다.
- **Unity PASS** · 기존 Unity 6000.6.5f1 호환 QA 복제본의 Battle에서 제품 카탈로그가 불러온 Sprite와 버튼 Image가 일치했고 40px 배율에서 표시됐습니다. 기존 공격·방어·스킬 표시도 확인했습니다. 실물 던전 B2 보스 스폰으로 전투를 시작해 플레이어 차례에 도망 버튼을 눌렀습니다. 기존 버튼은 클릭 가능하며 “보스전에서는 도망칠 수 없습니다.”가 표시됐고 전투가 유지됐습니다. 배치 7 PASS, 종료 0. 일반전은 기존 PASS를 유지하고 이번 배치에서는 생략했습니다.
- 증거는 로컬 `Temp/i124-flee-apply/unity.log` 754–862행 및 `Boss_Product_Flee_UI_Crop.png` SHA256 `48b624d4a9bfb9c50849e76abcda82f38ad432b13be76977b66e12c27430a26b`입니다. QA에는 합성 Save·Settings 경로를 사용했습니다. 제품 클린 Import와 Save 왕복은 미실행입니다.
- Git 변경 목록으로 기존 공격·방어·스킬·전리품, Kenney 원본·라이선스, 전투 로직 보호를 확인했습니다. 원본 main과 #85 후보 브랜치 SHA는 작업 전후 동일합니다. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
