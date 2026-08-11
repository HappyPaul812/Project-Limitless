using ProjectLimitless.Player;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 현재 게임 실행 동안 Scene 사이에 전달할 간단한 선택값을 보관합니다.
    /// static 데이터는 CharacterCreation Scene이 사라져도 같은 Play 세션 안에서는 유지됩니다.
    /// 저장 파일은 만들지 않으므로 게임을 다시 실행하면 기본 Male로 시작합니다.
    /// </summary>
    public static class GameSessionData
    {
        /// <summary>Character Creation에서 선택한 플레이어 외형이며, 아무 조작을 하지 않아도 Male입니다.</summary>
        public static PlayerVisualType SelectedPlayerVisual { get; private set; } = PlayerVisualType.Male;

        /// <summary>게임 시작 버튼을 누르기 전에 현재 외형 선택을 세션에 기록합니다.</summary>
        public static void SelectPlayerVisual(PlayerVisualType visualType)
        {
            SelectedPlayerVisual = visualType;
        }

        /// <summary>새 Play 세션의 초기 상태를 명시적으로 Male로 되돌립니다.</summary>
        public static void Reset()
        {
            SelectedPlayerVisual = PlayerVisualType.Male;
        }
    }
}
