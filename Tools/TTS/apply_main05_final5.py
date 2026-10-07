"""최신 사용자 청취 승인을 받은 Main05 원본 5개를 기존 GUID에 적용한다."""
import csv, hashlib, json, re, shutil, subprocess, wave
from pathlib import Path
from story_dialogue_consistency_audit import authored
ROOT=Path(__file__).resolve().parents[2]; SOURCE=ROOT/'Limitless_TTS_Regen_Main05_5'; TEMP=ROOT/'Temp/Main05Final5'; DOC=ROOT/'문서/00_프로젝트'
TEMP.mkdir(parents=True,exist_ok=True)
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
checkpoint=json.loads((SOURCE/'_PASS_CHECKPOINT.json').read_text(encoding='utf-8-sig'))
ids=[k for k in checkpoint if not k.startswith('_')]
assert set(ids)=={'main05_miel_supp_001','main05_miel_supp_002','main05_miel_supp_003','main05_taeon_supp_002','main05_taeon_supp_003'}
assert {p.stem for p in SOURCE.glob('*.wav')}==set(ids)
baseline={}
for folder in ['Unity/Client/Assets','Unity/Client/Packages','Unity/Client/ProjectSettings','Unity/Client/UserData','Limitless_TTS_Regen_Main05_5']:
    for p in (ROOT/folder).rglob('*'):
        if p.is_file():baseline[p.relative_to(ROOT).as_posix()]=sha(p)
assert not (TEMP/'baseline.json').exists(),'Do not overwrite original baseline'
(TEMP/'baseline.json').write_text(json.dumps(baseline,ensure_ascii=False),encoding='utf-8')
(TEMP/'git-status-start.txt').write_bytes(subprocess.check_output(['git','status','--porcelain=v1','-z']))
data={r['dialogue_id']:r for r in authored() if r['main']==5 and r['dialogue_id']}
catalog=(ROOT/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset').read_text(encoding='utf-8-sig')
report={r['Stable ID']:r for r in csv.DictReader((SOURCE/'REGEN_REPORT.csv').open(encoding='utf-8-sig',newline=''))}
norm=lambda s:' '.join(s.split())
rows=[]
for id in ids:
    c=checkpoint[id]; a=data[id]; src=SOURCE/(id+'.wav'); dst=ROOT/('Unity/Client/Assets/_Project/Audio/Voice/Story/Main05/'+id+'.wav')
    assert c['stable_id']==id and c['status']=='AUTO_QA_PASS' and c['sha']==sha(src)
    assert norm(c['text'])==norm(a['raw_text'])==norm(report[id]['Exact Text'])
    assert report[id]['Speaker']==a['raw_name'] and c['voice']==('Sulafat' if 'miel' in id else 'Gacrux')
    with wave.open(str(src)) as w:
        pcm=w.readframes(w.getnframes()); ph=hashlib.sha256(pcm).hexdigest()
        assert w.getsampwidth()==2 and w.getnchannels()==1 and w.getframerate()==24000 and ph==c['pcm']
        rate,channels,bits,duration=w.getframerate(),w.getnchannels(),w.getsampwidth()*8,w.getnframes()/w.getframerate()
    guid=re.search(r'guid: (\w+)',Path(str(dst)+'.meta').read_text()).group(1)
    assert re.search(r'id: '+id+r'\s+clip: \{fileID: 8300000, guid: '+guid+r', type: 3\}\s+speakerId: '+a['raw_speaker_id'],catalog)
    rows.append(dict(stable_id=id,speaker=a['raw_name'],speaker_id=a['raw_speaker_id'],text=a['raw_text'],source_wav=src.relative_to(ROOT).as_posix(),unity_asset=dst.relative_to(ROOT).as_posix(),clip_name=id,guid=guid,sample_rate=rate,channels=channels,bit_depth=bits,duration=duration,sha256=sha(src),pcm_sha256=ph,user_listening='PASS_LATEST_USER_REQUEST',automatic_qa='PASS',previous_csv_listening=report[id]['실제 청취']))
assert len({r['pcm_sha256'] for r in rows})==5
for p in (ROOT/'Unity/Client/Assets/_Project/Audio/Voice').rglob('*.wav'):
    if p.stem in ids:continue
    with wave.open(str(p)) as w:ph=hashlib.sha256(w.readframes(w.getnframes())).hexdigest()
    assert ph not in {r['pcm_sha256'] for r in rows},p
for r in rows:
    shutil.copyfile(ROOT/r['source_wav'],ROOT/r['unity_asset']);assert sha(ROOT/r['unity_asset'])==r['sha256']
with (DOC/'Main05_Final_Source_Mapping.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
print('PASS source5 / mapping5 / duplicate0 / GUID5 preserved / copied5; baseline',len(baseline))
