using System;

namespace ProjectLimitless.Battle
{
    /// <summary>열맥 거신의 자기 행동 순환과 미실행 예고를 보관합니다. 공용 턴/상태 및 파수꾼을 변경하지 않습니다.</summary>
    public sealed class BattleMain20BossRuntime
    {
        public const string MonsterId = "veinfire_colossus";
        public Combatant Boss { get; private set; }
        public bool PhaseTwo { get; private set; }
        public string Prepared { get; private set; }
        public int PhaseTransitions { get; private set; }
        private int cursor;
        public event Action PhaseChanged;
        public void Bind(Combatant boss)
        {
            if (Boss != null) Boss.HpChanged -= ObserveHp;
            Boss = boss; PhaseTwo = false; Prepared = null; cursor = 0; PhaseTransitions = 0;
            if (Boss != null) { Boss.HpChanged += ObserveHp; ObserveHp(); }
        }
        // 피해 직후 호출되므로 자기 차례 이전에도 Overlay/HUD를 바꿀 수 있습니다. HP0에서도 임계 도달 기록은 한번입니다.
        private void ObserveHp()
        {
            if (Boss == null || PhaseTwo || (long)Boss.CurrentHp * 2 > Boss.MaxHp) return;
            PhaseTwo = true; PhaseTransitions++; cursor = 0; PhaseChanged?.Invoke();
        }
        /// <summary>기존 응축 예고가 남아 있으면 원래 파동을 먼저 이행합니다. 추가 행동/턴은 만들지 않습니다.</summary>
        public string TakeAction()
        {
            ObserveHp();
            if (Prepared != null)
            {
                string action = Prepared; Prepared = null;
                if (!PhaseTwo) cursor = 0;
                else if (action == "eruption") cursor = 4;
                return action;
            }
            string[] cycle = PhaseTwo ? new[] { "injection", "molten", "resonance", "eruption", "wave" }
                : new[] { "injection", "molten", "condensation", "core_wave" };
            string result = cycle[cursor]; cursor = (cursor + 1) % cycle.Length;
            if (result == "condensation") Prepared = "core_wave";
            if (result == "resonance") Prepared = "eruption";
            return result;
        }
        public string Telegraph => Prepared == "core_wave" ? "다음 거신 행동: 열핵 파동 — 생존 아군 전체 80% 피해"
            : Prepared == "eruption" ? "다음 거신 행동: 열핵 분출 — 생존 아군 전체 60% 피해 후 과열 +1" : string.Empty;
    }
}
