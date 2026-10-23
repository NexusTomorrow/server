using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the look and colour of items a unit shows, slot by slot.
/// </summary>
[WireMessage(GameMessageOpcode.Server0933, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitItemVisualsChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The items, each giving one slot of the unit's model its look and colour.
    /// </summary>
    [Array(CountBits = 32)] public List<ItemVisual> Items { get; set; } = [];
}
