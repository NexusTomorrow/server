using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server05E1, Direction.ServerToClient)]
public sealed partial class Server05E1
{
    [UInt(6)] public byte Unknown0 { get; set; }
    [UInt(4)] public byte Unknown1 { get; set; }
}
