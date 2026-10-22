using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// The request HousingLib.RequestRandomCommunityList sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client052C, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingRequestRandomCommunityList
{
}
