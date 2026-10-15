using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardRotationLockedAdd)]
[WireMessage(GameMessageOpcode.ServerRewardRotationLockedAdd, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedAdd : IWritable
{
    public LockedReward Reward { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Reward.Write(writer);
    }
}
