from pathlib import Path
import shutil,json,hashlib
root=Path('F:/study/codex/Project-Limitless');task=root/'Temp/LocalSync20261103';wt=task/'Worktree';q=task/'QA/Client';p=wt/'Unity/Client';tools=wt/'Tools/LocalSync86';tools.mkdir(parents=True,exist_ok=True)
if not (q/'Assets').exists():shutil.copytree(p/'Assets',q/'Assets',ignore=shutil.ignore_patterns('*.wav','*.ogg','*.mp3','*.zip','*.psd','*.blend'))
for folder in ['ProjectSettings','Packages']:
    if not (q/folder).exists():shutil.copytree(root/'Unity/Client'/folder,q/folder)
# 사용자 파일은 원본을 쓰지 않고 비파괴 백업에서 복제 QA에만 반영합니다.
a=json.loads((task/'BackupManifest.json').read_text(encoding='utf-8'))
for e in a['dirty_nonprivate']:
    name=e['path']
    if not e['exists'] or not name.startswith('Unity/Client/Assets/'):continue
    out=q/Path(name).relative_to('Unity/Client');out.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(task/'Backup/UserChanges'/name,out)
for f in (q/'Assets').rglob('*.cs'):
    if '/Editor/' in f.as_posix():f.rename(f.with_suffix('.cs.disabled'))
shutil.copy2(task/'BuildSettings_F2_StagingProposal.asset',q/'ProjectSettings/EditorBuildSettings.asset')
for name,folder in [('LocalSync86Probe.cs','Assets/QA86'),('LocalSync86Batch.cs','Assets/Editor')]:
    out=q/folder/name;out.parent.mkdir(parents=True,exist_ok=True);shutil.copy2(tools/name,out)
assert (q/'ProjectSettings/ProjectVersion.txt').read_text().strip()=='m_EditorVersion: 6000.6.5f1\nm_EditorVersionWithRevision: 6000.6.5f1 (3ff58d469c8a)'
print('QA uses integrated Assets + preserved user overlays, original Packages unchanged; only fixture adds F2 BuildSettings entry')
