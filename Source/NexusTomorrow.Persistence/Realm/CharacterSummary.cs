using System.Collections.Immutable;
using System.Numerics;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// A live character as the character list shows it.
/// </summary>
public sealed record CharacterSummary(
    ulong Id, long AccountId, string Name, byte Race, byte Sex, byte Class, uint FactionId, byte Path, uint Level,
    ushort WorldId, ushort ZoneId, Vector3 Position, bool Disabled, bool RequiresRename, ImmutableArray<float> Bones);
