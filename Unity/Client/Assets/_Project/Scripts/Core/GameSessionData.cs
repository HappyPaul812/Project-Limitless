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

        public static void SelectPlayerVisual(PlayerVisualType visualType) => SelectedPlayerVisual = visualType;

        public static void ConfigurePlayer(PlayerVisualType visualType, string playerName)
        {
            SelectedPlayerVisual = visualType;
            PlayerName = playerName;
        }

        public static void SelectPlayerPath(string pathId) => SelectedPlayerPathId = pathId ?? string.Empty;

        public static void SelectJob(string jobId) => SelectedJobId = jobId ?? string.Empty;

        public static void Reset()
        {
            SelectedPlayerVisual = PlayerVisualType.Male;
            PlayerName = string.Empty;
            SelectedPlayerPathId = string.Empty;
            SelectedJobId = string.Empty;
        }
    }
}
