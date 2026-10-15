using System.Runtime.InteropServices;
using System.Text;
using Newtonsoft.Json;
using NexusTomorrow.Shared;

namespace NexusTomorrow.GameTable;

public class TextTable
{
    public TextTableEntry[] Entries { get; }

    private readonly int[] lookup;

    [JsonConstructor]
    public TextTable(IEnumerable<TextTableEntry> entries)
    {
        Entries = [.. entries];
        lookup = BuildLookup(Entries);
    }

    public TextTable(string path)
    {
        using (FileStream stream = File.OpenRead(path))
            Entries = Read(stream);

        lookup = BuildLookup(Entries);
    }

    public TextTable(Stream stream)
    {
        Entries = Read(stream);
        lookup = BuildLookup(Entries);
    }

    private static TextTableEntry[] Read(Stream inputStream)
    {
        using (var stream = new BufferedStream(inputStream, 64 * 1024))
        using (var reader = new BinaryReader(stream, Encoding.Unicode))
        {
            if (reader.BaseStream.Remaining() < Marshal.SizeOf<TextTableHeader>())
                throw new InvalidDataException();

            // header
            ReadOnlySpan<TextTableHeader> headerSpan
                = MemoryMarshal.Cast<byte, TextTableHeader>(reader.ReadBytes(Marshal.SizeOf<TextTableHeader>()));

            TextTableHeader header = headerSpan[0];

            if (header.Signature != 0x4C544558)
                throw new InvalidDataException();

            // fields
            stream.Position = Marshal.SizeOf<TextTableHeader>() + (int)header.RecordOffset;

            ReadOnlySpan<TextTableField> fields = MemoryMarshal.Cast<byte, TextTableField>(
                reader.ReadBytes(Marshal.SizeOf<TextTableField>() * (int)header.RecordCount));

            return ReadEntries(reader, header, fields);
        }
    }

    private static TextTableEntry[] ReadEntries(BinaryReader reader, TextTableHeader header, ReadOnlySpan<TextTableField> fields)
    {
        var entries = new TextTableEntry[fields.Length];

        // build string table
        reader.BaseStream.Position = Marshal.SizeOf<TextTableHeader>() + (long)header.StringTableOffset;
        using (var stringTable = new StringTable(reader.ReadBytes((int)header.StringTableLength * 2)))
        {
            for (int i = 0; i < fields.Length; i++)
            {
                TextTableField field = fields[i];
                string @string = stringTable.GetEntry(field.Offset * 2);
                entries[i] = new TextTableEntry(header.Language, field.Id, @string);
            }
        }

        return entries;
    }

    private static int[] BuildLookup(TextTableEntry[] entries)
    {
        // final field will always have the max id
        int[] lookup = new int[entries[entries.Length - 1].Id + 1];
        for (int i = 0; i < lookup.Length; i++)
            lookup[i] = -1;

        for (int i = 0; i < entries.Length; i++)
        {
            TextTableEntry entry = entries[i];
            lookup[entry.Id] = i;
        }

        return lookup;
    }

    /// <summary>
    /// Return the text for the supplied id, or null when the table has none.
    /// </summary>
    public string? GetEntry(uint id)
    {
        if (id >= lookup.Length)
            return null;

        int lookupId = lookup[id];
        if (lookupId == -1)
            return null;

        return Entries[lookupId].Text;
    }
}
