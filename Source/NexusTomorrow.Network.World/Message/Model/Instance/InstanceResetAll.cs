using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Client07B4, Direction.ClientToServer)]
public sealed partial class InstanceResetAll
{
}
