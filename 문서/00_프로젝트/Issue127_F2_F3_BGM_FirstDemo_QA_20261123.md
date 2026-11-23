# LIMITLESS Issue #127 — F2/F3 신규 BGM 1차 데모 수령·QA / 2026-11-23

- 담당: BGM
- 브랜치: `docs/bgm`
- Issue: #127
- 상태: **AUDIO_RECEIVED_2 / TECH_QA_PASS_RECOMMENDED / USER_LISTENING_PENDING / LOOP_MASTER_NOT_BUILT / UNITY_NOT_APPLIED**
- 신규 생성: F2 1곡 + F3 1곡, 발주 범위 내
- 기존 #31 F1·서릿길 승인곡 재생성/변경: 0
- 유료 API/키조회/자동 호출: 0

## 1. 곡 배정

### F2 첫눈의 숲
- 생성 파일: `Beneath_the_Frozen_Boughs.mp3`
- 권장 곡명: **Beneath the Frozen Boughs**
- 배정: **F2 첫눈의 숲**
- 기술/음악 판정: **PASS 추천 / USER_LISTENING_PENDING**

청취상:
- 초반 피아노와 가벼운 목관 중심의 서늘하고 조심스러운 분위기가 F2의 첫눈·살아 있는 숲에 적합.
- F1보다 한 단계 차가워졌지만 공포/전투/설원 재난으로 가지 않음.
- 중반 이후 저현이 더해져 한기 위협과 형제 사이의 조심스러운 긴장을 무겁지 않게 암시.
- F3보다 더 섬세하고 공기감이 있어 두 지역의 역할 구분이 분명함.
- 기존 F1을 그대로 반복한 인상은 아니며, F1→F2의 온도 하강이 자연스럽다.

### F3 눈 덮인 고갯길
- 생성 파일: `Steps_on_Frozen_Stone.mp3`
- 권장 곡명: **Steps on Frozen Stone**
- 배정: **F3 눈 덮인 고갯길**
- 기술/음악 판정: **PASS 추천 / USER_LISTENING_PENDING**

청취상:
- 낮은 현악과 반복되는 움직임이 F2보다 지형의 무게, 좁은 고갯길, 신중한 전진을 더 강하게 표현.
- 결의와 보호 본능은 느껴지지만 보스/전투곡으로 과장되지 않음.
- 후반 오케스트레이션이 넓어져 북부 원정의 진척감을 주되 영웅적 승리곡까지 가지 않음.
- F2와 음색/리듬 중심이 충분히 달라 연속 필드에서 같은 곡의 변주처럼만 들리지 않는다.

## 2. 파일 기술 QA

### Beneath_the_Frozen_Boughs.mp3
- Size: **1,843,393 bytes**
- SHA256: `c3f1339e30d8973ad467e30650072f9fc2360ea1f90627a3570bfcaeb384e3b1`
- Duration: **76.4865 sec**
- Sample rate: **44,100 Hz**
- Channels: **Stereo**
- Integrated loudness: 약 **-11.33 LUFS**
- True/Sample peak: 약 **-0.09 dBFS**
- Leading near-silence: 약 **0.07 sec**
- Trailing near-silence: 약 **1.91 sec**
- 자동 beat estimate: 약 **156.6 BPM 세부 grid / 체감 약 78.3 BPM**
- clipping sample: 0
- Decode: **PASS**
- 길이 발주 60–85초: **PASS**

### Steps_on_Frozen_Stone.mp3
- Size: **2,227,707 bytes**
- SHA256: `ce3aa05a5235791892dd222bb8e958084634c085f69edfae0ec31850ec6d20e7`
- Duration: **92.4996 sec**
- Sample rate: **44,100 Hz**
- Channels: **Stereo**
- Integrated loudness: 약 **-11.65 LUFS**
- True/Sample peak: 약 **0.00 dBFS** (sample peak 약 -0.006 dBFS)
- Leading near-silence: 약 **0.07 sec**
- Trailing near-silence: 약 **2.67 sec**
- 자동 beat estimate: 약 **161.5 BPM 세부 grid / 체감 약 80.7 BPM**
- 0 dBFS 초과 clipping: **0**
- Decode: **PASS**
- 파일 전체는 90초 상한을 약2.5초 초과하나, 끝 무음 2.67초 제외한 실제 음악 구간은 약 **89.83초** → **재생성 불필요 / 승인 후 꼬리 제거 시 규격 PASS**

## 3. 레벨/대사 공간

두 파일은 평균 소스 레벨이 서로 매우 근접:
- F2 약 -11.33 LUFS
- F3 약 -11.65 LUFS
- 차이 약 0.32 LU

따라서 F2→F3 전환에서 원본 소스 레벨 점프는 작다.

다만 게임용 최종 레벨은 기존 BGM Mixer 정책을 따르며, 이번 단계에서 normalize/compress/EQ하지 않는다.
청취상 대사·발자국·UI가 들어갈 공간은 확보되어 있으나 실제 Korean TTS 동시재생은 **NOT_RUN**. Unity/Codex 단계에서 별도 Delta QA.

## 4. Loop 후보 — 아직 편집하지 않음

원본 그대로 끝→처음 반복은 두 곡 모두 끝 무음 때문에 권장하지 않는다.

기계적 음색/화성/박자 후보:
- F2 `Beneath the Frozen Boughs`
  - Intro 후보: 0 → 약 **12.8s**
  - Loop Body 후보: 약 **12.8 → 62.4s**
  - 추후 0.5~0.6s crossfade 검토
- F3 `Steps on Frozen Stone`
  - Intro 후보: 0 → 약 **24.5s**
  - Loop Body 후보: 약 **24.5 → 84.1s**
  - 추후 0.5~0.6s crossfade 검토

위 시간은 **최종 LoopPlan 승인값이 아니라 후보**. 곡 자체 사용자 청취 승인 후 A/B를 만들고 사람 귀로 확정한다.

## 5. 기존곡 대비

- F1 `Where the Grass Bends`: 약한 북풍·전이지대
- F2 `Beneath the Frozen Boughs`: 첫눈·섬세한 주의·한기 첫 위협
- F3 `Steps on Frozen Stone`: 깊어진 적설·지형 무게·결의와 방어

세 곡의 온도/무게 단계가 **F1 < F2 < F3**로 자연스럽게 상승.
F2/F3 서로의 중심 악기·리듬·다이내믹도 구분됨.

## 6. 생성 도구/권리

- 사용자 수동 Gemini Create Music 워크플로로 생성된 것으로 접수.
- Google 공식 도움말상 Gemini Apps 음악 생성은 Lyria 기반이나, **실제 생성 세션의 정확한 모델 버전/계정 티어는 파일에서 검증 불가**.
- 신규 유료 API 호출/키조회0.
- 기존 기록과 동일하게:
  - Google generated-original ownership claim 없음: 확인됨.
  - 무료 Gemini/Lyria 생성 음악의 유료 게임 상업사용을 명시적으로 보장하는 별도 라이선스 문구: **NOT_VERIFIED**.
- 상태: `COMMERCIAL_GAME_USE_EXPLICIT_LICENSE_NOT_VERIFIED`.
- 최종 상용 배포 자산 승인 전에 권리 게이트 유지.

## 7. 현재 승인 게이트

BGM 담당 기술/음악 평가:
- F2: **PASS 추천**
- F3: **PASS 추천**
- 재생성: **권장하지 않음**

바울 대표가 실제 청취 후:
- `F2_USER_APPROVED`
- `F3_USER_APPROVED`

여부를 확정한다.

승인 전:
- Loop Master 제작0
- Unity 적용0
- main 병합0
- 기존 승인 BGM/TTS/Mixer/Save/UserData 변경0
- Issue #85 HOLD 유지
