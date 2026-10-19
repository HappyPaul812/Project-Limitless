# Main18 세린 승인 음성10 최종 Unity 통합 QA — 2026-10-19

**승인10 적용 완료 / Main18 NPC22 전체 Runtime PASS / 정상25 보호 / QA_PARTIAL_KNOWN_SEARCH_AND_EDITOR_VERSION / PUSH_NO / MAIN_NOT_MERGED**

- 지시·청취/사용 승인: [GitHub 이슈 #17](https://github.com/HappyPaul812/Project-Limitless/issues/17), docs/tts `55805caa3dee01025cc66bb6c41b0e3278923e2d`의 `TTS_Main18_Serin10_Approved_Unity_Handoff_20261019.md`.
- 바울 대표의 2026-10-19 최종 승인을 적용했습니다. 과거 PREVIEW_AUDIT/복구 results의 PENDING 표기는 역사 기록으로 보존하고 이번 승인과 구분합니다. 재청취·재생성 승인을 다시 요구하지 않았습니다.
- 브랜치: `codex/tts-main18-serin10-integration-20261019`.
- 기능 commit: `94a68cee54be397b16d8bacd4c3f930bad2eece3`. 이 QA·증거·CURRENT_STATUS는 후속 Docs commit에 기록합니다.
- 기준 main: `c263d9e36fc0570b16130d9c011abf3965def5f7`. 원본 main HEAD `ff022e63dfe364f559fcc29819d27b123ad31127` 불변.
- 독립 worktree: `Temp/Main18Serin10Integration20261019/worktree`. 원본 Unity 디스크에는 적용하지 않았습니다.

## 1. 10개 적용 결과

[최종 소스·SHA·GUID·Unity 경로·원문·Runtime 결과 전수표](Main18_Serin10_Final_Source_Matrix.csv).

|StoryOrder|Dialogue ID|실제 승인 출처|초|Runtime / PCM 최대오차|
|---:|---|---|---:|---|
|2|`main18_leon_serin_01`|승인 DSP PREVIEW|5.20|PASS / 0|
|6|`main18_ground_serin_01`|승인 샘플|6.52|PASS / 0|
|7|`main18_heat_vent_serin_01`|승인 수동 복구|4.88|PASS / 0|
|8|`main18_overheat_serin_01`|승인 DSP PREVIEW|4.96|PASS / 0|
|9|`main18_overheat_serin_02`|승인 DSP PREVIEW|2.96|PASS / 0|
|10|`main18_overheat_serin_03`|승인 DSP PREVIEW|6.92|PASS / 0|
|11|`main18_overheat_serin_04`|승인 DSP PREVIEW|3.52|PASS / 0|
|12|`main18_vibration_serin_01`|승인 DSP PREVIEW|5.52|PASS / 0|
|13|`main18_watcher_witness_serin_01`|승인 수동 복구|4.96|PASS / 0|
|14|`main18_watcher_witness_serin_02`|승인 수동 복구|4.20|PASS / 0|

- ground는 지정 `sample_candidate`의 SHA `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`와 일치합니다.
- DSP6은 `LIMITLESS_Serin6_ConsistentVoice_Previews.zip`의 `_PREVIEW.wav` bytes와 추출본이 모두 일치합니다. ZIP SHA `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`, CRC 오류0, PREVIEW_AUDIT 및 승인 인계의 개별6 SHA도 일치합니다.
- 복구3은 `ManualRecovery_20261019/candidates`만 사용했습니다. 각 results SHA·attempts Dialogue ID/화자/StoryOrder/ExactText SHA를 실제 bytes와 대조했습니다. 불합격 `remaining9_wav/main18_heat_vent_serin_01.wav`나 옛 남성/변동 음색은 사용하지 않았습니다.
- 최종 이름은 모두 `<DialogueId>.wav`. PREVIEW 접미사만 제거했고 소스 bytes를 그대로 복사했습니다. 24kHz/mono/PCM16, decode10/10, clipping0, 서로 다른 파일/PCM10개입니다.
- TTS API0, 추가 DSP/Trim/Normalize/Pitch/EQ/Speed/재인코딩0. 이미 승인된 DSP6을 되돌리거나 다시 보정하지 않았습니다.

## 2. 최소 변경과 보호

- 신규 WAV10 + 새 meta10, 기존 StoryVoiceCatalog에 `companion_serin` 항목10만 추가했습니다. 기존 WAV/meta/GUID 덮어쓰기0, 새 GUID 충돌0.
- Catalog **280→290**, 기존280개 항목의 전체 bytes prefix/ID/Clip GUID/Speaker 보존. LOCAL Catalog는 작업 시작 시 최신 main과 내용이 같음을 확인했습니다.
- 기존 Main18 정상12 + Main19 정상13의25개 WAV·meta·Catalog 매핑을 유지했습니다. 정상25는 Source Matrix의 기존 SHA와도 일치합니다.
- Main18 Manifest는 성공 후 해당10개의 Status만 `REGEN_REQUIRED → RUNTIME_VERIFIED`로 바꿨습니다. 기존12 Status·Player/지문·원문·화자·ID 및 나머지 행은 동일합니다.
- Quest/Scene/전투/저장 코드·Voice 설정·Subtitle·기존 Main01~17을 수정하지 않았습니다. 신규 시스템 코드나 Runtime API 호출을 추가하지 않았습니다.
- 원본 Unity/Save/설정 및 TTS 소스·체크포인트 **4169파일 SHA 변경0**. `REMAINING9_HOLD.json`, `REMAINING9_SESSION.lock`, old_error 백업과 모든 수동 복구 기록을 유지했습니다.
- worktree 기존 Assets3998파일 중 허용된 Catalog1개만 변경, 나머지3997파일 불변입니다. 신규 WAV/meta20개만 추가했습니다.
- [보호 검증](Evidence/Main18Serin10Integration20261019/ProtectionVerification.json), [정적 요약](Evidence/Main18Serin10Integration20261019/StaticSummary.json).

## 3. 실제 Unity Runtime 검증

- 설치 Unity **6000.6.5f1** 숨김 Batch에서 실행했습니다. 기존 격리 Client를 재사용했고 원본 Editor·Play·Refresh·OS 포커스·키보드/마우스 조작0입니다.
- 실행 복사본과 worktree의 게임 C#163개 및 Catalog 내용이 일치합니다. 소스 WAV35개의 bytes/SHA와 Unity `AudioClip.GetData` float 전 샘플을 대조해 최대오차0을 확인했습니다. [실행 동일성](Evidence/Main18Serin10Integration20261019/ExecutionParity.json), [PCM35 결과](Evidence/Main18Serin10Integration20261019/PCMResults.tsv).
- Main18의 정본 Get/Dialogue ID/Speaker/ExactText를 수집해 NPC22와 승인10의 StoryOrder를 대조했습니다. [StoryOrder22](Evidence/Main18Serin10Integration20261019/StoryOrder22.tsv).

|검사|결과|
|---|---|
|Main18 NPC22/22|실제 레온/조사 Site 상호작용·DialoguePresenter 페이지·Clip 재생·AudioSource timeSamples 진행 PASS|
|신규 승인10|10/10 실제 재생, 화자 companion_serin·원문·정확한 Clip 연결 PASS|
|기존 Main18 정상12|12/12 실제 재생·자연 종료 PASS|
|기존 Main19 정상13|13/13 실제 Site·조건부 동료 대화·자연 종료 회귀 PASS|
|화자/자막/StoryOrder|원문·표시 화자·22 정본 순서 및 승인10 StoryOrder PASS|
|동료 분기|세린+미엘/폴/태온 편성을 번갈아 실제 활성 동료만 발화, 페이지 중 Party 불변 PASS|
|자연 종료|음성이 끝나도 페이지/Quest가 임의 진행하지 않음 PASS|
|Next/빠른 Next|이전 Clip 즉시 정리, Player 전환 무음, 연속 Next 후 마지막 Clip만 재생 PASS|
|Player/지문|Clip null/재생false, NPC 음성 fallback 없음 PASS|
|Mixer/Mute|Voice 그룹·2D·pitch1·Source volume1, Voice100/40/0, 전체 Mute/Unmute 설정 보존 PASS|
|Scene/전투 경계|Scene 전환/Battle 진입·복귀 뒤 잔여 Source/Clip0 PASS|
|Save/Continue|Main18 레온/ Main19 목격 뒤 실제 Bootstrap 슬롯1 Continue, Scene/좌표/Quest/Party 일치 PASS|
|기존 Catalog 전체|기존255 Resolve+단기 실제 재생, 정상25+신규10 Resolve/PCM 확인, 총290 유효 PASS|
|Main17 세린11|기본8+청각 길 전용3을 모두 수집해 실제 DialoguePresenter 재생/timeSamples/Hide 정리 PASS|
|Missing Script|검사한 Scene0 PASS|
|Compile|최종 실행 컴파일 오류0|

- 본 검사는 목표 인덱스를 설정한 격리 Quest fixture로 각 실제 상호작용을 검증했습니다. Chapter1부터 전체 Story를 다시 플레이한 결과로 표현하지 않습니다. 목격 대화 종료 콜백의 실제 Battle 자동 진입을 확인하고 다음 편성 검사는 Field 재로드 후 수행했습니다.
- 전투 경계 테스트는 실제 Battle Scene/복귀 UI를 사용하되 음성 QA용 종료 호출로 승리 처리했습니다. 전투 밸런스·손조작 재검증을 의미하지 않습니다.
- `AudioListener.volume=0`으로 자동 검증 음성이 바울 대표의 스피커에서 재생되지 않게 했습니다. 실제 DSP clock/PCM/Source 상태를 검사했고 이번10개의 청취 승인은 이슈17의 기존 승인으로 기록합니다.
- 최종 자동 assertion **1370 PASS / 0 FAIL**. 반복 페이지·Catalog 검사를 포함하며 독립 기능 개수는 아닙니다.
- [전체 검사](Evidence/Main18Serin10Integration20261019/checks.txt), [실제 재생35 ID](Evidence/Main18Serin10Integration20261019/played_ids.txt), [최종 요약](Evidence/Main18Serin10Integration20261019/FinalSummary.json).

## 4. 전체 QA PARTIAL의 범위

- 기존 `UnityEditor.Search.SearchDatabase.EnumerateAll` startup 예외가 같은 격리 환경에서 재현됐습니다. 신규 게임 Runtime 오류는0입니다. [Console 원문](Evidence/Main18Serin10Integration20261019/errors.txt).
- 원격 main ProjectVersion6000.5.7f1은 미설치이므로 해당 버전 검증은 하지 못했습니다. 실행 복사본은 현재 설치된6000.6.5f1입니다. 원본/worktree ProjectVersion을 변경하지 않았습니다.
- OS 직접 키보드·게임패드 입력 및 물리 스피커 출력은 이번 백그라운드 검사의 범위 밖입니다. 이 사유로 기능 PASS와 전체 QA_PARTIAL을 구분합니다.
- 초기 검수 도구의 메서드 인자·화자 없는 안내 제외·목격 후 Battle 복귀·Main17 청각 분기 포함 문제는 보완했습니다. 과거 Pilot 로그를 `Temp/Main18Serin10Integration20261019/Evidence`에 보존했고 최종 수정본 전체 Runtime을 다시 통과시켰습니다. 게임 코드를 바꿔 검사를 통과시키지 않았습니다.

## 5. 저장 위치·후속

- 증거/일회성 검수 소스: `문서/00_프로젝트/Evidence/Main18Serin10Integration20261019`. 검수 C#은 Assets 밖 증거로만 저장하여 게임에 사용하지 않는 코드를 추가하지 않았습니다.
- 대용량 원본 해시 snapshot·전체 Unity/Pilot 로그는 `Temp/Main18Serin10Integration20261019/Evidence`에 보존합니다. API 키나 요청 비밀을 수집·출력·커밋하지 않았습니다.
- 이번 브랜치에는 기능+문서 로컬 commit만 만들었습니다. **Git Push=NO / main 병합=NO / 원본 Unity 적용=NO.**
- 다음 권장 작업은 바울 대표의 통합 결과 검토 후 별도 승인에 따른 원격 Push/최종 main 반영입니다. 승인 음성10개의 재청취나 재생성 요청은 필요하지 않습니다.
