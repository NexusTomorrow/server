using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Marketplace;

/// <summary>
/// Asks for the account's CREDD history, which <see cref="CreddOperationHistory"/> answers.
/// </summary>
[WireMessage(GameMessageOpcode.Client03E7, Direction.ClientToServer)]
[Verified(Rung.Fields)]
public sealed partial class CreddHistoryRequest
{
}
