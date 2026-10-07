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
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>검수한 Main18 납품 원본과 새 콘텐츠만 등록합니다. 이전 Scene/몬스터/상점은 다시 생성하지 않습니다.</summary>
    public static class Main18ContentBuilder
    {
        public const string ScenePath = "Assets/_Project/Scenes/Field_09_ObsidianScar.unity";
        private const string Root = "Assets/_Project/Resources/";
        private const string Art = Root + "Main18/";
        public static readonly string[] Objectives = { "return_to_arbel", "report_to_leon", "reach_deep_route", "enter_obsidian_scar",
            "inspect_obsidian_ground", "inspect_heat_vent", "defeat_obsidian_beetle", "inspect_overheat_residue", "follow_deep_vibration",
            "witness_scorching_watcher", "defeat_scorching_watcher", "inspect_after_watcher", "locate_deeper_route" };
        public static string Build()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) throw new InvalidOperationException("Edit Mode에서만 생성합니다.");
            ImportArt();
            var beetle = Monster("16_ObsidianBeetle", "obsidian_beetle", "흑요석 갑충", "Obsidian_Beetle", 13,250,240,8,50,14);
            var watcher = Monster("17_ScorchingWatcher", "scorching_watcher", "작열 감시자", "Scorching_Watcher", 14,560,280,11,85,22);
            var item = Asset<ItemDefinition>(Root+"ItemDefinitions/CoolingRemedy.asset");
            item.ConfigureContent(Chapter2Main18Flow.CoolingItem, "냉각약", "살아 있는 아군 한 명의 과열을 모두 제거합니다. 정화와 네 상태 해제약으로는 과열을 제거할 수 없습니다.",
                ItemCategory.Consumable,99,40,20,AssetDatabase.LoadAssetAtPath<Sprite>(Art+"UI/Item_Cooling_Remedy.png"),Color.white,ItemUseType.Battle,"과열 중첩 모두 제거");
            item.ConfigureEffect(ItemEffectType.RemoveOverheat,0); Save(item); ItemCatalog.ReloadForAudit();
            var starter=Resources.Load<ShopDefinition>("ShopDefinitions/StarterVillageGeneralShop");
            var shop=Asset<ShopDefinition>(Root+"ShopDefinitions/ArbelGeneralShop.asset");
            shop.ConfigureContent("shop_general_arbel", "아르벨 잡화점",starter.ItemIds.Concat(new[]{Chapter2Main18Flow.CoolingItem}).Distinct().ToArray()); Save(shop);
            string[] labels={"아르벨로 돌아가세요.","레온에게 붉은 균열 안쪽 상황을 보고하세요.","붉은 균열의 더 깊은 길로 돌아가세요.","흑요석 상흔에 들어가세요.",
                "검게 굳은 지면을 조사하세요.","열기가 새어 나오는 틈을 조사하세요.","흑요석 갑충을 물리치세요.","전투 뒤 남은 열기를 확인하세요.",
                "더 깊은 곳에서 이어지는 진동을 확인하세요.","앞을 막고 있는 존재를 확인하세요.","작열 감시자를 물리치세요.","감시자가 쓰러진 자리를 조사하세요.","열기와 진동이 이어지는 더 깊은 길을 확인하세요."};
            var steps=Objectives.Select((id,i)=>{var step=new QuestObjectiveDefinition(); step.ConfigureForAudit(id,labels[i],
                i==1?QuestObjectiveType.TalkToNpc:i==0||i==2||i==3||i==9?QuestObjectiveType.ReachLocation:
                i==6||i==10?QuestObjectiveType.DefeatEncounter:QuestObjectiveType.Interact,Chapter2Main18Flow.Targets[i]);return step;}).ToArray();
            var quest=Asset<QuestDefinition>(Root+"QuestDefinitions/Main18_BlackHeat.asset");
            quest.ConfigureForAudit(Chapter2Main18Flow.QuestId,"검은 열기",QuestType.Main,steps,new RewardBundle{Experience=70,Currency=60},new[]{Chapter2Main17Flow.QuestId});
            quest.ConfigureDescription("아르벨에서 냉각약을 준비하고 흑요석 상흔의 열기와 진동을 조사한다.");Save(quest);QuestCatalog.ReloadForAudit();
            Connection("Field08_WestToField09",Chapter2Main18Flow.PreviousField,"west_to_field09","Spawn_From_Field09",-7.2f,
                Chapter2Main18Flow.Field,"Spawn_From_Field08",false);
            Connection("Field09_EastToField08",Chapter2Main18Flow.Field,"east_to_field08","Spawn_From_Field08",7.2f,
                Chapter2Main18Flow.PreviousField,"Spawn_From_Field09",true);
            Spawn("StoryEncounterReturns/Main18_BeetleReturn",Chapter2Main18Flow.BeetleEncounter,beetle,Chapter2Main18Flow.Positions[6]);
            Spawn("StoryEncounterReturns/Main18_WatcherReturn",Chapter2Main18Flow.WatcherEncounter,watcher,Chapter2Main18Flow.Positions[10]);
            Spawn("MonsterSpawns/Field09_Beetle01","field09_beetle_01",beetle,new Vector2(4,-4.6f));
            Spawn("MonsterSpawns/Field09_Lizard01","field09_lizard_01",Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Single(x=>x.MonsterId=="fissure_lizard"),new Vector2(-.5f,4.7f));
            Spawn("MonsterSpawns/Field09_Wraith01","field09_wraith_01",Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").Single(x=>x.MonsterId=="ember_wraith"),new Vector2(-2.5f,-4.6f));
            BuildField();
            // Scene이 이미 있더라도 Build 등록은 복원합니다. 생성기 재실행은 Scene 본문을 덮지 않습니다.
            if(!EditorBuildSettings.scenes.Any(x=>x.path==ScenePath))EditorBuildSettings.scenes=EditorBuildSettings.scenes.Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray();
            Bgm(); Manifest();
            return "Main18 Art17/Monster2/Item/Shop/Quest13/Field09/Connections/BGM/TTS registered";
        }
        private static T Asset<T>(string path) where T:ScriptableObject
        {
            var value=AssetDatabase.LoadAssetAtPath<T>(path);if(value!=null)return value;
            Directory.CreateDirectory(Path.GetDirectoryName(path));AssetDatabase.Refresh();
            value=ScriptableObject.CreateInstance<T>();AssetDatabase.CreateAsset(value,path);return value;
        }
        private static void Save(UnityEngine.Object value){EditorUtility.SetDirty(value);AssetDatabase.SaveAssetIfDirty(value);}
        private static void ImportArt()
        {
            string source=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../Temp/Main18Audit"));
            foreach(string folder in new[]{"Environment","Monsters","UI","VFX"})
            {
                Directory.CreateDirectory(Art+folder);
                foreach(string png in Directory.GetFiles(Path.Combine(source,folder),"*.png"))
                {
                    string path=Art+folder+"/"+Path.GetFileName(png);
                    // 존재하는 등록본도 원본과 같아야 합니다. 재실행이 사용자 그림을 덮지 않게 막습니다.
                    if(File.Exists(path)&&!File.ReadAllBytes(path).SequenceEqual(File.ReadAllBytes(png))) throw new InvalidOperationException("기존 Art와 원본 충돌: "+path);
                    if(!File.Exists(path))File.Copy(png,path);
                    AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceSynchronousImport);
                    var importer=(TextureImporter)AssetImporter.GetAtPath(path);
                    importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=folder=="Monsters"?SpriteImportMode.Multiple:SpriteImportMode.Single;
                    importer.spritePixelsPerUnit=folder=="Monsters"?314:128;importer.filterMode=FilterMode.Point;
                    importer.textureCompression=TextureImporterCompression.Uncompressed;importer.maxTextureSize=2048;
                    importer.npotScale=TextureImporterNPOTScale.None;importer.alphaIsTransparency=true;importer.mipmapEnabled=false;importer.wrapMode=TextureWrapMode.Clamp;
                    var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);settings.spriteMeshType=SpriteMeshType.FullRect;importer.SetTextureSettings(settings);importer.SaveAndReimport();
                    if(folder!="Monsters")continue;
                    var factories=new SpriteDataProviderFactories();factories.Init();var provider=factories.GetSpriteEditorDataProviderFromObject(importer);provider.InitSpriteEditorDataProvider();
                    // 기존 5종의 발밑 Pivot·행 순서·314px 셀을 따르고 재실행 시 기존 Sprite ID를 보존합니다.
                    var old=provider.GetSpriteRects();var rects=new SpriteRect[16];
                    for(int i=0;i<16;i++){string name=Path.GetFileNameWithoutExtension(path)+"_"+i.ToString("00");var previous=old.FirstOrDefault(x=>x.name==name);
                        rects[i]=new SpriteRect{name=name,rect=new Rect(i%4*314,(3-i/4)*314,314,314),alignment=SpriteAlignment.BottomCenter,pivot=new Vector2(.5f,0),spriteID=previous==null?GUID.Generate():previous.spriteID};}
                    provider.SetSpriteRects(rects);provider.Apply();importer.SaveAndReimport();
                }
            }
        }
        private static MonsterDefinition Monster(string filename,string id,string name,string sprite,int lv,int hp,int attack,int agility,int exp,int talent)
        {
            var frames=AssetDatabase.LoadAllAssetsAtPath(Art+"Monsters/"+sprite+"_Sprite_Sheet.png").OfType<Sprite>().OrderBy(x=>x.name).ToArray();
            if(frames.Length!=16)throw new InvalidOperationException("Main18 sheet frame count: "+id);
            Sprite[] Row(int n)=>frames.Skip(n*4).Take(4).ToArray();
            var value=Asset<MonsterDefinition>(Root+"MonsterDefinitions/"+filename+".asset");
            value.ConfigureContent(id,name,lv,exp,talent,hp,attack,agility,.8f,new Vector2(1.25f,1.25f),1f,frames[0],Row(0),Row(0),Row(1),Row(2),Row(3),null,Array.Empty<MonsterLootEntry>());Save(value);return value;
        }
        private static void Spawn(string filename,string id,MonsterDefinition monster,Vector2 at)
        {
            var value=Asset<FieldMonsterSpawnDefinition>(Root+filename+".asset");value.Configure(Chapter2Main18Flow.Field,id,monster,at,.6f,35);value.SetNonRespawningBoss(false);Save(value);
        }
        private static void Connection(string file,string scene,string id,string ownSpawn,float x,string target,string targetSpawn,bool replace)
        {
            var value=Asset<FieldConnectionDefinition>(Root+"FieldConnections/"+file+".asset");
            value.Configure(scene,id,ownSpawn,new Vector2(x,0),new Vector2(Math.Sign(x)*10.25f,0),new Vector2(1.1f,3),target,targetSpawn,replace);Save(value);
        }
        private static void BuildField()
        {
            if(AssetDatabase.LoadAssetAtPath<SceneAsset>(ScenePath)!=null)return; // 사용자 후속 편집을 재생성하지 않습니다.
            if(!AssetDatabase.CopyAsset("Assets/_Project/Scenes/Field_08_RedRift.unity",ScenePath))throw new InvalidOperationException("Field09 copy failed");
            var active=UnityEngine.SceneManagement.SceneManager.GetActiveScene();var selected=Selection.objects;
            var scene=EditorSceneManager.OpenScene(ScenePath,OpenSceneMode.Additive);
            try
            {
                var environment=scene.GetRootGameObjects().Single(x=>x.name=="FieldEnvironment").transform;
                foreach(Transform child in environment.Cast<Transform>().ToArray())UnityEngine.Object.DestroyImmediate(child.gameObject);
                Visual(environment,"Ground", "Ground_Base",Vector2.zero,new Vector2(21,15),-20);
                Visual(environment,"GroundCrackedWest","Ground_Cracked_01",new Vector2(-6,0),new Vector2(9,15),-19);
                // 깊은 틈은 북/남의 우회 동선을 남기고 조사 위치·선택 조우·동쪽 출입구를 침범하지 않습니다.
                Visual(environment,"Fissure","Fissure_Edge",new Vector2(-5,3.8f),new Vector2(2.1f,2.5f),-8);
                Visual(environment,"Glow","Fissure_Glow",new Vector2(-5,3.8f),new Vector2(2.1f,2.5f),-7);
                Visual(environment,"HeatVent","HeatVent_01",new Vector2(3,-2),new Vector2(1.8f,1.8f),-6);
                Visual(environment,"Ash","AshPatch_01",new Vector2(0,-1.2f),new Vector2(2.1f,2.1f),-5);
                Visual(environment,"Slab","Obsidian_Slab_01",new Vector2(5.2f,1),new Vector2(2,2),-5);
                Visual(environment,"Spire","Obsidian_Spire_01",new Vector2(-8.8f,4.8f),new Vector2(2.5f,2.5f),2).AddComponent<BoxCollider2D>();
                Visual(environment,"Cliff","Cliff_Blocker_01",new Vector2(-9,0),new Vector2(2.6f,2.6f),2);
                var bounds=scene.GetRootGameObjects().Select(x=>x.GetComponent<WorldBounds2D>()).First(x=>x!=null);
                foreach(Transform child in bounds.transform.Cast<Transform>().Where(x=>x.name.StartsWith("Boundary_")).ToArray())UnityEngine.Object.DestroyImmediate(child.gameObject);
                bounds.Configure(Vector2.zero,new Vector2(21,15));WorldBounds2D.CreateBoundaryColliders(bounds.transform,bounds.Bounds,.3f,new WorldBoundaryOpening(WorldBoundarySide.Right,0,3));
                var title=new GameObject("ObsidianScarTitle",typeof(TextMesh));title.transform.SetParent(environment,false);title.transform.position=new Vector2(0,6.5f);
                var text=title.GetComponent<TextMesh>();text.text="흑요석 상흔";text.characterSize=.16f;text.anchor=TextAnchor.MiddleCenter;
                if(!EditorSceneManager.SaveScene(scene,ScenePath))throw new InvalidOperationException("Field09 save failed");
            }
            finally{EditorSceneManager.CloseScene(scene,true);UnityEngine.SceneManagement.SceneManager.SetActiveScene(active);Selection.objects=selected;}
            if(!EditorBuildSettings.scenes.Any(x=>x.path==ScenePath))EditorBuildSettings.scenes=EditorBuildSettings.scenes.Concat(new[]{new EditorBuildSettingsScene(ScenePath,true)}).ToArray();
        }
        private static GameObject Visual(Transform parent,string name,string asset,Vector2 at,Vector2 size,int order)
        {
            var value=new GameObject("ObsidianScar_"+name,typeof(SpriteRenderer));value.transform.SetParent(parent,false);value.transform.position=at;
            var renderer=value.GetComponent<SpriteRenderer>();renderer.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(Art+"Environment/ObsidianScar_"+asset+".png");renderer.sortingOrder=order;
            // 지면은 원본을 반복해 확대 흐림을 줄이고 Props만 지정 월드 크기로 맞춥니다.
            if (asset.StartsWith("Ground")) { renderer.drawMode=SpriteDrawMode.Tiled; renderer.size=size; return value; }
            value.transform.localScale=new Vector3(size.x/renderer.sprite.bounds.size.x,size.y/renderer.sprite.bounds.size.y,1);return value;
        }
        private static void Bgm()
        {
            var catalog=Resources.Load<BgmSceneCatalog>("Audio/Music/BgmSceneCatalog");var so=new SerializedObject(catalog);var entries=so.FindProperty("scenes");
            int i=0;for(;i<entries.arraySize;i++)if(entries.GetArrayElementAtIndex(i).FindPropertyRelative("sceneName").stringValue==Chapter2Main18Flow.Field)break;
            if(i==entries.arraySize)entries.arraySize++;
            var row=entries.GetArrayElementAtIndex(i);row.FindPropertyRelative("sceneName").stringValue=Chapter2Main18Flow.Field;
            row.FindPropertyRelative("clip").objectReferenceValue=AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/_Project/Audio/Music/The_Weight_of_Obsidian.mp3");
            so.ApplyModifiedPropertiesWithoutUndo();Save(catalog);
        }
        private static void Manifest()
        {
            // 다섯 Path의 공통 NPC 발화를 중복 집계하지 않습니다. 표시 페이지별 ID는 고유합니다.
            var rows=new Dictionary<string,ProjectLimitless.UI.DialogueLine>();
            foreach(string path in new[]{"path.vision","path.hearing","path.intellectual","path.mobility","path.emotional-scar"})
                foreach(int i in new[]{1,2,4,5,7,8,9,11,12,13})foreach(var line in Main18DialogueCatalog.Get(i,path,true))rows[line.DialogueId]=line;
            string Csv(string s)=>"\""+(s??"").Replace("\"","\"\"")+"\"";
            string output=Path.GetFullPath(Path.Combine(Application.dataPath,"../../../문서/00_프로젝트/Main18_TTS_Manifest.csv"));
            File.WriteAllLines(output,new[]{"DialogueId,SpeakerId,Speaker,Text,VoiceExpected,Status"}.Concat(rows.Values.OrderBy(x=>x.DialogueId).Select(x=>
                string.Join(",",Csv(x.DialogueId),Csv(x.SpeakerId),Csv(x.SpeakerName),Csv(x.Message),x.SpeakerId!=""&&x.SpeakerId!="player"?"true,TTS_PENDING":"false,NOT_EXPECTED"))));
        }
    }
}
