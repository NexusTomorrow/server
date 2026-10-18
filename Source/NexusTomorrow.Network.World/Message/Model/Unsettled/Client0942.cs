using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Unsettled;

[WireMessage(GameMessageOpcode.Client0942, Direction.ClientToServer)]
public sealed partial class Client0942
{
    [UInt(2)] public byte Unknown0 { get; set; }
}
