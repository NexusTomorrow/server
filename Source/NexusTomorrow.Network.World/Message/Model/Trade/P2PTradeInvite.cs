using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

/// <summary>
/// Invites the player to a peer trade; the client stores the inviter and raises P2PTradeInvite.
/// </summary>
[WireMessage(GameMessageOpcode.Server0193, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class P2PTradeInvite
{
    /// <summary>
    /// The unit inviting the trade, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint InviterUnitId { get; set; }
}
