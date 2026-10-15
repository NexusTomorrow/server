using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;
using NexusTomorrow.Network.World.Message.Model.Shared;

namespace NexusTomorrow.Network.World.Message.Model;

[WireMessage(GameMessageOpcode.ClientMatchingMatchInitiateVoteToSurrender, Direction.ClientToServer)]
public sealed partial class ClientMatchingMatchInitiateVoteToSurrender
{
}
