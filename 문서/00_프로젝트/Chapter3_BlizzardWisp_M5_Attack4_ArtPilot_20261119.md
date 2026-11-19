# LIMITLESS / M5 눈보라 정령 Attack4 제작 및 오프라인 검증 — 2026-11-19

- Issue [#112](https://github.com/HappyPaul812/Project-Limitless/issues/112), [Idle8 v2 승인·Attack4 발주](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-7002458727), [정령계 공통 기준](https://github.com/HappyPaul812/Project-Limitless/commit/98ef6141c5160bfa68cce516017c5938e21f0a46)
- 담당: `docs/art-new`
- 상태: `KEYPOSE_KEEP / IDLE8_V1_KEEP / IDLE8_V2_USER_APPROVED / ATTACK4_ART_CANDIDATE / USER_ATTACK4_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / ATTACK4_UNITY_NOT_APPLIED`

## 원본 보호 및 기존 승인
- 실제 기존 KeyPose SHA256 `0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`, Idle8 v2 PNG `ba6a7bddcd37bdf7fe78939862e93a50828561a08564756572d0bba19f51692a` 일치. KeyPose, v1/v2 시트 및 기존 v1/v2 ZIP 5개 모두 작업 전후 SHA 동일. 원본 수정0.
- Idle8 v2는 [Codex 이전 적용 기록](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-7002165110) 기준 Unity6000.6.5f1 8셀/0.96초 클립/Controller/시각 Prefab 로컬 적용 및 격리 검증 PASS, 정식 MonsterDefinition/조우/F4·F5 실전 미검증, REMOTE_NOT_PUSHED. 본 작업은 Attack4 신규 미술만 수행.

## 신규 시트 한 장 및 검토물
- `Sprite/BlizzardWisp_M5_Attack4_2048x512_RGBA_CANDIDATE.png`: 2048×512 RGBA, 4셀 각512², 788,615bytes. SHA256 `f8ca104d24c2826c4a757c5dd3ad80e30e29b222cbdcd6e38c4eaa4b051a9934`.
- 응축 → 우향 눈바람 방출 → 잔류 흐름 → 대기 복귀. 승인 v2의 실제 0/2/4/6번 위상을 기본으로 국소 압축·전방 눈바람띠의 행별 좌표 변형 및 잔류 표현, 마지막 셀은 v2의 0-based 6번 위상 그대로. 전체 원화 평면 회전 및 별도 VFX/외부 게임 자산 추가0. 앞서 생성한 별도 시안은 최종 납품에 미사용. 2D 변형 기반으로 공격의 폭발감 및 회전 연속성은 사용자 미술 검토 대상.
- 4셀 상이, 경계 alpha0, 개별 QA 프레임 전RGBA 일치. bbox(반열림) [69,38,427,441], [54,17,490,452], [38,16,491,459], [40,10,483,466]. 실제 최종 프레임에서 전체512 비교판·축소 128/40·간략 비교판 직접 생성.
- 실제 최종 프레임 배경 합성/색상 양자화로 만든 비루프 GIF 재개봉 4프레임 RGB 일치, 실제 지연 200/120/180/320ms 합계820ms, loop 확장 없음. Unity Animator 타이밍/판정 계약 아님.

## 실제 ZIP 및 승인
- `BlizzardWisp_Issue112_Attack4.zip` 2,896,857bytes, 13 entries, SHA256 `a3f919971890686617e8335e65749f1c05dc71394a5b3f11d9234b8b589d792f`. CRC PASS / Manifest payload11/11 크기·SHA PASS / SHA256SUMS12/12 PASS / 비순환. SourceDeclaration_And_QA.json 포함.
- Library `/LIMITLESS/Issue112_BlizzardWisp_Attack4/` ZIP·시트·GIF·비교판2 실제 저장 성공. 사용자 PC 복사 및 GitHub 이미지 바이너리 등록 없음.
- **Attack4 사용자 미술 승인 필요 YES.** Hit4/Defeat4는 승인 후 순차 발주. 신규 Unity 적용/실행, 스킬/판정/VFX/스탯/드롭/펫 작업0. 기존 Idle8 적용 기록은 보존. 승인 자산/#85 HOLD/Save/UserData/Discord/Unity6000.6.5f1 보호. 불씨망령 HOLD. main 직접 commit/Push/Merge0.
