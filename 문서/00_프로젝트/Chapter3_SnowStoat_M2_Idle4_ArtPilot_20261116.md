# LIMITLESS / 메인 신규 아트 / Chapter3 M2 설원족제비 Idle4 파일럿 (2026-11-16)

**담당:** `docs/art-new` / **Issue:** [#100](https://github.com/HappyPaul812/Project-Limitless/issues/100)

**판정:** `KEYPOSE_USER_APPROVED / IDLE4_ART_CANDIDATE_DELIVERED / PNG_DIMENSIONS_PASS / ZIP_CRC_SHA_PASS / USER_IDLE_ART_REVIEW_PENDING / UNITY_NOT_TESTED / NOT_ORIGINAL_APPLIED`.

## 1. 대표 승인과 범위

- 바울 대표의 **“족제비 보니까 좋구만...”**을 2026-11-16 신규 지시문에 따라 **설원족제비 1포즈 사용자 시각 승인**으로 기록. 이것은 **Idle4 자체 승인이나 Unity/게임 적용 승인과 다름**.
- 이전 [#100 1포즈 원화 보고](Chapter3_SnowStoat_M2_OnePose_ArtPilot_20261116.md) 기준 승인 `SnowStoat_M2_KeyPose_512_RGBA_CANDIDATE.png` SHA256 **`fd25ef50528a5f630e44382e3fc40a3df607a267176f758706962a2cbc41e778`**.
- 이번 제작 대상은 **Idle4 4프레임 가로시트 1종만**; Attack/Hit/Defeat/Run/펫 동작 및 스탯·ID·스폰·전리품은 0.

## 2. 실제 납품 PNG/애니메이션 구성

- `Sprite/SnowStoat_M2_Idle4_2048x512_RGBA_CANDIDATE.png` **2048×512 / RGBA 투명 / 512×512 × 4 셀 / 우향**, 실측 PNG SHA256 **`ee6951b0333ba6586b4009ccf51cfd3a397d69f839552fe88a709f9c34bf0633`**.
- **Frame00은 기존 승인 1포즈와 RGBA 픽셀이 100% 동일**(`changed pixels=0`). Frame01/02/03는 같은 승인 원화 위 **머리·목, 흉곽의 약한 들숨/날숨, 검은 꼬리끝 방향에만 국소 변위**를 적용. 무리한 체형 재설계나 프레임 전체 평행 이동 없음.
- **4개 프레임 전부 서로 다름**, 각 프레임 가장자리 비투명픽셀 0, 발 접지 구간 y440~470 변경픽셀 0, alpha>8 bbox 각각 `[29,242,482,455]`, `[30,239,482,455]`, `[28,243,482,455]`, `[29,241,482,455]` (각 좌표는 프레임 내부, 반열림 구간).
- `Sprite/SnowStoat_M2_Idle_00_512_RGBA.png`~`03_512_RGBA.png` 4장 독립 QA 컷; 추가 동작 아님.
- `Preview/SnowStoat_Idle4_Actual_128px_4Frames_RGBA.png`, `Preview/SnowStoat_Idle4_Actual_40px_4Frames_RGBA.png` **최종 4프레임 실제 픽셀 축소**.
- `Preview/SnowStoat_Idle4_512_128_40_Review_OFFLINE.png` **512/128/40 실제 크기·설원/어두운 배경 비교**; 기존 몬스터 실제 Unity 화면 아님. `Preview/SnowStoat_Idle4_8FPS_OFFLINE.gif`는 확인용 8FPS 0.5초 순환 미리보기. 기존 Ch3 Mouse 8FPS 스타일을 참고한 **제안**으로 실제 Stoat Animator/import 설정 검증 `PENDING`.

## 3. 원본 보호·제작 방식

- 기존 AI 생성 원화를 새 캐릭터로 다시 그리지 않고 승인 512×512 RGBA 원본에 지역별 **작고 부드러운 픽셀 좌표 변형**으로 숨쉬는 Idle만 변주. 공식 원본 PNG 자체에 쓰기/교체0. 저작권/출처: 승인 사용자 소유 LIMITLESS 후보 원화를 기초로 한 독립 파생 작업; 제3자 아트 트레이싱 또는 Sprite 복제 아님.
- 이전 2172×724 비규격 생성 시안은 디자인과 크기를 자동 통과시키지 않기 위해 **게임용 PNG에 사용하지 않음**.
- 승인 Mouse16/Owl/늑대·여우/Ch1·2/환경/설빙의 폭군/냉기 VFX/Save/UserData/Discord·Issue #85 HOLD 전부 보존. `Unity 6000.6.5f1` 원본/Scene/Prefab/GUID/meta/Code/BuildSettings 변경0.
- 다른 공식 몬스터와의 **실제 동일 게임 표시 PPU·Pivot·Scale·40px GameView 검증은 Codex 후속**; 오프라인 미술 후보를 기술검증 PASS로 표시하지 않음.

## 4. 실파일 무결성/납품

- ZIP: **`SnowStoat_Issue100_Idle4.zip`**, **904,015 bytes / 12 entries**, SHA256 **`fa850041d0ed88d30a9e23a7b60e4345f22e2c12b4d01875042fb755970b734d`**.
- `Manifest.json`, `SHA256SUMS.txt`, `Docs/DesignNotes_SourceDeclaration_QA.md` 실제 파일 포함. Python zipfile testzip **CRC PASS**; 압축 해제한 내부 파일 해시 **SHA256SUMS 11/11 PASS**; sheet RGBA/2048×512/4셀·개별프레임 차등/접지·원본00 일치 **PASS**.
- 실제 PNG/ZIP/Preview는 **이번 ChatGPT 대화 내 다운로드 제공**. 사용자 Windows F:\ 다운로드 자동 복사, GitHub 바이너리 저장, Unity 로컬 Import/실제 플레이 테스트 모두 수행하지 않음.

## 5. 다음 의사결정

**대표님 이번 신규 Idle4 미술 승인 여부만 PENDING.** 승인 시 Codex가 원본 Local F: 실파일 SHA 재확인 후 Unity `6000.6.5f1`의 별도 최소 Import/Animator·실제 128/40 비교 검토 필요. Attack/Hit/Defeat 자동 착수 금지, main 직접 commit·Push·Merge 금지.

**최종 판정:** `IDLE4_ART_CANDIDATE / USER_VISUAL_APPROVAL_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`.
