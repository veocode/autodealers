using GTA;
using LemonUI.Menus;


namespace VeoAutoMod.Dealers.Menus
{
    class PlateMenu : Menu
    {
        public PlateMenu(Dealer dealer) : base(dealer) { }

        public override void CreateItems()
        {
            string platePrompt = Game.Player.Name;

            NativeItem noCustomPlate = new NativeItem("No, thanks");
            noCustomPlate.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                dealer.BuyCurrentVehicle();
            };

            NativeItem blackPlate = new NativeItem("Black background");
            blackPlate.Description = "$450";
            blackPlate.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                string plateText = Game.GetUserInput(platePrompt);
                Game.Player.Money -= 450;
                dealer.BuyCurrentVehicle(plateText, LicensePlateStyle.YellowOnBlack);
            };

            NativeItem bluePlate = new NativeItem("Blue background");
            bluePlate.Description = "$300";
            bluePlate.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                string plateText = Game.GetUserInput(platePrompt);
                Game.Player.Money -= 300;
                dealer.BuyCurrentVehicle(plateText, LicensePlateStyle.YellowOnBlue);
            };

            NativeItem whitePlate = new NativeItem("White background");
            whitePlate.Description = "$250";
            whitePlate.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                string plateText = Game.GetUserInput(platePrompt);
                Game.Player.Money -= 250;
                dealer.BuyCurrentVehicle(plateText, LicensePlateStyle.BlueOnWhite1);
            };

            items.Add(noCustomPlate);
            items.Add(blackPlate);
            items.Add(bluePlate);
            items.Add(whitePlate);
        }

        protected override void BeforeShow()
        {
        }
    }
}
