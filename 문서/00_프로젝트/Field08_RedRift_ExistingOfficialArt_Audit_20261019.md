# LIMITLESS / Field08 RedRift 기존 Main17 공식 환경 아트 감사 및 최소 보완 판단 — 2026-10-19

**담당 브랜치:** `docs/art-renewal` / **감사 기준 main:** `675988c9f2c5c507d9a696b0378be27c0bd3309b`.
**판정:** `OFFICIAL_ART_PROTECTED / EXISTING_SCENE_AUDITED / CONFIRMED_NEW_PNG_REQUIRED_0 / FIELD07_TO_FIELD08_SIDE_BY_SIDE_PENDING / NO_NEW_ART_AUTHORIZED / UNITY_UNCHANGED`.
**사용자 최신 승인:** Field06 v1.1과 Field07 v1의 **미술**은 바울 대표가 승인(대화 전달). 이 사실을 임의로 **원격 main 통합/실제 Runtime 통과**로 승격하지 않는다.

## 1. 확인한 정본

- `Unity/Client/Assets/_Project/Scenes/Field_08_RedRift.unity`, **Git blob SHA `6100dde3685e26c85b06fd2d4f06995fb6d582ee`** — GitHub `fetch_blob`로 실제 YAML 전체 **1,099,645자**를 읽어 GameObject / Renderer / 활성 상태·GUID를 직접 대조.
- `문서/00_프로젝트/Main17_Field08_Art_Applied.md`, `Main17_Field08_Art_Handoff.md`, `Main17_Art_Source_Audit.json`, `Main17_ArtAudio_QA.md`, `MAIN17_FINAL_QA.md`, `Main17_Field08_Geometry.txt`, `Main17_Field08_ActiveSpritePlacement.txt` 확인.
- 공식 현재 파일 디렉터리: `Unity/Client/Assets/_Project/Art/Environment/Chapter2/RedRift/`의 9개 원본 PNG·Manifest·5개 공식 Material 및 원본 `.meta`. 원본 ZIP `Limitless_RedRift_Environment_Assets_English.zip` SHA256 `a238bc2eca0450b95fb5a61cd8cc655018bff82e54096fa91cfb2f7f424e19fb` **기존 문서 보고값**. ZIP 자체는 본 세션에서 열지 않음.
- 연결 자산 `Field07_WestToField08.asset`, `Field08_EastToField07.asset`, `Field08_WestToField09.asset`과 최신 `Field_07_AshenReach.unity` Scene blob `2f267575bfbd1d7802b58c8653c2b33d7a84d6ac` 확인. Main17 기존 조사 문서의 '서쪽 차단'은 그 당시 퀘스트 동선 조건이며, 현재 서쪽→Field09 연결 정의도 있으므로 Main17 조건을 영구적인 물리·기능 삭제 허가로 해석하지 않음.

## 2. 실제 Scene 정적 구성 및 표시 여부

GitHub YAML 직렬화 기준: **GameObject 428개, SpriteRenderer 375개, MeshRenderer 45개, MeshFilter 44개, Collider 가진 GameObject 15개**. 전부 추가·이동·변경0.

| 항목 | 직렬화 수량 | 실제 활성 상태(씬 YAML 기준) | 해석 |
|---|---:|---:|---|
| `AshSoil_` | 315 SpriteRenderer | GameObject 비활성315 | 구 지면 구조 보존, 표시되지 않음 |
| `RoadLeft_/RoadRight_` | 30 SpriteRenderer | GameObject 비활성30 | 과거 세로 Road, 현재 환경 시각 아님 |
| `BrokenTrack_` | 10 SpriteRenderer | GameObject 비활성10 | 공식 트랙 표현 방식으로 대체 |
| `Tree*` | 5 SpriteRenderer | GameObject 비활성5 | 기존 숲 식생 비표시 |
| `AshCrack_` | 8 SpriteRenderer | GameObject 활성 / SpriteRenderer.enabled=0 | 과거 타일 확대 도형, 비표시 |
| 공식 `RockWest/RockNorth` | 2 SpriteRenderer | 활성2 | 공식 RedRift Obsidian PNG ref |
| 공식 `CanyonLip_0/1` | 2 SpriteRenderer | 활성2 | 공식 Canyon PNG ref, collider 보호 |
| 공식 `RedRiftArt_DeadShrub0~2` | 3 SpriteRenderer | 활성3 | 공식 DeadShrub PNG ref |
| `RedRiftGeometry_Ash0..23` | 24 MeshRenderer | 표시24 | 공식 Ash Material 적용 |
| `RedRiftGeometry_Obsidian0..2` | 3 MeshRenderer | 표시3 | 공식 Obsidian Material 적용 |
| `RedRiftGeometry_Ground/DeepCrack/InnerGlow` | 3 MeshRenderer | 표시3 | 공식 Ground/Fissure/Glow |
| `RedRiftGeometry_Branch0..5/Track0..7` | 14 MeshRenderer | MeshRenderer.enabled=0 | 기존 단색/임시 선·발자국은 비표시 |
| `RedRiftTitle` | 1 MeshRenderer | MeshRenderer.enabled=0 | 환경 아트 교체 대상 아님 |

**합계: 실제 표시중 SpriteRenderer 7, MeshRenderer 30 = 정적 활성 37; 비표시 SpriteRenderer 368 (= inactive GameObject360 + Renderer.enabled=0 8), 비표시 MeshRenderer 15**. 이는 YAML 상태이며 실제 런타임 동적 효과·퀘스트 오브젝트 총수와 동일하지 않다.

## 3. 공식 PNG·실제 적용 분류

문서 `Main17_Field08_Art_Applied.md`는 **실행용 PNG9종 등록, 실제 사용7종, 불투명 지면 변형2종 의도적 UNUSED**를 기록. 실제 현행 Scene Material / Sprite GUID·Importer 일치 확인.

| 공식 PNG (모두 기존 승인 원본 보호) | 실제 대상/사용 | 분류 |
|---|---|---|
| `RedRift_Ground_Base.png` | `Ground.mat`→Ground Mesh(21×15) | 적용 |
| `RedRift_Fissure_Edge.png` | `Fissure.mat`→DeepCrack Mesh / 기존 Collider(0.7×7) | 적용 |
| `RedRift_Fissure_Glow.png` | `Glow.mat`→InnerGlow Mesh | 적용 |
| `RedRift_AshPatch_01.png` | `Ash.mat`→Ash24 Mesh | 적용 |
| `RedRift_Obsidian_01.png` | `Obsidian.mat`→Obsidian3 Mesh / RockWest·RockNorth2 Sprite | 적용 |
| `RedRift_Canyon_Edge_01.png` | CanyonLip2 Sprite, 기존 Collider 보존 | 적용 |
| `RedRift_DeadShrub_01.png` | RedRiftArt_DeadShrub3 Sprite | 적용 |
| `RedRift_Ground_Cracked_01.png` | 공식 보존만, 현재 활성 참조 없음 | 의도적 UNUSED |
| `RedRift_Ground_Cracked_02.png` | 공식 보존만, 현재 활성 참조 없음 | 의도적 UNUSED |

- 원본 PNG 규격은 전부 **1254×1254**, 공식 Importer `Sprite Single, PPU128, Center, Point, Uncompressed`, 원본 크기 및 byte 유지. RGB/Alpha와 Mirror/Clamp 구분은 정본 Importer 기준. `RedRift_AssetSheet_Preview.png`는 원본 패키지에는 있지만 프로젝트 Runtime 등록 대상이 아니며 미술 QA 참고 이미지. **128×128 타일을 새로 요구하거나 원본을 Resize/Crop하지 않는다.**
- Scene 직렬화 GUID 실제 대조: `RockWest/RockNorth` `8bb909a04e73ca345aad785264524fd8` (Obsidian), `CanyonLip` `a36472f61bf1d8647b4c2c8ce965382d`, `DeadShrub` `c30a4d7dfc8e5114e8346853a48cd620`. Mesh Material `Ground c0fcf570...`, `Fissure 4a884042...`, `Glow d64c8402...`, `Ash 76942650...`, `Obsidian 1032662f...` 모두 각 공식 `_MainTex` GUID가 정본 파일 .meta와 연결된 상태를 YAML로 확인.
- 44개의 과거 Quad 구조는 공식 텍스처를 활용한 활성 Mesh30과 disabled Mesh14로 나뉘며, 활성 Mesh에 **임시 단색만 남아 있다**는 증거가 아니다. 과거 15행 남북 Road·풀·균열 텍스처가 저장소에 남아 있다는 이유만으로 교체 필요하다고 결론짓지 않음.

## 4. Main17 기존 Unity 실제 검증과 한계

- `Main17_ArtAudio_QA.md`: **고유 PASS270 / FAIL0**, 기존 44개 전투 승리 등은 별도 재사용으로 계산. 실제 Field07↔08, Main17 12목표, Story/일반 전투, 저장/Continue, DeepCrack 차단·북남 우회, Source PNG/Manifest/Import 보호, Transform425·Collider15 비의도 변경0 근거 포함.
- 공식 Runtime QA에서 **1920×1080 / 1600×900 / 1280×720** HUD 포함 화면의 Player·세린·적·목표 가독성, 표시 빈 영역·외곽 사각 seam 없음 판정. 이것은 2026-10-06 당시 보고 결과이지, 2026-10-19 **Field07 승인 v1 신규 아트와 나란히 놓은 후속 캡처 검증이 아님**.
- GitHub 최신 main의 `Field_07_AshenReach.unity` blob은 이전 Field07 v1 납품 감사 시 `2f267575...`와 동일. 바울 대표의 최근 **Field07 v1 미술 승인**을 실제 `main` Sprite 교체·동서 Road Import/Runtime 완료로 자동 판단하지 않는다.

## 5. '꼭 필요한 부분만' 후보 분류

| 우선 | 위치/항목 | 현재 증거 | 필요한 다음 행동 | 신규 PNG 승인 필요? |
|---|---|---|---|---|
| **P0 / 신규 제작 확정0** | 기존 RedRift Ground·DeepCrack·Glow·Ash·Obsidian·Canyon·DeadShrub | 공식 7종 실제 active ref 확인 / 기존 3해상도·물리 QA PASS | **그대로 보호** | **아니요** |
| **P1 / 확인만 필요** | **Field07 서쪽(-8,0) ↔ Field08 동쪽(+7,0)** 연결 색감·지면 경계 | Field07 v1 사용자 미술 승인, GitHub main Field07 Sprite 교체 상태는 과거 본체 유지; 최신 승인 시안과 동일카메라 후속 비교 없음 | Field07 승인분 격리 적용 이후 Codex가 동·서 입구 좌우 왕복 16:9/기타 해상도 1~2장 실제 캡처. 색/패턴 급변이 명확히 보일 때만 *Field08 공식 PNG 재작업이 아닌* 한정형 비충돌 경계 Decal 또는 기존 Material Tint 조정 **별도 사용자 범위 승인 요청** | **현재 아니요; 실물 실패 때만 별도 신규 제작안 재제출** |
| **P2 / 승인 자료 유지** | 남북형 옛 Road30·비활성 기존 Grass315/Tree5/Track10/AshCrack8 | 모두 GameObject 비활성 또는 Renderer.disabled | 실물 가시화 부활/삭제 금지, Scene 정리 작업 자동 추진 금지 | **아니요** |
| **P2 / 의도된 미사용** | `RedRift_Ground_Cracked_01/02` | 불투명 사각 접합 문제로 미사용 결정 | 원본 보호, 단독 섞기/자동 적용 금지 | **아니요** |
| **범위 밖 / 기능 보호** | 서쪽 Field09 연결 정의·깊은 틈 북/남 우회·Main17 및 후속 Quest/Scene 동선 | `Field08_WestToField09.asset` 존재. Main17 당시 '서쪽 차단'은 당시 진행 제약 | Portal/Exit/Collider/Quest/Save 변경 금지 | **아니요** |

**판정:** 새 아트가 반드시 필요하다는 명백한 픽셀·Renderer·접합 결함 근거 **0건**. 제안 신규 PNG **0종**, 제작 파일/ZIP **0**, 기존 자산 덮어쓰기 **0**. 실물 비교로 추가 수요가 입증되기 전까지는 **Field08 신규 제작 착수 금지**.

## 6. 적용/승인/수정 이력

- 본 작업은 GitHub `main`의 코드·Scene·공식 PNG·Material·Importer·GUID/meta 파일을 **읽기 전용**으로 조사. 이미지 생성·Unity Editor/Play 실행·로컬 F: ZIP 접근·실제 RenderTexture 새 캡처 **0**.
- 문서만 `docs/art-renewal` 브랜치에 기록. `main` 직접 커밋/Push/병합/강제 Push·원본 Scene/Save/사용자 미커밋 파일 수정0.
- **사용자 판단 요청:** 현 단계에서 Field08 추가 PNG 제작은 **불필요**하다는 감사 결론 승인 여부. 필요하다면 추후 Field07↔Field08 격리 비교 결과를 본 뒤 한정 보완 범위를 별도 발주.
