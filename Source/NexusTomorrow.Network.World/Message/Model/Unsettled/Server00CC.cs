using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server00CC, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server00CC
{
    [UInt(15)] public ushort Unknown0 { get; set; }
}
