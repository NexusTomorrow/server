using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The answer to <see cref="CharacterCreate"/>. On success the client selects the new character.
/// </summary>
/// <remarks>
/// Taken only while a creation is pending.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00DC, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterCreateResult
{
    /// <summary>
    /// The new character's id, which the client then selects.
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; set; }

    /// <summary>
    /// The client gives this value no meaning.
    /// </summary>
    [UInt(32)] public uint Unknown1 { get; set; }

    /// <summary>
    /// The result. 3 means the character was created. Any other value is a failure the client hands to its interface, which has a
    /// message of its own for 6 (the name is taken), 9 (the account is full), 10 (the name is not allowed), 11 (the opposing faction)
    /// and 44 (a character is online), and a general message for the rest.
    /// </summary>
    [UInt(6)] public byte Result { get; set; }
}
