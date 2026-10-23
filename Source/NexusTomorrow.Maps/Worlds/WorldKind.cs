namespace NexusTomorrow.Maps.Worlds;

/// <summary>
/// What the server does with a world.
/// </summary>
public enum WorldKind
{
    /// <summary>
    /// A world no table read holds, or no table was read.
    /// </summary>
    Unknown,

    /// <summary>
    /// One shared map every character in the world enters.
    /// </summary>
    OpenWorld,

    /// <summary>
    /// An open-world type with a flag no continent carries, such as the new player and ark-ship maps; held until that flag is
    /// understood, so no character enters it.
    /// </summary>
    Held,

    /// <summary>
    /// An instance type (dungeons, raids, PvP, housing and the rest), not hosted yet.
    /// </summary>
    Instanced,
}
