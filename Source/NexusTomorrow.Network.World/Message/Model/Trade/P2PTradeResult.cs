using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Trade;

/// <summary>
/// Reports the result of a player-to-player trade, which the client raises as P2PTradeResult and, when the flag is set, tears down the trade.
/// </summary>
[WireMessage(GameMessageOpcode.Server0196, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class P2PTradeResult
{
    /// <summary>
    /// The result, which the client switches on to set the trade state, look up the partner, and pick the message it announces.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Result { get; set; }

    /// <summary>
    /// When set, the client cancels the trade, raising P2PCancelTrade and freeing the trade session.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Cancel { get; set; }
}