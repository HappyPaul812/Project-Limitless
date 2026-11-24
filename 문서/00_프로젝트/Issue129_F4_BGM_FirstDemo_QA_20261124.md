# LIMITLESS Issue #129 — F4 신규 데모 수령 및 1차 QA / 2026-11-24

## 상태
AUDIO_RECEIVED_1 / COMPOSITION_PASS_RECOMMENDED / PEAK_HEADROOM_AND_LOOP_EDIT_REQUIRED / USER_LISTENING_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED

- 사용자 제출 실제 MP3: Breath_Upon_The_Pass.mp3
- 제안 배정: F4 눈보라 정령 조우 구간 탐험 BGM (정식 Scene/런타임 배정 미검증)
- 사용자 직접 제출로 파일럿 수령. BGM 담당의 Gemini/API 직접 호출 0. 정확한 생성 모델 버전/계정 유형은 미확인.
- 기존 F1/F2/F3/서릿길 승인곡 및 Intro/Loop WAV8 KEEP, 재작업 0.

## 실제 파일 검증
- 원본 파일 크기: 1,763,772 bytes
- SHA256: f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47
- MP3 / 44.1kHz / Stereo
- ffprobe 길이 73.168917초, 발주 45~90초 PASS
- ffmpeg float32 전체 디코딩 PASS
- Integrated -11.5 LUFS, LRA 8.3 LU, true peak 약 +0.2 dBTP (ffmpeg ebur128)
- decoded sample peak +0.157 dBFS; abs(sample)>1인 채널 샘플 19개. MP3 디코딩 오버슈트이며 원본 작곡 단계의 클리핑 여부는 확정하지 않음.
- -50dBFS RMS 10ms 블록 기준 시작 저레벨 약 0.14초, 끝 약 2.48초.
- 원본 그대로 반복 재생 비권장. 원본 SHA 보호.

## 평가 및 다음 게이트
- F4 탐험 역할 적합 PASS 추천: F3 이후 추위와 경계감이 높아지되 보스전까지 과장되지 않는 방향.
- 정확 BPM 및 실제 TTS 동시 재생, F3→F4 런타임 전환은 NOT_VERIFIED.
- 원곡 사용자 청취 승인 후에만 루프 A/B 후보 제작. 피크 여유를 확보하는 최소 선형 감쇠 검토(예: -2dB, 최종 true peak 재검증), EQ/압축/재편곡 불필요. 감쇠 및 루프 편집은 별도 파일로 수행.
- 현재 원곡 수정0 / 신규 생성0(담당 직접 호출 기준) / A/B0 / Master0 / Unity0 / main 병합0.
- COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING 유지. 상용 출시 전 권리 확인 별도.
- Unity6000.6.5f1, #85 HOLD, 기존 BGM/TTS/Mixer/Save/UserData 보호.
