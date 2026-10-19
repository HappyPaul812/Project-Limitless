# LIMITLESS Field06 ScorchedTrail v1.1 — 사용자 승인 미술의 최소 기술 보완 납품

- 날짜: 2026-10-19
- 기준 GitHub main: `675988c9f2c5c507d9a696b0378be27c0bd3309b` (문서 조회 시점)
- 기획 근거: `docs/planning/문서/00_프로젝트/Field06_Art_v1_UserVisualApproval_TechnicalFix_20261019.md`
- 담당: Chapter1·2 리뉴얼 아트, 문서 브랜치 `docs/art-renewal`.
- **상태:** `USER_V1_VISUAL_DIRECTION_APPROVED / ART_V1_1_DELIVERED / OFFLINE_ART_DELTA_QA_PASS / ROAD_A_B_UNDECIDED / UNITY_NOT_APPLIED / RUNTIME_NOT_VERIFIED`.

## 실물 v1.1 ZIP

- 실제 이 대화에서 생성된 ZIP: `Limitless_Field06_ScorchedTrail_Renewal_Art_v1_1.zip`.
- SHA256 **`d8551df7315495dcc6e4556ec33f30b711af8aeb85dc7b0a1c3612a5a6197f42`**
- **1,399,079 bytes, 41개 파일, PNG29개**: 실사용 후보 PNG22개(도로4개 수정+기존18개 정확히 바이트 보존), 미리보기/QA PNG7개(기존6+새 비교1).
- 보호 v1 원본 ZIP: `Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip`, SHA256 `d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70`. v1 ZIP 변경0.

## 실제 미술 수정과 이유

기존 도로 PNG의 Upper 그림은 Canvas 위쪽에만, Lower 그림은 아래쪽에만 놓여 있었습니다. 그러나 실제 Upper 타일은 y=+0.5(위 행), Lower 타일은 y=-0.5(아래 행)이므로, 두 타일 사이 **중앙이 투명하게 갈라졌습니다**. v1의 기존 오프라인 최외곽 seam0은 두 곳이 모두 투명하여 오류를 놓쳤을 수 있습니다.

- **변경 PNG 딱 4종:** `Road/LL_C2_F06_Road_EW_Upper_128_v1_1.png`, `...Lower_128_v1_1.png`, `...WestEnd_Upper_128_v1_1.png`, `...WestEnd_Lower_128_v1_1.png`.
- 위/아래 그림의 실제 Canvas 위치를 재대응하고, 중앙 타일 경계의 RGBA 한 행씩을 맞춰 빈틈을 보완했습니다. 128×128, PPU128, CenterPivot, Point/No Mips, Z0/Flipfalse 원칙.
- **새 PNG는 v1 황토·그을음 소재를 재사용한 최소 픽셀 이동/접합**이며 Ground3/Props7/Runtime선택6/Decal선택2의 기존 18종 PNG는 전부 v1 ZIP과 바이트 동일합니다. 이들 PNG는 단독 미술 보완 대상으로 재제작하지 않았습니다.
- `Renderer_Mapping_363.csv`의 원본 363 이름/좌표/GUID/fileID 참조 보존, **기존 Road30 대상에는 반드시 A30/B42 배치 승인 뒤 매핑해야 한다는 HOLD**를 명시했습니다. `Road_EW_Placement_Proposals.csv`는 후보 72행에 실제 v1.1 파일명을 반영했지만 A/B 확정이나 원본 Scene 이동으로 간주하지 않습니다.
- `QA/Props_Foot_Candidate_v1_1.csv`에 새 후보 PNG의 Alpha bbox 및 수치형 foot proxy만 측정하여 보관. 원본 Source Sprite / Collider 본체 접지와 같다고 주장하지 않으며, 정확히 문제가 입증된 소품만 Codex 현장 조사 후 별도 최소 수정 대상으로 삼습니다.
- **Runtime Grass 49개 비활성화는 Codex 코드/Scene 담당**이고 이 납품은 해결하지 않습니다. 선택 Runtime/Decal 8종도 미배치 상태 유지.

## 기술 검증 구분

- 오프라인 ZIP CRC PASS, PNG29 해독 PASS, Manifest 실사용 PNG22 SHA일치.
- `QA/Technical_Delta_v1_1.json`: 새 Upper/Lower 중앙 128픽셀의 동일 RGBA 접합(투명 아닌 도로), WestEnd 중앙 동일; Upper 반복, Lower 반복, WestEnd→직선 연결 각각 RGBA mismatch **0**(총 6조건).
- 비교: `Preview/LL_C2_F06_Road_CenterGap_BeforeAfter_v1_1.png`은 기존/수정 **PNG 합성**이지 Unity RenderTexture가 아닙니다.
- 기존 Codex v1 격리 `717 PASS / 0 FAIL`은 **이전 보고의 역사적 결과**이며 v1.1에서 재실행한 것이 아닙니다. v1.1 Unity Import/실제 중앙 이음/49 Grass/출입구/휠체어/Main15 Quest12/Encounter4/Save/Screen QA는 **NOT_VERIFIED**.
- Field06 v1 전체 미술 방향 승인과 v1.1 기술 수정 아트 QA PASS는 공식 디스크 적용 승인·Git main 병합 권한이 아닙니다.
- 보호 대상: Field04·Arbel·Field05 공식 아트, 세린·Main15, 몬스터·BGM/TTS, Collider/Save/Unity Scene/GUID/meta/사용자 dirty 모두 변경0.

## 다음 단계

바울 대표에게 v1.1 PNG와 전후 Preview를 직접 다운로드 제공 → Codex가 현재 Main09 작업/한도 상황을 보호하며 소규모 Road/Grass/접지 Delta QA 수행 → A/B 이동의 별도 사용자 결론 및 격리 Runtime 검증 → 정식 적용 여부 사용자 결정.

이 문서만 `docs/art-renewal`에 기록. main 직접 Commit/Push/병합 없음.
