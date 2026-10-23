using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Releases the threshold a spell opened with <see cref="SpellThresholdStart"/>.
/// </summary>
[WireMessage(GameMessageOpcode.Server0815, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellThresholdCancel
{
    /// <summary>
    /// The spell, a Spell4 table id.
    /// </summary>
    [UInt(18)] public uint SpellId { get; set; }
}
