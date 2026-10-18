namespace NexusTomorrow.Commands;

/// <summary>
/// Where a command line came from.
/// </summary>
public enum FrontEnd
{
    /// <summary>
    /// The server's own console.
    /// </summary>
    Cli,

    /// <summary>
    /// A player's chat in the game.
    /// </summary>
    InGame,

    /// <summary>
    /// The web admin console.
    /// </summary>
    Web,
}

/// <summary>
/// Who runs a command: a name for the audit, the front end it came through, the permissions it holds, and where it connected from.
/// </summary>
public sealed record CommandActor(string Name, FrontEnd FrontEnd, IReadOnlySet<string> Permissions, string? RemoteAddress)
{
    /// <summary>
    /// Whether the actor holds <paramref name="permission"/>, directly or through <see cref="PermissionKeys.All"/>.
    /// </summary>
    public bool Holds(string permission) => Permissions.Contains(PermissionKeys.All) || Permissions.Contains(permission);
}
