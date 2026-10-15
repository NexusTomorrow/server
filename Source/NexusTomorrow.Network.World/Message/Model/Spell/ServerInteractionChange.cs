using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

// Switches interaction to the specified unit.
// If this differs from the clients current InteractionUnitId,
// changes the active InteractionType to none.
[WireMessage(GameMessageOpcode.ServerInteractionChange, Direction.ServerToClient)]
public sealed partial class ServerInteractionChange
{
    [UInt(32)] public uint InteractionUnitId { get; set; } = 0;
}
