using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners.Text;

// The text checks' log lines. A text a player typed is never logged, and neither is a filter row's own text.
internal static partial class TextLog
{
    [LoggerMessage(EventId = 1201, Level = LogLevel.Warning, Message = "Table {Path} is missing or unreadable; the word filter allows every text")]
    public static partial void TableUnavailable(ILogger logger, string path, Exception? exception);

    [LoggerMessage(EventId = 1202, Level = LogLevel.Warning, Message = "No client data path is configured; the word filter allows every text")]
    public static partial void NoClientDataPath(ILogger logger);

    [LoggerMessage(EventId = 1203, Level = LogLevel.Warning, Message = "Word filter row {Index} is a pattern that does not compile; it is skipped")]
    public static partial void PatternSkipped(ILogger logger, int index, Exception exception);

    [LoggerMessage(EventId = 1204, Level = LogLevel.Warning, Message = "Word filter row {Index} has type {Type}, which the filter does not know; it is skipped")]
    public static partial void UnknownRowType(ILogger logger, int index, uint type);
}
