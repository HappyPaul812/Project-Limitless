using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using ProjectLimitless.Audio;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>Main19 새 데이터와 Field10만 만듭니다. 기존 Scene·Monster·PNG·Voice를 다시 생성하지 않습니다.</summary>
    public static class Main19ContentBuilder
    {
        public const string ScenePath="Assets/_Project/Scenes/Field_10_BurningPulse.unity";
        const string Root="Assets/_Project/Resources/";
        public static readonly string[] Objectives={"reach_field09_deeper_route","enter_burning_pulse","inspect_giant_track","inspect_melted_cliff","defeat_patrol_a",
            "inspect_pulse_node","cross_heated_corridor","defeat_patrol_b","inspect_massive_footprint","reach_silhouette_ridge","witness_flame_colossus","withdraw_and_prepare"};
        public static string Build()
        {
            if(EditorApplication.isPlayingOrWillChangePlaymode||EditorSceneManager.GetActiveScene().isDirty)throw new InvalidOperationException("저장된 Edit Mode에서 생성하세요.");
            string[] labels={"더 깊은 길로 향하세요.","맥동의 열맥에 들어가세요.","거대한 흔적을 조사하세요.","녹아내린 암벽을 조사하세요.","앞을 막는 무리를 물리치세요.",
                "지면의 맥동을 확인하세요.","열기가 몰리는 통로를 지나가세요.","심부를 지키는 무리를 물리치세요.","더 거대한 흔적을 조사하세요.","앞쪽 능선까지 이동하세요.","심부의 존재를 확인하세요.","물러나서 다음 준비를 정리하세요."};
            var steps=Objectives.Select((id,i)=>{var s=new QuestObjectiveDefinition();s.ConfigureForAudit(id,labels[i],i==0||i==1||i==6||i==9?QuestObjectiveType.ReachLocation:i==4||i==7?QuestObjectiveType.DefeatEncounter:QuestObjectiveType.Interact,Chapter2Main19Flow.Targets[i]);return s;}).ToArray();
            var quest=New<QuestDefinition>(Root+"QuestDefinitions/Main19_BurningPulse.asset");
            quest.ConfigureForAudit(Chapter2Main19Flow.QuestId,"타오르는 맥동",QuestType.Main,steps,new RewardBundle{Experience=80,Currency=70},new[]{Chapter2Main18Flow.QuestId});
            quest.ConfigureDescription("맥동의 열맥에서 거대한 흔적과 열기를 조사하고 다음 대면을 준비한다.");Save(quest);QuestCatalog.ReloadForAudit();
            var monsters=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions");
            var beetle=monsters.Single(x=>x.MonsterId=="obsidian_beetle");var lizard=monsters.Single(x=>x.MonsterId=="fissure_lizard");var wraith=monsters.Single(x=>x.MonsterId=="ember_wraith");
            Spawn("StoryEncounterReturns/Main19_PatrolA",Chapter2Main19Flow.PatrolA,beetle,Chapter2Main19Flow.Positions[4]);
            Spawn("StoryEncounterReturns/Main19_PatrolB",Chapter2Main19Flow.PatrolB,beetle,Chapter2Main19Flow.Positions[7]);
            Spawn("MonsterSpawns/Field10_Beetle01","field10_beetle_01",beetle,new Vector2(4.3f,-4.5f));
            Spawn("MonsterSpawns/Field10_Lizard01","field10_lizard_01",lizard,new Vector2(.5f,4.7f));
            Spawn("MonsterSpawns/Field10_Wraith01","field10_wraith_01",wraith,new Vector2(-3.5f,-4.8f));
            Connection("Field09_WestToField10",Chapter2Main18Flow.Field,"west_to_field10","Spawn_From_Field10",-7.8f,Chapter2Main19Flow.Field,"Spawn_From_Field09",false);
            Connection("Field10_EastToField09",Chapter2Main19Flow.Field,"east_to_field09","Spawn_From_Field09",7.8f,Chapter2Main18Flow.Field,"Spawn_From_Field10",true);
            BuildField();
            if(!EditorBuildSettings.scenes.Any(x=>x.path==ScenePath))EditorBuildSettings.scenes=EditorBuildSettings.scenes.Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray();
            Bgm();Manifest();AssetDatabase.SaveAssets();return "Main19 Quest12/Field10/Routes2/StoryMixed2/Optional3/BGM/TTS manifest created; Art pending";
        }
        // 이미 생성된 데이터는 재실행으로 덮지 않습니다. 후속 수정은 별도 명시 작업에서 합니다.
        static T New<T>(string path)where T:ScriptableObject
        {
            if(AssetDatabase.LoadAssetAtPath<T>(path)!=null)throw new InvalidOperationException("기존 Main19 자산을 덮지 않습니다: "+path);
            Directory.CreateDirectory(Path.GetDirectoryName(path));var value=ScriptableObject.CreateInstance<T>();AssetDatabase.CreateAsset(value,path);return value;
        }
        static void Save(UnityEngine.Object value){EditorUtility.SetDirty(value);AssetDatabase.SaveAssetIfDirty(value);}
        static void Spawn(string file,string id,MonsterDefinition monster,Vector2 at)
        {
            var s=New<FieldMonsterSpawnDefinition>(Root+file+".asset");s.Configure(Chapter2Main19Flow.Field,id,monster,at,.6f,35);s.SetNonRespawningBoss(false);Save(s);
        }
        static void Connection(string file,string scene,string id,string spawn,float x,string target,string targetSpawn,bool replace)
        {
            var c=New<FieldConnectionDefinition>(Root+"FieldConnections/"+file+".asset");c.Configure(scene,id,spawn,new Vector2(x,0),new Vector2(Math.Sign(x)*10.25f,0),new Vector2(1.1f,3),target,targetSpawn,replace);Save(c);
        }
        static void BuildField()
        {
            if(AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath)!=null)throw new InvalidOperationException("기존 Field10을 덮지 않습니다.");
            if(!AssetDatabase.CopyAsset("Assets/_Project/Scenes/Field_09_ObsidianScar.unity",ScenePath))throw new InvalidOperationException("Field10 copy failed");
            var active=UnityEngine.SceneManagement.SceneManager.GetActiveScene();var selected=Selection.objects;
            var scene=EditorSceneManager.OpenScene(ScenePath,OpenSceneMode.Additive);
            try
            {
                var env=scene.GetRootGameObjects().Single(x=>x.name=="FieldEnvironment").transform;
                foreach(Transform child in env.Cast<Transform>().ToArray())UnityEngine.Object.DestroyImmediate(child.gameObject);
                Visual(env,"Ground","Ground_Base",Vector2.zero,new Vector2(21,15),-20,true);
                Visual(env,"CrackedGround","Ground_Cracked_02",new Vector2(-5,0),new Vector2(11,15),-19,true);
                Visual(env,"Track1_ArtPending","AshPatch_01",new Vector2(5.5f,1),new Vector2(2.2f,1.5f),-5);
                Visual(env,"MeltedCliff_ArtPending","Cliff_Blocker_01",new Vector2(3.4f,-3.5f),new Vector2(3,2),2);
                Visual(env,"Pulse_ArtPending","Fissure_Glow",new Vector2(-.6f,1.8f),new Vector2(2.1f,1.5f),-4);
                Visual(env,"Corridor_ArtPending","HeatVent_01",new Vector2(-2.5f,2.8f),new Vector2(1.5f,1.5f),-4);
                Visual(env,"MassiveTrack_ArtPending","AshPatch_01",new Vector2(-5.3f,1),new Vector2(3,2),-5);
                Visual(env,"Ridge_ArtPending","Obsidian_Slab_01",new Vector2(-7,2.4f),new Vector2(3,1.4f),-4);
                // 원본 열기 장식은 임시 배경일 뿐입니다. Main20 Boss 이미지나 전투 종을 생성하지 않습니다.
                Visual(env,"DistantFlame_ArtPending","HeatVent_01",new Vector2(-8.6f,4.5f),new Vector2(1.8f,2.6f),-3);
                var bounds=scene.GetRootGameObjects().Select(x=>x.GetComponent<WorldBounds2D>()).First(x=>x!=null);
                foreach(Transform child in bounds.transform.Cast<Transform>().Where(x=>x.name.StartsWith("Boundary_")).ToArray())UnityEngine.Object.DestroyImmediate(child.gameObject);
                bounds.Configure(Vector2.zero,new Vector2(21,15));WorldBounds2D.CreateBoundaryColliders(bounds.transform,bounds.Bounds,.3f,new WorldBoundaryOpening(WorldBoundarySide.Right,0,3));
                var title=new GameObject("BurningPulseTitle",typeof(TextMesh));title.transform.SetParent(env,false);title.transform.position=new Vector2(0,6.5f);
                var text=title.GetComponent<TextMesh>();text.text="맥동의 열맥";text.characterSize=.16f;text.anchor=TextAnchor.MiddleCenter;
                if(!EditorSceneManager.SaveScene(scene,ScenePath))throw new InvalidOperationException("Field10 save failed");
            }
            finally{EditorSceneManager.CloseScene(scene,true);UnityEngine.SceneManagement.SceneManager.SetActiveScene(active);Selection.objects=selected;}
        }
        static void Visual(Transform parent,string name,string art,Vector2 at,Vector2 size,int order,bool tiled=false)
        {
            var s=AssetDatabase.LoadAssetAtPath<Sprite>(Root+"Main18/Environment/ObsidianScar_"+art+".png");if(s==null)throw new InvalidOperationException("기존 임시환경 Sprite 누락: "+art);
            var go=new GameObject(name,typeof(SpriteRenderer));go.transform.SetParent(parent,false);go.transform.position=at;var renderer=go.GetComponent<SpriteRenderer>();renderer.sprite=s;renderer.sortingOrder=order;
            if(tiled){renderer.drawMode=SpriteDrawMode.Tiled;renderer.size=size;}else go.transform.localScale=new Vector3(size.x/s.bounds.size.x,size.y/s.bounds.size.y,1);
        }
        static void Bgm()
        {
            var c=Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");var s=new SerializedObject(c);var rows=s.FindProperty("scenes");
            for(int i=0;i<rows.arraySize;i++)if(rows.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue==Chapter2Main19Flow.Field)throw new InvalidOperationException("기존 Field10 BGM을 덮지 않습니다.");
            int n=rows.arraySize++;var row=rows.GetArrayElementAtIndex(n);row.FindPropertyRelative("sceneName").stringValue=Chapter2Main19Flow.Field;
            var clip=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/Music/Paths_of_Cracked_Earth.mp3");if(clip==null)throw new InvalidOperationException("정식 탐색곡 누락");
            row.FindPropertyRelative("clip").objectReferenceValue=clip;s.ApplyModifiedPropertiesWithoutUndo();Save(c);
        }
        static void Manifest()
        {
            var rows=new Dictionary<string,ProjectLimitless.UI.DialogueLine>();
            foreach(string path in new[]{"path.vision","path.hearing","path.intellectual","path.mobility","path.emotional-scar"})
                foreach(int index in new[]{2,3,5,8,10,11,12})foreach(var line in Main19DialogueCatalog.Get(index,path,true))rows[line.DialogueId]=line;
            string Csv(string s)=>"\""+(s??"").Replace("\"","\"\"")+"\"";
            File.WriteAllLines(Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트/Main19_TTS_Manifest.csv")),new[]{"DialogueId,SpeakerId,Speaker,Text,VoiceExpected,Status"}.Concat(rows.Values.OrderBy(x=>x.DialogueId).Select(l=>string.Join(",",Csv(l.DialogueId),Csv(l.SpeakerId),Csv(l.SpeakerName),Csv(l.Message),l.IsPlayer||l.IsDirection?"false,NOT_EXPECTED":"true,TTS_PENDING"))));
        }
    }
}
