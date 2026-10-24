using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners.Realm;

internal static partial class RealmLog
{
    [LoggerMessage(EventId = 1101, Level = LogLevel.Warning, Message = "Table {Path} is missing or unreadable; no character creation option is offered")]
    public static partial void TableUnavailable(ILogger logger, string path, Exception? exception);

    [LoggerMessage(EventId = 1102, Level = LogLevel.Warning, Message = "No client data path is configured; no character creation option is offered")]
    public static partial void NoClientDataPath(ILogger logger);

    // The line's text is never logged: a refused line is told by its length and reason only.
    [LoggerMessage(EventId = 1104, Level = LogLevel.Debug, Message = "Connection {ConnectionId}'s chat line of {Length} units was dropped: {Reason}")]
    public static partial void ChatDropped(ILogger logger, long connectionId, int length, string reason);

    [LoggerMessage(EventId = 1103, Level = LogLevel.Warning, Message = "Character {CharacterId} cannot enter world {World}: no map hosts it")]
    public static partial void WorldNotHosted(ILogger logger, ulong characterId, ushort world);
}
