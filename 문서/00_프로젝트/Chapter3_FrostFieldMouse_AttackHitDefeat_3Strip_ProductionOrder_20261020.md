# LIMITLESS / Chapter3 서리들쥐 A 전투3모션 Strip 실물 제작 발주 (2026-10-20)

- 바울 대표: "쥐는 빨리 만들어야지 되는거 아냐?" → "그래." 서리들쥐 A 첫 전투 몬스터 빠른 제작 승인. 설빙의 폭군 B Attack/Hit/Defeat는 후순위.
- 담당: 기획 docs/planning; 실제 아트 제작 docs/art-new; GitHub Issue #30.
- 사용자 전달 Codex **기술 계약 최종 PASS**, branch codex/chapter3-mouse-combat-contract-20261020, 계약 commit ad9e4b46, 인계 13178071. 기존 코드/Chapter2 정의8종 감사·승인 PNG SHA 확인 완료. 이전 4×4 통합시트/PPU314 잠정안 폐기. 실제 PNG 제작·Unity 구현·Push/merge는 아직 안함.
- 상태: COMBAT_ART_3STRIP_PRODUCTION_AUTHORIZED / APPROVED_IDLE_UNCHANGED / GAME_RUNTIME_PENDING / USER_COMBAT_ANIMATION_APPROVAL_PENDING.

## 확정 기술 규격

| Strip(게임 독립 PNG) | 프레임 | 재생/종료 |
|---|---|---|
| Attack | 2048×512 RGBA, 좌→우 512×512 ×4 | 8 FPS, 앞발 할퀴기 → 기존 Idle 복귀 |
| Hit | 2048×512 RGBA, 좌→우 512×512 ×4 | 8 FPS, 피격 움찔/자세 복원 → Idle 복귀 |
| Defeat | 2048×512 RGBA, 좌→우 512×512 ×4 | 8 FPS, 쓰러짐 → **마지막 03 프레임 유지** |

- 세 Strip 전부 Import PPU512 / Pivot 하단중앙 (0.5,0) / VisualScale1 / 우향 1종 / 기존 엔진의 좌우 반전 사용.
- 기존 **승인 Idle**은 새로 그리지 않고 동일 파일/바이트 재사용: Sprites/FrostFieldMouse_A_Idle_4Frames.png, SHA256 5afebfa1f95ca5673162513fdea63b025546aa836550c0ceb17f0334659988f2. 승인 1포즈 기준 SHA256 0516914ddad36af5854065d66d01b9664329aa89eea8b394480eed6af92e32c2. 4프레임/8FPS/0.5초 Idle 선례와 접지/루프 승인 유지.
- Codex는 MonsterDefinition ExplicitFrame/MonsterSpriteSheetAnimation 기존 모션별 Sprite 참조 방식을 재사용하도록 계약함. 실제 엔진 연결·Damage Event·MonsterDef 신규 생성/Unity 소스 수정은 **이 미술 납품 범위에서 제외**.

## 미술 콘티 및 품질

- Attack: 준비 자세→앞발로 날카롭게 할퀴는 전방 공격→팔·몸통 반동→Idle 복귀 가능 포즈. 타격 시점 분명하게.
- Hit: 머리·어깨·등·꼬리의 피격 반사, Attack과 구분되고 끝나면 Idle 기준선으로 자연스럽게 복원.
- Defeat: 중심 붕괴→한쪽으로 무너짐→바닥에 눕고 움직임 정지→03 완전한 쓰러진 자세 유지.
- 승인된 야생적인 체형·긴 꼬리·앞니·날카로운 발톱·거친 털·눈빛·색을 보호. 프레임4장을 똑같이 복사하거나 전체 그림만 일괄 XY이동/단순 확대·색상보정/블러로 만든 가짜 모션 금지.
- 투명 RGBA, 프레임 간 발 기준선 안정·지면 미끄럼0·꼬리/발톱/머리 잘림0·셀 침범/떠돌이 픽셀0. Hit 및 Defeat는 물리적 자세 변형이어야 하며 다른 행의 완성 공격FX/냉기 이펙트를 내장하지 않음.
- 실제 게임 표시 크기 (서리들쥐 일반 몬스터 VisualScale1, 과거 Idle 실효 대략102×80 UI px)에서 타격/피격/사망이 식별되어야 함.

## 실제 납품·검수

신규 실 PNG3장만 제작:
1. Sprites/FrostFieldMouse_A_Attack_4Frames.png
2. Sprites/FrostFieldMouse_A_Hit_4Frames.png
3. Sprites/FrostFieldMouse_A_Defeat_4Frames.png

권장 ZIP: Limitless_Chapter3_FrostFieldMouse_Combat_3Strips_v1.zip. 각각 4컷 프레임 컨택트시트·정확한 8 FPS APNG/GIF 짧은 루프(Defeat 마지막 정지 프레임 명기)·128px 축소 미리보기, Manifest 및 각 파일/ZIP SHA256, ZIP CRC/testzip/PNG RGBA 크기·프레임/alpha/crop 검사 결과 포함. 실제 산출 파일을 전달해야 하며 문서만으로 완료처리 금지.
- docs/art-new 브랜치에서 납품 문서/커밋/실제 ZIP 전달 경로/승인 필요 여부 보고; main 직접 Push/merge 금지.
- 납품 뒤에만 Codex 격리 Unity에서 모션별 8FPS, Attack→Idle, Hit→Idle, Defeat 03유지, 좌우반전, UI 크기 및 시각 QA 최소 실행→바울 대표 미술 승인→정식 Unity 구현·main PR 별도.
- 다른 몬스터·보스 B 전투 애니메이션, 승인 Idle 재제작, Chapter3 F1/서릿길 마을 환경, Field09 리뉴얼, Save/UserData/Editor/원본 Unity 변경 금지.
