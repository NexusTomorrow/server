using NexusTomorrow.Game.Static.Reward;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[WireMessage(GameMessageOpcode.ServerRewardRotations, Direction.ServerToClient)]
public sealed partial class ServerRewardRotations
{
    [WireStruct]
    public sealed partial class RewardRotation
    {
        [UInt(32)] public uint RotationId { get; set; } // Appears to be an ID used to denote a reward offered for some rotation period
                                                        // Values seen vary been 0 and ~5600
                                                        // Seen multiple times over time with different values in the RewardInfo
                                                        // Not certain how this should be created/managed
        [UInt(14)] public uint RewardRotationContentId { get; set; }
        [Float32] public float ExpirationTime { get; set; } // in days from now
        [UInt(8)] public RewardRotationRewardType RewardType { get; set; }
        [UInt(32)] public uint RewardId { get; set; }
    }

    [Array(CountBits = 32)] public List<RewardRotation> RewardRotations { get; set; }
}
