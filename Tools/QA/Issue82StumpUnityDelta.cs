#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>게임 코드나 저장소를 로드하지 않고 승인된 그루터기 한 PNG의 표시만 검증하는 독립 배치 도구입니다.</summary>
public static class Issue82StumpUnityDelta
{
    const string Asset = "Assets/ApprovedStump.png";
    const string ApprovedHash = "22f2f5816b3fd2ffe45234ec807bb3d81a83f87440eb8eb1f338e0dff33404a8";
    static readonly List<string> Checks = new List<string>();
    static string Output => Path.GetFullPath(Path.Combine(Application.dataPath, "../../UnityEvidence"));
    static Camera camera;
    static SpriteRenderer stump, proxy;
    static Texture2D source;
    static Vector2 pivot;
    static int left, bottom, right, top;

    public static void Run()
    {
        if (!Application.isBatchMode || !Application.dataPath.Replace('\\','/').EndsWith("/Temp/Issue82StumpSpiderBee20261103/QA/Client/Assets"))
            throw new InvalidOperationException("Issue82 독립 fixture batch 경로에서만 실행할 수 있습니다.");
        Directory.CreateDirectory(Output);
        try
        {
            Check(Application.unityVersion == "6000.6.5f1", "Editor.Exact6000_6_5");
            Check(Hash(File.ReadAllBytes(Asset)) == ApprovedHash, "Source.ApprovedSHAExact");
            source = new Texture2D(2,2,TextureFormat.RGBA32,false);
            Check(source.LoadImage(File.ReadAllBytes(Asset)), "Source.Decode");
            Check(source.width == 320 && source.height == 384, "Source.Canvas320x384");
            left=source.width;bottom=source.height;right=top=0;bool transparent=false,opaque=false;
            for(int y=0;y<source.height;y++) for(int x=0;x<source.width;x++)
            {
                byte alpha=source.GetPixel(x,y).a > 0 ? (byte)Mathf.RoundToInt(source.GetPixel(x,y).a*255) : (byte)0;
                transparent |= alpha==0;opaque |= alpha==255;
                if(alpha==0)continue;left=Math.Min(left,x);right=Math.Max(right,x+1);bottom=Math.Min(bottom,y);top=Math.Max(top,y+1);
            }
            Check(transparent && opaque, "Alpha.TransparentAndOpaque");
            Check(left>0 && bottom>0 && right<source.width && top<source.height, "Alpha.NoCanvasEdgeContact");
            // 투명 여백이 아닌 실제 보이는 최하단을 발 위치에 맞추는 후보입니다. 정식 Pivot 확정은 아닙니다.
            pivot=new Vector2(.5f,(float)bottom/source.height);
            Configure(128);
            Sprite sprite=AssetDatabase.LoadAssetAtPath<Sprite>(Asset);
            Check(sprite!=null, "Importer.ActualSpriteLoaded");
            Check(Mathf.Abs(sprite.pixelsPerUnit-128)<.001f, "Importer.PPU128");
            Check(Vector2.Distance(sprite.pivot,new Vector2(160,bottom))<.01f, "Importer.PivotAtVisibleBottom");
            var importer=(TextureImporter)AssetImporter.GetAtPath(Asset);
            Check(importer.alphaIsTransparency && !importer.mipmapEnabled && importer.filterMode==FilterMode.Point, "Importer.AlphaPointNoMip");
            Check(importer.textureCompression==TextureImporterCompression.Uncompressed, "Importer.Uncompressed");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
            camera=new GameObject("FixtureCamera").AddComponent<Camera>();camera.transform.position=new Vector3(0,1,-10);
            camera.orthographic=true;camera.orthographicSize=5;camera.clearFlags=CameraClearFlags.SolidColor;camera.backgroundColor=new Color(.30f,.35f,.40f,1);
            stump=new GameObject("ApprovedStumpOnly").AddComponent<SpriteRenderer>();stump.sprite=sprite;stump.sortingOrder=0;
            Check(stump.transform.localScale==Vector3.one, "Scale.Default1");
            Check(Mathf.Abs(stump.bounds.size.x-2.5f)<.001f && Mathf.Abs(stump.bounds.size.y-3)<.001f,"Scale.FullRect2_5x3WU");
            float visibleFoot=(bottom-sprite.pivot.y)/sprite.pixelsPerUnit;
            Check(Mathf.Abs(visibleFoot)<.001f,"Ground.VisibleAlphaFootAtZero");
            DrawLine(new Vector3(-8,0,.1f),new Vector3(8,0,.1f),new Color(.8f,.85f,.9f),-2);
            for(int x=-8;x<=8;x++)DrawLine(new Vector3(x,-4,1),new Vector3(x,6,1),new Color(.36f,.41f,.46f),-4);
            for(int y=-4;y<=6;y++)DrawLine(new Vector3(-8,y,1),new Vector3(8,y,1),new Color(.36f,.41f,.46f),-4);
            foreach(int width in new[]{1280,960,1680})
            {
                camera.aspect=(float)width/720;
                Vector3 a=camera.WorldToViewportPoint(stump.bounds.min),b=camera.WorldToViewportPoint(stump.bounds.max);
                Check(a.x>0 && a.y>0 && b.x<1 && b.y<1,"Viewport.FullyVisible."+width);
                Capture("Stump_PPU128_Scale1_"+width+"x720",width,720);
            }
            // 실제 Player가 아니라 정렬 중첩 시험용 사각형입니다. 이동·접근성 PASS로 해석하지 않습니다.
            var pixel=new Texture2D(1,1);pixel.SetPixel(0,0,Color.white);pixel.Apply();
            proxy=new GameObject("SORTING_PROXY_NOT_PLAYER").AddComponent<SpriteRenderer>();
            proxy.sprite=Sprite.Create(pixel,new Rect(0,0,1,1),new Vector2(.5f,0),1);
            proxy.color=new Color(.15f,.65f,1,1);proxy.transform.localScale=new Vector3(.5f,1,1);proxy.transform.position=new Vector3(0,.2f,0);
            camera.aspect=1280f/720;
            proxy.sortingOrder=-10;var behind=Capture("Sorting_ProxyBehind",1280,720);
            proxy.sortingOrder=10;var front=Capture("Sorting_ProxyInFront",1280,720);
            int changed=0;for(int i=0;i<front.Length;i++)if(front[i].b>behind[i].b+20)changed++;
            Check(changed>100,"Sorting.FrontBehindOcclusionChangesVisiblePixels");proxy.gameObject.SetActive(false);
            stump.transform.localScale=Vector3.one*.75f;Capture("Scale0_75_1280",1280,720);
            Check(Mathf.Abs(stump.bounds.size.x-1.875f)<.001f,"Scale.Candidate0_75");stump.transform.localScale=Vector3.one;
            Configure(256);stump.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(Asset);Capture("PPU256_Scale1_1280",1280,720);
            Check(Mathf.Abs(stump.bounds.size.x-1.25f)<.001f,"PPU.Candidate256HalfWorldSize");
            Configure(128);stump.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(Asset);
            Check(Hash(File.ReadAllBytes(Asset))==ApprovedHash,"Source.SHAUnchangedAfterImport");
            File.WriteAllText(Path.Combine(Output,"spec.txt"),"Unity="+Application.unityVersion+"\nPPU_candidate=128\nPivot_normalized="+pivot+"\nVisible_bbox_texture_bottom_origin="+left+","+bottom+","+right+","+top+"\nScale_candidate=1\nCamera_ortho=5\nVisible_height_WU="+(top-bottom)/128f+"\nVisible_height_px_720="+(top-bottom)/128f*72+"\nNoOfficialF2Scene=true\nRealPlayerColliderWheelchair=NOT_VERIFIED\n");
            File.WriteAllText(Path.Combine(Output,"status.txt"),"STUMP_ISOLATED_SPRITE_DISPLAY_PASS / REAL_F2_ACCESSIBILITY_NOT_VERIFIED");
            EditorApplication.Exit(0);
        }
        catch(Exception e)
        {
            File.WriteAllText(Path.Combine(Output,"failure.txt"),e.ToString());File.WriteAllText(Path.Combine(Output,"status.txt"),"FAIL");EditorApplication.Exit(1);
        }
    }
    static void Configure(float ppu)
    {
        var t=(TextureImporter)AssetImporter.GetAtPath(Asset);t.textureType=TextureImporterType.Sprite;t.spriteImportMode=SpriteImportMode.Single;
        t.spritePixelsPerUnit=ppu;
        t.alphaIsTransparency=true;t.mipmapEnabled=false;t.filterMode=FilterMode.Point;t.textureCompression=TextureImporterCompression.Uncompressed;t.npotScale=TextureImporterNPOTScale.None;
        var settings=new TextureImporterSettings();t.ReadTextureSettings(settings);settings.spriteMeshType=SpriteMeshType.FullRect;settings.spriteAlignment=(int)SpriteAlignment.Custom;settings.spritePivot=pivot;t.SetTextureSettings(settings);t.SaveAndReimport();
    }
    static void DrawLine(Vector3 a,Vector3 b,Color color,int order)
    {
        var line=new GameObject("FIXTURE_GRID").AddComponent<LineRenderer>();line.positionCount=2;line.SetPositions(new[]{a,b});line.startWidth=line.endWidth=.015f;
        line.sharedMaterial=new Material(Shader.Find("Sprites/Default"));line.startColor=line.endColor=color;line.sortingOrder=order;
    }
    static Color32[] Capture(string name,int width,int height)
    {
        var rt=RenderTexture.GetTemporary(width,height,24);var old=RenderTexture.active;camera.targetTexture=rt;camera.Render();RenderTexture.active=rt;
        var t=new Texture2D(width,height,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,width,height),0,0);t.Apply();var colors=t.GetPixels32();
        File.WriteAllBytes(Path.Combine(Output,name+".png"),t.EncodeToPNG());Object.DestroyImmediate(t);camera.targetTexture=null;RenderTexture.active=old;RenderTexture.ReleaseTemporary(rt);return colors;
    }
    static string Hash(byte[] bytes){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(bytes)).Replace("-","").ToLowerInvariant();}
    static void Check(bool ok,string name){Checks.Add((ok?"PASS|":"FAIL|")+name);File.WriteAllLines(Path.Combine(Output,"checks.txt"),Checks);if(!ok)throw new Exception(name);}
}
#endif
