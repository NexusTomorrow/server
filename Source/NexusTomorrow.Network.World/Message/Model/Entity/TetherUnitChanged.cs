using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets which unit the player's controlled unit is tethered to.
/// </summary>
/// <remarks>
/// The client keeps the id on the controlled unit, and ignores the message while the player controls none. Its movement
/// control looks the unit up by this id and uses the unit it finds. When control passes to another unit, the client
/// carries the id over to it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0881, Direction.ServerToClient)]
[Verified(Rung.Fields)]
[LayoutConfirmed]
public sealed partial class TetherUnitChanged
{
    /// <summary>
    /// The unit tethered to, by unit id.
    /// </summary>
    [UInt(32)] public uint TetherUnitId { get; set; }
}
