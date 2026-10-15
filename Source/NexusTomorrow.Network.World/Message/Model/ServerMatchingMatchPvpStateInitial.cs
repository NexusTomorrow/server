using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model
{
    [Message(GameMessageOpcode.ServerMatchingMatchPvpStateInitial)]
    public class ServerMatchingMatchPvpStateInitial : IWritable
    {
        public MatchTeam Team { get; set; }
        public StateInfo State { get; set; }

        public void Write(GamePacketWriter writer)
        {
            writer.Write(Team, 2u);
            State.Write(writer);
        }
    }
}
