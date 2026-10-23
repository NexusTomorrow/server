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
/// <see cref="WireSecretAttribute"/>, at any depth, is recorded without its bytes.
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
        _connection.Send(Line(new JsonObject
        {
            ["dir"] = "c2s",
            ["n"] = Interlocked.Increment(ref _received),
            ["t"] = _time.GetUtcNow().ToUnixTimeMilliseconds(),
            ["session"] = session,
            ["opcode"] = $"0x{inbound.Opcode:X4}",
            ["result"] = inbound.Result.ToString(),
            ["reason"] = inbound.Reason,
            ["type"] = inbound.Message?.GetType().Name,
            ["envelope"] = secret ? null : Convert.ToHexString(envelope),
            ["redacted"] = secret,
        }));
    }

    /// <inheritdoc/>
    public void Sent(long session, ushort opcode, ReadOnlySpan<byte> body, Type messageType)
    {
        bool secret = HoldsSecret(messageType);
        _connection.Send(Line(new JsonObject
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
    /// Whether <paramref name="type"/> or any wire struct it holds has a member marked <see cref="WireSecretAttribute"/>.
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
            if (inner.GetCustomAttribute<WireStructAttribute>() is not null && HoldsSecret(inner, seen))
                return true;
        }

        return false;
    }

    private static Type? ClientMessageType(ushort opcode) =>
        (Network.World.WireMessages.CreateClientToServer(opcode)
            ?? Network.Auth.WireMessages.CreateClientToServer(opcode)
            ?? Network.WireMessages.CreateClientToServer(opcode))?.GetType();

    private static string Line(JsonObject record) => new JsonObject { ["tap"] = record }.ToJsonString();
}
