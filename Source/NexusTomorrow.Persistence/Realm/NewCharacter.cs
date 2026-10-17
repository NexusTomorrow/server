using System.Collections.Immutable;
using System.Numerics;

namespace NexusTomorrow.Persistence.Realm;

/// <summary>
/// What a character create writes: the character's identity and look, as chosen at creation.
/// </summary>
public sealed record NewCharacter(
    ulong Id, long AccountId, string Name, uint CreationId, byte Race, byte Sex, byte Class, uint FactionId, byte Path, uint Level,
    uint TotalXp, ushort WorldId, ushort ZoneId, Vector3 Position, DateTimeOffset CreatedAt,
    ImmutableArray<(uint Label, uint Value)> Customizations, ImmutableArray<float> Bones)
{
    /// <summary>
    /// The character as the list shows it right after the create: enabled and not awaiting a rename.
    /// </summary>
    public CharacterSummary ToSummary() => new(
        Id, AccountId, Name, Race, Sex, Class, FactionId, Path, Level, WorldId, ZoneId, Position, Disabled: false, RequiresRename: false, Bones);
}
