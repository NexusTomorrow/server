using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05B8, Direction.ServerToClient)]
public sealed partial class MatchingEligibilityChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
