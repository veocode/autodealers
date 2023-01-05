using GTA;
using LemonUI.Menus;


namespace VeoAutoMod.Dealers.Menus
{
    class VehicleMenu : Menu
    {
        private NativeItem buy;
        private NativeItem openDoors;
        private NativeItem closeDoors;
        private NativeItem testDrive;
        private NativeItem nextSeat;
        private NativeItem backSeat;
        private NativeItem leave;

        public VehicleMenu(Dealer dealer) : base(dealer) { }

        public override void CreateItems()
        {
            nativeMenu.UseMouse = false;

            buy = new NativeItem("Buy this vehicle");
            buy.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                dealer.SelectCurrentVehiclePlate();
            };

            openDoors = new NativeItem("Open all doors");
            openDoors.Activated += (object sender, System.EventArgs e) =>
            {
                foreach (VehicleDoor door in dealer.GetCurrentVehicle().Doors) door.Open();
            };

            closeDoors = new NativeItem("Close all doors");
            closeDoors.Activated += (object sender, System.EventArgs e) =>
            {
                foreach (VehicleDoor door in dealer.GetCurrentVehicle().Doors) door.Close();
            };

            testDrive = new NativeItem("Test Drive");
            testDrive.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                dealer.TestDriveCurrentVehicle();
            };

            nextSeat = new NativeItem("Shuffle to next seat");
            nextSeat.Activated += (object sender, System.EventArgs e) =>
            {
                Game.Player.Character.Task.ShuffleToNextVehicleSeat(dealer.GetCurrentVehicle());
            };

            backSeat = new NativeItem("Sit to back seat");
            backSeat.Activated += (object sender, System.EventArgs e) =>
            {
                Game.Player.Character.Task.EnterVehicle(dealer.GetCurrentVehicle(), VehicleSeat.RightRear);
            };

            leave = new NativeItem("Leave vehicle");
            leave.Activated += (object sender, System.EventArgs e) =>
            {
                Game.Player.Character.Task.LeaveVehicle();
            };

            items.Add(buy);
            items.Add(openDoors);
            items.Add(closeDoors);
            items.Add(testDrive);
            items.Add(nextSeat);
            items.Add(backSeat);
            items.Add(leave);
        }

        protected override void BeforeShow()
        {
            string vehicleClass = VehicleService.GetClassNameText(vehicle);

            buy.Title = $"Buy this {vehicleClass}";
            leave.Title = $"Leave {vehicleClass}";

            openDoors.Enabled = !VehicleService.IsBike(vehicle);
            closeDoors.Enabled = !VehicleService.IsBike(vehicle);
            nextSeat.Enabled = !VehicleService.IsBike(vehicle);
            backSeat.Enabled = VehicleService.IsCar(vehicle);
        }
    }
}
