using NexusTomorrow.Game.Static.Entity.Movement.Command;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.World.Entity.Command;

[EntityCommand(EntityCommand.SetScaleKeys)]
[WireStruct]
public sealed partial class SetScaleKeysCommand : IEntityCommandModel
{
    [UInt(8), Count] public byte TimesCount { get; set; }
    [Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times { get; set; } = [];
    [Array(CountFrom = nameof(TimesCount)), PackedFloat] public List<float> Scales { get; set; } = [];
    [UInt(2)] public byte Type { get; set; }
    [UInt(32)] public uint Offset { get; set; }
    [UInt(1)] public bool Blend { get; set; }

    public void Read(GamePacketReader reader)
    {
        byte count = reader.ReadByte();

        for (int i = 0; i < count; i++)
            Times.Add(reader.ReadUInt());

        for (int i = 0; i < count; i++)
            Scales.Add(reader.ReadPackedFloat());

        Type = reader.ReadByte(2u);
        Offset = reader.ReadUInt();
        Blend = reader.ReadBit();
    }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Times.Count, 8u);
        foreach (uint time in Times)
            writer.Write(time);

        foreach (float scale in Scales)
            writer.WritePackedFloat(scale);

        writer.Write(Type, 2u);
        writer.Write(Offset);
        writer.Write(Blend);
    }
}
