# LIMITLESS / Chapter2 Field06 v1 — 사용자 미술 방향 승인과 최소 기술 보완 지시 (2026-10-19)

> 기획 담당, `docs/planning`. **2026-10-19 바울 대표가 “리뉴얼도 보고서로 봤는데 좋아.”라고 실제 Unity 격리 검수 보고를 보고 발언. 이 발언은 Field06 v1의 전체 미술 방향 수락으로 처리하되 기술 오류 수정·원본 디스크 적용·PR/main 병합 완료 선언은 하지 않는다.**

## 상태·보고 근거

- `FIELD06_V1_USER_VISUAL_DIRECTION_APPROVED / TECHNICAL_FIX_REQUIRED / ZIP_CHECK_PASS_REPORTED / ISOLATED_UNITY_QA_717_PASS_0_FAIL / OFFICIAL_UNITY_NOT_APPLIED / USER_MAIN_MERGE_NOT_AUTHORIZED`.
- 리뉴얼 `docs/art-renewal/문서/00_프로젝트/Field06_ScorchedTrail_Renewal_Art_v1_Delivery_20261019.md`: `F:\Downloads\Limitless_Field06_ScorchedTrail_Renewal_Art_v1.zip` (대표가 로컬 다운로드 경로 제시); 리뉴얼 보고 SHA256 `d01f797dd5566c33c6bf917a397c2032f5fc44fcf31a53dc328807e912a23b70`, 1,362,496 bytes, ZIP38/PNG28(정적 후보14·선택 Runtime6+Decal2).
- Codex가 바울 대표에게 제공한 2026-10-19 Field06 완료 보고: `codex/field06-renewal-art-v1-review-20261019`, 기능/검수 커밋 `1de59ea47821bcd057a62c738de42af02ed4cd5a` (대표 캡처 기록), ZIP SHA/CRC/Manifest/PNG PASS, 격리 Unity **717 PASS / 0 FAIL**, 실제 Unity 비교 20장. **미술 수정 필요: 도로 상하 배치 대응 오류, 중앙 Alpha 틈, Runtime Grass 49개 비활성화, 일부 소품 접지 차이**. 선택 Runtime6/Decal2 미배치. 원본 Field06 미적용; 원본 Editor live Play/Main15 전체 사용자 손플레이 미검증. 소스 보고서의 개별 이미지별 판정은 아직 이 기획 세션에서 직접 열람하지 않았으므로 QA 수치/세부 원인을 임의 확대하지 않는다.
- 동일 Codex 보고 Main09: 승인 WAV27 새 음성 경로에 보존·StoryVoiceCatalog 연결·순서 수정 유지·기존550PASS 재사용/추가261PASS0FAIL·원본 디스크 안전 반영 완료. TTS/API0, Push/main merge0. **Main09 재작업/중복검증 금지**. Codex Main09 브랜치 `codex/main09-dialogue-voice-fix-20261019`.
- 사용자 Codex 리소스: 캡처상 5시간 잔여 **8%**, 1주 잔여 **70%**. **새 전체 QA 및 수십 장 영상 재촬영 금지**. 일단 재개용 정확한 작은 수정 계획을 기록하고 필요시 5시간 한도 초기화 이후 실행. 기획/신규 아트 담당 등 병렬 작업 가능한 업무는 Codex 한도에 묶지 않는다.

## 승인 범위 정확히

- **승인:** Field06 동쪽 올리브·황토 → 중앙 건조·그을음 → 서쪽 균열과 검은 바위, 기존 아트 Palette·스타일/기본 자산 v1, 동서 진행로 의도 및 실제 격리 비교의 전체 미술 방향. 승인된 정적 후보14는 **시각 스타일 변경 없이** 재사용하되 기술 오류만 바로잡는다.
- **미확정:** A30/B42 중 실제 최종 좌표 배치. **Field05 B42와 같은 일관된 동서 진행로를 우선** 비교·제안하나 이번 발언만으로 Field06 B42의 좌표/Collider 변경을 자동 승인하지 않는다. 기술 오류 고친 후보 중 기존 Main15 동선과 일치하며 메인 진행 방향이 뚜렷한 최소 변경안을 제안한다. 시각적 차이가 없는 기술 수정에 반복 승인 요청하지 않되, A/B 선택이 실질적 이동 동선·화면 배치 변경을 수반하면 최종안 명확히 보고한다.
- **미승인/보류:** 6개 Runtime Quad 대체와 추가 Decal2, 새로운 길/Portal, 대규모 필드 전체 재구성, 다른 Field 아트 재작업. 신규 8종은 `OPTIONAL_NOT_APPLIED`를 유지한다.

## Codex 다음 단계 — 기존 717 QA 결과 재사용, 실패 원인 4곳만 고치기

1. **사전 보호:** 최신 원격 main/원본 local 동기화 상태와 dirty·untracked·Save·UserData 및 이미 공식 반영된 Main09 27 WAV/Catalog SHA 스냅샷. 활성 Editor/Play/포커스 조작 금지. Field06 아트 ZIP 원본 SHA 고정; 신규 수정이 꼭 필요할 때는 파생 v1.1 후보로 별도 저장, 기존 승인 ZIP overwrite 금지. Codex 전용 worktree `codex/field06-renewal-art-techfix-20261019` 사용 권장.
2. **Road 위/아래 대응:** 실제 `Field_06_ScorchedTrail.unity`의 Road30와 `Road_EW_Placement_Proposals.csv`의 A30/B42 및 `Manifest`의 Upper/Lower/WestEnd 임포트·좌표·Flip·Sorting 대응을 **개별로 재산출**하여 잘못 연결된 행만 수정. 2행·시점·끝타일 이미지가 방향과 맞는지 Scene 실 렌더링으로 확인. 이동 시 기존 콜라이더와 Quest/NPC/Spawn/Exit 방해 금지.
3. **중앙 Alpha 틈:** 오프라인 seam 27 PASS는 개별 PNG 접합 검사이지 실제 월드 배치/Runtime Tint 검사가 아니다. 실제 틈 좌표·렌더러 이름·원인(아트 가장자리 투명, 잘못된 겹침, Sorting/PPU/Pivot, Runtime Grass 비활성) 구분 후 **국소 수정**. 겹침을 가짜 덮개 Quad로 임시 은폐하거나 Scene 전체 보정·무단 Collider 추가 금지.
4. **Runtime Grass 49 비활성:** `Chapter2Main15Flow.BuildEnvironment()`의 `x < -3 && Mathf.Abs(Mathf.RoundToInt(y)) % 2 == 0` 비활성 규칙이 시각적 구멍을 만드는지 확인. 필요하면 새 Ground 실사용 유지 + 기존 마른 풀/그을림 분위기만 보존하는 최소 스크립트 변경을 **격리** 검토; Story/전투/Save/Quest 변화0. 원본 Grass/Tint 장식이 의도적으로 빠지는 것과 바닥이 투명해지는 것을 구분. `49`은 보고된 원인 대상 수이지 무조건 모든 비활성화 코드를 삭제하라는 뜻이 아니다.
5. **Props 접지:** 해당 나무/바위/수풀의 실제 source sprite rect, PPU/Pivot/alpha foot, sprite position·Sorting·기존 Collider와 후보 PNG 캔버스 비교, 바닥에 뜨거나 묻히는 **대상만** 보정. 원래 GameObject의 물리 Collider/상호작용 좌표를 이미지에 맞춰 임의 이전 금지.
6. **기술 후속 QA:** 이미 717 PASS는 재실행 금지. 신규 변경분의 Sprite/GUID/mapping/diff와 실 렌더 변경점만 검사하며, 도보·휠체어 동쪽 Arbel 입출구/중앙길/서쪽 조사 5곳·세린·Encounter4 스폰·Save/Continue·Main15 12단계 보호를 우선한 소규모 delta 증빙 제출. 이전 Search/URP 환경 문제와 새로운 런타임 오류 분리. Actual Unity 16:9 기준 수정 전후 주요 위치 최소 화면(전체+문제부)으로 충분. User live original Editor 기능 결과를 검증한 것처럼 말하지 않는다.
7. **인계·게이트:** `TECH_FIX_PASS / USER_ART_DIRECTION_APPROVED / ROAD_LAYOUT_SELECTED_OR_PENDING / OFFICIAL_UNITY_NOT_APPLIED`를 별도로 기록. 대표에게 추가 디자인 재승인을 요구하지 말고 **실제로 수정한 범위와 선택할 Road 안만 요약 보고**. 미술 방향 승인 후라도 최종 정식 적용/원격 PR/main 병합을 유효성·원본 보호 없이 선행하지 않는다. 원본 디스크 반영은 기술 수정과 격리 QA 후 별도 승인 범위로 안전 적용.

## 우선순위

Main09 오디오 27개 청취 승인 및 원본 디스크 연결은 Codex 보고상 완료, **원격 feature Push/PR/main 병합은 별도**. Field06 4항목 작은 수정→최소 delta QA→표준 Git 인계. 그 후 Field07 환경 신규 아트 착수(리뉴얼 담당, Codex와 분리); 필드06을 반복 재제작하지 않는다.

**기획만 GitHub docs/planning에 남김.** Codex를 원격 명령으로 직접 실행한 것이 아님. 이 파일을 그대로 다음 Codex 세션에 전달할 수 있다.

## 역할 분리 후속 (같은 날)

**리뉴얼 아트 담당**이 Field06 v1.1의 PNG·Alpha 틈·소품의 시각적 접지·Road 위아래 텍스처/네이밍 등 실제 그림/Manifest/Mapping 수정을 책임진다. 이전 v1 ZIP은 그대로 보존하고 고쳐야 할 PNG만 새 영문 ZIP에 납품; 49개 Ground 비활성화 문제는 시각적 빈틈의 실측 자료만 인계한다. **Codex**는 원본 Unity Scene·Importer/Transform/Runtime Grass 49비활성화 코드의 근본 원인과 QA를 처리하며, 아트 이미지를 새로 그리거나 기존 PNG를 임의 리터칭하지 않는다. 필요하면 실제 Unity Renderer별 오류 좌표/GUID/foot 판정을 리뉴얼 담당에 넘겨 최소 PNG 수정을 받는다. 각 팀 업무와 승인 게이트는 독립한다.

Field07 신규 아트 v1은 `문서/00_프로젝트/Chapter2_Field07_AshenReach_Renewal_Art_Assignment_20261019.md` 발주서에 따라 **리뉴얼 담당이 Field06 v1.1과 병렬 시작 가능**. Field06 Code/Unity QA 및 Main09 feature 통합을 기다리느라 신규 art 작업 전체를 멈추지 않는다.
