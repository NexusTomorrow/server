using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets whether the player ignores invites from strangers.
/// </summary>
/// <remarks>
/// A server sends 2 to turn it on and 0 to turn it off; a 1 leaves it off.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03A5, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipIgnoreStrangersState
{
    /// <summary>
    /// Flags. Bit 1 (value 2) is ignore-strangers, and the client keeps only that bit, which FriendshipLib.GetPersonalStatus returns as
    /// bIgnoreStrangerInvites.
    /// </summary>
    [UInt(32)] public uint Flags { get; set; }
}
