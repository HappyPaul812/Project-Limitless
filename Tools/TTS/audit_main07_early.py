"""Main07 보충 TTS handoff와 기존 Paul 원본/Catalog 감사를 생성한다. 음성은 생성하지 않는다."""
import csv,hashlib,json,re,wave
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2];DOC=ROOT/'문서/00_프로젝트'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
def save(p,rows):
    with p.open('w',encoding='utf-8-sig',newline='') as f:
        w=csv.DictWriter(f,fieldnames=rows[0]);w.writeheader();w.writerows(rows)
data=[('WheelTracks','main07_taeon_supp_001','태온','수레가 지나간 흔적은 아닌 것 같습니다.'),('WheelTracks','main07_taeon_supp_002','태온','폭이 일정하고… 한쪽이 계속 더 깊게 눌려 있어요.'),('WheelTracks','main07_miel_supp_001','미엘','누군가 이쪽으로 지나간 것 같네요.'),('WoundedTraveler','main07_miel_supp_002','미엘','이분을 그냥 두고 갈 수는 없어요. 제가 상태를 볼게요.'),('WoundedTraveler','main07_taeon_supp_003','태온','혼자 괜찮겠습니까?'),('WoundedTraveler','main07_miel_supp_003','미엘','네. 두 분은 흔적을 확인해주세요.'),('WoundedTraveler','main07_miel_supp_004','미엘','상황이 안 좋으면 바로 돌아오시고요.')]
rows=[dict(order=i+1,sequence=s,stable_id=id,speaker=n,speaker_id='companion_taeon' if n=='태온' else 'companion_miel',exact_text=t,voice_id='Gacrux' if n=='태온' else 'Sulafat',output_file=id+'.wav',unity_asset='Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/'+id+'.wav',status='TTS_REQUIRED',catalog_status='PENDING',source_file='Unity/Client/Assets/_Project/Scripts/World/MainQuest07FieldFlow.cs') for i,(s,id,n,t) in enumerate(data)]
save(DOC/'Main07_Early_TTS_Handoff.csv',rows)
catalog=(ROOT/'Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset').read_text(encoding='utf-8-sig')
manifest={r['clip_id']:r for r in csv.DictReader((ROOT/'Limitless_TTS_Output_PreSerin/dialogue_manifest_main01_12.csv').open(encoding='utf-8-sig',newline=''))}
clips=[]
for id in ['main07_paul_001','main07_paul_002']:
    p=ROOT/('Unity/Client/Assets/_Project/Audio/Voice/Story/Main07/'+id+'.wav');m=manifest[id];src=ROOT/'Limitless_TTS_Output_PreSerin'/m['output_file']
    guid=re.search(r'guid: (\w+)',Path(str(p)+'.meta').read_text()).group(1)
    assert re.search(r'id: '+id+r'\s+clip: \{fileID: 8300000, guid: '+guid+r', type: 3\}\s+speakerId: companion_paul',catalog)
    assert sha(p)==sha(src)
    with wave.open(str(p)) as w:
        raw=w.readframes(w.getnframes());duration=w.getnframes()/w.getframerate();rate=w.getframerate();channels=w.getnchannels();bits=w.getsampwidth()*8
    clips.append(dict(stable_id=id,speaker='폴',expected_text=m['text'],source_wav=src.relative_to(ROOT).as_posix(),unity_asset=p.relative_to(ROOT).as_posix(),guid=guid,sha256=sha(p),source_sha256=sha(src),pcm_sha256=hashlib.sha256(raw).hexdigest(),duration=duration,sample_rate=rate,channels=channels,bit_depth=bits,catalog_mapping='PASS_REFERENCE_ONLY',runtime_resolve='PENDING',semantic_status='USER_PAGE0_WRONG_CONTENT_REPORTED' if id.endswith('001') else 'NOT_VERIFIED',runtime_pcm_error='PENDING'))
save(DOC/'Main07_Paul_Clip_Matrix.csv',clips)
save(DOC/'Main07_Paul_TTS_Regen_Required.csv',[dict(stable_id='main07_paul_001',speaker='폴',exact_text=manifest['main07_paul_001']['text'],status='TTS_REGEN_REQUIRED',evidence='User: page0 first subtitle unchanged while second sentence spoken; inspect runtime reference/PCM separately. No replacement performed.')])
print('TTS_REQUIRED7; existing Paul reference/source hash2 PASS; semantic not inferred')
