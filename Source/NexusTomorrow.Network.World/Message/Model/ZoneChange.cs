using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model;

/// <summary>
/// Tells the server the zone of the unit the client moves has changed.
/// </summary>
/// <remarks>
/// The client checks for a change once each period of <see cref="WorldSettings.MovementInterval"/> while it moves a unit. It also sends one
/// at once, with both values the same, when it first takes over the player's own unit and when control of that unit comes back after it was
/// taken away.
/// </remarks>
[WireMessage(GameMessageOpcode.Message063A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ZoneChange
{
    /// <summary>
    /// The zone id the client reported last, in <see cref="WorldReady"/> or an earlier <see cref="ZoneChange"/>; 0 when it has reported
    /// none since it entered the world.
    /// </summary>
    [UInt(15)] public ushort PreviousZoneId { get; private set; }

    /// <summary>
    /// The zone id of the unit the client moves, the id <see cref="WorldReady.ZoneId"/> also carries.
    /// </summary>
    [UInt(15)] public ushort ZoneId { get; private set; }
}
