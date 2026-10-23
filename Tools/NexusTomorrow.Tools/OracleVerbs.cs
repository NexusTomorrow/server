using System.Reflection;
using System.Text.Json.Nodes;
using NexusTomorrow.Owners.Account;

namespace NexusTomorrow.Tools;

/// <summary>
/// The endpoint's requests: <c>{"id": n, "verb": "...", ...}</c>, each answered with <c>{"id": n, "ok": true, ...}</c> or
/// <c>{"id": n, "ok": false, "error": "..."}</c>.
/// </summary>
public sealed class OracleVerbs
{
    private readonly AccountOwner _accounts;

    /// <summary>
    /// Create the verbs.
    /// </summary>
    public OracleVerbs(AccountOwner accounts)
    {
        _accounts = accounts;
    }

    /// <summary>
    /// Answer one request from <paramref name="connection"/>.
    /// </summary>
    public Task<JsonObject> HandleAsync(JsonObject request, OracleConnection connection, CancellationToken cancellationToken)
    {
        JsonNode? id = request["id"]?.DeepClone();
        string? verb = (request["verb"] as JsonValue)?.TryGetValue(out string? name) == true ? name : null;
        JsonObject reply = verb switch
        {
            "status" => Status(),
            null => Error("a request names its verb"),
            _ => Error($"no verb {verb}"),
        };
        reply["id"] = id;
        return Task.FromResult(reply);
    }

    private JsonObject Status() => new()
    {
        ["ok"] = true,
        ["build"] = typeof(OracleVerbs).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
        ["online"] = _accounts.OnlineCount,
    };

    private static JsonObject Error(string error) => new() { ["ok"] = false, ["error"] = error };
}
