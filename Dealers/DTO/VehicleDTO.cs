using System.Globalization;


namespace VeoAutoMod.Dealers.DTO
{
    class VehicleDTO
    {
        public readonly int Id;
        public readonly string Name;
        public readonly int Price;

        public VehicleDTO(int id, string name, int price)
        {
            Id = id;
            Name = name;
            Price = price;
        }

        public string GetFormattedPrice()
        {
            return string.Format(new CultureInfo("en-US"), "{0:c}", Price).Split('.')[0];
        }
    }
}
