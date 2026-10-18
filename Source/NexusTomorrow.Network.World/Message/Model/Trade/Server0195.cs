using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

[WireMessage(GameMessageOpcode.Server0195, Direction.ServerToClient)]
public sealed partial class Server0195
{
    [UInt(64)] public ulong Unknown0 { get; set; }
}
