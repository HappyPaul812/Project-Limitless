# Main08 Field02 흔적 조사 음성 문제 — 사용자 런타임 제보 (2026-10-13)

**상태: USER_RUNTIME_REPORTED / TECH_ROOT_CAUSE_NOT_VERIFIED / CORRECTION_PENDING.** 바울 대표가 Unity 6.6(6000.6.5f1) 원본 프로젝트에서 Field_02 플레이 도중 캡처 3장으로 제보. 이 문서는 사실관계 및 부서 인계를 기록할 뿐, WAV/게임 자산 수정·재생성·Unity 적용이 아니다.

## 범위 / 사용자 실제 청취

- **첫 장면** 태온 “여기부터 흔적이 거의 없습니다.” — **음성이 들리지 않음**.
- **둘째 장면부터 셋째 장면 직전** 미엘 “몬스터가 적다는 건 좋은 일 아닌가요?”부터 미엘 “다들 이쪽을 피해서 지나가고 있어요.” **전까지** — **자막과 다른 대사가 들림** (어느 페이지가 어떤 문장을 말했는지 아직 미확인). 셋째 장면 자체를 오발화로 확정하지 말 것.
- 순차 진행은 Main08 Trace01 → Trace02 → Trace03. 전체 Main08을 동일 결함으로 단정하지 않는다.

## GitHub 정본 대조 (main 확인)

`문서/03_스토리/LOCAL_Story_Dialogue_원문_부록.md`의 Main08 섹션과 `Unity/Client/Assets/_Project/Scripts/World/MainQuest08FieldFlow.cs`의 GetLines(Trace01/02/03)가 아래 본문·화자·안정 ID로 일치함.

| Trace | Stable Dialogue ID | 정본 화자·자막 | 사용자 결과 |
| --- | --- | --- | --- |
| Trace01 | `main08_taeon_supp_002` | 태온 / 여기부터 흔적이 거의 없습니다. | 무음 체감, 원인 미확정 |
| Trace01 | `main08_miel_supp_001` | 미엘 / 몬스터가 적다는 건 좋은 일 아닌가요? | 다른 대사 재생 제보 |
| Trace02 | `main08_taeon_supp_003` | 태온 / 평소라면 그렇겠죠. | 오발화 의심 범위 |
| Trace02 | `main08_taeon_supp_004` | 태온 / 그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다. | 오발화 의심 범위 |
| Trace03 | `main08_miel_supp_002` | 미엘 / 없어진 게 아니네요. | 오발화 의심 범위 |
| Trace03 | `main08_miel_supp_003` | 미엘 / 다들 이쪽을 피해서 지나가고 있어요. | 경계 페이지(보호 대상으로 포함하되 오류 단정 금지) |

앞 페이지 `main08_taeon_supp_001`(“잠깐만요.”)과 다음 페이지 `main08_taeon_supp_005` 등도 경계 회귀 대상으로 보호한다.

## 기존 기술 감사와 미해결 차이

`문서/00_프로젝트/Missing_Story_Voice_Supplement_Audit.json`에는 위 여섯 ID 모두 `Unity/Client/Assets/_Project/Audio/Voice/Story/Main08/<ID>.wav` 경로·GUID·SHA가 등록돼 있으며 이전 `Missing_Story_Voice_Supplement_QA.md`에는 Main08 19개 보충 연결/매핑 기술 PASS가 기록돼 있음. **기술 매핑 PASS는 음성 발화 의미·실제 사용자의 청취 PASS와 다름.**

`StoryVoice_SemanticRisk_Audit.csv` 이전 측정상 태온 `main08_taeon_supp_002.wav`는 약 1.03초, RMS 약 -48.98 dBFS, peak 약 -30.57 dBFS로 매우 낮음. 작은 음량·내용 누락·재생 억제 중 어느 원인인지 현재 확정하지 말 것. 이전 감사 수치와 현재 원본 로컬 WAV/AudioMixer/Unity Runtime 상태를 재비교해야 함.

## Codex 작업 인계 (새 구현 승인 아님)

1. 바울 대표 **원본 프로젝트**의 정본 경로·미커밋 사용자 변경·Scene·Save·음성 원본 해시를 기록/백업한다. 조회·검증은 별도 worktree/격리 Save, 백그라운드·비포커스 우선.
2. Trace01~03의 정확한 **현재 자막 텍스트 → Stable ID → 실제 Catalog/GUID → 실제 Runtime AudioClip → WAV PCM 및 의미**를 순서대로 비교한다. WAV 이름이나 연결 정상만으로 발화 의미 PASS 하지 않는다.
3. 첫 무음의 Source IsPlaying, Play 실패/중지, AudioMixer, AudioClip 길이, 샘플 레벨, 실제 발화·음량을 분리한다. 무작정 볼륨 일괄상승/Normalize 금지.
4. 둘째~셋째 전 단계 오발화는 행 이동/잘못된 파일 내용/다른 퀘스트의 Clip/페이지 전환 문제를 각각 분리한다. 실제 녹음이 정본과 다르면 원래 파일을 보존하고 `TTS_REGEN_REQUIRED` 목록/정확한 대사 Handoff만 작성한다.
5. 이미 존재하는 정상 소스 WAV로 바로잡을 수 있으면 원본 보존·사용자 승인 하에 해당 ID만 최소 연결/파일 교체. **유료 TTS/API 호출·무단 재생성·기존 정상 WAV 변경 금지.** 새 제작이 필요한 경우 TTS 담당에게만 인계하고 사용자 청취 승인 전 Unity 적용 금지.
6. 이전/다음 정상 페이지, Next 입력, 화자·Portrait·자막·퀘스트 단계, Voice Mixer/볼륨/빨리 넘기기/Save·Continue/무음 Player를 회귀. 새 WAV가 없으면 기술 QA는 PARTIAL 및 청취 미완 표기.
7. 결과는 대상 ID별 `TEXT_MATCH / CATALOG_MATCH / PCM_MATCH / HUMAN_LISTENING / ROOT_CAUSE / ACTION`을 분리하고 브랜치·커밋·원본 보호 전후 SHA·Push 없음까지 보고한다. `main` 자동 통합/직접 Push 금지.

## 보호 범위

Main08 대사 원문/화자·Dialogue ID·퀘스트 순서·기존 정상 음성 전부 유지. Main03~07 및 09~21 TTS, 별도 Main18 세린 오류10건, 기존 사용자 Save·Scene/아트/음악/프로젝트 설정, Git 브랜치·미커밋 변경 보호.

기획 담당 판정: **신규 문제 제보를 확정된 재생성 건수로 확대하지 않음. Codex가 현장 파일·의미를 구분 감사한 뒤 TTS 담당 제작 필요 여부 결정.**


## 2026-10-13 Codex 후속 로컬 ASR·의미 대조 결과 — 사용자 보고 접수

**상태 갱신: CONTENT_SHIFT_EVIDENCE_REPORTED / ASR_CANDIDATE_ONLY / USER_LISTENING_PENDING / AUDIO_REPAIR_NOT_APPLIED / QA_PARTIAL.**
사용자가 Codex 로컬 보고를 전달했다. 조사 브랜치 `codex/main08-field02-voice-20261013`, 로컬 커밋 `9024edafc3b6de640e6d7c6e1ca4393ff5d35f7a` **(사용자 보고의 로컬 커밋; GitHub 원격 존재/Push 검증 아님)**.

| Stable ID | Codex 로컬 ASR 보고상 WAV 내용 | 정본과 비교 / 다음 분류 |
| --- | --- | --- |
| `main08_taeon_supp_002` | 문자 검출 없음, 약 1.03초, RMS -48.98dBFS | 자막 무음 체감과 합치. `taeon_supp_003`에서 올바른 문장 후보를 청취·PCM 검토 |
| `main08_miel_supp_001` | “저도 같이 가겠습니다. 제가 할 수 있는 일이 있을 거예요.” | 다른 문장. `miel_supp_002`에서 정본 문장 후보 검토 |
| `main08_taeon_supp_003` | “여기부터 흔적이 거의 없습니다.” | `taeon_supp_002`의 후보 복구 원본 |
| `main08_taeon_supp_004` | “평소라면 그렇겠죠.” | `taeon_supp_003`의 후보 복구 원본; 본 ID의 긴 대사는 별도 정확원본 검색 우선 |
| `main08_miel_supp_002` | “몬스터가 적다는 건 좋은 일 아닌가요?” | `miel_supp_001`의 후보 복구 원본 |
| `main08_miel_supp_003` | “없어진 게 아니네요. 다들 이쪽을 피해서 지나가고 있어요.” | `miel_supp_002`와 `miel_supp_003` 정본문장 합본. 의미/호흡/품질 손실 없이 분리 가능한지 먼저 검사 |

- 기존 **Code → Catalog → GUID/meta → WAV 재생 경로 정상**이라는 기술 검사와, **WAV의 실제 발화 내용이 ID/원문과 맞는지**는 별개다. 이번 국소 ASR은 기존 콘텐츠 자체의 잘못된 할당·합본 정황을 확인했지만, **인간 청취 확인/파일 교체 완료를 뜻하지 않는다**.
- 사용자 원본 Unity/코드/Catalog/GUID/meta/WAV 수정0, 신규 TTS 생성0, 공식 반영0. Codex 보고상 처리 검사 Next/Mixer/Quest/Save/Continue **129 PASS / 0 FAIL**, 전체 **PARTIAL**(기존 Search 예외 및 청취 미승인). 보호 파일 5,539개 SHA 및 HEAD/미커밋 보존·Push0.
- 신규 제작 **개수를 지금 확정하지 않는다**. `taeon_supp_004`의 정확 원본을 찾지 못한 경우에는 **재제작 후보 1건**, `miel_supp_003`의 정본문장 분할이 품질상 불가한 경우에는 해당 ID별 추가 재제작 후보를 별도 표기한다. 재활용·절단본의 ID별 사용자 청취 승인을 받기 전 정식 Unity WAV로 교체하지 않는다.

### 다음 업무 — Codex / TTS 역할 분리

1. **Codex**: 사용자 로컬 소스 WAV 전체에서 정확한 원문을 먼저 재검색한다. 임의 파일 이름·CSV 순서로 연결하지 않는다. 기존 WAV 원본 byte/GUID 보존, 별도의 제안 폴더에 (a) 4개 단순 재사용 후보, (b) 미엘 002·003 구간 분리 후보, (c) 태온 004 부재 판정을 준비한다. 분리된 오디오는 원본 캐릭터별 Voice를 그대로 유지하되 잘림·문장 경계·음량/호흡을 검사한다. 후보 이름은 Stable ID 기준으로 만들되 원본 WAV를 덮어쓰지 않는다.
2. **TTS 담당**: 후보 정확본문/화자/목소리(Gacrux 태온, Sulafat 미엘)와 Scene Style·파일 상태 검토, **새 생성은 승인 전 0회**. 사용자 청취로 복구 불가가 확인된 ID만 `TTS_REGEN_REQUIRED`로 확정해 해당 프로젝트/모델 무료 quota 확인과 샘플 별도 승인 후 1 Stable ID/1 Call/1 WAV 정책으로 진행한다. Main18 세린10 대기 건의 정책과 quota를 섞거나 소비했다고 단정하지 않는다.
3. **바울 대표**: 최초 후보를 직접 청취해 대사 일치·캐릭터 음색·자연스러운 시작/종료 여부 승인. 새 TTS 호출 필요 시 별도로 명시적 승인.
4. **Codex**: 청취 승인된 WAV만 정확한 ID에 연결. Main08 Trace01~03 및 인접 ID, Next/화자·Portrait·UI/AudioMixer, Save·Continue, 원본 정상 TTS·메타·Unity 상태 보호 회귀. **`main` 직접 commit/Push/병합 금지**.

최종 상태: 제작 원본의 발화 오류 **원인 정황 접수 완료**, **음성 복구 미적용 / 인간 청취 승인 대기**. 기획 원문 수정은 필요하지 않음.


## 2026-10-13 Codex 현장 기술 감사 추가 — 원본 음성 변경0

SOURCE_CONTENT_MISMATCH_EVIDENCE / HUMAN_LISTENING_PENDING / CORRECTION_NOT_APPLIED. 해당ID→GUID→실제Clip 연결과 재생은 정상이나 제작 원본 WAV도 잘못된 문장/낮은 신호·경계 혼입을 포함하는 자동인식 증거를 확인했습니다. 태온002 RMS -48.98dBFS/1.03초, 미엘001 Main05문장·미엘002 앞문장, 미엘003 정본문장+앞문장 검출. 새 격리Runtime129PASS/FAIL0, 기존Search로PARTIAL, 사용자 실물 청취 승인은미완료입니다. 게임 코드/WAV/Catalog/원본 수정0. [ID별 관찰·재사용 후보·정확한TTS인계](Main08_Field02_Voice_Technical_QA_20261013.md). 최초 제보 내용은위 원문 그대로 보존했으며 전체Main08 결함/미엘003 문장오류로 확대하지 않았습니다.

## 2026-10-13 Codex 기존 음성 복구 후보 납품 — 정식 미적용

후보6개 준비 완료/재제작 확정0/모두 사용자청취대기. 태온002←003,003←004,미엘001←002 동일바이트복사, 태온004 긴정본문장은기존005에서찾아복사했습니다. 미엘합본003은450ms 낮은신호 간격의0샘플에서002/003으로무손실분리했고합산PCM은원본과동일합니다. 여섯후보 ASR정본문장일치, 인간청취승인은미완료이며원본WAV/GUID/meta/Catalog·Unity·Save·설정·TTS/API·Push변경0입니다. [청취파일·정확ID목록·길이·RMS/Peak·원본SHA·검증](Main08_ExistingVoice_Recovery_Candidates_20261013.md).


## 2026-10-13 바울 대표 청취 승인 — 기존 녹음 복구 후보 채택

**최신 사용자 직접 피드백:** “코덱스가 원래 짝 찾았고 목소리 다 들었는데 이제 어떻게 해야 해? 다 맞아.”

- **USER_LISTENING_APPROVED (후보 집합):** Codex가 찾아서 제시한 **올바른 기존 녹음 매칭/분리 복구 후보 전부**를 바울 대표가 실제로 청취했고 내용이 모두 맞는다고 승인했다. 1차 ASR 추정 상태보다 우선하는 최신 청취 확인이다. 후보 파일별 경로/SHA·정확한 여섯 ID 매핑의 로컬 결과는 Codex가 자신의 최신 결과를 읽고 추가 확정해야 하므로 **경로나 해시를 기획이 임의 기재하지 않는다**.
- **TTS 신규 제작 불필요 (현재 승인 후보 집합 기준):** 새 Gemini/API 호출·재녹음 금지. 태온 `supp_004`의 긴 문장과 미엘 `supp_002/003` 분리 부분까지 현장 후보 목록에 정확히 포함되어 있는지 Codex가 재확인하고 하나라도 누락되면 해당 ID만 HOLD로 두어 사용자에게 결과를 보고한다. 청취 승인이 있다고 해서 존재하지 않는 WAV가 생긴 것으로 가정하지 않는다.
- **후속 Unity 정식 연결 승인:** Codex가 사용자 청취 PASS를 받은 실제 후보만 올바른 Stable Dialogue ID로 연결/복사·분할, 반드시 **원본/목적지 모두 백업 후 최소 변경**, Catalog→GUID/meta→PCM→대사 런타임 확인. 어떤 ID에 원본을 재사용하더라도 **출처 음성은 유지**, 다른 대사의 클립을 파괴하는 단순 이동·rename 금지.
- 현재 **APPROVED_FOR_CODEX_INTEGRATION / NOT_YET_UNITY_APPLIED / FINAL_RUNTIME_QA_PENDING**. 원본 Unity, 미커밋, Save, Scene, 게임 원문, 기존 정상 TTS를 보호하며 검증은 비포커스 별도 worktree/격리 Save로 수행. 신규 `codex/*` 브랜치에서만 commit; **main 직접 commit/Push/병합 금지**. 완료 후 청취 승인과 런타임 음성 재생 PASS를 나누어 보고한다.


## 2026-10-13 Codex 승인6개 정식 연결·원본 반영 결과

APPROVED_SIX_APPLIED_TO_BRANCH_AND_ORIGINAL. 승인WAV6개를Main08/ApprovedRecovery20261013에별도등록하고Catalog6GUID만교정해기존원음·GUID/meta를전부보존했습니다. 실제Clip의승인PCM/GUID/경로6PASS,Trace01~03/화자·초상화·Next/Mixer/Quest/SaveContinue새177PASS/FAIL0,기존Search로PARTIAL. 원본프로젝트디스크에반영완료/원본Play·포커스·HEAD/Save·설정·Scene·BGM보호/신규TTS·Push0. 승인집합밖태온005/006의기존내용불일치와대화중일반조우사례OPEN,전체Main08발화PASS라고보고하지않습니다. [정식변경목록·원본백업·최종QA](Main08_ApprovedVoice_Integration_QA_20261013.md).
