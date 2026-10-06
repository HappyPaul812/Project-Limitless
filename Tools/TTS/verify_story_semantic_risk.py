"""C# 감사 출력과 원본 WAV를 독립 계산으로 검증한다. 데이터/음성 수정 없음."""
import csv
import hashlib
import json
import math
import re
import wave
from collections import Counter, defaultdict
from pathlib import Path

import numpy as np

ROOT = Path(__file__).resolve().parents[2]
DOC = ROOT/'문서/00_프로젝트'


def read_csv(path):
    with path.open(encoding='utf-8-sig', newline='') as f:
        reader = csv.DictReader(f)
        assert len(reader.fieldnames) == len(set(reader.fieldnames)), f'Duplicate header: {path}'
        rows = list(reader)
        assert all(None not in r for r in rows), f'CSV shape: {path}'
        return rows


def verify():
    data = json.loads((DOC/'StoryVoice_SemanticRisk_Audit.json').read_text(encoding='utf-8'))
    voices = [r for r in data['rows'] if r['voice_target']]
    audit = read_csv(DOC/'StoryVoice_SemanticRisk_Audit.csv')
    gameplay_queue = read_csv(DOC/'StoryVoice_ListeningQueue.csv')
    queue = read_csv(DOC/'StoryVoice_PriorityListeningQueue.csv')
    protected = read_csv(DOC/'StoryVoice_Protected_PASS.csv')
    assert len(audit) == len(voices) == data['voice']
    assert len(queue) == data['pending'] == data['queue_count']
    assert {r['dialogue_id'] for r in queue}=={r['dialogue_id'] for r in gameplay_queue}
    assert [int(r['main']) for r in gameplay_queue]==sorted(int(r['main']) for r in gameplay_queue)
    assert len(protected) == data['protected_count']
    assert len({r['dialogue_id'] for r in voices}) == len(voices)
    assert len({r['dialogue_id'] for r in queue}) == len(queue)
    assert all(r['user_pass']=='False' and r['known_regen']=='False' for r in queue)
    assert all(r['risk']=='LOW' and r['semantic_state']=='PROTECTED_PASS' for r in protected)
    checks, max_rms_error, max_peak_error = 0, 0, 0
    casting = {'companion_taeon':'Gacrux','companion_miel':'Sulafat','companion_paul':'Achird',
               'companion_serin':'Schedar','arbel-leon':'Orus'}
    for r in voices:
        path = ROOT/r['wav_path']
        assert hashlib.sha256(path.read_bytes()).hexdigest() == r['sha256']
        with wave.open(str(path)) as w:
            width = w.getsampwidth()
            assert width == 2, 'Independent validator currently checks signed16 PCM only'
            frames = w.readframes(w.getnframes())
            values = np.frombuffer(frames,dtype='<i2').astype(np.float64)/32768
            rms = 20*math.log10(float(np.sqrt(np.mean(values**2))))
            peak = 20*math.log10(float(np.max(np.abs(values))))
            max_rms_error=max(max_rms_error,abs(rms-r['rms_db']))
            max_peak_error=max(max_peak_error,abs(peak-r['peak_db']))
            assert abs(w.getnframes()/w.getframerate()-r['duration'])<1e-9
            assert values.size==r['sample_count']
            prefix=f"1:{w.getnchannels()}:{w.getframerate()}:{width*8}:".encode('ascii')
            assert hashlib.sha256(prefix+frames).hexdigest()==r['pcm_hash']
        assert max_rms_error<1e-8 and max_peak_error<1e-8
        guid=re.search(r'^guid: (\w+)',Path(str(path)+'.meta').read_text(encoding='utf-8-sig'),re.M)[1]
        assert guid==r['guid']==r['catalog_guid']
        assert path.stem==r['dialogue_id'], f'Catalog filename identity mismatch: {r["dialogue_id"]}'
        assert re.sub(r'\s+','',r['runtime_text'])==re.sub(r'\s+','',r['manifest_text'])
        if r['speaker'] in casting:
            assert r['voice_id']==casting[r['speaker']], f'Registry casting mismatch: {r["dialogue_id"]}'
        checks+=1
    # 이전 PASS 증거의 SHA도 대조하여 이번 시작 시점의 해시만으로 보호 판정을 만들지 않는다.
    evidence=[]
    for name in ['Main03_Supp008_012_Final_Audit.csv','Main04_FirstEncounter_Final_Source_Audit.csv',
                 'Main04_PostFirstEncounter_Final_Source_Audit.csv','Main04_Direction_Miel009_Sequence_Audit.csv']:
        evidence += read_csv(DOC/name)
    listening=json.loads((DOC/'Story_Dialogue_Listening_Results.json').read_text(encoding='utf-8'))['results']
    for r in protected:
        candidates=[e.get('source_sha256',e.get('sha256','')) for e in evidence if e.get('dialogue_id')==r['dialogue_id']]
        candidates += [e.get('replacement_sha256','') for e in listening if e['dialogue_id']==r['dialogue_id'] and e.get('runtime_listening_pass')]
        assert r['sha256'] in candidates, f'Protected historical PASS hash mismatch: {r["dialogue_id"]}'
    # Main 선택 우선도와 페이지 진행을 구분한다. Main 도중 다른 Main으로 이동하지 않고 각 Sequence 순서를 유지한다.
    seen=set(); last=None
    for r in queue:
        if r['main'] != last:
            assert r['main'] not in seen
            seen.add(r['main']);last=r['main']
    for main in seen:
        rows=[r for r in queue if r['main']==main]
        keys=[(int(r['sequence_rank']),1 if main=='16' and r['route']=='default' else 0,int(r['page']),r['route']) for r in rows]
        assert keys==sorted(keys), f'Gameplay order: {main}'
    # Source/Matrix 분모, 기존 재생성5건, 보호23건 및 미확인 분모를 확인한다.
    assert data['dialogue']==len(data['rows'])
    assert data['high']+data['medium']+data['low']==len(voices)
    assert data['pending']+data['known_regen']+data['protected_count']==len(voices)
    existing=read_csv(DOC/'Story_Dialogue_TTS_REGEN_REQUIRED.csv')
    assert {r['dialogue_id'] for r in existing}=={r['dialogue_id'] for r in voices if r['known_regen']}
    matrix=read_csv(DOC/'Story_Dialogue_Audit_Matrix.csv')
    assert len(matrix)==data['dialogue']
    assert sum('NEEDS_LISTENING' in r['audit_result'] for r in matrix)==244
    assert sum(r['runtime_listening_pass']=='True' for r in matrix)==data['protected_count']
    matrix_ids={r['dialogue_id']:r for r in matrix if r['dialogue_id']}
    for r in voices:
        current=matrix_ids[r['dialogue_id']]
        assert current['wav_sha256']==r['sha256'] and current['pcm_hash']==r['pcm_hash']
        assert current['semantic_risk']==r['risk'] and current['runtime_subtitle_text']==r['runtime_text']
    results=dict(result='PASS',independent_wav_checks=checks,protected_historical_hash_checks=len(protected),
                 max_rms_error_db=max_rms_error,max_peak_error_db=max_peak_error,
                 dialogue=data['dialogue'],voice=data['voice'],player=data['player'],
                 human_pass=data['protected_count'],known_regen=data['known_regen'],pending=data['pending'],
                 risk=dict(Counter(r['risk'] for r in voices)),queue_risk=dict(Counter(r['risk'] for r in queue)),
                 first_main=int(queue[0]['main']),queue_main_order=list(dict.fromkeys(int(r['main']) for r in queue)),
                 anomaly_counts={k:sum(r[k] for r in voices) for k in ['missing','duplicate_id','duplicate_pcm','cross_main_duplicate','cross_speaker_duplicate','low_volume','short_duration','multi_sentence_short','neighbor_outlier','manifest_drift','catalog_mismatch']},
                 by_main=[dict(main=i,dialogue=sum(r['main']==i for r in data['rows']),voice=len(m:=[r for r in voices if r['main']==i]),high=sum(r['risk']=='HIGH' for r in m),medium=sum(r['risk']=='MEDIUM' for r in m),low=sum(r['risk']=='LOW' for r in m),human_pass=sum(r['user_pass'] for r in m),runtime_pass=sum(r['runtime_pass'] for r in m),pending=sum(not r['user_pass'] and not r['known_regen'] for r in m),known_regen=sum(r['known_regen'] for r in m),missing=sum(r['missing'] for r in m),possible_partial=sum(r['multi_sentence_short'] or r['short_duration'] for r in m),duplicate_pcm=sum(r['duplicate_pcm'] for r in m),low_volume=sum(r['low_volume'] for r in m)) for i in range(17)])
    (DOC/'StoryVoice_SemanticRisk_Verification.json').write_text(json.dumps(results,ensure_ascii=False,indent=2),encoding='utf-8')
    print(json.dumps(results,ensure_ascii=False,indent=2))
    return results


if __name__=='__main__':
    verify()
