using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingDeclineInvite)]
[WireMessage(GameMessageOpcode.ClientP2PTradingDeclineInvite, Direction.ClientToServer)]
public sealed partial class ClientP2PTradingDeclineInvite : IReadable
{
    // Zero byte message
    public void Read(GamePacketReader reader)
    {
    }
}
