using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// Can only be sent after dueling has begun with ServerDuelStart (0x895)
[WireMessage(GameMessageOpcode.ClientDuelForfeit, Direction.ClientToServer)]
public sealed partial class ClientDuelForfeit
{
}
