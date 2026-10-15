using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

// Can only update rewards already added via LockedList or LockedAdd
[Message(GameMessageOpcode.ServerRewardRotationLockedUpdate)]
[WireMessage(GameMessageOpcode.ServerRewardRotationLockedUpdate, Direction.ServerToClient)]
public sealed partial class ServerRewardRotationLockedUpdate : IWritable
{
    public LockedReward Reward { get; set; }

    public void Write(GamePacketWriter writer)
    {
        Reward.Write(writer);
    }
}
