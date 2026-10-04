# -*- coding: utf-8 -*-
"""Cleanup v2의 지정 셀만 정수 이동/잔여 성분 제거하고 원본과 픽셀 차이를 기록합니다.

게임 Asset 적용은 별도 단계입니다. 생성/보간/리사이즈/방향 반전을 하지 않습니다.
"""
import hashlib
import io
import json
import subprocess
import zipfile
from pathlib import Path

import numpy as np
from PIL import Image, ImageDraw
from scipy.ndimage import label, find_objects

ROOT = Path(__file__).resolve().parents[1]
OUT = ROOT / 'Temp/PlayerSpriteCellCleanup'
SOURCE = Path('F:/Downloads/Limitless_IntellectualVision_8_Cleanup_v2.zip')
SOURCE_HASH = '95d2299df6b09e69165bc31a51045fad691901be1df58b97aad2edde3e737822'
# 이 목록 바깥의 셀은 변경을 허용하지 않습니다.
TARGETS = {
    'Intellectual_Fighter_Female.png': list(range(8, 16)),
    'Intellectual_Mage_Female.png': list(range(12, 16)),
    'Intellectual_Mage_Male.png': list(range(12, 16)),
    'Vision_Fighter_Male.png': [1, 2, 3, 5, 7, 9, 11, 12, 13, 14],
    'Vision_Guardian_Male.png': [5, 6] + list(range(8, 16)),
    'Vision_Mage_Female.png': list(range(12, 16)),
    'Vision_Sharpshooter_Female.png': [5, 6],
    'Vision_Sharpshooter_Male.png': [5, 6],
}
# 머리/무기/망토 방향을 유지하는 최소 정수 이동 후보입니다.
# 반대쪽에 본체 crop이 생기면 후보를 버리고 해당 셀은 조각 제거만 수행합니다.
MOVES = {
    'Intellectual_Fighter_Female.png': {**{f: (0, 2) for f in range(8, 12)}, **{f: (0, 1) for f in range(12, 16)}},
    'Intellectual_Mage_Female.png': {f: (0, 1) for f in range(12, 16)},
    'Intellectual_Mage_Male.png': {f: (0, 2) for f in range(12, 16)},
    'Vision_Fighter_Male.png': {1: (-2, 0), 3: (2, 0), 5: (2, 0), 7: (2, 0), 9: (-2, 0), 11: (2, 0), 12: (0, 2), 13: (-2, 2), 14: (0, 2)},
    'Vision_Guardian_Male.png': {5: (-2, 0), **{f: (0, 2) for f in range(8, 12)}, **{f: (0, 1) for f in range(12, 16)}},
    'Vision_Mage_Female.png': {f: (0, 2) for f in range(12, 16)},
    'Vision_Sharpshooter_Female.png': {5: (-2, 0), 6: (-2, 0)},
    'Vision_Sharpshooter_Male.png': {5: (-2, 0)},
}


def digest(data):
    return hashlib.sha256(data).hexdigest()


def bounds(array, threshold=0):
    yy, xx = np.where(array[:, :, 3] > threshold)
    return None if not len(xx) else [int(xx.min()), int(yy.min()), int(xx.max()+1), int(yy.max()+1)]


def cleanup(cell, move):
    result = cell.copy()
    alpha = result[:, :, 3]
    labels, _ = label(alpha > 0, np.ones((3, 3)))
    counts = np.bincount(labels.ravel())
    counts[0] = 0
    body = int(counts.argmax())
    removals = []
    # 이번 8종은 주변 프레임과 대조했습니다. 최대169px 인접 검 조각 외에는 미세 잔여물입니다.
    # 큰 파츠나 본래의 독립 장식을 삭제하지 않도록 상한을 둡니다.
    for index, section in enumerate(find_objects(labels), 1):
        if index == body or section is None:
            continue
        mask = labels == index
        pixels = int(mask.sum())
        assert pixels <= 169, '큰 독립 성분은 자동 삭제할 수 없습니다'
        yy, xx = section
        removals.append({'pixels': pixels, 'bbox': [xx.start, yy.start, xx.stop, yy.stop], 'maxAlpha': int(alpha[mask].max())})
        result[mask] = 0
    dx, dy = move
    operation = {'removedComponents': removals, 'requestedMove': [dx, dy], 'appliedMove': [0, 0], 'discardedEdgeNoisePixels': 0}
    if dx or dy:
        yy, xx = np.indices((128, 128))
        lost = ((xx+dx < 0) | (xx+dx >= 128) | (yy+dy < 0) | (yy+dy >= 128)) & (result[:, :, 3] > 0)
        # 본체를 자르지 않습니다. 밖으로 나가는 Alpha1–2 경계 잔여물만 기록 후 제거합니다.
        if lost.any() and int(result[:, :, 3][lost].max()) > 2:
            operation['moveRejected'] = '반대쪽의 실제 윤곽/Alpha를 잃으므로 이동하지 않음'
            return result, operation
        moved = np.zeros_like(result)
        sx0, sx1 = max(0, -dx), min(128, 128-dx)
        sy0, sy1 = max(0, -dy), min(128, 128-dy)
        moved[sy0+dy:sy1+dy, sx0+dx:sx1+dx] = result[sy0:sy1, sx0:sx1]
        # 기존 프레임에 없던 강한 발/장비 경계 접촉을 새로 만들지 않는지 검사합니다.
        def edges(a):
            al=a[:, :, 3]
            return [bool((al[0] > 10).any()), bool((al[-1] > 10).any()), bool((al[:, 0] > 10).any()), bool((al[:, -1] > 10).any())]
        if any(new and not old for old, new in zip(edges(result), edges(moved))):
            operation['moveRejected'] = '반대쪽에 새 본체 경계 접촉이 생기므로 이동하지 않음'
            return result, operation
        operation.update(appliedMove=[dx, dy], discardedEdgeNoisePixels=int(lost.sum()))
        # 남긴 픽셀의 RGBA 값을 바꾸지 않았는지 확인합니다. 보간/색 변경/새 픽셀 생성은 없습니다.
        kept = result[(result[:, :, 3] > 0) & ~lost]
        actual = moved[moved[:, :, 3] > 0]
        assert sorted(map(tuple, kept.tolist())) == sorted(map(tuple, actual.tolist()))
        result = moved
    return result, operation


def run():
    assert digest(SOURCE.read_bytes()) == SOURCE_HASH
    OUT.mkdir(parents=True, exist_ok=True)
    for folder in ['Original', 'Corrected', 'Cells', 'Comparison']:
        (OUT / folder).mkdir(exist_ok=True)
    # 最初の実行だけを基準として残し、再実行で保護対象の基準を上書きしません。
    if not (OUT/'baseline.json').exists():
        protected = [p for folder in ['Unity/Client/Assets', 'Unity/Client/ProjectSettings', 'Unity/Client/UserData'] for p in (ROOT/folder).rglob('*') if p.is_file()]
        baseline = {str(p.relative_to(ROOT)): digest(p.read_bytes()) for p in protected}
        (OUT/'baseline.json').write_text(json.dumps(baseline), encoding='utf-8')
        (OUT/'git-status-before.txt').write_bytes(subprocess.check_output(['git','status','--porcelain=v1','-uall'], cwd=ROOT))
    inventory = json.loads((ROOT/'Unity/Client/Assets/_Project/Resources/PlayerAppearances/Validated50.json').read_text(encoding='utf-8-sig'))
    report={'sourceZip':str(SOURCE),'sourceZipSha256':SOURCE_HASH,'entries':[],'normalCellChangedPixels':0}
    with zipfile.ZipFile(SOURCE) as archive:
        assert len(archive.namelist()) == 8 and set(archive.namelist()) == set(TARGETS)
        for filename, allowed in TARGETS.items():
            data=archive.read(filename)
            image=Image.open(io.BytesIO(data)); assert image.mode=='RGBA' and image.size==(512,512)
            assetname=filename.replace('Vision_', 'Visual_').replace('_Sharpshooter_', '_Marksman_')
            entry=next(e for e in inventory['entries'] if Path(e['assetPath']).name==assetname)
            (OUT/'Original'/filename).write_bytes(data)
            original=np.array(image); corrected=original.copy(); frames=[]
            contact=Image.new('RGB',(1056,1120),'#36404e'); draw=ImageDraw.Draw(contact)
            for f in range(16):
                x,y=f%4*128,f//4*128
                cell=original[y:y+128,x:x+128].copy()
                if f in allowed:
                    changed,operation=cleanup(cell,MOVES[filename].get(f,(0,0)))
                else:
                    changed=cell.copy();operation={'preservedNormalCell':True}
                diff=np.any(cell!=changed,axis=2)
                if f not in allowed:assert not diff.any()
                corrected[y:y+128,x:x+128]=changed
                frame={'frame':f,'allowed':f in allowed,'beforeHash':digest(cell.tobytes()),'afterHash':digest(changed.tobytes()),'changedPixels':int(diff.sum()),'beforeBounds':bounds(cell),'afterBounds':bounds(changed),'beforeVisibleBounds':bounds(cell,10),'afterVisibleBounds':bounds(changed,10),**operation}
                frames.append(frame)
                for suffix,ar in [('before',cell),('after',changed)]:Image.fromarray(ar).save(OUT/'Cells'/f'{filename[:-4]}_{f:02d}_{suffix}.png')
                display=Image.fromarray(changed).resize((256,256),Image.Resampling.NEAREST)
                contact.paste(display,(f%4*264,f//4*280),display)
                draw.text((f%4*264+2,f//4*280+257),str(f)+' '+str(operation.get('appliedMove','unchanged')),fill='white')
            contact.save(OUT/'Comparison'/(filename+'.contact.png'))
            Image.fromarray(corrected).save(OUT/'Corrected'/filename)
            report['entries'].append({'sourceEntry':filename,'appearanceId':entry['appearanceId'],'assetPath':entry['assetPath'],'allowedFrames':allowed,'changedFrames':[f['frame'] for f in frames if f['changedPixels']],'originalPngSha256':digest(data),'correctedPngSha256':digest((OUT/'Corrected'/filename).read_bytes()),'frames':frames})
    (OUT/'report.json').write_text(json.dumps(report,ensure_ascii=False,indent=2)+'\n',encoding='utf-8')
    print('Normal cells unchanged:',128-sum(len(v) for v in TARGETS.values()))
    for e in report['entries']:print(e['sourceEntry'],e['changedFrames'],[(f['frame'],f.get('appliedMove'),f.get('moveRejected')) for f in e['frames'] if f['allowed']])


if __name__ == '__main__':
    run()
