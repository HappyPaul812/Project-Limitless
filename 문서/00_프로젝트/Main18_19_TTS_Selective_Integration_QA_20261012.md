# Main18·19 TTS 오류 감사 및 정상본 선택적 통합 — 2026-10-12

## 결과

**Main18 정상12/12·Main19 정상13/13 Unity 연결 및 실제 Story Runtime 검증 완료. Main18 세린 오류10/10 미연결·실제 무음 확인. 전체 QA PARTIAL(기존 Unity Search 예외). 오류 음색의 원인은 미확정이다.**

시작 HEAD `5daacd5`, 사용자 미커밋337항목 보호. 이전 Field01/하렌세린 아트 작업 종료 후 기존 격리 Client 재사용, 대용량 프로젝트 복사 추가0. Unity6000.6.5f1·원본 Editor PID31832 유지. 원본 Scene/Play/종료/OS 포커스 조작0. 백그라운드 Batch의 AudioListener.volume=0으로 사용자 스피커에 QA 음성이 들리지 않게 했다. TTS API/재생성/Voice 변경/Trim·Normalize·Pitch·Speed·재인코딩/Push0.

## 1. 세린 오류10개 — 원인 미확정

- 10개는 사용자 청취에서 남성처럼 들린다고 확인한 **USER_REPORTED_VOICE_MISMATCH / REGEN_REQUIRED**이다. 음색 오류 사실과 그 원인을 구분한다.
- 실제 원본은 저장소 `Limitless_TTS_Output_Main18/`의 해당10 WAV다. _DAY1_CALL_CHECKPOINT.json의 SAVED SHA가10/10 현재 WAV와 일치한다. 출력 파일명이 Dialogue ID와 대응하고35개 전체 파일/PCM 해시가 서로 달라 동일 오디오를 다른 화자 이름으로 복사한 증거는 없다. 오프라인 ASR도 각 대사의 내용을 대체로 지지한다. ASR만으로 다른 화자의 음색 혼입을 배제하지는 못한다.
- 확보한 Main18 DAY1/v2/v3 코드의 `VOICE['companion_serin']`는 Schedar이고 synth가 이 값을 speech_config.voice_config.voice에 넘긴다. 모델 상수는 gemini-3.8-flash-tts. _TTS_RESUME_2026-10-12.json도 모델을 같은 이름으로 기록한다. Review CSV의 VoiceId 역시 Schedar다.
- **성공한 원본10개의 실제 요청 payload/VoiceId·SDK버전·응답 Voice 정보는 없다. 실제 호출 Voice ID는 UNKNOWN이다.** 코드 설정이나 Review CSV를 실제 Wire 요청 로그로 간주하지 않는다. 실제 호출 당시 Voice 선택 실패·모델/스타일 영향·서비스 내부 변동 중 무엇이 원인인지 확정할 수 없다.
- 발견된 차이: M18_v3의 세린 style은 젊고 침착한 관찰자라고만 표현하고, 정상5개·Main19 생성 후보 TTS1012_v4는 젊은 **여성**이라고 명시한다. 정상5개와Main19 원본은 Resume 기록의 SAVED hash와 대응한다. 이것은 코드의 조건 차이이며 실제 호출 옵션과 인과를 입증하는 기록은 아니다. 별도 temperature/seed 옵션은 해당 synth 코드에서 지정하지 않는다.
- wavize는 RIFF 응답이면 바이트 그대로, raw이면 MIME rate를 읽고24kHz/mono16bit로 감싼다. 두 코드의 저장 방식은 같다. 전체35개 실제 WAV도24kHz/mono16bit PCM·정상 decode·clipping0. 원본 응답 MIME/raw data 로그가 없어 생성 당시 잘못된 rate 해석까지 확정 배제하지 않는다.
- `F:/Downloads/M18_Serin10_Staged.py`는 발견해 읽기 감사했다. 후보 Voice가 Erinome/Kore인 이전 도구여서 현재 **Schedar 유지** 정책과 다르다. 실행/prepare/sample/approve/remaining을 하지 않았다. 요청한 `M18_Serin10_Schedar_v2.py`와 TTS 수정 도구 v2 ZIP은 조사한 저장소·F:/Downloads 및 Voice 경로에서 발견하지 못했다. 없는 자료를 생성해 납품본으로 취급하지 않았다.
- 현재 공식 [Gemini3.8 단일 화자 TTS 문서](https://ai.google.dev/gemini-api/docs/generate-content/speech-generation)는 voice_config.voice와 speech_metadata.style 형태를 예시로 제시한다. 따라서 이 요청 형식 자체를 잘못된 필드라고 단정하지 않았다. 제작 당시 SDK가 이 형식을 어떻게 직렬화했는지는 UNKNOWN이다. 문서 조회만 했으며 Google 생성/Voice 목록 API는 호출하지 않았다.

세린 공식 Voice 설정 **Schedar 유지**. 다음 재제작에는 실제 요청 Voice·모델·SDK버전·style/옵션·응답 MIME·원본/WAV 해시를 비밀키 없이 기록하고, 향후 명시적 생성 요청에서 샘플을 먼저 검토한다. 이번에는 원본10 WAV와 체크포인트를 변경하지 않았다. [원인 증거](Evidence/Main1819VoiceSelective_20261012/cause_audit.json)·[재제작 인계](Main18_Serin10_Schedar_Regen_Handoff_20261012.csv).

## 2. 원본 검수와 승인 단계

35개 모두 실제 로컬 WAV 존재·파일명·정본 CSV/Review의 화자·원문·SHA 대응 PASS. 25개만 사용자 정상 의견에 따른 허용 목록으로 선택했다. 나머지10개는 기계적 WAV 검사가 정상이어도 음색 오류로 제외했다. Main19는VoiceExpected=true13개만 선택, Player/지문 Import0.

사용자의 의견은 **USER_LISTENED_NORMAL / VOICE_MISMATCH**로 기록한다. 파일별 기계적 검수·Unity 연결·Runtime 재생·원문의 의미 청취 승인은 별개다. 원본 Review CSV의 과거 PENDING은 수정하지 않았다. 의미별 최종 사람 청취 승인은 별도 기록되지 않았다.

기존 캐시된 faster-whisper-small을 local_files_only와 HF_HUB_OFFLINE으로 실행했다. 35/35 ASR 비교는 대체로 대응(정상25개 모두 유사도0.8 이상)했다. 이는 의미·음색 사람 청취 승인이나 완벽한 발화 정확성 판정이 아니다. 새 모델 다운로드·TTS API 호출0. ASR만으로 확정 가능한 대사 혼입/손상은 발견되지 않아 정상25개 모두 연결했다.

추가 단어 청취를 권장하는 ASR 차이5개: main18_after_watcher_serin_01(쓰러뜨렸는데도), main18_leon_leon_03(세 병), main18_watcher_witness_miel_01(식혀야), main18_watcher_witness_paul_01(광역 열기), main19_track_1_serin_02(이르지만). 인식 오류일 수 있어 실제 오발화로 단정하거나 자동 제외하지 않았다. [ASR 원시 결과](Evidence/Main1819VoiceSelective_20261012/offline_asr.json).

## 3. 개별 상태

### Main18 — 22개

| Dialogue ID | 화자 | 상태 | Unity |
|---|---|---|---|
| main18_after_watcher_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main18_after_watcher_serin_02 | 세린 | RUNTIME_VERIFIED | 연결 |
| main18_after_watcher_serin_03 | 세린 | RUNTIME_VERIFIED | 연결 |
| main18_deeper_route_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main18_deeper_route_serin_02 | 세린 | RUNTIME_VERIFIED | 연결 |
| main18_ground_serin_01 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_heat_vent_serin_01 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_leon_leon_01 | 레온 | RUNTIME_VERIFIED | 연결 |
| main18_leon_leon_02 | 레온 | RUNTIME_VERIFIED | 연결 |
| main18_leon_leon_03 | 레온 | RUNTIME_VERIFIED | 연결 |
| main18_leon_leon_04 | 레온 | RUNTIME_VERIFIED | 연결 |
| main18_leon_serin_01 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_overheat_serin_01 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_overheat_serin_02 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_overheat_serin_03 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_overheat_serin_04 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_vibration_serin_01 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_watcher_witness_miel_01 | 미엘 | RUNTIME_VERIFIED | 연결 |
| main18_watcher_witness_paul_01 | 폴 | RUNTIME_VERIFIED | 연결 |
| main18_watcher_witness_serin_01 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_watcher_witness_serin_02 | 세린 | REGEN_REQUIRED | 미연결·무음 |
| main18_watcher_witness_taeon_01 | 태온 | RUNTIME_VERIFIED | 연결 |

### Main19 — 13개

| Dialogue ID | 화자 | 상태 | Unity |
|---|---|---|---|
| main19_cliff_miel_01 | 미엘 | RUNTIME_VERIFIED | 연결 |
| main19_cliff_paul_01 | 폴 | RUNTIME_VERIFIED | 연결 |
| main19_cliff_paul_02 | 폴 | RUNTIME_VERIFIED | 연결 |
| main19_pulse_1_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_pulse_1_serin_02 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_track_1_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_track_1_serin_02 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_track_2_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_withdraw_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_witness_miel_01 | 미엘 | RUNTIME_VERIFIED | 연결 |
| main19_witness_paul_01 | 폴 | RUNTIME_VERIFIED | 연결 |
| main19_witness_serin_01 | 세린 | RUNTIME_VERIFIED | 연결 |
| main19_witness_taeon_01 | 태온 | RUNTIME_VERIFIED | 연결 |

전체 파일별 원문·Speaker·해시·GUID·원본/Unity경로·길이·Voice설정·사용자 의견·ASR는 [개별 적용표](Main18_19_TTS_Selective_Applied_Matrix_20261012.csv)에 기록했다. 오류10개 GUID/Unity경로는 비어 있고 UnityConnected=false다. 정본 Manifest는 실제 검증25개만 RUNTIME_VERIFIED, 오류10개만 REGEN_REQUIRED로 변경했다. Player/지문 상태와 Status 외 모든 필드·본문 개행은 원문 그대로다.

## 4. Import / Catalog / 보호

`Assets/_Project/Audio/Voice/Story/Main18/` 정상12 WAV·`Main19/` 정상13 WAV만 원본 byte 그대로 복사했다. Unity API가 새 meta를 생성했다. 기존 Main17 규격 PCM/PreserveSampleRate/DecompressOnLoad/forceToMono=false/normalize=false/preloadAudioData=false를 유지했다. Unity AudioClip.GetData 전체 sample과 원본16bit PCM 비교25/25 **최대 오차0**. 새 Audio GUID25개 중복0.

실제 기존 Catalog는 Main17 보고 당시250개에서 이후 승인본까지 포함해 **255개**다. 이번255→280. **기존 Catalog 파일 byte prefix 전체를 그대로 보존**하고 끝에 정상25 entry만 추가했다. 기존 entry 순서·ID·Speaker·Clip GUID/fileID·Catalog meta 불변. 중복 Dialogue ID0·잘못된 Speaker/누락Audio0. 오류10 Catalog entry0·WAV Import0·fallback0.

기존3632개 보호 파일 중 의도한 Catalog1개 외3631개 불변·삭제0. WAV 원본·체크포인트·생성 스크립트 등 조사 입력50파일도 해시 불변. 신규52파일=WAV25/meta25/폴더meta2. Story Runtime C#·DialogueCatalog·Quest Flow·자막·전투·Party·Save스키마 변경0. 기존 Main01~17 WAV/meta/GUID·마을v4·Field01·하렌세린·사용자Save/설정 보호. [보호 결과](Evidence/Main1819VoiceSelective_20261012/protection.json).

## 5. 실제 Runtime QA

| 검사 | 결과와 범위 |
|---|---|
| Main18 정상12 | 실제 레온 Story handler 및 Field Main18Site→DialoguePresenter→VoicePlaybackSource 재생·DSP timeSamples 진행·자연 종료 PASS |
| Main18 오류10 | 실제 각 대사 Clip=null/isPlaying=false PASS, 다음 페이지 진행·레온 정상 Clip 복구 PASS |
| Main19 정상13 | 실제 Main19Site에서 원문·화자·ID·Clip 일치·재생·DSP·자연 종료 PASS |
| 조건부3동료 | 폴/미엘/태온 활성 편성별 실제 발화·미편성 페이지 없음 PASS. 파티를 영구 강제 변경하지 않음 |
| 페이지/자연 종료 | 실제 Clip 종료 뒤 같은 페이지/목표 유지, Next만 진행 PASS |
| Mixer/Mute | Voice100/40/0 dB·Source volume1/pitch1/non-loop/Voice routing·MasterMute -80dB·채널 보존/unmute PASS |
| 중복 음성 | 단일 Voice Source·빠른 Next→Player 무음·다음 정상 최신 Clip만·Scene 전환 잔류0 PASS |
| Save/Continue | 실제 Save→Bootstrap 슬롯 버튼→좌표/Quest/Party 복원2지점 PASS. Continue 자동 발화/잔류0 |
| 기존255 승인 entry | 전부 Resolve+AudioSource 짧은 재생 PASS, 기존 원본 보호. 전체255개 의미 청취/자연 종료를 재검사한 것은 아님 |
| Main18·19 전투/Quest | 실제 지정 Story 진입·승리 처리/Return 버튼·목표1회 진행·Voice 정리 PASS. 승리는 QA에서 EndBattle 경로 호출, 전체 정상 전투 입력/밸런스는 미검증 |
| Missing/Console | 검사한 Arbel/Field09/Field10/Bootstrap/Battle Missing Script0·검사한280 Catalog MissingAudio0·CompileError0·신규Runtime오류0 |
| 전체 QA | 기존 Unity Search 예외가 재발해 PARTIAL. 기존 알려진2건 별도 미해결 유지 |

최초 실행은 목격 대화 종료의 자동 Battle 전환을 QA가 처리하지 않아 다음 Field 계측에서 NullReference로 FAIL했다. 게임 오류나 정상 음성 손상은 아니다. Main18 정상10개·오류10개 무음·Mixer·Continue PASS 증거를 보존하고 나머지 조건부2개/전체Main19/경계/회귀부터 별도 디렉터리에서 재개했다. 최초 FAIL 로그를 삭제하거나 전체PASS로 덮어쓰지 않았다.

오디오 엔진의 Clip/DSP/Mixer 검사이며 AudioListener.volume=0으로 OS 청취를 하지 않았다. 사용자 청취 의견을 자동 검사로 대신하지 않는다. 원본 Editor를 강제 Import/Play 하지 않아 원본 창의 새 Import 완료는 미검증이며, 동일WAV/meta/GUID/entry의 격리 검증을 사용했다.

원시 로그: Temp/Main1819VoiceQA/import.log·runtime.log·runtime_remaining.log. 테스트 Save/Settings: 기존 격리 Client의 Main1819VoiceResults/Main1819VoiceRemaining 아래. Catalog/게임 파일과 혼동하지 않는다. [첫 검사](Evidence/Main1819VoiceSelective_20261012/runtime_first_checks.txt)·[잔여 검사](Evidence/Main1819VoiceSelective_20261012/runtime_remaining_checks.txt)·[PCM/GUID](Evidence/Main1819VoiceSelective_20261012/import_report.tsv).

## 6. 다음 작업·Git

세린 오류10개만 Schedar로 향후 재제작 및 청취 검토한다. 기존 정상25개는 이미 게임 연결 완료. 원인 UNKNOWN 때문에 정상 적용을 보류하지 않았다. 의미 단어 확인5개는 ASR 차이 검토 대상으로 별도 기록한다.

선별 Commit: 신규52파일, Catalog 정상25개 append, Manifest Status35개, 본 보고서/개별 적용표/인계/Evidence와 CURRENT_STATUS 이번 요약. 사용자 기존337개 미커밋 변경과 원본 TTS 출력/스크립트는 제외. git add -A/reset/clean/checkout/기존음성변경/Push0. GitHub Push는 사용자 직접 수행.

Evidence 텍스트 SHA256은 Git blob LF기준이며 원시 로그는 그대로 유지했다. Unity가 생성한 신규meta의 빈 YAML scalar 후행공백은 원본 직렬화를 보존하고 해당 공백만 제외해 Git 검사한다.

선별 기능 통합 Commit: `e83393ceabc74c38dc49661969c1dd5e68bf06c0`. CURRENT_STATUS는 이 기능 SHA를 기록하는 별도 Docs Commit으로 갱신하며 기존 사용자 MCP 기록은 미스테이징 유지한다.
