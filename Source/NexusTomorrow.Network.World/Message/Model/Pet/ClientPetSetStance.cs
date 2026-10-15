using NexusTomorrow.Game.Static.Pet;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet
{
    [Message(GameMessageOpcode.ClientPetSetStance)]
    public class ClientPetSetStance : IReadable
    {
        public uint PetUnitId { get; private set; } //The id number for the pet whose stance is being set. Setting this to 0 will set the stance for all of the player's pets.
        public PetStance Stance { get; private set; }

        public void Read(GamePacketReader reader)
        {
            PetUnitId = reader.ReadUInt();
            Stance = reader.ReadEnum<PetStance>(8u);
        }
    }
}
