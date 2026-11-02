using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Reputation;

/// <summary>
/// Raises the player's reputation with a faction by an amount, which the client reports and alerts on when it crosses a threshold.
/// </summary>
[WireMessage(GameMessageOpcode.Server01A5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ReputationUpdate
{
    /// <summary>
    /// The faction, a Faction2 table id.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort FactionId { get; set; }

    /// <summary>
    /// How much reputation the faction gains or loses.
    /// </summary>
    [VerifiedMember, Float32] public float Amount { get; set; }
}