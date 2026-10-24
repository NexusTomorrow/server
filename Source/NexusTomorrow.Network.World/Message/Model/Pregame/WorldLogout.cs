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
[Verified(Rung.Purpose)]
public sealed partial class WorldLogout
{
    /// <summary>
    /// With an ordinary reason (0 or 1), true makes the client leave the world and ask the server for the character list, and false
    /// shows the error screen and asks for nothing.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool ToCharacterSelect { get; set; }

    [UInt(5)] public byte Unknown1 { get; set; }
}
