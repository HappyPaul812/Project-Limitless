# LIMITLESS / Main18 세린 10개 최종 사용자 승인 및 Codex Unity 통합 인계 (2026-10-19)

**APPROVED_BY_USER_FOR_INTEGRATION / LOCAL_UNITY_NOT_YET_APPLIED / TTS_API_0**

사용자 최신 지시: “다 이상 없으니까 메인18은 끝인거 같으니 게임에 넣자, 지금 파일이 여기저기에 있으니까 알어서 할 수 있게 코덱스한테 시켜줘.”

따라서 **Main18 세린 교체 음성 10개에 대한 최종 청취 승인과 Unity 적용 승인**을 인정한다. 이전 QA의 청취 대기 표시는 이 지시로 갱신한다. 보정 파일 6개는 옛 원본이 아닌 사용자가 긍정적으로 평가한 DSP PREVIEW를 승인된 최종 후보로 채택한다.

## Codex 실제 통합 작업 지시

- 로컬 프로젝트: `F:\study\codex\Project-Limitless\Unity\Client`.
- `AGENTS.md`, `CURRENT_STATUS.md`, `CODEX_WORKFLOW.md`, `Main18_TTS_Manifest.csv`, `Main18DialogueCatalog.cs`, `Main18_19_TTS_Selective_Integration_QA_20261012.md`, `Main18_19_TTS_Selective_Applied_Matrix_20261012.csv`, TTS 인계 최신 내용과 LOCAL Git status 먼저 확인.
- 메인 직접 commit, Push/force Push/merge 금지. `codex/tts-main18-serin10-integration-20261019`의 독립 로컬 worktree 사용. 기존 사용자 미커밋 변경 보호.
- 정확히 **10개만** 가져와 `Assets/_Project/Audio/Voice/Story/Main18/<DialogueId>.wav`로 Unity Import 및 현재 실제 Voice Audio Catalog 패턴대로 `companion_serin` 매핑을 추가. 기존 Main18 정상12, Main19 정상13 합계25개 및 모든 기존 Main01~17·Catalog 항목·GUID·Source WAV·Checkpoints·Save 전부 불변. Main18 NPC 22/22 연결 목표.
- **Gemini TTS 요청/재생성 0**, 기존 잘못된 원본 WAV를 임의 복사 금지. WAV 재인코딩·Pitch/EQ/Trim/Speed/Normalize 금지.
- 전체 필수 파일 10개의 출처·해시·형식·DialogueId/ExactText/화자·실제 StoryOrder 일치 확인. 일부 없거나 해시가 다르면 임의 대체하지 말고 BLOCKED와 찾은 경로를 보고.

## 10개 파일의 올바른 출처

| Story | Dialogue ID | **반드시 선택할 출처** |
|---:|---|---|
| 02 | `main18_leon_serin_01` | **보정** `main18_leon_serin_01_PREVIEW.wav` |
| 06 | `main18_ground_serin_01` | **승인 샘플** `sample_candidate/main18_ground_serin_01.wav` |
| 07 | `main18_heat_vent_serin_01` | **수동 복구** `ManualRecovery_20261019/candidates/main18_heat_vent_serin_01.wav`; 옛 불합격본 금지 |
| 08 | `main18_overheat_serin_01` | **보정** `main18_overheat_serin_01_PREVIEW.wav` |
| 09 | `main18_overheat_serin_02` | **보정** `main18_overheat_serin_02_PREVIEW.wav` |
| 10 | `main18_overheat_serin_03` | **보정** `main18_overheat_serin_03_PREVIEW.wav` |
| 11 | `main18_overheat_serin_04` | **보정** `main18_overheat_serin_04_PREVIEW.wav` |
| 12 | `main18_vibration_serin_01` | **보정** `main18_vibration_serin_01_PREVIEW.wav` |
| 13 | `main18_watcher_witness_serin_01` | **수동 복구** `ManualRecovery_20261019/candidates/main18_watcher_witness_serin_01.wav` |
| 14 | `main18_watcher_witness_serin_02` | **수동 복구** `ManualRecovery_20261019/candidates/main18_watcher_witness_serin_02.wav` |

파일 찾기:
- 기본 루트 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2`.
- 승인 샘플: `sample_candidate`.
- 최신 복구 3개: `ManualRecovery_20261019\candidates` 및 `results`의 SHA 기록. Story07 4.88s / Story13 4.96s / Story14 4.20s가 사용자 콘솔 결과. SHA는 **실제 bytes/결과 기록**으로 확인, 추측 금지.
- 보정 6개: `LIMITLESS_Serin6_ConsistentVoice_Previews.zip` / 동명 압축 해제 폴더를 `F:\Downloads\Voice`, `F:\Downloads` 등에서 검색. ZIP SHA256 `a5878e6d4f1b1e858f4b505f23669ab147da2dd466fe30443b0d6116a7246cb1`; `PREVIEW_AUDIT.csv` 참고. 없다면 미보정 `remaining9_wav`를 대체로 쓰지 말 것.
- 승인 샘플 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`.
- 보정 6 WAV SHA256 (source bytes):
  - leon `9d341d7fba030fabffda9a2266fdfb4b367b3bf80a210c0113e318f2a322b60e`
  - overheat01 `deafc7b775659b8f8826dad675af2f5bc9674b7c103feeafbc8a014bc07e5fdf`
  - overheat02 `bbfb6ac61f76c92d32f6c61fafe61625e7d9399874fdddb0564dfc5874eacbb5`
  - overheat03 `b67811bbd3abae2d53ad4cdecc6209c936a5b01a37c2f8ba82e9806e749ea442`
  - overheat04 `bc73622d70a4b0c16e3a554cee7467302466d9808fcca327cca2fc18b81d5f57`
  - vibration `638d83be61f7ec22e54fe019969beca72f7fc3ed7fd2106617ae396e29095732`
- 보정 입력은 최종 파일명 `<DialogueId>.wav`로 **바이트 그대로 복사**(접미사 `_PREVIEW` 제거)한다. 복사본 SHA가 선택 소스 SHA와 동일해야 한다.
- 기존 Story13 `REMAINING9_HOLD.json`, `REMAINING9_SESSION.lock`은 **역사적 생성 실패 증거로 보존**하며 제거/초기화하지 않는다. Story13의 승인된 별도 수동 복구 WAV만 사용할 것.

## QA/문서/검증

- 오디오 파일10 모두 24kHz mono PCM16, 길이/Decode/Clipping, 바이트 SHA와 Unity GetData PCM 일치 검증. 기존25 원본+GUID/AudioCatalog 보존 및 기존 Catalog 전체 Resolve 확인.
- Unity 현재 설치 버전 프로젝트 파일 기준 사용. 원본 Editor 강제 종료/Play Mode·OS포커스 개입 금지. 격리 백그라운드 Import/Compile/Runtime QA.
- Main18 대사 NPC22/22 StoryOrder, Clip/Dialogue ID/Speaker/ExactText 연결, 자연 종료·Next·빠른 Next·Player/지문 무음·Mixer/Mute·Scene전환·Save/Continue 확인. 기존 Main19 정상13 회귀 및 Main17 중요 오디오 확인.
- 기존 Search 예외는 Known Issue로 분리, 신규 오류와 혼동하지 않는다. 자동 ASR만으로 사람 청취 승인 판단하지 않음. **이번 10개 사용자 청취·사용 승인 확정**으로 문서화.
- 성공한 경우에만 Main18 Manifest **10개 Status만** 업데이트하고 `CURRENT_STATUS.md`에 통합 결과, QA 상태, 보호 내용을 기록. 자막·화자·Stable ID·Voice 설정·Scene/Quest/전투 등 불필요한 내용 변경 금지.
- 산출: `Main18_Serin10_Final_Source_Matrix.csv`, `Main18_Serin10_Final_Unity_Integration_QA_20261019.md`, 선별 로컬 commit. 브랜치/Commit SHA/Unity 성공·부분·미검증/기존25 보호/Push=NO/최종 main 반영에 사용자 승인 필요 명시.
- 전부 합격하지 못하면 `QA_PARTIAL` 또는 BLOCKED를 솔직히 보고하고 임의로 전체 PASS 처리하지 않는다.

**Codex에 대한 실행 위임이며, 이 ChatGPT TTS 세션 자체가 사용자 PC Unity 작업을 수행한 것은 아님.** `docs/tts` 담당 문서는 승인·통합 지시만 보관하고, 실제 구현은 `codex/...` 전용 로컬 작업 브랜치에서 수행한다.
