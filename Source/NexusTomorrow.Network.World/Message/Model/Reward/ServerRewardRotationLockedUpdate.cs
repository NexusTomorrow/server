using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

// Can only update rewards already added via LockedList or LockedAdd
[WireMessage(GameMessageOpcode.ServerRewardRotationLockedUpdate, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedUpdate
{
    public LockedReward Reward { get; set; }
}
