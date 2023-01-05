using GTA;

namespace VeoAutoMod.Dealers.Menus
{
    abstract class Menu : Objects.Menu
    {
        protected Dealer dealer;
        protected Vehicle vehicle;

        public Menu(Dealer dealer, string title = "", string subtitle = "") : base(title, subtitle)
        {
            this.dealer = dealer;
        }

        public Menu SetVehicle(Vehicle vehicle)
        {
            this.vehicle = vehicle;
            return this;
        }
    }
}
