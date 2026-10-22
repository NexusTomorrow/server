using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// Answers a neighbor invitation: the request HousingLib.NeighborInviteAccept and HousingLib.NeighborInviteDecline send.
/// </summary>
[WireMessage(GameMessageOpcode.Client0513, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingNeighborInviteAnswer
{
    /// <summary>
    /// True from HousingLib.NeighborInviteAccept, false from HousingLib.NeighborInviteDecline.
    /// </summary>
    [UInt(1)] public bool Accepted { get; private set; }
}
