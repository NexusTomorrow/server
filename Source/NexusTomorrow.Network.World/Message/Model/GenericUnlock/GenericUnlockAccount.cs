using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

/// <summary>
/// Unlocks an account generic unlock entry; the client looks the entry up in its GenericUnlockEntry table.
/// </summary>
[WireMessage(GameMessageOpcode.Server0982, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericUnlockAccount
{
    /// <summary>
    /// The generic unlock entry, by id; the client looks it up in its GenericUnlockEntry table.
    /// </summary>
    [VerifiedMember, UInt(14)] public ushort UnlockEntryId { get; set; }
}
