# Issue #126 · 빙각 수호체 Hit4 격리 Unity Delta — 2026-11-23

**격리 재생 PASS · 피격 표시 PASS · Idle 복귀 PASS.** [기존 파일·기획 미술 검수](https://github.com/HappyPaul812/Project-Limitless/issues/126#issuecomment-7091921272)는 재사용했습니다. 최신 `LIMITLESS_Issue126_RimeSentinel_Hit4_20261123.zip`의 Hit4 시트만 신규 검증했습니다.

- Unity **6000.6.5f1 (3ff58d469c8a)** 숨김 Play Mode. 실제 Animator의 **비반복 610ms** 클립에 100/160/130/220ms로 Sprite를 배치했습니다. 136×112 uGUI 슬롯으로 112px 전투 크기를 재현하고 40px 참조도 렌더했습니다.
- 120Hz 캡처에서 Hit0→Hit1→Hit2→Hit3→Idle0→Idle1이 순서대로 표시됐습니다. 명목 610ms에서 전이하도록 설정했으며, 추적에서 최초 Hit0 8.3ms, 최초 Idle0 625.0ms였습니다. 캡처 시점은 프레임 단위이며 승인 Idle4의 260ms/프레임 클립을 그대로 재사용했습니다.
- Hit0의 41.7ms에 시험 피격 플래시가 1회 시작되어 15프레임 동안 표시됐고, 효과가 꺼진 상태로 Idle에 복귀했습니다. 112px에서 충격 후 어깨와 상체 반동이 구분됐으며, 반동과 복귀의 최하단 실루엣은 같은 행에 있었습니다. 40px에서는 플래시와 외곽은 보이지만 상체 세부 반동이 약해 **PARTIAL**입니다.
- [충격·반동·복귀 실제 Unity 캡처](Evidence/Issue126_Hit4_20261123/Hit4_Unity_Comparison.png) · [재생·이벤트 추적](Evidence/Issue126_Hit4_20261123/hit4-check.txt)

피격 플래시는 격리용 UI 색상 효과입니다. 실제 제품 피해·VFX·전열/F5 조우 연동은 미검증입니다. 격리 Editor Search 초기화 예외 1건은 재생·캡처를 막지 않았으며 배치는 코드 0으로 종료했습니다. KeyPose·Idle4·Attack4 v2와 기존 증거를 보존했습니다. Hit4 대표 최종 미술 승인 및 본편 적용은 별도입니다. #85 HOLD, 원본 main·Save/UserData·Discord 보호, 본편 적용·병합·Push 0.

로컬 브랜치 `codex/issue126-hit4-delta-20261123`. 실제 SHA는 Issue #126에 기록합니다.
