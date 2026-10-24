using System.Buffers;
using System.Globalization;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Nodes;
using NexusTomorrow.Commands;
using NexusTomorrow.Gateway.Connections;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Owners;
using NexusTomorrow.Owners.Account;
using NexusTomorrow.Transport;

namespace NexusTomorrow.Tools;

/// <summary>
/// The endpoint's requests: <c>{"id": n, "verb": "...", ...}</c>, each answered with <c>{"id": n, "ok": true, ...}</c> or
/// <c>{"id": n, "ok": false, "error": "..."}</c>.
/// </summary>
/// <remarks>
/// <list type="bullet">
/// <item><c>status</c>: the build and the accounts online.</item>
/// <item><c>inject</c>: <c>account</c>, <c>opcode</c> (<c>"0xNNNN"</c>), and either <c>fields</c> (every wire member, see
/// <see cref="MessageBinder"/>) or <c>body</c> (hex, sent as given). A built message must read back to the same bytes with no whole
/// byte left over; it is then queued on the account's world session below the send gate. Answered with the body sent.</item>
/// <item><c>tap</c>: <c>account</c>; from the answer on, every message the account's world session reads or writes arrives as a
/// <c>{"tap": {...}}</c> line (<see cref="SessionRecorder"/>), a message the server read with its decoded fields, for as long as this
/// connection lasts.</item>
/// <item><c>layout</c>: <c>opcode</c>; the server message's members in wire order, each with its path, width and encoding
/// (<see cref="LayoutRows"/>).</item>
/// <item><c>command</c>: <c>line</c>, a command line as the server's console takes it, run through <see cref="CommandDispatcher"/> as
/// the actor <c>oracle</c>, which holds every permission and is audited under that name. Answered with the command's outcome and its
/// reply text.</item>
/// </list>
/// </remarks>
public sealed class OracleVerbs
{
    private static readonly TimeSpan s_lookup = TimeSpan.FromSeconds(5);
    private static readonly CommandActor s_actor = new("oracle", FrontEnd.Cli, new HashSet<string> { PermissionKeys.All }, null);

    private readonly AccountOwner _accounts;
    private readonly CommandDispatcher _commands;
    private readonly TimeProvider _time;

    /// <summary>
    /// Create the verbs.
    /// </summary>
    public OracleVerbs(AccountOwner accounts, CommandDispatcher commands, TimeProvider time)
    {
        _accounts = accounts;
        _commands = commands;
        _time = time;
    }

    /// <summary>
    /// Answer one request from <paramref name="connection"/>; a verb that fails on an error it does not name is answered with that
    /// error, so the connection goes on.
    /// </summary>
    public async Task<JsonObject> HandleAsync(JsonObject request, OracleConnection connection, CancellationToken cancellationToken)
    {
        JsonNode? id = request["id"]?.DeepClone();
        string? verb = Text(request["verb"]);
        JsonObject reply;
        try
        {
            reply = verb switch
            {
                "status" => Status(),
                "inject" => await InjectAsync(request, cancellationToken),
                "tap" => await TapAsync(request, connection, cancellationToken),
                "layout" => Layout(request),
                "command" => await CommandAsync(request, cancellationToken),
                null => Error("a request names its verb"),
                _ => Error($"no verb {verb}"),
            };
        }
        catch (Exception exception) when (exception is not OperationCanceledException)
        {
            reply = Error($"{verb} failed: {exception.GetType().Name}: {exception.Message}");
        }

        reply["id"] = id;
        return reply;
    }

    /// <summary>
    /// Create the server message <paramref name="opcode"/> names, from the world's declarations first, then the login's, then the shared
    /// ones; null when none declares it.
    /// </summary>
    internal static IWireMessage? CreateServerMessage(ushort opcode) =>
        Network.World.WireMessages.CreateServerToClient(opcode)
        ?? Network.Auth.WireMessages.CreateServerToClient(opcode)
        ?? Network.WireMessages.CreateServerToClient(opcode);

    /// <summary>
    /// Build the message <paramref name="opcode"/> names from <paramref name="fields"/>, and check that it reads back to the same bytes
    /// with no whole byte left over. Returns the message and its body, or the reason it was refused.
    /// </summary>
    internal static (IWireStruct? Message, byte[] Body, string? Refusal) Build(ushort opcode, JsonObject fields)
    {
        if (CreateServerMessage(opcode) is not { } message)
            return (null, [], $"no server message 0x{opcode:X4}");

        try
        {
            MessageBinder.Bind(message, fields);
            byte[] body = Serialize(message);
            IWireMessage again = CreateServerMessage(opcode)!;
            var reader = new BitReader(body);
            again.Read(ref reader);
            if (reader.BitsRemaining >= 8)
                return (null, [], $"{reader.BitsRemaining} bits left after reading it back");
            if (!Serialize(again).AsSpan().SequenceEqual(body))
                return (null, [], "it does not read back as written");

            return (message, body, null);
        }
        catch (Exception exception) when (exception is ArgumentException or CodecException)
        {
            return (null, [], exception.Message);
        }
    }

    private async Task<JsonObject> InjectAsync(JsonObject request, CancellationToken cancellationToken)
    {
        if (Text(request["account"]) is not { } account)
            return Error("inject names an account");
        if (Opcode(request["opcode"]) is not { } opcode)
            return Error("inject names an opcode as 0xNNNN");

        bool hasBody = request["body"] is not null;
        if (hasBody == request["fields"] is not null)
            return Error("inject takes either fields or body");
        if (await FindAsync(account, cancellationToken) is not IDebugSession session)
            return Error($"{account} has no world session");

        if (hasBody)
        {
            byte[] raw;
            try
            {
                raw = Convert.FromHexString(Text(request["body"]) ?? "");
            }
            catch (FormatException)
            {
                return Error("body is a hex string");
            }

            session.SendRaw(opcode, raw);
            return Sent("raw", raw);
        }

        if (request["fields"] is not JsonObject fields)
            return Error("fields is an object");

        (IWireStruct? message, byte[] body, string? refusal) = Build(opcode, fields);
        if (message is null)
            return Error(refusal!);

        session.SendUnchecked(opcode, message);
        return Sent(message.GetType().Name, body);
    }

    private async Task<JsonObject> TapAsync(JsonObject request, OracleConnection connection, CancellationToken cancellationToken)
    {
        if (Text(request["account"]) is not { } account)
            return Error("tap names an account");
        if (await FindAsync(account, cancellationToken) is not IDebugSession session)
            return Error($"{account} has no world session");

        connection.Hold(session, new SessionRecorder(connection, _time));
        return new JsonObject { ["ok"] = true, ["session"] = session.Id };
    }

    private static JsonObject Layout(JsonObject request)
    {
        if (Opcode(request["opcode"]) is not { } opcode)
            return Error("layout names an opcode as 0xNNNN");
        if (CreateServerMessage(opcode) is not { } message)
            return Error($"no server message 0x{opcode:X4}");

        var members = new JsonArray();
        foreach ((string path, string bits, string encoding) in LayoutRows.Of(message.GetType()))
            members.Add(new JsonObject { ["path"] = path, ["bits"] = bits, ["encoding"] = encoding });
        return new JsonObject { ["ok"] = true, ["type"] = message.GetType().Name, ["members"] = members };
    }

    private async Task<JsonObject> CommandAsync(JsonObject request, CancellationToken cancellationToken)
    {
        if (Text(request["line"]) is not { } line)
            return Error("command names a line");

        CommandReply reply = await _commands.DispatchAsync(s_actor, line, cancellationToken);
        return new JsonObject { ["ok"] = true, ["outcome"] = reply.Outcome.ToString(), ["text"] = reply.Text };
    }

    private async Task<IClientSession?> FindAsync(string account, CancellationToken cancellationToken)
    {
        var reply = new TaskCompletionSource<IClientSession?>(TaskCreationOptions.RunContinuationsAsynchronously);
        if (!_accounts.Post(new FindOnlineSession(account, reply)))
            return null;

        try
        {
            return await reply.Task.WaitAsync(s_lookup, cancellationToken);
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    private JsonObject Status() => new()
    {
        ["ok"] = true,
        ["build"] = typeof(OracleVerbs).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
        ["online"] = _accounts.OnlineCount,
    };

    private static byte[] Serialize(IWireStruct message)
    {
        byte[] rented = FrameWriter.RentBody(message, out int length);
        try
        {
            return rented.AsSpan(0, length).ToArray();
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(rented, clearArray: true);
        }
    }

    private static string? Text(JsonNode? node) =>
        node is JsonValue value && value.GetValueKind() == JsonValueKind.String ? value.GetValue<string>() : null;

    private static ushort? Opcode(JsonNode? node) =>
        Text(node) is { } text && text.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
            && ushort.TryParse(text.AsSpan(2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ushort opcode)
            ? opcode
            : null;

    private static JsonObject Sent(string type, byte[] body) => new() { ["ok"] = true, ["type"] = type, ["body"] = Convert.ToHexString(body) };

    private static JsonObject Error(string error) => new() { ["ok"] = false, ["error"] = error };
}
