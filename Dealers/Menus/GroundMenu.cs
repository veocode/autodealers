using System.Drawing;
using LemonUI.Menus;


namespace VeoAutoMod.Dealers.Menus
{
    class GroundMenu : Menu
    {
        private NativeItem sitInCarText;

        public GroundMenu(Dealer dealer) : base(dealer) { }

        public override void CreateItems() {
            nativeMenu.AcceptsInput = false;
            nativeMenu.DisableControls = true;
            nativeMenu.UseMouse = false;

            sitInCarText = new NativeItem("") { 
                Enabled = false,
                Colors = { TitleDisabled = Color.Black }
            };

            items.Add(sitInCarText);
        }

        protected override void BeforeShow() {
            string inTheVehicle = VehicleService.GetSitText(vehicle);

            sitInCarText.Title = $"Sit {inTheVehicle} to buy or test drive it";
        }
    }
}
