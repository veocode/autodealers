using GTA.Math;


namespace VeoAutoMod.Dealers.DTO
{
    class SlotDTO
    {
        public readonly Vector3 Position;
        public readonly float Rotation;

        public SlotDTO(Vector3 position, float rotation)
        {
            Position = position;
            Rotation = rotation;
        }
    }
}
