using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardRotationLockedRemove)]
[WireMessage(GameMessageOpcode.ServerRewardRotationLockedRemove, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedRemove : IWritable
{
    public LockedReward Reward { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Reward.Write(writer);
    }
}
