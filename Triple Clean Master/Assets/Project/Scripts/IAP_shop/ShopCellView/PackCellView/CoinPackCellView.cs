using System;
using EnhancedUI.EnhancedScroller;
using Project.Scripts.Effect;
using TMPro;
using UnityEngine.UI;

namespace UI.Screen.Shop
{
    public class CoinPackCellView : EnhancedScrollerCellView,IShopCellView
    {
        public string id;
        public Image iconCoin;
        public TMP_Text textAmountCoin;
        public OfferType offerType;
        public TMP_Text priceText;
        public bool isPurchased;
        private CoinPackData _data;
        public Action<IShopCellView> Onclick { get; set; }

        public void OnClickPurchase()
        {
            Onclick.Invoke(this);
        }

        public void OnPurchasing(ShopItemDataBase database)
        {
            if (database is CoinPackData coinPackData)
            {
                if (!id.Equals(coinPackData.id)) return;
                if(!isPurchased)
                {
                    if (coinPackData.offerType == OfferType.Free || coinPackData.offerType == OfferType.Ads)
                    {
                        isPurchased = true;
                        PlayerInventoryManager.AddCoin(coinPackData.coin);
                        coinPackData.isPurchase = isPurchased;
                    }
                }
                if (coinPackData.offerType == OfferType.Buy)
                {
                    PlayerInventoryManager.AddCoin(coinPackData.coin);
                }
                
            }
        }

        public void SetData(ShopItemDataBase dataBase, int index)
        {
            if (dataBase is CoinPackData data)
            {
                _data = data;
                id = data.id;
                iconCoin.sprite = data.icon;
                textAmountCoin.text = $"{data.coin}";
                offerType = data.offerType;
                priceText.text =offerType == OfferType.Buy?$"{data.price:N0}₫": "Free";
                isPurchased = data.isPurchase;
            }
        }

        public ShopItemDataBase Data()
        {
            return _data;
        }
    }
}
