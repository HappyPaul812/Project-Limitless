> 2026-11-17 EOD 선별 기록: 출처 docs/art-new a912841490119048b2e10ae5a5797b7945c396b1 / blob b2073d1260041fad5843f41186c97cbcd603add1. 아래는 작성 당시 상태이며 최신 승인·실제 적용은 EOD 보고가 우선합니다. 얼음갑충·펫분양독립원화·눈보라정령은 게임 미적용, 설원족제비 Defeat4는 승인대기.

# LIMITLESS / Chapter3 F3~F4 M4 얼음갑충(Ice Beetle) — 신규 1포즈 파일럿 (2026-11-17)

**담당:** 메인 신규 아트 `docs/art-new`  
**기획 이슈:** [#103](https://github.com/HappyPaul812/Project-Limitless/issues/103)  
**정본:** [북부 7종 상세기획](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/08_몬스터/Chapter3_설빙의폭군_온기의화로_북부몬스터_상세기획_20261014.md), `SPRITE_ART_LATEST.md`.

**판정:** `ONE_POSE_CANDIDATE_DELIVERED / USER_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / OFFICIAL_EMBER_SAME_SCALE_NOT_VERIFIED / UNITY_NOT_APPLIED`.

## 1. 기존 원화 중복 조사
- GitHub Issue #103 및 `docs/art-new` 최신 인계, 북부 7종 상세 기획, GitHub 코드 검색 `IceBeetle`/`얼음갑충`, ChatGPT Library 제목·내용 검색: **기존 완료/승인 M4 원화 발견되지 않음**.
- 이것은 **사용자 Windows F: 드라이브 전수 감사가 아님**. 따라서 `EXISTING_ORIGINAL_NOT_FOUND_IN_ACCESSIBLE_SOURCES / WINDOWS_LOCAL_NOT_AUDITED`.
- 기존 승인 Mouse/ Stoat/ Owl/ 환경/ 보스 원본 변경0.

## 2. 실제 단일 게임용 KeyPose
- `Candidate/IceBeetle_M4_KeyPose_512_RGBA_CANDIDATE.png`, **512×512 RGBA 완전 투명 캔버스 / 정지 원화 1장 / 우향 3/4 측면**.
- **PNG SHA256:** `b7ff0592226c2a1f28270053cf03483b6051c69edb412b59a7965ccf8699b3ad` (**241,066 bytes**).
- `alpha>0` 실제 bbox **(29,125)~(484,464)**. 네 캔버스 가장자리 Alpha0. 원본 판독을 위해 밝은 눈배경/어두운 배경에서 실축소 확인; PPU/pivot/Unity GameView 기준 정식 상대 크기 검증은 아직 아님.
- 형태: **낮고 넓은 방어형 청록 얼음 갑각, 작은 다리, 어두운 보라 회색 밑몸, 얼음 결정의 굵고 각진 분절, 어둡고 명확한 윤곽, 지극히 약한 금빛 과거 온기 잔광**. 금빛은 디자인 스토리 장식이며 신규 능력/상태이상 구현 계약 아님.
- 이미지 생성: 이번 대화에서 새 생성한 오리지널 후보 `a_highly_detailed_pixel_art_ice_beetle_style_cre.png` **1254×1254 RGBA**를 사용, 알파>8 보이는 범위 `(53,218)-(1179,1056)` 크롭, 비율 유지 **456×339** 축소를 512 RGBA 투명 캔버스 하단 y464에 정렬. 제3자 기존 몬스터 PNG에 팔레트 변환/트레이스0. 생성 원본은 게임용 2번째 포즈로 납품하지 않음.

## 3. 실제 축소 비교/공식 몬스터 참조 한계
- `Preview/IceBeetle_M4_Actual_128px_RGBA.png`, `Preview/IceBeetle_M4_Actual_40px_RGBA.png` **512 게임용 파일에서 직접 축소**, Preview 보드 `Preview/IceBeetle_M4_512_128_40_ReadabilityBoard_OFFLINE.png`에는 512·128·40 및 밝은 설원·어두운 필드 실제 픽셀 출력.
- [Chapter2 화열딱정벌레 공식 원본 경로](https://github.com/HappyPaul812/Project-Limitless/blob/main/Unity/Client/Assets/_Project/Art/Monsters/14_EmberBeetle/Ember_Beetle_Sprite_Sheet.png)와 GitHub blob SHA1 **`fe701ce82ccd1bd0bec9cfcfb208e43c0872f3da`**는 확인. 다만 이미지 본문 바이너리 직접 추출은 연결에서 지원되지 않아 **실제 화열딱정벌레 원본의 동배율 나란히 판독은 `NOT_VERIFIED`**. 단순히 참고용 합성 이미지를 공식 실물이라고 표시하거나 공식 픽셀을 재구성하는 비교판 제작 금지.
- `Preview/IceBeetle_vs_EmberBeetle_SourceStatus_OFFLINE.png`: 신규 후보의 512 실제 그림 + 공식 출처 존재/실물 비교 미완료 상태를 명시한 **비교 검증 한계 보고판**, 진짜 두 몬스터 동배율 시각 PASS 판정이 아님. 화열딱정벌레는 불꽃 갑충류이고 이번 M4는 낮고 넓은 겹겹의 빙결 갑각으로 구상하여 **디자인 차별화 의도**는 있으나 최종 픽셀 동등성 검증은 별도.
- 승인 전 실제 GameView 비교/동일 월드 표시 크기 통합 미시행. 기존 게임 상태 임의 비교 PASS 금지.

## 4. 출처·무결성·납품
- `Docs/SourceDeclaration_And_QA.md`, `Manifest.json`, `SHA256SUMS.txt` 실제 파일 제공.
- **Manifest**는 payload **6개**의 파일크기+해시만 기록(**6/6 PASS**), Manifest 또는 SHA256SUMS 자신을 참조하지 않음.
- **SHA256SUMS**는 payload6 + 최종 Manifest1 = **7/7 PASS**, SHA256SUMS 자신을 제외. 순환 참조0.
- 납품 ZIP `IceBeetle_Issue103_OnePose.zip` **830,282 bytes / 8 entries**, SHA256 **`ca56030e22d581ef58bafe51b66cc9181478dfa8343f16f33155aeda8f14323a`**.
- Python `zipfile.testzip()` **CRC PASS**, ZIP 내부 Manifest **6/6** 크기·SHA PASS, SHA256SUMS **7/7 PASS**, 원화 크기 RGBA·투명 외곽 확인.
- 납품 위치: ChatGPT 대화에 PNG/ZIP/비교판 제공 + **Library `/LIMITLESS/Issue103_IceBeetle_OnePose/`** 보관. GitHub에 PNG/ZIP 바이너리 직접 업로드했다고 주장하지 않음. Windows F:\Downloads 자동 복사0.

## 5. 승인 대기/보호
**`USER_ART_REVIEW_PENDING`**: 신규 Ice Beetle 1포즈 자체를 바울 대표님이 승인한 것은 아님. 승인 전 Idle/Attack/Hit/Defeat·다방향 원화·펫 동작·스킬/전리품·MonsterID/Spawn/스탯/Unity Import/Animator 생성0.
기존 승인 서리들쥐·설원족제비·서리올빼미/Chapter2 화열딱정벌레·환경·보스 원본 보호. **Unity 6000.6.5f1**, Issue #85 HOLD, Save/UserData, Discord, 대형 Unity QA·추가 worktree·main 직접 commit/Push/Merge0.
