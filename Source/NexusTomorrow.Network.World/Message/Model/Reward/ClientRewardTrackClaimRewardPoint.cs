using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ClientRewardTrackClaimRewardPoint)]
[WireMessage(GameMessageOpcode.ClientRewardTrackClaimRewardPoint, Direction.ClientToServer)]
public sealed partial class ClientRewardTrackClaimRewardPoint : IReadable
{
    [UInt(14)] public ushort RewardTrackRewardsId { get; private set; }
    [UInt(32)] public uint ChoiceIndex { get; private set; }

    public void Read(GamePacketReader reader)
    {
        RewardTrackRewardsId = reader.ReadUShort(14u);
        ChoiceIndex = reader.ReadUInt();
    }
}
