"""기존 atomic 후보를 보존하고 LSA/설치기 두 파일만 교체한 미설치 후보를 만듭니다."""
import hashlib
import json
from pathlib import Path
import shutil
import zipfile


def sha(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def inventory(root):
    output = {}
    for path in sorted(root.rglob('*')):
        if path.is_symlink() or path.is_junction():
            raise ValueError('REPARSE_HOLD')
        if path.is_file():
            output[path.relative_to(root).as_posix()] = sha(path)
    return output


def prepare():
    repo = Path(__file__).resolve().parents[2]
    root = repo.parents[2]
    source = root / 'Temp/Session0TaskSecurityAtomicCandidate20261026'
    target = root / 'Temp/Session0LsaRepairCandidate20261026'
    archive = target.with_suffix('.zip')
    evidence = repo / '문서/00_프로젝트/팀운영/Evidence/Session0LsaRepair20261026'
    if target.exists() or archive.exists():
        raise ValueError('NEW_CANDIDATE_ONLY_NO_OVERWRITE')
    old = inventory(source)
    if (old['HostManifest.json'] != '6efbd7839bd4e452d9fbca8a465290533472e87cde71845a73bd37bdb21493ab'
            or sha(source.with_suffix('.zip')) != '4b2c56edf5bb8edc6ee7a40a10504f537223bc77874b43a36e61be735c318f83'):
        raise ValueError('APPROVED_BASELINE_SHA_HOLD')
    manifest = json.loads((source / 'HostManifest.json').read_text(encoding='utf-8-sig'))
    if len(old) != 856 or len(manifest['files_sha256']) != 855 or {k: v for k, v in old.items() if k != 'HostManifest.json'} != manifest['files_sha256']:
        raise ValueError('EXACT855_BASELINE_HOLD')
    expected_names = {'Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs', 'Deployment/install-session0-trial.ps1'}
    original_ready = root / 'Temp/Session0HostBundle20261026Ready'
    ready_before = inventory(original_ready)
    consumed = repo / '문서/00_프로젝트/팀운영/Evidence/Session0AtomicInstall20261026/0f8e15c951a144efa79f5ccdbeed9c55.json'
    consumed_before = sha(consumed)
    if consumed_before != '80523d2b2320cabce31789d7322a3a7c480ff8f225d42c1f0120a3bdcbea5d6b':
        raise ValueError('INSTALL1_EVIDENCE_HOLD')
    shutil.copytree(source, target)
    mapping = {'Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs': 'Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs',
               'Deployment/install-session0-trial.ps1': 'Tools/DiscordAIServiceDiagnostics/install-session0-trial.ps1'}
    for destination, local in mapping.items():
        shutil.copyfile(repo / local, target / destination)
        manifest['files_sha256'][destination] = sha(target / destination)
    (target / 'HostManifest.json').write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    new = inventory(target)
    changed = {k for k in old if k != 'HostManifest.json' and old[k] != new[k]}
    if set(old) != set(new) or changed != expected_names or manifest['execution_mode'] != 'FAKE_ONLY' or manifest['auto_start'] is not False:
        raise ValueError('EXACT_TWO_FILE_DIFF_HOLD')
    before_manifest = json.loads((source / 'HostManifest.json').read_text(encoding='utf-8-sig'))
    for name in expected_names:
        before_manifest['files_sha256'][name] = new[name]
    if before_manifest != manifest:
        raise ValueError('UNRELATED_METADATA_CHANGE_HOLD')
    with zipfile.ZipFile(archive, 'x', compression=zipfile.ZIP_DEFLATED) as output:
        for name in sorted(new):
            info = zipfile.ZipInfo(name, date_time=(2026, 10, 26, 0, 0, 0))
            info.compress_type = zipfile.ZIP_DEFLATED
            output.writestr(info, (target / name).read_bytes())
    with zipfile.ZipFile(archive) as output:
        if output.testzip() is not None or set(output.namelist()) != set(new) or any(hashlib.sha256(output.read(k)).hexdigest() != v for k, v in new.items()):
            raise ValueError('ARCHIVE_SHA_HOLD')
    if inventory(source) != old or inventory(original_ready) != ready_before or sha(consumed) != consumed_before:
        raise ValueError('PRIOR_SOURCE_OR_INSTALL1_CHANGED_HOLD')
    result = dict(status='OFFLINE_CANDIDATE_NOT_APPROVED_NOT_INSTALLED', candidate=str(target), archive=str(archive),
                  payload_files=855, unchanged_payload_files=853, added=[], removed=[],
                  changed=[dict(path=k, before=old[k], after=new[k]) for k in sorted(changed)],
                  installer_sha256=new['Deployment/install-session0-trial.ps1'],
                  policy_sha256=new['Code/Tools/DiscordAIServiceDiagnostics/TrialAccountPolicy.cs'],
                  manifest_sha256=new['HostManifest.json'], zip_sha256=sha(archive),
                  manifest_semantic_changes=sorted('files_sha256.' + k for k in changed),
                  atomic_candidate_preserved=True, ready_candidate_preserved=True, previous_install1_preserved=True,
                  installed_system_assets_written=0)
    evidence.mkdir(parents=True, exist_ok=True)
    for name, value in [('Candidate.json', result), ('BaselineInventory.json', old), ('CandidateInventory.json', new)]:
        (evidence / name).write_text(json.dumps(value, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(json.dumps(result, ensure_ascii=False))


if __name__ == '__main__':
    prepare()
