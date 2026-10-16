using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server05C6, Direction.ServerToClient)]
public sealed partial class Server05C6
{
    [UInt(14)] public uint Unknown0 { get; set; }
}
