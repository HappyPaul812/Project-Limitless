# LIMITLESS Chapter3 BGM Loop Edit / A-B QA — 2026-11-23

- 담당: BGM
- 브랜치: `docs/bgm`
- 근거: Issue #31 2026-11-23 후속 발주
- 상태: **SOURCE_SHA_PASS / APPROVAL_RECOVERY_NOT_FOUND / LOOP_EDIT_PLAN_READY / SMALL_AB_READY / USER_LISTENING_REQUIRED / UNITY_NOT_APPLIED**
- Unity: **6000.6.5f1 보호 / 접근·Import·수정 0**
- 기존 승인 BGM/TTS/Mixer/Save/UserData: 변경 0
- 보스 풀트랙 신규 제작: 0
- 신규 Gemini 재생성: 0

## 1. 원본 SHA 재확인

### Main22 F1 후보
- 파일: `Where_the_Grass_Bends.mp3`
- 길이: 74.239958초
- 44.1kHz / Stereo / MP3
- SHA256: `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`
- 2026-10-20 기록과 **일치 PASS**
- 기존 끝 무음: 약 71.88초 이후, 약 2.36초

### Main23 서릿길 마을 후보
- 파일: `Hearth_in_the_Low_Valley.mp3`
- 길이: 72.620333초
- 44.1kHz / Stereo / MP3
- SHA256: `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`
- 2026-10-20 기록과 **일치 PASS**
- 기존 시작 무음 약 0.45초 / 끝 무음 약 2.55초

## 2. 사용자 청취 승인 복구 판정

Issue #31의 2026-11-23 지시는 “11/19 승인 복구 ref 참조”를 요구한다.
확인한 범위:
- Issue #31 전체 댓글
- `docs/bgm` 최신 `BGM_LATEST.md`
- 2026-11-19 마감/복구 Issue #121의 BGM 관련 기록

위 범위에서는 두 곡에 대한 **명시적 사람 청취 승인 문구를 복구하지 못했다**.
`BGM_LATEST.md`의 최신 상태도 두 곡을 `USER_REVIEW_PENDING`으로 남긴다.

따라서:
- 두 곡을 **최종본/공식 Master라고 부르지 않음**
- 역할 적합 후보 상태 유지
- 실제 배포용 Master / Unity 적용 보류
- 상태: **APPROVAL_RECOVERY_NOT_FOUND / USER_LISTENING_REQUIRED**

## 3. Loop Edit 계획

### A. Where the Grass Bends — F1

단순 끝 무음 제거 후 0초로 되감는 방식은 권장하지 않는다.
원본 시작부가 매우 조용하고 후반부는 상대적으로 강해, 끝→처음에서 다이내믹 리셋이 들릴 가능성이 크다.

기계적 후보 분석:
- Intro 보존 후보: 0.00 → 약 14.80초
- Loop body 후보: 약 **14.80 → 68.05초**
- 후보 접점의 국소 loudness는 대략 -15.4dB 수준으로 서로 근접
- 음색/화성 유사도 휴리스틱 약 0.92
- Crossfade 후보: **0.55초**, 짧은 equal/linear 계열
- 실제 Master 확정 전 사람 청취로 박/프레이즈/호흡 확인 필요

권장 구조:
`Intro 0–14.80s → Loop Body 14.80–68.05s ↺`

주의:
현행 Unity BGM 재생기가 Intro + Body 분리 재생을 지원하는지 이번 BGM 작업에서 변경/검증하지 않았다.
Unity 구현은 별도 Codex 계약 전 **HOLD**.

### B. Hearth in the Low Valley — 서릿길 마을

시작/끝 무음만 제거한 전체곡 반복도 가능성은 있으나 종지감이 남는다.
본문 내부의 유사 구간을 이용한 Intro + Body 방식이 더 안전한 후보다.

기계적 후보 분석:
- Intro 보존 후보: 0.00 → 약 4.32초
- Loop body 후보: 약 **4.32 → 60.72초**
- 후보 접점 국소 loudness는 약 -14.0dB로 매우 근접
- 음색/화성 유사도 휴리스틱 약 0.97
- Crossfade 후보: **0.55초**
- 실제 사람 청취로 멜로디 프레이즈/박자 겹침 확인 필요

권장 구조:
`Intro 0–4.32s → Loop Body 4.32–60.72s ↺`

## 4. 소형 A/B 비교본

**배포용 Master가 아니다.** 승인 판단용 짧은 비교 파일만 생성했다.
원본 두 MP3는 수정하지 않았다.

### Where the Grass Bends
A / Raw Wrap:
- 내용: 원본 66.00s→끝 + 원본 0→6.00s
- 길이: 14.288980초
- SHA256: `30a415e9bd9c0fbf98593dcf4bb5ad7f4227cf312ef9ead4597e8e89fa680735`
- 목적: 끝 무음과 원점 복귀 문제 비교

B / Loop Plan:
- 내용: 62.05→68.05s + 14.80→20.80s, 0.55s crossfade
- 길이: 11.493878초
- SHA256: `d369f299f2da794ce3ff4921fb3a402ca450bd426872483eb8bbfd40ea46877a`
- 목적: 제안 loop boundary의 접합만 비교

### Hearth in the Low Valley
A / Raw Wrap:
- 내용: 원본 64.60s→끝 + 원본 0→6.00s
- 길이: 14.053878초
- SHA256: `1f7ce6597f2fe10a2918a74cebfa3aadbaeb1df2baceaa85969e9ab1e00267c7`

B / Loop Plan:
- 내용: 54.72→60.72s + 4.32→10.32s, 0.55s crossfade
- 길이: 11.493878초
- SHA256: `e6c1cc562d7d4a6da328749a063a3ac37a9de0e4703136801c63d189d1120e17`

### 파일 QA
- 4개 MP3 전체 ffmpeg decode: **PASS**
- Sample peak: A/B 모두 0.0 ~ -0.1dBFS 범위
- 추가 gain/compression/EQ: **0**
- 원본 SHA 변경: **0**
- GitHub binary 등록: **0**
- 실제 작업공간 산출물: ChatGPT 세션 작업공간 `/mnt/data/limitless_bgm_ab_20261123/`
- A/B 파일은 임시 청취 비교본이며 제품 자산으로 취급하지 않는다.

## 5. 클릭 / 박자 / 다이내믹 / TTS 판정

### Raw A
- 두 곡 모두 종료 무음 때문에 루프 공백이 명확히 생기는 구조.
- F1은 후반→매우 조용한 도입부로 복귀하는 다이내믹 리셋 위험이 큼.
- 마을곡은 시작/끝 무음 + 종지 때문에 전체 raw wrap을 제품 loop로 승인하기 어려움.

### Planned B
- 0.55초 crossfade로 샘플 단절에 의한 hard click 위험을 낮춤.
- 기계적 decode/peak 검사 PASS.
- 후보 접점은 음색/다이내믹이 Raw보다 유사함.
- **사람 청취 기준의 박자/프레이즈 자연스러움은 아직 승인하지 않음**.

### TTS 공간
- 원본 믹스의 gain/EQ/compression을 변경하지 않았고, 접점 테스트만 수행.
- 10/20의 “대사 공간 PASS 추천”을 훼손하는 변경은 없음.
- 실제 TTS와 동시 재생 A/B: **NOT_RUN / USER_LISTENING_REQUIRED**
- Mixer/ducking 정책 변경: 0

## 6. 보호 및 보류

- Chapter3 역할 유지:
  - F1 탐험 후보 = `Where the Grass Bends`
  - 서릿길 안전거점 후보 = `Hearth in the Low Valley`
- 기존 BGM/Mixer/TTS/Save/UserData/승인 아트 변경 0
- Unity 6000.6.5f1 접근/Import/Play/ProjectVersion/Packages 변경 0
- Issue #85 HOLD 유지
- 보스 풀트랙 작업 0
- 신규 생성 0
- 배포용 Master 0
- Unity 적용 0

## 7. 다음 승인 게이트

바울 대표가 A/B를 사람 귀로 확인한 뒤 각각:
- `A_RAW_KEEP`
- `B_LOOP_PLAN_APPROVED`
- `REVISE_LOOP_POINT`

중 하나로 판정.

**B_LOOP_PLAN_APPROVED 이후에만** 원본 보존 + 별도 Master/Loop 파일 제작을 허용한다.
Unity 연결은 Master 승인과 별도의 Codex 적용 계약이 필요하다.
