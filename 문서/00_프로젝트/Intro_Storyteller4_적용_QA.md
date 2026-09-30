# Intro Storyteller_4 적용 QA — 2026-09-30

## 입력과 대응

입력은 `F:/Downloads/Voice/Limitless_TTS_Output/02_intro_flash_Storyteller_4/segments`이다. 인접 `VOICE_USED.txt`와 외부 제작 스크립트 `F:/Downloads/Voice/limitless_gemini_tts_v6.py`를 읽기만 했다. 추가 생성이나 API 호출은 없다. 원본 18개 SHA-256과 Unity 복사본이 모두 같다.

외부 `INTRO_LINES` → LOCAL `Tools/TTS/opening_narration.csv` → `OpeningIntroSequence.Slides`의 ID·문장이 모두 일치한다. 외부 스크립트는 전체 WAV를 긴 무음 경계로 분할한 뒤 해당 ID로 저장한다. 문장 데이터와 연결의 검증이며 실제 발화의 청취 검수는 별도다. Intro는 내레이션 18블록과 음성 없는 제목 1블록이다.

| ID | 바이트 | 길이(초) |
| --- | ---: | ---: |
| opening_001 | 194924 | 4.06 |
| opening_002 | 154124 | 3.21 |
| opening_003 | 318284 | 6.63 |
| opening_004 | 286604 | 5.97 |
| opening_005 | 213644 | 4.45 |
| opening_006 | 264044 | 5.50 |
| opening_007 | 193964 | 4.04 |
| opening_008 | 189164 | 3.94 |
| opening_009 | 213164 | 4.44 |
| opening_010 | 87404 | 1.82 |
| opening_011 | 144524 | 3.01 |
| opening_012 | 316364 | 6.59 |
| opening_013 | 353324 | 7.36 |
| opening_014 | 170924 | 3.56 |
| opening_015 | 293324 | 6.11 |
| opening_016 | 159884 | 3.33 |
| opening_017 | 259244 | 5.40 |
| opening_018 | 181484 | 3.78 |

합계 83.20초. 전부 PCM WAV, 24kHz, mono, 16bit. 전체 프레임 읽기 성공. Unity Catalog에서 18개 실제 AudioClip 참조와 `GetData` 디코딩 성공, 원본 주파수·길이 유지. 기존 WAV GUID 유지, PCM Import, 강제 모노/정규화 끔. 외부 전체 WAV는 게임 Asset에 추가하지 않았다.

## 구현

기존 `Assets/_Project/Audio/Voice/Opening`의 18개 WAV와 meta를 교체했다. `OpeningNarrationCatalog.asset`의 안정 ID→직접 AudioClip 참조를 그대로 사용하므로 외부 폴더·순서 인덱스로 파일을 검색하지 않는다. 기존 Story·블록·자동 최소 시간·일시정지·Skip 설정 UI를 유지한다.

`OpeningIntroController`는 Next에서 진행 Coroutine을 중단하고 다음 블록을 즉시 시작한다. 마지막 블록 Next는 정상 종료한다. 기존 자동 시간은 `max(블록 Duration, Clip.length + 0.5)`이며 Fade는 별도다. Skip/종료는 즉시 음성 정리 후 비동기 Scene 전환, 비활성화/외부 전환에서도 Coroutine과 Clip을 정리한다.

`VoicePlaybackSource`는 하나의 2D AudioSource를 재사용하며 loop/playOnAwake를 끈다. 프로젝트에 AudioMixer 및 Master/BGM/SFX/Voice 음량 설정이 없어 공통 Voice `Volume`(0–1) 기반만 추가했다. 다른 음향에 영향 없이 향후 설정과 연결 가능하다. per-Clip gain, pitch, 원본 재인코딩, ducking 없음. null Clip은 자막만 계속 진행한다.

계획 Docs: `602b1b8`. Feature: `7e60218`(18 WAV+18 meta+관련 코드 2개).

## 검증 결과

실제 Unity6000.5.7f1 백그라운드 Play Mode에서 검증했다. Game View/OS 포커스 전환 없음. QA 입력은 실제 버튼과 연결된 메서드를 호출했으며 물리 키보드·마우스 입력 검증은 아니다.

- 전체 자동 진행: 156.47초, 내레이션 18개와 무음 제목 1개를 완료하고 Replay 목적지 Bootstrap 복귀, 남은 AudioSource 0개.
- 매 블록 화면 자막이 Sequence와 동일하며 opening_001→018 순서로 Clip과 IsPlaying=true를 관찰했다. 제목은 Clip 없음/재생 없음. 중복·누락 없음.
- 재생 중 001→002 Next 즉시 전환. 이어 같은 프레임 8회 Next로 010 도달, Source 1개·새 Clip 재생, 이전 Clip 잔류 없음.
- Voice Volume 0.25가 Source 음량에 반영됨. 신규 시작 모드 Skip 즉시 IsPlaying=false/Clip=null, CharacterCreation 전환 후 Source 0개.
- 두 번 재진입 모두 001부터 재생. 메모리 복제 Catalog의 001 Clip만 null로 바꾸어 자막 진행·무음 상태·기존 시간 후 002 자동 재생 확인. 공식 Catalog Asset은 수정하지 않았다.
- 002 재생 중 외부 Bootstrap 전환 후 Source 0개.
- 추가 재생에서 `AudioSource.GetOutputData`로 18개 모두 0이 아닌 출력 신호 확인. 제목은 무음이며 마지막 Next로 Bootstrap 종료. 구조적 재생 검증이며 청취 품질 판정은 아니다.
- QA 전체 관찰 코드가 종료 순간 인덱스 19를 읽으며 `MCPDynamicCode`에서 IndexOutOfRangeException 1건을 냈다. 게임 코드 예외가 아니며 검증 도우미의 범위 검사를 보강했다. null 테스트의 참조 누락 Warning 1건은 의도한 fallback 진단이다. 두 건 기록 후 Console을 정리해 최종 재조회한다.

최종 Edit Mode Console Error0/Warning0. Bootstrap clean, Play Mode 종료, 검증용 Save 경로 해제, Game View 진입 동작 복원. 사용자 save_slot_01 SHA-256 `EEB297EFDF0411FD49167124D67E7339416693458663A9E32658709E9CF37213` 유지. 검증 슬롯 파일 생성 없음. 기능·이번 문서 diff 검사는 통과한다. 전체 작업 트리 `git diff --check`는 기존 사용자 변경의 Animation/Scene/meta 등의 trailing whitespace로 exit2이며 해당 파일은 수정하거나 Stage하지 않았다. GitHub push 없음.

청취 품질·실제 발화와 문장의 일치·자동 분할 발화 경계·음악과의 체감 밸런스는 미검증이다. PCM 디코딩·자막/ID/Clip 및 출력 신호 검증을 청취 완료로 기록하지 않는다.
