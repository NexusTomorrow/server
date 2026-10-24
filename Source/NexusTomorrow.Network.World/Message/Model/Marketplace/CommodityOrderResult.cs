using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The result of posting a commodity order; the client raises PostCommodityOrderResult from it.
/// </summary>
[WireMessage(GameMessageOpcode.Server06DB, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class CommodityOrderResult
{
    /// <summary>
    /// The result, a GameLib.CodeEnumGenericError code.
    /// </summary>
    [UInt(8)] public byte Result { get; set; }

    /// <summary>
    /// The order.
    /// </summary>
    public CommodityOrder Order { get; set; } = new();

    [UInt(64)] public ulong Unknown2 { get; set; }
}
