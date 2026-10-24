using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// Applies an action to one hazard. The client needs the hazard's <see cref="HazardStates"/> record first, and raises the hazard's events
/// for its add-ons, which build, redraw or remove the hazard's bar and its minimap marker.
/// </summary>
[WireMessage(GameMessageOpcode.Server0109, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HazardAction
{
    /// <summary>
    /// The hazard, by id in the Hazard table. The client looks up the hazard's row and its <see cref="HazardState"/> record by it.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint HazardId { get; set; }

    [UInt(3)] public byte Unknown1 { get; set; }
}
