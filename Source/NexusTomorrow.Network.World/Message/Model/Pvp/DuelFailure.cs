using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

/// <summary>
/// Reports a duel's failure with its reason; the client looks the reason up as text and shows it.
/// </summary>
[WireMessage(GameMessageOpcode.Server00EB, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class DuelFailure
{
    /// <summary>
    /// The duel failure reason, 1 to 18; the client looks up its text.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Reason { get; set; }
}
