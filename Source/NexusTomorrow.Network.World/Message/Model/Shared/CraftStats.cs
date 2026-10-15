using NexusTomorrow.Game.Static.Entity;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Shared;

[WireStruct]
public sealed partial class CraftStats : IReadable, IWritable
{
    [FixedArray(5), UInt(8)] public Property[] StatType { get; set; } = new Property[5];
    [Pad(8)] public Pad Reserved { get; }
    [UInt(8)] public byte Unknown { get; set; }
    [UInt(8)] public byte ApSpSplit { get; set; } // Attack Power and Support Power split
    [NotOnWire] public uint CircuitComplete { get; set; }

    public void Read(GamePacketReader reader)
    {
        ulong temp = reader.ReadULong();

        for (int i = 0; i < StatType.Length; i++)
        {
            StatType[i] = (Property)(temp & 0xFF);
            temp >>= 8;
        }

        temp >>= 8;
        Unknown = (byte)(temp & 0xFF);

        temp >>= 8;
        ApSpSplit = (byte)(temp & 0xFF);

        temp >>= 8;
        CircuitComplete = (uint)(temp & 0xFFFFFFFF);
    }

    public void Write(GamePacketWriter writer)
    {
        ulong temp = 0;

        for (int i = 0; i < 5; i++)
        {
            temp |= (ulong)StatType[i];
            temp <<= 8;
        }

        temp |= (ulong)Unknown;
        temp <<= 8;

        temp |= (ulong)ApSpSplit;
        temp <<= 8;

        temp |= (ulong)CircuitComplete;
        temp <<= 8;

        writer.Write(temp);
    }
}
