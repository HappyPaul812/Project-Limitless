# LIMITLESS Field07 「재바람 황야」 — 환경 리뉴얼 v1 실제 제작·납품

- 제작일: 2026-10-19 / GitHub `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`.
- 기획 정본: `docs/planning/문서/00_프로젝트/Chapter2_Field07_AshenReach_Renewal_Art_Assignment_20261019.md`.
- 담당 브랜치: `docs/art-renewal`.
- **상태:** `ART_DELIVERED / OFFLINE_SEAM_QA_PASS / USER_FINAL_ART_REVIEW_PENDING / ROAD_LAYOUT_HOLD / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.

## 최신 실제 Scene/Runtime 조사

- `Unity/Client/Assets/_Project/Scenes/Field_07_AshenReach.unity` blob SHA `2f267575bfbd1d7802b58c8653c2b33d7a84d6ac`, 실제 **정적 SpriteRenderer372개** = **AshSoil315, 기존 Road30, AshCrack8, BrokenTrack10, CanyonLip2, Tree5, Rock2**.
- 기존 AshSoil315는 원본 `3ffe8024a48343dcbe3ee849067c02d3`의 **`fileID278596618` 하나를 전체 바닥에 참조**(RoadLeft와 같은 레거시 Sprite). Scene AshSoil315 Color `(0.67,0.58,0.48,1)`, Sorting -10, FlipX0/1 각 158/157. 기존 Road30은 레거시 **남북형 15행×2열**. Road Sprite-only 적용으로 동서 진행 축 변경 완료라고 주장 금지.
- Main16 `Chapter2Main16Flow.CreateAshMarks()`에서 **이름이 `AshSoil_`로 시작하는 첫 SpriteRenderer의 현재 Sprite를 재사용하여 `WindlessAsh_0..6` 7개를 Runtime 생성**. Ground 교체 시 동적 재 그래픽도 간접적으로 바뀔 수 있습니다. 별도 optional PNG는 제안에 불과하며 코드 변경/Unity QA 전 자동 적용 금지.
- 고정 Quest/접근 위치: 입장(7,0), 재(4,0), 지면 진동(1,-2), 발자국(-1,1), 불씨망령 목격(-3,0), 승리 후 잔상(-3,-1), 협곡(-8,0), 공식 세린(4,-2). Main16 순차 12목표, Path Hearing 분기/불씨망령 공식 Story 연출·전투/Save/귀환·휠체어 및 Collider 보존.

## ZIP / 원본 PNG

- **실제 생성한 ZIP:** `Limitless_Field07_AshenReach_Renewal_Art_v1.zip`
- **크기:** 477,759 bytes / **27파일·PNG18개**.
- **SHA256:** `5d398295b6f98491a294628a18763b6d72e0dedfdf5f691ccd2e8d0539150403`.
- 오리지널 실사용 후보 PNG **15종**:
  - Ground `AshSoil` 3 (황갈색 동쪽/회갈색 중앙/그을음 서쪽)
  - Road 동서 Upper/Lower/WestEnd2 = 4
  - Tree2, RockWest1, RockNorth1, CanyonLip1 = Props5
  - 기존 AshCrack8/BrokenTrack10에 대응하는 투명 Decal2
  - **선택 Runtime 전용 WindlessAsh1** (기존 런타임 코드 변경·개별 승인 필요, 미적용).
- 나머지 PNG3 = Contact Sheet / 21×15 원본 좌표 기반 합성 전체 Scene 시안 / 반복 Road QA Preview. 색상변화는 동쪽 갈색·중앙 재빛 회갈색·서쪽 검은 자갈/약한 붉은 균열. 용암·산불·강제 화면 점멸·망령 정체 신규 실루엣 없음.
- `Manifest.json`에 실사용후보15종 SHA256, Canvas/Alpha bbox/PPU/CenterPivot/Point/NoMips, `Renderer_Mapping_372.csv`에 정적 Renderer 이름·원본 GUID/fileID·좌표·Sorting/Flip·target 및 HOLD 기록(372행). `Road_EW_A_B_Proposals.csv` A30/B42 **72행**, `Runtime_WindlessAsh_7_Audit.csv` 런타임7 분리.
- `README.md`, `QA/Offline_Validation_v1.json`, `Rights_and_Provenance.md`, `Codex_Integration_Handoff.md`, `Codex_ReadOnly_SceneAudit_Prompt.md` 포함. 기존 Field04·Arbel·Field05·Field06 공식 PNG는 복사·덮어쓰기 없음.

## 오프라인 검증 / 실제 Unity 미검증

- ZIP CRC PASS, 모든 PNG18 decode PASS, Manifest PNG15 파일 SHA 전부 PASS, 정적 Renderer 대상 이름 372/372 고유, Ground3 18개 좌우/상하 조합 + Road6개 연결 **총24검사 RGBA mismatch0**.
- 기존 Road30는 여전히 남북 배치이므로 새 Road4를 단순 Sprite 교체로 적용할 수 없음. **A30(x=-4..10,y±0.5)** 또는 **B42(x=-10..10,y±0.5, 신규 Renderer 12개)**는 미선택. 서쪽 Canyon은 Main16 정본 기존 연결만 보호하고 새 Portal은 그림/구조에 추가하지 않음.
- 실제 Source AssetImporter Pivot/Alpha foot/Physics Collider, Main16 Runtime Ash7의 작은 직사각형 문제, Color Tint, 실제 RenderTexture·세린/Story 적/Quest12/Save/휠체어/Scene 왕복은 **NOT_VERIFIED**. Preview는 오프라인 생성 PNG를 배치한 **합성 모식도**로 Unity Game View 캡처가 아닙니다.
- 현 단계는 제작·오프라인 파일 검증만 완료. 바울 대표 미술 승인 → Codex 안전 격리 Import·접근성·Main16 12목표 및 공식 몬스터 실제 Runtime 검사 → 별도 승인 후 Unity 정식 반영. Main09 음성 버그 작업/사용자 dirty·Save를 침해하지 않음.
- Git: 문서 기록만 `docs/art-renewal`; main 직접 Commit/Push/병합 없음.
