# Issue #126 · 빙각 수호체 Idle4 Unity Play 검수 — 2026-11-23

**판정: GIF와 동일하게 체감됨 — 프레임당 260ms 설정 기준. Idle4 최종 승인 가능.** 기존 8fps를 그대로 쓰면 0.50초로 빨라져 **일부 차이 있음**입니다. 아래 설정을 제품 적용 계약으로 함께 유지해야 합니다. Idle4 최종 승인 확인 전 Attack/Hit/Defeat 작업은 보류입니다.

## 실제 재생과 증거

- Unity **6000.6.5f1 (3ff58d469c8a)** 숨김 배치 Play Mode. 전달 ZIP의 GIF와, 기존 파일 QA에서 시트와 픽셀 일치가 확인된 4개 셀을 사용했습니다. CRC·SHA·KeyPose·파일 QA 반복 0. [기존 Idle4 납품 및 260ms 계약](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7088874033)
- SpriteRenderer는 실제 Animator의 반복 Sprite 키로, 전투 UI는 제품 `MonsterSpriteSheetAnimation`과 `MonsterDefinition`을 복사해 실제 uGUI Image의 136×112 슬롯·비율 유지·Point·중앙 Pivot으로 재생했습니다. 별도 40px 슬롯도 동일 프레임을 표시했습니다. 캡처는 Play Mode의 Camera RenderTexture입니다.
- [GIF와 실제 Unity 캡처 비교](Evidence/Issue126_Idle4_Play_20261123/Unity_vs_Delivered_260ms.gif) · [재생 화면 캡처](Evidence/Issue126_Idle4_Play_20261123/Unity_Play_Battle_Capture.png) · [측정 요약](Evidence/Issue126_Idle4_Play_20261123/comparison.txt) · [시간 추적](Evidence/Issue126_Idle4_Play_20261123/runtime.csv). 비교 GIF는 각 Unity 렌더 캡처를 260ms로 배치한 것이며, 실제 재생 시간은 별도의 시간 추적으로 검증했습니다.

| 경로 | 안정 구간 프레임 순서 | 중앙 유지시간 / 전체 주기 |
|---|---|---|
| 실제 Animator | 0→1→2→3→0 반복 | 259.93ms / 1,039.72ms |
| 실제 전투 Image, 260ms 설정 | 동일 반복 | 260.02ms / 1,040.00ms |
| 실제 전투 Image, 기존 8fps | 동일 프레임, 빠른 반복 | 124.96ms / 499.75~500.24ms |

전투 표시에서 느린 하중 이동과 어깨·주먹 반동이 유지됩니다. 렌더된 112px 이미지 하단 18행은 4/4 동일했고, 픽셀 실루엣 중심 이동 폭은 가로 0.009px·세로 0.347px로 뚜렷한 중심축 튐이 없었습니다. 112px와 40px에서 각각 4개 프레임이 구분됩니다. 40px에서는 내부 얼음 세부보다 큰 양쪽 주먹·어깨 외곽이 더 잘 읽힙니다.

## 적용 조건 및 한계

- 필드 Animator는 0/0.26/0.52/0.78초 Sprite 키와 **1.04초 반복**을 사용합니다. 격리에서 100Hz PPtr 클립 끝 키를 1.03초에 마지막 Sprite로 두어 Unity의 끝 샘플을 포함한 길이 1.04초를 얻었습니다. 전투 UI는 **Idle에 260ms/프레임**을 유지합니다. 이번 격리 정의는 약 3.846154fps로 재현했습니다. 제품의 공용 FPS가 다른 동작에도 사용되므로, 해당 모션이 생기면 Idle 전용 시간으로 분리하거나 모션별 승인 속도를 보존하는지 확인해야 합니다. 원본 그림 수정은 필요하지 않습니다.
- 안정 구간 2.34초 이후로 수치를 산출했습니다. 첫 구동·첫 캡처 사이의 초기 지연을 제외했으며, 시작시 지연 자체는 이 테스트에서 보장하지 않습니다. 첫 임시 클립은 Unity 마지막 샘플로 1.05초가 돼 보정했습니다. 두 번째 배치의 Unity Editor Search 초기화 예외 1건은 테스트 코드 외부에서 발생했고 캡처·재생 완료는 확인했습니다.
- 제품 F5 Scene, 실제 조우 전열 배치·정렬, 통합 게임 GameView는 **NOT_VERIFIED**. 적용 및 다른 모션 착수는 각각 승인에 따릅니다. 승인 자산·Unity 버전·#85 HOLD·Save/UserData 보존, 본편 적용·main 병합·Push 0.
