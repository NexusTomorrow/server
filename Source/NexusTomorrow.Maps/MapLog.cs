using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Maps;

internal static partial class MapLog
{
    [LoggerMessage(EventId = 7001, Level = LogLevel.Error, Message = "Map {Map} failed a tick")]
    public static partial void TickFailed(ILogger logger, string map, Exception exception);

    [LoggerMessage(EventId = 7002, Level = LogLevel.Warning, Message = "Map {Map} asked for a dedicated thread, but every one is in use; it runs in the pool")]
    public static partial void NoDedicatedThread(ILogger logger, string map);

    [LoggerMessage(EventId = 7003, Level = LogLevel.Error, Message = "Map {World} failed handling {Message} for connection {ConnectionId}")]
    public static partial void HandlerFailed(ILogger logger, ushort world, string message, long connectionId, Exception exception);

    [LoggerMessage(EventId = 7004, Level = LogLevel.Error, Message = "Map {World} failed running its timers")]
    public static partial void TimersFailed(ILogger logger, ushort world, Exception exception);

    [LoggerMessage(EventId = 7005, Level = LogLevel.Debug, Message = "Map {World} took character {CharacterId} as unit {UnitId}")]
    public static partial void Entered(ILogger logger, ushort world, ulong characterId, uint unitId);

    [LoggerMessage(EventId = 7006, Level = LogLevel.Debug, Message = "Map {World} handed character {CharacterId} back")]
    public static partial void HandedBack(ILogger logger, ushort world, ulong characterId);

    [LoggerMessage(EventId = 7007, Level = LogLevel.Error, Message = "Map {World} was handed character {CharacterId}, which it already holds; connection {ConnectionId} is ended")]
    public static partial void AlreadyHeld(ILogger logger, ushort world, ulong characterId, long connectionId);
}
