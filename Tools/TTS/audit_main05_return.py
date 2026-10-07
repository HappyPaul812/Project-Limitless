"""Main05 연결·본문과 보호 baseline을 기록한다. 음성 의미 PASS를 추정하지 않는다."""
import csv, hashlib, json, re, subprocess, wave
from pathlib import Path
from story_dialogue_consistency_audit import authored
ROOT=Path(__file__).resolve().parents[2]
DOC=ROOT/'문서/00_프로젝트'; TEMP=ROOT/'Temp/Main05Return'; TEMP.mkdir(parents=True,exist_ok=True)
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
baseline={}
for folder in ['Unity/Client/Assets','Unity/Client/Packages','Unity/Client/ProjectSettings','Unity/Client/UserData']:
    for p in (ROOT/folder).rglob('*'):
        if p.is_file():baseline[p.relative_to(ROOT).as_posix()]=sha(p)
(TEMP/'baseline.json').write_text(json.dumps(baseline,ensure_ascii=False),encoding='utf-8')
(TEMP/'git-status-start.txt').write_bytes(subprocess.check_output(['git','status','--porcelain=v1','-z']))
data=[r for r in authored() if r['main']==5]
print('Main05 authored',len(data),'keys',list(data[0]))
(TEMP/'authored.json').write_text(json.dumps(data,ensure_ascii=False,indent=2),encoding='utf-8')
catalog=(ROOT/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset').read_text(encoding='utf-8-sig')
manifest={r['clip_id']:r for r in csv.DictReader((ROOT/'Limitless_TTS_Output_Missing_Main01_05_08_12_v2/dialogue_manifest_missing_main01_05_08_12_v2.csv').open(encoding='utf-8-sig',newline=''))}
all_pcm={}
for p in (ROOT/'Unity/Client/Assets/_Project/Audio/Voice').rglob('*.wav'):
    with wave.open(str(p)) as w:all_pcm[p.stem]=hashlib.sha256(w.readframes(w.getnframes())).hexdigest()
rows=[]
for r in data:
    id=r['dialogue_id']
    if not id:continue
    p=ROOT/('Unity/Client/Assets/_Project/Audio/Voice/Story/Main05/'+id+'.wav')
    guid=re.search(r'guid: (\w+)',Path(str(p)+'.meta').read_text()).group(1)
    entry=re.search(r'id: '+id+r'\s+clip: \{fileID: 8300000, guid: (\w+), type: 3\}\s+speakerId: (\S+)',catalog)
    with wave.open(str(p)) as w:duration=w.getnframes()/w.getframerate()
    m=manifest[id]
    assert m['text'].replace('\r\n','\n')==r['raw_text'].replace('\r\n','\n'),id
    assert m['speaker']==r['raw_name'],id
    rows.append(dict(stable_id=id,speaker=r['raw_name'],display_text=r['raw_text'],expected_voice_text=m.get('text',m.get('exact_text','')),asset_path=p.relative_to(ROOT/'Unity/Client').as_posix(),asset_name=p.name,resolver_key=id+'|'+entry[2],catalog_guid=entry[1],asset_guid=guid,mapping_status='PASS' if entry[1]==guid else 'WRONG_MAPPING',runtime_resolve='NOT_VERIFIED',semantic_status='NOT_VERIFIED;USER_REPORTED_RANGE_MISMATCH',duration=duration,sha256=sha(p),pcm_sha256=all_pcm[id],duplicate_pcm=';'.join(k for k,v in all_pcm.items() if k!=id and v==all_pcm[id])))
with (DOC/'Main05_Voice_Matrix.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
print('Matrix6:',[(r['stable_id'],r['mapping_status'],r['duplicate_pcm']) for r in rows])
