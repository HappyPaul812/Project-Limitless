# Issue #124 승인 기본 취소 버튼 격리 적용 · 2026-11-24

- **제품 PASS** · 기존 작업 브랜치 `codex/issue124-loot2-attack-icon-20261124`의 최신 HEAD `23f98986e182cab4e64910755386fc4b73bb83ec` 위에 제품 `f47a0908db0eaa7ef2c17e751b3556f783a30fc5`를 커밋했습니다. `command.cancel`은 `BattleItemIcons/CancelButton_Original_128_RGBA`에서 Sprite를 로드합니다. 승인 PNG와 고유 메타 추가, 카탈로그 경로 한 줄 수정. [최종 승인과 적용 지시](https://github.com/HappyPaul812/Project-Limitless/issues/124#issuecomment-7121662039)
- 다운로드 ZIP SHA256 `b879e2ce1768511d0f06ab07063dbd5ea9eead3dd4fcc7165c06baa6cf6e8ef0`, PNG SHA256 `3eeb5e4090ec330c60cf45ee65ebf02b7bc008b1557e0a77a1a20c5b9ba47533` 일치, ZIP CRC 통과. 신규 GUID `308d007e795c47158c5c185abeedf79d`의 유일성 확인.
- **Unity 6000.6.5f1 호환 QA PASS** · 실제 Battle의 기존 카탈로그 경로로 제품 Sprite가 로드됐습니다. 아이템 메뉴에서 40px 배율 표시, 취소 버튼 클릭 후 기본 명령 복귀와 아이템 버튼 포커스 복원을 확인했습니다. 배치 표적 5 PASS, 종료 0. 기존 PASS 재실행 없음.
- 증거 `Temp/i124-cancel-apply/unity.log` 892–1461행, `Product_Cancel_UI_Crop.png` SHA256 `cf2075cc6cc847fe7c0cc1c2e849f7d5aef312db6a84915b62e472dcbf13f36d`. 합성 Save·Settings 경로 사용. 제품 클린 Import와 Save 왕복은 미실행입니다.
- 제품 변경 목록 3파일을 확인했습니다. 공격·방어·스킬·도망·전리품, 기존 Kenney 원본, 전투 로직, Unity 버전, #85 후보 및 원본 main 보호. LOCAL_COMMITTED / REMOTE_NOT_PUSHED.
