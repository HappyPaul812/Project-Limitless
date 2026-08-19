using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.CameraSystem;
using UnityEngine;

namespace ProjectLimitless.World
{
    public enum WorldBoundarySide
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [Serializable]
    public readonly struct WorldBoundaryOpening
    {
        public WorldBoundaryOpening(WorldBoundarySide side, float center, float width)
        {
            Side = side;
            Center = center;
            Width = width;
        }

        public WorldBoundarySide Side { get; }
        public float Center { get; }
        public float Width { get; }
    }

    /// <summary>World/Field의 플레이 가능 영역을 정의하고 CameraFollow에 동일 Bounds를 제공합니다.</summary>
    public sealed class WorldBounds2D : MonoBehaviour
    {
        [SerializeField] private Vector2 center;
        [SerializeField] private Vector2 size;

        public Bounds Bounds => new Bounds(center, new Vector3(size.x, size.y, 0f));

        public void Configure(Vector2 boundsCenter, Vector2 boundsSize)
        {
            center = boundsCenter;
            size = boundsSize;
            BindCamera();
        }

        private void Awake() => BindCamera();

        private void BindCamera()
        {
            if (size.x <= 0f || size.y <= 0f) return;

            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
            if (cameraFollow != null) cameraFollow.SetWorldBounds(this);
        }

        /// <summary>Bounds 네 면을 막되 명시된 구간만 출입구로 비워 둡니다.</summary>
        public static void CreateBoundaryColliders(
            Transform parent,
            Bounds bounds,
            float thickness,
            params WorldBoundaryOpening[] openings)
        {
            CreateHorizontalSide(parent, bounds, thickness, WorldBoundarySide.Top, openings);
            CreateHorizontalSide(parent, bounds, thickness, WorldBoundarySide.Bottom, openings);
            CreateVerticalSide(parent, bounds, thickness, WorldBoundarySide.Left, openings);
            CreateVerticalSide(parent, bounds, thickness, WorldBoundarySide.Right, openings);
        }

        private static void CreateHorizontalSide(
            Transform parent,
            Bounds bounds,
            float thickness,
            WorldBoundarySide side,
            IEnumerable<WorldBoundaryOpening> openings)
        {
            float y = side == WorldBoundarySide.Top ? bounds.max.y : bounds.min.y;
            CreateSegments(side, bounds.min.x, bounds.max.x, openings, (index, start, end) =>
            {
                CreateCollider(parent, $"Boundary_{side}_{index}",
                    new Vector2((start + end) * .5f, y), new Vector2(end - start, thickness));
            });
        }

        private static void CreateVerticalSide(
            Transform parent,
            Bounds bounds,
            float thickness,
            WorldBoundarySide side,
            IEnumerable<WorldBoundaryOpening> openings)
        {
            float x = side == WorldBoundarySide.Right ? bounds.max.x : bounds.min.x;
            CreateSegments(side, bounds.min.y, bounds.max.y, openings, (index, start, end) =>
            {
                CreateCollider(parent, $"Boundary_{side}_{index}",
                    new Vector2(x, (start + end) * .5f), new Vector2(thickness, end - start));
            });
        }

        private static void CreateSegments(
            WorldBoundarySide side,
            float minimum,
            float maximum,
            IEnumerable<WorldBoundaryOpening> openings,
            Action<int, float, float> createSegment)
        {
            float cursor = minimum;
            int index = 0;
            foreach (WorldBoundaryOpening opening in openings.Where(item => item.Side == side).OrderBy(item => item.Center))
            {
                float openingStart = Mathf.Clamp(opening.Center - (opening.Width * .5f), minimum, maximum);
                float openingEnd = Mathf.Clamp(opening.Center + (opening.Width * .5f), minimum, maximum);
                if (openingStart > cursor) createSegment(index++, cursor, openingStart);
                cursor = Mathf.Max(cursor, openingEnd);
            }

            if (cursor < maximum) createSegment(index, cursor, maximum);
        }

        private static void CreateCollider(Transform parent, string name, Vector2 position, Vector2 colliderSize)
        {
            GameObject boundary = new GameObject(name, typeof(BoxCollider2D));
            boundary.transform.SetParent(parent, false);
            boundary.transform.localPosition = position;
            boundary.GetComponent<BoxCollider2D>().size = colliderSize;
        }
    }
}
