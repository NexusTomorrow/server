using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Tells the client a <see cref="CharacterSelect"/> failed; the client returns to idle and passes the reason to its interface.
/// </summary>
/// <remarks>
/// Taken only while a selection is pending.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0162, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterSelectFailed
{
    /// <summary>
    /// Why the selection failed. The interface has a message of its own for 1 (the character must be renamed first), 3 (the character
    /// is disabled) and 4 (the character is already in the world), and a general message for 2 and any other value.
    /// </summary>
    [UInt(3)] public byte Reason { get; set; }
}
