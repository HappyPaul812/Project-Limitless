# Main04 후반 TTS 5개 최종 반영 및 Runtime QA (2026-10-07)

LOCAL 정본과 사용자 제공 결과를 대조해 5개만 반영했다. 신규 생성·재인코딩0, Source 수정0. 결과 폴더: `F:/study/codex/Project-Limitless/Limitless_TTS_Regen_Main04_LateSequence_5`. `_PASS_CHECKPOINT.json`의 exact_text/file SHA/PCM SHA와 REGEN_REPORT·기존 Handoff·현재 Flow·Catalog GUID/Speaker를 대조한 뒤 기존 Unity WAV만 byte복사했다. 미엘3/태온2, 24kHz mono PCM16, 내부 및 기존 전체 Voice와 Duplicate PCM0.

## Mapping

Source는 위 결과 폴더의 `<Stable ID>.wav`, Unity Asset은 `Unity/Client/Assets/_Project/Audio/Voice/Story/Main04/<Stable ID>.wav`. Runtime reference는 `StoryVoiceCatalog.Find(Stable ID, Speaker ID) → DialoguePresenter → VoicePlaybackSource.AudioSource.clip`. meta/GUID/Catalog 변경0.

| Stable ID | Speaker | AfterBattle 페이지 | Dialogue | 길이 |
|---|---|---:|---|---:|
| main04_miel_supp_009 | 미엘 / companion_miel | 3 | 저도 볼 겁니다. / 이번에는 순서대로요. | 3.84s |
| main04_taeon_supp_006 | 태온 / companion_taeon | 4 | 제가 본 움직임과 / 이곳에서 있었던 일까지 합치면… | 5.44s |
| main04_miel_supp_010 | 미엘 / companion_miel | 6 | 초원보다 더 안쪽에서 / 무언가가 벌어지고 있는 것 같아요. | 5.88s |
| main04_taeon_supp_007 | 태온 / companion_taeon | 7 | 여기서 더 들어가는 건 / 지금은 위험할 것 같습니다. | 4.76s |
| main04_miel_supp_011 | 미엘 / companion_miel | 8 | 마을에도 이 상황을 알려야 해요. | 3.0s |

파일별 경로·GUID·교체전/후 SHA·PCM SHA·Runtime reference: [Source Mapping CSV](Main04_Late5_Source_Mapping.csv), [자동 검증 JSON](Main04_Late5_Verification.json).

## 검증 결과와 범위

- 정본/화자/Stable ID/Source content checkpoint 일치. 사용자 전체 원본 청취PASS를 근거로 사용하고 Runtime PCM을 그 원본과 비교했다. 5개 모든 sample **오차0**, 잘못된 화자 Resolve 차단 PASS. 자동검사만으로 의미 PASS를 추정하지 않았다.
- 기존 비포커스 격리 Launch로 Play Mode 실행. 전투후 대화 직전 진행도를 격리 설정해 실제 Field_01 Actor.TryInteract로 진입. 기존 전투·Main04 전반은 재실행하지 않았다.
- 실제 전투후9페이지/7Voice 자연종료·clip reference·playback invocation/sample 진행·duration·순서·Next·speaker/text·portrait·줄바꿈/본문높이·Footer 비중첩·cleanup PASS. 새5개 미엘→태온→미엘→태온→미엘 순서와 중간 Player 페이지 유지.
- **148개 기술검사 PASS / FAIL0**. 동일프레임 Next2는 기존 수동skip 정책대로 정확히 두 페이지 진행 후 해당 Clip 재생; 단일Source·중첩0. 입력합치기 정책은 추가하지 않았다.
- 대화완료→Main04 Completed→Main05 다음Objective, 완료 전후 실제 Bootstrap ContinueGame→Field복귀/진행도복원 PASS. Player Voice/Portrait0, 지문 Voice/Portrait0.
- Unity6000.5.7f1 컴파일완료/Error0, 최종Console Error0/Warning0, Field Runtime Missing Script0. GameView 활성화·OS/키보드/마우스 포커스전환0. AudioListener 출력만 임시0으로 처리하고 Source 재생 유지; 종료 시 기존값 복원.
- 종료 clean Bootstrap EditMode/is_focused=false, 격리Save·Settings 해제. 사용자 저장/Settings 보호.

[Runtime 로그](Main04_Late5_Runtime_Results.txt). 도구: Main04Late5VoiceAudit.cs 및 Tools/TTS의 apply_main04_late5.py/verify_main04_late5.py. Temp에 baseline/sample 덤프 보관. 전체프로젝트 대형QA 미실행.

## 보호 및 최신 상태

기존2361파일 중 WAV5만 변경·**2356파일 byte보호**. Main03/정상Main04 Voice/사용자 PNG/Sprite/meta/GUID/Catalog/Registry/게임C#/Scene/ProjectSettings/Packages/원본TTS/Save 변경0. 새 Editor QA/helper meta만 추가.

5개 각각 **REGENERATED / USER_LISTENED_PASS / UNITY_APPLIED / RUNTIME_VERIFIED**. Runtime human listening PASS를 새로 주장하지 않으며 이번5개 USER_LISTENING_REQUIRED 없음. 재생성목록 해결5개 제거, Matrix 해당5행만 갱신. **NEEDS_LISTENING244 역사 marker 유지**, 다른행/Queue219/과거 Listening JSON 변경0·일괄PASS승격0. Main17 TTS_PENDING14/Manifest 변경0. 이전 Wrong/Suspect 및 Handoff는 역사자료.

다음: Queue219 사용자청취 또는 별도 후속요청. 이번5개 추가수동확인 필수항목0, 기존 실물입력/Standalone 검증은 별도 유지.

## Git

음성5개 commit `0e7809a`. QA·문서 별도commit(최신local log). 직접변경 파일만 stage, GitHub Push0. 작업범위 staged git diff --check PASS. 전체 Working Tree에는 기존 unrelated 사용자 파일의 trailing whitespace가 있어 전체PASS로 주장하지 않으며 그 파일들은 수정하지 않았다. 기존 사용자 변경과 untracked TTS/Asset 유지.
