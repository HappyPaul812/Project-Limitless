"""Phase3 비실행 파일 검사. 안전성 거부와 계약/권리 HOLD를 구분합니다."""
import hashlib
import io
import json
import stat
import struct
import warnings
import zipfile
import zlib
from pathlib import PurePosixPath

from PIL import Image
from .core import Denied
from .intake import safe_name,MAX_ZIP,MAX_TOTAL,MAX_MEMBER,MAX_FILES,PROTECTED

ALLOWED={'.png','.json','.md','.txt','.csv'}
MAX_TOTAL_PIXELS=67_108_864


def strict_json(data):
    if len(data)>1024*1024:raise ValueError('size')
    def pairs(items):
        result={}
        for k,v in items:
            if k in result:raise ValueError('duplicate')
            result[k]=v
        return result
    return json.loads(data,object_pairs_hook=pairs,parse_constant=lambda _:(_ for _ in ()).throw(ValueError('constant')))


def png(data):
    if not data.startswith(b'\x89PNG\r\n\x1a\n'):raise Denied('PNG_SIGNATURE_INVALID')
    at=8;types=[]
    while at<len(data):
        if at+12>len(data):raise Denied('PNG_TRUNCATED')
        length=struct.unpack('>I',data[at:at+4])[0];kind=data[at+4:at+8];end=at+12+length
        if end>len(data) or length>MAX_MEMBER:raise Denied('PNG_CHUNK_INVALID')
        if zlib.crc32(data[at+4:at+8+length])&0xffffffff!=struct.unpack('>I',data[at+8+length:end])[0]:raise Denied('PNG_CRC_INVALID')
        types.append(kind);at=end
        if kind==b'IEND':
            if length or at!=len(data):raise Denied('PNG_TRAILING_PAYLOAD')
            break
    if not types or types[0]!=b'IHDR' or types[-1]!=b'IEND' or b'acTL' in types:raise Denied('PNG_STRUCTURE_OR_APNG_INVALID')
    with warnings.catch_warnings():
        warnings.simplefilter('error',Image.DecompressionBombWarning)
        try:
            with Image.open(io.BytesIO(data)) as im:
                if im.width*im.height>16_777_216:raise Denied('PNG_PIXEL_LIMIT')
                im.verify()
            with Image.open(io.BytesIO(data)) as im:
                im.load();alpha=im.getchannel('A') if im.mode=='RGBA' else None
                return {'dimensions':[im.width,im.height],'mode':im.mode,'alpha_range':list(alpha.getextrema()) if alpha else None,
                        'bbox':list(alpha.getbbox()) if alpha and alpha.getbbox() else None}
        except (OSError,ValueError,Image.DecompressionBombError,Image.DecompressionBombWarning):raise Denied('PNG_DECODE_FAILED') from None


def archive(data):
    if not data.startswith(b'PK\x03\x04'):raise Denied('ZIP_SIGNATURE_INVALID')
    files={};names=set()
    try:
        with zipfile.ZipFile(io.BytesIO(data)) as z:
            entries=z.infolist()
            if len(entries)>MAX_FILES or sum(e.file_size for e in entries)>MAX_TOTAL:raise Denied('ZIP_EXPANSION_LIMIT')
            for entry in entries:
                # Windows zipfile이 역슬래시를 정규화하기 전의 원래 이름을 검사합니다.
                path=safe_name(entry.orig_filename.rstrip('/'));key=str(path).casefold();mode=entry.external_attr>>16
                if any(part.casefold() in PROTECTED for part in path.parts):raise Denied('ZIP_PROTECTED_PATH')
                if key in names:raise Denied('ZIP_DUPLICATE_PATH')
                names.add(key)
                if stat.S_ISLNK(mode) or stat.S_IFMT(mode) not in (0,stat.S_IFREG,stat.S_IFDIR):raise Denied('ZIP_LINK_OR_SPECIAL_FILE')
                if entry.flag_bits&1:raise Denied('ZIP_ENCRYPTED')
                if entry.is_dir():continue
                if path.suffix.lower() not in ALLOWED or entry.file_size>MAX_MEMBER or entry.file_size/max(entry.compress_size,1)>200:raise Denied('ZIP_FORBIDDEN_MEMBER_OR_RATIO')
                value=z.read(entry)
                if value.startswith((b'MZ',b'\x7fELF',b'#!')):raise Denied('DISGUISED_EXECUTABLE')
                files[str(path)]=value
    except (zipfile.BadZipFile,RuntimeError,NotImplementedError,EOFError):raise Denied('ZIP_CRC_OR_FORMAT_INVALID') from None
    return files


def inspect(name,data,metadata,catalog,base_sha):
    path=safe_name(name)
    if len(path.parts)!=1 or path.suffix.lower() not in ('.png','.zip'):raise Denied('DELIVERY_NAME_OR_TYPE_INVALID')
    if not 0<len(data)<=MAX_ZIP:raise Denied('DELIVERY_SIZE_LIMIT')
    is_zip=path.suffix.lower()=='.zip'
    files=archive(data) if is_zip else {name:data}
    manifest=None;hold=[]
    manifest_bytes=files.get('Manifest.json') if is_zip else metadata
    if manifest_bytes:
        try:
            manifest=strict_json(manifest_bytes)
            if not isinstance(manifest,dict):raise ValueError()
        except (ValueError,TypeError,RecursionError,UnicodeError):hold.append('MANIFEST_INVALID');manifest={}
    else:hold.append('MANIFEST_MISSING');manifest={}
    envelope={}
    if is_zip and metadata:
        try:
            envelope=strict_json(metadata)
            if not isinstance(envelope,dict):raise ValueError()
        except (ValueError,TypeError,RecursionError,UnicodeError):hold.append('DELIVERY_CONTEXT_INVALID');envelope={}
    context={k:manifest.get(k,envelope.get(k)) for k in ('artist_role','issue','base_sha')}
    role=context['artist_role'];issue=context['issue']
    if role not in ('NewArtAgent','RenewalArtAgent'):hold.append('ARTIST_ROLE_UNKNOWN');role='UNKNOWN'
    if type(issue) is not int or not 0<issue<1000000:hold.append('ISSUE_UNKNOWN');issue=None
    if context['base_sha']!=base_sha:hold.append('BASE_SHA_UNKNOWN_OR_MISMATCH')
    if (issue==59 and role!='NewArtAgent') or (issue==63 and role!='RenewalArtAgent'):hold.append('ROLE_ISSUE_CONFLICT')
    for k in context:
        if k in manifest and k in envelope and manifest[k]!=envelope[k]:hold.append('CONTEXT_CONFLICT')
    contracts=manifest.get('files');declared={}
    if not isinstance(contracts,list) or not contracts:hold.append('MANIFEST_FILES_MISSING');contracts=[]
    if len(contracts)>MAX_FILES:raise Denied('MANIFEST_FILE_COUNT_LIMIT')
    for c in contracts:
        if not isinstance(c,dict):hold.append('MANIFEST_CONTRACT_INVALID');continue
        target=c.get('path',c.get('file'))
        try:target=str(safe_name(target))
        except Denied:raise Denied('MANIFEST_PATH_INVALID') from None
        if target in declared or target=='Manifest.json':raise Denied('MANIFEST_DUPLICATE_PATH')
        if target not in files:raise Denied('MANIFEST_FILE_MISSING')
        actual=hashlib.sha256(files[target]).hexdigest()
        if c.get('sha256')!=actual:raise Denied('MANIFEST_SHA_MISMATCH')
        declared[target]=c
    if set(files)-{'Manifest.json'}!=set(declared):hold.append('MANIFEST_UNLISTED_FILES')
    report={'status':'HOLD','integrity':'TECH_PASS','format':'ZIP' if is_zip else 'PNG','zip_crc':'PASS' if is_zip else 'NOT_APPLICABLE',
            'artist_role':role,'issue':issue,'base_sha':base_sha,'art':'USER_REVIEW_PENDING','unity':'NOT_VERIFIED',
            'rights':'RIGHTS_SOURCE_PENDING','hold_reasons':hold,'png':{},'files':{},'collisions':[]}
    total_pixels=0
    for filename,value in files.items():
        digest=hashlib.sha256(value).hexdigest();report['files'][filename]=digest
        if filename.lower().endswith('.png'):
            info=png(value);c=declared.get(filename,{})
            total_pixels+=info['dimensions'][0]*info['dimensions'][1]
            if total_pixels>MAX_TOTAL_PIXELS:raise Denied('PNG_TOTAL_PIXEL_LIMIT')
            if info['mode']!='RGBA':hold.append('PNG_NOT_RGBA')
            if info['alpha_range'] is None or info['alpha_range'][0]!=0 or info['alpha_range'][1]==0:hold.append('PNG_TRANSPARENCY_OR_CONTENT_HOLD')
            if c.get('dimensions')!=info['dimensions']:hold.append('SPRITE_DIMENSIONS_UNKNOWN_OR_MISMATCH')
            frame=c.get('frame_size');count=c.get('frames')
            valid=(isinstance(frame,list) and len(frame)==2 and all(type(n) is int and n>0 for n in frame) and type(count) is int and count>0)
            if valid:
                w,h=info['dimensions'];valid=w%frame[0]==0 and h%frame[1]==0 and count<=(w//frame[0])*(h//frame[1])
            if not valid:hold.append('FRAME_CONTRACT_UNKNOWN_OR_MISMATCH')
            info.update(frame_size=frame if valid else None,frames=count if valid else None,frame_layout='PASS' if valid else 'HOLD')
            report['png'][filename]=info
            if PurePosixPath(filename).name.casefold() in catalog.get('names',[]) or digest in catalog.get('sha256',[]):report['collisions'].append(filename)
    if not report['png']:hold.append('NO_SPRITE_PNG')
    if not catalog.get('names') or not catalog.get('sha256'):hold.append('PROTECTED_CATALOG_UNAVAILABLE')
    if report['collisions']:hold.append('PROTECTED_ASSET_COLLISION')
    evidence=manifest.get('license_evidence')
    if isinstance(evidence,str) and evidence in files and files[evidence] and isinstance(manifest.get('source'),str) and manifest['source'].strip() and manifest.get('commercial_rights') is True:
        report['rights']='RIGHTS_HUMAN_REVIEW_PENDING'
    else:hold.append('RIGHTS_OR_SOURCE_UNCLEAR')
    claimed=manifest.get('approval_status',manifest.get('status',''))
    if not isinstance(claimed,str) or 'APPROVED' in claimed.upper():hold.append('UNTRUSTED_APPROVAL_CLAIM')
    hold.extend(['ART_APPROVAL_PENDING','RIGHTS_HUMAN_REVIEW_REQUIRED'])
    report['hold_reasons']=sorted(set(hold));return report
