using System.Globalization;


namespace VeoAutoMod.Dealers.DTO
{
    class VehicleDTO
    {
        public readonly int Id;
        public readonly string Name;
        public readonly int BasePrice;

        private int price;
        public int Price { get => price; }

        public VehicleDTO(int id, string name, int price)
        {
            Id = id;
            Name = name;
            BasePrice = price;
            this.price = price;
        }

        public string GetFormattedPrice()
        {
            return string.Format(new CultureInfo("en-US"), "{0:c}", Price).Split('.')[0];
        }

        public void RandomizePrice()
        {
            int minus = 200 * World.Random.Next(0, 15);
            int plus = 200 * World.Random.Next(0, 25);
            price = BasePrice - minus + plus;
        }

        public VehicleDTO CloneWithRandomizedPrice()
        {
            VehicleDTO clone = new VehicleDTO(Id, Name, BasePrice);
            clone.RandomizePrice();
            return clone;
        }
    }
}
