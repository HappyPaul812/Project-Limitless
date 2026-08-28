using System;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// 사수 전용 야수의 전투 연출 데이터입니다. 야수는 파티 동료가 아니라 스킬 연출에 참여하는 존재이므로
    /// Combatant를 상속하지 않으며 HP, 독립 턴, Formation 슬롯도 갖지 않습니다.
    /// </summary>
    public sealed class BeastCompanionDefinition
    {
        public BeastCompanionDefinition(string id, string displayName, string runResourcePath,
            int frameWidth, int iconFrameIndex, float framesPerSecond, float displayScale, float travelSpeed)
        {
            Id = id ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            RunResourcePath = runResourcePath ?? string.Empty;
            FrameWidth = Math.Max(1, frameWidth);
            IconFrameIndex = Math.Max(0, iconFrameIndex);
            FramesPerSecond = Math.Max(1f, framesPerSecond);
            DisplayScale = Math.Max(.1f, displayScale);
            TravelSpeed = Math.Max(1f, travelSpeed);
        }

        public string Id { get; }
        public string DisplayName { get; }
        public string RunResourcePath { get; }
        public int FrameWidth { get; }
        /// <summary>
        /// Run 시트 중 작은 버튼에서도 몸통·머리·꼬리·다리가 잘 구분되는 프레임 번호입니다.
        /// 야수별 실루엣이 다르므로 UI 코드에 Wolf 전용 숫자를 넣지 않고 야수 데이터가 직접 제공합니다.
        /// </summary>
        public int IconFrameIndex { get; }
        public float FramesPerSecond { get; }
        public float DisplayScale { get; }
        public float TravelSpeed { get; }
    }

    /// <summary>
    /// 현재는 장착 UI가 없으므로 기본 Wolf를 반환하지만, Controller는 Wolf 경로나 프레임 수를 모릅니다.
    /// 향후 저장된 장착 ID를 받아 이 반환값만 Bear/Fox 정의로 바꾸면 같은 스킬과 Presenter를 재사용합니다.
    /// </summary>
    public static class BeastCompanionCatalog
    {
        private static readonly BeastCompanionDefinition DefaultWolf = new BeastCompanionDefinition(
            // Run 재생은 기존 12 FPS를 유지하고, 실제 Transform 이동만 최초 구현 속도의 2/3로 낮춥니다.
            // 프레임 속도와 이동 속도가 분리되어 있으므로 발 동작은 유지하면서 돌진 거리만 천천히 이동합니다.
            // 0부터 세는 4번 프레임(실제 다섯 번째)은 몸통과 네 다리의 간격이 비교적 분명해
            // 40px 안팎의 작은 스킬 버튼에서도 달리는 늑대 형태를 알아보기 쉽습니다.
            "wolf", "Wolf", "CompanionAssaultValidation/Wolf_Run", 64, 4, 12f, 2f, 760f * 2f / 3f);

        /// <summary>
        /// 현재 기본 야수를 UI 아이콘 같은 비전투 표시에도 제공합니다. 호출자는 Wolf 경로나 프레임 번호를
        /// 직접 알 필요가 없으므로, 향후 기본 또는 장착 야수가 Bear/Fox로 바뀌면 이 경계만 교체하면 됩니다.
        /// </summary>
        public static BeastCompanionDefinition GetDefaultDefinition()
        {
            return DefaultWolf;
        }

        public static BeastCompanionDefinition GetEquippedOrDefault(Combatant owner)
        {
            return DefaultWolf;
        }
    }
}
