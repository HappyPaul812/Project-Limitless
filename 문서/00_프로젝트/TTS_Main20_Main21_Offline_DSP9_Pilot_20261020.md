# LIMITLESS / Main20·21 음색 오류 9개 오프라인 파형 보정 시험 (2026-10-20)

**DSP_PYTHON_DELIVERED / OFFLINE_MOCK_QA_PASS / REAL_USER_WAV_NOT_ACCESSED / REAL_WAV_MODIFIED=0 / GEMINI_API=0 / UNITY=0**

## 사용자 요청과 결정

바울 대표는 정상 제작되던 음색이 갑자기 달라진 현상을 TTS 자체의 일관성 문제로 지적했다. 보이스 프리셋의 성별 분류만으로 원인을 확정하지 않는다. 이어서 “니가 파형 변조로 못고치니?” 요청. 새 Gemini TTS 재생성이 아니라 **기존 9개 WAV에 오프라인 피치/포먼트 동시 보정**을 시험한다.

사용자가 직접 듣고 FAIL 보고한 원본9:
- 세린8: `main20_after_colossus_serin_02`, `main20_after_colossus_serin_01`, `main20_confront_serin_03`, `main20_confront_serin_02`, `main20_confront_serin_01`, `main21_route_serin_01`, `main21_pulse_serin_01`, `main21_heat_serin_01`
- 태온1: `main21_route_taeon_01`

## 직접 제작한 Python DSP 배포물

- ZIP `LIMITLESS_TTS_DSP9_Offline_20261020.zip`, 6,663 bytes, SHA256 `1a91fa078a367c7a6336fdb44209b0cd743ee43d41da4c75c392fd04c99afec9`.
- 구성 `LIMITLESS_TTS_DSP9_Offline.py`, `README_KO.txt`, `requirements.txt`; 사용자 대화에서 직접 다운로드 제공.
- 의존성 numpy/scipy/librosa/soundfile/soxr. **Google SDK, 네트워크, API Key 코드 없음**.
- 원본 기본 위치 `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_20261019\candidates\*.wav`.
- 독립 결과 위치 `F:\Downloads\Voice\Limitless_TTS_Main20_Main21_DSP_20261020\previews\` 또는 `selected\`.
- 명령 `python .\LIMITLESS_TTS_DSP9_Offline.py check` (9개 원본 read-only 검사), `python .\LIMITLESS_TTS_DSP9_Offline.py preview` (샘플2×gentle/medium/strong = 6개 미리듣기), `python .\LIMITLESS_TTS_DSP9_Offline.py apply --serin medium --taeon gentle` (귀로 승인한 세린/태온 프리셋을 각 오류 ID에만 적용 → 별도 후보 9개).
- 임의 보정 값 세린: +1.4/+2.7/+4.0 semitones 및 formant ratio 1.07/1.13/1.19. 태온: -1.4/-2.7/-4.0 semitones 및 0.94/0.88/0.83. 모두 청취용 **실험값**이며 공식 음색 승인/원래 Voice ID 변경 아님.
- librosa pitch-shift(말 길이 고정) 후 broad spectral-envelope Gaussian smoothing과 EQ를 이용한 포먼트 근사 보정. 최대 EQ gain ±6dB, 최대 peak0.97, 단일 채널24kHz PCM16. 이는 성우 voice conversion·원래 고유 음색 복원 보증 아님.
- 원본 파일은 **읽기 전용** SHA 취득·마무리 검증, 결과 파일 `xb` 독점 생성, 기존 출력 감지 시 중단, separate folder AUDIT SHA/길이/사용 프리셋 기록, 청취 전 `USER_LISTENING_PENDING`.
- 기존 다른 정상8, Main18 승인 세린10, Main18/19 정상25, Original/Backup/HOLD/lock/Manifest/Save·Unity 변경0.

## 검사 및 현재 한계

- Python compile PASS; **합성 음성 WAV 9개** 이용해 기존 원본 SHA 보호·check9/9·preview6·apply9·DSP_AUDIT.json9·결과 PCM16 24k mono·frames 일치 검증 PASS. 기존 출력 재시도 차단 PASS. ZIP CRC PASS.
- **실제 사용자 F: 오디오 파일은 접근 불가**. 실제 보정 효과/원문 정확도/세린·태온의 캐릭터 음색 유사도/인공적 흔들림은 바울 대표 청취 전까지 **NOT_VERIFIED**. 유사도가 부족하면 API 재시도 없이 보정값 수정 또는 다른 보존 전략을 검토.
- 기존 2026-10-20 `GenderFix9`(Erinome/Alnilam 보이스 교체)과 2026-10-19 성별 Style 강제 `VoiceFix9`는 **DO_NOT_RUN**. 원인 진단 불충분하며 기존 음색 불변 우선.
- `docs/tts` 담당 문서만 작성, `main` 커밋/푸시/병합 금지. Codex는 청취 승인된 결과의 Unity 통합만 담당.
