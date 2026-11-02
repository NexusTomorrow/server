using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

/// <summary>
/// Reports a generic unlock's result; the client raises a GenericFloater for 0, 1 and 3 and a ConfirmPartialUnlock for 2.
/// </summary>
[WireMessage(GameMessageOpcode.Server0985, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericUnlockResult
{
    /// <summary>
    /// The unlock result; 0, 1 and 3 each raise a GenericFloater with their text, and 2 raises ConfirmPartialUnlock.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Result { get; set; }
}
