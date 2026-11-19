# Issue #120 · 불씨망령 승인 Idle4·Attack4 적용

2026-11-19 · 바울 대표 승인 통합 시트를 등록된 불씨망령의 기존 PNG 경로에 반영했습니다. 제품 변경은 PNG 한 파일입니다. 기존 .meta·GUID·16 Sprite 파일 ID와 MonsterDefinition, Field07 스폰 및 전투 전환을 보존했습니다.

- 두 ZIP의 기존 SHA, CRC, Manifest, SHA256SUMS를 대조했습니다. Idle 단독 승인 시트는 원본의 셀 0~3만 변경, 통합 시트는 그 위에 4~7만 변경했습니다. 피격 8~11 및 전투불능 12~15의 모든 RGBA 픽셀이 현행 원본과 일치합니다. 통합 시트 SHA256은 `d25afb4cc932fd333f67854a28821bf239afa43c36ed937f479793e03285197a`입니다.
- Unity 6000.6.5f1, 기존 격리 프로젝트와 기존 승인 스폰/아티팩트 재사용. 제품 전투 컨트롤러와 애니메이션 코드에 변경 없이 Field07 등록 조우 → Battle → 실제 방어·공격 버튼 경로로 검사했습니다. 16개 Sprite를 가져왔고 Battle Image에서 Idle·Attack·피격·전투불능을 각 4프레임 모두 실측했습니다. Attack 뒤 Idle 복귀와 전투불능 마지막 셀 유지, 14탈렌트 1회 지급, Field07 복귀 및 합성 Save를 확인했습니다. **30 PASS / 0 FAIL, 게임 오류 0, 배치 종료 코드 0**.
- 전투 추적에서 마지막 전투불능 셀 직전 Idle 첫 셀이 한 렌더 프레임 관찰되었습니다. 현재 공격 종료 경로의 Sprite 복원 호출과 일치하며, 이번에는 피격·전투불능 픽셀 및 기존 전투 코드를 유지했습니다. 마지막 전투불능 셀은 이후 유지됐습니다.
- 합성 Save/Settings만 사용했습니다. 개인 Save 접근, 기존 전투 데이터·다른 몬스터·씬·버전 변경은 없었습니다. Unity Search 관련 기존 환경 예외는 게임 오류 집계와 별도로 기록합니다. [실제 프레임·검수 증거](Evidence/Issue120EmberWraith/) · [Issue #120](https://github.com/HappyPaul812/Project-Limitless/issues/120)

로컬 브랜치 `codex/issue120-ember-wraith-idle-attack` · 제품 커밋 `37248c3b4345c7152db2ad2afad2aaf743294369` · 원격 Push 미실행.
