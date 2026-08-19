using System;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.CameraSystem;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>월드 외곽의 어느 면에 출입구 또는 경계가 있는지 나타냅니다.</summary>
    public enum WorldBoundarySide
    {
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary>
    /// 외곽 Collider를 만들 때 비워 둘 출입구 한 곳을 정의합니다.
    /// 예를 들어 남쪽 면의 중앙 좌표 0, 폭 3은 남쪽 중앙에 3칸 너비의 통로를 남긴다는 뜻입니다.
    /// </summary>
    [Serializable]
    public readonly struct WorldBoundaryOpening
    {
        public WorldBoundaryOpening(WorldBoundarySide side, float center, float width)
        {
            Side = side;
            Center = center;
            Width = width;
        }

        /// <summary>출입구가 위치하는 상·하·좌·우 면입니다.</summary>
        public WorldBoundarySide Side { get; }
        /// <summary>가로 면에서는 X 좌표, 세로 면에서는 Y 좌표로 사용하는 출입구 중심입니다.</summary>
        public float Center { get; }
        /// <summary>Collider를 비워 실제로 통과할 수 있게 만들 구간의 길이입니다.</summary>
        public float Width { get; }
    }

    /// <summary>
    /// World/Field Scene에서 플레이 가능한 사각 영역을 한 곳에 정의합니다.
    /// Scene의 WorldBounds GameObject에 붙으며, CameraFollow와 외곽 Collider가 같은 영역 값을 사용하게 합니다.
    /// </summary>
    public sealed class WorldBounds2D : MonoBehaviour
    {
        // 배경 Tile 영역의 중심 좌표입니다. Scene마다 맵 배치가 다를 수 있어 Inspector에 저장합니다.
        [SerializeField] private Vector2 center;
        // 검은 외부 영역을 제외한 실제 배경 Tile 영역의 전체 가로·세로 크기입니다.
        [SerializeField] private Vector2 size;

        /// <summary>카메라 제한과 Collider 생성에서 함께 사용하는 Unity Bounds 값입니다.</summary>
        public Bounds Bounds => new Bounds(center, new Vector3(size.x, size.y, 0f));

        /// <summary>Scene Generator나 런타임 설치 코드가 영역 값을 지정할 때 호출합니다.</summary>
        public void Configure(Vector2 boundsCenter, Vector2 boundsSize)
        {
            center = boundsCenter;
            size = boundsSize;
            BindCamera();
        }

        // Scene에 저장된 WorldBounds2D가 활성화될 때 카메라가 이 영역을 사용하도록 연결합니다.
        private void Awake() => BindCamera();

        /// <summary>현재 Scene의 CameraFollow를 찾아 이 Bounds를 카메라 이동 제한으로 전달합니다.</summary>
        private void BindCamera()
        {
            if (size.x <= 0f || size.y <= 0f) return;

            CameraFollow cameraFollow = FindFirstObjectByType<CameraFollow>();
            if (cameraFollow != null) cameraFollow.SetWorldBounds(this);
        }

        /// <summary>
        /// Scene Generator가 Bounds의 네 면에 BoxCollider2D를 만들 때 호출합니다.
        /// <paramref name="openings"/>로 지정한 구간은 Collider를 나눠 생성하여 실제 출입구로 비워 둡니다.
        /// </summary>
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
            // 한 면의 시작점부터 진행하면서 출입구 직전까지만 Collider Segment로 만듭니다.
            // 여러 출입구가 전달되어도 좌표순으로 처리하므로 Segment가 서로 겹치지 않습니다.
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
