using GTA;
using LemonUI.Menus;


namespace VeoAutoMod.Dealers.Menus
{
    class VehicleMenu : Menu
    {
        public VehicleMenu(Dealer dealer) : base(dealer) { }

        public override void CreateItems() 
        {
            nativeMenu.UseMouse = false;

            NativeItem buy = new NativeItem("Buy this vehicle");
            buy.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                dealer.SelectCurrentVehiclePlate();
            };            
            
            NativeItem openDoors = new NativeItem("Open all doors");
            openDoors.Activated += (object sender, System.EventArgs e) =>
            {
                foreach (VehicleDoor door in dealer.GetCurrentVehicle().Doors) door.Open();
            };

            NativeItem closeDoors = new NativeItem("Close all doors");
            closeDoors.Activated += (object sender, System.EventArgs e) =>
            {
                foreach (VehicleDoor door in dealer.GetCurrentVehicle().Doors) door.Close();
            };

            NativeItem testDrive = new NativeItem("Test Drive");
            testDrive.Activated += (object sender, System.EventArgs e) =>
            {
                Hide();
                dealer.TestDriveCurrentVehicle();
            };

            NativeItem nextSeat = new NativeItem("Shuffle to next seat");
            nextSeat.Activated += (object sender, System.EventArgs e) =>
            {
                Game.Player.Character.Task.ShuffleToNextVehicleSeat(dealer.GetCurrentVehicle());
            };

            NativeItem backSeat = new NativeItem("Sit to back seat");
            backSeat.Activated += (object sender, System.EventArgs e) =>
            {
                Game.Player.Character.Task.EnterVehicle(dealer.GetCurrentVehicle(), VehicleSeat.RightRear);
            };

            NativeItem leave = new NativeItem("Leave vehicle");
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
    }
}
