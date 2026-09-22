#if UNITY_EDITOR
using System;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.Monster;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>사용자 최종 몬스터 시트를 수정하지 않고 Slice·Definition·Field 03 데이터를 생성합니다.</summary>
    public static class Field03Main08ContentBuilder
    {
        const string BeetlePath="Assets/_Project/Art/Monsters/05_MossBeetle/Moss_Beetle_Battle_Final.png";
        const string BatPath="Assets/_Project/Art/Monsters/06_ShadeBat/Shade_Bat_Battle_Final.png";
        const string ResourceRoot="Assets/_Project/Resources";

        [MenuItem("Project Limitless/Content/Build Field 03 and Main 08")]
        public static void Build()
        {
            Slice(BeetlePath,6,5,229,"MossBeetle"); Slice(BatPath,6,6,209,"ShadeBat");
            ItemDefinition shell=Item("MossBeetleShell","material_moss_beetle_shell","이끼갑충의 등껍질","이끼갑충의 단단한 등껍질.",6);
            ItemDefinition wing=Item("ShadeBatWing","material_shade_bat_wing","그늘박쥐의 날개막","그늘박쥐의 얇고 질긴 날개막.",7);
            MonsterDefinition spider=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").FirstOrDefault(x=>x.MonsterId=="forest_spider");
            MonsterDefinition beetle=Monster("05_MossBeetle","monster_moss_beetle","이끼갑충",5,24,7,115,170,7,.48f,new Vector2(1.12f,.86f),.78f,BeetlePath,6,5,spider,"material_moss_beetle_shell",.4f);
            MonsterDefinition bat=Monster("06_ShadeBat","monster_shade_bat","그늘박쥐",6,28,8,90,190,15,1.05f,new Vector2(.95f,.82f),.72f,BatPath,6,6,beetle,"material_shade_bat_wing",.35f);
            CreateFieldAssets(beetle,bat,spider); CreateSceneAndBuildEntry();
            MainQuestContentEditor.Create(); AssetDatabase.SaveAssets(); AssetDatabase.Refresh();
            Debug.Log("Field 03·Main 08와 이끼갑충·그늘박쥐 콘텐츠 생성을 완료했습니다.");
        }

        static void Slice(string path,int columns,int rows,int size,string prefix)
        {
            AssetDatabase.ImportAsset(path,ImportAssetOptions.ForceUpdate); var importer=(TextureImporter)AssetImporter.GetAtPath(path);
            importer.textureType=TextureImporterType.Sprite; importer.spriteImportMode=SpriteImportMode.Multiple; importer.spritePixelsPerUnit=size;
            importer.filterMode=FilterMode.Point; importer.wrapMode=TextureWrapMode.Clamp; importer.mipmapEnabled=false; importer.alphaIsTransparency=true;
            importer.sRGBTexture=true; importer.textureCompression=TextureImporterCompression.Uncompressed; importer.maxTextureSize=2048; importer.SaveAndReimport();
            var factory=new SpriteDataProviderFactories(); factory.Init(); var provider=factory.GetSpriteEditorDataProviderFromObject(importer); provider.InitSpriteEditorDataProvider();
            var rects=new SpriteRect[columns*rows];
            for(int top=0;top<rows;top++) for(int col=0;col<columns;col++) { int i=top*columns+col; rects[i]=new SpriteRect{
                name=$"{prefix}_R{top}_F{col:00}", rect=new Rect(col*size,(rows-1-top)*size,size,size), alignment=SpriteAlignment.BottomCenter,
                pivot=new Vector2(.5f,0f), spriteID=GUID.Generate()}; }
            provider.SetSpriteRects(rects); provider.Apply(); importer.SaveAndReimport();
        }

        static MonsterDefinition Monster(string asset,string id,string name,int level,int exp,int talent,int hp,int attackPercent,int agility,float speed,Vector2 visual,float scale,string path,int columns,int rows,MonsterDefinition support,string item,float chance)
        {
            string target=$"{ResourceRoot}/MonsterDefinitions/{asset}.asset"; var d=AssetDatabase.LoadAssetAtPath<MonsterDefinition>(target);
            if(d==null){d=ScriptableObject.CreateInstance<MonsterDefinition>();AssetDatabase.CreateAsset(d,target);} var all=AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().OrderBy(s=>s.name).ToArray();
            Sprite[] Row(int r,int count=6)=>all.Where(s=>s.name.Contains($"_R{r}_")).Take(count).ToArray();
            int hitRow=rows==5?3:4, defeatRow=rows-1; var loot=new MonsterLootEntry();loot.Configure(item,chance);
            d.ConfigureContent(id,name,level,exp,talent,hp,attackPercent,agility,speed,visual,scale,Row(0)[0],Row(0),Row(1),Row(2),Row(hitRow,rows==5?4:6),Row(defeatRow),support,new[]{loot});EditorUtility.SetDirty(d);return d;
        }

        static ItemDefinition Item(string asset,string id,string name,string description,int sell)
        { string path=$"{ResourceRoot}/ItemDefinitions/{asset}.asset";var d=AssetDatabase.LoadAssetAtPath<ItemDefinition>(path);if(d==null){d=ScriptableObject.CreateInstance<ItemDefinition>();AssetDatabase.CreateAsset(d,path);}d.ConfigureContent(id,name,description,ItemCategory.Material,99,0,sell,null,Color.white,ItemUseType.None,"판매 재료");d.ConfigureEffect(ItemEffectType.None,0);EditorUtility.SetDirty(d);return d; }

        static void CreateFieldAssets(MonsterDefinition beetle,MonsterDefinition bat,MonsterDefinition spider)
        {
            MonsterDefinition snake=Resources.LoadAll<MonsterDefinition>("MonsterDefinitions").FirstOrDefault(x=>x.MonsterId=="venom_snake");
            Spawn("Field03_EntranceSpider","field03_entrance_spider",spider,new Vector2(-5.5f,3.2f),1.1f); Spawn("Field03_EntranceSnake","field03_entrance_snake",snake,new Vector2(5.2f,2.4f),1.1f); Spawn("Field03_EntranceBeetle","field03_entrance_beetle",beetle,new Vector2(-2.8f,1.1f),1f);
            Spawn("Field03_MiddleBeetle","field03_middle_beetle",beetle,new Vector2(3.8f,-1.7f),1f); Spawn("Field03_MiddleBat","field03_middle_bat",bat,new Vector2(-3.4f,-2.6f),1.35f); Spawn("Field03_DeepBat","field03_deep_bat",bat,new Vector2(5.8f,-4.4f),.7f);
            Connection("Field02_SouthToField03","Field_02","south_to_field03","Spawn_From_Field03",new Vector2(0,-5.2f),new Vector2(0,-7.15f),"Field_03","Spawn_From_Field02",false);
            Connection("Field03_NorthToField02","Field_03","north_to_field02","Spawn_From_Field02",new Vector2(0,4.7f),new Vector2(0,6.85f),"Field_02","Spawn_From_Field03",true);
            string themePath=$"{ResourceRoot}/FieldThemes/Field03_SilentForest.asset";var theme=AssetDatabase.LoadAssetAtPath<FieldThemeDefinition>(themePath);if(theme==null){theme=ScriptableObject.CreateInstance<FieldThemeDefinition>();AssetDatabase.CreateAsset(theme,themePath);}theme.Configure("Field_03",new Color(.46f,.61f,.46f,1),new Color(.025f,.045f,.04f,1),"TreeWest","BushWest",
                new[]{new Vector2(-8,4),new Vector2(8,3.7f),new Vector2(-7,1),new Vector2(7,.5f),new Vector2(-6,-2),new Vector2(6,-3),new Vector2(-4,-5),new Vector2(4,-5.4f)},new[]{new Vector2(-5,3),new Vector2(5,2),new Vector2(-4,0),new Vector2(4,-1),new Vector2(-3,-3.8f)});EditorUtility.SetDirty(theme);
        }
        static void Spawn(string asset,string id,MonsterDefinition monster,Vector2 pos,float radius){string p=$"{ResourceRoot}/MonsterSpawns/{asset}.asset";var d=AssetDatabase.LoadAssetAtPath<FieldMonsterSpawnDefinition>(p);if(d==null){d=ScriptableObject.CreateInstance<FieldMonsterSpawnDefinition>();AssetDatabase.CreateAsset(d,p);}d.Configure("Field_03",id,monster,pos,radius,35);EditorUtility.SetDirty(d);}
        static void Connection(string asset,string scene,string id,string spawnId,Vector2 spawn,Vector2 exit,string target,string targetSpawn,bool replace){string p=$"{ResourceRoot}/FieldConnections/{asset}.asset";var d=AssetDatabase.LoadAssetAtPath<FieldConnectionDefinition>(p);if(d==null){d=ScriptableObject.CreateInstance<FieldConnectionDefinition>();AssetDatabase.CreateAsset(d,p);}d.Configure(scene,id,spawnId,spawn,exit,new Vector2(3,1),target,targetSpawn,replace);EditorUtility.SetDirty(d);}
        static void CreateSceneAndBuildEntry(){const string src="Assets/_Project/Scenes/Field_02.unity",dst="Assets/_Project/Scenes/Field_03.unity";if(!System.IO.File.Exists(dst))AssetDatabase.CopyAsset(src,dst);var scenes=EditorBuildSettings.scenes.ToList();if(scenes.All(s=>s.path!=dst)){scenes.Add(new EditorBuildSettingsScene(dst,true));EditorBuildSettings.scenes=scenes.ToArray();}}
    }
}
#endif
