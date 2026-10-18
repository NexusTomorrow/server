using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

[WireMessage(GameMessageOpcode.Server0689, Direction.ServerToClient)]
public sealed partial class Server0689
{
    [UInt(3)] public byte Unknown0 { get; set; }
}
