using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets a spell's charges.
/// </summary>
[WireMessage(GameMessageOpcode.Server0914, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellCharges
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }

    [UInt(32)] public uint Unknown1 { get; set; }
}
