using System;
using System.Collections.Generic;
using GTA;
using GTA.UI;
using GTA.Math;
using VeoAutoMod.Dealers;
using VeoAutoMod.Objects;
using VeoAutoMod.Spawner;

namespace VeoAutoMod
{
    class World
    {
        const string ModDataFolderPath = "scripts\\AutoDealersData";

        public static Random Random = new Random();
        private static bool isDebug;

        private static List<Dealer> autoDealers = new List<Dealer>();

        private static List<Blip> blips = new List<Blip>();
        private static bool isBlips = false;

        public static void Create(bool isDebug = false)
        {
            World.isDebug = isDebug;
            autoDealers = Factory.CreateDealers(ModDataFolderPath, "Dealers.xml");
            CreateBlips();

            if (isDebug) Notification.Show("~y~AutoDealers Debug Mode Enabled");
        }

        public static void Update()
        {
            SpawnManager.Update();
            MarkerManager.Update();

            foreach (Dealer dealer in autoDealers) dealer.Update();
        }

        public static List<Dealer> GetDealers() => autoDealers;

        public static bool IsPlayerInRadius(Vector3 center, float radius)
        {
            return center.DistanceTo(Game.Player.Character.Position) <= radius;
        }

        public static void CreateBlips()
        {
            foreach (Dealer dealer in autoDealers)
            {
                Blip blip = GTA.World.CreateBlip(dealer.GetPosition());
                blip.Scale = 1f;
                blip.Sprite = BlipSprite.VehicleWarehouse;
                blip.IsShortRange = true;
                blip.Name = dealer.GetName();
                blips.Add(blip);
            }
        }

        public static void RemoveBlips()
        {
            foreach (Blip blip in blips)
            {
                blip.Delete();
            }
            blips.Clear();
        }

        public static void ToggleBlips()
        {
            isBlips = !isBlips;
            if (isBlips)
            {
                CreateBlips();
            } else
            {
                RemoveBlips();
            }
        }

        public static bool IsDebug()
        {
            return isDebug;
        }
    }
}
