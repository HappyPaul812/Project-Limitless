# Missing Story Voice 보충팩 적용과 검증

> 2026-10-05 후속 전수 감사: 기존 기술 Mapping/원본 해시 PASS는 WAV 발화 의미 PASS가 아니다. 사용자 청취로 main03_taeon_supp_005/006/007의 내용 불일치를 확인했다. 006은005 문장, 007은006 첫 문장만 말하고 종료한다. 현재 원본은 보존하며 재생성 필요3개와 다른 미청취 Voice는 [Story 정합성 QA](Story_Dialogue_Consistency_QA.md)를 따른다.

## 입력과 병합 정책

- Source Folder: `F:/study/codex/Project-Limitless/Limitless_TTS_Output_Missing_Main01_05_08_12_v2`
- 정식 Manifest: 위 폴더의 `dialogue_manifest_missing_main01_05_08_12_v2.csv`. PREVIEW CSV나 파일 순서를 연결 기준으로 쓰지 않는다.
- 정식 CSV 58행, 입력 WAV 61개. 실제 대상은 Main03 12/Main04 18/Main05 6/Main08 19/Main12 3개, 합계246.12초다. Main01/02는 제공 행이 없으며 일반 NPC/Player 음성은 제공되지 않았다. Manifest 미참조 WAV3개는 Import하지 않는다.
- **Existing Voice Wins**: 기존 Clip이 있는 ID는 충돌로 제외하며 기존143개 매핑·Character·WAV/Meta·GUID·Intro18개·BGM을 보존한다. Catalog를 재생성하지 않고 새 항목만 추가한다. 원본 WAV 바이트를 그대로 복사하며 Trim/Normalize/Pitch/Speed/Re-encode를 하지 않는다.
- 사용자 추가 허용: LOCAL 기존 무음 대사와 본문·화자·소스 위치가 정확히 일치하면 CSV의 `*_supp_*` 안정 ID를 새로 지정할 수 있다. 기존 ID 변경은 허용하지 않는다. Runtime은 배열 번호/CSV 행 번호로 연결하지 않는다.
- Main03 구형 페이지의 `태온\n`은 기존 화면용 화자 표기다. CSV는 그 다음 발화 본문이다. 기존 화면 문자열·순서·Player 페이지를 그대로 보존하고 발화 본문과 화자를 대조한다. 문구를 TTS에 맞추어 바꾸지 않는다.
- Main08은 명시적인 보충 대상이다. Main06~11 기존 매핑 불변 조건은 기존 Main06/07/09/10/11의102개 보존과 Main08 신규19개 추가로 적용한다. Main13~15 기존41개는 보존한다.

## Character Voice Registry

이번 CASTING은 태온 companion_taeon/Gacrux34개·미엘 companion_miel/Sulafat22개·폴 companion_paul/Achird2개다. 모델 `gemini-3.8-flash-tts`, voice_design=false, purpose=missing_story_voice_supplement다. 기존 Registry와 동일 Voice ID를 사용하고 기존 배치의 모델 정보를 소급 변경하지 않는다. Narrator/세린/레온 변경 없음. Tone/속도/기본 음색은 기존 TBD를 유지한다. 개발 적용과 실제 청취 채택을 구분한다.

## 적용 후 Coverage

기존 QA와 동일한 LOCAL 정적 대화 페이지 정의228쪽을 기준으로 계산한다. 일반 NPC/Player/Path 변형도 분모에 포함한다. 이번58개는 서로 다른 페이지이므로 신규 Mapping과 신규 음성 페이지 수가 같다. Main12는 공유 Chapter2 코드 전체가 아니라 Main12 조사3쪽/잡화상인1쪽만 센다.

| Quest | 총 Dialogue | 기존 Voice | 신규 보충 | 최종 Voice | Voice 없음 | Coverage |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| Main01 | 10 | 0 | 0 | 0 | 10 | 0% |
| Main02 | 7 | 0 | 0 | 0 | 7 | 0% |
| Main03 | 16 | 0 | 12 | 12 | 4 | 75% |
| Main04 | 22 | 0 | 18 | 18 | 4 | 81.82% |
| Main05 | 14 | 0 | 6 | 6 | 8 | 42.86% |
| Main06 | 4 | 3 | 0 | 3 | 1 | 75% |
| Main07 | 40 | 26 | 0 | 26 | 14 | 65% |
| Main08 | 23 | 0 | 19 | 19 | 4 | 82.61% |
| Main09 | 32 | 27 | 0 | 27 | 5 | 84.38% |
| Main10 | 25 | 18 | 0 | 18 | 7 | 72% |
| Main11 | 31 | 28 | 0 | 28 | 3 | 90.32% |
| Main12 | 4 | 0 | 3 | 3 | 1 | 75% |
| 합계 | 228 | 102 | 58 | 160 | 68 | 70.18% |

## 사전 검증과 구현 계획

58행 모두 LOCAL 화자/본문/소스 위치 및 WAV에 대응한다. Existing Voice 충돌0·Text mismatch0·Speaker mismatch0·Missing Audio0·Missing Dialogue0·Duplicate ID0·범위 밖 Row0이다. 새 ID 미지정58개는 위 사용자 허용에 따라 지정한다. 입력61 WAV 중 Manifest58개만 적용한다.

Unity 실제 Catalog143개 Missing Clip0, GUID `9ccf2395ba242ea409bc5d845ae76679`, Bootstrap clean Edit Mode를 확인했다. 구현 전 기존 Voice/Resources Audio/Audio 코드 SHA-256 및143개 ID→Clip GUID→Character 기준을 확보한다. 실제 적용/보존 감사/컴파일/Runtime 검증은 아래 결과가 기록되기 전에는 완료로 간주하지 않는다.

기존 DialoguePresenter/VoicePlaybackSource/Voice Mixer를 그대로 사용한다. Main03 구형 문자열 페이지만 명시적 DialogueLine 데이터로 전환하고 기존 화면 메시지·화자 표시·Quest 진행 콜백을 유지한다. Main04/05/08 helper의 선택적 ID 및 Main12 조사 ID만 추가한다. Scene/Quest/Save/BGM 변경 없음.

## 검증 계획과 청취 QA

포커스 전환 없는 Unity Import/Catalog/컴파일/Console 및 격리 Save/Settings를 사용하는 Background Play Mode를 검증한다. Main03 또는04·Main05·Main08·Main12 신규 샘플, 기존 Intro/Main07/Main09/Main11/Main13~15 한 구간의 회귀, Next/연속 Next·Voice Volume·Mute·종료/Scene 정리를 확인한다. Main01 신규 샘플은 입력 미제공으로 불가능하며 기존 텍스트 fallback을 확인한다.

발음·감정·호흡·캐릭터 취향·실제 음량 균형·전체 청취는 사용자 QA 대상으로 남긴다. 기술적 참조 및 출력 검증으로 청취 완료를 선언하지 않는다.

## 실제 적용 및 최종 검증

- 정책 문서화 commit: 5814d34. 58개 ID를 허용 범위에 따라 새로 지정하고 원본 WAV58개만 Import했다. 기존143개 뒤에58개를 추가한 Story Catalog201개다. Existing Voice 충돌/Clip 교체/ID 삭제/GUID 변경/Character 변경 모두0. Text mismatch/Speaker mismatch/Missing Audio/Missing Dialogue/Duplicate ID/범위 밖 Row도 모두0.
- 원본 PCM24kHz/mono/16bit를 유지하고 Unity PCM/원본 SampleRate/forceToMono OFF/Normalize OFF로 Import했다. WAV58개 원본 바이트 동일·GUID58개 고유·Clip 참조/디코딩/비무음 확인. 기존 음성/Meta/Resources Audio/Audio 코드353파일 해시 동일. 별도 기존 사용자 수정73파일 해시 동일. 기존 대사의 화자/본문/페이지 순서도 보존했다.
- 실제 Background Play Mode에서58 ID→화자→Clip을 확인하고58회 연속 Next·끝 콜백·Hide 후 Clip/재생 정리·무음 fallback을 통과했다. Main03/04 FirstConversation, Main05 GuardReport, Main08 Lines, Main12 조사 Line/VoiceId를 통해 대표 샘플을 생성해 Voice Mixer 실제 출력까지 확인했다. Main12는 조사 본문/ID 생성 및 Presenter 재생을 확인했으며 전체 Quest 상호작용/플레이 진행은 미검증이다.
- Main01 신규 Sample은 음성 미제공으로 미검증 대상이다. 기존 주민 대표5쪽의 무음 진행/완료/정리 fallback은 확인했다. Main02도 신규 입력이 없으며 음성을 임의 생성하지 않았다.
- 기존 Main07 PaulFirst/Main09 Lines(0)/Main11 Lines(1)/Main13 세린 NPC의 샘플 연결/출력 유지. Intro Storyteller opening_006→007 실제 Next·음성 출력·Replay 종료→Bootstrap·잔류 Voice Source0 회귀 통과. 신규 Main12 태온 음성 재생 중 Scene 전환 후 Source0도 확인했다.
- 같은 Main03 샘플의 출력 RMS peak: Voice100=0.180369, Voice40=0.072519, Voice0=0, 전체 Mute=0, Unmute=0.180369. 모두 Voice Group·Source gain1·채널값 유지. 짧은 샘플은 측정 종료 전에 자연 재생 완료할 수 있으며 출력 수집과 시작 시 IsPlaying을 함께 확인했다.
- 게임 컴파일 오류0·최종 Console Error0/Warning0. 재컴파일 중 MCP WebSocket 초기화 Warning1은 후속 조회에서 사라졌다. 임시 QA 코드의 API namespace/delegate 타입 및 Main11의 무음 step0 선택을 고쳐 다시 실행했다. 프로젝트 동작 수정은 필요하지 않았다.
- Foreground/Game View 활성화/OS 입력 없음. 격리 Save/Settings 종료·원래 백그라운드 설정/PlayFocused 진입 옵션 복원·Bootstrap clean Edit Mode. 실제 청취의 발음/감정/호흡/캐릭터 취향/음량 균형과 전체 Quest playthrough·물리 입력·시각 QA는 미완료다.
- 상세 신규58개 ID/화자/본문/소스/Asset/GUID/SHA-256 및 Runtime 출력 근거: [감사 JSON](Missing_Story_Voice_Supplement_Audit.json). 제작 Output61 WAV는 Stage하지 않고 실제 사용하는58 WAV/Meta·직접 연결에 필요한 C#5개·Catalog·문서만 커밋한다.

- 기능 commit: 8756399. 최종 관련 변경131파일(신규 WAV58·WAV Meta58·폴더 Meta5·직접 연결 C#5·Catalog1·문서4). 이번 변경 diff 검사 통과, 기존 사용자 변경93항목은 보존하며 GitHub push 없음.
