# LIMITLESS Chapter3 BGM 파일럿 제작 브리프 — 2026-10-20

- 담당 브랜치: `docs/bgm`
- 발주 근거: docs/planning `Chapter3_Team_Parallel_Production_Kickoff_20261020.md`
- 담당 Issue: #31 `[Chapter3][BGM] F1 북풍·서릿길 마을 2곡 샘플`
- 상태: **PROMPT_LOCKED / AUDIO_GENERATION_PENDING / USER_LISTENING_PENDING / UNITY_NOT_APPLIED**
- 기존 Chapter1·2 공식 BGM/TTS/Unity 원본 변경: **0**

## 1. 제작 범위

이번 게이트에서는 아래 2곡만 짧은 오리지널 데모로 제작한다.

1. Main22 F1 북풍의 들머리 — 약한 북풍·얇은 서리·조사 긴장
2. Main23 서릿길 마을 — 주민 생활·안전거점·아직 남아 있는 온기

권장 길이는 각각 45~90초. 둘 다 루프를 전제로 하며, 최종 설빙의 폭군 보스 음악은 전체곡 제작 금지. 보스는 정식 전투 패턴·주요 연출 잠금 전 짧은 모티브 시안만 허용된다.

## 2. F1 데모 제작 프롬프트

```text
Create an original instrumental field exploration demo for the fantasy turn-based RPG "Limitless".

Scene:
Chapter 3, the first northern field known provisionally as "The Threshold of the North Wind".

This is NOT a full snowfield yet.
The player has just left a familiar warm starter village and entered a transitional landscape where green grass still remains, but a thin layer of frost appears in shaded areas and on the tips of plants.
A weak cold north wind bends the grass.
There is little or no falling snow.
The ground must not feel like a frozen lake or an apocalyptic blizzard.

Story tone:
The party is investigating why animals are moving south.
The first designated enemy is only a single Frost Rat.
This area does NOT introduce the severe Chill, Frostbite, or Frost Necrosis mechanics yet.
The music should suggest curiosity, caution, and the first sign that the climate is changing, not immediate catastrophe.

Musical identity:
Keep subtle continuity with the established world of "Limitless".
If the earlier Limitless main theme and its recognizable 5-note leitmotif are available in context, hide a restrained fragmented variation of that motif inside the piano, flute, or strings.
Do not quote any existing copyrighted game soundtrack or composer.

Mood:
- cool air
- quiet curiosity
- mild tension
- cautious forward movement
- first contact with an unfamiliar climate
- a small sense of wonder
- never hopeless

Instrumentation:
- soft piano
- restrained cello and viola
- light string ostinato
- breathy flute or low woodwind
- very subtle hammered dulcimer or harp
- light dry frame percussion used sparingly
- occasional soft French horn only for a small emotional lift

Do not use literal wind sound effects as the main musical element.
Suggest wind through moving string texture and breathy woodwind phrases.

Tempo:
Approximately 76–86 BPM.

Length:
About 60–75 seconds.

Structure:
1. Familiar warmth fading behind the player
2. A cool repeating motif appears
3. Light investigative tension develops
4. Brief hint of danger without becoming battle music
5. Return naturally toward the opening texture

Loop:
Design the ending so it can transition naturally back to the opening.
Avoid a large final chord or cinematic ending.

Important:
- instrumental only
- no vocals
- no choir
- no Christmas or holiday sound
- no sleigh bells
- no heroic fanfare
- no huge cinematic drums
- no horror jump scares
- no full blizzard atmosphere
- leave sonic space for dialogue, footsteps, SFX, and TTS
- completely original composition

Final feeling:
"The north is not frozen yet. But the air has changed, and something farther ahead is driving life south."
```

## 3. 서릿길 마을 데모 제작 프롬프트

```text
Create an original instrumental village theme demo for the fantasy turn-based RPG "Limitless".

Scene:
Chapter 3, a northern safe settlement provisionally called "Frostroad Village" / "서릿길 마을".

This is a real lived-in settlement, not an abandoned snow village.
The party arrives after investigating the colder northern field.
Residents are still living ordinary lives:
wood is stacked to dry,
small hearths are burning,
warm drinks are prepared,
shops and an inn are open,
and people are adapting to colder weather.

There is frost on some stone and roof edges and the climate is becoming colder, but the village still contains human warmth and daily life.

Story tone:
This is the first new safe zone of Chapter 3.
The player should feel relief and safety, but not complete comfort.
The northern mystery still exists beyond the village gates.
The track must support dialogue, shopping, party management, skill shop use, rest, and optional everyday conversations.

Musical identity:
Keep clear but subtle continuity with the established "Limitless" soundtrack.
If the earlier 5-note Limitless leitmotif is available in context, transform it into a warmer village variation.
Do not directly reuse the exact arrangement of the earlier starter village theme.
This village should feel older, colder, and more self-reliant than the early-game starter village.

Mood:
- warm
- lived-in
- calm
- gently rustic
- northern
- safe but not carefree
- small community enduring a changing climate

Instrumentation:
- warm piano
- acoustic or nylon-string guitar used softly
- gentle cello
- clarinet or warm woodwind
- pizzicato or light sustained strings
- subtle harp or hammered dulcimer
- very restrained hand percussion
- occasional soft horn pad

Avoid:
- Christmas carol feeling
- sleigh bells
- tavern drinking-song energy
- comedy music
- overly cheerful festival music
- choir
- vocals
- heavy drums
- cinematic climax
- sadness dominating the track

Tempo:
Approximately 80–92 BPM.

Length:
About 60–80 seconds.

Structure:
1. Warm, simple opening
2. Main village motif appears
3. Small sense of daily movement and community
4. Brief cooler harmonic shade reminding us of the northern threat
5. Return to warmth and a loop-compatible ending

Loop:
The ending should naturally return to the opening without a dramatic final cadence.

Important:
- instrumental only
- no vocals
- no choir
- background music, not a showcase piece
- comfortable during long menu/shop/dialogue use
- leave generous sonic space for TTS and UI sounds
- completely original composition
- do not imitate any existing game soundtrack or composer

Final feeling:
"Outside the gate, the cold is growing. Inside, people are still living, working, talking, and keeping one another warm."
```

## 4. 데모 QA 계약

실제 MP3/WAV 납품 후 아래를 별도로 검증한다.

- 파일 decode / 길이 / sample rate / channel 수
- SHA256 및 byte size
- 45~90초 범위
- 초반 5~10초 내 공간 성격 식별
- 루프 후보점과 끝→처음 접합 청감
- TTS/대사 공간: 중역 리드·퍼커션이 과도하지 않은지
- 기존 Chapter2 BGM 대비 체감 음량이 과하지 않은지
- F1은 전투/보스/완전한 설원처럼 들리지 않는지
- 마을은 Christmas/축제/술집곡처럼 들리지 않는지
- 두 곡이 서로 확실히 다른 공간으로 들리는지
- 사용자 청취 승인과 Unity 적용 승인을 분리

## 5. 레벨/권리 계획

- 기존 `BgmPlaybackService`와 BGM Mixer 정책은 이번 샘플에서 변경하지 않는다.
- 생성 원본은 편집 전 별도 보관하고, 루프 편집본이 필요하면 별도 파일로 만든다.
- 후보 데모는 기존 Chapter2 공식 BGM/TTS보다 체감상 앞에 튀지 않게 제작/검수한다.
- 최종 레벨 매칭과 ducking은 Unity 적용 단계에서 별도 Codex 계약으로 검증한다.
- 생성 도구에는 "completely original composition / do not imitate existing composers or copyrighted soundtracks"를 명시한다.
- 실제 생성 도구/생성일/파일 SHA/상업 이용 근거는 음원 납품 시 기록한다.

## 6. 현재 미완료

- 실제 음원 파일 2개: **PENDING**
- SHA256 / 길이 / 루프 실청취 QA: **PENDING**
- 사용자 청취 승인: **PENDING**
- Unity Import/연결: **NOT AUTHORIZED / 0**
- 보스 전체곡: **NOT AUTHORIZED / 0**
