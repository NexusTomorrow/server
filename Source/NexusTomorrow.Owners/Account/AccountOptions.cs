namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The <c>Account</c> configuration section.
/// </summary>
public sealed class AccountOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "Account";

    /// <summary>
    /// How long a game token or a session key stays redeemable (decisions.md D9 2b: about a minute).
    /// </summary>
    public TimeSpan TokenLifetime { get; set; } = TimeSpan.FromSeconds(60);
}
