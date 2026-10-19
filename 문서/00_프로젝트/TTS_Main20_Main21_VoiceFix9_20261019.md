# LIMITLESS TTS / Main20·Main21 신규 음성 음색 오류 9개 및 안전 재제작 대기 (2026-10-19)

**USER_REPORTED_TIMBRE_MISMATCH=9 / REGEN_CANDIDATE=9 / SAMPLE_FIRST_SCRIPT_PREPARED / USER_SAMPLE_REVIEW_PENDING / REAL_NEW_API=0 / UNITY=0**

## 바울 대표 직접 청취 피드백

바울 대표가 실제로 청취하고 아래 파일에 대해 음색 오류를 보고함. **TTS 담당이 해당 사용자 PC의 9개 WAV를 직접 개봉·독립 분석한 것은 아니다**. 실제 호출 당시 wire voice 설정 및 Gemini 내부 음색 변동의 원인은 UNKNOWN. 저장 성공/SHA 정상 여부와 사람의 음색 판단을 혼동하지 않는다.

| Dialogue ID | 화자 | 사용자 지적 / 판정 |
| --- | --- | --- |
| `main20_after_colossus_serin_02` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main20_after_colossus_serin_01` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main20_confront_serin_03` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main20_confront_serin_02` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main20_confront_serin_01` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main21_route_serin_01` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main21_pulse_serin_01` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main21_heat_serin_01` | 세린 | 남자 목소리 / USER_REPORTED_MALE_TIMBRE |
| `main21_route_taeon_01` | 태온 | 여자 목소리 / USER_REPORTED_FEMALE_TIMBRE |

기존 17회 큐의 **나머지 8개는 미보고**이지 반드시 청취 합격으로 확정된 것이 아니다. 파일별 청취/정합 승인이 없는 한 Unity 통합 금지.

## 최초 스크립트 감사 소견

- 기존 배포 `LIMITLESS_TTS_Main20_Main21_17.py`의 `VOICE` 설정: 세린 Schedar, 태온 Gacrux. 그러나 `STYLES`에서 세린은 measured/observant, 태온은 steady/sincere로만 지시하여 여성/남성 음색 구체 조건이 빠졌음.
- 이 스타일 누락은 차이점이지 실제 음색 오류 원인 확정 증거가 아님. Google 공식 Gemini 3.8 TTS는 `speech_metadata.style`로 발화 스타일을 제어할 수 있지만 원하는 목소리 성별·음색이 항상 보장된다고 명시하지 않음.
- 세린과 태온의 **공식 Voice ID를 일방적으로 다른 Voice로 변경하지 않음**. 원본 Stable Dialogue ID/원문/순서/Manifest 불변.
- Google 참고: https://ai.google.dev/gemini-api/docs/speech-generation.

## TTS 담당 직접 작성: 오프라인 점검 + 샘플 우선 Python 패키지

- `LIMITLESS_Main20_Main21_VoiceFix9_SampleFirst_20261019.zip` (11,049 bytes), SHA256 `28c130098d1e33d7889ffd79dce715ace992283885b7e01c8cdca59255160dcb`.
- `LIMITLESS_TTS_VoiceFix9.py` SHA256 `8440a69fc7bc2031da7c592e257e4bd469efeab16be25f7efdd2dfbc667c50ca`, 원본17행 CSV SHA256 `6d951073665e12141fc3d2af9cfdc9e3df53f3a8dc351818493db8224652deff`, README 포함.
- 배포 ZIP은 사용자 대화에서 다운로드 가능. **GitHub에는 Python/ZIP 원문 대신 정책·인계만 기록**, Secret/Key 없음.
- 원본 경로 기본값: `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019`. 원본 `candidates/results`의 SAVED ID/원문 SHA/Voice ID/WAV SHA 전수 읽기 대조. 전 단계에서 원본 SHA snapshot 대조. 새 쓰기는 독립 `F:\Downloads\Voice\Limitless_TTS_VoiceFix9_20261019` 경로 한정.
- 1차 **샘플2**: 세린 `main20_confront_serin_01` (Schedar + 명시적 한국 여성 음색 스타일), 태온 `main21_route_taeon_01` (Gacrux + 명시적 한국 남성 음색 스타일). 각각 API **1회**까지만 사람의 quota/과금 확인 후 실행.
- 실제 청취 둘 다 PASS면 Python `approve-samples`으로 샘플 WAV SHA에 연계해 승인한 후 세린 나머지 **7개**만 별도 `remaining`으로 생성. 샘플 음색 불일치면 나머지7 호출 금지, Voice 선택 재검토. 원본 9개를 덮어쓰거나 변조하지 않음.
- 사용자 보고한 당일 키1 7회, 키2 10회는 최초 17회 제작에 이미 소비되었을 가능성이 있음. **현재 추가 무료 quota가 9회 있다고 가정하지 않음**. 각 요청 전 해당 Google Project+Model의 현재 FreeTier 잔여/비용을 다시 확인. 키 소속 Project 간 중복 합산 금지.
- 재제작 성공은 `SAVED_USER_QA_PENDING`이며 Unity에 자동 추가하지 않음. 첫 429/실패/불확실 시 새 `HOLD.json`+`RUNNING.lock` 보존, 다음 ID·자동 retry·무작위 Key 변경 금지.

### 사용자 PC 안내 (API0 확인 먼저)

```powershell
conda activate py3_12
cd F:\Downloads\Voice\LIMITLESS_TTS_VoiceFix9
python .\LIMITLESS_TTS_VoiceFix9.py check
```

무료 quota 새로 확인된 다음에만:

```powershell
python .\LIMITLESS_TTS_VoiceFix9.py sample-serin --key-slot 1 --confirm-free
python .\LIMITLESS_TTS_VoiceFix9.py sample-taeon --key-slot 1 --confirm-free
# 두 WAV 직접 청취 후 정말 PASS인 경우에만
python .\LIMITLESS_TTS_VoiceFix9.py approve-samples
# 그 후 별도 남은 무료 quota 최소 7회 확인
python .\LIMITLESS_TTS_VoiceFix9.py remaining --key-slot 1 --max-calls 7 --confirm-free
```

각 실행에서 KEY 슬롯은 확인된 무료 quota에 따라 1 또는 2로 변경 가능. 남은7회 미만이면 `--max-calls` 낮춤. Key 입력은 환경변수 또는 화면에 노출되지 않는 입력.

## 실제 오프라인 검증 및 경계

- Python compile PASS, 원본 17행 CSV 고정 SHA PASS, 오류9개 ID/Voice/화자 검증 PASS.
- **합성 가짜 WAV/가짜 API** 기반: 원본 17 SAVED SHA 보호, 샘플 2개 생성 모의 → 샘플2 SHA 승인 기록 → 나머지7 개별 생성 모의 PASS. 원본17 WAV+결과 파일 변경0. 두 번째 가짜 호출 예외 시 바로 HOLD/lock, 세 번째 및 뒤 API0 PASS. 실제 TTS 모델의 음색, 원본 PC 파일, 무료 잔여 quota는 검증하지 않음.
- 본 TTS 세션 **Gemini API 호출0, 실제 신규 WAV0, Unity0**. `main` 직접 commit/Push/merge0. 해당 업무는 TTS 담당이 직접 Python 작성·제작 관리, Codex는 나중에 사용자 승인 WAV의 Unity 통합만 담당.
