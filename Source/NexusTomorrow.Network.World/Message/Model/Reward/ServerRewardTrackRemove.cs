using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardTrackRemove)]
[WireMessage(GameMessageOpcode.ServerRewardTrackRemove, Direction.ServerToClient)]
public sealed partial class ServerRewardTrackRemove : IWritable
{
    [UInt(14)] public uint RewardTrackId { get; set; } // if 0 it removes all reward tracks

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RewardTrackId, 14u);
    }
}
