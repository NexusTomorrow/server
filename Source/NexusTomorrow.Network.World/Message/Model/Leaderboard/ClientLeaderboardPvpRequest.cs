using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[Message(GameMessageOpcode.ClientLeaderboardPvpRequest)]
[WireMessage(GameMessageOpcode.ClientLeaderboardPvpRequest, Direction.ClientToServer)]
public sealed partial class ClientLeaderboardPvpRequest : IReadable
{
    [UInt(4)] public LeaderboardType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<LeaderboardType>(4u);
    }
}
