using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server01AB, Direction.ServerToClient)]
public sealed partial class Server01AB
{
    [UInt(8)] public byte Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
