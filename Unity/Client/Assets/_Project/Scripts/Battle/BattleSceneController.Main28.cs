using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ProjectLimitless.World;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.Battle
{
    public sealed partial class BattleSceneController
    {
        private static bool Main28Encounter => BattleEncounterContext.StoryEncounterId == Chapter3Main28Flow.EncounterId
            && BattleEncounterContext.Monster?.MonsterId == "frost_tyrant";
        private int main28Action;
        private bool main28PhaseTwo;
        private Combatant main28MarkedTarget;
        private readonly HashSet<Combatant> main28HearthOffered = new HashSet<Combatant>();
        private readonly Dictionary<Combatant, Image> main28ColdOverlays = new Dictionary<Combatant, Image>();
        private Sprite[] main28Breath, main28Hit, main28Knockout;

        private void InitializeMain28()
        {
            statusEffects.ColdStatusChanged += HandleMain28ColdChange;
            main28Breath = LoadMain28Vfx("BreathCast");
            main28Hit = LoadMain28Vfx("BreathHit");
            main28Knockout = LoadMain28Vfx("Cryonecrosis");
            var hearth = MakeImage(battlefieldTransform, "LastWarmthHearth", Color.white);
            hearth.sprite = Resources.Load<Sprite>("Main28FrostTyrant/Hearth");
            hearth.preserveAspect = true;
            hearth.raycastTarget = false;
            SetRect(hearth.rectTransform, new Vector2(.15f, .18f), new Vector2(96, 96));
            // 도망이 금지된 보스전에서는 다섯 번째 명령 칸에 화로를 표시합니다.
            var icon = fleeButton.GetComponentsInChildren<Image>().FirstOrDefault(image => image.transform != fleeButton.transform);
            if (icon != null) icon.sprite = hearth.sprite;
            var help = MakeText(battlefieldTransform, "HearthHelp",
                "동결괴사 3중첩 시 즉시 전투불능 · 화로 사용: 생존 아군 1명의 중첩 전체 제거 · 행동 1회 · 아이템·탈렌트 소비 없음",
                battleFont, 15, new Vector2(.5f, .04f), new Vector2(1080, 26));
            help.color = Color.white;
        }

        private void RecordMain28HearthOpportunity()
        {
            if (!currentActor.IsAlive || statusEffects.IsColdActionBlocked(currentActor)) return;
            foreach (var target in allies.LivingMembers)
                if (statusEffects.GetFrozenNecrosisStacks(target) == 2) main28HearthOffered.Add(target);
        }

        private void BeginMain28Hearth()
        {
            if (battleEnded || actionPlaying || choosingTarget || !opportunityOpen || currentActor == null
                || !currentActor.IsAlive || !currentActor.IsPlayerControlled || statusEffects.IsColdActionBlocked(currentActor)) return;
            var targets = allies.LivingMembers.Where(target => statusEffects.GetFrozenNecrosisStacks(target) > 0).ToArray();
            if (targets.Length == 0)
            {
                messageText.text = "동결괴사가 쌓인 생존 아군이 없습니다.";
                return;
            }
            BeginTargetSelection(targets, target =>
            {
                if (!target.IsAlive || !statusEffects.RemoveFrozenNecrosis(target))
                {
                    SetCommandButtons(true);
                    RefreshCombatantViews(null);
                    return;
                }
                messageText.text = $"{target.DisplayName}의 동결괴사 중첩을 모두 제거했습니다.";
                FinishCurrentAction();
            }, "화로 사용 · 동결괴사를 제거할 생존 아군을 선택하세요.");
        }

        private IEnumerator PlayMain28Action(Combatant boss)
        {
            if (battleEnded || boss == null || !boss.IsAlive) yield break;
            actionPlaying = true;
            // 예고를 실행할 차례에는 페이즈가 바뀌어도 대상과 예고된 기술을 유지합니다.
            if (!main28PhaseTwo && boss.CurrentHp <= 900 && main28Action != 1)
            {
                main28PhaseTwo = true;
                main28Action = 0;
            }

            int action = main28Action;
            if (action == 1 && main28MarkedTarget != null && main28MarkedTarget.IsAlive
                && statusEffects.GetFrozenNecrosisStacks(main28MarkedTarget) == 2
                && !main28HearthOffered.Contains(main28MarkedTarget))
            {
                // 실제 명령 선택 기회가 열리기 전에는 같은 예고를 보존하고 보스 행동을 소모합니다.
                messageText.text = "화로로 대응할 기회가 생길 때까지 폭군이 냉기를 머금습니다.";
                FinishCurrentAction();
                yield break;
            }

            main28Action = (action + 1) % 4;
            if (action == 0)
            {
                var choices = TargetResolver.ResolveHostileTargets(boss, allies, TargetRangeType.Magic);
                var forced = boss.ForcedTargetActionsRemaining > 0 ? boss.ForcedTarget : null;
                main28MarkedTarget = forced != null && choices.Contains(forced) ? forced
                    : choices.OrderByDescending(statusEffects.GetFrozenNecrosisStacks).FirstOrDefault();
                messageText.text = main28PhaseTwo ? "폭군의 응시!" : "깊은 들숨!";
                RefreshCombatantViews(null);
                FinishCurrentAction();
                yield break;
            }

            if (action == 1)
            {
                var target = main28MarkedTarget;
                main28MarkedTarget = null;
                if (target == null || !target.IsAlive)
                {
                    var fallback = ChooseEnemyTarget(boss, TargetResolver.ResolveHostileTargets(boss, allies, TargetRangeType.MeleePhysical));
                    if (fallback != null) PlayBasicAttack(boss, fallback);
                    else FinishCurrentAction();
                    yield break;
                }
                messageText.text = main28PhaseTwo ? "마지막 서리!" : "설빙의 숨결!";
                RefreshCombatantViews(null);
                yield return PlayMain28Breath(boss, target);
                if (target.IsAlive)
                {
                    var fx = MakeMain28TargetVfx(target, 92, new Vector2(.68f, .28f), true, true, -16, .62f);
                    StartCoroutine(PlayMain28Vfx(fx, main28Hit, 1, 1));
                    statusEffects.ApplyFrozenNecrosis(boss, target);
                }
                RefreshCombatantViews(null);
                FinishCurrentAction();
                yield break;
            }

            if (action == 2)
            {
                var target = ChooseEnemyTarget(boss, TargetResolver.ResolveHostileTargets(boss, allies, TargetRangeType.MeleePhysical));
                if (target != null) PlayBasicAttack(boss, target, main28PhaseTwo ? 140 : 120, "얼음 발톱");
                else FinishCurrentAction();
                yield break;
            }

            bool area = !main28PhaseTwo;
            Combatant[] targets;
            if (area) targets = allies.LivingMembers.ToArray();
            else
            {
                var target = ChooseEnemyTarget(boss, TargetResolver.ResolveHostileTargets(boss, allies, TargetRangeType.Magic));
                targets = target == null ? System.Array.Empty<Combatant>() : new[] { target };
            }
            messageText.text = area ? "눈보라!" : "침묵하는 설원!";
            int percent = area ? 65 : 80;
            int raw = statusEffects.ModifyOutgoingDamage(boss, (boss.Attack * percent + 99) / 100);
            foreach (var target in targets)
            {
                int damage = pathTraits.ApplyDirectDamage(statusEffects, boss, target, raw, areaAttack: area);
                if (damage > 0) ShowMain28DamageVfx(target);
                if (!area && target.IsAlive && allies.LivingMembers.Count() > 1
                    && !allies.LivingMembers.Any(member => statusEffects.IsColdActionBlocked(member)
                        || statusEffects.GetFrozenNecrosisStacks(member) > 0))
                    statusEffects.TryApplyChill(target);
            }
            RefreshCombatantViews(null);
            yield return new WaitForSeconds(.5f);
            FinishCurrentAction();
        }

        private void RefreshMain28Presentation()
        {
            string tell = main28MarkedTarget != null && main28MarkedTarget.IsAlive
                ? $"{main28MarkedTarget.DisplayName} 대상 · 다음 폭군 행동: {(main28PhaseTwo ? "마지막 서리" : "설빙의 숨결")}" : string.Empty;
            bossTelegraphText.text = tell;
            bossTelegraphText.gameObject.SetActive(!battleEnded && tell.Length > 0);
            if (battleEnded)
            {
                foreach (var overlay in main28ColdOverlays.Values) overlay.gameObject.SetActive(false);
                return;
            }
            foreach (var target in allies.Members)
            {
                string id = statusEffects.GetFrozenNecrosisStacks(target) > 0 ? "cold.necrosis"
                    : statusEffects.GetFrostbiteRemaining(target) > 0 ? "cold.frostbite"
                    : statusEffects.GetChillRemaining(target) > 0 ? "cold.chill" : null;
                if (!main28ColdOverlays.TryGetValue(target, out var image) && id != null)
                {
                    image = MakeMain28TargetVfx(target, 128, new Vector2(.5f, .2f), true, true, 0, 1);
                    main28ColdOverlays.Add(target, image);
                }
                if (image == null) continue;
                image.gameObject.SetActive(id != null);
                if (id != null) image.sprite = BattleApprovedColdAssets.Vfx(id);
            }
        }

        private void HandleMain28ColdChange(Combatant target, ColdStatusKind kind, ColdStatusChange change, int value)
        {
            if (kind != ColdStatusKind.FrozenNecrosis) return;
            if (change == ColdStatusChange.Removed || change == ColdStatusChange.KnockedOut)
                main28HearthOffered.Remove(target);
            if (change == ColdStatusChange.KnockedOut && value == 3 && combatantViews.ContainsKey(target))
            {
                var fx = MakeMain28TargetVfx(target, 144, new Vector2(.5f, .12f), true, true, 0, 1);
                fx.name = "Main28NecrosisKO";
                StartCoroutine(PlayMain28Vfx(fx, main28Knockout, 0, 8));
            }
        }

        private void ShowMain28DamageVfx(Combatant target)
        {
            var fx = MakeMain28TargetVfx(target, 112, new Vector2(.68f, .48f), false, false, -10, .74f);
            StartCoroutine(PlayMain28Vfx(fx, main28Hit, 0, 8));
        }

        private Image MakeMain28TargetVfx(Combatant target, int size, Vector2 pivot, bool foot, bool behind, int offset, float opacity)
        {
            var view = combatantViews[target];
            Vector2 anchor = Main28SpritePoint(view.SpriteImage, new Vector2(.5f, foot ? .12f : .49f));
            var image = MakeMain28Vfx(behind ? view.ActionRoot.GetSiblingIndex() : battlefieldTransform.childCount,
                pivot, anchor + Vector2.up * offset, Vector2.one * size, opacity);
            return image;
        }

        private Vector2 Main28SpritePoint(Image body, Vector2 normalized)
        {
            Rect rect = body.rectTransform.rect;
            float square = Mathf.Min(rect.width, rect.height);
            return battlefieldTransform.InverseTransformPoint(body.rectTransform.TransformPoint(
                rect.center + (normalized - Vector2.one * .5f) * square));
        }

        private IEnumerator PlayMain28Breath(Combatant boss, Combatant target)
        {
            var body = combatantViews[boss].SpriteImage;
            var victim = combatantViews[target].SpriteImage;
            Vector2 mouth = Main28SpritePoint(body, new Vector2(374f / 512f, 1f - 211f / 512f));
            Vector2 destination = Main28SpritePoint(victim, new Vector2(.5f, .53f));
            Vector2 direction = destination - mouth;
            var image = MakeMain28Vfx(battlefieldTransform.childCount, new Vector2(.05f,.47f), mouth,
                new Vector2(direction.magnitude * 1.08f, 230), 1);
            image.rectTransform.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            yield return PlayMain28Vfx(image, main28Breath, 0, 8);
        }

        private Image MakeMain28Vfx(int index, Vector2 pivot, Vector2 position, Vector2 size, float opacity)
        {
            var image = MakeImage(battlefieldTransform, "Main28ColdVfx", new Color(1, 1, 1, opacity));
            var rect = image.rectTransform;
            var field = (RectTransform)battlefieldTransform;
            SetRect(rect, field.pivot, size);
            rect.pivot = pivot;
            rect.anchoredPosition = position;
            rect.SetSiblingIndex(index);
            image.raycastTarget = false;
            return image;
        }

        private static IEnumerator PlayMain28Vfx(Image image, Sprite[] frames, int first, int count)
        {
            for (int i = first; i < Mathf.Min(first + count, frames.Length); i++)
            {
                image.sprite = frames[i];
                yield return new WaitForSeconds(.125f);
            }
            Destroy(image.gameObject);
        }

        private void DestroyMain28Frames()
        {
            foreach (var frames in new[] { main28Breath, main28Hit, main28Knockout })
            {
                if (frames == null) continue;
                foreach (var frame in frames) Destroy(frame);
            }
        }

        private static Sprite[] LoadMain28Vfx(string name)
        {
            var sheet = Resources.Load<Texture2D>("Main28FrostTyrant/" + name);
            if (sheet == null) return System.Array.Empty<Sprite>();
            return Enumerable.Range(0, 8).Select(i => Sprite.Create(sheet,
                new Rect(i % 4 * 256, (1 - i / 4) * 256, 256, 256), Vector2.one * .5f, 256, 0, SpriteMeshType.FullRect)).ToArray();
        }
    }
}
