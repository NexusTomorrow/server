using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardTrackUpdate)]
[WireMessage(GameMessageOpcode.ServerRewardTrackUpdate, Direction.ServerToClient)]
public sealed partial class ServerRewardTrackUpdate : IWritable
{
    [UInt(14)] public uint RewardTrackId { get; set; }
    [UInt(32)] public uint RewardPointsEarned { get; set; }
    [FixedArray(4), UInt(8)] public byte[] ClaimedRewards { get; set; } = new byte[4]; // 32 flags, least significant bit of each byte first
    [UInt(32)] public uint Unlocked { get; set; } // needs more research, unclear if this is a bit array or an integer
    [UInt(1)] public bool IsActive { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RewardTrackId, 14u);
        writer.Write(RewardPointsEarned);
        writer.WriteBytes(ClaimedRewards);
        writer.Write(Unlocked);
        writer.Write(IsActive);
    }
}
