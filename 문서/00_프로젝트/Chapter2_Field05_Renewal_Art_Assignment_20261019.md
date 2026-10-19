# LIMITLESS — Chapter2 Field05 기존 환경 아트 리뉴얼 1차 발주 (2026-10-19)

- 기획 담당, 브랜치 `docs/planning`
- 발주 시점 원격 `main` HEAD: `c263d9e36fc0570b16130d9c011abf3965def5f7`
- **담당:** Chapter 1·2 리뉴얼 아트 총괄(`docs/art-renewal`). **Unity 로컬·코드 통합:** Codex 별도.
- **상태:** `ART_ASSIGNMENT_APPROVED / SOURCE_AUDIT_AND_ORIGINAL_PNG_V1_REQUESTED / USER_ART_REVIEW_PENDING / UNITY_NOT_APPLIED`.
- 바울 대표의 2026-10-19 요청 “먼저 리뉴얼부터 일 시켜”에 따른 **최우선 신규 제작 대상**.

## 1. 우선순위 정정/보호 범위

현재 `main`에는 **Field04 승인 v2 Road42/Renderer375** 및 **Arbel v3 승인 기본23종·국소 지붕/처마 Depth 보완(2026-10-14 PR#12)**이 적용되어 있으므로 두 곳을 다시 그리거나 리그레션시켜선 안 된다. `docs/art-renewal`의 10월14일 오래된 인계 문서에 “Arbel 미승인/미적용”, “Field04 승인 대기”가 남아 있다면 **최신 `main`·PR·QA가 우선**. 아르벨 선택 장식11개는 **미승인 미배치**이고 Portal-나무 겹침은 별도 OPEN.

**다음 지역:** `Unity/Client/Assets/_Project/Scenes/Field_05_WesternOutskirts.unity`. Chapter2 Main14 「땅 아래의 울림」의 **메마른 수로·감시 초소·강해지는 땅의 진동**을 스토리 사건과 맞춰 표현. **Field06 이후 환경은 Field05 v1 미술 스타일 기준 정리 뒤 순차 착수**, Chapter3 신규 환경/몬스터/펫은 절대 제작하지 않음.

기존 Chapter1 Field03 Rock B v2 `LL_C1_F03_Rock_Moss_B_128_v2.png`은 2026-10-13 **기술·격리 실제 Unity 접지 QA PASS, 바울 대표 미술 승인 대기, 게임 원본 적용 없음**. Field05 새 제작과 섞지 말 것. B1 봉인문/Portal·선택장식 등도 별도 후속 범위.

## 2. Field05 GitHub 소스 기준

- `FieldThemes/Field05_DryOutskirts.asset`: `groundTint (0.65,0.52,0.33)`, `cameraColor (0.17,0.12,0.07)`, 기존 `TreeWest` / `BushWest` 템플릿. 이 값은 현재 코드 근거이지 사용자가 고정 승인한 새 미술 색상 자체는 아니다. 향후 교체 후보는 Field04→Arbel→Field05가 시각적으로 이어지게 디자인.
- 기존 `FieldConnections/Field05_EastToArbel.asset`: Field05 `east_to_arbel`, `Spawn_From_Arbel (8,0)`, 전환표식 `(10.7,0)`, 대상 Arbel/`Spawn_From_Field05`. 좌표/Collider/Exit/도보·휠체어 통로 보호. Arbel→Field05 귀환도 기존 정의 참조.
- 기존 `MonsterSpawns/Field05_SootHound.asset`: `field05_soot_hound`, `(3,-3)`, 활동반경1, 리스폰35초.
- 기존 `MonsterSpawns/Field05_FissureLizard.asset`: `field05_fissure_lizard`, `(-4,3)`, 활동반경1, 리스폰35초.
- Main14 진행 및 세린 재합류·기존 조사/초소/진동 문구, NPC/QuestTarget, 안전복귀 Arbel, 세린 공식 아트/전투·펫, Player·휠체어 접근성·Save/Continue 보존.
- `Field_05_WesternOutskirts.unity`는 실물 GitHub `main`에 존재. **환경 Renderer 총수, Sprite GUID/fileID/PPU/Pivot, 레이어·타일 매핑·정확 조사 지점은 추측해 숫자를 고정하지 말고 씬에서 실제 수집**. 필요하면 Codex에 사용자 PC 로컬 Scene **읽기전용** 대응표 감사 프롬프트를 제출.

## 3. 아트 지시 — 먼저 실물 Source Audit, 이어 v1 실제 제작

1. 최신 main(`AGENTS.md`, `CURRENT_STATUS.md`, `문서/00_프로젝트/팀운영/담당/WORLD_ART_RENEWAL.md`, `docs/art-renewal` 인계)과 Field04/Arbel 공식 미술을 확인. 기존 무료/공용 Sprite와 공식 승인 Sprite를 **교체 후보/보호/불확정**으로 분류. 기존 UID/GUID를 임의 재사용하거나 원본 수정하지 않음.
2. **시각 키워드:** 따뜻한 황토·올리브색 거점 뒤편의 더 마른 외곽, 낮은 물/바닥이 드러난 수로 흔적, 낡은 감시 표식·초소, 갈라지는 지면·미세한 진동의 징조. Field06 불길·Field08 붉은 균열·Field09 흑요석 수준을 **Field05에 앞당겨 표현하지 않는다**. 반복 Ground/Road/Cliff·원경과 전경 소품의 대비로 절제된 JRPG 픽셀 아트.
3. **절차:** (a) 기존 Renderer/그림 규격·교체 자산 매핑 및 기존 Portal/몬스터/Quest/Bounds 보호표 (b) Palette·스타일시트·실물 좌표 기반 비교 미리보기 (c) **실사용 오리지널 PNG v1** 제작 (d) Manifest/Alpha·Seam·접지·Sprite rect QA (e) ZIP 납품 및 실제 접근 가능한 다운로드 링크. 사용자 승인 전 기존 Unity Scene/Importer/WAV/Save/Settings 변경 금지.
4. 통합용 PNG(투명 RGBA 2D 환경)마다 실제 크기·PPU/Pivot·Source Role/Target Renderer·Sorting 및 반복 seam/alpha 접지 QA. `Manifest.json`, `Renderer_Mapping.csv`, `README.md`, `QA/...`, `Preview/...`, 제작/권리 선언, Codex 전용 인계. 신규 PNG 수량은 실물 Renderer 감사 뒤 정하며 Scene 숫자/ZIP/Download 경로 **지어내지 않는다**. PNG 없는 Preview/기획만 납품 완료로 표시 금지.
5. 이미 승인된 기존 공식 주요 몬스터·세린/폴/미엘/태온/하렌·BGM/TTS/VFX·펫 시스템은 변경 금지. 원본 Scene에 신규 문/길/Collider/Portal/몬스터 Spawn 추가 금지. Field05에서만 미술 교체 후보를 만든다.

## 4. 검수/협업 게이트 및 회계

- 첫 중간보고: **현재 Scene 감사표·교체 대상을 몇 종으로 판단하는지·스타일 시안**. 그 뒤 **사용자가 볼 수 있는 실제 v1 PNG/Preview/ZIP** 제작 단계로 진행. 이미지가 없으면 `ASSET_AUDIT` 또는 `ART_IN_PROGRESS`라고 정직히 보고.
- 최종 사용자의 **미술 승인** 뒤에만 Codex가 별도 feature worktree와 안전 격리 Unity로 Import/새 GUID/meta·Scene Sprite 참조 변경·접지/도보·휠체어/Monster Spawn/Quest14/SaveContinue 회귀. 현 로컬 `main`은 PR#16으로 로컬 문서2커밋이 원격 조상에 보존되었지만 **Dirty 464겹침, 서로 다른 내용5경로 때문에 로컬 동기화 PARTIAL**이므로 절대 무단 git clean/reset/checkout/merge로 사용자 작업을 덮어쓰지 말 것.
- 리뉴얼 담당 문서/인계 쓰기는 **`docs/art-renewal`만**, main 직접 변경 금지. 브랜치 커밋 SHA·QA/시각 승인 여부·다운로드 파일 링크·실제 Unity 반영 여부를 반드시 별도 보고. 새 공식 적용/Push/main 병합은 Codex 작업·사용자 승인 뒤.
- 기획 담당은 우선순위/범위 승인만, Codex는 로컬 감사/기술 검증만 수행. 세션 종료 전 최신 `WORLD_ART_RENEWAL_LATEST.md`에 배포물과 미완료 항목 정리.


## 2026-10-19 리뉴얼 v1 ZIP 수령 위치 전달·검수 대기

- 바울 대표가 **`F:\\Downloads\\Limitless_Field05_Renewal_Art_v1.zip`** 로컬 파일 경로를 기획 담당에게 전달. **파일을 이 기획 세션에서 실제 열거나 SHA·바이트를 검증한 것은 아님**. 실제 PC 파일 존재·bytes는 Codex 전용 격리 read-only 감사로 확인한다.
- `docs/art-renewal`의 최신 납품 `문서/00_프로젝트/Field05_Renewal_Art_v1_Delivery_20261019.md` 확인: 3,275,850 bytes, ZIP SHA256 **`a6646a676a619bc149798fe0f3909401b9c3b5a75f253cf6a1e61e21661b1258`** 보고; 전체35파일/PNG26, 실사용19=기존 Sprite 교체용12 (Ground3/Road2/Props7)+별도 신생 Renderer 필요 선택7, Preview·QA PNG7. `Renderer_Mapping_363.csv` 363/363(Grass315/Road30/Props18). 이 수치는 아트 담당 보고이며 **실제 원본 Unity에서 직접 확정하지 않음**.
- **다음 게이트:** Codex가 ZIP SHA/CRC/Manifest/ZIP Slip·Sprite Source/Scene 363행 실제 GUID/fileID/Transform/PPU/Pivot·RockNorth PPU100(85×54)/alpha-foot/물리 Collider·기존 Quest14 조사점/세린/Monster Spawn/Arbel 출구 실제 값 read-only 검증. 승인 **기존 교체후보12종만 별도 임시 worktree에 Unity Import**, 기존 GUID/meta 보호와 실제 16:9/4:3 Runtime 카메라 Before/After 비교, Main14·보행/휠체어·몬스터35초 리스폰·Save/Continue 회귀 후 바울 대표에게 시각 미술 승인 요청. **선택7종 신규 배치0** (아트의 별도 채택·좌표 승인 이후).
- **미술 승인 전 원본 Unity/Scene/Save/Git `main` 변경·Codex 브랜치 Push/PR 병합 금지**. 로컬 PR16 후 미해결 `dirty 464 overlap/5 conflict` 때문에 활성 Unity 원본 dirty/Save 보존 우선. 아트/영상/스토리 담당은 유저 미승인 v1을 공식 완료로 표기하지 않음.
- 검수 실패는 어떤 정확한 Source PNG/GUID/맵핑/시각 문제인지 12 교체종 별로 증거 캡처와 함께 리뉴얼 담당에게 수정 요청. Field03 Rock B, Arbel Optional11, Field04 기승인, Chapter3 신작은 별개.
