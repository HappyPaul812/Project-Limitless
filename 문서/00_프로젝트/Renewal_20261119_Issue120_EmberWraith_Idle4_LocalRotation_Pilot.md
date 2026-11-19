# LIMITLESS / Chapter2 / Issue #120 — 불씨망령 Idle4 국소 순환 보완 후보 납품

## 1. 승인 범위와 선행 확인
- 대표 지시: Chapter1·2 NPC 리뉴얼 최종 종료. [NPC 종료 기록](https://github.com/HappyPaul812/Project-Limitless/issues/111#issuecomment-7000806171) 및 [Issue #120](https://github.com/HappyPaul812/Project-Limitless/issues/120), [정령계 공통 규칙 최신](https://github.com/HappyPaul812/Project-Limitless/commit/8f22f2b33794a7296cd584c635d4c618b6d80cf3), Chapter2_몬스터_1차_설계.md, PLANNING_LATEST 확인. 기존 NPC 보류 해제는 불씨망령 Idle 검토/보완에 한정.
- 복구 checkpoint ba59838bf923832362f3b1fccf4897864cc678fa의 실제 PNG blob, .meta, 15_EmberWraith.asset을 조사. 원본 경로 Unity/Client/Assets/_Project/Art/Monsters/15_EmberWraith/Ember_Wraith_Sprite_Sheet.png, 2,443,509 bytes, Git blob SHA1 56d00511dea6f0bace2276199f32974b8b8680ad, GUID 4828f7482c83eb949abd1f3017c227b3.
- GitHub 커넥터의 바이너리 다운로드 실패 후 ChatGPT Library의 실제 Ember_Wraith_Sprite_Sheet(1).png(파일 ID file_0000000032808230bb20b7d7ce68ed59)를 회수. 원본 raw byte로 Git blob SHA1을 독립 계산하여 위 GitHub SHA와 정확히 일치. 원본 SHA256 e6dfe61da6d754f615aaeff7f10719cc336b46d0ff7f5c09249545fd10c25655, 전후 동일. 공식 수정본 1256² RGBA/314² 4×4/16셀, Idle 00–03, explicitAttack 04–07, Hit 08–11, Defeat 12–15. 8FPS, PPU314, 슬라이스 하단 중앙 pivot, Point/무압축 계약. 실제 Unity 실행/최신 사용자 PC 파일 직접 재확인 없음.
- 원본 Idle4는 바깥 불꽃과 연기 일부 위치 변화가 있으나 후드·핵과 주요 띠의 형태가 거의 고정되어 회전감 보완 후보 필요로 판단. 눈보라 정령 v1/v2 비교 GIF(라이브러리 회수본)는 방향 참고만, v2 최종 미술 승인 및 ZIP 원본 대조를 주장하지 않음. 외부 게임 이미지/텍스처 사용0.

## 2. 제작 방식
- 기존 4개 Idle 셀을 그대로 기반으로 사용. 전체 회전 대신 기존 연기의 국소 접선 방향 이동, 원본 연기 명암 텍스처를 샘플링한 두 개의 테이퍼형 흐름, 원형 궤도를 따라 움직이는 낮은 강도의 불씨·재 포인트를 합성. 앞/뒤 통과 시 가림/밝기 차이를 두고 중심핵·후드/가슴의 보호 마스크를 유지. 작은 외곽 연기 alpha는 움직임 때문에 의도적으로 수정. 원화 전체 재생성0, 추가 프레임0.
- 기존 전체 PNG는 그대로 보관. 별도 후보 1256×1256 전체 시트의 **상단 Idle 4셀만 수정**. 공격/피격/패배 12셀 RGBA 픽셀 차이0(파일 재디코드 검사). Idle 전후 보호 마스크 각10,729 pixels RGBA 차이0, 셀 가장자리 alpha 차이0. Idle별 RGBA 변경 픽셀 37414/30200/36443/28193, alpha 변경 13419/5802/10438/7683.
- **미술 검토 한계:** 4프레임/8FPS(0.5초 루프) 제약 및 기존 후드·팔·큰 연기 실루엣 보존을 우선해 움직임은 국소적이다. 40px에서 작은 재 포인트 판독은 제한. 눈보라 v2와 직접 동일한 최종 모션 승인으로 해석 금지. 새로운 검은 연기와 불씨 궤적은 기존 공격의 큰 원형 화염/강한 플래시보다 낮은 강도로 제한.

## 3. 실제 산출물
- 게임용 후보 `Ember_Wraith_Sprite_Sheet_Idle4_Candidate.png` (1256×1256 RGBA), SHA256 **7b26e90652f9916faf5712647a2bf23c91a1ca2ea8bc266ca273a1de1dd58e70**.
- 검수용 후보 Idle strip `Ember_Wraith_Idle4_Candidate_1256x314.png`, SHA256 da5a33dad80aca97cd12c3dbe3f2643cfe47048fd37234241456cfa941e7c65d; 원본 Idle crop `Review_Original_Idle4_1256x314.png`, SHA256 658b85d373467db08f75333e3292218cfd6c106c60faccd98ccb1c85100d24ad. 원본 전체 시트 자체는 별도 보존.
- 실제 프레임 기반 비교 GIF `Ember_Wraith_Idle4_Before_After.gif`, SHA256 e52e9e802481a09b0d152e6f8cdcf5d19866132ca7f16c68299cd590897048c3, 실제 재디코드 4프레임/120,130,120,130ms/루프 500ms. 원본/신규 단독 GIF 각1 포함, GIF는 어두운 검수 배경만 사용, 게임용 PNG는 투명.
- 실제 314/128/40px 비교판 `Ember_Wraith_Actual_314_128_40.png`, SHA256 9748cd96a1a799c5fd47d75a09030fa27e0eb52333b2a8fb85b515358cf483ee. 현행 314셀 기준, Unity GameView 아님.
- 전체 ZIP `Limitless_EmberWraith_Issue120_Idle4.zip`, 5,848,242 bytes, SHA256 **0c5356d6e807f79c0fde9b1763715e89c2d03c4443ee57a100b81b2f376b9a37**, 12 entries, CRC PASS/Manifest 10/10/SUMS 11/11 PASS. README, SourceDeclaration, QA.json 포함. 실제 ChatGPT 작업환경 /mnt/data 파일; 사용자 Windows 자동 복사 및 GitHub 바이너리 업로드 없음.

## 4. 보호 / 다음
- **FILE_TECH_PASS / IDLE_ART_REVIEW_PENDING / NOT_UNITY_APPLIED / ATTACK_REVIEW_DEFERRED**.
- 기존 MonsterDefinition, GUID/meta, 전투 기믹(잿불 비산·불씨 응축·열핵 분출 및 타이밍), 드롭, 펫 불가, 필드/배틀, Unity6000.6.5f1, Issue #85 HOLD, Save/UserData/Discord/다른 승인 NPC·몬스터 보호. 새 원화/Attack/Hit/Defeat 작업0(기존 12셀 복사만). 기존 source PNG 덮어쓰기0, Unity 적용0, main 직접 commit/Push/Merge0.
- 사용자 Idle 미술 승인 후 Attack 보완 필요 여부를 별도 결정. Unity 적용은 별도 승인 및 Codex 최소 검증으로 기존 파일/메타/프레임 ID 및 실제 크기/타이밍을 보호.
