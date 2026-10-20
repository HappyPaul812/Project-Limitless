# Field09 승인 B 파생 Glow — 격리 등록·저장 Scene 검증

2026-10-20. **격리 구현 PASS / 변경분39PASS·0FAIL / 사용자 원본 적용 보류 / Push·main 병합0**.

## 승인·기준

[Issue #38 승인 댓글](https://github.com/HappyPaul812/Project-Limitless/issues/38#issuecomment-6284873545), [기획 구현 명령](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/Field09_B_DerivedGlow_UserApproval_ImplementationOrder_20261020.md)(blob `64cd37d060516728975aaa447675663f3c4e2879`) 확인. 최신 GitHub main `12c69c72f9c09f716ee6242418b1d62f83a4a0d2`에서 **새 managed worktree**를 만들었다.

- 브랜치: `codex/field09-derived-glow-b-integration-20261020`
- worktree: `C:\Users\windows\.codex\worktrees\field09-derived-glow-b-integration\Project-Limitless`
- 승인5종 등록 commit: `77978aa11f43a9d04c9873f0ca914bd2bf5bd8ca`
- **B 등록·Scene 한 줄 교체 commit: `913eceb73fcbc440b49c19f71469380aeaec1168`**

선행 QA 브랜치에는 이전 실패 실험8종 Scene 참조가 있었으므로 그 구현 이력을 그대로 통합하지 않았다. 승인된 PNG6만 가져왔다. 기획의 과거 QA SHA `68855edd`는 실제 로컬 A/B QA `08855ead9b46bd14270211d7d47eafc4c63182f9`와 다르며, 실제 후보 SHA/바이트를 기준으로 확인했다. 기존 인계 `453736cb`는 일치한다.

## 실제 제품 변경 파일

공통 경로: `Unity/Client/Assets/_Project/Art/Environment/Chapter2/Field09ApprovedRedesign/`.

1. 승인 기준 등록: 아래5 PNG와 각 새 `.meta`, 폴더 `.meta`, `Field_09_ObsidianScar.unity`의 해당 Sprite 참조5개. **12파일**. PNG 픽셀·압축 바이트는 기존 승인 파일과 동일하다. 이전 QA의 임시 연결을 저장 Scene의 명시적 참조로 옮긴 것이며 아트 재제작이 아니다.
2. B 자체: **`LL_Field09_Fissure_Glow_Derived_B_v1.png` + 새 `.meta` + Scene의 Glow Sprite 참조 한 줄**, **3파일**. Scene은 두 커밋에서 공통이므로 main 대비 제품 변경은 **14개 고유 파일**이다.
3. C#·Shader·Material·Animator·ProjectSettings·기존 공식 PNG/meta 변경0. A SpriteMask/GameObject/설정 도입0.

| 오브젝트 / PNG | SHA256 |
|---|---|
| Ground / `LL_Field09_Redesign_Ground_Base_v1.png` | `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203` |
| GroundCrackedWest / `LL_Field09_Redesign_Ground_Cracked_Overlay_v2.png` | `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7` |
| Slab / `LL_Field09_Redesign_Obsidian_Slab_v1.png` | `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692` |
| Cliff / `LL_Field09_Phase2_Cliff_Blocker_v2.png` | `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1` |
| Fissure / `LL_Field09_Phase2_Fissure_Edge_v2.png` | `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a` |
| **Glow / `LL_Field09_Fissure_Glow_Derived_B_v1.png`** | **`64529ce564c05deda576f7bb151fa6426c22c006009b073c76a59d7b5775ab48`** |

B는 기존 QA 후보와 바이트가 같으며128,463bytes다. 새 디자인·재생성은 하지 않았다.

## 신규 Glow와 Scene 참조

- 신규 B GUID: **`1ba49ce409e5492db864e3d155fef75c`**. 기존 A/B 임시 후보 GUID와도 다른 신규 등록이다.
- 기존 공식 Glow GUID: **`a50d3e0a79def394685ca5e30b2f3c1b`**, PNG SHA256 `9cc2bf8ffeede089482a66accd1d5779192a2aa5bc14823b5f62c36a45d68824`. PNG/meta/GUID를 보호했다.
- Scene `Assets/_Project/Scenes/Field_09_ObsidianScar.unity`, `ObsidianScar_Glow` SpriteRenderer fileID **957851641**.

```diff
-  m_Sprite: {fileID: 21300000, guid: a50d3e0a79def394685ca5e30b2f3c1b, type: 3}
+  m_Sprite: {fileID: 21300000, guid: 1ba49ce409e5492db864e3d155fef75c, type: 3}
```

B 차이는 위1줄뿐이다. Position(-5,3.8), Scale(.525,.625), Sorting -7, MaskInteraction=None, 흰 Tint, Sprite-Lit-Default를 유지했다. Fissure Sorting -8도 그대로다. 신규6 GUID는 Assets 전체에서 각각1건으로 충돌0이다. 512×512 RGBA / PPU128 / 중앙 Pivot(256,256) / Point / NoMip / 비압축이다.

## Unity 변경분 검증 — PASS

기존 숨김 Batch 검수 Client `F:\study\codex\Project-Limitless\Temp\Field05Review20261019\Client`에 **이번 worktree의 저장 Scene/신규6 PNG/meta를 그대로 복사**했다. Scene SHA와 PNG SHA/각 GUID의 일치를 검증했다. 승인5종을 런타임에 교체해서만 통과한 검사가 아니다. 처음 Scene 로드와 Continue 후 재로드 모두 디스크에 저장된6개 참조를 확인했다.

**신규 Delta39PASS / 0FAIL**. 구성은 최초/Continue로드 참조6×2·MissingSprite각1, Scene로드3, Import1, Mask/Sorting1, 두 상태의 다른Renderer/Transform/Collider각3, 실제해상도6, Fissure/Cliff확대4, 열기표식확대1, 격리Save1·Continue위치/Quest1, 신규제품오류0 확인1이다.

| 항목 | 직접 확인 결과 |
|---|---|
| 실제 GameView | 1280×720(16:9), 960×720(4:3), 1680×720(21:9), 승인5+공식Glow 대조군과 저장B 후보 전후6장 |
| 붉은 띠·Glow 흰 가이드 | **3화면비 모두 제거 PASS** |
| 균열 내부 붉은 열기·조사 안내 | **유지 PASS** |
| Cliff 접지·Edge/B 정합 | **PASS** — 기존 승인 모습 유지 |
| Missing Sprite/분홍 텍스처 | **없음**, 저장 참조 로드/실화면 모두 확인 |
| Renderer/Material/Sorting | Glow Sprite 이외 변경 없음, Mask0 |
| Collider·이동 구조·Scene 연결 | 환경 Collider 전후 동일; Scene 텍스트 전체에서6 Sprite 참조 외 차이0. 이동/퀘스트/전환 코드·오브젝트 변화0 |
| 격리 Save/Continue | Audit 저장경로에서1회 저장→Bootstrap 슬롯1 이어하기→Field09 위치/Quest 스냅샷과 신규6참조 복원 PASS |

기존 **A/B34PASS·과열UI10PASS**, Phase2기술19·Sample3 17·Main18 92 결과를 재사용했다. 이번에는 전투 과열/전투/Quest 전주행·전체 회귀·물리 이동 입력 전수검사를 반복하지 않았다. 과열 위험 문구는 B의 RGB/알파가 승인 후보와 동일하고 Battle코드가 바뀌지 않아 기존UI10 증거를 재사용한다. 실제 열기 조사표식/HeatVent는 이번 화면에서도 확인했다.

Batch는6000.6.5f1(기존 사용자/격리 Editor 환경)로 정상 종료, 신규 컴파일·제품 Runtime 오류0. 기존 UnityEditor.Search 인덱스 진단은 원본 로그에 분리한다. 원격 main의 추적 ProjectVersion6000.5.7f1은 변경하지 않았으므로6000.5.7f1에서 새로 실행했다고 주장하지 않는다.

## 보호·최종 통합 주의점

사용자 원본 `F:\study\codex\Project-Limitless`의 Assets/_Project·UserData·Settings·Packages **3,546파일 SHA 변경0**, Git HEAD/status 동일. 원본 Editor Bootstrap / Play OFF / 비포커스 유지. 사용자 Save/UserData 읽기·덮어쓰기 대신 격리 Audit 경로를 사용했다. 공식 Glow와 기존 자산은 삭제/재압축/덮어쓰기하지 않았다.

main 기준에서 승인5종 등록이 필요했던 만큼 최종 통합 시 **77978aa1(승인기준)→913eceb7(B단일참조)** 순서와 전체14제품파일을 함께 검토해야 한다. B커밋만 적용하면 파일의 상위 폴더/meta와 승인5종 기준이 빠질 수 있다. 사용자의 미커밋 Scene 수정은 별도 비교 후 해당 Sprite 참조만 의미 병합하며 자동 덮어쓰지 않는다. 이 브랜치는 기존 실패 실험8종의 커밋을 포함하지 않는다.

**격리 등록/QA 완료 ≠ 사용자 PC 원본 적용 또는 GitHub main 통합.** 이번 미술 선택 승인은 완료됐으며, 원본 디스크 반영·Push·PR/main 병합은 추가 승인 전까지 보류한다. Field09의 다른 미교체 소품 전체 미술 완료를 선언하지 않는다.

[전후 GameView 모음](Evidence/Field09GlowBIntegration_20261020/index.html) · [39개 검사](Evidence/Field09GlowBIntegration_20261020/checks.txt) · [SHA/GUID Manifest](Evidence/Field09GlowBIntegration_20261020/Manifest.json) · [Scene 한 줄 참조](Evidence/Field09GlowBIntegration_20261020/GlowRef.json) · [원본 보호·동일 Scene 증거](Evidence/Field09GlowBIntegration_20261020/Protection.json) · [Unity 로그](Evidence/Field09GlowBIntegration_20261020/Unity.log.txt).
