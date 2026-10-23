using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Server0986, Direction.ServerToClient)]
[Dormant]
public sealed partial class Server0986
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
