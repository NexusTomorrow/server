using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.ClientResetInstances, Direction.ClientToServer)]
public sealed partial class ClientResetInstances
{
}
