using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[Message(GameMessageOpcode.ServerMatchingMatchPvpStateInitial)]
[WireMessage(GameMessageOpcode.ServerMatchingMatchPvpStateInitial, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchPvpStateInitial : IWritable
{
    [UInt(2)] public MatchTeam Team { get; set; }
    public StateInfo State { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Team, 2u);
        State.Write(writer);
    }
}
