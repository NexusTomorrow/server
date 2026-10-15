using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

[Message(GameMessageOpcode.ClientItemGenericUnlock)]
[WireMessage(GameMessageOpcode.ClientItemGenericUnlock, Direction.ClientToServer)]
public sealed partial class ClientItemGenericUnlock : IReadable
{
    public ItemLocation Location { get; } = new();

    public void Read(GamePacketReader reader)
    {
        Location.Read(reader);
    }
}
