# LIMITLESS TTS / Renewal_Voices 3인 한국어 오디션 실행 계획 (2026-10-20)

**사용자 지시:** “17개 남았으니까. 해보자. 한국어 샘플 먼저.. 작업 폴더 F:\Downloads\Renewal_Voices”

**TTS 담당 상태:** 배포용 Windows Python ZIP 작성 + 오프라인 검증 완료 / 실제 Gemini TTS POST 0 / 한국어 WAV 생성 0 / 사용자 청취 미완료 / Unity 수정 0.

## 시험 범위 및 호출 게이트

- 사용자가 현재 무료 잔여량 **17회**라고 보고함. 이는 Google API 대시보드와 프로젝트/모델에 대한 현재 잔여량을 직접 인증한 수치는 아니다. 키를 바꿔 17회가 늘어나는 것으로 가정 금지.
- 신규 캐스팅 *공식 데모 긍정 평가*에 이어, **실제 게임 한국어 대사 3개를 한 캐릭터당 정확히 1회** 합성. 성공 시 예상 잔여 14회(다른 사용자 호출/차감 여부에 따라 달라질 수 있음). 반복 생성·추가 14회 무조건 소진 금지.
- 기존 창작 대사의 Stable Dialogue ID/원문 유지, 추정 프롬프트 금지. 정확 출처: GitHub main `문서/00_프로젝트/Main20_TTS_Manifest.csv`, `문서/03_스토리/Main21_TTS_Manifest_20261010.json`.

| 순서 | 화자/Stable ID | 새 Voice | 정확 원문 |
| --- | --- | --- | --- |
| 1 | 하렌 `story_haren` / `main21_haren_haren_01` | **Alnilam** (Male / Firm) | `태온. 너 아직 멀었어.` |
| 2 | 세린 `companion_serin` / `main20_confront_serin_01` | **Erinome** (Female / Clear) | `저 존재가 움직일 때마다 지면의 맥동이 따라옵니다.\n가까이 오니 더 분명해졌어요.` |
| 3 | 태온 `companion_taeon` / `main21_route_taeon_01` | **Algieba** (Male / Smooth) | `여긴 지난번에 흔들리던 곳과 다릅니다. 제가 다시 확인하겠습니다.` |

## 직접 제작·전달 산출물

`LIMITLESS_Renewal_Voices_Korean_Audition_3Samples_20261020.zip` — **8,040bytes**, SHA256 `8649f90cffe73baeed22e52390666833f2a263f1c4821b525cfa7ba15a0ecd10`.

ZIP 구성:
- `Renewal_Voices_Korean_Audition.py` — **13,504bytes**, SHA256 `0a6b858c66d03550039cf1f36b8dbb7e72b729e5876b36dcdc4039ee5d9ab1e5`.
- `KOREAN_AUDITION_MANIFEST.json` — **1,980bytes**, SHA256 `2430f4bb9e8af7cfa8de4478454d6d456a01641f321cc752ffec90ff3c354c7b`. 한국어 원문 SHA, VoiceID, SpeakerID 및 Style 포함.
- `README_KO.txt`.

압축 해제 및 도구/결과 작업 경로: **`F:\Downloads\Renewal_Voices`** (기존 Voice 하위 디렉터리나 다른 경로 아님). 결과는 `F:\Downloads\Renewal_Voices\samples`.

```powershell
conda activate py3_12
cd F:\Downloads\Renewal_Voices
python .\Renewal_Voices_Korean_Audition.py check
python .\Renewal_Voices_Korean_Audition.py sample
```

개별 샘플 생성도 가능:
```powershell
python .\Renewal_Voices_Korean_Audition.py sample --speaker haren
python .\Renewal_Voices_Korean_Audition.py sample --speaker serin
python .\Renewal_Voices_Korean_Audition.py sample --speaker taeon
```

## 상세 보호·결과 판정

- Gemini 모델 `gemini-3.8-flash-tts`, 기존 실제 성공했던 요청 구조 `contents[].parts[].speech_metadata.style` 및 `generationConfig.speechConfig.voiceConfig.voice`; Style만 해당 3인 인물 성격에 맞게 지정.
- 실행 전 manifest 해시·ID·보이스 검증, 기존 성공 WAV+메타 SHA 대조; 실패 또는 산출물 유실 시 **HOLD/lock** 보존, 자동 재시도0. 성공2개 후3번째 실패하면 1~2번 WAV 보존하고 다음 호출0. 호출 사이28초. 같은 대사 중복출력 자동 차단.
- 모델 메타 GET은 음성 생성 호출에 포함하지 않으나 Free quota·청구 상태를 자동 확인하는 방법이 아님. 사용자가 별도 Google AI Studio 프로젝트/모델 무료 잔여·과금위험 확인 후 **FREE**를 명시하고 `YES3` (일부 완료면 `YESn`)를 입력해야 생성. API Key는 비공개 입력/환경변수 사용, 결과 JSON에 미기록. 원본 WAV/로그 API 키값 저장0.
- 실제 본문은 JSON manifest에 원문 그대로, 음성 결과는 각각 `main21_haren_haren_01__Alnilam.wav`, `main20_confront_serin_01__Erinome.wav`, `main21_route_taeon_01__Algieba.wav`. 동명 .json에 Voice/Model/Style/본문SHA/음원SHA/PCM 품질 및 `USER_LISTENING_PENDING` 저장. Unity 연결0.
- python compile PASS, ZIP 추출 후 `check` PASS, localhost mock GET+POST3·정규 WAV 읽기/기존 결과 중복 차단/파일 메타 불일치 차단/실패 HOLD·lock 중단 PASS. **이는 실제 모델 음질 또는 live 생성 성공 증거가 아님.** ChatGPT는 사용자 F: 드라이브 및 비밀 API 키에 접근할 수 없어 실제 Google 호출을 수행하지 않았다.
- 사용자 음성 샘플 3개를 듣고 정식 채택 후에만 하렌3→세린67→태온85 (현재 Main21까지 확정 대사155개) 제작. 캐릭터가 챕터마다 구/신 목소리 혼재하지 않도록 별도 Codex Unity 통합 승인 필요. 기존 WAV·GUID/meta·Save·HOLD 모두 보호.

## 관련 정본

- [사고 Postmortem / 3인 범위](TTS_캐스팅_성별역전_음색불일치_사고보고서_및_3인재캐스팅_20261020.md)
- [3인 캐스팅 및 3·67·85 대사량](TTS_세린_태온_하렌_캐스팅_대사집계_20261020.md)
- [TTS 담당 최신 인계](팀운영/인계/TTS_LATEST.md)
- [GitHub Issue #20](https://github.com/HappyPaul812/Project-Limitless/issues/20)

**Git:** 브랜치 `docs/tts`만 문서화. main 직접 커밋/Push/merge0. 사용자 3개 실제 한국어 음성 청취 승인 대기.
