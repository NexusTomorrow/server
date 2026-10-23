using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The marketplace status, which the client keeps, raising CommodityAuctionsDisabled or ItemAuctionsDisabled as it changes.
/// </summary>
[WireMessage(GameMessageOpcode.Server0640, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MarketplaceStatusChanged
{
    [UInt(32)] public uint Unknown0 { get; set; }
}
