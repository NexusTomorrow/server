using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// Sets the player's ability points.
/// </summary>
/// <remarks>
/// GameLib.GetTotalAbilityPoints reports <see cref="TotalAbilityPoints"/>, and GameLib.GetAbilityPoints reports
/// <see cref="AbilityPoints"/>, or a working copy while tier changes are pending. AbilityBook.UpdateSpellTier limits a raise to what
/// <see cref="AbilityPoints"/> pays for. The client raises AbilityBookChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0169, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AbilityPointsChanged
{
    /// <summary>
    /// The ability points the player has.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint AbilityPoints { get; set; }

    /// <summary>
    /// The most ability points the player can have.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint TotalAbilityPoints { get; set; }
}
