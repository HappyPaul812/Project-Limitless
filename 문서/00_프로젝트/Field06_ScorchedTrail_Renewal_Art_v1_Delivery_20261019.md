# LIMITLESS / Field06 Scorched Trail 리뉴얼 아트 v1 — 실제 PNG·ZIP 제작 납품

**제작일:** 2026-10-19 / **기준 main:** `675988c9f2c5c507d9a696b0378be27c0bd3309b` / **담당 브랜치:** `docs/art-renewal`  
**상태:** `ART_DELIVERED / OFFLINE_QA_PASS / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED / UNITY_RUNTIME_NOT_VERIFIED`.

기획 발주서 `docs/planning:문서/00_프로젝트/Chapter2_Field06_ScorchedTrail_Renewal_Art_Assignment_20261019.md`를 확인하고, 현행 [Field06 Scene](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scenes/Field_06_ScorchedTrail.unity), [Main15 Runtime](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Scripts/World/Chapter2Main15Flow.cs), Main15 Encounter Asset 네 개와 리뉴얼 담당 인계 정본을 읽었습니다.

## 1. Scene 조사 결과

- GitHub `Field_06_ScorchedTrail.unity`의 scene blob SHA `01c31f42fb1ff477f496193f14ca9c1e349ad7fc`, 직렬화 **SpriteRenderer 363개** = **Grass 315 / Road 30 / Props 18**. 기타 GameObject 포함 374개.
- Ground x=-10~10(21열), y=-7~7(15행). Grass 3원본 GUID `3ffe8024a48343dcbe3ee849067c02d3`: local sprite IDs `1606921373` 104개, `405149151` 105개, `1271049111` 106개. FlipX는 격자 홀짝마다 달라 정본 패턴을 CSV로 보존.
- RoadLeft15/Right15는 **여전히 기존 남북 배열**. Road PNG만 교체하면 사용자 요청 동서 진행로가 달성되지 않으므로 `A30`(15열×2행, x=-4..10) / `B42`(21열×2행, x=-10..10; 기존 대비 새 Renderer12개 필요) 선택 배치안을 **제안만** 포함. **A/B 선택·Scene 이동·추가 Renderer 생성 승인 없음**.
- Props18 = Fence8, Trees5, Bush3, Rocks2. 원본 RockNorth는 레거시 `rock_02` Multiple Sprite/85×54/PPU100이어서 동일 후보 Canvas/PPU100 제안하고 실제 Sprite foot·Collider 일치는 미검증.
- Main15 `BuildEnvironment()`이 만드는 `DrySoilEast`(4,-3), `OchreGround`(0,3), `CrackedGround`(-4,2), `CrackBranch`(-3.6,2.35), `ScorchedRock`(-7,-2), `DryGrass`(5,2)는 **SpriteRenderer가 아니라 MeshRenderer 달린 비충돌 Quad 6개**. 후보 PNG6을 납품해도 현재 Runtime 스크립트의 Quad가 자동으로 교체되지는 않음. 코드 변경 권한 없음.
- Main15는 x<2의 Grass 색을 조정하고 x<-3이며 `abs(round(y))`가 짝수인 Ground GameObject를 비활성화합니다. 사용자 승인 후 Runtime 시각 빈틈을 별도로 QA해야 하며, 제작 시 GameObject 로직은 변경하지 않음.
- 보호 위치: Field06 입장(7,0), 그을음(5,-2), 발자국(2,2), 진동(-1,-2), 균열(-4,2), 검게 변한 바위(-7,-2), 세린(-6,1). `Field06_M15_01` 조우 (0,-3), `_02` (0,3), `_03` (-5,0), `_04` (-7,3), 각 활동반경1/리스폰35초. Main15 12단계·정식 세린 동료·아르벨 귀환·휠체어/Save 기능 보호.

## 2. 제작 ZIP 및 실사용 이미지

- 파일명: **`Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip`**. ChatGPT에서 제작 후 바울 대표 다운로드용 파일로 제공. 사용자의 F드라이브에 직접 기록한 것은 아님.
- **ZIP SHA256:** `d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70` / **1,362,496 bytes** / **총 38파일 · PNG28**.
- 새로운 오리지널 **실사용 후보 PNG22종**:
  - Ground3 (기존 Grass315용 무한 반복 호환 타일)
  - Road4 (Upper/Lower 및 서쪽 End 2장; 본질적으로 동서 방향 그림)
  - Props7 (기존 18 Renderer에 대한 나무 크기2, 수풀2, 바위2, 울타리1)
  - **선택 Runtime 환경6** (현재 Main15 MeshRenderer Quad와 별도 Sprite 후보 — 코드 검토·별도 미술 승인 필수, 자동 적용0)
  - **선택 Decal2** (추가 배치 승인 전 미배치).
- 정적 대상 후보 합계 **14종**(Ground3/Road4/Props7), 추가 대상 **8종**(Runtime6/Decal2). Road4는 실제 좌표 재배치 결정 없이 Sprite-only 적용 금지.
- 나머지 PNG6은 오프라인 Preview5 + Seam QA 이미지1; 기존 Unity 게임 화면이나 RenderTexture 캡처가 아니라 실제 생성한 PNG로 합성한 미술 시안.
- **필수 파일:** `Manifest.json`(PNG22 SHA·Canvas·RGBA bbox·PPU/Pivot·용도), `Renderer_Mapping_363.csv`(static source name/GUID/localfileID/XY/sorting/flip/target), `Static_Grass_FlipX_Audit.csv`, `Runtime_Patch_Audit_6.csv`, `Road_EW_Placement_Proposals.csv`(A30+B42 총72행), `README.md`, `Rights_and_Provenance.md`, `QA/Offline_Validation_v1.json`, `Codex_Integration_Handoff.md`, `Codex_ReadOnly_SceneAudit_Prompt.md`.
- 미술 콘셉트는 **동쪽 황토·마른 풀/올리브 → 중앙 마른 황갈색 → 서쪽 검게 탄 돌·마른 고사목·미세 균열**. 용암·거대한 산불·새 서쪽 Portal·큰 불꽃·보스 거점 없음.

## 3. 실제 오프라인 검증과 보류

- `Renderer_Mapping_363.csv` **363행 / 이름 중복0**, 원본 Ground 참조104/105/106건, 기존 RoadLeft15/Right15, Props18 및 원본 Sorting/FlipX 대응 검토.
- `Ground3` 모든 조합 좌우/상하 및 Road 직선 반복·Upper/Lower 중앙·WestEnd→직선·Alpha 외곽 **27개 seam 조건 모두 픽셀 RGBA 불일치0**. `QA/Offline_Validation_v1.json` 및 6단계 반복도 이미지로 기록.
- ZIP `testzip()==None`, PNG28/28 decode, Manifest 실사용 PNG22/22 SHA256 일치, 전체 ZIP38파일 PASS.
- **이것은 미술 제작/오프라인 정합 QA이며 Unity 적용·Runtime QA가 아닙니다.** 기존 네 조우 실제 전투/퀘스트·Scene 전환/Save·Player/휠체어/카메라·Runtime 비활성 Ground 시각/물리 충돌/원본 Sprite foot 및 Sorting 검증은 **NOT_VERIFIED**.
- 원본 Scene/Prefab/C#/Main15/Collider/Quest/Save/MonsterSpawn·세린 스프라이트/TTS/Field04/Arbel/Field05 승인 자산, 사용자 미커밋 파일을 직접 변경한 것 **0**. ZIP에도 기존 승인 PNG나 .meta 파일을 덮어쓰기용으로 포함하지 않음.
- Codex는 현재 Main09 음성 버그 우선이므로 본 인계는 **읽기전용 검사 문서만 포함**, 실제 Codex 조사·Unity 작업 수행을 주장하지 않음.

## 4. 미술 승인 필요·다음 단계

1. 바울 대표가 `Preview/LL_C2_F06_Annotated_Overview_v1.png`, `Preview/LL_C2_F06_AssetContactSheet_v1.png`를 검토해 동서 황갈색→검게 탄 흔적 미술 방향 판단.
2. 기존 정적 대상 14종(특히 Root Scene 원본과 다르게 **남북 Road**가 있는 문제), 선택 Runtime6/Decal2 각각 사용 여부 및 A30/B42 분리 승인.
3. 사용자 미술 승인 후 Codex가 별도 `codex/*` worktree에서 현재 Source Sprite 규격·Collider/Monster 안전지대·Main15 실행 UI 등을 재측정하고 승인 범위만 Unity Import/배치 및 실제 QA.
4. **최종 사용자 미술 승인·Unity 정식 적용 및 main 병합은 이 문서 작성에 포함되지 않음.** Chapter3 신규 아트 제작 금지 및 기존 완료 Field04·Arbel·Field05 재제작 금지 유지.

**Git 운영:** 문서·인계는 담당 `docs/art-renewal`에만 커밋. `main` 직접 커밋·강제 Push·병합 없음.
