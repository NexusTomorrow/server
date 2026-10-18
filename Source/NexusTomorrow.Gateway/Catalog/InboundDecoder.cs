using System.Buffers.Binary;
using System.Collections.Frozen;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.World.Message.Model.Connection;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Gateway.Catalog;

/// <summary>
/// What <see cref="InboundDecoder.Decode"/> made of a frame.
/// </summary>
public enum InboundResult
{
    /// <summary>
    /// A message the server may read, read in full.
    /// </summary>
    Message,

    /// <summary>
    /// A message whose own body the server does not read (a <c>0x038C</c> wrapper around it is read for its tag and opcode). Without a
    /// table row, switched off, undeclared, or read below the rung its row admits (<c>Fields</c>, or <c>Purpose</c> for a row that reads
    /// only verified members): counted and dropped by the caller, and the session continues. Refused by the connection's
    /// <see cref="InboundAdmission"/>: the admission has already acted on it, counting the drop or ending the session.
    /// </summary>
    Dropped,

    /// <summary>
    /// Bytes no client writes: a wrong envelope, a lying length, a wrapper nested past one level, a tagged message without an inner
    /// opcode, a tag other than its row's, a body over its row's size limit, a body the declaration cannot read, or bits left after it.
    /// The session ends. The envelope and a <c>0x025C</c> or <c>0x038C</c> wrapper are read before the admission, and the body's length
    /// is checked against the row's limit before it; the inner message's body is read, and so can be found unreadable, only once the
    /// connection's <see cref="InboundAdmission"/> has admitted its row.
    /// </summary>
    Malformed,
}

/// <summary>
/// The outcome of decoding one frame: the result, the inner opcode once known, the message when there is one, and why otherwise.
/// </summary>
public readonly record struct Inbound(InboundResult Result, ushort Opcode, IWireMessage? Message, string Reason);

/// <summary>
/// A connection's say over a client message, asked by <see cref="InboundDecoder.Decode"/> once it has resolved the message's row
/// (after reading any <c>0x038C</c> wrapper around it) and before it creates the message or reads the message's own body. True lets
/// the decoder create and read the message, if the catalog allows. False leaves the message's body unread, and the connection has
/// acted on the refusal itself: counted the drop, or ended the session.
/// </summary>
public delegate bool InboundAdmission(OpcodeRow row);

/// <summary>
/// Turns a client frame into a message, in this order: opens the <c>0x0244</c> envelope with the connection's cipher, opens a
/// <c>0x025C</c> or <c>0x038C</c> wrapper inside it, resolves the inner opcode's row (in the opcode table, under the tag the row
/// names, not switched off), ends the session for a body over the row's <see cref="OpcodeRow.MaxBodyBytes"/>, asks the connection's
/// <see cref="InboundAdmission"/>, and only for an admitted row creates the message, if the catalog lets the server read it, and reads
/// its body. The inner message of a row the size limit or the admission refuses never has its body read; only the envelope and
/// wrapper around it are.
/// </summary>
public sealed class InboundDecoder
{
    private const ushort ClientEnvelope = 0x0244;
    private const ushort Nested = 0x025C;
    private const ushort Tagged = 0x038C;

    private readonly MessageCatalog _catalog;
    private readonly FrozenDictionary<ushort, OpcodeRow> _table;
    private readonly IReadOnlySet<ushort> _disabled;

    /// <summary>
    /// Create a decoder over <paramref name="catalog"/>, admitting the rows of <paramref name="table"/> except the
    /// <paramref name="disabled"/> opcodes.
    /// </summary>
    public InboundDecoder(MessageCatalog catalog, FrozenDictionary<ushort, OpcodeRow> table, IReadOnlySet<ushort> disabled)
    {
        _catalog = catalog;
        _table = table;
        _disabled = disabled;
    }

    /// <summary>
    /// The row of the table this decoder admits <paramref name="opcode"/> by. Call it only for an opcode a decode returned as a
    /// message, which the table admitted.
    /// </summary>
    /// <exception cref="KeyNotFoundException">The table has no row for <paramref name="opcode"/>.</exception>
    public OpcodeRow RowFor(ushort opcode) => _table[opcode];

    /// <summary>
    /// Decode one frame's body, decrypted in place: the envelope and any wrapper, then the row, then the row's size limit, then
    /// <paramref name="admit"/>, then the message. <paramref name="admit"/> is asked at most once, and only for a row the table holds
    /// under the frame's tag that is not switched off and whose size limit the body respects; a row it refuses comes back
    /// <see cref="InboundResult.Dropped"/> with the inner message's body unread.
    /// </summary>
    public Inbound Decode(ushort frameOpcode, Span<byte> body, PacketCipher cipher, InboundAdmission admit)
    {
        if (frameOpcode != ClientEnvelope)
            return new Inbound(InboundResult.Malformed, frameOpcode, null, "not a client envelope");
        if (!EnvelopeReader.TryOpen(body, cipher, out ushort opcode, out Span<byte> inner))
            return new Inbound(InboundResult.Malformed, frameOpcode, null, "the envelope's length is not its own");

        byte? tag = null;
        if (opcode == Nested)
        {
            // The nested wrapper is itself a declared message: a 32-bit length counting itself, then one clear frame.
            if (!EnvelopeReader.TryOpen(inner, cipher: null, out opcode, out inner))
                return new Inbound(InboundResult.Malformed, Nested, null, "the nested blob's length is not its own");
        }
        else if (opcode == Tagged)
        {
            // The tagged message is a declared message too: its payload is the inner opcode and body, and its tag must be the one the
            // inner opcode's row names.
            var tagged = new TaggedMessage();
            try
            {
                var reader = new BitReader(inner);
                tagged.Read(ref reader);
                if (reader.BitsRemaining >= 8)
                    return new Inbound(InboundResult.Malformed, Tagged, null, $"{reader.BitsRemaining} bits left after the tagged message");
            }
            catch (CodecException exception)
            {
                return new Inbound(InboundResult.Malformed, Tagged, null, exception.Message);
            }

            if (tagged.Payload.Length < sizeof(ushort))
                return new Inbound(InboundResult.Malformed, Tagged, null, "a tagged message without an inner opcode");

            tag = tagged.Tag;
            opcode = BinaryPrimitives.ReadUInt16LittleEndian(tagged.Payload);
            inner = tagged.Payload.AsSpan(sizeof(ushort));
        }

        if (opcode is ClientEnvelope or Nested or Tagged)
            return new Inbound(InboundResult.Malformed, opcode, null, "a wrapper nested past one level");
        if (!_table.TryGetValue(opcode, out OpcodeRow? row))
            return new Inbound(InboundResult.Dropped, opcode, null, "no opcode table row");
        if (row.Tag != tag)
            return new Inbound(InboundResult.Malformed, opcode, null, "a tag other than its row's");
        if (_disabled.Contains(opcode))
            return new Inbound(InboundResult.Dropped, opcode, null, "switched off");
        if (row.MaxBodyBytes is int limit && inner.Length > limit)
            return new Inbound(InboundResult.Malformed, opcode, null, "a body over its row's size limit");
        if (!admit(row))
            return new Inbound(InboundResult.Dropped, opcode, null, "refused by the connection");

        IWireMessage? message = _catalog.CreateInbound(opcode, row.ReadsVerifiedMembers, out InboundRefusal refusal);
        if (message is null)
            return new Inbound(InboundResult.Dropped, opcode, null, refusal.ToString());

        try
        {
            var reader = new BitReader(inner);
            message.Read(ref reader);
            if (reader.BitsRemaining >= 8)
                return new Inbound(InboundResult.Malformed, opcode, null, $"{reader.BitsRemaining} bits left after the message");
        }
        catch (CodecException exception)
        {
            return new Inbound(InboundResult.Malformed, opcode, null, exception.Message);
        }

        return new Inbound(InboundResult.Message, opcode, message, "");
    }
}
