using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.WorldEntry;

/// <summary>
/// Warns the player of a coming removal from the world. The client starts a countdown of its own and raises PendingWorldRemovalWarning
/// for its add-ons, whose interface shows the time left; when the countdown ends, the client takes the warning down.
/// </summary>
/// <remarks>
/// The client removes no one itself: the removal is the server's to make when its own time runs out, and
/// <see cref="WorldRemovalCancelled"/> calls it off.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0689, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WorldRemovalWarning
{
    /// <summary>
    /// The reason for the pending removal, which the client raises as its PendingWorldRemovalWarning event.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Reason { get; set; }
}
