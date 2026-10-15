using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireStruct]
public sealed partial class RewardTrack : IWritable
{
    [UInt(14)] public uint RewardTrackId { get; set; }
    [UInt(32)] public uint RewardPointsEarned { get; set; }
    [FixedArray(4), UInt(8)] public byte[] ClaimedRewards { get; set; } = new byte[4]; // 32 flags, least significant bit of each byte first
    [UInt(32), Count] public uint RewardTrackRewardsIdCount { get; set; }
    [UInt(32)] public uint Unlocked { get; set; } // needs more research, unclear if this is a bit array or an integer
    [Array(CountFrom = nameof(RewardTrackRewardsIdCount)), UInt(32)] public List<uint> RewardTrackRewardsIds { get; set; } = [];
    [UInt(1)] public bool IsActive { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RewardTrackId, 14u);
        writer.Write(RewardPointsEarned);
        writer.WriteBytes(ClaimedRewards);
        writer.Write(RewardTrackRewardsIds.Count);
        writer.Write(Unlocked, 32u);
        RewardTrackRewardsIds.ForEach(rewardId => writer.Write(rewardId));
        writer.Write(IsActive);
    }
}
