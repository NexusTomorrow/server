namespace NexusTomorrow.Maps.Worlds;

/// <summary>
/// Sorts a World table row into a <see cref="WorldKind"/>.
/// </summary>
public static class WorldKinds
{
    /// <summary>
    /// Type 0 is the continents' type and bit 0 of the flags is one no continent sets: a type other than 0 is
    /// <see cref="WorldKind.Instanced"/>, type 0 with that bit is <see cref="WorldKind.Held"/>, and type 0 without it is
    /// <see cref="WorldKind.OpenWorld"/>.
    /// </summary>
    public static WorldKind Classify(uint type, uint flags)
    {
        if (type != 0)
            return WorldKind.Instanced;

        return (flags & 1) != 0 ? WorldKind.Held : WorldKind.OpenWorld;
    }
}
