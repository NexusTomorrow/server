using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0640, Direction.ServerToClient)]
public sealed partial class Server0640
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
