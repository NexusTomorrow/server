using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.ClientLeaderboardPvpRequest, Direction.ClientToServer)]
public sealed partial class ClientLeaderboardPvpRequest
{
    [UInt(4)] public LeaderboardType Type { get; private set; }
}
