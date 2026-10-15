namespace NexusTomorrow.ModelConverter.Tests;

public sealed class ConverterTests
{
    [Fact]
    public void AMessageGainsItsWidthsInWireOrder()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            [Message(GameMessageOpcode.ServerHello)]
            public class Hello : IWritable
            {
                public byte Kind { get; set; }
                public uint Id { get; set; }
                public bool Flag { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Id);
                    writer.Write(Flag);
                    writer.Write(Kind, 5u);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "using NexusTomorrow.Network.Codec;",
            "[Message(GameMessageOpcode.ServerHello)] [WireMessage(GameMessageOpcode.ServerHello, Direction.ServerToClient)] public sealed partial class Hello : IWritable {",
            "[UInt(32)] public uint Id { get; set; }",
            "[UInt(1)] public bool Flag { get; set; }",
            "[UInt(5)] public byte Kind { get; set; }",
            "public void Write(GamePacketWriter writer)");
    }

    // The legacy writer's overloads default to the argument type's size, and an enum to 64 bits, which an
    // int-backed enum declares as its 32 bits plus 32 bits of zeros.
    [Fact]
    public void DefaultWidthsComeFromTheChosenOverload()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public enum Mode
            {
                Idle,
                Busy,
            }

            public class Sample : IWritable
            {
                public uint Small { get; set; }
                public ulong Large { get; set; }
                public byte Tiny { get; set; }
                public float Ratio { get; set; }
                public double Precise { get; set; }
                public Mode Current { get; set; }
                public short Offset { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Small);
                    writer.Write(Large);
                    writer.Write(Tiny);
                    writer.Write(Ratio);
                    writer.Write(Precise);
                    writer.Write(Current);
                    writer.Write(Offset);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[WireStruct] public sealed partial class Sample : IWritable",
            "[UInt(32)] public uint Small",
            "[UInt(64)] public ulong Large",
            "[UInt(8)] public byte Tiny",
            "[Float32] public float Ratio",
            "[Float64] public double Precise",
            "[UInt(32)] public Mode Current",
            "[Pad(32)] public Pad CurrentHigh { get; }",
            "[UInt(16)] public short Offset",
            "[Pad(16)] public Pad OffsetHigh { get; }");
    }

    [Fact]
    public void ACountBeforeItsListBecomesCountBitsAndTheElementTypeAWireStruct()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Entry : IWritable
            {
                public uint Value { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Value, 18u);
                }
            }

            public class Holder : IWritable
            {
                public List<Entry> Entries { get; set; } = [];

                public void Write(GamePacketWriter writer)
                {
                    writer.Write((byte)Entries.Count, 5u);
                    Entries.ForEach(entry => entry.Write(writer));
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[WireStruct] public sealed partial class Entry",
            "[UInt(18)] public uint Value",
            "[WireStruct] public sealed partial class Holder",
            "[Array(CountBits = 5)] public List<Entry> Entries { get; set; } = [];");
    }

    // The seed writes one count and then each list as a column; the count's list need not be the first written.
    [Fact]
    public void AColumnarCountBecomesASharedCountTheListsName()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Keys : IWritable
            {
                public List<uint> Times { get; set; } = [];
                public List<Vector3> Positions { get; set; } = [];
                public bool Blend { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Positions.Count, 10u);
                    foreach (uint time in Times)
                        writer.Write(time);
                    Positions.ForEach(writer.WriteVector3);
                    writer.Write(Blend);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[UInt(10), Count] public ushort TimesCount { get; set; }",
            "[Array(CountFrom = nameof(TimesCount)), UInt(32)] public List<uint> Times",
            "[Array(CountFrom = nameof(TimesCount)), Float32] public List<Vector3> Positions",
            "[UInt(1)] public bool Blend");
    }

    [Fact]
    public void AConstantZeroBecomesAPadAndAVector3WrittenByComponentIsOneFloat32()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Sample : IWritable
            {
                public Vector3 Position { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(0u, 3u);
                    writer.Write(Position.X);
                    writer.Write(Position.Y);
                    writer.Write(Position.Z);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[Pad(3)] public Pad Pad0 { get; }",
            "[Float32] public Vector3 Position");
    }

    [Fact]
    public void VectorsReadAndWrittenByComponentAreOneVectorEachWay()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Place : IReadable, IWritable
            {
                public Vector3 Spot { get; private set; }
                public Vector2 Grid { get; private set; }

                public void Read(GamePacketReader reader)
                {
                    Spot = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                    Grid = new Vector2(reader.ReadPackedFloat(), reader.ReadPackedFloat());
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Spot.X);
                    writer.Write(Spot.Y);
                    writer.Write(Spot.Z);
                    writer.WritePackedFloat(Grid.X);
                    writer.WritePackedFloat(Grid.Y);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[Float32] public Vector3 Spot { get; private set; }",
            "[PackedFloat] public Vector2 Grid { get; private set; }");
    }

    [Fact]
    public void AQuaternionReadAndWrittenByComponentIsOneValueEachWay()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Facing : IReadable, IWritable
            {
                public Quaternion Rotation { get; private set; }

                public void Read(GamePacketReader reader)
                {
                    Rotation = new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Rotation.X);
                    writer.Write(Rotation.Y);
                    writer.Write(Rotation.Z);
                    writer.Write(Rotation.W);
                }
            }
            """);

        ConverterHarness.AssertInOrder(ConverterHarness.Converted(result), "[Float32] public Quaternion Rotation { get; private set; }");
    }

    [Fact]
    public void AFixedArrayTakesItsLengthFromItsInitializer()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Sample : IWritable
            {
                public uint[] Slots { get; set; } = new uint[3];

                public void Write(GamePacketWriter writer)
                {
                    for (int i = 0; i < Slots.Length; i++)
                        writer.Write(Slots[i], 8u);
                }
            }
            """);

        ConverterHarness.AssertInOrder(ConverterHarness.Converted(result), "[FixedArray(3), UInt(8)] public uint[] Slots");
    }

    [Fact]
    public void AReadCountLoopOfModelsBecomesCountBitsOnAClientMessage()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Entry : IReadable
            {
                public ushort Value { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Value = reader.ReadUShort(14u);
                }
            }

            [Message(GameMessageOpcode.ClientCastSpell)]
            public class Request : IReadable
            {
                public List<Entry> Entries { get; } = [];
                public string Name { get; private set; }

                public void Read(GamePacketReader reader)
                {
                    uint count = reader.ReadUInt(4u);
                    for (int i = 0; i < count; i++)
                    {
                        var entry = new Entry();
                        entry.Read(reader);
                        Entries.Add(entry);
                    }

                    Name = reader.ReadWideString();
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[UInt(14)] public ushort Value",
            "[WireMessage(GameMessageOpcode.ClientCastSpell, Direction.ClientToServer)] public sealed partial class Request : IReadable",
            "[Array(CountBits = 4)] public List<Entry> Entries { get; } = [];",
            "[WideString] public string Name { get; private set; }");
    }

    [Fact]
    public void ReadAndWriteThatAgreeMakeABothWaysMessage()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            [Message(GameMessageOpcode.ServerHello)]
            public class Echo : IReadable, IWritable
            {
                public uint Value { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Value = reader.ReadUInt(20u);
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Value, 20u);
                }
            }
            """);

        ConverterHarness.AssertInOrder(ConverterHarness.Converted(result), "Direction.Both", "[UInt(20)] public uint Value");
    }

    [Fact]
    public void ReadAndWriteThatDisagreeStayLegacy()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Seed : IReadable, IWritable
            {
                public uint Value { get; set; }

                public void Read(GamePacketReader reader)
                {
                    Value = reader.ReadUInt(14u);
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Value);
                }
            }
            """);

        TypeOutcome outcome = ConverterHarness.Outcome(result, "Tests.Seed");
        Assert.False(outcome.Converted);
        Assert.Contains("disagree", outcome.Reason, StringComparison.Ordinal);
        Assert.Empty(result.Rewritten);
    }

    [Fact]
    public void ATypeNestingAnUnconvertibleTypeStaysLegacy()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Inner : IWritable
            {
                public byte Type { get; set; }
                public bool Flag { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Type, 2u);
                    if (Type == 0)
                        writer.Write(Flag);
                }
            }

            public class Holder : IWritable
            {
                public Inner Value { get; set; } = new();

                public void Write(GamePacketWriter writer)
                {
                    Value.Write(writer);
                }
            }
            """);

        Assert.False(ConverterHarness.Outcome(result, "Tests.Inner").Converted);
        TypeOutcome holder = ConverterHarness.Outcome(result, "Tests.Holder");
        Assert.False(holder.Converted);
        Assert.Contains("Inner, which stays legacy", holder.Reason, StringComparison.Ordinal);
        Assert.Empty(result.Rewritten);
    }

    // A legacy holder may still call a converted member's legacy Write, so a converted type keeps it.
    [Fact]
    public void AConvertedTypeKeepsItsLegacyMethodsAndMarksMembersOffTheWire()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public class Sample : IReadable, IWritable
            {
                private readonly int _seed = 4;

                public uint Value { get; set; }
                public int Cached { get; set; }
                public int Doubled => _seed * 2;

                public void Read(GamePacketReader reader)
                {
                    Value = reader.ReadUInt();
                }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Value);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "public sealed partial class Sample : IReadable, IWritable",
            "[NotOnWire] private readonly int _seed = 4;",
            "[UInt(32)] public uint Value",
            "[NotOnWire] public int Cached",
            "[NotOnWire] public int Doubled",
            "public void Read(GamePacketReader reader)",
            "public void Write(GamePacketWriter writer)");
    }

    [Fact]
    public void AnInterfaceMemberIsLeftForTheUnionPass()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            public interface IModel : IWritable
            {
            }

            public class Holder : IWritable
            {
                public byte Type { get; set; }
                public IModel Model { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Type, 4u);
                    Model.Write(writer);
                }
            }
            """);

        TypeOutcome holder = ConverterHarness.Outcome(result, "Tests.Holder");
        Assert.False(holder.Converted);
        Assert.Contains("union", holder.Reason, StringComparison.Ordinal);
    }

    // dotnet format orders plain usings first, then aliases; the codec using joins the plain ones.
    [Fact]
    public void TheCodecUsingJoinsThePlainUsingsBeforeAnAlias()
    {
        ConversionResult result = ConverterHarness.Convert("""
            using System.Text;
            using Builder = System.Text.StringBuilder;

            namespace Tests;

            public class Sample : IWritable
            {
                public uint Value { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Value);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "using System.Text; using NexusTomorrow.Network.Codec; using Builder = System.Text.StringBuilder;");
    }

    // An attribute on a type binds its arguments in the type's scope, where a member named Direction hides the enum.
    [Fact]
    public void AMemberNamedDirectionQualifiesTheDirectionEnum()
    {
        ConversionResult result = ConverterHarness.Convert("""
            namespace Tests;

            [Message(GameMessageOpcode.ServerHello)]
            public class Turn : IWritable
            {
                public byte Direction { get; set; }

                public void Write(GamePacketWriter writer)
                {
                    writer.Write(Direction, 2u);
                }
            }
            """);

        ConverterHarness.AssertInOrder(
            ConverterHarness.Converted(result),
            "[WireMessage(GameMessageOpcode.ServerHello, NexusTomorrow.Network.Codec.Direction.ServerToClient)]",
            "[UInt(2)] public byte Direction");
    }
}
