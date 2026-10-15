using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

public class LockedReward : IWritable
{
    public RewardRotationContentType ContentType { get; set; }
    public uint RewardRotationContentId { get; set; }
    public uint RewardId { get; set; } // RewardRotationItemId, RewardRotationEssenceId, RewardRotationModifierId
    public RewardRotationRewardType RewardType { get; set; }
    public GrantedRewardFlags RewardedFlags { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(ContentType, 3u);
        writer.Write(RewardRotationContentId);
        writer.Write(RewardId);
        writer.Write(RewardType, 8u);
        writer.Write(RewardedFlags);
    }
}
