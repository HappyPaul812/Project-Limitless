# LIMITLESS TTS / Main18 세린 Schedar v2 안전 준비 — 2026-10-13

**OFFLINE_PREPARED / LOCAL_NOT_EXECUTED / API_NOT_EXECUTED / USER_APPROVAL_REQUIRED**

## 근거

2026-10-13 `main` 정본 [Staged 안전 감사](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Serin10_Staged_Safety_Audit_20261013.md), [Schedar 인계](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Main18_Serin10_Schedar_Regen_Handoff_20261012.csv), [선택적 Unity 통합 QA](https://github.com/HappyPaul812/Project-Limitless/blob/main/문서/00_프로젝트/Main18_19_TTS_Selective_Integration_QA_20261012.md), `TTS_음성_제작_정책.md`, quota 기록, CURRENT_STATUS를 확인했다.

- 현재 다운로드 확인된 v1: `F:\Downloads\M18_Serin10_Staged.py` / SHA256 `e358511d852a014f0c2196754debe523cd55336bda8a11d18717a3278276b10f`. Erinome/Kore만 허용해 **실행 보류**. 수정·실행하지 않음.
- 공식 Voice: **Schedar 유지**. 실제 원본 10개의 과거 Wire Voice는 UNKNOWN. 요청 설정/서비스 변동 등 오류 원인은 미확정.
- Main18 오류10개만 USER_REPORTED_VOICE_MISMATCH / REGEN_REQUIRED. 기존 Main18 정상 세린5, Main19 정상 세린7, Main17 승인 세린11과 Unity에 이미 연결된 정상25 WAV **KEEP**.
- 샘플 1개: `main18_ground_serin_01`, `companion_serin`, 모델 `gemini-3.8-flash-tts`, Schedar, 원문 Manifest 고정.

## 새 v2 안전 준비본

다운로드된 독립 새 파일: `M18_Serin10_Schedar_v2_SAFE.py`, SHA256 `2edd62ff4c7cbc7fe055c665b39f35aaa5ef126691d1393c692fb42e8e1cd741` (13,057 bytes).

- **오프라인 전용**: Gemini/SDK/HTTP 요청 경로 없음, `sample` 명령은 무조건 차단. 사용자 새 승인 이후 1회 전용 호출 도구는 별도 검토·제공.
- `audit`: 쓰기0. Main18 Handoff10 정확한 ID·화자·ExactText·원본 해시, Main18 DialogueCatalog 순서 및 Manifest 상태, Main18 정상12 + Main19 정상13 원본↔Unity WAV SHA 및 세린 정상5+7 집합 검증. 불일치 중단.
- `prepare`: 실제 감사 후 `F:\Downloads\Voice\Limitless_TTS_Serin10_Schedar_v2` **신규 외부 폴더**만 생성. 오류10개 원본 byte 복사/해시검증, `BASELINE.json`, `API_CALL_LOCKED.json`, `STORY_ORDER_REVIEW.csv` 기록. v1·Unity·기존 WAV/Manifest에 쓰기 없음. 재실행/기존 폴더, 프로젝트경로 중복, symlink/junction 차단.
- 실행 예: `python .\M18_Serin10_Schedar_v2_SAFE.py audit`, 결과 확인 후 `python .\M18_Serin10_Schedar_v2_SAFE.py prepare`.
- 테스트: `py_compile` PASS, ZIP CRC PASS, 격리 인공 WAV/CSV fixture 기반 10 ID·기존 정상25·백업 SHA, 재실행 차단, 원본 해시 변조 차단, link 경로 차단, sample 실행 차단 PASS. **사용자 실제 F: 검사는 NOT_EXECUTED**.

## quota·Billing (계정별 실측 필요)

공식 Google 정책상 한도는 API Key 개수가 아닌 Project + Model 기준이다. 활성 프로젝트/모델별 무료 잔량, 현재 Billing Tier/과금 위험은 사용자 Google 계정 인증 접근이 없어 **NOT_VERIFIED**. 안전한 실행 예산 **0회**. 키 변경/한국 날짜 변경만으로 횟수를 산정하지 않는다.

- 사용자 확인: [Google AI Studio](https://aistudio.google.com/) Projects(해당 프로젝트 Billing Tier/키 소속), Dashboard > Usage, Rate limits(모델별 활성 RPD), Billing 상태. 키값·전체 환경변수/개인정보 공유 금지.
- 샘플 호출 전 **현재 무료 잔여 최소1 확인 + 유료 청구 위험 없음 확인 + 사용자 명시적 샘플1회 승인** 전부 필요.
- 샘플 1회는 별도 실행 코드에서 **1 ID=1 호출=1 WAV, checkpoint 사전 기록, 자동 재시도0, 실패/429 즉시 중단, Key 교체·후속 ID 호출 금지**.
- 생성 WAV는 별도 폴더, SHA/모델/Voice/SDK/요청 style/응답 MIME(비밀 제외) 기록. **사람의 실제 음색 청취 PASS 전 나머지9 생성 금지**. 승인 샘플은 10개 중 1개로 재사용.
- 나머지9는 청취 승인 뒤에도 별도 명시적 사용자 승인과 quota 재확인 필요. 최종 승인 WAV의 Unity 교체는 별도 Codex 작업.

## 현재 완료·미완

- 신 도구 오프라인 설계·fixture 검증 완료. 실제 사용자 로컬 감사/prepare **NOT_EXECUTED**.
- Gemini API 0, 신규 WAV 0, 신규 샘플 SHA 없음, 사용자 청취 미실시. Unity 수정0, GitHub main 수정0, Push/merge0.
- 분기: 이번 기록 **`docs/tts` 전용**. `main` 병합은 사용자 승인 필요.
