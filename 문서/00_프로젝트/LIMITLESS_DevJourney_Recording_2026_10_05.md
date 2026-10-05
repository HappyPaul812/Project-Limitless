# LIMITLESS 개발 여정 초반 Story 녹화 — 2026-10-05

LOCAL Unity 6000.5.7f1의 실제 Runtime을 기록했다. 사전 격리 회귀검증과 최소 참조/UI 수정 후 리허설·최종 녹화를 수행했다. [종합 QA](LIMITLESS_CURRENT_FULL_REGRESSION_QA.md)를 따른다. GitHub 조회/Push·YouTube 업로드 없음.

## 최종 파일

`F:/Downloads/Limitless_DevJourney_2026-10-05/Limitless_DevJourney_EarlyStory_2026-10-05.mp4`

|항목|실제 결과|
|---|---|
|Capture|공식 Unity Recorder 5.1.7, 순수 Game View + Game Audio|
|영상|1920×1080 / 16:9 / H.264 / 30fps|
|길이|312.866633초 — 5분12.87초|
|파일 크기|131,256,386 bytes / 약125.18 MiB|
|Audio|AAC / 48,000Hz / stereo 2채널|
|Audio 신호|평균 -18.3dB / 최대 -1.7dB, 무음 아님|
|검사|ffprobe 트랙/규격, ffmpeg 전체 decode 오류0, Title/대표구간/승리/마지막 Field 프레임 시각 확인|
|후처리|최종 파일은 한 번의 실행을 그대로 기록. Cut/속도변경/장면합성/프레임반복 없음|

60fps 대신 안정적인30fps를 사용했다. 임시 Recorder의 manifest/lock은 작업 전 바이트로 복원하고 package resolve/컴파일을 완료했다. 임시 녹화 Editor/이동 드라이버2개와 meta2개를 제거했다. Build/Runtime에 Recorder 의존성 없음.

## 실제 포함 범위

- Title 약12초: 빈 격리 슬롯과 정식 Before the First Light BGM. 실제 사용자 슬롯은 보이지 않는다.
- Intro 약50초: 정식 Narration/Subtitle·Visual·BGM. 현재 음성 한 문장이 끝난 후 Skip했다.
- 생성 약1분: Female→Male, 이름「여행자」, Hearing→Vision, Guardian→Sharpshooter→Mage Preview. 최종 Male/Vision/Mage이며 정식 네 단계로 Confirm/World에 진입했다.
- Town 약1분20초: 시작 마을 주민 대표와 남문 경비병의 완결된 Main01 대화, NPC 거리 상호작용, Main01 완료/정식 Main02 시작. Arbel/후반 Quest가 아닌 초반 시작 마을이다.
- Field: 실제 이동 애니메이션/충돌, 마을 출구→Field_01, 조사 지점 진입과 지정 `grass_slime_01` 접촉. Field/Battle BGM 전환.
- 일반 Battle 약1분: 정식 Gaia Well/Fireball/Thunderbolt와 쿨타임 사이 기본 공격, 슬라임3마리 승리. 실제 EXP24/탈렌트9/정상랜덤 Loot 결과 패널, Field 복귀 후 걸어서 안전하게 이동하며 종료. HP/적 상태/보상/Quest를 연출용으로 주입하지 않았다.

Intro Voice와 정식 Title/Town/Field/Battle BGM이 파일에 포함됐다. 현재 SFX는 Mixer routing만 있고 게임 효과음 재생이 미구현이므로 SFX는 **NOT_APPLICABLE**이며 가짜 효과음을 추가하지 않았다. Main01/02 일부 NPC는 원래 무음·Portrait 미연결(null-safe) 상태다. 이 영상의 Town 대사는 그 상태를 그대로 보여준다. 청취 취향·발음/감정/BGM masking에 대한 사람 청취 PASS를 주장하지 않는다.

Editor/Inspector/Console/Desktop/개인정보/QA Overlay/Test 버튼 없음. 필드 Quest Marker·레벨/이름과 설정 버튼은 정식 Runtime UI다. 세부 미관과 전체 화면비 검수는 종합 QA의 T에 미검증으로 남긴다. 추가 Progress Teaser는 만들지 않았다.

## 자동 조작과 보호

기존 UI 버튼/Scene 흐름/거리 검사 InteractionSystem/전투 명령을 호출한 자동 플레이다. 임시 방향 드라이버는 기존 PlayerController의 이동 값과 FixedUpdate 물리 경계에 방향을 전달했다. 위치를 순간이동하거나 Quest·스탯·HP·적 상태를 변경하는 녹화용 Seed는 사용하지 않았다. 물리 키보드/게임패드 입력을 검증했다고 주장하지 않는다.

Save/Settings는 `Unity/Client/Temp/Partial9FixedRuntime/<GUID>`에 격리했다. 정식 Audio 기본값100/100/80·Mute OFF를 격리 프로필에서 사용했으며 사용자 진행/Settings는 덮어쓰지 않았다. 종료 후 Audit 경로·세션·Game View 진입 동작·Play 옵션·runInBackground를 복원했고 clean Bootstrap Edit Mode로 돌아왔다.

리허설의 입력 전달 실패/일반 전투 패배/승리 뒤 두 번째 재조우가 있는 테이크는 최종으로 사용하지 않았다. 이전 임시 후보의 끝부분 Cut도 검토했으나 최종본은 위의 새 전체 테이크로 교체했다. 출력 폴더의 `Rehearsal.mp4`, `EarlyStory_Raw_NotFinal.mp4`는 최종본/Teaser가 아니다. MP4와 임시 Recording Asset/Package는 Git에 포함하지 않았다.

## 관련 로컬 커밋

- `aa12810`: 종합 QA 계획
- `1e0a957`: Renderer2D 구형 디버그 참조6개 정상복원
- `80d2f0b`: Title 다시보기 버튼 하단 Outline 여백 최소수정
- `deb181c`: 격리 전체/Title 회귀와 Main09/10 Fixture 정정

문서 결과 커밋은 최종 보고에서 확인한다. GitHub Push 없음.
