using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet
{
    [Message(GameMessageOpcode.ServerPetClearFlair)]
    public class ServerPetClearFlair : IWritable
    {
        public void Write(GamePacketWriter writer)
        {
            // Zero byte message
        }
    }
}
