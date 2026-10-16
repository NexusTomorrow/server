using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Marks one listed character disabled or enabled on character select.
/// </summary>
/// <remarks>
/// Taken in any state; a character the list does not hold is ignored.
/// </remarks>
[WireMessage(GameMessageOpcode.Server00E7, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class CharacterDisabled
{
    /// <summary>
    /// The character, as listed in the character list (0x0117).
    /// </summary>
    [UInt(64)] public ulong CharacterId { get; set; }

    /// <summary>
    /// Whether the character is disabled; the client keeps the flag with the character and shows it.
    /// </summary>
    [UInt(1)] public bool Disabled { get; set; }
}
