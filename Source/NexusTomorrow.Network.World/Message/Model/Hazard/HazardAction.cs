using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// For some values of <see cref="Action"/>, raises hazard events for the client's add-ons. Except for one value of
/// <see cref="Action"/>, which clears every hazard and ignores <see cref="HazardId"/>, the client needs the hazard's Hazard row and
/// its <see cref="HazardState"/> first.
/// </summary>
[WireMessage(GameMessageOpcode.Server0109, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HazardAction
{
    /// <summary>
    /// The hazard, by id in the Hazard table. Except for the value of <see cref="Action"/> that clears every hazard, the client looks up the
    /// hazard's row and its <see cref="HazardState"/> record by it.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint HazardId { get; set; }

    /// <summary>
    /// What to do with the hazard: enable it, remove it, defer it, or clear every hazard (which ignores <see cref="HazardId"/>).
    /// </summary>
    [VerifiedMember, UInt(3)] public byte Action { get; set; }
}
