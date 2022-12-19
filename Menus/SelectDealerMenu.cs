using GTA.UI;
using LemonUI.Menus;
using VeoAutoMod.Dealers;


namespace VeoAutoMod.Menus
{
    class SelectDealerMenu : Objects.Menu
    {
        public SelectDealerMenu(string title = "AutoDealers V") : base(title, "Select Auto Dealer")
        {
            
        }

        public override void CreateItems()
        {
            foreach(Dealer dealer in World.GetDealers())
            {
                NativeItem item = new NativeItem(dealer.GetName());
                item.Activated += (object sender, System.EventArgs e) =>
                {
                    DealerManager.AddCurrentVehicleToDealer(dealer);                    
                };

                items.Add(item);
            }
        }
    }
}
