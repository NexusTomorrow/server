using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[WireMessage(GameMessageOpcode.ClientLeaderboardPveRequest, Direction.ClientToServer)]
public sealed partial class ClientLeaderboardPveRequest
{
    [UInt(4)] public LeaderboardType Type { get; private set; }
    [UInt(32)] public uint MatchingGameMapdId { get; private set; }
    [UInt(32)] public uint PrimeLevel { get; private set; }
}
