using System;
using Project.Scripts.IAP_shop.ShopData;

namespace Project.Scripts.IAP_shop.ShopCellView
{
    public interface IShopCellView
    {
        Action<IShopCellView> Onclick { get; set; }
        public void OnClickPurchase();
        public void OnPurchasing(ShopItemDataBase database);
        public void SetData(ShopItemDataBase dataBase, int index);
    }
}
