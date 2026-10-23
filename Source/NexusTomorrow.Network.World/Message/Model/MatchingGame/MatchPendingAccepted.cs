using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05CC, Direction.ServerToClient)]
public sealed partial class MatchPendingAccepted
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
