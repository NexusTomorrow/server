using System.Buffers;
using System.Collections.Frozen;
using System.Numerics;
using NexusTomorrow.Gateway.Catalog;
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

    private static InboundDecoder Decoder(params ushort[] disabled) =>
        new(MessageCatalog.Default, OpcodeTable.All, disabled.ToFrozenSet());

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
        // 0x0356 is a declared client message with no rung. The table gives it a row, so the catalog's refusal drops it, not a
        // missing row.
        FrozenDictionary<ushort, OpcodeRow> table = new OpcodeRow[]
        {
            new(0x0356, MessageOwner.Realm, MessagePriority.Normal, 1, OverLimitAction.Drop),
        }.ToFrozenDictionary(row => row.Opcode);
        byte[] body = ClientFrames.EnvelopeBody(0x0356, new Client0356(), s_cipher);
        var decoder = new InboundDecoder(MessageCatalog.Default, table, new HashSet<ushort>());
        Inbound inbound = decoder.Decode(ClientFrames.ClientEnvelope, body, s_cipher);
        Assert.Equal(InboundResult.Dropped, inbound.Result);
        Assert.Equal(0x0356, inbound.Opcode);
        Assert.Equal(nameof(InboundRefusal.BelowFields), inbound.Reason);
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

    [Fact]
    public void AMessageInsideATaggedMessageIsTakenUnderItsRowsTag()
    {
        WorldReady ready = ClientMessage.With(new WorldReady(), nameof(WorldReady.ZoneId), (ushort)6);
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x00F2, ready, s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher);
        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.Equal(0x00F2, inbound.Opcode);
        Assert.Equal(6, Assert.IsType<WorldReady>(inbound.Message).ZoneId);
    }

    [Fact]
    public void AClientMovementIsReadAtPurposeUnderItsRow()
    {
        var movement = new ClientMovement { Unknown0 = 1000 };
        movement.Unknown1.Add(new Server0262Part5 { Unknown0 = 2, Unknown1 = new Server0262Union2Arm02 { Unknown0 = new Vector3(1, 2, 3) } });
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x0637, movement, s_cipher);

        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher);

        Assert.Equal(InboundResult.Message, inbound.Result);
        Assert.IsType<ClientMovement>(inbound.Message);
    }

    [Fact]
    public void AMessageUnderAnotherTagThanItsRowsIsMalformed()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x0B, 0x00F2, new WorldReady(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    [Fact]
    public void ATaggedRowsMessageSentWithoutATagIsMalformed()
    {
        byte[] body = ClientFrames.EnvelopeBody(0x00F2, new WorldReady(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    [Fact]
    public void AnUntaggedRowsMessageSentUnderATagIsMalformed()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, 0x07E0, new CharacterListRequest(), s_cipher);
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    [Fact]
    public void ATaggedMessageWithNoRowIsDropped()
    {
        // The storefront request the client sends at character select travels inside 0x038C under tag 0x13 and has no row.
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x13, 0x082D, new Client082D(), s_cipher);
        Inbound inbound = Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher);
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
        Assert.Equal(InboundResult.Malformed, Decoder().Decode(ClientFrames.ClientEnvelope, body, s_cipher).Result);
    }

    [Fact]
    public void ATaggedMessageHoldingAnotherWrapperIsMalformed()
    {
        byte[] body = ClientFrames.TaggedEnvelopeBody(0x03, ClientFrames.Tagged, new RawBody([0x03]), s_cipher);
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
}
