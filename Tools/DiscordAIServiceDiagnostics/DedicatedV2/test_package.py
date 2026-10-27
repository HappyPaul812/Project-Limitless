"""후보/원본/SHA 검증만 수행합니다. native API와 실행 파일을 호출하지 않습니다."""
import ast,hashlib,json,sys,zipfile
from pathlib import Path
root=Path(sys.argv[1]);out=Path(sys.argv[2]);here=Path(__file__).resolve().parent
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
p=json.loads(root.with_suffix('.package.json').read_bytes());m=json.loads((root/'HostManifest.json').read_bytes());rows=[]
def check(n,v):
    rows.append(dict(name=n,status='PASS' if v else 'FAIL'))
check('PAYLOAD766_MANIFEST1',len(m['files_sha256'])==766 and len(list(root.rglob('*.*')))>0 and len([f for f in root.rglob('*') if f.is_file()])==767)
check('ALL_PAYLOAD_SHA',all(sha(root/n)==s for n,s in m['files_sha256'].items()))
check('MANIFEST_SHA',sha(root/'HostManifest.json')==p['manifest_sha256'])
check('INSTALLER_SHA',sha(root/'Deployment/install-session0-v2.ps1')==p['installer_sha256'])
check('ZIP_SHA',sha(root.with_suffix('.zip'))==p['zip_sha256'])
with zipfile.ZipFile(root.with_suffix('.zip')) as z:
    check('ZIP_EXACT767_SAME_BYTES',len(z.namelist())==767 and set(z.namelist())==set(p['file_sha256']) and all(z.read(n)==(root/n).read_bytes() for n in z.namelist()))
local=[f for f in here.iterdir() if f.suffix in ('.cs','.ps1','.py') and not f.name.startswith(('build-','test_'))]
check('SOURCE_PACKAGE_IDENTICAL',all(f.read_bytes()==(root/('Deployment' if f.name=='install-session0-v2.ps1' else 'Code/V2')/f.name).read_bytes() for f in local))
check('AUTH_PLAN_IDENTICAL',(here/'AuthenticationPlan.md').read_bytes()==(root/'Deployment/AuthenticationPlan.md').read_bytes())
for f in here.glob('*.py'):ast.parse(f.read_text(encoding='utf8'))
check('PYTHON_SYNTAX_ALL_SOURCE',True)
base=root.parent/'Session0V2Candidate20261027-r11'
for n,s in [('Deployment/install-session0-v2.ps1','eb321fc5ee0426cdd7e1c8f989f05a5e2ec55e6d1887789f0e16490598311bc2'),('HostManifest.json','e8d6ed623934a577f88a5bb99ebafc78a2a469c10485a0c648c40dd457356dc0')]:check('R11_FROZEN_'+n,sha(base/n)==s)
check('R11_FROZEN_ZIP',sha(base.with_suffix('.zip'))=='0250f736b44798d3ba73b5d9674fc154fa233bc74e3b972f85931c2aa59a41d0')
check('BORROWED741_RUNTIME_UNCHANGED',all(sha(base/n)==s and (not n.startswith('Runtime/') or sha(root/n)==s) for n,s in p['borrowed_sha256'].items()))
out.mkdir(parents=True,exist_ok=True)
for src,name in [(root/'HostManifest.json','HostManifest.json'),(root.with_suffix('.package.json'),'PackageProof.json')]: (out/name).write_bytes(src.read_bytes())
(out/'SourceSHA256.json').write_text(json.dumps({str(f.relative_to(here)):sha(f) for f in sorted(here.iterdir()) if f.is_file()},indent=2)+'\n',encoding='utf8')
result=dict(pass_count=sum(r['status']=='PASS' for r in rows),fail_count=sum(r['status']=='FAIL' for r in rows),tests=rows,actual_windows_changes=0)
(out/'PackageTests.json').write_text(json.dumps(result,indent=2)+'\n',encoding='utf8');print(json.dumps(result))
if result['fail_count']:raise AssertionError('PACKAGE_TEST_FAILURE')
