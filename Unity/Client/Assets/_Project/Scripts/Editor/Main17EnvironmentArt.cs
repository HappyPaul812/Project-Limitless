using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace ProjectLimitless.EditorTools
{
    /// <summary>외부 PNG를 그대로 가져와 Field08 시각 참조만 교체합니다. 충돌과 퀘스트 오브젝트는 유지합니다.</summary>
    public static class Main17EnvironmentArt
    {
        const string Root = "Assets/_Project/Art/Environment/Chapter2/RedRift/";
        // 원본 전체 UV를 유지한 채 대각선 그림의 중심을 기존 직선 Collider에 맞춥니다. PNG를 자르지 않습니다.
        static Mesh RiftMesh()
        {
            string path = Root + "RiftSurface.asset";
            var mesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);
            if (mesh == null) { mesh = new Mesh(); AssetDatabase.CreateAsset(mesh,path); }
            const int count = 32;
            var vertices = new Vector3[1089]; var uv = new Vector2[1089]; var colors = new Color[1089]; var indices = new int[6144];
            for (int row=0; row<=count; row++)
            {
                float v=row/(float)count, center=.22f+.68f*(1-v);
                for (int col=0; col<=count; col++)
                {
                    float u=col/(float)count, worldX=(u-center)*3f;
                    int n=row*33+col; vertices[n]=new Vector3(worldX/.7f,v-.5f,0); uv[n]=new Vector2(u,v);
                    // 원본 불투명 Terrain의 직사각 외곽만 Scene vertex alpha로 바닥에 섞습니다.
                    // 깊은 틈 Collider 내부는 불투명하게 유지하고 바깥 절벽 그림은 부드럽게 연결합니다.
                    float alpha=1-Mathf.InverseLerp(.35f,.85f,Mathf.Abs(worldX));
                    colors[n]=new Color(1,1,1,alpha);
                    if(row<count&&col<count){int t=(row*32+col)*6;indices[t]=n;indices[t+1]=n+33;indices[t+2]=n+1;indices[t+3]=n+1;indices[t+4]=n+33;indices[t+5]=n+34;}
                }
            }
            mesh.Clear(); mesh.vertices=vertices; mesh.uv=uv; mesh.colors=colors; mesh.triangles=indices; mesh.RecalculateNormals(); mesh.RecalculateBounds(); EditorUtility.SetDirty(mesh); return mesh;
        }
        static Material Mat(string name, string image, Color color)
        {
            string path = Root + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null) { material = new Material(Shader.Find("Sprites/Default")); AssetDatabase.CreateAsset(material, path); }
            material.mainTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(Root + image + ".png");
            material.color = color; EditorUtility.SetDirty(material); return material;
        }
        public static string Apply()
        {
            foreach (string path in AssetDatabase.FindAssets("t:Texture2D", new[] {Root.TrimEnd('/')} ).Select(AssetDatabase.GUIDToAssetPath))
            {
                var importer = (TextureImporter)AssetImporter.GetAtPath(path);
                importer.textureType = TextureImporterType.Sprite; importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 128; importer.filterMode = FilterMode.Point;
                importer.textureCompression = TextureImporterCompression.Uncompressed; importer.maxTextureSize = 2048;
                importer.npotScale = TextureImporterNPOTScale.None; importer.alphaIsTransparency = true;
                var settings = new TextureImporterSettings(); importer.ReadTextureSettings(settings);
                settings.spriteMeshType = SpriteMeshType.FullRect; settings.spritePivot = new Vector2(.5f,.5f);
                importer.SetTextureSettings(settings);
                importer.wrapMode = path.Contains("Ground") ? TextureWrapMode.Mirror : TextureWrapMode.Clamp;
                importer.SaveAndReimport();
            }
            var active = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var scene = EditorSceneManager.OpenScene("Assets/_Project/Scenes/Field_08_RedRift.unity", OpenSceneMode.Additive);
            try
            {
                var environment = scene.GetRootGameObjects().Single(x => x.name == "FieldEnvironment");
                var ground = Mat("Ground", "RedRift_Ground_Base", new Color(.55f,.55f,.55f,1));
                var ash = Mat("Ash", "RedRift_AshPatch_01", Color.white);
                var glow = Mat("Glow", "RedRift_Fissure_Glow", Color.white);
                var rock = Mat("Obsidian", "RedRift_Obsidian_01", Color.white);
                var fissure = Mat("Fissure", "RedRift_Fissure_Edge", new Color(.55f,.55f,.55f,1));
                foreach (var renderer in environment.GetComponentsInChildren<MeshRenderer>(true))
                {
                    string name = renderer.name;
                    if (name == "RedRiftGeometry_Ground") renderer.sharedMaterial = ground;
                    else if (name == "RedRiftGeometry_DeepCrack") { renderer.sharedMaterial = fissure; renderer.GetComponent<MeshFilter>().sharedMesh=RiftMesh(); }
                    else if (name.Contains("Glow")) renderer.sharedMaterial = glow;
                    else if (name.Contains("Obsidian")) renderer.sharedMaterial = rock;
                    else if (name.Contains("Ash")) { renderer.sharedMaterial = ash; renderer.transform.localScale = new Vector3(.5f,.5f,1); }
                    else renderer.enabled = false; // 과거 단색 선/발자국은 정식 지면의 균열 표현으로 대체합니다.
                }
                foreach (var renderer in environment.GetComponentsInChildren<SpriteRenderer>(true))
                {
                    if (renderer.name.StartsWith("AshCrack")) { renderer.enabled = false; continue; }
                    string image = renderer.name.StartsWith("Rock") ? "RedRift_Obsidian_01" : renderer.name.StartsWith("CanyonLip") ? "RedRift_Canyon_Edge_01" : null;
                    if (image == null) continue;
                    Vector2 size = renderer.name == "RockNorth" ? new Vector2(.85f,.54f) : Vector2.one;
                    renderer.sprite = AssetDatabase.LoadAssetAtPath<Sprite>(Root + image + ".png");
                    // Transform과 Collider를 바꾸지 않고 렌더링 크기만 기존 슬롯에 맞춥니다.
                    renderer.drawMode = SpriteDrawMode.Sliced; renderer.size = size;
                    renderer.transform.localScale = Vector3.one; // Sprite 참조 교체 시 Editor의 자동 크기 보정을 충돌 root에 남기지 않습니다.
                    renderer.color = Color.white;
                }
                for (int i=0;i<3;i++)
                {
                    string name = "RedRiftArt_DeadShrub"+i;
                    var existing = environment.transform.Find(name);
                    var shrub = existing != null ? existing.gameObject : new GameObject(name,typeof(SpriteRenderer));
                    shrub.transform.SetParent(environment.transform,false);
                    shrub.transform.localPosition = new Vector3(i==0 ? 0 : i==1 ? 8 : -8, i==2 ? -5 : 5,0);
                    shrub.transform.localScale = Vector3.one*.12f;
                    var renderer = shrub.GetComponent<SpriteRenderer>(); renderer.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(Root+"RedRift_DeadShrub_01.png"); renderer.sortingOrder=2;
                }
                AssetDatabase.SaveAssets(); EditorSceneManager.SaveScene(scene);
                return "Field08 source art assigned; colliders/transitions unchanged";
            }
            finally { EditorSceneManager.CloseScene(scene,true); UnityEngine.SceneManagement.SceneManager.SetActiveScene(active); }
        }
    }
}
