using LemonUI.Menus;


namespace VeoAutoMod.Dealers.Menus
{
    class GroundMenu : Menu
    {
        public GroundMenu(Dealer dealer) : base(dealer) { }

        public override void CreateItems() {
            nativeMenu.AcceptsInput = false;
            nativeMenu.DisableControls = true;
            nativeMenu.UseMouse = false;

            NativeItem sitInCarText = new NativeItem("Sit in the vehicle to buy or test drive it");
            sitInCarText.Enabled = false;

            items.Add(sitInCarText);
        }
    }
}
