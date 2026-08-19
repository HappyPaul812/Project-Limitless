using ProjectLimitless.World;
using UnityEngine;

namespace ProjectLimitless.Editor
{
    /// <summary>
    /// Unity Editor의 World/Field Scene Generator가 공통 경계 구조를 만들 때 사용하는 도구입니다.
    /// WorldBounds GameObject와 네 면 Collider를 함께 생성하여 새 필드마다 같은 코드를 복사하지 않게 합니다.
    /// </summary>
    public static class WorldBoundaryGeneratorUtility
    {
        /// <summary>
        /// Scene 생성 중 호출하며, 지정한 중심·크기의 WorldBounds2D와 출입구를 제외한 외곽 Collider를 만듭니다.
        /// 반환된 Component는 Generator가 추가 설정이나 검증에 사용할 수 있습니다.
        /// </summary>
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
