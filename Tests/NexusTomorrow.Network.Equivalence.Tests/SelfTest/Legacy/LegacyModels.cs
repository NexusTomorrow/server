using System.Numerics;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Equivalence.Tests.SelfTest.Legacy;

// Twins of the declarations in SelfTest.Declared, written against GamePacketWriter and GamePacketReader as the
// legacy models are. Each deliberate mismatch is marked where it sits.

public enum SelfTestKind : byte
{
    None = 0,
    First = 1,
    Last = 5,
}

public sealed class MatchingMessage : IReadable, IWritable
{
    public bool Flag { get; set; }
    public byte Small { get; set; }
    public ushort Medium { get; private set; }
    public ulong UniqueId { get; set; }
    public short Offset { get; set; }
    public SelfTestKind Kind { get; set; }
    public float Ratio { get; set; }
    public Vector3 Position { get; set; }
    public string Name { get; set; } = "";
    public MatchingPoint Origin { get; private set; } = new();
    public List<MatchingPoint> Points { get; } = [];
    public byte[] Slots { get; private set; } = new byte[3];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Flag);
        writer.Write(Small, 3u);
        writer.Write(Medium, 14u);
        writer.Write(UniqueId);
        writer.Write(unchecked((ushort)Offset), 10u);
        writer.Write(Kind, 3u);
        writer.Write(Ratio);
        writer.WritePackedVector3(Position);
        writer.WriteStringWide(Name);
        Origin.Write(writer);
        writer.Write((uint)Points.Count, 3u);
        foreach (MatchingPoint point in Points)
            point.Write(writer);
        foreach (byte slot in Slots)
            writer.Write(slot);
    }

    public void Read(GamePacketReader reader)
    {
        Flag = reader.ReadBit();
        Small = reader.ReadByte(3u);
        Medium = reader.ReadUShort(14u);
        UniqueId = reader.ReadULong();
        Offset = (short)(unchecked((short)(reader.ReadUShort(10u) << 6)) >> 6);
        Kind = reader.ReadEnum<SelfTestKind>(3u);
        Ratio = reader.ReadSingle();
        Position = reader.ReadPackedVector3();
        Name = reader.ReadWideString();
        Origin = new MatchingPoint();
        Origin.Read(reader);

        uint count = reader.ReadUInt(3u);
        for (uint i = 0u; i < count; i++)
        {
            var point = new MatchingPoint();
            point.Read(reader);
            Points.Add(point);
        }

        Slots = reader.ReadBytes(3u);
    }
}

public sealed class MatchingPoint : IReadable, IWritable
{
    public byte X { get; set; }
    public ushort Y { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(X);
        writer.Write(Y, 12u);
    }

    public void Read(GamePacketReader reader)
    {
        X = reader.ReadByte();
        Y = reader.ReadUShort(12u);
    }
}

public sealed class WidthMessage : IWritable
{
    public byte Value { get; set; }
    public ushort Tail { get; set; }

    public void Write(GamePacketWriter writer)
    {
        // Mismatch: 9 bits where the declaration says 8.
        writer.Write((ushort)Value, 9u);
        writer.Write(Tail);
    }
}

public sealed class ReaderMessage : IReadable, IWritable
{
    public ushort Value { get; set; }
    public byte Tail { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Value, 12u);
        writer.Write(Tail);
    }

    public void Read(GamePacketReader reader)
    {
        // Mismatch: 11 bits where Write puts 12.
        Value = reader.ReadUShort(11u);
        Tail = reader.ReadByte();
    }
}

public interface IUnionArm : IReadable, IWritable
{
}

public sealed class UnionMessage : IReadable, IWritable
{
    public byte Kind { get; set; }
    public IUnionArm Arm { get; set; } = new UnionArmA();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Kind, 2u);
        Arm.Write(writer);
    }

    public void Read(GamePacketReader reader)
    {
        Kind = reader.ReadByte(2u);
        Arm = Kind switch
        {
            0 => new UnionArmA(),
            1 => new UnionArmB(),
            2 => new UnionArmC(),
            _ => throw new InvalidDataException($"No arm has the kind {Kind}."),
        };
        Arm.Read(reader);
    }
}

public sealed class UnionArmA : IUnionArm
{
    public byte A { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(A, 4u);
    }

    public void Read(GamePacketReader reader)
    {
        A = reader.ReadByte(4u);
    }
}

public sealed class UnionArmB : IUnionArm
{
    public byte B { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(B);
    }

    public void Read(GamePacketReader reader)
    {
        B = reader.ReadByte();
    }
}

public sealed class UnionArmC : IUnionArm
{
    public byte C { get; set; }

    public void Write(GamePacketWriter writer)
    {
        // Mismatch: 7 bits where the declaration says 6.
        writer.Write(C, 7u);
    }

    public void Read(GamePacketReader reader)
    {
        C = reader.ReadByte(6u);
    }
}

public sealed class OrphanMessage : IReadable, IWritable
{
    public byte Kept { get; set; }

    // Mismatch: the declaration's Extra has no property here; its byte is written as zero and skipped on read.
    public void Write(GamePacketWriter writer)
    {
        writer.Write(Kept);
        writer.Write((byte)0);
    }

    public void Read(GamePacketReader reader)
    {
        Kept = reader.ReadByte();
        reader.ReadByte();
    }
}

public sealed class CountPadMessage : IReadable, IWritable
{
    public List<ushort> Keys { get; } = [];
    public uint[] Values { get; private set; } = [];

    // The count comes from Keys and the pad is a literal zero, so neither has a property to map.
    public void Write(GamePacketWriter writer)
    {
        writer.Write((uint)Keys.Count, 3u);
        foreach (ushort key in Keys)
            writer.Write(key);
        writer.Write(0u, 5u);
        foreach (uint value in Values)
            writer.Write(value);
    }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt(3u);
        for (uint i = 0u; i < count; i++)
            Keys.Add(reader.ReadUShort());
        reader.ReadByte(5u);
        Values = new uint[count];
        for (uint i = 0u; i < count; i++)
            Values[i] = reader.ReadUInt();
    }
}

public sealed class EmptyMessage : IWritable
{
    public uint TargetId { get; }
    public ushort Value { get; }

    public void Write(GamePacketWriter writer)
    {
        // Mismatch: Value is left out when TargetId is zero, which the empty instance always holds and a 32-bit draw
        // practically never does.
        writer.Write(TargetId);
        if (TargetId != 0u)
            writer.Write(Value);
    }
}

public sealed class BitArrayMessage : IReadable, IWritable
{
    public byte Kind { get; set; }
    public NetworkBitArray Flags { get; set; } = new(32, NetworkBitArray.BitOrder.LeastSignificantBit);

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Kind);
        writer.WriteBytes(Flags.GetBuffer());
    }

    public void Read(GamePacketReader reader)
    {
        Kind = reader.ReadByte();
        reader.ReadBytes(4u).CopyTo(Flags.GetBuffer(), 0);
    }
}

public sealed class ReversedBitArrayMessage : IReadable
{
    public NetworkBitArray Flags { get; } = new(32, NetworkBitArray.BitOrder.LeastSignificantBit);

    public void Read(GamePacketReader reader)
    {
        // Mismatch: the bytes land in the buffer last first.
        byte[] bytes = reader.ReadBytes(4u);
        Array.Reverse(bytes);
        bytes.CopyTo(Flags.GetBuffer(), 0);
    }
}
