using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Destroys a unit.
/// </summary>
[WireMessage(GameMessageOpcode.Server0355, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitDestroyed
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
