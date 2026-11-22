# LIMITLESS Chapter3 BGM Loop Edit / A-B QA — 2026-11-23

- 담당: BGM
- 브랜치: `docs/bgm`
- 근거: Issue #31 2026-11-23 후속 발주 및 바울 대표 승인 상태 정정
- 상태: **SONG_APPROVED_BOTH / SOURCE_SHA_PASS / B_LOOPPLAN_TECH_PASS / LOOP_EDIT_USER_APPROVAL_PENDING / MASTER_NOT_BUILT / UNITY_NOT_APPLIED**
- Unity: **6000.6.5f1 보호 / 접근·Import·수정 0**
- 기존 공식 BGM/TTS/Mixer/Save/UserData: 변경 0
- 신규 Gemini 재생성: 0
- 보스 풀트랙 작업: 0

## 1. 원곡 승인 및 SHA 보호

두 원곡은 바울 대표가 2026-10-20 이미 최종 청취 승인했다. 재승인 요청하지 않는다.

### Main22 F1
- 곡: `Where the Grass Bends`
- 파일: `Where_the_Grass_Bends.mp3`
- 길이: 74.239958초
- 44.1kHz / Stereo / MP3
- 원본 SHA256: `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`
- 상태: **USER_LISTENING_APPROVED / SOURCE_SHA_PASS**

### Main23 서릿길 마을
- 곡: `Hearth in the Low Valley`
- 파일: `Hearth_in_the_Low_Valley.mp3`
- 길이: 72.620333초
- 44.1kHz / Stereo / MP3
- 원본 SHA256: `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`
- 상태: **USER_LISTENING_APPROVED / SOURCE_SHA_PASS**

원본 두 파일은 편집·덮어쓰기하지 않았다.

## 2. B 방식 LoopPlan 최종 검토 후보

이전 1차 B 방식(본문 내부 유사 구간 + 짧은 crossfade)을 유지하되, 박자 위상과 다이내믹을 다시 계산해 경계를 미세 조정했다. 이 값은 **배포 Master가 아니라 최종 루프 편집 승인용 후보**다.

### Where the Grass Bends
- Intro 보존: 0.00 → 14.60초
- Loop Body 후보: **14.60 → 68.10초**
- Body 길이: **53.50초**
- Crossfade: **0.55초**
- 약 80 BPM 계열 기준으로 반복 길이가 박자 격자에 거의 정렬됨.
- 기존 14.80→68.05 초안보다 박자 위상 정렬과 경계 RMS 일치를 개선.

권장 구조:
`Intro 0–14.60s → Loop Body 14.60–68.10s ↺`

### Hearth in the Low Valley
- Intro 보존: 0.00 → 4.35초
- Loop Body 후보: **4.35 → 60.80초**
- Body 길이: **56.45초**
- Crossfade: **0.55초**
- 약 76 BPM / 세부 onset grid 기준 반복 길이가 거의 정렬됨.
- 기존 4.32→60.72 초안에서 경계를 소폭 정리.

권장 구조:
`Intro 0–4.35s → Loop Body 4.35–60.80s ↺`

## 3. 반복 경계 기술 검증

### A / 원곡 끝→처음 비교
원곡 그대로 끝까지 재생 후 처음으로 돌아가는 비교본.

- Grass: -50dBFS 이하 연속 무음 최대 약 **2.53초**
- Hearth: -50dBFS 이하 연속 무음 최대 약 **3.07초**
- 판정: **RAW_WRAP_FAIL** — 무음/종지 때문에 필드·마을 상시 Loop에 부적합.

### B / LoopPlan 비교
제안 Loop Body의 끝 6초 + 시작 6초를 0.55초 crossfade로 연결한 소형 비교본.

#### 무음
- Grass B: 경계 포함 파일에서 -50dBFS 이하 연속 무음 **0.00초**
- Hearth B: **0.00초**
- 판정: **PASS**

#### Hard click
경계 ±0.8초의 sample-to-sample delta를 파일 전체 고변화 구간과 비교했다.

- Grass B: 경계 최대 delta 약 **0.066**, 파일 전체 99.9 percentile 약 **0.086**
- Hearth B: 경계 최대 delta 약 **0.059**, 파일 전체 99.9 percentile 약 **0.063**
- 경계가 파일 내부의 일반적인 순간 변화보다 비정상적으로 큰 spike를 만들지 않음.
- 판정: **기계검사상 HARD_CLICK 없음 / PASS**

#### 박자 끊김
자동 onset/beat 추적 기준:

- Grass B: 약 **79.5 BPM** 계열. 경계 인접 beat interval 약 **0.65~0.75초**, 전체 중앙값 약 **0.743초**. 누락 beat 수준의 긴 공백은 없음. crossfade 내부의 가벼운 완급 변화는 있으나 끊김으로 판정할 정도의 gap은 검출되지 않음.
- Hearth B: 약 **152 BPM 세부 grid(체감 약 76 BPM)**. 경계 인접 interval 약 **0.337~0.395초**, 중앙값 약 **0.395초**. 연속성 유지.
- 판정: **MECHANICAL_RHYTHM_CONTINUITY_PASS**
- 단, 음악적 프레이즈의 최종 선호 판단은 바울 대표의 루프 편집 청취 승인으로 확정한다.

## 4. 비교 청취 파일

실제 작업공간:
`/mnt/data/LIMITLESS_Issue31_LoopReview_20261123/`

### Where the Grass Bends
A / 원곡 경계:
- `Where_the_Grass_Bends_A_OriginalBoundary.mp3`
- SHA256: `d05fdc2e7622f0cc767fbefd8b55f0289d9bf3438bbc05c08bcc7b0127c512d4`

B / LoopPlan:
- `Where_the_Grass_Bends_B_LoopPlan_Review.mp3`
- SHA256: `a0ec5513f719fcd1dfb9f2406b3172657cacbcff53ea2fe53b61b038c6f29a82`

### Hearth in the Low Valley
A / 원곡 경계:
- `Hearth_in_the_Low_Valley_A_OriginalBoundary.mp3`
- SHA256: `320c1670fe9dd84a5a94c94c0bbeae5fbb200283461b9b0b6595ee821ebd539c`

B / LoopPlan:
- `Hearth_in_the_Low_Valley_B_LoopPlan_Review.mp3`
- SHA256: `809218890960139c5c185f164f1fcce8f3451e5777dade1f1e56bf70f2cae68c`

묶음:
- `LIMITLESS_Issue31_LoopReview_20261123.zip`
- Size: 2,199,087 bytes
- SHA256: `65a37dc8322a0eb5ed6e52527de1ac0754a5bd3252e108dd6dd56bd1406a3484`

검증:
- 4 MP3 전체 ffmpeg decode **PASS**
- 44.1kHz / Stereo 유지
- 추가 gain / EQ / compression: **0**
- 원본 SHA 변경: **0**
- GitHub binary 등록: **0**
- 위 파일은 **비교 청취용 Review Preview**이며 배포용 Master가 아니다.

## 5. TTS / Mixer / Unity 경계

- 원본 곡의 gain/EQ/compression을 바꾸지 않았으므로 기존 대사 공간을 훼손하는 믹싱 변경 없음.
- 실제 TTS 동시재생 QA: 이번 범위에서 **NOT_RUN**.
- 기존 Mixer / ducking 정책 변경: 0.
- Unity 6000.6.5f1 Import/Play/ProjectVersion/Packages 변경: 0.
- 배포용 Master / Unity 연결: **대표의 LoopPlan 최종 승인 후 별도 단계**.
- Issue #85 HOLD 유지.

## 6. 현재 결론

- 두 원곡: **최종 청취 승인 완료 / 재승인 불필요**
- B 방식 LoopPlan: **기계 QA PASS / 최종 청취 승인 대기**
- A 원곡 wrap: 무음 때문에 비권장
- 배포 Master: **미제작**
- Unity 적용: **0**
- 기존 공식 BGM·TTS·Mixer·Save/UserData: **보호**

다음 게이트는 바울 대표가 비교 청취본에서 각 곡의 **B LoopPlan 편집만 최종 승인**하는 것이다. 승인 후에만 별도 Master/Loop 파일 제작 및 Unity 적용 계약으로 이동한다.
