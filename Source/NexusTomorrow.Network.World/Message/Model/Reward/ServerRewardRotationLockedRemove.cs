using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardRotationLockedRemove)]
public class ServerRewardRotationLockedRemove : IWritable
{
    public LockedReward Reward { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Reward.Write(writer);
    }
}
