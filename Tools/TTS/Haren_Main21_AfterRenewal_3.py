#!/usr/bin/env python3
# LIMITLESS: Haren 3 official Main21 lines, ONLY AFTER Serin67/Taeon85 READY.
# Reuses existing PT quotas/ledger and 429 quarantine; no keys in repo or logs.
import argparse
import csv
import hashlib
import importlib.util
import json
import os
import re
import sys
import time
from pathlib import Path

VOICE_HOME = Path('F:/Downloads/Voice')
RENEWAL_HOME = Path('F:/Downloads/Renewal_Voices')
BATCH_HOME = VOICE_HOME / 'Haren_Main21'
HAREN_MANIFEST_PATH = BATCH_HOME / 'HAREN_MAIN21_3_FROZEN.json'
SOURCE_SCRIPT = RENEWAL_HOME / 'Renewal_Auto_TTS_Failover_v2.py'
RUN_SESSION_LOCK = BATCH_HOME / 'HAREN_BATCH_RUNNING.lock'
CANON = (
 {'id':'main21_haren_haren_01','speaker':'story_haren','text':'태온. 너 아직 멀었어.','chapter':'Main21','source':'Main21_TTS_Manifest_20261010.json'},
 {'id':'main21_haren_haren_02','speaker':'story_haren','text':'다친 데는 없고?','chapter':'Main21','source':'Main21_TTS_Manifest_20261010.json'},
 {'id':'main21_haren_haren_03','speaker':'story_haren','text':'그럼 됐어.','chapter':'Main21','source':'Main21_TTS_Manifest_20261010.json'},
)
PROTECTED_MAIN21 = (
 'main21_carriage_leon_01.wav','main21_aid_miel_02.wav','main21_aid_paul_01.wav',
 'main21_aid_miel_01.wav','main21_arbel_leon_01.wav','main21_route_paul_02.wav',
 'main21_route_paul_01.wav',
)


def digest(data):return hashlib.sha256(data).hexdigest()


def load_program(name):
    if not SOURCE_SCRIPT.is_file():
        raise RuntimeError('Existing v2 engine missing in Renewal_Voices; do not reinstall or guess.')
    spec=importlib.util.spec_from_file_location(name, SOURCE_SCRIPT)
    mod=importlib.util.module_from_spec(spec)
    spec.loader.exec_module(mod)
    return mod


def get_4_keys_in_original_order():
    # A project budget and key_slot are shared between batches; reusing existing
    # account mapping is safe ONLY when the secret API keys are identical.
    def read_keys(path):
        if not path.is_file():
            raise RuntimeError('Missing private API.txt. Must exist BOTH under Voice and Renewal_Voices; no key values displayed.')
        parts=[p.strip() for p in re.split(r'[;\r\n]+',path.read_text(encoding='utf-8-sig')) if p.strip()]
        if len(parts)!=4 or len(set(parts))!=4:
            raise RuntimeError('Expected exactly four DISTINCT keys in both API.txt files. Stop; do not guess quotas.')
        return parts
    source=read_keys(RENEWAL_HOME/'API.txt')
    candidate=read_keys(VOICE_HOME/'API.txt')
    if set(source)!=set(candidate):
        raise RuntimeError('Voice/API.txt does not match the four Renewal_Voices keys. STOP: separate quota/project mapping audit needed.')
    # Key order in Voice/API.txt can differ; use original key_slot ordering for ledger safety.
    return source


def assert_renewal_ready(base,require_complete):
    # Full content+hash validation of the 152 frozen entries, not just stale counters.
    data=base.get_manifest();rows=data['rows']
    if len(rows)!=152:raise RuntimeError('Expected 152 frozen renewal IDs, STOP')
    done=0
    for row in rows:
        if base.completed(row):done+=1
    if require_complete and done!=152:
        raise RuntimeError(f'Renewal batch is only {done}/152. Finish Taeon first. Haren API POST 0.')
    return done


def guard_frozen_manifest():
    expected={'version':'Main21-Haren3-20261024','source':'GitHub main / Main21_TTS_Manifest_20261010.json',
              'rows':list(CANON),'voice':'Alnilam','user_audio_qa':'PENDING'}
    if HAREN_MANIFEST_PATH.exists():
        actual=json.loads(HAREN_MANIFEST_PATH.read_text(encoding='utf-8-sig'))
        if actual!=expected:raise RuntimeError('Frozen Haren manifest differs; STOP, do not overwrite')
    return expected


def make_engine():
    base=load_program('limitless_tts_original_152')
    engine=load_program('limitless_tts_haren_v2')
    engine.ROOT=BATCH_HOME
    engine.MANIFEST=HAREN_MANIFEST_PATH
    # Shared actual quota ledger, HOLD/lock and project settings avoid double-counting.
    engine.SETTINGS=RENEWAL_HOME/'RENEWAL_SETTINGS.json'
    engine.JOURNAL=RENEWAL_HOME/'_progress'/'api_events.jsonl'
    engine.HOLD=RENEWAL_HOME/'_progress'/'HOLD.json'
    engine.LOCK=RENEWAL_HOME/'_progress'/'RUNNING.lock'
    engine.CHARACTERS={'story_haren':('Haren','Alnilam')}
    engine.STYLE={'Haren':('Mature Korean man, low and measured, reserved, caring elder brother. '
                           'Not sarcastic. Speak verbatim Korean. Natural diction and unhurried pacing.')}
    engine.SAMPLE_PATHS={}  # Old first Haren sample has WRONG rising intonation: NOT approved.
    engine.get_manifest=lambda: {'rows':list(CANON)}
    engine.key_list=get_4_keys_in_original_order
    def haren_payload(row):
        tone={
          'main21_haren_haren_01':('This is a firm DECLARATIVE statement, NOT a question. '
                                  'In Korean say exactly: "태온. 너 아직 멀었어." '
                                  'End "멀었어." with distinctly FALLING intonation. No rising pitch. '),
          'main21_haren_haren_02':('This is a natural caring QUESTION, with appropriate questioning tone. '
                                  'Say exactly: "다친 데는 없고?" '),
          'main21_haren_haren_03':('This is a short calm DECLARATIVE statement, NOT a question. '
                                  'Say exactly: "그럼 됐어." End with falling intonation. ')
        }[row['id']]
        body={'contents':[{'role':'user','parts':[{'text':row['text'],
                   'speech_metadata':{'style':tone+engine.STYLE['Haren']}}]}],
              'generationConfig':{'responseModalities':['AUDIO'],
                   'speechConfig':{'voiceConfig':{'voice':'Alnilam'}}}}
        return json.dumps(body,ensure_ascii=False,separators=(',',':')).encode('utf-8')
    engine.payload=haren_payload
    return base,engine


def done_count(engine):return sum(engine.completed(row) for row in CANON)


def show(base,engine):
    ready=assert_renewal_ready(base,False)
    current=done_count(engine)
    day,tz,calls,success,kc,gc=engine.count_usage()
    config=engine.settings()
    print(f'[RENEWAL READY] {ready}/152 (must reach 152 before Haren run)')
    print(f'[HAREN READY] {current}/3 | Alnilam | location {BATCH_HOME}')
    print(f'[PT USAGE SHARED] {day} {tz} attempts={len(calls)}/{config["daily_request_budget"]} keys={kc} groups={gc}')
    print(f'[HOLD] {engine.HOLD.exists() or engine.LOCK.exists()} | [429 GROUPS] {sorted(engine.blocked_groups_today())}')
    print(f'[PROJECT LABELS] {config["project_groups"]}; must represent VERIFIED real independent projects')
    print(f'[PROTECTED OLD7] {sum((VOICE_HOME / "Limitless_TTS_Main20_Main21_20261019" / "candidates" / x).is_file() for x in PROTECTED_MAIN21)}/7 user-reported Main21 candidate WAVs present (not touched)')


def save_status_csv(engine):
    folder=BATCH_HOME/'Haren'
    folder.mkdir(parents=True,exist_ok=True)
    report=folder/'dialogues.csv'
    # This is a status file; do not overwrite any WAV or JSON metadata.
    with report.open('w',encoding='utf-8-sig',newline='') as f:
        writer=csv.writer(f);writer.writerow(['DialogueId','SpeakerId','Voice','Text','Status','WAV'])
        for r in CANON:
            writer.writerow([r['id'],r['speaker'],'Alnilam',r['text'],
                             'READY_QA_PENDING' if engine.completed(r) else 'PENDING',r['id']+'.wav'])


def run(max_calls=3):
    base,engine=make_engine()
    assert_renewal_ready(base,True)
    if engine.LOCK.exists() or engine.HOLD.exists():
        raise RuntimeError('Old TTS HOLD / RUNNING.lock exists. Do NOT remove, stop.')
    if RUN_SESSION_LOCK.exists():
        raise RuntimeError('Haren batch session lock exists. Verify processes and logs, no manual deletion.')
    old=base.settings()
    if not old.get('free_tier_confirmed') or not old.get('project_groups_confirmed'):
        raise RuntimeError('Google quota/billing/project map not confirmed in existing settings')
    if len(set(old['project_groups']))>1 and not old.get('verified_independent_projects_for_failover'):
        raise RuntimeError('Distinct project labels not yet verified; stop')
    keys=engine.key_list()  # Ensures Voice API matches Renewal keys without displaying secrets.
    print('[SAFETY] Using shared Renewal_Voices event ledger, same PT total 40 and group caps; Voice API keys matched.')
    print('[SAFETY] Do not run any other TTS program concurrently; same Project/Model external usage must be verified.')
    show(base,engine)
    if done_count(engine)==3:
        print('[COMPLETE] Haren 3/3 already ready; NO API POST. User listening QA pending.');return
    answer=input('After reviewing FREE quota, project membership, and billing, type RUN_HAREN3 to synthesize at most 3 remaining dialogue WAV: ').strip()
    if answer!='RUN_HAREN3':print('[NOT AUTHORIZED] 0 API POST');return
    BATCH_HOME.mkdir(parents=True,exist_ok=True)
    frozen=guard_frozen_manifest()
    if not HAREN_MANIFEST_PATH.exists():
        with HAREN_MANIFEST_PATH.open('x',encoding='utf-8') as f:
            json.dump(frozen,f,ensure_ascii=False,indent=2)
            f.flush();os.fsync(f.fileno())
    # Acquire process lock safely. Do not automatically recover stale lock.
    with RUN_SESSION_LOCK.open('x',encoding='utf-8') as f:
        f.write(json.dumps({'pid':os.getpid(),'task':'Haren 3 post-renewal','pt_date':engine.pt_now()[0].date().isoformat()}))
        f.flush();os.fsync(f.fileno())
    safe_to_release=False
    try:
        posted=0
        while done_count(engine)<3 and posted<max_calls:
            if engine.LOCK.exists() or engine.HOLD.exists():raise RuntimeError('Prior API attempt uncertain / HOLD found; stop')
            date,tz,calls,success,kc,gc=engine.count_usage()
            if len(calls)>=old['daily_request_budget']:
                print('[DAILY CAP] Shared PT budget exhausted.');safe_to_release=True;break
            idx=engine.choose_key(keys,kc,gc,old,engine.blocked_groups_today())
            if idx is None:
                print('[PROJECT CAP] No verified eligible independent group remains today.');safe_to_release=True;break
            row=next(r for r in CANON if not engine.completed(r))
            engine.model_check(keys[idx])
            ok=engine.request(keys[idx],row,idx+1,old['project_groups'][idx],old)
            posted+=1
            if ok:print('[HAREN CREATED]',row['id'],'= user listening QA PENDING')
            else:print('[429] Project quarantined; alternate group only if remaining quota verified')
            save_status_csv(engine)
            if done_count(engine)<3 and posted<max_calls:time.sleep(old['sleep_seconds_between_calls'])
        safe_to_release=not(engine.LOCK.exists() or engine.HOLD.exists())
    finally:
        # If any ambiguous in-flight call occurred, preserve the session lock for human audit.
        if safe_to_release and not engine.LOCK.exists() and not engine.HOLD.exists():
            RUN_SESSION_LOCK.unlink()
        else:
            print('[SESSION LOCK PRESERVED] Outcome uncertain; do not delete without audit.')
    show(base,engine)
    if done_count(engine)==3:print('[HAREN FINISHED] 3/3 WAV prepared. Hearing/rising-vs-falling QA still needed, Unity remains unchanged.')


def main():
    p=argparse.ArgumentParser(description='LIMITLESS Haren Main21 ONLY 3 WAV after Serin67/Taeon85 152 completed')
    p.add_argument('action',choices=['check','run'])
    p.add_argument('--max-calls',type=int,default=3)
    a=p.parse_args()
    try:
        if a.action=='check':
            base,engine=make_engine();guard_frozen_manifest();get_4_keys_in_original_order();show(base,engine)
            print('[PREFLIGHT] 0 Google POST; checks only. Previous Haren rising-intonation sample is NOT reused.')
        else:
            if a.max_calls not in (1,2,3):raise RuntimeError('--max-calls 1,2,or3 only')
            run(a.max_calls)
        return 0
    except KeyboardInterrupt:
        print('[USER INTERRUPTED] Check shared RUNNING.lock and Haren batch lock; preserve all evidence.');return 130
    except Exception as ex:
        print('[STOP]',str(ex),file=sys.stderr);return 1

if __name__=='__main__':sys.exit(main())
