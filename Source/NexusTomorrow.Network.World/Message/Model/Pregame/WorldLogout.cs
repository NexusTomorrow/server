using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pregame;

/// <summary>
/// Ends the player's session in the world.
/// </summary>
/// <remarks>
/// Character select and realm select drop it.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0594, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class WorldLogout
{
    /// <summary>
    /// With an ordinary <see cref="Reason"/>, true returns the player to character select on the same connection and false shows a
    /// disconnect message.
    /// </summary>
    [UInt(1)] public bool ToCharacterSelect { get; set; }

    /// <summary>
    /// Why the session ended. 6, 7, 8, 12, 15, 16, 19 and 20 always end in a disconnect message, whatever
    /// <see cref="ToCharacterSelect"/> says; with any other value <see cref="ToCharacterSelect"/> decides.
    /// </summary>
    [UInt(5)] public byte Reason { get; set; }
}
