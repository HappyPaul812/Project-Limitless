using System;
using System.IO;
using System.Linq;
using ProjectLimitless.CameraSystem;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>#83 전용 QA 복제 프로젝트에서만 새 Scene을 만듭니다. 원본 Scene·BuildSettings는 저장하지 않습니다.</summary>
    public static class Main24F2ContentBuilder
    {
        public const string AssetsPath = "Assets/_Project/Resources/Main24F2Candidate/";
        public const string StumpFile = "F2_SnowStumpPerch_NaturalBroken_v4_CANDIDATE_320x384_RGBA.png";
        public const string ScenePath = "Assets/_Project/Scenes/World_NorthF2_CANDIDATE.unity";
        public static void Guard()
        {
            if (!Application.isBatchMode || !Application.dataPath.Replace('\\', '/').EndsWith("/Temp/Main24F2FirstTraversal20261103/QA/Client/Assets"))
                throw new Exception("#83 전용 격리 QA에서만 실행합니다.");
        }

        public static void Build()
        {
            Guard();
            // 기존 마을의 실제 Player 구성과 Camera만 인스턴스로 재사용합니다. Scene/GUID 복제는 하지 않습니다.
            var village = EditorSceneManager.OpenScene("Assets/_Project/Scenes/World_FrostwayVillage.unity", OpenSceneMode.Single);
            var playerRoot = Object.FindAnyObjectByType<PlayerController>().transform.root.gameObject;
            var cameraRoot = Camera.main.transform.root.gameObject;
            var playerCopy = Object.Instantiate(playerRoot); playerCopy.name = playerRoot.name;
            var cameraCopy = Object.Instantiate(cameraRoot); cameraCopy.name = cameraRoot.name;
            var forest = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(playerCopy, forest);
            UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(cameraCopy, forest);
            EditorSceneManager.CloseScene(village, true);
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(forest);
            var player = playerCopy.GetComponentInChildren<PlayerController>();
            player.transform.position = new Vector2(0, -8);
            player.GetComponent<Rigidbody2D>().position = player.transform.position;
            var camera = cameraCopy.GetComponentInChildren<Camera>();
            camera.transform.position = new Vector3(0, -6, -10);
            camera.orthographicSize = 5;
            camera.backgroundColor = new Color(.62f, .69f, .73f);
            camera.GetComponent<CameraFollow>().SetTarget(player.transform);

            var bounds = new GameObject("F2_CANDIDATE_WorldBounds", typeof(WorldBounds2D)).GetComponent<WorldBounds2D>();
            bounds.Configure(Vector2.zero, new Vector2(32, 24));
            // 출입은 기존처럼 표식에서 명시 선택합니다. 경계 밖 탈출이나 Trigger 왕복 루프를 만들지 않습니다.
            WorldBounds2D.CreateBoundaryColliders(bounds.transform, bounds.Bounds, .4f);
            var spawn = new GameObject(Main24F2Traversal.EntrySpawn, typeof(SceneSpawnPoint));
            spawn.transform.position = new Vector2(0, -8);
            spawn.GetComponent<SceneSpawnPoint>().Configure(Main24F2Traversal.EntrySpawn);

            var snow = Texture("PLACEHOLDER_Snow_ValidationOnly", 32, 32, (x,y) =>
                (x*13+y*7)%19 < 4 ? new Color(.71f,.77f,.79f) : new Color(.81f,.85f,.85f));
            var dirt = Texture("PLACEHOLDER_FirmTrail_ValidationOnly", 32, 32, (x,y) =>
                (x+y*7)%13 < 3 ? new Color(.53f,.49f,.40f) : new Color(.65f,.60f,.49f));
            var tree = Texture("PLACEHOLDER_Tree_ValidationOnly", 32, 48, (x,y) => {
                if (y < 9) return x>=14&&x<=17 ? new Color(.29f,.25f,.21f) : Color.clear;
                float width = y < 23 ? (31-y)*.85f : (48-y)*.5f;
                return Mathf.Abs(x-15.5f)<width ? ((x+y)%8<2 ? new Color(.53f,.65f,.65f) : new Color(.25f,.39f,.39f)) : Color.clear;
            });
            Visual("PLACEHOLDER_SnowGround_ValidationOnly",snow,Vector2.zero,new Vector2(32,24),-20);
            Visual("PLACEHOLDER_FirmTrail_South",dirt,new Vector2(0,-6),new Vector2(4.5f,12),-19);
            Visual("PLACEHOLDER_FirmTrail_Bend",dirt,new Vector2(1.5f,0),new Vector2(7.5f,4.5f),-19);
            Visual("PLACEHOLDER_FirmTrail_North",dirt,new Vector2(3,4),new Vector2(4.5f,8),-19);
            foreach (var point in new[] { new Vector2(-7,-7),new Vector2(7,-6),new Vector2(-5,-2),new Vector2(8,1),new Vector2(-4,6),new Vector2(8,7),new Vector2(-11,2),new Vector2(12,-3) }) {
                var prop=Visual("PLACEHOLDER_Tree_ValidationOnly",tree,point,new Vector2(2,3),2);
                var box=prop.AddComponent<BoxCollider2D>();box.size=new Vector2(.45f,.2f);box.offset=new Vector2(0,-.4f);
            }

            Import(AssetsPath+StumpFile,128,new Vector2(.5f,11f/384));
            var stump=new GameObject("USER_APPROVED_Stump_v4",typeof(SpriteRenderer),typeof(BoxCollider2D),typeof(Main24F2StumpDepth));
            stump.transform.position=new Vector2(-.3f,1);
            var view=stump.GetComponent<SpriteRenderer>();view.sprite=AssetDatabase.LoadAssetAtPath<Sprite>(AssetsPath+StumpFile);view.spriteSortPoint=SpriteSortPoint.Pivot;
            var trunk=stump.GetComponent<BoxCollider2D>();trunk.size=new Vector2(1.65f,.4f);trunk.offset=new Vector2(0,.2f);
            EditorSceneManager.SaveScene(forest,ScenePath);
            // BuildSettings 연결은 QA 복제본에만 추가합니다. 후보 인계 때 이 파일을 제품에 복사하지 않습니다.
            var scenes=EditorBuildSettings.scenes.ToList();
            if (!scenes.Any(x=>x.path==ScenePath)) scenes.Add(new EditorBuildSettingsScene(ScenePath,true));
            EditorBuildSettings.scenes=scenes.ToArray();AssetDatabase.SaveAssets();
            Debug.Log("MAIN24_F2_CANDIDATE_BUILD_PASS");
        }
        static Sprite Texture(string name,int width,int height,Func<int,int,Color> color)
        {
            var texture=new Texture2D(width,height,TextureFormat.RGBA32,false);
            for(int y=0;y<height;y++)for(int x=0;x<width;x++)texture.SetPixel(x,y,color(x,y));
            texture.Apply();string path=AssetsPath+name+".png";File.WriteAllBytes(path,texture.EncodeToPNG());Object.DestroyImmediate(texture);
            AssetDatabase.ImportAsset(path);Import(path,32,new Vector2(.5f,.5f));return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }
        static void Import(string path,float ppu,Vector2 pivot)
        {
            var importer=(TextureImporter)AssetImporter.GetAtPath(path);importer.textureType=TextureImporterType.Sprite;importer.spriteImportMode=SpriteImportMode.Single;
            importer.spritePixelsPerUnit=ppu;importer.alphaIsTransparency=true;importer.mipmapEnabled=false;importer.filterMode=FilterMode.Point;importer.textureCompression=TextureImporterCompression.Uncompressed;importer.npotScale=TextureImporterNPOTScale.None;
            var settings=new TextureImporterSettings();importer.ReadTextureSettings(settings);settings.spriteAlignment=(int)SpriteAlignment.Custom;settings.spritePivot=pivot;settings.spriteMeshType=SpriteMeshType.FullRect;importer.SetTextureSettings(settings);importer.SaveAndReimport();
        }
        static GameObject Visual(string name,Sprite sprite,Vector2 at,Vector2 size,int order)
        {
            var g=new GameObject(name,typeof(SpriteRenderer));g.transform.position=at;g.transform.localScale=new Vector3(size.x/sprite.bounds.size.x,size.y/sprite.bounds.size.y,1);
            var view=g.GetComponent<SpriteRenderer>();view.sprite=sprite;view.sortingOrder=order;return g;
        }
    }
}
