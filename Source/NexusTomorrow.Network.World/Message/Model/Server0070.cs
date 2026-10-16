using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0070, Direction.ServerToClient)]
public sealed partial class Server0070
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
