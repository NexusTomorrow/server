using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardTrackAdd)]
[WireMessage(GameMessageOpcode.ServerRewardTrackAdd, Direction.ServerToClient)]
public sealed partial class ServerRewardTrackAdd : IWritable
{
    public RewardTrack RewardTrack { get; set; }

    public void Write(GamePacketWriter writer)
    {
        RewardTrack.Write(writer);
    }
}
