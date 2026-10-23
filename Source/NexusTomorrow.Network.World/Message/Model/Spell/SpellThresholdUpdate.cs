using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Moves a spell's threshold to a new tier.
/// </summary>
/// <remarks>
/// The client raises UpdateSpellThreshold with the tier plus one.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0817, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellThresholdUpdate
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    /// <summary>
    /// The tier, from 0.
    /// </summary>
    [UInt(8)] public byte Tier { get; set; }
}
