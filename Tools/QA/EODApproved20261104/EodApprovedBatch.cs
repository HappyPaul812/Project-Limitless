using System;
using System.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
// 원본 Editor와 다른 고정 경로에서만 신규 통합 Delta를 실행합니다.
[InitializeOnLoad] public static class EodApprovedBatch {
 const string Key="EOD20261104QA";static double deadline;
 static EodApprovedBatch(){EditorApplication.playModeStateChanged+=s=>{if(s==PlayModeStateChange.EnteredPlayMode&&SessionState.GetBool(Key,false)){deadline=EditorApplication.timeSinceStartup+240;EditorApplication.update+=Poll;}};}
 public static void RunVisual(){SessionState.SetBool("EODVisualOnly",true);RunCore();}
 public static void Run(){SessionState.SetBool("EODVisualOnly",false);RunCore();}
 static void RunCore(){if(!Application.isBatchMode||Application.unityVersion!="6000.6.5f1"||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/EODApproved20261104/QA/Client/Assets"))throw new Exception("격리/버전가드실패");BindDefinitions();EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);new GameObject("EodApprovedProbe").AddComponent<EodApprovedProbe>();SessionState.SetBool(Key,true);EditorApplication.isPlaying=true;}
 static void BindDefinitions(){
  Bind("02_VenomBee",new[]{"VenomBee/IdleFolded","VenomBee/IdleSpread"},new[]{"VenomBee/Prepare","VenomBee/Prepare","VenomBee/Sting","VenomBee/Sting","VenomBee/Recover","VenomBee/Recover","VenomBee/Recover","VenomBee/Recover","VenomBee/Recover","VenomBee/Recover"},new string[0]);
  Bind("03_ForestSpider",System.Linq.Enumerable.Range(0,8).Select(i=>"Spider/Idle/"+i.ToString("00")).ToArray(),System.Linq.Enumerable.Range(0,8).Select(i=>"Spider/Attack/"+i.ToString("00")).ToArray(),System.Linq.Enumerable.Range(0,8).Select(i=>"Spider/Walk/"+i.ToString("00")).ToArray());AssetDatabase.SaveAssets();
 }
 static void Bind(string name,string[] idle,string[] attack,string[] walk){var d=AssetDatabase.LoadAssetAtPath<ProjectLimitless.Monster.MonsterDefinition>("Assets/_Project/Resources/MonsterDefinitions/"+name+".asset");var so=new SerializedObject(d);foreach(var pair in new[]{new System.Collections.Generic.KeyValuePair<string,string[]>("explicitIdleFrames",idle),new System.Collections.Generic.KeyValuePair<string,string[]>("explicitAttackFrames",attack),new System.Collections.Generic.KeyValuePair<string,string[]>("explicitWalkFrames",walk)}){var a=so.FindProperty(pair.Key);a.arraySize=pair.Value.Length;for(int i=0;i<pair.Value.Length;i++){var sprite=AssetDatabase.LoadAssetAtPath<Sprite>("Assets/_Project/Resources/Approved20261104/"+pair.Value[i]+".png");if(sprite==null)throw new Exception("승인Sprite로드실패:"+pair.Value[i]);a.GetArrayElementAtIndex(i).objectReferenceValue=sprite;}}so.FindProperty("idleSpriteSheet").objectReferenceValue=null;so.FindProperty("attackSpriteSheet").objectReferenceValue=null;so.ApplyModifiedPropertiesWithoutUndo();EditorUtility.SetDirty(d);}
 static void Poll(){var p=UnityEngine.Object.FindAnyObjectByType<EodApprovedProbe>();if(p!=null&&p.Done){SessionState.SetBool(Key,false);EditorApplication.update-=Poll;int code=p.Failed?1:0;EditorApplication.isPlaying=false;EditorApplication.delayCall+=()=>EditorApplication.Exit(code);}else if(EditorApplication.timeSinceStartup>deadline){File.WriteAllText(EodApprovedProbe.Output+"/Timeout.txt","시간초과");EditorApplication.Exit(2);}}
}
