using System.Collections.Generic;

namespace ProjectLimitless.EditorTools
{
    /// <summary>이전 감사 도구 이름의 호환 진입점입니다. 폐기한 수동 외형 UI를 검사하지 않고 정식 조합 감사를 실행합니다.</summary>
    public static class PlayerAppearanceSelectionAudit
    {
        public static string Status => PlayerSpriteCombinationAudit.Status;
        public static List<string> Results => PlayerSpriteCombinationAudit.Results;
        public static string Launch() => PlayerSpriteCombinationAudit.Launch();
    }
}
