# LIMITLESS / 승인 세린 WAV 실자료 기반 직접 오프라인 음색 보정 8종 (2026-10-20)

**USER_ASKED_ASSISTANT_DO_IT / SOURCE_AUDIO_RECEIVED / APPROVED_REFERENCE_RECOVERED_FROM_LIBRARY / PREVIEWS_16_GENERATED / AUDIO_QA_FORMAT_PASS / HUMAN_LISTENING_PENDING / GEMINI_API_0 / UNITY_0**

## 요청 및 자료

바울 대표 “아니 니가 해.”와 대화 첨부 `candidates.zip` (17개 WAV: Main20·21 신규 제작 출력) 실바이트를 직접 사용했다. ChatGPT 파일 보관함에서 **이전에 바울 대표가 청취 승인한** `main18_ground_serin_01.wav`와 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip`을 찾아 가져와 변환에 사용. 기획·GitHub 승인본 SHA에 부합함.

- 승인 원본 `main18_ground_serin_01.wav` SHA256: `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e` — 실제 파일과 일치.
- 승인 보정본6 ZIP SHA256: `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1` — 실제 파일과 일치.
- 기준 총 7 WAV, 승인 목소리의 Praat 추정 기본 주파수(F0) 중간값 범위 대략 178~193Hz(전체 기준 중앙184.4Hz).
- 문제 세린 WAV 8개는 사용자 업로드 실제 바이트 기준 F0 대략 109~154Hz. 17개 나머지 NPC WAV는 변경/재출력하지 않음.

## 직접 변환 및 납품

- `LIMITLESS_Serin8_ReferenceGuided_WAV_Previews_20261020.zip` SHA256 `349d6bf7047a92d04972f68c769595969b3ff69d8b6e03f1ea8bfd7179ea535e` (4,419,922 bytes).
- ZIP 내부 `A_natural/<DialogueId>.wav` 8개 및 `B_closer/<DialogueId>.wav` 8개, 총 **16개 24kHz mono PCM16 WAV**. `QA_MANIFEST.csv`, `QA_SUMMARY.json`, `README_KO.txt` 포함.
- A: 승인 참조 피치 기준 약180.7Hz, formant ratio1.075, 참조 스펙트럼 EQ 약하게. B: 약188.1Hz, formant ratio1.15 및 참조 스펙트럼 EQ 상대적으로 강하게. 원본 텍스트·길이 보존을 우선해 **Praat PSOLA 기반 피치·포먼트** 및 **실제 승인 WAV7의 평균 로그 스펙트럼 형태를 반영한 제한적 EQ** 사용. 임의 고정 피치만 바꾼 과거 DSP보다 승인 음성 기준에 맞춘 실험이나 **딥러닝 AI 음성 복제 아님**.
- 실제 OpenVoice V2 ML 모델 및 사전학습 checkpoint는 작업 컨테이너에 없고 외부 모델 다운로드 연결 제한으로 실행 불가. 따라서 과거 약속했던 실제 OpenVoice AI voice conversion 실행을 완료했다고 **주장하지 않음**. 완전히 같은 성우 목소리·자연스러움/청취 품질 미검증.
- 각 WAV 사용자 원본 길이 유지, SHA256 검증, 24k/1ch/PCM16, 유한 샘플, 오디오 peak≤0.951, 전부 읽기 디코딩 PASS. 참조 WAV와 업로드 원본은 모두 바이트 그대로 보존. Google 생성/API0, 로컬 Unity0.

### 8개 해당 대사

`main20_after_colossus_serin_02`, `main20_after_colossus_serin_01`, `main20_confront_serin_03`, `main20_confront_serin_02`, `main20_confront_serin_01`, `main21_route_serin_01`, `main21_pulse_serin_01`, `main21_heat_serin_01`.

### 사용자 청취 게이트

`main20_confront_serin_01`의 A/B WAV 2개를 먼저 비교하고 어느 버전이 승인 세린 목소리에 자연스럽게 가까운지 확인한다. 과거 피치/포먼트 DSP 전량 FAIL 경험이 있으므로 **두 버전 모두 부자연스러우면 이 결과 전량 FAIL/Unity 적용 금지**. 동의 없이 Main18 기존 승인 음성·다른 캐릭터 태온 등에 파형 변조 적용하지 않는다. 태온 오류1은 세린 승인 기준 WAV로 대체 불가하므로 이번 작업에서 제외.

이번 정본은 `docs/tts` 브랜치 문서만 기록, main 직접 commit/Push/merge 금지. Codex는 바울 대표 최종 승인 WAV에 대한 Unity 통합만 담당한다.
