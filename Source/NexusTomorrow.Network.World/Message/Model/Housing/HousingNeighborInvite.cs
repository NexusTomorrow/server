using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Invites a player to be a neighbor: the request HousingLib.NeighborInviteByName sends, among others.
/// </summary>
/// <remarks>
/// The client names the player by id or by name: FriendshipLib.NeighborInvite sends a friend's id and no name,
/// HousingLib.NeighborInviteByName zeros and the name.
/// </remarks>
[WireMessage(GameMessageOpcode.Client0512, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingNeighborInvite
{
    /// <summary>
    /// The player, by character id; zeros when the client names the player instead.
    /// </summary>
    public RealmScopedId PlayerId { get; set; } = new();

    /// <summary>
    /// The player, by name: HousingLib.NeighborInviteByName's argument; empty when the client sends an id instead.
    /// </summary>
    [WideString] public string PlayerName { get; set; } = "";
}
