using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server024B, Direction.ServerToClient)]
public sealed partial class Server024B
{
    public Server024APart0 Unknown0 { get; set; } = new();
    [UInt(32)] public uint Unknown1 { get; set; }
}
