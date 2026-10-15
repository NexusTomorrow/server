using NexusTomorrow.Game.Static.Leaderboard;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Leaderboard;

[Message(GameMessageOpcode.ClientLeaderboardPvpRequest)]
public class ClientLeaderboardPvpRequest : IReadable
{
    public LeaderboardType Type { get; private set; }

    public void Read(GamePacketReader reader)
    {
        Type = reader.ReadEnum<LeaderboardType>(4u);
    }
}
