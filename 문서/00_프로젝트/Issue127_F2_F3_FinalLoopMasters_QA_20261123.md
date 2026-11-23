# LIMITLESS Issue #127 — F2/F3 Final Intro+Loop Masters / 2026-11-23

- 담당: BGM
- 브랜치: `docs/bgm`
- 근거: Issue #127 바울 대표 B LoopPlan 최종 승인
- 상태: **F2_F3_SONG_APPROVED / B_LOOP_PLAN_USER_APPROVED_BOTH / FINAL_MASTER_DELIVERED / MASTER_QA_PASS / UNITY_NOT_APPLIED / MAIN_NOT_MERGED**
- 신규 Gemini 생성: 0
- 재편곡/원곡 재작업: 0
- Unity 6000.6.5f1 / 기존 BGM·TTS·Mixer·Save/UserData 변경: 0
- 상업 사용권 상태: `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`

## 1. 승인 원곡 보호

### F2 — Beneath the Frozen Boughs
- Source: `Beneath_the_Frozen_Boughs.mp3`
- 승인 원본 SHA256: `c3f1339e30d8973ad467e30650072f9fc2360ea1f90627a3570bfcaeb384e3b1`
- 원본 변경/덮어쓰기: **0**

### F3 — Steps on Frozen Stone
- Source: `Steps_on_Frozen_Stone.mp3`
- 승인 원본 SHA256: `ce3aa05a5235791892dd222bb8e958084634c085f69edfae0ec31850ec6d20e7`
- 원본 변경/덮어쓰기: **0**

## 2. 승인 B LoopPlan

### F2
- Logical Loop Start: **13.5604535147s**
- Logical Loop End: **62.7751473923s**
- Crossfade: **0.55s**

### F3
- Logical Loop Start: **24.1023129252s**
- Logical Loop End: **81.1073015873s**
- Crossfade: **0.55s**

렌더 구조:
- Intro는 승인 loop head의 0.55s를 포함해 Intro→Loop handoff를 자연스럽게 유지.
- Loop WAV는 `logical start + 0.55s`부터 시작하고, 말단 tail→head 0.55s crossfade를 포함하여 Loop→Loop 반복을 닫음.
- 기존 승인 B Preview의 음악적 연결 방향을 바꾸지 않고 배포용 WAV 구조로만 렌더.

## 3. 최종 게임용 Master

포맷 공통:
- WAV
- PCM16
- 44,100 Hz
- Stereo
- Normalize/EQ/Compression 추가: **0**

### F2
#### Intro
- `Beneath_the_Frozen_Boughs_Intro_FINAL.wav`
- Duration: **14.110454s**
- Size: **2,489,128 bytes**
- SHA256: `090bf9417981173fb9cfc5fd998786413aa8b06c58c7e5b3157bb4e45b050504`

#### Loop
- `Beneath_the_Frozen_Boughs_Loop_FINAL.wav`
- Duration: **48.664694s**
- Size: **8,584,496 bytes**
- SHA256: `8cefeeea2735d28efa796b987254ab6af96be1cbe297062622f029529a55327f`

### F3
#### Intro
- `Steps_on_Frozen_Stone_Intro_FINAL.wav`
- Duration: **24.652313s**
- Size: **4,348,712 bytes**
- SHA256: `9b157be7560f30c5401a7a225431cc791214d1933e4154355e6fca6cb8296036`

#### Loop
- `Steps_on_Frozen_Stone_Loop_FINAL.wav`
- Duration: **56.454989s**
- Size: **9,958,704 bytes**
- SHA256: `09609b95da9f907a98079ad659c4ef8b559e9324b410d22e0e9c96df0662c1f8`

## 4. 최소 Master QA

### F2
- ffmpeg full decode: **PASS**
- Loop repeat 경계 near-silence: **0.00s**
- Loop→Loop sample delta: 약 **0.00626**
- Intro→Loop sample delta: 약 **0.00626**
- Loop 내부 99.9 percentile delta: 약 **0.08707**
- Hard-click 기계검사: **PASS**
- Intro→Loop handoff: **PASS**
- 반복 경계 100ms RMS 차: 약 **2.49dB**

### F3
- ffmpeg full decode: **PASS**
- Loop repeat 경계 near-silence: **0.00s**
- Loop→Loop sample delta: 약 **0.00644**
- Intro→Loop sample delta: 약 **0.00647**
- Loop 내부 99.9 percentile delta: 약 **0.11872**
- Hard-click 기계검사: **PASS**
- Intro→Loop handoff: **PASS**
- 반복 경계 100ms RMS 차: 약 **1.57dB**

판정:
**MASTER_QA_PASS**.
새로운 음악적 변경/재편곡 없이 승인된 B LoopPlan을 게임용 Intro+Loop WAV로 렌더했다.

## 5. 최종 납품

ZIP:
`LIMITLESS_Issue127_FinalLoopMasters_20261123.zip`

- 실제 세션 납품 경로: `/mnt/data/LIMITLESS_Issue127_FinalLoopMasters_20261123.zip`
- Size: **24,120,430 bytes**
- SHA256: `0d0a3a098440edd9f8805cff02891db67530561d63b715bf4d46288d9fe32c2b`
- ZIP CRC: **PASS**

ZIP 포함:
- Final WAV 4개
- `MANIFEST.json`
- `SHA256SUMS.txt`
- `README.txt`

GitHub binary 등록: **0**.
음원은 세션 납품 ZIP으로 전달하며 GitHub에는 SHA/메타/QA만 기록한다.

## 6. 보호 / 다음 단계

- 기존 승인 원곡 MP3 변경0.
- 기존 F1/서릿길 및 Chapter1·2 승인 BGM 변경0.
- TTS/Mixer/Save/UserData 변경0.
- Unity6000.6.5f1 접근/Import/ProjectVersion/Packages 변경0.
- Issue #85 HOLD 유지.
- Unity 제품 적용과 main 병합은 Codex 별도 작업.
- 상업 사용권은 출시 전 별도 게이트 `COMMERCIAL_USE_EXPLICIT_LICENSE_PENDING`으로 관리한다.
