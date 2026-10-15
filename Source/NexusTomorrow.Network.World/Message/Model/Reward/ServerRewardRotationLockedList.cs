using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardRotationLockedList)]
[WireMessage(GameMessageOpcode.ServerRewardRotationLockedList, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedList : IWritable
{
    [Array(CountBits = 32)] public List<LockedReward> Rewards { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Rewards.Count);
        Rewards.ForEach(reward => reward.Write(writer));
    }
}
