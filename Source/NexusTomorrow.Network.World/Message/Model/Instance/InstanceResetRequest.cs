using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Client0153, Direction.ClientToServer)]
public sealed partial class InstanceResetRequest
{
    [UInt(32)] public uint Unknown0 { get; private set; }
}
