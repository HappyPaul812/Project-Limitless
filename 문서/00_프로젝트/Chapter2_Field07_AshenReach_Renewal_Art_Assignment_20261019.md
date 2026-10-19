# LIMITLESS / Chapter2 Field07 「재바람 황야」 환경 리뉴얼 v1 제작 발주 (2026-10-19)

- 기획 브랜치 `docs/planning`; 납품·자체 검수 담당 `docs/art-renewal`.
- 대상 Unity Scene: `Unity/Client/Assets/_Project/Scenes/Field_07_AshenReach.unity`; Main16 「재 속의 형상」.
- 상태: **PLANNING_ART_ASSIGNMENT_READY / ART_V1_PRODUCTION_AUTHORIZED / USER_FINAL_ART_REVIEW_PENDING / ORIGINAL_UNITY_UNCHANGED**.
- 선행: 바울 대표 2026-10-19 **Field06 v1 미술 방향 승인**. Field06 4가지 기술 보완(도로 상하 매핑/중앙 Alpha 틈/Runtime Grass49 비활성/Props 접지)은 **별도 v1.1 최소 수정 단계**로 지속. 기존 Field04, Arbel, Field05 승인 아트 중복 제작 금지. Field07 신규 콘셉트는 Codex 검증 작업과 병렬 진행해도 되나 Field06 서쪽 경계와 접속되는 화면 품질은 유지.
- 근거 정본: `문서/03_스토리/Chapter2_Main16_재_속의_형상.md`, `Unity/Client/Assets/_Project/Scripts/World/Chapter2Main16Flow.cs`, `Unity/Client/Assets/_Project/Resources/QuestDefinitions/Main16_ShapeInTheAsh.asset`, `문서/00_프로젝트/LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`.

## 1. 미술 콘셉트 — 그을린 산길에서 바람 없는 재의 황야로

- **동쪽**: Field06 마지막 흙길의 갈색/어두운 탄 흔적을 유지하며 황토·검게 탄 자갈·드문 죽은 풀.
- **중앙**: 고사목·마른 관목 감소, 회갈색/석탄빛 미세 재와 붉지 않은 균열·검은 바위 증가. 움직이는 재는 **바람 방향과 무관한 현상**이라는 것이 Main16 핵심이므로 산불·강풍 효과로 오해되지 않게 디자인.
- **서쪽**: 붉은 균열 협곡(Field08)으로 이어질 짙은 암갈색 바위·중간 강도의 붉은 틈·약한 열 아지랑이. Field08 붉은 균열 폭발적 강도와 보스의 화염 심부 모습은 앞당기지 않는다.
- **피해야 할 것**: 용암 강, 활활 타는 화염, 화산재 폭풍으로 시야 전체 가림, 빛 번쩍임/화면 점멸, 망령/보스 정체를 미리 보여주는 벽화·실루엣. 음성만으로 방향을 알려주는 아트 금지.
- **주 이동 방향 동↔서**: 동쪽 Field06에 들어오고 왼쪽 서쪽 협곡에서 Main17로 자연히 넘어갈 수 있도록 길의 시각적 방향을 확보한다. 실제 Scene Road Renderer가 기존 남북형인지 최신 YAML을 조사한 다음 자산·배치안 작성; 임의로 기존 Field06의 B42를 기정사실로 복사하지 않는다.

## 2. 읽기 전용 Scene/Runtime 구조 감사

- GitHub 현재 main에서 **실제 Field07 Scene 원본 SpriteRenderer/Asset/GUID+fileID/Importer rect·PPU·Pivot/Tile Renderer/Transform/Flip/Tint/Sorting/Collider/Spawn/Exit/Bounds**를 파악한 뒤 실사용 자산 수량과 맵핑표 결정. Field06과 똑같이 363이라고 추정해 기록하지 않는다.
- `Chapter2Main16Flow`는 Arbel·Field06·Field07의 기존 Quest/Scene 위에 Runtime 진행을 설치한다. Field06 서쪽 출구 `field06_main16_west_exit`(-8,0); Field07 도착 `field07_main16_entry`(7,0), 재 `field07_main16_ash`(4,0), 진동 `field07_main16_vibration`(1,-2), 동물 자국 `field07_main16_tracks`(-1,1), 불씨망령 목격 `field07_main16_witness`(-3,0), 승리 뒤 균열 `field07_main16_afterimage`(-3,-1), 협곡 진입 확인 `field07_main16_canyon`(-8,0), 세린 Story NPC (4,-2). 이 좌표의 접근·시야·표시와 휠체어 동선 보존.
- `CreateAshMarks()`는 `AshSoil_` Sprite를 참조해 Runtime `WindlessAsh_0~6`을 배치하며 `Main16AshPulse`를 붙인다. **정적 Sprite와 Runtime7·Quest/Wraith 시각 연출은 별개**. 기존 코드의 Source Name이 사라지거나 런타임 아트가 비정상 밝기로 바뀌는 것을 방지할 대응 계획 필수. 망령 Story Apparition 및 Monster Idle/FieldSprite는 공식 승인 시트, 새 몬스터 시트 제작 금지.
- Main16 **12목표**: 레온→Field06 서쪽→Field07 진입→재 조사→진동→야생동물 흔적→망령 목격→지정 망령 전투→잔상 균열→협곡→Arbel 귀환→레온 보고. 몬스터 일반조우·Story Encounter/패배/재도전/Save와 Path Hearing 분기, 세린 미편성 Story NPC, BGM/TTS를 변경하지 않는다.

## 3. v1 제작/납품 요구

- 실제 원본 Sprite 역할에 맞는 Ground/동서 Road(필요한 경우)/재 쌓인 바위·소규모 균열·마른 식물·고사목 및 별도 구역표식 등의 **실사용 가능한 개별 투명 PNG**와 필요한 타일 접합 후보. 수량·캔버스·PPU는 현장 매핑 결과로 제시하고 임의 확정 금지. 가독성을 위협하는 밝은 배경 광원/짙은 불투명 재구름 사용 금지.
- 안전한 배치 경계: 동쪽 입구 7,0·서쪽 -8,0 / 조사 중앙(4,0),(1,-2),(-1,1),(-3,0),(-3,-1) 주위에 소품·바위·나무가 플레이어와 마커를 가리지 않도록 설계. 기존 Collider, Spawn, Monster의 접근 반경·Bounds·Save 좌표를 움직이도록 아트만으로 강제하지 않는다.
- 권장 영문 납품명: `Limitless_Field07_AshenReach_Renewal_Art_v1.zip`. 포함: 개별 PNG·`Manifest.json`(SHA256/width,height/PPU/Pivot/Alpha/role), `Renderer_Mapping.csv`(source GUID+fileID/scene/object/transform/static-runtime 판별), `README.md`, `QA/Offline_Validation.json`, `Rights_and_Provenance.md`, ContactSheet/동서 변화를 보여 주는 합성 Preview, 필요시 Road 위치 매핑 후보, Codex Unity 통합/읽기 전용 감사 지시.
- 프리뷰가 합성이라면 합성이라고 표시, 실 Unity 캡처로 가장하지 않는다. Asset alpha/연결 Seam0/발 접지 후보/ZIP CRC/Manifest SHA를 오프라인 검사. 몬스터 직접 조우, 지정 망령 연출, 월드 맵 왕복, 보행/휠체어/실 Save, 3비율 카메라/Collider 검증은 Codex Runtime 이후 단계로 명시.

## 4. 순서·승인·보호

1. 리뉴얼 담당은 **먼저 Field06 v1.1에서 실제 미술 PNG/Alpha/Props Foot/UpperLower 네이밍에 해당하는 최소 수정만** 점검하고 승인된 Field06의 나머지 PNG byte를 보호. 실 Unity 런타임의 49 Grass 비활성화·Collider·Quest 로직/Scene 파일 패치는 Codex 담당이며 아트 담당이 직접 구현하지 않는다. 이 역할 분리로 Codex에게 PNG를 창작시키지 않는다.
2. Field06 아트 v1.1 인계(필요한 자산만 수정)와 병행해 Field07 아트 콘셉트·소스 감사·독립 제작을 시작한다. 그러나 Field06 미술과 이어지는 동쪽 Field07 팔레트와 Road 부위 불일치가 없도록 동쪽 접속안을 조정한다. Field06의 미검증 배치안(A30/B42) 선택을 몰래 확정하지 않는다.
3. Field07 실제 PNG/ZIP v1 → 바울 대표 미술 승인 → 별도 Codex feature `codex/*` 격리 Import·Quest12/몹·Story·BGM/TTS·Save/Continue/도보휠체어 QA → 비파괴 원본 적용 검토. Field06 기술수정을 Field07 신규 아트 PR과 섞지 않는다.
4. GitHub 문서는 **`docs/art-renewal`에만** 작성·인계 갱신, 작업 완료 보고 브랜치/커밋 SHA/ZIP SHA/QA 범위/대표 승인 여부 기록. main 직접 커밋·Push·병합 금지, 기존 사용자 원본 dirty·Save·Assets/meta/GUID/Field04~06 승인본·승인된 세린/보스/TTS 변경 금지.

**기획 허가 = 신규 Field07 오프라인 아트 제작과 문서화만**. 미술 최종 승인·원본 Unity 배치·신규 몬스터/보스/Runtime 코드 작업 허가로 확대하지 않는다.
