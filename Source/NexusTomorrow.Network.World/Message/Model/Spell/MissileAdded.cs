using System.Numerics;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Spell;

/// <summary>
/// Launches a missile from one unit to another.
/// </summary>
[WireMessage(GameMessageOpcode.Server025F, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class MissileAdded
{
    /// <summary>
    /// The missile, a Spell4ClientMissile table id.
    /// </summary>
    [UInt(16)] public ushort Spell4ClientMissileId { get; set; }

    /// <summary>
    /// The unit the missile flies from, by unit id.
    /// </summary>
    [UInt(32)] public uint SourceUnitId { get; set; }

    [Float32] public Vector3 Unknown2 { get; set; }

    /// <summary>
    /// The unit the missile flies to, by unit id.
    /// </summary>
    [UInt(32)] public uint TargetUnitId { get; set; }

    [Float32] public Vector3 Unknown4 { get; set; }

    /// <summary>
    /// The missile's id, which removes it.
    /// </summary>
    [UInt(32)] public uint MissileId { get; set; }

    [UInt(32)] public uint Unknown6 { get; set; }
}
