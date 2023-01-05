using System.Collections.Generic;
using System.Linq;
using GTA;
using GTA.Math;
using GTA.Native;
using GTA.UI;
using VeoAutoMod.Dealers.DTO;
using VeoAutoMod.Dealers.Menus;
using VeoAutoMod.Objects;
using VeoAutoMod.Spawner;

namespace VeoAutoMod.Dealers
{
    class Dealer : Company
    {
        const float GreetingDistance = 15f;

        const float MarkerCarFrontOffset = 3.2f;
        const float MarkerBikeFrontOffset = 2.5f;
        const float MarkerTruckFrontOffset = 5.2f;

        const float VehicleMarkerRadius = .5f;
        const float VehicleMarkerHeight = .5f;

        private DealerConfig config;

        private List<Prop> spawnedProps = new List<Prop>();
        private List<Vehicle> spawnedVehicles = new List<Vehicle>();
        private Dictionary<Vehicle, VehicleDTO> spawnedVehicleDTOs = new Dictionary<Vehicle, VehicleDTO>();
        private Dictionary<Vehicle, Marker> spawnedMarkers = new Dictionary<Vehicle, Marker>();

        private Vehicle currentVehicle;
        private TestDrive testDrive;

        private float markerZ;

        private Dictionary<string, Menus.Menu> menus = new Dictionary<string, Menus.Menu>();

        private bool isGreet = false;

        public Dealer(string name, Vector3 position, DealerConfig config) : base(name, position) {
            this.config = config;
            
            testDrive = new TestDrive();
            testDrive.OnFinish(OnTestDriveFinish);
        }

        public DealerConfig GetConfig() => config;

        protected void CreateMenus()
        {            
            menus.Add("ground", new GroundMenu(this));
            menus.Add("vehicle", new VehicleMenu(this));
            menus.Add("plate", new PlateMenu(this));
        }

        protected void DestroyMenus()
        {
            foreach (Menus.Menu menu in menus.Values) menu.Destroy();
            menus.Clear();
        }

        protected void HideMenus()
        {
            foreach (Menus.Menu menu in menus.Values) menu.Hide();
        }

        protected override void Spawn()
        {
            if (World.IsDebug()) Notification.Show("Spawn: " + GetName());
            
            SetState(CompanyState.Spawning);
            CreateMenus();

            ClearSpawnArea();
            RemoveProps();
            SpawnProps();
            SpawnVehicles();            
        }

        protected void SpawnVehicles()
        {
            VehicleDTO[] vehicleDTOs = config.GetShuffledVehicles();
            int slotIndex = 0;

            foreach (SlotDTO slotDTO in config.Slots)
            {
                VehicleDTO vehicleDTO = vehicleDTOs[slotIndex];
                vehicleDTO.RandomizePrice();

                slotIndex++;
                if (slotIndex > config.Vehicles.Count - 1)
                {
                    slotIndex = 0;
                    vehicleDTOs = config.GetShuffledVehicles();
                }

                SpawnTask spawnTask = new SpawnTask(SpawnType.Vehicle, vehicleDTO.Id);

                spawnTask.SetPosition(slotDTO.Position);
                spawnTask.SetRotation(slotDTO.Rotation);

                spawnTask.OnVehicleCreated(
                    (Vehicle vehicle) => RegisterVehicle(vehicle, vehicleDTO)
                );

                SpawnManager.Queue(spawnTask);
            }
        }

        protected void SpawnProps()
        {
            foreach (EntityDTO entityDTO in config.Entities)
            {
                SpawnTask spawnTask = new SpawnTask(SpawnType.Prop, entityDTO.Id);

                spawnTask.SetPosition(entityDTO.Position);
                spawnTask.SetRotation3d(entityDTO.Rotation);

                spawnTask.OnPropCreated(RegisterProp);

                SpawnManager.Queue(spawnTask);
            }
        }

        protected void RemoveProps()
        {
            foreach(RemovalDTO removalDTO in config.Removals)
            {
                RemoveProp(removalDTO.EntityId, removalDTO.Position);
            }
        }

        protected void RemoveProp(int id, Vector3 position)
        {
            Prop prop = Function.Call<Prop>(
                Hash.GET_CLOSEST_OBJECT_OF_TYPE, 
                position.X, position.Y, position.Z, 
                1f, 
                id, 
                0
            );

            if (prop != null && prop.Handle != 0) prop.Delete();
        }

        protected void CheckIfSpawned()
        {
            bool allVehiclesSpawned = spawnedVehicles.Count == config.Slots.Count;
            bool allPropsSpawned = spawnedProps.Count == config.Entities.Count;

            if (allVehiclesSpawned && allPropsSpawned && IsSpawning())
            {
                if (World.IsDebug()) Notification.Show("Spawned: " + GetName());
                SetState(CompanyState.Spawned);
                CreateVehicleMarkers();
            }
        }

        protected override void Remove()        
        {
            if (testDrive.IsActive) return;

            if (World.IsDebug()) Notification.Show("Remove: " + GetName());

            DestroyMenus();

            foreach(Vehicle vehicle in spawnedVehicles) vehicle.Delete();
            foreach(Prop prop in spawnedProps) prop.Delete();
            foreach(Marker marker in spawnedMarkers.Values) marker.Delete();

            spawnedVehicles.Clear();
            spawnedVehicleDTOs.Clear();
            spawnedProps.Clear();
            spawnedMarkers.Clear();

            SetState(CompanyState.Removed);
            isGreet = false;
        }

        protected void RegisterProp(Prop prop) 
        {
            if (!IsSpawning())
            {
                prop.Delete();
                return;
            }

            spawnedProps.Add(prop);
            CheckIfSpawned();
        }

        protected void RegisterVehicle(Vehicle vehicle, VehicleDTO dto) 
        {
            if (IsRemoved())
            {
                vehicle.Delete();
                return;
            }

            vehicle.Mods.LicensePlate = config.PlateText;

            spawnedVehicles.Add(vehicle);
            spawnedVehicleDTOs.Add(vehicle, dto);
            
            vehicle.IsDriveable = false;
            vehicle.IsRadioEnabled = false;

            CheckIfSpawned();
        }

        protected void CreateVehicleMarkers()
        {
            markerZ = 99999;
            foreach(Vehicle vehicle in spawnedVehicles)
            {
                if (vehicle.Position.Z < markerZ) markerZ = vehicle.Position.Z;
            }

            foreach (Vehicle vehicle in spawnedVehicles)
            {
                CreateVehicleMarker(vehicle, spawnedVehicleDTOs[vehicle]);
            }
        }

        protected void CreateVehicleMarker(Vehicle vehicle, VehicleDTO dto)
        {
            List<VehicleClass> longVehicleClasses = new List<VehicleClass>() { 
                VehicleClass.Commercial,
                VehicleClass.Industrial,
                VehicleClass.Military,
                VehicleClass.Utility,
                VehicleClass.Service
            };

            float offset = MarkerCarFrontOffset;
            if (longVehicleClasses.Contains(vehicle.ClassType))
            {
                offset = MarkerTruckFrontOffset;
            }   
            
            if (vehicle.ClassType == VehicleClass.Motorcycles)
            {
                offset = MarkerBikeFrontOffset;
            }

            Vector3 markerPosition = vehicle.Position + vehicle.ForwardVector * offset;
            markerPosition.Z = markerZ;
            Marker marker = MarkerManager.CreateMarker(markerPosition, VehicleMarkerRadius, VehicleMarkerHeight);

            marker.OnPlayerTouch((Marker m) => OnPlayerTouchVehicleMarker(vehicle, dto));
            marker.OnPlayerLeft((Marker m) => OnPlayerLeftVehicleMarker());
            spawnedMarkers.Add(vehicle, marker);
        }

        protected void UnregisterVehicle(Vehicle vehicle)
        {
            RemoveVehicleMarker(vehicle);
            spawnedVehicleDTOs.Remove(vehicle);            
            spawnedVehicles.Remove(vehicle);
            vehicle.MarkAsNoLongerNeeded();
        }

        protected void RemoveVehicleMarker(Vehicle vehicle)
        {
            if (spawnedMarkers.ContainsKey(vehicle))
            {
                spawnedMarkers[vehicle].Delete();
                spawnedMarkers.Remove(vehicle);
            }
        }

        public Vehicle GetCurrentVehicle() => currentVehicle;

        protected void OnPlayerTouchVehicleMarker(Vehicle vehicle, VehicleDTO dto)
        {
            if (dto == null) return;

            string menuTitle = dto.Name;
            string menuSubTitle = "Price: " + dto.GetFormattedPrice();

            menus["ground"]
                .SetVehicle(vehicle)
                .SetTitle(menuTitle, menuSubTitle)                
                .Show();
        }        
        
        protected void OnPlayerLeftVehicleMarker()
        {
            menus["ground"].Hide();
        }

        protected void OnPlayerSitInVehicle(Vehicle vehicle, VehicleDTO dto)
        {
            if (dto == null) return;
            if (vehicle.IsDamaged) return;

            string menuTitle = dto.Name;
            string menuSubTitle = "Price: " + dto.GetFormattedPrice();
            menus["vehicle"]
                .SetVehicle(vehicle)
                .SetTitle(menuTitle, menuSubTitle)
                .Show();
        }
        
        protected void OnPlayerLeftVehicle()
        {
            HideMenus();
        }

        protected override void UpdateSpawned()
        {
            if (!isGreet && World.IsPlayerInRadius(GetPosition(), GreetingDistance))
            {
                isGreet = true;
                Notification.Show("~g~AutoDealers V\n~w~Welcome to ~b~" + GetName()+"~w~!");
            }

            if (currentVehicle != null && !Game.Player.Character.IsInVehicle() && !Game.Player.Character.IsGettingIntoVehicle)
            {                
                currentVehicle = null;
                OnPlayerLeftVehicle();
            }

            if (currentVehicle == null && Game.Player.Character.IsGettingIntoVehicle)
            {
                Vehicle vehicle = Game.Player.Character.VehicleTryingToEnter;
                if (spawnedVehicles.Contains(vehicle))
                {
                    currentVehicle = vehicle;
                    OnPlayerSitInVehicle(vehicle, spawnedVehicleDTOs[vehicle]);
                }
            }

            List<Vehicle> vehiclesToRemove = new List<Vehicle>();
            foreach(Vehicle vehicle in spawnedMarkers.Keys)
            {
                if (vehicle.IsDamaged) vehiclesToRemove.Add(vehicle);
            }

            foreach (Vehicle vehicle in vehiclesToRemove)
            {
                if (!testDrive.IsActive || testDrive.Vehicle != vehicle)
                {
                    UnregisterVehicle(vehicle);
                }
            }

            if (testDrive.IsActive) testDrive.Update();
        }

        public void SelectCurrentVehiclePlate()
        {
            VehicleDTO dto = spawnedVehicleDTOs[currentVehicle];

            if (Game.Player.Money < dto.Price)
            {
                Notification.Show("~r~Not enough money!");
                return;
            }

            menus["plate"].SetTitle(dto.Name, "Do you want custom license plates?");
            menus["plate"].Show();
        }
        
        public void BuyCurrentVehicle(string plateText = "", LicensePlateStyle plateStyle = LicensePlateStyle.BlueOnWhite1)
        {
            VehicleDTO dto = spawnedVehicleDTOs[currentVehicle];

            if (Game.Player.Money < dto.Price)
            {
                Notification.Show("~r~Not enough money!");
                HideMenus();
                return;
            }

            if (plateText == "") plateText = GetRandomLicensePlateText();

            currentVehicle.Mods.LicensePlate = plateText;
            currentVehicle.Mods.LicensePlateStyle = plateStyle;
            currentVehicle.IsDriveable = true;

            Game.Player.Money -= dto.Price;
            UnregisterVehicle(currentVehicle);
            
            Notification.Show("~g~Congratulations!~w~\nYou have just bought\n~b~" + dto.Name + "\n\n~g~Drive it now!");
        }

        public void TestDriveCurrentVehicle()
        {
            if (currentVehicle.Driver != Game.Player.Character)
            {
                Notification.Show("~r~You must be in the driver's seat to start the Test Drive!");
                menus["vehicle"].Show();
                return;
            }

            Screen.FadeOut(1500);
            Script.Wait(1500);

            VehicleDTO dto = spawnedVehicleDTOs[currentVehicle];

            currentVehicle.IsDriveable = true;
            RemoveVehicleMarker(currentVehicle);

            testDrive.Start(currentVehicle, dto);

            Screen.FadeIn(1500);
            Script.Wait(1500);
        }

        public void OnTestDriveFinish(string reason)
        {
            Screen.FadeOut(500);
            Script.Wait(500);

            Vehicle vehicle = testDrive.Vehicle;
            VehicleDTO dto = testDrive.VehicleDTO;
            Vector3 startPosition = testDrive.VehiclePosition;
            Vector3 startRotation = testDrive.VehicleRotation;

            Notification.Show("Test Drive has finished:\n" + reason);

            vehicle.Position = startPosition;
            vehicle.Rotation = startRotation;

            if (vehicle.Driver == Game.Player.Character)
            {
                Game.Player.Character.Task.LeaveVehicle();
            } else
            {
                Game.Player.Character.Position = vehicle.Position + vehicle.ForwardVector * MarkerCarFrontOffset;
            }

            vehicle.IsDriveable = false;

            if (!vehicle.IsDamaged)
            {
                CreateVehicleMarker(vehicle, dto);                
            } else
            {
                vehicle.LockStatus = VehicleLockStatus.PlayerCannotEnter;
            }

            Screen.FadeIn(2000);
            Script.Wait(2000);
        }

        protected string GetRandomLicensePlateText(int length = 8)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[World.Random.Next(s.Length)]).ToArray());
        }

        protected void ClearSpawnArea()
        {
            Vector3 position = GetPosition();

            Vehicle vehicleOnPosition = Function.Call<Vehicle>(Hash.GET_CLOSEST_VEHICLE, position.X, position.Y, position.Z, 20f, 0);

            if (vehicleOnPosition != null && vehicleOnPosition.Handle != 0)
            {
                do
                {
                    vehicleOnPosition.Delete();
                    vehicleOnPosition = Function.Call<Vehicle>(Hash.GET_CLOSEST_VEHICLE, position.X, position.Y, position.Z, 20f, 0);
                } while (vehicleOnPosition != null && vehicleOnPosition.Handle != 0);
            }
        }        

        public void AddVehicleForSale(int id, string name, int price)
        {
            config.Vehicles.Add(new VehicleDTO(id, name, price));
        }
    }
}
