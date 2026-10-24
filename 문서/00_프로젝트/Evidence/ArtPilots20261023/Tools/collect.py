from pathlib import Path
import json,subprocess,hashlib,shutil,csv
import numpy as np
from PIL import Image,ImageDraw
R=Path('F:/study/codex/Project-Limitless');O=R/'Temp/ArtPilots20261023';W=O/'Worktree';E=W/'문서/00_프로젝트/Evidence/ArtPilots20261023'
# Unity가 자동 생성한 문서용 메타만 삭제합니다. 후보 소스·원본에는 적용하지 않습니다.
for n in ['Assets/_Project/Animations/Monsters.meta','Assets/_Project/Animations/Monsters/01_GrassSlime.meta','Assets/_Project/Animations/Monsters/01_GrassSlime/ASSET_REQUIREMENTS.md.meta','Assets/_Project/Art/Monsters.meta','Assets/_Project/Art/Monsters/01_GrassSlime.meta','Assets/_Project/Art/Monsters/01_GrassSlime/ASSET_REQUIREMENTS.md.meta','ProjectSettings/SceneTemplateSettings.json']:
 p=(W/'Unity/Client'/n).resolve();assert p.is_relative_to((W/'Unity/Client').resolve())
 if p.exists():p.unlink()
# 修正比較のみ上書きし、最初の不公平な20px旧看板・下端pivot蜂画像は公開対象から除きます。
for folder in [O/'Runtime',O/'Runtime/CaptureFix']:
 for p in folder.glob('*.png'):shutil.copyfile(p,E/p.name)
for n in ['checks.txt','RuntimeRendererMap.csv','LiveBeeFrame.txt']:
 if (O/'Runtime'/n).exists():shutil.copyfile(O/'Runtime'/n,E/('Initial_'+n))
for n in ['checks.txt','RuntimeRendererMap.csv'] :shutil.copyfile(O/'Runtime/CaptureFix'/n,E/('Corrected_'+n))
# 固定カメラの実レンダリングを縮尺変更なしで並べ、ラベルのみ追加します。
pairs=[('Roof_BeforeAfter_UnityComparison_UNAPPROVED.png',['UNAPPROVED_SouthGateModule_ActualResidence_Before_1280x720.png','UNAPPROVED_SouthGate_DoubleRoof_ART_HOLD_1280x720.png','UNAPPROVED_SouthGate_DoorOnly_MINIMAL_OPTION_1280x720.png'],['BEFORE - actual Residence at NW','AFTER - double roof ART_HOLD','MINIMAL OPTION - omit Gable + Trim']),('Orna_Scale_UnityComparison_UNAPPROVED.png',['UNAPPROVED_Orna_Before_1280x720.png','UNAPPROVED_Orna40_1280x720.png','UNAPPROVED_Orna128_1280x720.png'],['EXISTING 82px canvas','CANDIDATE 40px canvas','CANDIDATE 128px canvas']),('Inn_BeforeAfter_UnityComparison_UNAPPROVED.png',['UNAPPROVED_Inn_Before_1280x720.png','UNAPPROVED_Inn_OldSignReference_1280x720.png','UNAPPROVED_Inn_NewSign_1280x720.png'],['BEFORE - sign embedded in building','OLD fragment - PPU128 reference','NEW standalone - duplicate sign HOLD'])]
for name,files,labels in pairs:
 board=Image.new('RGB',(1280,len(files)*750),'#1c242d');d=ImageDraw.Draw(board)
 for i,(f,l) in enumerate(zip(files,labels)):
  board.paste(Image.open(E/f).convert('RGB'),(0,i*750+30));d.text((15,i*750+8),'UNITY PLAY CAMERA + UI / '+l,fill='white')
 board.save(E/name)
# 겹친 사각형뿐 아니라 alpha>128 불투명 실루엣 교차도 같은 월드 좌표에서 측정합니다.
body=W/'Unity/Client/Assets/_Project/Art/PresentationCandidates/FrostwayV5/Buildings/Frostway_Residence_Building.png';roof=O/'Input/Main23/Buildings/SouthGate_RoofGable_Joint_CANDIDATE.png'
res=128;bounds=(-15,7,-10,12);masks=[]
for p,x,y,ppu,scale in [(body,-12.5,7.2,128,1),(roof,-12.5,8.8,256,1.4)]:
 im=Image.open(p).getchannel('A');w,h=im.size;im=im.resize((round(w/ppu*scale*res),round(h/ppu*scale*res)));target=Image.new('L',(5*res,5*res));target.paste(im,(round((x-w/ppu*scale/2+15)*res),round((12-y-h/ppu*scale)*res)));masks.append(np.array(target)>128)
count=int((masks[0]&masks[1]).sum());(E/'RoofOverlapMeasurement.json').write_text(json.dumps({'units':'WU','body_rect':[-14.4609375,7.2,-10.5390625,10.121875],'roof_rect':[-14.444140625,8.8,-10.555859375,11.5234375],'aabb_intersection_width':3.88828125,'aabb_intersection_height':1.321875,'sample_ppu':128,'alpha_threshold':128,'intersection_samples':count,'approx_alpha_overlap_area_WU2':count/res**2},indent=2),encoding='utf8')
print('screens',len(list(E.glob('*.png'))),'alpha overlap WU2',count/res**2)
