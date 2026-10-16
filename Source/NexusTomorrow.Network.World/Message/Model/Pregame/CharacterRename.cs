using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Renames one listed character.
/// </summary>
/// <remarks>
/// The client then waits for the reply (0x014B).
/// </remarks>
[WireMessage(GameMessageOpcode.Client0786, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CharacterRename
{
    /// <summary>
    /// The character, as listed in the character list (0x0117).
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; private set; }

    /// <summary>
    /// The new name. The client sends only names that pass its own check; a server checks it again.
    /// </summary>
    [WideString] public string NewName { get; private set; } = "";
}
