using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ServerMatchingMatchPvpPoolUpdated, Direction.ServerToClient)]
public sealed partial class ServerMatchingMatchPvpPoolUpdated
{
    [UInt(32)] public uint LivesRemainingTeam1 { get; set; }
    [UInt(32)] public uint LivesRemainingTeam2 { get; set; }
}
