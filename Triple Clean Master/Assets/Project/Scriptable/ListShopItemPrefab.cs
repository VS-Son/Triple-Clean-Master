using System;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Project.Scripts.IAP_shop.ShopData;
using UnityEngine;

namespace Project.Scriptable
{
    [Serializable]
    public class ShopItem
    {
        public ShopItemType shopItemType;
        public EnhancedScrollerCellView item;
    }
    [Serializable]
    [CreateAssetMenu(fileName = "List Item", menuName = "IAP_Shop/Shop item")]
    public class ListShopItemPrefab : ScriptableObject
    {
        public List<ShopItem> items = new();
    }
}
