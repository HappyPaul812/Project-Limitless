using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 전투 UI에서 사용하는 아이콘 이름과 실제 Kenney Sprite 경로를 한곳에서 연결합니다.
    ///
    /// BattleSceneController가 PNG 파일명을 직접 알게 만들면 아이콘을 교체할 때 화면 구성 코드까지
    /// 찾아 고쳐야 합니다. 이 카탈로그를 사이에 두면 버튼과 HUD는 "공격", "방어" 같은 역할만 요청하고,
    /// 어떤 원본 Sprite를 보여 줄지는 이 파일에서 바꿀 수 있습니다. 향후 BattleSkillDefinition에 아이콘
    /// 식별자를 추가할 때도 같은 Load 흐름을 재사용할 수 있습니다.
    /// </summary>
    public static class BattleUiIconCatalog
    {
        public const string Attack = "command.attack";
        public const string Skill = "command.skill";
        public const string Defend = "command.defend";
        public const string Flee = "command.flee";
        public const string Cancel = "command.cancel";
        public const string Acting = "status.acting";
        public const string Taunt = "status.taunt";
        public const string Cooldown = "status.cooldown";

        // Resources 폴더 아래의 상대 경로만 보관합니다. 원본 압축과 라이선스는 ThirdParty에 그대로
        // 보존하고, 런타임에 필요한 PNG만 Resources에서 Sprite로 불러오는 구조입니다.
        private static readonly Dictionary<string, string> ResourcePaths = new Dictionary<string, string>
        {
            { Attack, "KenneyBattleIcons/sword" },
            { Skill, "KenneyBattleIcons/star" },
            { Defend, "KenneyBattleIcons/shield" },
            { Flee, "KenneyBattleIcons/exitRight" },
            { Cancel, "KenneyBattleIcons/cross" },
            { Acting, "KenneyBattleIcons/arrowRight" },
            { Taunt, "KenneyBattleIcons/target" },
            { Cooldown, "KenneyBattleIcons/hourglass" }
        };

        private static readonly Dictionary<string, Sprite> Cache = new Dictionary<string, Sprite>();

        /// <summary>
        /// 역할 식별자에 맞는 Sprite를 한 번만 읽고 재사용합니다.
        /// 에셋이 빠졌거나 Import가 아직 끝나지 않았으면 null을 반환합니다. 호출 쪽은 유니코드 기호를
        /// 대신 만들지 않고 한글 텍스트만 남기므로, 아이콘 문제 때문에 명령 자체가 사라지지 않습니다.
        /// </summary>
        public static Sprite Load(string iconId)
        {
            if (string.IsNullOrEmpty(iconId) || !ResourcePaths.TryGetValue(iconId, out string resourcePath))
                return null;
            if (Cache.TryGetValue(iconId, out Sprite cached)) return cached;

            Sprite sprite = Resources.Load<Sprite>(resourcePath);
            Cache[iconId] = sprite;
            return sprite;
        }
    }
}
