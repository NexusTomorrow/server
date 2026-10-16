using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server06DB, Direction.ServerToClient)]
public sealed partial class Server06DB
{
    [UInt(8)] public byte Unknown0 { get; set; }
    public Server024APart0 Unknown1 { get; set; }
    [UInt(64)] public ulong Unknown2 { get; set; }
}
