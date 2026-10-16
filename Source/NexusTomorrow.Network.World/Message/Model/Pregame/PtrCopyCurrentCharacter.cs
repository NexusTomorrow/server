using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Asks to copy the current character to the test realm, from inside the world.
/// </summary>
/// <remarks>
/// Sent on the world connection, at most once every three seconds from the interface or without that limit from a chat command. The
/// character-select version is 0x06E7, which carries a character id. The answer is <see cref="PtrCopyQueued"/>.
/// </remarks>
[WireMessage(GameMessageOpcode.Client06E8, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class PtrCopyCurrentCharacter
{
}
