using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Static;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server0565, Direction.ServerToClient)]
public sealed partial class Server0565
{
    public Server055FPart0 Unknown0 { get; set; }
    [UInt(8)] public byte Unknown1 { get; set; }
}
