using NexusTomorrow.GameTable;

namespace NexusTomorrow.Owners.Tests;

// Writes a table in the layout GameTable reads: the header, one descriptor per column, then the rows, with no strings. Every
// column here is four bytes wide.
internal static class TableFile
{
    private const uint Signature = 0x4454424C;
    private const int ColumnDescriptorBytes = 24;

    public static void Write(string path, DataType[] columns, params uint[][] rows)
    {
        ulong recordBytes = (ulong)columns.Length * sizeof(uint);
        using var writer = new BinaryWriter(File.Create(path));
        writer.Write(Signature);
        writer.Write(0u); // version
        writer.Write(0UL); // name length
        writer.Write(0UL); // unknown
        writer.Write(recordBytes);
        writer.Write((ulong)columns.Length);
        writer.Write(0UL); // the descriptors' offset from the end of the header
        writer.Write((ulong)rows.Length);
        writer.Write(recordBytes * (ulong)rows.Length); // every record and no string table
        writer.Write((ulong)(columns.Length * ColumnDescriptorBytes)); // the records' offset from the end of the header
        writer.Write((ulong)rows.Max(row => row[0]) + 1); // one past the highest id
        writer.Write(0UL); // lookup offset
        writer.Write(0UL); // unknown

        foreach (DataType column in columns)
        {
            writer.Write(0UL); // name length
            writer.Write(0UL); // name offset
            writer.Write((ushort)column);
            writer.Write((ushort)0);
            writer.Write(0u);
        }

        foreach (uint[] row in rows)
        {
            foreach (uint value in row)
                writer.Write(value);
        }
    }
}
