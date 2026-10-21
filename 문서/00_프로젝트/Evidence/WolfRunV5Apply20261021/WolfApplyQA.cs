using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;

// 승인 자산을 실제 제품 Presenter에 연결해 실행하는 격리 검수 전용 코드입니다. 제품에는 포함하지 않습니다.
public sealed class WolfApplyQA : MonoBehaviour
{
    [Serializable] public class Sample { public int trial,frame,engineFrame; public float seconds,x,y,scaleX; public string png; }
    [Serializable] public class Report { public string unity,graphics; public List<string> checks=new List<string>(); public List<Sample> samples=new List<Sample>(); public List<string> errors=new List<string>(); }
    Report report=new Report(); string output; Camera cameraView; RenderTexture rt; Texture2D readback; int trial; float started; int previous=-1; BattleActionPresenter presenter;
    void Check(bool ok,string text) { report.checks.Add((ok?"PASS ":"FAIL ")+text); if(!ok) Debug.LogError("WOLF_QA_FAIL "+text); }
    IEnumerator Start()
    {
        output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../Runtime"));Directory.CreateDirectory(output);
        report.unity=Application.unityVersion;report.graphics=SystemInfo.graphicsDeviceType.ToString();
        Application.logMessageReceived+=(message,stack,type)=>{if(type==LogType.Error||type==LogType.Exception)report.errors.Add(message+"\n"+stack);};
        Application.runInBackground=true;Application.targetFrameRate=120;QualitySettings.vSyncCount=0;AudioListener.volume=0;
        GameSaveService.AuditSaveDirectory=Path.Combine(output,"Saves");UserSettingsService.BeginAudit(Path.Combine(output,"Settings"));
        var wolf=BeastCompanionCatalog.Get("wolf");var sheet=Resources.Load<Texture2D>(wolf.RunResourcePath);
        Check(sheet!=null&&sheet.width==3072&&sheet.height==384,"Imported native 3072x384 without downscale");
        Check(wolf.FrameWidth==512&&wolf.FramesPerSecond==12&&wolf.DisplayScale==.25f&&wolf.TravelSpeed==570,"Wolf catalog contract");
        Check(wolf.IconResourcePath=="CompanionAssaultValidation/Wolf_Run"&&wolf.IconFrameWidth==64&&wolf.IconFrameIndex==4,"Legacy icon contract preserved");
        Check(Resources.Load<Texture2D>(wolf.IconResourcePath).width==384,"Legacy icon still loads original sheet");
        var method=typeof(BattleActionPresenter).GetMethod("SliceBeastRunSheet",BindingFlags.Static|BindingFlags.NonPublic);
        var frames=(Sprite[])method.Invoke(null,new object[]{sheet,wolf});Check(frames.Length==6,"Actual product slicer returns six frames");
        for(int i=0;i<6;i++){Check(frames[i].rect==new Rect(i*512,0,512,384),"Frame rect "+i);Destroy(frames[i]);}
        cameraView=new GameObject("QA_Camera").AddComponent<Camera>();cameraView.orthographic=true;cameraView.orthographicSize=360;cameraView.transform.position=new Vector3(0,0,-100);cameraView.clearFlags=CameraClearFlags.SolidColor;cameraView.backgroundColor=new Color(.16f,.19f,.22f);
        rt=new RenderTexture(1280,720,24);rt.Create();cameraView.targetTexture=rt;readback=new Texture2D(1280,720,TextureFormat.RGB24,false);
        var canvas=new GameObject("ActualPresenterCanvas",typeof(Canvas)).GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=cameraView;canvas.planeDistance=10;
        var actor=Box(canvas.transform,"Actor",new Vector2(450,0),Color.cyan);var target=Box(canvas.transform,"Target",new Vector2(-450,0),Color.gray);
        presenter=new GameObject("ActualProductPresenter").AddComponent<BattleActionPresenter>();
        var font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        for(trial=0;trial<3;trial++)
        {
            actor.localPosition=trial==0?new Vector3(450,0):trial==1?new Vector3(-450,0):new Vector3(380,75);
            target.localPosition=trial==0?new Vector3(-450,0):trial==1?new Vector3(450,0):new Vector3(-380,-30);
            var actorOrigin=actor.localPosition;var targetOrigin=target.localPosition;int impacts=0,notifications=0,completions=0;previous=-1;started=Time.time;
            yield return presenter.PlayBeastCompanionAssault(actor,target,target.GetComponent<Image>(),font,wolf,()=>{impacts++;return 0;},damage=>notifications++,()=>completions++);
            yield return null;
            Check(impacts==1&&notifications==1&&completions==1,"Trial "+trial+" impact and completion once");
            Check(actor.localPosition==actorOrigin&&target.localPosition==targetOrigin,"Trial "+trial+" actor and target position preserved");
            Check(GameObject.Find("BeastCompanion_wolf")==null,"Trial "+trial+" transient Wolf removed");
        }
        File.WriteAllText(Path.Combine(output,"RuntimeQA.json"),JsonUtility.ToJson(report,true));
        Debug.Log("WOLF_APPLY_QA_COMPLETE "+report.checks.Count);
        GameSaveService.FinishAudit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying=false;
#endif
    }
    RectTransform Box(Transform parent,string name,Vector2 position,Color color)
    {
        var image=new GameObject(name,typeof(Image)).GetComponent<Image>();image.transform.SetParent(parent,false);image.color=color;image.rectTransform.sizeDelta=new Vector2(80,120);image.rectTransform.localPosition=position;return image.rectTransform;
    }
    void LateUpdate()
    {
        var wolfObject=GameObject.Find("BeastCompanion_wolf");if(wolfObject==null||cameraView==null)return;
        var image=wolfObject.GetComponent<Image>();if(image.sprite==null)return;int index=Mathf.RoundToInt(image.sprite.rect.x/512);if(index==previous)return;previous=index;
        var rect=image.rectTransform;bool right=trial==1;Check((rect.localScale.x<0)==right,"Trial "+trial+" frame "+index+" direction");
        Check(rect.sizeDelta==new Vector2(128,96),"Trial "+trial+" display 128x96");
        Canvas.ForceUpdateCanvases();cameraView.Render();var old=RenderTexture.active;RenderTexture.active=rt;readback.ReadPixels(new Rect(0,0,1280,720),0,0);readback.Apply();RenderTexture.active=old;
        string name=$"trial{trial}_{report.samples.Count:000}_frame{index}.png";File.WriteAllBytes(Path.Combine(output,name),readback.EncodeToPNG());
        report.samples.Add(new Sample{trial=trial,frame=index,engineFrame=Time.frameCount,seconds=Time.time-started,x=rect.localPosition.x,y=rect.localPosition.y,scaleX=rect.localScale.x,png=name});
    }
}
