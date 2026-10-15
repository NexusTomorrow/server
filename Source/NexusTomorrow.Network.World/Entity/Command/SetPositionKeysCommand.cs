using System.Numerics;
using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetPositionKeys)]
[WireStruct]
public sealed partial class SetPositionKeysCommand : IEntityCommandModel
{
    [UInt(10), Count] public ushort TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), Float32] public List<Vector3> Positions { get; set; } = [];
    [UInt(2)] public byte Type { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUShort(10u);
        for (int i = 0; i < count; i++)
            Times.Add(reader.ReadUInt());

        for (int i = 0; i < count; i++)
            Positions.Add(reader.ReadVector3());

        Type = reader.ReadByte(2u);
        Offset = reader.ReadUInt();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Positions.Count, 10u);
        foreach (uint time in Times)
            writer.Write(time);

        Positions.ForEach(writer.WriteVector3);

        writer.Write(Type, 2u);
        writer.Write(Offset);
        writer.Write(Blend);
    }
}
