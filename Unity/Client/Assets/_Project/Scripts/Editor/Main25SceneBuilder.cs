using System;
using System.IO;
using System.Linq;
using ProjectLimitless.CameraSystem;
using ProjectLimitless.Player;
using ProjectLimitless.World;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace ProjectLimitless.EditorTools
{
    /// <summary>승인 PNG 원본을 수정하지 않고 고갯길 Scene에 배치합니다.</summary>
    public static class Main25SceneBuilder
    {
        private const string Root = "Assets/_Project/";
        private const string ScenePath = Root + "Scenes/World_NorthF3.unity";

        public static void Build()
        {
            if (!Application.isBatchMode || Environment.GetEnvironmentVariable("LIMITLESS_I130_QA") != Application.dataPath)
                throw new InvalidOperationException("Main25 Scene은 명시적으로 지정된 격리 배치에서만 생성합니다.");

            var village = EditorSceneManager.OpenScene(Root + "Scenes/World_FrostwayVillage.unity");
            var playerRoot = Object.Instantiate(Object.FindAnyObjectByType<PlayerController>().transform.root.gameObject);
            var cameraRoot = Object.Instantiate(Camera.main.transform.root.gameObject);
            playerRoot.name = "Player";
            cameraRoot.name = "Main Camera";
            var field = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            SceneManager.MoveGameObjectToScene(playerRoot, field);
            SceneManager.MoveGameObjectToScene(cameraRoot, field);
            EditorSceneManager.CloseScene(village, true);
            SceneManager.SetActiveScene(field);

            var player = playerRoot.GetComponentInChildren<PlayerController>();
            player.transform.position = new Vector2(0, -10);
            player.GetComponent<Rigidbody2D>().position = player.transform.position;
            var camera = cameraRoot.GetComponentInChildren<Camera>();
            camera.transform.position = new Vector3(0, -9, -10);
            camera.orthographicSize = 5;
            camera.backgroundColor = new Color(.57f, .66f, .72f);
            camera.GetComponent<CameraFollow>().SetTarget(player.transform);

            var bounds = new GameObject("F3 World Bounds", typeof(WorldBounds2D)).GetComponent<WorldBounds2D>();
            bounds.Configure(Vector2.zero, new Vector2(24, 28));
            // 출입은 범위 안 표식에서 명시적으로 선택합니다. 북쪽 미개통 경계도 닫혀 있습니다.
            WorldBounds2D.CreateBoundaryColliders(bounds.transform, bounds.Bounds, .4f);
            var spawn = new GameObject(Chapter3Main25Flow.EntrySpawn, typeof(SceneSpawnPoint));
            spawn.transform.position = player.transform.position;
            spawn.GetComponent<SceneSpawnPoint>().Configure(Chapter3Main25Flow.EntrySpawn);

            var snow = AssetDatabase.LoadAssetAtPath<Sprite>(Root + "Resources/F2_FirstSnow_GroundOverlay_v1_CANDIDATE.png");
            if (snow == null) throw new InvalidOperationException("승인 FirstSnow Sprite 없음");
            var transition = Load("F3_SnowRock_GroundTransition");
            var rock = Load("F3_LowFrostRock_Outcrop");
            var marker = Load("F3_FlushStone_EdgeMarker_v2");
            var ground = new GameObject("승인 설원 바닥");
            var frost = Enumerable.Range(1, 3).Select(i => AssetDatabase.LoadAssetAtPath<Sprite>(
                Root + $"Resources/FrostwayVillageV10/Sprites/Frostway_Ground_ThinFrost_{i:00}.png")).ToArray();
            var earth = AssetDatabase.LoadAssetAtPath<Sprite>(
                Root + "Resources/FrostwayVillageV10/Sprites/Frostway_Ground_Earth_01.png");
            if (frost.Any(tile => tile == null) || earth == null)
                throw new InvalidOperationException("승인 서릿길 지면 Sprite 없음");
            // 기반 타일로 영역 전체를 덮고 투명 눈은 길 밖에만 흩어 배치합니다.
            for (int row = 0; row < 14; row++)
                for (int column = 0; column < 12; column++)
                {
                    var tile = frost[(row * 5 + column * 7) % frost.Length];
                    var point = new Vector2(-11 + column * 2, -13 + row * 2);
                    Place("설면", tile, point - (Vector2)tile.bounds.center, new Vector2(2, 2), -20, ground.transform);
                    if (Mathf.Abs(point.x) > 3 && (row * 3 + column) % 5 == 0)
                        Place("가장자리 적설", snow, point, snow.bounds.size, -18, ground.transform);
                }
            for (int row = 0; row < 14; row++)
            {
                var point = new Vector2(0, -13 + row * 2);
                var scale = new Vector2(4.5f / earth.bounds.size.x, 2 / earth.bounds.size.y);
                Place("고갯길 · 폭 4.5", earth, point - Vector2.Scale(earth.bounds.center, scale),
                    new Vector2(4.5f, 2), -19, ground.transform, new Color(.80f, .84f, .89f));
            }
            Place("지면 접합 · 단일 배치", transition, new Vector2(-2.5f, -8), transition.bounds.size, -17, ground.transform);
            for (int i = 0; i < 8; i++)
            {
                float side = i % 2 == 0 ? -1 : 1;
                var outcrop = Place("길 밖 저상 암반", rock, new Vector2(side * (4.3f + i % 3), -10 + i * 3),
                    rock.bounds.size, 1, ground.transform);
                var collider = outcrop.AddComponent<BoxCollider2D>();
                collider.size = new Vector2(1.45f, .65f);
            }
            Place("남쪽 경계석 v2", marker, new Vector2(-3.3f, -10), marker.bounds.size, 0, ground.transform);
            Place("북쪽 경계석 v2", marker, new Vector2(3.3f, 10), marker.bounds.size, 0, ground.transform);
            if (!EditorSceneManager.SaveScene(field, ScenePath)) throw new IOException(ScenePath);
            var scenes = EditorBuildSettings.scenes.Where(scene => scene.path != ScenePath).ToList();
            scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.SaveAssets();
            Debug.Log("I130_F3_SCENE_BUILT");
        }

        private static Sprite Load(string prefix)
        {
            string path = AssetDatabase.FindAssets("t:Sprite", new[] { Root + "Resources/Main25Environment" })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Single(name => Path.GetFileName(name).StartsWith(prefix, StringComparison.Ordinal));
            return AssetDatabase.LoadAssetAtPath<Sprite>(path);
        }

        private static GameObject Place(string name, Sprite sprite, Vector2 position, Vector2 size, int order,
            Transform parent, Color? tint = null)
        {
            var go = new GameObject(name, typeof(SpriteRenderer));
            go.transform.SetParent(parent, false);
            go.transform.position = position;
            go.transform.localScale = new Vector3(size.x / sprite.bounds.size.x, size.y / sprite.bounds.size.y, 1);
            var view = go.GetComponent<SpriteRenderer>();
            view.sprite = sprite;
            view.sortingOrder = order;
            view.color = tint ?? Color.white;
            return go;
        }
    }
}
