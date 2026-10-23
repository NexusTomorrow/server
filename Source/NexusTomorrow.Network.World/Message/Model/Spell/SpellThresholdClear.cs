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
[Verified(Rung.Purpose)]
public sealed partial class SpellThresholdClear
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
