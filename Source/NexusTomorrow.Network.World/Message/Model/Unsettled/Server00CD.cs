using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server00CD, Direction.ServerToClient)]
public sealed partial class Server00CD
{
    [UInt(15)] public ushort Unknown0 { get; set; }
}
