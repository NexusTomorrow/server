using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Housing;

/// <summary>
/// The request HousingLib.RequestRandomResidenceList sends.
/// </summary>
[WireMessage(GameMessageOpcode.Client052D, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class HousingRequestRandomResidenceList
{
}
