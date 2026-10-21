"""이번 납품의 파일/픽셀 동일성을 원본 변경 없이 독립 계산합니다."""
from pathlib import Path
import hashlib,json,zipfile
import numpy as np
from PIL import Image,ImageDraw
from scipy.ndimage import label,find_objects

ROOT=Path('F:/study/codex/Project-Limitless');BASE=ROOT/'Temp/JointVillageV3WolfV4QA20261021'
OUT=BASE/'Worktree/문서/00_프로젝트/Evidence/JointVillageV3WolfV4_20261021'
sha=lambda p:hashlib.sha256(p.read_bytes()).hexdigest()
for key,name,expected in [('A','Limitless_Chapter3_FrostwayVillage_Environment_GameArt_v3.zip','2a74677d5f62d7e13e64ea85ba62e12175be0be0e0d19864c23233f1bd84e79a'),('B','Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v4.zip','3ab0b57d2389b55ed33f73c1d3d13862b9b595245e196d1f3a4c98f090b8a412')]:
    folder=BASE/'Extracted'/key;zp=Path('F:/Downloads')/name;z=zipfile.ZipFile(zp);m=json.loads((folder/'Manifest.json').read_text(encoding='utf-8'));assets=m.get('assets',[])
    refs={x['path']:x for x in assets} if assets else m['files'];records=[]
    for p in sorted(folder.rglob('*')):
        if not p.is_file():continue
        rel=p.relative_to(folder).as_posix();r={'path':rel,'bytes':p.stat().st_size,'sha256':sha(p)}
        if p.suffix.lower() in ['.png','.gif','.apng']:
            im=Image.open(p);im.load();r.update(size=list(im.size),mode=im.mode,frames=getattr(im,'n_frames',1));r['durations_ms']=[]
            for i in range(r['frames']):im.seek(i);im.load();r['durations_ms'].append(im.info.get('duration'))
            if r['frames']==1:
                a=np.array(im.convert('RGBA'))[:,:,3];r['alpha_extrema']=[int(a.min()),int(a.max())];r['alpha_bbox']=im.convert('RGBA').getchannel('A').getbbox();r['border3_nonzero']=int((a[:3]>0).sum()+(a[-3:]>0).sum()+(a[:,:3]>0).sum()+(a[:,-3:]>0).sum())
                if key=='A' and rel in refs and not rel.startswith(('Preview/','QA/')):
                    lab,n=label(a>=32);counts=np.bincount(lab.ravel());parts=[]
                    for i,s in enumerate(find_objects(lab),1):
                        if s is not None and counts[i]>=16:parts.append({'area':int(counts[i]),'bbox':[s[1].start,s[0].start,s[1].stop,s[0].stop]})
                    r['alpha32_components_area16']=sorted(parts,key=lambda x:-x['area'])
        if rel in refs:r['manifest_sha_match']=r['sha256']==refs[rel]['sha256']
        records.append(r)
    result={'zip':name,'zip_sha256':sha(zp),'zip_bytes':zp.stat().st_size,'expected_match':sha(zp)==expected,'entries':len(z.infolist()),'crc_bad':z.testzip(),'files':records,'manifest_mismatches':[x['path'] for x in records if x.get('manifest_sha_match') is False]}
    if key=='A':
        result['game_png_count']=sum(not p.startswith(('Preview/','QA/')) for p in refs);result['tile_edges']=[]
        sheet=Image.new('RGB',(1536,1050),'#30363d');d=ImageDraw.Draw(sheet)
        for j,p in enumerate(sorted((folder/'Tiles').glob('*.png'))):
            im=Image.open(p).convert('RGBA');ar=np.array(im);result['tile_edges'].append({'path':p.name,'lr_equal':bool(np.array_equal(ar[:,0],ar[:,-1])),'tb_equal':bool(np.array_equal(ar[0],ar[-1])),'alpha_opaque':bool((ar[:,:,3]==255).all())})
            for n in [2,3]:
                c=Image.new('RGBA',(256*n,256*n))
                for x in range(n):
                    for y in range(n):c.paste(im,(x*256,y*256))
                c.save(OUT/f'A_{p.stem}_Repeat_{n}x{n}.png')
            x=(j%2)*768;y=(j//2)*520;small=c.convert('RGB').resize((750,490));sheet.paste(small,(x,y+25));d.text((x+5,y+5),p.name,fill='white')
        sheet.save(OUT/'A_Repeat_Overview.png');result['sha256sums_mismatches']=[]
        for line in (folder/'SHA256SUMS.txt').read_text().splitlines():
            h,p=line.split(None,1);p=p.strip().lstrip('*')
            if sha(folder/p)!=h:result['sha256sums_mismatches'].append(p)
    else:
        old=ROOT/'Temp/JointVillageWolfQA20261021/Extracted/B/Assets/Poses';strip=Image.open(folder/'Assets/Wolf_Run6_v4_Strip_3072x384.png').convert('RGBA');atlas=Image.open(folder/'Assets/Wolf_Run6_v4_Atlas3x2_1536x768.png').convert('RGBA');result['delta']=[]
        for i in range(6):
            p=folder/f'Assets/Poses/Wolf_Run_{i:02}_v4.png';q=old/f'Wolf_Run_{i:02}_v3.png';im=Image.open(p).convert('RGBA');a=np.array(im);b=np.array(Image.open(q).convert('RGBA'));diff=np.any(a!=b,axis=2);ys,xs=np.where(diff)
            result['delta'].append({'frame':i,'png_sha_equal':sha(p)==sha(q),'previous_sha256':sha(q),'current_sha256':sha(p),'changed_pixels':int(diff.sum()),'changed_bbox_exclusive':[int(xs.min()),int(ys.min()),int(xs.max()+1),int(ys.max()+1)] if len(xs) else None,'strip_equal':im.tobytes()==strip.crop((i*512,0,(i+1)*512,384)).tobytes(),'atlas_equal':im.tobytes()==atlas.crop(((i%3)*512,(i//3)*384,(i%3+1)*512,(i//3+1)*384)).tobytes(),'tail_roi_equal_x346_486_y88_165':bool(np.array_equal(a[88:165,346:486],b[88:165,346:486]))})
    (OUT/f'{key}_Validation.json').write_text(json.dumps(result,indent=2),encoding='utf-8');print(key,result['expected_match'],result['manifest_mismatches'],result.get('game_png_count'),result.get('delta'))
