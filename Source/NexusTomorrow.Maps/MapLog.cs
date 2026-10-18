using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Maps;

internal static partial class MapLog
{
    [LoggerMessage(EventId = 7001, Level = LogLevel.Error, Message = "Map {Map} failed a tick")]
    public static partial void TickFailed(ILogger logger, string map, Exception exception);

    [LoggerMessage(EventId = 7002, Level = LogLevel.Warning, Message = "Map {Map} asked for a dedicated thread, but every one is in use; it runs in the pool")]
    public static partial void NoDedicatedThread(ILogger logger, string map);
}
