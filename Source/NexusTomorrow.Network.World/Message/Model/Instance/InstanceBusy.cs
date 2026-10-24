using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Instance;

[WireMessage(GameMessageOpcode.Server0555, Direction.ServerToClient)]
public sealed partial class InstanceBusy
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
