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

    private static string WritePathOf(string generated)
    {
        int start = generated.IndexOf("void Write(", StringComparison.Ordinal);
        int end = generated.IndexOf("int MeasureBits(", Math.Max(start, 0), StringComparison.Ordinal);
        Assert.True(start >= 0 && end > start, "The generated source has no Write method followed by MeasureBits.");
        return generated[start..end];
    }
}
