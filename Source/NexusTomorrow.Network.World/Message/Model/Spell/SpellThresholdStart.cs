using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Opens a spell's threshold.
/// </summary>
/// <remarks>
/// The client raises StartSpellThreshold.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0816, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellThresholdStart
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    [UInt(18)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The spell whose cooldown starts when the threshold closes.
    /// </summary>
    [UInt(18)] public uint CooldownSpellId { get; set; }

    [UInt(32)] public uint Unknown3 { get; set; }
}
