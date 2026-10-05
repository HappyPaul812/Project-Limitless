"""LOCAL Story 대사와 제작/Import 근거를 전수 대조합니다. WAV/게임 데이터는 수정하지 않습니다."""
import argparse
import collections
import csv
import hashlib
import json
import pathlib
import re
import wave
from audit_story_voice import ROOT, LITERAL, arguments, calls, decode

OUT = ROOT / '문서/00_프로젝트'
SCRIPTS = ROOT / 'Unity/Client/Assets/_Project/Scripts'
NAMES = {'태온':'companion_taeon','미엘':'companion_miel','폴':'companion_paul',
         '세린':'companion_serin','레온':'arbel-leon','플레이어':'player',
         '주민 대표':'starter-village-main-guide','남문 경비병':'starter-village-gate-guard'}
SCENES = {1:'World_StarterVillage',2:'Field_01',3:'Field_01',4:'Field_01',5:'World_StarterVillage',
          6:'Field_02',7:'Field_02',8:'Field_02/Field_03',9:'Field_03',10:'Field_03/Dungeon_01',
          11:'Dungeon_01/Dungeon_01_B2/Field_03',12:'Field_03/World_StarterVillage',
          13:'Field_04/World_ArbelVillage',14:'World_ArbelVillage/Field_05',15:'Field_06/World_ArbelVillage',16:'Field_07/World_ArbelVillage'}

def speaker_id(expression,name):
    literal=decode(expression)
    if literal is not None:return literal
    if expression=='string.Empty':return ''
    leaf=expression.rsplit('.',1)[-1]
    for character in ['Taeon','Miel','Paul','Serin']:
        if leaf==character+'Id':return 'companion_'+character.lower()
    return NAMES.get(name,'UNRESOLVED:'+expression)

def norm(text):
    return re.sub(r'\s+','',text.replace('\\n','\n'))

def body(text):
    # Main03의 화면용 화자 접두사는 음성 본문과 구분하되 원문 컬럼도 보존합니다.
    return re.sub(r'^(태온|플레이어)\n','',text)

def method_at(source,pos):
    matches=list(re.finditer(r'\b(?:public|private|protected)\s+(?:static\s+)?[\w\[\]<>]+\s+(\w+)\s*(?:\([^;{}]*\)|=>)',source[:pos]))
    return matches[-1][1] if matches else 'inline'

def authored():
    result=[]
    def add(main,path,source,pos,sid,name,text,did='',branch=''):
        line=source.count('\n',0,pos)+1
        result.append(dict(main=main,source=path.relative_to(ROOT).as_posix(),source_line=line,source_pos=pos,
                           branch=branch or method_at(source,pos),raw_speaker_id=sid,raw_name=name,
                           raw_text=text,dialogue_id=did or ''))
    for main in range(3,12):
        path=next((SCRIPTS/'World').glob(f'MainQuest{main:02d}*Flow.cs'))
        source=path.read_text(encoding='utf-8-sig')
        for c in calls(source):
            args,_=arguments(source,source.index('(',c['start']))
            if c['kind']=='U': sid,name='', '<PlayerName>'
            elif c['kind'] in ('new DialogueLine','Line','L'):
                name=decode(args[1]) or '<PlayerName>'; sid=speaker_id(args[0],name)
            else: name=c['speaker'];sid=NAMES[name]
            add(main,path,source,c['start'],sid,name,c['text'],c['id'])
    path=SCRIPTS/'NPC/MainQuest01NpcFlow.cs'; source=path.read_text(encoding='utf-8-sig')
    for m in re.finditer(r'string\[\]\s+(RepresentativeDialogue|GuardDialogue)\s*=\s*\{(.*?)\}',source,re.S):
        name='주민 대표' if m[1].startswith('Representative') else '남문 경비병'
        for text in re.finditer(LITERAL,m[2]): add(1,path,source,m.start(2)+text.start(),NAMES[name],name,decode(text[0]),branch=m[1])
    path=SCRIPTS/'World/MainQuest02FieldFlow.cs';source=path.read_text(encoding='utf-8-sig')
    for m in re.finditer(r'case\s+('+LITERAL+r'):\s*return\s+('+LITERAL+r')|default:\s*return\s+('+LITERAL+r')',source):
        add(2,path,source,m.start(),'','조사',decode(m[2] or m[3]),branch=decode(m[1]) if m[1] else 'default')
    m=re.search(r'CommonConclusion\s*=\s*('+LITERAL+r')',source)
    add(2,path,source,m.start(),'','조사',decode(m[1]),branch='all_paths/common_conclusion')
    for filename in ['Chapter2IntroFlow.cs','Chapter2Main15Flow.cs']:
        path=SCRIPTS/'World'/filename;source=path.read_text(encoding='utf-8-sig')
        for c in calls(source):
            args,_=arguments(source,source.index('(',c['start']));name=decode(args[1]) or '<PlayerName>'
            did=c['id'] or ''; main=int(re.match(r'main(\d+)',did)[1]) if did else 12
            sid=speaker_id(args[0],name)
            if name=='잡화 상인':sid='starter-village-general-store'
            add(main,path,source,c['start'],sid,name,c['text'],did)
        if filename=='Chapter2IntroFlow.cs':
            for m in re.finditer(r'case\s+('+LITERAL+r'):\s*return\s+('+LITERAL+r');',source):
                target=decode(m[1]);text=decode(m[2]); q=re.search(r'main(\d+)',target)
                if not q or text.startswith('main'):continue
                main=int(q[1]);sid=NAMES['태온'] if target=='field03_main12_party_decision' else NAMES['폴']
                name='태온' if sid==NAMES['태온'] else '폴'
                id_match=re.search(r'case "'+re.escape(target)+r'": return "(main[^"\n]+)";',source)
                add(main,path,source,m.start(),sid,name,text,id_match[1] if id_match else '',target)
    path=SCRIPTS/'World/Main16DialogueCatalog.cs';source=path.read_text(encoding='utf-8-sig')
    for m in re.finditer(r'\bLine\(('+LITERAL+r'),\s*('+LITERAL+r'),\s*('+LITERAL+r')\)',source):
        did,sid,text=map(decode,m.groups());name=next((n for n,s in NAMES.items() if s==sid),'현장 관찰')
        cases=list(re.finditer(r'case \"([^\"]+)\":',source[:m.start()]))
        scene=cases[-1][1] if cases else 'unknown'
        add(16,path,source,m.start(),sid,name,text,did,scene+'/'+('hearing' if '_hearing_' in did else 'default' if '_default_' in did else 'shared'))
    path=SCRIPTS/'Core/OpeningIntroSequence.cs';source=path.read_text(encoding='utf-8-sig')
    for m in re.finditer(r'new OpeningIntroSlide\(('+LITERAL+r'),[^\n]*?,\s*"(opening_\d+)"\)',source):
        add(0,path,source,m.start(),'narrator','Narrator',decode(m[1]),m[2],'Intro')
    return sorted(result,key=lambda r:(r['main'],r['source'],r['source_pos']))

def run(pre=False):
    # 중앙 처리 전 원문과 Editor가 실제 생성한 DialogueLine 값을 따로 보존합니다.
    runtime_path=ROOT/'Temp/StoryConsistency20261005/resolved-runtime.json'
    if not pre and not runtime_path.is_file():
        raise RuntimeError('먼저 격리 QA와 실제 DialogueLine export를 실행하세요. 구현 전 조사만 --pre 사용 가능.')
    runtime={r['audit_key']:r for r in json.loads(runtime_path.read_text(encoding='utf-8'))['rows']} if runtime_path.is_file() and not pre else {}
    runtime_log=ROOT/'Temp/StoryConsistency20261005/runtime.txt'
    if runtime and not runtime_log.read_text(encoding='utf-8').startswith('PASS '):
        raise RuntimeError('Post Matrix는 격리 Runtime PASS 근거가 필요합니다.')
    listening_path=OUT/'Story_Dialogue_Listening_Results.json'
    listening={r['dialogue_id']:r for r in json.loads(listening_path.read_text(encoding='utf-8'))['results']} if listening_path.is_file() else {}
    manifest=[]
    for file in sorted(ROOT.glob('Limitless_TTS_Output_*/dialogue_manifest*.csv')):
        for row in csv.DictReader(file.open(encoding='utf-8-sig')):
            manifest.append(dict(row,manifest_file=file.relative_to(ROOT).as_posix(),root=file.parent))
    opening=ROOT/'Tools/TTS/opening_narration.csv'
    for r in csv.DictReader(opening.open(encoding='utf-8-sig')):
        manifest.append(dict(clip_id=r['id'],speaker='Narrator',text=r['text'],manifest_file=opening.relative_to(ROOT).as_posix(),root=None,output_file=''))
    by_id=collections.defaultdict(list)
    for r in manifest:by_id[r['clip_id']].append(r)
    guid_map={}
    for meta in (ROOT/'Unity/Client/Assets').rglob('*.meta'):
        match=re.search(r'^guid: (\w+)',meta.read_text(encoding='utf-8-sig'),re.M)
        if match:guid_map[match[1]]=meta.with_suffix('')
    catalog=[]
    for kind in ['Story/StoryVoiceCatalog','Opening/OpeningNarrationCatalog']:
        path=ROOT/('Unity/Client/Assets/_Project/Resources/Audio/Voice/'+kind+'.asset')
        content=path.read_text(encoding='utf-8-sig')
        for m in re.finditer(r'  - id: ([^\n]+)\n    clip: \{fileID: (\d+), guid: (\w+), type: \d+\}(?:\n    speakerId:([^\n]*))?',content):
            catalog.append(dict(id=m[1].strip(),guid=m[3],speaker=(m[4] or '').strip(),path=guid_map.get(m[3]),catalog=path.relative_to(ROOT).as_posix()))
    catalogs=collections.defaultdict(list)
    for r in catalog:catalogs[r['id']].append(r)
    portraits={}
    for path in (ROOT/'Unity/Client/Assets/_Project/Resources/DialoguePortraitDefinitions').glob('*.asset'):
        content=path.read_text(encoding='utf-8-sig');sid=re.search(r'  speakerId: (.+)',content)[1]
        ref=re.search(r'  portrait: \{fileID: (\d+)(?:, guid: (\w+))?',content)
        portraits[sid]=dict(character=sid if ref[1]!='0' else 'NONE',guid=ref[2],definition=path.relative_to(ROOT).as_posix())
    docs=[*sorted((ROOT/'문서/03_스토리').glob('*.md')),ROOT/'문서/00_프로젝트/Intro_Storyteller4_적용_QA.md']
    doc_text={str(p.relative_to(ROOT)):p.read_text(encoding='utf-8-sig') for p in docs}
    rows=[];orders=collections.Counter()
    for r in authored():
        status=[];notes=[];text=body(r['raw_text'])
        semantic='player' if r['raw_text'].startswith('플레이어\n') or r['raw_name'] in ('<PlayerName>','플레이어','플레이어/동적 화자') or r['raw_speaker_id']=='player' else r['raw_speaker_id']
        key=f"M{r['main']:02d}:{r['source']}:{r['source_pos']}"
        actual=runtime.get(key,{})
        actual_sid=actual.get('raw_speaker_id',r['raw_speaker_id'])
        actual_name=actual.get('raw_name',r['raw_name'])
        actual_text=actual.get('raw_text',r['raw_text'])
        if semantic=='player' and actual_sid not in ('','player'):status.append('SPEAKER_MISMATCH');status.append('PLAYER_PORTRAIT_SHOULD_BE_NONE')
        ms=by_id.get(r['dialogue_id'],[]); cs=catalogs.get(r['dialogue_id'],[])
        if len(ms)>1 or len(cs)>1:status.append('DUPLICATE_DIALOGUE_ID')
        mr=ms[0] if ms else {}; cr=cs[0] if cs else {}; path=cr.get('path')
        if r['dialogue_id'] and semantic not in ('','player') and not cr:status.append('MISSING_AUDIO')
        if semantic=='player' and cr:status.append('PLAYER_VOICE_SHOULD_BE_NONE')
        if mr:
            if norm(mr['text'])!=norm(text):status.append('TEXT_MISMATCH')
            expected=NAMES.get(mr['speaker'],'narrator' if r['main']==0 else '')
            if expected!=semantic:status.append('SPEAKER_MISMATCH')
        if cr and r['main'] and cr['speaker']!=actual_sid:status.append('SPEAKER_MISMATCH')
        audio={}; source_same='';wav=''
        if path and path.is_file():
            wav=path.relative_to(ROOT).as_posix();data=path.read_bytes()
            with wave.open(str(path)) as w:audio=dict(seconds=w.getnframes()/w.getframerate(),sample_rate=w.getframerate(),channels=w.getnchannels(),sha256=hashlib.sha256(data).hexdigest())
            if mr.get('root'):
                # 교체 이력이 있는3개는 사용자 채택한 새 원본을 비교합니다. 이전 보충팩은 역사 자료로 보존합니다.
                replacement=listening.get(r['dialogue_id'],{}).get('replacement_source','')
                src=ROOT/replacement if replacement else mr['root']/mr['output_file']
                source_same=src.is_file() and src.read_bytes()==data
                if not source_same:status.append('WRONG_CLIP')
                if not replacement and path.name!=pathlib.Path(mr['output_file']).name:status.append('WRONG_CLIP')
            if r['dialogue_id'] in listening:
                status.extend(listening[r['dialogue_id']]['status'].split(';'))
                notes.append(listening[r['dialogue_id']]['reason'])
            else:status.append('NEEDS_LISTENING')
        elif cr:status.append('MISSING_AUDIO')
        canon=[name for name,content in doc_text.items() if norm(text) in norm(content)]
        if not canon:notes.append('DOC_TEXT_NOT_SPECIFIED: 기존 Story 문서는 요약/기획만 있고 이 문장의 정확한 원문 대조 근거 없음')
        portrait='NONE' if semantic=='player' and actual else portraits.get(actual_sid,{}).get('character','NONE')
        order_key=(r['main'],r['branch']);orders[order_key]+=1
        row=dict(r,audit_key=f"M{r['main']:02d}:{r['source']}:{r['source_pos']}",scene='OpeningIntro' if r['main']==0 else SCENES[r['main']],sequence_order=orders[order_key],
                 resolved_speaker=semantic,runtime_speaker_id=actual_sid,runtime_speaker_display_name=actual_name,runtime_subtitle_text=actual_text,spoken_text=text,heard_text=listening.get(r['dialogue_id'],{}).get('heard_text',''),listening_evidence='USER_LISTENING' if r['dialogue_id'] in listening else '',canonical_story_doc=';'.join(canon),canonical_story_doc_text=text if canon else '',
                 manifest_dialogue_id=mr.get('clip_id',''),manifest_speaker=mr.get('speaker',''),manifest_text=mr.get('text',''),manifest_file=mr.get('manifest_file',''),
                 expected_voice_character='NONE' if semantic in ('','player') or not r['dialogue_id'] else semantic,
                 catalog_audioclip=wav,catalog_guid=cr.get('guid',''),wav_file=path.name if path else '',source_hash_equal=source_same,
                 replacement_source=listening.get(r['dialogue_id'],{}).get('replacement_source',''),
                 runtime_listening_pass=listening.get(r['dialogue_id'],{}).get('runtime_listening_pass',False),
                 runtime_playback_pass=listening.get(r['dialogue_id'],{}).get('runtime_playback_pass',listening.get(r['dialogue_id'],{}).get('runtime_listening_pass',False)),
                 source_rms_db=listening.get(r['dialogue_id'],{}).get('source_rms_db',''),
                 semantic_listening_required=listening.get(r['dialogue_id'],{}).get('semantic_listening_required',False),
                 portrait_character=portrait,expected_portrait_rule='NONE' if semantic in ('','player') else semantic+' (미제작은 NONE)',
                 audio=audio,audit_result=';'.join(dict.fromkeys(status)) or 'OK',notes=';'.join(notes))
        rows.append(row)
    # 재생성은 실행하지 않습니다. 청취로 확인된 잘못된 원본만 다음 제작 세션에 전달합니다.
    with (OUT/'Story_Dialogue_TTS_REGEN_REQUIRED.csv').open('w',encoding='utf-8-sig',newline='') as f:
        columns=['dialogue_id','speaker_id','voice_id','expected_text','heard_text','wav_file','reason','evidence','action']
        writer=csv.DictWriter(f,fieldnames=columns);writer.writeheader()
        for row in rows:
            if not listening.get(row['dialogue_id'],{}).get('tts_regen_required',False):continue
            evidence=listening[row['dialogue_id']]
            writer.writerow(dict(dialogue_id=row['dialogue_id'],speaker_id=row['resolved_speaker'],voice_id='Gacrux',
                expected_text=row['spoken_text'],heard_text=evidence['heard_text'],wav_file=row['catalog_audioclip'],
                reason=evidence['reason'],evidence='USER_LISTENING 2026-10-05',
                action='새 원본 생성/사용자 원본 청취 완료; Unity Runtime 의미 검증 대기' if evidence.get('replacement_source') else '올바른 완전한 원본 제공 또는 다음 TTS 세션 재생성. 이번 생성/가공 없음'))
    used={r['dialogue_id'] for r in rows if r['dialogue_id']}
    duplicates={id:len(rs) for id,rs in catalogs.items() if len(rs)>1}
    shared={guid:ids for guid,ids in ((g,[r['id'] for r in catalog if r['guid']==g]) for g in {r['guid'] for r in catalog}) if len(ids)>1}
    summary=dict(total=len(rows),intro=sum(r['main']==0 for r in rows),main_dialogue=sum(r['main']>0 for r in rows),player=sum(r['resolved_speaker']=='player' for r in rows),
                 mapped=sum(bool(r['catalog_audioclip']) for r in rows),by_main=dict(collections.Counter(r['main'] for r in rows)),
                 by_result=dict(collections.Counter(s for r in rows for s in r['audit_result'].split(';'))),
                 catalog_total=len(catalog),duplicate_ids=duplicates,duplicate_clip_references=shared,
                 unused_catalog_ids=sorted(set(catalogs)-used),excluded_manifest_rows=[{k:v for k,v in r.items() if k!='root'} for r in manifest if r['clip_id'] not in used],
                 doc_text_not_specified=sum(not r['canonical_story_doc'] for r in rows),original_doc_exact_coverage=23,original_doc_text_not_specified=308,player_voice_mapping=sum(r['resolved_speaker']=='player' and bool(r['catalog_audioclip']) for r in rows),player_portrait_display=sum(r['resolved_speaker']=='player' and r['portrait_character']!='NONE' for r in rows),branch_collisions={id:len({(r['resolved_speaker'],r['spoken_text']) for r in rows if r['dialogue_id']==id}) for id in used if len({(r['resolved_speaker'],r['spoken_text']) for r in rows if r['dialogue_id']==id})>1},tts_regen_required=sorted(k for k,v in listening.items() if v.get('tts_regen_required',False)),portrait_definitions=portraits,
                 by_speaker={sid:dict(dialogue=sum(r['resolved_speaker']==sid for r in rows),mapped=sum(r['resolved_speaker']==sid and bool(r['catalog_audioclip']) for r in rows)) for sid in {r['resolved_speaker'] for r in rows}})
    focused_log=ROOT/'Temp/Main03Regen20261005/runtime-listening.txt'
    supp_log=ROOT/'Temp/Main03SuppRegen20261005/runtime-listening.txt'
    remaining_log=ROOT/'Temp/Main03Remaining20261005/runtime-listening.txt'
    report=dict(summary=summary,rows=rows,runtime_evidence=runtime_log.read_text(encoding='utf-8').splitlines()[0] if runtime else 'PRE_IMPLEMENTATION',
                focused_runtime_evidence=focused_log.read_text(encoding='utf-8').splitlines()[0] if focused_log.is_file() else 'NOT_RUN',
                remaining_runtime_playback_evidence=remaining_log.read_text(encoding='utf-8').splitlines()[0] if remaining_log.is_file() else 'NOT_RUN',
                supp_runtime_evidence=supp_log.read_text(encoding='utf-8').splitlines()[0] if supp_log.is_file() else 'NOT_RUN',
                source_user_listening_pass=sum(v.get('source_user_listening_pass',False) for v in listening.values()),
                runtime_user_listening_pass=sum(v.get('runtime_listening_pass',False) for v in listening.values()),
                limitations=['003/004: new source and actual Unity Runtime user semantic PASS. Original NEEDS_LISTENING244 scope preserved; resolved two are overlapping members flagged RUNTIME_LISTENING_PASS. Do not sum counts to249.','전체331개Runtime의 기존 증거와 이번 태온5페이지 집중 Runtime 증거를 구분한다.','태온3개는 별도 사용자 청취 증거와 원본 교체 이력으로 판정한다. 나머지244개 NEEDS_LISTENING은 유지한다. Metadata/PCM 일치만으로 사람 청취 PASS를 만들지 않는다.','동적 NPC 공용 fallback은 새 Quest 작성 페이지로 세지 않으며 호출점은 별도 Coverage에 기록한다.','Intro의 무음 마지막 제목은 Narrator18개의 분모에 포함하지 않는다.'])
    (OUT/'Story_Dialogue_Audit.json').write_text(json.dumps(report,ensure_ascii=False,indent=2),encoding='utf-8')
    columns=[k for k in rows[0] if k!='audio']+['wav_seconds','wav_sample_rate','wav_channels','wav_sha256']
    with (OUT/'Story_Dialogue_Audit_Matrix.csv').open('w',encoding='utf-8-sig',newline='') as f:
        writer=csv.DictWriter(f,fieldnames=columns);writer.writeheader()
        for r in rows:writer.writerow(dict({k:v for k,v in r.items() if k!='audio'},**{'wav_'+k:r['audio'].get(k,'') for k in ['seconds','sample_rate','channels','sha256']}))
    print(json.dumps(summary,ensure_ascii=False,indent=2))
    return report

if __name__=='__main__':
    parser=argparse.ArgumentParser(description=__doc__)
    parser.add_argument('--pre',action='store_true',help='중앙 처리 전 Source 값 조사')
    run(parser.parse_args().pre)
