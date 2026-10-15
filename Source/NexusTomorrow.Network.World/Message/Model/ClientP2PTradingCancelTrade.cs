using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingCancelTrade)]
[WireMessage(GameMessageOpcode.ClientP2PTradingCancelTrade, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingCancelTrade : IReadable
{
    // Zero byte message
    public void Read(GamePacketReader reader)
    {
    }
}
