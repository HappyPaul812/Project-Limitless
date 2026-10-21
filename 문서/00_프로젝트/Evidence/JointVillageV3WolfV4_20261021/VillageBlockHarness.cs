using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using ProjectLimitless.World;
using ProjectLimitless.CameraSystem;

// 실게임 Quest/NPC 없이 PNG 조립·정렬·통행폭만 확인하는 임시 거리 블록입니다.
public sealed class VillageBlockHarness:MonoBehaviour
{
    [Serializable] public sealed class Probe {public string name;public bool clear;public string obstacle;public float radius;}
    [Serializable] public sealed class Result {public string unity;public string graphics;public float ppu=128,gridWU=2,pathClearanceWU=3;public string actor="CircleCollider2D radius0.5 proxy + original wheelchair visual; not production controller";public List<Probe> probes=new List<Probe>();public int portalColliderCount;public string[] shots;}
    string root,output;GameObject block;Camera cam;WorldBounds2D bounds;GameObject visual;Result result=new Result();
    Sprite Load(string p,float ppu=128){var t=new Texture2D(2,2,TextureFormat.RGBA32,false);t.LoadImage(File.ReadAllBytes(Path.Combine(root,"Input/A",p)));t.filterMode=FilterMode.Point;return Sprite.Create(t,new Rect(0,0,t.width,t.height),new Vector2(.5f,0),ppu,0,SpriteMeshType.FullRect);}
    SpriteRenderer Put(string path,Vector2 at,int order=0,float scale=1){var g=new GameObject(path);g.transform.SetParent(block.transform);g.transform.position=at;g.transform.localScale=Vector3.one*scale;var r=g.AddComponent<SpriteRenderer>();r.sprite=Load(path);r.sortingOrder=order;return r;}
    void Box(string name,Vector2 at,Vector2 size){var g=new GameObject(name,typeof(BoxCollider2D));g.transform.SetParent(block.transform);g.transform.position=at;g.GetComponent<BoxCollider2D>().size=size;}
    void Footprint(string name,Vector2 at,float width){Box(name,at+Vector2.up*.4f,new Vector2(width,.8f));}
    void Route(string name,Vector2 a,Vector2 b){var v=b-a;var hit=Physics2D.CircleCast(a,.5f,v.normalized,v.magnitude);result.probes.Add(new Probe{name=name,clear=hit.collider==null,obstacle=hit.collider==null?"":hit.collider.name,radius=.5f});}
    void Shot(string name,int width,int height,Vector2 at,float ortho)
    {
        cam.transform.position=new Vector3(at.x,at.y,-10);cam.orthographicSize=ortho;cam.aspect=(float)width/height;
        var rt=new RenderTexture(width,height,24);rt.Create();cam.targetTexture=rt;cam.Render();
        var old=RenderTexture.active;RenderTexture.active=rt;var t=new Texture2D(width,height,TextureFormat.RGB24,false);t.ReadPixels(new Rect(0,0,width,height),0,0);t.Apply();RenderTexture.active=old;
        File.WriteAllBytes(Path.Combine(output,name),t.EncodeToPNG());cam.targetTexture=null;Destroy(rt);Destroy(t);
    }
    void Outline(Vector2 center,Vector2 size)
    {
        var g=new GameObject("RESERVED_NO_COMPONENTS");g.transform.SetParent(block.transform);var l=g.AddComponent<LineRenderer>();l.sharedMaterial=new Material(Shader.Find("Sprites/Default"));l.startColor=l.endColor=Color.cyan;l.startWidth=l.endWidth=.035f;l.positionCount=4;l.loop=true;l.sortingOrder=300;
        l.SetPositions(new[]{new Vector3(center.x-size.x/2,center.y-size.y/2),new Vector3(center.x+size.x/2,center.y-size.y/2),new Vector3(center.x+size.x/2,center.y+size.y/2),new Vector3(center.x-size.x/2,center.y+size.y/2)});
    }
    void Start()
    {
        root=Directory.GetParent(Application.dataPath).FullName;output=Path.Combine(root,"Output");Directory.CreateDirectory(output);block=new GameObject("QA_Village_Block");
        bounds=new GameObject("WorldBounds",typeof(WorldBounds2D)).GetComponent<WorldBounds2D>();bounds.transform.SetParent(block.transform);bounds.Configure(Vector2.zero,new Vector2(32,24));WorldBounds2D.CreateBoundaryColliders(block.transform,bounds.Bounds,.3f);
        cam=new GameObject("Village_QA_Camera",typeof(Camera)).GetComponent<Camera>();cam.transform.SetParent(block.transform);cam.orthographic=true;cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=Color.black;
        for(int x=-15;x<=15;x+=2)for(int y=-12;y<12;y+=2)Put("Tiles/Frostway_Ground_Earth_01.png",new Vector2(x,y),-100);
        for(int y=-10;y<10;y+=2)for(int x=-1;x<=1;x+=2)Put("Tiles/Frostway_Path_Stone_01.png",new Vector2(x,y),-90);
        for(int x=-7;x<=7;x+=2)Put("Tiles/Frostway_Path_Stone_01.png",new Vector2(x,-2),-89);
        var inn=Put("Buildings/Frostway_Inn_Building.png",new Vector2(-6,3),70);Footprint("Inn_Base",new Vector2(-6,3),4);
        Put("Buildings/Frostway_GeneralShop_Building.png",new Vector2(6,3),70);Footprint("General_Base",new Vector2(6,3),3.3f);
        Put("Buildings/Frostway_SkillShop_Building.png",new Vector2(-6,-5),150);Footprint("Skill_Base",new Vector2(-6,-5),3.5f);
        Put("Props/Frostway_Lantern_Post.png",new Vector2(3,-1),110);Footprint("Lantern_Base",new Vector2(3,-1),.4f);
        Put("Props/Frostway_Firewood_Stack.png",new Vector2(-10,-4),140);Footprint("Wood_Base",new Vector2(-10,-4),2);
        Put("Props/Frostway_SupplyCrate_Small.png",new Vector2(8,5),50);Footprint("Crate_Base",new Vector2(8,5),1);
        Put("Portal_Inactive/Frostway_EmptyPortal_Platform.png",new Vector2(10,-2),100);Outline(new Vector2(10,-1),new Vector2(7,7));
        Box("North_Unopened_Stop",new Vector2(0,10.6f),new Vector2(3,.4f));Box("South_Inside_Stop",new Vector2(0,-10.6f),new Vector2(3,.4f));
        // 본체의 실제 기본 반경과 같은 Physics2D shape query만 사용합니다. 이동입력/경로찾기 재구현이 아닙니다.
        Physics2D.SyncTransforms();Route("South_to_North",new Vector2(0,-9),new Vector2(0,9));Route("Door_Inn",new Vector2(0,1.5f),new Vector2(-6,1.5f));Route("Door_General",new Vector2(0,1.5f),new Vector2(6,1.5f));Route("Door_Skill",new Vector2(0,-6),new Vector2(-6,-6));Route("East_Reserved",new Vector2(0,-2.6f),new Vector2(10,-2.6f));
        Route("SouthNorth_LeftMargin",new Vector2(-1,-9),new Vector2(-1,9));Route("SouthNorth_RightMargin",new Vector2(1,-9),new Vector2(1,9));Route("NorthStop_ExpectedBlocked",new Vector2(0,9),new Vector2(0,11));
        result.portalColliderCount=Physics2D.OverlapBoxAll(new Vector2(10,-1),new Vector2(7,7),0).Length;
        // 원본 외형PNG의 한 셀만 복사해 같은 크기로 표시하고 모양을 다시 그리지 않습니다.
        var wt=new Texture2D(2,2);wt.LoadImage(File.ReadAllBytes(Path.Combine(root,"Input/wheel.png")));var ws=Sprite.Create(wt,new Rect(0,384,128,128),new Vector2(.5f,0),128);
        visual=new GameObject("Wheelchair_Visual_Only",typeof(SpriteRenderer));visual.transform.SetParent(block.transform);visual.transform.position=new Vector2(0,-1);var vr=visual.GetComponent<SpriteRenderer>();vr.sprite=ws;vr.sortingOrder=110;
        Shot("A_Unity_16x9.png",1600,900,Vector2.zero,7);Shot("A_Unity_4x3.png",1200,900,Vector2.zero,7);Shot("A_Unity_21x9.png",2100,900,Vector2.zero,Mathf.Min(7,16f/(21f/9)));
        visual.transform.position=new Vector2(-6,2.5f);vr.sortingOrder=75;Shot("A_Sorting_Front.png",640,480,new Vector2(-6,4),2.5f);
        visual.transform.position=new Vector2(-6,5);vr.sortingOrder=50;Shot("A_Sorting_Behind.png",640,480,new Vector2(-6,4),2.5f);
        // 모듈은 native128PPU,동일발기준으로만 조립합니다. 결함을 숨기는 per-part 비율변형은 하지 않습니다.
        var mroot=new Vector2(-11,2);Put("Buildings/Modular/Frostway_Wall_Timber.png",mroot,200);Put("Buildings/Modular/Frostway_Roof_Gable.png",mroot+Vector2.up*1.15f,201);Put("Buildings/Modular/Frostway_Door_StoneArch.png",mroot+new Vector2(.1f,0),202);
        Shot("A_Modular_Assembly_NativePPU.png",640,640,new Vector2(-11,3.3f),2);
        mroot=new Vector2(-11,-3);Put("Buildings/Modular/Frostway_Wall_Plain.png",mroot,200);Put("Buildings/Modular/Frostway_Roof_Shingle_A.png",mroot+Vector2.up*1.15f,201);Put("Buildings/Modular/Frostway_Door_WoodFrame.png",mroot,202);Shot("A_Modular_Assembly_B.png",640,640,new Vector2(-11,-1.7f),2);
        mroot=new Vector2(-11,-8);Put("Buildings/Modular/Frostway_Wall_Window.png",mroot,200);Put("Buildings/Modular/Frostway_Roof_Shingle_B.png",mroot+Vector2.up*1.15f,201);Put("Buildings/Modular/Frostway_Door_StoneFrame.png",mroot,202);Shot("A_Modular_Assembly_C.png",640,640,new Vector2(-11,-6.7f),2);
        result.unity=Application.unityVersion;result.graphics=SystemInfo.graphicsDeviceType.ToString();result.shots=new[]{"A_Unity_16x9.png","A_Unity_4x3.png","A_Unity_21x9.png"};File.WriteAllText(Path.Combine(output,"Village.json"),JsonUtility.ToJson(result,true));
        Debug.Log("VILLAGE_BLOCK_QA_COMPLETE");block.SetActive(false);Destroy(block);new GameObject("WolfQA").AddComponent<WolfPlaybackHarness>();Destroy(gameObject);
    }
}
