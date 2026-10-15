using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Pvp;

// No target information is sent. Relies on the target sent in ClientEntitySelect (0x185)
[WireMessage(GameMessageOpcode.ClientDuelInitate, Direction.ClientToServer)]
public sealed partial class ClientDuelInitate
{
}
