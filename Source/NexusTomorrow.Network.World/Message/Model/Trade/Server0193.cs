using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

[WireMessage(GameMessageOpcode.Server0193, Direction.ServerToClient)]
public sealed partial class Server0193
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
