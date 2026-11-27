using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    public sealed partial class BattleSceneController
    {
        private static bool Main26WispEncounter => BattleEncounterContext.StoryEncounterId == Chapter3Main26Flow.EncounterId
            && BattleEncounterContext.Monster?.MonsterId == "blizzard_wisp";
        private int main26WispTurn;
        private Combatant main26WispMarked, main26FrostbiteImpact;
        private readonly Dictionary<Combatant, Image> main26FrostbiteOverlays = new Dictionary<Combatant, Image>();

        private void PlayMain26WispAction(Combatant actor)
        {
            var candidates = TargetResolver.ResolveHostileTargets(actor, allies, TargetRangeType.Magic);
            int action = main26WispTurn++ % 4;
            var target = ChooseEnemyTarget(actor, candidates);
            if (action == 1 && target != null && !allies.LivingMembers.Any(statusEffects.IsColdActionBlocked))
            {
                main26WispMarked = target;
                messageText.text = $"눈폭풍 응축 · {target.DisplayName}에게 동상 예고";
                RefreshCombatantViews(null);
                FinishCurrentAction();
                return;
            }

            if (action == 2)
            {
                var marked = main26WispMarked;
                main26WispMarked = null;
                // 예고한 대상이 사라지면 동상을 옮기지 않고 기존 기본 공격으로 돌아갑니다.
                if (marked != null && marked.IsAlive && !allies.LivingMembers.Any(statusEffects.IsColdActionBlocked))
                {
                    main26FrostbiteImpact = marked;
                    PlayBasicAttack(actor, marked, 80, "동상 부여");
                    return;
                }
            }
            if (target != null) PlayBasicAttack(actor, target, 110, "흩날리는 눈발");
            else FinishCurrentAction();
        }

        private void ApplyMain26FrostbiteAtImpact(Combatant actor, Combatant target)
        {
            if (actor.Side != BattleSide.Enemies || main26FrostbiteImpact != target) return;
            main26FrostbiteImpact = null;
            if (target.IsAlive && !allies.LivingMembers.Any(statusEffects.IsColdActionBlocked))
                statusEffects.TryApplyFrostbite(target);
        }

        private void RefreshMain26WispDisplay()
        {
            if (bossTelegraphText == null) return;
            bool pending = !battleEnded && main26WispMarked != null && main26WispMarked.IsAlive;
            bossTelegraphText.text = pending ? $"{main26WispMarked.DisplayName} 대상 · 다음 정령 행동: 동상 부여" : string.Empty;
            bossTelegraphText.gameObject.SetActive(pending);
            foreach (var target in allies.Members)
            {
                bool active = !battleEnded && statusEffects.GetFrostbiteRemaining(target) > 0;
                if (!main26FrostbiteOverlays.TryGetValue(target, out var image) && active)
                {
                    var body = combatantViews[target].SpriteImage;
                    image = MakeImage(body.transform.parent, "Main26FrostbiteVfx", Color.white);
                    image.sprite = BattleApprovedColdAssets.Vfx("cold.frostbite");
                    BattleApprovedColdAssets.PlaceVfx(body, image, 128, null);
                    main26FrostbiteOverlays.Add(target, image);
                }
                if (image != null) image.gameObject.SetActive(active);
            }
        }
    }
}
