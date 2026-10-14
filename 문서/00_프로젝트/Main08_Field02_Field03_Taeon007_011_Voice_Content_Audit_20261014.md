# LIMITLESS — Main08 Field_02~Field_03 태온 007~011 자막/음성 어긋남 재제보 (2026-10-14)

**상태: USER_RUNTIME_AUDIO_MISMATCH_REPORTED / ROOT_CAUSE_NOT_YET_PROVEN / READ_ONLY_SOURCE_AUDIT_REQUIRED / CATALOG_AND_MAIN_UNCHANGED.**
**담당:** 기획 (docs/planning) / 후속 PCM·Unity 검증: Codex. 바울 대표가 직접 Unity Play 장면 5개 캡처와 각 구간 청취 결과를 제공함. 이 문서는 사용자 실측 제보와 GitHub **현행 main**의 대사 코드를 교차 기록한 것이며, 로컬 WAV를 기획 세션이 직접 청취했다고 주장하지 않는다.

## 1. 사용자 화면과 고정된 정식 대사·ID

GitHub `main`의 `Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs` `Lines()` 확인. 다음 **화자·정식 텍스트·Stable DialogueId 및 Quest/Scene 구조는 수정 대상이 아니다.**

| 순서 | 위치·Step/정식 화면 발화 | ID | 사용자 청취 관찰 |
| --- | --- | --- | --- |
| 1 | **Field_02**, `WheelTracks`, 태온 “폭이 같습니다. 아마 맞을 겁니다.” | `main08_taeon_supp_007` | 해당 화면에서 해당 태온 음성이 안 들림 |
| 2 | **Field_03**, `Investigation`, 태온 “누군가 이곳을 조사한 흔적입니다.” | `main08_taeon_supp_008` | 1의 “폭이…” 대사 재생 |
| 3 | **Field_03**, `DeepZone` 후반 페이지, 태온 “너무 조용합니다.” | `main08_taeon_supp_009` | 2의 “누군가…” 대사 재생 |
| 4 | **Field_03**, `DeepZone` 후반 페이지, 태온 “그 이유가 무엇인지는 아직 모르겠습니다.” | `main08_taeon_supp_010` | 앞 태온 화면에서는 음성 없이 넘어가고 여기에서 앞 대사가 붙어서 나오는 현상 보고. **혼합 내용/정확 분리점은 PCM 검사 전 미확정** |
| 5 | **Field_03**, `DeepZone` 후반 페이지, 태온 “마법 같습니다.” | `main08_taeon_supp_011` | 문장 내용은 정상이지만 시작 전 여백 과다 |

중간 **미엘 “폴 씨일까요?” / “조용하네요.” / “몬스터들이 여기를 피하는 이유가 있는 거겠죠?” / “…저쪽에서 빛이 났어요.”, 그리고 플레이어의 무음 페이지**는 비수정 경계로 보호한다. 사용자 제보에서 미엘 대사 정상임을 명시. `WheelTracks` 이전 미엘004도 원문 보호. `DeepZone`은 `MainQuest08Location`의 5페이지+3페이지 분할 표시 및 파란빛 연출이 있으며 **그 동작 유지**.

## 2. 유력 원인과 금지할 오판

- 현행 `DialoguePresenter.RefreshSequenceText`는 새 페이지마다 기존 Clip을 `VoicePlaybackSource.Play`로 전환하고, `VoicePlaybackSource.Play`는 `Stop` 후 해당 Catalog AudioClip을 `AudioSource.Play()`함. 따라서 눈에 보이는 **한 대사씩 밀림이 AudioSource가 대사를 끝까지 재생하는 기능 탓이라고 단정할 수 없음**. 카탈로그 GUID가 가리키는 **WAV 내부 실제 발화/앞 여백·여러 대사 합본·잘못된 ID 연결**을 우선 확인하고, 필요시 런타임 이벤트 중복/Play 호출 타이밍을 계측.
- 앞서 PR #11로 `main08_taeon_supp_005/006`만 사용자 청취 승인·복구하여 **2026-10-14 main `314f7fbbd66799ff93ec71404bcf388023c655be`**에 병합됨. 이번 007~011 오류를 이 PR이 해결했다고 주장하지 않음; **005/006 승인 clip과 Catalog GUID를 절대 변경하지 않음**.
- 스크린샷에 WAV 원음의 정확한 편집구간/파일간 발화분포는 나오지 않는다. “007 무음 / 008=007 / 009=008 / 010 혼합 / 011 선행무음”은 **사용자 귀로 관찰한 UI 현상**이지 GUID가 틀렸는지 원본 합본인지 분석 완료된 사실은 아님. 편집 전 **각 ID별 실 PCM 및 Catalog GUID·meta·실제 AudioSource 재생 Clip을 일치 비교**할 것.

## 3. Codex 1단계 — 원본 보호·읽기전용 감사

1. 최신 origin/main, `AGENTS.md`, `CURRENT_STATUS.md` 및 Main08 최근 PCM·QA 정본, 사용자 원본 Save/Unity 6.6 Editor 상태를 읽기 전용으로 확인. 별도 worktree/백그라운드. 이전 승인 음성 8개(006 포함)·Catalog·main PR#11 및 Arbel 적용 보류 보호.
2. `main08_taeon_supp_007~011` 각각의 **정본 텍스트↔Stable ID↔StoryVoiceCatalog.asset의 실제 GUID/fileID↔연결 WAV 경로/meta SHA/시간↔실제 발화 PCM 및 구간별 ASR**를 **한 표**로 정리. 앞뒤 미엘004/005/006/007/008·태온005/006도 보호용 대조. 기존 TTS 원본 전체와 의도한 문장이 다른 번호 음원에 섞여 있는지 원본 추적.
3. clip별 `leading_silence`, 첫 본문 발화 onset, 문장 경계·호흡/여운, AudioSource.Play 직후 timeSamples, Stop/Next 입력·직전 Clip 재사용 여부를 확인. 011의 **과다 공백이 WAV 내부인지 재생 지연인지** 명확히 구분. 필요한 곳만 스펙트럼/파형+로컬 ASR 보조 사용; 자동전사를 사람 청취 승인으로 주장하지 말 것.
4. `Field02 WheelTracks → Field03 Investigation → DeepZone(5+3 페이지)`를 사용자 관찰과 같은 흐름으로 격리 재생. 원본 Editor Play/포커스 조작 금지, 대화 중 일반 몬스터 조우 suppression은 **기존 OPEN을 가렸다는 사실을 보고**.
5. 후보 음성 **새 제작 없이 기존 WAV에서 정확 분리·재연결·선행 무음 최소 Trim 가능한지 우선 평가**. 기존 음원의 source folder/PCM/GUID/meta 변경 금지. 원문 일부가 없다면 필요한 ID만 TTS 담당과 협의하고 사용자 승인 전 API 호출 금지.

## 4. Codex 2단계 — 후보 파일과 인간 청취 승인 (정식 게임 연결 전)

- 원인 분석에서 007~011의 원문 각 1문장에 대한 재사용 가능한 고품질 PCM이 발견되면 **후보 WAV를 별도 임시 폴더**에 순서대로 제작. 원본 발화 PCM suffix·내부 숨/여운을 보존, 클릭/말잘림/샘플 중복/음량 변조/무리한 결합 금지. 011은 **첫 '마' 음절과 자연스러운 호흡이 잘리지 않도록 선행 여백만 다루고** 기존 대사 내용 유지.
- `007` `008` `009` `010` `011`에 대해 **ID·원문·후보 출처 WAV/원본 offset·길이·ASR·시그널·클리핑·전/후 PCM 및 QA** 표와 사용자가 듣기 쉬운 5파일/간단 재생 페이지를 제공한다. **한 개라도 본문 누락/발화 경계 불명확이면 억지로 틀린 음원을 옮기지 말고 원본 자료 추가검토를 요청**.
- 현 단계에서 **원본 Catalog/GUID 변경·기존 WAV overwrite·Unity 실사용 프로젝트 적용/Commit·Push/main 병합 금지**. 바울 대표 **개별 청취 승인**을 받고 난 뒤 승인받은 ID만 신규 GUID/WAV 폴더 Catalog 연결 → 격리 Play/Save/Continue·페이지 교차/전후 미엘 및 Player 무음 보호 → Codex 전용 브랜치 Push → 사용자 동의 PR/main 병합 순.
- 원본 Editor/Play/OS 포커스·Saved game·Arbel v3 지붕/Field04/미술 승인·TTS/BGM 및 이전 보호 Sprite 손대지 않는다.

## 5. 현재 판정 및 다음 책임

- `MAIN08_TAEON_007_011_CONTENT_BOUNDARY_AUDIT_OPEN`
- 사용자 제보 수집 및 **스크립트의 정확한 Stable ID 확인 완료**, WAV PCM 직접검사·음성 원본 원인검증·후보·실제 Runtime 검증·사용자 청취 승인 **아직 안 됨**.
- **다음 담당: Codex**, 1단계 읽기전용 감사 및 2단계 기존 음원 기반 후보 생성. 결과를 받은 다음 기획 담당이 청취 승인 여부와 후속 구현 범위를 결정. 리뉴얼 아트 담당은 이 음성 문제와 무관.
