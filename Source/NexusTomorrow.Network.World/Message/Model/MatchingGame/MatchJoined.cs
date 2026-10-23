using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05C6, Direction.ServerToClient)]
public sealed partial class MatchJoined
{
    [UInt(14)] public uint Unknown0 { get; set; }
}
