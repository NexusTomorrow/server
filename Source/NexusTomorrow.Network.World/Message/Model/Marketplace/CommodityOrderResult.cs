using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

[WireMessage(GameMessageOpcode.Server06DB, Direction.ServerToClient)]
public sealed partial class CommodityOrderResult
{
    [UInt(8)] public byte Unknown0 { get; set; }
    public CommodityOrder Unknown1 { get; set; } = new();
    [UInt(64)] public ulong Unknown2 { get; set; }
}
