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
    /// The client gives this value no meaning. Every caller the client ships passes 0, and the wake-here sender writes 0.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Unknown2 { get; private set; }
}
