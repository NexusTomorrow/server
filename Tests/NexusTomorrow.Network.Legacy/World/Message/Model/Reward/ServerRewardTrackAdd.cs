using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

[Message(GameMessageOpcode.ServerRewardTrackAdd)]
public class ServerRewardTrackAdd : IWritable
{
    public RewardTrack RewardTrack { get; set; }

    public void Write(GamePacketWriter writer)
    {
        RewardTrack.Write(writer);
    }
}
