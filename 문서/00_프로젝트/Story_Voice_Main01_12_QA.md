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
