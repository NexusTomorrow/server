using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

/// <summary>
/// The item the server removed from an open player-to-player trade; the client drops it and refreshes the trade window.
/// </summary>
[WireMessage(GameMessageOpcode.Server0195, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerP2PTradeItemRemoved
{
    /// <summary>
    /// The item's id, matched against the trade's items so the client can remove it.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ItemId { get; set; }
}
