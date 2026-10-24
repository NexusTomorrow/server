using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Reports a commodity order partly filled; the client raises CommodityAuctionFilledPartial from it.
/// </summary>
[WireMessage(GameMessageOpcode.Server024A, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CommodityOrderFilledPartial
{
    /// <summary>
    /// The order.
    /// </summary>
    public CommodityOrder Order { get; set; } = new();

    [UInt(32)] public uint Unknown1 { get; set; }
}
