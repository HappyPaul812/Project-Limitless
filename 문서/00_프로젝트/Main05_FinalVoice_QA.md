# Main05 승인 신규 Voice 최종 반영

## 구현 전 계획 — 2026-10-07

LOCAL 기존 입력 수정 f2d7e0c를 유지한다. 사용자 승인 `Limitless_TTS_Regen_Main05_5` WAV 정확히 5개만 기존 Asset에 복사하고 meta/GUID/Catalog/대사/Quest/SaveVersion은 보존한다. 원본 REGEN_REPORT의 미엘001 청취 FAIL은 과거 기록이며 이번 요청의 최신 사용자 전체 청취 PASS 5/5를 우선한다. 원본은 수정하지 않는다.

원본 ID/화자/본문/형식/SHA/PCM 중복을 먼저 검사한다. Main04 후속9→Guard6→독립 대표8→공식 해금→Main06 Field02 첫001을 격리 저장·비포커스 Runtime으로 연속 검사한다. 실제 Input System 가상 Enter/Space/A 중복 방지와 Save/Continue(보고 중/합류 후/Main06)를 검사한다. PCM 연결 검증은 사용자 청취 승인과 구분한다.

보호 baseline으로 Main03/04/06/17 등 다른 Voice와 시스템, 원본 폴더, meta, 244 NEEDS_LISTENING의 유지 여부를 확인한다. Compile/Console/Missing Script 및 실제 결과를 기록한 후 해당 WAV5와 QA 문서만 관련 commit에 포함한다. Push 금지.

## 최종 결과 — CLOSED

신규 원본 폴더 `Limitless_TTS_Regen_Main05_5`의 WAV5를 SHA256 그대로 반영했다. 사용자 최신 전체 청취 PASS5/5, 자동 Audio QA PASS5/5, 24kHz/모노/16bit, Duplicate PCM0. 원본 CSV 미엘001의 과거 FAIL은 최신 사용자 승인으로 해소되었으며 원본 기록은 수정하지 않았다. 기존 태온001은 사용자 정상 확인 항목으로 보호했다.

- [승인 원본5 상세 매핑](Main05_Final_Source_Mapping.csv): 화자/본문/원본/Unity/GUID/형식/길이/전체SHA.
- [Main05 전체6 최종 Matrix](Main05_Voice_Matrix.csv): Runtime resolve/PCM 전sample오차0, authored page당 재생1, 단일Source/자연종료/직전Clip정리. 입력회귀 fixture의 재생은 실제 보고 페이지 횟수와 별도다.
- 실제 Main04 후속9 → Guard6 → 별도 대표 상호작용8 → 태온/미엘 공식해금 → Main05완료 → Field01 Main06시작 → Field02 첫 `main06_taeon_001` 진입. [24페이지 기록](Main05_Final_Runtime_Page_Order.jsonl).
- Enter/Space/같은프레임 Enter+Space/A/닫힘+A 검사 PASS. page skip0, duplicate0, input bleed0, Guard→대표 자동연결0. 기존 수정 f2d7e0c 유지, 중복 게임코드 수정0.
- Main05보고 중/합류 후/Main06Field02 실제 Bootstrap Continue3회 목표·동료·Voice잔류 검사 PASS. 첫 시도는 QA의 직접 Scene로드가 저장위치를 갱신하지 않아 Field02검사 실패. 기존 SaveCurrentSession(scene) API에 현 Scene을 명시해 QA만 보정했고 최종 실행 전체 PASS. SaveVersion 변경0.
- Main04 `main04_miel_supp_011` Clip/GUID/PCM의 Main05오염0. Player portrait/voice0, 지문 speaker/portrait/voice0.
- Runtime PASS **239**, FAIL0, NOT_VERIFIED0, USER_INPUT_REQUIRED0(이번 승인 원본·자동 검증 범위). 실물 장치의 전기적 입력 검사는 가상 Input System 범위에 포함하지 않는다.
- CompileError0, 기존 CS0618 경고16, 신규 QA 경고0. 최종 Runtime ConsoleError/Warning0, MissingScript0. [검증 JSON](Main05_Final_Verification.json), [로그](Main05_Final_Runtime_Results.txt).
- baseline 3140개 동일: Voice변경은 Main05대상WAV5뿐, QA Editor helper1 변경. meta/GUID/Catalog/Dialogue/Objective/Save/Battle/Beast/Main17Quest·Art·BGM·다른Voice/사용자Save·원본 폴더 보호. 역사적 NEEDS_LISTENING244 유지, 최신 승인 상태만5행 갱신.

WAV commit `c67d632`, 기존 Input commit `f2d7e0c`. QA/문서 commit은 이 문서가 포함된 최신 `Docs: Main05 Voice Dialogue 최종 QA 상태` commit을 참조한다. Push0. 작업 범위 diff--check PASS이며 저장소 전체의 기존 unrelated Unity YAML trailing whitespace는 유지했다.
