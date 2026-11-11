#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using ProjectLimitless.NPC;
using ProjectLimitless.Battle;

// 기존107PASS를 반복하지 않고 원본 이식과 같은 자산의 Import·연결만 확인합니다.
public static class ArtApply9591Preflight
{
    public static void Run()
    {
        if(!Application.isBatchMode||Application.unityVersion!="6000.6.5f1"||!Application.dataPath.Replace('\\','/').EndsWith("/Temp/Art95_91Unity20261111/QA/Client/Assets"))throw new Exception("격리 Import 검사 전용");
        var results=new List<string>();
        Action<bool,string> check=(ok,name)=>{results.Add((ok?"PASS ":"FAIL ")+name);if(!ok)throw new Exception(name);};
        var a=Resources.Load<Sprite>("VillageNpcSprites/Eldiran/OGA06_Healer_Original_Approved");
        var b=Resources.Load<Sprite>("VillageNpcSprites/Eldiran/OGA16_Bank_Original_Approved");
        check(a!=null&&a.rect.size==new Vector2(512,512)&&a.pixelsPerUnit==329&&Vector2.Distance(a.pivot,new Vector2(261.5f,42))<.001f,"OGA06.ImportPPUPivot");
        check(b!=null&&b.rect.size==new Vector2(512,512)&&b.pixelsPerUnit==320.25f&&Vector2.Distance(b.pivot,new Vector2(269,43))<.001f,"OGA16.ImportPPUPivot");
        var map=(IReadOnlyDictionary<string,string>)typeof(VillageNpcAppearanceCatalog).GetField("ResourcePaths",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null);
        check(new[]{"starter-village-healer","safezone-catacomb-healer","arbel-healer"}.All(id=>Resources.Load<Sprite>(map[id])==a)&&new[]{"starter-village-bank","arbel-pet-management"}.All(id=>Resources.Load<Sprite>(map[id])==b),"NpcStableId5.ResourceLinks");
        var cat=Resources.Load<BattleBackgroundCatalog>("BattleBackgrounds/Chapter3");
        check(cat!=null&&cat.Find("World_NorthF1","").name=="Chapter3_Snowroad_v2_Approved"&&cat.Find("World_NorthF2_CANDIDATE","").name=="Chapter3_Forest_F2_v2_Approved","Chapter3.SceneKeysAndSprites");
        check(Resources.Load<BattleBackgroundCatalog>("BattleBackgrounds/Chapter1").Find("Field_01","")!=null&&Resources.Load<BattleBackgroundCatalog>("BattleBackgrounds/Chapter2").Find("Field_05_WesternOutskirts","")!=null,"Chapter1And2.ImportReferences");
        check(AppDomain.CurrentDomain.GetAssemblies().Any(x=>x.GetType("F2SnowPathBuilder")!=null),"B352.TypePreserved");
        check(!EditorApplication.isPlaying&&!EditorApplication.isCompiling,"EditMode.CompiledWithoutPlay");
        File.WriteAllLines("F:/study/codex/Project-Limitless/Temp/Art95_91Apply20261111/isolated-import-checks.txt",results);
        Debug.Log("Art9591Apply minimal Import: "+results.Count+" PASS");
    }
}
#endif
