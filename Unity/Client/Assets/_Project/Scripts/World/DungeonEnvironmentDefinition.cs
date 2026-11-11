using System;
using UnityEngine;
namespace ProjectLimitless.World
{
    /// <summary>승인 환경의 그림 배치와 보행 바닥 기하를 함께 정의합니다. 퀘스트·Save 진행값은 포함하지 않습니다.</summary>
    public sealed class DungeonEnvironmentDefinition : ScriptableObject
    {
        public enum RenderMode { RetainLegacy, HideDetail, Tiled, NativeSprite }
        [Serializable] public sealed class Placement
        {
            public string anchorName;
            public Vector3 localPosition;
            public RenderMode mode;
            public Sprite sprite;
            // 같은 위치의 상태별 표시에 쓰는 선택 이미지입니다. 상태 판정은 담당 시스템이 수행합니다.
            public Sprite alternateSprite;
            public Vector2 worldSize;
            // 보행 가능한 방·복도만 지정합니다. 배경 StoneBase와 벽/소품은 포함하지 않습니다.
            public bool walkableFloor;
            public Vector2 visualOffset;
            public Color color = Color.white;
        }
        public string sceneName;
        // 접지 지름보다 좁아 실제로 통과할 수 없는 기존 소품 사이 틈만 막습니다.
        // 통로를 새로 막는 용도가 아니며 현재 B1 잔해 위 0.95유닛 틈 한 곳만 등록합니다.
        public Rect[] blockedGaps = Array.Empty<Rect>();
        public Placement[] placements;
    }
}
