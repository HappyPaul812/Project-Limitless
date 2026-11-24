# LIMITLESS Issue #129 — F4 최종 Intro/Loop Master 납품 / 2026-11-24

## 승인 및 상태
- 원곡 사용자 승인 “승인.”, B 방식/-2dB 비교본 사용자 승인 “좋아”.
- FINAL_MASTER_DELIVERED / MASTER_QA_PASS / UNITY_INTEGRATION_PENDING_CODEX / MAIN_NOT_MERGED.
- 원곡 Breath_Upon_The_Pass.mp3, 1763772 bytes, SHA256 f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47. 렌더 전후 실제 SHA 일치.
- 승인 리뷰 ZIP SHA256 6c565077e19c3281bba835aec720ddfa013d73441f9808aa1106a86cbb5feac1 실제 확인, CRC PASS.

## 승인 B 방식 그대로 제작
- 44100Hz stereo, logical start 522444, end exclusive 2611336, crossfade 24255 samples (0.55s), -2dB linear gain. Intro = source[0:S+X], Loop = source[S+X:E-X] + linear tail-to-head crossfade.
- 추가 gain/EQ/compression/재편곡/새 Gemini 생성0. PCM16 WAV 신규 2종, 원본 덮어쓰기0.

## Master 실물
| 파일 | samples | 초 | bytes | SHA256 | true peak |
|---|---:|---:|---:|---|---:|
| Breath_Upon_The_Pass_Intro_FINAL.wav | 546699 | 12.396803 | 2186840 | 6a8e907c6212c7e56f206593bb5c673213ad12dbdb027cb0e93b7bb8afab9c27 | -2.2dBTP |
| Breath_Upon_The_Pass_Loop_FINAL.wav | 2064637 | 46.817166 | 8258592 | b38f6c7dcd2d8ab335e5475d418eb5111e2e68bfb64c0609a1702ea6efbaf7ef | -1.8dBTP |

## 최소 QA
- WAV header/frame count/전체 ffmpeg decode 2/2 PASS.
- Intro/Loop integrated -16.7/-12.6 LUFS, sample peak -2.243/-1.843dBFS. 최종 true peak 양쪽 -1dBTP 이하.
- Crossfade 진입/Loop 반복/Intro→Loop 경계 샘플 변화 0.006714/0.007843/0.007843, Loop 내부 p99.9 0.114017보다 낮음. 기계적 클릭 proxy PASS.
- 위 세 경계 주변 10ms RMS -50dBFS 미만 최대 연속 구간 0초.
- 최종 Loop의 승인 B 비교본에 해당하는 11.45초 구간 PCM16 전 샘플 대조 결과 최대 차이 0 LSB. 승인된 실제 연결부 동일.
- Unity 재생/실제 TTS 동시 재생은 이번 범위 밖.

## 납품
- LIMITLESS_Issue129_FinalLoopMasters_20261124.zip / 9890962 bytes
- ZIP SHA256: 2ab6609247c0232565e3adf51c0654b9d417ce9300a7634bc4a42c69e9af26ac
- ZIP CRC PASS. WAV2 + MANIFEST.json + README.txt + SHA256SUMS.txt.
- MANIFEST SHA256: fefee8cb09993bdad60057bbe64d411cea12847e6d947bb777f242aacbd7209d
- ChatGPT 납품 ZIP으로 제공, GitHub binary 등록0. 사용자 Windows 다운로드 경로 미확인.

## 보호 및 후속
- Intro 1회 재생 후 Loop 반복, gapless DSP scheduling은 Codex 별도 구현/검증. F4 실제 Scene ID 미확정.
- COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING 유지: 출시 전 별도 확인.
- 기존 승인곡4/WAV8, Unity6000.6.5f1, 기존 BGM/TTS/Mixer/Save/UserData, #85 HOLD 보호. Unity/main 변경0. 추가 생성 없음.
