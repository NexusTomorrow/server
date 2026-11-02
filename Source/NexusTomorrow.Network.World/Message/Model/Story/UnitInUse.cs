using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Story;

/// <summary>
/// Sets whether a unit is in use.
/// </summary>
/// <remarks>
/// The client stores the flag on the unit; a newly in-use unit that owns an open dialog closes it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0755, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitInUse
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Whether the unit is in use; a change to in-use on the dialog's owner closes its dialog.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool InUse { get; set; }
}