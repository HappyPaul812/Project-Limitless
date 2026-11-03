using ProjectLimitless.Player;
using UnityEngine;
namespace ProjectLimitless.World
{
    /// <summary>승인 그루터기 한 개에만 발 위치 깊이 정렬을 적용합니다. 기존 Player 렌더 순서는 바꾸지 않습니다.</summary>
    public sealed class Main24F2StumpDepth : MonoBehaviour
    {
        SpriteRenderer view;
        PlayerController player;
        void Awake() => view = GetComponent<SpriteRenderer>();
        void LateUpdate()
        {
            if (player == null) player = FindAnyObjectByType<PlayerController>();
            if (player == null || view == null) return;
            var actor = player.GetComponentInChildren<SpriteRenderer>();
            if (actor == null) return;
            view.sortingLayerID = actor.sortingLayerID;
            view.sortingOrder = actor.sortingOrder + (player.transform.position.y > transform.position.y ? 1 : -1);
        }
    }
}
