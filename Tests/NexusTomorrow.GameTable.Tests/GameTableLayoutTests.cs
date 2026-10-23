namespace NexusTomorrow.GameTable.Tests;

public sealed class GameTableLayoutTests
{
    private const ulong FirstCost = 0x0000_0001_0000_0002;
    private const ulong SecondCost = 0x0000_0003_0000_0004;

    /// <summary>
    /// A table file holding the given columns and records: the header, the column descriptors, then the records, with no strings.
    /// </summary>
    private static MemoryStream Table(DataType[] columns, int recordSize, params byte[][] records)
    {
        return TableWithStrings(columns, recordSize, [], records);
    }

    /// <summary>
    /// A table file holding the given columns and records followed by its string block; a string column's offset counts from
    /// the first record.
    /// </summary>
    private static MemoryStream TableWithStrings(DataType[] columns, int recordSize, byte[] strings, params byte[][] records)
    {
        const int fieldSize = 24;
        var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            writer.Write(0x4454424Cu);
            writer.Write(0u);
            writer.Write(0ul);
            writer.Write(0ul);
            writer.Write((ulong)recordSize);
            writer.Write((ulong)columns.Length);
            writer.Write(0ul);
            writer.Write((ulong)records.Length);
            writer.Write((ulong)((recordSize * records.Length) + strings.Length));
            writer.Write((ulong)(fieldSize * columns.Length));
            writer.Write((ulong)(records.Length + 1));
            writer.Write(0ul);
            writer.Write(0ul);

            foreach (DataType column in columns)
            {
                writer.Write(0ul);
                writer.Write(0ul);
                writer.Write((ushort)column);
                writer.Write((ushort)0);
                writer.Write(0u);
            }

            foreach (byte[] record in records)
            {
                Assert.Equal(recordSize, record.Length);
                writer.Write(record);
            }

            writer.Write(strings);
        }

        stream.Position = 0;
        return stream;
    }

    /// <summary>
    /// A string block holding each value as UTF-16 with a terminating zero, in order.
    /// </summary>
    private static byte[] WideStrings(params string[] values)
    {
        return [.. values.SelectMany(value => System.Text.Encoding.Unicode.GetBytes(value + '\0'))];
    }

    /// <summary>
    /// The bytes of one record, written field by field.
    /// </summary>
    private static byte[] Record(Action<BinaryWriter> write)
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream))
            write(writer);

        return stream.ToArray();
    }

    [Fact]
    public void AULongAfterAnOddCountOfFourByteColumnsReadsFromItsEightByteBoundary()
    {
        byte[] Row(uint id, ulong cost, uint after) => Record(writer =>
        {
            writer.Write(id);
            writer.Write(0u);
            writer.Write(cost);
            writer.Write(after);
            writer.Write(0u);
        });

        using MemoryStream stream = Table([DataType.UInt, DataType.ULong, DataType.UInt], 24, Row(1, FirstCost, 7), Row(2, SecondCost, 9));
        var table = new GameTable<OddColumnsEntry>(stream);

        Assert.Equal([(1u, FirstCost, 7u), (2u, SecondCost, 9u)], table.Entries.Select(entry => (entry.Id, entry.Cost, entry.After)));
    }

    [Fact]
    public void AULongArrayAfterAnOddCountOfFourByteColumnsReadsFromItsEightByteBoundary()
    {
        byte[] row = Record(writer =>
        {
            writer.Write(1u);
            writer.Write(0u);
            writer.Write(FirstCost);
            writer.Write(SecondCost);
            writer.Write(7u);
            writer.Write(0u);
        });

        using MemoryStream stream = Table([DataType.UInt, DataType.ULong, DataType.ULong, DataType.UInt], 32, row);
        OddColumnsArrayEntry entry = Assert.Single(new GameTable<OddColumnsArrayEntry>(stream).Entries);

        Assert.Equal([FirstCost, SecondCost], entry.Costs);
        Assert.Equal(7u, entry.After);
    }

    [Fact]
    public void AStringAfterAnOddCountOfFourByteColumnsLeavesTheLastColumnItsOwnValue()
    {
        // Two 24-byte records, then "Ab" at string offset 48 and "Cde" at 54.
        byte[] Row(uint id, ulong name, uint after) => Record(writer =>
        {
            writer.Write(id);
            writer.Write(0u);
            writer.Write(name);
            writer.Write(after);
            writer.Write(0u);
        });

        using MemoryStream stream = TableWithStrings([DataType.UInt, DataType.String, DataType.UInt], 24, WideStrings("Ab", "Cde"),
            Row(1, 48, 7), Row(2, 54, 9));
        var table = new GameTable<StringBeforeLastEntry>(stream);

        Assert.Equal([(1u, "Ab", 7u), (2u, "Cde", 9u)], table.Entries.Select(entry => (entry.Id, entry.Name, entry.After)));
    }

    [Fact]
    public void TwoStringsAfterAnOddCountOfFourByteColumnsLeaveTheLastColumnItsOwnValue()
    {
        // One 32-byte record, then "Ab" at string offset 32 and "Cde" at 38.
        byte[] row = Record(writer =>
        {
            writer.Write(1u);
            writer.Write(0u);
            writer.Write(32ul);
            writer.Write(38ul);
            writer.Write(7u);
            writer.Write(0u);
        });

        using MemoryStream stream = TableWithStrings([DataType.UInt, DataType.String, DataType.String, DataType.UInt], 32,
            WideStrings("Ab", "Cde"), row);
        TwoStringsEntry entry = Assert.Single(new GameTable<TwoStringsEntry>(stream).Entries);

        Assert.Equal(("Ab", "Cde", 7u), (entry.First, entry.Second, entry.After));
    }

    [Fact]
    public void AStringAfterAnEvenCountOfFourByteColumnsLeavesTheLastColumnItsOwnValue()
    {
        // One 24-byte record, then "Ab" at string offset 24.
        byte[] row = Record(writer =>
        {
            writer.Write(1u);
            writer.Write(5u);
            writer.Write(24ul);
            writer.Write(7u);
            writer.Write(0u);
        });

        using MemoryStream stream = TableWithStrings([DataType.UInt, DataType.UInt, DataType.String, DataType.UInt], 24,
            WideStrings("Ab"), row);
        AlignedStringEntry entry = Assert.Single(new GameTable<AlignedStringEntry>(stream).Entries);

        Assert.Equal((5u, "Ab", 7u), (entry.Before, entry.Name, entry.After));
    }

    // GameTable fills a row model's public fields through reflection, in the order typeof(T).GetFields() returns them.
#pragma warning disable CS0649
    internal sealed class OddColumnsEntry
    {
        public uint Id;
        public ulong Cost;
        public uint After;
    }

    internal sealed class OddColumnsArrayEntry
    {
        public uint Id;
        [GameTableFieldArray(2)]
        public ulong[] Costs = [];
        public uint After;
    }

    internal sealed class StringBeforeLastEntry
    {
        public uint Id;
        public string Name = "";
        public uint After;
    }

    internal sealed class TwoStringsEntry
    {
        public uint Id;
        public string First = "";
        public string Second = "";
        public uint After;
    }

    internal sealed class AlignedStringEntry
    {
        public uint Id;
        public uint Before;
        public string Name = "";
        public uint After;
    }
#pragma warning restore CS0649
}
