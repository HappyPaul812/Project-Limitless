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
using ProjectLimitless.Monster;
using ProjectLimitless.Battle;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

// #97 맹독뱀 전용 격리 검증기입니다. 원본 프로젝트와 과거 전체 QA를 실행하지 않습니다.
[InitializeOnLoad]
public static class SnakeBatch
{
    const string Key = "Limitless97Armed";
    static double deadline;
    static SnakeBatch()
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
            !Application.dataPath.Replace('\\','/').EndsWith("/Temp/Issue97/QA/Client/Assets"))
            throw new Exception("#97 맹독뱀 격리 batch 프로젝트에서만 허용합니다.");
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        new GameObject("Snake97_DeltaOnly").AddComponent<SnakeProbe>();
        SessionState.SetBool(Key,true); EditorApplication.isPlaying = true;
    }
    public static void Supplement() { SessionState.SetBool("Snake97Supplement",true); Run(); }
    public static void PostApply() { SessionState.SetBool("Snake97PostApply",true); SessionState.SetBool("Snake97Supplement",true); Run(); }
    static void Poll()
    {
        var p = Object.FindAnyObjectByType<SnakeProbe>();
        if (p != null && p.Done) {
            SessionState.SetBool(Key,false); EditorApplication.update -= Poll;
            int exit = p.Failed ? 1 : 0; EditorApplication.isPlaying = false;
            EditorApplication.delayCall += () => EditorApplication.Exit(exit);
        } else if (EditorApplication.timeSinceStartup > deadline) {
            File.WriteAllText(SnakeProbe.Output + "/timeout.txt","timeout"); EditorApplication.Exit(2);
        }
    }
}

[DefaultExecutionOrder(-10000)]
public sealed class SnakeProbe : MonoBehaviour
{
    public static string Output => Path.GetFullPath(Path.Combine(Application.dataPath,SessionState.GetBool("Snake97PostApply",false)?"../../PostApplyEvidence":SessionState.GetBool("Snake97Supplement",false)?"../../SupplementEvidence":"../../Evidence"));
    public bool Done, Failed;
    readonly List<string> checks = new List<string>(), errors = new List<string>(), trace = new List<string>();
    IEnumerator routine;
    EditorWindow gameView;

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
        File.WriteAllLines(Output+"/animation-trace.csv",trace);
        File.WriteAllText(Output+"/status.txt",Failed?"FAIL":"PASS");
    }
    void Check(bool pass,string name) { checks.Add((pass?"PASS ":"FAIL ")+name); File.WriteAllLines(Output+"/checks.txt",checks); }
    static IEnumerator Flatten(IEnumerator e)
    {
        var stack = new Stack<IEnumerator>(); stack.Push(e);
        while (stack.Count>0) { var top=stack.Peek(); if(!top.MoveNext()){stack.Pop();continue;} if(top.Current is IEnumerator n)stack.Push(n);else yield return top.Current; }
    }
    static IEnumerator Wait(float seconds) { float end=Time.realtimeSinceStartup+seconds;while(Time.realtimeSinceStartup<end)yield return null; }
    // GameView 자체의 PlayModeView 렌더 경로를 사용합니다. Camera.Render나 오프라인 UI 합성이 아닙니다.
    void SetSize(int width,int height=720)
    {
        var a=typeof(EditorWindow).Assembly;var t=a.GetType("UnityEditor.GameView");
        if(gameView==null)gameView=EditorWindow.GetWindow(t,false,"VenomSnake97 QA",false);
        var sizesType=a.GetType("UnityEditor.GameViewSizes");
        var singleton=typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var sizes=singleton.GetProperty("instance",Flags).GetValue(null,null);
        var groupType=a.GetType("UnityEditor.GameViewSizeGroupType");
        var group=sizesType.GetMethod("GetGroup",Flags).Invoke(sizes,new[]{Enum.Parse(groupType,"Standalone")});
        int count=(int)group.GetType().GetMethod("GetTotalCount",Flags).Invoke(group,null);
        var sizeType=a.GetType("UnityEditor.GameViewSize");var kind=a.GetType("UnityEditor.GameViewSizeType");
        var size=Activator.CreateInstance(sizeType,Flags,null,new object[]{Enum.Parse(kind,"FixedResolution"),width,height,"Snake97_"+width},null);
        group.GetType().GetMethod("AddCustomSize",Flags).Invoke(group,new[]{size});
        t.GetProperty("selectedSizeIndex",Flags).SetValue(gameView,count,null);
        t.GetProperty("targetSize",Flags).SetValue(gameView,new Vector2(width,height),null);
        gameView.position=new Rect(0,0,width,height+30);gameView.Repaint();
        Camera.main.aspect=width/(float)height;
    }
    void Capture(string variant,int width,int height=720)
    {

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
        File.WriteAllBytes(Output+"/"+variant+"_GameView_"+width+"x"+height+".png",tex.EncodeToPNG());RenderTexture.active=old;Destroy(tex);
        Check(rt.width==width&&rt.height==height,variant+".ActualGameViewResolution."+width);
        Check(Camera.main.targetTexture==null,variant+".NoCameraRTSubstitution."+width);
        File.AppendAllText(Output+"/capture-metadata.txt",variant+"|"+width+"x"+height+"|UnityEditor.GameView.RenderView|batch="+Application.isBatchMode+"|camera="+Camera.main.transform.position+"|ortho="+Camera.main.orthographicSize+"|screen="+Screen.width+"x"+Screen.height+"|overlayCanvases="+Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None).Count(c=>c.renderMode==RenderMode.ScreenSpaceOverlay)+"\n");
    }

    object Get(object o,string name) => o.GetType().GetField(name,Flags).GetValue(o);
    void Set(object o,string name,object value) => o.GetType().GetField(name,Flags).SetValue(o,value);
    IEnumerator Shot(string label) {
        SetSize(1280);yield return Wait(.5f);
        gameView.GetType().GetMethod("RenderView",Flags).Invoke(gameView,new object[]{Vector2.zero,true});
        yield return Wait(.5f);Capture(label,1280);
    }
    IEnumerator Animate(MonsterSpriteSheetAnimation anim,string label) {
        var seen=new HashSet<int>();float start=Time.realtimeSinceStartup;
        while(Time.realtimeSinceStartup-start<1.1f) {
            seen.Add(anim.CurrentFrameIndex);
            trace.Add(label+",Idle,"+(Time.realtimeSinceStartup-start)+","+anim.CurrentFrameIndex+","+anim.CurrentFrame.name);
            yield return null;
        }
        Check(seen.SetEquals(new[]{0,1,2,3}),label+".IdleFourFramesLoop");
        anim.PlayAttack(true);seen.Clear();start=Time.realtimeSinceStartup;var changes=new List<float>();int previous=-1;
        while(anim.IsAttackPlaying&&Time.realtimeSinceStartup-start<1) {
            if(previous!=anim.CurrentFrameIndex){changes.Add(Time.realtimeSinceStartup-start);previous=anim.CurrentFrameIndex;}
            seen.Add(anim.CurrentFrameIndex);trace.Add(label+",Attack,"+(Time.realtimeSinceStartup-start)+","+anim.CurrentFrameIndex+","+anim.CurrentFrame.name);yield return null;
        }
        float duration=Time.realtimeSinceStartup-start;
        Check(seen.SetEquals(new[]{0,1,2,3})&&duration>=.47f&&duration<.58f,label+".AttackFourFrames8FPS.Duration="+duration);
        Check(changes.Count==4&&changes.Skip(1).Zip(changes,(a,b)=>a-b).All(x=>x>=.085f&&x<=.17f),label+".EachFrame125msTolerance");
        Check(!anim.IsAttackPlaying&&anim.CurrentFrame.name.Contains("Idle"),label+".AutomaticReturnIdle");
    }
    IEnumerator Run() { if(SessionState.GetBool("Snake97Supplement",false)) { yield return Supplement();yield break; } yield return Full(); }

    IEnumerator Supplement() {
        Time.timeScale=1;
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Snake97 QA");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(8,0);
        var d=Resources.Load<MonsterDefinition>("MonsterDefinitions/04_VenomSnake");
        var load=SceneManager.LoadSceneAsync("Field_02");while(!load.isDone)yield return null;yield return Wait(.3f);
        Player.transform.position=new Vector3(-4,-1.5f,0);Player.GetComponent<Rigidbody2D>().position=Player.transform.position;
        var snake=Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None).First(m=>m.Definition.MonsterId=="venom_snake");
        foreach(var m in Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None))m.enabled=false;
        snake.transform.position=new Vector3(-2.2f,.3f,0);snake.GetComponent<Rigidbody2D>().position=snake.transform.position;
        Check(d.IdleSpriteSheet.width==2560&&d.IdleSpriteSheet.height==640&&d.AttackSpriteSheet.width==2560&&d.AttackSpriteSheet.height==640,"ImportedSheetsNoResizeOrClip");
        foreach(var res in new[]{new Vector2Int(800,450),new Vector2Int(960,540)}) {
            SetSize(res.x,res.y);yield return Wait(.5f);gameView.GetType().GetMethod("RenderView",Flags).Invoke(gameView,new object[]{Vector2.zero,true});yield return Wait(.5f);
            Capture("Field_Readability",res.x,res.y);
            float pixels=.71859375f*res.y/(Camera.main.orthographicSize*2);
            Check(pixels>=31&&pixels<=40,"ActualFieldReadabilityPixels."+pixels+".Resolution="+res);
        }
        BattleEncounterContext.Set(d,snake.SpawnDefinition,Player.GetComponentInChildren<Animator>()?.runtimeAnimatorController,Player.GetComponent<PlayerVisualController>()?.ActiveDefaultSprite,Player.transform.position,snake.transform.position);
        load=SceneManager.LoadSceneAsync("Battle");while(!load.isDone)yield return null;
        // 기존 동률 연출을 실제로 끝낸 뒤 검사하므로 전투 Sprite가 가려진 사진을 통과시키지 않습니다.
        yield return Wait(2.25f);
        var c=Object.FindAnyObjectByType<BattleSceneController>();c.enabled=false;c.StopAllCoroutines();var presenter=c.GetComponent<BattleActionPresenter>();if(presenter!=null)presenter.StopAllCoroutines();
        Check(GameObject.Find("TieBreakOverlay")==null,"Battle.ExistingOpeningFinishedBeforeCapture");
        var views=(IDictionary)Get(c,"combatantViews");Combatant actor=null,target=null;object view=null;
        foreach(DictionaryEntry e in views) {var who=(Combatant)e.Key;var a=Get(e.Value,"MonsterAnimation") as MonsterSpriteSheetAnimation;if(a!=null)a.StopAttackAndReturnToIdle();
            if(a!=null&&a.CurrentFrame.name.StartsWith("venom_snake_")){actor=who;view=e.Value;}
            if(who.Side==BattleSide.Allies&&(target==null||who.MaxHp>target.MaxHp))target=who;
        }
        var anim=(MonsterSpriteSheetAnimation)Get(view,"MonsterAnimation");
        Check(views.Keys.Cast<Combatant>().Where(x=>x.Side==BattleSide.Allies).All(x=>((Image)Get(views[x],"SpriteImage")).sprite!=null),"Battle.PlayerVisualTransferredByActualEncounterContract");
        yield return Shot("Battle_VisibleIdle");
        yield return null;yield return null; // RenderView 직후의 긴 frame delta를 다음 공격의 시작 시간에 섞지 않습니다.
        Set(c,"currentActor",actor);int hp=target.CurrentHp;
        float started=Time.time;float deltaAtStart=Time.deltaTime;float actual=-1;
        c.GetType().GetMethod("PlayBasicAttack",Flags).Invoke(c,new object[]{actor,target,100,null,false,false});
        var seen=new HashSet<int>();float stop=Time.realtimeSinceStartup+1;
        while(Time.realtimeSinceStartup<stop) {
            if(anim.IsAttackPlaying)seen.Add(anim.CurrentFrameIndex);
            if(target.CurrentHp<hp&&actual<0)actual=Time.time-started;
            if(actual>=0&&anim.CurrentFrameIndex==2&&anim.IsAttackPlaying) {Capture("Battle_VisibleAttack",1280);break;}
            yield return null;
        }
        yield return Wait(.5f);c.StopAllCoroutines();c.GetComponent<BattleActionPresenter>().StopAllCoroutines();
        Check(actual>=.25f-deltaAtStart-.025f&&actual<=.25f+.08f,"ImpactMatchesUnchangedPresenter025ScaledSeconds.actual="+actual+".startDelta="+deltaAtStart);
        var status=(BattleStatusEffectRuntime)Get(c,"statusEffects");Check(status.HasActivePoison(target)&&status.GetPoisonDefinition(target).Id=="venom","ActualVenomAfterImpact");
        Check(!anim.IsAttackPlaying&&anim.CurrentFrame.name.Contains("Idle"),"ActualActionReturnsIdle");
        foreach(var id in new[]{"venom_bee","forest_spider"}) {
            var md=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").First(x=>x.MonsterId==id);var obj=new GameObject("97AffectedSmoke");var renderer=obj.AddComponent<SpriteRenderer>();var a=obj.AddComponent<MonsterSpriteSheetAnimation>();a.Configure(renderer,md);
            var idleSlots=new HashSet<int>();float end=Time.realtimeSinceStartup+.35f;
            while(Time.realtimeSinceStartup<end){idleSlots.Add(a.CurrentFrameIndex);yield return null;}
            Check(idleSlots.Count>1,id+".ActualIdlePlayback");a.PlayAttack(true);
            var attackSlots=new HashSet<int>();end=Time.realtimeSinceStartup+.35f;
            while(Time.realtimeSinceStartup<end){if(a.IsAttackPlaying)attackSlots.Add(a.CurrentFrameIndex);yield return null;}
            Check(attackSlots.Count>1,id+".ActualAttackPlayback");Destroy(obj);
        }
        foreach(var id in new[]{"wolf","bear","fox"}) {var pet=BeastCompanionCatalog.Get(id);var texture=Resources.Load<Texture2D>(pet.RunResourcePath);Check(texture!=null&&pet.FramesPerSecond==12,id+".ApprovedPetRunReferenceAndFPSUnchanged");}
        Check(errors.Count==0,"SupplementRuntimeErrorsZero");
    }
    IEnumerator Full() {
        Check(Application.unityVersion=="6000.6.5f1","Unity6000.6.5f1");
        GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"Snake97 QA");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(8,0);
        var d=Resources.Load<MonsterDefinition>("MonsterDefinitions/04_VenomSnake");
        Check(d.AnimationFramesPerSecond==8&&d.SourceFacesRight&&d.IdleFrameCount==4&&d.AttackFrameCount==4,"DefinitionVisualContract");
        Check(d.MaxHp==95&&d.BattleAttackPercent==160&&d.BaseExperience==20&&d.CurrencyReward==6&&d.BasicAttackPoisonActions==3&&d.BasicAttackPoisonCooldownActions==2,"DefinitionCombatRewardsUnchanged");
        var op=SceneManager.LoadSceneAsync("Field_02");while(!op.isDone)yield return null;yield return Wait(.4f);
        Player.transform.position=new Vector3(-4,-1.5f,0);Player.GetComponent<Rigidbody2D>().position=Player.transform.position;
        var snake=Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None).First(m=>m.Definition.MonsterId=="venom_snake");
        var fieldAnim=snake.GetComponentInChildren<MonsterSpriteSheetAnimation>();var sr=snake.GetComponentInChildren<SpriteRenderer>();
        Vector3 before=snake.transform.position;yield return Wait(3);
        Check(Vector3.Distance(before,snake.transform.position)>.02f,"Field.ExistingWanderAI");
        foreach(var m in Object.FindObjectsByType<MonsterFieldController>(FindObjectsSortMode.None))m.enabled=false;
        snake.transform.position=new Vector3(-2.2f,.3f,0);snake.GetComponent<Rigidbody2D>().position=snake.transform.position;
        yield return Animate(fieldAnim,"Field");
        Check(!sr.flipX&&sr.transform.lossyScale.x>0,"Field.RightFacingSource");
        Check(fieldAnim.CurrentFrame.rect.width==640&&fieldAnim.CurrentFrame.pixelsPerUnit==640&&fieldAnim.CurrentFrame.pivot==new Vector2(320,320),"RuntimeCellPPUPivot");
        yield return Shot("Field_Idle");
        var pix=Camera.main.WorldToScreenPoint(sr.transform.position+Vector3.right*.718f)-Camera.main.WorldToScreenPoint(sr.transform.position);
        File.WriteAllText(Output+"/field-size.txt","visibleWidthPixels="+Mathf.Abs(pix.x)+";rendererScale="+sr.transform.lossyScale+";cameraOrtho="+Camera.main.orthographicSize);
        // Actual animation instance also reads the public definition used by field encounter.
        BattleEncounterContext.Set(d,snake.SpawnDefinition,Player.GetComponentInChildren<Animator>()?.runtimeAnimatorController,Player.GetComponent<PlayerVisualController>()?.ActiveDefaultSprite,Player.transform.position,snake.transform.position);
        op=SceneManager.LoadSceneAsync("Battle");while(!op.isDone)yield return null;yield return Wait(.3f);
        var controller=Object.FindAnyObjectByType<BattleSceneController>();controller.enabled=false;controller.StopAllCoroutines();
        var presenter=controller.GetComponent<BattleActionPresenter>();if(presenter!=null)presenter.StopAllCoroutines();
        var views=(IDictionary)Get(controller,"combatantViews");Combatant actor=null,target=null;object view=null;
        foreach(DictionaryEntry entry in views) { var c=(Combatant)entry.Key;var v=entry.Value;var a=Get(v,"MonsterAnimation") as MonsterSpriteSheetAnimation;
            if(a!=null&&a.CurrentFrame.name.StartsWith("venom_snake_")){actor=c;view=v;}
            if(c.Side==BattleSide.Allies&&(target==null||c.MaxHp>target.MaxHp))target=c;
        }
        Check(actor!=null&&target!=null,"Battle.ActualEncounterContainsSnake");
        var anim=(MonsterSpriteSheetAnimation)Get(view,"MonsterAnimation");anim.StopAttackAndReturnToIdle();
        var image=(Image)Get(view,"SpriteImage");Check(image.transform.localScale.x>0,"Battle.EnemyFacesRightTowardAllies");
        yield return Animate(anim,"Battle");yield return Shot("Battle_Idle");
        Set(controller,"currentActor",actor);int hp=target.CurrentHp;
        controller.GetType().GetMethod("PlayBasicAttack",Flags).Invoke(controller,new object[]{actor,target,100,null,false,false});
        float start=Time.realtimeSinceStartup;float impact=-1;var frames=new HashSet<int>();
        while(Time.realtimeSinceStartup-start<.4f) {
            if(anim.IsAttackPlaying)frames.Add(anim.CurrentFrameIndex);
            if(target.CurrentHp<hp&&impact<0)impact=Time.realtimeSinceStartup-start;
            trace.Add("BattleAction,"+(Time.realtimeSinceStartup-start)+","+anim.CurrentFrame.name+","+target.CurrentHp);yield return null;
        }
        Capture("Battle_Attack",1280);
        while(Time.realtimeSinceStartup-start<.85f) { if(anim.IsAttackPlaying)frames.Add(anim.CurrentFrameIndex);yield return null; }
        controller.StopAllCoroutines();controller.GetComponent<BattleActionPresenter>().StopAllCoroutines();
        var statuses=(BattleStatusEffectRuntime)Get(controller,"statusEffects");var abilities=(BattleMonsterAbilityRuntime)Get(controller,"monsterAbilities");
        Check(target.CurrentHp<hp&&impact>=.20f&&impact<=.34f,"Battle.BasicAttackImpactPreserved.At="+impact);
        Check(statuses.HasActivePoison(target)&&statuses.GetPoisonDefinition(target).Id=="venom","Battle.ActualVenomApplied");
        Check(abilities.GetPoisonInflictionCooldown(actor)==2,"Battle.PoisonCooldownTwoActions");
        Check(frames.SetEquals(new[]{0,1,2,3})&&!anim.IsAttackPlaying&&anim.CurrentFrame.name.Contains("Idle"),"Battle.ActualActionFourFramesReturnIdle");
        foreach(var other in Resources.LoadAll<MonsterDefinition>("MonsterDefinitions")) {
            if(other.MonsterId=="venom_snake"||!other.UsesSpriteSheetAnimation)continue;
            var o=new GameObject("97OtherSmoke");var s=o.AddComponent<SpriteRenderer>();var a=o.AddComponent<MonsterSpriteSheetAnimation>();a.Configure(s,other);
            Check(a.CurrentFrame!=null,"OtherMonster.ReferenceSmoke."+other.MonsterId);
            Check(a.UsesApprovedAttackTiming==(other.MonsterId=="venom_bee"||other.MonsterId=="forest_spider"),"OtherMonster.TimingContract."+other.MonsterId);Destroy(o);
        }
        foreach(var id in new[]{"venom_bee","forest_spider","venom_snake"}) {
            var pet=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").First(x=>x.MonsterId==id);
            var petFrames=BeastCompanionCatalog.GetMonsterFrames(pet,out bool generated);
            Check(petFrames.Length>0&&petFrames.All(x=>x!=null),"Pet.ReusedMonsterFrames."+id);
            if(generated)foreach(var frame in petFrames)Destroy(frame);
        }
        Check(errors.Count==0,"NewRuntimeErrorsZero");
    }
}
#endif
