# LIMITLESS Issue #127 — F2·F3 신규 BGM 제작 브리프 / 2026-11-23

- 담당: BGM
- 브랜치: `docs/bgm`
- 근거: `BGM_WORKFLOW_ROLE_BOUNDARY_20261123.md`, Issue #127
- 상태: **ROLE_BOUNDARY_ACCEPTED / PROMPTS_LOCKED / GEMINI_MANUAL_STEP_REQUIRED / AUDIO_NOT_YET_GENERATED / UNITY_NOT_APPLIED**
- 기존 #31 F1·서릿길 승인곡 재생성: **0**
- 유료 API/신규 결제/키 조회: **0**

## 1. 현재 제작 범위

이번 게이트에서는 각 1차 데모 1곡씩만 제작한다.

1. F2 첫눈의 숲 — 가제 `Whispers of the First Snow`
2. F3 눈 덮인 고갯길 — 가제 `The Quiet Ascent`

F4/F5/빙각 수호체/설빙의 폭군 등 추가 음악은 자동 제작하지 않는다.

## 2. Gemini 제작 환경

Google 공식 Gemini Apps Help 기준:
- Gemini Apps의 음악 생성은 Lyria 기반.
- `Create music`에서 생성 가능.
- 길이는 Short(약 1분) / Full(약 2~3분) 선택 가능.
- MP3 다운로드 가능.
- 생성 음악에는 AI 식별용 디지털 워터마크가 포함됨.

이번 Issue #127은 60~90초 데모이므로 **Short**를 기본값으로 사용한다.
보컬은 **Instrumental / No vocals**를 선택한다.

BGM 담당 세션에서 Gemini 자체를 직접 조작할 연결은 현재 확인되지 않았다.
따라서 대표가 Gemini UI에서 **Create music 선택 → 아래 프롬프트 붙여넣기 → Short / Instrumental 생성 → MP3 다운로드 → 이 BGM 세션에 업로드**하는 최소 수동 단계만 수행한다.
유료 업그레이드/결제 요청이 나타나면 진행하지 말고 별도 승인 전 HOLD.

## 3. F2 첫눈의 숲 — 최종 생성 프롬프트 v1

```text
Compose a completely original instrumental exploration track for the fantasy turn-based RPG "Limitless".

Scene:
Chapter 3, F2 — "Forest of the First Snow."

The party is walking through a living northern conifer forest where the first visible snow has begun to collect on branches, moss, roots, and shaded ground. The dark earth path is still clearly visible. This is a transitional winter forest, not a frozen wasteland and not a blizzard.

Narrative purpose:
The player has already experienced the milder cold of F1. F2 should feel distinctly colder and more cautious, but still beautiful and explorable.
This area introduces the first clearly telegraphed single-target Chill threat. There is also quiet emotional tension between two brothers: one is protective, while the other wants to make his own decisions.
Do not turn that relationship into tragedy or melodrama. The field is still about exploration.

Core feeling:
delicate first snow, cautious curiosity, restrained uncertainty, living forest, quiet forward movement, a small trace of warmth beneath the cold.

Musical direction:
- approximately 74–84 BPM
- 60–85 seconds
- instrumental only
- expressive soft piano
- breathy flute or warm clarinet
- gentle viola and cello
- very light harp
- sparse repeating string pulse
- minimal soft dry percussion
- clear but modest melodic identity
- cooler harmony than the earlier F1 theme
- enough empty midrange space for Korean dialogue, footsteps, UI sounds, and combat-warning SFX

Structure:
1. quiet arrival beneath snow-dusted trees
2. a small memorable motif emerges
3. restrained investigative tension develops
4. a brief colder shade suggests the first Chill threat
5. return toward the opening texture with a natural loop-friendly ending

Loop intent:
Avoid a dramatic final cadence, long fade-out, or long silence. The ending should be suitable for later seamless looping.

Avoid:
vocals, choir, sleigh bells, Christmas or holiday music, horror stingers, boss music, battle music, huge cinematic drums, bombastic brass, literal blizzard sound effects, overly sad music, triumphant climax, or generic stock fantasy clichés.

Copyright:
Do not imitate, quote, or closely resemble any existing composer, game soundtrack, film score, copyrighted melody, or recognizable song.

Final emotional statement:
"The first snow is beautiful, but the forest is asking the party to become more careful."
```

Suggested Gemini settings:
- Tool: Create music
- Length: Short
- Vocals: Off / Instrumental
- One generation only for first review

## 4. F3 눈 덮인 고갯길 — 최종 생성 프롬프트 v1

```text
Compose a completely original instrumental exploration track for the fantasy turn-based RPG "Limitless".

Scene:
Chapter 3, F3 — "Snow-covered Mountain Pass."

The party has climbed beyond the first-snow forest. Snow is now deeper, pale rock faces and exposed stone are more prominent, and the route narrows between cliffs. The path must still feel clearly passable and accessible; this is not a survival-horror climb and not an impassable avalanche zone.

Narrative purpose:
F3 is heavier and more exposed than F2.
The player is learning that defense, formation, and protective tactics matter against an armored ice beetle.
Serin's judgment becomes more important, while Taeon and Haren disagree in a tense but caring way about how the group should proceed.
The track should suggest weight, caution, resolve, and a colder horizon without becoming combat or boss music.

Core feeling:
grounded ascent, cold exposure, controlled tension, determination, protective instinct, quiet disagreement, and steady forward movement.

Musical direction:
- approximately 78–90 BPM
- 60–90 seconds
- instrumental only
- low lyrical cello and viola
- restrained piano ostinato
- thin flute/clarinet or other light woodwind
- very subtle soft French horn phrase
- sparse, dry, non-booming percussion
- controlled low frequencies
- flowing harmonic movement
- a warmer inner emotional undertone beneath the colder exterior
- clearly distinct motif and rhythmic identity from the F2 first-snow forest

Structure:
1. grounded opening with the sense of climbing
2. low strings establish physical weight
3. restrained pulse suggests careful tactical movement
4. a brief harmonic tension suggests disagreement and exposure
5. a small warm inner phrase reminds us that the party remains united
6. return to a steady loop-friendly exploration texture

Loop intent:
Avoid a final crashing chord, long reverb tail, long silence, or cinematic ending. Leave a natural loop candidate.

Avoid:
vocals, choir, sleigh bells, Christmas melody, bombastic brass, huge cinematic drums, literal wind or blizzard sound effects, boss music, battle music, horror, heroic victory music, or excessive low-end rumble.

Copyright:
Do not imitate, quote, or closely resemble any existing composer, game soundtrack, film score, copyrighted melody, or recognizable song.

Final emotional statement:
"The mountain is heavier now, but the party keeps climbing with care rather than fear."
```

Suggested Gemini settings:
- Tool: Create music
- Length: Short
- Vocals: Off / Instrumental
- One generation only for first review

## 5. 상업 이용/권리 상태

확인된 공식 근거:
- Google Terms of Service: 일부 서비스가 생성한 원본 콘텐츠에 대해 Google은 소유권을 주장하지 않음.
- Gemini Apps Help: Lyria 음악 생성, MP3 다운로드/공유, 디지털 AI 워터마크 존재.

현재 미확인:
- 무료 소비자 Gemini/Lyria 생성 음악을 **유료 게임 BGM으로 사용하는 것을 명시적으로 보장하는 별도 상업 라이선스 문구**.

따라서 현재 상태:
**GOOGLE_OWNERSHIP_CLAIM_NONE_CONFIRMED / COMMERCIAL_GAME_USE_EXPLICIT_LICENSE_NOT_VERIFIED**

파일럿 생성·청취·기술 QA는 진행 가능하나, 최종 상용 배포 자산 확정 전 추가 권리 확인을 기록한다.
프롬프트는 기존 곡/작곡가 모사를 금지하여 제3자 권리 위험을 낮춘다.

## 6. 음원 수령 후 최소 QA

각 MP3 수령 즉시:
- 파일 SHA256 / bytes
- decode
- 길이
- sample rate / channel
- peak
- 종료 무음
- 루프 후보
- F2/F3 서로의 음색·리듬 구분
- 기존 F1/마을과 중복감
- TTS/SFX 공간
- 생성 도구/모델/무료·유료 여부
- 사용자 실제 청취 승인

를 기록한다.

Unity 6000.6.5f1 연결, Mixer 변경, Save/UserData 변경, main 병합은 하지 않는다.
