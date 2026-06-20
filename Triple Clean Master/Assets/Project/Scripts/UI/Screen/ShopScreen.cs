namespace UI.Screen.Shop
{
    public class ShopScreen : UICanvas
    {    private ShopOpenSource _source;

        public void OpenFrom(ShopOpenSource source)
        {
            _source = source;
        }
        public void OnBack()
        {
            Close();
        }
    }
}
