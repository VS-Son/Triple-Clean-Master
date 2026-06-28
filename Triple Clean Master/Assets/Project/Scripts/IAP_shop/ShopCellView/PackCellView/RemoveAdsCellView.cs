using System;
using EnhancedUI.EnhancedScroller;
using Project.Scripts.IAP_shop.ShopData;
using Project.Scripts.Manager;
using TMPro;
using UnityEngine.UI;

namespace Project.Scripts.IAP_shop.ShopCellView.PackCellView
{
    public class RemoveAdsCellView : EnhancedScrollerCellView,IShopCellView
    {
        public string id;
        public TMP_Text textName;
        public TMP_Text textDescription;
        public Image iconAds;
        public TMP_Text priceText;
        public bool isPurchased;
        private RemoveAdsData _data; 
        public Action<IShopCellView> Onclick { get; set; }
        public void OnClickPurchase()
        {
            Onclick.Invoke(this);
        }

        public void OnPurchasing(ShopItemDataBase database)
        {
            if (database is RemoveAdsData removeAds)
            {
                if (!id.Equals(removeAds.id))return;
                if (!isPurchased)
                {
                    isPurchased = true;
                    PlayerInventoryManager.AddCoin(removeAds.coin);
                    removeAds.isPurchase = isPurchased;
                }
            }
        }

        public void SetData(ShopItemDataBase dataBase, int index)
        {
            if (dataBase is RemoveAdsData data)
            {
                _data = data;
                id = data.id;
                textName.text = data.name;
                textDescription.text = data.description;
                iconAds.sprite = data.icon;
                isPurchased = data.isPurchase;
                priceText.text =!isPurchased? $"{data.price:N0}₫" : "Purchased";

            }
        }

        public ShopItemDataBase Data()
        {
            return _data;
        }
    }
}
