using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Server;

// Nothing here takes a configuration value: a log line must stay safe to share.
internal static partial class ServerLog
{
    [LoggerMessage(EventId = 6001, Level = LogLevel.Information, Message = "Still waiting for {Backlog} queued saves to commit; the database is reachable: {Reachable}")]
    public static partial void WaitingForSaves(ILogger logger, int backlog, bool reachable);
}
