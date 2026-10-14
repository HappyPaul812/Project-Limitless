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
