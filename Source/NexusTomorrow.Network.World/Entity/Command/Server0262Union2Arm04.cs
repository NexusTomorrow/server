using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[WireStruct]
[UnionCase(4)]
public sealed partial class Server0262Union2Arm04 : IServer0262Union2
{
    /// <summary>
    /// The points of the path the unit's position moves along, in order.
    /// </summary>
    [VerifiedMember, Array(CountBits = 10), Float32] public List<Vector3> Points { get; set; } = [];

    /// <summary>
    /// The unit's speed along the path, in units a second: the client paces the unit's position along <see cref="Points"/> by it, and
    /// fails the command unless it is above 0.00001.
    /// </summary>
    [VerifiedMember, PackedFloat] public float Speed { get; set; }

    [UInt(2)] public byte Unknown2 { get; set; }
    [UInt(4)] public byte Unknown3 { get; set; }
    [UInt(32)] public uint Unknown4 { get; set; }

    /// <summary>
    /// Whether the segment blends in: when set, on a unit the client does not move, the client blends away the jump to the segment; when
    /// clear, the unit snaps to it.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool Blend { get; set; }
}
