# LIMITLESS Issue #127 — F2/F3 Loop A-B Review QA / 2026-11-23

- 담당: BGM
- 브랜치: `docs/bgm`
- 상태: **F2_F3_USER_APPROVED / LOOP_AB_READY / B_LOOP_TECH_PASS / LOOP_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED**
- 기존 승인 원곡 변경: 0
- 신규 생성: 0
- Unity 6000.6.5f1 / TTS / Mixer / Save/UserData 변경: 0

## 1. 사용자 청취 승인

바울 대표 직접 피드백 **“나도 좋아.”**를 Issue #127의 F2/F3 두 1차 데모에 대한 청취 승인으로 접수했다.

- F2 `Beneath the Frozen Boughs` → **F2_USER_APPROVED**
- F3 `Steps on Frozen Stone` → **F3_USER_APPROVED**
- 재생성 불필요.

## 2. 원본 보호

### F2
- Source: `Beneath_the_Frozen_Boughs.mp3`
- SHA256: `c3f1339e30d8973ad467e30650072f9fc2360ea1f90627a3570bfcaeb384e3b1`

### F3
- Source: `Steps_on_Frozen_Stone.mp3`
- SHA256: `ce3aa05a5235791892dd222bb8e958084634c085f69edfae0ec31850ec6d20e7`

원본 두 MP3는 편집·덮어쓰기하지 않았다.

## 3. Loop A/B 후보

### F2 — Beneath the Frozen Boughs
- Intro 후보: 0 → **13.560s**
- Loop Body 후보: **13.560 → 62.775s**
- Crossfade: **0.55s**
- 자동 beat grid 기준 시작/끝이 같은 위상 후보로 선택.
- B 경계 near-silence 최대: **0.02s**
- 경계 delta: 약 **0.0693**
- 파일 전체 99.9 percentile delta: 약 **0.0785**
- 판정: **B_LOOP_TECH_PASS**

### F3 — Steps on Frozen Stone
- Intro 후보: 0 → **24.102s**
- Loop Body 후보: **24.102 → 81.107s**
- Crossfade: **0.55s**
- 자동 beat grid 기준 시작/끝이 같은 위상 후보로 선택.
- B 경계 near-silence 최대: **0.02s**
- 경계 delta: 약 **0.1114**
- 파일 전체 99.9 percentile delta: 약 **0.1183**
- 판정: **B_LOOP_TECH_PASS**

F3는 최초 tentative 24.5→84.1s보다 반복 경계 transient/beat-grid 안정성이 좋은 후보로 조정했다.

## 4. 비교 청취 파일

ZIP:
`LIMITLESS_Issue127_LoopReview_20261123.zip`

- Size: **1,168,179 bytes**
- SHA256: `71cd20113303053b74ed77230089d2e48bfd232f117e9c78c638555d85954b8e`
- ZIP CRC: PASS

포함:
- `Beneath_the_Frozen_Boughs_A_OriginalBoundary.mp3`
  - SHA256 `ad997adf7784877f605a7d50bf798c1cc089e9d1b59da2a766b0daac871b81bc`
- `Beneath_the_Frozen_Boughs_B_LoopPlan_Review.mp3`
  - SHA256 `c00a61759891030dbaee289c16dbfad440c31425fa5312584d9d045fb5594970`
- `Steps_on_Frozen_Stone_A_OriginalBoundary.mp3`
  - SHA256 `07e6828bdf366330042b9c9fb923bb73c4ea57eb95bd8fbd7721250e754cb326`
- `Steps_on_Frozen_Stone_B_LoopPlan_Review.mp3`
  - SHA256 `7bddb2a723d73bd417173da477f16689e9c9659ee1c8d83467b0b9e2e59ac520`

4 MP3 decode PASS.
A는 승인 원곡의 끝→처음 문제를 들려주는 비교본이며, B는 0.55s crossfade 후보다.
둘 다 **검수용 Preview**이며 배포 Master가 아니다.

## 5. 현재 결론

- 두 곡 자체: **USER_APPROVED**
- B Loop 후보: **기계 QA PASS**
- 루프의 음악적 연결감: **대표 청취 승인 대기**
- Master: 미제작
- Unity 적용: 0
- main 병합: 0
- 상업권: `COMMERCIAL_GAME_USE_EXPLICIT_LICENSE_NOT_VERIFIED` 유지

다음 게이트:
바울 대표가 A/B를 비교 청취하여 각 곡 B 편집을 승인한 뒤에만 게임용 Intro/Loop Master를 제작한다.
