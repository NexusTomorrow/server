using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// The request HousingLib.RequestCommunityPlacedResidencesList sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client052A, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingRequestCommunityPlacedResidencesList
{
}
