# Bear Run6 v4 승인분 제품 연결

승인 근거: 바울 대표의 최종 승인 및 [Issue #51](https://github.com/HappyPaul812/Project-Limitless/issues/51). 납품 Manifest의 제작 당시 `ART_REVIEW_PENDING` 표기는 이후 사용자 최종 승인을 대체하지 않습니다.

## 변경 범위

- `Resources/CompanionAssaultValidation/Bear_Run6_v4.png`에 승인 Strip을 바이트 그대로 추가합니다. 신규 `.meta`만 별도 GUID를 부여합니다.
- `BeastCompanionCatalog`의 `bear` 달리기 경로만 새 시트로 연결합니다. 셀 512×384, 6프레임, 12FPS, 표시 128×96, 이동 속도 570을 사용합니다.
- 기존 `bear` 저장 ID, 기존 `Bear_Run` 아이콘 64px/Frame00, KeyArt/Idle, Animator/Controller/Scene은 유지합니다. Presenter 변경은 규격 설명 주석뿐입니다.
- 기존 Wolf, 미승인 Fox, Village, Main22/F1/서리들쥐 및 전체 전투 회귀는 재검수·재반영하지 않습니다.

## 납품 파일

ZIP SHA256: `4a9fedd54d58999def05b92839a1f00bc1d001375746e572f5d88bcade44977a`

Strip SHA256: `47177f72a202039b4a806bf4607e25384cf811d245169235862760152151bcb3`

CRC·Manifest·SHA256SUMS 및 Strip/개별 6프레임 픽셀 동일성은 [납품 근거](Evidence/BearRunV4Apply20261022/Delivery.json)와 `ArchiveChecks.json`에 기록합니다.

## 검증 방식과 한계

원본 Unity는 열기·Refresh·Play·포커스 조작을 하지 않습니다. 별도 scratch 프로젝트에서 Unity 6000.6.5f1 Batch Play Mode로 실제 `BattleActionPresenter`를 실행합니다. 제품 파일은 worktree와 SHA 동일성을 확인합니다. scratch 패키지는 기존 격리 검증의 로컬 캐시와 manifest/lock을 사용하며 제품 Packages/ProjectSettings는 변경하지 않습니다.

실제 `PetFacilityPresenter.GetIcon`, 장착 서비스, 격리 슬롯 저장·불러오기·장착 해석을 확인합니다. 실제 공격 Presenter는 좌향·우향·높이 차이 3조건으로 실행하여 6프레임/05→00/12FPS/이동/flip/128px/공격·완료 콜백/일회성 객체 정리를 확인합니다. 40px는 같은 프레임을 별도 검증 Image에 축소 표시한 Camera 자료이며 제품 스킬 표시 크기나 기존 아이콘을 변경한 것이 아닙니다. 피해량 콜백은 검증용 stub이며 전체 전투 밸런스 검증은 아닙니다.

이미지·GIF는 실제 Camera/RenderTexture 캡처이며 Editor GameView UI 캡처가 아닙니다. GIF 시간은 83ms 설정(포맷 반올림 가능)이며 실제 엔진 프레임/시간은 `RuntimeQA.json`과 `RuntimeSummary.json`이 근거입니다. 원본 Editor 자연 Import 및 사용자 손플레이는 수행하지 않습니다.

## 결과

**GAME_APPLIED_LOCAL / 변경분 Unity PASS / GitHub MAIN_MERGED 아님.**

| 항목 | 결과 |
| --- | --- |
| ZIP | CRC PASS, Manifest 33/33, SHA256SUMS 34/34, Strip 6셀 픽셀 동일 |
| 제품 연결 | Bear만 승인 v4, 3072×384 원본 Import, 셀 512×384×6, 12FPS 설정 |
| 실제 Play | 154 PASS / 0 FAIL, 양방향·높이 차이 3회, 각 6프레임·05→00 루프·이동·flip 확인 |
| 관리/저장 | 실제 장착 저장→불러오기→곰 경로 해석 PASS, 기존 시설 아이콘 유지 |
| 공격 연결 | 각 조건 impact/notify/complete 1회, 배우/대상 좌표 복원, 임시 곰 제거 PASS |
| 원본 적용 | 대상 dirty 충돌 0, 기존 코드 2개 백업 후 복사, 신규 PNG/meta 2개 추가 |
| 보호 | 기존 파일 4400개 검사, 승인된 기존 코드 2개 외 차이 0, 사용자 미커밋 858개 SHA 동일, 원본 HEAD/index 동일 |

실측 프레임 간격은 약 84.3–91.1ms입니다. 제품 12FPS 설정 및 1/12초 대기는 유지되며 Batch 캡처/렌더 프레임 경계에 따른 지연을 기록했습니다. 정확한 벽시계 12.000FPS 보장을 선언하지 않습니다. 실제 프레임/시간/좌표는 [RuntimeSummary](Evidence/BearRunV4Apply20261022/RuntimeSummary.json)를 따릅니다.

초기 검증의 Wolf 아이콘 너비 가정 384px를 실제 Bear 원본 320×33에 맞춰 수정한 뒤 재실행했습니다. 초기 결과는 `Attempt1RuntimeQA.json`에 보존합니다. 최종 검증의 제품 런타임 오류 및 C# 컴파일 오류는 0입니다. 별도 **Unity Editor SearchDatabase 초기화 ArgumentOutOfRangeException 1건은 OPEN**이며 원문을 최종 RuntimeQA에 보존했습니다. 이를 Console 전체 무오류로 해석하지 않습니다. 범위 밖 Search/URP 수정은 하지 않았습니다.

[실제 Presenter 좌향 128px](Evidence/BearRunV4Apply20261022/ActualPresenter_Trial0_128px.gif), [우향 128px](Evidence/BearRunV4Apply20261022/ActualPresenter_Trial1_128px.gif), [40px Camera 표시](Evidence/BearRunV4Apply20261022/ActualCamera_Trial0_40px.gif), [전체 RT 캡처](Evidence/BearRunV4Apply20261022/ActualCamera_Trial0.png).

브랜치 `codex/bear-run6-v4-apply`, 제품 commit `5b89ac925402855251f01386009390780d74a6d4`. 원본은 `main` / HEAD `510c021b4159167e211be60ca90e1cb7328bafe9`를 유지하며, 승인 제품 4파일만 미스테이징 변경으로 반영했습니다. 원본 dirty 기록은 344→348로 승인 4경로만 늘었습니다. [보호·적용 증거](Evidence/BearRunV4Apply20261022/ProtectionAndApplication.json).

코드 원본 백업: `Temp/BearRunV4Apply20261022/OriginalBackup/`. 원본 전체 보호 스냅샷, 미커밋 해시, 전체 비공개 Unity 로그는 같은 Temp 아래에 로컬 보존했습니다. 공개용 검증 자료에는 라이선스/개인 정보가 있는 전체 로그 대신 완료 마커만 담았습니다.

GitHub Push/PR/병합은 요청되지 않았으므로 수행하지 않습니다. 로컬 원본 디스크 적용과 GitHub main 병합을 구분합니다.
