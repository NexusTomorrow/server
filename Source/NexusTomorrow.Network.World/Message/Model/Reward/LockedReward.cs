using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireStruct]
public sealed partial class LockedReward
{
    [UInt(3)] public RewardRotationContentType ContentType { get; set; }
    [UInt(32)] public uint RewardRotationContentId { get; set; }
    [UInt(32)] public uint RewardId { get; set; } // RewardRotationItemId, RewardRotationEssenceId, RewardRotationModifierId
    [UInt(8)] public RewardRotationRewardType RewardType { get; set; }
    [UInt(32)] public GrantedRewardFlags RewardedFlags { get; set; }
}
