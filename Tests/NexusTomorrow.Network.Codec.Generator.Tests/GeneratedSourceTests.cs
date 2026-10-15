using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace NexusTomorrow.Network.Codec.Generator.Tests;

public sealed class GeneratedSourceTests
{
    private const string EveryShape = """
        using System.Collections.Generic;
        using System.Numerics;
        using NexusTomorrow.Network.Codec;
        using NexusTomorrow.Network.Message;

        namespace Tests;

        public enum Mode : byte
        {
            Idle = 0,
            Busy = 3,
        }

        public enum Shape : byte
        {
            Point = 0,
            Box = 1,
        }

        public interface IArm : IWireStruct
        {
        }

        [WireStruct]
        public partial class Header
        {
            [UInt(16)] public ushort Version { get; set; }
        }

        [WireStruct]
        public sealed partial class Entry : Header
        {
            [UInt(8)] public byte Level { get; set; }
        }

        [WireStruct]
        public sealed partial class Position
        {
            [Float32] public float X { get; set; }

            [Float32] public float Y { get; set; }
        }

        [WireStruct]
        [UnionCase(Shape.Point)]
        public sealed partial class PointArm : IArm
        {
        }

        [WireStruct]
        [UnionCase(Shape.Box)]
        public sealed partial class BoxArm : IArm
        {
            [PackedFloat] public float Size { get; set; }
        }

        [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
        public sealed partial class Everything
        {
            [UInt(20)] public uint Id { get; set; }

            [SInt(12)] public short Offset { get; set; }

            [UInt(1)] public bool Enabled { get; set; }

            [UInt(2)] public Mode Activity { get; set; }

            [Float32] public float Speed { get; set; }

            [Float32] public Vector3 Facing { get; set; }

            [PackedFloat] public Vector3 Velocity { get; set; }

            [Float64] public double Clock { get; set; }

            [WideString] public string Name { get; set; } = "";

            [NarrowString] public string Tag { get; set; } = "";

            [TerminatedWideString] public string Note { get; set; } = "";

            public Position Origin { get; set; } = new();

            [Array(CountBits = 4)] public List<Entry> Entries { get; set; } = [];

            [Count, UInt(8)] public byte PairCount { get; set; }

            [Array(CountFrom = nameof(PairCount)), UInt(32)] public uint[] Keys { get; set; } = [];

            [Array(CountFrom = nameof(PairCount)), WideString] public List<string> Values { get; set; } = [];

            [FixedArray(3), UInt(8)] public byte[] Slots { get; set; } = new byte[3];

            [UInt(1)] public Shape VolumeShape { get; set; }

            [Union(nameof(VolumeShape))] public IArm Volume { get; set; } = new PointArm();

            [Pad(3)] public Pad Reserved { get; set; }

            [Blob(16, Align = true, LengthIncludesSelf = true)] public byte[] Payload { get; set; } = [];
        }
        """;

    private const string FixedMessage = """
        using System.Numerics;
        using NexusTomorrow.Network.Codec;
        using NexusTomorrow.Network.Message;

        namespace Tests;

        public enum Mode : byte
        {
            Idle = 0,
            Busy = 3,
        }

        [WireStruct]
        public sealed partial class Header
        {
            [UInt(16)] public ushort Version { get; set; }
        }

        [WireMessage(GameMessageOpcode.ClientCastSpell, Direction.ClientToServer)]
        public sealed partial class Fixed
        {
            [UInt(32)] public uint Id { get; set; }

            [SInt(16)] public short Delta { get; set; }

            [UInt(1)] public bool Enabled { get; set; }

            [UInt(2)] public Mode Activity { get; set; }

            [Float32] public Vector3 Position { get; set; }

            [PackedFloat] public float Turn { get; set; }

            [Float64] public double Clock { get; set; }

            public Header Prefix { get; set; } = new();

            [FixedArray(2), UInt(8)] public byte[] Slots { get; set; } = new byte[2];

            [Pad(5)] public Pad Reserved { get; set; }
        }
        """;

    [Fact]
    public void MessageUsingEveryMemberShapeCompilesWithoutErrors()
    {
        GeneratorRun run = GeneratorHarness.Run(EveryShape);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        string[] types = ["Header", "Entry", "Position", "PointArm", "BoxArm", "Everything"];
        foreach (string type in types)
            Assert.Contains($"Tests.{type}.g.cs", run.GeneratedSources);
    }

    [Fact]
    public void RegistryCreatesEachMessageByOpcode()
    {
        GeneratorRun run = GeneratorHarness.Run(EveryShape);

        string registry = Assert.Contains("WireMessages.g.cs", run.GeneratedSources);
        Assert.Contains($"namespace {GeneratorHarness.AssemblyName}", registry);
        Assert.Contains("public static class WireMessages", registry);
        Assert.Contains("0x0003 => new global::Tests.Everything(),", registry);
    }

    // Analyzers skip generated sources, so the hot-path ban list (RS0030) never sees the generated Write;
    // this checks its text for LINQ, reflection, Convert and list allocation instead.
    [Theory]
    [InlineData("System.Linq")]
    [InlineData("Activator")]
    [InlineData("Convert.")]
    [InlineData("new List")]
    public void FixedMessageWritePathNamesNoBannedApi(string banned)
    {
        GeneratorRun run = GeneratorHarness.Run(FixedMessage);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        Assert.DoesNotContain(banned, WritePathOf(Assert.Contains("Tests.Fixed.g.cs", run.GeneratedSources)));
    }

    // The code standard holds a generated Write to no allocation, and analyzers skip generated sources,
    // so this measures one on the thread that runs it.
    [Fact]
    public void FixedMessageWriteAllocatesNothing()
    {
        GeneratorRun run = GeneratorHarness.Run(FixedMessage + """

            public static class Probe
            {
                public static long WriteAllocations()
                {
                    var message = new Fixed();
                    byte[] buffer = new byte[64];
                    for (int i = 0; i < 2; i++)
                    {
                        var warmup = new BitWriter(buffer);
                        message.Write(ref warmup);
                    }

                    long before = System.GC.GetAllocatedBytesForCurrentThread();
                    for (int i = 0; i < 100; i++)
                    {
                        var writer = new BitWriter(buffer);
                        message.Write(ref writer);
                    }

                    return System.GC.GetAllocatedBytesForCurrentThread() - before;
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        object? allocated = GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "WriteAllocations");
        Assert.Equal(0L, Assert.IsType<long>(allocated));
    }

    // A count is written at its member's width, so a collection too long for that width must throw
    // CodecException, as BitWriter.WriteBits does for any value that does not fit; wrapping the count
    // writes a header that disagrees with the elements after it.
    [Fact]
    public void ArrayLongerThanItsCountWidthThrowsOnWrite()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public sealed partial class Counted
            {
                [Count, UInt(8)] public byte Length { get; set; }

                [Array(CountFrom = nameof(Length)), UInt(8)] public byte[] Values { get; set; } = [];
            }

            public static class Probe
            {
                public static bool WriteThrows()
                {
                    var counted = new Counted { Values = new byte[256] };
                    var writer = new BitWriter(new byte[512]);
                    try
                    {
                        counted.Write(ref writer);
                        return false;
                    }
                    catch (CodecException)
                    {
                        return true;
                    }
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        object? threw = GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "WriteThrows");
        Assert.True(threw is true, "Writing 256 elements under an 8-bit count did not throw.");
    }

    // An arm's [UnionCase] value and the selector the reader sees must be the same number once both
    // pass through the selector's width; -1 written to an 8-bit signed selector reads back as -1.
    [Fact]
    public void NegativeUnionCaseRoundTripsThroughANarrowSignedSelector()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public interface IArm : IWireStruct
            {
            }

            [WireStruct]
            [UnionCase(0)]
            public sealed partial class Zero : IArm
            {
            }

            [WireStruct]
            [UnionCase(-1)]
            public sealed partial class Minus : IArm
            {
            }

            [WireStruct]
            public sealed partial class Holder
            {
                [SInt(8)] public sbyte Selector { get; set; }

                [Union(nameof(Selector))] public IArm Arm { get; set; } = new Zero();
            }

            public static class Probe
            {
                public static string RoundTrip()
                {
                    byte[] buffer = new byte[4];
                    var written = new Holder { Arm = new Minus() };
                    var writer = new BitWriter(buffer);
                    written.Write(ref writer);

                    var read = new Holder();
                    var reader = new BitReader(buffer);
                    try
                    {
                        read.Read(ref reader);
                    }
                    catch (CodecException exception)
                    {
                        return exception.Message;
                    }

                    return read.Arm.GetType().Name;
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        Assert.Equal("Minus", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "RoundTrip"));
    }

    // A [UInt] selector on a signed type carries its value's unsigned bits: 200 in an 8-bit sbyte
    // selector is -56 in memory and 200 on the wire, and an sbyte enum's -1 is 255 on the wire.
    [Fact]
    public void AFullWidthUIntSelectorOnASignedTypeRoundTripsEveryCase()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public enum Kind : sbyte
            {
                Negative = -1,
                Zero = 0,
            }

            public interface IArm : IWireStruct
            {
            }

            [WireStruct]
            [UnionCase(0)]
            public sealed partial class Low : IArm
            {
            }

            [WireStruct]
            [UnionCase(200)]
            public sealed partial class High : IArm
            {
            }

            public interface IShape : IWireStruct
            {
            }

            [WireStruct]
            [UnionCase(Kind.Zero)]
            public sealed partial class ZeroShape : IShape
            {
            }

            [WireStruct]
            [UnionCase(Kind.Negative)]
            public sealed partial class NegativeShape : IShape
            {
            }

            [WireStruct]
            public sealed partial class NumberHolder
            {
                [UInt(8)] public sbyte Selector { get; set; }

                [Union(nameof(Selector))] public IArm Arm { get; set; } = new Low();
            }

            [WireStruct]
            public sealed partial class EnumHolder
            {
                [UInt(8)] public Kind Selector { get; set; }

                [Union(nameof(Selector))] public IShape Shape { get; set; } = new ZeroShape();
            }

            public static class Probe
            {
                public static string RoundTrip()
                {
                    byte[] numberBytes = new byte[1];
                    var numberWriter = new BitWriter(numberBytes);
                    new NumberHolder { Arm = new High() }.Write(ref numberWriter);
                    var numberRead = new NumberHolder();
                    var numberReader = new BitReader(numberBytes);
                    numberRead.Read(ref numberReader);

                    byte[] enumBytes = new byte[1];
                    var enumWriter = new BitWriter(enumBytes);
                    new EnumHolder { Shape = new NegativeShape() }.Write(ref enumWriter);
                    var enumRead = new EnumHolder();
                    var enumReader = new BitReader(enumBytes);
                    enumRead.Read(ref enumReader);

                    return $"{numberBytes[0]} {numberRead.Arm.GetType().Name} {enumBytes[0]} {enumRead.Shape.GetType().Name}";
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        Assert.Equal("200 High 255 NegativeShape", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "RoundTrip"));
    }

    [Fact]
    public void AVector2IsTwoFloatsOnTheWire()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Numerics;
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public sealed partial class Point
            {
                [Float32] public Vector2 Full { get; set; }

                [PackedFloat] public Vector2 Packed { get; set; }
            }

            public static class Probe
            {
                public static string RoundTrip()
                {
                    var written = new Point { Full = new Vector2(1.5f, -2f), Packed = new Vector2(0.5f, 4f) };
                    byte[] bytes = new byte[12];
                    var writer = new BitWriter(bytes);
                    written.Write(ref writer);
                    var read = new Point();
                    var reader = new BitReader(bytes);
                    read.Read(ref reader);
                    return System.FormattableString.Invariant($"{written.MeasureBits()} {read.Full.X} {read.Full.Y} {read.Packed.X} {read.Packed.Y} {bytes[0]:X2}{bytes[1]:X2}{bytes[2]:X2}{bytes[3]:X2}");
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        // 1.5f is 0x3FC00000, written least significant byte first.
        Assert.Equal("96 1.5 -2 0.5 4 0000C03F", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "RoundTrip"));
    }

    [Fact]
    public void AQuaternionIsFourFloatsOnTheWireInXYZWOrder()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Numerics;
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public sealed partial class Turn
            {
                [Float32] public Quaternion Rotation { get; set; }
            }

            public static class Probe
            {
                public static string RoundTrip()
                {
                    var written = new Turn { Rotation = new Quaternion(1, 2, 3, 4) };
                    byte[] bytes = new byte[16];
                    var writer = new BitWriter(bytes);
                    written.Write(ref writer);
                    var read = new Turn();
                    var reader = new BitReader(bytes);
                    read.Read(ref reader);
                    return System.FormattableString.Invariant($"{written.MeasureBits()} {read.Rotation} {System.Convert.ToHexString(bytes)}");
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        // Each float goes out as its IEEE bits, low byte first: 1f is 0x3F800000, 2f 0x40000000, 3f 0x40400000, 4f 0x40800000.
        Assert.Equal("128 {X:1 Y:2 Z:3 W:4} 0000803F000000400000404000008040", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "RoundTrip"));
    }

    // NTC003 keeps a member whose enum has a value too wide for it, such as a sentinel never sent; that value must not reach the wire.
    [Fact]
    public void WritingAnEnumValueWiderThanItsFieldThrows()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            public enum Slot : uint
            {
                First = 1,
                None = 0xFFFFFFFF,
            }

            [WireStruct]
            public sealed partial class Sample
            {
                [UInt(4)] public Slot Where { get; set; }
            }

            public static class Probe
            {
                public static string Write()
                {
                    var sample = new Sample { Where = Slot.None };
                    byte[] bytes = new byte[1];
                    var writer = new BitWriter(bytes);
                    try
                    {
                        sample.Write(ref writer);
                        return "written";
                    }
                    catch (CodecException exception)
                    {
                        return exception.GetType().Name;
                    }
                }
            }
            """);

        Assert.Equal("NTC003", Assert.Single(run.Diagnostics).Id);
        Assert.Empty(run.CompilationErrors);
        Assert.Equal("CodecException", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "Write"));
    }

    // The analysis and the sources are values, so an edit that touches no wire type reruns neither.
    [Fact]
    public void AnEditOutsideTheWireTypesReusesTheAnalysisAndTheSources()
    {
        CSharpCompilation compilation = GeneratorHarness.Compile("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public sealed partial class Sample
            {
                [UInt(8)] public byte Value { get; set; }
            }
            """);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(
            [new WireGenerator().AsSourceGenerator()],
            driverOptions: new GeneratorDriverOptions(IncrementalGeneratorOutputKind.None, trackIncrementalGeneratorSteps: true));

        CancellationToken cancellationToken = TestContext.Current.CancellationToken;
        driver = driver.RunGenerators(compilation, cancellationToken);
        SyntaxTree unrelated = CSharpSyntaxTree.ParseText("namespace Tests; public static class Unrelated { }", cancellationToken: cancellationToken);
        driver = driver.RunGenerators(compilation.AddSyntaxTrees(unrelated), cancellationToken);

        GeneratorRunResult result = driver.GetRunResult().Results.Single();
        IncrementalStepRunReason[] reasons =
        [
            .. new[] { WireGenerator.ProductionStep, WireGenerator.SourcesStep }
                .SelectMany(step => result.TrackedSteps[step])
                .SelectMany(run => run.Outputs)
                .Select(output => output.Reason),
        ];
        Assert.NotEmpty(reasons);
        Assert.All(reasons, reason => Assert.True(reason is IncrementalStepRunReason.Cached or IncrementalStepRunReason.Unchanged, reason.ToString()));
    }

    // The client nests whole message bodies in a list (ServerMissileActorListAdd carries ServerMissileActorAdd bodies).
    [Fact]
    public void AMessageNestedInAnotherRoundTrips()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Collections.Generic;
            using NexusTomorrow.Network.Codec;
            using NexusTomorrow.Network.Message;

            namespace Tests;

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
            public sealed partial class Item
            {
                [UInt(5)] public byte Value { get; set; }
            }

            [WireMessage(GameMessageOpcode.ServerChat, Direction.ServerToClient)]
            public sealed partial class Items
            {
                [Array(CountBits = 3)] public List<Item> Entries { get; set; } = [];
            }

            public static class Probe
            {
                public static string RoundTrip()
                {
                    var written = new Items { Entries = [new Item { Value = 9 }, new Item { Value = 30 }] };
                    byte[] bytes = new byte[2];
                    var writer = new BitWriter(bytes);
                    written.Write(ref writer);
                    var read = new Items();
                    var reader = new BitReader(bytes);
                    read.Read(ref reader);
                    return $"{written.MeasureBits()} {read.Entries.Count} {read.Entries[0].Value} {read.Entries[1].Value}";
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        Assert.Equal("13 2 9 30", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "RoundTrip"));
    }

    [Fact]
    public void AMessageDerivingFromAMessageCarriesTheBaseFieldsFirst()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using NexusTomorrow.Network.Codec;
            using NexusTomorrow.Network.Message;

            namespace Tests;

            [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)]
            public partial class Parent
            {
                [UInt(8)] public byte First { get; set; }
            }

            [WireMessage(GameMessageOpcode.ServerChat, Direction.ServerToClient)]
            public sealed partial class Child : Parent
            {
                [UInt(8)] public byte Second { get; set; }
            }

            public static class Probe
            {
                public static string RoundTrip()
                {
                    byte[] bytes = new byte[2];
                    var writer = new BitWriter(bytes);
                    var written = new Child { First = 1, Second = 2 };
                    written.Write(ref writer);
                    var read = new Child();
                    var reader = new BitReader(bytes);
                    read.Read(ref reader);
                    return $"{bytes[0]} {bytes[1]} {written.MeasureBits()} {read.First} {read.Second}";
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        Assert.Equal("1 2 16 1 2", GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "RoundTrip"));
    }

    // A reader takes a terminated string's count of 0 as "", so an element holding one guarantees only
    // its 16-bit count, and the count check must not reject two such elements in 32 bits.
    [Fact]
    public void ElementsWithEmptyTerminatedStringsSentAsCountsAloneRead()
    {
        GeneratorRun run = GeneratorHarness.Run("""
            using System.Collections.Generic;
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public sealed partial class Note
            {
                [TerminatedWideString] public string Text { get; set; } = "";
            }

            [WireStruct]
            public sealed partial class Notes
            {
                [Array(CountBits = 8)] public List<Note> Items { get; } = [];
            }

            public static class Probe
            {
                public static int ReadTwo()
                {
                    var notes = new Notes();
                    var reader = new BitReader(new byte[] { 0x02, 0x00, 0x00, 0x00, 0x00 });
                    notes.Read(ref reader);
                    return notes.Items.Count;
                }
            }
            """);

        Assert.Empty(run.Diagnostics);
        Assert.Empty(run.CompilationErrors);
        Assert.Equal(2, GeneratorHarness.Invoke(run.Compilation, "Tests.Probe", "ReadTwo"));
    }

    private static string WritePathOf(string generated)
    {
        int start = generated.IndexOf("void Write(", StringComparison.Ordinal);
        int end = generated.IndexOf("int MeasureBits(", Math.Max(start, 0), StringComparison.Ordinal);
        Assert.True(start >= 0 && end > start, "The generated source has no Write method followed by MeasureBits.");
        return generated[start..end];
    }
}
