# Issue #112 · 승인 눈보라 정령 Idle8 v2 기준 자산 반영

2026-11-19 · 바울 대표 최종 승인. 게임용 기준 자산을 `Assets/_Project/Art/Monsters/BlizzardWisp/`에 반영했습니다. 승인 ZIP의 원본 PNG SHA256 `ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a`를 보존했습니다. KeyPose 및 v1 ZIP 보관본은 기존 SHA와 일치하며 해당 원본은 수정하지 않았습니다. [기존 기술 검수](Issue112_BlizzardWisp_Idle8_V2_Review_20261119.md) · [Issue #112](https://github.com/HappyPaul812/Project-Limitless/issues/112)

- Unity 6000.6.5f1 격리 Editor에서 4096×512 RGBA를 512×512 Sprite 8장으로 가져왔습니다. Idle 클립은 120ms씩 8장, 0.96초 반복입니다. 시각용 프리팹의 SpriteRenderer와 Animator, 기본 `Idle_Right` 상태 및 v2 클립 연결을 확인했습니다. 프레임별 샘플링과 8→1쪽 반복 경계 검사 통과. 512 PPU와 중앙 피벗은 기존 512px 몬스터 체계의 기준값이며 실제 F4/F5 배치 시 부유 위치·월드 크기는 검증이 필요합니다.
- 현재 정식 눈보라 정령 MonsterDefinition·조우·F4/F5 Scene 연결 또는 v1 사용 참조를 찾지 못했습니다. 시각 자산·프리팹만 등록했습니다. 후속은 정식 몬스터 등록 시 승인 v2를 연결하고 해당 Scene 및 전투에서 검증하는 것입니다. 전투 기획/수치, 다른 몬스터, 기존 Scene 및 Save 변경은 없었습니다.
- 로컬 작업 브랜치 `codex/issue112-wisp-idle8-v2-review`, 제품 커밋 `32297d6ad37a1be7f31f94e98b57dcfb91a5886e`. 원격 Push 미실행.
