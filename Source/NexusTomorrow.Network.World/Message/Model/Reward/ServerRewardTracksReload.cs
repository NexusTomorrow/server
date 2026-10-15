using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

// Clears all existing RewardTrack data then loads it from this message
// Triggers RewardTrackedLoaded event
[Message(GameMessageOpcode.ServerRewardTracksReload)]
[WireMessage(GameMessageOpcode.ServerRewardTracksReload, Direction.ServerToClient)]
public sealed partial class ServerRewardTracksReload : IWritable
{
    [Array(CountBits = 32)] public List<RewardTrack> RewardsTracks { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(RewardsTracks.Count);
        RewardsTracks.ForEach(rewardTrack => rewardTrack.Write(writer));
    }
}
