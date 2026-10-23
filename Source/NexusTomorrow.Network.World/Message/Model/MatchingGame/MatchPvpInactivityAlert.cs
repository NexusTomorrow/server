using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05E6, Direction.ServerToClient)]
public sealed partial class MatchPvpInactivityAlert
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
