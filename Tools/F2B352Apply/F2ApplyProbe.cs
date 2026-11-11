#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// #75 전용 격리 검증기입니다. 원본 프로젝트와 과거 전체 QA를 실행하지 않습니다.
[InitializeOnLoad]
public static class F2ApplyBatch
{
    const string Key = "Limitless75ApplyArmed";
    static double deadline;
    static F2ApplyBatch()
    {
        EditorApplication.playModeStateChanged += state => {
            if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool(Key, false)) return;
            deadline = EditorApplication.timeSinceStartup + 180;
            EditorApplication.update += Poll;
        };
    }
    public static void Run()
    {
        if (!Application.isBatchMode || Application.unityVersion != "6000.6.5f1" ||
            !Application.dataPath.Replace('\\','/').EndsWith("/Temp/F2B352Apply20261111/QA/Client/Assets"))
            throw new Exception("#75 B/C 격리 batch 프로젝트에서만 허용합니다.");
        const string scenePath="Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity";
        var scene=EditorSceneManager.OpenScene(scenePath,OpenSceneMode.Single);
        var components=scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<Component>(true)).ToArray();
        string before=string.Join("\n",components.Select(x=>EditorJsonUtility.ToJson(x)));
        if(scene.GetRootGameObjects().Any(x=>x.GetComponent<F2SnowPathBuilder>()!=null))throw new Exception("Builder 중복 Scene 적용 금지");
        var root=new GameObject("F2_ApprovedSnowPath");SceneManager.MoveGameObjectToScene(root,scene);
        var builder=root.AddComponent<F2SnowPathBuilder>();var serialized=new SerializedObject(builder);
        var ground=serialized.FindProperty("groundSprites");ground.arraySize=9;int index=0;
        foreach(var kind in new[]{"Earth","ThinFrost","Gravel"})for(int i=1;i<=3;i++)
            ground.GetArrayElementAtIndex(index++).objectReferenceValue=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Resources/FrostwayVillageV10/Sprites/Frostway_Ground_"+kind+"_0"+i+".png");
        serialized.FindProperty("firstSnow").objectReferenceValue=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Resources/F2_FirstSnow_GroundOverlay_v1_CANDIDATE.png");
        serialized.FindProperty("wornTrail").objectReferenceValue=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Art/Environment/Chapter3/NorthF1/NorthF1_Path_WornTrail_Overlay_v2.png");serialized.ApplyModifiedPropertiesWithoutUndo();
        string after=string.Join("\n",components.Select(x=>EditorJsonUtility.ToJson(x)));
        if(before!=after)throw new Exception("기존 Scene 컴포넌트 변경 감지");
        if(!EditorSceneManager.SaveScene(scene))throw new Exception("격리 Scene 저장 실패");
        string target=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Worktree/Unity/Client/Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity"));
        File.Copy(scenePath,target,true);
        Directory.CreateDirectory(F2ApplyProbe.Output);File.WriteAllText(F2ApplyProbe.Output+"/scene-authoring.txt","PASS ExistingSerializedComponentsUnchanged\nPASS OneRootOneBuilderAddedByUnityEditor\n");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        new GameObject("F2_B352_ApplyDeltaOnly").AddComponent<F2ApplyProbe>();
        SessionState.SetBool(Key,true); EditorApplication.isPlaying = true;
    }
    static void Poll()
    {
        var p = Object.FindAnyObjectByType<F2ApplyProbe>();
        if (p != null && p.Done) {
            SessionState.SetBool(Key,false); EditorApplication.update -= Poll;
            int exit = p.Failed ? 1 : 0; EditorApplication.isPlaying = false;
            EditorApplication.delayCall += () => EditorApplication.Exit(exit);
        } else if (EditorApplication.timeSinceStartup > deadline) {
            File.WriteAllText(F2ApplyProbe.Output + "/timeout.txt","timeout"); EditorApplication.Exit(2);
        }
    }
}

[DefaultExecutionOrder(-10000)]
public sealed class F2ApplyProbe : MonoBehaviour
{
    public static string Output => Path.GetFullPath(Path.Combine(Application.dataPath,"../../Evidence"));
    public bool Done, Failed;
    readonly List<string> checks = new List<string>(), errors = new List<string>(), trace = new List<string>();
    IEnumerator routine;
    EditorWindow gameView;
    GameObject reuse;
    string colliders;
    PlayerController Player => Object.FindAnyObjectByType<PlayerController>();
    const BindingFlags Flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
    void Awake()
    {
        DontDestroyOnLoad(gameObject); Directory.CreateDirectory(Output);
        GameSaveService.AuditSaveDirectory = Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSaves"));
        UserSettingsService.BeginAudit(Path.GetFullPath(Path.Combine(Application.dataPath,"../../SyntheticSettings")));
        UserSettingsService.SetMuteAll(true); AudioListener.volume = 0;
        Application.runInBackground = true; QualitySettings.vSyncCount = 0; Application.targetFrameRate = 60;
        Application.logMessageReceived += (text,stack,type) => {
            if (type != LogType.Error && type != LogType.Exception && type != LogType.Assert) return;
            if (stack.Contains("UnityEditor.Search.SearchDatabase")) File.AppendAllText(Output+"/EnvironmentErrors.txt",text+"\n"+stack);
            else errors.Add(text+"\n"+stack);
        };
    }
    void Start() { routine = Flatten(Run()); }
    void Update()
    {
        if (Done || routine == null) return;
        try { if (!routine.MoveNext()) Finish(); }
        catch (Exception e) { errors.Add(e.ToString()); Check(false,"HarnessException"); Finish(); }
    }
    void Finish()
    {
        Done = true; Failed = checks.Any(x=>x.StartsWith("FAIL")) || errors.Count > 0;
        File.WriteAllLines(Output+"/checks.txt",checks); File.WriteAllLines(Output+"/Errors.txt",errors);
        File.WriteAllLines(Output+"/physics-trace.csv",trace);
        File.WriteAllText(Output+"/status.txt",Failed?"FAIL":"PASS");
    }
    void Check(bool pass,string name) { checks.Add((pass?"PASS ":"FAIL ")+name); File.WriteAllLines(Output+"/checks.txt",checks); }
    static IEnumerator Flatten(IEnumerator e)
    {
        var stack = new Stack<IEnumerator>(); stack.Push(e);
        while (stack.Count>0) { var top=stack.Peek(); if(!top.MoveNext()){stack.Pop();continue;} if(top.Current is IEnumerator n)stack.Push(n);else yield return top.Current; }
    }
    static IEnumerator Wait(float seconds) { float end=Time.realtimeSinceStartup+seconds;while(Time.realtimeSinceStartup<end)yield return null; }
    void At(Vector2 p) { Player.SetGuidedMovement(Vector2.zero); Player.transform.position=p;var rb=Player.GetComponent<Rigidbody2D>();rb.position=p;rb.linearVelocity=Vector2.zero;Physics2D.SyncTransforms(); }
    string ColliderState() => string.Join("\n",Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None).Where(c=>!c.transform.IsChildOf(Player.transform)).OrderBy(c=>c.GetEntityId().ToString()).Select(c=>c.GetEntityId()+"|"+c.name+"|"+c.enabled+"|"+c.isTrigger+"|"+c.transform.position+"|"+c.transform.lossyScale+"|"+c.offset+"|"+c.bounds));

    // GameView 자체의 PlayModeView 렌더 경로를 사용합니다. Camera.Render나 오프라인 UI 합성이 아닙니다.
    void SetSize(int width)
    {
        var a=typeof(EditorWindow).Assembly;var t=a.GetType("UnityEditor.GameView");
        if(gameView==null)gameView=EditorWindow.GetWindow(t,false,"F2 B C QA",false);
        var sizesType=a.GetType("UnityEditor.GameViewSizes");
        var singleton=typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var sizes=singleton.GetProperty("instance",Flags).GetValue(null,null);
        var groupType=a.GetType("UnityEditor.GameViewSizeGroupType");
        var group=sizesType.GetMethod("GetGroup",Flags).Invoke(sizes,new[]{Enum.Parse(groupType,"Standalone")});
        int count=(int)group.GetType().GetMethod("GetTotalCount",Flags).Invoke(group,null);
        var sizeType=a.GetType("UnityEditor.GameViewSize");var kind=a.GetType("UnityEditor.GameViewSizeType");
        var size=Activator.CreateInstance(sizeType,Flags,null,new object[]{Enum.Parse(kind,"FixedResolution"),width,720,"F2BC_"+width},null);
        group.GetType().GetMethod("AddCustomSize",Flags).Invoke(group,new[]{size});
        t.GetProperty("selectedSizeIndex",Flags).SetValue(gameView,count,null);
        t.GetProperty("targetSize",Flags).SetValue(gameView,new Vector2(width,720),null);
        gameView.position=new Rect(0,0,width,750);gameView.Repaint();
        Camera.main.aspect=width/720f;
    }
    void Capture(string variant,int width)
    {
        Camera.main.transform.position=new Vector3(2,0,-10);Camera.main.orthographicSize=5;
        Canvas.ForceUpdateCanvases();
        var rt=(RenderTexture)gameView.GetType().GetMethod("RenderView",Flags).Invoke(gameView,new object[]{Vector2.zero,false});
        if(rt==null)throw new Exception("실제 GameView RenderView가 RenderTexture를 반환하지 않았습니다.");
        var old=RenderTexture.active;RenderTexture.active=rt;var tex=new Texture2D(rt.width,rt.height,TextureFormat.RGBA32,false);
        tex.ReadPixels(new Rect(0,0,rt.width,rt.height),0,0);tex.Apply();
        // D3D의 GameView RT 저장 방향만 정규화합니다. 게임 내용/UI를 합성하거나 수정하지 않습니다.
        if(SystemInfo.graphicsUVStartsAtTop) {
            var pixels=tex.GetPixels32();int stride=tex.width;
            for(int y=0;y<tex.height/2;y++)for(int x=0;x<stride;x++) {
                int a=y*stride+x,b=(tex.height-1-y)*stride+x;var value=pixels[a];pixels[a]=pixels[b];pixels[b]=value;
            }
            tex.SetPixels32(pixels);tex.Apply();
        }
        File.WriteAllBytes(Output+"/"+variant+"_GameView_"+width+"x720.png",tex.EncodeToPNG());RenderTexture.active=old;Destroy(tex);
        Check(rt.width==width&&rt.height==720,variant+".ActualGameViewResolution."+width);
        var panel=GameObject.Find("Main23Guidance").transform.Find("Panel").GetComponent<RectTransform>();
        var corners=new Vector3[4];panel.GetWorldCorners(corners);
        Check(corners.All(x=>x.x>=0&&x.x<=width&&x.y>=0&&x.y<=720),variant+".HudInViewport."+width);
        File.AppendAllText(Output+"/hud-layout.txt",width+"|"+panel.anchoredPosition+"|"+panel.sizeDelta+"|"+string.Join(";",corners.Select(x=>x.ToString("F3")))+"\n");
        Check(Camera.main.targetTexture==null,variant+".NoCameraRTSubstitution."+width);
        File.AppendAllText(Output+"/capture-metadata.txt",variant+"|"+width+"x720|UnityEditor.GameView.RenderView|batch="+Application.isBatchMode+"|camera="+Camera.main.transform.position+"|ortho="+Camera.main.orthographicSize+"|player="+Player.transform.position+"|screen="+Screen.width+"x"+Screen.height+"|overlayCanvases="+Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Count(c=>c.renderMode==RenderMode.ScreenSpaceOverlay)+"\n");
    }
    IEnumerator Captures(string variant)
    {
        At(new Vector2(2,0));yield return Wait(.25f);
        foreach(int width in new[]{1280,960,1680}) {
            SetSize(width);yield return Wait(.5f);
            gameView.GetType().GetMethod("RenderView",Flags).Invoke(gameView,new object[]{Vector2.zero,true});
            yield return Wait(.5f);Capture(variant,width);
        }
    }
    IEnumerator WheelRoute(string variant)
    {
        SetSize(1280);At(new Vector2(0,-3));yield return Wait(.1f);
        var points=new[]{Vector2.zero,new Vector2(3,0),new Vector2(3,3),new Vector2(3,0),Vector2.zero,new Vector2(0,-3)};
        bool pass=true;int samples=0;foreach(var target in points) {
            float end=Time.realtimeSinceStartup+8;
            while(Vector2.Distance(Player.transform.position,target)>.1f&&Time.realtimeSinceStartup<end) {
                Player.SetGuidedMovement((target-(Vector2)Player.transform.position).normalized);
                if(samples++%5==0)trace.Add(variant+","+Time.frameCount+","+Player.transform.position.x+","+Player.transform.position.y);
                yield return null;
            }
            Player.SetGuidedMovement(Vector2.zero);if(Vector2.Distance(Player.transform.position,target)>.2f)pass=false;
            yield return Wait(.05f);
        }
        Check(pass,variant+".WheelchairOneLocalRoundTrip");Check(ColliderState()==colliders,variant+".ExistingCollidersUnchanged");

    }
    IEnumerator Run()
    {
        Check(Application.unityVersion=="6000.6.5f1","Unity6000.6.5f1");
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"F2 B C 비교");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(14,0);
        QuestService.ImportSaveData(new QuestProgressSaveData{CompletedQuestIds=new[]{Chapter3Main23Flow.QuestId,Chapter3Main22Flow.QuestId,Chapter2Main21Flow.QuestId}});QuestService.TryStart(Chapter3Main24Flow.QuestId);
        var seed=QuestService.ExportSaveData();foreach(var o in seed.ActiveQuests.Single(q=>q.QuestId==Chapter3Main24Flow.QuestId).Objectives)if(o.ObjectiveId!="await_f2_opening")o.CurrentCount=1;QuestService.ImportSaveData(seed);
        // 실제 SceneTransitionService로 마을에서 F2에 입장합니다. Builder.Build를 수동 호출하지 않습니다.
        var village=SceneManager.LoadSceneAsync(Chapter3Main23Flow.Village);while(!village.isDone)yield return null;yield return Wait(.3f);
        Check(Object.FindObjectsByType<F2SnowPathBuilder>(FindObjectsSortMode.None).Length==0,"Village.NoBuilder");
        yield return EnterF2();
        var builder=Object.FindAnyObjectByType<F2SnowPathBuilder>();reuse=builder.gameObject;
        Check(builder.MovedSnowCount==14&&reuse.GetComponentsInChildren<Transform>().Length==352,"Entry.AutoStart352AndApproved14");
        Check(Object.FindObjectsByType<F2SnowPathBuilder>(FindObjectsSortMode.None).Length==1,"Entry.OneBuilder");
        Check(DisabledFour(),"Entry.ExactFourPlaceholdersHidden");
        Check(Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Count(x=>x.name=="PLACEHOLDER_Tree_ValidationOnly"&&x.enabled)==8,"Entry.EightExistingTreesVisible");
        Check(Object.FindObjectsByType<Renderer>(FindObjectsSortMode.None).All(x=>x.name!="SnowPine_REUSE"&&x.name!="FrostShrub_REUSE"&&x.name!="C_GameEdge256_ONLY"),"Entry.NoExcludedEnvironment");
        Check(Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).All(x=>x.GetType().Name!="F2ReuseHudLayout"),"Entry.NoHudLayout");
        foreach(var anim in Object.FindObjectsByType<Animator>(FindObjectsSortMode.None))anim.enabled=false;
        foreach(var component in Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None))if(component.GetType().Name=="MonsterSpriteSheetAnimation")component.enabled=false;
        colliders=ColliderState();string existing=ExistingState();
        File.WriteAllText(Output+"/sprites.txt",SpriteState(reuse));
        var meshes=reuse.GetComponentsInChildren<MeshFilter>().Select(x=>x.sharedMesh).ToArray();
        var materials=reuse.GetComponentsInChildren<MeshRenderer>().Select(x=>x.sharedMaterial).Distinct().ToArray();
        builder.Build();yield return null;
        Check(reuse.GetComponentsInChildren<Transform>().Length==352&&meshes.SequenceEqual(reuse.GetComponentsInChildren<MeshFilter>().Select(x=>x.sharedMesh)),"Entry.BuildDoesNotDuplicate");
        Check(meshes.Length==3&&materials.Length==3,"Entry.Mesh3Material3");
        yield return Captures("Applied");
        Check(ExistingState()==existing,"Capture.ExistingTreesStumpOwlReferencesUnchanged");
        yield return WheelRoute("Applied");
        Check(ExistingState()==existing,"Movement.ExistingTreesStumpOwlUnchanged");
        yield return LeaveF2();
        Check(meshes.All(x=>x==null)&&materials.All(x=>x==null),"Exit.OwnedMeshesMaterialsDestroyed");
        Check(Object.FindObjectsByType<F2SnowPathBuilder>(FindObjectsSortMode.None).Length==0,"Exit.BuilderDestroyed");
        Check(Resources.FindObjectsOfTypeAll<Mesh>().Count(x=>x.name=="F2_ApprovedTrail_Combined")==0,"Exit.NoCombinedMeshLeak");
        yield return EnterF2();builder=Object.FindAnyObjectByType<F2SnowPathBuilder>();reuse=builder.gameObject;
        Check(reuse.GetComponentsInChildren<Transform>().Length==352&&builder.MovedSnowCount==14,"Reentry.AutoStart352Once");
        Check(Object.FindObjectsByType<F2SnowPathBuilder>(FindObjectsSortMode.None).Length==1&&Resources.FindObjectsOfTypeAll<Mesh>().Count(x=>x.name=="F2_ApprovedTrail_Combined")==3,"Reentry.NoAccumulation");
        Check(DisabledFour(),"Reentry.FourHidden");
        meshes=reuse.GetComponentsInChildren<MeshFilter>().Select(x=>x.sharedMesh).ToArray();materials=reuse.GetComponentsInChildren<MeshRenderer>().Select(x=>x.sharedMaterial).Distinct().ToArray();
        yield return LeaveF2();
        Check(meshes.All(x=>x==null)&&materials.All(x=>x==null),"SecondExit.ResourcesDestroyed");
        Check(errors.Count==0,"NewRuntimeErrorsZero");
    }
    IEnumerator EnterF2()
    {
        Check(SceneTransitionService.TryLoad(Main24F2Traversal.SceneId,Main24F2Traversal.EntrySpawn),"Travel.EnterAccepted."+Time.frameCount);
        float deadline=Time.realtimeSinceStartup+15;while(SceneManager.GetActiveScene().name!=Main24F2Traversal.SceneId&&Time.realtimeSinceStartup<deadline)yield return null;
        yield return Wait(.5f);if(SceneManager.GetActiveScene().name!=Main24F2Traversal.SceneId)throw new Exception("F2 입장 실패");
    }
    IEnumerator LeaveF2()
    {
        Check(SceneTransitionService.TryLoad(Chapter3Main23Flow.Village,Main24F2Traversal.VillageSpawn),"Travel.LeaveAccepted."+Time.frameCount);
        float deadline=Time.realtimeSinceStartup+15;while(SceneManager.GetActiveScene().name!=Chapter3Main23Flow.Village&&Time.realtimeSinceStartup<deadline)yield return null;
        yield return Wait(.3f);if(SceneManager.GetActiveScene().name!=Chapter3Main23Flow.Village)throw new Exception("마을 복귀 실패");
    }
    static bool IsHiddenTarget(string name)=>name=="PLACEHOLDER_SnowGround_ValidationOnly"||name.StartsWith("PLACEHOLDER_FirmTrail_");
    bool DisabledFour(){var all=Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Where(x=>IsHiddenTarget(x.name)).ToArray();return all.Length==4&&all.All(x=>!x.enabled);}
    string ExistingState()=>string.Join("\n",Object.FindObjectsByType<SpriteRenderer>(FindObjectsSortMode.None).Where(x=>!IsHiddenTarget(x.name)&&!x.transform.IsChildOf(Player.transform)&&!x.name.StartsWith("Ground_")&&!x.name.StartsWith("FirstSnow_")&&!x.name.StartsWith("Snow_PathEdge")&&!x.name.StartsWith("Worn_")).OrderBy(x=>x.GetEntityId().ToString()).Select(x=>x.GetEntityId()+"|"+x.name+"|"+x.enabled+"|"+x.transform.position+"|"+x.transform.rotation+"|"+x.transform.lossyScale+"|"+AssetDatabase.GetAssetPath(x.sprite)+"|"+x.sortingOrder));
    string SpriteState(GameObject root)=>string.Join("\n",root.GetComponentsInChildren<SpriteRenderer>().Select(x=>x.name+"|"+x.transform.position.ToString("F6")+"|"+x.transform.rotation.ToString("F6")+"|"+x.transform.lossyScale.ToString("F6")+"|"+x.sortingOrder+"|"+x.sprite.texture.imageContentsHash+"|"+x.sprite.pivot.ToString("F6")).OrderBy(x=>x));
}
#endif
