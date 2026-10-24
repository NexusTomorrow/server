using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// Sets abilities' tiers in the player's ability book, each for one action set.
/// </summary>
/// <remarks>
/// The client adds an entry for an ability the book lacks, then raises AbilityBookChange.
/// </remarks>
[WireMessage(GameMessageOpcode.Server01A0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class AbilityBookChanged
{
    /// <summary>
    /// An ability's tier in one action set.
    /// </summary>
    [WireStruct]
    public sealed partial class AbilityBookEntry
    {
        /// <summary>
        /// The ability, as a Spell4Base id. The client looks up the ability's tier spells by it, and leaves no entry for a base it cannot
        /// build.
        /// </summary>
        [VerifiedMember, UInt(18)] public uint Spell4BaseId { get; set; }

        /// <summary>
        /// The ability's tier in that action set; 0 leaves it inactive.
        /// </summary>
        [VerifiedMember, UInt(4)] public byte Tier { get; set; }

        /// <summary>
        /// Which action set the tier is for, counted from 0. The client drops an entry whose index is at or above the ability's own count
        /// of action sets, which is 1 for some spells.
        /// </summary>
        [VerifiedMember, UInt(3)] public byte ActionSetIndex { get; set; }
    }

    /// <summary>
    /// The book's entries, each an ability's tier in one action set.
    /// </summary>
    [VerifiedMember, Array(CountBits = 32)] public List<AbilityBookEntry> Abilities { get; set; } = [];
}
