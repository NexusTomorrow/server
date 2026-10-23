using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05F1, Direction.ServerToClient)]
public sealed partial class MatchingRoleCheckStarted
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
