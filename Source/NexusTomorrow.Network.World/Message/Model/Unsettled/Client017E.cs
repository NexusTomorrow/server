using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client017E, Direction.ClientToServer)]
public sealed partial class Client017E
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
