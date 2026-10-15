using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchPvpStateInitial, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchPvpStateInitial
{
    [UInt(2)] public MatchTeam Team { get; set; }
    public StateInfo State { get; set; }
}
