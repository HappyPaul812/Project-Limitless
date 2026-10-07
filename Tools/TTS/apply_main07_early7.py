"""원본7을 대조하고 기존 Main07 Importer 정책과 Catalog에 신규 자산을 등록한다."""
import csv,hashlib,json,re,shutil,uuid,wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트';SOURCE=ROOT/'Limitless_TTS_Main07_Early_7';DEST=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story/Main07'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
handoff={r['stable_id']:r for r in csv.DictReader((DOC/'Main07_Early_TTS_Handoff.csv').open(encoding='utf-8-sig',newline=''))}
checkpoint=json.loads((SOURCE/'_GEN_CHECKPOINT.json').read_text(encoding='utf-8-sig'))
report={r['stable_id']:r for r in csv.DictReader((SOURCE/'REGEN_REPORT.csv').open(encoding='utf-8-sig',newline=''))}
assert {p.stem for p in SOURCE.glob('*.wav')}==set(handoff)
catalogpath=ROOT/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset';catalog=catalogpath.read_text(encoding='utf-8-sig')
template=(DEST/'main07_paul_001.wav.meta').read_text(encoding='utf-8-sig')
rows=[]
for id,h in handoff.items():
    c=checkpoint[id];src=SOURCE/(id+'.wav');dst=DEST/(id+'.wav')
    assert c['status']=='PASS' and sha(src)==c['sha'] and c['text']==h['exact_text']==report[id]['exact_text'] and h['speaker']==report[id]['speaker'] and c['voice']==h['voice_id']
    with wave.open(str(src)) as w:
        pcm=w.readframes(w.getnframes());ph=hashlib.sha256(pcm).hexdigest();assert ph==c['pcm'] and w.getsampwidth()==2
        duration=w.getnframes()/w.getframerate();rate=w.getframerate();channels=w.getnchannels()
    assert not dst.exists() and 'id: '+id+'\n' not in catalog
    guid=uuid.uuid4().hex;meta=re.sub(r'guid: \w+', 'guid: '+guid,template,count=1)
    rows.append(dict(stable_id=id,speaker=h['speaker'],speaker_id=h['speaker_id'],exact_text=h['exact_text'],source_wav=src.relative_to(ROOT).as_posix(),unity_asset=dst.relative_to(ROOT).as_posix(),guid=guid,sha256=c['sha'],pcm_sha256=ph,sample_rate=rate,channels=channels,bit_depth=16,duration=duration,automatic_audio_qa='PASS',actual_listening=report[id]['actual_full_listening'],runtime_resolve='PENDING'))
    shutil.copyfile(src,dst);Path(str(dst)+'.meta').write_text(meta,encoding='utf-8')
    catalog+='  - id: '+id+'\n    clip: {fileID: 8300000, guid: '+guid+', type: 3}\n    speakerId: '+h['speaker_id']+'\n'
assert len({r['pcm_sha256'] for r in rows})==7
catalogpath.write_text(catalog,encoding='utf-8')
with (DOC/'Main07_Early7_Applied_Mapping.csv').open('w',encoding='utf-8-sig',newline='') as f:
    w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
print('PASS applied7 new GUID7; existing references preserved; source7 untouched; listening pending7')
