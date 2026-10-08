using System;
using System.IO;
using System.Linq;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace ProjectLimitless.EditorTools
{
    /// <summary>감사한 Main19 원본13을 등록하고 Field10 환경만 교체합니다. 기존 Quest·배치 도구 전체를 재실행하지 않습니다.</summary>
    public static class Main19ArtContentBuilder
    {
        private const string Art = "Assets/_Project/Resources/Main19/";
        public static string Apply()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode || EditorSceneManager.GetActiveScene().isDirty)
                throw new InvalidOperationException("저장된 Edit Mode에서만 Art를 반영합니다.");
            string source = Path.GetFullPath(Path.Combine(Application.dataPath, "../../../Temp/Main19Art/Source"));
            foreach (string folder in new[] { "Environment", "Background", "VFX" })
            {
                Directory.CreateDirectory(Art + folder);
                foreach (string png in Directory.GetFiles(Path.Combine(source, folder), "*.png"))
                {
                    string path = Art + folder + "/" + Path.GetFileName(png);
                    // 재실행 시 사용자 변경본을 덮지 않고 충돌을 보고합니다.
                    if (File.Exists(path) && !File.ReadAllBytes(path).SequenceEqual(File.ReadAllBytes(png)))
                        throw new InvalidOperationException("원본과 기존 등록본이 다릅니다: " + path);
                    if (!File.Exists(path)) File.Copy(png, path);
                    AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport);
                    var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    importer.spritePixelsPerUnit = 128; importer.filterMode = FilterMode.Point;
                    importer.textureCompression = TextureImporterCompression.Uncompressed;
                    importer.maxTextureSize = 2048; importer.npotScale = TextureImporterNPOTScale.None;
                    importer.alphaIsTransparency = true; importer.mipmapEnabled = false;
                    importer.wrapMode = TextureWrapMode.Clamp;
                    var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
                    settings.spriteMeshType = SpriteMeshType.FullRect; importer.SetTextureSettings(settings);
                    importer.SaveAndReimport();
                }
            }
            var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene(); var selected = Selection.objects;
            var scene = EditorSceneManager.OpenScene(Main19ContentBuilder.ScenePath, OpenSceneMode.Additive);
            try
            {
                var env = scene.GetRootGameObjects().Single(x => x.name == "FieldEnvironment").transform;
                // Field10 환경과 연출만 교체합니다. Player/Camera/Bounds와 Runtime Quest Target 좌표는 그대로입니다.
                foreach (Transform child in env.Cast<Transform>().ToArray()) UnityEngine.Object.DestroyImmediate(child.gameObject);
                var previous = scene.GetRootGameObjects().SingleOrDefault(x => x.name == "Main19FieldArt");
                if (previous != null) UnityEngine.Object.DestroyImmediate(previous);
                Visual(env, "Ground", "Environment/BurningPulse_Ground_Base", Vector2.zero, new Vector2(21,15), -20, 1, true);
                Visual(env, "EntryAsh", "../Main18/Environment/ObsidianScar_AshPatch_01", new Vector2(7,-3), new Vector2(2,2), -19, .35f);
                Visual(env, "Track1", "Environment/BurningPulse_GiantTrack_01", Chapter2Main19Flow.Positions[2], new Vector2(3.2f,2.2f), -16, .8f);
                Visual(env, "MeltedCliff", "Environment/BurningPulse_MeltedCliff_01", new Vector2(3.4f,-3.5f), new Vector2(3.2f,2.4f), -8, 1);
                Visual(env, "PulseNode", "Environment/BurningPulse_PulseCrack_01", Chapter2Main19Flow.Positions[5], new Vector2(3,3), -18, .4f);
                Visual(env, "PulseCorridor", "Environment/BurningPulse_PulseCrack_02", new Vector2(-2.5f,.5f), new Vector2(3,2.5f), -18, .35f);
                Visual(env, "DragScar", "Environment/BurningPulse_DragScar_01", new Vector2(-3,-2.7f), new Vector2(3.6f,1.8f), -17, .65f);
                Visual(env, "HeatVent", "Environment/BurningPulse_HeatVent_01", new Vector2(-3.2f,3.5f), new Vector2(2,2), -8, .7f);
                Visual(env, "MassiveTrack", "Environment/BurningPulse_GiantTrack_02", Chapter2Main19Flow.Positions[8], new Vector2(4,2.8f), -16, .85f);
                Visual(env, "Ridge", "Environment/BurningPulse_Ridge_01", new Vector2(-7.6f,2.15f), new Vector2(3.6f,1.65f), -8, 1);
                Visual(env, "DeepCliff", "Environment/BurningPulse_DeepCliff_Blocker_01", new Vector2(-9.65f,-3), new Vector2(1.6f,3.5f), -8, 1);
                var silhouette = Visual(env, "DistantSilhouette", "Background/BurningPulse_DistantFlameSilhouette", new Vector2(-8.1f,3.4f), new Vector2(4.6f,2.3f), -11, 1);
                var ground = Visual(env, "GroundPulse", "VFX/BurningPulse_GroundPulse_VFX", Chapter2Main19Flow.Positions[5], new Vector2(3,3), -15, 0);
                var flare = Visual(env, "DistantFlare", "VFX/BurningPulse_DistantFlare_VFX", new Vector2(-8.1f,3.4f), new Vector2(3.2f,2.4f), -12, 0);
                var root = new GameObject("Main19FieldArt", typeof(Main19FieldArtPresentation));
                UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(root, scene);
                root.GetComponent<Main19FieldArtPresentation>().Configure(silhouette, ground, flare);
                if (!EditorSceneManager.SaveScene(scene, Main19ContentBuilder.ScenePath)) throw new InvalidOperationException("Field10 Art 저장 실패");
            }
            finally
            {
                EditorSceneManager.CloseScene(scene, true);
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(active); Selection.objects = selected;
            }
            AssetDatabase.SaveAssets(); return "Main19 PNG13 imported; Field10 environment and gated witness applied";
        }
        private static SpriteRenderer Visual(Transform parent, string name, string asset, Vector2 at, Vector2 size, int order, float alpha, bool tiled = false)
        {
            string path = asset.StartsWith("../") ? "Assets/_Project/Resources/" + asset.Substring(3) : Art + asset;
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path + ".png");
            if (sprite == null) throw new InvalidOperationException("Sprite 누락: " + path);
            var go = new GameObject(name, typeof(SpriteRenderer)); go.transform.SetParent(parent, false); go.transform.position = at;
            var renderer = go.GetComponent<SpriteRenderer>(); renderer.sprite = sprite; renderer.sortingOrder = order;
            renderer.color = new Color(1,1,1,alpha);
            if (tiled) { renderer.drawMode = SpriteDrawMode.Tiled; renderer.size = size; }
            else go.transform.localScale = new Vector3(size.x/sprite.bounds.size.x, size.y/sprite.bounds.size.y, 1);
            return renderer;
        }
    }
}
