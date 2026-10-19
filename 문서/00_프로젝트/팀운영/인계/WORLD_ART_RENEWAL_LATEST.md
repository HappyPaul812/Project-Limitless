# WORLD_ART_RENEWAL 최신 인계 — 2026-10-12

## 2026-10-19 Field06 v1.1 기술 보완 및 Field07 v1 신규 아트 — 두 ZIP 납품

**최신 정본 우선 (2026-10-19 오후 기준):** 기획 `docs/planning`의 `Field06_Art_v1_UserVisualApproval_TechnicalFix_20261019.md`, `Chapter2_Field07_AshenReach_Renewal_Art_Assignment_20261019.md`에 의해 **Field06 v1 미술 방향은 바울 대표 승인**, 단 **기술 수정/Unity 정식 적용/PR·main 병합은 미승인·미완료**. 기존 역사적 Field06 `USER_ART_REVIEW_PENDING`을 오늘 현재 상태로 오인하지 않을 것. Field04/Arbel/Field05는 승인 적용 정본 유지.

### 작업 A — Field06 v1.1 기술 최소 보완

- 제작 ZIP: `Limitless_Field06_ScorchedTrail_Renewal_Art_v1_1.zip`, **1,399,079 bytes / 41파일 / PNG29(실사용22 + Preview/QA7)**, SHA256 **`d8551df7315495dcc6e4556ec33f30b711af8aeb85dc7b0a1c3612a5a6197f42`**. v1 ZIP `d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70` 보호·무변경.
- v1 실사용 PNG22 중 **Road Upper/Lower/WestEnd Upper/WestEnd Lower 4개만 v1.1 기술 수정**. 기존 그림의 상하 위상과 실제 y=+0.5/-0.5 행이 반대로 읽히는 배치 오류를 교정하고, 중앙 경계 Alpha0 자리에 불투명한 동일 RGBA 연결 한 줄 부여. 기존 Ground3/Props7/선택 Runtime6/Decal2 **실사용 PNG18개 모두 v1 SHA/바이트 동일**. 새 Road4는 128×128 RGBA PPU128 Center/Point NoMip.
- `Road_EW_Placement_Proposals.csv` A30/B42 72행은 새로운 v1.1 파일명으로 갱신했으나 **A/B 사용자 최종 결정 없음**. 기존 `Renderer_Mapping_363.csv` 원본 source/coord 보존, **Road30은 `LAYOUT_GATE_REQUIRES_A30_OR_B42` 명시**. 원본 Scene을 Sprite-only 교체로 처리하지 말 것.
- QA: 중앙 불투명 연결 128px(직선 Upper/Lower), WestEnd 동일 중앙 결합, 직선 반복·End→직선·UpperLower **6조건 seam RGBA mismatch0**; ZIP CRC/PNG29 해독/Manifest22 SHA PASS. Props 7 새 후보 Alpha 접지 수치표만 제공하고 기존 소품 PNG는 전혀 수정하지 않음. Codex가 원본 Sprite.rect/본체 Alpha/Collider 차이를 실제 측정한 후 개별 문제 확정 필요.
- **Runtime Grass 49개 비활성화 및 Unity Sorting/실제 빈틈 문제는 Codex 영역.** 717 PASS/0 FAIL은 Codex v1 이전 격리 QA이므로 v1.1 재검증으로 승격 금지. 현재 `USER_V1_VISUAL_DIRECTION_APPROVED / ART_V1_1_OFFLINE_QA_PASS / ROAD_A_B_UNDECIDED / OFFICIAL_UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.
- [납품·기술 QA 정본](../../Field06_ScorchedTrail_Renewal_Art_v1_1_TechFix_Delivery_20261019.md), 문서 첫 커밋 `c189e8a784d33c979b58b4800e5993740ca53e86`.

### 작업 B — Field07 AshenReach v1 오리지널 신규 미술

- 실제 GitHub `Field_07_AshenReach.unity` blob `2f267575bfbd1d7802b58c8653c2b33d7a84d6ac`: **정적 SpriteRenderer372 = AshSoil315/Road30/AshCrack8/BrokenTrack10/CanyonLip2/Tree5/Rock2**, 기존 Road30은 남북형. Runtime `Chapter2Main16Flow.CreateAshMarks()`는 첫 `AshSoil_` Sprite를 선택해 **WindlessAsh7개** 생성하므로 새 Ground 적용 시 재 입자의 불투명 사각형 표현·가독성 변화를 꼭 Codex가 별도 테스트해야 함.
- v1 ZIP **`Limitless_Field07_AshenReach_Renewal_Art_v1.zip`**, **477,759 bytes, 27파일, PNG18(실사용15+Preview/QA3)**, SHA256 **`5d398295b6f98491a294628a18763b6d72e0dedfdf5f691ccd2e8d0539150403`**. 새 후보 PNG15=Ground3/Road4/Props5/Decal2/별도 Runtime 선택1. 본체/세린/불씨망령/공식 자산·이전 Field 아트 재사용/덮어쓰기0.
- `Renderer_Mapping_372.csv` 이름 고유372·GUID/fileID/XY/Sorting/Flip/target, `Road_EW_A_B_Proposals.csv` A30/B42 후보72행, `Runtime_WindlessAsh_7_Audit.csv`, Manifest15 SHA 및 Alpha footprint·PPU/Pivot·QA, Contact Sheet·실제 PNG 합성 Preview(실 Unity 캡처 아님) 포함. **오프라인 Ground18 + Road6 =24 seam RGBA mismatch0**, ZIP CRC/PNG18 Decode/SHA PASS.
- Main16 12목표/세린(4,-2), 조사·목격(7,0)(4,0)(1,-2)(-1,1)(-3,0)(-3,-1)(-8,0), Path Hearing/공식 Ember Wraith·아르벨왕복/Save/도보·휠체어 모두 보호, **실제 Unity 검증 전**.
- **상태:** `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / ROAD_LAYOUT_UNDECIDED / RUNTIME_ASH7_HOLD / UNITY_NOT_APPLIED`; A30/B42 사용자 승인 전 Scene 이동/추가 Renderer12 생성 금지. Runtime WindlessAsh 선택1도 별도 코드 승인 전 자동 적용0.
- [Field07 납품·검수 정본](../../Field07_AshenReach_Renewal_Art_v1_Delivery_20261019.md), 문서 첫 커밋 `670173ee19d422992086f4dd11644ad4b1b32136`.

**둘 다** 이 ChatGPT 세션에서 생성해 다운로드로 전달. 바울 대표 PC F: 드라이브에 자동 저장했다고 말하지 않음. GitHub 문서 커밋은 `docs/art-renewal` 전용이며 main/Unity 원본/GUID/meta/Save/사용자 미커밋/Monsters/Chapter3 변경 없음. 다음: Field06은 Codex 소규모 Delta QA, Field07은 바울 대표 시안 미술 승인 후 Codex 격리 통합. Field07 미술 검토 전에 Field08 공식 자산 중복 제작 착수 금지.

## 2026-10-19 Field06 ScorchedTrail 환경 리뉴얼 v1 제작·ZIP 납품

- **최신 확인 기준:** `main` `675988c9f2c5c507d9a696b0378be27c0bd3309b`; 기획 발주 `docs/planning/문서/00_프로젝트/Chapter2_Field06_ScorchedTrail_Renewal_Art_Assignment_20261019.md`. 현행 Field04 승인42길/Arbel 공식23종/Field05 공식 B42는 모두 **기존 공식 적용 자산**, 재작업 금지. 이전 본 인계에서 Field05 A/B 미선정이라고 쓴 부분은 2026-10-19 그때의 **역사 기록**이며 최신 main Field05 B42 공식 적용 상태가 우선.
- 실제 GitHub Field06 Scene(원본 blob `01c31f42fb1ff477f496193f14ca9c1e349ad7fc`) **static SpriteRenderer363 = Grass315 + Road30 + Props18** 분석. 21×15 지면/원본 Ground Source ID 104·105·106개, Road 기존 15행×2열 **남북형**. `Chapter2Main15Flow.BuildEnvironment`는 **Runtime MeshRenderer Quad6** 추가하며 x<2 Grass tint/x<-3 짝수 Y 비활성화 처리 → 정적 렌더러와 분리. Main15 입장 (7,0), 조사 5곳·세린(-6,1), Encounter4 원본 위치 및 Quest12·기능 보호.
- 실제 제작 다운로드 **`Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip`**, SHA256 **`d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70`**, 크기 **1,362,496 bytes**, **38파일/PNG28**(새 실사용 후보22 + Preview/QA6). **새 PNG22종**: 정적 후보 **14**(Ground3/Road4/Props7), 별도 사용자 개별 승인 선택 **8**(Main15 Runtime Quad6용 후보·새 Decal2). 기존 Field04/Arbel/Field05/세린/몬스터 Sprite 복사·재제작·덮어쓰기0.
- **A30 / B42 동서 Road 배치는 둘 다 제안만**, 기존 남북 Road30의 위치 이동이 필요하므로 **Sprite-only 적용 금지**. A30 기존 30위치 재배치 x=-4..10/y±0.5, B42는 12 Renderer 추가 x=-10..10/y±0.5; 서쪽 흙길 끝은 **출입구·Portal 아님**. `Road_EW_Placement_Proposals.csv` 72행 제공, 어떤 안도 확정 선택하지 않음.
- 오프라인 QA: 대응표 static 363행·고유 이름363, Ground3× 양방향 + Road 중앙/반복/WestEnd/Alpha **27조건 RGBA 불일치0**, ZIP CRC PASS, PNG28 해독 PASS, Manifest 신규 PNG22 SHA PASS. 실물 Unity Import·Runtime/도보·휠체어/Monster/Quest15/Save·Main15 Runtime Grass 빈틈·원본 Sprite foot/Cam QA는 **NOT_VERIFIED**.
- 출처/권리 및 Codex 읽기전용 감사 프롬프트, `Runtime_Patch_Audit_6.csv`, `Renderer_Mapping_363.csv`, ContactSheet/Scene좌표 합성 비교 Preview 제공. 사용자 F드라이브·Unity 원본·Git main 수정0, Codex는 Main09 음성작업 우선이므로 **실제 Codex 조사 수행을 주장하지 않음**.
- [Field06 v1 납품 정본](../../Field06_ScorchedTrail_Renewal_Art_v1_Delivery_20261019.md), 최초 docs commit `532c8c0af196192a0e9caa051afff4fcc817c5ff`, 작업 브랜치 `docs/art-renewal`.
- **현재 상태 `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / UNITY_RUNTIME_NOT_VERIFIED`**. 다음: 바울 대표 실 PNG/미리보기 미술 승인 → Road A/B·선택8 개별 여부 확정 → Codex 격리 Scene·접근성·Main15·Arbel왕복 QA → 별도 승인 후 Unity 적용. 다음 차례 Field07 환경은 Field06 미술 방향 확인 후 착수, Chapter3 아트는 `docs/art-new`.

## 2026-10-19 Field05 동서 Road v2 최소 아트 보완 납품 — A/B 미선정

- 발주: [`docs/planning`의 동서 진행로 정본](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Chapter2_Field05_EastWestRoad_VisualCorrection_20261019.md), 기획 SHA `b614a5c6d6a1e51316c0ed22e03cc34b517982c5` 및 사용자 전달 Codex 실제 Unity6000.6.5f1 격리 비교/PNG 인계. 리뉴얼 담당은 Codex 로컬 `F:/.../Road_PNG_Handoff.md`를 직접 읽었다고 주장하지 않고 전달받은 전문으로 작업.
- **신규 최소 Road4종 실제 제작**: `LL_C2_F05_Road_EW_Upper_128_v2.png`, `...Lower_128_v2.png`, `...WestEnd_Upper_128_v2.png`, `...WestEnd_Lower_128_v2.png`. PPU128/Center/128×128 RGBA, Z0/Flipfalse. 북·남 외곽 투명 흙 경계/서쪽 끝 흙으로 자연 소멸. 새 서쪽 Exit/Portal/화살표 아트0.
- 채팅 다운로드 실제 ZIP **`Limitless_Field05_EastWest_Road_v2.zip`**, 589,850 bytes, 14파일/PNG8(실사용4+합성Preview4). **SHA256 `3a0d3b91cd87a5362ebbe435c1e5acd06ffc74354eafaa41f02b4f181d27c3d3`**. v1 ZIP은 **`a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`** 그대로 보존, Ground3/Props7/선택7·Field04/Arbel 승인 파일 재제작0.
- 오프라인 RGBA 연결 seam 검증: 직선 Upper/Lower 좌우 반복, Upper-Lower, End→직선, End Upper-Lower 각 mismatch **0**, 북·남/서쪽 외곽 Alpha0. PNG4 hash/Decode/ZIP CRC PASS. **실제 Unity RenderTexture/접근성/물리/Save·Quest14/몬스터/Arbel 귀환 Runtime 미검증.**
- 두 배치 모두 별도 승인 대기: **A 30기존 Renderer(x=-4..10,y±0.5)** / **B42(신규12추가, x=-10..10,y±0.5)**. 두 경우 서쪽 최소 두 타일은 Endcap. B 끝은 지도 경계이지 새 서쪽 출구 아님. `Road_Placement_A_B.csv` 72행 및 Codex 적용 인계 ZIP 포함.
- **현재 `ART_DELIVERED / OFFLINE_SEAM_QA_PASS / A_B_UNDECIDED / USER_FINAL_ART_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.** Codex A/B 기능 검수는 사용자 인계 시 아직 진행 중. 여기서 A/B 최종 선택·원본 Scene 반영·새 GUID/Renderer 추가 승인으로 해석하지 않음.
- 다음: 바울 대표 4개 PNG 및 반복/WestEnd 미술 검토 + A/B 최종 선택 → 승인안만 Codex 격리 worktree 실제 Unity Road·가독성·휠체어·Main14/Save 검증 → 별도 사용자 정식 적용 승인. 기존 Field03 Rock B·선택 소품7·Field06 범위는 독립 보류.
- [Field05 Road v2 납품 문서](../../Field05_EastWest_Road_v2_Art_Delivery_20261019.md), 최초 기록 SHA `8463ef25be012a792ab67c29fb277647e34ad771`, 담당 `docs/art-renewal`. `main` 직접 Commit/Push/병합 금지.

## 2026-10-19 최신 main 우선 갱신 및 Field05 v1 아트 납품

**상태 기준시점 2026-10-19 / 기준 `main` `c263d9e36fc0570b16130d9c011abf3965def5f7`.** 현재 `main`에서는 **Field04 v2 공식 42도로**와 **Arbel v3 기본23종·건물 Depth/가림 보완**이 승인·적용 및 검증된 상태입니다. 이 문서 10월13~14일의 `USER_ART_REVIEW_PENDING` / `UNITY_NOT_APPLIED`는 **그 당시의 역사 기록**으로만 읽으세요. Arbel 선택 장식11개는 여전히 미배치, 기존 Portal/나무 겹침 OPEN. Field03 Rock B 미술 승인은 Field05 범위와 무관하게 별도 유지.

### Field05 실제 납품 (리뉴얼 담당)

- 최신 기획 `docs/planning` 발주서 `Chapter2_Field05_Renewal_Art_Assignment_20261019.md` 조회 후 Source Audit. GitHub `Field_05_WesternOutskirts.unity` **직렬화 SpriteRenderer363 = Grass315 + Road30 + Props18**, 실제 좌표·GUID/local fileID 대조. Grass 원본 Sprite 분포 104/105/106참조, 기존 Road는 남북 계단형; 포지션 이동0.
- 메인14 보호좌표: 메마른 수로 **(5,2)**, 감시초소 **(-2,2)**, 강한 진동 **(-6,-2)**, 세린 **(1,0)**. 몬스터 두 종 각각 **(3,-3)** / **(-4,3)**, 리스폰35초, 동쪽 Arbel Spawn **(8,0)** / Trigger **(10.7,0)**.
- 실제 제작 `Limitless_Field05_Renewal_Art_v1.zip` **3,275,850 bytes / 35파일 / PNG26**. PNG **19종 신규 환경 후보** = 기존 Renderer에 바로 대응하는 12종(Ground3/Road2/Props7) + 현재 Scene에 없는 **별도 승인 필요 선택7종**(수로·낡은 감시초소·진동 등). Preview/QA PNG 7. ZIP SHA256 **`a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`**.
- `Renderer_Mapping_363.csv` 363/363 (GameObject 이름·XY·원본 GUID/fileID·Sorting·신품경로), `Manifest.json` PNG19 SHA, `QA/Alpha_Grounding_Audit.csv`, `QA/Offline_Validation_v1.json`, `Codex_ReadOnly_Source_Audit_Prompt.md`, `README.md`, `Rights_and_Provenance.md`, ContactSheet 및 비교(기존 Scene 역할 **모식도**, 실제 Unity 캡처 아님) 포함.
- 오프라인 ZIP CRC PASS, 전체 PNG26 decode, PNG19 Manifest SHA 일치, Ground3·Road2 좌우/상하 최외곽 RGBA seam 불일치0. **Original Sprite alpha foot, new Unity Import, 실제 가독성·휠체어·Monster/Quest14/Save·Runtime는 NOT_VERIFIED**. 특히 RockNorth의 기존 Sprite 85×54/PPU100 Pivot/접지는 Codex 실물 검수 필수.
- 이 ChatGPT 세션의 다운로드 링크로 제공하며 바울 대표 F드라이브 직접 기록/Unity 적용은 하지 않았습니다. Field04·Arbel 승인 PNG 재작업0, 정식 Scene·Save·기존 GUID/meta·NPC/몬스터·TTS/BGM·ThirdParty 원본수정0.
- **현재 상태:** `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 다음: 바울 대표 전체 Scene 합성 및 개별 PNG 미술 승인(직접교체12/선택7 범위 별도 결정) → Codex 읽기 전용 실물 대조 → 승인 후 feature worktree에서 Import/Scene·기능 회귀. 다음 환경 Field06은 Field05 미술 방향 검토 후 착수.
- [Field05 v1 납품 정본](../../Field05_Renewal_Art_v1_Delivery_20261019.md). 납품 문서 최초 커밋 `236bb3c2a4478f9098f7cbf5634ab8e290010541`; 담당 `docs/art-renewal`. main 직접 커밋/Push/병합 금지.

---

**최종 사용자 결정(2026-10-12): 리뉴얼 담당은 Chapter 1·2 기존 아트 교체(환경·몬스터·보스·펫·NPC·VFX)만 전담합니다. 기존 스프라이트 담당은 Chapter 3 이후 모든 신규 아트(환경·필드·던전·몬스터·보스·펫·NPC·캐릭터·VFX·UI 필요 이미지)를 총괄합니다. 기존 공식 승인 자산은 별도 승인 없이 교체하지 않습니다. Codex는 구현·Unity 통합·QA 전담입니다.**

Chapter2 Field04 실제 아트 ZIP 제작부터 시작. 환경 순서 Field04 → Arbel → Field05~Field11. 환경 완료 후 Chapter1·2 기존 몬스터→펫→NPC→VFX. 실제 공식 승인 몬스터 시트·캐릭터·보스는 임의 교체하지 않습니다. B1 문 상태 표시는 Codex 보완 보고, Field03 Rock B는 **ART_FIX_PENDING**. 최신 로드맵 `../../LIMITLESS_Chapter2_Original_Art_Roadmap_20261012.md`·전체결정 `../../LIMITLESS_20261012_전체대화_결정_인계.md` 참고. 사용자 승인 뒤 Codex Unity 통합. 세션 여유가 줄면 사용자에게 사전 보고·후임 인계.


## 2026-10-13 신규 리뉴얼 담당 인수인계 중

사용자 보고: Chapter 1·2 기존 아트 **리뉴얼 담당 신입 세션이 교육 중**입니다. 교육 완료·실제 신규 납품·Unity 통합 완료로 기록하지 않습니다. 위 역할 경계와 기존 리뉴얼 로드맵을 우선 학습시키고, 진행 중인 Field03 Rock B v2·Field04 결과는 새 담당이 최신 검수/승인 상황을 조회한 뒤 인계받도록 합니다. 이전 담당의 결과물을 중복 제작하거나 신규 Chapter3 아트를 담당하지 않습니다.

## 2026-10-13 Field04 v2 동서 길 보완 — 아트 납품, 사용자 승인 대기

- 담당 브랜치: `docs/art-renewal`. 납품·기술 보류 정본: [Field04 v2](../../Field04_Art_v2_EastWestRoad_Delivery_20261013.md). 기존 v1/Unity 원본·Scene·GUID/meta·Save·공식 세린/몬스터를 변경하지 않았음.
- 실제 v2 ZIP `Limitless_Field04_Renewal_Art_v2.zip`을 ChatGPT 파일로 생성하여 사용자 다운로드용으로 제공. SHA256 `178ad6b235c65848cce48f4e2cfe86ff77c36857a12663237f7d7afa561f969d`; 파일68개/PNG60개. 사용자 F: 저장 확인은 아직 없음.
- 기존 Road30 오브젝트별 서→동 15열×2행 후보(30개 독립 PNG와 배치표) 및 추가 양끝 연결용 12PNG(별도 승인 필요) 제공. Scene 자체 회전/원본 전면 교체하지 않음. v1 합성 대비 Preview는 실제 Unity RenderTexture 아님.
- Grass `tiles_grass_6_0` 라벨 정정, 실제 Medium3/Big2 나무 역할 정정, Field04 RockNorth 접지 후보만 신규 제작. **Field03 Rock B v2와 혼동 금지**.
- 상태: `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 다음: 사용자 미술 승인 → Codex 전용 worktree에서 선별 연결·접지/휠체어/세린 Main12~13/Save QA. 기존 서쪽 Spawn-TreeWest 겹침, 북쪽 경계 이슈 유지.
- 저장소 협업: main 직접 커밋/강제 Push 금지. 담당별 문서는 `docs/art-renewal`; 사용자 승인 후 별도 main 병합.

## 2026-10-14 Arbel 리뉴얼 v1 — 실제 Sprite PNG34종 ZIP 납품

- GitHub main 최신 `bddca540d2fdd9e93f642f8d0f387b34730bd2d8`, PR10 병합 및 5씬 Sprite복구1747/11PNG/123Sprite·23BuildScene PASS를 기준으로 복구 원본 자산 모두 보호. 과거 PR10 Blocked를 최신 상태로 오인하지 않음.
- 현행 Arbel 직렬화 SpriteRenderer **284(Grass247/Path16/건물8/기타13)**, Runtime295는 별개. GUID+fileID·이름·좌표를 근거로 정확히 284행 Renderer 대응표 제작. 실제 서비스 NPC는 대부분 Runtime 생성.
- 새 납품: `Limitless_Arbel_Renewal_Art_v1.zip` 파일47/PNG40 중 **신규 환경34**(기존 렌더러 치환후보23 + 별도 장식11). SHA256 `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626`. ChatGPT 다운로드용이며 사용자 F드라이브 저장·청취/미술 검수는 별도.
- 미술 콘셉트: Field04 v2와 연속되는 건조 올리브·황토 흙길, 따뜻한 목재·지붕/석재·중앙 광장/생활 소품. 정본 고정된 두 건물 및 기존 Path16/Grass247로 정적 합성 Preview 제작. Preview는 실제 Unity RenderTexture 아님.
- 현행 PortalAnchor(-7,-4)와 기존 TreeSouthWest(-7,-4.2) 중첩 확인. Portal 장식 추가는 **HOLD_COLLISION_WITH_EXISTING_TREE**로 등록하고 미리보기에서도 제외. 두 지붕 384×192 확장으로 NPC/Camera·접지 QA 필요.
- 아트 결과물 내부 `Manifest.json`, `Renderer_Mapping_284.csv`, `Optional_Service_Placement.csv`, `README.md`, `QA/Offline_Validation_v1.json`, 출처·상업 사용 선언, Codex 인계, Preview/Contact Sheet 및 비교 모식도 포함.
- **상태:** `ART_DELIVERED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 오프라인 PNG/CRC/SHA·284행 대응과 Ground 외곽 검사는 PASS이나 실제 Unity Import/직접 플레이/휠체어·Quest·Portal·마차/23Scene회귀는 검증 전.
- 문서 기록: [Arbel v1 납품](../../Arbel_Renewal_Art_v1_Delivery_20261014.md), 최초 Docs Commit `e84acba692f8e211bd2f28cef0de7f27a1ca82f6`. 담당 `docs/art-renewal`, main 무변경·강제 Push 없음.
- 다음 순서: 바울 대표 미술 승인(교체23종, 장식11종 각각 선택) → Codex의 로컬 원본 비교/선별Import·Scene 적용·복구 자산 보호·Runtime 검증 → 사용자 최종 화면 확인. 기존 Field04 공식42, Chapter3 신규 아트는 타 담당 영역으로 보호.

## 2026-10-14 Arbel v2 최소 수정 납품 — 사용자 최종 미술 승인 대기

- 기획 `docs/planning`의 `Arbel_Renewal_v1_MinorFix_QA_Handoff_20261014.md` 검토 후 **v1 전체 재제작이 아닌 Road2 + 북서 건물1 + Props9 접지 정보**만 보완.
- 실제 v2 ZIP `Limitless_Arbel_Renewal_Art_v2.zip`, SHA256 `17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7` (1,509,138 bytes, 파일55개/PNG43개 중 실사용34·Preview8·QA1). v1 ZIP SHA `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626` 불변.
- 사용 PNG34 가운데 **변경3, 동일 바이트31**: Road Left10pixel / Right4pixel 변경해 Left→Right·상하 RGBA seam mismatch 0. 북서 `Roof_Respite`의 실제 치유 위치와 무관한 십자205pixel만 동일 메달 배경으로 바꾸고 외형/384×192 HOLD 보호.
- Props9종의 원본 회복 Sprite.rect px와 신품 canvas/alpha foot를 `QA/Props_9_Grounding_Audit_v2.csv`로 보완. Props PNG 변경0; 원본 PNG의 실제 alpha foot 및 Unity 직접 접지는 `NOT_VERIFIED`. 부정확한 이미지 전면교체보다 현장 재측정 우선.
- `Renderer_Mapping_284.csv`: 이름/원본GUID+fileID/Transform/Sorting/PPU 불변, PNG target 필드 17참조만 새이름으로 정정. 직접 후보23 / 선택장식11 유지. PortalAnchor와 Tree 충돌/대형 지붕 런타임 가림 HOLD, 공식 캐릭터/NPC/Save/Quest/GUID/meta/복구 자산 변경0.
- 정본 기록: [Arbel v2 납품 및 QA](../../Arbel_Renewal_Art_v2_MinorFix_Delivery_20261014.md). 아트 문서 최초 commit `5d975a65cef117061fee9c3b6c89247ec18301aa`, 브랜치 `docs/art-renewal`.
- 현 단계 `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`. 사용자 미술 검토→Codex 읽기전용 소품 원본 Alpha 접지/scene 감사→미술 승인 후에만 Unity 최소 적용 및 QA. 기존 v1 인계는 이력으로 남기며 v2 미술 승인으로 자동 승격하지 않습니다.

## 2026-10-14 Arbel v3 소품 rect 기록 정정 — Codex 인계 접수

- **현재 최신 Arbel 후보는 v3**. 바울 대표가 Codex의 완료 인계/검증 근거를 전달했고, 리뉴얼 담당 세션은 **로컬 F: ZIP을 직접 열거나 검사하지 않았습니다**. v2는 역사 자료이며 8종의 `source_sprite_rect` 수치가 최신 정본이 아닙니다.
- 사용자 로컬 ZIP: `F:\Downloads\Limitless_Arbel_Renewal_Art_v3.zip`; 보고 SHA256 **`84e9707df7f0409e0238781f0860499dd4344b79ec88b49964a7ce7d8e10633b`**, **1,521,440 bytes / 60파일 / PNG43** (실사용34·Preview8·QA1). v2 SHA `17625d35c93599bdd74a4b8280c0416240c402be279815c8966199410bca97b7`·v1 SHA `ec707452169aa99ec297a7ac23a14928d4a0123b4b7b051d17ddcb282953d626` 보호.
- v2 대비 **PNG43/43 전부 byte·SHA 동일**. 변경은 문서/CSV·JSON뿐이고 새 PNG 재작업0. **직접 교체 후보23/선택11** 및 Renderer284/284, 이름·좌표·GUID/fileID·PPU/Pivot/Sorting/target 경로 불변.
- `fileID21300000` Single Sprite 실체 확인 후 8종 원본 `source_sprite_rect`만 정정: Barrel/Bush_Dry/Bush_Olive/Campfire/Chest/DryMossRock 모두 **128×128**; OldTree_Big **255×256**, OldTree_Medium **128×156**. Fence **128×128 불변**. 기존 meta의 Multiple slice rect를 Single Sprite rect로 혼용 금지.
- 원본/후보 Alpha>0 하단은 그림자 포함 **오프라인 계측**이며 **본체·Collider 물리 접지 PASS가 아님**. v3 CSV `QA/Props_9_Grounding_Audit_v3.csv`·`QA/Props_8_Rect_BeforeAfter_v3.csv`·Manifest를 정본으로 사용. **v2 Props9 Preview의 숫자 자막은 의도적으로 보존한 구 버전 수치이므로 재사용 금지**.
- Codex 보고 QA: `main` 대응 **2455 PASS**, 패키지 **191 PASS/FAIL0**, rect 정정 오류0, Road RGB/Alpha Seam0, ZIP CRC·Manifest·PNGDecode PASS, 기존 원본5,221개 SHA 및 dirty·HEAD 불변, Unity/Play/포커스/원본 PNG·GUID/meta/Scene/Quest/Save 변경0, Codex commit·Push·main병합0.
- **HOLD 유지:** Roof2 NPC·출입구·Camera 가림, Props9 실제 본체 접지, Optional11 개별 미술·배치 승인, Portal(-7,-4)/TreeSouthWest(-7,-4.2) 겹침. **미술 승인·Unity 적용·Runtime 승인 없음**.
- 정본 문서: [Arbel v3 Codex 문서 정정 인계](../../Arbel_Renewal_Art_v3_Metadata_Correction_Handoff_20261014.md). Codex 로컬 상세보고: `F:\study\codex\Project-Limitless\Temp\ArbelV3Metadata20261014\Arbel_v3_Metadata_Correction_Report_20261014.md` (이 세션 미열람). 문서 담당 브랜치 `docs/art-renewal`, main 직접 커밋/강제 Push/병합 금지.
- **현재 상태:** `CODEX_METADATA_QA_PASS / USER_ART_FINAL_APPROVAL_PENDING / UNITY_NOT_APPLIED / RUNTIME_NOT_APPROVED`. 다음: 바울 대표 v3 미술 승인 범위 결정 → Codex가 원본 물리 접지·Roof·Portal/선택장식 HOLD 재검토 → 승인 범위만 Unity 적용 후 Runtime QA.
