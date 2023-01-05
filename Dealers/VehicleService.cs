using System;
using System.Collections.Generic;
using GTA;
using GTA.Math;
using GTA.UI;
using LemonUI.TimerBars;
using VeoAutoMod.Dealers.DTO;
using VeoAutoMod.Objects;


namespace VeoAutoMod.Dealers
{
    static class VehicleService
    {
        public static string GetSitText(Vehicle vehicle)
        {
            if (IsCar(vehicle)) return "in the car";
            if (IsTruck(vehicle)) return "in the truck";
            if (IsBike(vehicle)) return "on the bike";

            return "in the vehicle";
        }

        public static string GetClassNameText(Vehicle vehicle)
        {
            if (IsCar(vehicle)) return "car";
            if (IsTruck(vehicle)) return "truck";
            if (IsBike(vehicle)) return "bike";

            return "vehicle";
        }

        public static bool IsCar(Vehicle vehicle)
        {
            List<VehicleClass> carClasses = new List<VehicleClass>() { 
                VehicleClass.Sedans,
                VehicleClass.Emergency,
                VehicleClass.Muscle,
                VehicleClass.OffRoad,
                VehicleClass.Sports,
                VehicleClass.SportsClassics,
                VehicleClass.SUVs,
                VehicleClass.Vans,
                VehicleClass.Super,
                VehicleClass.Coupes,
                VehicleClass.Compacts,
                VehicleClass.Military
            };

            return carClasses.Contains(vehicle.ClassType);
        }

        public static bool IsBike(Vehicle vehicle)
        {
            List<VehicleClass> bikeClasses = new List<VehicleClass>() { 
                VehicleClass.Motorcycles,
                VehicleClass.Cycles
            };

            return bikeClasses.Contains(vehicle.ClassType);
        }

        public static bool IsTruck(Vehicle vehicle)
        {
            List<VehicleClass> bikeClasses = new List<VehicleClass>() { 
                VehicleClass.Commercial,
                VehicleClass.Industrial,
                VehicleClass.Utility,
                VehicleClass.Service
            };

            return bikeClasses.Contains(vehicle.ClassType);
        }
    }
}
