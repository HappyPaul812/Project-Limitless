# LIMITLESS Chapter3 BGM 파일럿 2곡 1차 청취·파일 QA — 2026-10-20

- 담당 브랜치: `docs/bgm`
- Issue: #31
- 상태: **COMPOSITION_PASS_RECOMMENDED / LOOP_EDIT_REQUIRED / USER_REVIEW_PENDING / UNITY_NOT_APPLIED**
- 소스: 이 BGM 담당 세션에 사용자가 업로드한 MP3 2개. GitHub/Unity에는 아직 음원 파일을 등록하지 않음.

## 1. Main22 F1 — Where the Grass Bends

- 파일명: `Where_the_Grass_Bends.mp3`
- 길이: 74.239958초
- Sample rate: 44,100 Hz
- Channels: Stereo 2ch
- Size: 1,787,885 bytes
- SHA256: `2bbbe6fe02b46c391653a5409802dde819ba64b5b5be99b20bbeafb4ad9b0fca`
- Integrated loudness 측정: 약 -11.1 LUFS
- Loudness range: 약 15.8 LU
- 끝 무음: 약 71.88초부터 종료(약 2.36초)

### 청취 판정
**F1 역할 적합 PASS 추천.**
초록이 남은 전이지대의 서늘함, 조심스러운 이동, 약한 조사 긴장이 들리며 전투·보스·완전한 설원 음악처럼 과장되지 않는다. 북부 첫 진입의 “낯선 공기”가 중심이고 절망감은 낮다. 대화가 들어갈 여백도 확보되어 있다.

### Loop
원본 MP3 직접 Loop는 **FAIL/보완 필요**. 2.36초 무음 꼬리 때문에 wrap 지점이 드러난다.
사용자 곡 승인 후 원본 보존, 별도 Loop Edit에서 후반 무음 제거 + 음악적 접점 청취 + 짧은 crossfade 후보를 만든다.

## 2. Main23 서릿길 마을 — Hearth in the Low Valley

- 파일명: `Hearth_in_the_Low_Valley.mp3`
- 길이: 72.620333초
- Sample rate: 44,100 Hz
- Channels: Stereo 2ch
- Size: 1,749,014 bytes
- SHA256: `a8cae73d15d276ad5cf25415ee13bce6e9761b331822f8dcb5928da9b87c772b`
- Integrated loudness 측정: 약 -11.4 LUFS
- Loudness range: 약 10.1 LU
- 시작 무음: 약 0.45초
- 끝 무음: 약 70.07초부터 종료(약 2.55초)

### 청취 판정
**서릿길 마을 역할 적합 PASS 추천.**
따뜻한 현악/목관 중심으로 주민 생활감과 안전거점의 안도감을 주되, 축제·술집·크리스마스풍으로 가지 않는다. 마을 밖의 냉기가 남아 있다는 약한 서늘함도 유지한다. 장시간 대화/상점/파티 관리 배경으로 과도하게 전면에 나오지 않는다.

### Loop
원본 MP3 직접 Loop는 **FAIL/보완 필요**. 시작/끝 무음과 종지 때문에 반복점이 들린다.
사용자 곡 승인 후 시작 약 0.45초와 끝 약 2.55초 무음 제거를 우선 검토하고, 음악적 접점 기준 별도 Loop Edit를 만든다.

## 3. 두 곡 비교 / 레벨

- 역할 분리: **PASS**. F1은 차갑고 탐색적, 마을은 따뜻하고 생활감 있음.
- 길이 45~90초: **PASS**
- 44.1kHz Stereo decode: **PASS**
- 두 원본의 Integrated loudness 차: 약 0.3 LU로 서로 매우 근접.
- 절대 소스 레벨은 배경음악으로 바로 고정하지 않고 기존 BGM Mixer 정책으로 Unity 적용 시 조정한다.
- TTS/Dialogue 공간: 1차 청취 **PASS 추천**, 실제 더빙과 동시재생 검증은 Unity 적용 후 별도.
- 권리/생성도구: Gemini 생성 원본으로 관리 예정. 상업 이용 근거/생성일/원본 보관은 정식 납품 메타데이터에 추가 기록 필요.

## 4. 현재 결론

- `Where the Grass Bends`: **F1 1차 공식 후보 PASS 추천 / 사용자 승인 대기**
- `Hearth in the Low Valley`: **서릿길 마을 1차 공식 후보 PASS 추천 / 사용자 승인 대기**
- Raw MP3 seamless loop: **두 곡 모두 미승인 / LOOP_EDIT_REQUIRED**
- Unity Import/연결: **0 / NOT AUTHORIZED**
- 기존 Chapter1·2 공식 BGM/TTS/Mixer 변경: **0**
