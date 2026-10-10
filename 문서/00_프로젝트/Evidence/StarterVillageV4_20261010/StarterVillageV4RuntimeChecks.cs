using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectLimitless.Core;
using ProjectLimitless.NPC;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    // 격리 복사본에서만 기존 회귀 Audit에 추가하는 마을 Sprite/기능 검사입니다. 원본 Editor에서는 호출하지 않습니다.
    public static class StarterVillageV4RuntimeChecks
    {
        private static string Root => Path.GetFullPath(Path.Combine(Application.dataPath, "../V4AuditResults"));
        private static void Check(bool ok, string label)
        {
            Directory.CreateDirectory(Root);
            File.AppendAllText(Path.Combine(Root, "checks.txt"), (ok ? "PASS|" : "FAIL|") + label + "\n");
            if (!ok) throw new InvalidOperationException(label);
        }
        private static bool Env(SpriteRenderer r) => r.sprite != null && AssetDatabase.GetAssetPath(r.sprite).StartsWith("Assets/_Project/Art/Environment/StarterVillage/V4/");
        public static void Validate()
        {
            Check(Application.isBatchMode && Application.dataPath.Replace('\\','/').Contains("/Temp/SVRenewalV4QA/Client/Assets"), "IsolationGuard");
            var roots = SceneManager.GetActiveScene().GetRootGameObjects();
            var renderers = roots.SelectMany(g => g.GetComponentsInChildren<SpriteRenderer>(true)).ToArray();
            var env = renderers.Where(Env).ToArray();
            Check(env.Length == 302 && env.Count(r => r.gameObject.activeInHierarchy) == 300, "Environment.Active300.Inactive2");
            Check(env.Select(r => AssetDatabase.GetAssetPath(r.sprite)).Distinct().Count() == 15, "Environment.Unique15");
            Check(renderers.All(r => r.sprite != null), "MissingSprite0");
            Check(roots.Sum(g => g.GetComponentsInChildren<Transform>(true).Sum(t => GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject))) == 0, "MissingScript0");
            var roles = Object.FindObjectsByType<VillageNpcRole>(FindObjectsSortMode.None).Where(n => n.NpcId.StartsWith("starter-village-")).ToArray();
            string[] ids = { "main-guide", "general-shop", "equipment-shop", "healer", "bank", "party-manager", "training-guide", "resident-01", "resident-02", "resident-03", "resident-04", "gate-guard" };
            foreach (var id in ids)
            {
                var npc = roles.Single(r => r.NpcId == "starter-village-" + id);
                Check(npc.GetComponent<NpcController>() != null && npc.GetComponent<NpcQuestMarkerPresenter>() != null && npc.GetComponentsInChildren<TextMesh>(true).Length > 0, "Npc.StableId.Name.Marker." + id);
                Check(!Physics2D.OverlapPointAll((Vector2)npc.transform.position + new Vector2(.7f,0)).Any(c => !c.isTrigger && c.GetComponentInParent<PlayerController>() == null && c.GetComponentInParent<NpcController>() == null), "Npc.AdjacentAccess." + id);
            }
            Check(Object.FindAnyObjectByType<WorldBounds2D>().Bounds.size == new Vector3(19,13,0), "WorldBounds19x13");
            Check(!Physics2D.OverlapPointAll(new Vector2(0,-6.05f)).Any(c => !c.isTrigger && c.GetComponentInParent<PlayerController>() == null), "SouthGate.CentralColliderOpening");
            Check(Physics2D.OverlapPointAll(new Vector2(-2,-6.05f)).Any(c => !c.isTrigger), "SouthGate.FenceCollider");
            foreach (var r in env) Check(Mathf.Approximately(r.sprite.pixelsPerUnit,128) && r.transform.lossyScale == Vector3.one, "Sprite.PPU.Scale." + r.name);
            // 실제 Scene 렌더러를 RenderTexture로 촬영합니다. QA 카메라 변경은 격리 프로젝트 안에서 즉시 복구합니다.
            Capture("village_runtime_overview.png", new Vector3(0,0,-10),6.5f,1520,1040);
            Capture("village_runtime_house.png", new Vector3(-4.5f,4,-10),1.5f,768,768);
            Capture("village_runtime_south_gate.png", new Vector3(0,-4.75f,-10),2f,960,540);
            Check(File.Exists(Path.Combine(Root,"village_runtime_overview.png")), "RenderTexture.Captured");
        }
        private static void Capture(string name, Vector3 position, float size, int width, int height)
        {
            if (SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null) { File.AppendAllText(Path.Combine(Root,"checks.txt"),"NOT_VERIFIED|RenderTexture.NullGraphics\n"); return; }
            Camera camera=Camera.main; var previousTarget=camera.targetTexture; var previousPosition=camera.transform.position; float previousSize=camera.orthographicSize; float previousAspect=camera.aspect;
            var rt=RenderTexture.GetTemporary(width,height,24,RenderTextureFormat.ARGB32); var previousActive=RenderTexture.active;
            try {
                camera.transform.position=position; camera.orthographicSize=size; camera.aspect=(float)width/height; camera.targetTexture=rt; camera.Render(); RenderTexture.active=rt;
                var image=new Texture2D(width,height,TextureFormat.RGBA32,false); image.ReadPixels(new Rect(0,0,width,height),0,0); image.Apply(); File.WriteAllBytes(Path.Combine(Root,name),image.EncodeToPNG()); Object.DestroyImmediate(image);
            } finally { camera.targetTexture=previousTarget;camera.transform.position=previousPosition;camera.orthographicSize=previousSize;camera.aspect=previousAspect;RenderTexture.active=previousActive;RenderTexture.ReleaseTemporary(rt); }
        }
        private static IEnumerator WaitScene(string expected)
        {
            double until=EditorApplication.timeSinceStartup+30;
            while(SceneManager.GetActiveScene().name!=expected && EditorApplication.timeSinceStartup<until) yield return null;
            Check(SceneManager.GetActiveScene().name==expected,"Gate.Scene."+expected);
            double settled=EditorApplication.timeSinceStartup+1;while(EditorApplication.timeSinceStartup<settled)yield return null;
            ProjectLimitless.Monster.MonsterEncounterService.SuppressForSeconds(3600);
        }
        public static IEnumerator GateRoundTrip()
        {
            var player=Object.FindAnyObjectByType<PlayerController>();
            Check(Vector2.Distance(player.transform.position,new Vector2(0,-5f))<.1f,"InitialSpawn.Preserved");
            var exit=GameObject.Find("Exit_To_Field01").GetComponent<SceneTransitionTrigger>();
            exit.SendMessage("OnTriggerEnter2D",player.GetComponent<Collider2D>(),SendMessageOptions.RequireReceiver);
            yield return WaitScene("Field_01");
            player=Object.FindAnyObjectByType<PlayerController>();Check(Vector2.Distance(player.transform.position,new Vector2(0,4.7f))<.1f,"Gate.FieldSpawn");
            var back=Object.FindObjectsByType<SceneTransitionTrigger>(FindObjectsSortMode.None).Single(t=>t.name=="Entrance_To_StarterVillage");
            back.SendMessage("OnTriggerEnter2D",player.GetComponent<Collider2D>(),SendMessageOptions.RequireReceiver);
            yield return WaitScene("World_StarterVillage");
            player=Object.FindAnyObjectByType<PlayerController>();Check(Vector2.Distance(player.transform.position,new Vector2(0,-4.65f))<.1f,"Gate.ReturnSpawn");
            Check(GameSaveService.SaveCurrentWorldPosition(player.transform.position,"World_StarterVillage","Spawn_From_Field01"),"Gate.SavePosition");
        }
    }
}
