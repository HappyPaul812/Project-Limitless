"""외부 파일은 검역 데이터일 뿐 설치·Import·스크립트 실행 대상이 아닙니다."""
from __future__ import annotations

import io
import hashlib
import json
import os
import re
import stat
import tempfile
import warnings
import zipfile
from pathlib import Path, PurePosixPath

from .core import Denied, sha256, valid_sha

MAX_ZIP = 32 * 1024 * 1024
MAX_TOTAL = 128 * 1024 * 1024
MAX_MEMBER = 32 * 1024 * 1024
MAX_FILES = 256
SUFFIXES = {'.png','.json','.md','.txt','.csv','.wav','.mp3'}
PROTECTED = {'unity','userdata','saves','.git','thirdparty','assets'}


def safe_name(name):
    if not isinstance(name,str) or len(name)>240 or '\\' in name or ':' in name or '\x00' in name:
        raise Denied('ZIP 경로 오류')
    path = PurePosixPath(name)
    if path.is_absolute() or any(x in ('..','.') for x in name.split('/')):
        raise Denied('ZIP 경로 탈출')
    if not path.parts:
        raise Denied('빈 ZIP 경로')
    for part in path.parts:
        if (part.endswith((' ','.')) or re.search('[\x00-\x1f<>"|?*]',part) or
            part.split('.')[0].upper() in {'CON','PRN','AUX','NUL',*(f'COM{i}' for i in range(1,10)),*(f'LPT{i}' for i in range(1,10))}):
            raise Denied('Windows 예약·위험 파일명')
    return path


def safe_root(root):
    root = Path(root).absolute()
    if any(p.lower() in PROTECTED for p in root.parts):
        raise Denied('보호 경로에는 검역할 수 없습니다')
    for part in (root,*root.parents):
        if part.is_symlink() or (hasattr(part,'is_junction') and part.is_junction()):
            raise Denied('링크된 검역 경로 금지')
    root.mkdir(parents=True,exist_ok=True)
    return root.resolve()


def png_report(data, contract=None):
    from PIL import Image
    # 작은 압축 파일로 거대 픽셀 메모리를 할당하는 것도 거부합니다.
    with warnings.catch_warnings():
        warnings.simplefilter('error',Image.DecompressionBombWarning)
        try:
            with Image.open(io.BytesIO(data)) as im:
                if im.format != 'PNG' or im.width*im.height>16_777_216:
                    raise Denied('PNG 크기·포맷 제한')
                im.verify()
            with Image.open(io.BytesIO(data)) as im:
                im.load()
                if im.mode != 'RGBA':
                    raise Denied('RGBA PNG가 필요합니다')
                alpha = im.getchannel('A')
                if alpha.getextrema()[0] != 0 or alpha.getextrema()[1] == 0:
                    raise Denied('투명 배경 또는 유효한 그림이 없습니다')
                result = {'width':im.width,'height':im.height,'mode':im.mode,
                          'alpha_range':alpha.getextrema(),'bbox':alpha.getbbox()}
                if contract:
                    if [im.width,im.height] != contract.get('dimensions'):
                        raise Denied('PNG 해상도 계약 불일치')
                    frames = contract.get('frames',1)
                    if type(frames) is not int or frames<1:
                        raise Denied('프레임 수 계약 오류')
                    if frames>1:
                        size = contract.get('frame_size')
                        if (not isinstance(size,list) or len(size)!=2 or
                            any(type(x) is not int or x<1 for x in size) or
                            im.width%size[0] or im.height%size[1] or
                            (im.width//size[0])*(im.height//size[1]) < frames):
                            raise Denied('시트 프레임 계약 불일치')
                return result
        except (OSError,ValueError,Image.DecompressionBombError,Image.DecompressionBombWarning) as exc:
            raise Denied('PNG decode 실패') from exc


def inspect_zip(source, quarantine, expected_sha=None, official_names=()):
    source = Path(source)
    if source.is_symlink() or not source.is_file() or source.stat().st_size>MAX_ZIP:
        raise Denied('입력 파일 크기·링크 오류')
    if source.suffix.lower() != '.zip':
        raise Denied('ZIP만 검역할 수 있습니다')
    archive_bytes = source.read_bytes()
    if len(archive_bytes)>MAX_ZIP:
        raise Denied('입력 파일 크기 초과')
    digest = hashlib.sha256(archive_bytes).hexdigest()
    if expected_sha is not None and digest != valid_sha(expected_sha):
        raise Denied('제작본 SHA256 불일치')
    root = safe_root(quarantine)
    report = {'sha256':digest,'bytes':source.stat().st_size,'integrity':'NOT_VERIFIED',
              'art':'USER_REVIEW_PENDING','unity':'NOT_VERIFIED',
              'rights':'RIGHTS_SOURCE_PENDING','collisions':[], 'png':{},'files':{}}
    # 전체를 메모리 한도 안에서 검사한 다음에만 새로운 디렉터리를 만듭니다.
    buffers = {}
    try:
        with zipfile.ZipFile(io.BytesIO(archive_bytes)) as archive:
            entries = archive.infolist()
            if len(entries)>MAX_FILES or sum(x.file_size for x in entries)>MAX_TOTAL:
                raise Denied('ZIP 파일 수·확장 크기 초과')
            names = set()
            for entry in entries:
                path = safe_name(entry.filename.rstrip('/'))
                mode = entry.external_attr >> 16
                if stat.S_ISLNK(mode) or (stat.S_IFMT(mode) not in (0,stat.S_IFREG,stat.S_IFDIR)):
                    raise Denied('ZIP 링크·특수 파일 금지')
                if entry.flag_bits & 1:
                    raise Denied('암호화 ZIP 금지')
                folded = str(path).casefold()
                if folded in names:
                    raise Denied('중복·대소문자 충돌 경로')
                names.add(folded)
                if entry.is_dir():
                    continue
                if (path.suffix.lower() not in SUFFIXES or entry.file_size>MAX_MEMBER or
                    entry.file_size/max(entry.compress_size,1)>200):
                    raise Denied('실행 파일 또는 압축 확장 비율 제한')
                data = archive.read(entry)  # CRC 오류도 여기서 확인됩니다.
                if data[:2] == b'MZ' or data[:4] == b'\x7fELF' or data.startswith(b'#!'):
                    raise Denied('확장자 위장 실행 파일 금지')
                buffers[str(path)] = data
        manifest_data = buffers.get('Manifest.json')
        if not manifest_data or len(manifest_data)>1024*1024:
            raise Denied('루트 Manifest.json 필요')
        manifest = json.loads(manifest_data)
        contracts = manifest.get('files')
        if not isinstance(contracts,list) or not contracts:
            raise Denied('Manifest files 목록 필요')
        declared = {}
        for contract in contracts:
            name = str(safe_name(contract['path']))
            if name in declared or name == 'Manifest.json' or name not in buffers:
                raise Denied('Manifest 중복·누락 경로')
            actual = hashlib.sha256(buffers[name]).hexdigest()
            if actual != valid_sha(contract['sha256']):
                raise Denied('Manifest SHA 불일치')
            declared[name] = contract
            report['files'][name] = actual
        if set(buffers)-{'Manifest.json'} != set(declared):
            raise Denied('Manifest 미등재 파일 존재')
        official = {n.casefold() for n in official_names}
        for name,data in buffers.items():
            if PurePosixPath(name).name.casefold() in official:
                report['collisions'].append(name)
            if name.lower().endswith('.png'):
                report['png'][name] = png_report(data,declared[name])
        if manifest.get('source') and manifest.get('commercial_rights') and manifest.get('license_evidence') in buffers:
            report['rights'] = 'RIGHTS_HUMAN_REVIEW_PENDING'
        report['integrity'] = 'TECH_PASS'
    except (zipfile.BadZipFile,KeyError,TypeError,json.JSONDecodeError,NotImplementedError,RuntimeError) as exc:
        raise Denied('ZIP·Manifest 무결성 오류') from exc
    # hash별 디렉터리 재사용 시 덮어쓰지 않고 기존 내용을 확인합니다.
    target = root/digest
    if target.exists():
        if target.is_symlink() or not target.is_dir():
            raise Denied('검역 대상 경로 충돌')
        for name,data in buffers.items():
            file = target.joinpath(*PurePosixPath(name).parts)
            parents = [file,*list(file.parents)[:len(PurePosixPath(name).parts)]]
            if any(p.is_symlink() or (hasattr(p,'is_junction') and p.is_junction()) for p in parents):
                raise Denied('기존 검역 경로 링크 변조')
            if not file.is_file() or file.read_bytes()!=data:
                raise Denied('기존 검역 내용 변조')
        if ((target/'_bundle.zip').is_symlink() or not (target/'_bundle.zip').is_file() or
            (target/'_bundle.zip').read_bytes()!=archive_bytes):
            raise Denied('기존 검역 ZIP 변조')
    else:
        with tempfile.TemporaryDirectory(prefix='verify-',dir=root) as stage:
            staging = Path(stage)/'files'
            staging.mkdir()
            for name,data in buffers.items():
                file = staging.joinpath(*PurePosixPath(name).parts)
                file.parent.mkdir(parents=True,exist_ok=True)
                file.write_bytes(data)
            (staging/'_bundle.zip').write_bytes(archive_bytes)
            try:
                os.rename(staging,target)
            except FileExistsError:
                raise Denied('동시 검역 감지: 재조회 필요')
    report['quarantine'] = str(target)
    report['artifact'] = str(target/'_bundle.zip')
    return report


def fetch_url(*_args, **_kwargs):
    raise Denied('파일럿 URL 다운로드 비활성: 인증된 Discord 첨부 또는 공유 inbox 사용')
