using NexusTomorrow.Settings;

namespace NexusTomorrow.Owners.Account;

/// <summary>
/// The <c>AccountDefaults</c> configuration section: what every account holds until accounts store their own.
/// </summary>
[SettingsSection(Section, 8, "Owners", "Owners#knobs")]
public sealed class AccountDefaultsOptions
{
    /// <summary>
    /// The configuration section's name.
    /// </summary>
    public const string Section = "AccountDefaults";

    /// <summary>
    /// The entitlements every account holds, sent with the world grant before character select; unset, the full game's set
    /// (<see cref="FullGame"/>). A list given here replaces that set whole.
    /// </summary>
    [ComputedDefault("the full game's set: 4, 10, 14 to 22, 61 and 62, one of each, and the store licenses 12, 3, 27, 23, 25, 26, 36, 71, 74, 75, 42 and 43 at their caps")]
    [EntitlementGrants]
    public List<EntitlementGrant>? Entitlements { get; set; }

    /// <summary>
    /// The premium tier every account holds, sent with the world grant (<c>AccountPremiumTier</c>) and on every player's unit; 1 is
    /// the top tier of the premium system the client runs, the full game.
    /// </summary>
    [Between(0, 31)]
    public byte PremiumTier { get; set; } = 1;

    /// <summary>
    /// The creation level limit sent with the world grant: of the creation options that need no entitlement, the client offers those
    /// starting at this level or below (<c>CreationLevelLimit.Level</c>), and the realm creates only those.
    /// </summary>
    [AtLeast(1)]
    public uint CreationLevelLimit { get; set; } = 3;

    /// <summary>
    /// The entitlements the full game gives an account: Warplots access (4), the base set (10, 14 to 21), the Signature tier's own
    /// (22), and the Chua Warrior and Aurin Engineer unlocks (61, 62), one of each; and every store license at its cap: character
    /// slots (12), costume slots (3), bank bags (27), auctions (23), commodities (25), decor (26), costume displays (36), satchel (71),
    /// the shared realm bank and its slots (74, 75), and the marketplace's extra commodity orders and auctions (42, 43).
    /// </summary>
    public static IReadOnlyList<EntitlementGrant> FullGame() =>
    [
        .. new uint[] { 4, 10, 14, 15, 16, 17, 18, 19, 20, 21, 22, 61, 62 }.Select(id => new EntitlementGrant { Id = id, Count = 1 }),
        .. new uint[] { 12, 3, 27, 23, 25, 26, 36, 71, 74, 75, 42, 43 }.Select(id => new EntitlementGrant { Id = id }),
    ];

    /// <summary>
    /// The entitlements every account holds: <see cref="Entitlements"/> when it is set, else <see cref="FullGame"/>.
    /// </summary>
    public IReadOnlyList<EntitlementGrant> Held() => Entitlements ?? FullGame();
}
