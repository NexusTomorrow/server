using System.Collections.Frozen;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests;

public sealed class InboundDecoderTests
{
    private static readonly PacketCipher s_cipher = new(PacketCipher.BuildKey);

    private static InboundDecoder Decoder(params ushort[] disabled) =>
        new(MessageCatalog.Default, OpcodeTable.Pregame, disabled.ToFrozenSet());

    [Fact]
    public void AnEnvelopedMessageIsDecrypted()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x0352, new CharacterDelete { CharacterId = 0x0102030405060708 }, s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher);
        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.Equal(0x0102030405060708UL, Assert.IsType<CharacterDelete>(inbound.Message).CharacterId);
    }

    [Fact]
    public void CharacterCreateIsTakenFromInsideTheNestedWrapper()
    {
        var create = new CharacterCreate { CreationId = 5, Name = "Rebaser Head", Path = 2 };
        create.Bones.Add(0.5f);
        byte[] body = ClientFrames.NestedEnvelopeBody(0x025B, create, s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher);
        Assert.Equal(InboundResult.Message, inbound.Result);
        CharacterCreate read = Assert.IsType<CharacterCreate>(inbound.Message);
        Assert.Equal("Rebaser Head", read.Name);
        Assert.Equal([0.5f], read.Bones);
    }

    [Fact]
    public void AMessageBelowFieldsIsDroppedNotMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x038C, new UnverifiedStandIn(), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher);
        Assert.Equal(InboundResult.Dropped, inbound.Result);
        Assert.Equal(0x038C, inbound.Opcode);
    }

    [Fact]
    public void AMessageWithoutATableRowIsDropped()
    {
        // 0x07A4 is read to Fields but has no row in the pregame table.
        byte[] body = ClientFrames.EnvelopeBody(0x07A4, new RealmListRequest(), s_cipher);
        Assert.Equal(InboundResult.Dropped, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    [Fact]
    public void ADisabledOpcodeIsDropped()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x07E0, new CharacterListRequest(), s_cipher);
        Assert.Equal(InboundResult.Dropped, Decoder(0x07E0).Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    [Fact]
    public void ABodyWithBitsLeftAfterTheMessageIsMalformed()
    {
        // CharacterListRequest has no fields; a body byte after it is a byte the client never writes.
        byte[] body = ClientFrames.EnvelopeBody(0x0352, new CharacterDelete(), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, Rewrap(body, 0x07E0), s_cipher);
        Assert.Equal(InboundResult.Malformed, inbound.Result);
    }

    [Fact]
    public void AFrameThatIsNotTheClientEnvelopeIsMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x07E0, new CharacterListRequest(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(0x0076, body, s_cipher).Result);
    }

    [Fact]
    public void ABlobWhoseLengthLiesIsMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x07E0, new CharacterListRequest(), s_cipher);
        body[0]++;
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    // Decrypt an envelope body, replace its inner opcode, and encrypt it again.
    private static byte[] Rewrap(byte[] body, ushort opcode)
    {
        byte[] copy = [.. body];
        s_cipher.Decrypt(copy.AsSpan(4));
        copy[4] = (byte)opcode;
        copy[5] = (byte)(opcode >> 8);
        s_cipher.Encrypt(copy.AsSpan(4));
        return copy;
    }

    // Writes nothing: stands in for a message the catalog refuses before reading.
    private sealed class UnverifiedStandIn : Network.Codec.IWireStruct
    {
        public void Read(ref Network.Codec.BitReader reader) => throw new NotSupportedException();

        public void Write(ref Network.Codec.BitWriter writer)
        {
        }

        public int MeasureBits() => 0;
    }
}
