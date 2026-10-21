using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// 본게임과 연결하지 않고 Unity Play Mode의 실제 시간으로 12FPS 교체를 기록하는 격리 검수기입니다.
public sealed class WolfPlaybackHarness : MonoBehaviour
{
    [Serializable] public sealed class Sample { public double seconds; public int frame; public int engineFrame; public string png; }
    [Serializable] public sealed class Result { public string unityVersion; public string graphics; public int fps=12; public int width=512,height=288; public float cellDisplayPixels=128,iconDisplayPixels=40; public double elapsed; public List<Sample> samples=new List<Sample>(); }
    Sprite[] oldFrames=new Sprite[6],newFrames=new Sprite[6];
    SpriteRenderer[] renderers=new SpriteRenderer[4]; Camera cameraView; RenderTexture target; Texture2D capture;
    double started; int previous=-1; Result result=new Result(); string output;
    void Start()
    {
        Application.runInBackground=true; Application.targetFrameRate=120; QualitySettings.vSyncCount=0;
        string root=Directory.GetParent(Application.dataPath).FullName;
        output=Path.Combine(root,"Output"); Directory.CreateDirectory(output);
        for(int i=0;i<6;i++)
        {
            oldFrames[i]=Load(Path.Combine(root,"Input","v2",$"Wolf_Run_{i:00}_v3.png"));
            newFrames[i]=Load(Path.Combine(root,"Input","v3",$"Wolf_Run_{i:00}_v4.png"));
        }
        for(int i=0;i<4;i++)
        {
            var go=new GameObject("Wolf_"+i);renderers[i]=go.AddComponent<SpriteRenderer>();
            go.transform.position=new Vector3(i%2==0?-2:2,i<2?.3f:-1.25f,0);
            go.transform.localScale=Vector3.one*(i<2?1:40f/128);
        }
        cameraView=new GameObject("QA_Camera").AddComponent<Camera>();cameraView.orthographic=true;
        cameraView.orthographicSize=2.25f;cameraView.transform.position=new Vector3(0,0,-10);
        cameraView.clearFlags=CameraClearFlags.SolidColor;cameraView.backgroundColor=new Color(.15f,.18f,.21f);
        target=new RenderTexture(512,288,24);target.Create();cameraView.targetTexture=target;
        capture=new Texture2D(512,288,TextureFormat.RGB24,false);
        result.unityVersion=Application.unityVersion;result.graphics=SystemInfo.graphicsDeviceType.ToString();
        started=Time.realtimeSinceStartupAsDouble;
    }
    Sprite Load(string path)
    {
        var texture=new Texture2D(2,2,TextureFormat.RGBA32,false);texture.LoadImage(File.ReadAllBytes(path));
        texture.filterMode=FilterMode.Bilinear;
        return Sprite.Create(texture,new Rect(0,0,512,384),new Vector2(.5f,.14f),256,0,SpriteMeshType.FullRect);
    }
    void Update()
    {
        if(cameraView==null)return;
        double elapsed=Time.realtimeSinceStartupAsDouble-started;int frame=(int)Math.Floor(elapsed*12)%6;
        for(int i=0;i<4;i++)renderers[i].sprite=i%2==0?oldFrames[frame]:newFrames[frame];
        if(frame!=previous)
        {
            cameraView.Render();var old=RenderTexture.active;RenderTexture.active=target;
            capture.ReadPixels(new Rect(0,0,512,288),0,0);capture.Apply();RenderTexture.active=old;
            string name=$"capture_{result.samples.Count:000}_{frame:00}.png";
            File.WriteAllBytes(Path.Combine(output,name),capture.EncodeToPNG());
            result.samples.Add(new Sample{seconds=elapsed,frame=frame,engineFrame=Time.frameCount,png=name});previous=frame;
        }
        if(elapsed<2.1)return;
        result.elapsed=elapsed;File.WriteAllText(Path.Combine(output,"Playback.json"),JsonUtility.ToJson(result,true));
        Debug.Log("WOLF_QA_PLAYBACK_COMPLETE "+result.samples.Count);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
#endif
        enabled=false;
    }
}
