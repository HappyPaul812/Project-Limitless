"""승인된 로컬 음성만 감사/복사합니다. API·재생성·원본 수정은 하지 않습니다."""
import argparse, collections, csv, hashlib, json, pathlib, re, shutil, struct, subprocess, wave
import audit_story_voice

ROOT = pathlib.Path(__file__).resolve().parents[2]
CLIENT = ROOT / 'Unity/Client'
DOC = ROOT / '문서/00_프로젝트'
OUT = ROOT / 'Temp/TTS162'
CAT = CLIENT / 'Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset'
MANIFEST = ROOT / 'Tools/TTS/tts162_integration_manifest.json'
KEEP = ['main21_carriage_leon_01','main21_aid_miel_02','main21_aid_paul_01','main21_aid_miel_01','main21_arbel_leon_01','main21_route_paul_02','main21_route_paul_01']
def sha(p): return hashlib.sha256(p.read_bytes()).hexdigest()
def read(p): return json.loads(p.read_text(encoding='utf-8-sig'))
def digest(text): return hashlib.sha256(text.encode('utf-8')).hexdigest()
def audit():
    OUT.mkdir(parents=True,exist_ok=True)
    renewal=read(DOC/'TTS_Renewal_Voices_152_OriginalDialogue_Manifest_20261020.json')['rows']
    main21=read(ROOT/'문서/03_스토리/Main21_TTS_Manifest_20261010.json')
    expected=list(renewal)+[dict(id=x['DialogueId'],speaker=x['SpeakerId'],text=x['Text'],chapter=21) for x in main21 if x['DialogueId'] in KEEP or x['SpeakerId']=='story_haren']
    assert len(expected)==162 and len({x['id'] for x in expected})==162
    catrows=re.findall(r'  - id: (\S+)\s+clip: \{fileID: 8300000, guid: (\w+), type: 3\}\s+speakerId: (\S+)',CAT.read_text(encoding='utf-8-sig'))
    assert len(catrows)==len({x[0] for x in catrows})
    current={i:(g,s) for i,g,s in catrows};guids={};errors=[];rows=[];baseline_collisions=[]
    for p in (CLIENT/'Assets').rglob('*.meta'):
        m=re.search(r'^guid: (\w+)',p.read_text(encoding='utf-8-sig'),re.M)
        if m:
            if m[1] in guids:
                # 중복을 숨기지 않습니다. 이번 음성 GUID이면 무조건 차단하고, 그 밖에는
                # main 원본 두 meta와 바이트 정규화 대조가 되는 기존 결함만 별도 보존합니다.
                pair=[guids[m[1]].with_suffix(guids[m[1]].suffix+'.meta'),p]
                proof=[]
                for item in pair:
                    rel=item.relative_to(ROOT).as_posix()
                    old=subprocess.check_output(['git','show','HEAD:'+rel]).decode('utf-8-sig').replace('\r\n','\n')
                    proof.append(dict(path=rel,unchanged=old==item.read_text(encoding='utf-8-sig').replace('\r\n','\n')))
                baseline_collisions.append(dict(guid=m[1],proof=proof))
                if m[1] in {x[1] for x in catrows} or not all(x['unchanged'] for x in proof):errors.append('GUID_COLLISION:'+m[1])
            guids[m[1]]=p.with_suffix('')
    runtime_path=ROOT/'Temp/TTS162Validation/Temp/TTS162/runtime_dialogues.json'
    runtime=read(runtime_path)['rows'] if runtime_path.is_file() else []
    if not runtime:errors.append('ACTUAL_UNITY_RUNTIME_EXPORT_REQUIRED')
    # 배열을 반환하지 않고 Presenter에 직접 전달하는 구 챕터도 존재합니다.
    # 접두사 허용 대신 실제 constructor의 네 인수(ID/화자/본문 포함)를 정확히 파싱합니다.
    sources={p:p.read_text(encoding='utf-8-sig') for p in (CLIENT/'Assets/_Project/Scripts').rglob('*.cs') if 'Editor' not in p.parts}
    symbols=collections.defaultdict(set)
    for source in sources.values():
        for name,value in re.findall(r'const\s+string\s+(\w+)\s*=\s*("(?:\\.|[^"\\])*")',source):symbols[name].add(json.loads(value))
    constructors=[]
    for path,source in sources.items():
        for found in re.finditer(r'\bnew\s+DialogueLine\s*\(',source):
            args,end=audit_story_voice.arguments(source,found.end()-1)
            if len(args)!=4:continue
            text=audit_story_voice.decode(args[2]);ident=audit_story_voice.decode(args[3]);speaker=audit_story_voice.decode(args[0])
            if speaker is None:
                choices=symbols.get(args[0].split('.')[-1],set())
                if len(choices)==1:speaker=next(iter(choices))
            if text is not None and ident is not None and speaker is not None:constructors.append(dict(id=ident,speaker=speaker,text=text,source=path.relative_to(ROOT).as_posix()))
    attempts=[json.loads(s) for s in pathlib.Path('F:/Downloads/Voice/Limitless_TTS_Main20_Main21_20261019/attempts.jsonl').read_text(encoding='utf-8-sig').splitlines() if s.strip()]
    for x in expected:
        i,s,t=x['id'],x['speaker'],x['text'];voice={'companion_serin':'Erinome','companion_taeon':'Algieba','story_haren':'Alnilam','arbel-leon':'Orus','companion_miel':'Sulafat','companion_paul':'Achird'}[s]
        folder=pathlib.Path('F:/Downloads/Renewal_Voices')/('Serin' if s=='companion_serin' else 'Taeon') if s in ('companion_serin','companion_taeon') else pathlib.Path('F:/Downloads/Voice/Haren_Main21/Haren') if s=='story_haren' else pathlib.Path('F:/Downloads/Voice/Limitless_TTS_Main20_Main21_20261019/candidates')
        wav=folder/(i+'.wav');meta=folder/(i+'.json')
        if not wav.is_file():errors.append('MISSING_WAV:'+i);continue
        h=sha(wav);metadata='MANIFEST_AND_GENERATION_LOG'
        if meta.is_file():
            m=read(meta);metadata='SIDECAR_SHA_VERIFIED'
            if any(m.get(k)!=v for k,v in dict(dialogue_id=i,speaker_id=s,text=t,voice=voice,wav_sha256=h,text_sha256=digest(t)).items()): errors.append('SIDECAR_MISMATCH:'+i)
        else:
            matching=[a for a in attempts if a.get('DialogueId')==i and a.get('TextSha256')==digest(t) and a.get('VoiceId')==voice]
            if not matching:errors.append('KEEP_GENERATION_LOG_MISMATCH:'+i)
        # 정적 ID 추정 대신 Unity가 실제 생성한 ID/화자/본문을 모두 대조합니다.
        candidates=[v for v in runtime if v['id']==i and v['speaker']==s]
        valid=[v for v in candidates if v['text']==t or (i.startswith('main03_taeon_supp_') and v['text']=='태온\n'+t)]
        exact=[v for v in constructors if v['id']==i and v['speaker']==s and (v['text']==t or (i.startswith('main03_taeon_supp_') and v['text']=='태온\n'+t))]
        if not valid and not exact:errors.append('EXACT_RUNTIME_OR_CONSTRUCTOR_MISMATCH:'+i)
        mapping_proof='LIVE_UNITY_GENERATED' if valid else 'COMPILED_SOURCE_EXACT_CONSTRUCTOR' if exact else 'MISSING'
        with wave.open(str(wav),'rb') as w:
            data=w.readframes(w.getnframes());rate=w.getframerate();channels=w.getnchannels();width=w.getsampwidth();frames=w.getnframes()
            if not frames or len(data)!=frames*channels*width or w.getcomptype()!='NONE' or width!=2 or channels!=1 or rate!=24000:errors.append('PCM_MISMATCH:'+i)
            samples=struct.unpack('<'+'h'*(len(data)//2),data);peak=max(abs(v) for v in samples);clipped=sum(abs(v)>=32760 for v in samples)/len(samples)
            if peak==0:errors.append('SILENT:'+i)
        if i in current:
            guid,old_speaker=current[i]
            if old_speaker!=s or guid not in guids:errors.append('EXISTING_REFERENCE_MISMATCH:'+i);continue
            dst=guids[guid];action='REPLACE';old=sha(dst);oldmeta=sha(dst.with_suffix('.wav.meta'))
        else:
            dst=CLIENT/f'Assets/_Project/Audio/Voice/Story/Main{x["chapter"]:02d}/{i}.wav';action='ADD';guid='';old='';oldmeta=''
            if dst.exists():errors.append('UNREGISTERED_PATH_COLLISION:'+i)
        rows.append(dict(id=i,speaker=s,text=t,voice=voice,source=str(wav),sourceSha=h,textSha=digest(t),assetPath=dst.relative_to(CLIENT).as_posix(),action=action,originalGuid=guid,originalSha=old,originalMetaSha=oldmeta,seconds=frames/rate,rate=rate,channels=channels,bits=width*8,peak=peak,clippedFraction=clipped,metadata=metadata,mappingProof=mapping_proof,listening='USER_APPROVED_HAREN_R2_1' if i=='main21_haren_haren_01' else 'USER_INTEGRATION_AUTHORIZED_NOT_INDEPENDENT_LISTENING'))
    approved=read(pathlib.Path('F:/Downloads/Voice/Haren_Main21/HAREN_01_APPROVED.json'))
    if approved['wav_sha256']!='d6188257ac6d72a86336c89c211e6e3701dec6dedaa1d3516ffccd3db61cb3c2' or approved['wav_sha256']!=sha(pathlib.Path('F:/Downloads/Voice/Haren_Main21/Haren/main21_haren_haren_01.wav')) or approved['wav_sha256']!=sha(pathlib.Path('F:/Downloads/Voice/Haren_Main21/ProsodyRetakes/R2/Haren/main21_haren_haren_01.wav')):errors.append('HAREN_APPROVAL_MISMATCH')
    targeted={x['id'] for x in expected}
    if any(s in ('companion_serin','companion_taeon') and i not in targeted for i,g,s in catrows):errors.append('UNCOVERED_SERIN_TAEON_CATALOG')
    for x in main21:
        if x['VoiceExpected'] and x['DialogueId'] not in targeted and x['DialogueId'] not in current:errors.append('MAIN21_MISSING:'+x['DialogueId'])
    with (DOC/'Main20_TTS_Manifest.csv').open(encoding='utf-8-sig',newline='') as f:
        for x in csv.DictReader(f):
            if x['VoiceExpected'].lower()=='true' and (x['DialogueId'] not in targeted or next(y for y in expected if y['id']==x['DialogueId'])['text']!=x['Text'].replace('\r\n','\n')):errors.append('MAIN20_MISMATCH:'+x['DialogueId'])
    result=dict(status='PASS' if not errors else 'BLOCKED',errors=errors,catalogBefore=len(catrows),preexistingNonVoiceGuidCollisions=baseline_collisions,rows=rows)
    MANIFEST.write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps(dict(status=result['status'],rows=len(rows),actions=dict(collections.Counter(x['action'] for x in rows)),speakers=dict(collections.Counter(x['speaker'] for x in rows)),errors=errors),ensure_ascii=False))
    return result
def stage(result):
    assert result['status']=='PASS'
    for x in result['rows']:
        src=pathlib.Path(x['source']);dst=CLIENT/x['assetPath'];assert sha(src)==x['sourceSha']
        if x['action']=='REPLACE':
            backup=OUT/'originals'/x['assetPath'];backup.parent.mkdir(parents=True,exist_ok=True)
            if not backup.exists():shutil.copyfile(dst,backup);shutil.copyfile(dst.with_suffix('.wav.meta'),backup.with_suffix('.wav.meta'))
            assert sha(backup)==x['originalSha']
        dst.parent.mkdir(parents=True,exist_ok=True);shutil.copyfile(src,dst);assert sha(dst)==x['sourceSha']
        if x['action']=='REPLACE':assert sha(dst.with_suffix('.wav.meta'))==x['originalMetaSha']
    print('STAGED_162_WAV; CATALOG_NOT_YET_CHANGED; ORIGINAL_META_PRESERVED')
if __name__=='__main__':
    parser=argparse.ArgumentParser();parser.add_argument('--stage',action='store_true');args=parser.parse_args();result=audit()
    if args.stage:stage(result)
    if result['status']!='PASS':raise SystemExit(2)
