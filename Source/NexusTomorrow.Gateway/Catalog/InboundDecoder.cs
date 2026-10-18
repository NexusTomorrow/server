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
    /// A well-formed message the server does not read: undeclared, below <c>Fields</c>, without a table row, or switched off. Counted
    /// and dropped; the session continues.
    /// </summary>
    Dropped,

    /// <summary>
    /// Bytes no client writes: a wrong envelope, a lying length, a body the declaration cannot read, or bits left after it. The
    /// session ends.
    /// </summary>
    Malformed,
}

/// <summary>
/// The outcome of decoding one frame: the result, the inner opcode once known, the message when there is one, and why otherwise.
/// </summary>
public readonly record struct Inbound(InboundResult Result, ushort Opcode, IWireMessage? Message, string Reason);

/// <summary>
/// Turns a client frame into a message: opens the <c>0x0244</c> envelope with the connection's cipher, opens a <c>0x025C</c> or
/// <c>0x038C</c> wrapper inside it, then reads the message if the opcode table admits it under the tag it came with and the catalog
/// lets the server read it.
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
    /// Decode one frame's body. The body is decrypted in place.
    /// </summary>
    public Inbound Decode(ushort frameOpcode, Span<byte> body, PacketCipher cipher)
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
