using System.Numerics;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Holds a declared model to the legacy model it replaces, over the empty instance and a run of seeded ones: the generated writer must put the legacy writer's bytes
/// on the wire, and the generated and legacy readers must read those bytes to the same values.
/// </summary>
internal sealed class Equivalence
{
    private const string WriteDirection = "write";
    private const string ReadDirection = "read";

    private readonly Type _declared;
    private readonly Type _legacy;
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
        _pairing = pairing;
        _legacyServices = legacyServices;
        _filler = new Filler(pairing.SkipsArm);
        _mapper = new TwinMapper(pairing, legacyServices);
        _comparer = new GraphComparer(pairing);
    }

    /// <summary>
    /// Every difference between <paramref name="declared"/> and <paramref name="legacy"/> over the empty instance and seeds 0 to <paramref name="seeds"/> - 1, each prefixed
    /// with the declared type's name, "empty" or the seed, and the direction. The write check runs when the legacy type is <see cref="IWritable"/> and the pairing does not
    /// hold the type to its legacy Read only, the read check when it is <see cref="IReadable"/>. Each filled instance takes the pairing's limits first.
    /// </summary>
    public static List<string> Check(Type declared, Type legacy, Pairing pairing, IServiceProvider legacyServices, int seeds)
    {
        var equivalence = new Equivalence(declared, legacy, pairing, legacyServices);
        equivalence.CheckInstance(null);
        for (int seed = 0; seed < seeds; seed++)
            equivalence.CheckInstance(seed);
        return equivalence._differences;
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

        byte[] bytes;
        try
        {
            bytes = WriteDeclared(value);
        }
        catch (Exception exception)
        {
            Report(label, WriteDirection, $"the declared writer threw {Describe(exception)}");
            return;
        }

        if (typeof(IWritable).IsAssignableFrom(_legacy) && !_pairing.IsReadOnly(_declared))
            CheckWrite(label, value, bytes);
        if (typeof(IReadable).IsAssignableFrom(_legacy))
            CheckRead(label, bytes);
    }

    private void CheckWrite(string label, IWireStruct value, byte[] bytes)
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
        try
        {
            legacyBytes = WriteLegacy((IWritable)twin);
        }
        catch (Exception exception)
        {
            Report(label, WriteDirection, $"the legacy writer threw {Describe(exception)}");
            return;
        }

        if (FirstDifferentBit(bytes, legacyBytes) is { } bit)
            Report(label, WriteDirection, $"the bytes differ from bit {bit}: declared [{Convert.ToHexString(bytes)}], legacy [{Convert.ToHexString(legacyBytes)}]");
    }

    private void CheckRead(string label, byte[] bytes)
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
            using var stream = new MemoryStream(bytes, writable: false);
            using var reader = new GamePacketReader(stream);
            ((IReadable)legacyRead).Read(reader);

            // GamePacketReader takes a byte when it needs that byte's first bit, so a reader that reads every written bit ends on the last byte.
            if (stream.Position != bytes.Length)
                Report(label, ReadDirection, $"the legacy reader stopped at byte {stream.Position} of {bytes.Length}");
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

    private static byte[] WriteDeclared(IWireStruct value)
    {
        byte[] buffer = new byte[(value.MeasureBits() + 7) / 8];
        var writer = new BitWriter(buffer);
        value.Write(ref writer);
        return buffer;
    }

    private static byte[] WriteLegacy(IWritable value)
    {
        using var stream = new MemoryStream();
        using var writer = new GamePacketWriter(stream);
        value.Write(writer);
        writer.FlushBits();
        return stream.ToArray();
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
