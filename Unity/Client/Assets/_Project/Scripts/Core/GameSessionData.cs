using ProjectLimitless.Player;

namespace ProjectLimitless.Core
{
    /// <summary>현재 실행 동안 Scene 사이에 캐릭터 생성 선택값을 전달합니다.</summary>
    public static class GameSessionData
    {
        public static PlayerVisualType SelectedPlayerVisual { get; private set; } = PlayerVisualType.Male;
        public static string PlayerName { get; private set; } = string.Empty;

        /// <summary>
        /// 선택한 길의 안정적인 ID입니다. ScriptableObject 참조가 없어도 Scene 이동과 향후 저장 파일에서 사용할 수 있습니다.
        /// </summary>
        public static string SelectedPlayerPathId { get; private set; } = string.Empty;

        /// <summary>선택한 직업을 Scene 이동과 향후 저장 시스템에서 복원하기 위한 안정적인 ID입니다.</summary>
        public static string SelectedJobId { get; private set; } = string.Empty;

        /// <summary>다음 Scene에서 플레이어를 배치할 Spawn Point ID입니다.</summary>
        public static string PendingSpawnPointId { get; private set; } = string.Empty;

        /// <summary>현재 레벨과 그 레벨 안에서 진행 중인 경험치입니다. 평생 누적 경험치가 아닙니다.</summary>
        public static int Level { get; private set; } = 1;
        public static int CurrentExperience { get; private set; }

        /// <summary>
        /// 현재 실행 중 마지막으로 안전하게 도착한 월드 Scene과 Spawn입니다. PendingSpawnPointId는
        /// "지금 이동 중인 목적지"이고, 이 두 값은 "다음 실행에서 이어갈 위치"라는 차이가 있습니다.
        /// </summary>
        public static string CurrentSceneId { get; private set; } = string.Empty;
        public static string LastSpawnPointId { get; private set; } = string.Empty;
        public static bool HasSavedWorldPosition { get; private set; }
        public static float SavedPositionX { get; private set; }
        public static float SavedPositionY { get; private set; }

        public static void SelectPlayerVisual(PlayerVisualType visualType) => SelectedPlayerVisual = visualType;

        public static void ConfigurePlayer(PlayerVisualType visualType, string playerName)
        {
            SelectedPlayerVisual = visualType;
            PlayerName = playerName;
        }

        public static void SelectPlayerPath(string pathId) => SelectedPlayerPathId = pathId ?? string.Empty;

        public static void SelectJob(string jobId) => SelectedJobId = jobId ?? string.Empty;

        public static void SetPendingSpawnPoint(string spawnPointId) => PendingSpawnPointId = spawnPointId ?? string.Empty;

        public static void ClearPendingSpawnPoint() => PendingSpawnPointId = string.Empty;

        public static void ConfigureProgress(int level, int currentExperience)
        {
            Level = System.Math.Max(1, System.Math.Min(CharacterGrowthCalculator.MaxLevel, level));
            CurrentExperience = Level == CharacterGrowthCalculator.MaxLevel ? 0 : System.Math.Max(0, currentExperience);
        }

        public static void RecordLocation(string sceneId, string spawnPointId)
        {
            CurrentSceneId = sceneId ?? string.Empty;
            LastSpawnPointId = spawnPointId ?? string.Empty;
        }

        /// <summary>
        /// Scene ID는 어느 지도에 있는지만 알려주고, 이 두 float는 그 지도 안의 실제 위치를 알려줍니다.
        /// Transform 자체는 디스크에 저장하지 않고 실행 환경과 무관한 숫자만 세션에 보관합니다.
        /// </summary>
        public static void RecordWorldPosition(float x, float y)
        {
            HasSavedWorldPosition = true;
            SavedPositionX = x;
            SavedPositionY = y;
        }

        /// <summary>Scene 전환 전에는 이전 지도의 좌표가 새 지도에 적용되지 않도록 먼저 무효화합니다.</summary>
        public static void ClearWorldPosition()
        {
            HasSavedWorldPosition = false;
            SavedPositionX = 0f;
            SavedPositionY = 0f;
        }

        public static void Reset()
        {
            SelectedPlayerVisual = PlayerVisualType.Male;
            PlayerName = string.Empty;
            SelectedPlayerPathId = string.Empty;
            SelectedJobId = string.Empty;
            PendingSpawnPointId = string.Empty;
            Level = 1;
            CurrentExperience = 0;
            CurrentSceneId = string.Empty;
            LastSpawnPointId = string.Empty;
            ClearWorldPosition();
        }
    }
}
