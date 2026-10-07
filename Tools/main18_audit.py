# -*- coding: utf-8 -*-
"""납품 PNG를 수정하지 않고 크기·알파·프레임·복사본 일치를 재검사합니다."""
from pathlib import Path
import hashlib, json, re
from PIL import Image

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / '문서/00_프로젝트'
SRC = ROOT / 'Temp/Main18Audit'
ART = ROOT / 'Unity/Client/Assets/_Project/Resources/Main18'
sha = lambda p: hashlib.sha256(p.read_bytes()).hexdigest()
audit = json.loads((OUT/'Main18_Art_Source_Audit.json').read_text(encoding='utf-8-sig'))
seen = {}
manifest = (SRC/'Manifest.txt').read_text(encoding='utf-8-sig')
for row in audit['Entries']:
    name = row['File']
    if not name.endswith('.png'): continue
    im = Image.open(SRC/name); im.load(); alpha = im.getchannel('A')
    expected = (1256,1256) if name.startswith('Monsters/') else (128,128) if name.startswith('UI/') else (512,512)
    same = sha(ART/name) == row['SHA256']
    row.update(ImportByteIdentical=same,ManifestListed=name in manifest,ExpectedSize=list(expected),DuplicateOf=seen.get(row['SHA256']),AlphaBoundaryOpaquePixels=sum(1 for x in range(im.width) if alpha.getpixel((x,0))>0 or alpha.getpixel((x,im.height-1))>0),AutomaticCropApplied=False)
    seen[row['SHA256']] = name
    row['Gate'] = 'PASS_WITH_NOTE' if im.size==expected and im.mode=='RGBA' and same else 'FAIL_BLOCKING'
    row['VisualGate'] = 'USER_ART_REVIEW_REQUIRED'
    row['VisualNotes'] = '원본에 흰색 가이드 형태 선이 보입니다. 허락 없이 제거하지 않습니다.' if any(x in name for x in ['Cliff','Fissure','HeatVent','Slab','Spire','Cooling']) else '투명도·테두리·축소 시 가독성은 사용자 최종 검토가 필요합니다.'
    if name.startswith('Monsters/'):
        frames=[]
        for n in range(16):
            x=n%4*314; y=n//4*314; cell=alpha.crop((x,y,x+314,y+314)); box=cell.getbbox()
            frames.append({'Frame':n,'Size':[314,314],'AlphaBounds':box,'Empty':box is None,'TouchesCellEdge':bool(box and (box[0]==0 or box[1]==0 or box[2]==314 or box[3]==314))})
        row['Frames']=frames
        if any(f['Empty'] for f in frames): row['Gate']='FAIL_BLOCKING'
audit['VisualReview']='USER_ART_REVIEW_REQUIRED'
audit['Counts']={'PNG':17,'PASS_WITH_NOTE':sum(r.get('Gate')=='PASS_WITH_NOTE' for r in audit['Entries']),'FAIL_BLOCKING':sum(r.get('Gate')=='FAIL_BLOCKING' for r in audit['Entries']),'USER_ART_REVIEW_REQUIRED':17}
(OUT/'Main18_Art_Source_Audit.json').write_text(json.dumps(audit,ensure_ascii=False,indent=2),encoding='utf-8')
# 최초 baseline은 갱신하지 않습니다. 기존 수동 변경도 해시로 보호합니다.
baseline=json.loads((SRC/'baseline.json').read_text(encoding='utf-8-sig'))
changed=[]; missing=[]
for name,digest in baseline['Files'].items():
    p=ROOT/name
    if not p.exists(): missing.append(name)
    elif sha(p)!=digest: changed.append(name)
result={'BaselineCount':len(baseline['Files']),'Unchanged':len(baseline['Files'])-len(changed)-len(missing),'Changed':changed,'Missing':missing}
(OUT/'Main18_Protected_Files_Audit.json').write_text(json.dumps(result,ensure_ascii=False,indent=2),encoding='utf-8')
# 설계 문서의 동일한 첫 주석만 정리합니다.
for p in (ROOT/'문서').rglob('*.md'):
    t=p.read_text(encoding='utf-8-sig')
    if t.startswith('> 2026-10-08 Main18 후속 확정:'):
        line=t.split('\n\n',1)[0]+'\n\n'
        while t.startswith(line+line): t=t[len(line):]
        p.write_text(t,encoding='utf-8')
print(json.dumps(audit['Counts'])); print(json.dumps(result,ensure_ascii=False))
