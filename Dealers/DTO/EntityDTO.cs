using GTA.Math;


namespace VeoAutoMod.Dealers.DTO
{
    class EntityDTO
    {
        public readonly int Id;
        public readonly Vector3 Position = new Vector3(0, 0, 0);
        public readonly Vector3 Rotation = new Vector3(0, 0, 0);
        public readonly bool IsDynamic = false;

        public EntityDTO(int id, Vector3 position, Vector3 rotation, bool isDynamic)
        {
            Id = id;
            Position = position;
            Rotation = rotation;
            IsDynamic = isDynamic;
        }
    }
}
