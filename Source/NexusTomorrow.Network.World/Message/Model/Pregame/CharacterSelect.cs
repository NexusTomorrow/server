using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Enters the world with one listed character.
/// </summary>
/// <remarks>
/// The client takes the realm id from the character list (0x0117) as its current realm, then waits for <see cref="EnterWorld"/> or
/// the selection's failure (<see cref="CharacterSelectFailed"/>).
/// </remarks>
[WireMessage(GameMessageOpcode.Client07DD, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CharacterSelect
{
    /// <summary>
    /// The character, as listed in the character list (0x0117).
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; private set; }
}
