using System.Buffers;
using System.Collections.Frozen;
using System.Numerics;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Entity;
using NexusTomorrow.Network.World.Entity.Command;
using NexusTomorrow.Network.World.Message.Model.Movement;
using NexusTomorrow.Network.World.Message.Model.Pregame;
using NexusTomorrow.Network.World.Message.Model.Store;
using NexusTomorrow.Network.World.Message.Model.Story;
using NexusTomorrow.Network.World.Message.Model.WorldEntry;
using NexusTomorrow.Testing;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Tests;

public sealed class InboundDecoderTests
{
    private static readonly PacketCipher s_cipher = new(PacketCipher.BuildKey);

    // The connection's admission is ClientConnection's to test; here every row the decoder resolves is admitted.
    private static readonly InboundAdmission s_admitAll = _ => true;

    private static InboundDecoder Decoder(params ushort[] disabled) =>
        new(MessageCatalog.Default, OpcodeTable.All, disabled.ToFrozenSet());

    [Fact]
    public void AnEnvelopedMessageIsDecrypted()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x0352, new CharacterDelete { CharacterId = 0x0102030405060708 }, s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.Equal(0x0102030405060708UL, Assert.IsType<CharacterDelete>(inbound.Message).CharacterId);
    }

    [Fact]
    public void CharacterCreateIsTakenFromInsideTheNestedWrapper()
    {
        var create = new CharacterCreate { CreationId = 5, Name = "Rebaser Head", Path = 2 };
        create.Bones.Add(0.5f);
        byte[] body = ClientFrames.NestedEnvelopeBody(0x025B, create, s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal(InboundResult.Message, inbound.Result);
        CharacterCreate read = Assert.IsType<CharacterCreate>(inbound.Message);
        Assert.Equal("Rebaser Head", read.Name);
        Assert.Equal([0.5f], read.Bones);
    }

    [Fact]
    public void AMessageBelowFieldsIsDroppedNotMalformed()
    {
        // 0x0356 is a declared client message with no rung. The table gives it a row, so the catalog's refusal drops it, not a
        // missing row.
        FrozenDictionary<ushort, OpcodeRow> table = new OpcodeRow[]
        {
            new(0x0356, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop),
        }.ToFrozenDictionary(row => row.Opcode);
        byte[] body = ClientFrames.EnvelopeBody(0x0356, new Client0356(), s_cipher);
        var decoder = new InboundDecoder(MessageCatalog.Default, table, new HashSet<ushort>());
        Inbound inbound = decoder.Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal(InboundResult.Dropped, inbound.Result);
        Assert.Equal(0x0356, inbound.Opcode);
        Assert.Equal(nameof(InboundRefusal.BelowFields), inbound.Reason);
    }

    [Fact]
    public void AMessageWithoutATableRowIsDropped()
    {
        // 0x07A4 is read to Fields but has no row in the opcode table, which this decoder holds whole (OpcodeTable.All).
        byte[] body = ClientFrames.EnvelopeBody(0x07A4, new RealmListRequest(), s_cipher);
        Assert.Equal(InboundResult.Dropped, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void ADisabledOpcodeIsDropped()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x07E0, new CharacterListRequest(), s_cipher);
        Assert.Equal(InboundResult.Dropped, Decoder(0x07E0).Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void ABodyWithBitsLeftAfterTheMessageIsMalformed()
    {
        // CharacterListRequest has no fields; a body byte after it is a byte the client never writes.
        byte[] body = ClientFrames.EnvelopeBody(0x0352, new CharacterDelete(), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, Rewrap(body, 0x07E0), s_cipher, s_admitAll);
        Assert.Equal(InboundResult.Malformed, inbound.Result);
    }

    [Fact]
    public void AFrameThatIsNotTheClientEnvelopeIsMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x07E0, new CharacterListRequest(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(0x0076, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void ABlobWhoseLengthLiesIsMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x07E0, new CharacterListRequest(), s_cipher);
        body[0]++;
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal((InboundResult.Malformed, "the envelope's length is not its own"), (inbound.Result, inbound.Reason));
    }

    [Fact]
    public void ANestedBlobWhoseLengthLiesIsMalformed()
    {
        var frame = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(frame, 0x07E0, new CharacterListRequest());
        byte[] lying = [.. frame.WrittenSpan];
        lying[0]++;
        byte[] body = ClientFrames.EnvelopeBody(ClientFrames.Nested, new RawBody(lying), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal((InboundResult.Malformed, ClientFrames.Nested, "the nested blob's length is not its own"),
            (inbound.Result, inbound.Opcode, inbound.Reason));
    }

    [Theory]
    [InlineData(ClientFrames.ClientEnvelope)]
    [InlineData(ClientFrames.Nested)]
    public void ANestedWrapperHoldingAnotherWrapperIsMalformed(ushort inner)
    {
        byte[] body = ClientFrames.NestedEnvelopeBody(inner, new RawBody([0x01]), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal((InboundResult.Malformed, inner, "a wrapper nested past one level"), (inbound.Result, inbound.Opcode, inbound.Reason));
    }

    [Fact]
    public void ATaggedMessageWithAByteLeftAfterItIsMalformed()
    {
        var frame = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(frame, 0x00F2, new WorldReady());
        byte[] body = ClientFrames.EnvelopeBody(ClientFrames.Tagged, new RawBody([0x03, .. frame.WrittenSpan, 0xFF]), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal((InboundResult.Malformed, ClientFrames.Tagged, "8 bits left after the tagged message"),
            (inbound.Result, inbound.Opcode, inbound.Reason));
    }

    [Fact]
    public void ATaggedMessageWithoutAnInnerOpcodeIsMalformed()
    {
        // The tag, then a payload of one byte: its 32-bit length (5, counting itself) and half an opcode.
        byte[] body = ClientFrames.EnvelopeBody(ClientFrames.Tagged, new RawBody([0x03, 0x05, 0x00, 0x00, 0x00, 0xF2]), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal((InboundResult.Malformed, ClientFrames.Tagged, "a tagged message without an inner opcode"),
            (inbound.Result, inbound.Opcode, inbound.Reason));
    }

    [Fact]
    public void AnAdmittedBodyTheDeclarationCannotReadIsMalformedWithTheCodecsReason()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x0637, new RawBody(ClientFrames.OverclaimingMovement()), s_cipher);
        string codec = Assert.Throws<CodecException>(() =>
        {
            var reader = new BitReader(ClientFrames.OverclaimingMovement());
            new ClientMovement().Read(ref reader);
        }).Message;

        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);

        Assert.Equal((InboundResult.Malformed, (ushort)0x0637, codec), (inbound.Result, inbound.Opcode, inbound.Reason));
    }

    [Fact]
    public void AMessageInsideATaggedMessageIsTakenUnderItsRowsTag()
    {
        WorldReady ready = ClientMessage.With(new WorldReady(), nameof(WorldReady.ZoneId), (ushort)6);
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x00F2, ready, s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.Equal(0x00F2, inbound.Opcode);
        Assert.Equal(6, Assert.IsType<WorldReady>(inbound.Message).ZoneId);
    }

    [Fact]
    public void AClientMovementIsReadAtPurposeUnderItsRow()
    {
        var movement = new ClientMovement { ClientTime = 1000 };
        movement.Unknown1.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new SetPositionCommand { Position = new Vector3(1, 2, 3) } });
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x0637, movement, s_cipher);

        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);

        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.IsType<ClientMovement>(inbound.Message);
    }

    [Fact]
    public void AMessageUnderAnotherTagThanItsRowsIsMalformed()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x0B, 0x00F2, new WorldReady(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void ATaggedRowsMessageSentWithoutATagIsMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x00F2, new WorldReady(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void AnUntaggedRowsMessageSentUnderATagIsMalformed()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x07E0, new CharacterListRequest(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void ATaggedMessageWithNoRowIsDropped()
    {
        // The storefront request the client sends at character select travels inside 0x038C under tag 0x13 and has no row.
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x13, 0x082D, new Client082D(), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal(InboundResult.Dropped, inbound.Result);
        Assert.Equal(0x082D, inbound.Opcode);
    }

    [Fact]
    public void ATaggedMessageWhoseLengthLiesIsMalformed()
    {
        var frame = new ArrayBufferWriter<byte>();
        FrameWriter.WriteMessage(frame, 0x00F2, new WorldReady());
        byte[] lying = [.. frame.WrittenSpan];
        lying[0]++;
        byte[] body = ClientFrames.EnvelopeBody(ClientFrames.Tagged, new RawBody([0x03, .. lying]), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll).Result);
    }

    [Fact]
    public void ATaggedMessageHoldingAnotherWrapperIsMalformed()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, ClientFrames.Tagged, new RawBody([0x03]), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);
        Assert.Equal((InboundResult.Malformed, ClientFrames.Tagged, "a wrapper nested past one level"),
            (inbound.Result, inbound.Opcode, inbound.Reason));
    }

    [Fact]
    public void AMessageOverItsRowsSizeLimitIsMalformedBeforeItsBodyIsRead()
    {
        // 17 zero bytes read as a clock of 0, no commands and nine bytes left over: malformed if the body were read.
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x0637, new RawBody(new byte[17]), s_cipher);
        bool asked = false;

        Inbound inbound = LimitedDecoder(16).Decode(ClientFrames.ClientEnvelope, body, s_cipher, _ =>
        {
            asked = true;
            return true;
        });

        Assert.Equal(InboundResult.Malformed, inbound.Result);
        Assert.Equal("a body over its row's size limit", inbound.Reason);
        Assert.False(asked);
    }

    [Fact]
    public void AMessageAtItsRowsSizeLimitIsRead()
    {
        // 8 bytes of clock and command count, then ten commands of 5 selector bits and 1 value bit: 60 bits, 8 bytes.
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x0637, MovementOf(10), s_cipher);

        Inbound inbound = LimitedDecoder(16).Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);

        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.Equal(10, Assert.IsType<ClientMovement>(inbound.Message).Unknown1.Count);
    }

    [Fact]
    public void ARowWithoutALimitReadsALargeBody()
    {
        // 8 bytes, then 3000 commands of 6 bits: 18,000 bits, 2250 bytes, 2258 in all.
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x0637, MovementOf(3000), s_cipher);

        Inbound inbound = LimitedDecoder(null).Decode(ClientFrames.ClientEnvelope, body, s_cipher, s_admitAll);

        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.Equal(3000, Assert.IsType<ClientMovement>(inbound.Message).Unknown1.Count);
    }

    // A decoder whose only row is the movement's, under tag 0x03, with the given body limit.
    private static InboundDecoder LimitedDecoder(int? maxBodyBytes)
    {
        FrozenDictionary<ushort, OpcodeRow> table = new OpcodeRow[]
        {
            new(0x0637, MessageOwner.Map, MessagePriority.High, 60, OverLimitAction.Drop, OpcodeTable.WorldTag, ReadsVerifiedMembers: true, MaxBodyBytes: maxBodyBytes),
        }.ToFrozenDictionary(row => row.Opcode);
        return new InboundDecoder(MessageCatalog.Default, table, new HashSet<ushort>());
    }

    // A movement of one-bit commands (command 10, a 5-bit selector and a 1-bit value).
    private static ClientMovement MovementOf(int commands)
    {
        var movement = new ClientMovement { ClientTime = 1000 };
        for (int i = 0; i < commands; i++)
            movement.Unknown1.Add(new Server0262Part5 { Unknown0 = 10, Unknown1 = new Server0262Union2Arm0A { Unknown0 = true } });

        return movement;
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
}
