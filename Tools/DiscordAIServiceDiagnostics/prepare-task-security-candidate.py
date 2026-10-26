"""원본 Ready 855개를 보존하고 설치기만 바꾼 별도 후보를 생성합니다. 시스템 API는 호출하지 않습니다."""
import hashlib
import json
from pathlib import Path
import shutil
import zipfile


def digest(path):
    return hashlib.sha256(path.read_bytes()).hexdigest()


def inventory(root):
    result = {}
    for path in sorted(root.rglob('*')):
        if path.is_symlink() or path.is_junction():
            raise ValueError('REPARSE_POINT_HOLD')
        if path.is_file():
            result[path.relative_to(root).as_posix()] = digest(path)
    return result


def prepare():
    repo = Path(__file__).resolve().parents[2]
    original_root = repo.parents[2]
    source = original_root / 'Temp/Session0HostBundle20261026Ready'
    target = original_root / 'Temp/Session0TaskSecurityAtomicCandidate20261026'
    archive_path = target.with_suffix('.zip')
    evidence = repo / '문서/00_프로젝트/팀운영/Evidence/Session0TaskSecurityAtomic20261026'
    evidence.mkdir(parents=True, exist_ok=True)
    if target.exists() or archive_path.exists():
        raise ValueError('NEW_CANDIDATE_ONLY_NO_OVERWRITE')
    old = inventory(source)
    manifest_name = 'HostManifest.json'
    installer_name = 'Deployment/install-session0-trial.ps1'
    if old[manifest_name] != '9aa4a60d6dd57c320f2d268ef96dcf86694e53ca9dfdcbb6a3e97d1f33dfe7ec':
        raise ValueError('OLD_MANIFEST_HOLD')
    if old[installer_name] != '337ad613ef6471ae6f4058043965bb3b812a0e6b31adc05ce1efb61ce0158829':
        raise ValueError('OLD_INSTALLER_HOLD')
    manifest = json.loads((source / manifest_name).read_text(encoding='utf-8-sig'))
    expected = manifest['files_sha256']
    if len(expected) != 855 or {k: v for k, v in old.items() if k != manifest_name} != expected:
        raise ValueError('EXACT_855_HOLD')
    if manifest['execution_mode'] != 'FAKE_ONLY' or manifest['auto_start'] is not False:
        raise ValueError('FAKE_DISABLED_HOLD')
    shutil.copytree(source, target)
    shutil.copyfile(repo / 'Tools/DiscordAIServiceDiagnostics/install-session0-trial.ps1', target / installer_name)
    manifest['files_sha256'][installer_name] = digest(target / installer_name)
    (target / manifest_name).write_text(json.dumps(manifest, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    new = inventory(target)
    payload = {k: v for k, v in new.items() if k != manifest_name}
    changed = [k for k in expected if old[k] != new[k]]
    old_manifest = json.loads((source / manifest_name).read_text(encoding='utf-8-sig'))
    if (len(payload) != 855 or payload != manifest['files_sha256'] or changed != [installer_name]
            or set(new) != set(old) or inventory(source) != old):
        raise ValueError('CANDIDATE_DIFF_OR_SOURCE_CHANGED_HOLD')
    old_manifest['files_sha256'][installer_name] = manifest['files_sha256'][installer_name]
    if old_manifest != manifest:
        raise ValueError('UNRELATED_MANIFEST_CHANGE_HOLD')
    # 파일 순서와 시각을 고정합니다. 인증자료를 새로 읽거나 추가하지 않습니다.
    with zipfile.ZipFile(archive_path, 'x', compression=zipfile.ZIP_DEFLATED) as archive:
        for name in sorted(new):
            info = zipfile.ZipInfo(name, date_time=(2026, 10, 26, 0, 0, 0))
            info.compress_type = zipfile.ZIP_DEFLATED
            archive.writestr(info, (target / name).read_bytes())
    with zipfile.ZipFile(archive_path) as archive:
        if set(archive.namelist()) != set(new) or archive.testzip() is not None:
            raise ValueError('ARCHIVE_HOLD')
        if any(hashlib.sha256(archive.read(k)).hexdigest() != v for k, v in new.items()):
            raise ValueError('ARCHIVE_SHA_HOLD')
    result = dict(status='OFFLINE_CANDIDATE_PASS_NOT_INSTALLED', source=str(source), candidate=str(target),
                  archive=str(archive_path), payload_files=855, unchanged_payload_files=854,
                  changed_payload_files=changed, added_files=[], removed_files=[],
                  original_manifest_sha256=old[manifest_name], manifest_sha256=new[manifest_name],
                  original_installer_sha256=old[installer_name], installer_sha256=new[installer_name],
                  archive_sha256=digest(archive_path), original_preserved=inventory(source) == old,
                  manifest_semantic_changes=['files_sha256.' + installer_name], system_changes=0)
    (evidence / 'OriginalInventory.json').write_text(json.dumps(old, indent=2) + '\n', encoding='utf-8')
    (evidence / 'CandidateInventory.json').write_text(json.dumps(new, indent=2) + '\n', encoding='utf-8')
    (evidence / 'Candidate.json').write_text(json.dumps(result, ensure_ascii=False, indent=2) + '\n', encoding='utf-8')
    print(json.dumps(result, ensure_ascii=False))


if __name__ == '__main__':
    prepare()
