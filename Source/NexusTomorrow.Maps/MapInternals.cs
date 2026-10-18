using NexusTomorrow.Owners.World;

namespace NexusTomorrow.Maps;

internal enum MapTimerKind
{
    Save,
}

// Holds the character by id: the timer re-resolves it when it fires, so a character that left in the meantime costs nothing.
internal readonly record struct MapTimer(MapTimerKind Kind, ulong CharacterId);

// Posted by a save's completion; names the arrival's serial so a save from an earlier stay cannot mark this one clean.
internal sealed record CharacterSaved(ulong CharacterId, long Serial, uint Version) : MapMessage;
