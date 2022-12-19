using GTA;
using GTA.Math;
using GTA.Native;
using System.Collections.Generic;


namespace VeoAutoMod.Spawner
{
    class SpawnManager
    {
        protected static List<SpawnTask> queue = new List<SpawnTask>();

        protected static Model currentModel;
        protected static SpawnTask currentTask;

        public static void Queue(SpawnTask task)
        {
            queue.Add(task);
        }

        public static void Update()
        {
            if (currentTask == null && currentModel == null && queue.Count > 0)
            {
                StartNextTask();
            }

            if ((currentModel != null) && currentModel.IsLoaded)
            {
                SpawnAndCloseCurrentTask();
            }
        }

        protected static void StartNextTask()
        {
            currentTask = queue[0];
            currentModel = new Model(currentTask.GetModelId());
            queue.RemoveAt(0);

            currentModel.Request(250);
        }

        protected static void SpawnAndCloseCurrentTask()
        {
            if (currentTask.IsVehicle())
            {
                Vector3 position = currentTask.GetPosition();

                Vehicle vehicle = GTA.World.CreateVehicle(
                    currentModel,
                    position,
                    currentTask.GetRotation()
                );

                currentTask.VehicleCreated(vehicle);
                CurrentTaskDone();

                return;
            }
            
            if (currentTask.IsProp())
            {
                Prop prop = GTA.World.CreateProp(
                    currentModel,
                    currentTask.GetPosition(),
                    currentTask.GetRotation3d(),
                    true,
                    false
                );

                prop.Position = currentTask.GetPosition();
                prop.LodDistance = 3000;

                currentTask.PropCreated(prop);
                CurrentTaskDone();

                return;
            }
        }

        protected static void CurrentTaskDone()
        {
            currentTask = null;
            currentModel = null;
        }
    }
}
