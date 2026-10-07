using ProjectLimitless.Core;

namespace ProjectLimitless.Battle
{
    /// <summary>전투 화면과 분리된 소비 아이템 판정입니다. 대상 조건과 실제 효과를 확인한 뒤 공용 수량을 줄입니다.</summary>
    public static class BattleItemUseService
    {
        public static bool TryUse(ItemDefinition item, Combatant target, BattleStatusEffectRuntime statuses,
            out string message)
        {
            message = "아이템이나 대상이 유효하지 않습니다.";
            if (item == null || target == null || statuses == null || !target.IsAlive
                || target.Side != BattleSide.Allies || item.TargetType != ItemTargetType.LivingAllySingle
                || item.Category != ItemCategory.Consumable
                || (item.UseType != ItemUseType.Battle && item.UseType != ItemUseType.WorldAndBattle)
                || !InventoryService.HasItem(item.ItemId, 1)) return false;

            int applied = 0;
            message = "제거할 상태이상이 없습니다.";
            switch (item.EffectType)
            {
                case ItemEffectType.RecoverHp:
                    message = "HP가 이미 최대입니다.";
                    if (target.CurrentHp < target.MaxHp) applied = target.RecoverHp(item.EffectAmount);
                    break;
                case ItemEffectType.RecoverMp:
                    message = target.UsesMp ? "MP가 이미 최대입니다." : "MP를 사용하지 않는 대상입니다.";
                    if (target.UsesMp && target.CurrentMp < target.MaxMp) applied = target.RecoverMp(item.EffectAmount);
                    break;
                case ItemEffectType.RemovePoison:
                    applied = statuses.RemoveHarmfulStatus(target, HarmfulStatusType.Poison) ? 1 : 0; break;
                case ItemEffectType.RemoveBurn:
                    applied = statuses.RemoveHarmfulStatus(target, HarmfulStatusType.Burn) ? 1 : 0; break;
                case ItemEffectType.RemoveShock:
                    applied = statuses.RemoveHarmfulStatus(target, HarmfulStatusType.Shock) ? 1 : 0; break;
                case ItemEffectType.RemoveSilence:
                    applied = statuses.RemoveHarmfulStatus(target, HarmfulStatusType.Silence) ? 1 : 0; break;
                case ItemEffectType.RemoveOverheat:
                    // 과열은 정화 목록과 독립적입니다. 실제 제거가 없는 경우 소비와 행동 성공을 반환하지 않습니다.
                    message = "제거할 과열이 없습니다.";
                    applied = statuses.RemoveOverheat(target) ? 1 : 0; break;
                default:
                    message = "사용할 수 없는 아이템 효과입니다."; break;
            }
            if (applied <= 0) return false;
            // 전투는 Unity 메인 스레드에서 순서대로 처리합니다. 효과 성공 직후 소비 실패는 중복 지급이므로 숨기지 않습니다.
            if (!InventoryService.TryRemoveItem(item.ItemId, 1))
                throw new System.InvalidOperationException("전투 아이템 효과 적용 뒤 소비에 실패했습니다.");
            message = $"{target.DisplayName}에게 {item.DisplayName} 사용!";
            return true;
        }
    }
}
