using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerRewardRotationLockedAdd, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedAdd
{
    public LockedReward Reward { get; set; }
}
