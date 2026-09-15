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
        [SerializeField] private Color iconTint = Color.white;
        [SerializeField] private ItemUseType useType;
        [SerializeField] private string effectPreview = string.Empty;
        public string ItemId => itemId;
        public string DisplayName => displayName;
        public string Description => description;
        public ItemCategory Category => category;
        public int MaxStack => maxStack;
        public int BuyPrice => buyPrice;
        public int SellPrice => sellPrice;
        public Sprite Icon => icon;
        public Color IconTint => iconTint;
        public ItemUseType UseType => useType;
        public string EffectPreview => effectPreview;
#if UNITY_EDITOR
        public void ConfigureForAudit(string id, int stack = 99) { itemId = id; maxStack = stack; }
        public void ConfigureContent(string id, string name, string details, ItemCategory itemCategory,
            int stack, int purchasePrice, int resalePrice, Sprite itemIcon, Color tint,
            ItemUseType itemUseType, string preview)
        {
            itemId = id; displayName = name; description = details; category = itemCategory;
            maxStack = stack; buyPrice = purchasePrice; sellPrice = resalePrice;
            icon = itemIcon; iconTint = tint; useType = itemUseType; effectPreview = preview;
        }
#endif
    }
}
