using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets values on a unit, each a base and a current value under a property id.
/// </summary>
/// <remarks>
/// The client sets each entry as it sets those of <see cref="UnitCreate.Properties"/>, and for the player's unit it then recomputes
/// its property hash. An id above 196 ends the message there: the client sets no later entry and skips the recompute.
/// </remarks>
[WireMessage(GameMessageOpcode.Server093A, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitPropertiesChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The properties to set, each a base and a current value under its property id.
    /// </summary>
    [Array(CountBits = 8)] public List<UnitPropertyValue> Properties { get; set; } = [];
}
