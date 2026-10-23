using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Asks for a unit's equipped items.
/// </summary>
/// <remarks>
/// The server answers with <see cref="InspectResponse"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0552, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class InspectRequest
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }
}
