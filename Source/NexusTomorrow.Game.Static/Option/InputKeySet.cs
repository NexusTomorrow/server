namespace NexusTomorrow.Game.Static.Option;

/// <summary>
/// The player's key binding sets, as GameLib.CodeEnumInputSets names them. The server stores the bindings of <see cref="Account"/> and
/// <see cref="Character"/>; the client asks it for no other set's.
/// </summary>
public enum InputKeySet
{
    Default1 = 0,
    Default2 = 1,
    Default3 = 2,

    /// <summary>
    /// The account's own bindings, which the client keys by owner 0.
    /// </summary>
    Account = 3,

    /// <summary>
    /// The character's own bindings, which the client keys by the character's id.
    /// </summary>
    Character = 4,
}
