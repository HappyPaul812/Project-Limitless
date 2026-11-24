# Issue #125·#130 · Main25 승인 음성 연결

2026-11-24 · `codex/ch3-main24-main25-playable-20261124` · 제품 `751c8458b203b55ad71355b5f7d267bd4bc7c4fd`

[대표 청취 승인 인계](https://github.com/HappyPaul812/Project-Limitless/issues/125#issuecomment-7123232724) · [연결 발주](https://github.com/HappyPaul812/Project-Limitless/issues/130#issuecomment-7123233472)

로컬 `F:/Downloads/Voice/Chapter3_StageOnly_72/Main25`에서 WAV, sidecar, 고정 manifest를 대조했습니다. 독립 Main25 progress 파일은 검색 범위에서 미확보입니다. SHA256 9/9는 sidecar 실측값과 일치했고, sidecar의 원문·원문 해시·화자·Voice를 docs/tts 정본과 고정 manifest 및 구현된 Main25 대사 순서에 대조했습니다. WAV는 PCM 24 kHz, 16-bit, 모노이며 기록된 길이와 일치합니다. 생성 시점의 청취 대기 sidecar는 승인 인계 기록과 함께 확인했으며 원본 파일은 읽기만 했습니다.

| Stage ID | Runtime ID | 화자 / Voice | WAV SHA256 |
|---|---|---|---|
| `ch3stage_main25_serin_001` | `main25_serin_001` | `companion_serin` / Erinome | `4c23155cfec976cd7d3259c649161aedf71fa02cea80bc87f245d69bea7fbc12` |
| `ch3stage_main25_paul_001` | `main25_paul_001` | `companion_paul` / Achird | `372aec106750c311b207949973780453986fdc3cc8610812831c5e71b03f583d` |
| `ch3stage_main25_paul_002` | `main25_paul_002` | `companion_paul` / Achird | `38edfc50c9ea63ed37187382639fc833ff8204c40ebf1804e94d0f93266497b4` |
| `ch3stage_main25_taeon_001` | `main25_taeon_001` | `companion_taeon` / Algieba | `a54acbce38c9ff66b67918ba8eaa4697faaab975c2ebf7a3b721d818bfa10fb2` |
| `ch3stage_main25_haren_001` | `main25_haren_001` | `story_haren` / Alnilam | `63a68cd199164d2f551bebe66e4411fdbbd1a6a8ff923f368b080239be9e237b` |
| `ch3stage_main25_serin_002` | `main25_serin_002` | `companion_serin` / Erinome | `6b3f73baa7bb7aaf8fa3254971e2968615b40fe4f9a77306bc45527736f92939` |
| `ch3stage_main25_taeon_002` | `main25_taeon_002` | `companion_taeon` / Algieba | `11c69c74235027df6ef9cb2d52d51a79da9a27f9082307aab90296157c5b8829` |
| `ch3stage_main25_miel_001` | `main25_miel_001` | `companion_miel` / Sulafat | `dd66600832134a40f94cf4f02197f57e286d8c16192ea78993896b9e50cbb009` |
| `ch3stage_main25_paul_003` | `main25_paul_003` | `companion_paul` / Achird | `f5d5f13f4c18e920fd5c0ec8fcc134f37322ffaa0c3916ff40609708067b0bde` |

- **Unity 6000.6.5f1 호환 QA PASS** · 실제 F3의 대사 트리거 네 곳에서 표식 1 → 표식 2 → 갑충전 직전 → 정리 대화 순으로 확인했습니다. 구간별 대사 배열의 순서·화자와 실제 `VoicePlaybackSource.Clip` 9/9를 확인했고, 플레이어 마지막 대사는 무음 자막으로 유지됐습니다. 대화 열기·닫기를 포함한 표적 검사 27 PASS, 실패 0, 종료 0. 로그 `Temp/i130-main25-voice-qa/play.log` 803–1180행. 처음 두 표식은 기존 진행 콜백으로 이어지고, 정리 대화는 별도 합성 진행 상태로 열었습니다. 기존 223 PASS 테스트는 재실행하지 않았습니다.
- Main24 WAV 10개는 고정 manifest의 해시와 일치하고 기존 Voice 자산 및 기존 카탈로그 항목을 보존했습니다. 새 WAV는 기존 프로젝트 WAV 및 서로 간 바이트 중복 0. Main25의 기존 Runtime ID와 원문은 유지하고 카탈로그 연결·화자 키만 추가했습니다. 재합성 및 TTS API 호출 0.
- 원본 프로젝트 클린 Import·Editor Search·실제 출력 장치 청취는 미검증입니다. Main26 이후 작업, 원본 main 병합 및 Push 없이 로컬에 커밋했습니다.
