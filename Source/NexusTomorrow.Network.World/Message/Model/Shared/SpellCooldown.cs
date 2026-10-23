using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

/// <summary>
/// One of the player's spell cooldowns.
/// </summary>
[WireStruct]
public sealed partial class SpellCooldown
{
    /// <summary>
    /// The cooldown's kind.
    /// </summary>
    [UInt(3)] public byte Kind { get; set; }

    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    [UInt(32)] public uint Unknown2 { get; set; }

    /// <summary>
    /// How long the cooldown lasts, in milliseconds.
    /// </summary>
    [UInt(32)] public uint Duration { get; set; }
}
