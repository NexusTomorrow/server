using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Costume;

[Message(GameMessageOpcode.ClientCostumeItemUnlock)]
[WireMessage(GameMessageOpcode.ClientCostumeItemUnlock, Direction.ClientToServer)]
public sealed partial class ClientCostumeItemUnlock : IReadable
{
    public ItemLocation Location { get; } = new();

    public void Read(GamePacketReader reader)
    {
        Location.Read(reader);
    }
}
