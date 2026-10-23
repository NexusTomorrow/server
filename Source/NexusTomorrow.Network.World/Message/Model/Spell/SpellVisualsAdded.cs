using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Adds spell visuals.
/// </summary>
[WireMessage(GameMessageOpcode.Server0261, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class SpellVisualsAdded
{
    /// <summary>
    /// The visuals, each as <see cref="SpellVisualAdded"/> adds one.
    /// </summary>
    [Array(CountBits = 32)] public List<SpellVisualAdded> Visuals { get; set; } = [];
}
