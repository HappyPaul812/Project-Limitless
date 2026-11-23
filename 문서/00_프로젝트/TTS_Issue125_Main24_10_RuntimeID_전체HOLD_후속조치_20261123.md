# LIMITLESS TTS / Issue #125 — Main24 읽기 전용 감사 결과 및 최소 후속

**기록일:** 2026-11-23
**현재:** MAIN24_10_RUNTIME_ID_HOLD / NO_API_POST / NO_NEW_WAV
**담당:** TTS·Codex, docs/tts 브랜치

## 1. 사용자 제공 실제 Codex 감사 화면

- Unity 6000.6.5f1에서 11월 19일 승인 주요 인물 대사 10개와 연결되는 실제 Runtime Dialogue ID + Speaker ID + Character ID 조합은 **0/10개**.
- VERIFIED_NEW=0 / VERIFIED_KEEP_EXISTING=0 / HOLD_RUNTIME_ID=10. 기존 Main24 candidate ID를 실제 대사 ID로 간주하지 않는다.
- 현재 확인된 탐험·안내용 12개:
  - main24_candidate_supply_01 ~ 03 (3개)
  - main24_candidate_north_01 (1개)
  - main24_candidate_f2_observe_01 ~ 03 (3개)
  - main24_candidate_guide_01 (1개)
  - main24_candidate_skill_01 ~ 04 (4개)
  이 12개는 Speaker ID와 Character ID 발화 연결이 없어 승인 대사 10개를 대표하지 않는다.
- StoryVoiceCatalog 312항목에서 ID 중복 0건, 위 안내 ID와 72개 승인 대사 후보 ID의 해당 Catalog 문자열 충돌 0건.
- 기존 승인 WAV **162개 SHA-256·매핑/GUID 일치** 보고. 조사 WAV 941경로 중 **대사 원문·화자 검증 가능한 348경로에서 동일 발화 0건**. 나머지 **593경로는 원문 메타데이터 부족으로 중복 여부 미확인**. 따라서 전체 941개의 중복 없음으로 확대해석 금지.
- reviewed_by_user=false, 전체 중복 검사 완료 flag=false 유지. 감사는 읽기 전용: Unity/원본 WAV/Save/Google API 변경 없음.

## 2. 결정: Main24 최소 Runtime 대사 연결 선행

기획 72개 원문·캐스팅 승인 자체는 유효. Chapter3 전체 구현까지 기다리지 않되, **Main24의 10개를 실제 플레이 가능한 Story/Dialogue 경로와 올바른 SpeakerID에 연결할 수 있는 범위만** Codex 별도 worktree에서 최소 구현한다.

- 실제 Runtime에 존재하고 진입 가능한 ID/원문/화자 일치가 확인된 행만 VERIFIED_NEW. 기존 WAV가 정확히 존재하면 VERIFIED_KEEP_EXISTING + SHA. 불확실하면 HOLD.
- 단순 CSV candidate ID 기재, 문서/임시 파일 추가, 12개 탐험 candidate ID 강제 치환은 Runtime ID 확정 증거가 아님.
- Main24 현재 런타임에 안전하게 대사를 연결할 수 없다면 강제 Story/Quest/씬 대수술 금지. STAGED_ID_PLAN 만 보고하고 TTS 파일럿의 HOLD 유지. 별도 stage-only 출력 방식은 사용자와 안전 기준 정리 전 자동 활성화하지 않는다.
- 남은 593개의 메타/manifest/journal/파일명을 읽기 전용으로 추적하되 확인 불가면 UNVERIFIABLE 그대로. 원본·다른 팀 자산 편집 금지.

## 3. Codex 복사 지시문

[LIMITLESS / Codex / Main24 TTS 10개 최소 Runtime ID 연결 및 593 WAV 추가 감사]

GitHub Issue #125, docs/tts의 Chapter3_Protagonist72_ApprovedQueue_20261123.json,
TTS_Issue125_Main24_10_RuntimeID_전체HOLD_후속조치_20261123.md 및 11월19일 승인 기획 원문을 확인하세요.

현재 감사에서 Main24 주요인물 대사 10개 모두 HOLD_RUNTIME_ID, 실제 연결 0/10입니다.
탐험 안내용 main24_candidate_* 총 12 ID는 화자 없이 존재합니다.
이 ID를 승인 음성 10개에 임의로 연결하거나 Runtime이라고 주장하지 마세요.

1) 실제 Unity 6000.6.5f1, 기존 worktree/main 상태를 확인하고
   codex/main24-tts-dialogue-contract-20261123 별도 브랜치/worktree에서만 작업합니다.
2) Main24 실제 Story/Dialogue/Scene/Quest 소스를 따라가서
   이미 승인된 주요인물 10개 원문과 SpeakerID를 변경 없이
   실제 Runtime에 안전하게 연결하는 최소 변경을 검토·구현합니다.
   플레이어 무음, NPC21 무음/마레2 보류, 탐험 ID12 및 기존 Main22/23 보호.
3) 발화 이벤트가 구현되지 않아 무리한 Quest/Scene 변경이 필요하면
   런타임 구현을 강행하지 말고 STAGED_ID_PLAN 및 BLOCKED 사유만 보고합니다.
   후보 ID를 실제 Runtime ID로 위장해서 승인표를 통과시키지 마세요.
4) 현재 941 WAV의 확인 가능한 348경로는 보호하고, 메타 부족 593경로는
   manifest, sidecar, 승인 journal, catalog를 읽기 전용 대조하세요.
   검증 불가 항목을 중복0으로 기재하지 마세요.
5) 실제 발화 트리거/StableID/화자/정확 원문 연결을 검증한 행만
   Main24_RuntimeAudit.json에서 VERIFIED_NEW 및 실제 ID를 채우고,
   이미 있는 승인 WAV는 VERIFIED_KEEP_EXISTING + 정확 WAV SHA를 기록합니다.
   나머지는 HOLD_RUNTIME_ID. reviewed_by_user는 false,
   approved_162_and_other_wav_duplicate_scan도 입증 전 false 유지.
6) 기존 승인 WAV162 SHA·.meta/GUID, Save/UserData, 원본 작업 폴더,
   main, Unity 버전, Discord, #85 HOLD 보호. Google API/키 조회,
   TTS 생성·기존 WAV 교체, 강제 포커스 전환, main merge·push 금지.
7) 범위 내 Unity 컴파일·런타임 대사 자막/Skip/Quest 회귀 검증.
   실제 변경 범위, VERIFIED_NEW/KEEP/HOLD 건수, 593 경로 확인 범위,
   미검증, 브랜치·커밋 SHA 및 사용자 승인 필요 사항 보고.

## 4. TTS 담당의 다음 단계

- 현재 Main24 시험 합성 **0건**이 정상 안전 중단. Chapter3_Principal72_Pilot.py는 검증 전 init/check만 허용.
- Codex가 실제 Runtime 안정 ID/기존 WAV 유무를 검증하고 사용자 검수한 뒤, 실제 Gemini 무료 quota/프로젝트별 외부 사용량/결제 상태를 사용자 PC에서 확인한다.
- 그 뒤에만 최초 VERIFIED_NEW 1건에 대해 run --max-calls 1을 사용자 본인이 실행, 청취 PASS 후 소량 배치.
- 나머지 Main25~29의 62개는 이미 승인된 원문·5인 캐스팅을 그대로 동결해 대기. 전체 Chapter3 구현 완료까지 기다리지는 않는다.
- 이번 문서 작업 API0, 신규 WAV0, Unity0, 기존 자산/Save/main0.

**관련 근거:** [72개 승인 큐](https://github.com/HappyPaul812/Project-Limitless/blob/docs/tts/Tools/TTS/Chapter3_Protagonist72_ApprovedQueue_20261123.json) / [Main24 파일럿](TTS_Issue125_Ch3_72_Main24_10_실제제작_안전실행_20261123.md) / [Issue #125](https://github.com/HappyPaul812/Project-Limitless/issues/125).
