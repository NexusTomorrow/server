using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Removes spell visuals.
/// </summary>
[WireMessage(GameMessageOpcode.Server077E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellVisualsRemoved
{
    /// <summary>
    /// The visuals, by the ids <see cref="SpellVisualAdded"/> gave them.
    /// </summary>
    [Array(CountBits = 32), UInt(32)] public List<uint> VisualIds { get; set; } = [];
}
