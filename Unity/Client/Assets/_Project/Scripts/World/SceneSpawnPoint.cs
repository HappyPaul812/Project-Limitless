using System.Collections;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>PendingSpawnPointId와 ID가 일치하면 Scene의 플레이어를 이 위치로 옮깁니다.</summary>
    public sealed class SceneSpawnPoint : MonoBehaviour
    {
        [SerializeField] private string spawnPointId;

        public void Configure(string id) => spawnPointId = id;

        private IEnumerator Start()
        {
            if (string.IsNullOrWhiteSpace(spawnPointId) || GameSessionData.PendingSpawnPointId != spawnPointId) yield break;

            PlayerController player = null;
            for (int frame = 0; frame < 10 && player == null; frame++)
            {
                player = FindFirstObjectByType<PlayerController>();
                if (player == null) yield return null;
            }

            if (player == null)
            {
                Debug.LogError($"Spawn Point '{spawnPointId}': PlayerController를 찾지 못했습니다.", this);
                yield break;
            }

            Rigidbody2D body = player.GetComponent<Rigidbody2D>();
            if (body != null)
            {
                body.position = transform.position;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                player.transform.position = transform.position;
            }

            GameSessionData.ClearPendingSpawnPoint();
        }
    }
}
