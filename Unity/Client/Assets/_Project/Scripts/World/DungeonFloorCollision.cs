using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 승인 환경 데이터에서 보행 바닥의 합집합을 구하고, 그 바깥 허공만 고정 물리 벽으로 만듭니다.
    /// 그림·기존 소품·퀘스트 문·외곽 WorldBounds는 그대로 두며 이동 좌표를 강제로 제한하지 않습니다.
    /// </summary>
    public sealed class DungeonFloorCollision : MonoBehaviour
    {
        private readonly List<Rect> floors = new List<Rect>();
        private Rect[] blockedGaps;
        public IReadOnlyList<Rect> Floors => floors;
        public IReadOnlyList<Rect> BlockedGaps => blockedGaps;
        public CompositeCollider2D Solid { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Register()
        {
            SceneManager.sceneLoaded -= Install;
            SceneManager.sceneLoaded += Install;
        }

        private static void Install(Scene scene, LoadSceneMode mode)
        {
            if (!AppliesTo(scene.name)) return;
            foreach (var root in scene.GetRootGameObjects())
                if (root.GetComponent<DungeonFloorCollision>() != null) return;
            var profile = Resources.Load<DungeonEnvironmentDefinition>("DungeonEnvironmentDefinitions/" +
                (scene.name == "Dungeon_01" ? "Dungeon01B1Renewal" : "Dungeon01B2Renewal"));
            if (profile == null) { Debug.LogError("던전 바닥 충돌 데이터가 없습니다."); return; }
            var go = new GameObject("DungeonFloorCollision");
            SceneManager.MoveGameObjectToScene(go, scene);
            go.AddComponent<DungeonFloorCollision>().Build(profile);
        }

        public static bool AppliesTo(string scene) => scene == "Dungeon_01" || scene == "Dungeon_01_B2";

        private void Build(DungeonEnvironmentDefinition profile)
        {
            var baseTile = profile.placements.First(p => p.anchorName == "StoneBase");
            Rect area = new Rect((Vector2)baseTile.localPosition - baseTile.worldSize / 2, baseTile.worldSize);
            foreach (var p in profile.placements.Where(p => p.walkableFloor))
                floors.Add(new Rect((Vector2)p.localPosition - p.worldSize / 2, p.worldSize));
            blockedGaps = profile.blockedGaps ?? System.Array.Empty<Rect>();
            if (floors.Count == 0) { Debug.LogError("던전 보행 바닥이 비어 있습니다.", this); return; }

            // 원래 B1 북쪽 바닥 사이 0.05유닛 줄눈은 통로입니다. 인접 끝점만 0.06 이내에서
            // 같은 선으로 맞춥니다. 방을 넓히거나 실제 벽/문의 위치를 바꾸는 여유폭이 아닙니다.
            var xs = Coordinates(floors.Concat(blockedGaps).SelectMany(r => new[] { r.xMin, r.xMax }).Concat(new[] { area.xMin, area.xMax }));
            var ys = Coordinates(floors.Concat(blockedGaps).SelectMany(r => new[] { r.yMin, r.yMax }).Concat(new[] { area.yMin, area.yMax }));
            for (int i = 0; i < floors.Count; i++)
            {
                var r = floors[i];
                floors[i] = Rect.MinMaxRect(Nearest(xs, r.xMin), Nearest(ys, r.yMin), Nearest(xs, r.xMax), Nearest(ys, r.yMax));
            }
            var body = gameObject.AddComponent<Rigidbody2D>(); body.bodyType = RigidbodyType2D.Static;
            Solid = gameObject.AddComponent<CompositeCollider2D>();
            Solid.geometryType = CompositeCollider2D.GeometryType.Polygons;
            Solid.generationType = CompositeCollider2D.GenerationType.Manual;
            Solid.vertexDistance = .001f;
            // 바닥 끝점으로 나눈 셀 중 바닥이 아닌 셀만 채웁니다. 가로로 연속한 셀은 먼저 합치고,
            // Composite가 모든 접합선을 제거하므로 휠체어/도보 원형 접지체가 셀 이음매에 걸리지 않습니다.
            for (int y = 0; y < ys.Count - 1; y++)
            {
                int start = -1;
                for (int x = 0; x < xs.Count; x++)
                {
                    Vector2 center = x < xs.Count - 1 ? new Vector2((xs[x] + xs[x + 1]) / 2, (ys[y] + ys[y + 1]) / 2) : Vector2.zero;
                    bool blocked = x < xs.Count - 1 && (!floors.Any(r => r.Contains(center)) || blockedGaps.Any(r => r.Contains(center)));
                    if (blocked && start < 0) start = x;
                    if ((blocked || start < 0)) continue;
                    var box = gameObject.AddComponent<BoxCollider2D>();
                    box.offset = new Vector2((xs[start] + xs[x]) / 2, (ys[y] + ys[y + 1]) / 2);
                    box.size = new Vector2(xs[x] - xs[start], ys[y + 1] - ys[y]);
                    box.compositeOperation = Collider2D.CompositeOperation.Merge;
                    start = -1;
                }
            }
            Solid.GenerateGeometry();
            Physics2D.SyncTransforms();
        }

        private static List<float> Coordinates(IEnumerable<float> values)
        {
            var result = new List<float>();
            foreach (float value in values.OrderBy(v => v))
            {
                if (result.Count > 0 && value - result[result.Count - 1] <= .06f)
                    result[result.Count - 1] = (result[result.Count - 1] + value) / 2;
                else result.Add(value);
            }
            return result;
        }
        private static float Nearest(List<float> values, float value) => values.OrderBy(v => Mathf.Abs(v - value)).First();

        /// <summary>이어하기에서만 사용합니다. 정상 좌표는 보존하고 벽 안 좌표만 가까운 안전 바닥으로 복구합니다.</summary>
        public bool TryResolveSavedPosition(Collider2D player, Vector2 saved, out Vector2 safe)
        {
            safe = saved;
            Physics2D.SyncTransforms();
            Vector2 offset = (Vector2)player.bounds.center - (Vector2)player.transform.position;
            float radius = Mathf.Max(player.bounds.extents.x, player.bounds.extents.y);
            // 물리 엔진의 접촉 오차 안에서 벽에 붙어 저장된 정상 좌표는 복구 대상으로 보지 않습니다.
            if (IsSafe(saved + offset, Mathf.Max(.001f, radius - .005f), player)) return true;
            radius += .01f;
            // 잘못 저장된 좌표에서만 후보를 찾습니다. 매 프레임 Clamp/텔레포트를 하지 않습니다.
            var candidates = new List<Vector2>();
            foreach (var floor in floors)
                for (float y = floor.yMin + radius; y <= floor.yMax - radius; y += .2f)
                    for (float x = floor.xMin + radius; x <= floor.xMax - radius; x += .2f)
                        candidates.Add(new Vector2(x, y));
            foreach (var center in candidates.OrderBy(v => (v - saved - offset).sqrMagnitude))
            {
                // 복구가 미처치 보스 장벽 너머로 플레이어를 보내 진행을 건너뛰지 않게 합니다.
                if (gameObject.scene.name == "Dungeon_01_B2" && !MainQuest11DungeonFlow.BossDefeated && center.y > 10.36f - radius) continue;
                if (!IsSafe(center, radius, player)) continue;
                safe = center - offset;
                return true;
            }
            return false;
        }

        private bool IsSafe(Vector2 center, float radius, Collider2D player)
        {
            if (!floors.Any(r => r.Contains(center))) return false;
            foreach (var c in Physics2D.OverlapCircleAll(center, radius))
            {
                if (!c.enabled || c.isTrigger || c.transform.IsChildOf(player.transform) || c.gameObject.scene != gameObject.scene
                    || c.attachedRigidbody != null && c.attachedRigidbody.bodyType != RigidbodyType2D.Static
                    || Physics2D.GetIgnoreLayerCollision(player.gameObject.layer, c.gameObject.layer)
                    || Physics2D.GetIgnoreCollision(player, c)) continue;
                return false;
            }
            return true;
        }
    }
}
