> 2026-10-08 Main19 정식: Field10 탐색·복귀 Paths of Cracked Earth, 일반/Story전투 Blade and Gambit. 신규곡/보스곡/Main20곡 선사용0. 기존 Source/Fade/Loop/Mixer 재사용. 아래 Main19 후보는 이전 이력.

> 2026-10-08 Main18 후속 확정: Field09 탐색/복귀: 기존 The Weight of Obsidian MP3. Field09 일반·Tutorial Beetle·작열 감시자 Elite: Blade and Gambit. Elite 전용곡/ Iron and Incantation 배정 없음. Paths of Cracked Earth는 Main19 후보 유지. 기존 Source1/Fade/Loop/Mixer 유지. [정식 계약](../03_스토리/Chapter2_Main18_검은_열기.md). 아래 Main18 후보/TBD는 이전 이력이다.

# Chapter2 BGM 정본 — 2026-10-06

사용자 이번 확정이 이전 null/TBD 기록을 대체한다. 원본 음악은 편집하지 않는다.

| 역할 | 곡 |
| --- | --- |
| Chapter1 일반전 | Steel and Sunlight |
| Chapter2 서부 Field04~08 일반/Story Battle | Blade and Gambit |
| Field07 탐색/복귀 | Where the Earth Breathes |
| Field08 탐색/복귀 | Beneath The Cracked Earth |
| 침묵의 지하묘지 파수꾼 | The Warden’s Final Stand |
| 향후 마법형 강적/중간보스 후보 | Iron and Incantation |
| 후반 중요 Boss 후보 | The Weight of Crowns |

Blade 원본 `F:\Downloads\bgm\Blade_and_Gambit.mp3`, Runtime `Assets/_Project/Resources/Audio/Music/Blade_and_Gambit.mp3`. SHA256 `26653c0ed2b91a8c6fbc16efcbe014d856decad54a4cc309eac33f953c103cc0`,4341985bytes, Unity AudioClip180.662857초. 이번 검색에서 정확한 원본1개. Field07의 실제 AudioImporter 정책을 복사: Streaming / Preserve sample rate / Vorbis quality1 / preload false / stereo 유지. MP3 원본과 등록본 byte 동일.

파수꾼 Encounter ID의 전용곡을 최우선으로 한다. 나머지 isBoss=true는 공통곡을 강제하지 않는다. 현재 전용 특수 Encounter 계약은 변경하지 않는다. 미래 Boss 배정은 이번 범위가 아니다.

기존 BgmPlaybackService의 persistent AudioSource1·BGM Mixer를 유지한다. Battle 진입은 필드곡 Stop 후0.20초 FadeIn. Battle에서 필드로 Scene 전환하면 기존곡0.75초 FadeOut → Stop/clip교체 → 필드곡0.9초 FadeIn. 한 Source의 순차 전환이므로 두 곡 동시 재생은 없다. 승리 결과 화면 동안 Battle곡 유지, 복귀 클릭 이후 Fade가 끝나면 잔류 없음. Time.unscaledDeltaTime 사용. Source.volume은 전환 gain만, 사용자 음량은 Mixer에서만 적용한다. 새 요청/Continue/Scene 전환/Disable 시 이전 Fade를 취소한다. LoopON. 음악적 자연스러움은 USER_LISTENING_REQUIRED.
