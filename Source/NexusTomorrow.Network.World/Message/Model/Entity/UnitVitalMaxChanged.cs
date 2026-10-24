using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Entity;

/// <summary>
/// Sets the maximum of one of a unit's vitals.
/// </summary>
[WireMessage(GameMessageOpcode.Server093D, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class UnitVitalMaxChanged
{
    /// <summary>
    /// The unit, by unit id.
    /// </summary>
    [UInt(32)] public uint UnitId { get; set; }

    [UInt(5)] public byte Unknown1 { get; set; }
    [Float32] public float Unknown2 { get; set; }

    /// <summary>
    /// The maximum.
    /// </summary>
    [Float32] public float Max { get; set; }
}
