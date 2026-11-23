# Main24 승인 대사 10행 · STAGED_ID_PLAN — 2026-11-23

**실제 연결 0/10 · VERIFIED_NEW 0 · VERIFIED_KEEP_EXISTING 0 · HOLD_RUNTIME_ID 10**

[Issue #125 후속 정본](https://github.com/HappyPaul812/Project-Limitless/issues/125) · [docs/tts 최신 인계](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/문서/00_프로젝트/팀운영/인계/TTS_LATEST.md) · [11/19 승인 원문](../03_스토리/Chapter3_정식대사_보상기준_밸런스검증_20261119.md).

## 연결 범위와 단계 계약

현재 `Chapter3Main24Flow`의 준비 대사는 구매 불필요·대응 물품 지급 준비 중을 안내합니다. `Main24F2Traversal`은 후보 Scene으로 이동하고, 관찰의 완료 콜백과 마을 복귀 단계는 실제 탐험 진행만 기록합니다. 선택형 `Main24FrostOwlCandidate`는 공식 이야기 승리와 별개입니다. 승인 11/19 대사의 출발·조사·승리 맥락은 이 경로에 연결돼 있지 않습니다. 탐험 안내 12개 또는 선택형 후보 전투를 공식 발화 트리거로 사용하면 기존 경로에 승인 이야기 전제를 섞게 됩니다.

| 구간 | 승인 원문·화자·Voice가 고정된 제안 ID | 안전한 연결 조건 |
|---|---|---|
| 출발 4 | `main24_serin_001`, `main24_miel_001`, `main24_serin_002`, `main24_paul_001` | 정식 보급 준비 및 최초 출발 경계 확정. 실제 출발 의사 확인 뒤 1회 표시, 취소·재입장은 기존 이동/진행 보호. |
| 조사/올빼미 4 | `main24_serin_003`, `main24_haren_001`, `main24_taeon_001`, `main24_haren_002` | 정식 조사·전조 또는 이야기 조우 경계, 하렌·태온 등 발화자가 참여하는 정식 시퀀스. 기존 QA 조우와 분리. |
| 승리 2 | `main24_miel_002`, `main24_paul_002` | 정확한 정식 조우의 승리 확정 후 1회. 도주·패배·후보 전투에서는 실행하지 않음. |

향후 `DialogueLine(speakerId, speakerName, text, dialogueId)`를 사용하고 승인 캐스팅과 공용 Presenter/Catalog 화자 검사를 유지합니다. 제안 ID는 현재 정확 문자열 충돌 0이지만 실제 Runtime ID는 아님. `main24_candidate_supply/north/f2_observe/guide/skill`의 12개 안내 ID 보존. 보급·조우·정식 Story 연결은 이번 작업에서 구현하지 않았고 Save 구조는 그대로입니다. Main25~29 62행 원문/VoiceID는 대기열에 유지했습니다.

## WAV 추가 조사 및 보호

- 기존 미확인 593경로를 기준으로 Sidecar, Manifest·CSV·완료 제작 장부 **16개 파일**을 추가 대조했습니다. 파일명으로만 이어지는 8경로를 찾았으나 성공 기록·SHA를 현재 WAV와 연결하는 근거가 없습니다. **추가 확정 0, 미확인 593경로(고유 해시 401개)**. 근거 있는 348경로에서 동일 화자·정확 원문 0. 전체 중복 확인 플래그 및 `reviewed_by_user`는 false.
- 승인 WAV162 SHA/GUID·화자 **162/162 일치**, StoryVoiceCatalog 312 ID 중복 0. 기존 WAV/카탈로그 변경 0. TTS·Google API·API 키·개인 Save 작업 0.

## 검증 범위

Unity **6000.6.5f1** 숨김 배치. 현재 제품 `Chapter3Main24Flow.cs`를 컴파일하고 인메모리 Presenter/Quest/이동 fixture로 기존 12 안내 ID·순서·빈 Speaker, 완료 전 진행 방지, 기존 스킬/보급/관찰 콜백과 3단계 이동 분기를 확인했습니다. **17 PASS / 0 FAIL**. 실제 제품 전체 컴파일·Scene 주행·Save 검증은 수행하지 않았습니다. 저장 슬롯은 0이며 fixture의 저장 호출은 금지했습니다. [격리 로그](Evidence/Issue125_Main24_Contract_20261123/unity-delta.txt)

담당 worktree/브랜치 `codex/main24-tts-dialogue-contract-20261123`. 원본 main, Unity 버전, 기존 자산을 보호했습니다. 실제 출발·정식 조우 계약 승인과 근거 미확인 WAV 검토 이후 각 행을 재평가해야 합니다.
