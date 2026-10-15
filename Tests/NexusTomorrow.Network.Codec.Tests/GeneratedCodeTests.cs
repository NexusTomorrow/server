using System.Numerics;
using NexusTomorrow.Network.Codec.Tests.Fixtures;

namespace NexusTomorrow.Network.Codec.Tests;

public sealed class GeneratedCodeTests
{
    private static WireLayout LayoutOf<T>()
        where T : IWireMessage
    {
        return T.Layout;
    }

    private static byte[] WriteAll(IWireStruct value)
    {
        int bits = value.MeasureBits();
        byte[] buffer = new byte[(bits + 7) / 8];
        var writer = new BitWriter(buffer);
        value.Write(ref writer);
        Assert.Equal(bits, writer.BitPosition);
        return buffer;
    }

    private static T ReadAll<T>(byte[] bytes)
        where T : IWireStruct, new()
    {
        var value = new T();
        var reader = new BitReader(bytes);
        value.Read(ref reader);
        return value;
    }

    [Fact]
    public void AFixedMessageWritesItsFieldsInDeclarationOrder()
    {
        var message = new CastSpellFixture { ClientSpellCastUniqueId = 0x01020304, BagIndex = 0x0506, TargetUnitId = 0x0708090A, ButtonPressed = true };

        byte[] bytes = WriteAll(message);

        Assert.Equal([0x04, 0x03, 0x02, 0x01, 0x06, 0x05, 0x0A, 0x09, 0x08, 0x07, 0x01], bytes);
        CastSpellFixture read = ReadAll<CastSpellFixture>(bytes);
        Assert.Equal(0x0708090Au, read.TargetUnitId);
        Assert.True(read.ButtonPressed);
    }

    [Fact]
    public void AFixedMessageDeclaresItsOpcodeDirectionAndWidths()
    {
        WireLayout layout = LayoutOf<CastSpellFixture>();

        Assert.Equal(0x009A, layout.Opcode);
        Assert.Equal(Direction.ClientToServer, layout.Direction);
        Assert.True(layout.IsFixed);
        Assert.Equal([32, 16, 32, 1], layout.EmptyPathWidths);
        Assert.Equal(81, layout.EmptyPathBits);
        Assert.Empty(layout.Parts);
    }

    [Fact]
    public void EveryScalarEncodingRoundTrips()
    {
        var message = new ScalarsFixture
        {
            Flag = true,
            Small = 5,
            Medium = 0x3FFF,
            Large = ulong.MaxValue,
            FullWidth = -1,
            Offset = -300,
            Kind = FixtureKind.Last,
            SignedKind = FixtureSignedKind.Low,
            Ratio = 1.5f,
            Precise = Math.PI,
            Packed = 0.5f,
            Position = new Vector3(1, 2, 3),
            PackedPosition = new Vector3(0.5f, -2, 1),
            Note = "not on the wire",
        };

        byte[] bytes = WriteAll(message);
        ScalarsFixture read = ReadAll<ScalarsFixture>(bytes);

        Assert.Equal(386, message.MeasureBits());
        Assert.Equal(49, bytes.Length);
        Assert.True(read.Flag);
        Assert.Equal(5, read.Small);
        Assert.Equal(0x3FFF, read.Medium);
        Assert.Equal(ulong.MaxValue, read.Large);
        Assert.Equal(-1, read.FullWidth);
        Assert.Equal(-300, read.Offset);
        Assert.Equal(FixtureKind.Last, read.Kind);
        Assert.Equal(FixtureSignedKind.Low, read.SignedKind);
        Assert.Equal(1.5f, read.Ratio);
        Assert.Equal(Math.PI, read.Precise);
        Assert.Equal(0.5f, read.Packed);
        Assert.Equal(new Vector3(1, 2, 3), read.Position);
        Assert.Equal(new Vector3(0.5f, -2, 1), read.PackedPosition);
        Assert.Equal("", read.Note);
    }

    [Fact]
    public void AValueWiderThanItsFieldFailsTheWrite()
    {
        var message = new ScalarsFixture { Small = 9 };

        Assert.Throws<CodecException>(() => WriteAll(message));
    }

    [Fact]
    public void StringsRoundTripAndMeasureTheirLength()
    {
        var message = new StringsFixture { Wide = "h\u00E9llo", Narrow = "abc", Terminated = "end" };

        StringsFixture read = ReadAll<StringsFixture>(WriteAll(message));

        Assert.Equal(200, message.MeasureBits());
        Assert.Equal("h\u00E9llo", read.Wide);
        Assert.Equal("abc", read.Narrow);
        Assert.Equal("end", read.Terminated);
    }

    [Fact]
    public void AnEmptyTerminatedStringIsItsCountAndTerminatorOnTheEmptyPath()
    {
        WireLayout layout = LayoutOf<StringsFixture>();

        Assert.Equal([1, 7, 1, 7, 16, 16], layout.EmptyPathWidths);
        Assert.Equal(
            [WirePartShape.WideString, WirePartShape.NarrowString, WirePartShape.TerminatedWideString],
            layout.Parts.Select(part => part.Shape));
        Assert.Equal([0, 2, 4], layout.Parts.Select(part => part.At));
    }

    [Fact]
    public void ArraysRoundTripAndTheSharedCountTakesTheirLength()
    {
        var message = new CollectionsFixture { Fixed = [1, 2, 3] };
        message.Ids.AddRange([1, 0x3FFFF]);
        message.Times.AddRange([10, 20]);
        message.Values.AddRange([1.5f, -2f]);
        message.Points.Add(new PointFixture { X = 1, Y = 2 });

        CollectionsFixture read = ReadAll<CollectionsFixture>(WriteAll(message));

        Assert.Equal(2u, message.KeyCount);
        Assert.Equal(213, message.MeasureBits());
        Assert.Equal([1u, 0x3FFFFu], read.Ids);
        Assert.Equal([10u, 20u], read.Times);
        Assert.Equal([1.5f, -2f], read.Values);
        Assert.Equal([1, 2, 3], read.Fixed);
        Assert.Equal(2, read.Points[0].Y);
    }

    [Fact]
    public void ArraysSharingACountMustHaveOneLength()
    {
        var message = new CollectionsFixture();
        message.Times.AddRange([10, 20]);
        message.Values.Add(1.5f);

        Assert.Throws<CodecException>(() => WriteAll(message));
    }

    [Fact]
    public void AFixedArrayOfTheWrongLengthFailsTheWrite()
    {
        var message = new CollectionsFixture { Fixed = [1, 2] };

        Assert.Throws<CodecException>(() => WriteAll(message));
    }

    [Fact]
    public void AnArrayLongerThanItsCountWidthFailsTheWrite()
    {
        var message = new CollectionsFixture();
        message.Ids.AddRange([1, 2, 3, 4, 5, 6, 7, 8]);

        Assert.Throws<CodecException>(() => WriteAll(message));
    }

    [Fact]
    public void ACountLargerThanTheBodyFailsTheReadBeforeAnyElementIsMade()
    {
        // A 3-bit count of 7 18-bit ids in a one-byte body.
        Assert.Throws<CodecException>(() => ReadAll<CollectionsFixture>([0x07]));
    }

    [Fact]
    public void ArrayPartsSitAtTheirCountsAndCarryTheirElementBits()
    {
        WireLayout layout = LayoutOf<CollectionsFixture>();

        Assert.Equal([3, 4, 8, 8, 8, 2], layout.EmptyPathWidths);
        Assert.Equal([0, 1, 1, 5], layout.Parts.Select(part => part.At));
        Assert.Equal([18, 32, 32, 16], layout.Parts.Select(part => part.ElementBits));
        Assert.False(layout.IsFixed);
    }

    [Fact]
    public void AUnionWritesItsArmsSelectorAndReadsTheArmBack()
    {
        var message = new ChatFixture { Channel = new ChannelFixture { ChannelType = 4, ChatId = 0x1122334455667788 }, Message = "Hi", ChatMessageId = 7 };
        message.Formats.Add(new ChatFormatFixture { StartIndex = 0, StopIndex = 2, Model = new ChatFormatItemFixture { ItemId = 0x2ABCD } });

        ChatFixture read = ReadAll<ChatFixture>(WriteAll(message));

        Assert.Equal(2, message.Formats[0].Type);
        Assert.Equal(193, message.MeasureBits());
        Assert.Equal(0x1122334455667788ul, read.Channel.ChatId);
        Assert.Equal("Hi", read.Message);
        ChatFormatItemFixture arm = Assert.IsType<ChatFormatItemFixture>(read.Formats[0].Model);
        Assert.Equal(0x2ABCDu, arm.ItemId);
        Assert.Equal(7, read.ChatMessageId);
    }

    [Fact]
    public void ASelectorWithNoArmFailsTheRead()
    {
        // Type 7 in 4 bits, then two zero 16-bit indexes.
        Assert.Throws<CodecException>(() => ReadAll<ChatFormatFixture>([0x07, 0x00, 0x00, 0x00, 0x00]));
    }

    [Fact]
    public void AUnionArrayCarriesTheElementBitsOfArmZero()
    {
        WireLayout layout = LayoutOf<ChatFixture>();

        Assert.Equal([14, 64, 1, 7, 5, 16], layout.EmptyPathWidths);
        Assert.Equal([WirePartShape.WideString, WirePartShape.Array], layout.Parts.Select(part => part.Shape));
        Assert.Equal(37, layout.Parts[1].ElementBits);
    }

    [Fact]
    public void AnAlignedBlobCountsItsLengthAndSkipsToAByte()
    {
        var message = new EnvelopeFixture { Prefix = 3, Payload = [0xAA, 0xBB] };

        byte[] bytes = WriteAll(message);

        Assert.Equal([0x03, 0x06, 0x00, 0x00, 0x00, 0xAA, 0xBB], bytes);
        Assert.Equal([0xAA, 0xBB], ReadAll<EnvelopeFixture>(bytes).Payload);
    }

    [Fact]
    public void ABaseLayoutComesFirstAndPaddingIsZero()
    {
        var message = new DerivedFixture { BaseValue = 0xAB, DerivedValue = 0xF };

        byte[] bytes = WriteAll(message);

        Assert.Equal([0xAB, 0x00, 0xF0], bytes);
        Assert.Equal([8, 12, 4], LayoutOf<DerivedFixture>().EmptyPathWidths);
        DerivedFixture read = ReadAll<DerivedFixture>(bytes);
        Assert.Equal(0xAB, read.BaseValue);
        Assert.Equal(0xF, read.DerivedValue);
    }

    [Fact]
    public void ANestedTypeRoundTrips()
    {
        var inner = new OuterFixture.InnerFixture { Value = 63 };

        Assert.Equal(63, ReadAll<OuterFixture.InnerFixture>(WriteAll(inner)).Value);
        Assert.Equal(6, inner.MeasureBits());
    }

    [Fact]
    public void TheRegistryCreatesEachMessageForItsDirections()
    {
        Assert.IsType<CastSpellFixture>(WireMessages.CreateClientToServer(0x009A));
        Assert.Null(WireMessages.CreateServerToClient(0x009A));
        Assert.IsType<ScalarsFixture>(WireMessages.CreateClientToServer(0xFF01));
        Assert.IsType<ScalarsFixture>(WireMessages.CreateServerToClient(0xFF01));
        Assert.Null(WireMessages.CreateClientToServer(0x1234));
    }

    [Fact]
    public void TheRegistryListsEveryLayoutInOpcodeOrder()
    {
        Assert.Equal(
            [0x009A, 0x0104, 0x01C3, 0xFF01, 0xFF02, 0xFF03, 0xFF04, 0xFF05],
            WireMessages.Layouts.Select(layout => (int)layout.Opcode));
    }
}
