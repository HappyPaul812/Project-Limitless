"""원본을 읽기만 하여 Main22 적용 경계와 GUID 의존성을 기록합니다."""
import gzip, hashlib, json, pathlib, re, subprocess
ROOT = pathlib.Path('F:/study/codex/Project-Limitless')
HERE = pathlib.Path(__file__).resolve().parents[2]
OUT = HERE / '문서/00_프로젝트/Evidence/Main22ApplyPreparation20261021'
OUT.mkdir(parents=True, exist_ok=True)
def git(*args):
    return subprocess.check_output(['git', '-c', 'core.quotepath=false', '-C', str(ROOT), *args])
def sha(data): return hashlib.sha256(data).hexdigest()
target='be7f64a3ff48a8848471166d4aefc4969080bd8b'
base=git('rev-parse','main').decode().strip()
paths=json.loads((HERE/'문서/00_프로젝트/Evidence/Main22FirstPlayable20261021/ChangedFiles.json').read_text(encoding='utf-8-sig'))
rows=[]
for p in paths:
    candidate=git('show',target+':'+p)
    try: old=git('show',base+':'+p)
    except subprocess.CalledProcessError: old=None
    disk=(ROOT/p).read_bytes() if (ROOT/p).is_file() else None
    state='NEW_ABSENT' if old is None and disk is None else 'ALREADY_IDENTICAL' if disk==candidate else 'CLEAN_TRACKED' if disk==old else 'LOCAL_DIVERGED'
    # CRLF만 다른 경우도 raw SHA와 함께 구분하여 기록합니다.
    if state=='LOCAL_DIVERGED' and disk is not None and disk.replace(b'\r\n',b'\n')==candidate.replace(b'\r\n',b'\n'): state='ALREADY_TEXT_EQUIVALENT'
    if state=='LOCAL_DIVERGED' and disk is not None and old is not None and disk.replace(b'\r\n',b'\n')==old.replace(b'\r\n',b'\n'): state='CLEAN_TEXT_EQUIVALENT'
    rows.append(dict(path=p,state=state,base_sha256=sha(old) if old is not None else None,disk_sha256=sha(disk) if disk is not None else None,candidate_sha256=sha(candidate)))
all_delta=git('diff','--name-status',base,target).decode().splitlines()
feature_delta=git('diff-tree','--no-commit-id','--name-status','-r','1764741c').decode().splitlines()
# 후보에서 사용한 모든 GUID가 허용된 제품58경로와 기존 main만으로 풀리는지 검사합니다.
product=set(paths)
tracked=git('ls-tree','-r','--name-only',target,'Unity/Client/Assets').decode().splitlines()
guid_paths={}
for p in tracked:
    if not p.endswith('.meta'): continue
    txt=(HERE/p).read_text(encoding='utf-8-sig',errors='replace')
    m=re.search(r'^guid: ([a-f0-9]{32})',txt,re.M)
    if m: guid_paths.setdefault(m.group(1),[]).append(p[:-5])
refs=[]
package_guid='a97c105638bdf8b4a8650670310a4cd3'
package_materials=list((ROOT/'Unity/Client/Library/PackageCache').glob('com.unity.render-pipelines.universal@*/Runtime/Materials/Sprite-Lit-Default.mat.meta'))
package_match=[str(p.relative_to(ROOT)) for p in package_materials if ('guid: '+package_guid) in p.read_text(encoding='utf-8-sig')]
for p in paths:
    if p.endswith(('.png','.meta')): continue
    txt=(HERE/p).read_text(encoding='utf-8-sig',errors='replace')
    for guid in sorted(set(re.findall(r'guid: ([a-f0-9]{32})',txt))):
        matches=guid_paths.get(guid,[])
        available=[]
        for match in matches:
            exists=bool(git('ls-tree','--name-only',base,'--',match).strip())
            available.append(dict(path=match,source='PRODUCT' if match in product else 'MAIN' if exists else 'EXCLUDED_DEPENDENCY'))
        refs.append(dict(owner=p,guid=guid,resolves=available,builtin=guid.startswith('0000000000000000')))
        if guid==package_guid and package_match: refs[-1]['resolves']=[dict(path=p,source='ORIGINAL_URP_PACKAGE') for p in package_match]
result=dict(base=base,candidate=target,main_is_ancestor=subprocess.call(['git','-C',str(ROOT),'merge-base','--is-ancestor',base,target])==0,product_count=len(rows),counts={s:sum(r['state']==s for r in rows) for s in sorted(set(r['state'] for r in rows))},rows=rows,guid_references=refs,excluded_dependencies=[r for r in refs if any(x['source']=='EXCLUDED_DEPENDENCY' for x in r['resolves'])],unresolved=[r for r in refs if not r['resolves'] and not r['builtin']],duplicate_guids={g:ps for g,ps in guid_paths.items() if len(ps)>1},branch_delta=all_delta,main22_feature_delta=feature_delta)
(OUT/'StaticSafetyAudit.json').write_text(json.dumps(result,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
(OUT/'ProductFiles.tsv').write_text('state\tpath\tmain_SHA256\toriginal_disk_SHA256\tcandidate_SHA256\n'+'\n'.join('\t'.join(str(r[k] or '') for k in ['state','path','base_sha256','disk_sha256','candidate_sha256']) for r in rows)+'\n',encoding='utf-8')
pending=[r['path'] for r in rows if not r['state'].startswith('ALREADY_')]
patch=git('diff','--binary',base,target,'--',*pending)
(OUT/'Pending45_REVIEW_ONLY.patch.gz').write_bytes(gzip.compress(patch,mtime=0))
check=subprocess.run(['git','-C',str(ROOT),'apply','--check','--whitespace=nowarn','-'],input=patch,capture_output=True)
(OUT/'PatchCheck.json').write_text(json.dumps(dict(command='git apply --check --whitespace=nowarn - (stdin; no mutation)',exit_code=check.returncode,stdout=check.stdout.decode(errors='replace'),stderr=check.stderr.decode(errors='replace'),pending_count=len(pending)),ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
build='Unity/Client/ProjectSettings/EditorBuildSettings.asset'
patch44=git('diff','--binary',base,target,'--',*[p for p in pending if p!=build])
check44=subprocess.run(['git','-C',str(ROOT),'apply','--check','--whitespace=nowarn','-'],input=patch44,capture_output=True)
(OUT/'PatchCheck44.json').write_text(json.dumps(dict(command='git apply --check --whitespace=nowarn - (stdin; excludes BuildSettings; no mutation)',exit_code=check44.returncode,stdout=check44.stdout.decode(errors='replace'),stderr=check44.stderr.decode(errors='replace'),pending_count=len(pending)-1),ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
diskbuild=(ROOT/build).read_text(encoding='utf-8-sig')
guid=re.search(r'^guid: ([a-f0-9]+)',(HERE/'Unity/Client/Assets/_Project/Scenes/World_NorthF1.unity.meta').read_text(),re.M).group(1)
addition='  - enabled: 1\n    path: Assets/_Project/Scenes/World_NorthF1.unity\n    guid: '+guid+'\n'
(OUT/'EditorBuildSettings_Proposed_REVIEW_ONLY.txt').write_text(diskbuild.replace('  m_configObjects:\n',addition+'  m_configObjects:\n'),encoding='utf-8')
print(json.dumps({k:result[k] for k in ['base','candidate','main_is_ancestor','product_count','counts','excluded_dependencies','unresolved','duplicate_guids']},ensure_ascii=False,indent=2))
