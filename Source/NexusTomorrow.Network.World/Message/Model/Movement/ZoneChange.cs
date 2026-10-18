using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;

namespace NexusTomorrow.Network.World.Message.Model.Movement;

/// <summary>
/// Tells the server the zone of the unit the client moves when it changes, and on some takeovers of the player's own unit repeats the last
/// one it reported.
/// </summary>
/// <remarks>
/// The client checks for a change each time its movement timer runs while it moves a unit; the timer's period is
/// <see cref="WorldSettings.MovementInterval"/>. It also sends one at once, with both values the same, when a <see cref="MovementControl"/>
/// names the player's own unit while the unit the client moves, or last moved, carries the mark <see cref="UnitMovement.Unknown3"/>
/// describes, as on the client's first takeover of that unit and after a <see cref="MovementControlEnd"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Message063A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class ZoneChange
{
    /// <summary>
    /// The zone id the client reported last, in <see cref="WorldReady"/> or an earlier <see cref="ZoneChange"/>; 0 when it has reported
    /// none since <see cref="Pregame.EnterWorld"/>.
    /// </summary>
    [UInt(15)] public ushort PreviousZoneId { get; private set; }

    /// <summary>
    /// The zone id of the unit the client moves when the client sends a change, the id <see cref="WorldReady.ZoneId"/> also carries. In the
    /// one the client sends at once on taking control it repeats <see cref="PreviousZoneId"/>, and so can be 0.
    /// </summary>
    [UInt(15)] public ushort ZoneId { get; private set; }
}
