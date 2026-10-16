using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The answer to <see cref="CharacterRename"/>.
/// </summary>
/// <remarks>
/// Taken only while a rename is pending. The client also reports 15 itself, without sending, for a name that fails its own check.
/// </remarks>
[WireMessage(GameMessageOpcode.Server014B, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterRenameResult
{
    /// <summary>
    /// The result. 18 means the character was renamed: the client applies the new name and clears the character's rename-required
    /// mark. The interface has a message of its own for 14 (an internal error), 15 (the name is not allowed), 16 (the name is taken),
    /// 17 (the character may not be renamed) and 43 (the character is online), and a general message for the rest.
    /// </summary>
    [UInt(6)] public byte Result { get; set; }

    /// <summary>
    /// The character, as listed in the character list (0x0117).
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; set; }

    /// <summary>
    /// The character's new name, applied when the result is 18.
    /// </summary>
    [WideString] public string Name { get; set; } = "";
}
