using System;
using System.IO;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using ProjectLimitless.Audio;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>격리 프로젝트에서 검수 원본17개와 Main20 새 콘텐츠만 생성합니다. 시작 마을과 Field10 원본을 재생성하지 않습니다.</summary>
    public static class Main20ContentBuilder
    {
        public const string ScenePath="Assets/_Project/Scenes/Field_11_DeepCore.unity";
        private const string Root="Assets/_Project/Resources/",Art=Root+"Main20/";
        public static void BuildBatch()
        {try{Build();Debug.Log("MAIN20_BUILD_PASS");EditorApplication.Exit(0);}catch(Exception e){Debug.LogException(e);EditorApplication.Exit(1);}}
        public static void Build()
        {
            if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main20QA/Client/Assets"))throw new InvalidOperationException("Main20 생성은 격리 Batch 프로젝트에서만 실행합니다.");
            Import();
            var frames=AssetDatabase.LoadAllAssetsAtPath(Art+"Boss/Veinfire_Colossus_Sprite_Sheet.png").OfType<Sprite>().OrderBy(x=>x.name).ToArray();
            if(frames.Length!=16)throw new InvalidOperationException("Boss frames !=16");
            var monster=New<MonsterDefinition>(Root+"MonsterDefinitions/18_VeinfireColossus.asset");
            monster.ConfigureContent(Battle.BattleMain20BossRuntime.MonsterId,"열맥 거신",15,140,40,1200,300,10,.1f,new Vector2(3,3),3,frames[0],
                frames.Take(4).ToArray(),frames.Take(4).ToArray(),frames.Skip(4).Take(4).ToArray(),frames.Skip(8).Take(4).ToArray(),frames.Skip(12).Take(4).ToArray(),null,Array.Empty<MonsterLootEntry>());Save(monster);
            string[] ids={"enter_deep_core","inspect_core_rift","follow_colossus_trace","reach_colossus_arena","confront_veinfire_colossus","defeat_veinfire_colossus","inspect_collapsed_core","confirm_heat_recession","return_from_deep_core"};
            string[] labels={"열맥 심부로 들어가세요.","심부의 거대한 균열을 조사하세요.","거신의 흔적을 따라가세요.","심부의 열린 지대로 이동하세요.","열맥 거신을 확인하세요.","열맥 거신을 쓰러뜨리세요.","무너진 거신의 흔적을 조사하세요.","주변 열기의 변화를 확인하세요.","서부로 돌아갈 길을 확보하세요."};
            var steps=ids.Select((id,i)=>{var step=new QuestObjectiveDefinition();step.ConfigureForAudit(id,labels[i],i==0||i==2||i==3||i==8?QuestObjectiveType.ReachLocation:i==5?QuestObjectiveType.DefeatEncounter:QuestObjectiveType.Interact,Chapter2Main20Flow.Targets[i]);return step;}).ToArray();
            var quest=New<QuestDefinition>(Root+"QuestDefinitions/Main20_ColossusOfTheDepths.asset");quest.ConfigureForAudit(Chapter2Main20Flow.QuestId,"심부의 거신",QuestType.Main,steps,new RewardBundle{Experience=100,Currency=80},new[]{Chapter2Main19Flow.QuestId});quest.ConfigureDescription("열맥 심부의 거신을 쓰러뜨리고 남은 진동과 완화된 열기를 확인한다.");Save(quest);
            var spawn=New<FieldMonsterSpawnDefinition>(Root+"StoryEncounterReturns/Main20_ColossusReturn.asset");spawn.Configure(Chapter2Main20Flow.Field,Chapter2Main20Flow.EncounterId,monster,Chapter2Main20Flow.Positions[5],.6f,0);spawn.SetNonRespawningBoss(true);Save(spawn);
            Connect("Field10_WestToField11",Chapter2Main19Flow.Field,-1,"Spawn_From_Field11",Chapter2Main20Flow.Field,"Spawn_From_Field10");
            Connect("Field11_EastToField10",Chapter2Main20Flow.Field,1,"Spawn_From_Field10",Chapter2Main19Flow.Field,"Spawn_From_Field11");
            Field();Music();
            if(!EditorBuildSettings.scenes.Any(s=>s.path==ScenePath))EditorBuildSettings.scenes=EditorBuildSettings.scenes.Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray();
            AssetDatabase.SaveAssets();
        }
        private static T New<T>(string path)where T:ScriptableObject
        {if(File.Exists(path))throw new InvalidOperationException("기존 자산 보호: "+path);Directory.CreateDirectory(Path.GetDirectoryName(path));var v=ScriptableObject.CreateInstance<T>();AssetDatabase.CreateAsset(v,path);return v;}
        private static void Save(UnityEngine.Object value){EditorUtility.SetDirty(value);AssetDatabase.SaveAssetIfDirty(value);}
        private static void Import()
        {
            string input=Path.Combine(Directory.GetCurrentDirectory(),"Main20Input");
            foreach(string folder in new[]{"Boss","Environment","VFX"})
            {
                Directory.CreateDirectory(Art+folder);
                foreach(string source in Directory.GetFiles(Path.Combine(input,"Art",folder),"*.png"))
                {
                    string path=Art+folder+"/"+Path.GetFileName(source);if(File.Exists(path))throw new InvalidOperationException("기존 아트 보호: "+path);File.Copy(source,path);
                    AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceSynchronousImport);var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                    importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=folder=="Boss"?SpriteImportMode.Multiple:SpriteImportMode.Single;
                    importer.spritePixelsPerUnit=folder=="Boss"?314:folder=="VFX"?512:128;importer.filterMode=FilterMode.Point;importer.textureCompression=TextureImporterCompression.Uncompressed;
                    importer.maxTextureSize=2048;importer.npotScale=TextureImporterNPOTScale.None;importer.mipmapEnabled=false;importer.alphaIsTransparency=true;
                    importer.wrapMode=TextureWrapMode.Clamp;var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);settings.spriteMeshType=SpriteMeshType.FullRect;settings.spriteAlignment=(int)SpriteAlignment.Custom;settings.spritePivot=folder=="Boss"?new Vector2(.5f,0):new Vector2(.5f,.5f);importer.SetTextureSettings(settings);importer.SaveAndReimport();
                    if(folder!="Boss")continue;
                    var factories=new SpriteDataProviderFactories();factories.Init();var provider=factories.GetSpriteEditorDataProviderFromObject(importer);provider.InitSpriteEditorDataProvider();
                    var rects=Enumerable.Range(0,16).Select(i=>new SpriteRect{name=Path.GetFileNameWithoutExtension(path)+"_"+i.ToString("00"),rect=new Rect(i%4*314,(3-i/4)*314,314,314),alignment=SpriteAlignment.BottomCenter,pivot=new Vector2(.5f,0),spriteID=GUID.Generate()}).ToArray();
                    provider.SetSpriteRects(rects);provider.Apply();importer.SaveAndReimport();
                }
            }
            string music="Assets/_Project/Audio/Music/The_Weight_of_Crowns.mp3";if(File.Exists(music))throw new InvalidOperationException("기존 곡 보호");File.Copy(Path.Combine(input,"The_Weight_of_Crowns.mp3"),music);AssetDatabase.ImportAsset(music,ImportAssetOptions.ForceSynchronousImport);
            var audio=(AudioImporter)AssetImporter.GetAtPath(music);var sample=audio.defaultSampleSettings;sample.loadType=AudioClipLoadType.Streaming;sample.compressionFormat=AudioCompressionFormat.Vorbis;sample.quality=1;sample.sampleRateSetting=AudioSampleRateSetting.PreserveSampleRate;sample.preloadAudioData=false;audio.defaultSampleSettings=sample;audio.forceToMono=false;audio.SaveAndReimport();
        }
        private static void Connect(string file,string scene,int side,string ownSpawn,string target,string targetSpawn)
        {var c=New<FieldConnectionDefinition>(Root+"FieldConnections/"+file+".asset");c.Configure(scene,file,ownSpawn,new Vector2(side*7.8f,0),new Vector2(side*10.25f,0),new Vector2(1.1f,3),target,targetSpawn,false);Save(c);}
        private static void Field()
        {
            if(File.Exists(ScenePath))throw new InvalidOperationException("기존 Field11 보호");
            AssetDatabase.CopyAsset("Assets/_Project/Scenes/Field_10_BurningPulse.unity",ScenePath);
            var scene=EditorSceneManager.OpenScene(ScenePath,OpenSceneMode.Single);
            CleanPreviousPresentation(scene);
            var env=scene.GetRootGameObjects().Single(x=>x.name=="FieldEnvironment").transform;
            foreach(Transform c in env.Cast<Transform>().ToArray())UnityEngine.Object.DestroyImmediate(c.gameObject);
            Visual(env,"Ground","DeepCore_Ground_Base",Vector2.zero,new Vector2(21,15),-20,true);
            Visual(env,"CoreRiftHot","DeepCore_Rift_02",new Vector2(5,1.1f),new Vector2(3,2),-9);
            Visual(env,"ColossusTrace","DeepCore_ColossusTrace",new Vector2(2.8f,.2f),new Vector2(3,1.8f),-8);
            Visual(env,"ArenaPressure","DeepCore_Arena_PressureMark",new Vector2(-1.5f,0),new Vector2(7,5),-7);
            Visual(env,"CollapsedCore","DeepCore_CollapsedCore",new Vector2(-2,1),new Vector2(3,2),1);
            Visual(env,"HeatRecession","DeepCore_HeatRecession",new Vector2(1,-1.2f),new Vector2(5,3),-6);
            Title(env);
            foreach(float y in new[]{-4.7f,4.7f})
            {var cliff=Visual(env,"CliffBoundary_"+y,"DeepCore_Cliff_Boundary",new Vector2(-4,y),new Vector2(3,2),2);ConfigureCliff(cliff);}
            var boss=new GameObject("VeinfireColossusField",typeof(SpriteRenderer));boss.transform.SetParent(env,false);boss.transform.position=new Vector2(-2,0);boss.transform.localScale=Vector3.one*3;boss.GetComponent<SpriteRenderer>().sprite=Resources.Load<MonsterDefinition>("MonsterDefinitions/18_VeinfireColossus").FieldSprite;boss.GetComponent<SpriteRenderer>().sortingOrder=5;
            var bounds=scene.GetRootGameObjects().Select(x=>x.GetComponent<WorldBounds2D>()).First(x=>x!=null);
            foreach(Transform c in bounds.transform.Cast<Transform>().Where(x=>x.name.StartsWith("Boundary_")).ToArray())UnityEngine.Object.DestroyImmediate(c.gameObject);
            bounds.Configure(Vector2.zero,new Vector2(21,15));WorldBounds2D.CreateBoundaryColliders(bounds.transform,bounds.Bounds,.3f,new WorldBoundaryOpening(WorldBoundarySide.Right,0,3));
            if(!EditorSceneManager.SaveScene(scene,ScenePath))throw new InvalidOperationException("Field11 저장 실패");
        }
        private static GameObject Visual(Transform parent,string name,string art,Vector2 at,Vector2 size,int order,bool tile=false)
        {
            var sprite=AssetDatabase.LoadAssetAtPath<Sprite>(Art+"Environment/"+art+".png");if(sprite==null)throw new InvalidOperationException("Sprite 누락 "+art);
            var go=new GameObject(name,typeof(SpriteRenderer));go.transform.SetParent(parent,false);go.transform.position=at;var renderer=go.GetComponent<SpriteRenderer>();renderer.sprite=sprite;renderer.sortingOrder=order;
            if(tile){renderer.drawMode=SpriteDrawMode.Tiled;renderer.size=size;}else go.transform.localScale=new Vector3(size.x/sprite.bounds.size.x,size.y/sprite.bounds.size.y,1);return go;
        }
        // 절벽의 그림자/상단 장식 대신 하부 암석 폭을 막습니다. 중앙 Quest 동선과 출입구는 겹치지 않습니다.
        private static void ConfigureCliff(GameObject cliff)
        {
            var box=cliff.GetComponent<BoxCollider2D>()??cliff.AddComponent<BoxCollider2D>();
            var scale=cliff.transform.lossyScale;
            box.size=new Vector2(3/scale.x,1/scale.y);box.offset=new Vector2(0,-.5f/scale.y);
        }
        private static void CleanPreviousPresentation(UnityEngine.SceneManagement.Scene scene)
        {
            // 복사 출발점 Field10의 목격 실루엣 Presenter는 Field11에 속하지 않습니다. Field10 원본은 보존합니다.
            foreach(var root in scene.GetRootGameObjects())if(root.GetComponent<Main19FieldArtPresentation>()!=null)UnityEngine.Object.DestroyImmediate(root);
        }
        private static void Title(Transform environment)
        {
            if(environment.Find("DeepCoreTitle")!=null)return;
            var go=new GameObject("DeepCoreTitle",typeof(TextMesh));go.transform.SetParent(environment,false);go.transform.position=new Vector2(0,6.5f);
            var text=go.GetComponent<TextMesh>();text.text="열맥 심부";text.characterSize=.16f;text.anchor=TextAnchor.MiddleCenter;
            text.font=Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");text.fontSize=32;text.GetComponent<MeshRenderer>().sharedMaterial=text.font.material;
        }
        public static void FinalizeBatch()
        {
            try
            {
                if(!Application.isBatchMode||!Application.dataPath.Replace('\\','/').Contains("/Temp/Main20QA/Client/Assets"))throw new InvalidOperationException("격리 Main20QA 전용");
                var scene=EditorSceneManager.OpenScene(ScenePath,OpenSceneMode.Single);
                CleanPreviousPresentation(scene);
                var env=scene.GetRootGameObjects().Single(x=>x.name=="FieldEnvironment");
                Title(env.transform);
                foreach(var box in env.GetComponentsInChildren<BoxCollider2D>())if(box.name.StartsWith("CliffBoundary_"))ConfigureCliff(box.gameObject);
                if(!EditorSceneManager.SaveScene(scene,ScenePath))throw new InvalidOperationException("Field11 최종 저장 실패");
                Debug.Log("MAIN20_FINALIZE_PASS");EditorApplication.Exit(0);
            }catch(Exception e){Debug.LogException(e);EditorApplication.Exit(1);}
        }
        private static void Music()
        {
            var catalog=Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");var so=new SerializedObject(catalog);var rows=so.FindProperty("scenes");int index=rows.arraySize++;
            var row=rows.GetArrayElementAtIndex(index);row.FindPropertyRelative("sceneName").stringValue=Chapter2Main20Flow.Field;row.FindPropertyRelative("clip").objectReferenceValue=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/Music/Paths_of_Cracked_Earth.mp3");
            so.FindProperty("veinfireColossusClip").objectReferenceValue=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/Music/The_Weight_of_Crowns.mp3");so.ApplyModifiedPropertiesWithoutUndo();Save(catalog);
        }
    }
}
