using System;
using System.Linq;
using System.Reflection;
using ProjectLimitless.Battle;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectLimitless.EditorTools
{
    /// <summary>
    /// Scene이나 저장 슬롯을 열지 않고 공용 침묵 상태와 정화의 행동 경계를 확인합니다.
    /// 실제 전투 명령 버튼의 포커스와 안내는 별도 Play Mode 확인 대상으로 둡니다.
    /// </summary>
    public static class BattleSilenceAudit
    {
        public static string Run()
        {
            var status = new BattleStatusEffectRuntime();
            var cooldowns = new BattleSkillCooldowns();
            var momentum = new BattleFighterResourceRuntime();
            var executor = new BattleSkillExecutor(cooldowns, status, momentum);
            var actor = NewCombatant("actor", BattleSide.Allies);
            var healer = NewCombatant("healer", BattleSide.Allies);
            var enemy = NewCombatant("enemy", BattleSide.Enemies);
            var skill = new BattleSkillDefinition("audit_skill", "검증 스킬", "", true,
                BattleSkillEffectType.SingleAllyHeal, 0, 0);
            var cleanse = new BattleSkillDefinition("audit_cleanse", "정화", "", true,
                BattleSkillEffectType.RemoveAllHarmfulStatuses, 2, 0, mpCost: 5);

            Check(executor.CanUse(actor, skill, out _), "정상 캐릭터 스킬 사용 가능");
            status.ApplyOrRefreshSilence(actor);
            Check(status.HasSilence(actor) && !executor.CanUse(actor, skill, out string blockedReason)
                && blockedReason.Contains("침묵") && actor.CurrentMp == actor.MaxMp,
                "침묵 스킬 차단과 MP 미소비");
            Check(!executor.ExecuteSingleAllyCleanse(actor, actor, cleanse, out _, out string cleanseReason)
                && cleanseReason.Contains("침묵") && status.HasSilence(actor),
                "침묵 치유사 자기 정화 차단");
            status.ApplyOrRefreshSilence(actor);
            Check(status.GetHarmfulStatuses(actor).Count(type => type == HarmfulStatusType.Silence) == 1,
                "재적용해도 침묵 중첩 없음");
            status.CompleteActorAction(enemy);
            Check(status.HasSilence(actor), "다른 참가자 행동에는 침묵 유지");
            actor.Defend();
            actor.CompleteAction();
            status.CompleteActorAction(actor);
            Check(!status.HasSilence(actor) && actor.IsDefending && executor.CanUse(actor, skill, out _),
                "방어 행동 완료 후 침묵 제거와 스킬 복귀");

            status.ApplyOrRefreshPoison(actor, 3);
            status.ApplyOrRefreshBurn(actor, 3, 2);
            status.ApplyOrRefreshShock(actor);
            status.ApplyOrRefreshSilence(actor);
            status.ApplyGaiaWall(actor, 2);
            status.ApplyIronWall(actor, 2);
            status.ApplyGuardianCover(actor);
            actor.ApplyTaunt(enemy, 2);
            momentum.AddMomentum(actor, 2);
            cooldowns.Start(actor, skill.Id, 3);
            int healerMp = healer.CurrentMp;
            Check(status.GetHarmfulStatuses(actor).Count == 4,
                "독·화상·감전·침묵 공용 해로운 상태 조회");
            Check(executor.ExecuteSingleAllyCleanse(healer, actor, cleanse, out int removed, out _)
                && removed == 4 && status.GetHarmfulStatuses(actor).Count == 0 && healer.CurrentMp == healerMp - 5,
                "정화 1회로 네 해로운 상태 제거");
            Check(status.GetGaiaWallRemaining(actor) == 2 && status.GetIronWallRemaining(actor) == 2
                && status.HasGuardianCover(actor) && actor.ForcedTargetActionsRemaining == 2
                && actor.IsDefending && momentum.GetMomentum(actor) == 2
                && cooldowns.GetRemaining(actor, skill.Id) == 3,
                "정화가 방어·도발·버프·기세·쿨타임을 보존");
            Check(!executor.ExecuteSingleAllyCleanse(healer, actor, cleanse, out _, out string emptyReason)
                && emptyReason.Contains("정화할") && healer.CurrentMp == healerMp - 5
                && cooldowns.GetRemaining(healer, cleanse.Id) == 0,
                "해로운 상태 없는 정화는 MP·쿨타임 미소비");

            status.ApplyOrRefreshSilence(actor);
            var model = BattleCombatantStatusViewModelFactory.Create(actor, null, null, cooldowns, momentum, status);
            Check(model.Markers.Any(marker => marker.Id == "silence" && marker.DisplayText == "침묵 1")
                && model.DetailText.Contains("다음 행동에서 스킬을 사용할 수 없습니다."),
                "침묵 HUD 텍스트와 상세 설명");
            actor.TakeDamage(actor.MaxHp, applyDefending: false);
            status.RemoveInvalidPersistentEffects(new[] { actor, healer, enemy });
            Check(!status.HasSilence(actor) && !BattleCombatantStatusViewModelFactory.Create(
                actor, null, null, cooldowns, momentum, status).Markers.Any(),
                "전투불능 침묵 정리");
            Check(!new BattleStatusEffectRuntime().HasSilence(healer), "새 전투 런타임에 침묵 잔존 없음");
            AuditCommandGate();
            return "BATTLE_SILENCE_AUDIT ALL PASS";
        }

        /// <summary>
        /// Editor 화면을 열지 않고 명령 버튼의 실제 클릭 경계를 호출합니다. 테스트 오브젝트는 Scene과
        /// 저장 대상에서 제외하고 즉시 폐기하므로 사용자가 편집 중인 장면에는 남지 않습니다.
        /// </summary>
        private static void AuditCommandGate()
        {
            var root = new GameObject("SilenceCommandAudit") { hideFlags = HideFlags.HideAndDontSave };
            var messageObject = new GameObject("SilenceMessageAudit", typeof(RectTransform),
                typeof(CanvasRenderer), typeof(Text)) { hideFlags = HideFlags.HideAndDontSave };
            try
            {
                var controller = root.AddComponent<BattleSceneController>();
                var message = messageObject.GetComponent<Text>();
                var actor = NewCombatant("command_actor", BattleSide.Allies);
                Type type = typeof(BattleSceneController);
                BindingFlags fields = BindingFlags.NonPublic | BindingFlags.Instance;
                type.GetField("currentActor", fields).SetValue(controller, actor);
                type.GetField("messageText", fields).SetValue(controller, message);
                var status = (BattleStatusEffectRuntime)type.GetField("statusEffects", fields).GetValue(controller);
                status.ApplyOrRefreshSilence(actor);
                type.GetMethod("ShowSkillMenu", fields).Invoke(controller, null);
                bool choosingSkill = (bool)type.GetField("choosingSkill", fields).GetValue(controller);
                Check(!choosingSkill && message.text.Contains("침묵") && status.HasSilence(actor)
                    && actor.CurrentMp == actor.MaxMp,
                    "침묵 Skill 명령 안내와 행동·MP 미소비");
            }
            finally
            {
                UnityEngine.Object.DestroyImmediate(messageObject);
                UnityEngine.Object.DestroyImmediate(root);
            }
        }

        private static Combatant NewCombatant(string id, BattleSide side) =>
            new Combatant(id, id, side, new FormationSlot(FormationRow.Front, 0),
                100, 10, 10, 0, TargetRangeType.Magic, true, maxMp: 30);

        private static void Check(bool condition, string description)
        {
            if (!condition) throw new InvalidOperationException("침묵 감사 실패: " + description);
        }
    }
}
