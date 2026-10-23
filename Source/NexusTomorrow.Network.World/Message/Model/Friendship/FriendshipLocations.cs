using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Friendship;

/// <summary>
/// Sets the zones the player's friends are in.
/// </summary>
/// <remarks>
/// Each id is a friendship's <see cref="FriendshipCharacter.Id"/>, not one of the character ids <see cref="FriendshipGetLocations"/>
/// sends. The client skips an id it does not hold, and the id and zone pairs after it shift. It reaches an account friend's character
/// only through the found friendship's <see cref="FriendshipCharacter.CharacterId"/>. It then raises FriendshipLocation.
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
    /// The friendships, by <see cref="FriendshipCharacter.Id"/>.
    /// </summary>
    [Array(CountFrom = nameof(Count)), UInt(64)] public List<ulong> Ids { get; set; } = [];

    /// <summary>
    /// Each id's zone in the same order, a WorldZone table id, which add-ons see as strWorldZone.
    /// </summary>
    [Array(CountFrom = nameof(Count)), UInt(32)] public List<uint> WorldZoneIds { get; set; } = [];
}
