using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Tells the player a unit evaded.
/// </summary>
/// <remarks>
/// The client raises UnitEvaded for reason 4.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0897, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitEvaded
{
    /// <summary>
    /// The unit that evaded, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// Why the unit evaded.
    /// </summary>
    [UInt(32)] public uint Reason { get; set; }
}
