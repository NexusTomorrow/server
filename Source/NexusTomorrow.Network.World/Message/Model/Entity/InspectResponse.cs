using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Answers an <see cref="InspectRequest"/> with a unit's equipped items.
/// </summary>
/// <remarks>
/// The client raises Inspect with the items, and averages their item levels into the unit's effective item level.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0553, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class InspectResponse
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's equipped items.
    /// </summary>
    [Array(CountBits = 5)] public List<ItemInstance> Items { get; set; } = [];
}
