using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reward;

// Clears all existing RewardTrack data then loads it from this message
// Triggers RewardTrackedLoaded event
[WireMessage(GameMessageOpcode.ServerRewardTracksReload, Direction.ServerToClient)]
public sealed partial class ServerRewardTracksReload
{
    [Array(CountBits = 32)] public List<RewardTrack> RewardsTracks { get; set; } = [];
}
