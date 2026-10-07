# -*- coding: utf-8 -*-
"""최신 Main07 검사 결과를 정본과 현재 상태에 기록한다."""
import json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트'
s=json.loads((DOC/'Main07_Early_Verification.json').read_text(encoding='utf-8'))
assert s['runtime_fail']==0 and s['console_errors']==0
result=f'''\n## 결과 — 입력 수정 완료 / Voice 제작 대기

### 사용자 관찰과 Paul 원인

사용자는 후속 질문에서 **첫 자막이 그대로였음**을 확인했다. 이는 PAGE_AUTO_ADVANCE와 구별되는 **WRONG_AUDIO_MAPPING 범주 중 WAV CONTENT 오류**다. Catalog reference shift는 아니다. Paul001/002 모두 본래ID→본래GUID→본래WAV→원본TTS SHA→Runtime PCM 전sample오차0. 따라서 사용자 오발화가 난 페이지0의 실제 파일은 **main07_paul_001**이며 해당 파일을 **TTS_REGEN_REQUIRED1**로 분리했다. 파일 이름/길이/Hash로 발화 의미 PASS하지 않았다. Paul002의 내용 자체는 별도 청취 미검증이며 임의 Wrong 판정/재생성 확정을 하지 않는다.

수정 전에도 Paul 첫 페이지를 E로 연 뒤 {s['no_input_wait_seconds']:.3f}초(duration13.37+5초 이상) 무입력 대기하여 page0/첫 자막/PauI001 Clip 유지, Paul002 Resolve/Playback0을 확인했다. 명시 E Next1회에서만 page1/PauI002 Clip으로 변경했다. 자동 Voice 종료→다음페이지 코드 없음. 사용자가 확인한 오발화는 음성 교체 전까지 해결됐다고 주장하지 않는다.

### 별도로 재현·수정한 입력 결함

Main07 custom `Main07Interact`와 공용 `Interact`가 모두 E/F/A를 받는다. 공용 마지막 Next가 대화를 닫으면 WorldModalState는 해제되지만, 같은 프레임의 custom callback은 기존 Current()/modal 검사만 통과해 다시 대화를 열었다. 마지막-page fixture에서 동일프레임 E/A 각각 **열림True**를 재현했다. 이 결함은 무입력 Paul0→1 관찰의 원인으로 합치지 않는다.

Main07 custom Interact 진입에 기존 공용 `CanBeginInteractionThisFrame`을 적용했다. 닫힘 프레임에는 열지 않으며 열린 대화 Next는 공용 InteractionSystem이 담당한다. 공용 시스템 변경0. 수정 후 E/A 재열림0, E/F 열기page0, Enter/Space/E/F/A 각각Next1page, 동일프레임 복합입력1page, held E 반복Next0. Production 진단 spam0; Editor QA helper의 InputSystem Action 이벤트와 Before/After snapshot만 파일 기록.

### 초반 Voice7

GetLines NPC7은 Stable ID 누락이었다. 기존 Main07 보충번호 충돌0을 확인하고 태온 supp001~003/미엘 supp001~004를 부여했다. 기존 Paul001~020/태온001~003/미엘001~003 ID와 대사 순서/화자/본문/Objective/SaveVersion 유지. Player “따라가 보죠.”는 Voice/Portrait0.

- [정확본문 Handoff7](Main07_Early_TTS_Handoff.csv)
- TTS 담당 입력 manifest: `Tools/TTS/main07_early_tts_manifest.csv`
- [Paul001 재생성 목록1](Main07_Paul_TTS_Regen_Required.csv)
- [Paul 기존2 기술 Matrix](Main07_Paul_Clip_Matrix.csv)

**TTS_REQUIRED7 / Catalog PENDING7**, 임의 TTS 생성0/WAV 교체0. 초반 NPC7 실제 Voice 재생은 파일 제작·승인·연결 후 검증해야 한다.

### 연속 Runtime

실제 Main06 Field02 조사4페이지 완료→기존 Scene 설치 경계에 맞춰 Field02 재진입→Main07 시작→WheelTracks4→WoundedTraveler4→실제 PaulTrail 위치trigger→Paul첫 만남을 한 격리 Play에서 검증했다. Main07은 현재 SceneLoaded에서 시작하므로 Main06 완료 즉시 같은 Scene에서 자동 시작하는 것으로 보고하지 않는다. 그 설계/시작 로직은 범위 밖이며 변경0. PaulFirst 마지막까지 종료하면 StoryBattle가 시작되므로 전투 진입 전 Hide로 QA를 마감했다.

최종 **{s['runtime_pass']}PASS / 0FAIL**, CompileError0, ConsoleError/Warning0, Field02/runtime MissingScript0. 무입력 Paul0→1 재현0. NOT_VERIFIED: Paul002 content1, 신규TTS7의 실제음성 재생은 PENDING. 실물 키보드/패드는 가상 InputSystem 검증과 구분한다. [최종 로그](Main07_Early_Final_Results.txt), [수정전 로그](Main07_Early_Before_Results.txt), [Frame/Action/Control/페이지 Trace](Main07_Early_Runtime_Trace.jsonl), [검증 JSON](Main07_Early_Verification.json).

첫 QA 환경에서는 직접 Scene로드 전 저장위치가 비어 자동저장 ConsoleError5가 발생했다. GameSessionData.RecordLocation(Field02)로 QA 초기화만 보정해 최종 전체실행 Console0을 확인했다. 사용자 Save/Settings와 InputSettings 두 값 복원, Bootstrap EditMode/is_focused=false. 기존 baseline {s['protected_files']}파일 byte동일, 기존 게임 변경은 Main07Flow1파일뿐. Main03~06/Main07기존/Main17 Voice/meta/Catalog/244 NEEDS_LISTENING/Tools 기존 audit JSON 보호. 원문 부록은 신규7 제목ID만 갱신.

Stable ID commit `c8de0ec`, 입력 경계 commit `89ff828`. QA/문서는 이 기록이 포함된 최신 Docs commit 참조. 작업 commit diff--check PASS / 기존 unrelated 변경 보호 / Push0. **전체 Voice 문제 CLOSED 아님: 신규7 제작과 Paul001 재생성·사용자승인 후 반영이 남음.**
'''
result=result.replace('PauI','Paul')
p=DOC/'Main07_Early_Dialogue_Voice_QA.md';p.write_text(p.read_text(encoding='utf-8')+result,encoding='utf-8')
notice='''## 2026-10-07 Main07 초반 Voice / Paul 감사

NPC7 Stable ID 부여 완료, TTS_REQUIRED7/Catalog pending. 사용자 첫 자막 유지 확인 및 Runtime page0 유지/001 PCM 정합으로 Paul001 WAV 내용 오류 TTS_REGEN_REQUIRED1 분리(Voice 교체0). Main07 custom E/A 동일프레임 닫힘→재열림 재현·수정, 기존 공용 fix 유지. [상세 Main07 QA](Main07_Early_Dialogue_Voice_QA.md), [TTS7 handoff](Main07_Early_TTS_Handoff.csv). 기존 Voice/244 NEEDS_LISTENING 보호. 아래는 이전 감사 이력이다.

'''
for name in ['Story_Voice_Main01_12_QA.md','Story_Dialogue_Consistency_QA.md']:
    p=DOC/name;p.write_text(notice+p.read_text(encoding='utf-8-sig'),encoding='utf-8')
p=DOC/'CURRENT_STATUS.md';old=p.read_text(encoding='utf-8-sig')
latest=f'''## 2026-10-07 Main07 초반 ID·입력 경계 수정 / TTS 대기

- NPC7 Stable ID 추가, **TTS_REQUIRED7** handoff/manifest 준비. 사용자 첫 자막 유지 오발화 확인으로 **Paul001 TTS_REGEN_REQUIRED1** 분리. 기존WAV 변경0; 전체 Voice CLOSED 아님.
- 실제 Main06완료→Field02재진입/Main07시작→흔적4→부상자4→Paul0/1 연속 {s['runtime_pass']}PASS/FAIL0. 무입력 {s['no_input_wait_seconds']:.2f}초page0 유지/002재생0, 명시Next1회page1. custom E/A 같은프레임 재열림 수정. Compile/ConsoleError0·MissingScript0.
- 사용자 확인/다음작업: TTS 담당이 신규7 및 Paul001 재생성 후 전체 청취·승인하고 별도 반영. Paul002 본문청취 미검증1 유지. 기존Voice/244/Main17/사용자Save 보호. [상세QA](Main07_Early_Dialogue_Voice_QA.md).
- 마지막 관련 commit `c8de0ec`(ID7), `89ff828`(Main07입력경계). QA/문서 최신 Docs commit 참조, GitHub Push0.

'''
p.write_text(latest+old,encoding='utf-8')
print('PASS Main07 current/story/final documents updated')
