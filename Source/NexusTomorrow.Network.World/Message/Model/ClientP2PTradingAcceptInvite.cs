using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingAcceptInvite)]
[WireMessage(GameMessageOpcode.ClientP2PTradingAcceptInvite, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingAcceptInvite : IReadable
{
    // Zero byte message
    public void Read(GamePacketReader reader)
    {
    }
}
