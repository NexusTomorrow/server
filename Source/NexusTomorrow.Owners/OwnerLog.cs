using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners;

internal static partial class OwnerLog
{
    [LoggerMessage(EventId = 1001, Level = LogLevel.Error, Message = "Owner {Owner} failed handling {Message}")]
    public static partial void HandlerFailed(ILogger logger, string owner, string message, Exception exception);

    [LoggerMessage(EventId = 1002, Level = LogLevel.Warning, Message = "Owner {Owner} has handled nothing for {Seconds:F0} s with {Queued} messages queued")]
    public static partial void Stalled(ILogger logger, string owner, double seconds, int queued);
}
