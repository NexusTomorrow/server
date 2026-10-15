using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientCreddRedeem)]
[WireMessage(GameMessageOpcode.ClientCreddRedeem, Direction.ClientToServer)]
public sealed partial class ClientCreddRedeem : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
