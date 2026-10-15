using System.Numerics;
using NexusTomorrow.Game.Static.Housing;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

[WireStruct]
public sealed partial class DecorInfo : IReadable
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

    public void Read(GamePacketReader reader)
    {
        TargetResidence.Read(reader);
        DecorId = reader.ReadULong();
        DecorType = reader.ReadEnum<DecorType>(32u);

        uint temp = reader.ReadUInt();
        MannequinCostumeIndex = (ushort)(temp & 0xFFFF);
        MannequinPoseId = (ushort)(temp >> 16);

        RecordFlags = reader.ReadUInt();
        HookIndex = reader.ReadUInt();
        PlotIndex = reader.ReadUInt();
        Scale = reader.ReadSingle();
        Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        Rotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        DecorInfoId = reader.ReadUInt();
        ActivePropUnitId = reader.ReadUInt();
        ParentDecorId = reader.ReadULong();
        ColourShiftId = reader.ReadUShort(14u);
    }
}
