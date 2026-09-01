using ProjectLimitless.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ProjectLimitless.World
{
    /// <summary>
    /// 월드 Player의 실제 좌표를 일정 간격으로 저장하고 이어하기 때 복원합니다.
    /// 매 프레임 JSON을 쓰면 디스크 작업이 지나치게 잦아 끊김과 파일 손상 위험이 커지므로 5초 간격을 사용합니다.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class WorldPositionSaveController : MonoBehaviour
    {
        private const float AutoSaveIntervalSeconds = 5f;
        private float elapsed;
        private WorldBounds2D worldBounds;
        private bool restored;

        private void Awake()
        {
            worldBounds = FindAnyObjectByType<WorldBounds2D>();
            TryRestoreSavedPosition();
        }

        // 시작 마을처럼 Bounds를 다른 Awake에서 설치하는 Scene도 있으므로 Start에서 한 번 더 찾습니다.
        private void Start()
        {
            if (worldBounds == null) worldBounds = FindAnyObjectByType<WorldBounds2D>();
            TryRestoreSavedPosition();
        }

        private void Update()
        {
            if (worldBounds == null || GameSaveService.CurrentSlotIndex <= 0) return;
            elapsed += Time.unscaledDeltaTime;
            if (elapsed < AutoSaveIntervalSeconds) return;
            elapsed = 0f;
            SaveNow();
        }

        /// <summary>
        /// 저장 좌표가 현재 Scene과 Bounds에 모두 맞을 때만 SpawnPoint보다 먼저 사용합니다.
        /// 좌표가 없거나 맵 밖이면 PendingSpawnPoint를 지우지 않아 기존 안전 Spawn이 fallback으로 동작합니다.
        /// </summary>
        private void TryRestoreSavedPosition()
        {
            Scene scene = gameObject.scene;
            if (restored || worldBounds == null || !GameSessionData.HasSavedWorldPosition || GameSessionData.CurrentSceneId != scene.name) return;
            Vector2 saved = new Vector2(GameSessionData.SavedPositionX, GameSessionData.SavedPositionY);
            if (!IsFinite(saved) || !worldBounds.Bounds.Contains(new Vector3(saved.x, saved.y, worldBounds.Bounds.center.z)))
            {
                Debug.LogWarning($"저장 좌표 {saved}가 '{scene.name}'의 유효 범위를 벗어나 SpawnPoint를 사용합니다.");
                GameSessionData.ClearWorldPosition();
                return;
            }

            Rigidbody2D body = GetComponent<Rigidbody2D>();
            if (body != null) { body.position = saved; body.linearVelocity = Vector2.zero; }
            else transform.position = saved;
            GameSessionData.ClearPendingSpawnPoint();
            restored = true;
            Debug.Log($"마지막 월드 위치에서 이어합니다: {scene.name} {saved}");
        }

        private void SaveNow()
        {
            if (worldBounds == null || !gameObject.scene.IsValid()) return;
            Vector2 position = transform.position;
            if (!IsFinite(position) || !worldBounds.Bounds.Contains(new Vector3(position.x, position.y, worldBounds.Bounds.center.z))) return;
            GameSaveService.SaveCurrentWorldPosition(position, gameObject.scene.name, GameSessionData.LastSpawnPointId);
        }

        // 일시정지·종료 콜백은 가능한 범위에서 마지막 위치를 한 번 더 남깁니다. Battle에는 이 컴포넌트가 없어
        // 전투 도중 종료해도 마지막으로 안전하게 저장한 월드 좌표가 유지됩니다.
        private void OnApplicationPause(bool paused) { if (paused) SaveNow(); }
        private void OnApplicationQuit() => SaveNow();

        private static bool IsFinite(Vector2 value) => !float.IsNaN(value.x) && !float.IsInfinity(value.x) && !float.IsNaN(value.y) && !float.IsInfinity(value.y);
    }
}
