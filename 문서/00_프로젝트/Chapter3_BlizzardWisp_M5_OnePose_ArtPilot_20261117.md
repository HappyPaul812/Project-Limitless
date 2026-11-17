> 2026-11-17 EOD 선별 기록: 출처 docs/art-new a912841490119048b2e10ae5a5797b7945c396b1 / blob be0335433b067aec02253427a220650b91f8ddf2. 아래는 작성 당시 상태이며 최신 승인·실제 적용은 EOD 보고가 우선합니다. 얼음갑충·펫분양독립원화·눈보라정령은 게임 미적용, 설원족제비 Defeat4는 승인대기.

# LIMITLESS / Chapter3 F4~F5 M5 눈보라 정령(Blizzard Wisp) 1포즈 — 2026-11-17

- Issue [#112](https://github.com/HappyPaul812/Project-Limitless/issues/112), [기획 발주](https://github.com/HappyPaul812/Project-Limitless/issues/112#issuecomment-6941465995)
- 담당: docs/art-new
- 상태: `ONE_POSE_CANDIDATE / USER_ART_REVIEW_PENDING / OFFLINE_FILE_QA_PASS / UNITY_NOT_APPLIED`

## 기존 승인 및 중복 확인
- M4 얼음갑충 KeyPose·Idle4·Attack4·Hit4·Defeat4 최종 승인: [#103 기획 승인](https://github.com/HappyPaul812/Project-Limitless/issues/103#issuecomment-6941419315). 재제작 금지, Unity 미적용.
- #112, 북부 몬스터 7종 상세기획 및 최신 인계, GitHub BlizzardWisp 검색, Library 검색에서 기존 승인/완성 M5 원화 미발견. Windows F: 전수 검색은 미실행.

## 신규 원화 1종
- `Candidate/BlizzardWisp_M5_KeyPose_512_RGBA_CANDIDATE.png` **512×512 RGBA**, 217,196 bytes, SHA256 **`0c72c3f8fc80c9e74bf648236a0ff1746ef344ef71f03b4c005c38db59915586`**.
- 은백색 눈바람 소용돌이, 어두운 청색 핵, 오른쪽으로 뻗는 비대칭 흐름, 적은 수의 분리 얼음 결정. 인간형/유령 얼굴/팔다리 없음. 설원에서 어두운 윤곽 대비. 방향은 얼굴이 아닌 흐름으로 표현.
- 신규 생성 1254×1254 RGBA 원본 SHA256 `2b25898098f0c711065d390a7959a984232a7d30530106ee6e86191eb6d77d1d`을 알파 정리(8 이하 및 작은 고립 성분 제거), 가시영역 크롭 (124,110,1131,1155), 비율 유지 439×456으로 Lanczos 축소, 512 투명 캔버스 배치. 실제 alpha bbox [36,8,475,464], 가장자리 알파0. 신규 외부 자산 복사 없음. 부유체의 런타임 pivot/PPU는 미확정.
- 실제 512/128/40px 축소와 밝고 어두운 단색 배경 비교판. 추가로 실제 승인 설원족제비 M2 및 얼음갑충 M4 KeyPose의 SHA256을 기존 값과 대조한 뒤 동일 캔버스 256/40px 비교판 제작(기존 PNG 수정0). 전체 북부 몬스터나 Unity 월드 배율 검증을 의미하지 않음.

## 실제 납품 및 검수
- ZIP `BlizzardWisp_Issue112_OnePose.zip`: **798,663 bytes / 8 entries**, SHA256 **`53b21965bd0513a42bb04ebefd78572554a7e9803106e56f8573b36da7150ede`**.
- ZIP 재개봉 CRC PASS, Manifest payload 파일 크기/SHA/커버 6/6 PASS, SHA256SUMS payload+Manifest 7/7 PASS, 순환 참조 및 중복0. SourceDeclaration 포함.
- ChatGPT Library `/LIMITLESS/Issue112_BlizzardWisp_OnePose/`에 ZIP·PNG·실축소 비교판·북부 참조 비교판 4개 업로드 성공. GitHub 바이너리 업로드0, Windows F: 직접 복사0.

## 승인 및 보호
사용자 M5 원화 미술 승인 필요 YES. Idle/Attack/Hit/Defeat, Unity, Codex 호출, 기원/스킬/수치/전리품/Spawn/펫 구현0. 기존 승인 M1~M4와 환경·보스, Unity6000.6.5f1, #85 HOLD, Save/UserData/Discord/main 보호. main 직접 commit·Push·Merge0.
