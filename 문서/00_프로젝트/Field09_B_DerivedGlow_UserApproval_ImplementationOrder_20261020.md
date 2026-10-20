# LIMITLESS / Field09 B 파생 Glow 최종 미술 승인 및 Codex 안전 적용 명령 (2026-10-20)

- 작성: 기획 `docs/planning`; 업무 이슈: [#38](https://github.com/HappyPaul812/Project-Limitless/issues/38).
- **바울 대표 사용자 명시 승인(2026-10-20): “승인”**. 직전 기획이 “Field09 B안(파생 Glow)을 최종 미술안으로 승인하시겠습니까? 승인해 주시면 기존 Glow 원본은 보존하고 B안만 안전하게 Unity에 적용하는 단계로 넘어가겠습니다.”라고 제시한 데 대한 응답. **B 파생 Glow 미술안 채택 및 별도 격리 구현·변경분 QA 착수 승인**으로 해석한다.
- 상태 `B_DERIVED_GLOW_USER_ART_APPROVED / B_IMPLEMENTATION_AUTHORIZED_IN_ISOLATED_CODEX_BRANCH / MAIN_MERGE_NOT_AUTHORIZED / ORIGINAL_USER_EDITOR_SAVE_PROTECTED / FULL_FIELD09_COMPLETION_PENDING`.
- 참고: Codex A/B 격리 QA `codex/field09-glow-composition-ab-20261020`, QA `68855edd`, 인계 `453736cb`. A SpriteMask 및 B 파생 Glow 모두 기존 붉은 띠·가이드 제거/내부 열기·조사 마커 유지/타 Renderer 간섭0 시각 PASS. 변경분34PASS/0FAIL, 과열 UI10PASS. **B는 마스크 오브젝트/범위 추가 없이 새로운 Glow Sprite 참조만 교체해 기술 관리가 단순한 이유로 채택**. 기존 Glow 내부 필요한 RGB/Alpha 약99.9% 보존은 Codex 보고값이며 성능 우월성 수치 아님; 성능 측정 별도0.
- 같은 대화의 신규 결정은 Codex가 로컬에 이미 만든 **B 후보**를 채택하는 것이지, 리뉴얼 담당에게 다른 Glow 디자인/Cliff/Edge v3를 다시 그리게 하라는 지시가 아니다.

## Codex 구현 범위 — 순서대로

1. **기존 QA 아티팩트·B 자산 검증/출처 고정**: 원격 최신 main, 실제 로컬 Codex `codex/field09-glow-composition-ab-20261020`의 선택된 B 파생 Glow PNG와 SHA256/Import meta/GUID/참조 경로·A/B 3 GameView 캡처, Scene 객체 `ObsidianScar_Glow` 원본 참조 SHA를 읽는다. B 후보가 저장돼 있지 않다면 이전 격리 결과/파생 생성 규칙을 **원본 불변 상태에서 동일 SHA로 재생성**하고 새 SHAs 및 동일물성 증거를 기록. 결코 임의 재색칠/전혀 다른 Glow로 바꾸고 이전 A/B PASS를 전용 QA로 유용하지 않다고 포장하지 말 것.
2. **별도 새 기능 브랜치·격리 worktree** `codex/field09-derived-glow-b-integration-20261020` 에 실제 B 파생 Glow를 독립 PNG + 신규 Meta/GUID로 import. **원래 공식** `ObsidianScar_Fissure_Glow.png` 및 `.meta`/GUID/SHA 바이트 그대로. `Field_09_ObsidianScar` Scene의 **Glow Renderer 하나** 또는 설정 정본에서 규정한 최소 참조만 파생 자산으로 교체한다. A SpriteMask 기능/Obj/Material/Shader 등 추가 금지, 불필요 전체 Scene 재생성 금지.
3. **기존 승인 5종 보존**: Sample3 Base Ground v1 SHA `1bd8a614988ee182640a71b9771646c3009545140e0522a3290ee3b0e3138203`, Cracked Overlay v2 `0147e6aff58e5304fd71e7474a9c018958a4234ec4c2dc2cf7382a905a7be9b7`, Obsidian Slab v1 `dfa3abb2c6c86abc3f94baa213428038a1c15b4c963e76fef69491a6deb09692`, Phase2 Cliff v2 `ad995f3cc4914759d6c88fa47c80227cb1a7c5f1a9a549679e06472402fee9f1`, Fissure Edge v2 `c3dc0c131c11fef72e4d210a1a43528f9882d8c50486e8da0bc15b6984ea145a`. 기존 상태가 여러 독립 샘플 ZIP과 QA worktree에 흩어져 있으면 정확한 참조/asset 위치 확인 후 **승인 5종+신규 B Glow를 동일 격리 Field09 Scene에 모은 후보**를 만들어야 한다. 그냥 Glow 하나만 기존 main 원본 Field09에 적용하고 Field09 Phase2 전체 아트가 완성됐다고 선언 금지. 각 승인 PNG 실제 SHA와 meta/GUID 충돌 및 참조 연결 확인.
4. **변경분만 QA**: 실제 Unity 3 GameView 화면비에서 (a) B Glow 새 asset 참조유효/분홍 MissingTexture0 (b) 위/오른쪽 붉은 띠·흰 가이드0 (c) 내부 열기/조사 마커 유지 (d) Cliff 접지와 Edge/Glow 정합 (e) 기존 Field09 이동 Collider/Scene 연결/퀘스트·Main18 과열 표지/Save·Continue를 깨뜨리지 않는 *최소 Delta* 검사. 기존 격리 A/B 34PASS + UI10PASS + Phase2 Cliff QA19PASS / 승인 Sample3 17PASS / Main18 92PASS를 **전수 재실행하지 말고 재사용 근거·새로 영향받은 항목만** 보고. 기술상 필요하면 정당한 신설 검사를 추가.
5. **사용자 PC 원본 보호**: 로컬 original main tracked dirty308/untracked548 등 사용자 변경(2026-10-19 시점 수치 — **현재 값임을 가정하지 말 것**)·Save/UserData·원본 Editor/Play 상태·공식 아트10/.meta GUID를 현재 SHA·Snapshot/백업 확인하고 **원본 local main에 무단 copy/checkout/clean/stash/restore/reset 또는 Unity Editor Play/Refresh 강제 조작하지 말 것**. 격리 작업공간 QA까지 이번 지시로 진행하고, 원본 사용자 열린 게임/디스크 직접 적용은 검증 보고 후 **별도 범위 결정**. GitHub `main` 직접 Commit/Push·자동 merge 금지.
6. **산출**: 실제 변경된 PNG+메타/참조 Scene 파일 및 Manifest SHA, 바울 대표가 확인한 B 결함 해결 GameView 전후 캡처, `git diff --stat`, 내부 새 Asset GUID/원본 Glow SHA 불일치 0, Delta QA(PASS/FAIL), 사용 브랜치/커밋/Push 여부, 원본 disk/local/remote main 적용 여부, 필요 PR/원본 디스크 최소 적용·병합 시 충돌 주의점을 보고. **격리 QA PASS ≠ 원본 게임 공식 통합/완성**.

## 여타 팀

- 리뉴얼 담당 `docs/art-renewal`: Cliff/Fissure/Glow 추가 아트 재작업 **불필요**, Phase2 다른 소품(AshPatch KEEP 후보, HeatVent/Spire 다음 묶음) 아직 **별도 아트 발주 대기**. 원본 Field09 전체 완료 선언 금지.
- Chapter3 메인 아트 `docs/art-new`: 승인 서리들쥐A/설빙의폭군B Idle 애니메이션 시트2 제작 진행 그대로, 본 Field09 Codex 작업과 충돌 금지.
- BGM 기존 두 샘플 및 후속 루프 제작 상황 유지, TTS/SFX/영상 신규 업무 바울 대표 현재 보류.
