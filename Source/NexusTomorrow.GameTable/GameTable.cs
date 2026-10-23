using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using NexusTomorrow.Shared;

namespace NexusTomorrow.GameTable;

public class GameTable<T> where T : class, new()
{
    private const int MinimumBufferSize = 1024;
    private const int MaximumBufferSize = 16 * 1024;
    private const int RecordSizeMultiplier = 32; // How many records do we want buffered.

    public T[] Entries { get; }

    private readonly GameTableHeader _header;
    private readonly int[] _lookup;

    private static readonly Dictionary<FieldInfo, GameTableFieldArrayAttribute?> s_attributeCache;
    private static readonly int s_headerSize;
    private static readonly int s_fieldSize;
    private static readonly int s_bufferSize;

    static GameTable()
    {
        s_attributeCache = [];
        foreach (FieldInfo modelField in typeof(T).GetFields())
        {
            GameTableFieldArrayAttribute? attribute = modelField.GetCustomAttribute<GameTableFieldArrayAttribute>();
            s_attributeCache.Add(modelField, attribute);
        }

        s_headerSize = Marshal.SizeOf<GameTableHeader>();
        s_fieldSize = Marshal.SizeOf<GameTableField>();

        s_bufferSize = s_fieldSize * RecordSizeMultiplier;
        if (s_bufferSize < MinimumBufferSize)
            s_bufferSize = MinimumBufferSize;
        if (s_bufferSize > MaximumBufferSize)
            s_bufferSize = MaximumBufferSize;
    }

    public GameTable(string path)
    {
        using (FileStream stream = File.OpenRead(path))
            (_header, Entries, _lookup) = Read(stream);
    }

    public GameTable(Stream stream)
    {
        (_header, Entries, _lookup) = Read(stream);
    }

    private static (GameTableHeader Header, T[] Entries, int[] Lookup) Read(Stream inputStream)
    {
        using (var stream = new BufferedStream(inputStream, s_bufferSize))
        using (var reader = new BinaryReader(stream))
        {
            if (reader.BaseStream.Remaining() < Marshal.SizeOf<GameTableHeader>())
                throw new InvalidDataException();

            // header
            ReadOnlySpan<GameTableHeader> headerSpan
                = MemoryMarshal.Cast<byte, GameTableHeader>(reader.ReadBytes(s_headerSize));

            GameTableHeader header = headerSpan[0];

            if (header.Signature != 0x4454424C)
                throw new InvalidDataException();

            // fields
            stream.Position = s_headerSize + (int)header.FieldOffset;
            ReadOnlySpan<GameTableField> fields = MemoryMarshal.Cast<byte, GameTableField>(
                reader.ReadBytes(s_fieldSize * (int)header.FieldCount));

            ValidateModelFields(fields);

            (T[] entries, int[] lookup) = ReadEntries(reader, header, fields);
            return (header, entries, lookup);
        }
    }

    private static Type GetElementType(FieldInfo modelField)
    {
        return modelField.FieldType.GetElementType()
            ?? throw new InvalidOperationException($"GameTable model array field {modelField.Name} has no element type.");
    }

    [Conditional("DEBUG")]
    private static void ValidateModelFields(ReadOnlySpan<GameTableField> fields)
    {
        FieldInfo[] modelFields = [.. s_attributeCache.Keys];
        if (modelFields.Length != fields.Length)
        {
            // models with arrays will not have the correct number of fields, compensate for this
            uint fieldCount = 0u;
            foreach (FieldInfo modelField in modelFields)
            {
                GameTableFieldArrayAttribute? attribute = s_attributeCache[modelField];
                if (attribute != null)
                    fieldCount += attribute.Length;
                else
                    fieldCount++;
            }

            if (fieldCount != fields.Length)
                throw new GameTableException($"GameTable model has an invalid field count {modelFields.Length}! Expected: {fields.Length}");
        }

        void ValidateModelField(DataType dataType, Type modelFieldType)
        {
            switch (dataType)
            {
                case DataType.UInt:
                    if (modelFieldType != typeof(uint) && !modelFieldType.IsEnum)
                        throw new GameTableException($"Invalid GameTable model field type {modelFieldType}! Expected: {typeof(uint)}");
                    break;
                case DataType.Single:
                    if (modelFieldType != typeof(float))
                        throw new GameTableException($"Invalid GameTable model field type {modelFieldType}! Expected: {typeof(float)}");
                    break;
                case DataType.Boolean:
                    if (modelFieldType != typeof(bool))
                        throw new GameTableException($"Invalid GameTable model field type {modelFieldType}! Expected: {typeof(bool)}");
                    break;
                case DataType.ULong:
                    if (modelFieldType != typeof(ulong))
                        throw new GameTableException($"Invalid GameTable model field type {modelFieldType}! Expected: {typeof(ulong)}");
                    break;
                case DataType.String:
                    if (modelFieldType != typeof(string))
                        throw new GameTableException($"Invalid GameTable model field type {modelFieldType}! Expected: {typeof(string)}");
                    break;
                default:
                    throw new GameTableException($"Unknown GameTable field type {dataType}!");
            }
        }

        int fieldIndex = 0;
        foreach (FieldInfo modelField in modelFields)
        {
            GameTableFieldArrayAttribute? attribute = s_attributeCache[modelField];
            if (attribute != null)
            {
                Type elementType = GetElementType(modelField);
                for (uint i = 0u; i < attribute.Length; i++)
                    ValidateModelField(fields[fieldIndex++].Type, elementType);
            }
            else
            {
                ValidateModelField(fields[fieldIndex++].Type, modelField.FieldType);
            }
        }
    }

    private static (T[] Entries, int[] Lookup) ReadEntries(BinaryReader reader, GameTableHeader header, ReadOnlySpan<GameTableField> fields)
    {
        var entries = new T[header.RecordCount];

        int[] lookup = new int[header.MaxId];
        for (int i = 0; i < lookup.Length; i++)
            lookup[i] = -1;

        uint recordSize = (uint)(header.RecordSize * header.RecordCount);

        // build string table
        reader.BaseStream.Position = s_headerSize + (uint)header.RecordOffset + recordSize;
        int stringTableSize = (int)(header.TotalRecordSize - header.RecordSize * header.RecordCount);
        using (var stringTable = new StringTable(reader.ReadBytes(stringTableSize)))
        {
            for (int i = 0; i < (int)header.RecordCount; i++)
            {
                long recordStartPosition = s_headerSize + (long)header.RecordOffset + (long)header.RecordSize * i;
                if (reader.BaseStream.Position != recordStartPosition)
                    reader.BaseStream.Position = recordStartPosition;

                var entry = new T();

                int fieldIndex = 0;
                FieldInfo[] typeFields = [.. s_attributeCache.Keys];
                foreach (FieldInfo modelField in typeFields)
                {
                    GameTableFieldArrayAttribute? attribute = s_attributeCache[modelField];
                    if (attribute != null)
                    {
                        Type elementType = GetElementType(modelField);
                        var array = Array.CreateInstance(elementType, attribute.Length);
                        for (uint j = 0u; j < attribute.Length; j++)
                        {
                            DataType dataType = fields[fieldIndex++].Type;
                            switch (dataType)
                            {
                                case DataType.UInt:
                                    {
                                        if (elementType.IsEnum)
                                            array.SetValue(Enum.ToObject(elementType, reader.ReadUInt32()), j);
                                        else
                                            array.SetValue(reader.ReadUInt32(), j);
                                        break;
                                    }
                                case DataType.Single:
                                    array.SetValue(reader.ReadSingle(), j);
                                    break;
                                case DataType.Boolean:
                                    array.SetValue(Convert.ToBoolean(reader.ReadUInt32()), j);
                                    break;
                                case DataType.ULong:
                                    SkipToEightByteBoundary(reader, recordStartPosition);
                                    array.SetValue(reader.ReadUInt64(), j);
                                    break;
                            }
                        }

                        modelField.SetValue(entry, array);
                    }
                    else
                    {
                        DataType dataType = fields[fieldIndex++].Type;
                        switch (dataType)
                        {
                            case DataType.UInt:
                                {
                                    if (modelField.FieldType.IsEnum)
                                        modelField.SetValue(entry, Enum.ToObject(modelField.FieldType, reader.ReadUInt32()));
                                    else
                                        modelField.SetValue(entry, reader.ReadUInt32());
                                    break;
                                }
                            case DataType.Single:
                                modelField.SetValue(entry, reader.ReadSingle());
                                break;
                            case DataType.Boolean:
                                modelField.SetValue(entry, Convert.ToBoolean(reader.ReadUInt32()));
                                break;
                            case DataType.ULong:
                                SkipToEightByteBoundary(reader, recordStartPosition);
                                modelField.SetValue(entry, reader.ReadUInt64());
                                break;
                            case DataType.String:
                                {
                                    uint offset1 = reader.ReadUInt32();
                                    uint offset2 = reader.ReadUInt32();
                                    uint offset3 = Math.Max(offset1, offset2);

                                    string @string = stringTable.GetEntry(offset3 - recordSize);
                                    modelField.SetValue(entry, @string);

                                    if (fieldIndex < typeFields.Length - 1)
                                    {
                                        if (offset1 == 0 && fields[fieldIndex].Type != DataType.String)
                                            reader.BaseStream.Position += 4;
                                    }

                                    break;
                                }
                        }
                    }
                }

                entries[i] = entry;

                // id will always be the first column
                uint id = (uint)(typeFields[0].GetValue(entry) ?? throw new InvalidOperationException($"GameTable model {typeof(T).Name} has no id in its first column."));
                lookup[id] = i;
            }
        }

        return (entries, lookup);
    }

    /// <summary>
    /// A 64-bit column starts on an 8-byte boundary within its record, so one that follows an odd number of 4-byte columns
    /// has 4 bytes of padding before it (TradeskillTier's RelearnCost, after eleven uints).
    /// </summary>
    private static void SkipToEightByteBoundary(BinaryReader reader, long recordStart)
    {
        long misalignment = (reader.BaseStream.Position - recordStart) % 8;
        if (misalignment != 0)
            reader.BaseStream.Position += 8 - misalignment;
    }

    public T? GetEntry(ulong id)
    {
        if (id >= _header.MaxId)
            return null;

        int lookupId = _lookup[id];
        if (lookupId == -1)
            return null;

        return Entries[lookupId];
    }
}
