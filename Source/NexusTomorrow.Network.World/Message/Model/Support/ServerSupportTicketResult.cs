using NexusTomorrow.Network.Codec;
using NexusTomorrow.Network.Message;

namespace NexusTomorrow.Network.World.Message.Model.Support;

[WireMessage(GameMessageOpcode.Server0972, Direction.ServerToClient)]
public sealed partial class Server0972
{
    [UInt(1)] public bool Unknown0 { get; set; }
}
