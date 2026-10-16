using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0560, Direction.ServerToClient)]
public sealed partial class Server0560
{
    [UInt(8)] public byte Unknown0 { get; set; }
    public Server055FPart0 Unknown1 { get; set; } = new();
}
