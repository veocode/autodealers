using System.Collections.Generic;
using VeoAutoMod.Dealers.DTO;

namespace VeoAutoMod.Dealers
{
    class DealerConfig
    {
        public const string DefaultPlateText = "SALE";

        public readonly string FilePath;
        public readonly string PlateText;        
        public readonly List<SlotDTO> Slots = new List<SlotDTO>();
        public readonly List<VehicleDTO> Vehicles = new List<VehicleDTO>();
        public readonly List<EntityDTO> Entities = new List<EntityDTO>();
        public readonly List<RemovalDTO> Removals = new List<RemovalDTO>();

        public DealerConfig(
            string filePath,
            List<SlotDTO> slots, 
            List<VehicleDTO> vehicles, 
            List<EntityDTO> entities, 
            List<RemovalDTO> removals, 
            string plateText = ""
        )
        {
            this.FilePath = filePath;
            this.Slots = slots;
            this.Vehicles = vehicles;
            this.Entities = entities;
            this.Removals = removals;

            if (plateText == "") plateText = DefaultPlateText;
            this.PlateText = plateText;
        }

        public VehicleDTO[] GetShuffledVehicles()
        {
            VehicleDTO[] array = Vehicles.ToArray();

            int n = array.Length;
            while (n > 1)
            {
                int k = World.Random.Next(n--);
                VehicleDTO temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }

            return array;
        }
    }
}
