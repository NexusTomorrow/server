using System.Numerics;
using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class DecorInfo
{
    public Identity TargetResidence { get; } = new();
    [UInt(64)] public ulong DecorId { get; private set; }
    [UInt(32)] public DecorType DecorType { get; private set; }
    [UInt(16)] public ushort MannequinCostumeIndex { get; private set; }
    [UInt(16)] public ushort MannequinPoseId { get; private set; }
    [UInt(32)] public uint RecordFlags { get; private set; }
    [UInt(32)] public uint HookIndex { get; private set; }
    [UInt(32)] public uint PlotIndex { get; private set; }
    [Float32] public float Scale { get; private set; }
    [Float32] public Vector3 Position { get; private set; }
    [Float32] public Quaternion Rotation { get; private set; }
    [UInt(32)] public uint DecorInfoId { get; private set; }
    [UInt(32)] public uint ActivePropUnitId { get; private set; }
    [UInt(64)] public ulong ParentDecorId { get; private set; }
    [UInt(14)] public ushort ColourShiftId { get; private set; }
}
