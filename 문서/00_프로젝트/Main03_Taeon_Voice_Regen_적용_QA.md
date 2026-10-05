# Main03 태온 첫 조우 Voice 005~007 교체 계획과 QA

2026-10-05 LOCAL 기준. **현재 단계: Unity 교체/Import/백그라운드 QA와 실제 Runtime 재생 PASS, 사용자 Runtime 의미 청취 확인 대기.** 문서화 → 기존 WAV 내용 교체 → Import/백그라운드 QA → 실제 첫 조우 재생·청취 확인 순서로 진행한다. Metadata만으로 의미 일치 PASS를 만들지 않는다.

## 입력과 기존 문제

입력 폴더 `F:/study/codex/Project-Limitless/Limitless_TTS_Regen_Main03_Taeon_005_007`. 005.wav/006.wav/007.wav만 사용한다. REGEN_REPORT.csv/txt와 사용자가 이번 요청에서 세 원본의 직접 청취 PASS를 확인했다. 모델 gemini-3.8-flash-tts, Voice Gacrux, Voice Design 사용 안 함. 이번 세션에서 생성/normalize/trim/reencode/rename을 하지 않는다.

| 입력 | Stable Dialogue ID | 현재 정식 본문 | 형식·길이 |
|---|---|---|---|
| 005.wav | main03_taeon_supp_005 | 잠깐만요. 더 가까이 가지 않는 게 좋겠습니다. | PCM24kHz/mono/16bit, 4.040초 |
| 006.wav | main03_taeon_supp_006 | 저 몬스터들 말입니다. / 그냥 돌아다니는 것 같지만… / 계속 같은 쪽을 피하고 있어요. | 동일, 7.160초 |
| 007.wav | main03_taeon_supp_007 | 그렇군요. / 그러면 제가 보고 있던 움직임하고 / 이어질지도 모르겠습니다. | 동일, 6.520초 |

기존005는 사용자 불일치 보고, 006은005 본문, 007은006 첫 문장만 발화하고 종료했다. 새 원본은 현재 본문으로 제작했고 사용자 원본 청취 PASS다. 원래 보충 Manifest의 Text/ID는 이미 현재 본문과 일치하므로 원문/ID를 바꾸지 않는다.

## 적용 계약

- 대상 Unity 경로: `Unity/Client/Assets/_Project/Audio/Voice/Story/Main03/main03_taeon_supp_005.wav`, `_006.wav`, `_007.wav`.
- 기존 .meta/GUID/AudioImporter/StoryVoiceCatalog Entry/Dialogue ID/companion_taeon Speaker와 Gacrux Registry 유지. Catalog 재구성/새 ID/게임 C# 기능 수정 없음.
- 새 WAV 전체 바이트를 그대로 복사한다. 다른 Voice/001~004/008 이후/Intro/다른 Character/원본 보충팩과 다른244개는 변경하지 않는다. 새 Source 폴더 전체를 Stage하지 않는다.
- 전체331대사/Player42/Voice247 정책 보존. 재생성 필요3개는 Runtime까지 통과한 뒤에만0으로 갱신한다. 244 NEEDS_LISTENING은 유지한다.
- 원본3039개 보호 해시와 Git415항목(기존410+새Source5개)을 기록했다. 실제 Save/Settings/Mixer/Scene/사용자 변경 보존, 격리 QA 사용. GitHub Push 없음.

## 검증 계획

Import load, 원본·Unity 바이트 및 PCM 일치, Catalog missing/duplicate0, ID/화자/자막/Clip1:1를 백그라운드에서 확인한다. 전체331개 재플레이 없이 Matrix 최소 consistency만 실행한다.

Field_01의 실제 Main03 첫 조우 factory/Presenter를 사용해 Taeon→Player→Taeon→Player→Taeon5페이지를 검사한다. 재생중/종료후 Next, 연속 Next, Player Voice/Portrait0 및 복원, Sequence 종료, Scene 정리, Voice0/복원·Mute ON/OFF·BGM 동시재생을 확인한다. 기존 격리 PlayUnfocused/Save/Settings 복원 경로를 사용한다.

이번 요청은 최소 Runtime 재생/청취를 명시적으로 허용한다. 오디오 의미는 Metadata만으로 확정하지 않는다. 사용자 원본 청취 증거와 Runtime에 실제 재생된 Clip 내용 검증을 구분하고, 필요한 Runtime 청취 결과를 확인한 뒤에만 최종 해결한다. Codex 오디오 입력/전사 미지원 한계는 유지한다.

## 결과

기존 Unity WAV3개만 새 원본 전체 바이트로 교체했다. meta/GUID/Importer/Catalog/게임 Runtime 코드/Mixer/Scene/Save/Settings/다른 Voice 및 Source 원본은 그대로다. 집중 QA용 Editor helper와 기존 감사 Python의 교체 원본 추적만 추가/갱신했다.

| ID | 기존·현재 동일 GUID | Unity length / sample count | Import / PCM 비교 |
|---|---|---|---|
| main03_taeon_supp_005 | 0dc9970aa869f5645add7545bc7696ca | 4.040초 / 96960 | Loaded, 원본과 차이0 |
| main03_taeon_supp_006 | 86cecb5f27e225f4f9a992b37e2e17e9 | 7.160초 / 171840 | Loaded, 원본과 차이0 |
| main03_taeon_supp_007 | 50d6684884c921344a0d2630b2b07e66 | 6.520초 / 156480 | Loaded, 원본과 차이0 |

- AudioImporter 정책 동일: loadType0, sampleRateSetting0, compressionFormat0, normalize0. 실제 Load/GetData 성공, PCM24kHz/mono/16bit 원본과 Unity float PCM을 모든 샘플에서 비교해 최대 오차0.
- Story Catalog229/Intro18/전체247 유지, Missing0/Duplicate ID0/Duplicate Clip0/Branch Collision0. 최신 본문·원래 Manifest·Speaker companion_taeon·각 기존 Clip1:1 유지. Catalog 전체 파일 해시 동일.
- 격리 백그라운드 **57 checks PASS**. Field_01 실제 Main03TaeonActor.TryInteract로 첫 조우를 시작했다. 실제5페이지/Player2개, 재생중 Next·빠른 연속 Next·NPC→Player Stop/Clip clear/Portrait Sprite null/영역 숨김·Player→Taeon Voice/Portrait 복원·단일VoiceSource·끝/Scene 정리 확인. 실제 종료 이벤트가 다음 전투 목표로 진행되며 전체Main03 플레이/전투/사용자Save 기록은 하지 않았다.
- 별도 실제 재생 **45 checks PASS**. 005→Player무음→006→Player무음→007 순서로 각 Voice를 끝까지 재생한 뒤 Next했다. Voice와 Field01 BGM 동시재생/독립 경로 정상. Voice0/복원·Mute ON/OFF는 앞선 백그라운드 QA에서 통과했다. 두 실행의 겹치는 검사를 독립 기능102개로 합산하지 않는다.
- 오디오 입력/전사 미지원인 Codex가 직접 의미를 듣고 PASS했다고 주장하지 않는다. 사용자 원본 청취 PASS + 실제 재생 Clip의 PCM 동일 증거는 확보했다. Runtime 재생에 대한 사용자 의미 확인은 별도로 요청했으며 답변 전에는 TTS_REGEN_REQUIRED3/NEEDS_LISTENING244를 유지한다.
- 공용 격리 Save/Settings/PlayUnfocused 절차로 기존 포커스 전환 없이 실제 재생했고 Bootstrap Edit Mode로 복귀했다. 본 요청의 최소 Runtime 청취 권한을 사용했으며 OS창/키보드/마우스 조작은 하지 않았다.

QA 로그/원본·기존 파일 hash/PCM 증거는 ignored `Temp/Main03Regen20261005/`에 보존한다. `Main03TaeonRegenAudit.Launch(false)`는 무음 기술 검사, `.Launch(true)`는 실제 음성 재생이며 사람 청취 PASS를 자동 생성하지 않는다. 전체331개는 Python Matrix 최소 consistency만 대조했고 전체Runtime 전수 재플레이는 하지 않았다.

격리 재생 QA는19:33에 종료·Bootstrap EditMode 복귀했다. 그 뒤 사용자의 실제 Field01 Play가 시작되고19:37 이후 슬롯1(사용자 캐릭터)의 저장 내용이 갱신되는 것을 읽기 확인했다. 이 사용자의 새 진행은 원복하지 않는다. 보호 해시에서 이 후속 Save 변경과 작업 WAV3개를 구분하며 기존 사용자 변경/새Source5개를 Stage하지 않는다. 최종 Editor가 사용자 Play 중이면 이를 강제로 종료하지 않는다.

WAV교체/집중 QA 로컬 commit `191e817`. 직접6파일(WAV3·Editor QA/helper meta·Python 감사)만 포함했고 새Source5개/다른Voice/사용자 변경을 제외했다. staged diff --check 통과. Runtime 의미 청취 대기 상태는 최종 문서에서 별도 관리하며 GitHub Push하지 않았다.
