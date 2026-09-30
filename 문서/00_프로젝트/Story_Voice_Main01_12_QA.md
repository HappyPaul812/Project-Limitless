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
