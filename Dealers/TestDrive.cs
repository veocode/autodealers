using System;
using GTA;
using GTA.Math;
using GTA.UI;
using LemonUI.TimerBars;
using VeoAutoMod.Dealers.DTO;
using VeoAutoMod.Objects;


namespace VeoAutoMod.Dealers
{
    class TestDrive
    {
        const int DurationSec = 60;

        private Vehicle vehicle;
        public Vehicle Vehicle { get => vehicle; }

        private VehicleDTO vehicleDTO;
        public VehicleDTO VehicleDTO { get => vehicleDTO; }

        private Vector3 vehiclePosition;
        public Vector3 VehiclePosition { get => vehiclePosition; }

        private Vector3 vehicleRotation;
        public Vector3 VehicleRotation { get => vehicleRotation; }

        private string vehicleClass;

        private Timer timer;
        private TimerBar timerBar;
        private bool isActive = false;
        private Action<string> callback;

        public bool IsActive { get => isActive; }

        public void OnFinish(Action<string> callback) => this.callback = callback;

        public void Start(Vehicle vehicle, VehicleDTO vehicleDTO)
        {
            isActive = true;

            this.vehicle = vehicle;
            this.vehicleDTO = vehicleDTO;

            vehiclePosition = new Vector3(vehicle.Position.X, vehicle.Position.Y, vehicle.Position.Z);
            vehicleRotation = new Vector3(vehicle.Rotation.X, vehicle.Rotation.Y, vehicle.Rotation.Z);            

            timer = Timer.Create(DurationSec * 1000, () => Finish("~y~Time is over") );

            timerBar = UIManager.CreateTimerBar("Test Drive Time Left", DurationSec.ToString());

            vehicleClass = VehicleService.GetClassNameText(vehicle);

            Notification.Show("~g~TestDrive Started!\n~w~You have ~b~" + DurationSec + " ~w~seconds");
            Notification.Show($"Leave the {vehicleClass} to finish at any moment");
        }

        public void Update()
        {
            if (!isActive) return;

            int secondsPassed = timer.GetSecondsPassed();

            timerBar.Info = (DurationSec - secondsPassed).ToString();

            if (vehicle.IsDamaged)
            {
                Finish($"~r~You damaged the {vehicleClass}!\nIt's not on sale anymore!");
            }
            if (!Game.Player.Character.IsInVehicle(vehicle))
            {
                Finish($"~g~You left the {vehicleClass}");
            }
        }

        public void Finish(string reason)
        {
            if (!isActive) return;

            timer.Stop();
            timer = null;

            UIManager.RemoveTimerBar(timerBar);
            timerBar = null;

            callback?.Invoke(reason);

            isActive = false;
        }

    }
}
