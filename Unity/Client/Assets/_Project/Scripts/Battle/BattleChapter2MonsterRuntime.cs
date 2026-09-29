using System.Collections.Generic;

namespace ProjectLimitless.Battle
{
    /// <summary>
    /// Chapter 2 일반 몬스터의 전투 중 행동 횟수와 예고만 보관합니다.
    /// MonsterDefinition을 공유하는 두 개체도 Combatant 참조별로 독립적으로 행동합니다.
    /// </summary>
    public sealed class BattleChapter2MonsterRuntime
    {
        private sealed class State
        {
            public int Action;
            public bool Prepared;
            public bool ShellProtected;
        }

        private readonly Dictionary<Combatant, State> states = new Dictionary<Combatant, State>(CombatantReferenceComparer.Instance);

        private State Get(Combatant actor)
        {
            if (!states.TryGetValue(actor, out State state))
                states[actor] = state = new State();
            return state;
        }

        /// <summary>자신의 다음 행동이 시작될 때만 갑각 수축이 끝납니다. 다른 참가자의 행동은 영향을 주지 않습니다.</summary>
        public void BeginActorAction(Combatant actor)
        {
            if (actor == null) return;
            State state = Get(actor);
            state.Action++;
            state.ShellProtected = false;
        }

        public bool IsShellProtected(Combatant actor) => actor != null && states.TryGetValue(actor, out State state)
            && state.ShellProtected;

        /// <summary>스킬 순서는 개체 자신의 실제 행동 횟수 기준입니다. 예고는 반드시 다음 자기 행동에 소비합니다.</summary>
        public string TakeAction(Combatant actor, string monsterId)
        {
            State state = Get(actor);
            int action = state.Action;
            switch (monsterId)
            {
                case "soot_hound": return action % 3 == 1 ? "bite" : action % 3 == 2 ? "charge" : "basic";
                case "heatwind_hawk": return action % 3 == 1 ? "feather" : action % 3 == 2 ? "dive" : "basic";
                case "fissure_lizard":
                    if (state.Prepared) { state.Prepared = false; return "fissure_charge"; }
                    if (action % 4 == 2) { state.Prepared = true; return "rumble"; }
                    return action % 4 == 1 ? "scratch" : "basic";
                case "ember_beetle":
                    if (action % 4 == 2) { state.ShellProtected = true; return "shell"; }
                    return action % 4 == 1 ? "secretion" : "basic";
                case "ember_wraith":
                    if (state.Prepared) { state.Prepared = false; return "core"; }
                    if (action % 4 == 1) return "scatter";
                    if (action % 4 == 2) { state.Prepared = true; return "concentrate"; }
                    return "basic";
                default: return null;
            }
        }

        public void RemoveInvalidCombatants(IEnumerable<Combatant> members)
        {
            var living = new HashSet<Combatant>(CombatantReferenceComparer.Instance);
            foreach (Combatant actor in members) if (actor.IsAlive) living.Add(actor);
            foreach (Combatant actor in new List<Combatant>(states.Keys))
                if (!living.Contains(actor)) states.Remove(actor);
        }
    }
}
