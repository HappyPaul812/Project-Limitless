# Main03 AfterBattleConversation 최종 Voice 적용 계획·QA

2026-10-05 LOCAL. 문서화 →012 WAV 교체·필요한 QA 보강 → 격리 백그라운드/실제 재생 검증.

- 입력 `Limitless_TTS_Regen_Main03_Taeon_Supp012/main03_taeon_supp_012.wav`:7.76초/PCM24kHz mono16bit, 원본 사용자 의미 청취PASS. Unity 기존012 WAV만 전체바이트 교체하며 Path/meta/GUID/import/Catalog/ID/Speaker 유지.
- 정확한 본문: “목적이 같다면 잠시 함께 가시죠.
혼자 움직이는 것보다는 안전할 겁니다.”012 재생·음량·전체PCM·끝까지대기/Next/다음단서001/002·종료/Quest/격리Save 재로드 검증.
-008은 이전 Runtime 및 WAV 직접 사용자 청취가 모두 “그냥 돌아다니는 것 같지만…” 계열로 확정되었다. 이번 요청의Case A=WRONG_AUDIO_CONTENT(이전 원인QA의Case B와 같은 결함; A/B 명칭 순서만 바뀜). 원본 hash 불변이면 이 청취 증거 유지. Mapping/C#/정상006 교체/TTS 생성 없이008 한 건 전달.
- 기존 실제 의미PASS005/006/007/003/004와 다른 모든Voice/Player Voice0/Portrait0/Mixer/Quest 구조 보존.244 NEEDS_LISTENING 원래 관리범위 유지.
- 현재 cleanBootstrap EditMode/is_focused=false. 기존 Main03RemainingVoiceAudit 격리 프로필/PlayUnfocused로 전체흐름 재사용. 후속008/Player/009/Player/010/011/012/단서001/002 연속 기술·UI·Next·Cleanup·Main03완료 확인. 의미 청취를 기술PASS로 대체하지 않으며009/010/011/001/002는 별도 청취 미확정 상태 유지.
- 실제 재생은 후반부만 완료시간까지대기; 첫 조우/전투전은 무음 보호검증. 사용자 Save/Settings는 격리하며 창 활성화/OS 입력/포커스전환0. 직접 변경만commit, GitHubPush0.

결과는 아래에 추가한다.
