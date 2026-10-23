using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Resurrects a unit, the player included, the way the player chose.
/// </summary>
[WireMessage(GameMessageOpcode.Client015A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class Resurrect
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; private set; }

    /// <summary>
    /// The way the player chose to resurrect.
    /// </summary>
    [UInt(7)] public ResurrectionType RezType { get; private set; }

    /// <summary>
    /// The value the resurrection takes; the client sends 0 with <see cref="ResurrectionType.WakeHereServiceToken"/>.
    /// </summary>
    [UInt(32)] public uint RezData { get; private set; }
}
