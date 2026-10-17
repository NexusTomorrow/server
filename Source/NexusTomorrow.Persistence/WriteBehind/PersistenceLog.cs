using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Persistence;

internal static partial class PersistenceLog
{
    [LoggerMessage(EventId = 3001, Level = LogLevel.Warning, Message = "Persistence work {Work} failed; retrying in {Delay}")]
    public static partial void Retrying(ILogger logger, string work, TimeSpan delay, Exception exception);

    [LoggerMessage(EventId = 3002, Level = LogLevel.Error, Message = "Completing persistence work {Work} threw")]
    public static partial void CompletionFailed(ILogger logger, string work, Exception exception);

    [LoggerMessage(EventId = 3003, Level = LogLevel.Information, Message = "Waiting for {Backlog} pieces of persistence work to finish")]
    public static partial void Draining(ILogger logger, int backlog);
}
