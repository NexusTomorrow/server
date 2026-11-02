using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// The result of posting a commodity order; the client raises PostCommodityOrderResult from it.
/// </summary>
[WireMessage(GameMessageOpcode.Server06DB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CommodityOrderResult
{
    /// <summary>
    /// The result, a GameLib.CodeEnumGenericError code.
    /// </summary>
    [VerifiedMember, UInt(8)] public byte Result { get; set; }

    /// <summary>
    /// The order.
    /// </summary>
    [VerifiedMember] public CommodityOrder Order { get; set; } = new();

    /// <summary>
    /// The order's total amount in credits, which the client raises as the money part of PostCommodityOrderResult.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong Amount { get; set; }
}
