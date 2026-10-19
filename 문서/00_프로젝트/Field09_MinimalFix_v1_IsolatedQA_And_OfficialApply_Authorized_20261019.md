# LIMITLESS / Field09 공식 아트 최소 보완 v1 — 격리 검수 및 조건부 Unity 원본 적용 승인 (2026-10-19)

- **바울 대표의 최신 지시:** “Field09: 별도 검수 및 Unity 적용하자.”
- 기획 담당 브랜치: `docs/planning`, 담당 분리: 리뉴얼 `docs/art-renewal`(이미 제작·납품), Codex `codex/field09-minimal-fix-unity-integration-20261019`(통합·검수·조건부 원본 적용).
- **상태:** `USER_INTEGRATION_AND_QA_AUTHORIZED / ZIP_DELIVERED / ART_OFFLINE_QA_PASS_REPORTED / UNITY_ISOLATED_QA_PENDING / ORIGINAL_DISK_APPLY_IF_PASS / ORIGINAL_EDITOR_USER_PLAY_PENDING / GIT_MAIN_MERGE_NOT_AUTHORIZED`.
- **권한 해석:** 사용자는 Field09 보완본을 대상으로 실제 Unity 검수 및 **그 결과가 합격하면 안전한 원본 디스크 적용**을 명시 허가했다. 이는 8개 PNG의 실제 미술 품질을 이미 보았다는 증거, 검수 불합격 시 무조건 적용하라는 허가, 기존 official PNG 원본 overwrite, Git Push/PR/main 병합 허가가 아니다.

## 1. 인수 ZIP 및 정본

- 리뉴얼 납품 정본: `docs/art-renewal/문서/00_프로젝트/Field09_ObsidianScar_OfficialArt_MinimalFix_v1_Delivery_20261019.md`.
- 실제 GitHub 파일: `docs/art-renewal/Art_Deliveries/Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip`.
- 사용자 이전 전달 경로 `F:\Downloads\Limitless_Field09_ObsidianScar_Minimal_Fix_v1.zip`도 후보로 검토하되, **실제 로컬 경로가 없거나 SHA가 다르면 GitHub 정본 ZIP을 별도로 취득**. 사용자 경로로부터 파일 존재/동일성을 미리 단정하지 않는다.
- 납품 보고 ZIP SHA256 `7dfa2417a8135e29e3e72c7d7921d3ade76fbedfdb376f764c09b02892296139`; **2,995,422B / 13 entries / PNG 8종 / 각512×512 RGBA**. ZIP 내 `Manifest.json`, `QA/Original_Source_SHA256_and_Delta.csv`, `QA/Offline_Validation.json`, `README.md`, `Codex_Integration_HOLD.md`, `Fixed_Candidates/`를 읽고 원본/후보 SHA·CRC·PNG Dimensions·RGBA·Importer 권장값을 검증.
- 공식 원본 Source: `Unity/Client/Assets/_Project/Resources/Main18/Environment/`에 PNG10종(활성9). `ObsidianScar_AshPatch_01.png`, 미사용 `ObsidianScar_Ground_Cracked_02.png`는 byte/meta/GUID 불변. 수정 대상은 GroundBase/GroundCracked01(지면2) + Cliff_Blocker/Fissure_Edge/Fissure_Glow/HeatVent/Obsidian_Slab/Obsidian_Spire(가이드 6).

## 2. 작업 시작 전 철저한 안전/차이 감사

1. `AGENTS.md`, `CURRENT_STATUS.md`, 공통규칙/인계, Field09 기존 `Field09_ObsidianScar_ExistingArt_MinimalReplacement_Audit_20261019.md`, 사용자 승인/납품 문서, Codex Field09 화면결함 격리 리포트부터 읽기.
2. 원본 로컬 `main`, 활성 Unity Editor Play 여부, dirty/tracked/untracked, UserData23, Save, Settings, 기존 Main09 27/Main10 18 승인 음성, Field06/07 B42 및 최신 local 적용분/원격 branch 기준 SHA 스냅샷. **현재 GitHub main과 원본 local 사이에 아직 미병합된 Field06/07·Main10 적용분이 있으므로 main을 무작정 reset/clean/stash/overwrite하지 않는다.** Git `codex/*` worktree만 사용.
3. ZIP/Manifest/CSV에 적힌 *원본 SHA*를 현재 Source PNG와 대조. 불일치·원본손상·GUID 메타 충돌·후보 PNG 내용 불명은 **STOP/보고**, 임의로 8개를 덮어쓰지 않는다.

## 3. 통합 방식 — 기존 공식 PNG 자체 보호

- 원칙적으로 현재 `Resources/Main18/Environment`의 기존 공식 8개 PNG **bytes 및 .meta/GUID는 유지**한다. 별도 버전 폴더(예: `_Project/Art/Environment/Chapter2/ObsidianScarMinimalFix_v1/`)에 승인 후보 PNG8를 새 Asset으로 Import, 새 GUID.meta를 확보하고, **실제 Scene의 지정 SpriteRenderer8개만** 새 Sprite로 참조하게 격리 장면에서 교체한다.
- 이 방식을 사용하는 경우 Main18 Runtime의 `Resources.Load`/Sprite GUID/Atlas/Prefab/SpriteName 등을 grep/기존 Scene 참조에서 확인하여 runtime이 원본 경로를 요구하는지 점검한다. 추가 중복 Renderer/Collider 생성 금지, 기준 원본 Scene/meta와 승인 수정 외 변경 diff0.
- 만약 runtime과 사양 때문에 같은 경로/기존 GUID가 필수라면 **원본 byte 보존·백업 및 승인 디스크 경계 검증 없이는 교체 금지**. 안전한 새 GUID 연결/단일 파일-byte diff 중 실제로 가장 적은 변경 경로를 선택하되 기존 원본에 대한 추적 가능한 rollback 필수.
- Sprite 크기512 RGBA / PPU128 / Pivot Center / Filter Point / no Mip / Sorting·Scale·Transform·BoxCollider·Bounds 고정. `ObsidianScar_Ground` Tiled21×15, `GroundCrackedWest` Tiled9×15 위치(-6,0), 기존 나머지6개 Object 좌표와 Collider 변경0.

## 4. 격리 Unity 실제 QA — 미술 + 게임 회귀

- 검수 대상 8개 후보 각각 원본과 새 소재의 동일 카메라, 동일 Zoom/Layer 조건 비교, **1920×1080 / 1600×900 / 1280×720** Game View/캡처 최소세트. 화면이 편집용 합성 Preview가 아니라 **실 Unity 렌더링**임을 명시.
- GroundBase 자체의 반복경계, GroundCracked01 자체 반복경계, **Base와 CrackedWest 사이 이종 불투명 Overlay x≈-1.5**의 수직 seam을 따로 판정. 오프라인 outer-edge seam PASS만으로 cross-material seam PASS 선언 금지. 눈에 보이는 반복선이 여전히 남는다면 **VISUAL_QA_FAIL**, 무리한 Renderer Tint/추가 바닥덮개 임시 패치로 PASS 처리 금지.
- Cliff_Blocker, FissureEdge, FissureGlow, HeatVent, ObsidianSlab, ObsidianSpire **6종 각 잔여 흰 점선·하얀 테두리** 확인. 자동으로 지운 픽셀이 의도된 바위/빛/열기 실루엣을 훼손했는지 확인; 과열·틈새 표식 가독성 유지.
- Field08↔Field09 입출구, Field09 서쪽 Main18 잠금 안내/후속 Field10 전환 정의, Main18 13목표의 조사점(ground/vent/monster/과열/감시자/흔적), 세린·몬스터, 0~2 과열/세번째 HP8% 피해·냉각약, Collider/휠체어·도보 이동, Save/Continue 및 키보드/게임패드 기본 상호작용을 **변경 영향 범위 내 delta QA**. 기존 Main18 전체 500PASS/0FAIL 전수 재실행 금지.
- 기존 환경 Search/URP 에러 원문과 신규 Console 오류 분리, Editor/user live Play·OS 포커스 강제변경 금지; 격리 백그라운드 테스트 우선.

## 5. 검증 게이트 → 원본 적용 → 보고

- **QA 0 FAIL이며 시각적 문제8개가 실제로 개선된 경우에만** 정식 원본 디스크 안전 적용을 허가. 원본 working tree 변경/미추적, UserData/Save, 메타 GUID, 원래 PNG 및 다른 승인 자산을 적용 전후 SHA로 확인. 원본 활성 Editor를 강제 Import/Play/Focus 하지 않고 추가되는 코드/Scene+새 PNG/meta만 명시적 변경, 롤백 경로 제공.
- 하나라도 신규 미술 결함·입출구/퀘스트 회귀·SHA 충돌이 발견되면 **ORIGINAL_APPLY_BLOCKED**. 기존 통과 결과 숨기지 말고 사례별 실패 화면, Asset/Renderer/좌표, 더 필요한 최소 pixel/art 재작업 또는 Sprite Renderer 설정 보완을 별도 보고. 기존 승인 PNG 원본으로 임의 revert해 합격한 듯 보고 금지.
- 별도 보고: `ZIP_SHA_MATCH`, `8_ASSET_DELTA`, `CROSS_MATERIAL_SEAM_PASS`, `GUID_AND_ORIGINAL_ASSET_PROTECTED`, `MAIN18_RUNTIME_DELTA_PASS`, `ORIGINAL_DISK_APPLIED`, `USER_ORIGINAL_EDITOR_NOT_VERIFIED`, `REMOTE_MAIN_NOT_MERGED`, 기능/QA branch SHA, 적용 전후 backup & original SHA, 보호 파일 수, 대표 미술 확인 필요사항.
- 수행 문서 `CURRENT_STATUS` 및 Codex 인계, 기존 main 직접 Commit/Push/PR/merge0. 다음 몬스터·펫 아트 파일럿 수정/반려와 Field09 기술 통합은 분리한다.
