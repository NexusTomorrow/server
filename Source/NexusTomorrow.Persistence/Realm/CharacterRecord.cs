namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// A live character in full, as select loads it and a map holds it.
/// </summary>
public sealed record CharacterRecord(
    ulong Id, long AccountId, string Name, byte Race, byte Sex, byte Class, uint FactionId, byte Path, uint Level, uint TotalXp,
    CharacterLocation Location);
