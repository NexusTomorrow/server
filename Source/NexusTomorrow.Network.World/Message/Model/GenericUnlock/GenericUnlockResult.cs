using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.GenericUnlock;

/// <summary>
/// Reports a generic unlock's result; the client raises a GenericFloater for 0, 1 and 2 and a ConfirmPartialUnlock for 3.
/// </summary>
[WireMessage(GameMessageOpcode.Server0985, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class GenericUnlockResult
{
    /// <summary>
    /// The unlock result; 0, 1 and 2 each raise a GenericFloater with their own text, and 3 raises ConfirmPartialUnlock. The
    /// client ignores any other value.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Result { get; set; }
}
