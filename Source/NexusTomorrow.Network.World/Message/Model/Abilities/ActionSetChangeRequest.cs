using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Abilities;

/// <summary>
/// Asks to change the current action set: the abilities on its bar, the pending tier changes and its AMPs.
/// </summary>
/// <remarks>
/// The client sends it from ActionSetLib.RequestActionSetChanges, after checking the request itself.
/// </remarks>
[WireMessage(GameMessageOpcode.Client00B1, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ActionSetChangeRequest
{
    /// <summary>
    /// One ability's pending tier change.
    /// </summary>
    [WireStruct]
    public sealed partial class TierChange
    {
        /// <summary>
        /// The ability whose tier changes, as a Spell4Base id.
        /// </summary>
        [VerifiedMember, UInt(18)] public uint Spell4BaseId { get; private set; }

        /// <summary>
        /// The tier asked for; the client sends only tiers below 10.
        /// </summary>
        [VerifiedMember, UInt(8)] public byte Tier { get; private set; }
    }

    /// <summary>
    /// The proposed action set: 12 Spell4Base ids, one per bar slot. The client refuses any other count (result 25), checks slots 0 to 7
    /// for class abilities and the rest for other kinds (result 24), and always sends 12.
    /// </summary>
    [VerifiedMember, Array(CountBits = 4), UInt(32)] public List<uint> Spell4BaseIds { get; } = [];

    /// <summary>
    /// The action set the request is for: the current one.
    /// </summary>
    [VerifiedMember, UInt(3)] public byte ActionSetIndex { get; private set; }

    /// <summary>
    /// The pending tier changes, one per ability changed.
    /// </summary>
    [VerifiedMember, Array(CountBits = 5)] public List<TierChange> TierChanges { get; } = [];

    /// <summary>
    /// The current action set's AMPs, then the pending ones, by EldanAugmentation id.
    /// </summary>
    [VerifiedMember, Array(CountBits = 7), UInt(16)] public List<ushort> EldanAugmentationIds { get; } = [];
}
