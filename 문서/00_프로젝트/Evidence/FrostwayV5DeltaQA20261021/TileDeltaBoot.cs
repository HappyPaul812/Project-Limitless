using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 변경된 지면만 같은 배율로 비교하는 배치 전용 도구입니다. 본편 Scene/Quest/NPC를 생성하지 않습니다.
public static class TileDeltaBoot
{
 [Serializable] public class Check{public string name;public bool pass;}
 [Serializable] public class Report{public string unity,graphics;public float ppu=128,pixelsPerWU=64;public List<Check> checks=new List<Check>();public List<string> images=new List<string>();}
 static string root,output,version;static GameObject content;static Camera cam;static Report report=new Report();
 static string[] tiles={"Ground_Earth_01","Ground_Gravel_01","Ground_ThinFrost_01","Path_Stone_01"};
 static Sprite Load(string path){var t=new Texture2D(2,2,TextureFormat.RGBA32,false);t.LoadImage(File.ReadAllBytes(Path.Combine(root,"Input",version,path)));t.filterMode=FilterMode.Point;return Sprite.Create(t,new Rect(0,0,t.width,t.height),new Vector2(.5f,0),128,0,SpriteMeshType.FullRect);}
 static SpriteRenderer Put(string path,Vector2 p,int order){var g=new GameObject(path);g.transform.SetParent(content.transform);g.transform.position=p;var r=g.AddComponent<SpriteRenderer>();r.sprite=Load(path);r.sortingOrder=order;return r;}
 static void Clear(){if(content!=null)UnityEngine.Object.DestroyImmediate(content);content=new GameObject("DeltaOnly_"+version);}
 static void Box(string n,Vector2 p,Vector2 size){var g=new GameObject(n,typeof(BoxCollider2D));g.transform.SetParent(content.transform);g.transform.position=p;g.GetComponent<BoxCollider2D>().size=size;}
 static void Shot(string n,int w,int h,Vector2 center,float ortho){cam.transform.position=new Vector3(center.x,center.y,-10);cam.orthographicSize=ortho;cam.aspect=(float)w/h;var rt=new RenderTexture(w,h,24);rt.Create();cam.targetTexture=rt;cam.Render();var previous=RenderTexture.active;RenderTexture.active=rt;var t=new Texture2D(w,h,TextureFormat.RGB24,false);t.ReadPixels(new Rect(0,0,w,h),0,0);t.Apply();File.WriteAllBytes(Path.Combine(output,n),t.EncodeToPNG());RenderTexture.active=previous;cam.targetTexture=null;rt.Release();UnityEngine.Object.DestroyImmediate(rt);UnityEngine.Object.DestroyImmediate(t);report.images.Add(n);}
 static void ThreeShots(string stem){Shot(stem+"_16x9.png",1600,900,Vector2.zero,7.03125f);Shot(stem+"_4x3.png",1200,900,Vector2.zero,7.03125f);Shot(stem+"_21x9.png",2100,900,Vector2.zero,7.03125f);}
 static void Panels(){Clear();for(int k=0;k<4;k++)for(int x=0;x<3;x++)for(int y=0;y<3;y++)Put("Tiles/Frostway_"+tiles[k]+".png",new Vector2((k%2==0?-6:2)+x*2,(k<2?1:-7)+y*2),0);ThreeShots(version+"_Repeat3x3");}
 static void Context()
 {
  Clear();for(int x=-9;x<=9;x+=2)for(int y=-8;y<8;y+=2){int k=x<0?0:2;if(y< -4)k=1;Put("Tiles/Frostway_"+tiles[k]+".png",new Vector2(x,y),-100);}
  for(int x=-1;x<=1;x+=2)for(int y=-8;y<8;y+=2)Put("Tiles/Frostway_Path_Stone_01.png",new Vector2(x,y),-90);
  for(int x=-7;x<=7;x+=2)Put("Tiles/Frostway_Path_Stone_01.png",new Vector2(x,-2),-89);
  Put("Buildings/Frostway_Inn_Building.png",new Vector2(-6,3),70);Box("Inn_Foot",new Vector2(-6,3.4f),new Vector2(4,.8f));
  Put("Buildings/Frostway_GeneralShop_Building.png",new Vector2(6,3),70);Box("General_Foot",new Vector2(6,3.4f),new Vector2(3.3f,.8f));
  Put("Buildings/Frostway_SkillShop_Building.png",new Vector2(-6,-5),150);Box("Skill_Foot",new Vector2(-6,-4.6f),new Vector2(3.5f,.8f));
  Physics2D.SyncTransforms();
  foreach(float radius in new[]{.3f,.5f})
  {
   foreach(var route in new[]{new[]{new Vector2(0,-6),new Vector2(0,6)},new[]{new Vector2(0,1.5f),new Vector2(-6,1.5f)},new[]{new Vector2(0,-6),new Vector2(-6,-6)}})
   {var d=route[1]-route[0];report.checks.Add(new Check{name=version+" route "+route[1]+" radius "+radius,pass=Physics2D.CircleCast(route[0],radius,d.normalized,d.magnitude).collider==null});}
  }
  ThreeShots(version+"_Connections");
  var texture=new Texture2D(2,2);texture.LoadImage(File.ReadAllBytes(Path.Combine(root,"Input/wheel.png")));var sprite=Sprite.Create(texture,new Rect(0,384,128,128),new Vector2(.5f,0),128);
  var go=new GameObject("OriginalWheelchairVisual",typeof(SpriteRenderer));go.transform.SetParent(content.transform);var sr=go.GetComponent<SpriteRenderer>();sr.sprite=sprite;
  go.transform.position=new Vector2(-6,2.5f);sr.sortingOrder=75;Shot(version+"_DoorFront.png",640,480,new Vector2(-6,4),2.5f);
  go.transform.position=new Vector2(-6,5);sr.sortingOrder=50;Shot(version+"_DoorBehind.png",640,480,new Vector2(-6,4),2.5f);
 }
 public static void Run()
 {
  if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/FrostwayV5DeltaQA20261021/UnityQA/Assets"))throw new Exception("격리 QA 경로만 허용합니다.");
  root=Directory.GetParent(Application.dataPath).FullName;output=Path.Combine(root,"Output");Directory.CreateDirectory(output);EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
  cam=new GameObject("DeltaQA_Camera",typeof(Camera)).GetComponent<Camera>();cam.orthographic=true;cam.clearFlags=CameraClearFlags.SolidColor;cam.backgroundColor=new Color(.15f,.18f,.21f);
  foreach(string v in new[]{"v4","v5"}){version=v;Panels();Context();}
  report.unity=Application.unityVersion;report.graphics=SystemInfo.graphicsDeviceType.ToString();File.WriteAllText(Path.Combine(output,"UnityDelta.json"),JsonUtility.ToJson(report,true));Debug.Log("FROSTWAY_V5_DELTA_COMPLETE");EditorApplication.Exit(0);
 }
}
