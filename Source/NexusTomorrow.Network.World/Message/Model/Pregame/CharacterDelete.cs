using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Deletes one listed character.
/// </summary>
/// <remarks>
/// The client then waits for the reply (<see cref="CharacterDeleteResult"/>).
/// </remarks>
[WireMessage(GameMessageOpcode.Client0352, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CharacterDelete
{
    /// <summary>
    /// The character, as listed in the character list (0x0117).
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; private set; }
}
