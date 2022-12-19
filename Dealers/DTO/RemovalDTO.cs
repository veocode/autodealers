using GTA.Math;


namespace VeoAutoMod.Dealers.DTO
{
    class RemovalDTO
    {
        public readonly int EntityId;
        public readonly Vector3 Position = new Vector3(0, 0, 0);

        public RemovalDTO(int entityId, Vector3 position)
        {
            EntityId = entityId;
            Position = position;
        }
    }
}
