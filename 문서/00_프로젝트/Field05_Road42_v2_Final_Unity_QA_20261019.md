# Field05 v2 Road4 / B42 실제 Unity 최종 후보 검수

상태: **TECHNICAL PASS / KNOWN_ENV_PARTIAL / FINAL_ART_APPROVAL_PENDING**. 바울 대표 최종 미술 승인 전입니다. 원본 Field05 디스크 반영·Push·PR·main 병합은 수행하지 않았습니다.

- 브랜치 `codex/field05-eastwest-road42-v2-20261019`, 기능 커밋 `ebb363567b3932cce8af12e315dda03362c8fd19`. 기준 main `6f988dad77316347be69ac3b0979237771c09ed9`.
- 실제 ZIP: `F:/Downloads/Limitless_Field05_Renewal_Art_v1.zip`, SHA `a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`; `F:/Downloads/Limitless_Field05_EastWest_Road_v2.zip`, SHA `3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3`. CRC·실사용14 PNG Manifest SHA PASS. v2 Road4 RGBA/128×128/PPU128/중앙Pivot/Point/mipmapOff/Z0/FlipFalse. Road 반복·Upper/Lower·서쪽 End 접합 RGBA 불일치0.
- Ground315 + Road42 + Props18 = 환경 SpriteRenderer375. 기존 Road30 위치만 동서 x=-4..10/y±0.5로 수정, Collider 없는 Road12 추가 x=-10..-5, x=-10의2장은 WestEnd. 기존 GameObject/Scene/meta GUID 보존, 새 GUID 충돌0. 실사용 소스14종=Ground3+Props7+Road4. v1 Road2 최종 참조0, 선택7 배치0, 서쪽Portal 추가0.
- 원본 Scene의 기존363 Sprite 참조, Road30 위치, 환경자식12 외 기존 직렬화 항목 불변. Collider/Scale/Quest/NPC/Spawn/Exit/WorldBounds/기존 Arbel·Field04 변경0. RockNorth85×54/PPU100과 중앙Pivot 유지.

## 검증

- 기존 1,582 PASS 회전 시안 결과를 기반으로 최종 Road4/B42만 재검증: **416 PASS / 0 FAIL**. 실제 Main14 수로(5,2)·감시초소(-2,2)·강한진동(-6,-2), 세린(1,0), StoryBattle/일반 몬스터2종 실제 UI 승리·복귀, 위치/Spawn/35초 리스폰 중복0, 도보/휠체어 물리 경로, 실제 Arbel 왕복 트리거, Save/Bootstrap Continue/퀘스트·편성·위치, Missing Sprite/Script0.
- 최종 브랜치 Scene을 격리 실행 복사본으로 옮겨 다시 Runtime 로드: **84 PASS / 0 FAIL**, 환경375·도로42·PNG14·도로Collider0/회전0/FlipFalse·16:9/4:3/21:9 네 방향 Camera Bounds PASS.
- 같은 카메라에서 v1 남북 / 회전 임시 B42 / 실제 v2 B42를 7구도×3화면비=63 RenderTexture로 비교. 최종 저장 Scene도21장을 별도 재렌더. [3단 비교 갤러리](Evidence/Field05Road42Final20261019/index.html).
- 실제 설치 Editor6000.6.5f1, main ProjectVersion6000.5.7f1. 기존 Search SearchDatabase 예외 및 URP SpeedTree Host type 오류는 기준선과 동일; 신규 Runtime 오류0. 전체 meta 감사에서 기존 GoldenArrow/HealerMagicalProjectile 중복GUID 5쌍을 발견했으며 기준 main bytes와 동일함을 확인, 이번 변경 범위 밖으로 보존했습니다. 새 Field05 GUID 충돌0. 원본 Editor/포커스 및 OS 손입력 미검증.

## 미술 승인 및 OPEN

바울 대표는 새 v2 도로의 색·반복 무늬·불규칙 가장자리·서쪽 흙길 소멸을 최종 화면에서 판단하셔야 합니다. 기존 나무가 도로에 걸치는 부분과 동쪽 Portal/나무 근접, 조사점의 기존 사각 표식은 보존했습니다. 지형의 기존 v1 타일별 명도 차이도 유지했습니다. 길 위 나무를 옮기거나 기존 표식을 바꾸는 작업은 이번 적용 범위에 포함하지 않았습니다.

강한진동 정확 중심은 기존 RockWest Collider와 겹치므로 접근좌표(-6,-1)에서 상호작용하는 기존 유효 경로를 검증했습니다. 기술 PASS를 미술 승인으로 해석하지 않습니다. 최소 PNG 재제작 요청 없음; 반복감 등 미술 판단은 대표 승인 대기입니다. 다음은 대표 최종 화면 승인 후 이 브랜치 Push/PR·원본 적용 범위 재검사입니다.
