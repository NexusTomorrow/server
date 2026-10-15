using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientItemMove)]
[WireMessage(GameMessageOpcode.ClientItemMove, Direction.ClientToServer)]
public sealed partial class ClientItemMove : IReadable
{
    public ItemLocation From { get; } = new();
    public ItemLocation To { get; } = new();

    public void Read(GamePacketReader reader)
    {
        From.Read(reader);
        To.Read(reader);
    }
}
