using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server06F7, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server06F7
{
    [UInt(15)] public ushort Unknown0 { get; set; }
    [UInt(1)] public bool Unknown1 { get; set; }
}
