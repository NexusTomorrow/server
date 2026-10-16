using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server098A, Direction.ServerToClient)]
public sealed partial class Server098A
{
    [UInt(5)] public byte Unknown0 { get; set; }
}
