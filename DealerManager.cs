using System;
using GTA;
using GTA.UI;
using VeoAutoMod.Dealers;
using VeoAutoMod.Menus;


namespace VeoAutoMod
{
    static class DealerManager
    {
        private static WelcomeMenu welcomeMenu;
        private static SelectDealerMenu selectDealerMenu;

        public static void Create()
        {
            welcomeMenu = new WelcomeMenu();
            selectDealerMenu = new SelectDealerMenu();
        }

        public static void ToggleMenu()
        {
            selectDealerMenu.Hide();

            welcomeMenu.Toggle();
        }

        public static void AddCurrentVehicleForSale()
        {
            bool isInVehicle = Game.Player.Character.IsSittingInVehicle();

            if (!isInVehicle)
            {
                Notification.Show("~r~You're not in the vehicle\n~w~Sit in the vehicle you want to add");
                return;
            }
            
            selectDealerMenu.Show();
        }

        public static void AddCurrentVehicleToDealer(Dealer dealer)
        {
            selectDealerMenu.Hide();

            bool isInVehicle = Game.Player.Character.IsSittingInVehicle();
            if (!isInVehicle) return;

            Vehicle vehicle = Game.Player.Character.CurrentVehicle;

            string name;
            int price, id;

            try
            {
                name = Game.GetUserInput(vehicle.DisplayName);
                if (name == "")
                {
                    Notification.Show("~r~Vehicle not added:~w~\nName is required");
                    return;
                }

                price = int.Parse(Game.GetUserInput("100000"));
                id = vehicle.Model.GetHashCode();

                dealer.AddVehicleForSale(id, name, price);
                Dumper.DumpDealerToFile(dealer, dealer.GetConfig().FilePath);
            }
            catch (FormatException)
            {
                Notification.Show("~r~Vehicle not added:~w~\nPrice should only contain numbers");
                return;
            }
            catch (Exception)
            {
                Notification.Show("~r~Vehicle not added:~w~\nSomething went wrong");
                return;
            }

            Notification.Show(
                "~g~Vehicle added!\n" + 
                "~b~Name:~w~ " + name + "\n" + 
                "~b~Price:~w~ $" + price.ToString() + "\n" +
                "~b~Dealer:~w~ " + dealer.GetName()
            );            
        }

    }
}
