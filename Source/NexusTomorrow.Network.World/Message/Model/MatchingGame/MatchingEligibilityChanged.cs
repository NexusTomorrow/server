using NexusTomorrow.Game.Static.Matching;
using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.MatchingGame;

/// <summary>
/// Sets the player's matching privileges, which decide the maps the player may queue for.
/// </summary>
[WireMessage(GameMessageOpcode.Server05B8, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class MatchingEligibilityChanged
{
    /// <summary>
    /// The player's matching privileges, a mask. The client offers and queues a map only when the map requires none of these bits or
    /// the player holds one of those it requires, and otherwise refuses with <see cref="MatchingQueueResult.PrivilegeRestricted"/>; a
    /// group queue tests each member's privileges the same way, refusing with
    /// <see cref="MatchingQueueResult.GroupMemberPrivilegeRestricted"/>.
    /// </summary>
    [UInt(32)] public uint Privileges { get; set; }
}
