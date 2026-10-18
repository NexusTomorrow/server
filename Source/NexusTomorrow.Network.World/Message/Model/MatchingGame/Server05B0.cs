using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05B0, Direction.ServerToClient)]
public sealed partial class Server05B0
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
