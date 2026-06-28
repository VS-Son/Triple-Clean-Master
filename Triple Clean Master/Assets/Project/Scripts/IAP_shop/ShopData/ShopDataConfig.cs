using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.IAP_shop.ShopData
{
    [Serializable]
    public class PackageData: ShopItemDataBase
    {
        public Sprite backgound;
        public int undo;
        public int magicWand;
        public int shuffle;
        
        public override ShopItemType ItemType => ShopItemType.Package;
        public override PurchaseType PurchaseType => PurchaseType.NonConsumable;
    }
    [Serializable]
    public class CoinPackData:ShopItemDataBase
    {
        public OfferType  offerType;
        public override ShopItemType ItemType => ShopItemType.Coin;
        public override PurchaseType PurchaseType => PurchaseType.Consumable;
    }
    [Serializable]
    public class RemoveAdsData:ShopItemDataBase
    {
        public string description;
        public override ShopItemType ItemType => ShopItemType.Ads;
        public override PurchaseType PurchaseType => PurchaseType.NonConsumable;
    }

    [Serializable]
    [CreateAssetMenu(fileName = "Shop Data Config", menuName = "IAP_Shop/Shop Config")]
    public class ShopDataConfig : ScriptableObject
    {
        [SerializeField]private List<PackageData> packageCellViews;
        [SerializeField]private List<CoinPackData> coinPackCellViews;
        [SerializeField]private List<RemoveAdsData> removeAdsCellViews;

        public List<ShopItemDataBase> GetRangData()
        {
            List<ShopItemDataBase> allItems = new();
            allItems.AddRange(packageCellViews);
            allItems.AddRange(coinPackCellViews);
            allItems.AddRange(removeAdsCellViews);
            return allItems;
        }
    }
}
