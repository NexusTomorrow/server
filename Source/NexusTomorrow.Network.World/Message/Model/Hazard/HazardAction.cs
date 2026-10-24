using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Hazard;

/// <summary>
/// Raises hazard events for the client's add-ons. Except for one value of <see cref="Unknown1"/>, which clears every hazard and ignores
/// <see cref="HazardId"/>, the client needs the hazard's Hazard row and its <see cref="HazardState"/> first.
/// </summary>
[WireMessage(GameMessageOpcode.Server0109, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HazardAction
{
    /// <summary>
    /// The hazard, by id in the Hazard table. Except for the value of <see cref="Unknown1"/> that clears every hazard, the client looks up the
    /// hazard's row and its <see cref="HazardState"/> record by it.
    /// </summary>
    [VerifiedMember, UInt(14)] public uint HazardId { get; set; }

    [UInt(3)] public byte Unknown1 { get; set; }
}
