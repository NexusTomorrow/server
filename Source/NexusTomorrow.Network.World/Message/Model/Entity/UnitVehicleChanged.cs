using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the vehicle a unit is in.
/// </summary>
/// <remarks>
/// The client raises Mount when the unit is the player.
/// </remarks>
[WireMessage(GameMessageOpcode.Server089B, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitVehicleChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    /// <summary>
    /// The vehicle, by unit id.
    /// </summary>
    [UInt(32)] public uint VehicleUnitId { get; set; }

    [UInt(2)] public byte Unknown2 { get; set; }
    [UInt(3)] public byte Unknown3 { get; set; }
}
