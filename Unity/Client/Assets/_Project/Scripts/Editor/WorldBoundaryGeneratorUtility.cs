using ProjectLimitless.World;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>World/Field Generator가 Bounds와 출입구 기반 외곽 Collider를 동일 방식으로 생성하게 합니다.</summary>
    public static class WorldBoundaryGeneratorUtility
    {
        public static WorldBounds2D Create(
            string rootName,
            Vector2 center,
            Vector2 size,
            float colliderThickness,
            params WorldBoundaryOpening[] openings)
        {
            GameObject root = new GameObject(rootName, typeof(WorldBounds2D));
            WorldBounds2D worldBounds = root.GetComponent<WorldBounds2D>();
            worldBounds.Configure(center, size);
            WorldBounds2D.CreateBoundaryColliders(root.transform, worldBounds.Bounds, colliderThickness, openings);
            return worldBounds;
        }
    }
}
