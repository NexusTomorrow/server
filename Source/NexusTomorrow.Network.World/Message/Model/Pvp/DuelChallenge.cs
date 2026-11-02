using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

/// <summary>
/// Tells the client a duel has been challenged between two players, which it announces to the relevant channel.
/// </summary>
[WireMessage(GameMessageOpcode.Server0896, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DuelChallenge
{
    /// <summary>
    /// One of the duel's participants, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The duel's other participant, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint OtherUnitId { get; set; }
}
