# -*- coding: utf-8 -*-
from pathlib import Path
import hashlib,json,zipfile,subprocess,shutil,uuid,io
from PIL import Image
root=Path(r'F:/study/codex/Project-Limitless')
base=root/'Temp/WolfRunV5Apply20261021'; wt=base/'Worktree'
e=wt/'문서/00_프로젝트/Evidence/WolfRunV5Apply20261021';e.mkdir(parents=True,exist_ok=True)
def sha(p):return hashlib.sha256(p.read_bytes()).hexdigest()
def snapshot():
 return {str(p.relative_to(root)):sha(p) for folder in ['Unity/Client/Assets','Unity/Client/Packages','Unity/Client/ProjectSettings','Unity/Client/UserData','UserData'] for p in (root/folder).rglob('*') if p.is_file()}
protection={'files':snapshot(),'head':subprocess.check_output(['git','rev-parse','HEAD'],cwd=root).decode().strip(),'status':hashlib.sha256(subprocess.check_output(['git','status','--porcelain=v1','-z'],cwd=root)).hexdigest()}
(base/'ProtectionBefore.json').write_text(json.dumps(protection,ensure_ascii=False,indent=2),encoding='utf8')
zpath=Path(r'F:/Downloads/Limitless_Beast_Wolf_Run6_FromApprovedV5_ArtRework_v5.zip')
assert sha(zpath)=='16ba7923771c98f05b0f041b5c5a63857cdecfcdfd2e1f7768d9f24d1af0e351'
z=zipfile.ZipFile(zpath);assert z.testzip() is None
data=z.read('Assets/Wolf_Run6_v5_Strip_3072x384.png');strip=Image.open(io.BytesIO(data)).convert('RGBA')
for i in range(6):assert strip.crop((i*512,0,(i+1)*512,384)).tobytes()==Image.open(io.BytesIO(z.read(f'Assets/Poses/Wolf_Run_{i:02}_v5.png'))).convert('RGBA').tobytes()
res=wt/'Unity/Client/Assets/_Project/Resources/CompanionAssaultValidation'
(res/'Wolf_Run6_v5.png').write_bytes(data)
meta=(res/'Wolf_Run.png.meta').read_text(encoding='utf8').replace('8c410e828c92433389e8f18e8536bb2e',uuid.uuid4().hex).replace('maxTextureSize: 2048','maxTextureSize: 4096')
(res/'Wolf_Run6_v5.png.meta').write_text(meta,encoding='utf8')
scripts=wt/'Unity/Client/Assets/_Project/Scripts'
p=scripts/'Battle/BeastCompanionSystem.cs';s=p.read_text(encoding='utf8')
s=s.replace('float displayScale, float travelSpeed)','float displayScale, float travelSpeed,\n            string iconResourcePath = null, int iconFrameWidth = 0)')
s=s.replace('FrameWidth = Math.Max(1, frameWidth);','FrameWidth = Math.Max(1, frameWidth);\n            IconResourcePath = iconResourcePath ?? RunResourcePath;\n            IconFrameWidth = iconFrameWidth > 0 ? iconFrameWidth : FrameWidth;')
s=s.replace('public int FrameWidth { get; }','public int FrameWidth { get; }\n        // 승인 Run 교체가 기존 스킬/시설 아이콘을 바꾸지 않도록 아이콘 원본 규격을 별도로 보존합니다.\n        public string IconResourcePath { get; }\n        public int IconFrameWidth { get; }')
s=s.replace('"wolf", "Wolf", "CompanionAssaultValidation/Wolf_Run", 64, 4, 12f, 2f, 760f * 3f / 4f);','// 승인 512×384 셀은 0.25배로 128×96 UI 영역에 표시합니다. 기존 아이콘 64px 원본은 유지합니다.\n            "wolf", "Wolf", "CompanionAssaultValidation/Wolf_Run6_v5", 512, 4, 12f, .25f, 760f * 3f / 4f,\n            "CompanionAssaultValidation/Wolf_Run", 64);')
p.write_text(s,encoding='utf8')
for rel in ['Battle/BattleUiIconCatalog.cs','UI/PetFacilityPresenter.cs']:
 p=scripts/rel;s=p.read_text(encoding='utf8').replace('beast.RunResourcePath','beast.IconResourcePath').replace('beast.FrameWidth','beast.IconFrameWidth');p.write_text(s,encoding='utf8')
p=scripts/'Battle/BattleActionPresenter.cs';s=p.read_text(encoding='utf8').replace('384×40 한 줄 SpriteSheet를 64×40 여섯 장으로 나눈 Sprite 배열을','야수 정의의 셀 너비로 한 줄 시트를 나눈 Sprite 배열을').replace('1행 6열 Wolf 시트는 전체 384×40이고 각 칸은 64×40입니다. 원본 파일을 수정하지 않고','승인 Wolf Run6는 전체 3072×384, 각 칸 512×384입니다. Bear/Fox는 기존 규격을 사용하며').replace('x=0,64,128,192,256,320 영역을 각각 별도 Sprite로 만들어 Run Coroutine에 전달합니다.','정의의 FrameWidth 간격으로 별도 Sprite를 만들어 Run Coroutine에 전달합니다.');p.write_text(s,encoding='utf8')
(e/'Delivery.json').write_text(json.dumps({'zip_sha256':sha(zpath),'crc_errors':0,'strip_sha256':hashlib.sha256(data).hexdigest(),'pixel_identical_frames':6,'manifest':json.loads(z.read('Manifest.json'))},ensure_ascii=False,indent=2),encoding='utf8')
print('Prepared 6 product files; protected snapshot',len(protection['files']))
