using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server05F1, Direction.ServerToClient)]
public sealed partial class Server05F1
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
