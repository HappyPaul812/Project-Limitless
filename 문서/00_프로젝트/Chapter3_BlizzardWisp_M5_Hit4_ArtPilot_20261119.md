# LIMITLESS / M5 눈보라 정령 Hit4 제작 및 오프라인 검증 — 2026-11-19

- Issue [#112](https://github.com/HappyPaul812/Project-Limitless/issues/112), [Attack4 직전 기술 검수](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-7003359918), [정령계 공통 기준](https://github.com/HappyPaul812/Project-Limitless/commit/98ef6141c5160bfa68cce516017c5938e21f0a46)
- 담당 `docs/art-new`. 이번 대화의 대표 직접 지시 "눈보라 정령 Attack4 최종 승인. 다음은 Hit4 제작"에 따라 Attack4 미술 승인 상태를 갱신. 기존 #112 승인 대기 표기는 과거 이력.
- 상태: `KEYPOSE_KEEP / IDLE8_V1_KEEP / IDLE8_V2_USER_APPROVED / ATTACK4_USER_APPROVED / HIT4_ART_CANDIDATE / USER_HIT4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / HIT4_UNITY_NOT_APPLIED`.

## 원본 보호
Library 실제 PNG 4종 회수 및 제작 전후 SHA256 일치, 원본 수정0.
- KeyPose: `0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`
- Idle8 v1: `28a8ae1d325617431ed3c2441467c235c323ce229fe91fa5190018ac8bde1cd7`
- Idle8 v2: `ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a`
- Attack4: `f8ca104d24c2826c4a757c5dd3ad80e30e29b222cbdcd6e38c4eaa4b051a9934`
기존 Codex Idle8 v2 로컬 시각 Prefab 적용 이력은 유지. 이번 작업에서 Windows 원본 프로젝트에 접근하거나 Unity 실행하지 않음.

## 신규 Hit4 한 시트
- `Sprite/BlizzardWisp_M5_Hit4_2048x512_RGBA_CANDIDATE.png` 2048×512 투명 RGBA, 가로4×512셀, 836,115bytes, SHA256 `2e07e2178f48bf0dcd6367a73a9f9c4af5311073df2dc2c9a7e33e139b311de1`.
- 승인 Idle8 v2 실제 위상 6/7/0/1(0-based) 기반. 00 피격 핵 반동 및 눈바람 흔들림, 01 국소 수축, 02 반동 회복, 03 대기 복귀. 프리멀티플라이 알파 역좌표 변형, 중심부 반동 및 눈바람 띠의 국소 반경/각도 변위. 전체 원화 단순 평면 회전/외부 자산/별도 VFX/붕괴 추가 없음. 공격 모션 프레임 복사 사용0. 마지막 셀은 승인 Idle8 v2 위상1과 전RGBA 완전 동일.
- 셀 4개 상이, 알파 경계0, 개별 QA PNG 4장 일치, 실제 alpha>0 bbox [38,9,456,471], [46,27,440,452], [41,8,472,462], [33,6,470,464] (반열림). 2D 변형 기반이므로 실제 피격 연출 가독성과 회전 연속성은 사용자 미술 검토 대상.
- 최종 시트에서 직접 생성한 512/128/40px 비교판, 간략 검토판, 128/40 투명 축소 스트립. 배경 합성·256색 양자화 비루프 검토 GIF 직접 디코드 4프레임 RGB 일치, 실제 시간 90/130/110/190ms = 520ms(비루프). Unity Animator 타이밍/게임 판정 계약 아님.

## ZIP 및 납품
- `BlizzardWisp_Issue112_Hit4.zip` 3,231,506bytes/13 entries, SHA256 `be090af54ba2e4699b72b4325e876d2248c79b2cb1a0ca6800bb890b34625a8c`.
- CRC PASS, Manifest 11/11 크기·SHA 및 전체 커버 PASS, SHA256SUMS 12/12 PASS, 자기·순환 참조 없음. SourceDeclaration_And_QA.json 포함.
- Library `/LIMITLESS/Issue112_BlizzardWisp_Hit4/` ZIP·시트·GIF·비교판2 실제 저장. Library ZIP 재회수 후 byte-exact, SHA256 및 CRC PASS. Windows PC 자동 복사/GitHub 바이너리 업로드0.

## 승인 게이트
**Hit4 미술 승인 필요 YES.** Defeat4는 Hit4 승인 전 미제작. 이번 Unity 적용/실행, 신규 스킬/전리품/수치/펫/전투 변경0. 기존 승인 자산, Unity6000.6.5f1, #85 HOLD, Save/UserData, Discord 보호. 불씨망령 보류. main 직접 commit/Push/Merge0.
