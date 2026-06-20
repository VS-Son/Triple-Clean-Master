using System;
using UnityEngine;

namespace UI.Screen.Shop
{
    public enum ShopItemType
    {
        Package,
        Coin,
        Ads
    }

    public enum OfferType
    {
        Buy,
        Free,
        Ads
    }

    public enum PurchaseType
    {
        Consumable,   
        NonConsumable 
    }
    [Serializable]
    public abstract class ShopItemDataBase
    {
        public string id;
        public string name;
        public Sprite icon;
        public int coin;
        public int price;
        public bool isPurchase;
        public abstract ShopItemType ItemType {get;}
        public abstract PurchaseType PurchaseType { get;}
    }
}
