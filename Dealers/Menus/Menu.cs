namespace VeoAutoMod.Dealers.Menus
{
    abstract class Menu : Objects.Menu
    {
        protected Dealer dealer;

        public Menu(Dealer dealer, string title = "", string subtitle = "") : base (title, subtitle)
        {
            this.dealer = dealer;
        }
    }
}
