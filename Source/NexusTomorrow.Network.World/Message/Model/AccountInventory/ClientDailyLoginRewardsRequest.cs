using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.AccountInventory;

[WireMessage(GameMessageOpcode.ClientDailyLoginRewardsRequest, Direction.ClientToServer)]
public sealed partial class ClientDailyLoginRewardsRequest
{
}
