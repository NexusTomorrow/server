using System.Buffers.Binary;
using System.Collections.Concurrent;
using System.Numerics;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Network.Equivalence.Tests;

/// <summary>
/// Builds a declared instance with every wire member filled from a seeded <see cref="Random"/>, each value within its declared width, so the generated writer accepts it.
/// Count and selector members are left unset and pads untouched: the generated Write assigns the first two and writes zeros for the third.
/// </summary>
internal sealed class Filler
{
    private const int MaxCount = 4;
    private const int MaxShortStringLength = 20;
    // Past the 7-bit string length (at most 0x7F), so the 15-bit length is exercised.
    private const int LongStringLength = 130;
    private const int LongStringEvery = 8;
    private const int MaxBlobLength = 8;
    private const double PackedFloatRange = 1000;

    private static readonly ConcurrentDictionary<Type, Type[]> s_arms = new();

    private readonly Func<Type, bool> _skipsArm;

    /// <param name="skipsArm">Union arms never picked: a union whose arms are all skipped fails to fill.</param>
    public Filler(Func<Type, bool> skipsArm)
    {
        _skipsArm = skipsArm;
    }

    /// <summary>
    /// A new <paramref name="type"/> filled from <paramref name="random"/>; <paramref name="seed"/> picks the union arms and the long strings.
    /// </summary>
    public object Fill(Type type, Random random, int seed)
    {
        object instance = Activator.CreateInstance(type)!;
        FillMembers(instance, random, seed);
        return instance;
    }

    /// <summary>
    /// The empty instance of <paramref name="type"/>: zero numbers, empty strings, lists and blobs, the first fitting enum value and the first union arm,
    /// with every nested member built, which the seed's models leave null until a caller sets them.
    /// </summary>
    public object Empty(Type type)
    {
        return Fill(type, new ZeroRandom(), 0);
    }

    private void FillMembers(object instance, Random random, int seed)
    {
        IReadOnlyList<WireMember> members = WireMembers.Of(instance.GetType());
        HashSet<string> selectors = [.. members.Select(member => member.Selector).OfType<string>()];
        var sharedCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (WireMember member in members)
        {
            switch (member.Shape)
            {
                case WireShape.Value when !member.IsCount && !selectors.Contains(member.Name):
                    FillValue(instance, member, random, seed);
                    break;
                case WireShape.Array:
                    PutElements(instance, member, CountOf(member, members, sharedCounts, random), random, seed);
                    break;
                case WireShape.FixedArray:
                    PutElements(instance, member, member.FixedLength, random, seed);
                    break;
                case WireShape.Union:
                    MemberAccess.SetValue(member.Member, instance, Fill(ArmOf(member, seed), random, seed));
                    break;
                case WireShape.Blob:
                    MemberAccess.SetValue(member.Member, instance, NextBlob(random));
                    break;
            }
        }
    }

    private void FillValue(object instance, WireMember member, Random random, int seed)
    {
        // A get-only nested structure is filled where it stands, as the generated Read fills it.
        if (member.Encoding == WireEncoding.Struct && !MemberAccess.HasSetter(member.Member))
        {
            if (MemberAccess.GetValue(member.Member, instance) is { } nested)
                FillMembers(nested, random, seed);
            return;
        }

        MemberAccess.SetValue(member.Member, instance, NextValue(member, random, seed));
    }

    private void PutElements(object instance, WireMember member, int count, Random random, int seed)
    {
        object?[] elements = new object?[count];
        for (int i = 0; i < count; i++)
            elements[i] = NextValue(member, random, seed);

        // A get-only list or array is refilled in place, as the generated Read refills it.
        if (MemberAccess.HasSetter(member.Member) || !CollectionAccess.TryRefill(MemberAccess.GetValue(member.Member, instance), elements))
            MemberAccess.SetValue(member.Member, instance, CollectionAccess.Create(member.MemberType, member.ValueType, elements));
    }

    private static int CountOf(WireMember array, IReadOnlyList<WireMember> members, Dictionary<string, int> sharedCounts, Random random)
    {
        if (array.CountFrom is null)
            return random.Next(LargestCount(WireEncoding.UInt, array.CountBits) + 1);

        if (!sharedCounts.TryGetValue(array.CountFrom, out int count))
        {
            WireMember countMember = members.First(member => member.Name == array.CountFrom);
            count = random.Next(LargestCount(countMember.Encoding, countMember.Bits) + 1);
            sharedCounts[array.CountFrom] = count;
        }

        return count;
    }

    private static int LargestCount(WireEncoding encoding, int bits)
    {
        long largest = encoding == WireEncoding.SInt ? (1L << (bits - 1)) - 1 : (1L << bits) - 1;
        return (int)Math.Min(MaxCount, largest);
    }

    private object? NextValue(WireMember member, Random random, int seed)
    {
        Type type = member.ValueType;
        return member.Encoding switch
        {
            WireEncoding.UInt => NextUnsigned(type, member.Bits, random),
            WireEncoding.SInt => NextSigned(type, member.Bits, random),
            WireEncoding.Float32 => NextFloat(type, () => NextSingle(random)),
            WireEncoding.Float64 => NextDouble(random),
            WireEncoding.PackedFloat => NextFloat(type, () => (float)(((random.NextDouble() * 2) - 1) * PackedFloatRange)),
            WireEncoding.WideString or WireEncoding.NarrowString or WireEncoding.TerminatedWideString => NextString(member.Encoding, random, seed),
            WireEncoding.Struct => Fill(type, random, seed),
            _ => throw new InvalidOperationException($"{member.QualifiedName} has no value encoding."),
        };
    }

    private static object NextUnsigned(Type type, int bits, Random random)
    {
        if (type == typeof(bool))
            return random.Next(2) == 1;
        // A negative enum value is left out: the legacy writer converts an enum with ToUInt64, which throws for one, so the seed could never send it.
        if (type.IsEnum)
            return NextEnum(type, number => number >= 0 && ScalarValues.FitsUnsigned(number, bits, ScalarValues.SizeOf(type)), random);

        // Any pattern of the low bits: for a signed type as wide as its field, that includes every negative value.
        return ScalarValues.FromInteger(NextBits(random, bits), type);
    }

    private static object NextSigned(Type type, int bits, Random random)
    {
        if (type.IsEnum)
            return NextEnum(type, number => ScalarValues.FitsSigned(number, bits), random);

        int shift = 64 - bits;
        long value = unchecked((long)(NextBits(random, bits) << shift)) >> shift;
        return ScalarValues.FromInteger(value, type);
    }

    private static object NextEnum(Type type, Func<Int128, bool> fits, Random random)
    {
        object[] fitting = [.. Enum.GetValues(type).Cast<object>().Where(value => ScalarValues.TryGetInteger(value, out Int128 number) && fits(number))];
        return fitting.Length == 0 ? Enum.ToObject(type, 0) : fitting[random.Next(fitting.Length)];
    }

    private static object NextFloat(Type type, Func<float> next)
    {
        if (type == typeof(Vector3))
            return new Vector3(next(), next(), next());
        if (type == typeof(Vector2))
            return new Vector2(next(), next());
        if (type == typeof(Quaternion))
            return new Quaternion(next(), next(), next(), next());

        return next();
    }

    private static float NextSingle(Random random)
    {
        float value = BitConverter.UInt32BitsToSingle((uint)NextBits(random, 32));
        while (!float.IsFinite(value))
            value = BitConverter.UInt32BitsToSingle((uint)NextBits(random, 32));
        return value;
    }

    private static double NextDouble(Random random)
    {
        double value = BitConverter.UInt64BitsToDouble(NextBits(random, 64));
        while (!double.IsFinite(value))
            value = BitConverter.UInt64BitsToDouble(NextBits(random, 64));
        return value;
    }

    private static ulong NextBits(Random random, int bits)
    {
        Span<byte> bytes = stackalloc byte[sizeof(ulong)];
        random.NextBytes(bytes);
        ulong value = BinaryPrimitives.ReadUInt64LittleEndian(bytes);
        return bits >= 64 ? value : value & ((1UL << bits) - 1);
    }

    private static string NextString(WireEncoding encoding, Random random, int seed)
    {
        bool hasLengthHeader = encoding != WireEncoding.TerminatedWideString;
        int length = hasLengthHeader && seed % LongStringEvery == LongStringEvery - 1 ? LongStringLength : random.Next(MaxShortStringLength + 1);

        // A wide string takes any UTF-16 unit below the surrogates, which the legacy writer's encoder would replace; the others take printable ASCII.
        int end = encoding == WireEncoding.WideString ? 0xD800 : 0x7F;
        char[] chars = new char[length];
        for (int i = 0; i < length; i++)
            chars[i] = (char)random.Next(0x20, end);
        return new string(chars);
    }

    private static byte[] NextBlob(Random random)
    {
        byte[] blob = new byte[random.Next(MaxBlobLength + 1)];
        random.NextBytes(blob);
        return blob;
    }

    /// <summary>
    /// The arm with index <paramref name="seed"/> modulo the arm count, among the [UnionCase] classes in the declaring type's assembly that implement the union's type
    /// and are not skipped, in full-name order.
    /// </summary>
    private Type ArmOf(WireMember union, int seed)
    {
        Type[] arms = [.. ArmsOf(union).Where(arm => !_skipsArm(arm))];
        if (arms.Length == 0)
            throw new InvalidOperationException($"{union.QualifiedName} has no [UnionCase] arm in its assembly that is not skipped.");

        return arms[seed % arms.Length];
    }

    /// <summary>
    /// Every arm of <paramref name="union"/>: the [UnionCase] classes in the declaring type's assembly that implement the union's type, in full-name order.
    /// </summary>
    public static Type[] ArmsOf(WireMember union)
    {
        return s_arms.GetOrAdd(union.ValueType, unionType =>
        [
            .. union.Member.DeclaringType!.Assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsDefined(typeof(UnionCaseAttribute), inherit: false) && unionType.IsAssignableFrom(type))
                .OrderBy(type => type.FullName, StringComparer.Ordinal),
        ]);
    }

    /// <summary>
    /// Draws zero every time; <see cref="NextDouble"/> gives the midpoint, which the packed float range maps to zero.
    /// </summary>
    private sealed class ZeroRandom : Random
    {
        public override int Next(int maxValue)
        {
            return 0;
        }

        public override int Next(int minValue, int maxValue)
        {
            return minValue;
        }

        public override double NextDouble()
        {
            return 0.5;
        }

        public override void NextBytes(byte[] buffer)
        {
            Array.Clear(buffer);
        }

        public override void NextBytes(Span<byte> buffer)
        {
            buffer.Clear();
        }
    }
}
