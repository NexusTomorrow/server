using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet
{
    [Message(GameMessageOpcode.ServerPetDespawned)]
    public class ServerPetDespawned : IWritable
    {
        public uint PetUnitId { get; set; } // TBC if this follows other messages, 0 means all engineer pets

        public void Write(GamePacketWriter writer)
        {
            writer.Write(PetUnitId);
        }
    }
}
