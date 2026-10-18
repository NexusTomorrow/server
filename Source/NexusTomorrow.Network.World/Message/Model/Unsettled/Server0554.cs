using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0554, Direction.ServerToClient)]
public sealed partial class Server0554
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
