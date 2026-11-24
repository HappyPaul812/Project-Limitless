# BGM · 2026-11-24 마감 발췌

[원격 인계 기준](https://github.com/HappyPaul812/Project-Limitless/commit/7d5d6457fd6923b85b00175df1f0377b8ca743b6)

## 2026-11-24 Issue #129 — F4 눈보라 구간 BGM 브리프/프롬프트

- 최신 Issue #129 및 `20261124_Parallel_Department_Orders_While_Issue85_Repair.md` 확인.
- 기존 완료 보호:
  - F1 `Where the Grass Bends`
  - 서릿길 마을 `Hearth in the Low Valley`
  - F2 `Beneath the Frozen Boughs`
  - F3 `Steps on Frozen Stone`
  - 승인 Intro/Loop WAV8
- 기존곡 재생성/재편집/기술 QA 반복0.
- F4 방향:
  - F3보다 더 춥고 위험한 탐험
  - 눈보라/노출/정령성 또는 이상현상 감각 강화
  - F3의 low strings + forward pulse 계보는 유지
  - 화성/고역 texture는 더 차갑고 불안정하게
  - F5/최종 보스 대비를 위해 대형 brass/drum/choir/climax 금지
  - 80~90BPM, 60~85s 권장(45~90s 허용), TTS midrange 공간 확보
- Gemini/Lyria 생성 프롬프트 v1 작성 및 잠금.
- 상세: `문서/00_프로젝트/Issue129_F4_Blizzard_BGM_Production_Brief_20261124.md`
- 상태: **BRIEF_LOCKED / GEMINI_PROMPT_LOCKED / AUDIO_GENERATION_NOT_AUTHORIZED / AUDIO_FILE_0 / UNITY_CHANGE_0**
- 상업권: `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING` 유지.
- 실제 생성은 바울 대표 별도 승인 후 Gemini Short / Instrumental / 1회만.
- F4 실제 Scene/정식 런타임 배정은 NOT_VERIFIED. 새 적명/Scene ID 임의 확정 없음.
- Unity6000.6.5f1, 기존 승인 BGM/TTS/Mixer/Save/UserData, #85 HOLD 보호.

## 2026-11-24 Issue #129 — F4 데모 수령

- Breath_Upon_The_Pass.mp3 / 73.168917s / 44.1kHz stereo / 1,763,772bytes.
- SHA256 f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47.
- 전체 decode PASS, 길이 PASS, -11.5 LUFS, true peak +0.2 dBTP, decoded sample peak +0.157 dBFS(19 channel samples > 1), 끝 저레벨 약2.48s.
- F4 탐험곡 적합 PASS 추천. 원곡 청취 승인 대기. 피크 여유 및 루프 편집은 승인 후 별도 파일로 진행. A/B0 / Master0 / Unity0 / main0.
- COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING. 기존 승인곡4/WAV8 및 #85 HOLD 보호.
- 보고: 문서/00_프로젝트/Issue129_F4_BGM_FirstDemo_QA_20261124.md

## 2026-11-24 Issue #129 — F4 원곡 승인 및 Loop A/B

- 대표 직접 “승인.” → Breath Upon The Pass 원곡 USER_APPROVED. 원본 SHA f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47 전후 일치.
- B 후보 522444→2611336 samples @44100Hz (11.846803→59.213968s), xfade 24255 samples/0.55s, A/B 동일 -2dB gain. A 저레벨 2.73s, B 0s, true peak A -2.2/B -1.9dBTP. decode 및 기계적 경계 검사 PASS.
- ZIP LIMITLESS_Issue129_LoopReview_20261124.zip / 3843795 bytes / SHA256 6c565077e19c3281bba835aec720ddfa013d73441f9808aa1106a86cbb5feac1 / CRC PASS.
- 상태: ORIGINAL_USER_APPROVED / LOOP_AB_READY / B_LOOP_TECH_PASS / LOOP_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED.
- 다음: B 및 -2dB gain 청취 승인 후 Master. 상업권 COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING. 기존 승인곡4/WAV8 및 #85 HOLD 보호. Unity/main 변경0.
- 상세: 문서/00_프로젝트/Issue129_F4_LoopReview_QA_20261124.md

## 2026-11-24 Issue #129 — F4 최종 Intro/Loop Master 완료

- 대표 “좋아” → B 루프/-2dB 승인. 원곡 승인 유지. 원본 SHA f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47 전후 일치.
- Breath Upon The Pass: Intro 12.396803s / SHA 6a8e907c6212c7e56f206593bb5c673213ad12dbdb027cb0e93b7bb8afab9c27; Loop 46.817166s / SHA b38f6c7dcd2d8ab335e5475d418eb5111e2e68bfb64c0609a1702ea6efbaf7ef. PCM16/44.1kHz/stereo.
- 2/2 decode PASS, 경계 무음0, 기계적 클릭 검사 PASS, true peak -2.2/-1.8dBTP. 승인 B 비교 구간 PCM16 전 샘플 동일(0 LSB 차이).
- ZIP LIMITLESS_Issue129_FinalLoopMasters_20261124.zip / 9890962 bytes / SHA256 2ab6609247c0232565e3adf51c0654b9d417ce9300a7634bc4a42c69e9af26ac / CRC PASS. WAV2+Manifest+README+SHA256SUMS.
- 상태: FINAL_MASTER_DELIVERED / MASTER_QA_PASS / UNITY_INTEGRATION_PENDING_CODEX / MAIN_NOT_MERGED.
- 기존 승인곡4/WAV8 KEEP. 신규 생성/원본 변경/Unity/main 변경0. #85 HOLD 보호. 상업권 COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING 유지.
- 상세: 문서/00_프로젝트/Issue129_F4_FinalLoopMasters_QA_20261124.md
