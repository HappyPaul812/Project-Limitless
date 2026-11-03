using System;
using System.IO;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

// 이번 fixture에만 승인 프레임과 기존 F2/Battle Scene을 연결합니다. 본편 BuildSettings는 수정하지 않습니다.
[InitializeOnLoad]
public static class OwlBattle85Batch
{
    const string Key="OwlBattle85Armed";
    static double deadline;
    static OwlBattle85Batch()
    {
        EditorApplication.playModeStateChanged+=state=>{
            if(state==PlayModeStateChange.EnteredPlayMode&&SessionState.GetBool(Key,false))
            {deadline=EditorApplication.timeSinceStartup+360;EditorApplication.update+=Poll;}
        };
    }
    public static void Run()
    {
        if(!Application.isBatchMode||Application.unityVersion!="6000.6.5f1"||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/Main24FrostOwlFinish20261103/QA/Client/Assets"))throw new Exception("격리 #85 fixture만 허용");
        Directory.CreateDirectory(OwlBattle85Probe.Output);
        string root="Assets/_Project/Resources/Main24FrostOwlCandidate";
        foreach(string raw in Directory.GetFiles(root,"*.png",SearchOption.AllDirectories))
        {
            string p=raw.Replace('\\','/');var i=(TextureImporter)AssetImporter.GetAtPath(p);i.textureType=TextureImporterType.Sprite;i.spriteImportMode=SpriteImportMode.Single;i.spritePixelsPerUnit=512;i.maxTextureSize=2048;i.mipmapEnabled=false;i.alphaIsTransparency=true;i.isReadable=true;i.textureCompression=TextureImporterCompression.Uncompressed;i.npotScale=TextureImporterNPOTScale.None;
            var settings=new TextureImporterSettings();i.ReadTextureSettings(settings);settings.spriteAlignment=(int)SpriteAlignment.Center;settings.spritePivot=new Vector2(.5f,.5f);settings.spriteMeshType=SpriteMeshType.FullRect;i.SetTextureSettings(settings);i.SaveAndReimport();
        }
        var idle=Directory.GetFiles(root+"/Idle","*.png").OrderBy(x=>x,StringComparer.Ordinal).Select(x=>AssetDatabase.LoadAssetAtPath<Sprite>(x.Replace('\\','/'))).ToArray();
        var attack=Directory.GetFiles(root+"/Attack","*.png").OrderBy(x=>x,StringComparer.Ordinal).Select(x=>AssetDatabase.LoadAssetAtPath<Sprite>(x.Replace('\\','/'))).ToArray();
        var template=Resources.Load<MonsterDefinition>("MonsterDefinitions/01_GrassSlime");
        var def=AssetDatabase.LoadAssetAtPath<MonsterDefinition>(root+"/Definition.asset");if(def==null){def=ScriptableObject.CreateInstance<MonsterDefinition>();AssetDatabase.CreateAsset(def,root+"/Definition.asset");}
        // 정식 Owl 수치가 아닙니다. 기존 기본 몬스터 수치를 그대로 차용하고 미승인 보상은0입니다.
        def.ConfigureContent(Main24FrostOwlCandidate.MonsterId,"서리올빼미 [격리 수치]",template.MonsterLevel,0,0,template.MaxHp,template.BattleAttackPercent,template.BattleAgility,template.FieldMoveSpeed,Vector2.one,1,idle[0],idle,Array.Empty<Sprite>(),attack,Array.Empty<Sprite>(),Array.Empty<Sprite>(),null,Array.Empty<MonsterLootEntry>());
        EditorUtility.SetDirty(def);
        var spawn=AssetDatabase.LoadAssetAtPath<FieldMonsterSpawnDefinition>(root+"/Encounter.asset");if(spawn==null){spawn=ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>();AssetDatabase.CreateAsset(spawn,root+"/Encounter.asset");}
        spawn.Configure(Main24F2Traversal.SceneId,"CANDIDATE_f2_owl_repeatable_qa",def,Main24FrostOwlCandidate.EncounterPosition,.5f,0);EditorUtility.SetDirty(spawn);AssetDatabase.SaveAssets();
        var list=EditorBuildSettings.scenes.ToList();foreach(string name in new[]{"World_NorthF2_CANDIDATE","World_FrostwayVillage","Battle"})
        {var path="Assets/_Project/Scenes/"+name+".unity";if(File.Exists(path)&&!list.Any(x=>x.path==path))list.Add(new EditorBuildSettingsScene(path,true));}
        EditorBuildSettings.scenes=list.ToArray();
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        new GameObject("QA85SyntheticSeedFirst").AddComponent<OwlBattle85Probe>();Directory.CreateDirectory("Assets/QA85");EditorSceneManager.SaveScene(scene,"Assets/QA85/OwlBattle85Entry.unity");
        SessionState.SetBool(Key,true);EditorApplication.isPlaying=true;
    }
    static void Poll()
    {
        var probe=UnityEngine.Object.FindAnyObjectByType<OwlBattle85Probe>();
        if(probe!=null&&probe.Done)
        {SessionState.SetBool(Key,false);EditorApplication.update-=Poll;int code=probe.Failed?1:0;EditorApplication.isPlaying=false;EditorApplication.delayCall+=()=>EditorApplication.Exit(code);}
        else if(EditorApplication.timeSinceStartup>deadline){File.WriteAllText(Path.Combine(OwlBattle85Probe.Output,"timeout.txt"),"Timeout");EditorApplication.Exit(2);}
    }
}
