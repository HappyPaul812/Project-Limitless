from pathlib import Path
from PIL import Image,ImageDraw,ImageFont
import csv,json,hashlib,shutil,subprocess
R=Path('F:/study/codex/Project-Limitless');O=R/'Temp/MinimalDelta65';W=O/'Worktree';E=W/'문서/00_프로젝트/Evidence/MinimalDelta65';font=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',18);title=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',21)
for p in (O/'Runtime').glob('*.png'):shutil.copyfile(p,E/p.name)
shutil.copyfile(O/'Runtime/checks.txt',E/'B_NewDelta23Checks.txt')
# 초기 Animator 평가 전 Player 기본프레임 로그는 현재 외형 증거로 사용하지 않습니다.
rows=list(csv.DictReader((O/'Runtime/RuntimeRendererMap.csv').open(encoding='utf8')));rows=[r for r in rows if r['Role']!='CURRENT_WHEELCHAIR_BODY']
with (E/'OrnaReferenceRendererMap.csv').open('w',encoding='utf-8-sig',newline='') as f:
 wr=csv.DictWriter(f,fieldnames=rows[0].keys());wr.writeheader();wr.writerows(rows)
board=Image.new('RGB',(1240,665),'#19212c');d=ImageDraw.Draw(board);d.text((14,8),'B 신규 Unity 동일 카메라 / 인물 alpha 높이40·80·96·128px / 왼쪽 기존NPC · 오른쪽 플레이어',font=title,fill='white');src=[]
for i,h in enumerate([40,80,96,128]):
 p=E/f'UNAPPROVED_MinimalFinal_OrnaVisible{h}_1280x720.png';im=Image.open(p).convert('RGB').crop((190,110,810,380));x=(i%2)*620;y=45+(i//2)*310;board.paste(im,(x,y+30));d.text((x+7,y+3),f'인물 {h}px / 전체 캔버스 {h*512/484:.2f}px'+(' / 권고안·미승인' if h==80 else ''),font=font,fill='white');src.append({'source':p.name,'sha256':hashlib.sha256(p.read_bytes()).hexdigest(),'crop':[190,110,810,380],'rescale':False})
board.save(E/'B_Orna_Visible40_80_96_128_UnityCrops_UNAPPROVED.png');(E/'B_BoardSources.json').write_text(json.dumps(src,indent=2),encoding='utf8')
overview=Image.new('RGB',(1280,1080),'#14202b');d=ImageDraw.Draw(overview);d.text((15,8),'최종 검토용 축소 요약 / B: 새 인물높이 비교 · A: 기존 Unity 근거 재사용 / 미술 승인 대기',font=title,fill='white')
key=['B_Orna_Visible40_80_96_128_UnityCrops_UNAPPROVED.png','A_REUSED_Board2_Roof_Excluded_EntryDoorOnly_UNAPPROVED.png','A_REUSED_Board3_Inn_OneSign_KEEP_OLD.png','A_REUSED_A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png']
for i,n in enumerate(key):
 im=Image.open(E/n).convert('RGB');im.thumbnail((620,485));overview.paste(im,((i%2)*640,50+(i//2)*510))
overview.save(E/'Overview_SCALED_4Topics.png')
with (E/'ImageIndex_SHA256.csv').open('w',encoding='utf-8-sig',newline='') as f:
 wr=csv.writer(f);wr.writerow(['File','Width','Height','Type','SHA256'])
 for p in sorted(E.glob('*.png')):
  im=Image.open(p);kind='B_NEW_ACTUAL_UNITY_CAMERA_UI' if p.name.startswith('UNAPPROVED_MinimalFinal') else 'INPUT_OFFLINE_NOT_UNITY' if p.name.startswith('INPUT_OFFLINE') else 'SCALED_OVERVIEW_ONLY' if p.name.startswith('Overview') else 'B_NEW_UNSCALED_UNITY_CROPS' if p.name.startswith('B_Orna') else 'A_PREVIOUS_EVIDENCE_REUSE'
  wr.writerow([p.name,im.width,im.height,kind,hashlib.sha256(p.read_bytes()).hexdigest()])
paths=[p for p in subprocess.check_output(['git','diff','--name-only','-z'],cwd=W,stderr=subprocess.DEVNULL).decode().split('\0') if p];env=[p for p in paths if p!='Unity/Client/Assets/_Project/Scripts/Editor/ArtPilotsDeltaQA.cs'];assert all(p.startswith('Unity/') for p in env);(O/'RestoreEnvironment.txt').write_text('\n'.join(env)+'\n',encoding='utf8');print('images',len(list(E.glob('*.png'))),'restore environment',len(env))
