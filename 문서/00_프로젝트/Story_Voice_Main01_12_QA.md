## 2026-10-06 미엘 첫 조우 전체10페이지 감사 / 원본001 누락 확정

[전체 QA](Miel_First_Encounter_Voice_QA.md)·[순서10행](Miel_First_Encounter_Sequence_Audit.csv)·[확정 TTS전달](Miel_First_Encounter_TTS_Handoff.csv).Main04 FirstConversation,미엘6/태온3/Player1.문제 main04_miel_supp_001 정식본문 `조금만 참으세요. / 출혈은 멎었습니다.` 중사용자Runtime와직접WAV모두첫문장만청취:CaseA PARTIAL_AUDIO_CONTENT.2초원본전체PCM48000오차0/자연종료/자동Next·조기Cleanup재현0.이번TTS생성/Playback수정/WAV수정0,정확한전체본문으로재생성전달.

연속148assertions PASS/FAIL0,9Voice전PCM정합/Player Voice0·Portrait0/MielPortrait·UI/최종Cleanup·첫대화종료→다음Encounter Objective·격리Save읽기PASS.의미정상확정0/Partial확정1/나머지8 USER_LISTENING_REQUIRED;Metadata정상으로의미PASS추정금지. **TTS_REGEN_REQUIRED0→1**,NEEDS_LISTENING244기존관리범위유지,Main03태온008~012완료보호.Compile/최종ConsoleError0·Warning0.3042기존파일해시변경0,새EditorQA만추가/cleanBootstrapEditMode/포커스전환0.

다음:미청취8건전체발화확인→확정누락001원본재제작→사용자청취/기존GUID보존교체·회귀.마지막관련commit `5e660dba9fac846932eb3e075735a22e19ff6fa9`,이번감사commit은최종보고참조.직접변경만커밋·GitHubPush없음.아래는이전이력이다.

## 2026-10-06 Main03 008~012 최종 반영·연속 Runtime 사람 청취 PASS

008/010/011 새 원본·009 **NeFix 최종본**을 기존 Unity WAV 내용만 교체.012는 지정 원본과 이미 동일하여 재작성0/해결유지.5건 Source 사용자청취PASS + 이번 사용자 **“5개 모두 Runtime 청취 PASS”** 확인: 의미·전체문장·음량PASS,009 `네.` 포함.**TTS_REGEN_REQUIRED4→0**, NEEDS_LISTENING244 기존관리범위유지.331 Matrix에 완료flag 기록.005/006/007/003/004 기존PASS와001/002 보호,Player 정상무음.

LOCAL Runtime/Manifest 본문5건 줄바꿈 포함일치·Source/Unity WAV SHA동일·실제Unity PCM5개 전샘플오차0.실제 첫Battle→QA전용승리종료→정식복귀→008→Player→009→Player→010→011→012→단서001→002→Main03종료 연속PASS.전략전투/물리입력검증 아님.기존 QA 도구로 PlayUnfocused·격리Save/Settings 사용,포커스전환0.기술검사 227 PASS/FAIL0(7 TRACE·9 AUDIO는별도),Player Voice0/Portrait0·태온복원·UI Wrap/동적높이/본문잘림0/Footer·Portrait비중첩·끝까지Next/빠른Next/최종정리/Quest완료/격리자동Save읽기PASS.

[최종 적용QA](Main03_Supp008_012_Final_적용_QA.md)·[5건 Source/PCM 감사](Main03_Supp008_012_Final_Audit.csv)·[이번 연속 실행로그](Main03_Supp008_012_Final_Runtime_Results.txt).보호 3042파일 비교:변경은 Unity008~011 WAV4개뿐,meta/GUID/Importer/Catalog/게임·Editor C#/Save/Settings/Scene/Packages불변.종료cleanBootstrap EditMode/audit해제/격리설정·Play옵션복원.CompileError0/ConsoleError0·Warning0 조회;C#변경없어강제재컴파일0.초기 준비스크립트 실행실패 후 조기QA를중단했고 WAV교체후전체재실행PASS,조기실행은최종증거에포함하지않음.

다음 권장:244 관리범위의미확인Voice를별도청취.이번5건추가TTS불필요.마지막선행관련commit `e355b44c9b2a87b7072d504d95db33560ebf7b7c`,이번완료commit은최종보고참조.직접변경만Stage,원본TTS폴더/다른WAV Stage0·GitHubPush0.아래는이전이력이다.

## 2026-10-06 Main03 008~011 사용자 Runtime 의미 불일치 확정·TTS 전달

사용자 실제 Runtime 청취에서 008~011 모두 자막과 발화 불일치 확인. 네 건 모두 **WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**이며 Metadata 정상 여부로 의미 판정을 되돌리지 않는다. **TTS_REGEN_REQUIRED 1→4**, NEEDS_LISTENING **244는 기존 관리 범위**로 유지(이번 확정 문제/이미 해결된 항목을 포함하므로 미확인 244건이라는 뜻 아님). 012는 새 WAV·사용자 직접 청취·Unity Runtime 의미/전체 문장/정상 음량 PASS 해결 유지. 005/006/007/003/004 PASS와 001/002 기존 상태, Player 정상 무음 보호.

LOCAL MainQuest03FieldFlow.AfterBattleConversation 및 DialogueLine의 구형 화자 접두어 제거 규칙에서 전체 표시 본문을 추출한다. 현재 Manifest와 네 건 모두 줄바꿈까지 일치. 실제 AfterBattle 페이지 순서는 1/3/5/6(index 0/2/4/5); Player 페이지를 제외한 전달 4행이다. 009는 `네.` 뒤 줄바꿈을 보존한다. Actual Spoken Text는 **USER_CONFIRMED_MISMATCH**로 기록하며 잘못된 문장을 추측하지 않는다. 이전 008 직접청취의 별도 증거는 보존한다.

정본 전달: [4행 Handoff](Main03_Taeon_Supp008_011_TTS_Handoff.csv), [원본/참조 감사](Main03_Taeon_Supp008_011_Voice_Audit.csv), [공통 연기/설정/검증](Main03_Taeon_Supp008_011_TTS_Handoff_QA.md). 기존 7행 Remaining Handoff는 원본 청취 감사 목록이며 새 재생성 전달에는 이 4행만 사용한다. 문서화→감사/전달 생성→백그라운드 정적 검증 순서. WAV/meta/Unity 코드/Catalog/설정 변경 및 TTS API/Push 0. 이번 Editor/Play 재실행 없음; 새 음성 제공 후 직접/Runtime 의미 청취가 다음 작업이다.

마지막 선행 관련 commit: `bd4bffeca1efb1789079197f3b215e19ddb0fe48`. 이번 문서 commit은 최종 보고 참조. 아래 기존 기록은 당시 이력이며 현재 판정은 이 단락과 최신 Matrix/재생성 목록을 따른다.

# Main01~12 Story Voice 적용

## 입력과 적용 기준 — 2026-09-30

입력은 `F:/study/codex/Project-Limitless/Limitless_TTS_Output_PreSerin`이다. CSV `dialogue_manifest_main01_12.csv`의 화자·본문·source_file을 LOCAL 코드와 정확히 대조한다. 파일 순서나 source_pos만으로 추측하지 않는다. CASTING.json의 Voice ID를 사용하며 모델·기본 속도·Tone·제작 Batch는 자료가 없어 TBD로 둔다.

| Character Stable ID | 이름 | Voice Design / 제공 ID | 적용 수 |
| --- | --- | --- | ---: |
| companion_taeon | 태온 | Gacrux / Gacrux | 28 |
| companion_miel | 미엘 | Sulafat / Sulafat | 22 |
| companion_paul | 폴 | Achird / Achird | 52 |

위 Design은 CASTING의 display_name이다. 음색·연기 지시를 임의로 확정하지 않는다. 개발 적용을 채택하되 실제 청취 품질은 미검증이다. Narrator Storyteller_4와 Intro 18개는 기존 Registry를 유지한다. 세린 companion_serin은 모두 TBD·미적용이다.

107개 WAV 중 대사 segment는 104개, 캐릭터별 연결본은 3개다. CSV가 Main12로 표시한 미엘·태온 각 1개는 LOCAL `field05_main14_strong_pulse`의 Main14 대사다. 이 2개와 연결본 3개는 Import/Mapping에서 제외한다. TEXT_AUDIO_MISMATCH는 0개이며 정확히 대응하는 102개만 적용한다.

Main01~05·Main08·실제 Main12 음성은 제공되지 않았다. 기존 본문을 보존하고 선택적 Voice 없는 텍스트 대화로 진행한다. 통계는 해당 Main Flow의 정적으로 작성된 대화 쪽/분기 본문을 세며 재시도에 다시 작성된 쪽도 포함한다. Main12는 명시적 조사 3종과 잡화상인 1종이다. 공용 NPC 전체 대사를 포함하는 전 게임 통계는 아니다.

## 연결 계획

원본 WAV를 `Assets/_Project/Audio/Voice/Story/MainXX/`로 바이트 그대로 복사한다. Unity Import는 PCM·원본 샘플레이트·강제 모노 변환 및 정규화 없음으로 구성한다. 속도/pitch/trim/gain/EQ/재인코딩·새 TTS 생성은 하지 않는다.

기존 DialogueLine에 선택적 Dialogue ID를 추가하고 기존 VoiceClipCatalog 및 VoicePlaybackSource를 재사용한다. Manifest clip_id를 stable ID로 명시하며 Catalog의 Character ID까지 확인한다. 한 쪽=한 Clip인 제공 자료이므로 별도 segment 재생 구조는 추가하지 않는다. Next/연속 Next는 같은 Source의 현재 Clip을 즉시 교체한다. 수동 Next 정책은 유지하고 음성이 끝나도 대화를 자동 진행하지 않는다. Hide·단일 대화/확인창 전환·비활성화·Scene 제거에서 음성을 정리한다. Voice Mixer와 사용자 설정만 사용하며 Story Save에 Voice 상태를 추가하지 않는다.

## 검증 계획

재현 감사 도구는 `Tools/TTS/audit_story_voice.py`, 결과는 `Tools/TTS/story_voice_audit.json`이다. Import 디코딩/GUID/참조/중복 및 원본 SHA-256을 검증한다. 백그라운드 Play Mode에서 인물별 샘플, 실제 대사 factory, Next/연속 Next/null fallback/종료, Voice Volume/Mute 복원, Intro Next/Skip/18개 참조 회귀를 확인한다. Scene·Save·사용자 설정은 검증 전 상태로 복원한다. Game View 활성화나 foreground 검증은 현재 작업에서 별도 허락 없이는 실행하지 않는다.

실제 청취에서 발음·감정·호흡·문장 발화 일치·컷 경계·대사 간 음량 균형·음성/자막 타이밍은 사람이 확인해야 한다. 메타데이터와 자동 상태 검증을 청취 검수로 간주하지 않는다.

## 적용 결과

- 전체 입력 107개: PCM WAV 24kHz/mono/16bit, 합계1116.56초. Segment104개558.28초, 연결본3개558.28초. 실제 Import102개550.97초. 원본 바이트102개 동일, 전체 입력107개 프레임 디코딩 성공. 신규 GUID102개 중복 없음, Catalog 참조102개 정상. Import PCM/원본 rate/normalize0/forceToMono0, 오류0.
- `verify_story_voice.py`로 변경한5개 Flow의 모든 정적 화자/본문 순서가 Git 기준과 동일함을 검증했다. Mapping ID102개, 중복0, TEXT_AUDIO_MISMATCH0. 미연결2개는 Main14 범위 제외이며 연결본3개는 개별 Dialogue Clip이 아니다.

| Main | 대화 쪽 | 입력 Segment | Mapping | Voice 없음 | Mismatch | Unmapped |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| 01 | 10 | 0 | 0 | 10 | 0 | 0 |
| 02 | 7 | 0 | 0 | 7 | 0 | 0 |
| 03 | 16 | 0 | 0 | 16 | 0 | 0 |
| 04 | 22 | 0 | 0 | 22 | 0 | 0 |
| 05 | 14 | 0 | 0 | 14 | 0 | 0 |
| 06 | 4 | 3 | 3 | 1 | 0 | 0 |
| 07 | 40 | 26 | 26 | 14 | 0 | 0 |
| 08 | 23 | 0 | 0 | 23 | 0 | 0 |
| 09 | 32 | 27 | 27 | 5 | 0 | 0 |
| 10 | 25 | 18 | 18 | 7 | 0 | 0 |
| 11 | 31 | 28 | 28 | 3 | 0 | 0 |
| 12 | 4 | 2 | 0 | 4 | 0 | 2 |
| 합계 | 228 | 104 | 102 | 126 | 0 | 2 |

백그라운드 실제 Play Mode에서102쪽을 같은 프레임에 Next로 진행해 ID/Clip/Character/Voice Group과 마지막 정리를 확인했다. Missing ID·다른 화자 ID는 Clip 없이 자막과 Advance로 진행한다. 실제 Main07 PaulFirst·Main09·Main10·Main11 factory와 Main01 배열·Main02 반응/결론·Main05 보고의 무음 진행/완료 callback을 확인했다. Main06은 감사 대응 데이터로 재생했으며 OnInteract의 실제 입력·퀘스트 전체 진행 검증은 아니다. Main12 조사 fallback은 실제 Line factory로 확인한다. 대화 Hide/단일 Show/확인창/비활성화 및 재생 중 Scene 전환으로 Story 객체 제거를 확인했다.

AudioListener 출력 RMS peak는 Main06 태온0.25854/Main07 폴0.14828/Main09 미엘0.01681/Main11 폴0.04523이다. Voice0은 Mixer -80dB이며 전환 포함 peak0.00006305, Mute는0, 복원은0.18701이다. Voice0에서도 Source의 재생 시간은 진행하며 텍스트는 수동 Next를 기다린다. 전체 Mute는 Master -80dB/해제0dB, 채널72/63/47 값을 유지한다. BGM/SFX 정식 음원 신규 도입은 없다.

Intro 18개 Catalog 참조, 실제8회 연속 Next의 상대 인덱스/Clip, Voice0/Mute/복원, Skip→CharacterCreation 및 잔류 VoiceSource0을 확인했다. 최초 QA 도우미는 Scene 진입 후 자동 진행된 시간을 무시하고 Next가002라고 고정 가정하여 실패했다. 실제 현재 인덱스 기준으로 수정한 도우미로 재검증했으며 게임 코드 오류가 아니고 최종 Console Error/Warning0이다.

격리 Save/Settings 경로로 검증했고 정상 경로·Game View 진입 동작·runInBackground를 복원했다. Bootstrap clean Edit Mode로 종료하며 OS 포커스/Game View 활성화는 하지 않았다. 사람의 실제 청취·입력·시각 QA 및 전체 퀘스트 playthrough는 미검증이다. Main13 이후·세린은 적용하지 않았다.
