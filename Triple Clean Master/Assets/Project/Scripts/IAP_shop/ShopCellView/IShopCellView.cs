using System;

namespace UI.Screen.Shop
{
    public interface IShopCellView
    {
        Action<IShopCellView> Onclick { get; set; }
        public void OnClickPurchase();
        public void OnPurchasing(ShopItemDataBase database);
        public void SetData(ShopItemDataBase dataBase, int index);
    }
}
