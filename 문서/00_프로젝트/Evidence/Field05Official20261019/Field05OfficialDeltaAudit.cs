using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.World;
using ProjectLimitless.Player;
using ProjectLimitless.Monster;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using Object=UnityEngine.Object;
namespace ProjectLimitless.EditorTools
{
    /// <summary>기존 검증 저장으로 Continue만 수행합니다. 새 화면/텔레포트/플레이 검수 환경을 만들지 않습니다.</summary>
    [InitializeOnLoad] public static class Field05OfficialDeltaAudit
    {
        const string Armed="Field05OfficialDelta20261019";
        static string Root=>Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Field05Official20261019/Runtime"));
        static readonly List<string> Checks=new List<string>(),Errors=new List<string>();static IEnumerator routine;static double deadline;
        static Field05OfficialDeltaAudit(){EditorApplication.playModeStateChanged+=s=>{if(s!=PlayModeStateChange.EnteredPlayMode||!SessionState.GetBool(Armed,false))return;
            GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));Application.runInBackground=true;AudioListener.volume=0;
            Application.logMessageReceived+=(t,st,k)=>{if(k==LogType.Error||k==LogType.Exception){Errors.Add(t+"\n"+st);File.WriteAllLines(Path.Combine(Root,"errors.txt"),Errors);}};routine=Flatten(Run());deadline=EditorApplication.timeSinceStartup+180;EditorApplication.update+=Tick;};}
        public static void RunBatch(){if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Field05Review20261019/Client/Assets"))throw new Exception("격리 복사본만 허용");Directory.CreateDirectory(Root);GameSaveService.AuditSaveDirectory=Path.Combine(Root,"Saves");UserSettingsService.BeginAudit(Path.Combine(Root,"Settings"));File.WriteAllText(Path.Combine(Root,"status.txt"),"RUNNING");EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);SessionState.SetBool(Armed,true);EditorApplication.EnterPlaymode();}
        static void Check(bool ok,string id){Checks.Add((ok?"PASS|":"FAIL|")+id);File.WriteAllLines(Path.Combine(Root,"checks.txt"),Checks);if(!ok)throw new Exception(id);}
        static IEnumerator Wait(float s=.3f){float until=Time.realtimeSinceStartup+s;while(Time.realtimeSinceStartup<until)yield return null;}
        static IEnumerator Scene(string name){float until=Time.realtimeSinceStartup+40;while(SceneManager.GetActiveScene().name!=name&&Time.realtimeSinceStartup<until)yield return null;Check(SceneManager.GetActiveScene().name==name,"Scene."+name);MonsterEncounterService.SuppressForSeconds(3600);yield return Wait();}
        static IEnumerator Continue(){SceneManager.LoadSceneAsync("Bootstrap");yield return Scene("Bootstrap");GameSessionData.Reset();var button=GameObject.Find("StartMenuCanvas/Slot01/Action")?.GetComponent<Button>();Check(button!=null,"Continue.Slot01ActualUI");button.onClick.Invoke();yield return Scene("Field_05_WesternOutskirts");}
        static IEnumerator Run(){yield return Continue();var player=Object.FindAnyObjectByType<PlayerController>();Check(player!=null,"Player.Restored");var env=GameObject.Find("FieldEnvironment").GetComponentsInChildren<SpriteRenderer>(true);Check(env.Length==375,"Environment375");Check(env.Count(x=>x.name.StartsWith("Grass_"))==315,"Ground315");Check(env.Count(x=>x.name.StartsWith("Road"))==42,"Road42");Check(env.Count(x=>!x.name.StartsWith("Grass_")&&!x.name.StartsWith("Road"))==18,"Props18");
            Check(env.All(x=>x.sprite!=null),"MissingEnvironmentSprite0");Check(env.Select(x=>AssetDatabase.GetAssetPath(x.sprite)).Distinct().Count()==14,"SourcePNG14");
            foreach(var road in env.Where(x=>x.name.StartsWith("Road"))){Check(road.transform.rotation==Quaternion.identity&&!road.flipX&&!road.flipY&&road.GetComponents<Collider2D>().Length==0,"Road.Contract."+road.name);}
            Check(!env.Any(x=>AssetDatabase.GetAssetPath(x.sprite).Contains("Optional")),"Optional7Added0");Check(Object.FindObjectsByType<SceneTransitionTrigger>().Count(x=>x.gameObject.scene.name=="Field_05_WesternOutskirts")==1,"OnlyExistingEastPortal");
            var transforms=SceneManager.GetActiveScene().GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<Transform>(true)).ToArray();Check(transforms.All(x=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(x.gameObject)==0),"MissingScript0");
            foreach(var spec in new[]{new Vector2(5,2),new Vector2(-2,2),new Vector2(-6,-2)})Check(transforms.Any(x=>x.GetComponent<Chapter2ObjectiveSite>()!=null&&Vector2.Distance(x.position,spec)<.01f),"Main14.Site."+spec);
            Check(transforms.Any(x=>x.name=="field05-serin"&&Vector2.Distance(x.position,new Vector2(1,0))<.01f),"Serin.PositionProtected");
            var monsters=Resources.LoadAll<FieldMonsterSpawnDefinition>("MonsterSpawns").Where(x=>x.SceneName=="Field_05_WesternOutskirts").ToArray();Check(monsters.Length==2&&monsters.All(x=>x.RespawnSeconds==35&&x.ActivityRadius==1),"MonsterData2.Respawn35Unchanged");
            string quest=JsonUtility.ToJson(QuestService.ExportSaveData()),party=JsonUtility.ToJson(CompanionRosterService.ExportSaveData());Vector2 pos=player.transform.position;Check(GameSaveService.SaveCurrentWorldPosition(pos,"Field_05_WesternOutskirts"),"Save.ActualCurrentPosition");yield return Continue();Check(quest==JsonUtility.ToJson(QuestService.ExportSaveData())&&party==JsonUtility.ToJson(CompanionRosterService.ExportSaveData())&&Vector2.Distance(pos,Object.FindAnyObjectByType<PlayerController>().transform.position)<.05f,"Continue.QuestPartyPositionExact");
            Check(Errors.All(x=>x.Contains("UnityEditor.Search")||x.Contains("Host type is not matching")),"NewConsoleError0");File.WriteAllText(Path.Combine(Root,"status.txt"),"DELTA_PASS_KNOWN_ENV_PARTIAL");}
        static IEnumerator Flatten(IEnumerator r){var s=new Stack<IEnumerator>();s.Push(r);while(s.Count>0){var t=s.Peek();if(!t.MoveNext()){s.Pop();continue;}if(t.Current is IEnumerator c){s.Push(c);continue;}yield return t.Current;}}
        static void Tick(){try{if(EditorApplication.timeSinceStartup>deadline)throw new TimeoutException();if(routine.MoveNext())return;EditorApplication.update-=Tick;SessionState.SetBool(Armed,false);EditorApplication.Exit(0);}catch(Exception ex){File.WriteAllText(Path.Combine(Root,"status.txt"),"FAIL");File.WriteAllText(Path.Combine(Root,"failure.txt"),ex.ToString());EditorApplication.Exit(1);}}
    }
}
