using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Closes a spell's threshold.
/// </summary>
/// <remarks>
/// The client raises ClearSpellThreshold.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0814, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellThresholdClear
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [VerifiedMember, UInt(18)] public uint SpellId { get; set; }

    /// <summary>
    /// When set, the client clears all spell thresholds; when clear, only this spell's.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool ResetAll { get; set; }
}
