from pathlib import Path
from PIL import Image,ImageDraw,ImageFont
import csv,json,hashlib,shutil
R=Path('F:/study/codex/Project-Limitless');O=R/'Temp/ArtReview65';W=O/'Worktree';E=W/'문서/00_프로젝트/Evidence/ArtReview65';P=R/'Temp/ArtPilots20261023/Worktree/문서/00_프로젝트/Evidence/ArtPilots20261023'
font=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',18);title=ImageFont.truetype('C:/Windows/Fonts/malgun.ttf',22)
for folder in [O/'Runtime',O/'Runtime/FinalFrames']:
 for p in folder.glob('*.png'):shutil.copyfile(p,E/p.name)
for n in ['checks.txt','RuntimeRendererMap.csv']:shutil.copyfile(O/'Runtime'/n,E/('B_New65_'+n))
shutil.copyfile(O/'Runtime/FinalFrames/checks.txt',E/'B_FinalFrames_checks.txt')
reuses={'A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png':'UNAPPROVED_Bee_Ch2_Comparison_UnityCanvas_960x720.png','A64_Bee_RightFacing_PREVIEW_ONLY.png':'UNAPPROVED_Bee_CandidateRightFacing_PREVIEW_ONLY.png','A64_OFFLINE_Alpha_3Backgrounds.png':'OFFLINE_Alpha_3Backgrounds_UNAPPROVED.png'}
for new,old in reuses.items():shutil.copyfile(P/old,E/new)
sources=[]
def crop_board(name,items,size,label):
 board=Image.new('RGB',size,'#19212c');d=ImageDraw.Draw(board);d.text((14,8),label,font=title,fill='white')
 for source,rect,xy,caption in items:
  im=Image.open(source).convert('RGB').crop(rect);x,y=xy;board.paste(im,(x,y+30));d.text((x+7,y+3),caption,font=font,fill='white');sources.append({'board':name,'source':str(source.relative_to(R)),'source_sha256':hashlib.sha256(source.read_bytes()).hexdigest(),'crop':rect,'paste':[x,y+30],'rescale':False})
 board.save(E/name)
items=[]
for i,(px,caption) in enumerate([(40,'40px: 기존 NPC보다 작음'),(82,'82px: 기존 NPC 비례 권고안 (미승인)'),(96,'96px: 가독성 절충 검토안'),(128,'128px: 기존 NPC 대비 큼')]):
 items.append((E/f'UNAPPROVED_NEW65_Orna{px}_ReferencePlayer_1280x720.png',(190,110,810,380),((i%2)*620,45+(i//2)*310),caption))
crop_board('Board1_Orna_40_82_96_128_UNAPPROVED.png',items,(1240,665),'B 신규 Unity 고정카메라 원본 크롭 / 왼쪽: 기존 NPC 참고 복사본 · 오른쪽: 플레이어 / 배율 변경 없음')
crop_board('Board2_Roof_Excluded_EntryDoorOnly_UNAPPROVED.png',[(P/'UNAPPROVED_SouthGate_DoubleRoof_ART_HOLD_1280x720.png',(385,20,695,345),(0,45),'A #64 재사용: 겹지붕 HOLD'),(E/'UNAPPROVED_NEW65_Residence_KEEP_OLD_1280x720.png',(197,20,507,345),(310,45),'B #65: 기존 주택 유지'),(E/'UNAPPROVED_NEW65_Residence_EntryDoorOnly_1280x720.png',(197,20,507,345),(620,45),'B #65: 새 EntryDoor만')],(930,405),'실제 Unity 원본 크롭 / Gable·Trim 미설치 / B 두 화면은 동일 카메라 / 모든 후보 미승인')
crop_board('Board3_Inn_OneSign_KEEP_OLD.png',[(P/'UNAPPROVED_Inn_NewSign_1280x720.png',(430,0,840,280),(0,45),'A #64 재사용: 신규+내장 간판 중복'),(E/'UNAPPROVED_NEW65_Inn_KEEP_OLD_1280x720.png',(430,0,840,280),(410,45),'B #65: 본체 내장 간판만 KEEP_OLD')],(820,365),'Unity 크롭 / A:#64 재사용 · B:#65 신규 / 신규 간판 미설치')
(E/'BoardSourceMap.json').write_text(json.dumps(sources,ensure_ascii=False,indent=2),encoding='utf8')
# 축소 요약판은 네이티브 픽셀 크기 판정에 사용하지 않습니다.
overview=Image.new('RGB',(1280,1060),'#111923');d=ImageDraw.Draw(overview);d.text((15,8),'축소 요약판 / 실제 크기는 개별 비교판에서 확인 / 최종 미술 승인 대기',font=title,fill='white')
for i,n in enumerate(['Board1_Orna_40_82_96_128_UNAPPROVED.png','Board2_Roof_Excluded_EntryDoorOnly_UNAPPROVED.png','Board3_Inn_OneSign_KEEP_OLD.png','A64_Bee_Ch2_Original_UnityBoard_UNAPPROVED.png']):
 im=Image.open(E/n).convert('RGB');im.thumbnail((620,480));x=(i%2)*640;y=55+(i//2)*500;overview.paste(im,(x,y))
overview.save(E/'Overview_4Topics_SCALED_ContactSheet.png')
with (E/'ImageIndex_SHA256.csv').open('w',encoding='utf-8-sig',newline='') as f:
 wr=csv.writer(f);wr.writerow(['File','Width','Height','Class','SHA256'])
 for p in sorted(E.glob('*.png')):
  im=Image.open(p);typ='B_NEW65_UNITY_PLAY_CAMERA_UI' if p.name.startswith('UNAPPROVED_NEW65') else 'A_REUSED64_UNCHANGED' if p.name.startswith('A64') else 'SCALED_OVERVIEW_ONLY' if p.name.startswith('Overview') else 'UNSCALED_LABELED_UNITY_CAPTURE_CROPS';wr.writerow([p.name,im.width,im.height,typ,hashlib.sha256(p.read_bytes()).hexdigest()])
print('new Unity images',len(list(E.glob('UNAPPROVED_NEW65*.png'))),'review files',len(list(E.glob('*.png'))))
