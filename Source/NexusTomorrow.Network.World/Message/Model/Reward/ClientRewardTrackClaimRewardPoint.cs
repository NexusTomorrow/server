using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ClientRewardTrackClaimRewardPoint, Direction.ClientToServer)]
public sealed partial class ClientRewardTrackClaimRewardPoint
{
    [UInt(14)] public ushort RewardTrackRewardsId { get; private set; }
    [UInt(32)] public uint ChoiceIndex { get; private set; }
}
