using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

[WireMessage(GameMessageOpcode.Server05DC, Direction.ServerToClient)]
public sealed partial class Server05DC
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
