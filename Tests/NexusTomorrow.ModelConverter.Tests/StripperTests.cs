namespace NexusTomorrow.ModelConverter.Tests;

public sealed class StripperTests
{
    private const string Models = """
        using NexusTomorrow.Network.Codec;

        namespace Tests;

        public interface IArm : IReadable, IWritable, IWireStruct
        {
        }

        [WireStruct]
        [UnionCase(0)]
        public sealed partial class Arm : IArm
        {
            [UInt(8)] public byte Value { get; set; }

            public void Read(GamePacketReader reader)
            {
                Value = reader.ReadByte();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Value);
            }
        }

        // The sample message.
        [Message(GameMessageOpcode.ServerHello)]
        [WireMessage(GameMessageOpcode.ServerHello, Direction.Both)]
        public sealed partial class Sample : IReadable, IWritable
        {
            public class Row
            {
                public uint Id { get; set; }
            }

            [UInt(32), Count] public uint RowCount { get; set; }
            [Array(CountFrom = nameof(RowCount)), UInt(32)] public List<uint> Ids { get; set; } = [];
            [NotOnWire] public List<Row> Rows { get; set; } = [];
            [NotOnWire] public uint Kept { get; set; }
            [UInt(2)] public byte Kind { get; set; }
            [Union(nameof(Kind))] public IArm Payload { get; set; } = new Arm();

            [NotOnWire] public uint Total => Kept + 1;

            public void Read(GamePacketReader reader)
            {
                Kept = reader.ReadUInt();
            }

            public void Write(GamePacketWriter writer)
            {
                writer.Write(Rows.Count);
                Rows.ForEach(row => writer.Write(row.Id));
                writer.Write(Kept);
                writer.Write(Kind, 2u);
                Payload.Write(writer);
            }
        }
        """;

    [Fact]
    public void TheLegacyMethodsInterfacesAndMessageAttributeGo()
    {
        StripResult result = ConverterHarness.Strip(Models);
        string stripped = ConverterHarness.Stripped(result);

        Assert.Equal(4, result.Methods);
        Assert.DoesNotContain("GamePacketReader", stripped, StringComparison.Ordinal);
        Assert.DoesNotContain("GamePacketWriter", stripped, StringComparison.Ordinal);
        Assert.DoesNotContain("[Message(", stripped, StringComparison.Ordinal);
        ConverterHarness.AssertInOrder(
            stripped,
            "public interface IArm : IWireStruct {",
            "public sealed partial class Arm : IArm {",
            "// The sample message. [WireMessage(GameMessageOpcode.ServerHello, Direction.Both)] public sealed partial class Sample {");
    }

    // Rows only fed the legacy Write, and Row only typed Rows; Kept also feeds Total, so it stays.
    [Fact]
    public void AMemberOnlyTheLegacyCodeUsedGoesWithIt()
    {
        StripResult result = ConverterHarness.Strip(Models);
        string stripped = ConverterHarness.Stripped(result);

        Assert.DoesNotContain("Rows", stripped, StringComparison.Ordinal);
        Assert.DoesNotContain("class Row", stripped, StringComparison.Ordinal);
        ConverterHarness.AssertInOrder(stripped, "[NotOnWire] public uint Kept { get; set; }", "[NotOnWire] public uint Total => Kept + 1;");
        Assert.Contains(result.Removed, id => id.EndsWith("Sample.Rows", StringComparison.Ordinal));
        Assert.Contains(result.Removed, id => id.EndsWith("Sample.Row", StringComparison.Ordinal));
    }

    // An override is a declaration, not a reference, so the base member and its overrides go together or not at all.
    [Fact]
    public void AVirtualMemberGoesWithItsOverrides()
    {
        StripResult result = ConverterHarness.Strip("""
            using NexusTomorrow.Network.Codec;

            namespace Tests;

            [WireStruct]
            public abstract partial class Shape : IWritable
            {
                [NotOnWire] public virtual byte Kind { get; }

                [UInt(8)] public byte Size { get; set; }

                public virtual void Write(GamePacketWriter writer)
                {
                    writer.Write(Kind);
                    writer.Write(Size);
                }
            }

            [WireStruct]
            public sealed partial class Square : Shape
            {
                [NotOnWire] public override byte Kind => 1;
            }
            """);
        string stripped = Assert.Single(result.Files).Value.Replace("\r\n", "\n", StringComparison.Ordinal);

        Assert.DoesNotContain("Kind", stripped, StringComparison.Ordinal);
        Assert.Contains("{\n    [UInt(8)] public byte Size { get; set; }\n}", stripped, StringComparison.Ordinal);
        Assert.Empty(ConverterHarness.ErrorsOf(stripped));
    }

    [Fact]
    public void TheStrippedModelsStillBuild()
    {
        StripResult result = ConverterHarness.Strip(Models);

        Assert.Empty(ConverterHarness.ErrorsOf(Assert.Single(result.Files).Value));
    }
}
