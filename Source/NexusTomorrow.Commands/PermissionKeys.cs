namespace NexusTomorrow.Commands;

/// <summary>
/// The permission keys the built-in commands check. A key is dotted, and <see cref="All"/> grants every key.
/// </summary>
public static class PermissionKeys
{
    /// <summary>
    /// Every permission.
    /// </summary>
    public const string All = "*";

    /// <summary>
    /// Create an account.
    /// </summary>
    public const string AccountCreate = "account.create";

    /// <summary>
    /// Suspend an account.
    /// </summary>
    public const string AccountSuspend = "account.suspend";

    /// <summary>
    /// Lift an account's suspension.
    /// </summary>
    public const string AccountUnsuspend = "account.unsuspend";

    /// <summary>
    /// Read the server's status.
    /// </summary>
    public const string ServerStatus = "server.status";
}
