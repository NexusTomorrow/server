using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Renames a unit.
/// </summary>
/// <remarks>
/// The client raises UnitNameChanged.
/// </remarks>
[WireMessage(GameMessageOpcode.Server08CC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitNameChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's name; empty for its creature's name.
    /// </summary>
    [WideString] public string Name { get; set; } = "";
}
