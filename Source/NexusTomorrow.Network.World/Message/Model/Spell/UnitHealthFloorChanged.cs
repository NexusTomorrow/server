using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Sets a unit's health floor.
/// </summary>
/// <remarks>
/// The client raises RefreshHealthFloor.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0940, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class UnitHealthFloorChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The unit's health floor.
    /// </summary>
    [UInt(32)] public uint HealthFloor { get; set; }
}
