using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Tells the client which residence the player is in, and its privacy.
/// </summary>
/// <remarks>
/// The client raises HousingPrivacyUpdated with bit 0 of the privacy flags.
/// </remarks>
[WireMessage(GameMessageOpcode.Server010E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class HousingBasics
{
    /// <summary>
    /// The residence the player is in, by its id within the realm.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong ResidenceId { get; set; }

    /// <summary>
    /// The residence's plot, by its id within the realm.
    /// </summary>
    [VerifiedMember, UInt(64)] public ulong PlotId { get; set; }

    /// <summary>
    /// The residence's privacy: bit 0 private, bit 1 neighbors only, bit 2 roommates only, none public, which
    /// Residence:GetResidencePrivacyLevel reports as a HousingLib.ResidencePrivacyLevel.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint PrivacyFlags { get; set; }
}
