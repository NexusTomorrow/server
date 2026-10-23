using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Removes a spell visual.
/// </summary>
[WireMessage(GameMessageOpcode.Server0784, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SpellVisualRemoved
{
    /// <summary>
    /// The visual, by the id <see cref="SpellVisualAdded"/> gave it.
    /// </summary>
    [UInt(32)] public uint VisualId { get; set; }
}

