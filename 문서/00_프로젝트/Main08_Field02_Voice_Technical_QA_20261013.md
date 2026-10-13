# Main08 Field02 음성 원본·재생 경계 조사 — 2026-10-13

**SOURCE_CONTENT_MISMATCH_EVIDENCE / TECH_RUNTIME_PASS / HUMAN_LISTENING_PENDING / CORRECTION_NOT_APPLIED.** 브랜치 `codex/main08-field02-voice-20261013`. 원본과 제작팩의 WAV내용·ID 불일치가 주된 근거이며, 새 TTS/API 호출·WAV 편집/교체·Catalog 변경·게임 코드 변경0입니다. 바울 대표 청취 승인 전 기존 파일을 그대로 보존했습니다. 작업 결과는 QA문서·인계·증거로만 커밋합니다.

## 조사 기준과 발견

- GitHub docs/planning `589e5a915a687cc1ad3b36ba9e7ae92070746ecd`의 사용자 QA를 먼저 읽었습니다. 실제 원본 main HEAD는 ff022e6, 조사 작업 브랜치 기준은 ed0a3c3입니다. QA 작업 브랜치가 곧 원본 코드 상태라고 가정하지 않았으며 관련 Audio/Dialogue/Main08 코드·Catalog·WAV의 Git 차이가0임을 확인했습니다.
- 정본 자막/화자/StableID ↔ 원래 보충 제작 Manifest ↔ 로컬 Catalog GUID/fileID8300000 ↔ WAV.meta는 범위 내에서 일치했습니다. Main08 제작19파일 모두 기존 제작 원본과 게임 WAV의 SHA/PCM가 같고 Catalog ID 중복0입니다. Unity로 복사할 때 다른 WAV로 잘못 바뀐 증거는 없습니다.
- 기존 제작팩은 `Google TTS/Limitless_TTS_Output_Missing_Main01_05_08_12_v2`, README/CASTING 기록상 gemini-3.8-flash-tts, 태온 Gacrux·미엘 Sulafat입니다. 이는 **과거 제작 metadata**이며 현재 모델 제공/실제 API 사용 가능성을 새로 확인하거나 호출한 것이 아닙니다. 대상 파일별 추가 정상 원본은 이 제작팩에서 찾지 못했고 동일한 결함의 사본만 존재합니다. 반면 **다른 StableID 파일에 필요한 발화가 존재하는 재사용 후보**가 있습니다.
- 태온002는 **1.03초 / RMS -48.98dBFS / peak -30.57dBFS**, 24,720중24,411샘플이0은 아닙니다. 디지털 완전 무음이 아니라 매우 낮은 신호이고 Whisper에서 정본문장을 검출하지 못했습니다. 기존003에서는 “여기부터 흔적이 거의 없습니다.”를 검출했습니다. 단순 전체 볼륨/Normalize로 고치지 않았습니다.
- 미엘001의 인식 문장은 정본 Main05 `main05_miel_supp_003` 문장과 같습니다. 해당 Main08 WAV가 Main05 WAV와 동일 PCM라는 뜻은 아니며, 전체 Voice WAV PCM 검색에서 대상 Main08 파일과 같은 PCM의 다른ID는0개였습니다.
- 태온003/004/005의 발화와 미엘001/002의 발화가 Manifest의 해당 행과 다르고, 미엘003·회복 후보 태온006에 인접 두 문장이 함께 검출됐습니다. **제작 원본의 내용-파일ID·분할 경계 불일치**라는 근거입니다. 원 생성기의 내부 요청·응답 분할/저장 어느 지점에서 발생했는지는 원 request trace가 없어 NOT_VERIFIED입니다. 전체 Main08 또는 다른 Main 전체를 같은 결함으로 확대하지 않습니다.

## ID별 결과

| ID | 정본 자막 | 로컬 ASR 검출(청취 승인 아님) | 조치/재생성 판단 |
|---|---|---|---|
| `main08_taeon_supp_001` | 잠깐만요. | 잠깐만요. | 재생성 불필요. 기존 정상 경계 보호 |
| `main08_taeon_supp_002` | 여기부터 흔적이 거의 없습니다. | 문장 검출 없음 | 기존003에 정본 문장이 검출됨. 청취 승인 후 재사용 검토; 적합하지 않을 때만 재생성 필요 |
| `main08_miel_supp_001` | 몬스터가 적다는 건 좋은 일 아닌가요? | 저도 같이 가겠습니다. 제가 할 수 있는 일이 있을 거예요. | 기존002에 정본 문장이 검출됨. 청취 승인 후 재사용 검토 |
| `main08_taeon_supp_003` | 평소라면 그렇겠죠. | 여기부터 흔적이 거의 없습니다. | 기존004에 정본 문장이 검출됨. 청취 승인 후 재사용 검토 |
| `main08_taeon_supp_004` | 그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다. | 평소라면 그렇겠죠 | 기존005에 정본 문장이 검출됨. 청취 승인 후 재사용 검토 |
| `main08_miel_supp_002` | 없어진 게 아니네요. | 몬스터가 적다는 건 좋은 일 아닌가요? | 기존003에 정본 문장+다음 문장이 포함됨. 청취 후 분리 가능성 검토; 불가능하면 정확한 ID로 재생성 필요 |
| `main08_miel_supp_003` | 다들 이쪽을 피해서 지나가고 있어요. | 없어진 게 아니네요. 다들 이쪽을 피해서 지나가고 있어요. | 정본 문장 자체가 포함됨. 앞 페이지 혼입을 경계 문제로 검토; 분리 불가능하면 정확한 ID로 재생성 필요 |
| `main08_taeon_supp_005` | 초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다. | 그런데 조금 전까지 이쪽으로 이어지던 흔적들이 전부 방향을 바꾸고 있습니다. | 다음 경계에서도 정본과 다른 내용 검출. 기존006의 두 문장 분리 후보 검토 |
| `main08_taeon_supp_006` | 여기서는 그 방향이 조금 더 분명합니다. | 초원에서는 무언가를 피해 밀려오는 것처럼 보였습니다. 여기서는 그 방향이 조금 더 분명합니다. | 005·006 두 문장 포함. 경계 분리·청취 승인 전 보호 |

**TEXT_MATCH**는 자막↔Manifest 일치, **CATALOG_MATCH**는 명명된 WAV/화자/GUID 연결 일치, **PCM_MATCH**는 게임↔제작원본 동일을 뜻합니다. 이 기술 PASS들이 발화 의미 PASS는 아닙니다. 위 실제 발화 열은 자동 인식 증거이며 인간 청취·감정/억양/자연스러움 승인으로 보고하지 않습니다. 미엘003의 정본 문장은 포함돼 있고, 문제 후보는 **앞 문장 혼입/파일 경계**입니다. 해당 문장 자체가 틀렸다고 단정하지 않았습니다.

## 새 격리 Unity Runtime

- 실제 Unity6000.6.5f1에서 **129 PASS / FAIL0**: Main08 실제 Trace01→02→03 Interact·Quest 진행, 화자·자막·ID/페이지, 실제 Catalog AudioClip, Play 시작, 저음량 Clip.GetData·자연 종료, 빠른 Next 같은프레임 중복 차단, 단일Voice Source, Player 무음/이전Clip 제거, Pause/Resume, Voice100/35/0·Master Mute·라우팅, SaveVersion1·실제BootstrapContinue·Quest snapshot·잔류Clip0.
- 원본 사용자 설정 파일은 MuteAll=false/Voice100/SFX100/BGM7이었으며 쓰지 않았습니다. 격리 설정에도 동일한 비음소거·Voice100 조건을 만들어 태온002가 실제 재생되고 종료됨을 확인했습니다. Source 2D/volume1/Voice group, Mixer Voice100=0dB가 정상입니다. 원본 Editor의 제보 당시 AudioSource/Mixer 순간 상태와 실물 출력 장치는 NOT_VERIFIED입니다.
- 이 테스트는 **Listener.volume0**으로 격리 프로세스의 스피커 출력만 막았습니다. Source/Mixer 재생과Clip PCM를 조사했으며 실제 스피커 청취를 재현했다고 쓰지 않습니다. 격리 Save/설정 경로 외 원본 파일은 변경하지 않았습니다.
- 정본6000.5.7f1 직접 실행은 NOT_VERIFIED, 기존 UnityEditor.Search 예외로 Console 종합PARTIAL입니다. 새 Runtime 오류0. 최초 QA helper의 QuestCatalog.Find 호출 오기1개만 고쳐 재실행했고 게임 코드 컴파일 결함으로 분류하지 않았습니다.

## 수정/미수정과 후속

- 최종 게임 코드 변경0 / 음성·meta·Catalog 변경0 / Scene·Quest·Save 변경0. 재생 경계는 정상 검증됐으므로 근거 없는 코드 변경을 만들지 않았습니다. **사용자 음성 오류는 아직 수정 완료가 아닙니다.**
- 청취 승인 후 무제작 재사용 후보: 태온002←기존003, 태온003←기존004, 태온004←기존005, 미엘001←기존002. 해당 후보의 실제 문장·음색·앞뒤 경계를 먼저 확인해야 하며 이번엔 GUID 참조도 바꾸지 않았습니다.
- 미엘002/003와 태온005/006 경계는 기존 한 WAV에 두 문장이 있어 일괄 ID 이동만으로 해결되지 않습니다. TTS 담당은 원본을 보호한 채 정확한 StableID/원문으로 **기존 녹음 분리 가능성**을 검토하고, 분리가 부적합하거나 재사용 후보 청취가 실패할 때만 해당 ID의 재생성을 준비하십시오. 정확한 절단 샘플/새파일은 이번에 만들지 않았습니다. 새 생성/교체는 별도 청취 승인 전 금지입니다.
- [TTS 담당 인계 CSV](Evidence/Main08_Field02_Voice_20261013/Main08_Voice_Handoff.csv)에 정확한 원문/ID/화자/관찰/기존 후보/SHA·GUID를 분리했습니다. 담당에게 외부 메시지는 보내지 않았고 문서 인계만 준비했습니다. ASR와 제작 CSV를 TTS 실행 입력으로 자동 전환하지 않았습니다.
- 로컬 ASR faster-whisper1.2.1 + Systran faster-whisper-small INT8 CPU, 한국어/beam5/temp0/문맥 프롬프트0. 격리 Temp에 도구·공개 모델만 다운로드했고 음성 업로드/TTS/유료 호출/키 읽기0입니다. ASR 결과는 참고 증거입니다. 실행 코드는 [QA helper](Evidence/Main08_Field02_Voice_20261013/Main08VoiceAudit.cs.txt), [WAV·Catalog 감사](Evidence/Main08_Field02_Voice_20261013/pcm_catalog_source_audit.json), [런타임](Evidence/Main08_Field02_Voice_20261013/checks.txt), [ASR](Evidence/Main08_Field02_Voice_20261013/asr_results.json)로 확인할 수 있습니다.
- 원본 보호SHA/미커밋 목록/HEAD 전후 동일은 [보호 결과](Evidence/Main08_Field02_Voice_20261013/protection.json)에 기록했습니다. main Commit/다른담당브랜치변경/Push0. 원본 Editor 포커스0. 기존 정상 Main03~07/09~21·Main18 세린10 별도 감사·TTS/BGM 보호.
