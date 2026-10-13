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


## 2026-10-13 Codex 현장 기술 감사 추가 — 원본 음성 변경0

SOURCE_CONTENT_MISMATCH_EVIDENCE / HUMAN_LISTENING_PENDING / CORRECTION_NOT_APPLIED. 해당ID→GUID→실제Clip 연결과 재생은 정상이나 제작 원본 WAV도 잘못된 문장/낮은 신호·경계 혼입을 포함하는 자동인식 증거를 확인했습니다. 태온002 RMS -48.98dBFS/1.03초, 미엘001 Main05문장·미엘002 앞문장, 미엘003 정본문장+앞문장 검출. 새 격리Runtime129PASS/FAIL0, 기존Search로PARTIAL, 사용자 실물 청취 승인은미완료입니다. 게임 코드/WAV/Catalog/원본 수정0. [ID별 관찰·재사용 후보·정확한TTS인계](Main08_Field02_Voice_Technical_QA_20261013.md). 최초 제보 내용은위 원문 그대로 보존했으며 전체Main08 결함/미엘003 문장오류로 확대하지 않았습니다.
