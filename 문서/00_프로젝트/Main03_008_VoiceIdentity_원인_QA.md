# Main03 008 Voice 의미 불일치 원인 추적

2026-10-05 LOCAL. 문서화 → Runtime/Asset identity 추적 → 원본 직접청취·검증. 기존PASS005/006/007/003/004와012저음량 별도 이력을 보호하며 WAV/Mapping을 추측 수정하지 않는다. TTS API/Push 없음.

-008 기대 자막 “역시 이상합니다.”, 사용자 실제 Runtime 청취 “그냥 돌아다니는 것 같지만…”로 **RUNTIME_SEMANTIC_MISMATCH** 확정. 기존 기술 Resolve/Play/RMS PASS는 의미PASS가 아니며008 정상 의미 판정을 채택하지 않는다.
- Case A:008 화면에006 Clip이 Resolve/Play된다면 Catalog/Resolver/Index/Cache/Source 재사용 최소 수정, TTS 생성 금지. Case B:008 파일 자체가 잘못된 말을 한다면 WRONG_AUDIO_CONTENT로 전달하며 Mapping 보존.
-006/008의 Catalog Clip/Asset Path/GUID/instance/reference/hash/길이/RMS·Manifest본문을 비교한다. 실제 AudioSource.clip과008 페이지 ID/화자/자막을 읽기 추적하고 PCM을 파일과 대조한다. 현재 사용자 Field_01 Play를 종료하거나 Scene/포커스/Save를 조작하지 않는다.
-008의 Unity WAV를 별도 오디오로 제공하여 Runtime을 거치지 않은 직접 청취를 확인한다. 사람 발화 의미를 Metadata만으로 만들어내지 않는다.008 신규 원본 결함과012 기존 저음량을 별도로 집계한다.

최종 근거와 결과는 아래에 기록한다.

## Case B — 원본 내용 오류 확정

사용자는 Runtime과 Unity를 거치지 않은 현재008 WAV 직접청취에서 각각 “그냥 돌아다니는 것 같지만…” 계열을 확인했다. 기대/실제 화면 자막은 “역시 이상합니다.”. **WRONG_AUDIO_CONTENT / RUNTIME_SEMANTIC_MISMATCH**. 아직008 해결 완료가 아니며 기존 기술 재생PASS를 의미PASS로 해석하지 않는다.

| 항목 | 006 | 008 |
|---|---|---|
| Dialogue ID / Clip / WAV | main03_taeon_supp_006 / 같은 이름 / .wav | main03_taeon_supp_008 / 같은 이름 / .wav |
| GUID | 86cecb5f27e225f4f9a992b37e2e17e9 | 16a05ac1b6d319545a8f9113e2bd9aba |
| Unity Entity | 568105584918859162 | 568105584918859166 |
| 길이 | 7.16s | 2.25s |
| RMS/Peak dBFS | -18.9963/-2.5990 | -21.6288/-4.0663 |

Unity 경로는 각각 `Assets/_Project/Audio/Voice/Story/Main03/main03_taeon_supp_006.wav`, `Assets/_Project/Audio/Voice/Story/Main03/main03_taeon_supp_008.wav`. Catalog008은008 Clip을 참조하며006과 이름/Path/GUID/entity/reference/hash 모두 다르다.

006 본문: “저 몬스터들 말입니다. / 그냥 돌아다니는 것 같지만… / 계속 같은 쪽을 피하고 있어요.” 기존 의미PASS 유지.008 Manifest 기대 본문 “역시 이상합니다.”, 화자 companion_taeon / 태온 / Gacrux는 정상.

main03_taeon_supp_006 SHA256: `deb234f78c9211cc937a3f8e07f070bfa26619cf021f29fa1793440885ac7429`

main03_taeon_supp_008 SHA256: `a16d378060acc7ff290a24bd65f28fb62548da49416eada089bae144c6d7042c`

실제 사용자 Field_01 Play의 AfterBattleConversation index0: ID008 / 자막 “역시 이상합니다.” / resolved008 / AudioSource.clip008. sourceIs008=true, sourceIs006=false, sameClip=false. 조회 시 isPlaying=false는 종료 후 남은 Clip 상태이며 새 Play 증거로 취급하지 않는다. 이전 전체 Runtime 감사의008 Play 성공과 추출 PCM54,000개=현재 WAV 전체, 최대 오차0을 이번에 재대조했다.

Catalog는 ID+Speaker 문자열 조회이고 Dialogue순서 index로 Clip을 고르지 않는다. ShowSequence는 페이지0 초기화, RefreshSequenceText는 해당ID Resolve, VoicePlaybackSource.Play는 기존Source Stop/Clip 초기화 후 새Clip 대입/Play. ClearSequence도 초기화하며 Battle 전후 Presenter 수명이 바뀐다. 실제008 reference가 정상이라 Cache/Index/Fallback 수정 근거 없다.

추가 Runtime 재조회는 자동 승인 검토 모델 capacity 오류로 실행되지 않았다. 앞서 성공한 조회/PCM 증거를 사용했다. 사용자 Play/Scene/Save/Settings/포커스 변경0. Console Error0/Warning0. C# 변경이 없어 강제 재컴파일은 하지 않았다.

## TTS 전달 / 회귀

008: `main03_taeon_supp_008.wav` / Taeon(companion_taeon) / Gacrux / 정확한 전체 본문 **“역시 이상합니다.”** / 현재 실제 발화 “그냥 돌아다니는 것 같지만…” 계열 / WRONG_AUDIO_CONTENT / 재생성 필요. 새 WAV 원본 직접청취 및 Runtime 의미 일치 검증 필요. TTS API 실행0. C#/Catalog/Mapping/WAV/meta/import 변경0.

012: 기존 SOURCE_LOW_LEVEL_AUDIO, RMS-51.9037 / Peak-32.6244dBFS. 본문 “목적이 같다면 잠시 함께 가시죠. / 혼자 움직이는 것보다는 안전할 겁니다.” 별도 정상 음량 원본 대기. **확정 재생성2건:008 신규1+012 기존1**. 나머지6종 의미 미청취로 자동 정상/재생성 처리하지 않는다. 원래244 NEEDS_LISTENING 관리범위 유지.

005/006/007/003/004 원본과 사용자 의미PASS 유지. 이전 전체 Runtime 감사의 Player Voice0 / Portrait0 유지, 이번 사용자 Play를 조작하여 회귀 재실행하지 않았다. GitHub Push 없음.

보존 검증: 시작3042개 Asset/Settings/Save 등 해시 비교에서 변경은 현재 사용자 Play 중인 save_slot_01.json 1개뿐. Codex Save 조작0이며 사용자 진행 변경을 보존했다. 기존 git 상태419항목 누락0. Assets/C#/WAV/meta/import/Mapping 변경0.
