using System.Numerics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Holds a declared model to the legacy model it replaces, over the empty instance and a run of seeded ones: the generated writer must put the legacy writer's bits
/// on the wire, the legacy reader must take exactly the bits written, and the generated and legacy readers must read them to the same values.
/// </summary>
internal sealed class Equivalence
{
    private const string WriteDirection = "write";
    private const string ReadDirection = "read";

    // GamePacketWriter counts the bits of the byte it has begun in bitPosition; GamePacketReader keeps in currentBitPosition the index of the last bit
    // it took from its current byte, or 8 when it holds none.
    private static readonly FieldInfo s_writerBitPosition = typeof(GamePacketWriter).GetField("bitPosition", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new MissingFieldException(nameof(GamePacketWriter), "bitPosition");
    private static readonly FieldInfo s_readerBitPosition = typeof(GamePacketReader).GetField("currentBitPosition", BindingFlags.Instance | BindingFlags.NonPublic)
        ?? throw new MissingFieldException(nameof(GamePacketReader), "currentBitPosition");

    private readonly Type _declared;
    private readonly Type _legacy;
    private readonly MethodInfo? _legacyWrite;
    private readonly MethodInfo? _legacyRead;
    private readonly Pairing _pairing;
    private readonly IServiceProvider _legacyServices;
    private readonly Filler _filler;
    private readonly TwinMapper _mapper;
    private readonly GraphComparer _comparer;
    private readonly List<string> _differences = [];

    private Equivalence(Type declared, Type legacy, Pairing pairing, IServiceProvider legacyServices)
    {
        _declared = declared;
        _legacy = legacy;
        _legacyWrite = LegacyWriteOf(legacy);
        _legacyRead = LegacyReadOf(legacy);
        _pairing = pairing;
        _legacyServices = legacyServices;
        _filler = new Filler(pairing.SkipsArm);
        _mapper = new TwinMapper(pairing, legacyServices);
        _comparer = new GraphComparer(pairing);
    }

    /// <summary>
    /// Every difference between <paramref name="declared"/> and <paramref name="legacy"/> over the empty instance and seeds 0 to <paramref name="seeds"/> - 1, each prefixed
    /// with the declared type's name, "empty" or the seed, and the direction. The write check runs when the legacy type has a <c>Write(GamePacketWriter)</c>, unless the
    /// pairing holds the type or a type in the instance to its legacy Read only; the read check runs when it has a <c>Read(GamePacketReader)</c>. A legacy type with
    /// neither has nothing to check here (see <see cref="CanCheck"/>). Each filled instance takes the pairing's limits first.
    /// </summary>
    public static List<string> Check(Type declared, Type legacy, Pairing pairing, IServiceProvider legacyServices, int seeds)
    {
        var equivalence = new Equivalence(declared, legacy, pairing, legacyServices);
        equivalence.CheckInstance(null);
        for (int seed = 0; seed < seeds; seed++)
            equivalence.CheckInstance(seed);
        return equivalence._differences;
    }

    /// <summary>
    /// Whether <paramref name="legacy"/> writes or reads itself, so a pair with it checks something. The seed declares some readers without
    /// <see cref="IReadable"/> (<c>ClientMatchingQueueRandomParty</c>), so the methods decide, not the interfaces.
    /// </summary>
    public static bool CanCheck(Type legacy)
    {
        return LegacyWriteOf(legacy) is not null || LegacyReadOf(legacy) is not null;
    }

    /// <summary>
    /// Whether <paramref name="legacy"/> reads itself.
    /// </summary>
    public static bool Reads(Type legacy)
    {
        return LegacyReadOf(legacy) is not null;
    }

    private static MethodInfo? LegacyWriteOf(Type legacy)
    {
        return legacy.GetMethod("Write", BindingFlags.Instance | BindingFlags.Public, [typeof(GamePacketWriter)]);
    }

    private static MethodInfo? LegacyReadOf(Type legacy)
    {
        return legacy.GetMethod("Read", BindingFlags.Instance | BindingFlags.Public, [typeof(GamePacketReader)]);
    }

    private void CheckInstance(int? seed)
    {
        string label = seed.HasValue ? $"seed {seed.Value}" : "empty";
        IWireStruct value;
        try
        {
            object instance = seed.HasValue ? _filler.Fill(_declared, new Random(seed.Value), seed.Value) : _filler.Empty(_declared);
            _pairing.Limit(instance);
            value = (IWireStruct)instance;
        }
        catch (Exception exception)
        {
            Report(label, WriteDirection, $"filling {_declared.Name} threw {Describe(exception)}");
            return;
        }

        int bits;
        byte[] bytes;
        try
        {
            bits = value.MeasureBits();
            bytes = WriteDeclared(value, bits);
        }
        catch (Exception exception)
        {
            Report(label, WriteDirection, $"the declared writer threw {Describe(exception)}");
            return;
        }

        if (_legacyWrite is not null && !_pairing.HoldsReadOnly(value))
            CheckWrite(label, value, bytes, bits);
        if (_legacyRead is not null)
            CheckRead(label, bytes, bits);
    }

    private void CheckWrite(string label, IWireStruct value, byte[] bytes, int bits)
    {
        var mapDifferences = new List<string>();
        object? twin = null;
        try
        {
            twin = _mapper.Map(value, _legacy, mapDifferences);
        }
        catch (Exception exception)
        {
            mapDifferences.Add($"mapping onto {_legacy.Name} threw {Describe(exception)}");
        }

        foreach (string difference in mapDifferences)
            Report(label, WriteDirection, difference);
        if (twin is null)
            return;

        byte[] legacyBytes;
        long legacyBits;
        try
        {
            (legacyBytes, legacyBits) = WriteLegacy(twin, _legacyWrite!);
        }
        catch (Exception exception)
        {
            Report(label, WriteDirection, $"the legacy writer threw {Describe(exception)}");
            return;
        }

        if (FirstDifferentBit(bytes, legacyBytes) is { } bit)
            Report(label, WriteDirection, $"the bytes differ from bit {bit}: declared [{Convert.ToHexString(bytes)}], legacy [{Convert.ToHexString(legacyBytes)}]");
        else if (legacyBits != bits)
            Report(label, WriteDirection, $"the legacy writer wrote {legacyBits} bits where the declaration measures {bits}");
    }

    private void CheckRead(string label, byte[] bytes, int bits)
    {
        IWireStruct declaredRead;
        try
        {
            declaredRead = (IWireStruct)Activator.CreateInstance(_declared)!;
            var reader = new BitReader(bytes);
            declaredRead.Read(ref reader);
        }
        catch (Exception exception)
        {
            Report(label, ReadDirection, $"the declared reader threw {Describe(exception)}");
            return;
        }

        object legacyRead;
        try
        {
            legacyRead = ActivatorUtilities.CreateInstance(_legacyServices, _legacy);
            _pairing.Preset(_declared, legacyRead);
            using var stream = new BodyStream(bytes);
            using var reader = new GamePacketReader(stream);
            _legacyRead!.Invoke(legacyRead, [reader]);

            long read = BitsRead(reader, stream);
            if (read != bits)
                Report(label, ReadDirection, $"the legacy reader read {read} bits of the {bits} written");
        }
        catch (Exception exception)
        {
            Report(label, ReadDirection, $"the legacy reader threw {Describe(exception)}");
            return;
        }

        List<string> compared;
        try
        {
            compared = _comparer.Compare(declaredRead, legacyRead);
        }
        catch (Exception exception)
        {
            Report(label, ReadDirection, $"comparing the two reads threw {Describe(exception)}");
            return;
        }

        foreach (string difference in compared)
            Report(label, ReadDirection, difference);
    }

    private void Report(string label, string direction, string difference)
    {
        _differences.Add($"{_declared.Name} {label} {direction}: {difference}");
    }

    private static byte[] WriteDeclared(IWireStruct value, int bits)
    {
        byte[] buffer = new byte[(bits + 7) / 8];
        var writer = new BitWriter(buffer);
        value.Write(ref writer);
        return buffer;
    }

    /// <summary>
    /// The legacy body and its length in bits: the bytes it finished plus the bits of the byte it had begun, which equal bodies can differ in when the
    /// last field's width differs and its high bits are zero.
    /// </summary>
    private static (byte[] Bytes, long Bits) WriteLegacy(object value, MethodInfo write)
    {
        using var stream = new MemoryStream();
        using var writer = new GamePacketWriter(stream);
        write.Invoke(value, [writer]);
        long bits = (stream.Length * 8) + (byte)s_writerBitPosition.GetValue(writer)!;
        writer.FlushBits();
        return (stream.ToArray(), bits);
    }

    /// <summary>
    /// The bits the legacy reader took: the bytes it has taken, less the bits it has not reached in the last of them. The body throws on a byte past its
    /// end (<see cref="BodyStream"/>), so the count never stalls at the end while the reader goes on.
    /// </summary>
    private static long BitsRead(GamePacketReader reader, Stream stream)
    {
        byte lastBit = (byte)s_readerBitPosition.GetValue(reader)!;
        return (stream.Position * 8) - (lastBit <= 7 ? 7 - lastBit : 0);
    }

    /// <summary>
    /// The index of the first bit, in wire order, where the two bodies differ; a body that is a prefix of the other differs at its own end. Null when they are equal.
    /// </summary>
    private static int? FirstDifferentBit(byte[] declared, byte[] legacy)
    {
        int shared = Math.Min(declared.Length, legacy.Length);
        for (int i = 0; i < shared; i++)
        {
            // Bits fill each byte from its least significant end, so the lowest set bit of the difference comes first on the wire.
            int difference = declared[i] ^ legacy[i];
            if (difference != 0)
                return (i * 8) + BitOperations.TrailingZeroCount(difference);
        }

        return declared.Length == legacy.Length ? null : shared * 8;
    }

    private static string Describe(Exception exception)
    {
        Exception inner = exception;
        while (inner is TargetInvocationException { InnerException: { } wrapped })
            inner = wrapped;
        return $"{inner.GetType().Name}: {inner.Message}";
    }
}
