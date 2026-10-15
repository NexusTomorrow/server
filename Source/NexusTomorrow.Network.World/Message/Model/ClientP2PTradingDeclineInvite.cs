using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ClientP2PTradingDeclineInvite)]
public class ClientP2PTradingDeclineInvite : IReadable
{
    // Zero byte message
    public void Read(GamePacketReader reader)
    {
    }
}
