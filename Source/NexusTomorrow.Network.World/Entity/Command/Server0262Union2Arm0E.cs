using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

/// <summary>
/// Command 14: the unit's facing.
/// </summary>
[WireStruct]
[UnionCase(14)]
public sealed partial class Server0262Union2Arm0E : IServer0262Union2
{
    /// <summary>
    /// The facing, as three values. The client sends its yaw first, in radians, as the angle atan2(-x, -z) of the direction it faces, and 0
    /// for the other two.
    /// </summary>
    [Float32] public Vector3 Unknown0 { get; set; }

    [UInt(1)] public bool Unknown1 { get; set; }
}
