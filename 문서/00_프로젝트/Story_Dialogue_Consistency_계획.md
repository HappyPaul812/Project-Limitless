# Story Dialogue / Subtitle / Voice / Portrait 전수 감사 계획

2026-10-05 LOCAL 구현을 기준으로 문서화 → 최소 Runtime 수정 → 백그라운드 회귀검증 순서로 진행한다. 이전 Voice QA의 PASS와 MP4의 음성 파형 일치는 실제 WAV 발화 내용의 의미 일치 판정으로 재사용하지 않는다. GitHub 조회/Push와 TTS 생성은 하지 않는다.

## 정식 화자 규칙

- Player는 고정 Voice/TTS 없음, Portrait 없음. NPC/Narrator/기본 남녀 Portrait 또는 Voice fallback 금지.
- 새 Player/무음 페이지는 이전 Voice를 즉시 Stop/Cleanup하고 Sprite를 clear하며 Portrait 영역을 숨긴다. 다음 Named NPC 페이지는 자신의 등록 Portrait만 다시 표시한다.
- Named Character는 stable Speaker ID와 Catalog의 Speaker 제한을 모두 대조한다. 미제작 Portrait는 기존 null-safe 글자 배치를 유지하고 감사에 기록한다.
- 구형 Main03은 `대화` 표시 이름과 본문의 `태온\n`/`플레이어\n`을 사용한다. 현재 열린 실제 Player 페이지에서 Speaker ID가 companion_taeon, Portrait가 Portrait_Taeon인 것을 읽기 조회로 확인했다. Player 페이지 Clip은 null/IsPlaying=false였으므로 이를 음성 잔류 원인이라고 단정하지 않는다.

## 전수 Matrix와 근거

구현 전에 Intro18와 Main01~16의 정적 작성 대사, 무음 Player, 관찰, 재시도, Path 분기를 수집한다. 반복 플레이 횟수가 아닌 작성된 페이지를 센다. 공용/동적 fallback은 별도 식별하여 누락시키거나 임의 본문을 만들지 않는다. Matrix에 실제 Source 위치/분기/순서, raw Speaker/Subtitle, 의미상 Speaker, 문서 정본, Manifest ID/Speaker/Text, Catalog/GUID/파일/원본 Hash/Duration/rate/channel, Portrait, 상태와 Notes를 기록한다. Voice ID 없는 무음 대사의 Source 위치 키는 감사용 키이며 새 게임 ID를 만들지 않는다.

문서에 요약만 있고 정확한 대사 원문이 없으면 DOC_TEXT_NOT_SPECIFIED로 구분한다. 문서의 명시적 최신 문장과 충돌하면 DOC_RUNTIME_DRIFT로 보고하며 Voice에 자막을 맞추지 않는다. 실제 Runtime 본문 원문 부록을 작성할 때에도 기존 Story 기획/본문을 임의로 변경하지 않고 원래 문서와의 비교 근거를 남긴다.

Source manifest 네 종류와 Intro 제작 CSV를 대조한다. PreSerin의 Main12 표시 두 행이 실제 Main14의 제외 입력인 사실, Supplement Existing Voice Wins, Main13~15 Retrofit 설계 후보와 Main16 실제 Hearing/Default 분기를 구분한다. 정확한 ID/화자/파일 대응만 자동 판정한다.

## 수정·검증·보호

Player 차단은 DialogueLine/Presenter/Portrait Resolver의 중앙 경로에서 처리한다. 개별 Flow마다 조건문을 추가하지 않는다. 실제 Clip 오연결/ID 충돌은 증거가 있는 항목만 수정한다. WAV 발화가 의심되는 항목은 NEEDS_LISTENING이며 메타데이터만으로 WRONG_CLIP이나 STALE_TTS를 확정하지 않는다. 확정된 재생성 필요는 별도 TTS_REGEN_REQUIRED 목록으로 넘기고 이번 작업에서 음성을 생성/가공하지 않는다.

Voice/Mute/BGM/SFX 값과 Mixer 원본, WAV/meta/GUID, Scene, 기존 사용자 변경/Save는 보존한다. 자동 검증은 격리 프로필/PlayUnfocused로 시행하며 사용자가 현재 진행하는 Play 대화는 먼저 보호한다. Next/연속 Next/NPC→Player/연속 Player/Player→NPC/Hide/Scene 전환, 음량/Mute의 기존 동작을 검증한다. 실제 Story 전체 수동 플레이는 하지 않는다.

백그라운드 감사 후 실제 발화 의미를 들어야 하는 후보 Clip 수와 이유를 제시하고 이번 작업의 새 청취/Foreground 승인을 받은 뒤에만 청취한다. 승인 전에는 NEEDS_LISTENING으로 남긴다. 이전 녹화의 Foreground 허락을 재사용하지 않는다.
