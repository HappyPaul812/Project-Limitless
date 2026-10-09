# TTS Free Tier Quota 운영 기록

> **TTS quota는 API Key 개수가 아니라 Project + Model의 실제 quota 상태를 기준으로 관리한다. Quota 실패 시 성공 결과를 보존하고 checkpoint에서 남은 StoryOrder만 재개한다.**

## 기록 범위와 근거

2026-10-09 Main18 실제 제작 중단 사례와 후임 ChatGPT/Codex/TTS 세션의 재개 지점을 기록한다. 이번 작업은 문서화 전용이며 TTS API 호출0·WAV 생성/수정0·Unity 변경0·게임 코드 변경0이다. 시작 LOCAL HEAD는 `528f2054a671d7dfc9e16da92bf678fb8751274b`이며 사용자 제공 원격 기준 `528f205`와 같다. 원격 조회나 Push는 수행하지 않았다.

- 정식 입력: [Main18 Manifest](Main18_TTS_Manifest.csv)의 VoiceExpected=true 22개. Manifest 행 순서는 Dialogue ID 정렬이며 StoryOrder는 제작 checkpoint/이번 인계의 번호를 따른다. Manifest 상태는 수정하지 않는다.
- LOCAL 제작 상태: [checkpoint](../../Limitless_TTS_Output_Main18/_DAY1_CALL_CHECKPOINT.json)의 attempts17개와 items, [원본 WAV 폴더](../../Limitless_TTS_Output_Main18/)의 성공15개.
- 오류 이력: checkpoint의 Story15·17 error. [PARTIAL_REPORT](../../Limitless_TTS_Output_Main18/PARTIAL_REPORT.txt)는 Story15 실패 시점의 Total calls=15 기록으로, Story16·17을 포함한 최종17회 기록이 아니다. 덮어쓰지 않고 이 한계를 명시한다.
- 사용자 인계: 성공15·실패2·미호출5·Main19 생성0·Unity 적용0·전체 청취 미완료. Main18 Review CSV와 Main18 전용 생성 스크립트·별도 오늘 결과/오류 로그는 확인한 출력 폴더 및 Tools/TTS·Google TTS에서 발견되지 않았다. 외부 파일 존재 여부는 확정하지 않는다.

출력 폴더와 checkpoint는 현재 미추적 LOCAL 자료다. 이번 문서 commit에는 원본·checkpoint를 포함하지 않으므로 다른 PC/클론에서 링크만으로 자료가 제공된다고 가정하지 않는다. 재개 담당자는 실제 원본과 checkpoint를 먼저 확보·확인한다. Secret 값은 기록하지 않는다.

## 무엇이 잘못됐는가 / 실제 오류

Key별 10회를 독립 예산으로 합산하면 실제 Project + Model 제한을 놓칠 수 있다. Key 수로 총량을 확정하는 계산은 폐기한다. 관측 오류는 다음과 같다.

| 항목 | 관측값 |
| --- | --- |
| HTTP / status | 429 / RESOURCE_EXHAUSTED |
| quotaMetric | generativelanguage.googleapis.com/generate_content_free_tier_requests |
| quotaId | GenerateRequestsPerDayPerProjectPerModel-FreeTier |
| quotaValue | 10 |
| model / location | gemini-3.8-flash-tts / global |
| Story15 retryDelay | 77835s |
| Story17 retryDelay | 75485s |

식별자의 PerDay / PerProject / PerModel이 관측 근거다. 같은 Project에 속한 Key는 해당 Project + Model quota를 공유할 수 있으므로 Key를 바꿔도 새로운 10회가 생긴다고 가정하지 않는다. 실제 Key들의 소속 Project는 현재 자료로 확인되지 않았다. 성공15와 quotaValue10만으로 서로 다른 Project 사용이나 제한 우회·초기화 원인을 확정하지 않는다.

**과거 이력과 새 규칙을 구분한다.** Story15도 실제 quota 오류였고, 이후 Story16 성공·Story17 quota 실패가 checkpoint에 남아 있다. 따라서 당시 첫 quota 오류에서 즉시 중단했다고 기록하지 않는다. Story17 이후 Story18~22 추가 호출은 없었다. 앞으로는 첫 quota 실패에서 즉시 중단하며 다른 Dialogue 호출이나 Key 교체로 계속하지 않는다.

## 앞으로 계산·중단·재개하는 방법

장기 규칙의 정본은 [Free Tier Quota 운영 규칙](TTS_음성_제작_정책.md#free-tier-quota-운영-규칙)이다.

1. 제작 전 Model + Google Project + Key 소속 Project, 실제 남은 quota, 당일 시도 이력과 checkpoint를 확인한다. 같은 Project의 여러 Key를 중복 예산으로 더하지 않는다.
2. Handoff의 가능 최대 호출 수와 이번 작업에 안전하게 사용할 호출 수를 별도로 정한다. 미확인 quota로 20회를 선배정하지 않는다.
3. quota 실패 즉시 성공 목록·실패 ID·미호출 ID·총 시도 수를 기록하고 중단한다. 자동 retry·다른 Dialogue 선호출·Key 무작정 교체·성공 WAV 재생성·checkpoint 삭제를 금지한다.
4. 다음 재개 시 아래 StoryOrder와 checkpoint, 실제 WAV ID·hash를 대조한다. 성공15개는 그대로 보존하고 잔여7개만 순서대로 생성한다. 번호 이동·재번호화·합본을 하지 않는다. 1 Stable Dialogue ID = 1 API Call = 1 WAV, 자동 retry0을 유지한다.
5. 생성 후 checkpoint에 시도와 결과를 기록한다. 기존 성공 파일 누락·hash 불일치가 있으면 자동 재생성하지 않고 보고한다.

## 2026-10-09 Main18 Day1 결과

| 구간 | 상태 | 수 |
| --- | --- | ---: |
| Story01~14 | SUCCESS（checkpoint: SAVED） | 14 |
| Story15 / main18_watcher_witness_taeon_01 | FAILED（실제 quota 오류） | 1 |
| Story16 / main18_watcher_witness_miel_01 | SUCCESS（checkpoint: SAVED） | 1 |
| Story17 / main18_watcher_witness_paul_01 | FAILED_QUOTA（checkpoint: FAILED） | 1 |
| Story18~22 | NOT_CALLED | 5 |

전체22 = 성공15 + 실패2 + 미호출5. API 시도17 = 성공15 + 실패2. 잔여7 = 실패2 + 미호출5이며 **15 + 7 = 22**다. 성공 WAV는 원본 제작 단계다. **UNITY_APPLIED = 0**, 사용자 전체22개 직접 청취 미완료, Main19 TTS 생성0·아직 시작하지 않음이다. 성공 저장은 청취 PASS를 의미하지 않는다.

## 다음 재개 대상 — 정확히 7개

| StoryOrder | Dialogue ID | 현재 상태 |
| --- | --- | --- |
| Story15 | main18_watcher_witness_taeon_01 | FAILED |
| Story17 | main18_watcher_witness_paul_01 | FAILED_QUOTA |
| Story18 | main18_after_watcher_serin_01 | NOT_CALLED |
| Story19 | main18_after_watcher_serin_02 | NOT_CALLED |
| Story20 | main18_after_watcher_serin_03 | NOT_CALLED |
| Story21 | main18_deeper_route_serin_01 | NOT_CALLED |
| Story22 | main18_deeper_route_serin_02 | NOT_CALLED |

**이 StoryOrder 그대로 재개한다. Story01~14 및 Story16 성공 WAV15개는 절대 재생성하지 않는다.**

## 그 다음 순서와 Unity 적용 잠금

1. 실제 quota 상태 확인 후 Main18 잔여7개 생성.
2. Main18 전체22개의 존재·Stable ID·checkpoint 대응 확인.
3. 사용자 전체22개 직접 청취.
4. 필요한 재녹음만 별도 수행·검수.
5. 사용자 승인 후 별도 Codex 작업으로 Main18 Unity 정식 적용.
6. Main19 TTS13개 생성. 실제 quota 상태에 따라 제작 시점을 조절한다.

Main18 전체22개 완성 및 사용자 직접 청취 승인 전 Unity Voice 폴더 복사·StoryVoiceCatalog 변경·Dialogue 연결·기존 Voice 변경을 금지한다. Main18 성공 WAV를 희생하거나 재생성해 quota를 낭비하지 않는다.

## 미확정 사항

정확한 reset clock, 실제 Key 소속 Project, 현재 남은 quota와 재개 가능 시각은 미확정이다. `retryDelay`는 각 오류 응답에서 받은 당시 값이며 특정 한국시간 reset 시각을 보증하지 않는다. “한국 날짜 변경”, “Key 교체”, “어제 사용했으니 오늘20회”는 관측 사실이 아니다. 공식 자료/실제 계정 상태 재확인 전 reset을 한국시간 XX시로 정본화하지 않는다. 이번에는 외부 조회·API 호출을 수행하지 않았다.

## 문서 작성 후 검증

- Manifest VoiceExpected22, Main19 VoiceExpected13과 인계 수 일치. 재개7개 ID는 Main18 Manifest의 서로 다른 음성 대상과 정확히 일치.
- checkpoint attempts17 = SAVED15 + FAILED2, Story15/17 실패·Story16 성공 확인. Story18~22 시도0.
- 실제 WAV15개와 checkpoint SAVED15개 ID·SHA256 일치. 잔여7개 WAV 없음. 15 + 7 = 22.
- 기존 정책과 신규 장기 운영 규칙의 충돌 없음. 과거 합본/분할 및 Story15 이후 호출 이력은 향후 규칙 준수 사례로 승격하지 않음.
- Main18 Manifest·checkpoint·보고서·성공 WAV15개 변경0 확인. 이번 문서3개만 commit 대상으로 제한. Main19·Unity·C#·API Key 파일 수정0, Secret 기록0. 청취 및 Unity 실행 검증은 이번 문서화 범위에 포함하지 않음.
