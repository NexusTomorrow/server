using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[Message(GameMessageOpcode.ClientDailyLoginRewardsRequest)]
[WireMessage(GameMessageOpcode.ClientDailyLoginRewardsRequest, Direction.ClientToServer)]
public sealed partial class ClientDailyLoginRewardsRequest : IReadable
{
    public void Read(GamePacketReader reader)
    {
        // zero byte message
    }
}
