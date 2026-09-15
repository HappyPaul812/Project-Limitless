using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectLimitless.Core
{
    /// <summary>
    /// 상점 UI와 판매 목록을 분리한 데이터입니다. 같은 UI를 다른 마을 상점에도 재사용하고,
    /// 판매 품목은 코드를 고치지 않고 Asset에서 바꿀 수 있게 합니다.
    /// </summary>
    [CreateAssetMenu(menuName = "Project Limitless/Shop Definition")]
    public sealed class ShopDefinition : ScriptableObject
    {
        [SerializeField] private string shopId = string.Empty;
        [SerializeField] private string displayName = string.Empty;
        [SerializeField] private string[] itemIds = Array.Empty<string>();

        public string ShopId => shopId;
        public string DisplayName => displayName;
        public IReadOnlyList<string> ItemIds => itemIds;

#if UNITY_EDITOR
        public void ConfigureContent(string id, string title, string[] products)
        {
            shopId = id;
            displayName = title;
            itemIds = products ?? Array.Empty<string>();
        }
#endif
    }
}
