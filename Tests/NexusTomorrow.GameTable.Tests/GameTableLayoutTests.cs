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
            writer.Write((ulong)(recordSize * records.Length));
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
        }

        stream.Position = 0;
        return stream;
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
#pragma warning restore CS0649
}
