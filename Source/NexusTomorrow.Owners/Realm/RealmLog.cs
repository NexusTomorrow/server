using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners.Realm;

internal static partial class RealmLog
{
    [LoggerMessage(EventId = 1101, Level = LogLevel.Warning, Message = "Table {Path} is missing or unreadable; no character creation option is offered")]
    public static partial void TableUnavailable(ILogger logger, string path, Exception? exception);

    [LoggerMessage(EventId = 1102, Level = LogLevel.Warning, Message = "No client data path is configured; no character creation option is offered")]
    public static partial void NoClientDataPath(ILogger logger);

    [LoggerMessage(EventId = 1103, Level = LogLevel.Warning, Message = "Character {CharacterId} cannot enter world {World}: no map hosts it")]
    public static partial void WorldNotHosted(ILogger logger, ulong characterId, ushort world);
}
