using LemonUI.Menus;


namespace VeoAutoMod.Menus
{
    class WelcomeMenu : Objects.Menu
    {
        public WelcomeMenu(string title = "AutoDealers V") : base(title, "")
        {
            
        }

        public override void CreateItems()
        {
            NativeItem addVehicle = new NativeItem("Add Vehicle for Sale");
            addVehicle.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                DealerManager.AddCurrentVehicleForSale();
            };

            items.Add(addVehicle);
        }
    }
}
