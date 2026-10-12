using System;
using UnityEngine;
namespace ProjectLimitless.World
{
    /// <summary>기존 충돌체를 가진 환경 오브젝트와 새 그림의 역할을 연결합니다. 퀘스트와 Save 데이터는 포함하지 않습니다.</summary>
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
            public Vector2 visualOffset;
            public Color color = Color.white;
        }
        public string sceneName;
        public Placement[] placements;
    }
}
