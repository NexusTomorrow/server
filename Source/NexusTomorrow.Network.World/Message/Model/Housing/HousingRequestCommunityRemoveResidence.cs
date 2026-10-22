using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Asks to remove the player's residence from its community: the request
/// HousingLib.RequestCommunityRemoveResidence sends.
/// </summary>
/// <remarks>
/// The client sends it only while the player has a residence of its own and is in a guild of type 7.
/// </remarks>
[WireMessage(GameMessageOpcode.Client04EA, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingRequestCommunityRemoveResidence
{
    /// <summary>
    /// The player's own residence.
    /// </summary>
    public RealmScopedId ResidenceId { get; private set; } = new();
}
