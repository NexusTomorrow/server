using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Commands;

// A command's arguments never appear here: a parameter may be secret, and only the command's own name is logged.
internal static partial class CommandsLog
{
    [LoggerMessage(EventId = 5001, Level = LogLevel.Error, Message = "Command {Command} failed")]
    public static partial void Failed(ILogger logger, string command, Exception exception);

    [LoggerMessage(EventId = 5002, Level = LogLevel.Warning, Message = "Command {Command} was denied to {Actor} from {FrontEnd}")]
    public static partial void Denied(ILogger logger, string command, string actor, FrontEnd frontEnd);
}
