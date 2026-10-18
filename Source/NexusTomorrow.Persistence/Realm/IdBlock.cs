namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// <paramref name="Count"/> consecutive ids starting at <paramref name="First"/>.
/// </summary>
public readonly record struct IdBlock(ulong First, int Count);
