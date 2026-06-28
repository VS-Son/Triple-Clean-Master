using System;
using EnhancedUI.EnhancedScroller;
using Project.Scripts.Effect;
using Project.Scripts.UI.Screen;
using Project.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI.Screen.Shop
{
    public class PackageCellView : EnhancedScrollerCellView, IShopCellView
    {
        public Action<IShopCellView> Onclick { get; set; }
        public string id;
        public TMP_Text textName;
        public Image backgound;
        public Image iconCoin;
        public TMP_Text textCoinAmount;

        [Header("Booster")]
        public TMP_Text textUndoAmount;
        public TMP_Text textMagicWandAmount;
        public TMP_Text textShuffleAmount;
        
        public TMP_Text priceText;
        public bool isPurchased;
        private PackageData _data;

       
        public void OnClickPurchase()
        {
            Onclick.Invoke(this);
        }

        public void OnPurchasing(ShopItemDataBase database)
        {
            if (database is PackageData packageData)
            {
                if (!id.Equals(packageData.id))return;
                if (!isPurchased)
                {
                    isPurchased = true;
                    PlayerInventoryManager.AddBoosters(packageData.undo, packageData.magicWand, packageData.shuffle);
                    PlayerInventoryManager.AddCoin(packageData.coin);
                    UIManager.GetUI<PlayScreen>(TypeScreen.PlayScreen).UpdateTextBoosters();
                    packageData.isPurchase = isPurchased;
                }
            }
        }

        public void SetData(ShopItemDataBase dataBase, int index)
        {
            if (dataBase is PackageData data)
            {
                _data = data;
                id = data.id;
                textName.text = data.name;
                backgound.sprite = data.backgound;
                isPurchased = data.isPurchase;
                priceText.text = !isPurchased ? $"{data.price:N0}₫" : "Purchased";
                iconCoin.sprite = data.icon;
                textCoinAmount.text = $"{data.coin}";

                textUndoAmount.text = $"x{data.undo}";
                textMagicWandAmount.text = $"x{data.magicWand}";
                textShuffleAmount.text = $"x{data.shuffle}";

            }
        }
        public PackageData Data()
        {
            return _data;
        }

       
    }
}
