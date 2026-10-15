using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardRotations)]
[WireMessage(GameMessageOpcode.ServerRewardRotations, Direction.ServerToClient)]
public sealed partial class ServerRewardRotations : IWritable
{
    [WireStruct]
    public sealed partial class RewardRotation : IWritable
    {
        [UInt(32)] public uint RotationId { get; set; } // Appears to be an ID used to denote a reward offered for some rotation period
                                                        // Values seen vary been 0 and ~5600
                                                        // Seen multiple times over time with different values in the RewardInfo
                                                        // Not certain how this should be created/managed
        [UInt(14)] public uint RewardRotationContentId { get; set; }
        [Float32] public float ExpirationTime { get; set; } // in days from now
        [UInt(8)] public RewardRotationRewardType RewardType { get; set; }
        [UInt(32)] public uint RewardId { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(RotationId);
            writer.Write(RewardRotationContentId, 14u);
            writer.Write(ExpirationTime);
            writer.Write(RewardType, 8u);
            writer.Write(RewardId);
        }
    }

    [Array(CountBits = 32)] public List<RewardRotation> RewardRotations { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RewardRotations.Count);
        RewardRotations.ForEach(reward => reward.Write(writer));
    }
}
