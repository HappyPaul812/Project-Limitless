> 2026-11-16 EOD 선별 인계: 출처 `docs/video`, Git blob `bcd8625acbcd0fea8e9b4bb5c38ea9c09f1dbf3a`. 이하 보고의 승인/미적용/PENDING 표시는 작성 당시 상태입니다. 최신 결재는 EOD 보고 우선: Stoat KeyPose·Idle4·Attack4v2·Hit4 미술 승인/Unity미적용, NPC02/17·10/07 로컬적용, Issue85 Import/GUID HOLD. NPC 손 한정 지시는 기획 해석 오류였으며 최신 정정대로 인계합니다. 기존 제품·미완료 자산을 새로 적용하지 않습니다.

# LIMITLESS 개발 기록 영상 #3 — 비교·Raw 촬영 최종 인계 / 2026-11-16

**RAW_READY / EDITOR_RESTORED / LOCAL_DOCUMENTED / REMOTE_NOT_PUSHED / HUMAN_REVIEW_PENDING**

새 Raw와 비교 보고서를 납품했습니다. 완성 편집·YouTube 게시본이 아닙니다. 21개 실제 게임 컷을 정상 속도로 연결했으며 대기만 잘랐습니다. 기존 10월 화면을 새 영상으로 재사용하거나 합성하지 않았습니다. 정상 진행 컷과 환경·시설 직접 실행 컷은 아래에 구분했습니다.

## [작업 상태]

- 최초 한도 조회:5시간 잔여99%, 주간 잔여12%, 크레딧0. 중간 주간 한도로 중단했고, 바울 대표의 재개 요청 후 조회는 두 기간 사용0%(잔여100%)였습니다. 완료된 조사·촬영·오디오 테스트는 반복하지 않았고 새 Raw의 최종 검사만 추가했습니다.
- Unity 지정·실행 버전 **6000.6.5f1 (3ff58d469c8a)**. 변경0.
- 실제 로컬 게임 기준 SHA **d360813667d2ee47515067b8dd07c1a599efec15**. 미커밋 사용자 변경을 포함한 로컬 작업트리로 촬영했으므로 이 SHA만으로 전체 실물을 재현한다고 주장하지 않습니다.
- 시작 GitHub main **bfd281545632877325f5d21e04cefb924042c465**(10/24). 로컬은68커밋 선행. 최신 원격 main과 최신 로컬 적용은 다릅니다. 이번 원격 동기화·병합0.
- 문서 전용 **docs/video**, 소량 sparse worktree `Temp/DevJourney20261116/VideoDocs`. 중간 인계 commit `4a76a21f1e17cb63d9a426cc454eefaed46d20ea`; 최종 실제 commit은 최종 답변·외부 Final_receipt.json에 기록합니다. 자신의 commit SHA를 같은 commit 본문에 넣지 않습니다.
- 추가 촬영 승인을 기다리는 상태는 아닙니다. **사람의 최종 시청·청취와 완성 편집/게시 판단은 남아 있습니다. GitHub Push는 별도 명시 요청 전 미실행**입니다.

## [비교]

### 보존한 10월 자료

- 정본 `F:\Downloads\Limitless_DevJourney_2026-10-06_Final.mp4` 확인.44,273,546bytes/05:28.133/1920×1080/30fps/H.264/AAC48kHz Stereo. 시작·종료 SHA256 모두 `02fa8fc6da38d6f02f574611ef83cddb1a40e1715d533d5fa5e3455df77b2cb8`로 동일합니다.
- 이전 폴더 `F:\Downloads\Limitless_DevJourney_2026-10-06` 확인. 내부 `Limitless_DevJourney_FixedVoice_2026-10-06.mp4`는51,823,681bytes의 별도 파일입니다. Final과 혼동하지 않았고 덮어쓰기·삭제·대량 복사0.
- 선행 QA `문서/00_프로젝트/LIMITLESS_DevJourney_FixedVoice_2026_10_06.md`와 AAC_QA.csv 사용. 정본 전체 디코딩 오류0은 최초 조사에서 확인해 재개 후 반복하지 않았습니다.

|10월 Final 실제 확인 시점|새 Raw 대응 구간|실제로 확인한 차이·편집 안내|
|---|---|---|
|02:20 시작마을|00:00–00:08,00:54–01:22|같은 시작마을. 지면·건물·수목과 승인 OGA03 잡화상인/OGA09 주민 대표/OGA06 치유사/OGA16 은행 안내인의 실제 외형 확인. 같은 마을 전후 비교로 사용 가능|
|01:00 생성,02:00 직업 선택|00:08–00:54|현재 생성 UI·여성·청각의 길·사수 자동 외형. 10월 지체·마도사와 조합이 다름. 같은 조합 개선 비교로 표시하지 말 것|
|03:00–03:40 태온·미엘 초원 대화|02:53–03:04.500|현재 Main11 지하묘지 태온·미엘 Voice/초상화. 다른 대사·장소의 확장 사례이며 동일 대사의 수정 전후가 아님|
|04:20–05:10 초원 전투|03:04.500–04:44.500|현재 묘지 망자 전투·태온 도발·미엘 치유. 다른 조우이므로 초원 배경을 던전 배경으로 교체했다고 설명하지 말 것|
|05:15 Victory,05:20/05:25 필드 복귀|04:44.500–05:35.500|실제 정상 승리·보상·정식 VictoryReturn·B1 복귀. 10월 Field_01과 현재 Dungeon_01의 서로 다른 조우/귀환 사례|
|대응 장면 없음|01:22–02:30|Chapter2 Field04/Field10,Chapter3 F1의 현재 적용 환경. 신규 확장으로 표시하고 억지 전후 비교 금지|
|대응 장면 없음|02:30–02:53|정상 생성 사수의 기본 Wolf/Bear/Fox 보유·프리뷰. 월드 추종 또는 새 펫 전투 애니메이션을 촬영했다고 쓰지 말 것|

### 최신 담당·승인 근거

작업 시작 때 GitHub 각 ref를 직접 조회했습니다. `docs/video`의 DEV_VIDEO_LATEST는 API404/로컬 없음이었으므로 역할 정본 DEV_VIDEO.md와10월 QA에서 시작했습니다.

|원격 브랜치|조회한 SHA|
|---|---|
|docs/planning|6f69e3ac71171076e78b0aac0146b315ab25ebb5|
|docs/art-renewal|a4639a58adc9cf9ef1125fc4fad6804af70671ab|
|docs/art-new|3644200d40b64411b24ca240a7580b42b3ae2a1a|
|docs/tts|92feffbd544f8ec61d58032f0900222199adc7cd|
|docs/bgm|e44aee0ecd8bee6d578cf1e33f7f278a33e94f6f|
|docs/sfx / docs/video|73c37bf02232056bb9c101d80f0e82d383d1b980|

- 로컬 최신 CURRENT_STATUS/CODEX_LATEST: Bee/Spider/펫/Chapter2/TTS162는 로컬 main 포함. 옛 담당 인계의 HOLD를 최신 적용 상태로 혼동하지 않았습니다. 기존 QA 수십~수백 항목 전수 재실행0.
- [PR68](https://github.com/HappyPaul812/Project-Limitless/pull/68): 승인 TTS162 통합 근거. [Issue93](https://github.com/HappyPaul812/Project-Limitless/issues/93): Main11 동행 필터·PCM 복구·입구/귀환 후속. 이번에는 실제 Main11 태온007·미엘005를 정상 조사로 촬영했습니다.
- [Issue95](https://github.com/HappyPaul812/Project-Limitless/issues/95),[Issue57](https://github.com/HappyPaul812/Project-Limitless/issues/57): 승인 NPC 실제 적용 근거. Runtime Sprite 이름과 실제 화면을 대조했습니다.
- [기획 최신](https://github.com/HappyPaul812/Project-Limitless/blob/docs/planning/문서/00_프로젝트/팀운영/인계/PLANNING_LATEST.md):11/12 Clean Release PSD/GUID HOLD는 별도 후보 문제이며 본 영상에서 수정하거나 통합하지 않았습니다.
- [Issue85](https://github.com/HappyPaul812/Project-Limitless/issues/85),[Issue75](https://github.com/HappyPaul812/Project-Limitless/issues/75): F2/Owl 승인·적용 기록은 확인했지만 이번 최종 영상에 사용하지 않았습니다. 문서 존재만으로 새 실플레이 PASS를 선언하지 않았습니다.

### 실행 방식·제외 범위

- **정상 진행:** 기존 슬롯01을 바이트 그대로 격리 복사→정식 이어하기→Field03 입구 E→B1/B2 물리 이동·조사→일반 공격/정식 스킬·대상 버튼→정상 Victory/복귀. 새로운 슬롯03도 성별·길·직업·게임 시작 UI로 생성했습니다. 성장/Quest/HP/MP/보상/좌표를 주입하지 않았습니다.
- **직접 실행 환경·시설 컷:** Field04/Field10/F1/Arbel은 격리 세션의 기존 Scene을 직접 로드했습니다. 실제 적용 씬·캐릭터·NPC·UI·Collider를 사용했지만 정상 스토리로 해당 장소를 해금·도달한 증거는 아닙니다. 원래 Main11/새 캐릭터의 Quest를 해금하지 않았습니다. Arbel 펫 관리는 실제 NPC까지 걸어0.784m 거리에서 기존 NPC.Interact와 실제 UI를 사용했습니다. 무료 보유3종 프리뷰만 촬영, 구매·장착 변경0.
- 서릿길 마을:현재 저장의 Quest 상태로 직접 실행하면 지면만 나타나 최종에서 제외. 신규 NPC·Main23 진행·안전거점 완료를 촬영했다고 주장하지 않습니다.
- F2:실행 상태의 `PLACEHOLDER_*`/`CANDIDATE_*`를 근거로 **자동 승인 검토가 녹화를 차단**했습니다. 우회하지 않았고 F2 녹화0/최종 포함0. 대신 실제 승인 Sprite가 표시된 F1을 사용했습니다.
- 제외:미적용 BGM 파일럿2곡,C SnowEdge/OGA20/미승인 시안·동상/동결괴사/정화Ⅱ/아이템·Discord. Owl/Chapter3 스킬과 전투배경은 이번 영상에서 실제 촬영하지 않아 포함을 주장하지 않습니다. 맹독뱀/Spider의 새 공격 프레임도 최종에 없음.
- 실전 스킬은 **수호자 도발·치유사 치유의 빛**. 사수 스킬은 생성 UI의 설명 프리뷰이며 실제 사용 장면이 아닙니다. 5직업 전체 전투를 촬영한 것은 아닙니다.

## [녹화]

- 납품 `F:\Downloads\Limitless_DevJourney_2026-11-16\Limitless_DevJourney_2026-11-16_Raw.mp4`
- **67697964bytes(약64.56MiB) / 05:35.500 / 1920×1080 / 30fps / 10,065프레임**.
- Video:H.264 High/yuv420p. Audio:AAC LC/48kHz/Stereo/192kbps 지정. Video·Audio 실제길이 모두335.500초,차이0.
- SHA256 `7930689dad5c2299711b4a1037c06b1fd210de6ebe91208358458139da341463`.
- Unity 공식 Recorder5.1.7 Game View/Game Audio. 마이크 입력0·새 Voice 생성0·외부 BGM/SFX/가짜 화면/하이서드0. 원본 화면 비율·정상 속도 유지.
- 실제 포함:Chapter1 시작마을·새 사수 생성/외형·지하묘지·태온/미엘·정상 전투;Chapter2 Field04/Field10 환경·세린 및 몬스터 표시·Arbel 펫 관리;Chapter3 F1 적용 환경.
- 오디오:기존 UI로 첫 격리 테스트의 Voice100/BGM5 적용,후속 본촬영은 복사한 사용자 설정 Voice100/BGM7을 사용했습니다. 실제 Mixer Voice0dB/BGM−23.098dB 확인. 원래 사용자의 설정 파일은 변경0. 전투 BGM Steel_and_Sunlight 포함. 전체 평균−36.5dB/최대−5.1dB로 클리핑 없음. BGM이 낮은 Raw이므로 최종 배포 음량 판단은 편집 담당의 사람 청취가 필요합니다.
- 사전 테스트:실제33.667초(도구 왕복 포함),오프닝4문장 앞·중간·끝 원본 대조PASS. 재개 후 재촬영/재검증하지 않음. 테스트 마지막 잘린005와 미포함006은 본편에 넣지 않았습니다.
- **최종 Raw Voice2/2 전체 발화 정합:**태온 `main11_taeon_007` 시작02:55.732/4.160초,미엘 `main11_miel_005` 시작03:00.489/3.030초. 실제 AAC의 앞·중간·끝 원본 WAV 상관 최소0.94307,각각 일치. 실제 자막·정식 대사 ID·태온/미엘 초상화 프레임 대조. 비동행 폴은 무음 지문이며 폴 Voice 잔류 없음. 신규 사람 청취 승인과 파형 검증은 별개입니다.
- 전투:정식 명령으로 묘지 망자 승리,EXP32/탈렌트9/망자의 파편1 획득,정식 VictoryReturn로B1복귀. 강제승리/QA EndBattle/HP조작0. 컷으로 대기를 줄였으므로 원테이크라고 표현하지 않습니다.
- **전체 디코딩 exit0/오류0**,blackdetect0.5초 이상 전체 검은 화면0. 원본 컷과 최종 contact sheet/대사 프레임 확인. Arbel 펫 컷의 화면 우측·하단 띠와 일부 기존 안내선 등 실제 화면의 한계는 그대로 두었으며 숨기기 위해 크롭·합성하지 않았습니다.

### 새 Raw 및 촬영 원본 타임코드

원본 컷은 `F:/study/codex/Project-Limitless/Temp/DevJourney20261116/`에 있습니다. 10월 영상 전체 중복 복사는 하지 않았습니다. 표는 편집 담당이 더 긴 앞뒤 소재를 찾는 기준입니다.

|새 Raw 시점|실제 내용|촬영 원본 및 시점|
|---|---|---|
|00:00.000–00:08.000|현재 시작마을 / 동일 마을 비교|`01_CurrentVillage.mp4` 00:00.000–00:08.000|
|00:08.000–00:24.000|정상 생성 UI / 여성 선택|`12_CharacterCreation_Archer.mp4` 00:00.000–00:16.000|
|00:24.000–00:32.000|사수 선택 프리뷰|`13_ArcherAndPet.mp4` 00:00.000–00:08.000|
|00:32.000–00:46.000|정상 최종 확인·게임 시작|`13_ArcherAndPet.mp4` 00:20.000–00:34.000|
|00:46.000–00:54.000|정상 새 사수 월드 외형|`13_ArcherAndPet.mp4` 00:39.000–00:47.000|
|00:54.000–01:06.000|현재 마을 걷기|`02_VillageWalk.mp4` 00:00.000–00:12.000|
|01:06.000–01:22.000|마을 승인 NPC 외형|`02_VillageWalk.mp4` 00:24.000–00:40.000|
|01:22.000–01:46.000|Chapter2 Field04 / 환경 직접 실행 컷|`08_Chapter2_Field04_EnvironmentPreview.mp4` 00:00.000–00:24.000|
|01:46.000–02:06.000|Chapter2 Field10 / 환경 직접 실행 컷|`09_Chapter2_Field10_EnvironmentPreview.mp4` 00:00.000–00:20.000|
|02:06.000–02:30.000|Chapter3 F1 / 적용 환경 직접 실행 컷|`11_Chapter3_F1_AppliedEnvironment.mp4` 00:00.000–00:24.000|
|02:30.000–02:37.000|실제 Arbel 펫 관리 UI / Wolf|`14_Arbel_PetFacility.mp4` 02:26.000–02:33.000|
|02:37.000–02:45.000|실제 보유 Bear 프리뷰|`14_Arbel_PetFacility.mp4` 03:00.000–03:08.000|
|02:45.000–02:53.000|실제 보유 Fox 프리뷰|`14_Arbel_PetFacility.mp4` 03:14.000–03:22.000|
|02:53.000–03:04.500|정상 Main11 조사 / 지문·태온007·미엘005 전체 발화|`06_BlueFlow_Voice.mp4` 01:48.500–02:00.000|
|03:04.500–03:28.500|정상 Field03→B1→전투 진입|`03_Field03_Dungeon.mp4` 00:00.000–00:24.000|
|03:28.500–03:44.500|미엘 일반 공격|`03_Field03_Dungeon.mp4` 01:40.000–01:56.000|
|03:44.500–03:58.500|플레이어 일반 공격|`03_Field03_Dungeon.mp4` 02:10.000–02:24.000|
|03:58.500–04:17.500|태온 도발·적 행동|`03_Field03_Dungeon.mp4` 02:31.000–02:50.000|
|04:17.500–04:44.500|미엘 치유의 빛·회복|`03_Field03_Dungeon.mp4` 03:05.000–03:32.000|
|04:44.500–05:09.500|정상 공격→Victory 결과|`03_Field03_Dungeon.mp4` 03:42.000–04:07.000|
|05:09.500–05:35.500|정식 VictoryReturn→B1 복귀|`03_Field03_Dungeon.mp4` 04:34.000–05:00.000|

## [보호]

- 기존10월Final SHA동일. 기존녹화폴더·원본WAV/PNG/사용자수동Sprite/ThirdParty·Discord·기존백업 삭제/덮어쓰기0.
- **기존 게임 코드 변경0**. 촬영용 Editor C#·meta만 임시 추가 후 제거 완료. 공식 Recorder만 임시 사용한 뒤 manifest/lock은 작업 전 바이트와 동일하게 복원. 재컴파일 idle/Console오류0/저장된 Bootstrap/EditMode/원래Save경로 복귀 확인.
- Save·Settings 포함UserData **23파일 SHA변경0**. 격리 폴더에서만 실제 진행 저장. 원본 git status 시작·종료 목록 동일/main SHA동일. 사용자 기존 변경을 stage/commit/reset/stash하지 않았습니다.
- Unity 프로젝트 전체 복사0,Library/Temp 대용량 백업0,기존백업 삭제0,불필요패키지/패키지 업그레이드0. 추가 Recorder 캐시는 UPM 원복으로 없어졌습니다. 새 영상 원본·보고/QA 파일만 보존.
- F:시작326,247,772,160bytes→촬영/인코딩 후 측정324,476,821,504bytes 여유. 차이는 새 촬영/Raw·실행 캐시 등을 포함하며 원본 백업의 복사량이 아닙니다.
- GitHub Push/main직접commit/강제Push/원격병합0. 문서는 docs/video 로컬 커밋. 원격 문서 미반영이므로 GITHUB_REPORT_COMPLETE로 표기하지 않습니다.

## 편집 담당 ChatGPT 인계

1. 이 Raw와 기존 Final을 사용해 시작마을의 직접 비교를 우선 구성하세요. 다른 장소·직업 조합·대사는 확장 사례로 설명하세요.
2. 환경 직접 실행 컷(01:22–02:30)과 Arbel 시설 컷(02:30–02:53)을 스토리 정상 해금/도달 장면으로 설명하지 마세요. 서릿길 마을/F2/Owl/미촬영 스킬은 넣지 마세요.
3. 태온·미엘 발화를 자르지 말고 최종 청취로 음량을 판단하세요. 본 Raw에는 홍보 자막·오프닝/엔딩·YouTube 메타데이터를 추가하지 않았습니다.

근거 로컬:`Raw_CutPlan.json`,`Raw_ffprobe.json`,`Raw_QA_summary.json`,`Raw_decode_QA.log`,`Limitless_DevJourney_2026-11-16_Raw-source-proof.json`,`Protection-after.json`,원본/최종 contact sheet. 최종 Git SHA/원격 보고 여부는 Final_receipt.json 및 최종 답변을 참조하세요.
