"""통합 후 원본 WAV·기존 GUID·비대상 Catalog·무음 정책을 독립 대조합니다."""
import collections, hashlib, json, pathlib, re, subprocess

ROOT=pathlib.Path(__file__).resolve().parents[2]
CLIENT=ROOT/'Unity/Client'
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def parse(text):return {i:(g,s) for i,g,s in re.findall(r'  - id: (\S+)\s+clip: \{fileID: 8300000, guid: (\w+), type: 3\}\s+speakerId: (\S+)',text)}
def run():
    m=json.loads((ROOT/'Tools/TTS/tts162_integration_manifest.json').read_text(encoding='utf-8'))
    path='Unity/Client/Assets/_Project/Resources/Audio/Voice/Story/StoryVoiceCatalog.asset'
    original=parse(subprocess.check_output(['git','show','47ad2eecb40135b2ee28e4947d642ebdbf0bf68a:'+path],cwd=ROOT).decode('utf-8-sig'))
    text=(ROOT/path).read_text(encoding='utf-8-sig');current=parse(text)
    assert len(current)==len(re.findall(r'^  - id:',text,re.M)), 'DUPLICATE_OR_UNPARSED_ID'
    metas={};duplicate_voice_guids=[]
    for p in (CLIENT/'Assets/_Project/Audio/Voice').rglob('*.meta'):
        found=re.search(r'^guid: (\w+)',p.read_text(encoding='utf-8-sig'),re.M)
        if found:
            if found[1] in metas:duplicate_voice_guids.append(found[1])
            metas[found[1]]=p.with_suffix('')
    assert not duplicate_voice_guids
    ids={x['id'] for x in m['rows']};checks=[]
    for x in m['rows']:
        dst=CLIENT/x['assetPath'];guid=re.search(r'^guid: (\w+)',dst.with_suffix('.wav.meta').read_text(encoding='utf-8-sig'),re.M)[1]
        assert sha(pathlib.Path(x['source']))==sha(dst)==x['sourceSha'],x['id']+':WAV_BYTES'
        assert current[x['id']]==(guid,x['speaker']) and metas[guid]==dst,x['id']+':CATALOG'
        if x['action']=='REPLACE':
            assert guid==x['originalGuid'] and sha(dst.with_suffix('.wav.meta'))==x['originalMetaSha'],x['id']+':OLD_META_CHANGED'
        checks.append(dict(id=x['id'],speaker=x['speaker'],voice=x['voice'],guid=guid,assetPath=x['assetPath'],sourceSha=x['sourceSha'],action=x['action'],textSha=x['textSha']))
    assert all(current[i]==v for i,v in original.items() if i not in ids), 'NON_TARGET_CATALOG_CHANGED'
    assert len(current)==len(original)+sum(x['action']=='ADD' for x in m['rows'])
    assert {i for i,(g,s) in current.items() if s in ('companion_serin','companion_taeon')}=={x['id'] for x in m['rows'] if x['speaker'] in ('companion_serin','companion_taeon')}
    for x in json.loads((ROOT/'문서/03_스토리/Main21_TTS_Manifest_20261010.json').read_text(encoding='utf-8-sig')):
        assert (x['DialogueId'] in current)==x['VoiceExpected'],x['DialogueId']+':SILENT_POLICY'
    result=dict(status='PASS',count=len(checks),replaced=sum(x['action']=='REPLACE' for x in checks),added=sum(x['action']=='ADD' for x in checks),catalogBefore=len(original),catalogAfter=len(current),preservedNonTargetCatalog=len(original)-sum(x['action']=='REPLACE' for x in checks),speakers=dict(collections.Counter(x['speaker'] for x in checks)),voiceGuidCollisions=duplicate_voice_guids,rows=checks)
    out=ROOT/'문서/00_프로젝트/Evidence/TTS162Integration20261024';out.mkdir(parents=True,exist_ok=True)
    (out/'mapping_verified.json').write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print(json.dumps({k:v for k,v in result.items() if k!='rows'},ensure_ascii=False))
if __name__=='__main__':run()
