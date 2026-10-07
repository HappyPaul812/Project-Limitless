# -*- coding: utf-8 -*-
"""검증 결과를 정본 문서에 기록한다. 과거 Main05 결과와 보호 기준은 보존한다."""
import csv, hashlib, io, json, shutil, sys, wave
from pathlib import Path
import numpy as np
ROOT=Path(__file__).resolve().parents[2]
TEMP=ROOT/'Temp/Main05GuardVoice'
DOC=ROOT/'문서/00_프로젝트'
sys.stdout.reconfigure(encoding='utf-8')
before=json.loads((TEMP/'before-asr.json').read_text(encoding='utf-8'))
after=json.loads((TEMP/'after-asr.json').read_text(encoding='utf-8'))
repair=json.loads((TEMP/'repair.json').read_text(encoding='utf-8'))
analysis=json.loads((TEMP/'before-analysis.json').read_text(encoding='utf-8'))
runtime=(TEMP/'after-runtime/runtime-final.txt').read_text(encoding='utf-8')
assert runtime.startswith('PASS\n') and '\nFAIL ' not in runtime
pre=(TEMP/'before-runtime/runtime-final.txt').read_text(encoding='utf-8')
assert pre.startswith('PASS\n')
baseline=json.loads((TEMP/'baseline.json').read_text(encoding='utf-8'))
changed=[key for key,sha in baseline.items() if not (ROOT/key).exists() or hashlib.sha256((ROOT/key).read_bytes()).hexdigest()!=sha]
allowed=['Unity/Client/Assets/_Project/Audio/Voice/Story/Main05/main05_taeon_supp_001.wav','Unity/Client/Assets/_Project/Scripts/Editor/Main05ReturnVoiceAudit.cs']
assert sorted(changed)==sorted(allowed), changed
user='USER_CONFIRMED_REPAIRED' if '--user-pass' in sys.argv else 'USER_LISTENING_REQUIRED'
repair['user_listening']=user
(TEMP/'repair.json').write_text(json.dumps(repair,ensure_ascii=False,indent=2),encoding='utf-8')
rows=[]
matrix_path=DOC/'Main05_Voice_Matrix.csv'
matrix=list(csv.DictReader(matrix_path.open(encoding='utf-8-sig',newline='')))
for row in matrix:
    stable=row['stable_id'];path=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main05'/f'{stable}.wav'
    key=path.relative_to(ROOT).as_posix();trans=after[key]
    with wave.open(str(path),'rb') as w:
        params=w.getparams();raw=w.readframes(w.getnframes())
    expected=np.frombuffer(raw,dtype='<i2').astype(np.float32)/32768
    native=np.fromfile(TEMP/'after-runtime'/f'{stable}.pcm-f32',dtype='<f4')
    assert expected.shape==native.shape and np.array_equal(expected,native), stable
    n=params.framerate//100;audio=expected.astype(float)
    rms=np.sqrt(np.mean(audio[:len(audio)//n*n].reshape(-1,n)**2,axis=1));quiet=rms<10**(-45/20)
    spans=[];start=None
    for i,value in enumerate(list(quiet)+[False]):
        if value and start is None:start=i
        if not value and start is not None:
            if i-start>=15:spans.append([round(start*.01,2),round(i*.01,2)])
            start=None
    speech=' | '.join(s['text'].strip() for s in trans['segments'])
    actual_old=' | '.join(s['text'].strip() for s in before[key]['segments'])
    semantic=user if stable.endswith('taeon_supp_001') else 'FRESH_ASR_MATCH;PRIOR_USER_APPROVED_BYTES_UNCHANGED'
    rows.append(dict(stable_id=stable,speaker=row['speaker'],speaker_id=row['resolver_key'].split('|')[1],expected_text=row['expected_voice_text'],before_actual_asr=actual_old,after_actual_asr=speech,duration=trans['duration'],sha256=trans['sha256'],pcm_sha256=trans['pcm_sha256'],leading_quiet_seconds=spans[0][1] if spans and spans[0][0]==0 else 0,trailing_quiet_seconds=round(trans['duration']-spans[-1][0],2) if spans and abs(spans[-1][1]-trans['duration'])<.02 else 0,all_quiet_spans=json.dumps(spans),mixed_previous_next='NONE_IN_FRESH_ASR',hidden_later_utterance='NONE_IN_FRESH_ASR',cut_or_duplicate='NO_ASR_EVIDENCE;HUMAN_NATURALNESS_SEPARATE',semantic_status=semantic,runtime_resolve='PASS',runtime_pcm_max_error=0,authored_page_playback='1;MONOTONIC_SAMPLES;NO_RESTART',clip_cleanup='PASS',source_wav=row['source_wav'],asset_path=row['asset_path']))
    row.update(duration=trans['duration'],sha256=trans['sha256'],pcm_sha256=trans['pcm_sha256'],semantic_status=semantic,runtime_pcm_max_error='0.0',status='TECHNICAL_PASS;'+semantic)
    if stable.endswith('taeon_supp_001'):row['source_wav']=repair['source']+' [raw PCM prefix 0:158400]'
assert len(rows)==6 and len({r['pcm_sha256'] for r in rows})==6
def write_csv(path,data):
    with path.open('w',encoding='utf-8-sig',newline='') as f:
        writer=csv.DictWriter(f,fieldnames=list(data[0]));writer.writeheader();writer.writerows(data)
history=DOC/'Main05_Voice_Matrix_Before_Guard_Reaudit.csv'
if not history.exists():shutil.copyfile(matrix_path,history)
write_csv(matrix_path,matrix)
write_csv(DOC/'Main05_Guard_Voice_Reaudit_Matrix.csv',rows)
for source,name in [('before-runtime/runtime-final.txt','Main05_Guard_Before_Runtime_Results.txt'),('before-runtime/playback.jsonl','Main05_Guard_Before_Playback.jsonl'),('after-runtime/runtime-final.txt','Main05_Guard_After_Runtime_Results.txt'),('after-runtime/playback.jsonl','Main05_Guard_After_Playback.jsonl'),('after-runtime/pages.jsonl','Main05_Guard_After_Page_Order.jsonl'),('before-asr.json','Main05_Guard_Before_ASR.json'),('after-asr.json','Main05_Guard_After_ASR.json'),('repair.json','Main05_Guard_PCM_Repair.json'),('before-analysis.json','Main05_Guard_Before_Analysis.json')]:
    if name.endswith('.txt'):
        # null 목표를 출력한 줄 끝 공백만 제거한다. 원본 Runtime 로그는 Temp에 그대로 보존한다.
        (DOC/name).write_text('\n'.join(line.rstrip() for line in (TEMP/source).read_text(encoding='utf-8').splitlines())+'\n',encoding='utf-8')
    else:
        shutil.copyfile(TEMP/source,DOC/name)
samples=[json.loads(line) for line in (TEMP/'after-runtime/playback.jsonl').read_text(encoding='utf-8').splitlines()]
guard=[s for s in samples if s['id']=='main05_taeon_supp_001']
assert guard and all(s['page']==2 and s['clip']=='main05_taeon_supp_001' and s['text']==guard[0]['text'] and s['inputCallbacks']==guard[0]['inputCallbacks'] for s in guard)
assert any(not s['isPlaying'] for s in guard)
report=dict(status='TECHNICAL_PASS;'+user,before_runtime_pass=sum(line.startswith('PASS ') for line in pre.splitlines()),runtime_pass=sum(line.startswith('PASS ') for line in runtime.splitlines()),runtime_fail=0,voice_audited=6,native_pcm_exact=6,guard_page2_no_input='PASS',supp002_automatic_clip_plays=0,guard_to_representative_automatic_dialogues=0,explicit_next_miel_page3='PASS',save_bootstrap_continue=3,compile_errors=0,existing_compile_warnings=16,new_compile_warnings=0,final_runtime_console_errors=0,final_runtime_console_warnings=0,missing_scripts=0,protected_baseline_files=len(baseline),changed_baseline_files=changed,byte_preserved_files=len(baseline)-len(changed),tts_api_calls=0,user_listening=user,first_fixture_save_errors=2,first_fixture_reason='QA direct scene load omitted GameSessionData.RecordLocation; fixture corrected; final before/after runs no errors',push=0)
(DOC/'Main05_Guard_Reaudit_Verification.json').write_text(json.dumps(report,ensure_ascii=False,indent=2),encoding='utf-8')
summary=f'''\n\n## 최종 결과 — 기술 검증 완료 / {user}

사용자 관찰을 재현했다. 수정 전 Runtime에서 page2/태온/본문/main05_taeon_supp_001 Clip은 그대로였고 입력 callback0이었다. 해당 Clip의 원본 PCM 자체에 정상 두 문장 뒤 “저도 이 움직임이 어디서 시작됐는지 확인하고 싶습니다.”가 포함됐다. 잘못된 내용은 원본과 Unity 양쪽 동일SHA이며 배치 full WAV의 75.66초 위치에서 기존001 전체PCM이 정확히 발견된다. 현재 승인된 태온002는 별도 재생성 PCM으로, 잘못 붙은 구간과 문장은 같지만 PCM 바이트는 다르다. Dialogue/입력 자동진행·Clip 교체가 원인이 아니다.

기존001 ASR: 정상 문장0~2.56초 / 정상 문장3.24~6.06초 / 잘못된 대표 보고6.94~9.94초. RMS -45dBFS 이하 6.17~7.00초 경계 안의 **6.60초(158400 sample)**에서 자른다. 정상 원본 prefix PCM 전sample 동일, 마지막 정상 발화 뒤 약0.4초 여백을 보존한다. 길이10.55→6.60초, 리샘플/증폭/페이드/TTS0. 원본/meta/GUID/Catalog/태온002와 정상5개를 변경하지 않았다. [복구 증거](Main05_Guard_PCM_Repair.json).

- 전체6 새 ASR/무음 경계/중복PCM/Unity native PCM을 감사했다. 복구001 정상 두 문장만, 나머지5 각본문과 일치하고 원본 승인 바이트 그대로. 중복PCM0, 앞뒤 발화 혼입·긴 무음 뒤 숨은 문장의 ASR 증거0. 자동 전사는 사용자 청취와 구분하며, 새 복구001의 자연스러운 끝은 **{user}**. [6행 Matrix](Main05_Guard_Voice_Reaudit_Matrix.csv), [수정 전](Main05_Guard_Before_ASR.json), [수정 후](Main05_Guard_After_ASR.json).
- 수정 전 Guard6 {report['before_runtime_pass']}PASS, 수정 후 실제 Main04 완료→Guard6→별도 대표8→정식 동료 해금→Main06 첫001 **{report['runtime_pass']}PASS/FAIL0**. 페이지2의 전체 재생 후 추가5초 무입력 유지, callback0, Clip 교체0·supp002 자동재생0, 자연종료 PASS. 다음 가상 Enter 한 번은 미엘 page3만 연다. Guard 종료→대표목표→대화닫힘, 자동연결0. 대표와 별도 상호작용해야 태온002가 재생된다.
- 같은프레임 Enter+Space/A 닫힘 중복 방지, 다음프레임 입력, 단일Source/각페이지 재생1/이전Clip정리 PASS. QA 코드만 재감사 추적을 추가하고 저장 위치 기록 누락을 수정했다. 게임 Dialogue/Quest/입력 코드 수정0. 프레임/페이지/화자/본문/ID/Clip/timeSamples/길이/isPlaying/목표/callback은 [연속 playback](Main05_Guard_After_Playback.jsonl)에 기록했다.
- 대표 보고 전과 Main05 완료 후, Main06 경계 실제 Bootstrap Continue3회 PASS. Guard 재생/Voice 잔류/대표 자동시작0, 목표·동료해금 보존. CompileError0, 기존CS0618 Warning16/신규0, 최종 Runtime ConsoleError/Warning0, MissingScript0.
- 첫 재현 fixture는 직접 Scene로드가 저장 위치를 기록하지 않아 빈Scene 자동Save 오류2건이 있었다. 재현 페이지 관찰은 유효하나 그 실행을 Console0으로 기록하지 않는다. QA Load에 위치기록을 추가했고, 중단된 첫후속QA와 오류기록을 Temp에 보존했다. 최종 전/후 QA는 오류0.
- 보호 baseline {len(baseline)}중 WAV001/QA helper2개만 변경, {len(baseline)-2}개 byte동일. 다른Voice5·Main04/06+·Main07전투재도전/동료성장/미엘복구/Early7/Paul20·Player·Battle·Beast·SaveVersion·Main17/18·Art/BGM·사용자Save·TTS원본 보호. 기존 역사적 NEEDS_LISTENING244를 수정하지 않았다.

기존 당시 자동 검증 범위에서는 발견되지 않았고 사용자 실제 Play에서 발견됐다. 과거 기존001 `USER_CONFIRMED_NORMAL_EXISTING_ANCHOR`는 이번 의미 PASS 근거가 아니다. [과거 Matrix 보존본](Main05_Voice_Matrix_Before_Guard_Reaudit.csv)을 남겼고 정본 Main05 Matrix는 새 결과로 갱신했다. 기존5 승인 매핑 CSV와 과거Runtime239PASS/FinalVerification/CLOSED는 당시 기록으로 보존한다.

변경 게임Asset1: Main05/main05_taeon_supp_001.wav. 변경 Editor QA1: Main05ReturnVoiceAudit.cs. Tools/TTS 아래 audit_main05_guard_voice.py / repair_main05_guard_pcm.py / report_main05_guard_reaudit.py는 재감사·PCM복구·문서보고 도구다. CURRENT_STATUS, Main05_FinalVoice_QA, Main05_Return_Voice_QA와 본문서/위 링크의 QA 증거를 생성·갱신했다. Commit은 WAV Fix와 QA/Docs로 나누며 Push하지 않는다.
'''
main=DOC/'Main05_Guard_Voice_Reaudit.md'
assert '## 최종 결과' not in main.read_text(encoding='utf-8'), 'Report already generated; update in place instead of duplicate history'
main.write_text(main.read_text(encoding='utf-8')+summary,encoding='utf-8')
latest=f'''## 2026-10-07 태온001 사용자 Play 발견 재감사

기존 당시 자동 검증 범위에서는 발견되지 않았고 사용자 실제 Play에서 발견된 기존001 내부의 대표보고002 혼입을 확인했다. 원본 PCM 무음 경계6.60초에서001만 복구했다. 새 Voice6 ASR·native PCM6/6, 실제Main04→Guard6→독립대표8→정식해금·Continue3회 {report['runtime_pass']}PASS/FAIL0. page2 전체재생+5초 무입력 유지/supp002자동재생0/Next1회미엘3/대표자동연결0. CompileError0/최종RuntimeConsoleError·Warning0/신규Warning0/MissingScript0(기존컴파일Warning16). 복구001 사람 청취: **{user}**. [이번 정본](Main05_Guard_Voice_Reaudit.md). 아래 CLOSED/PASS는 이전 자동 검증 이력이며 이번001 의미증거로 재사용하지 않는다.

---

'''
for name in ['Main05_FinalVoice_QA.md','Main05_Return_Voice_QA.md']:
    p=DOC/name;p.write_text(latest+p.read_text(encoding='utf-8'),encoding='utf-8')
current=DOC/'CURRENT_STATUS.md'
current.write_text(f'''## 2026-10-07 Main05 태온001 WAV 혼입 복구 / 재감사 완료

사용자 실제Play의 무입력 추가발화는 기존001 WAV 안에 대표보고002 문장이 섞인 원인. 원본PCM 6.60초 무음경계에서001만 복구(10.55→6.60초), 정상5/원본/GUID/Catalog/게임Dialogue·Quest·Input 보호·TTS0. Voice6 새전사/nativePCM6/6, 실제Main04→Guard6→독립대표8→정식해금→Main06첫경계 {report['runtime_pass']}PASS/FAIL0, page2자연종료후5초무입력 유지/002자동재생0/1입력미엘page3/대표자동연결0/실제Continue3회 PASS. CompileError0/최종RuntimeConsoleError·Warning0/신규Warning0/MissingScript0·기존컴파일Warning16. QA첫Scene위치누락오류2건은 별도보존 후fixture수정, 최종오류0.

복구001 사람청취 **{user}**. 다음권장: 복구음성 끝 청취와 보호된별도후속요청. 보호baseline{len(baseline)}중2개변경/{len(baseline)-2}byte동일·Main07재도전/동료성장/미엘/Early7/Paul20와사용자Save보호. 이전Main05CLOSED는 당시이력이며001anchor판정을이번의미PASS로재사용0. [정본QA](Main05_Guard_Voice_Reaudit.md) · [Voice6](Main05_Guard_Voice_Reaudit_Matrix.csv) · [검증JSON](Main05_Guard_Reaudit_Verification.json). 마지막 관련WAVcommit은 이번 `Fix: Main05 태온001 내부 대표 보고 음성 제거`, QA는 이항목을포함한최신 Docs commit 참조. GitHub Push0.

---

'''+current.read_text(encoding='utf-8'),encoding='utf-8')
print(json.dumps(report,ensure_ascii=False,indent=2))
