using UnityEngine;

namespace ProjectLimitless.Core
{
    public enum ItemCategory { Consumable, Equipment, Material, Quest, KeyItem }
    public enum ItemUseType { None, World, Battle, WorldAndBattle }

    /// <summary>표시 이름이 바뀌어도 저장이 깨지지 않도록 안정적인 ItemId와 화면용 문구를 분리합니다.</summary>
    [CreateAssetMenu(menuName = "Project Limitless/Item Definition")]
    public sealed class ItemDefinition : ScriptableObject
    {
        [SerializeField] private string itemId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [TextArea, SerializeField] private string description = string.Empty;
        [SerializeField] private ItemCategory category;
        [SerializeField, Min(1)] private int maxStack = 99;
        [SerializeField, Min(0)] private int buyPrice;
        [SerializeField, Min(0)] private int sellPrice;
        [SerializeField] private Sprite icon;
        [SerializeField] private ItemUseType useType;
        public string ItemId => itemId;
        public string DisplayName => displayName;
        public string Description => description;
        public ItemCategory Category => category;
        public int MaxStack => maxStack;
        public int BuyPrice => buyPrice;
        public int SellPrice => sellPrice;
        public Sprite Icon => icon;
        public ItemUseType UseType => useType;
#if UNITY_EDITOR
        public void ConfigureForAudit(string id, int stack = 99) { itemId = id; maxStack = stack; }
#endif
    }
}
