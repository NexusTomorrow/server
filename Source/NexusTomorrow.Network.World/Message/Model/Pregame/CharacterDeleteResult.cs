using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// The answer to <see cref="CharacterDelete"/>.
/// </summary>
/// <remarks>
/// Taken only while a deletion is pending.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00E6, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterDeleteResult
{
    /// <summary>
    /// The result. 0 means the character was deleted: the client removes it from its list. The interface has a message of its own for
    /// 2 (the character leads a guild) and 42 (the character is online), and a general message for the rest.
    /// </summary>
    [UInt(6)] public byte Result { get; set; }

    /// <summary>
    /// A number the interface puts into result 2's message; it reads it for no other result.
    /// </summary>
    [UInt(32)] public uint Count { get; set; }
}
