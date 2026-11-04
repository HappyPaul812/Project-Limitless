#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.Player;
using UnityEngine;
using UnityEngine.UI;
using Object=UnityEngine.Object;
// 사용자 데이터 대신 새 합성 세션만 사용하여 실제 통합 Battle UI와 새 자산 연결을 확인합니다.
[DefaultExecutionOrder(-10000)] public sealed class EodApprovedProbe:MonoBehaviour {
 public static string Output=>"F:/study/codex/Project-Limitless/Temp/EODApproved20261104/"+(UnityEditor.SessionState.GetBool("EODVisualOnly",false)?"VisualEvidence":"Evidence");public bool Done,Failed;
 readonly List<string> checks=new List<string>(),errors=new List<string>(),environment=new List<string>();IEnumerator routine;BattleSceneController battle;Camera cam;Canvas canvas;Image effect;float attackStart;MonsterSpriteSheetAnimation observed;List<string> sequence=new List<string>();List<AttackRow> trace=new List<AttackRow>();string caseId;
 [Serializable]public class AttackRow{public string test,sprite;public float t;public int hp,index;public bool attack,playing;}
 [Serializable]public class Rows{public List<AttackRow> rows;}
 static T Read<T>(object obj,string name)=>(T)obj.GetType().GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).GetValue(obj);
 static void Set(object obj,string name,object value)=>obj.GetType().GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).SetValue(obj,value);
 static object Call(object obj,string name,params object[] args)=>obj.GetType().GetMethod(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).Invoke(obj,args);
 void Awake(){Directory.CreateDirectory(Output);Application.runInBackground=true;QualitySettings.vSyncCount=0;Application.targetFrameRate=60;GameSaveService.AuditSaveDirectory=Output+"/SyntheticSave";AudioListener.volume=0;Application.logMessageReceived+=(msg,stack,kind)=>{if(kind!=LogType.Error&&kind!=LogType.Exception&&kind!=LogType.Assert)return;if(stack.Contains("UnityEditor.Search.SearchDatabase"))environment.Add(msg+"\n"+stack);else errors.Add(msg+"\n"+stack);};}
 void Start(){routine=Flatten(Run());}
 void Update(){if(Done)return;try{if(!routine.MoveNext())Finish();}catch(Exception ex){errors.Add(ex.ToString());Failed=true;Finish();}}
 void Finish(){Time.timeScale=1;Time.captureDeltaTime=0;Done=true;Failed|=errors.Count>0;File.WriteAllLines(Output+"/Checks.txt",checks);File.WriteAllLines(Output+"/RuntimeErrors.txt",errors);File.WriteAllLines(Output+"/EnvironmentErrors.txt",environment);File.WriteAllText(Output+"/AttackTrace.json",JsonUtility.ToJson(new Rows{rows=trace},true));File.WriteAllText(Output+"/Status.txt",Failed?"FAIL":"PASS");}
 void Check(bool pass,string id){checks.Add((pass?"PASS ":"FAIL ")+id);Failed|=!pass;File.WriteAllLines(Output+"/Checks.txt",checks);}
 static IEnumerator Flatten(IEnumerator first){var stack=new Stack<IEnumerator>();stack.Push(first);while(stack.Count>0){var top=stack.Peek();if(!top.MoveNext()){stack.Pop();continue;}if(top.Current is IEnumerator e)stack.Push(e);else yield return top.Current;}}
 IEnumerator Run(){
  Check(Application.unityVersion=="6000.6.5f1","Engine6000_6_5f1");
  var bee=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Single(x=>x.MonsterId=="venom_bee");var spider=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Single(x=>x.MonsterId=="forest_spider");
  Check(bee.ExplicitIdleFrames.Length==2&&bee.ExplicitAttackFrames.Length==10&&bee.IdleSpriteSheet==null&&bee.AttackSpriteSheet==null,"63.New5PoseDefinitionNoLegacySheets");
  Check(spider.ExplicitIdleFrames.Length==8&&spider.ExplicitWalkFrames.Length==8&&spider.ExplicitAttackFrames.Length==8,"90.ApprovedOrganicIdleWalkAttackBindings");
  Check(Resources.Load<Texture2D>("MonsterValidation/PilotBee/pilot_bee_idle")==null&&Resources.Load<Texture2D>("MonsterValidation/PilotBee/pilot_bee_attack")==null,"63.LegacyResourcesAbsent");
  var catalog=Resources.Load<BattleBackgroundCatalog>("BattleBackgrounds/Chapter2");
  foreach(string scene in new[]{"Field_05_WesternOutskirts","Field_06_ScorchedTrail","Field_07_AshenReach","Field_08_RedRift","Field_09_ObsidianScar","Field_10_BurningPulse"})Check(catalog.Find(scene,"")!=null,"92.Mapping."+scene);
  Check(catalog.Find("Field_11_DeepCore","")==Resources.Load<Sprite>("Main20/Environment/DeepCore_Ground_Base"),"92.DeepCorePreserved");
  foreach(string scene in new[]{"Field_05_WesternOutskirts","Field_09_ObsidianScar"}){
   yield return Setup(bee,scene,true);Check(Read<Formation>(battle,"allies").Members.Count()==3&&Read<Formation>(battle,"allies").Members.All(c=>Body(c).sprite!=null&&Body(c).color.a>0),"92.Real3VisibleParty."+scene);
   foreach(int width in new[]{960,1280,1680}){Render(width);InjectCold(2);DrawVfx("cold.necrosis",width==960?40:128);Capture("BG_"+scene+"_"+width);Check(GameObject.Find("Background").GetComponent<Image>().sprite==catalog.Find(scene,""),"92.Display."+scene+"."+width);CheckCold(width);}
  }
  // 기존 승인 설원 환경의 과거 카메라 자료를 QA 배경으로만 재사용합니다. 미승인#91 신규BG는 넣지 않습니다.
  yield return Setup(bee,"Field_05_WesternOutskirts",true);Render(960);var snow=new Texture2D(2,2);snow.LoadImage(File.ReadAllBytes("F:/study/codex/Project-Limitless/Temp/Issue89LayoutDelta20261104/Evidence/F2_Source_Camera.png"));GameObject.Find("Background").GetComponent<Image>().sprite=Sprite.Create(snow,new Rect(0,0,snow.width,snow.height),Vector2.one*.5f);
  foreach(int stack in new[]{1,2}){InjectCold(stack);DrawVfx(stack==1?"cold.chill":"cold.frostbite",40);Capture("Cold_Snow960_Stack"+stack);CheckCold(960);}
  if(UnityEditor.SessionState.GetBool("EODVisualOnly",false)){Check(errors.Count==0,"VisualOnly.NoRuntimeErrors");yield break;}
  yield return Setup(bee,"Field_05_WesternOutskirts",true);Render(1280);yield return Attack("63.Bee",Read<Formation>(battle,"enemies").Members.First(x=>Read<IDictionary>(battle,"participantSetups")[x] is BattleParticipantSetup s&&s.MonsterDefinition==bee),10,1f);
  yield return Setup(spider,"Field_03",false);Render(1280);yield return Attack("90.Spider",Read<Formation>(battle,"enemies").Members.First(x=>Read<IDictionary>(battle,"participantSetups")[x] is BattleParticipantSetup s&&s.MonsterDefinition==spider),8,.8f);
  Check(errors.Count==0,"NoNewRuntimeErrors");
 }
 IEnumerator Setup(MonsterDefinition monster,string field,bool three){
  if(battle!=null){battle.StopAllCoroutines();Object.Destroy(battle.gameObject);}foreach(var c in Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None))Object.Destroy(c.gameObject);yield return null;effect=null;
  Time.timeScale=0;GameSessionData.Reset();GameSessionData.ConfigurePlayer(PlayerVisualType.Male,"EOD 격리 합성");GameSessionData.SelectPlayerPath("path.mobility");GameSessionData.SelectJob("guardian");GameSessionData.ConfigureProgress(14,0);
  var spawn=ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>();spawn.Configure(field,"eod_qa",monster,Vector2.zero,2.5f,30);
  BattleEncounterContext.SetStoryEncounter(three?MainQuest04EncounterBridge.EncounterId:"",monster,spawn,null,PlayerAppearanceCatalog.ForCombination(PlayerVisualType.Male,"path.mobility","guardian").frames[4],Vector2.zero,Vector2.one);
  battle=new GameObject("EOD_ActualBattle").AddComponent<BattleSceneController>();battle.StopAllCoroutines();var tie=GameObject.Find("TieBreakOverlay");if(tie!=null)Object.DestroyImmediate(tie);var es=UnityEngine.EventSystems.EventSystem.current;if(es!=null)es.gameObject.SetActive(false);Call(battle,"HideSkillDetailPopup");Call(battle,"HideCombatantDetailPopupForAction");Set(battle,"actionPlaying",false);Set(battle,"currentActor",Read<Formation>(battle,"allies").Members.First(x=>x.Id=="player"));
  canvas=GameObject.Find("BattleCanvas").GetComponent<Canvas>();cam=Camera.main;cam.orthographicSize=360;canvas.renderMode=RenderMode.ScreenSpaceCamera;canvas.worldCamera=cam;canvas.planeDistance=5;
  var six=Resources.LoadAll<JobDefinition>("JobDefinitions").SelectMany(j=>BattleSkillCatalog.GetSkills(j)).GroupBy(s=>s.Id).Select(g=>g.First()).Take(6).ToArray();var hero=Read<Formation>(battle,"allies").Members.First(x=>x.Id=="player");((IDictionary)Read<object>(battle,"equippedSkills"))[hero]=six;((IDictionary)Read<object>(battle,"equippedSkillSlots"))[hero]=new Dictionary<string,int>();Call(battle,"ShowSkillMenu");yield return null;Call(battle,"HideSkillDetailPopup");Call(battle,"HideCombatantDetailPopupForAction");
 }
 void Render(int width){cam.targetTexture=new RenderTexture(width,720,24);Canvas.ForceUpdateCanvases();}
 Combatant Hero=>Read<Formation>(battle,"allies").Members.First(x=>x.Id=="player");
 void InjectCold(int stack){var model=new BattleCombatantStatusViewModel("합성냉기","QA",Hero.CurrentHp,Hero.MaxHp,new[]{new BattleStatusMarker("cold.chill","한기",2),new BattleStatusMarker("cold.frostbite","동상",2),new BattleStatusMarker("cold.necrosis","동결괴사",stack)},Array.Empty<BattleCooldownStatus>());Call(battle,"RefreshHpRow",Hero,model);Canvas.ForceUpdateCanvases();}
 Image Body(Combatant c)=>Read<Image>(((IDictionary)Read<object>(battle,"combatantViews"))[c],"SpriteImage");
 void DrawVfx(string id,int size){if(effect!=null)Object.DestroyImmediate(effect.gameObject);var body=Body(Hero);effect=new GameObject("EOD_QA_ApprovedStaticVfx",typeof(RectTransform),typeof(Image)).GetComponent<Image>();effect.transform.SetParent(body.transform.parent,false);effect.sprite=BattleApprovedColdAssets.Vfx(id);BattleApprovedColdAssets.PlaceVfx(body,effect,size,cam);Canvas.ForceUpdateCanvases();}
 void CheckCold(int width){var row=((IDictionary)Read<object>(battle,"combatantHpRows"))[Hero];var badges=((IList)Read<object>(row,"StatusBadges")).Cast<object>().Where(x=>Read<GameObject>(x,"Root").activeSelf).ToArray();Check(badges.Length>=3,"89.Badges3."+checks.Count);Check(badges.Select(x=>Read<Image>(x,"Icon").sprite).Contains(BattleApprovedColdAssets.Icon("cold.chill"))&&badges.Select(x=>Read<Image>(x,"Icon").sprite).Contains(BattleApprovedColdAssets.Icon("cold.frostbite"))&&badges.Select(x=>Read<Image>(x,"Icon").sprite).Contains(BattleApprovedColdAssets.Icon("cold.necrosis")),"89.OfficialIconMapping."+checks.Count);Check(badges.Select(x=>Read<Text>(x,"Label").text).Any(t=>t=="한기 2턴")&&badges.Select(x=>Read<Text>(x,"Label").text).Any(t=>t.StartsWith("동결괴사 ")),"89.TextTurnsStacks."+checks.Count);Check(badges.All(x=>Read<Text>(x,"Label").preferredWidth<=Read<Text>(x,"Label").rectTransform.rect.width+.5f),"89.TextFits."+checks.Count);}
 IEnumerator Attack(string id,Combatant actor,int frames,float duration){
  battle.StopAllCoroutines();Call(battle,"RefreshCombatantViews",new object[]{null});Set(battle,"currentActor",actor);var view=((IDictionary)Read<object>(battle,"combatantViews"))[actor];observed=Read<MonsterSpriteSheetAnimation>(view,"MonsterAnimation");var target=Hero;int hp=target.CurrentHp;Time.timeScale=1;Time.captureDeltaTime=.01f;sequence.Clear();caseId=id;attackStart=Time.time;
  Call(battle,"PlayBasicAttack",actor,target,100,null,false,false);float guard=Time.realtimeSinceStartup+10;bool first=true;while((observed.IsAttackPlaying||first)&&Time.realtimeSinceStartup<guard){first=false;yield return null;}
  float elapsed=Time.time-attackStart;battle.StopAllCoroutines();Check(trace.Where(x=>x.test==id&&x.attack).Select(x=>x.index).Distinct().Count()==frames,id+".AllApprovedFrames");Check(elapsed>=duration-.02f&&elapsed<=duration+.1f,id+".FullDurationBeforeComplete");Check(observed.CurrentFrame!=null&&!observed.IsAttackPlaying && observed.CurrentFrame == (id.StartsWith("63") ? Resources.Load<Sprite>("Approved20261104/VenomBee/IdleFolded") : Resources.Load<Sprite>("Approved20261104/Spider/Idle/00")),id+".ReturnApprovedIdle");Check(target.CurrentHp<hp,id+".RealDamageApplied");Capture(id+"_ReturnIdle");caseId=null;observed=null;Time.timeScale=0;Time.captureDeltaTime=0;yield return null;
 }
 void LateUpdate(){if(caseId==null||observed==null)return;string s=observed.CurrentFrame?.name??"null";if(sequence.Count==0||sequence.Last()!=s){sequence.Add(s);Capture(caseId+"_"+sequence.Count.ToString("00"));}trace.Add(new AttackRow{test=caseId,sprite=s,t=Time.time-attackStart,hp=Hero.CurrentHp,index=observed.CurrentFrameIndex,attack=observed.IsAttackPlaying,playing=Read<bool>(battle,"actionPlaying")});}
 void Capture(string id){Canvas.ForceUpdateCanvases();cam.Render();var old=RenderTexture.active;RenderTexture.active=cam.targetTexture;var t=new Texture2D(cam.targetTexture.width,720,TextureFormat.RGBA32,false);t.ReadPixels(new Rect(0,0,t.width,720),0,0);t.Apply();File.WriteAllBytes(Output+"/"+id+".png",t.EncodeToPNG());Object.Destroy(t);RenderTexture.active=old;}
}
#endif
