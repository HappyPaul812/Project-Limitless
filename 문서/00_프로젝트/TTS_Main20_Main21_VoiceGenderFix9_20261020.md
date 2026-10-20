> ⚠️ **2026-10-20 사용자 지적에 따른 정정 — 이 문서의 진단·실행 명령은 현재 보류(SUPERSEDED / DO_NOT_RUN)**
>
> 바울 대표: “근데 잘 나오다가 안나오는거잖아? 니가 뭐 바꾼거도 아니고. 그럼 tts의 오류 아니니?” 이전에 같은 Voice ID로 정상 청취 승인된 WAV가 있었으므로, 보이스 성별 분류만으로 갑자기 변한 9개를 설명할 수 없다. 여기의 **“최신 문제 원인 확인” 및 Erinome/Alnilam 샘플 실행 권고는 성급했으므로 철회**한다. ZIP/스크립트는 폐기하지 않고 역사적 시제품으로 보존하되, **새 WAV 생성에 사용하지 말 것**.
>
> 2026-10-12 Google AI Developers Forum에 **동일 보이스·설정·텍스트인데도 전혀 다른 음색이 출력되는 사례**가 보고됨. 2026-10-15 Google 담당자가 **음색 일관성 문제를 조사 중**이라고 답변함. `gemini-3.8-flash-tts` 모델 출력 편차 가능성이 유력하나, LIMITLESS의 실제 원본 요청값 비교는 미완료. Google 공식 문서는 `speech_metadata.style`로 성별 등 고정 화자 특성 변경을 시도하지 말라고 안내함.
> - 증거: https://discuss.ai.google.dev/t/gemini-3-8-tts-voice-consistency-issue/186103
> - 공식 프롬프팅 가이드: https://docs.cloud.google.com/gemini-enterprise-agent-platform/models/text-to-speech/prompting-guide
>
> **수정된 정본 방향:** 1) 정상 WAV와 오류9 원본의 실제 요청 시 모델/Voice ID/Style/SDK/설정/해시 확인(읽기 전용). 2) 요청 조건이 일치하면 기존 Voice ID(`Schedar`, `Gacrux`) 유지, 스타일을 변경하지 않은 동일 입력으로 오류 샘플 **단1개만** 재생성하여 직접 청취 비교. 3) 문제 재현 시 재생성 연속 시도 금지하고 Google 보고/모델 변경 검토. 4) Main18 승인10 등 기존 사용자 승인 음색 유지, 전역 Voice Registry 변경0. 실제 API 0·Unity 0.

# LIMITLESS / Main20·21 세린8·태온1 성별 보이스 정정 시범 (2026-10-20)

**OFFICIAL_VOICE_GENDER_VERIFIED / PYTHON_SAMPLE_PACK_READY / MOCK_QA_PASS / USER_PC_CHECK_PENDING / LIVE_TTS_API=0 / UNITY=0**

## 사용자 새 지시 및 최신 문제 원인

바울 대표: “tts 되니까 작업하자.” 전날 보고한 세린8개 남성처럼 들림/태온1개 여성처럼 들림을 수정한다. 기존 17개 성공 원본 및 별도 Main18 사용자 청취 승인본10개, Main18·19 정상25개 전부 보호.

Google 공식 [Firebase Gemini TTS 보이스 목록](https://firebase.google.com/docs/ai-logic/generate-speech?api=vertex#voices)에 명시된 실제 성별:
- `Schedar` — **Male** (Even). 기존 Main20·21 세린 `Schedar`로 생성한 남성 음색 오류와 일치.
- `Gacrux` — **Female** (Mature). 기존 Main21 태온 `Gacrux`로 생성한 여성 음색 오류와 일치.

2026-10-19 만든 “같은 Voice ID에 여성/남성 style만 지정” 보정팩은 **추가 API를 사용하지 않는 과거 참고용**이며, **이번 재시도에는 사용하지 않는다.** 현재의 문제 해결에는 Voice 프리셋 자체의 성별 정합이 필요하다.

## 현재 시범 후보 두 개 (사용자 승인 전 임시)
- 세린 8개 중 샘플 `main20_confront_serin_01`: **Erinome**, 공식 **Female / Clear**. 세린의 차분한 관찰자 발성에 적합할지 실제 청취로 확인.
- 태온 1개 샘플 `main21_route_taeon_01`: **Alnilam**, 공식 **Male / Firm**. 태온의 진중한 수호자 발성에 적합할지 실제 청취로 확인.
- **후보일 뿐 정식 캐릭터 Voice ID 변경 결정 아님.** 기존 Main18 승인된 세린 Schedar 보정본, 과거 다른 챕터 태온 Gacrux WAV, TTS 전역 레지스트리·정본을 덮어쓰거나 소급 변경하지 않음. 사용자 승인 시 챕터간 음색 연속성도 따로 판정.

## TTS 담당이 직접 준비한 최신 Python 패키지

- `LIMITLESS_TTS_Main20_Main21_GenderFix9_SampleFirst_20261020.zip`, SHA256 `c9f8e6af586c37c3b881f37cc4ff3db46b3485991022c485a2c3b9f7c752925d`.
- `LIMITLESS_TTS_GenderFix9_20261020.py`, SHA256 `fd55444cb033d314a462d7f3a41d109f42260c9c12569ea7e1606e8368349c27`.
- 원본 17행 CSV 고정 SHA256 `6d951073665e12141fc3d2af9cfdc9e3df53f3a8dc351818493db8224652deff`, README 포함. Python/CSV/README 모두 사용자 대화 ZIP으로 전달.
- 보존할 원본: `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019`; 이번 후보 전용 별도 출력: `F:\Downloads\Voice\Limitless_TTS_GenderFix9_20261020`. 이전 VoiceFix9_20261019 출력과도 분리.
- Python 오프라인 compile PASS. 합성 가짜 WAV와 API mock: 원본17 SAVED의 SHA 불변; 샘플2 생성/승인→세린 남은7 생성→후보 총9 결과 Voice/원문/SHA 검증 PASS. 두 번째 호출 모의 실패 시 첫 성공 보존·HOLD/lock 생성·다음 호출0 PASS. ZIP CRC PASS.
- **실제 사용자 PC의 원본 SHA/WAV·Google Project별 quota·과금·제작 결과 음색은 확인하지 못함.** 실제 Gemini 호출0, 실제 신규 WAV0.

## 사용자 실행 순서 (오늘)

압축 파일 3개를 `F:\Downloads\Voice\LIMITLESS_TTS_GenderFix9_20261020`에 풀고 Anaconda PowerShell Prompt에서:

```powershell
conda activate py3_12
cd F:\Downloads\Voice\LIMITLESS_TTS_GenderFix9_20261020
python .\LIMITLESS_TTS_GenderFix9_20261020.py check
```

`[CHECK PASS]` 후 **실제 Project+Model의 무료 quota/과금 위험/현재 호출 가능 횟수**를 사용자 대시보드에서 재확인하고 샘플2회까지만:

```powershell
python .\LIMITLESS_TTS_GenderFix9_20261020.py sample-serin --key-slot 1 --confirm-free
python .\LIMITLESS_TTS_GenderFix9_20261020.py sample-taeon --key-slot 1 --confirm-free
```

Key1 소진/Key2 무료면 `--key-slot 2`를 선택할 수 있지만 **동일 Google Project의 Key는 Quota 공유**. 키 값은 로그·GitHub에 미기재. 사전검사는 호출0/쓰기0.

두 샘플 WAV 직접 청취 승인 후에만 `approve-samples`→`remaining --max-calls 7 --confirm-free` 단계. 남은 quota 확인 없거나 샘플 음색 불일치면 남은7 생성 금지. CLI가 매 단계 무료 확인 및 YES/SERIN_OK/TAEON_OK를 요구.

- Main20 오류세린5 + Main21 오류세린3 + 태온1, 총9개의 Stable ID·ExactText 불변.
- 새 후보 모두 **SAVED_USER_QA_PENDING**. 실제 사람 청취/게임 안에서의 일관성 승인 전 Unity Import 금지.
- 첫 429/실패/불확실 시 HOLD/lock 보존하고 즉시 중단, 자동 재시도0.
- Codex에 음성 생성 미위임. TTS 담당 Python 작성/운용, 사용자 로컬 Python 실행 및 귀로 QA, **Codex는 승인 후 Unity 통합만**.
- Github main commit·Push·merge 금지, TTS 기록 docs/tts만.
