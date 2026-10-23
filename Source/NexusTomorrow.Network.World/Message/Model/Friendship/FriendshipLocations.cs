using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the zones the player's friends are in.
/// </summary>
/// <remarks>
/// The client sets each zone on the friend its id finds, and on an account friend's character with that id, then raises
/// FriendshipLocation.
/// </remarks>
[WireMessage(GameMessageOpcode.Server03C0, Direction.ServerToClient)]
[Verified(Rung.Fields)]
public sealed partial class FriendshipLocations
{
    /// <summary>
    /// How many entries the two lists hold.
    /// </summary>
    [UInt(8), Count] public byte Count { get; set; }

    /// <summary>
    /// The ids the zones are for, each finding a friend or an account friend's character.
    /// </summary>
    [Array(CountFrom = nameof(Count)), UInt(64)] public List<ulong> Ids { get; set; } = [];

    /// <summary>
    /// Each id's zone in the same order, a WorldZone table id, which add-ons see as strWorldZone.
    /// </summary>
    [Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> WorldZoneIds { get; set; } = [];
}
