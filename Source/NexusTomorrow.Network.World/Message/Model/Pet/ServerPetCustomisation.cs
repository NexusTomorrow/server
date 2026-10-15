using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Pet
{
    [Message(GameMessageOpcode.ServerPetCustomisation)]
    public class ServerPetCustomisation : IWritable
    {
        public PetCustomisation PetCustomisation { get; set; }

        public void Write(GamePacketWriter writer)
        {
            PetCustomisation.Write(writer);
        }
    }
}
