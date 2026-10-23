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
    /// The resurrection type the player chose, 64 for waking where they fell.
    /// </summary>
    [UInt(7)] public byte RezType { get; private set; }

    /// <summary>
    /// The value the resurrection type takes; the client sends 0 with type 64.
    /// </summary>
    [UInt(32)] public uint RezData { get; private set; }
}
