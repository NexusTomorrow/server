using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.Server0594, Direction.ServerToClient)]
public sealed partial class Server0594
{
    [UInt(1)] public bool Unknown0 { get; set; }
    [UInt(5)] public byte Unknown1 { get; set; }
}
