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

// The seed's shape for a list of union arms: bare arms, each computing the kind written before it.
public sealed class ArmListMessage : IReadable, IWritable
{
    public List<IListArm> Entries { get; set; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Entries.Count, 3u);
        foreach (IListArm arm in Entries)
        {
            writer.Write(arm.Kind, 2u);
            arm.Write(writer);
        }
    }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt(3u);
        for (uint i = 0u; i < count; i++)
        {
            IListArm arm = ListArms.Create(reader.ReadByte(2u));
            arm.Read(reader);
            Entries.Add(arm);
        }
    }
}

// The seed's shape for one union arm whose kind the arm computes, so the message holds no selector member.
public sealed class ComputedSelectorMessage : IReadable, IWritable
{
    public IListArm Arm { get; set; } = new ListArmA();

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Arm.Kind, 2u);
        Arm.Write(writer);
    }

    public void Read(GamePacketReader reader)
    {
        Arm = ListArms.Create(reader.ReadByte(2u));
        Arm.Read(reader);
    }
}

public interface IListArm : IReadable, IWritable
{
    byte Kind { get; }
}

public static class ListArms
{
    public static IListArm Create(byte kind)
    {
        return kind switch
        {
            0 => new ListArmA(),
            1 => new ListArmB(),
            _ => throw new InvalidDataException($"No list arm has the kind {kind}."),
        };
    }
}

public sealed class ListArmA : IListArm
{
    public byte Kind => 0;

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

public sealed class ListArmB : IListArm
{
    public byte Kind => 1;

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

// The seed's shape for a payload union: every arm's fields flat on the holder, written by a switch on the kind, with no type per arm.
public sealed class FlatUnionMessage : IReadable, IWritable
{
    public byte Kind { get; set; }
    public byte A { get; set; }
    public byte B { get; set; }
    public byte C { get; set; }
    public byte Tail { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Kind, 1u);
        if (Kind == 0)
        {
            writer.Write(A, 4u);
        }
        else
        {
            writer.Write(B);
            writer.Write(C, 3u);
        }

        writer.Write(Tail);
    }

    public void Read(GamePacketReader reader)
    {
        Kind = reader.ReadByte(1u);
        if (Kind == 0)
        {
            A = reader.ReadByte(4u);
        }
        else
        {
            B = reader.ReadByte();
            C = reader.ReadByte(3u);
        }

        Tail = reader.ReadByte();
    }
}

// The seed's shape for a union kept as one member per arm, SidePlayer or SideCreature, written by the kind.
public sealed class SlotUnionMessage : IReadable, IWritable
{
    public byte SideKind { get; set; }
    public SlotPlayer? SidePlayer { get; set; }
    public SlotCreature? SideCreature { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(SideKind, 1u);
        if (SideKind == 0)
            SidePlayer!.Write(writer);
        else
            SideCreature!.Write(writer);
    }

    public void Read(GamePacketReader reader)
    {
        SideKind = reader.ReadByte(1u);
        if (SideKind == 0)
        {
            SidePlayer = new SlotPlayer();
            SidePlayer.Read(reader);
        }
        else
        {
            SideCreature = new SlotCreature();
            SideCreature.Read(reader);
        }
    }
}

public sealed class SlotPlayer : IReadable, IWritable
{
    public byte Level { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(Level, 4u);
    }

    public void Read(GamePacketReader reader)
    {
        Level = reader.ReadByte(4u);
    }
}

public sealed class SlotCreature : IReadable, IWritable
{
    public ushort CreatureId { get; set; }

    public void Write(GamePacketWriter writer)
    {
        writer.Write(CreatureId, 12u);
    }

    public void Read(GamePacketReader reader)
    {
        CreatureId = reader.ReadUShort(12u);
    }
}

// The seed's shape for columns: one list of records, each field written as a column.
public sealed class RowsMessage : IReadable, IWritable
{
    public List<RowEntry> Entries { get; } = [];

    public void Write(GamePacketWriter writer)
    {
        writer.Write((uint)Entries.Count, 3u);
        foreach (RowEntry entry in Entries)
            writer.Write(entry.Id);
        foreach (RowEntry entry in Entries)
            writer.Write(entry.Level, 4u);
    }

    public void Read(GamePacketReader reader)
    {
        uint count = reader.ReadUInt(3u);
        byte[] ids = new byte[count];
        for (uint i = 0u; i < count; i++)
            ids[i] = reader.ReadByte();
        foreach (byte id in ids)
            Entries.Add(new RowEntry { Id = id, Level = reader.ReadByte(4u) });
    }
}

// A struct, as some of the seed's records are, so the list hands out copies.
public struct RowEntry
{
    public byte Id { get; set; }
    public byte Level { get; set; }
}

public enum QuirkMode
{
    Fractional = 0,
    Whole = 1,
}

// Mismatches, each answered by one pinned rule: the value is named Amount where the declaration says Total, it is kept in a float, and an off-wire Mode
// that starts at Fractional picks its encoding.
public sealed class QuirkMessage : IWritable
{
    public QuirkMode Mode { get; set; }
    public float Amount { get; set; }

    public void Write(GamePacketWriter writer)
    {
        if (Mode == QuirkMode.Whole)
            writer.Write((uint)Amount);
        else
            writer.Write(Amount);
    }
}

public sealed class OneWayMessage : IReadable, IWritable
{
    public byte Value { get; set; }
    public byte Tail { get; set; }

    public void Write(GamePacketWriter writer)
    {
        // Mismatch: 9 bits where the declaration and the Read say 8.
        writer.Write((ushort)Value, 9u);
        writer.Write(Tail);
    }

    public void Read(GamePacketReader reader)
    {
        Value = reader.ReadByte();
        Tail = reader.ReadByte();
    }
}

public sealed class TrailingWidthMessage : IReadable, IWritable
{
    public byte Head { get; set; }
    public ushort Last { get; set; }

    // Mismatch: Last is 14 bits where the declaration says 13. Its high bit is always zero and both bodies fill 3 bytes, so the bytes alone agree.
    public void Write(GamePacketWriter writer)
    {
        writer.Write(Head);
        writer.Write(Last, 14u);
    }

    public void Read(GamePacketReader reader)
    {
        Head = reader.ReadByte();
        Last = reader.ReadUShort(14u);
    }
}
