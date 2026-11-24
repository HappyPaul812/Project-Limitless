# LIMITLESS Issue #129 — F4 원곡 승인 및 Loop A/B / 2026-11-24

대표 직접 승인: “승인.” (직전 F4 원곡 채택 요청에 대한 승인).
상태: ORIGINAL_USER_APPROVED / LOOP_AB_READY / B_LOOP_TECH_PASS / LOOP_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED.

## 원본 보호
Breath_Upon_The_Pass.mp3 / 1,763,772 bytes / SHA256 f0142ee5e0c7a30c24aead0c483aa9a3b10de97e1d1c0e6318c039157f8a3b47. 실제 편집 전후 SHA 일치. 재생성/재편곡/원본 수정 0.

## B 후보
44.1kHz stereo, logical start sample 522444 (11.846802721s), end exclusive sample 2611336 (59.213968254s), linear crossfade 24255 samples (0.55s). 예상 물리 Intro 12.396803s, Loop 46.817166s. 원본 float32 decode 후 -2dB 선형 gain만 적용, EQ/compression 없음. A/B 모두 동일 감쇠. 자동 onset 주기 후보 약80.9 BPM, chroma/레벨/onset 유사도 기반 경계 후보 선택. 사람 청감 검증은 대표 승인 대기.

## 비교 파일
WAV PCM16 / 44100 stereo, 전체 decode PASS.
- A: Breath_Upon_The_Pass_A_OriginalBoundary.wav / 14s / SHA256 44ae3fc1f028d5e8e81dd9e3bc4f3eba454a3e7b3042c319f2b418dc3eac81ae / 최대 연속 저레벨 2.73s / true peak -2.2 dBTP.
- B: Breath_Upon_The_Pass_B_LoopPlan_Review.wav / 11.45s / SHA256 5f0c9ca2647de14f0ce8953a839dae45612cf37080af8d76907b44cf118c12b9 / 최대 연속 저레벨 0s / true peak -1.9 dBTP.
- 저레벨 기준 10ms RMS < -50dBFS. 예상 반복 경계 저레벨 0s.
- float64 가상 Intro/Loop 렌더 경계 샘플 변화: crossfade 진입 0.006701, loop repeat 0.007864, intro handoff 0.007859, loop 내부 변화 p99.9 0.114018. 기계적 클릭 proxy PASS. 실제 최종 WAV 및 전체 true peak 재검증은 Master 단계.
- 전체 감쇠된 소스 sample peak -1.843 dBFS.

## 납품
LIMITLESS_Issue129_LoopReview_20261124.zip / 3,843,795 bytes / SHA256 6c565077e19c3281bba835aec720ddfa013d73441f9808aa1106a86cbb5feac1 / ZIP CRC PASS. WAV2 + MANIFEST.json + README.txt + SHA256SUMS.txt. 비교본만이며 Master 아님.

## 보호 및 다음
B 및 -2dB gain 승인 후에만 최종 Intro/Loop WAV 제작. 상업권 COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING 유지. 기존 승인곡4/WAV8 KEEP, 신규 생성0, Unity0, main0, #85 HOLD 보호.
