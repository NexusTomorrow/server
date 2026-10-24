using Microsoft.Extensions.Logging;

namespace NexusTomorrow.Owners.Account;

internal static partial class AccountLog
{
    [LoggerMessage(EventId = 1301, Level = LogLevel.Warning, Message = "Table {Path} is missing or unreadable; an account's entitlements without a count are not sent")]
    public static partial void TableUnavailable(ILogger logger, string path, Exception? exception);

    [LoggerMessage(EventId = 1302, Level = LogLevel.Warning, Message = "No client data path is configured, so {Table} is not read")]
    public static partial void NoClientDataPath(ILogger logger, string table);

    [LoggerMessage(EventId = 1303, Level = LogLevel.Warning, Message = "Entitlement {EntitlementId} has no count and no Entitlement row, so no account is sent it")]
    public static partial void EntitlementWithoutRow(ILogger logger, uint entitlementId);
}
