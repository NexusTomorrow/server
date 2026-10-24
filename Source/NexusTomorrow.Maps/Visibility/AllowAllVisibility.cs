namespace NexusTomorrow.Maps.Visibility;

/// <summary>
/// The predicate a map uses until phasing or stealth exists: every player sees every other in range.
/// </summary>
public sealed class AllowAllVisibility : IVisibilityPredicate
{
    /// <summary>
    /// The one instance.
    /// </summary>
    public static AllowAllVisibility Instance { get; } = new();

    /// <inheritdoc/>
    public bool CanSee(ulong viewerCharacterId, ulong targetCharacterId) => true;
}
