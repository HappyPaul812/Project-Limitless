# LIMITLESS — Field09 Phase2 Fissure/Glow 합성 시각 FAIL 기술 원인 및 격리 해법 비교 게이트 (2026-10-20)

- 업무: 기획 `docs/planning`; 기술 후보 검증: Codex 별도 `codex/*` worktree; 리뉴얼 담당 `docs/art-renewal` 신규 미술 제작 **대기**.
- 판정 근거: 2026-10-20 바울 대표가 제공한 Codex 실화면 QA 보고 스크린샷. 본 기획 세션이 실제 Unity 씬을 직접 재실행한 것은 아니다.
- **현재 확정 범위**: Phase2 v2 Cliff 하단 매트 제거·접지 **VISUAL PASS**, Fissure Edge 자체 외곽 매트 제거 **VISUAL PASS**, 기존 `ObsidianScar_Glow`와 결합한 화면만 **VISUAL FAIL**. Glow 자체의 원본 바이너리·현행 Scene/Renderer 설정 변경 없음. ZIP SHA/CRC/Manifest/PNG2 SHA **PASS**, 격리 Unity 변경분 **19PASS/0FAIL**, 기존 QA30/17/92 재사용, 원본 3,546개·승인3종·Glow/GUID/Save/UserData/Editor 보호. Codex `codex/field09-phase2-fix-v2-qa-20261020`, QA commit `16c1fb92`, 인계 `fe0b412d`. ZIP `F:\Downloads\Limitless_Field09_Redesign_Phase2_Pilot_Fix_v2.zip`, 담당 기존 SHA `0e4b7bfb3784b5b97a4a719c669f9564f502d67e88c2096d32475fa69a6876c8`.
- **실화면 현상**: Fissure 위쪽/오른쪽으로 붉은 띠 및 기존 가이드 잔흔 보임. **근본 원인**은 Edge/Glow 알파 형상 불일치 + 동일 Material, Mask 없는 현행 합성; `ObsidianScar_Glow` sorting -7이 Edge -8 위에서 그려지며 Glow 픽셀이 Edge 투명부 외측까지 보임. **Sorting order만 반전하면 전부 해결된다고 주장 금지**.
- 상태: `CLIFF_V2_VISUAL_PASS / FISSURE_EDGE_V2_STANDALONE_VISUAL_PASS / GLOW_COMPOSITION_VISUAL_FAIL / TECHNICAL_RENDER_PROTOTYPE_RECOMMENDED / ORIGINAL_SCENE_UNCHANGED / MAIN_UNMERGED`.

## 다음 결정: 같은 PNG를 계속 그리게 하지 않는다

1. **기존 미술 승인 보호**: Sample3 Base Ground v1 `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`; Cracked Overlay v2 `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`; Slab v1 `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`. Phase2 Cliff v2 `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1`; Fissure Edge v2 `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a`. **동일 바이트 유지**, Cliff/Edge 다시 제작 금지. 기존 공식 `ObsidianScar_Fissure_Glow.png` 및 `.meta/GUID`도 바이트 보호.
2. **Codex 기술 선택지 두 후보의 격리 비교**만 착수 (원본 적용 허가가 아니라 별도 실험 허가 전제):
   - **A: Glow 전용 SpriteMask**. Fissure 투명영역·실제 발광 영역에 적합한 별도 mask asset 또는 procedural editor-time mask candidate 생성. `SpriteRenderer.maskInteraction=VisibleInsideMask`, mask sorting-range 및 앞뒤 투명 sorting을 격리씬에서만 적용. 엉뚱한 소품까지 마스킹/발광 깜빡임·강한 Glow 삭제 없어야 한다.
   - **B: Edge에 맞춘 **별도 파생 Glow sprite**. 공식 Glow 원본을 직접 편집하지 않고 새 filename/GUID의 파생 후보를 만들어 Alpha를 Edge/균열 실제 geometry와 정합하되 사용자가 알아야 하는 과열·조사 시각 신호와 빛의 가장자리 표현을 유지한다. 원본 Glow/Renderer의 직접 변경 없음; 독립 Candidate reference만 격리씬에 사용. 이후 채택하면 원본 asset overwrite가 아닌 새 asset 역할로 제한.
   - 비교 실패하면 추가 대안으로 기능 보전이 가능한 국소 shader/Material clip (새 이름/Asset) 검토만 하되 빠르게 성공 가능한 해법 우선, 과도한 구조 개편 금지.
   - **Sorter 단순 역전만으로 해결했다는 식의 임시 가림, 불투명 검정 판, Glow 제거/과열 피드백 약화, 새 그림 전체 재제작 금지.**
3. 각 후보 동일 조건 **원본 상태 및 A/B** 총3개 GameView 화면비, 1:1 확대 bbox, `Fissure/Glow` -8/-7 원래 결합 위치, 빛 새기·Guide 잔여0, 과열 경고 식별 가능성, 퀘스트 조사 마커·충돌/동선 불변을 스크린샷으로 확인. 앵커·Sorting·Renderer/Mask 설정 및 이미지(후보만) SHA, 파생 자산 licensing/alpha/crop 설명, 변경 파일 수, 런타임 비용 등 보고. **후보 중 가장 미술적으로 자연스럽고 구현이 최소인 하나를 기술 권고**.
4. **이 게이트는 격리 실험 결과 및 추천 보고까지**: 바울 대표가 화면을 확인해 승인하고 나서야 **별도 정식 Unity 적용**과 추가 아트 묶음 생산을 결정. 기존 승인 3종, Phase2 두 종, 공식 Glow 원본 Scene/Save/UserData/Original Editor/Main은 그대로; Push/Main direct commit/merge 없음. Read-only baseline 및 SHA 보호·격리 branch.
5. Art-renewal은 보류: 새로운 Fissure/Cliff v3, Glow 재디자인, 다른 소품 양산 **즉시 불필요**. Codex 실험이 어떤 추가 실제 그림을 요구하는지 확인하기 전 임의 제작하지 않는다.

## 팀 상태
- Chapter3 메인 아트 서리들쥐A/설빙의 폭군B Idle2 시트 작업은 별도 `docs/art-new`에서 진행해도 됨. 기존 BGM 승인/후속 루프는 별도. TTS/SFX/영상은 기존 대기 지시 유지.
