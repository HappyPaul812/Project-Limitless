"""감사에서 정확히 확인된 항목에만 ID를 연결하고 원본을 복사합니다."""
import collections
import shutil
from audit_main13_15_voice import ROOT, INPUT, run

report=run()
assert report['status']=={'MAPPED':41}, '불일치가 있으면 적용하지 않습니다.'
groups=collections.defaultdict(list)
for row in report['rows']: groups[row['source_file']].append(row)
for file,rows in groups.items():
    path=ROOT/file
    source=path.read_text(encoding='utf-8-sig')
    for row in sorted((r for r in rows if 'call' in r),key=lambda r:r['call']['end'],reverse=True):
        call=row['call']
        if not call['id']:
            source=source[:call['end']]+', "'+row['clip_id']+'"'+source[call['end']:]
    path.write_text(source,encoding='utf-8')
    for row in rows:
        original=INPUT/row['output_file']
        destination=ROOT/'Unity/Client/Assets/_Project/Audio/Voice/Story'/f"Main{row['quest_no']}"/original.name
        destination.parent.mkdir(parents=True,exist_ok=True)
        if destination.exists(): assert destination.read_bytes()==original.read_bytes()
        else: shutil.copyfile(original,destination)
print('Applied',len(report['rows']))
