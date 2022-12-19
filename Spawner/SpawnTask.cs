using GTA;
using GTA.Math;
using System;


namespace VeoAutoMod.Spawner
{
    enum SpawnType
    {
        Vehicle,
        Prop
    }

    class SpawnTask
    {
        protected int modelId;
        protected SpawnType type;
        protected Action<Vehicle> vehicleCallback;
        protected Action<Prop> propCallback;

        protected Vector3 position;
        protected float rotation;
        protected Vector3 rotation3d;

        public SpawnTask(SpawnType type, int modelId)
        {
            this.type = type;
            this.modelId = modelId;            
        }

        public int GetModelId() => modelId;

        public SpawnType GetSpawnType() => type;
        public bool IsVehicle() => type == SpawnType.Vehicle;
        public bool IsProp() => type == SpawnType.Prop;

        public void SetPosition(Vector3 position) => this.position = position;
        public Vector3 GetPosition() => position;

        public void SetRotation(float rotation) => this.rotation = rotation;
        public void SetRotation3d(Vector3 rotation3d) => this.rotation3d = rotation3d;
        public float GetRotation() => rotation;
        public Vector3 GetRotation3d() => rotation3d;

        public void OnVehicleCreated(Action<Vehicle> callback) => vehicleCallback = callback;        
        public void VehicleCreated(Vehicle vehicle) => vehicleCallback?.Invoke(vehicle);

        public void OnPropCreated(Action<Prop> callback) => propCallback = callback;        
        public void PropCreated(Prop prop) => propCallback?.Invoke(prop);
    }
}
