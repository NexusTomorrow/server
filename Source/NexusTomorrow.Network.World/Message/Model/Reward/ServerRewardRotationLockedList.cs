using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerRewardRotationLockedList, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedList
{
    [Array(CountBits = 32)] public List<LockedReward> Rewards { get; set; } = [];
}
