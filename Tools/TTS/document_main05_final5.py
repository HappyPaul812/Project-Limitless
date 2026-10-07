# -*- coding: utf-8 -*-
"""최종 실행 결과를 정본 문서의 최신 상태에 반영하고 과거 감사 이력은 남긴다."""
import json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트'
s=json.loads((DOC/'Main05_Final_Verification.json').read_text(encoding='utf-8'))
assert s['status']=='CLOSED' and s['runtime_fail']==0
result=f'''\n## 최종 결과 — CLOSED

신규 원본 폴더 `Limitless_TTS_Regen_Main05_5`의 WAV5를 SHA256 그대로 반영했다. 사용자 최신 전체 청취 PASS5/5, 자동 Audio QA PASS5/5, 24kHz/모노/16bit, Duplicate PCM0. 원본 CSV 미엘001의 과거 FAIL은 최신 사용자 승인으로 해소되었으며 원본 기록은 수정하지 않았다. 기존 태온001은 사용자 정상 확인 항목으로 보호했다.

- [승인 원본5 상세 매핑](Main05_Final_Source_Mapping.csv): 화자/본문/원본/Unity/GUID/형식/길이/전체SHA.
- [Main05 전체6 최종 Matrix](Main05_Voice_Matrix.csv): Runtime resolve/PCM 전sample오차0, authored page당 재생1, 단일Source/자연종료/직전Clip정리. 입력회귀 fixture의 재생은 실제 보고 페이지 횟수와 별도다.
- 실제 Main04 후속9 → Guard6 → 별도 대표 상호작용8 → 태온/미엘 공식해금 → Main05완료 → Field01 Main06시작 → Field02 첫 `main06_taeon_001` 진입. [24페이지 기록](Main05_Final_Runtime_Page_Order.jsonl).
- Enter/Space/같은프레임 Enter+Space/A/닫힘+A 검사 PASS. page skip0, duplicate0, input bleed0, Guard→대표 자동연결0. 기존 수정 f2d7e0c 유지, 중복 게임코드 수정0.
- Main05보고 중/합류 후/Main06Field02 실제 Bootstrap Continue3회 목표·동료·Voice잔류 검사 PASS. 첫 시도는 QA의 직접 Scene로드가 저장위치를 갱신하지 않아 Field02검사 실패. 기존 SaveCurrentSession(scene) API에 현 Scene을 명시해 QA만 보정했고 최종 실행 전체 PASS. SaveVersion 변경0.
- Main04 `main04_miel_supp_011` Clip/GUID/PCM의 Main05오염0. Player portrait/voice0, 지문 speaker/portrait/voice0.
- Runtime PASS **{s['runtime_pass']}**, FAIL0, NOT_VERIFIED0, USER_INPUT_REQUIRED0(이번 승인 원본·자동 검증 범위). 실물 장치의 전기적 입력 검사는 가상 Input System 범위에 포함하지 않는다.
- CompileError0, 기존 CS0618 경고16, 신규 QA 경고0. 최종 Runtime ConsoleError/Warning0, MissingScript0. [검증 JSON](Main05_Final_Verification.json), [로그](Main05_Final_Runtime_Results.txt).
- baseline {s['protected_baseline_files']}개 동일: Voice변경은 Main05대상WAV5뿐, QA Editor helper1 변경. meta/GUID/Catalog/Dialogue/Objective/Save/Battle/Beast/Main17Quest·Art·BGM·다른Voice/사용자Save·원본 폴더 보호. 역사적 NEEDS_LISTENING244 유지, 최신 승인 상태만5행 갱신.

WAV commit `c67d632`, 기존 Input commit `f2d7e0c`. QA/문서 commit은 이 문서가 포함된 최신 `Docs: Main05 Voice Dialogue 최종 QA 상태` commit을 참조한다. Push0. 작업 범위 diff--check PASS이며 저장소 전체의 기존 unrelated Unity YAML trailing whitespace는 유지했다.
'''
p=DOC/'Main05_FinalVoice_QA.md';p.write_text(p.read_text(encoding='utf-8')+result,encoding='utf-8')
notice='''## 2026-10-07 최신 Main05 상태: CLOSED

사용자 전체 청취 PASS 신규5개를 기존GUID에 적용했다. Main05전체6 resolve/RuntimePCM PASS, Main04→Guard6→독립대표8→정식해금→Main06첫001 연속 및 Save/Continue3회 PASS. 입력 수정 f2d7e0c 유지. 과거 의미 미확정5개는 최신 승인 원본으로 해소했다. 역사적 NEEDS_LISTENING244 보존. 상세 정본은 [Main05 최종 QA](Main05_FinalVoice_QA.md), [검증 결과](Main05_Final_Verification.json), [최종 Matrix6](Main05_Voice_Matrix.csv). 아래는 이전 감사 이력이다.

'''
for name in ['Main05_Return_Voice_QA.md','Story_Voice_Main01_12_QA.md','Story_Dialogue_Consistency_QA.md']:
    p=DOC/name;p.write_text(notice+p.read_text(encoding='utf-8-sig'),encoding='utf-8')
p=DOC/'CURRENT_STATUS.md';old=p.read_text(encoding='utf-8-sig')
latest=f'''## 2026-10-07 Main05 승인 신규 Voice5 및 연속 QA 완료

- Main05 신규WAV5 적용, 최신 사용자 청취/자동QA5/5, 전체6매핑·PCM PASS. 문제 **CLOSED**.
- Main04후속9→Guard6→독립대표8→공식해금→Main06Field02첫001, 입력 중복/재열림/경계 및 실제 Save/Continue3회 PASS. Runtime {s['runtime_pass']}PASS/0FAIL, CompileError0, ConsoleError0, MissingScript0. 기존 CS0618경고16 별도.
- 다른Voice·Main17·Battle·Beast·원본·Save·GUID 보존, NEEDS_LISTENING244 유지. [상세QA](Main05_FinalVoice_QA.md).
- 사용자 직접 추가 확인 필수 없음(승인 원본 및 자동 검증 범위). 다음 권장: 보호된 기존 미청취 목록을 별도 작업에서 진행.
- 마지막 관련 구현 commit `c67d632`(WAV5), `f2d7e0c`(기존 입력수정). 최신 QA/문서 commit은 이 항목이 포함된 `Docs: Main05 Voice Dialogue 최종 QA 상태`를 참조. Push0.

'''
p.write_text(latest+old,encoding='utf-8')
print('PASS final QA/current/story documents updated')
