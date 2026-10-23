using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.PlayerPath;

/// <summary>
/// Raises SoldierHoldoutDeath for a soldier event.
/// </summary>
/// <remarks>
/// The client also clears the sound zone kit the event's wave set.
/// </remarks>
[WireMessage(GameMessageOpcode.Server067F, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class SoldierHoldoutDeath
{
    /// <summary>
    /// The soldier event, a PathSoldierEvent table id.
    /// </summary>
    [UInt(14)] public ushort SoldierEventId { get; set; }
}
