using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pet;

[Message(GameMessageOpcode.ServerPetClearFlair)]
[WireMessage(GameMessageOpcode.ServerPetClearFlair, Direction.ServerToClient)]
public sealed partial class ServerPetClearFlair : IWritable
{
    public void Write(GamePacketWriter writer)
    {
        // Zero byte message
    }
}
