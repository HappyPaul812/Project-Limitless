from pathlib import Path
from PIL import Image,ImageDraw
import json
O=Path('Temp/ArtPilots20261023');E=O/'Worktree/문서/00_프로젝트/Evidence/ArtPilots20261023';I=O/'Input'
files=[I/'Main23/NPC/Orna_VillageGuide_512_CANDIDATE.png',I/'Main23/Signs/Inn_StandaloneSign_CANDIDATE.png',I/'Bee/VenomBee_OriginalKeyArt_Pilot_v1_512.png',I/'Bee/VenomBee_Preview_40.png']
out=Image.new('RGB',(900,750),'#171d24');d=ImageDraw.Draw(out);d.text((15,8),'OFFLINE ALPHA DIAGNOSTIC / UNAPPROVED / SOURCE PNG UNCHANGED',fill='white')
for row,p in enumerate(files):
 for col,bg in enumerate(['#eeeeee','#191919','#65805a']):
  x=col*300;y=30+row*180;tile=Image.new('RGBA',(300,175),bg);im=Image.open(p).convert('RGBA');im.thumbnail((128,128));tile.alpha_composite(im,((300-im.width)//2,20));out.paste(tile.convert('RGB'),(x,y));d.text((x+8,y+150),('SUPPLIED 40px' if row==3 else p.stem[:33]),fill='black' if col==0 else 'white')
out.save(E/'OFFLINE_Alpha_3Backgrounds_UNAPPROVED.png')
# 20개는 기존 Idle10/Attack10 원본 프레임입니다. 후보 1포즈와 혼동하지 않게 명시합니다.
r=Path('Unity/Client/Assets/_Project/Resources/MonsterValidation/PilotBee');board=Image.new('RGB',(1300,330),'#6f865e');d=ImageDraw.Draw(board);d.text((10,5),'OFFLINE EXISTING Pilot Bee source cells: Idle10 / Attack10. Jana Ochse / 2D!PIXX. License version pending.',fill='white')
for row,(name,w,h,cols) in enumerate([('idle',238,215,8),('attack',315,253,10)]):
 im=Image.open(r/f'pilot_bee_{name}.png').convert('RGBA')
 for k in range(10):
  a=im.crop(((k%cols)*w,(k//cols)*h,(k%cols+1)*w,(k//cols+1)*h));a.thumbnail((110,110));board.paste(a,(10+k*128,40+row*145),a);d.text((10+k*128,155+row*145),f'{name} {k}',fill='white')
board.save(E/'OFFLINE_ExistingBee_Idle10_Attack10_SourceCells.png')
