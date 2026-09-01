using System.Collections;
using ProjectLimitless.Core;
using ProjectLimitless.Player;
using UnityEngine;

namespace ProjectLimitless.World
{
    /// <summary>
    /// Scene 이동 후 플레이어가 나타날 안전한 위치를 표시하는 Component입니다.
    /// GameSessionData에 전달된 PendingSpawnPointId와 자신의 ID가 같을 때만 플레이어를 이 위치로 옮깁니다.
    /// </summary>
    public sealed class SceneSpawnPoint : MonoBehaviour
    {
        // 여러 Spawn Point 중 목적지 Trigger가 선택한 지점을 구분하는 고유 문자열입니다.
        [SerializeField] private string spawnPointId;

        /// <summary>Scene Generator가 이 Spawn Point의 ID를 지정할 때 호출합니다.</summary>
        public void Configure(string id) => spawnPointId = id;

        /// <summary>
        /// Scene의 첫 프레임부터 실행되며, 생성 순서 차이를 고려해 플레이어를 최대 10프레임 동안 찾습니다.
        /// 일치하는 목적지라면 플레이어를 배치하고 PendingSpawnPointId를 지워 재사용을 막습니다.
        /// </summary>
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
                // 물리 오브젝트는 Rigidbody2D 위치를 바꾸고 이전 Scene에서 남은 속도도 제거해야
                // Spawn 직후 의도하지 않은 방향으로 미끄러지지 않습니다.
                body.position = transform.position;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                player.transform.position = transform.position;
            }

            GameSessionData.ClearPendingSpawnPoint();
            // Field/마을 전환은 Scene 로드뿐 아니라 Player가 목적 Spawn에 놓여야 정상 완료입니다.
            // 이 뒤에만 저장하여 다음 이어하기가 검증된 SpawnPoint에서 시작하도록 합니다.
            GameSaveService.SaveCurrentSession(gameObject.scene.name, spawnPointId);
        }
    }
}
