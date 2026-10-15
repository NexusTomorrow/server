using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerRewardRotationLockedRemove, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedRemove
{
    public LockedReward Reward { get; set; }
}
