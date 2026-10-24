using System.Collections.Concurrent;
using System.Reflection;
using System.Text.Json.Nodes;
using NexusTomorrow.Gateway.Catalog;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Network.Codec;

namespace NexusTomorrow.Tools;

/// <summary>
/// The <c>tap</c> verb's tap: every message a session reads or writes, sent to an endpoint connection as
/// <c>{"tap": {...}}</c> lines. Each direction is numbered from 1 in the order the session handled it, so the n-th message sent is the
/// n-th the client receives on this connection after the tap was attached. A message whose type has a member marked
/// <see cref="WireSecretAttribute"/>, in itself, a wire type it holds, or an arm of a union it holds, is recorded without its bytes.
/// A received message carries its decoded <c>fields</c> (<see cref="MessageDump"/>), which a secret one never does: the message the
/// server read, or, for one the table dropped unread, the message read here with its declaration (<c>read_by</c> says which). A
/// record the connection cannot take (it has ended, or left its outbox full) throws, so the session detaches the tap rather than
/// record with gaps.
/// </summary>
public sealed class SessionRecorder : ISessionTap
{
    private static readonly ConcurrentDictionary<Type, bool> s_secret = new();

    private readonly OracleConnection _connection;
    private readonly TimeProvider _time;
    private long _sent;
    private long _received;

    /// <summary>
    /// Create a recorder that writes to <paramref name="connection"/>.
    /// </summary>
    public SessionRecorder(OracleConnection connection, TimeProvider time)
    {
        _connection = connection;
        _time = time;
    }

    /// <inheritdoc/>
    public void Received(long session, ReadOnlySpan<byte> envelope, Inbound inbound)
    {
        // A frame whose decode failed has no message; the client's declaration for its opcode says whether it can carry a secret.
        Type? type = inbound.Message?.GetType() ?? ClientMessageType(inbound.Opcode);
        bool secret = type is not null && HoldsSecret(type);

        // A declared message the table dropped unread is read here with its declaration, unless that declaration holds a secret; a
        // malformed or undeclared frame is not read.
        bool readHere = !secret && inbound.Message is null && inbound.Result == InboundResult.Dropped && type is not null;
        Send(Line(new JsonObject
        {
            ["dir"] = "c2s",
            ["n"] = Interlocked.Increment(ref _received),
            ["t"] = _time.GetUtcNow().ToUnixTimeMilliseconds(),
            ["session"] = session,
            ["opcode"] = $"0x{inbound.Opcode:X4}",
            ["result"] = inbound.Result.ToString(),
            ["reason"] = inbound.Reason,
            ["type"] = (inbound.Message?.GetType() ?? (readHere ? type : null))?.Name,
            ["envelope"] = secret ? null : Convert.ToHexString(envelope),
            ["fields"] = secret ? null : inbound.Message is not null ? Fields(inbound.Message) : readHere ? Unread(envelope, inbound.Opcode) : null,
            ["read_by"] = secret ? null : inbound.Message is not null ? "server" : readHere ? "tap" : null,
            ["redacted"] = secret,
        }));
    }

    // A decoded message (the server's, or one Unread read), as MessageDump writes it and serialized here, so a value JSON cannot carry
    // fails inside the catch too. Whatever fails is named in "$error" instead of thrown, since a tap that throws is detached.
    private static JsonNode Fields(IWireMessage message)
    {
        try
        {
            JsonObject fields = MessageDump.Of(message);
            return JsonNode.Parse(fields.ToJsonString())!;
        }
        catch (Exception exception)
        {
            return new JsonObject { ["$error"] = $"{exception.GetType().Name}: {exception.Message}" };
        }
    }

    // A dropped frame's message read with the declaration of the opcode the decoder found, the envelope opened as the decoder opens
    // it; an envelope that opens to another opcode, or a frame that does not read or leaves a whole byte over, is named in "$error".
    private static JsonNode Unread(ReadOnlySpan<byte> envelope, ushort expected)
    {
        if (!InboundDecoder.TryOpenClear(envelope.ToArray(), out ushort opcode, out byte[] body))
            return new JsonObject { ["$error"] = "the envelope does not open" };
        if (opcode != expected)
            return new JsonObject { ["$error"] = $"the envelope opens to 0x{opcode:X4}, not 0x{expected:X4}" };
        if (CreateClientMessage(opcode) is not { } message)
            return new JsonObject { ["$error"] = $"0x{opcode:X4} has no declaration" };

        try
        {
            var reader = new BitReader(body);
            message.Read(ref reader);
            return reader.BitsRemaining >= 8
                ? new JsonObject { ["$error"] = $"{reader.BitsRemaining} bits left after the message" }
                : Fields(message);
        }
        catch (Exception exception)
        {
            return new JsonObject { ["$error"] = $"{exception.GetType().Name}: {exception.Message}" };
        }
    }

    /// <inheritdoc/>
    public void Sent(long session, ushort opcode, ReadOnlySpan<byte> body, Type messageType)
    {
        bool secret = HoldsSecret(messageType);
        Send(Line(new JsonObject
        {
            ["dir"] = "s2c",
            ["n"] = Interlocked.Increment(ref _sent),
            ["t"] = _time.GetUtcNow().ToUnixTimeMilliseconds(),
            ["session"] = session,
            ["opcode"] = $"0x{opcode:X4}",
            ["type"] = messageType.Name,
            ["body"] = secret ? null : Convert.ToHexString(body),
            ["redacted"] = secret,
        }));
    }

    /// <summary>
    /// Whether <paramref name="type"/>, a wire type it holds, or any arm of a union it holds has a member marked
    /// <see cref="WireSecretAttribute"/>.
    /// </summary>
    internal static bool HoldsSecret(Type type) => s_secret.GetOrAdd(type, static t => HoldsSecret(t, []));

    private static bool HoldsSecret(Type type, HashSet<Type> seen)
    {
        if (!seen.Add(type))
            return false;

        const BindingFlags Members = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        foreach (MemberInfo member in type.GetProperties(Members).Cast<MemberInfo>().Concat(type.GetFields(Members)))
        {
            if (member.GetCustomAttribute<WireSecretAttribute>() is not null)
                return true;

            Type memberType = member is PropertyInfo property ? property.PropertyType : ((FieldInfo)member).FieldType;
            Type inner = memberType.IsArray ? memberType.GetElementType()! : memberType.IsGenericType ? memberType.GetGenericArguments()[0] : memberType;
            if (MessageBinder.IsWireType(inner) && HoldsSecret(inner, seen))
                return true;
            if (member.GetCustomAttribute<UnionAttribute>() is not null && MessageBinder.Arms(member, inner).Any(arm => HoldsSecret(arm, seen)))
                return true;
        }

        return false;
    }

    private static Type? ClientMessageType(ushort opcode) => CreateClientMessage(opcode)?.GetType();

    private static IWireMessage? CreateClientMessage(ushort opcode) =>
        Network.World.WireMessages.CreateClientToServer(opcode)
            ?? Network.Auth.WireMessages.CreateClientToServer(opcode)
            ?? Network.WireMessages.CreateClientToServer(opcode);

    private static string Line(JsonObject record) => new JsonObject { ["tap"] = record }.ToJsonString();

    private void Send(string line)
    {
        if (!_connection.Send(line))
            throw new InvalidOperationException("the endpoint connection took no more records");
    }
}
