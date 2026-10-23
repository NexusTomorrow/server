using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Asks for the CREDD exchange's order counts, prices and orders, which <see cref="CreddExchangeInfo"/> answers.
/// </summary>
[WireMessage(GameMessageOpcode.Client0269, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CreddExchangeInfoRequest
{
}
