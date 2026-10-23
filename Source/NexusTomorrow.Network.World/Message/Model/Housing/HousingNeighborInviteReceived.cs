using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Tells the player of an invitation to be someone's neighbor.
/// </summary>
/// <remarks>
/// The client keeps the invitation and raises HousingNeighborInviteRecieved with the inviter's name.
/// </remarks>
[WireMessage(GameMessageOpcode.Server0514, Direction.ServerToClient)]
[Verified(Rung.Purpose)]
public sealed partial class HousingNeighborInviteReceived
{
    public RealmScopedId Unknown0 { get; set; } = new();

    /// <summary>
    /// The player who sent the invitation, by name.
    /// </summary>
    [WideString] public string InviterName { get; set; } = "";
}
