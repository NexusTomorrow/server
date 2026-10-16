using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server00FE, Direction.ServerToClient)]
public sealed partial class Server00FE
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
