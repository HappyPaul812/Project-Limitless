# LIMITLESS TTS / Main18 세린 Schedar 남은9 생성기 — 2026-10-13

**CODE_PREPARED / MOCK_TEST_PASS / LOCAL_EXECUTION_NOT_EXECUTED / GEMINI_API_CALLS_THIS_TASK=0**

## 사용자 승인 및 샘플 재사용
- 사용자: “뽑아. 그리고 지금 샘플도 쓸 수 있으면 쓰고 아깝잖아...ㅋㅋ” → **잔여 9개 제작 승인**.
- 기존 샘플 `main18_ground_serin_01`은 **정식 10개 중 한 개로 재사용**. 다시 호출하지 않음.
- 승인 샘플 SHA256 `3e655d2330a5caef7d6d37b3ff6be4c49a231bb89b1c9efa7ed5ec32c8b2505e`. 기존 성공 사용자의 직접 음색 청취 `USER_VOICE_APPROVED`, 문장 전체 정확성 `PENDING`.
- 예정 Voice `Schedar`, 모델 `gemini-3.8-flash-tts`, SpeakerId `companion_serin`, ExactText/Stable ID 변경 없음.

## 새 사용자 다운로드 생성기
- 신규 `M18_Serin_Schedar_REMAINING9.py`, SHA256 `94b75a9f0c7901bb4a94b2f0da017985036075809a593ee6c5838196a6a46c76`, 21,865 bytes.
- `M18_Serin_Schedar_REMAINING9.zip`으로 별도 배포. 기존 `M18_Serin10_Staged.py`, `M18_Serin10_Schedar_v2_SAFE.py`, `M18_Serin_Schedar_SAMPLE_ONE.py` 수정 없음.
- `check`: 기존 SAFE v2 10원본/10백업/25정상 WAV hash, Manifest+Catalog, 기존 샘플 생성 기록과 사용자 로그 SHA, Script SHA, Scene Style 일치 확인. API0/파일 쓰기0.
- `run`: **Key slot 2만**, 기존 샘플 제외 StoryOrder `02,07,08,09,10,11,12,13,14`의 9개만 최대 호출. 사용자가 현재 프로젝트+모델별 무료 남은 사용량과 과금 위험을 직접 확인한 뒤 대화형 `YES` 필요. SDK attempts1, 자동 retry0, 호출간 최소25초 대기.
- 생성 전 Dialogue ID별 독립 배타적 attempt checkpoint, 성공 후 결과/해시 기록. 중복 성공본은 다시 호출하지 않음. 오류/429/호출 결과 불확실 시 HOLD+세션 잠금, 다른 Key/ID 재개 금지. StoryOrder 중간 건너뛰기 방지.
- 출력은 프로젝트 외부 별도 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2\remaining9_wav\<ID>.wav`, 승인 샘플은 `sample_candidate\main18_ground_serin_01.wav`에 그대로 보관. 전체10개 StoryOrder CSV에 두 경로를 구분 기록.
- 원본10·백업10·기존 정상25·Main17 승인 세린11·Unity Catalog/Scene 수정0. Script가 사용자 PC에 다운로드된 것과 실제 실행은 미확인.

## 검증
- Python syntax compile/AST에서 실제 Gemini `generate_content` 호출 지점 1개, ZIP CRC 통과.
- 오프라인 mock 9회 순차 생성 → mock WAV9개, 샘플 재호출0, StoryOrder review, 결과9개 PASS.
- 오프라인 mock 둘째 호출에서 429 강제 → 저장 첫1개 보존, 2회만 시도하고 HOLD/Lock, 뒤7개 호출0 PASS.
- **테스트는 MOCK SDK이고 실제 Gemini 요청0**, 실제 성공/할당량·Billing 보증 불가.

## 계정 quota와 후속
- Google AI Studio 화면에서 무료 Tier/RPD 예시10·RPM3을 확인했으나 **Key2가 사용하는 프로젝트의 오늘 정확한 잔여량/결제 위험은 완전 검증되지 않음**. Key별 별도 quota라는 가정 금지.
- 1회 샘플이 이미 같은 Key2 슬롯에서 성공했으므로, 9회는 **다른 호출이 없고 Project+Model 무료 잔량 ≥9일 때만** 요청 가능. 사용자가 `--max-new-calls`로 안전한 수만 지정할 수 있다.
- 실패/429이면 즉시 중단하고 로그 판독 전 재실행 금지. 9개 WAV 성공 ≠ 사용자 청취 승인. 전체10 음색·원문·혼입/누락/길이 검수 후 Codex에게 별도 Unity 음성 연결 승인 필요.
- 이번 업무의 Unity 수정0, main 수정/merge0, 실제 GitHub 문서 기록은 `docs/tts`에서만.
