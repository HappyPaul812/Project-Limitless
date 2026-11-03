# F2 자연형 그루터기 v4 — 바울 대표 미술 승인 반영 / 2026-11-03

**USER_APPROVED / APPROVED_ART_CANDIDATE / UNITY_DELTA_PENDING / NOT_UNITY_APPLIED / LOCAL_COMMITTED / REMOTE_NOT_PUSHED**

## 승인 정본

바울 대표의 이번 채팅 명시 승인 “바울 대표가 F2 자연형 그루터기 후보를 승인했습니다”를 근거로 기존 KEEP_CANDIDATE/시각승인 대기를 **USER_APPROVED**로 변경합니다. 2026-11-03 Issue #81 기획 댓글의 승인 대기는 이 직접 승인으로 갱신됩니다. 후보 파일의 이름·바이트·GUID를 바꾸거나 새 승인 파일을 만들지 않습니다.

- 파일: `F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png`
- SHA256: `22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8`
- 크기/형식: 기존 검수320×384 RGBA. 이번에는 해당 SHA 재확인만 수행하고 PNG·Unity QA를 반복하지 않았습니다.
- 실제 승인 원본: `F:/study/codex/Project-Limitless/Temp/Issue81ArtDeltaFollowup20261103/Extracted/F2/Candidate/F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png`
- 수령 ZIP: `F:/Downloads/Limitless_F2_Stump_Natural_Issue81_20261103.zip`, 기존 검수 SHA `890013d4885319e1dc8a5a7f4ac2771c485b7ed2664617882cf535e6b48b70b5`. ZIP 무결성·미술 검수 재실행0.
- 이전 실제 검수: [후속 델타 검수](Issue81_NaturalStump_SpiderBee_ActualDelta_Review_20261103.md), 검수commit `cae0d8efebbfbf12bec51e15dd7ea8746f03bacc`, 영수증 `c56cf53e12f9d5aa8e1765bef85529cba8e3bcfd`.

**미술 승인 대상은 위 PNG1개뿐**입니다. 공식 승인 후보로 선택됐지만 제품 Asset 등록·Sprite 교체·Scene 배치·Unity 적용 완료 상태는 아닙니다.

## 예전 그루터기 후보의 현재 상태

|후보|현재 상태|정리 방식|
|---|---|---|
|`F2_SnowStumpPerch_v1_CANDIDATE.png`|SUPERSEDED / ARCHIVED_CANDIDATE|초기 후보·과거 검수 이력으로만 보존|
|`F2_SnowStumpPerch_v2_CANDIDATE.png`|SUPERSEDED / ARCHIVED_CANDIDATE|원판 단면 국소수정 과거 후보 보존|
|`F2_SnowStumpPerch_v3_CANDIDATE.png` (SHA `eb32bf200da3f08023f0f83b32df9a2f75b9357fa8ffc1f2647b116a5f5b60b8`)|SUPERSEDED / ARCHIVED_CANDIDATE|가구형/원판 HOLD 이력 보존, 신규 적용 대상 제외|
|`F2_SnowStumpPerch_FULLREDRAW_REFERENCE.png` (SHA `251fad7f5c6271f4f22dab6eeb4389e75864e4ec11380888b267b8892a435af3`)|SUPERSEDED / ARCHIVED_CANDIDATE|이전 전면 대안 HOLD 이력 보존, 신규 적용 대상 제외|

문서상 분류만 변경합니다. 실물 파일 이동/삭제/덮어쓰기0, 과거 HOLD 관찰은 당시 결과로 보존합니다. 예전 후보가 USER_APPROVED가 된 것은 아닙니다. 후속 적용 준비는 승인v4의 정확한 SHA만 사용합니다.

## 승인에 포함되지 않은 항목

- Sign FULLREDRAW SHA `864e964ee4cc3868bd315af2249fe92d04e888937ae66f963232636cda8e4488`: 기존 **KEEP_CANDIDATE / USER_APPROVAL_PENDING**, 이번 승인에 포함되지 않습니다.
- Fence FULLREDRAW SHA `49c2969b8607d91b6bf15cb0e4485bcc3f25379669064b025d63b099c9c62e8c`: 기존 **KEEP_CANDIDATE / USER_APPROVAL_PENDING**, 이번 승인에 포함되지 않습니다.
- Organic Spider v4: **REVISE / ALPHA_FINAL_HOLD**, 미승인. 기존 디자인 방향 채택과 최종 알파 승인은 구분합니다. 메카닉v3는 Chapter4 ARCHIVE_ONLY 유지.
- Bee 새5: **REVISE / MOTION_REVISE_CONTINUE / FULL_COMBAT_READY_NO / RIGHTS_HOLD**, 미승인.
- Owl Attack·기존 환경/기능/상품 승인 상태 변경0.

## 다음 Unity 최소 Delta 준비 — 이번에는 실행하지 않음

**다음 단계에 격리 Unity 최소 Delta 검증은 필요합니다. 현재는 준비 문서만 완료했습니다.**

1. 다음 별도 검증 작업에서 승인PNG SHA를 다시 확인하고, 실제 기존 F2 구조·프로젝트 생성기·최신 Editor 상태를 읽습니다. #80 기준 F2 실Scene/전투는 미구현이므로 이번 미술 승인을 F2 맵·전투 신규 구현 허가로 확대하지 않습니다.
2. 승인1PNG만 격리 QA용 Asset으로 가져올 준비를 합니다. PPU/Pivot/Footprint/Sorting은 **SPEC_PENDING**이며 기존 월드 아트 계약과 실제 배치 목적을 확인해 검증 범위를 정합니다. 이번 문서에서 임의 수치나 GUID를 발급하지 않습니다.
3. 격리된 최소 표시/통행 검증 범위에서3화면비 실제 게임 크기·128/40 외형·알파 경계·바닥 접지/눈판 이음·수피/주변 후보와 비례·Player/카메라 시야 가림을 확인합니다. 실제 길이 존재하는 검증 범위에서만 도보·휠체어 통행과 Collider/Bounds를 확인합니다. 해당 구조가 없으면 NOT_VERIFIED로 남기며 통행 PASS를 만들어내지 않습니다.
4. 승인하지 않은 표지판/울타리/Spider/Bee를 제품으로 함께 가져오지 않습니다. 문서 비교용 미승인 이미지를 보더라도 검증 후보/미승인 표시를 유지합니다.
5. 백그라운드·포커스 없는 검증을 우선하고 결과/SHA/변경 경로를 보고합니다. Main24 #80 81/14PASS·기존 Main23·저장85/72/72 전수 반복0. 실제 본게임 적용은 별도 요청/검증 후의 단계입니다.

현재 Collider/휠체어 통행/PPU/Pivot/Sorting/게임 실비율은 **NOT_VERIFIED**, Unity Import/Scene/Prefab/BuildSettings/Packages/ProjectVersion 변경0, 개인 Save/UserData 열람·복사·해시·수정0. 원본main/사용자dirty·기존Main24 브랜치·승인PNG 바이트 보존, Push/merge/원본 Editor 실행0.

## 변경 문서와 범위

- 본 승인 정본1개 신규.
- `팀운영/인계/CODEX_LATEST.md`, `팀운영/인계/SPRITE_ART_LATEST.md`: 최신 승인·승인 제외 범위·다음 검증 연결.
- 이전3ZIP/후속2ZIP 검수보고 상단: 현재 승인 상태를 연결하고 본문은 과거 검수 이력으로 유지.

브랜치 `codex/f2-natural-stump-approval-20261103`, 출발점 `c56cf53e12f9d5aa8e1765bef85529cba8e3bcfd`. 이번 delta는 Markdown5파일뿐입니다. 로컬 인계 문서는 이 브랜치에서만 반영하고 원격 docs/art-new 파일은 Push 없이 변경하지 않습니다. GitHub에는 Issue #75/#81 승인 결과 댓글을 실제 게시합니다.
