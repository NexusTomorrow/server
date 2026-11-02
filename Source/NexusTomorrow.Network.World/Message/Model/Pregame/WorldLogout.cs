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
    /// With an ordinary reason (0 or 1), true makes the client leave the world and ask the server for the character list, and false
    /// shows the error screen and asks for nothing.
    /// </summary>
    [VerifiedMember, UInt(1)] public bool ToCharacterSelect { get; set; }

    /// <summary>
    /// The logout reason; an ordinary reason is 0 or 1, and other values drive the client's error-screen text.
    /// </summary>
    [VerifiedMember, UInt(5)] public byte Reason { get; set; }
}
