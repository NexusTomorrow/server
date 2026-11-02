using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Inventory;

/// <summary>
/// The attribute points the player now has; the client stores them for the character screen.
/// </summary>
[WireMessage(GameMessageOpcode.Server019E, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class ServerAttributePoints
{
    /// <summary>
    /// The player's attribute points.
    /// </summary>
    [VerifiedMember, UInt(32)] public uint Points { get; set; }
}