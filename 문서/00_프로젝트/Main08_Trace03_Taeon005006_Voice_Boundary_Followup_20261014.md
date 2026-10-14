# LIMITLESS — Main08 Field02 Trace03 태온 005~006 음성 불일치 사용자 재제보

날짜: 2026-10-14 · 담당: 기획 · 문서 브랜치 `docs/planning`

**상태: USER_RUNTIME_REPORTED / PREVIOUSLY_KNOWN_OPEN_CONFIRMED_IN_DOCS / AUDIO_FIX_NOT_YET_APPLIED / HUMAN_LISTENING_PENDING.**

## 1. 사용자 제보와 정확한 구간

바울 대표가 Unity Field_02 Play 화면 캡처 2장을 전달하면서 다음처럼 지시했다.

> “'초원에서는.....' 저 장면부터 '이 흔적..' 저 장면 전까지 다른 대사..”

이것은 **화면 자막과 음성에서 다른 문장이 재생된다는 사용자 청취 제보**다. 두 캡처는 오류 구간 양쪽 경계이며 둘째 화면 자체의 오류를 의미하지 않는다.

정본 `Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs`의 `GetLines` 확인:

| 범위 | 페이지 | 정확한 화자/정본 대사 | Stable Voice ID |
|---|---|---|---|
| **수정 대상 (포함)** | Field_02 Trace03 태온1 | 태온: **초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다.** | `main08_taeon_supp_005` |
| **수정 대상 (포함)** | Field_02 Trace03 태온2 | 태온: **여기서는 그 방향이 조금 더 분명합니다.** | `main08_taeon_supp_006` |
| 경계 검증 (보호) | Trace03 다음 플레이어 | 플레이어: 그러면 우리가 찾던 방향은 맞는 것 같습니다. | 무음 (Voice ID 없음) |
| **제외 경계** | `WheelTracks` 첫 페이지 | 미엘: **이 흔적… 폴 씨 것 아닐까요?** | `main08_miel_supp_004` |

- Trace03 전날의 `main08_miel_supp_003` 등 앞쪽 승인 6개와 `WheelTracks` 이후 전부 무단 수정 금지.
- 대사 스크립트/화자/Stable ID는 올바르므로 **기획 대사 변경 대상 아님**.

## 2. 기존 원인 근거와 이번 위험

2026-10-13 [Main08 Field02 기술 QA](Main08_Field02_Voice_Technical_QA_20261013.md) / [승인 6개 정식 연결 QA](Main08_ApprovedVoice_Integration_QA_20261013.md)에서 다음을 이미 OPEN으로 명시했다.

- 기존 `main08_taeon_supp_005.wav`에 대한 로컬 ASR: “그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다.” — **005 정본 내용이 아님**.
- 기존 `main08_taeon_supp_006.wav`에 대한 로컬 ASR: “초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다. 여기서는 그 방향이 조금 더 분명합니다.” — **005/006이 하나의 WAV에 혼재**.
- 이전 사용자 청취 승인·Unity/원본 반영·GitHub main에 병합된 집합은 태온 002/003/004와 미엘001/002/003 **6개만**. 태온005/006은 해당 승인 대상이 아니었고 당시 OPEN이었다. 이는 새로운 임의 버그 추정이 아닌 **기존 QA에서 특정한 남은 2건의 재현 사용자 보고**다.
- 다만 이번 실행 중 원본 Editor가 재생한 AudioClip GUID/PCM의 현재 SHA를 직접 읽은 것은 아니므로, 실제 Runtime clip/패키지 경로를 재대조하여 **현재 로컬 상태가 기존 감사와 같은지 검증**할 필요가 있다.

## 3. 후속 Codex 지시

1. 최신 `main` 기준, `StoryVoiceCatalog.asset`, `MainQuest08FieldFlow.cs`, WAV/meta/GUID와 현재 Runtime 클립을 원본 **읽기전용으로** 확인. 원본 Unity Play/포커스, 사용자 dirty/Save/TTS/BGM/기존 승인6/Field04/Arbel 아트 모두 보호.
2. 기존 006 WAV의 **정확한 PCM**, 발화 타임라인·무음 구간·호흡·클리핑·샘플 zero-crossing을 측정하여 005 대사와 006 대사를 **원본 손실 없는 분리 후보 2개**로 만들 수 있는지 판단. 기존 005(잘못된 타 대사) 또는 006(합본)을 덮어쓰지 말고, 별도 후보 폴더에 `main08_taeon_supp_005.wav`·`main08_taeon_supp_006.wav`를 제작.
3. 분리 후보가 부자연스럽거나 원문 일부가 없으면 **TTS 담당의 추가 제작이 필요한 ID만** 보고. 타 ID 재연결/복사를 강행하지 말고 무료·유료 TTS/API 임의 호출 금지.
4. Codex가 기존 원본 006의 첫/둘째 문장을 실제 WAV PCM/ASR/소형 청취 페이지로 검증해 사용자에게 보여주고 **두 후보 개별 청취 승인을 요청**. 자동 전사만으로 인간 청취 합격·연결 승인 주장 금지.
5. 승인 후에만 원래 WAV/meta와 출처를 보호한 별도 ApprovedRecovery 폴더+고유 GUID를 만들고 Catalog 005/006 두 ID 참조만 변경. 이미 통합한 승인6개와 다른 정상 TTS는 불변. 기존 소스 WAV를 rename/overwrite/delete하지 않는다.
6. 별도 격리 Unity 환경에서 Trace03 전후→WheelTracks 화자/Subtitle/GUID/PCM/재생 종료/Next·취소/Quest/Save/Continue/실제 전투 조우 억제 여부를 단계별 검증하고 현재 기존 Monster Encounter 대화 중 재조우 이슈는 **별도 OPEN**으로 기록. 원본 Editor를 강제 Play/Refresh/포커스 전환하지 않는다.
7. 이번 **조사·후보 제작은 Unity 원본 연결/Push/main병합 승인 아님**. 구현 승인·사용자 청취 승인 경계 유지. Codex가 브랜치/후보 경로/SHA/QA·원본 보호 상태를 보고한 뒤 기획이 수정 적용 여부를 별도로 정리.

## 4. 담당 구분 및 우선순위

- **Codex**: 원본 006에서 무손실 분리 후보 만들기, 두 Stable ID/PCM/Catalog/Runtime 재검증, 청취 자료 납품.
- **TTS**: Codex가 원음 분리 불가로 확인한 경우에만 신규 제작 검토. 태온 기존 목소리/원문 유지, 승인 전 API 사용 금지.
- **기획**: 사용자 직접 청취·승인 기록, 해당 두 ID에 한정한 이후 Unity 연결 QA 지시.
- **바울 대표**: 후보 005/006 음성을 청취해 자막과 일치·캐릭터 음색·자연스러운 구분 승인.

**결론:** `main08_taeon_supp_005`와 `main08_taeon_supp_006` 기존 미해결 음성 경계 2건을 새로운 재제보로 접수. 경계 `main08_miel_supp_004`는 보호. **신규 TTS 0 / 실제 Unity 음성 파일 변경 0 / GitHub main 변경 0.**


## 2026-10-14 Codex 후속 — 태온005·006 무손실 분리 후보 2개 제작 완료

바울 대표가 Codex의 로컬 후속 보고 이미지를 제공함. **직접 청취 승인/정식 Unity 연결은 아직 미완**이다.

- 후보 `main08_taeon_supp_005`: **6.729초**, 정확 원문 “초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다.”
- 후보 `main08_taeon_supp_006`: **3.801초**, 정확 원문 “여기서는 그 방향이 조금 더 분명합니다.”
- Codex QA 보고: 기존 `main08_taeon_supp_006.wav`의 두 문장 합본 검출, 두 새 후보 자동 전사 **2/2 원문 일치**, 클리핑0, 삭제·중복 샘플0, 005+006 후보 **PCM 이어붙임=원본 PCM 정확 일치**, 기존 음색/호흡/여백 그대로. 자동 전사는 청취 품질 합격을 대신하지 않는다.
- **보호**: 기존 승인6개, 원본 WAV/GUID/meta/Catalog, 메인08 대사/퀘스트/Scene/Save 및 원본 Unity 변경0, 신규 TTS/API0, Game 연결0, 원격 Push0.
- **다음**: 바울 대표가 Codex의 `태온 005`과 `태온 006` 후보를 **각각 실제 청취**하여 두 원문의 발음·말끝/시작·편집 경계 자연스러움을 평가. 둘 다 승인 시에만 Codex가 원본 WAV/GUID/meta를 유지한 별도 승인 WAV 폴더 등록 + Catalog 해당2행 변경, 기존 승인6개 및 앞뒤 미엘004·Player무음/Save/Next 경계 보호 회귀를 수행. 청취에서 문제가 있으면 해당 Stable ID만 재분리·TTS 담당 후속 분류.
- **현재 상태**: `SPLIT_CANDIDATES_2_READY / TECH_QA_PASS / USER_LISTENING_PENDING / AUDIO_NOT_APPLIED`.


## 2026-10-14 사용자 직접 청취 — 2개 문장·음색 승인, 005 선행 무음 소폭 보정

바울 대표가 개별 분리 후보를 실제 청취한 뒤 **“둘 다 정상인데. '초원에서는...' 앞이 좀 공백이 있네?”**라고 확인했다.

- **`main08_taeon_supp_006`**: 발화·음색·문장 경계 **최종 사용자 청취 승인**, WAV PCM **그대로 사용**.
- **`main08_taeon_supp_005`**: 발화·음색 **사용자 승인**, 단 “초원에서는” **직전의 다소 긴 공백을 자연스럽게 축소**한 기술 마감 필요. 더빙 재제작/새 TTS·대사 변경 요청은 아님.
- Codex는 먼저 해당 공백의 원인이 분리 WAV 선행 무음인지 `DialoguePresenter`의 음성 시작 지연/이전 페이지 전환인지 실제 PCM·재생 타임라인에서 구분. 음원 내 무음이면 발화 시작·호흡을 충분히 보호한 짧은 pre-roll(예: 80~150ms 정도, 실제 파형으로 결정)만 남겨 **선행 무음만 제거**. exact trim ms 임의 지정 금지. 혹시 PCM 음성 앞에서 자르거나 자연스러운 들숨이 잘리면 실패/재청취 대상으로 보고.
- **기존 “분리 전 두 파일 PCM 연결=원본” 증명은 보관하되** 005 시작 무음만 제거한 **최종 적용 후보가 원본 PCM과 재결합해서 byte-identical 하다고 주장하지 않는다**. 005의 발화 구간은 원본 PCM을 그대로 유지, 006 PCM 전체는 기존 승인 후보 그대로 보존. 클릭·첫 음절 잘림·호흡 중단·급격한 볼륨 변환 금지.
- 최종 후보 005를 짧게 다시 들어볼 수 있도록 제공하고, 기술적으로 무음만 줄인 것이 입증되며 문장 내용·음색이 변하지 않을 경우 **이번 사용자 청취 승인에 따라** 정확한 005/006 두 Stable Voice ID만 Codex 격리 worktree에서 승인 음성 폴더 신규 GUID/meta로 등록하여 Catalog 연결·Runtime QA를 진행해도 된다. 품질 불확실·구간 일부 삭제 시 공식 적용 보류하고 재청취 요청.
- 기존 WAV/GUID/meta·앞선 승인6개·Player 무음·WheelTracks 미엘004, Scene·Quest·Save/TTS/BGM 및 원본 Editor 포커스 보호. 전체 회귀/청취 검증, Safe branch Commit·Push까지만 허용; **main 직접 Push·병합 금지**.
- 현 상태: `TWO_LINES_HUMAN_APPROVED / 005_LEADING_SILENCE_MINOR_FIX / 006_FINAL_APPROVED / CATALOG_NOT_YET_UPDATED`.


## 2026-10-14 최종 005/006 연결·feature branch Push 완료 — main 병합은 대기

Codex 최종 보고와 GitHub 원격 브랜치를 기획 담당이 교차 확인했다.

- 원격 feature branch: `codex/main08-taeon005006-final-20261014`, HEAD **`721602256ac4e8c4c7081e6b6b3f04122b266edd`** (원격 확인). 기능 Commit `fe588f9af1369ec825a9b9820ebfb0a92dc9499a`, 이후 QA/문서 Commit `7216022...`. **원격 main `bddca540d2fdd9e93f642f8d0f387b34730bd2d8` 불변.**
- 검증 QA 원문: feature branch의 `문서/00_프로젝트/Main08_Taeon005006_Final_Integration_QA_20261014.md` 및 `Evidence/Main08_TaeonFinal_20261014/QA_Summary.json`, `pcm_analysis.json`.
- `005`: 후보 선행 **4,460 프레임(24,000Hz 기준 185.833ms)**만 잘라 최종 **6.543625s**, 첫 새 샘플0, 남은 PCM은 원 승인 후보 suffix 그대로. 클리핑·내부 편집·resample·신규TTS0. `006`: 승인 WAV 원본 바이트 불변 **3.800542s**.
- 실제 Field02 격리 Unity: 대상 기능 **224 PASS/0 FAIL**, 두 GUID·자막·화자·이미 승인6과 Player 무음·미엘004 페이지 경계 및 Save/Continue 검사 PASS. 원본 프로젝트 **새WAV2 + 새 meta3, Catalog ID005/006 두 Clip 참조**에 한정한 디스크 적용 완료; 기존 WAV/meta 삭제·덮어쓰기0. 원본 편집기 Refresh/Play/포커스 조작0, 보호된 5,221파일 SHA 동일.
- **기존 이슈:** UnityEditor.Search 예외, 실제 물리 스피커·정본 Unity 버전·사용자 직접 최종 005 청취 미확인, 대화 중 일반 MonsterEncounter 정상 통과 아님(테스트에서 suppress). 전체 QA 상태는 `PARTIAL`, 대상 224 PASS를 전체 엔진 완전 PASS로 확대 해석 금지.
- **추가 확인할 구간:** PCM QA에서 005의 약한 신호 시작은 Trim 후 **~0.14s**지만 강한 신호 시작은 **~2.33s**. 2.33s를 첫 음절 시점으로 단정할 수 없고, 보존한 자연 호흡/중간 여백이 포함될 수 있다. **사용자가 처음 문제 삼은 체감 공백이 여전히 길게 느껴지는지 최종 005 음원을 직접 재청취해 확인 권장.** 승인 전에 추가 trim을 자동 지시하지 않는다.
- **다음 결정:** 바울 대표가 최종 `main08_taeon_supp_005`를 재청취해 시작 자연스러움 확인→ 승인 시에만 feature branch의 PR·main 정상 병합을 별도 요청/진행. 만일 길면 **내부 호흡/여백의 어떤 구간이 문제인지** 청취 근거 우선 조사. 이미 승인한 006과 나머지6은 손대지 않는다.

**현재 상태:** `FEATURE_PUSH_DONE / ORIGINAL_DISK_LINKED / TARGETED_QA_224_PASS / OVERALL_PARTIAL / FINAL_005_LISTENING_RECOMMENDED / MAIN_UNMERGED`.
